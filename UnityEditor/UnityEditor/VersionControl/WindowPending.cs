// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.WindowPending
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

namespace UnityEditor.VersionControl
{
  [EditorWindowTitle(icon = "UnityEditor.VersionControl", title = "Versioning")]
  internal class WindowPending : EditorWindow
  {
    private const float k_ResizerHeight = 17f;
    private const float k_MinIncomingAreaHeight = 50f;
    private const float k_BottomBarHeight = 17f;
    private static WindowPending.Styles s_Styles;
    private static Texture2D changeIcon;
    private Texture2D syncIcon;
    private Texture2D refreshIcon;
    private GUIStyle header;
    [SerializeField]
    private ListControl pendingList;
    [SerializeField]
    private ListControl incomingList;
    private bool m_ShowIncoming;
    private float s_ToolbarButtonsWidth;
    private float s_SettingsButtonWidth;
    private float s_DeleteChangesetsButtonWidth;
    private static GUIContent[] sStatusWheel;
    private static bool s_DidReload;

    internal static GUIContent StatusWheel
    {
      get
      {
        if (WindowPending.sStatusWheel == null)
        {
          WindowPending.sStatusWheel = new GUIContent[12];
          for (int index = 0; index < 12; ++index)
          {
            GUIContent guiContent = new GUIContent();
            guiContent.image = (Texture) EditorGUIUtility.LoadIcon("WaitSpin" + index.ToString("00"));
            guiContent.image.hideFlags = HideFlags.HideAndDontSave;
            guiContent.image.name = "Spinner";
            WindowPending.sStatusWheel[index] = guiContent;
          }
        }
        int index1 = (int) Mathf.Repeat(Time.realtimeSinceStartup * 10f, 11.99f);
        return WindowPending.sStatusWheel[index1];
      }
    }

    private void InitStyles()
    {
      if (WindowPending.s_Styles != null)
        return;
      WindowPending.s_Styles = new WindowPending.Styles();
    }

    private void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      if (this.pendingList == null)
        this.pendingList = new ListControl();
      this.pendingList.ExpandEvent += new ListControl.ExpandDelegate(this.OnExpand);
      this.pendingList.DragEvent += new ListControl.DragDelegate(this.OnDrop);
      this.pendingList.MenuDefault = "CONTEXT/Pending";
      this.pendingList.MenuFolder = "CONTEXT/Change";
      this.pendingList.DragAcceptOnly = true;
      if (this.incomingList == null)
        this.incomingList = new ListControl();
      this.incomingList.ExpandEvent += new ListControl.ExpandDelegate(this.OnExpandIncoming);
      this.UpdateWindow();
    }

    public void OnSelectionChange()
    {
      if (this.hasFocus)
        return;
      this.pendingList.Sync();
      this.Repaint();
    }

    private void OnDrop(ChangeSet targetItem)
    {
      Provider.ChangeSetMove(this.pendingList.SelectedAssets, targetItem).SetCompletionAction(CompletionAction.UpdatePendingWindow);
    }

    public static void ExpandLatestChangeSet()
    {
      foreach (WindowPending windowPending in Resources.FindObjectsOfTypeAll(typeof (WindowPending)) as WindowPending[])
        windowPending.pendingList.ExpandLastItem();
    }

    private void OnExpand(ChangeSet change, ListItem item)
    {
      if (!Provider.isActive)
        return;
      Task task = Provider.ChangeSetStatus(change);
      task.userIdentifier = item.Identifier;
      task.SetCompletionAction(CompletionAction.OnChangeContentsPendingWindow);
      if (item.HasChildren)
        return;
      Asset asset = new Asset("Updating...");
      this.pendingList.Add(item, asset.prettyPath, asset).Dummy = true;
      this.pendingList.Refresh(false);
      this.Repaint();
    }

    private void OnExpandIncoming(ChangeSet change, ListItem item)
    {
      if (!Provider.isActive)
        return;
      Task task = Provider.IncomingChangeSetAssets(change);
      task.userIdentifier = item.Identifier;
      task.SetCompletionAction(CompletionAction.OnChangeContentsPendingWindow);
      if (item.HasChildren)
        return;
      Asset asset = new Asset("Updating...");
      this.incomingList.Add(item, asset.prettyPath, asset).Dummy = true;
      this.incomingList.Refresh(false);
      this.Repaint();
    }

    private void UpdateWindow()
    {
      if (!Provider.isActive)
      {
        this.pendingList.Clear();
        Provider.UpdateSettings();
        this.Repaint();
      }
      else
      {
        if (Provider.onlineState != OnlineState.Online)
          return;
        Provider.ChangeSets().SetCompletionAction(CompletionAction.OnChangeSetsPendingWindow);
        Provider.Incoming().SetCompletionAction(CompletionAction.OnIncomingPendingWindow);
      }
    }

    private void OnGotLatest(Task t)
    {
      this.UpdateWindow();
    }

    private static void OnVCTaskCompletedEvent(Task task, CompletionAction completionAction)
    {
      foreach (WindowPending windowPending in Resources.FindObjectsOfTypeAll(typeof (WindowPending)) as WindowPending[])
      {
        switch (completionAction)
        {
          case CompletionAction.UpdatePendingWindow:
          case CompletionAction.OnCheckoutCompleted:
            windowPending.UpdateWindow();
            break;
          case CompletionAction.OnChangeContentsPendingWindow:
            windowPending.OnChangeContents(task);
            break;
          case CompletionAction.OnIncomingPendingWindow:
            windowPending.OnIncoming(task);
            break;
          case CompletionAction.OnChangeSetsPendingWindow:
            windowPending.OnChangeSets(task);
            break;
          case CompletionAction.OnGotLatestPendingWindow:
            windowPending.OnGotLatest(task);
            break;
        }
      }
      switch (completionAction)
      {
        case CompletionAction.OnSubmittedChangeWindow:
          WindowChange.OnSubmitted(task);
          break;
        case CompletionAction.OnAddedChangeWindow:
          WindowChange.OnAdded(task);
          break;
        case CompletionAction.OnCheckoutCompleted:
          if (EditorUserSettings.showFailedCheckout)
          {
            WindowCheckoutFailure.OpenIfCheckoutFailed(task.assetList);
            break;
          }
          break;
      }
      task.Dispose();
    }

    public static void OnStatusUpdated()
    {
      WindowPending.UpdateAllWindows();
    }

    public static void UpdateAllWindows()
    {
      foreach (WindowPending windowPending in Resources.FindObjectsOfTypeAll(typeof (WindowPending)) as WindowPending[])
        windowPending.UpdateWindow();
    }

    public static void CloseAllWindows()
    {
      WindowPending[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (WindowPending)) as WindowPending[];
      WindowPending windowPending = objectsOfTypeAll.Length <= 0 ? (WindowPending) null : objectsOfTypeAll[0];
      if (!((UnityEngine.Object) windowPending != (UnityEngine.Object) null))
        return;
      windowPending.Close();
    }

    private void OnIncoming(Task task)
    {
      this.CreateStaticResources();
      this.PopulateListControl(this.incomingList, task, this.syncIcon);
    }

    private void OnChangeSets(Task task)
    {
      this.CreateStaticResources();
      this.PopulateListControl(this.pendingList, task, WindowPending.changeIcon);
    }

    private void PopulateListControl(ListControl list, Task task, Texture2D icon)
    {
      ChangeSets changeSets = task.changeSets;
      ListItem listItem1 = list.Root.FirstChild;
      while (listItem1 != null)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated method
        if (changeSets.Find(new Predicate<ChangeSet>(new WindowPending.\u003CPopulateListControl\u003Ec__AnonStoreyC1() { cs = listItem1.Item as ChangeSet }.\u003C\u003Em__230)) == null)
        {
          ListItem listItem2 = listItem1;
          listItem1 = listItem1.Next;
          list.Root.Remove(listItem2);
        }
        else
          listItem1 = listItem1.Next;
      }
      using (List<ChangeSet>.Enumerator enumerator = changeSets.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ChangeSet current = enumerator.Current;
          ListItem listItem2 = list.GetChangeSetItem(current);
          if (listItem2 != null)
          {
            listItem2.Item = (object) current;
            listItem2.Name = current.description;
          }
          else
            listItem2 = list.Add((ListItem) null, current.description, current);
          listItem2.Exclusive = true;
          listItem2.CanAccept = true;
          listItem2.Icon = (Texture) icon;
        }
      }
      list.Refresh();
      this.Repaint();
    }

    private void OnChangeContents(Task task)
    {
      ListItem itemWithIdentifier = this.pendingList.FindItemWithIdentifier(task.userIdentifier);
      ListItem parent = itemWithIdentifier ?? this.incomingList.FindItemWithIdentifier(task.userIdentifier);
      if (parent == null)
        return;
      ListControl listControl = itemWithIdentifier != null ? this.pendingList : this.incomingList;
      parent.RemoveAll();
      AssetList assetList = task.assetList;
      if (assetList.Count == 0)
      {
        listControl.Add(parent, "Empty change list", (Asset) null).Dummy = true;
      }
      else
      {
        using (List<Asset>.Enumerator enumerator = assetList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Asset current = enumerator.Current;
            listControl.Add(parent, current.prettyPath, current);
          }
        }
      }
      listControl.Refresh(false);
      this.Repaint();
    }

    private ChangeSets GetEmptyChangeSetsCandidates()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WindowPending.\u003CGetEmptyChangeSetsCandidates\u003Ec__AnonStoreyC2 candidatesCAnonStoreyC2 = new WindowPending.\u003CGetEmptyChangeSetsCandidates\u003Ec__AnonStoreyC2();
      ChangeSets emptyChangeSets = this.pendingList.EmptyChangeSets;
      // ISSUE: reference to a compiler-generated field
      candidatesCAnonStoreyC2.toDelete = new ChangeSets();
      // ISSUE: reference to a compiler-generated method
      emptyChangeSets.FindAll((Predicate<ChangeSet>) (item => item.id != ChangeSet.defaultID)).ForEach(new System.Action<ChangeSet>(candidatesCAnonStoreyC2.\u003C\u003Em__232));
      // ISSUE: reference to a compiler-generated field
      return candidatesCAnonStoreyC2.toDelete;
    }

    private bool HasEmptyPendingChangesets()
    {
      return Provider.DeleteChangeSetsIsValid(this.GetEmptyChangeSetsCandidates());
    }

    private void DeleteEmptyPendingChangesets()
    {
      Provider.DeleteChangeSets(this.GetEmptyChangeSetsCandidates()).SetCompletionAction(CompletionAction.UpdatePendingWindow);
    }

    private void OnGUI()
    {
      this.InitStyles();
      if (!WindowPending.s_DidReload)
      {
        WindowPending.s_DidReload = true;
        this.UpdateWindow();
      }
      this.CreateResources();
      Event current = Event.current;
      float fixedHeight = EditorStyles.toolbar.fixedHeight;
      bool flag1 = false;
      GUILayout.BeginArea(new Rect(0.0f, 0.0f, this.position.width, fixedHeight));
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      int num = this.incomingList.Root != null ? this.incomingList.Root.ChildCount : 0;
      this.m_ShowIncoming = !GUILayout.Toggle(!this.m_ShowIncoming, "Outgoing", EditorStyles.toolbarButton, new GUILayoutOption[0]);
      this.m_ShowIncoming = GUILayout.Toggle(this.m_ShowIncoming, GUIContent.Temp("Incoming" + (num != 0 ? " (" + num.ToString() + ")" : string.Empty)), EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        flag1 = true;
      GUILayout.FlexibleSpace();
      EditorGUI.BeginDisabledGroup(Provider.activeTask != null);
      foreach (CustomCommand customCommand in Provider.customCommands)
      {
        if (customCommand.context == CommandContext.Global && GUILayout.Button(customCommand.label, EditorStyles.toolbarButton, new GUILayoutOption[0]))
          customCommand.StartTask();
      }
      EditorGUI.EndDisabledGroup();
      if (Mathf.FloorToInt(this.position.width - this.s_ToolbarButtonsWidth - this.s_SettingsButtonWidth - this.s_DeleteChangesetsButtonWidth) > 0 && this.HasEmptyPendingChangesets() && GUILayout.Button("Delete Empty Changesets", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        this.DeleteEmptyPendingChangesets();
      if (Mathf.FloorToInt(this.position.width - this.s_ToolbarButtonsWidth - this.s_SettingsButtonWidth) > 0 && GUILayout.Button("Settings", EditorStyles.toolbarButton, new GUILayoutOption[0]))
      {
        EditorApplication.ExecuteMenuItem("Edit/Project Settings/Editor");
        EditorWindow.FocusWindowIfItsOpen<InspectorWindow>();
        GUIUtility.ExitGUI();
      }
      Color color1 = GUI.color;
      GUI.color = new Color(1f, 1f, 1f, 0.5f);
      bool flag2 = GUILayout.Button((Texture) this.refreshIcon, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      bool flag3 = flag1 || flag2;
      GUI.color = color1;
      if (current.isKey && GUIUtility.keyboardControl == 0 && (current.type == EventType.KeyDown && current.keyCode == KeyCode.F5))
      {
        flag3 = true;
        current.Use();
      }
      if (flag3)
      {
        if (flag2)
          Provider.InvalidateCache();
        this.UpdateWindow();
      }
      GUILayout.EndArea();
      Rect rect = new Rect(0.0f, fixedHeight, this.position.width, (float) ((double) this.position.height - (double) fixedHeight - 17.0));
      bool flag4 = false;
      GUILayout.EndHorizontal();
      bool flag5;
      if (!Provider.isActive)
      {
        Color color2 = GUI.color;
        GUI.color = new Color(0.8f, 0.5f, 0.5f);
        rect.height = fixedHeight;
        GUILayout.BeginArea(rect);
        GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        string text = "DISABLED";
        if (Provider.enabled)
        {
          if (Provider.onlineState == OnlineState.Updating)
          {
            GUI.color = new Color(0.8f, 0.8f, 0.5f);
            text = "CONNECTING...";
          }
          else
            text = "OFFLINE";
        }
        GUILayout.Label(text, EditorStyles.miniLabel, new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        rect.y += rect.height;
        if (!string.IsNullOrEmpty(Provider.offlineReason))
          GUI.Label(rect, Provider.offlineReason);
        GUI.color = color2;
        flag5 = false;
      }
      else
      {
        flag5 = !this.m_ShowIncoming ? flag4 | this.pendingList.OnGUI(rect, this.hasFocus) : flag4 | this.incomingList.OnGUI(rect, this.hasFocus);
        rect.y += rect.height;
        rect.height = 17f;
        GUI.Label(rect, GUIContent.none, WindowPending.s_Styles.bottomBarBg);
        GUIContent content = new GUIContent("Apply All Incoming Changes");
        Vector2 vector2 = EditorStyles.miniButton.CalcSize(content);
        WindowPending.ProgressGUI(new Rect(rect.x, rect.y - 2f, (float) ((double) rect.width - (double) vector2.x - 5.0), rect.height), Provider.activeTask, false);
        if (this.m_ShowIncoming)
        {
          Rect position = rect;
          position.width = vector2.x;
          position.height = vector2.y;
          position.y = rect.y + 2f;
          position.x = (float) ((double) this.position.width - (double) vector2.x - 5.0);
          EditorGUI.BeginDisabledGroup(this.incomingList.Size == 0);
          if (GUI.Button(position, content, EditorStyles.miniButton))
          {
            Asset asset = new Asset(string.Empty);
            AssetList assets = new AssetList();
            assets.Add(asset);
            Provider.GetLatest(assets).SetCompletionAction(CompletionAction.OnGotLatestPendingWindow);
          }
          EditorGUI.EndDisabledGroup();
        }
      }
      if (!flag5)
        return;
      this.Repaint();
    }

    internal static bool ProgressGUI(Rect rect, Task activeTask, bool descriptionTextFirst)
    {
      if (activeTask == null || activeTask.progressPct == -1 && activeTask.secondsSpent == -1 && activeTask.progressMessage.Length == 0)
        return false;
      string progressMessage = activeTask.progressMessage;
      Rect position = rect;
      GUIContent statusWheel = WindowPending.StatusWheel;
      position.width = position.height;
      position.x += 4f;
      position.y += 4f;
      GUI.Label(position, statusWheel);
      rect.x += position.width + 4f;
      string text = progressMessage.Length != 0 ? progressMessage : activeTask.description;
      if (activeTask.progressPct == -1)
      {
        rect.width -= position.width + 4f;
        rect.y += 4f;
        GUI.Label(rect, text, EditorStyles.miniLabel);
      }
      else
      {
        rect.width = 120f;
        EditorGUI.ProgressBar(rect, (float) activeTask.progressPct, text);
      }
      return true;
    }

    private void CreateResources()
    {
      if ((UnityEngine.Object) this.refreshIcon == (UnityEngine.Object) null)
      {
        this.refreshIcon = EditorGUIUtility.LoadIcon("Refresh");
        this.refreshIcon.hideFlags = HideFlags.HideAndDontSave;
        this.refreshIcon.name = "RefreshIcon";
      }
      if (this.header == null)
        this.header = (GUIStyle) "OL Title";
      this.CreateStaticResources();
      if ((double) this.s_ToolbarButtonsWidth != 0.0)
        return;
      this.s_ToolbarButtonsWidth = EditorStyles.toolbarButton.CalcSize(new GUIContent("Incoming (xx)")).x;
      this.s_ToolbarButtonsWidth += EditorStyles.toolbarButton.CalcSize(new GUIContent("Outgoing")).x;
      this.s_ToolbarButtonsWidth += EditorStyles.toolbarButton.CalcSize(new GUIContent((Texture) this.refreshIcon)).x;
      this.s_SettingsButtonWidth = EditorStyles.toolbarButton.CalcSize(new GUIContent("Settings")).x;
      this.s_DeleteChangesetsButtonWidth = EditorStyles.toolbarButton.CalcSize(new GUIContent("Delete Empty Changesets")).x;
    }

    private void CreateStaticResources()
    {
      if ((UnityEngine.Object) this.syncIcon == (UnityEngine.Object) null)
      {
        this.syncIcon = EditorGUIUtility.LoadIcon("vcs_incoming");
        this.syncIcon.hideFlags = HideFlags.HideAndDontSave;
        this.syncIcon.name = "SyncIcon";
      }
      if (!((UnityEngine.Object) WindowPending.changeIcon == (UnityEngine.Object) null))
        return;
      WindowPending.changeIcon = EditorGUIUtility.LoadIcon("vcs_change");
      WindowPending.changeIcon.hideFlags = HideFlags.HideAndDontSave;
      WindowPending.changeIcon.name = "ChangeIcon";
    }

    internal class Styles
    {
      public GUIStyle box = (GUIStyle) "CN Box";
      public GUIStyle bottomBarBg = (GUIStyle) "ProjectBrowserBottomBarBg";
    }
  }
}

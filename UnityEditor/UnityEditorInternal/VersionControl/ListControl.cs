// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VersionControl.ListControl
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace UnityEditorInternal.VersionControl
{
  [Serializable]
  public class ListControl
  {
    private static int s_uniqueIDCount = 1;
    private static Dictionary<int, ListControl> s_uniqueIDList = new Dictionary<int, ListControl>();
    private ListItem root = new ListItem();
    private List<ListItem> visibleList = new List<ListItem>();
    private Dictionary<string, ListItem> pathSearch = new Dictionary<string, ListItem>();
    private ListControl.SelectDirection dragAdjust = ListControl.SelectDirection.Current;
    private Dictionary<string, ListItem> selectList = new Dictionary<string, ListItem>();
    private GUIContent calcSizeTmpContent = new GUIContent();
    private const float c_lineHeight = 16f;
    private const float c_scrollWidth = 14f;
    private const string c_changeKeyPrefix = "_chkeyprfx_";
    private const string c_metaSuffix = ".meta";
    internal const string c_emptyChangeListMessage = "Empty change list";
    private ListControl.ExpandDelegate expandDelegate;
    private ListControl.DragDelegate dragDelegate;
    private ListControl.ActionDelegate actionDelegate;
    private ListItem active;
    private Texture2D blueTex;
    private Texture2D greyTex;
    private Texture2D yellowTex;
    [SerializeField]
    private ListControl.ListState m_listState;
    private Texture2D defaultIcon;
    private bool readOnly;
    private bool scrollVisible;
    private string menuFolder;
    private string menuDefault;
    private bool dragAcceptOnly;
    private ListItem dragTarget;
    private int dragCount;
    private ListItem singleSelect;
    [NonSerialized]
    private int uniqueID;

    public ListControl.ListState listState
    {
      get
      {
        if (this.m_listState == null)
          this.m_listState = new ListControl.ListState();
        return this.m_listState;
      }
    }

    public ListControl.ExpandDelegate ExpandEvent
    {
      get
      {
        return this.expandDelegate;
      }
      set
      {
        this.expandDelegate = value;
      }
    }

    public ListControl.DragDelegate DragEvent
    {
      get
      {
        return this.dragDelegate;
      }
      set
      {
        this.dragDelegate = value;
      }
    }

    public ListControl.ActionDelegate ActionEvent
    {
      get
      {
        return this.actionDelegate;
      }
      set
      {
        this.actionDelegate = value;
      }
    }

    public ListItem Root
    {
      get
      {
        return this.root;
      }
    }

    public AssetList SelectedAssets
    {
      get
      {
        AssetList assetList = new AssetList();
        using (Dictionary<string, ListItem>.Enumerator enumerator = this.selectList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<string, ListItem> current = enumerator.Current;
            if (current.Value.Item is Asset)
              assetList.Add(current.Value.Item as Asset);
          }
        }
        return assetList;
      }
    }

    public ChangeSets SelectedChangeSets
    {
      get
      {
        ChangeSets changeSets = new ChangeSets();
        using (Dictionary<string, ListItem>.Enumerator enumerator = this.selectList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<string, ListItem> current = enumerator.Current;
            if (current.Value != null && current.Value.Item is ChangeSet)
              changeSets.Add(current.Value.Item as ChangeSet);
          }
        }
        return changeSets;
      }
    }

    public ChangeSets EmptyChangeSets
    {
      get
      {
        ListItem listItem = this.root.FirstChild;
        ChangeSets changeSets = new ChangeSets();
        for (; listItem != null; listItem = listItem.Next)
        {
          ChangeSet change = listItem.Change;
          if (change != null && listItem.HasChildren && listItem.FirstChild.Item == null && listItem.FirstChild.Name == "Empty change list")
            changeSets.Add(change);
        }
        return changeSets;
      }
    }

    public bool ReadOnly
    {
      get
      {
        return this.readOnly;
      }
      set
      {
        this.readOnly = value;
      }
    }

    public string MenuFolder
    {
      get
      {
        return this.menuFolder;
      }
      set
      {
        this.menuFolder = value;
      }
    }

    public string MenuDefault
    {
      get
      {
        return this.menuDefault;
      }
      set
      {
        this.menuDefault = value;
      }
    }

    public bool DragAcceptOnly
    {
      get
      {
        return this.dragAcceptOnly;
      }
      set
      {
        this.dragAcceptOnly = value;
      }
    }

    public int Size
    {
      get
      {
        return this.visibleList.Count;
      }
    }

    public ListControl()
    {
      this.uniqueID = ListControl.s_uniqueIDCount++;
      ListControl.s_uniqueIDList.Add(this.uniqueID, this);
      this.active = this.root;
      this.Clear();
    }

    ~ListControl()
    {
      ListControl.s_uniqueIDList.Remove(this.uniqueID);
    }

    public static ListControl FromID(int id)
    {
      try
      {
        return ListControl.s_uniqueIDList[id];
      }
      catch
      {
        return (ListControl) null;
      }
    }

    public ListItem FindItemWithIdentifier(int identifier)
    {
      return this.root.FindWithIdentifierRecurse(identifier);
    }

    public ListItem Add(ListItem parent, string name, Asset asset)
    {
      ListItem listItem1 = parent == null ? this.root : parent;
      ListItem listItem2 = new ListItem();
      listItem2.Name = name;
      listItem2.Asset = asset;
      listItem1.Add(listItem2);
      ListItem twinAsset = this.GetTwinAsset(listItem2);
      if (twinAsset != null && listItem2.Asset != null && twinAsset.Asset.state == (listItem2.Asset.state & ~Asset.States.MetaFile))
        listItem2.Hidden = true;
      if (listItem2.Asset == null || listItem2.Asset.path.Length <= 0)
        return listItem2;
      this.pathSearch[listItem2.Asset.path.ToLower()] = listItem2;
      return listItem2;
    }

    public ListItem Add(ListItem parent, string name, ChangeSet change)
    {
      ListItem listItem1 = parent == null ? this.root : parent;
      ListItem listItem2 = new ListItem();
      listItem2.Name = name;
      listItem2.Change = change ?? new ChangeSet(name);
      listItem1.Add(listItem2);
      this.pathSearch["_chkeyprfx_" + change.id.ToString()] = listItem2;
      return listItem2;
    }

    internal ListItem GetChangeSetItem(ChangeSet change)
    {
      if (change == null)
        return (ListItem) null;
      for (ListItem listItem = this.root.FirstChild; listItem != null; listItem = listItem.Next)
      {
        ChangeSet changeSet = listItem.Item as ChangeSet;
        if (changeSet != null && changeSet.id == change.id)
          return listItem;
      }
      return (ListItem) null;
    }

    public void Clear()
    {
      this.root.Clear();
      this.pathSearch.Clear();
      this.root.Name = "ROOT";
      this.root.Expanded = true;
    }

    public void Refresh()
    {
      this.Refresh(true);
    }

    public void Refresh(bool updateExpanded)
    {
      if (updateExpanded)
      {
        this.LoadExpanded(this.root);
        this.root.Name = "ROOT";
        this.root.Expanded = true;
        this.listState.Expanded.Clear();
        this.CallExpandedEvent(this.root, false);
      }
      this.SelectedRefresh();
    }

    public void Sync()
    {
      this.SelectedClear();
      foreach (UnityEngine.Object assetObject in Selection.objects)
      {
        if (AssetDatabase.IsMainAsset(assetObject))
        {
          ListItem listItem = this.PathSearchFind(Application.dataPath.Substring(0, Application.dataPath.Length - 6) + AssetDatabase.GetAssetPath(assetObject));
          if (listItem != null)
            this.SelectedAdd(listItem);
        }
      }
    }

    public bool OnGUI(Rect area, bool focus)
    {
      bool flag = false;
      this.CreateResources();
      Event current = Event.current;
      int openCount = this.active.OpenCount;
      int num = (int) ((double) area.height / 16.0);
      if (current.type == EventType.ScrollWheel)
      {
        flag = true;
        this.listState.Scroll += current.delta.y;
        this.listState.Scroll = Mathf.Clamp(this.listState.Scroll, 0.0f, (float) (openCount - num));
      }
      if (openCount > num)
      {
        Rect position = new Rect((float) ((double) area.x + (double) area.width - 14.0), area.y, 14f, area.height);
        area.width -= 14f;
        float scroll = this.listState.Scroll;
        this.listState.Scroll = GUI.VerticalScrollbar(position, this.listState.Scroll, (float) num, 0.0f, (float) openCount);
        this.listState.Scroll = Mathf.Clamp(this.listState.Scroll, 0.0f, (float) (openCount - num));
        if ((double) scroll != (double) this.listState.Scroll)
          flag = true;
        if (!this.scrollVisible)
          this.scrollVisible = true;
      }
      else if (this.scrollVisible)
        this.scrollVisible = false;
      this.UpdateVisibleList(area, this.listState.Scroll);
      if (focus && !this.readOnly)
      {
        if (current.isKey)
        {
          flag = true;
          this.HandleKeyInput(current);
        }
        this.HandleSelectAll();
        flag = this.HandleMouse(area) || flag;
        if (current.type == EventType.DragUpdated && area.Contains(current.mousePosition))
        {
          if ((double) current.mousePosition.y < (double) area.y + 16.0)
            this.listState.Scroll = Mathf.Clamp(this.listState.Scroll - 1f, 0.0f, (float) (openCount - num));
          else if ((double) current.mousePosition.y > (double) area.y + (double) area.height - 16.0)
            this.listState.Scroll = Mathf.Clamp(this.listState.Scroll + 1f, 0.0f, (float) (openCount - num));
        }
      }
      this.DrawItems(area, focus);
      return flag;
    }

    private bool HandleMouse(Rect area)
    {
      Event current = Event.current;
      bool flag1 = false;
      bool flag2 = area.Contains(current.mousePosition);
      if (current.type == EventType.MouseDown && flag2)
      {
        flag1 = true;
        this.dragCount = 0;
        GUIUtility.keyboardControl = 0;
        this.singleSelect = this.GetItemAt(area, current.mousePosition);
        if (this.singleSelect != null && !this.singleSelect.Dummy)
        {
          if (current.button == 0 && current.clickCount > 1 && this.singleSelect.Asset != null)
            this.singleSelect.Asset.Edit();
          if (current.button < 2)
          {
            float num = area.x + (float) ((this.singleSelect.Indent - 1) * 18);
            if ((double) current.mousePosition.x >= (double) num && ((double) current.mousePosition.x < (double) num + 16.0 && this.singleSelect.CanExpand))
            {
              this.singleSelect.Expanded = !this.singleSelect.Expanded;
              this.CallExpandedEvent(this.singleSelect, true);
              this.singleSelect = (ListItem) null;
            }
            else if (current.control || current.command)
            {
              if (current.button == 1)
                this.SelectedAdd(this.singleSelect);
              else
                this.SelectedToggle(this.singleSelect);
              this.singleSelect = (ListItem) null;
            }
            else if (current.shift)
            {
              this.SelectionFlow(this.singleSelect);
              this.singleSelect = (ListItem) null;
            }
            else if (!this.IsSelected(this.singleSelect))
            {
              this.SelectedSet(this.singleSelect);
              this.singleSelect = (ListItem) null;
            }
          }
        }
        else if (current.button == 0)
        {
          this.SelectedClear();
          this.singleSelect = (ListItem) null;
        }
      }
      else if ((current.type == EventType.MouseUp || current.type == EventType.ContextClick) && flag2)
      {
        GUIUtility.keyboardControl = 0;
        this.singleSelect = this.GetItemAt(area, current.mousePosition);
        this.dragCount = 0;
        flag1 = true;
        if (this.singleSelect != null && !this.singleSelect.Dummy)
        {
          if (current.type == EventType.ContextClick)
          {
            this.singleSelect = (ListItem) null;
            if (!this.IsSelectedAsset() && !string.IsNullOrEmpty(this.menuFolder))
            {
              ListControl.s_uniqueIDList[this.uniqueID] = this;
              EditorUtility.DisplayPopupMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f), this.menuFolder, new MenuCommand((UnityEngine.Object) null, this.uniqueID));
            }
            else if (!string.IsNullOrEmpty(this.menuDefault))
            {
              ListControl.s_uniqueIDList[this.uniqueID] = this;
              EditorUtility.DisplayPopupMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f), this.menuDefault, new MenuCommand((UnityEngine.Object) null, this.uniqueID));
            }
          }
          else if (current.type != EventType.ContextClick && current.button == 0 && (!current.control && !current.command) && (!current.shift && this.IsSelected(this.singleSelect)))
          {
            this.SelectedSet(this.singleSelect);
            this.singleSelect = (ListItem) null;
          }
        }
      }
      if (current.type == EventType.MouseDrag && flag2)
      {
        ++this.dragCount;
        if (this.dragCount > 2 && Selection.objects.Length > 0)
        {
          DragAndDrop.PrepareStartDrag();
          if (this.singleSelect != null)
            DragAndDrop.objectReferences = new UnityEngine.Object[1]
            {
              this.singleSelect.Asset.Load()
            };
          else
            DragAndDrop.objectReferences = Selection.objects;
          DragAndDrop.StartDrag("Move");
        }
      }
      if (current.type == EventType.DragUpdated)
      {
        flag1 = true;
        DragAndDrop.visualMode = DragAndDropVisualMode.Move;
        this.dragTarget = this.GetItemAt(area, current.mousePosition);
        if (this.dragTarget != null)
        {
          if (this.IsSelected(this.dragTarget))
            this.dragTarget = (ListItem) null;
          else if (this.dragAcceptOnly)
          {
            if (!this.dragTarget.CanAccept)
              this.dragTarget = (ListItem) null;
          }
          else
          {
            bool flag3 = !this.dragTarget.CanAccept || this.dragTarget.PrevOpenVisible != this.dragTarget.Parent;
            bool flag4 = !this.dragTarget.CanAccept || this.dragTarget.NextOpenVisible != this.dragTarget.FirstChild;
            float num1 = !this.dragTarget.CanAccept ? 8f : 2f;
            int num2 = (int) (((double) current.mousePosition.y - (double) area.y) / 16.0);
            float num3 = area.y + (float) num2 * 16f;
            this.dragAdjust = ListControl.SelectDirection.Current;
            if (flag3 && (double) current.mousePosition.y <= (double) num3 + (double) num1)
              this.dragAdjust = ListControl.SelectDirection.Up;
            else if (flag4 && (double) current.mousePosition.y >= (double) num3 + 16.0 - (double) num1)
              this.dragAdjust = ListControl.SelectDirection.Down;
          }
        }
      }
      if (current.type == EventType.DragPerform && this.dragTarget != null)
      {
        ListItem listItem = this.dragAdjust != ListControl.SelectDirection.Current ? this.dragTarget.Parent : this.dragTarget;
        if (this.dragDelegate != null && listItem != null && listItem.CanAccept)
          this.dragDelegate(listItem.Change);
        this.dragTarget = (ListItem) null;
      }
      if (current.type == EventType.DragExited)
        this.dragTarget = (ListItem) null;
      return flag1;
    }

    private void DrawItems(Rect area, bool focus)
    {
      float y = area.y;
      using (List<ListItem>.Enumerator enumerator = this.visibleList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ListItem current = enumerator.Current;
          float x = area.x + (float) ((current.Indent - 1) * 18);
          bool selected = !this.readOnly && this.IsSelected(current);
          if (current.Parent != null && current.Parent.Parent != null && current.Parent.Parent.Parent == null)
            x -= 16f;
          this.DrawItem(current, area, x, y, focus, selected);
          y += 16f;
        }
      }
    }

    private void HandleSelectAll()
    {
      if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "SelectAll")
      {
        Event.current.Use();
      }
      else
      {
        if (Event.current.type != EventType.ExecuteCommand || !(Event.current.commandName == "SelectAll"))
          return;
        this.SelectedAll();
        Event.current.Use();
      }
    }

    private void CreateResources()
    {
      if ((UnityEngine.Object) this.blueTex == (UnityEngine.Object) null)
      {
        this.blueTex = new Texture2D(1, 1);
        this.blueTex.SetPixel(0, 0, new Color(0.23f, 0.35f, 0.55f));
        this.blueTex.hideFlags = HideFlags.HideAndDontSave;
        this.blueTex.name = "BlueTex";
        this.blueTex.Apply();
      }
      if ((UnityEngine.Object) this.greyTex == (UnityEngine.Object) null)
      {
        this.greyTex = new Texture2D(1, 1);
        this.greyTex.SetPixel(0, 0, new Color(0.3f, 0.3f, 0.3f));
        this.greyTex.hideFlags = HideFlags.HideAndDontSave;
        this.greyTex.name = "GrayTex";
        this.greyTex.Apply();
      }
      if ((UnityEngine.Object) this.yellowTex == (UnityEngine.Object) null)
      {
        this.yellowTex = new Texture2D(1, 1);
        this.yellowTex.SetPixel(0, 0, new Color(0.5f, 0.5f, 0.2f));
        this.yellowTex.name = "YellowTex";
        this.yellowTex.hideFlags = HideFlags.HideAndDontSave;
        this.yellowTex.Apply();
      }
      if (!((UnityEngine.Object) this.defaultIcon == (UnityEngine.Object) null))
        return;
      this.defaultIcon = EditorGUIUtility.LoadIcon("vcs_document");
      this.defaultIcon.hideFlags = HideFlags.HideAndDontSave;
    }

    private void HandleKeyInput(Event e)
    {
      if (e.type != EventType.KeyDown || this.selectList.Count == 0)
        return;
      if (e.keyCode == KeyCode.UpArrow || e.keyCode == KeyCode.DownArrow)
      {
        ListItem listItem;
        if (e.keyCode == KeyCode.UpArrow)
        {
          listItem = this.SelectedFirstIn(this.active);
          if (listItem != null)
            listItem = listItem.PrevOpenSkip;
        }
        else
        {
          listItem = this.SelectedLastIn(this.active);
          if (listItem != null)
            listItem = listItem.NextOpenSkip;
        }
        if (listItem != null)
        {
          if (!this.ScrollUpTo(listItem))
            this.ScrollDownTo(listItem);
          if (e.shift)
            this.SelectionFlow(listItem);
          else
            this.SelectedSet(listItem);
        }
      }
      if (e.keyCode == KeyCode.LeftArrow || e.keyCode == KeyCode.RightArrow)
      {
        ListItem listItem = this.SelectedCurrentIn(this.active);
        listItem.Expanded = e.keyCode == KeyCode.RightArrow;
        this.CallExpandedEvent(listItem, true);
      }
      if (e.keyCode != KeyCode.Return || GUIUtility.keyboardControl != 0)
        return;
      this.SelectedCurrentIn(this.active).Asset.Edit();
    }

    private void DrawItem(ListItem item, Rect area, float x, float y, bool focus, bool selected)
    {
      bool flag1 = item == this.dragTarget;
      bool flag2 = selected;
      if (selected)
      {
        Texture2D texture2D = !focus ? this.greyTex : this.blueTex;
        GUI.DrawTexture(new Rect(area.x, y, area.width, 16f), (Texture) texture2D, ScaleMode.StretchToFill, false);
      }
      else if (flag1)
      {
        switch (this.dragAdjust)
        {
          case ListControl.SelectDirection.Up:
            if (item.PrevOpenVisible != item.Parent)
            {
              GUI.DrawTexture(new Rect(x, y - 1f, area.width, 2f), (Texture) this.yellowTex, ScaleMode.StretchToFill, false);
              break;
            }
            break;
          case ListControl.SelectDirection.Down:
            GUI.DrawTexture(new Rect(x, (float) ((double) y + 16.0 - 1.0), area.width, 2f), (Texture) this.yellowTex, ScaleMode.StretchToFill, false);
            break;
          default:
            if (item.CanAccept)
            {
              GUI.DrawTexture(new Rect(area.x, y, area.width, 16f), (Texture) this.yellowTex, ScaleMode.StretchToFill, false);
              flag2 = true;
              break;
            }
            break;
        }
      }
      else if (this.dragTarget != null && item == this.dragTarget.Parent && this.dragAdjust != ListControl.SelectDirection.Current)
      {
        GUI.DrawTexture(new Rect(area.x, y, area.width, 16f), (Texture) this.yellowTex, ScaleMode.StretchToFill, false);
        flag2 = true;
      }
      if (item.HasActions)
      {
        for (int actionIdx = 0; actionIdx < item.Actions.Length; ++actionIdx)
        {
          this.calcSizeTmpContent.text = item.Actions[actionIdx];
          Vector2 vector2 = GUI.skin.button.CalcSize(this.calcSizeTmpContent);
          if (GUI.Button(new Rect(x, y, vector2.x, 16f), item.Actions[actionIdx]))
            this.actionDelegate(item, actionIdx);
          x += vector2.x + 4f;
        }
      }
      if (item.CanExpand)
        EditorGUI.Foldout(new Rect(x, y, 16f, 16f), item.Expanded, GUIContent.none);
      Texture image = item.Icon;
      Color color = GUI.color;
      Color contentColor = GUI.contentColor;
      if (item.Dummy)
        GUI.color = new Color(0.65f, 0.65f, 0.65f);
      if (!item.Dummy)
      {
        if ((UnityEngine.Object) image == (UnityEngine.Object) null)
          image = (Texture) InternalEditorUtility.GetIconForFile(item.Name);
        Rect position = new Rect(x + 14f, y, 16f, 16f);
        if ((UnityEngine.Object) image != (UnityEngine.Object) null)
          GUI.DrawTexture(position, image);
        if (item.Asset != null)
        {
          Rect itemRect = position;
          itemRect.width += 12f;
          itemRect.x -= 6f;
          Overlay.DrawOverlay(item.Asset, itemRect);
        }
      }
      string str = this.DisplayName(item);
      Vector2 vector2_1 = EditorStyles.label.CalcSize(EditorGUIUtility.TempContent(str));
      float x1 = x + 32f;
      if (flag2)
      {
        GUI.contentColor = new Color(3f, 3f, 3f);
        GUI.Label(new Rect(x1, y, area.width - x1, 18f), str);
      }
      else
        GUI.Label(new Rect(x1, y, area.width - x1, 18f), str);
      if (this.HasHiddenMetaFile(item))
      {
        GUI.color = new Color(0.55f, 0.55f, 0.55f);
        float x2 = (float) ((double) x1 + (double) vector2_1.x + 2.0);
        GUI.Label(new Rect(x2, y, area.width - x2, 18f), "+meta");
      }
      GUI.contentColor = contentColor;
      GUI.color = color;
    }

    private void UpdateVisibleList(Rect area, float scrollPos)
    {
      float y = area.y;
      float num1 = (float) ((double) area.y + (double) area.height - 16.0);
      ListItem nextOpenVisible = this.active.NextOpenVisible;
      this.visibleList.Clear();
      for (float num2 = 0.0f; (double) num2 < (double) scrollPos; ++num2)
      {
        if (nextOpenVisible == null)
          return;
        nextOpenVisible = nextOpenVisible.NextOpenVisible;
      }
      for (ListItem listItem = nextOpenVisible; listItem != null && (double) y < (double) num1; listItem = listItem.NextOpenVisible)
      {
        this.visibleList.Add(listItem);
        y += 16f;
      }
    }

    private ListItem GetItemAt(Rect area, Vector2 pos)
    {
      int index = (int) (((double) pos.y - (double) area.y) / 16.0);
      if (index >= 0 && index < this.visibleList.Count)
        return this.visibleList[index];
      return (ListItem) null;
    }

    private bool ScrollUpTo(ListItem item)
    {
      int scroll = (int) this.listState.Scroll;
      for (ListItem listItem = this.visibleList.Count <= 0 ? (ListItem) null : this.visibleList[0]; listItem != null && scroll >= 0; listItem = listItem.PrevOpenVisible)
      {
        if (listItem == item)
        {
          this.listState.Scroll = (float) scroll;
          return true;
        }
        --scroll;
      }
      return false;
    }

    private bool ScrollDownTo(ListItem item)
    {
      int scroll = (int) this.listState.Scroll;
      for (ListItem listItem = this.visibleList.Count <= 0 ? (ListItem) null : this.visibleList[this.visibleList.Count - 1]; listItem != null && scroll >= 0; listItem = listItem.NextOpenVisible)
      {
        if (listItem == item)
        {
          this.listState.Scroll = (float) scroll;
          return true;
        }
        ++scroll;
      }
      return false;
    }

    private void LoadExpanded(ListItem item)
    {
      if (item.Change != null)
        item.Expanded = this.listState.Expanded.Contains(item.Change.id);
      for (ListItem listItem = item.FirstChild; listItem != null; listItem = listItem.Next)
        this.LoadExpanded(listItem);
    }

    internal void ExpandLastItem()
    {
      if (this.root.LastChild == null)
        return;
      this.root.LastChild.Expanded = true;
      this.CallExpandedEvent(this.root.LastChild, true);
    }

    private void CallExpandedEvent(ListItem item, bool remove)
    {
      if (item.Change != null)
      {
        if (item.Expanded)
        {
          if (this.expandDelegate != null)
            this.expandDelegate(item.Change, item);
          this.listState.Expanded.Add(item.Change.id);
        }
        else if (remove)
          this.listState.Expanded.Remove(item.Change.id);
      }
      for (ListItem listItem = item.FirstChild; listItem != null; listItem = listItem.Next)
        this.CallExpandedEvent(listItem, remove);
    }

    private ListItem PathSearchFind(string path)
    {
      try
      {
        return this.pathSearch[path.ToLower()];
      }
      catch
      {
        return (ListItem) null;
      }
    }

    private void PathSearchUpdate(ListItem item)
    {
      if (item.Asset != null && item.Asset.path.Length > 0)
        this.pathSearch.Add(item.Asset.path.ToLower(), item);
      else if (item.Change != null)
      {
        this.pathSearch.Add("_chkeyprfx_" + item.Change.id.ToString(), item);
        return;
      }
      for (ListItem listItem = item.FirstChild; listItem != null; listItem = listItem.Next)
        this.PathSearchUpdate(listItem);
    }

    private bool IsSelected(ListItem item)
    {
      if (item.Asset != null)
        return this.selectList.ContainsKey(item.Asset.path.ToLower());
      if (item.Change != null)
        return this.selectList.ContainsKey("_chkeyprfx_" + item.Change.id.ToString());
      return false;
    }

    private bool IsSelectedAsset()
    {
      using (Dictionary<string, ListItem>.Enumerator enumerator = this.selectList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, ListItem> current = enumerator.Current;
          if (current.Value != null && current.Value.Asset != null)
            return true;
        }
      }
      return false;
    }

    private void SelectedClear()
    {
      this.selectList.Clear();
      Selection.activeObject = (UnityEngine.Object) null;
      Selection.instanceIDs = new int[0];
    }

    private void SelectedRefresh()
    {
      Dictionary<string, ListItem> dictionary = new Dictionary<string, ListItem>();
      using (Dictionary<string, ListItem>.Enumerator enumerator = this.selectList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, ListItem> current = enumerator.Current;
          dictionary[current.Key] = this.PathSearchFind(current.Key);
        }
      }
      this.selectList = dictionary;
    }

    public void SelectedSet(ListItem item)
    {
      if (item.Dummy)
        return;
      this.SelectedClear();
      if (item.Asset != null)
      {
        this.SelectedAdd(item);
      }
      else
      {
        if (item.Change == null)
          return;
        this.selectList["_chkeyprfx_" + item.Change.id.ToString()] = item;
      }
    }

    public void SelectedAll()
    {
      this.SelectedClear();
      this.SelectedAllHelper(this.Root);
    }

    private void SelectedAllHelper(ListItem _root)
    {
      for (ListItem _root1 = _root.FirstChild; _root1 != null; _root1 = _root1.Next)
      {
        if (_root1.HasChildren)
          this.SelectedAllHelper(_root1);
        if (_root1.Asset != null)
          this.SelectedAdd(_root1);
      }
    }

    private ListItem GetTwinAsset(ListItem item)
    {
      ListItem prev = item.Prev;
      if (item.Name.EndsWith(".meta") && prev != null && (prev.Asset != null && AssetDatabase.GetTextMetaFilePathFromAssetPath(prev.Asset.path).ToLower() == item.Asset.path.ToLower()))
        return prev;
      return (ListItem) null;
    }

    private ListItem GetTwinMeta(ListItem item)
    {
      ListItem next = item.Next;
      if (!item.Name.EndsWith(".meta") && next != null && (next.Asset != null && next.Asset.path.ToLower() == AssetDatabase.GetTextMetaFilePathFromAssetPath(item.Asset.path).ToLower()))
        return next;
      return (ListItem) null;
    }

    private ListItem GetTwin(ListItem item)
    {
      return this.GetTwinAsset(item) ?? this.GetTwinMeta(item);
    }

    public void SelectedAdd(ListItem item)
    {
      if (item.Dummy)
        return;
      ListItem listItem = this.SelectedCurrentIn(this.active);
      if (item.Exclusive || listItem != null && listItem.Exclusive)
      {
        this.SelectedSet(item);
      }
      else
      {
        string lower = item.Asset.path.ToLower();
        int count = this.selectList.Count;
        this.selectList[lower] = item;
        ListItem twin = this.GetTwin(item);
        if (twin != null)
          this.selectList[twin.Asset.path.ToLower()] = twin;
        if (count == this.selectList.Count)
          return;
        int[] instanceIds = Selection.instanceIDs;
        int length = 0;
        if (instanceIds != null)
          length = instanceIds.Length;
        int mainAssetInstanceId = AssetDatabase.GetMainAssetInstanceID((!lower.EndsWith(".meta") ? lower : lower.Substring(0, lower.Length - 5)).TrimEnd('/'));
        if (mainAssetInstanceId == 0)
          return;
        int[] numArray = new int[length + 1];
        numArray[length] = mainAssetInstanceId;
        Array.Copy((Array) instanceIds, (Array) numArray, length);
        Selection.instanceIDs = numArray;
      }
    }

    private void SelectedRemove(ListItem item)
    {
      string lower = item.Asset.path.ToLower();
      this.selectList.Remove(lower);
      this.selectList.Remove(!lower.EndsWith(".meta") ? lower + ".meta" : lower.Substring(0, lower.Length - 5));
      int mainAssetInstanceId = AssetDatabase.GetMainAssetInstanceID((!lower.EndsWith(".meta") ? lower : lower.Substring(0, lower.Length - 5)).TrimEnd('/'));
      int[] instanceIds = Selection.instanceIDs;
      if (mainAssetInstanceId == 0 || instanceIds.Length <= 0)
        return;
      int num = Array.IndexOf<int>(instanceIds, mainAssetInstanceId);
      if (num < 0)
        return;
      int[] numArray = new int[instanceIds.Length - 1];
      Array.Copy((Array) instanceIds, (Array) numArray, num);
      if (num < instanceIds.Length - 1)
        Array.Copy((Array) instanceIds, num + 1, (Array) numArray, num, instanceIds.Length - num - 1);
      Selection.instanceIDs = numArray;
    }

    private void SelectedToggle(ListItem item)
    {
      if (this.IsSelected(item))
        this.SelectedRemove(item);
      else
        this.SelectedAdd(item);
    }

    private void SelectionFlow(ListItem item)
    {
      if (this.selectList.Count == 0)
      {
        this.SelectedSet(item);
      }
      else
      {
        if (this.SelectionFlowDown(item))
          return;
        this.SelectionFlowUp(item);
      }
    }

    private bool SelectionFlowUp(ListItem item)
    {
      ListItem listItem1 = item;
      for (ListItem listItem2 = item; listItem2 != null; listItem2 = listItem2.PrevOpenVisible)
      {
        if (this.IsSelected(listItem2))
          listItem1 = listItem2;
      }
      if (item == listItem1)
        return false;
      this.SelectedClear();
      this.SelectedAdd(listItem1);
      for (ListItem listItem2 = item; listItem2 != listItem1; listItem2 = listItem2.PrevOpenVisible)
        this.SelectedAdd(listItem2);
      return true;
    }

    private bool SelectionFlowDown(ListItem item)
    {
      ListItem listItem1 = item;
      for (ListItem listItem2 = item; listItem2 != null; listItem2 = listItem2.NextOpenVisible)
      {
        if (this.IsSelected(listItem2))
          listItem1 = listItem2;
      }
      if (item == listItem1)
        return false;
      this.SelectedClear();
      this.SelectedAdd(listItem1);
      for (ListItem listItem2 = item; listItem2 != listItem1; listItem2 = listItem2.NextOpenVisible)
        this.SelectedAdd(listItem2);
      return true;
    }

    private ListItem SelectedCurrentIn(ListItem root)
    {
      using (Dictionary<string, ListItem>.Enumerator enumerator = this.selectList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, ListItem> current = enumerator.Current;
          if (current.Value.IsChildOf(root))
            return current.Value;
        }
      }
      return (ListItem) null;
    }

    private ListItem SelectedFirstIn(ListItem root)
    {
      ListItem listItem1 = this.SelectedCurrentIn(root);
      for (ListItem listItem2 = listItem1; listItem2 != null; listItem2 = listItem2.PrevOpenVisible)
      {
        if (this.IsSelected(listItem2))
          listItem1 = listItem2;
      }
      return listItem1;
    }

    private ListItem SelectedLastIn(ListItem root)
    {
      ListItem listItem1 = this.SelectedCurrentIn(root);
      for (ListItem listItem2 = listItem1; listItem2 != null; listItem2 = listItem2.NextOpenVisible)
      {
        if (this.IsSelected(listItem2))
          listItem1 = listItem2;
      }
      return listItem1;
    }

    private string DisplayName(ListItem item)
    {
      string str1 = item.Name;
      string str2 = string.Empty;
      while (str2 == string.Empty)
      {
        int length = str1.IndexOf('\n');
        if (length >= 0)
        {
          str2 = str1.Substring(0, length).Trim();
          str1 = str1.Substring(length + 1);
        }
        else
          break;
      }
      if (str2 != string.Empty)
        str1 = str2;
      string str3 = str1.Trim();
      if (str3 == string.Empty && item.Change != null)
        str3 = item.Change.id.ToString() + " " + item.Change.description;
      return str3;
    }

    private bool HasHiddenMetaFile(ListItem item)
    {
      ListItem twinMeta = this.GetTwinMeta(item);
      if (twinMeta != null)
        return twinMeta.Hidden;
      return false;
    }

    public enum SelectDirection
    {
      Up,
      Down,
      Current,
    }

    [Serializable]
    public class ListState
    {
      [SerializeField]
      public List<string> Expanded = new List<string>();
      [SerializeField]
      public float Scroll;
    }

    public delegate void ExpandDelegate(ChangeSet expand, ListItem item);

    public delegate void DragDelegate(ChangeSet target);

    public delegate void ActionDelegate(ListItem item, int actionIdx);
  }
}

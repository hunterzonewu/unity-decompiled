// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameObjectTreeViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  internal class GameObjectTreeViewGUI : TreeViewGUI
  {
    protected static GameObjectTreeViewGUI.GameObjectStyles s_GOStyles;
    private float m_PrevScollPos;
    private float m_PrevTotalHeight;
    public System.Action scrollPositionChanged;
    public System.Action scrollHeightChanged;
    public System.Action mouseDownInTreeViewRect;

    private bool showingStickyHeaders
    {
      get
      {
        return SceneManager.sceneCount > 1;
      }
    }

    public GameObjectTreeViewGUI(TreeView treeView, bool useHorizontalScroll)
      : base(treeView, useHorizontalScroll)
    {
      this.k_TopRowMargin = 4f;
    }

    public override void OnInitialize()
    {
      this.m_PrevScollPos = this.m_TreeView.state.scrollPos.y;
      this.m_PrevTotalHeight = this.m_TreeView.GetTotalRect().height;
    }

    protected override void InitStyles()
    {
      base.InitStyles();
      if (GameObjectTreeViewGUI.s_GOStyles != null)
        return;
      GameObjectTreeViewGUI.s_GOStyles = new GameObjectTreeViewGUI.GameObjectStyles();
    }

    private void DetectScrollChange()
    {
      float y = this.m_TreeView.state.scrollPos.y;
      if (this.scrollPositionChanged != null && !Mathf.Approximately(y, this.m_PrevScollPos))
        this.scrollPositionChanged();
      this.m_PrevScollPos = y;
    }

    private void DetectTotalRectChange()
    {
      float height = this.m_TreeView.GetTotalRect().height;
      if (this.scrollHeightChanged != null && !Mathf.Approximately(height, this.m_PrevTotalHeight))
        this.scrollHeightChanged();
      this.m_PrevTotalHeight = height;
    }

    private void DetectMouseDownInTreeViewRect()
    {
      Event current = Event.current;
      if (this.mouseDownInTreeViewRect == null || current.type != EventType.MouseDown || !this.m_TreeView.GetTotalRect().Contains(current.mousePosition))
        return;
      this.mouseDownInTreeViewRect();
    }

    private void DoStickySceneHeaders()
    {
      int firstRowVisible;
      int lastRowVisible;
      this.GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible);
      if (firstRowVisible < 0 || lastRowVisible < 0)
        return;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GameObjectTreeViewGUI.\u003CDoStickySceneHeaders\u003Ec__AnonStorey79 headersCAnonStorey79 = new GameObjectTreeViewGUI.\u003CDoStickySceneHeaders\u003Ec__AnonStorey79();
      float y = this.m_TreeView.state.scrollPos.y;
      if (firstRowVisible == 0 && (double) y <= (double) this.topRowMargin)
        return;
      // ISSUE: reference to a compiler-generated field
      headersCAnonStorey79.firstItem = (GameObjectTreeViewItem) this.m_TreeView.data.GetItem(firstRowVisible);
      GameObjectTreeViewItem objectTreeViewItem = (GameObjectTreeViewItem) this.m_TreeView.data.GetItem(firstRowVisible + 1);
      // ISSUE: reference to a compiler-generated field
      bool flag = headersCAnonStorey79.firstItem.scene != objectTreeViewItem.scene;
      float width = GUIClip.visibleRect.width;
      Rect rowRect = this.GetRowRect(firstRowVisible, width);
      // ISSUE: reference to a compiler-generated field
      if (headersCAnonStorey79.firstItem.isSceneHeader && Mathf.Approximately(y, rowRect.y))
        return;
      if (!flag)
        rowRect.y = y;
      // ISSUE: reference to a compiler-generated method
      GameObjectTreeViewItem sceneHeaderItem = ((GameObjectTreeViewDataSource) this.m_TreeView.data).sceneHeaderItems.FirstOrDefault<GameObjectTreeViewItem>(new Func<GameObjectTreeViewItem, bool>(headersCAnonStorey79.\u003C\u003Em__117));
      if (sceneHeaderItem == null)
        return;
      bool selected = this.m_TreeView.IsItemDragSelectedOrSelected((TreeViewItem) sceneHeaderItem);
      bool focused = this.m_TreeView.HasFocus();
      bool useBoldFont = sceneHeaderItem.scene == SceneManager.GetActiveScene();
      this.DoNodeGUI(rowRect, firstRowVisible, (TreeViewItem) sceneHeaderItem, selected, focused, useBoldFont);
      if (GUI.Button(new Rect(rowRect.x, rowRect.y, rowRect.height, rowRect.height), GUIContent.none, GUIStyle.none))
        this.m_TreeView.Frame(sceneHeaderItem.id, true, false);
      this.m_TreeView.HandleUnusedMouseEventsForNode(rowRect, (TreeViewItem) sceneHeaderItem, false);
      this.HandleStickyHeaderContextClick(rowRect, sceneHeaderItem);
    }

    private void HandleStickyHeaderContextClick(Rect rect, GameObjectTreeViewItem sceneHeaderItem)
    {
      Event current = Event.current;
      if (Application.platform == RuntimePlatform.OSXEditor)
      {
        if ((current.type != EventType.MouseDown || current.button != 1) && current.type != EventType.ContextClick || !rect.Contains(Event.current.mousePosition))
          return;
        current.Use();
        this.m_TreeView.contextClickItemCallback(sceneHeaderItem.id);
      }
      else
      {
        if (Application.platform != RuntimePlatform.WindowsEditor || current.type != EventType.MouseDown || (current.button != 1 || !rect.Contains(Event.current.mousePosition)))
          return;
        current.Use();
      }
    }

    public override void BeginRowGUI()
    {
      this.DetectScrollChange();
      this.DetectTotalRectChange();
      this.DetectMouseDownInTreeViewRect();
      base.BeginRowGUI();
      if (!this.showingStickyHeaders || Event.current.type == EventType.Repaint)
        return;
      this.DoStickySceneHeaders();
    }

    public override void EndRowGUI()
    {
      base.EndRowGUI();
      if (!this.showingStickyHeaders || Event.current.type != EventType.Repaint)
        return;
      this.DoStickySceneHeaders();
    }

    public override Rect GetRectForFraming(int row)
    {
      Rect rectForFraming = base.GetRectForFraming(row);
      if (this.showingStickyHeaders && row < this.m_TreeView.data.rowCount)
      {
        GameObjectTreeViewItem objectTreeViewItem = this.m_TreeView.data.GetItem(row) as GameObjectTreeViewItem;
        if (objectTreeViewItem != null && !objectTreeViewItem.isSceneHeader)
        {
          rectForFraming.y -= this.k_LineHeight;
          rectForFraming.height = 2f * this.k_LineHeight;
        }
      }
      return rectForFraming;
    }

    public override bool BeginRename(TreeViewItem item, float delay)
    {
      GameObjectTreeViewItem objectTreeViewItem = item as GameObjectTreeViewItem;
      if (objectTreeViewItem == null || objectTreeViewItem.isSceneHeader)
        return false;
      if ((objectTreeViewItem.objectPPTR.hideFlags & HideFlags.NotEditable) == HideFlags.None)
        return base.BeginRename(item, delay);
      Debug.LogWarning((object) "Unable to rename a GameObject with HideFlags.NotEditable.");
      return false;
    }

    protected override void RenameEnded()
    {
      string name = !string.IsNullOrEmpty(this.GetRenameOverlay().name) ? this.GetRenameOverlay().name : this.GetRenameOverlay().originalName;
      int userData = this.GetRenameOverlay().userData;
      if (!this.GetRenameOverlay().userAcceptedRename)
        return;
      ObjectNames.SetNameSmartWithInstanceID(userData, name);
      TreeViewItem treeViewItem = this.m_TreeView.data.FindItem(userData);
      if (treeViewItem != null)
        treeViewItem.displayName = name;
      EditorApplication.RepaintAnimationWindow();
    }

    protected override void DoNodeGUI(Rect rect, int row, TreeViewItem item, bool selected, bool focused, bool useBoldFont)
    {
      GameObjectTreeViewItem goItem = item as GameObjectTreeViewItem;
      if (goItem == null)
        return;
      if (goItem.isSceneHeader)
      {
        Color color = GUI.color;
        GUI.color *= new Color(1f, 1f, 1f, 0.9f);
        GUI.Label(rect, GUIContent.none, GameObjectTreeViewGUI.s_GOStyles.sceneHeaderBg);
        GUI.color = color;
      }
      base.DoNodeGUI(rect, row, item, selected, focused, useBoldFont);
      if (goItem.isSceneHeader)
        this.DoAdditionalSceneHeaderGUI(goItem, rect);
      if (!SceneHierarchyWindow.s_Debug)
        return;
      GUI.Label(new Rect(rect.xMax - 70f, rect.y, 70f, rect.height), string.Empty + (object) row, EditorStyles.boldLabel);
    }

    protected void DoAdditionalSceneHeaderGUI(GameObjectTreeViewItem goItem, Rect rect)
    {
      Rect position = new Rect((float) ((double) rect.width - 16.0 - 4.0), rect.y + (float) (((double) rect.height - 6.0) * 0.5), 16f, rect.height);
      if (Event.current.type == EventType.Repaint)
        GameObjectTreeViewGUI.s_GOStyles.optionsButtonStyle.Draw(position, false, false, false, false);
      position.y = rect.y;
      position.height = rect.height;
      position.width = 24f;
      if (!EditorGUI.ButtonMouseDown(position, GUIContent.none, FocusType.Passive, GUIStyle.none))
        return;
      this.m_TreeView.SelectionClick((TreeViewItem) goItem, true);
      this.m_TreeView.contextClickItemCallback(goItem.id);
    }

    protected override void DrawIconAndLabel(Rect rect, TreeViewItem item, string label, bool selected, bool focused, bool useBoldFont, bool isPinging)
    {
      GameObjectTreeViewItem objectTreeViewItem = item as GameObjectTreeViewItem;
      if (objectTreeViewItem == null)
        return;
      if (objectTreeViewItem.isSceneHeader)
      {
        if (objectTreeViewItem.scene.isDirty)
          label += "*";
        if (!objectTreeViewItem.scene.isLoaded)
          label += " (not loaded)";
        bool useBoldFont1 = objectTreeViewItem.scene == SceneManager.GetActiveScene();
        EditorGUI.BeginDisabledGroup(!objectTreeViewItem.scene.isLoaded);
        base.DrawIconAndLabel(rect, item, label, selected, focused, useBoldFont1, isPinging);
        EditorGUI.EndDisabledGroup();
      }
      else
      {
        if (!isPinging)
        {
          float contentIndent = this.GetContentIndent(item);
          rect.x += contentIndent;
          rect.width -= contentIndent;
        }
        int colorCode = objectTreeViewItem.colorCode;
        if (string.IsNullOrEmpty(item.displayName))
        {
          objectTreeViewItem.displayName = !(objectTreeViewItem.objectPPTR != (UnityEngine.Object) null) ? "deleted gameobject" : objectTreeViewItem.objectPPTR.name;
          label = objectTreeViewItem.displayName;
        }
        GUIStyle guiStyle = TreeViewGUI.s_Styles.lineStyle;
        if (!objectTreeViewItem.shouldDisplay)
          guiStyle = GameObjectTreeViewGUI.s_GOStyles.disabledLabel;
        else if ((colorCode & 3) == 0)
          guiStyle = colorCode >= 4 ? GameObjectTreeViewGUI.s_GOStyles.disabledLabel : TreeViewGUI.s_Styles.lineStyle;
        else if ((colorCode & 3) == 1)
          guiStyle = colorCode >= 4 ? GameObjectTreeViewGUI.s_GOStyles.disabledPrefabLabel : GameObjectTreeViewGUI.s_GOStyles.prefabLabel;
        else if ((colorCode & 3) == 2)
          guiStyle = colorCode >= 4 ? GameObjectTreeViewGUI.s_GOStyles.disabledBrokenPrefabLabel : GameObjectTreeViewGUI.s_GOStyles.brokenPrefabLabel;
        guiStyle.padding.left = (int) this.k_SpaceBetweenIconAndText;
        guiStyle.Draw(rect, label, false, false, selected, focused);
      }
    }

    private enum GameObjectColorType
    {
      Normal,
      Prefab,
      BrokenPrefab,
      Count,
    }

    internal class GameObjectStyles
    {
      public GUIStyle disabledLabel = new GUIStyle((GUIStyle) "PR DisabledLabel");
      public GUIStyle prefabLabel = new GUIStyle((GUIStyle) "PR PrefabLabel");
      public GUIStyle disabledPrefabLabel = new GUIStyle((GUIStyle) "PR DisabledPrefabLabel");
      public GUIStyle brokenPrefabLabel = new GUIStyle((GUIStyle) "PR BrokenPrefabLabel");
      public GUIStyle disabledBrokenPrefabLabel = new GUIStyle((GUIStyle) "PR DisabledBrokenPrefabLabel");
      public GUIContent loadSceneGUIContent = new GUIContent((Texture) EditorGUIUtility.FindTexture("SceneLoadIn"), "Load scene");
      public GUIContent unloadSceneGUIContent = new GUIContent((Texture) EditorGUIUtility.FindTexture("SceneLoadOut"), "Unload scene");
      public GUIContent saveSceneGUIContent = new GUIContent((Texture) EditorGUIUtility.FindTexture("SceneSave"), "Save scene");
      public GUIStyle optionsButtonStyle = (GUIStyle) "PaneOptions";
      public GUIStyle sceneHeaderBg = (GUIStyle) "ProjectBrowserTopBarBg";
      public readonly int kSceneHeaderIconsInterval = 2;

      public GameObjectStyles()
      {
        this.disabledLabel.alignment = TextAnchor.MiddleLeft;
        this.prefabLabel.alignment = TextAnchor.MiddleLeft;
        this.disabledPrefabLabel.alignment = TextAnchor.MiddleLeft;
        this.brokenPrefabLabel.alignment = TextAnchor.MiddleLeft;
        this.disabledBrokenPrefabLabel.alignment = TextAnchor.MiddleLeft;
      }
    }
  }
}

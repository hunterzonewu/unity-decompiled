// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class TreeViewGUI : ITreeViewGUI
  {
    protected PingData m_Ping = new PingData();
    private bool m_AnimateScrollBarOnExpandCollapse = true;
    protected float k_LineHeight = 16f;
    protected float k_BaseIndent = 2f;
    protected float k_IndentWidth = 14f;
    protected float k_FoldoutWidth = 12f;
    protected float k_IconWidth = 16f;
    protected float k_SpaceBetweenIconAndText = 2f;
    protected float k_HalfDropBetweenHeight = 4f;
    protected TreeView m_TreeView;
    protected Rect m_DraggingInsertionMarkerRect;
    protected bool m_UseHorizontalScroll;
    protected float k_TopRowMargin;
    protected float k_BottomRowMargin;
    protected static TreeViewGUI.Styles s_Styles;

    public float iconLeftPadding { get; set; }

    public float iconRightPadding { get; set; }

    public float iconTotalPadding
    {
      get
      {
        return this.iconLeftPadding + this.iconRightPadding;
      }
    }

    public System.Action<TreeViewItem, Rect> iconOverlayGUI { get; set; }

    protected float indentWidth
    {
      get
      {
        return this.k_IndentWidth + this.iconTotalPadding;
      }
    }

    public float halfDropBetweenHeight
    {
      get
      {
        return this.k_HalfDropBetweenHeight;
      }
    }

    public virtual float topRowMargin
    {
      get
      {
        return this.k_TopRowMargin;
      }
    }

    public virtual float bottomRowMargin
    {
      get
      {
        return this.k_BottomRowMargin;
      }
    }

    public TreeViewGUI(TreeView treeView)
    {
      this.m_TreeView = treeView;
    }

    public TreeViewGUI(TreeView treeView, bool useHorizontalScroll)
    {
      this.m_TreeView = treeView;
      this.m_UseHorizontalScroll = useHorizontalScroll;
    }

    public virtual void OnInitialize()
    {
    }

    protected virtual void InitStyles()
    {
      if (TreeViewGUI.s_Styles != null)
        return;
      TreeViewGUI.s_Styles = new TreeViewGUI.Styles();
    }

    protected virtual Texture GetIconForNode(TreeViewItem item)
    {
      return (Texture) item.icon;
    }

    public virtual Vector2 GetTotalSize()
    {
      this.InitStyles();
      float x = 1f;
      if (this.m_UseHorizontalScroll)
        x = this.GetMaxWidth(this.m_TreeView.data.GetRows());
      float y = (float) this.m_TreeView.data.rowCount * this.k_LineHeight + this.topRowMargin + this.bottomRowMargin;
      if (this.m_AnimateScrollBarOnExpandCollapse && this.m_TreeView.expansionAnimator.isAnimating)
        y -= this.m_TreeView.expansionAnimator.deltaHeight;
      return new Vector2(x, y);
    }

    protected float GetMaxWidth(List<TreeViewItem> rows)
    {
      this.InitStyles();
      float num1 = 1f;
      using (List<TreeViewItem>.Enumerator enumerator = rows.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TreeViewItem current = enumerator.Current;
          float num2 = 0.0f + this.GetContentIndent(current);
          if ((UnityEngine.Object) current.icon != (UnityEngine.Object) null)
            num2 += this.k_IconWidth;
          float minWidth;
          float maxWidth;
          TreeViewGUI.s_Styles.lineStyle.CalcMinMaxWidth(GUIContent.Temp(current.displayName), out minWidth, out maxWidth);
          float num3 = num2 + maxWidth + this.k_BaseIndent;
          if ((double) num3 > (double) num1)
            num1 = num3;
        }
      }
      return num1;
    }

    public virtual int GetNumRowsOnPageUpDown(TreeViewItem fromItem, bool pageUp, float heightOfTreeView)
    {
      return (int) Mathf.Floor(heightOfTreeView / this.k_LineHeight);
    }

    public virtual void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible)
    {
      if (this.m_TreeView.data.rowCount == 0)
      {
        firstRowVisible = lastRowVisible = -1;
      }
      else
      {
        float y = this.m_TreeView.state.scrollPos.y;
        float height = this.m_TreeView.GetTotalRect().height;
        firstRowVisible = (int) Mathf.Floor((y - this.topRowMargin) / this.k_LineHeight);
        lastRowVisible = firstRowVisible + (int) Mathf.Ceil(height / this.k_LineHeight);
        firstRowVisible = Mathf.Max(firstRowVisible, 0);
        lastRowVisible = Mathf.Min(lastRowVisible, this.m_TreeView.data.rowCount - 1);
        if (firstRowVisible < this.m_TreeView.data.rowCount || firstRowVisible <= 0)
          return;
        this.m_TreeView.state.scrollPos.y = 0.0f;
        this.GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible);
      }
    }

    public virtual void BeginRowGUI()
    {
      this.InitStyles();
      this.m_DraggingInsertionMarkerRect.x = -1f;
      this.SyncFakeItem();
      if (Event.current.type == EventType.Repaint)
        return;
      this.DoRenameOverlay();
    }

    public virtual void EndRowGUI()
    {
      if ((double) this.m_DraggingInsertionMarkerRect.x >= 0.0 && Event.current.type == EventType.Repaint)
      {
        if (this.m_TreeView.dragging.drawRowMarkerAbove)
          TreeViewGUI.s_Styles.insertionAbove.Draw(this.m_DraggingInsertionMarkerRect, false, false, false, false);
        else
          TreeViewGUI.s_Styles.insertion.Draw(this.m_DraggingInsertionMarkerRect, false, false, false, false);
      }
      if (Event.current.type == EventType.Repaint)
        this.DoRenameOverlay();
      this.HandlePing();
    }

    public virtual void OnRowGUI(Rect rowRect, TreeViewItem item, int row, bool selected, bool focused)
    {
      this.DoNodeGUI(rowRect, row, item, selected, focused, false);
    }

    protected virtual void DoNodeGUI(Rect rect, int row, TreeViewItem item, bool selected, bool focused, bool useBoldFont)
    {
      EditorGUIUtility.SetIconSize(new Vector2(this.k_IconWidth, this.k_IconWidth));
      float foldoutIndent = this.GetFoldoutIndent(item);
      int itemControlId = TreeView.GetItemControlID(item);
      bool flag1 = false;
      if (this.m_TreeView.dragging != null)
        flag1 = this.m_TreeView.dragging.GetDropTargetControlID() == itemControlId && this.m_TreeView.data.CanBeParent(item);
      bool flag2 = this.IsRenaming(item.id);
      bool flag3 = this.m_TreeView.data.IsExpandable(item);
      if (flag2 && Event.current.type == EventType.Repaint)
      {
        float num1 = !((UnityEngine.Object) item.icon == (UnityEngine.Object) null) ? this.k_IconWidth : 0.0f;
        float num2 = (float) ((double) foldoutIndent + (double) this.k_FoldoutWidth + (double) num1 + (double) this.iconTotalPadding - 1.0);
        this.GetRenameOverlay().editFieldRect = new Rect(rect.x + num2, rect.y, rect.width - num2, rect.height);
      }
      if (Event.current.type == EventType.Repaint)
      {
        string label = item.displayName;
        if (flag2)
        {
          selected = false;
          label = string.Empty;
        }
        if (selected)
          TreeViewGUI.s_Styles.selectionStyle.Draw(rect, false, false, true, focused);
        if (flag1)
          TreeViewGUI.s_Styles.lineStyle.Draw(rect, GUIContent.none, true, true, false, false);
        this.DrawIconAndLabel(rect, item, label, selected, focused, useBoldFont, false);
        if (this.m_TreeView.dragging != null && this.m_TreeView.dragging.GetRowMarkerControlID() == itemControlId)
          this.m_DraggingInsertionMarkerRect = new Rect(rect.x + foldoutIndent + this.k_FoldoutWidth, rect.y, rect.width - foldoutIndent, rect.height);
      }
      if (flag3)
        this.DoFoldout(rect, item, row);
      EditorGUIUtility.SetIconSize(Vector2.zero);
    }

    private float GetTopPixelOfRow(int row)
    {
      return (float) row * this.k_LineHeight + this.topRowMargin;
    }

    public virtual Rect GetRowRect(int row, float rowWidth)
    {
      return new Rect(0.0f, this.GetTopPixelOfRow(row), rowWidth, this.k_LineHeight);
    }

    public virtual Rect GetRectForFraming(int row)
    {
      return this.GetRowRect(row, 1f);
    }

    protected virtual Rect DoFoldout(Rect rowRect, TreeViewItem item, int row)
    {
      Rect position = new Rect(this.GetFoldoutIndent(item), rowRect.y, this.k_FoldoutWidth, rowRect.height);
      TreeViewItemExpansionAnimator expansionAnimator = this.m_TreeView.expansionAnimator;
      EditorGUI.BeginChangeCheck();
      bool expand;
      if (expansionAnimator.IsAnimating(item.id))
      {
        Matrix4x4 matrix = GUI.matrix;
        float num = Mathf.Min(1f, expansionAnimator.expandedValueNormalized * 2f);
        GUIUtility.RotateAroundPivot(expansionAnimator.isExpanding ? (float) ((1.0 - (double) num) * -90.0) : num * 90f, position.center);
        bool isExpanding = expansionAnimator.isExpanding;
        expand = GUI.Toggle(position, isExpanding, GUIContent.none, TreeViewGUI.s_Styles.foldout);
        GUI.matrix = matrix;
      }
      else
        expand = GUI.Toggle(position, this.m_TreeView.data.IsExpanded(item), GUIContent.none, TreeViewGUI.s_Styles.foldout);
      if (EditorGUI.EndChangeCheck())
        this.m_TreeView.UserInputChangedExpandedState(item, row, expand);
      return position;
    }

    protected virtual void DrawIconAndLabel(Rect rect, TreeViewItem item, string label, bool selected, bool focused, bool useBoldFont, bool isPinging)
    {
      if (!isPinging)
      {
        float contentIndent = this.GetContentIndent(item);
        rect.x += contentIndent;
        rect.width -= contentIndent;
      }
      GUIStyle guiStyle = !useBoldFont ? TreeViewGUI.s_Styles.lineStyle : TreeViewGUI.s_Styles.lineBoldStyle;
      guiStyle.padding.left = (int) ((double) this.k_IconWidth + (double) this.iconTotalPadding + (double) this.k_SpaceBetweenIconAndText);
      guiStyle.Draw(rect, label, false, false, selected, focused);
      Rect position = rect;
      position.width = this.k_IconWidth;
      position.height = this.k_IconWidth;
      position.x += this.iconLeftPadding;
      Texture iconForNode = this.GetIconForNode(item);
      if ((UnityEngine.Object) iconForNode != (UnityEngine.Object) null)
        GUI.DrawTexture(position, iconForNode);
      if (this.iconOverlayGUI == null)
        return;
      Rect rect1 = rect;
      rect1.width = this.k_IconWidth + this.iconTotalPadding;
      this.iconOverlayGUI(item, rect1);
    }

    public virtual void BeginPingNode(TreeViewItem item, float topPixelOfRow, float availableWidth)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TreeViewGUI.\u003CBeginPingNode\u003Ec__AnonStorey2B nodeCAnonStorey2B = new TreeViewGUI.\u003CBeginPingNode\u003Ec__AnonStorey2B();
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey2B.item = item;
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey2B.\u003C\u003Ef__this = this;
      // ISSUE: reference to a compiler-generated field
      if (nodeCAnonStorey2B.item == null || (double) topPixelOfRow < 0.0)
        return;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TreeViewGUI.\u003CBeginPingNode\u003Ec__AnonStorey2C nodeCAnonStorey2C = new TreeViewGUI.\u003CBeginPingNode\u003Ec__AnonStorey2C();
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey2C.\u003C\u003Ef__ref\u002443 = nodeCAnonStorey2B;
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey2C.\u003C\u003Ef__this = this;
      this.m_Ping.m_TimeStart = Time.realtimeSinceStartup;
      this.m_Ping.m_PingStyle = TreeViewGUI.s_Styles.ping;
      // ISSUE: reference to a compiler-generated field
      Vector2 vector2 = this.m_Ping.m_PingStyle.CalcSize(GUIContent.Temp(nodeCAnonStorey2B.item.displayName));
      // ISSUE: reference to a compiler-generated field
      this.m_Ping.m_ContentRect = new Rect(this.GetContentIndent(nodeCAnonStorey2B.item), topPixelOfRow, this.k_IconWidth + this.k_SpaceBetweenIconAndText + vector2.x + this.iconTotalPadding, vector2.y);
      this.m_Ping.m_AvailableWidth = availableWidth;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey2C.useBoldFont = nodeCAnonStorey2B.item.displayName.Equals("Assets");
      // ISSUE: reference to a compiler-generated method
      this.m_Ping.m_ContentDraw = new System.Action<Rect>(nodeCAnonStorey2C.\u003C\u003Em__3F);
      this.m_TreeView.Repaint();
    }

    public virtual void EndPingNode()
    {
      this.m_Ping.m_TimeStart = -1f;
    }

    private void HandlePing()
    {
      this.m_Ping.HandlePing();
      if (!this.m_Ping.isPinging)
        return;
      this.m_TreeView.Repaint();
    }

    protected RenameOverlay GetRenameOverlay()
    {
      return this.m_TreeView.state.renameOverlay;
    }

    protected virtual bool IsRenaming(int id)
    {
      if (this.GetRenameOverlay().IsRenaming() && this.GetRenameOverlay().userData == id)
        return !this.GetRenameOverlay().isWaitingForDelay;
      return false;
    }

    public virtual bool BeginRename(TreeViewItem item, float delay)
    {
      return this.GetRenameOverlay().BeginRename(item.displayName, item.id, delay);
    }

    public virtual void EndRename()
    {
      if (this.GetRenameOverlay().HasKeyboardFocus())
        this.m_TreeView.GrabKeyboardFocus();
      this.RenameEnded();
      this.ClearRenameAndNewNodeState();
    }

    protected virtual void RenameEnded()
    {
    }

    public virtual void DoRenameOverlay()
    {
      if (!this.GetRenameOverlay().IsRenaming() || this.GetRenameOverlay().OnGUI())
        return;
      this.EndRename();
    }

    protected virtual void SyncFakeItem()
    {
    }

    protected virtual void ClearRenameAndNewNodeState()
    {
      this.m_TreeView.data.RemoveFakeItem();
      this.GetRenameOverlay().Clear();
    }

    public virtual float GetFoldoutIndent(TreeViewItem item)
    {
      if (this.m_TreeView.isSearching)
        return this.k_BaseIndent;
      return this.k_BaseIndent + (float) item.depth * this.indentWidth;
    }

    public virtual float GetContentIndent(TreeViewItem item)
    {
      return this.GetFoldoutIndent(item) + this.k_FoldoutWidth;
    }

    internal class Styles
    {
      public GUIStyle foldout = (GUIStyle) "IN Foldout";
      public GUIStyle insertion = (GUIStyle) "PR Insertion";
      public GUIStyle insertionAbove = (GUIStyle) "PR Insertion Above";
      public GUIStyle ping = new GUIStyle((GUIStyle) "PR Ping");
      public GUIStyle toolbarButton = (GUIStyle) "ToolbarButton";
      public GUIStyle lineStyle = new GUIStyle((GUIStyle) "PR Label");
      public GUIStyle selectionStyle = new GUIStyle((GUIStyle) "PR Label");
      public GUIContent content = new GUIContent((Texture) EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName));
      public GUIStyle lineBoldStyle;

      public Styles()
      {
        Texture2D background = this.lineStyle.hover.background;
        this.lineStyle.onNormal.background = background;
        this.lineStyle.onActive.background = background;
        this.lineStyle.onFocused.background = background;
        this.lineStyle.alignment = TextAnchor.MiddleLeft;
        this.lineBoldStyle = new GUIStyle(this.lineStyle);
        this.lineBoldStyle.font = EditorStyles.boldLabel.font;
        this.lineBoldStyle.fontStyle = EditorStyles.boldLabel.fontStyle;
        this.ping.padding.left = 16;
        this.ping.padding.right = 16;
        this.ping.fixedHeight = 16f;
      }
    }
  }
}

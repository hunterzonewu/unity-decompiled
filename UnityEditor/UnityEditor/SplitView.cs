// Decompiled with JetBrains decompiler
// Type: UnityEditor.SplitView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class SplitView : View, ICleanuppable, IDropArea
  {
    internal const float kGrabDist = 5f;
    public bool vertical;
    public int controlID;
    private SplitterState splitState;
    private static float[] s_StartDragPos;
    private static float[] s_DragPos;

    private void SetupSplitter()
    {
      int[] realSizes = new int[this.children.Length];
      int[] minSizes = new int[this.children.Length];
      for (int index = 0; index < this.children.Length; ++index)
      {
        View child = this.children[index];
        realSizes[index] = !this.vertical ? (int) child.position.width : (int) child.position.height;
        minSizes[index] = !this.vertical ? (int) child.minSize.x : (int) child.minSize.y;
      }
      this.splitState = new SplitterState(realSizes, minSizes, (int[]) null);
      this.splitState.splitSize = 10;
    }

    private void SetupRectsFromSplitter()
    {
      if (this.children.Length == 0)
        return;
      int num1 = 0;
      int num2 = 0;
      foreach (int realSize in this.splitState.realSizes)
        num2 += realSize;
      float num3 = 1f;
      if ((double) num2 > (!this.vertical ? (double) this.position.width : (double) this.position.height))
        num3 = (!this.vertical ? this.position.width : this.position.height) / (float) num2;
      for (int index = 0; index < this.children.Length; ++index)
      {
        int num4 = (int) Mathf.Round((float) this.splitState.realSizes[index] * num3);
        if (this.vertical)
          this.children[index].position = new Rect(0.0f, (float) num1, this.position.width, (float) num4);
        else
          this.children[index].position = new Rect((float) num1, 0.0f, (float) num4, this.position.height);
        num1 += num4;
      }
    }

    private static void RecalcMinMaxAndReflowAll(SplitView start)
    {
      SplitView splitView = start;
      SplitView node;
      do
      {
        node = splitView;
        splitView = node.parent as SplitView;
      }
      while ((bool) ((Object) splitView));
      SplitView.RecalcMinMaxRecurse(node);
      SplitView.ReflowRecurse(node);
    }

    private static void RecalcMinMaxRecurse(SplitView node)
    {
      foreach (View child in node.children)
      {
        SplitView node1 = child as SplitView;
        if ((bool) ((Object) node1))
          SplitView.RecalcMinMaxRecurse(node1);
      }
      node.ChildrenMinMaxChanged();
    }

    private static void ReflowRecurse(SplitView node)
    {
      node.Reflow();
      foreach (View child in node.children)
      {
        SplitView node1 = child as SplitView;
        if ((bool) ((Object) node1))
          SplitView.RecalcMinMaxRecurse(node1);
      }
    }

    internal override void Reflow()
    {
      this.SetupSplitter();
      for (int i1 = 0; i1 < this.children.Length - 1; ++i1)
        this.splitState.DoSplitter(i1, i1 + 1, 0);
      this.splitState.RelativeToRealSizes(!this.vertical ? (int) this.position.width : (int) this.position.height);
      this.SetupRectsFromSplitter();
    }

    private void PlaceView(int i, float pos, float size)
    {
      float num = Mathf.Round(pos);
      if (this.vertical)
        this.children[i].position = new Rect(0.0f, num, this.position.width, Mathf.Round(pos + size) - num);
      else
        this.children[i].position = new Rect(num, 0.0f, Mathf.Round(pos + size) - num, this.position.height);
    }

    public override void AddChild(View child, int idx)
    {
      base.AddChild(child, idx);
      this.ChildrenMinMaxChanged();
      this.splitState = (SplitterState) null;
    }

    public void RemoveChildNice(View child)
    {
      if (this.children.Length != 1)
      {
        int num1 = this.IndexOfChild(child);
        float t = num1 != 0 ? (num1 != this.children.Length - 1 ? 0.5f : 1f) : 0.0f;
        float num2 = !this.vertical ? Mathf.Lerp(child.position.xMin, child.position.xMax, t) : Mathf.Lerp(child.position.yMin, child.position.yMax, t);
        if (num1 > 0)
        {
          View child1 = this.children[num1 - 1];
          Rect position = child1.position;
          if (this.vertical)
            position.yMax = num2;
          else
            position.xMax = num2;
          child1.position = position;
          if (child1 is SplitView)
            ((SplitView) child1).Reflow();
        }
        if (num1 < this.children.Length - 1)
        {
          View child1 = this.children[num1 + 1];
          Rect position = child1.position;
          child1.position = !this.vertical ? new Rect(num2, position.y, position.xMax - num2, position.height) : new Rect(position.x, num2, position.width, position.yMax - num2);
          if (child1 is SplitView)
            ((SplitView) child1).Reflow();
        }
      }
      this.RemoveChild(child);
    }

    public override void RemoveChild(View child)
    {
      this.splitState = (SplitterState) null;
      base.RemoveChild(child);
    }

    private DropInfo DoDropZone(int idx, Vector2 mousePos, Rect sourceRect, Rect previewRect)
    {
      if (!sourceRect.Contains(mousePos))
        return (DropInfo) null;
      return new DropInfo((IDropArea) this) { type = DropInfo.Type.Pane, userData = (object) idx, rect = previewRect };
    }

    private DropInfo CheckRootWindowDropZones(Vector2 mouseScreenPosition)
    {
      DropInfo dropInfo1 = (DropInfo) null;
      if (!(this.parent is SplitView) && (this.children.Length != 1 || !((Object) DockArea.s_IgnoreDockingForView == (Object) this.children[0])))
      {
        Rect screenPosition = this.screenPosition;
        DropInfo dropInfo2 = !(this.parent is MainWindow) ? this.DoDropZone(-1, mouseScreenPosition, new Rect(screenPosition.x, screenPosition.yMax - 20f, screenPosition.width, 100f), new Rect(screenPosition.x, screenPosition.yMax - 50f, screenPosition.width, 200f)) : this.DoDropZone(-1, mouseScreenPosition, new Rect(screenPosition.x, screenPosition.yMax, screenPosition.width, 100f), new Rect(screenPosition.x, screenPosition.yMax - 200f, screenPosition.width, 200f));
        if (dropInfo2 != null)
          return dropInfo2;
        DropInfo dropInfo3 = this.DoDropZone(-2, mouseScreenPosition, new Rect(screenPosition.x - 30f, screenPosition.y, 50f, screenPosition.height), new Rect(screenPosition.x - 50f, screenPosition.y, 100f, screenPosition.height));
        if (dropInfo3 != null)
          return dropInfo3;
        dropInfo1 = this.DoDropZone(-3, mouseScreenPosition, new Rect(screenPosition.xMax - 20f, screenPosition.y, 50f, screenPosition.height), new Rect(screenPosition.xMax - 50f, screenPosition.y, 100f, screenPosition.height));
      }
      return dropInfo1;
    }

    public DropInfo DragOver(EditorWindow w, Vector2 mouseScreenPosition)
    {
      DropInfo dropInfo = this.CheckRootWindowDropZones(mouseScreenPosition);
      if (dropInfo != null)
        return dropInfo;
      for (int index = 0; index < this.children.Length; ++index)
      {
        View child = this.children[index];
        if (!((Object) child == (Object) DockArea.s_IgnoreDockingForView) && !(child is SplitView))
        {
          Rect screenPosition = child.screenPosition;
          int num1 = 0;
          float num2 = Mathf.Round(Mathf.Min(screenPosition.width / 3f, 300f));
          float height1 = Mathf.Round(Mathf.Min(screenPosition.height / 3f, 300f));
          if (new Rect(screenPosition.x, screenPosition.y + 39f, num2, screenPosition.height - 39f).Contains(mouseScreenPosition))
            num1 |= 1;
          if (new Rect(screenPosition.x, screenPosition.yMax - height1, screenPosition.width, height1).Contains(mouseScreenPosition))
            num1 |= 2;
          if (new Rect(screenPosition.xMax - num2, screenPosition.y + 39f, num2, screenPosition.height - 39f).Contains(mouseScreenPosition))
            num1 |= 4;
          if (num1 == 3)
          {
            Vector2 vector2_1 = new Vector2(screenPosition.x, screenPosition.yMax) - mouseScreenPosition;
            Vector2 vector2_2 = new Vector2(num2, -height1);
            num1 = (double) vector2_1.x * (double) vector2_2.y - (double) vector2_1.y * (double) vector2_2.x >= 0.0 ? 2 : 1;
          }
          else if (num1 == 6)
          {
            Vector2 vector2_1 = new Vector2(screenPosition.xMax, screenPosition.yMax) - mouseScreenPosition;
            Vector2 vector2_2 = new Vector2(-num2, -height1);
            num1 = (double) vector2_1.x * (double) vector2_2.y - (double) vector2_1.y * (double) vector2_2.x >= 0.0 ? 4 : 2;
          }
          float width = Mathf.Round(Mathf.Max(screenPosition.width / 3f, 100f));
          float height2 = Mathf.Round(Mathf.Max(screenPosition.height / 3f, 100f));
          if (this.vertical)
          {
            switch (num1)
            {
              case 1:
                return new DropInfo((IDropArea) this) { userData = (object) (index + 1000), type = DropInfo.Type.Pane, rect = new Rect(screenPosition.x, screenPosition.y, width, screenPosition.height) };
              case 2:
                return new DropInfo((IDropArea) this) { userData = (object) (index + 1), type = DropInfo.Type.Pane, rect = new Rect(screenPosition.x, screenPosition.yMax - height2, screenPosition.width, height2) };
              case 4:
                return new DropInfo((IDropArea) this) { userData = (object) (index + 2000), type = DropInfo.Type.Pane, rect = new Rect(screenPosition.xMax - width, screenPosition.y, width, screenPosition.height) };
              default:
                continue;
            }
          }
          else
          {
            switch (num1)
            {
              case 1:
                return new DropInfo((IDropArea) this) { userData = (object) index, type = DropInfo.Type.Pane, rect = new Rect(screenPosition.x, screenPosition.y, width, screenPosition.height) };
              case 2:
                return new DropInfo((IDropArea) this) { userData = (object) (index + 2000), type = DropInfo.Type.Pane, rect = new Rect(screenPosition.x, screenPosition.yMax - height2, screenPosition.width, height2) };
              case 4:
                return new DropInfo((IDropArea) this) { userData = (object) (index + 1), type = DropInfo.Type.Pane, rect = new Rect(screenPosition.xMax - width, screenPosition.y, width, screenPosition.height) };
              default:
                continue;
            }
          }
        }
      }
      if (this.screenPosition.Contains(mouseScreenPosition) && !(this.parent is SplitView))
        return new DropInfo((IDropArea) null);
      return (DropInfo) null;
    }

    protected override void ChildrenMinMaxChanged()
    {
      Vector2 zero1 = Vector2.zero;
      Vector2 zero2 = Vector2.zero;
      if (this.vertical)
      {
        foreach (View child in this.children)
        {
          zero1.x = Mathf.Max(child.minSize.x, zero1.x);
          zero2.x = Mathf.Max(child.maxSize.x, zero2.x);
          zero1.y += child.minSize.y;
          zero2.y += child.maxSize.y;
        }
      }
      else
      {
        foreach (View child in this.children)
        {
          zero1.x += child.minSize.x;
          zero2.x += child.maxSize.x;
          zero1.y = Mathf.Max(child.minSize.y, zero1.y);
          zero2.y = Mathf.Max(child.maxSize.y, zero2.y);
        }
      }
      this.splitState = (SplitterState) null;
      this.SetMinMaxSizes(zero1, zero2);
    }

    public override string ToString()
    {
      return this.vertical ? "SplitView (vert)" : "SplitView (horiz)";
    }

    public bool PerformDrop(EditorWindow w, DropInfo di, Vector2 screenPos)
    {
      int userData = (int) di.userData;
      DockArea instance1 = ScriptableObject.CreateInstance<DockArea>();
      Rect rect = di.rect;
      if (userData == -1 || userData == -2 || userData == -3)
      {
        bool flag1 = userData == -2;
        bool flag2 = userData == -1;
        this.splitState = (SplitterState) null;
        if (this.vertical == flag2 || this.children.Length < 2)
        {
          this.vertical = flag2;
          rect.x -= this.screenPosition.x;
          rect.y -= this.screenPosition.y;
          this.MakeRoomForRect(rect);
          this.AddChild((View) instance1, !flag1 ? this.children.Length : 0);
          instance1.position = rect;
        }
        else
        {
          SplitView instance2 = ScriptableObject.CreateInstance<SplitView>();
          Rect position = this.position;
          instance2.vertical = flag2;
          instance2.position = new Rect(position.x, position.y, position.width, position.height);
          if ((Object) this.window.mainView == (Object) this)
            this.window.mainView = (View) instance2;
          else
            this.parent.AddChild((View) instance2, this.parent.IndexOfChild((View) this));
          instance2.AddChild((View) this);
          this.position = new Rect(0.0f, 0.0f, position.width, position.height);
          Rect r = rect;
          r.x -= this.screenPosition.x;
          r.y -= this.screenPosition.y;
          instance2.MakeRoomForRect(r);
          instance1.position = r;
          instance2.AddChild((View) instance1, !flag1 ? 1 : 0);
        }
      }
      else if (userData < 1000)
      {
        Rect r = rect;
        r.x -= this.screenPosition.x;
        r.y -= this.screenPosition.y;
        this.MakeRoomForRect(r);
        this.AddChild((View) instance1, userData);
        instance1.position = r;
      }
      else
      {
        int idx = userData % 1000;
        if (this.children.Length != 1)
        {
          SplitView instance2 = ScriptableObject.CreateInstance<SplitView>();
          instance2.vertical = !this.vertical;
          Rect position = this.children[idx].position;
          instance2.AddChild(this.children[idx]);
          this.AddChild((View) instance2, idx);
          instance2.position = position;
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          Rect& local = @position;
          float num1 = 0.0f;
          position.y = num1;
          double num2 = (double) num1;
          // ISSUE: explicit reference operation
          (^local).x = (float) num2;
          instance2.children[0].position = position;
          Rect r = rect;
          r.x -= instance2.screenPosition.x;
          r.y -= instance2.screenPosition.y;
          instance2.MakeRoomForRect(r);
          instance2.AddChild((View) instance1, userData >= 2000 ? 1 : 0);
          instance1.position = r;
        }
        else
        {
          this.vertical = !this.vertical;
          Rect r = rect;
          r.x -= this.screenPosition.x;
          r.y -= this.screenPosition.y;
          this.MakeRoomForRect(r);
          this.AddChild((View) instance1, userData != 1000 ? 1 : 0);
          instance1.position = r;
        }
      }
      DockArea.s_OriginalDragSource.RemoveTab(w);
      w.m_Parent = (HostView) instance1;
      instance1.AddTab(w);
      this.Reflow();
      SplitView.RecalcMinMaxAndReflowAll(this);
      instance1.MakeVistaDWMHappyDance();
      return true;
    }

    private static string PosVals(float[] posVals)
    {
      string str = "[";
      foreach (float posVal in posVals)
        str = str + string.Empty + (object) posVal + ", ";
      return str + "]";
    }

    private void MakeRoomForRect(Rect r)
    {
      Rect[] sources = new Rect[this.children.Length];
      for (int index = 0; index < sources.Length; ++index)
        sources[index] = this.children[index].position;
      this.CalcRoomForRect(sources, r);
      for (int index = 0; index < sources.Length; ++index)
        this.children[index].position = sources[index];
    }

    private void CalcRoomForRect(Rect[] sources, Rect r)
    {
      float num1 = !this.vertical ? r.x : r.y;
      float num2 = num1 + (!this.vertical ? r.width : r.height);
      float num3 = (float) (((double) num1 + (double) num2) * 0.5);
      int index1 = 0;
      while (index1 < sources.Length && (!this.vertical ? (double) sources[index1].x + (double) sources[index1].width * 0.5 : (double) sources[index1].y + (double) sources[index1].height * 0.5) <= (double) num3)
        ++index1;
      float num4 = num1;
      for (int index2 = index1 - 1; index2 >= 0; --index2)
      {
        if (this.vertical)
        {
          sources[index2].yMax = num4;
          if ((double) sources[index2].height < (double) this.children[index2].minSize.y)
          {
            float num5 = sources[index2].yMax - this.children[index2].minSize.y;
            sources[index2].yMin = num5;
            num4 = num5;
          }
          else
            break;
        }
        else
        {
          sources[index2].xMax = num4;
          if ((double) sources[index2].width < (double) this.children[index2].minSize.x)
          {
            float num5 = sources[index2].xMax - this.children[index2].minSize.x;
            sources[index2].xMin = num5;
            num4 = num5;
          }
          else
            break;
        }
      }
      if ((double) num4 < 0.0)
      {
        float num5 = -num4;
        for (int index2 = 0; index2 < index1 - 1; ++index2)
        {
          if (this.vertical)
            sources[index2].y += num5;
          else
            sources[index2].x += num5;
        }
        num2 += num5;
      }
      float num6 = num2;
      for (int index2 = index1; index2 < sources.Length; ++index2)
      {
        if (this.vertical)
        {
          float yMax = sources[index2].yMax;
          sources[index2].yMin = num6;
          sources[index2].yMax = yMax;
          if ((double) sources[index2].height < (double) this.children[index2].minSize.y)
          {
            float num5 = sources[index2].yMin + this.children[index2].minSize.y;
            sources[index2].yMax = num5;
            num6 = num5;
          }
          else
            break;
        }
        else
        {
          float xMax = sources[index2].xMax;
          sources[index2].xMin = num6;
          sources[index2].xMax = xMax;
          if ((double) sources[index2].width < (double) this.children[index2].minSize.x)
          {
            float num5 = sources[index2].xMin + this.children[index2].minSize.x;
            sources[index2].xMax = num5;
            num6 = num5;
          }
          else
            break;
        }
      }
      float num7 = !this.vertical ? this.position.width : this.position.height;
      if ((double) num6 <= (double) num7)
        return;
      float num8 = num7 - num6;
      for (int index2 = 0; index2 < index1 - 1; ++index2)
      {
        if (this.vertical)
          sources[index2].y += num8;
        else
          sources[index2].x += num8;
      }
      float num9 = num2 + num8;
    }

    public void Cleanup()
    {
      SplitView parent1 = this.parent as SplitView;
      if (this.children.Length == 1 && (Object) parent1 != (Object) null)
      {
        View child = this.children[0];
        child.position = this.position;
        if ((Object) this.parent != (Object) null)
        {
          this.parent.AddChild(child, this.parent.IndexOfChild((View) this));
          this.parent.RemoveChild((View) this);
          if ((bool) ((Object) parent1))
            parent1.Cleanup();
          child.position = this.position;
          if (Unsupported.IsDestroyScriptableObject((ScriptableObject) this))
            return;
          Object.DestroyImmediate((Object) this);
          return;
        }
        if (child is SplitView)
        {
          this.RemoveChild(child);
          this.window.mainView = child;
          child.position = new Rect(0.0f, 0.0f, child.window.position.width, this.window.position.height);
          child.Reflow();
          if (Unsupported.IsDestroyScriptableObject((ScriptableObject) this))
            return;
          Object.DestroyImmediate((Object) this);
          return;
        }
      }
      if ((bool) ((Object) parent1))
      {
        parent1.Cleanup();
        SplitView parent2 = this.parent as SplitView;
        if ((bool) ((Object) parent2) && parent2.vertical == this.vertical)
        {
          int num = new List<View>((IEnumerable<View>) this.parent.children).IndexOf((View) this);
          foreach (View child in this.children)
          {
            parent2.AddChild(child, num++);
            child.position = new Rect(this.position.x + child.position.x, this.position.y + child.position.y, child.position.width, child.position.height);
          }
        }
      }
      if (this.children.Length == 0)
      {
        if ((Object) this.parent == (Object) null && (Object) this.window != (Object) null)
        {
          this.window.Close();
        }
        else
        {
          ICleanuppable parent2 = this.parent as ICleanuppable;
          if (this.parent is SplitView)
          {
            ((SplitView) this.parent).RemoveChildNice((View) this);
            if (!Unsupported.IsDestroyScriptableObject((ScriptableObject) this))
              Object.DestroyImmediate((Object) this, true);
          }
          parent2.Cleanup();
        }
      }
      else
      {
        this.splitState = (SplitterState) null;
        this.Reflow();
      }
    }

    public void SplitGUI(Event evt)
    {
      if (this.splitState == null)
        this.SetupSplitter();
      SplitView parent = this.parent as SplitView;
      if ((bool) ((Object) parent))
      {
        Event evt1 = new Event(evt);
        evt1.mousePosition += new Vector2(this.position.x, this.position.y);
        parent.SplitGUI(evt1);
        if (evt1.type == EventType.Used)
          evt.Use();
      }
      float num1 = !this.vertical ? evt.mousePosition.x : evt.mousePosition.y;
      int controlId = GUIUtility.GetControlID(546739, FocusType.Passive);
      this.controlID = controlId;
      switch (evt.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (this.children.Length == 1)
            break;
          int num2 = !this.vertical ? (int) this.children[0].position.x : (int) this.children[0].position.y;
          for (int index = 0; index < this.children.Length - 1; ++index)
          {
            if (index >= this.splitState.realSizes.Length)
            {
              DockArea current = GUIView.current as DockArea;
              string str = "Non-dock area " + (object) GUIView.current.GetType();
              if ((bool) ((Object) current) && current.m_Selected < current.m_Panes.Count && (bool) ((Object) current.m_Panes[current.m_Selected]))
                str = current.m_Panes[current.m_Selected].GetType().ToString();
              if (Unsupported.IsDeveloperBuild())
                Debug.LogError((object) ("Real sizes out of bounds for: " + str + " index: " + (object) index + " RealSizes: " + (object) this.splitState.realSizes.Length));
              this.SetupSplitter();
            }
            if ((!this.vertical ? new Rect((float) (num2 + this.splitState.realSizes[index] - this.splitState.splitSize / 2), this.children[0].position.y, (float) this.splitState.splitSize, this.children[0].position.height) : new Rect(this.children[0].position.x, (float) (num2 + this.splitState.realSizes[index] - this.splitState.splitSize / 2), this.children[0].position.width, (float) this.splitState.splitSize)).Contains(evt.mousePosition))
            {
              this.splitState.splitterInitialOffset = (int) num1;
              this.splitState.currentActiveSplitter = index;
              GUIUtility.hotControl = controlId;
              evt.Use();
              break;
            }
            num2 += this.splitState.realSizes[index];
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          GUIUtility.hotControl = 0;
          break;
        case EventType.MouseDrag:
          if (this.children.Length <= 1 || GUIUtility.hotControl != controlId || this.splitState.currentActiveSplitter < 0)
            break;
          int diff = (int) num1 - this.splitState.splitterInitialOffset;
          if (diff != 0)
          {
            this.splitState.splitterInitialOffset = (int) num1;
            this.splitState.DoSplitter(this.splitState.currentActiveSplitter, this.splitState.currentActiveSplitter + 1, diff);
          }
          this.SetupRectsFromSplitter();
          evt.Use();
          break;
      }
    }

    protected override void SetPosition(Rect newPos)
    {
      base.SetPosition(newPos);
      this.Reflow();
    }

    private class ExtraDropInfo
    {
      public Rect dropRect;
      public int idx;

      public ExtraDropInfo(Rect _dropRect, int _idx)
      {
        this.dropRect = _dropRect;
        this.idx = _idx;
      }
    }
  }
}

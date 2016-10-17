// Decompiled with JetBrains decompiler
// Type: UnityEditor.View
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEditor
{
  [StructLayout(LayoutKind.Sequential)]
  internal class View : ScriptableObject
  {
    [SerializeField]
    private View[] m_Children = new View[0];
    [SerializeField]
    private Rect m_Position = new Rect(0.0f, 0.0f, 100f, 100f);
    [SerializeField]
    private MonoReloadableIntPtr m_ViewPtr;
    [NonSerialized]
    private View m_Parent;
    [NonSerialized]
    private ContainerWindow m_Window;
    [SerializeField]
    internal Vector2 m_MinSize;
    [SerializeField]
    internal Vector2 m_MaxSize;

    public Vector2 minSize
    {
      get
      {
        return this.m_MinSize;
      }
    }

    public Vector2 maxSize
    {
      get
      {
        return this.m_MaxSize;
      }
    }

    public View[] allChildren
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        foreach (View child in this.m_Children)
          arrayList.AddRange((ICollection) child.allChildren);
        arrayList.Add((object) this);
        return (View[]) arrayList.ToArray(typeof (View));
      }
    }

    public Rect position
    {
      get
      {
        return this.m_Position;
      }
      set
      {
        this.SetPosition(value);
      }
    }

    public Rect windowPosition
    {
      get
      {
        if ((UnityEngine.Object) this.m_Parent == (UnityEngine.Object) null)
          return this.position;
        Rect windowPosition = this.parent.windowPosition;
        return new Rect(windowPosition.x + this.position.x, windowPosition.y + this.position.y, this.position.width, this.position.height);
      }
    }

    public Rect screenPosition
    {
      get
      {
        Rect windowPosition = this.windowPosition;
        if ((UnityEngine.Object) this.window != (UnityEngine.Object) null)
        {
          Vector2 screenPoint = this.window.WindowToScreenPoint(Vector2.zero);
          windowPosition.x += screenPoint.x;
          windowPosition.y += screenPoint.y;
        }
        return windowPosition;
      }
    }

    public ContainerWindow window
    {
      get
      {
        return this.m_Window;
      }
    }

    public View parent
    {
      get
      {
        return this.m_Parent;
      }
    }

    public View[] children
    {
      get
      {
        return this.m_Children;
      }
    }

    public View()
    {
      this.hideFlags = HideFlags.DontSave;
    }

    internal virtual void Reflow()
    {
      foreach (View child in this.children)
        child.Reflow();
    }

    internal string DebugHierarchy(int level)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 0; index < level; ++index)
        empty1 += "  ";
      string str1 = empty2 + empty1 + this.ToString() + " p:" + (object) this.position;
      string str2;
      if (this.children.Length > 0)
      {
        string str3 = str1 + " {\n";
        foreach (View child in this.children)
          str3 += child.DebugHierarchy(level + 2);
        str2 = str3 + empty1 + " }\n";
      }
      else
        str2 = str1 + "\n";
      return str2;
    }

    internal virtual void Initialize(ContainerWindow win)
    {
      this.SetWindow(win);
      foreach (View child in this.m_Children)
      {
        child.m_Parent = this;
        child.Initialize(win);
      }
    }

    internal void SetMinMaxSizes(Vector2 min, Vector2 max)
    {
      if (this.minSize == min && this.maxSize == max)
        return;
      this.m_MinSize = min;
      this.m_MaxSize = max;
      if ((bool) ((UnityEngine.Object) this.m_Parent))
        this.m_Parent.ChildrenMinMaxChanged();
      if (!(bool) ((UnityEngine.Object) this.window) || !((UnityEngine.Object) this.window.mainView == (UnityEngine.Object) this))
        return;
      this.window.SetMinMaxSizes(min, max);
    }

    protected virtual void ChildrenMinMaxChanged()
    {
    }

    protected virtual void SetPosition(Rect newPos)
    {
      this.m_Position = newPos;
    }

    internal void SetPositionOnly(Rect newPos)
    {
      this.m_Position = newPos;
    }

    public int IndexOfChild(View child)
    {
      int num = 0;
      foreach (UnityEngine.Object child1 in this.m_Children)
      {
        if (child1 == (UnityEngine.Object) child)
          return num;
        ++num;
      }
      return -1;
    }

    public void OnDestroy()
    {
      foreach (UnityEngine.Object child in this.m_Children)
        UnityEngine.Object.DestroyImmediate(child, true);
    }

    public void AddChild(View child)
    {
      this.AddChild(child, this.m_Children.Length);
    }

    public virtual void AddChild(View child, int idx)
    {
      Array.Resize<View>(ref this.m_Children, this.m_Children.Length + 1);
      if (idx != this.m_Children.Length - 1)
        Array.Copy((Array) this.m_Children, idx, (Array) this.m_Children, idx + 1, this.m_Children.Length - idx - 1);
      this.m_Children[idx] = child;
      if ((bool) ((UnityEngine.Object) child.m_Parent))
        child.m_Parent.RemoveChild(child);
      child.m_Parent = this;
      child.SetWindowRecurse(this.window);
      this.ChildrenMinMaxChanged();
    }

    public virtual void RemoveChild(View child)
    {
      int idx = Array.IndexOf<View>(this.m_Children, child);
      if (idx == -1)
        Debug.LogError((object) "Unable to remove child - it's not IN the view");
      else
        this.RemoveChild(idx);
    }

    public virtual void RemoveChild(int idx)
    {
      View child = this.m_Children[idx];
      child.m_Parent = (View) null;
      child.SetWindowRecurse((ContainerWindow) null);
      Array.Copy((Array) this.m_Children, idx + 1, (Array) this.m_Children, idx, this.m_Children.Length - idx - 1);
      Array.Resize<View>(ref this.m_Children, this.m_Children.Length - 1);
      this.ChildrenMinMaxChanged();
    }

    protected virtual void SetWindow(ContainerWindow win)
    {
      this.m_Window = win;
    }

    internal void SetWindowRecurse(ContainerWindow win)
    {
      this.SetWindow(win);
      foreach (View child in this.m_Children)
        child.SetWindowRecurse(win);
    }

    protected virtual bool OnFocus()
    {
      return true;
    }
  }
}

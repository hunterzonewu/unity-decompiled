// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.QuadTree`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class QuadTree<T> where T : IBounds
  {
    private Vector2 m_ScreenSpaceOffset = Vector2.zero;
    private QuadTreeNode<T> m_Root;
    private Rect m_Rectangle;

    public Vector2 screenSpaceOffset
    {
      get
      {
        return this.m_ScreenSpaceOffset;
      }
      set
      {
        this.m_ScreenSpaceOffset = value;
      }
    }

    public Rect rectangle
    {
      get
      {
        return this.m_Rectangle;
      }
    }

    public int Count
    {
      get
      {
        return this.m_Root.CountItemsIncludingChildren();
      }
    }

    public QuadTree()
    {
      this.Clear();
    }

    public void Clear()
    {
      this.SetSize(new Rect(0.0f, 0.0f, 1f, 1f));
    }

    public void SetSize(Rect rectangle)
    {
      this.m_Root = (QuadTreeNode<T>) null;
      this.m_Rectangle = rectangle;
      this.m_Root = new QuadTreeNode<T>(this.m_Rectangle);
    }

    public void Insert(List<T> items)
    {
      using (List<T>.Enumerator enumerator = items.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.Insert(enumerator.Current);
      }
    }

    public void Insert(T item)
    {
      this.m_Root.Insert(item);
    }

    public void Remove(T item)
    {
      this.m_Root.Remove(item);
    }

    public List<T> IntersectsWith(Rect area)
    {
      area.x -= this.m_ScreenSpaceOffset.x;
      area.y -= this.m_ScreenSpaceOffset.y;
      return this.m_Root.IntersectsWith(area);
    }

    public List<T> ContainedBy(Rect area)
    {
      area.x -= this.m_ScreenSpaceOffset.x;
      area.y -= this.m_ScreenSpaceOffset.y;
      return this.m_Root.ContainedBy(area);
    }

    public List<T> Elements()
    {
      return this.m_Root.GetElementsIncludingChildren();
    }

    public void DebugDraw()
    {
      this.m_Root.DebugDraw(this.m_ScreenSpaceOffset);
    }
  }
}

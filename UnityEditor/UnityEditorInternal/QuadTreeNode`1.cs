// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.QuadTreeNode`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class QuadTreeNode<T> where T : IBounds
  {
    private static Color m_DebugFillColor = new Color(1f, 1f, 1f, 0.01f);
    private static Color m_DebugWireColor = new Color(1f, 0.0f, 0.0f, 0.5f);
    private static Color m_DebugBoxFillColor = new Color(1f, 0.0f, 0.0f, 0.01f);
    private List<T> m_Elements = new List<T>();
    private List<QuadTreeNode<T>> m_ChildrenNodes = new List<QuadTreeNode<T>>(4);
    private const float kSmallestAreaForQuadTreeNode = 10f;
    private Rect m_BoundingRect;

    public bool IsEmpty
    {
      get
      {
        if ((double) this.m_BoundingRect.width != 0.0 || (double) this.m_BoundingRect.height != 0.0)
          return this.m_ChildrenNodes.Count == 0;
        return true;
      }
    }

    public Rect BoundingRect
    {
      get
      {
        return this.m_BoundingRect;
      }
    }

    public QuadTreeNode(Rect r)
    {
      this.m_BoundingRect = r;
    }

    public int CountItemsIncludingChildren()
    {
      return this.Count(true);
    }

    public int CountLocalItems()
    {
      return this.Count(false);
    }

    private int Count(bool recursive)
    {
      int count = this.m_Elements.Count;
      if (recursive)
      {
        using (List<QuadTreeNode<T>>.Enumerator enumerator = this.m_ChildrenNodes.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            QuadTreeNode<T> current = enumerator.Current;
            count += current.Count(recursive);
          }
        }
      }
      return count;
    }

    public List<T> GetElementsIncludingChildren()
    {
      return this.Elements(true);
    }

    public List<T> GetElements()
    {
      return this.Elements(false);
    }

    private List<T> Elements(bool recursive)
    {
      List<T> objList = new List<T>();
      if (recursive)
      {
        using (List<QuadTreeNode<T>>.Enumerator enumerator = this.m_ChildrenNodes.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            QuadTreeNode<T> current = enumerator.Current;
            objList.AddRange((IEnumerable<T>) current.Elements(recursive));
          }
        }
      }
      objList.AddRange((IEnumerable<T>) this.m_Elements);
      return objList;
    }

    public List<T> IntersectsWith(Rect queryArea)
    {
      List<T> objList = new List<T>();
      using (List<T>.Enumerator enumerator = this.m_Elements.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          T current = enumerator.Current;
          if (RectUtils.Intersects(current.boundingRect, queryArea))
            objList.Add(current);
        }
      }
      using (List<QuadTreeNode<T>>.Enumerator enumerator = this.m_ChildrenNodes.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          QuadTreeNode<T> current = enumerator.Current;
          if (!current.IsEmpty && RectUtils.Intersects(current.BoundingRect, queryArea))
          {
            objList.AddRange((IEnumerable<T>) current.IntersectsWith(queryArea));
            break;
          }
        }
      }
      return objList;
    }

    public List<T> ContainedBy(Rect queryArea)
    {
      List<T> objList = new List<T>();
      using (List<T>.Enumerator enumerator = this.m_Elements.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          T current = enumerator.Current;
          if (RectUtils.Contains(current.boundingRect, queryArea))
            objList.Add(current);
          else if (queryArea.Overlaps(current.boundingRect))
            objList.Add(current);
        }
      }
      using (List<QuadTreeNode<T>>.Enumerator enumerator = this.m_ChildrenNodes.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          QuadTreeNode<T> current = enumerator.Current;
          if (!current.IsEmpty)
          {
            if (RectUtils.Contains(current.BoundingRect, queryArea))
            {
              objList.AddRange((IEnumerable<T>) current.ContainedBy(queryArea));
              break;
            }
            if (RectUtils.Contains(queryArea, current.BoundingRect))
              objList.AddRange((IEnumerable<T>) current.Elements(true));
            else if (current.BoundingRect.Overlaps(queryArea))
              objList.AddRange((IEnumerable<T>) current.ContainedBy(queryArea));
          }
        }
      }
      return objList;
    }

    public void Remove(T item)
    {
      this.m_Elements.Remove(item);
      using (List<QuadTreeNode<T>>.Enumerator enumerator = this.m_ChildrenNodes.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Remove(item);
      }
    }

    public void Insert(T item)
    {
      if (!RectUtils.Contains(this.m_BoundingRect, item.boundingRect))
      {
        Rect intersection = new Rect();
        if (!RectUtils.Intersection(item.boundingRect, this.m_BoundingRect, out intersection))
          return;
      }
      if (this.m_ChildrenNodes.Count == 0)
        this.Subdivide();
      using (List<QuadTreeNode<T>>.Enumerator enumerator = this.m_ChildrenNodes.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          QuadTreeNode<T> current = enumerator.Current;
          if (RectUtils.Contains(current.BoundingRect, item.boundingRect))
          {
            current.Insert(item);
            return;
          }
        }
      }
      this.m_Elements.Add(item);
    }

    private void Subdivide()
    {
      if ((double) this.m_BoundingRect.height * (double) this.m_BoundingRect.width <= 10.0)
        return;
      float width = this.m_BoundingRect.width / 2f;
      float height = this.m_BoundingRect.height / 2f;
      this.m_ChildrenNodes.Add(new QuadTreeNode<T>(new Rect(this.m_BoundingRect.position.x, this.m_BoundingRect.position.y, width, height)));
      this.m_ChildrenNodes.Add(new QuadTreeNode<T>(new Rect(this.m_BoundingRect.xMin, this.m_BoundingRect.yMin + height, width, height)));
      this.m_ChildrenNodes.Add(new QuadTreeNode<T>(new Rect(this.m_BoundingRect.xMin + width, this.m_BoundingRect.yMin, width, height)));
      this.m_ChildrenNodes.Add(new QuadTreeNode<T>(new Rect(this.m_BoundingRect.xMin + width, this.m_BoundingRect.yMin + height, width, height)));
    }

    public void DebugDraw(Vector2 offset)
    {
      HandleUtility.ApplyWireMaterial();
      Rect boundingRect1 = this.m_BoundingRect;
      boundingRect1.x += offset.x;
      boundingRect1.y += offset.y;
      Handles.DrawSolidRectangleWithOutline(boundingRect1, QuadTreeNode<T>.m_DebugFillColor, QuadTreeNode<T>.m_DebugWireColor);
      using (List<QuadTreeNode<T>>.Enumerator enumerator = this.m_ChildrenNodes.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.DebugDraw(offset);
      }
      using (List<T>.Enumerator enumerator = this.Elements(false).GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Rect boundingRect2 = enumerator.Current.boundingRect;
          boundingRect2.x += offset.x;
          boundingRect2.y += offset.y;
          Handles.DrawSolidRectangleWithOutline(boundingRect2, QuadTreeNode<T>.m_DebugBoxFillColor, Color.yellow);
        }
      }
    }
  }
}

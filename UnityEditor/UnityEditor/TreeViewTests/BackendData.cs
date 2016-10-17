// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewTests.BackendData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.TreeViewTests
{
  internal class BackendData
  {
    public bool m_RecursiveFindParentsBelow = true;
    private int m_MaxItems = 10000;
    private const int k_MinChildren = 3;
    private const int k_MaxChildren = 15;
    private const float k_ProbOfLastDescendent = 0.5f;
    private const int k_MaxDepth = 12;
    private BackendData.Foo m_Root;

    public BackendData.Foo root
    {
      get
      {
        return this.m_Root;
      }
    }

    public int IDCounter { get; private set; }

    public void GenerateData(int maxNumItems)
    {
      this.m_MaxItems = maxNumItems;
      this.IDCounter = 1;
      this.m_Root = new BackendData.Foo("Root", 0, 0);
      while (this.IDCounter < this.m_MaxItems)
        this.AddChildrenRecursive(this.m_Root, Random.Range(3, 15), true);
    }

    public HashSet<int> GetParentsBelow(int id)
    {
      BackendData.Foo nodeRecursive = BackendData.FindNodeRecursive(this.root, id);
      if (nodeRecursive == null)
        return new HashSet<int>();
      if (this.m_RecursiveFindParentsBelow)
        return this.GetParentsBelowRecursive(nodeRecursive);
      return this.GetParentsBelowStackBased(nodeRecursive);
    }

    private HashSet<int> GetParentsBelowStackBased(BackendData.Foo searchFromThis)
    {
      Stack<BackendData.Foo> fooStack = new Stack<BackendData.Foo>();
      fooStack.Push(searchFromThis);
      HashSet<int> intSet = new HashSet<int>();
      while (fooStack.Count > 0)
      {
        BackendData.Foo foo = fooStack.Pop();
        if (foo.hasChildren)
        {
          intSet.Add(foo.id);
          using (List<BackendData.Foo>.Enumerator enumerator = foo.children.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              BackendData.Foo current = enumerator.Current;
              fooStack.Push(current);
            }
          }
        }
      }
      return intSet;
    }

    private HashSet<int> GetParentsBelowRecursive(BackendData.Foo searchFromThis)
    {
      HashSet<int> parentIDs = new HashSet<int>();
      BackendData.GetParentsBelowRecursive(searchFromThis, parentIDs);
      return parentIDs;
    }

    private static void GetParentsBelowRecursive(BackendData.Foo item, HashSet<int> parentIDs)
    {
      if (!item.hasChildren)
        return;
      parentIDs.Add(item.id);
      using (List<BackendData.Foo>.Enumerator enumerator = item.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
          BackendData.GetParentsBelowRecursive(enumerator.Current, parentIDs);
      }
    }

    public void ReparentSelection(BackendData.Foo parentItem, BackendData.Foo insertAfterItem, List<BackendData.Foo> draggedItems)
    {
      using (List<BackendData.Foo>.Enumerator enumerator = draggedItems.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          BackendData.Foo current = enumerator.Current;
          current.parent.children.Remove(current);
          current.parent = parentItem;
        }
      }
      if (!parentItem.hasChildren)
        parentItem.children = new List<BackendData.Foo>();
      List<BackendData.Foo> fooList = new List<BackendData.Foo>((IEnumerable<BackendData.Foo>) parentItem.children);
      int index = 0;
      if (parentItem == insertAfterItem)
      {
        index = 0;
      }
      else
      {
        int num = parentItem.children.IndexOf(insertAfterItem);
        if (num >= 0)
          index = num + 1;
        else
          Debug.LogError((object) "Did not find insertAfterItem, should be a child of parentItem!!");
      }
      fooList.InsertRange(index, (IEnumerable<BackendData.Foo>) draggedItems);
      parentItem.children = fooList;
    }

    private void AddChildrenRecursive(BackendData.Foo foo, int numChildren, bool force)
    {
      if (this.IDCounter > this.m_MaxItems || foo.depth >= 12 || !force && (double) Random.value < 0.5)
        return;
      if (foo.children == null)
        foo.children = new List<BackendData.Foo>(numChildren);
      for (int index = 0; index < numChildren; ++index)
        foo.children.Add(new BackendData.Foo("Tud" + (object) this.IDCounter, foo.depth + 1, ++this.IDCounter)
        {
          parent = foo
        });
      if (this.IDCounter > this.m_MaxItems)
        return;
      using (List<BackendData.Foo>.Enumerator enumerator = foo.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.AddChildrenRecursive(enumerator.Current, Random.Range(3, 15), false);
      }
    }

    public static BackendData.Foo FindNodeRecursive(BackendData.Foo item, int id)
    {
      if (item == null)
        return (BackendData.Foo) null;
      if (item.id == id)
        return item;
      if (item.children == null)
        return (BackendData.Foo) null;
      using (List<BackendData.Foo>.Enumerator enumerator = item.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          BackendData.Foo nodeRecursive = BackendData.FindNodeRecursive(enumerator.Current, id);
          if (nodeRecursive != null)
            return nodeRecursive;
        }
      }
      return (BackendData.Foo) null;
    }

    public class Foo
    {
      public string name { get; set; }

      public int id { get; set; }

      public int depth { get; set; }

      public BackendData.Foo parent { get; set; }

      public List<BackendData.Foo> children { get; set; }

      public bool hasChildren
      {
        get
        {
          if (this.children != null)
            return this.children.Count > 0;
          return false;
        }
      }

      public Foo(string name, int depth, int id)
      {
        this.name = name;
        this.depth = depth;
        this.id = id;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryElement
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;

namespace UnityEditor
{
  [Serializable]
  internal class MemoryElement
  {
    public List<MemoryElement> children;
    public MemoryElement parent;
    public ObjectInfo memoryInfo;
    public int totalMemory;
    public int totalChildCount;
    public string name;
    public bool expanded;
    public string description;

    public MemoryElement()
    {
      this.children = new List<MemoryElement>();
    }

    public MemoryElement(string n)
    {
      this.expanded = false;
      this.name = n;
      this.children = new List<MemoryElement>();
      this.description = string.Empty;
    }

    public MemoryElement(ObjectInfo memInfo, bool finalize)
    {
      this.expanded = false;
      this.memoryInfo = memInfo;
      this.name = this.memoryInfo.name;
      this.totalMemory = memInfo == null ? 0 : memInfo.memorySize;
      this.totalChildCount = 1;
      if (!finalize)
        return;
      this.children = new List<MemoryElement>();
    }

    public MemoryElement(string n, List<MemoryElement> groups)
    {
      this.name = n;
      this.expanded = false;
      this.description = string.Empty;
      this.totalMemory = 0;
      this.totalChildCount = 0;
      this.children = new List<MemoryElement>();
      using (List<MemoryElement>.Enumerator enumerator = groups.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.AddChild(enumerator.Current);
      }
    }

    public void ExpandChildren()
    {
      if (this.children != null)
        return;
      this.children = new List<MemoryElement>();
      for (int index = 0; index < this.ReferenceCount(); ++index)
        this.AddChild(new MemoryElement(this.memoryInfo.referencedBy[index], false));
    }

    public int AccumulatedChildCount()
    {
      return this.totalChildCount;
    }

    public int ChildCount()
    {
      if (this.children != null)
        return this.children.Count;
      return this.ReferenceCount();
    }

    public int ReferenceCount()
    {
      if (this.memoryInfo != null && this.memoryInfo.referencedBy != null)
        return this.memoryInfo.referencedBy.Count;
      return 0;
    }

    public void AddChild(MemoryElement node)
    {
      if (node == this)
        throw new Exception("Should not AddChild to itself");
      this.children.Add(node);
      node.parent = this;
      this.totalMemory += node.totalMemory;
      this.totalChildCount += node.totalChildCount;
    }

    public int GetChildIndexInList()
    {
      for (int index = 0; index < this.parent.children.Count; ++index)
      {
        if (this.parent.children[index] == this)
          return index;
      }
      return this.parent.children.Count;
    }

    public MemoryElement GetPrevNode()
    {
      int index = this.GetChildIndexInList() - 1;
      if (index < 0)
        return this.parent;
      MemoryElement child = this.parent.children[index];
      while (child.expanded)
        child = child.children[child.children.Count - 1];
      return child;
    }

    public MemoryElement GetNextNode()
    {
      if (this.expanded && this.children.Count > 0)
        return this.children[0];
      int index1 = this.GetChildIndexInList() + 1;
      if (index1 < this.parent.children.Count)
        return this.parent.children[index1];
      for (MemoryElement parent = this.parent; parent.parent != null; parent = parent.parent)
      {
        int index2 = parent.GetChildIndexInList() + 1;
        if (index2 < parent.parent.children.Count)
          return parent.parent.children[index2];
      }
      return (MemoryElement) null;
    }

    public MemoryElement GetRoot()
    {
      if (this.parent != null)
        return this.parent.GetRoot();
      return this;
    }

    public MemoryElement FirstChild()
    {
      return this.children[0];
    }

    public MemoryElement LastChild()
    {
      if (!this.expanded)
        return this;
      return this.children[this.children.Count - 1].LastChild();
    }
  }
}

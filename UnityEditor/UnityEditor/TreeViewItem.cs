// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class TreeViewItem : IComparable<TreeViewItem>
  {
    private int m_ID;
    private TreeViewItem m_Parent;
    private List<TreeViewItem> m_Children;
    private int m_Depth;
    private string m_DisplayName;
    private Texture2D m_Icon;
    private object m_UserData;

    public virtual int id
    {
      get
      {
        return this.m_ID;
      }
      set
      {
        this.m_ID = value;
      }
    }

    public virtual string displayName
    {
      get
      {
        return this.m_DisplayName;
      }
      set
      {
        this.m_DisplayName = value;
      }
    }

    public virtual int depth
    {
      get
      {
        return this.m_Depth;
      }
      set
      {
        this.m_Depth = value;
      }
    }

    public virtual bool hasChildren
    {
      get
      {
        if (this.m_Children != null)
          return this.m_Children.Count > 0;
        return false;
      }
    }

    public virtual List<TreeViewItem> children
    {
      get
      {
        return this.m_Children;
      }
      set
      {
        this.m_Children = value;
      }
    }

    public virtual TreeViewItem parent
    {
      get
      {
        return this.m_Parent;
      }
      set
      {
        this.m_Parent = value;
      }
    }

    public virtual Texture2D icon
    {
      get
      {
        return this.m_Icon;
      }
      set
      {
        this.m_Icon = value;
      }
    }

    public virtual object userData
    {
      get
      {
        return this.m_UserData;
      }
      set
      {
        this.m_UserData = value;
      }
    }

    public TreeViewItem(int id, int depth, TreeViewItem parent, string displayName)
    {
      this.m_Depth = depth;
      this.m_Parent = parent;
      this.m_ID = id;
      this.m_DisplayName = displayName;
    }

    public void AddChild(TreeViewItem child)
    {
      if (this.m_Children == null)
        this.m_Children = new List<TreeViewItem>();
      this.m_Children.Add(child);
      if (child == null)
        return;
      child.parent = this;
    }

    public virtual int CompareTo(TreeViewItem other)
    {
      return this.displayName.CompareTo(other.displayName);
    }

    public override string ToString()
    {
      return string.Format("Item: '{0}' ({1}), has {2} children, depth {3}, parent id {4}", (object) this.displayName, (object) this.id, (object) (!this.hasChildren ? 0 : this.children.Count), (object) this.depth, (object) (this.parent == null ? -1 : this.parent.id));
    }
  }
}

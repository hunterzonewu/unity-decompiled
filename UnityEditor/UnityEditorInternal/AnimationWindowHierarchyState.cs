// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchyState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;

namespace UnityEditorInternal
{
  [Serializable]
  internal class AnimationWindowHierarchyState : TreeViewState
  {
    private List<int> m_TallInstanceIDs = new List<int>();

    public bool GetTallMode(AnimationWindowHierarchyNode node)
    {
      return this.m_TallInstanceIDs.Contains(node.id);
    }

    public void SetTallMode(AnimationWindowHierarchyNode node, bool tallMode)
    {
      if (tallMode)
        this.m_TallInstanceIDs.Add(node.id);
      else
        this.m_TallInstanceIDs.Remove(node.id);
    }

    public int GetTallInstancesCount()
    {
      return this.m_TallInstanceIDs.Count;
    }

    public void AddTallInstance(int id)
    {
      if (this.m_TallInstanceIDs.Contains(id))
        return;
      this.m_TallInstanceIDs.Add(id);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.BaseHierarchySort
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>The base class used to create new sorting.</para>
  /// </summary>
  public abstract class BaseHierarchySort : IComparer<GameObject>
  {
    /// <summary>
    ///   <para>The content to display to quickly identify the hierarchy's mode.</para>
    /// </summary>
    public virtual GUIContent content
    {
      get
      {
        return (GUIContent) null;
      }
    }

    /// <summary>
    ///   <para>The sorting method used to determine the order of GameObjects.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    public virtual int Compare(GameObject lhs, GameObject rhs)
    {
      return 0;
    }
  }
}

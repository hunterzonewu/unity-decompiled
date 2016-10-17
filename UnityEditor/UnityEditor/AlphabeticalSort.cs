// Decompiled with JetBrains decompiler
// Type: UnityEditor.AlphabeticalSort
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Hierarchy sort method to allow for items and their children to be sorted alphabetically.</para>
  /// </summary>
  public class AlphabeticalSort : BaseHierarchySort
  {
    private readonly GUIContent m_Content = new GUIContent((Texture) EditorGUIUtility.FindTexture("AlphabeticalSorting"), "Alphabetical Order");

    /// <summary>
    ///   <para>Content to visualize the alphabetical sorting method.</para>
    /// </summary>
    public override GUIContent content
    {
      get
      {
        return this.m_Content;
      }
    }
  }
}

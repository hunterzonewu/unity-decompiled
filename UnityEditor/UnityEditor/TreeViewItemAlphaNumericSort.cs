// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewItemAlphaNumericSort
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor
{
  internal class TreeViewItemAlphaNumericSort : IComparer<TreeViewItem>
  {
    public int Compare(TreeViewItem lhs, TreeViewItem rhs)
    {
      if (lhs == rhs)
        return 0;
      if (lhs == null)
        return -1;
      if (rhs == null)
        return 1;
      return EditorUtility.NaturalCompare(lhs.displayName, rhs.displayName);
    }
  }
}

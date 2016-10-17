// Decompiled with JetBrains decompiler
// Type: UnityEditor.ChangesetItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEditor
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class ChangesetItem
  {
    public string fullPath;
    public string guid;
    public string assetOperations;
    public int assetIsDir;
    public ChangeFlags changeFlags;

    internal static int Compare(ChangesetItem p1, ChangesetItem p2)
    {
      return string.Compare(p1.fullPath, p2.fullPath, true);
    }
  }
}

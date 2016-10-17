// Decompiled with JetBrains decompiler
// Type: UnityEditor.DeletedAsset
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEditor
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class DeletedAsset
  {
    public int changeset;
    public string guid;
    public string parent;
    public string name;
    public string fullPath;
    public string date;
    public int assetIsDir;

    internal static int Compare(DeletedAsset p1, DeletedAsset p2)
    {
      if (p1.changeset > p2.changeset)
        return -1;
      if (p1.changeset < p2.changeset)
        return 1;
      return string.Compare(p1.fullPath, p2.fullPath, true);
    }
  }
}

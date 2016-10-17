// Decompiled with JetBrains decompiler
// Type: UnityEditor.ImportPackageItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEditor
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class ImportPackageItem
  {
    public string exportedAssetPath;
    public string destinationAssetPath;
    public string sourceFolder;
    public string previewPath;
    public string guid;
    public int enabledStatus;
    public bool isFolder;
    public bool exists;
    public bool assetChanged;
    public bool pathConflict;
    public bool projectAsset;
  }
}

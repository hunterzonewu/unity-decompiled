// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetsItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEditor
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class AssetsItem
  {
    public string guid;
    public string pathName;
    public string message;
    public string exportedAssetPath;
    public string guidFolder;
    public int enabled;
    public int assetIsDir;
    public int changeFlags;
    public string previewPath;
    public int exists;
  }
}

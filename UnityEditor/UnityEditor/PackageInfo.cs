// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  public struct PackageInfo
  {
    public string packagePath;
    public string jsonInfo;
    public string iconURL;

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern PackageInfo[] GetPackageList();
  }
}

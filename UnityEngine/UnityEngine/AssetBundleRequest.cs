// Decompiled with JetBrains decompiler
// Type: UnityEngine.AssetBundleRequest
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Asynchronous load request from an AssetBundle.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class AssetBundleRequest : AsyncOperation
  {
    /// <summary>
    ///   <para>Asset object being loaded (Read Only).</para>
    /// </summary>
    public Object asset { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Asset objects with sub assets being loaded. (Read Only)</para>
    /// </summary>
    public Object[] allAssets { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}

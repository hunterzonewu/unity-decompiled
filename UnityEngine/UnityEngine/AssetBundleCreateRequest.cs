// Decompiled with JetBrains decompiler
// Type: UnityEngine.AssetBundleCreateRequest
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Asynchronous create request for an AssetBundle.</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class AssetBundleCreateRequest : AsyncOperation
  {
    /// <summary>
    ///   <para>Asset object being loaded (Read Only).</para>
    /// </summary>
    public AssetBundle assetBundle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void DisableCompatibilityChecks();
  }
}

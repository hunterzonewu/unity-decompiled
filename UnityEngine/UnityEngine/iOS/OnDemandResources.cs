// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.OnDemandResources
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine.iOS
{
  /// <summary>
  ///   <para>On Demand Resources API.</para>
  /// </summary>
  public static class OnDemandResources
  {
    /// <summary>
    ///   <para>Indicates whether player was built with "Use On Demand Resources" player setting enabled.</para>
    /// </summary>
    public static extern bool enabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Creates an On Demand Resources (ODR) request.</para>
    /// </summary>
    /// <param name="tags">Tags for On Demand Resources that should be included in the request.</param>
    /// <returns>
    ///   <para>Object representing ODR request.</para>
    /// </returns>
    public static OnDemandResourcesRequest PreloadAsync(string[] tags)
    {
      return OnDemandResources.PreloadAsyncInternal(tags);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern OnDemandResourcesRequest PreloadAsyncInternal(string[] tags);
  }
}

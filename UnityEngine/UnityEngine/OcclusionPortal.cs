// Decompiled with JetBrains decompiler
// Type: UnityEngine.OcclusionPortal
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The portal for dynamically changing occlusion at runtime.</para>
  /// </summary>
  public sealed class OcclusionPortal : Component
  {
    /// <summary>
    ///   <para>Gets / sets the portal's open state.</para>
    /// </summary>
    public bool open { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}

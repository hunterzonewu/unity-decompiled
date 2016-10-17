// Decompiled with JetBrains decompiler
// Type: UnityEngine.DistanceJoint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Joint that keeps two Rigidbody2D objects a fixed distance apart.</para>
  /// </summary>
  public sealed class DistanceJoint2D : AnchoredJoint2D
  {
    /// <summary>
    ///   <para>Should the distance be calculated automatically?</para>
    /// </summary>
    public bool autoConfigureDistance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The distance separating the two ends of the joint.</para>
    /// </summary>
    public float distance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Whether to maintain a maximum distance only or not.  If not then the absolute distance will be maintained instead.</para>
    /// </summary>
    public bool maxDistanceOnly { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}

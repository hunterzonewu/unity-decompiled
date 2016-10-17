// Decompiled with JetBrains decompiler
// Type: UnityEngine.SpringJoint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Joint that attempts to keep two Rigidbody2D objects a set distance apart by applying a force between them.</para>
  /// </summary>
  public sealed class SpringJoint2D : AnchoredJoint2D
  {
    /// <summary>
    ///   <para>Should the distance be calculated automatically?</para>
    /// </summary>
    public bool autoConfigureDistance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The distance the spring will try to keep between the two objects.</para>
    /// </summary>
    public float distance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The amount by which the spring force is reduced in proportion to the movement speed.</para>
    /// </summary>
    public float dampingRatio { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The frequency at which the spring oscillates around the distance distance between the objects.</para>
    /// </summary>
    public float frequency { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}

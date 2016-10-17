// Decompiled with JetBrains decompiler
// Type: UnityEngine.RelativeJoint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Keeps two Rigidbody2D at their relative orientations.</para>
  /// </summary>
  public sealed class RelativeJoint2D : Joint2D
  {
    /// <summary>
    ///   <para>The maximum force that can be generated when trying to maintain the relative joint constraint.</para>
    /// </summary>
    public float maxForce { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum torque that can be generated when trying to maintain the relative joint constraint.</para>
    /// </summary>
    public float maxTorque { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Scales both the linear and angular forces used to correct the required relative orientation.</para>
    /// </summary>
    public float correctionScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should both the linearOffset and angularOffset be calculated automatically?</para>
    /// </summary>
    public bool autoConfigureOffset { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The current linear offset between the Rigidbody2D that the joint connects.</para>
    /// </summary>
    public Vector2 linearOffset
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_linearOffset(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_linearOffset(ref value);
      }
    }

    /// <summary>
    ///   <para>The current angular offset between the Rigidbody2D that the joint connects.</para>
    /// </summary>
    public float angularOffset { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The world-space position that is currently trying to be maintained.</para>
    /// </summary>
    public Vector2 target
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_target(out vector2);
        return vector2;
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_linearOffset(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_linearOffset(ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_target(out Vector2 value);
  }
}

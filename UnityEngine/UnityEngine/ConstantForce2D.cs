// Decompiled with JetBrains decompiler
// Type: UnityEngine.ConstantForce2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Applies both linear and angular (torque) forces continuously to the rigidbody each physics update.</para>
  /// </summary>
  public sealed class ConstantForce2D : PhysicsUpdateBehaviour2D
  {
    /// <summary>
    ///   <para>The linear force applied to the rigidbody each physics update.</para>
    /// </summary>
    public Vector2 force
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_force(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_force(ref value);
      }
    }

    /// <summary>
    ///   <para>The linear force, relative to the rigid-body coordinate system, applied each physics update.</para>
    /// </summary>
    public Vector2 relativeForce
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_relativeForce(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_relativeForce(ref value);
      }
    }

    /// <summary>
    ///   <para>The torque applied to the rigidbody each physics update.</para>
    /// </summary>
    public float torque { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_force(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_force(ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_relativeForce(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_relativeForce(ref Vector2 value);
  }
}

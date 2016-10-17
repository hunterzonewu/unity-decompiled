// Decompiled with JetBrains decompiler
// Type: UnityEngine.BoxCollider
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A box-shaped primitive collider.</para>
  /// </summary>
  public sealed class BoxCollider : Collider
  {
    /// <summary>
    ///   <para>The center of the box, measured in the object's local space.</para>
    /// </summary>
    public Vector3 center
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_center(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_center(ref value);
      }
    }

    /// <summary>
    ///   <para>The size of the box, measured in the object's local space.</para>
    /// </summary>
    public Vector3 size
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_size(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_size(ref value);
      }
    }

    [Obsolete("use BoxCollider.size instead.")]
    public Vector3 extents
    {
      get
      {
        return this.size * 0.5f;
      }
      set
      {
        this.size = value * 2f;
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_center(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_center(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_size(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_size(ref Vector3 value);
  }
}

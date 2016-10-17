// Decompiled with JetBrains decompiler
// Type: UnityEngine.OcclusionArea
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>OcclusionArea is an area in which occlusion culling is performed.</para>
  /// </summary>
  public sealed class OcclusionArea : Component
  {
    /// <summary>
    ///   <para>Center of the occlusion area relative to the transform.</para>
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
    ///   <para>Size that the occlusion area will have.</para>
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

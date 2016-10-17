// Decompiled with JetBrains decompiler
// Type: UnityEngine.SkinnedMeshRenderer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The Skinned Mesh filter.</para>
  /// </summary>
  public class SkinnedMeshRenderer : Renderer
  {
    /// <summary>
    ///   <para>The bones used to skin the mesh.</para>
    /// </summary>
    public Transform[] bones { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public Transform rootBone { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum number of bones affecting a single vertex.</para>
    /// </summary>
    public SkinQuality quality { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The mesh used for skinning.</para>
    /// </summary>
    public Mesh sharedMesh { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If enabled, the Skinned Mesh will be updated when offscreen. If disabled, this also disables updating animations.</para>
    /// </summary>
    public bool updateWhenOffscreen { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>AABB of this Skinned Mesh in its local space.</para>
    /// </summary>
    public Bounds localBounds
    {
      get
      {
        Bounds bounds;
        this.INTERNAL_get_localBounds(out bounds);
        return bounds;
      }
      set
      {
        this.INTERNAL_set_localBounds(ref value);
      }
    }

    internal Transform actualRootBone { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_localBounds(out Bounds value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_localBounds(ref Bounds value);

    /// <summary>
    ///   <para>Creates a snapshot of SkinnedMeshRenderer and stores it in mesh.</para>
    /// </summary>
    /// <param name="mesh">A static mesh that will receive the snapshot of the skinned mesh.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void BakeMesh(Mesh mesh);

    /// <summary>
    ///   <para>Returns weight of BlendShape on this renderer.</para>
    /// </summary>
    /// <param name="index"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float GetBlendShapeWeight(int index);

    /// <summary>
    ///   <para>Sets weight of BlendShape on this renderer.</para>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetBlendShapeWeight(int index, float value);
  }
}

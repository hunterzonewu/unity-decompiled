// Decompiled with JetBrains decompiler
// Type: UnityEngine.MeshFilter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A class to access the Mesh of the.</para>
  /// </summary>
  public sealed class MeshFilter : Component
  {
    /// <summary>
    ///   <para>Returns the instantiated Mesh assigned to the mesh filter.</para>
    /// </summary>
    public Mesh mesh { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the shared mesh of the mesh filter.</para>
    /// </summary>
    public Mesh sharedMesh { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}

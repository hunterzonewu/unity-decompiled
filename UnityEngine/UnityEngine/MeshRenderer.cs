// Decompiled with JetBrains decompiler
// Type: UnityEngine.MeshRenderer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Renders meshes inserted by the MeshFilter or TextMesh.</para>
  /// </summary>
  public sealed class MeshRenderer : Renderer
  {
    /// <summary>
    ///   <para>Vertex attributes in this mesh will override or add attributes of the primary mesh in the MeshRenderer.</para>
    /// </summary>
    public Mesh additionalVertexStreams { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}

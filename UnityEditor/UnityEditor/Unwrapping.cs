// Decompiled with JetBrains decompiler
// Type: UnityEditor.Unwrapping
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>This class holds everything you may need in regard to uv-unwrapping.</para>
  /// </summary>
  public sealed class Unwrapping
  {
    /// <summary>
    ///   <para>Will generate per-triangle uv (3 uv pairs for each triangle) with default settings.</para>
    /// </summary>
    /// <param name="src"></param>
    public static Vector2[] GeneratePerTriangleUV(Mesh src)
    {
      UnwrapParam settings = new UnwrapParam();
      UnwrapParam.SetDefaults(out settings);
      return Unwrapping.GeneratePerTriangleUV(src, settings);
    }

    /// <summary>
    ///   <para>Will generate per-triangle uv (3 uv pairs for each triangle) with provided settings.</para>
    /// </summary>
    /// <param name="src"></param>
    /// <param name="settings"></param>
    public static Vector2[] GeneratePerTriangleUV(Mesh src, UnwrapParam settings)
    {
      return Unwrapping.GeneratePerTriangleUVImpl(src, settings);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Vector2[] GeneratePerTriangleUVImpl(Mesh src, UnwrapParam settings);

    /// <summary>
    ///   <para>Will auto generate uv2 with default settings for provided mesh, and fill them in.</para>
    /// </summary>
    /// <param name="src"></param>
    public static void GenerateSecondaryUVSet(Mesh src)
    {
      MeshUtility.SetPerTriangleUV2(src, Unwrapping.GeneratePerTriangleUV(src));
    }

    /// <summary>
    ///   <para>Will auto generate uv2 with provided settings for provided mesh, and fill them in.</para>
    /// </summary>
    /// <param name="src"></param>
    /// <param name="settings"></param>
    public static void GenerateSecondaryUVSet(Mesh src, UnwrapParam settings)
    {
      MeshUtility.SetPerTriangleUV2(src, Unwrapping.GeneratePerTriangleUV(src, settings));
    }
  }
}

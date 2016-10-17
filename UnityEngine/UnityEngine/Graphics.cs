// Decompiled with JetBrains decompiler
// Type: UnityEngine.Graphics
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Rendering;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Raw interface to Unity's drawing functions.</para>
  /// </summary>
  public sealed class Graphics
  {
    /// <summary>
    ///   <para>Currently active color buffer (Read Only).</para>
    /// </summary>
    public static RenderBuffer activeColorBuffer
    {
      get
      {
        RenderBuffer res;
        Graphics.GetActiveColorBuffer(out res);
        return res;
      }
    }

    /// <summary>
    ///   <para>Currently active depth/stencil buffer (Read Only).</para>
    /// </summary>
    public static RenderBuffer activeDepthBuffer
    {
      get
      {
        RenderBuffer res;
        Graphics.GetActiveDepthBuffer(out res);
        return res;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property deviceName has been deprecated. Use SystemInfo.graphicsDeviceName instead (UnityUpgradable) -> UnityEngine.SystemInfo.graphicsDeviceName", true)]
    public static string deviceName
    {
      get
      {
        return SystemInfo.graphicsDeviceName;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property deviceVendor has been deprecated. Use SystemInfo.graphicsDeviceVendor instead (UnityUpgradable) -> UnityEngine.SystemInfo.graphicsDeviceVendor", true)]
    public static string deviceVendor
    {
      get
      {
        return SystemInfo.graphicsDeviceVendor;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property deviceVersion has been deprecated. Use SystemInfo.graphicsDeviceVersion instead (UnityUpgradable) -> UnityEngine.SystemInfo.graphicsDeviceVersion", true)]
    public static string deviceVersion
    {
      get
      {
        return SystemInfo.graphicsDeviceVersion;
      }
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows)
    {
      bool receiveShadows = true;
      Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties)
    {
      bool receiveShadows = true;
      bool castShadows = true;
      Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex)
    {
      bool receiveShadows = true;
      bool castShadows = true;
      MaterialPropertyBlock properties = (MaterialPropertyBlock) null;
      Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera)
    {
      bool receiveShadows = true;
      bool castShadows = true;
      MaterialPropertyBlock properties = (MaterialPropertyBlock) null;
      int submeshIndex = 0;
      Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer)
    {
      bool receiveShadows = true;
      bool castShadows = true;
      MaterialPropertyBlock properties = (MaterialPropertyBlock) null;
      int submeshIndex = 0;
      Camera camera = (Camera) null;
      Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
    }

    /// <summary>
    ///   <para>Draw a mesh.</para>
    /// </summary>
    /// <param name="mesh">The Mesh to draw.</param>
    /// <param name="position">Position of the mesh.</param>
    /// <param name="rotation">Rotation of the mesh.</param>
    /// <param name="matrix">Transformation matrix of the mesh (combines position, rotation and other transformations).</param>
    /// <param name="material">Material to use.</param>
    /// <param name="layer"> to use.</param>
    /// <param name="camera">If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.</param>
    /// <param name="submeshIndex">Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.</param>
    /// <param name="properties">Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.</param>
    /// <param name="castShadows">Should the mesh cast shadows?</param>
    /// <param name="receiveShadows">Should the mesh receive shadows?</param>
    /// <param name="materialIndex"></param>
    /// <param name="probeAnchor"></param>
    public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, [DefaultValue("null")] Camera camera, [DefaultValue("0")] int submeshIndex, [DefaultValue("null")] MaterialPropertyBlock properties, [DefaultValue("true")] bool castShadows, [DefaultValue("true")] bool receiveShadows)
    {
      Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, !castShadows ? ShadowCastingMode.Off : ShadowCastingMode.On, receiveShadows);
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
    {
      Transform probeAnchor = (Transform) null;
      Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor);
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
    {
      Transform probeAnchor = (Transform) null;
      bool receiveShadows = true;
      Graphics.DrawMesh(mesh, position, rotation, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor);
    }

    /// <summary>
    ///   <para>Draw a mesh.</para>
    /// </summary>
    /// <param name="mesh">The Mesh to draw.</param>
    /// <param name="position">Position of the mesh.</param>
    /// <param name="rotation">Rotation of the mesh.</param>
    /// <param name="matrix">Transformation matrix of the mesh (combines position, rotation and other transformations).</param>
    /// <param name="material">Material to use.</param>
    /// <param name="layer"> to use.</param>
    /// <param name="camera">If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.</param>
    /// <param name="submeshIndex">Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.</param>
    /// <param name="properties">Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.</param>
    /// <param name="castShadows">Should the mesh cast shadows?</param>
    /// <param name="receiveShadows">Should the mesh receive shadows?</param>
    /// <param name="materialIndex"></param>
    /// <param name="probeAnchor"></param>
    public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, [DefaultValue("true")] bool receiveShadows, [DefaultValue("null")] Transform probeAnchor)
    {
      Graphics.Internal_DrawMeshTR(ref new Internal_DrawMeshTRArguments()
      {
        position = position,
        rotation = rotation,
        layer = layer,
        submeshIndex = submeshIndex,
        castShadows = (int) castShadows,
        receiveShadows = !receiveShadows ? 0 : 1,
        reflectionProbeAnchorInstanceID = !((Object) probeAnchor != (Object) null) ? 0 : probeAnchor.GetInstanceID()
      }, properties, material, mesh, camera);
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows)
    {
      bool receiveShadows = true;
      Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties)
    {
      bool receiveShadows = true;
      bool castShadows = true;
      Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex)
    {
      bool receiveShadows = true;
      bool castShadows = true;
      MaterialPropertyBlock properties = (MaterialPropertyBlock) null;
      Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera)
    {
      bool receiveShadows = true;
      bool castShadows = true;
      MaterialPropertyBlock properties = (MaterialPropertyBlock) null;
      int submeshIndex = 0;
      Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer)
    {
      bool receiveShadows = true;
      bool castShadows = true;
      MaterialPropertyBlock properties = (MaterialPropertyBlock) null;
      int submeshIndex = 0;
      Camera camera = (Camera) null;
      Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows);
    }

    /// <summary>
    ///   <para>Draw a mesh.</para>
    /// </summary>
    /// <param name="mesh">The Mesh to draw.</param>
    /// <param name="position">Position of the mesh.</param>
    /// <param name="rotation">Rotation of the mesh.</param>
    /// <param name="matrix">Transformation matrix of the mesh (combines position, rotation and other transformations).</param>
    /// <param name="material">Material to use.</param>
    /// <param name="layer"> to use.</param>
    /// <param name="camera">If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.</param>
    /// <param name="submeshIndex">Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.</param>
    /// <param name="properties">Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.</param>
    /// <param name="castShadows">Should the mesh cast shadows?</param>
    /// <param name="receiveShadows">Should the mesh receive shadows?</param>
    /// <param name="materialIndex"></param>
    /// <param name="probeAnchor"></param>
    public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, [DefaultValue("null")] Camera camera, [DefaultValue("0")] int submeshIndex, [DefaultValue("null")] MaterialPropertyBlock properties, [DefaultValue("true")] bool castShadows, [DefaultValue("true")] bool receiveShadows)
    {
      Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, !castShadows ? ShadowCastingMode.Off : ShadowCastingMode.On, receiveShadows);
    }

    /// <summary>
    ///   <para>Draw a mesh.</para>
    /// </summary>
    /// <param name="mesh">The Mesh to draw.</param>
    /// <param name="position">Position of the mesh.</param>
    /// <param name="rotation">Rotation of the mesh.</param>
    /// <param name="matrix">Transformation matrix of the mesh (combines position, rotation and other transformations).</param>
    /// <param name="material">Material to use.</param>
    /// <param name="layer"> to use.</param>
    /// <param name="camera">If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.</param>
    /// <param name="submeshIndex">Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.</param>
    /// <param name="properties">Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.</param>
    /// <param name="castShadows">Should the mesh cast shadows?</param>
    /// <param name="receiveShadows">Should the mesh receive shadows?</param>
    /// <param name="materialIndex"></param>
    /// <param name="probeAnchor"></param>
    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
    {
      Transform probeAnchor = (Transform) null;
      Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor);
    }

    [ExcludeFromDocs]
    public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
    {
      Transform probeAnchor = (Transform) null;
      bool receiveShadows = true;
      Graphics.DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor);
    }

    public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, [DefaultValue("true")] bool receiveShadows, [DefaultValue("null")] Transform probeAnchor)
    {
      Graphics.Internal_DrawMeshMatrix(ref new Internal_DrawMeshMatrixArguments()
      {
        matrix = matrix,
        layer = layer,
        submeshIndex = submeshIndex,
        castShadows = (int) castShadows,
        receiveShadows = !receiveShadows ? 0 : 1,
        reflectionProbeAnchorInstanceID = !((Object) probeAnchor != (Object) null) ? 0 : probeAnchor.GetInstanceID()
      }, properties, material, mesh, camera);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_DrawMeshTR(ref Internal_DrawMeshTRArguments arguments, MaterialPropertyBlock properties, Material material, Mesh mesh, Camera camera);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_DrawMeshMatrix(ref Internal_DrawMeshMatrixArguments arguments, MaterialPropertyBlock properties, Material material, Mesh mesh, Camera camera);

    /// <summary>
    ///   <para>Draw a mesh immediately.</para>
    /// </summary>
    /// <param name="mesh">The Mesh to draw.</param>
    /// <param name="position">Position of the mesh.</param>
    /// <param name="rotation">Rotation of the mesh.</param>
    /// <param name="matrix">Transformation matrix of the mesh (combines position, rotation and other transformations). Note that the mesh will not be displayed correctly if matrix has negative scale.</param>
    /// <param name="materialIndex">Subset of the mesh to draw.</param>
    public static void DrawMeshNow(Mesh mesh, Vector3 position, Quaternion rotation)
    {
      Graphics.Internal_DrawMeshNow1(mesh, position, rotation, -1);
    }

    /// <summary>
    ///   <para>Draw a mesh immediately.</para>
    /// </summary>
    /// <param name="mesh">The Mesh to draw.</param>
    /// <param name="position">Position of the mesh.</param>
    /// <param name="rotation">Rotation of the mesh.</param>
    /// <param name="matrix">Transformation matrix of the mesh (combines position, rotation and other transformations). Note that the mesh will not be displayed correctly if matrix has negative scale.</param>
    /// <param name="materialIndex">Subset of the mesh to draw.</param>
    public static void DrawMeshNow(Mesh mesh, Vector3 position, Quaternion rotation, int materialIndex)
    {
      Graphics.Internal_DrawMeshNow1(mesh, position, rotation, materialIndex);
    }

    /// <summary>
    ///   <para>Draw a mesh immediately.</para>
    /// </summary>
    /// <param name="mesh">The Mesh to draw.</param>
    /// <param name="position">Position of the mesh.</param>
    /// <param name="rotation">Rotation of the mesh.</param>
    /// <param name="matrix">Transformation matrix of the mesh (combines position, rotation and other transformations). Note that the mesh will not be displayed correctly if matrix has negative scale.</param>
    /// <param name="materialIndex">Subset of the mesh to draw.</param>
    public static void DrawMeshNow(Mesh mesh, Matrix4x4 matrix)
    {
      Graphics.Internal_DrawMeshNow2(mesh, matrix, -1);
    }

    /// <summary>
    ///   <para>Draw a mesh immediately.</para>
    /// </summary>
    /// <param name="mesh">The Mesh to draw.</param>
    /// <param name="position">Position of the mesh.</param>
    /// <param name="rotation">Rotation of the mesh.</param>
    /// <param name="matrix">Transformation matrix of the mesh (combines position, rotation and other transformations). Note that the mesh will not be displayed correctly if matrix has negative scale.</param>
    /// <param name="materialIndex">Subset of the mesh to draw.</param>
    public static void DrawMeshNow(Mesh mesh, Matrix4x4 matrix, int materialIndex)
    {
      Graphics.Internal_DrawMeshNow2(mesh, matrix, materialIndex);
    }

    private static void Internal_DrawMeshNow1(Mesh mesh, Vector3 position, Quaternion rotation, int materialIndex)
    {
      Graphics.INTERNAL_CALL_Internal_DrawMeshNow1(mesh, ref position, ref rotation, materialIndex);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_DrawMeshNow1(Mesh mesh, ref Vector3 position, ref Quaternion rotation, int materialIndex);

    private static void Internal_DrawMeshNow2(Mesh mesh, Matrix4x4 matrix, int materialIndex)
    {
      Graphics.INTERNAL_CALL_Internal_DrawMeshNow2(mesh, ref matrix, materialIndex);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_DrawMeshNow2(Mesh mesh, ref Matrix4x4 matrix, int materialIndex);

    /// <summary>
    ///   <para>Draws a fully procedural geometry on the GPU.</para>
    /// </summary>
    /// <param name="topology"></param>
    /// <param name="vertexCount"></param>
    /// <param name="instanceCount"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DrawProcedural(MeshTopology topology, int vertexCount, [DefaultValue("1")] int instanceCount);

    [ExcludeFromDocs]
    public static void DrawProcedural(MeshTopology topology, int vertexCount)
    {
      int instanceCount = 1;
      Graphics.DrawProcedural(topology, vertexCount, instanceCount);
    }

    /// <summary>
    ///   <para>Draws a fully procedural geometry on the GPU.</para>
    /// </summary>
    /// <param name="topology">Topology of the procedural geometry.</param>
    /// <param name="bufferWithArgs">Buffer with draw arguments.</param>
    /// <param name="argsOffset">Offset where in the buffer the draw arguments are.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DrawProceduralIndirect(MeshTopology topology, ComputeBuffer bufferWithArgs, [DefaultValue("0")] int argsOffset);

    [ExcludeFromDocs]
    public static void DrawProceduralIndirect(MeshTopology topology, ComputeBuffer bufferWithArgs)
    {
      int argsOffset = 0;
      Graphics.DrawProceduralIndirect(topology, bufferWithArgs, argsOffset);
    }

    [ExcludeFromDocs]
    public static void DrawTexture(Rect screenRect, Texture texture)
    {
      Material mat = (Material) null;
      Graphics.DrawTexture(screenRect, texture, mat);
    }

    /// <summary>
    ///   <para>Draw a texture in screen coordinates.</para>
    /// </summary>
    /// <param name="screenRect">Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.</param>
    /// <param name="texture">Texture to draw.</param>
    /// <param name="sourceRect">Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.</param>
    /// <param name="leftBorder">Number of pixels from the left that are not affected by scale.</param>
    /// <param name="rightBorder">Number of pixels from the right that are not affected by scale.</param>
    /// <param name="topBorder">Number of pixels from the top that are not affected by scale.</param>
    /// <param name="bottomBorder">Number of pixels from the bottom that are not affected by scale.</param>
    /// <param name="color">Color that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.</param>
    /// <param name="mat">Custom Material that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.</param>
    public static void DrawTexture(Rect screenRect, Texture texture, [DefaultValue("null")] Material mat)
    {
      Graphics.DrawTexture(screenRect, texture, 0, 0, 0, 0, mat);
    }

    [ExcludeFromDocs]
    public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
    {
      Material mat = (Material) null;
      Graphics.DrawTexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat);
    }

    /// <summary>
    ///   <para>Draw a texture in screen coordinates.</para>
    /// </summary>
    /// <param name="screenRect">Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.</param>
    /// <param name="texture">Texture to draw.</param>
    /// <param name="sourceRect">Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.</param>
    /// <param name="leftBorder">Number of pixels from the left that are not affected by scale.</param>
    /// <param name="rightBorder">Number of pixels from the right that are not affected by scale.</param>
    /// <param name="topBorder">Number of pixels from the top that are not affected by scale.</param>
    /// <param name="bottomBorder">Number of pixels from the bottom that are not affected by scale.</param>
    /// <param name="color">Color that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.</param>
    /// <param name="mat">Custom Material that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.</param>
    public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [DefaultValue("null")] Material mat)
    {
      Graphics.DrawTexture(screenRect, texture, new Rect(0.0f, 0.0f, 1f, 1f), leftBorder, rightBorder, topBorder, bottomBorder, mat);
    }

    [ExcludeFromDocs]
    public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
    {
      Material mat = (Material) null;
      Graphics.DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, mat);
    }

    /// <summary>
    ///   <para>Draw a texture in screen coordinates.</para>
    /// </summary>
    /// <param name="screenRect">Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.</param>
    /// <param name="texture">Texture to draw.</param>
    /// <param name="sourceRect">Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.</param>
    /// <param name="leftBorder">Number of pixels from the left that are not affected by scale.</param>
    /// <param name="rightBorder">Number of pixels from the right that are not affected by scale.</param>
    /// <param name="topBorder">Number of pixels from the top that are not affected by scale.</param>
    /// <param name="bottomBorder">Number of pixels from the bottom that are not affected by scale.</param>
    /// <param name="color">Color that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.</param>
    /// <param name="mat">Custom Material that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.</param>
    public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [DefaultValue("null")] Material mat)
    {
      InternalDrawTextureArguments arguments = new InternalDrawTextureArguments();
      arguments.screenRect = screenRect;
      arguments.texture = texture;
      arguments.sourceRect = sourceRect;
      arguments.leftBorder = leftBorder;
      arguments.rightBorder = rightBorder;
      arguments.topBorder = topBorder;
      arguments.bottomBorder = bottomBorder;
      Color32 color32 = new Color32();
      color32.r = color32.g = color32.b = color32.a = (byte) 128;
      arguments.color = color32;
      arguments.mat = mat;
      Graphics.DrawTexture(ref arguments);
    }

    [ExcludeFromDocs]
    public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color)
    {
      Material mat = (Material) null;
      Graphics.DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, mat);
    }

    /// <summary>
    ///   <para>Draw a texture in screen coordinates.</para>
    /// </summary>
    /// <param name="screenRect">Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.</param>
    /// <param name="texture">Texture to draw.</param>
    /// <param name="sourceRect">Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.</param>
    /// <param name="leftBorder">Number of pixels from the left that are not affected by scale.</param>
    /// <param name="rightBorder">Number of pixels from the right that are not affected by scale.</param>
    /// <param name="topBorder">Number of pixels from the top that are not affected by scale.</param>
    /// <param name="bottomBorder">Number of pixels from the bottom that are not affected by scale.</param>
    /// <param name="color">Color that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.</param>
    /// <param name="mat">Custom Material that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.</param>
    public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color, [DefaultValue("null")] Material mat)
    {
      Graphics.DrawTexture(ref new InternalDrawTextureArguments()
      {
        screenRect = screenRect,
        texture = texture,
        sourceRect = sourceRect,
        leftBorder = leftBorder,
        rightBorder = rightBorder,
        topBorder = topBorder,
        bottomBorder = bottomBorder,
        color = (Color32) color,
        mat = mat
      });
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void DrawTexture(ref InternalDrawTextureArguments arguments);

    /// <summary>
    ///   <para>Execute a command buffer.</para>
    /// </summary>
    /// <param name="buffer">The buffer to execute.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ExecuteCommandBuffer(CommandBuffer buffer);

    /// <summary>
    ///   <para>Copies source texture into destination render texture with a shader.</para>
    /// </summary>
    /// <param name="source">Source texture.</param>
    /// <param name="dest">Destination RenderTexture, or null to blit directly to screen.</param>
    /// <param name="mat">Material to use. Material's shader could do some post-processing effect, for example.</param>
    /// <param name="pass">If -1 (default), draws all passes in the material. Otherwise, draws given pass only.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Blit(Texture source, RenderTexture dest);

    [ExcludeFromDocs]
    public static void Blit(Texture source, RenderTexture dest, Material mat)
    {
      int pass = -1;
      Graphics.Blit(source, dest, mat, pass);
    }

    /// <summary>
    ///   <para>Copies source texture into destination render texture with a shader.</para>
    /// </summary>
    /// <param name="source">Source texture.</param>
    /// <param name="dest">Destination RenderTexture, or null to blit directly to screen.</param>
    /// <param name="mat">Material to use. Material's shader could do some post-processing effect, for example.</param>
    /// <param name="pass">If -1 (default), draws all passes in the material. Otherwise, draws given pass only.</param>
    public static void Blit(Texture source, RenderTexture dest, Material mat, [DefaultValue("-1")] int pass)
    {
      Graphics.Internal_BlitMaterial(source, dest, mat, pass, true);
    }

    [ExcludeFromDocs]
    public static void Blit(Texture source, Material mat)
    {
      int pass = -1;
      Graphics.Blit(source, mat, pass);
    }

    /// <summary>
    ///   <para>Copies source texture into destination render texture with a shader.</para>
    /// </summary>
    /// <param name="source">Source texture.</param>
    /// <param name="dest">Destination RenderTexture, or null to blit directly to screen.</param>
    /// <param name="mat">Material to use. Material's shader could do some post-processing effect, for example.</param>
    /// <param name="pass">If -1 (default), draws all passes in the material. Otherwise, draws given pass only.</param>
    public static void Blit(Texture source, Material mat, [DefaultValue("-1")] int pass)
    {
      Graphics.Internal_BlitMaterial(source, (RenderTexture) null, mat, pass, false);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_BlitMaterial(Texture source, RenderTexture dest, Material mat, int pass, bool setRT);

    /// <summary>
    ///   <para>Copies source texture into destination, for multi-tap shader.</para>
    /// </summary>
    /// <param name="source">Source texture.</param>
    /// <param name="dest">Destination RenderTexture, or null to blit directly to screen.</param>
    /// <param name="mat">Material to use for copying. Material's shader should do some post-processing effect.</param>
    /// <param name="offsets">Variable number of filtering offsets. Offsets are given in pixels.</param>
    public static void BlitMultiTap(Texture source, RenderTexture dest, Material mat, params Vector2[] offsets)
    {
      Graphics.Internal_BlitMultiTap(source, dest, mat, offsets);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_BlitMultiTap(Texture source, RenderTexture dest, Material mat, Vector2[] offsets);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetNullRT();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetRTSimple(out RenderBuffer color, out RenderBuffer depth, int mip, CubemapFace face);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetMRTFullSetup(RenderBuffer[] colorSA, out RenderBuffer depth, int mip, CubemapFace face, RenderBufferLoadAction[] colorLoadSA, RenderBufferStoreAction[] colorStoreSA, RenderBufferLoadAction depthLoad, RenderBufferStoreAction depthStore);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetMRTSimple(RenderBuffer[] colorSA, out RenderBuffer depth, int mip, CubemapFace face);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetActiveColorBuffer(out RenderBuffer res);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetActiveDepthBuffer(out RenderBuffer res);

    /// <summary>
    ///   <para>Set random write target for Shader Model 5.0 level pixel shaders.</para>
    /// </summary>
    /// <param name="index">Index of the random write target in the shader.</param>
    /// <param name="uav">RenderTexture/ComputeBuffer to set as write target.</param>
    public static void SetRandomWriteTarget(int index, RenderTexture uav)
    {
      Graphics.Internal_SetRandomWriteTargetRT(index, uav);
    }

    /// <summary>
    ///   <para>Set random write target for Shader Model 5.0 level pixel shaders.</para>
    /// </summary>
    /// <param name="index">Index of the random write target in the shader.</param>
    /// <param name="uav">RenderTexture/ComputeBuffer to set as write target.</param>
    public static void SetRandomWriteTarget(int index, ComputeBuffer uav)
    {
      Graphics.Internal_SetRandomWriteTargetBuffer(index, uav);
    }

    /// <summary>
    ///   <para>Clear random write targets for Shader Model 5.0 level pixel shaders.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearRandomWriteTargets();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetRandomWriteTargetRT(int index, RenderTexture uav);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetRandomWriteTargetBuffer(int index, ComputeBuffer uav);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetupVertexLights(Light[] lights);

    internal static void CheckLoadActionValid(RenderBufferLoadAction load, string bufferType)
    {
      if (load != RenderBufferLoadAction.Load && load != RenderBufferLoadAction.DontCare)
        throw new ArgumentException(UnityString.Format("Bad {0} LoadAction provided.", (object) bufferType));
    }

    internal static void CheckStoreActionValid(RenderBufferStoreAction store, string bufferType)
    {
      if (store != RenderBufferStoreAction.Store && store != RenderBufferStoreAction.DontCare)
        throw new ArgumentException(UnityString.Format("Bad {0} StoreAction provided.", (object) bufferType));
    }

    internal static void SetRenderTargetImpl(RenderTargetSetup setup)
    {
      if (setup.color.Length == 0)
        throw new ArgumentException("Invalid color buffer count for SetRenderTarget");
      if (setup.color.Length != setup.colorLoad.Length)
        throw new ArgumentException("Color LoadAction and Buffer arrays have different sizes");
      if (setup.color.Length != setup.colorStore.Length)
        throw new ArgumentException("Color StoreAction and Buffer arrays have different sizes");
      foreach (RenderBufferLoadAction load in setup.colorLoad)
        Graphics.CheckLoadActionValid(load, "Color");
      foreach (RenderBufferStoreAction store in setup.colorStore)
        Graphics.CheckStoreActionValid(store, "Color");
      Graphics.CheckLoadActionValid(setup.depthLoad, "Depth");
      Graphics.CheckStoreActionValid(setup.depthStore, "Depth");
      if (setup.cubemapFace < CubemapFace.Unknown || setup.cubemapFace > CubemapFace.NegativeZ)
        throw new ArgumentException("Bad CubemapFace provided");
      Graphics.Internal_SetMRTFullSetup(setup.color, out setup.depth, setup.mipLevel, setup.cubemapFace, setup.colorLoad, setup.colorStore, setup.depthLoad, setup.depthStore);
    }

    internal static void SetRenderTargetImpl(RenderBuffer colorBuffer, RenderBuffer depthBuffer, int mipLevel, CubemapFace face)
    {
      RenderBuffer color = colorBuffer;
      RenderBuffer depth = depthBuffer;
      Graphics.Internal_SetRTSimple(out color, out depth, mipLevel, face);
    }

    internal static void SetRenderTargetImpl(RenderTexture rt, int mipLevel, CubemapFace face)
    {
      if ((bool) ((Object) rt))
        Graphics.SetRenderTargetImpl(rt.colorBuffer, rt.depthBuffer, mipLevel, face);
      else
        Graphics.Internal_SetNullRT();
    }

    internal static void SetRenderTargetImpl(RenderBuffer[] colorBuffers, RenderBuffer depthBuffer, int mipLevel, CubemapFace face)
    {
      RenderBuffer depth = depthBuffer;
      Graphics.Internal_SetMRTSimple(colorBuffers, out depth, mipLevel, face);
    }

    /// <summary>
    ///   <para>Sets current render target.</para>
    /// </summary>
    /// <param name="setup"></param>
    /// <param name="rt"></param>
    /// <param name="mipLevel"></param>
    /// <param name="face"></param>
    /// <param name="colorBuffer"></param>
    /// <param name="depthBuffer"></param>
    /// <param name="colorBuffers"></param>
    public static void SetRenderTarget(RenderTexture rt)
    {
      Graphics.SetRenderTargetImpl(rt, 0, CubemapFace.Unknown);
    }

    /// <summary>
    ///   <para>Sets current render target.</para>
    /// </summary>
    /// <param name="setup"></param>
    /// <param name="rt"></param>
    /// <param name="mipLevel"></param>
    /// <param name="face"></param>
    /// <param name="colorBuffer"></param>
    /// <param name="depthBuffer"></param>
    /// <param name="colorBuffers"></param>
    public static void SetRenderTarget(RenderTexture rt, int mipLevel)
    {
      Graphics.SetRenderTargetImpl(rt, mipLevel, CubemapFace.Unknown);
    }

    /// <summary>
    ///   <para>Sets current render target.</para>
    /// </summary>
    /// <param name="setup"></param>
    /// <param name="rt"></param>
    /// <param name="mipLevel"></param>
    /// <param name="face"></param>
    /// <param name="colorBuffer"></param>
    /// <param name="depthBuffer"></param>
    /// <param name="colorBuffers"></param>
    public static void SetRenderTarget(RenderTexture rt, int mipLevel, CubemapFace face)
    {
      Graphics.SetRenderTargetImpl(rt, mipLevel, face);
    }

    /// <summary>
    ///   <para>Sets current render target.</para>
    /// </summary>
    /// <param name="setup"></param>
    /// <param name="rt"></param>
    /// <param name="mipLevel"></param>
    /// <param name="face"></param>
    /// <param name="colorBuffer"></param>
    /// <param name="depthBuffer"></param>
    /// <param name="colorBuffers"></param>
    public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer)
    {
      Graphics.SetRenderTargetImpl(colorBuffer, depthBuffer, 0, CubemapFace.Unknown);
    }

    /// <summary>
    ///   <para>Sets current render target.</para>
    /// </summary>
    /// <param name="setup"></param>
    /// <param name="rt"></param>
    /// <param name="mipLevel"></param>
    /// <param name="face"></param>
    /// <param name="colorBuffer"></param>
    /// <param name="depthBuffer"></param>
    /// <param name="colorBuffers"></param>
    public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer, int mipLevel)
    {
      Graphics.SetRenderTargetImpl(colorBuffer, depthBuffer, mipLevel, CubemapFace.Unknown);
    }

    /// <summary>
    ///   <para>Sets current render target.</para>
    /// </summary>
    /// <param name="setup"></param>
    /// <param name="rt"></param>
    /// <param name="mipLevel"></param>
    /// <param name="face"></param>
    /// <param name="colorBuffer"></param>
    /// <param name="depthBuffer"></param>
    /// <param name="colorBuffers"></param>
    public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer, int mipLevel, CubemapFace face)
    {
      Graphics.SetRenderTargetImpl(colorBuffer, depthBuffer, mipLevel, face);
    }

    /// <summary>
    ///   <para>Sets current render target.</para>
    /// </summary>
    /// <param name="setup"></param>
    /// <param name="rt"></param>
    /// <param name="mipLevel"></param>
    /// <param name="face"></param>
    /// <param name="colorBuffer"></param>
    /// <param name="depthBuffer"></param>
    /// <param name="colorBuffers"></param>
    public static void SetRenderTarget(RenderBuffer[] colorBuffers, RenderBuffer depthBuffer)
    {
      Graphics.SetRenderTargetImpl(colorBuffers, depthBuffer, 0, CubemapFace.Unknown);
    }

    /// <summary>
    ///   <para>Sets current render target.</para>
    /// </summary>
    /// <param name="setup"></param>
    /// <param name="rt"></param>
    /// <param name="mipLevel"></param>
    /// <param name="face"></param>
    /// <param name="colorBuffer"></param>
    /// <param name="depthBuffer"></param>
    /// <param name="colorBuffers"></param>
    public static void SetRenderTarget(RenderTargetSetup setup)
    {
      Graphics.SetRenderTargetImpl(setup);
    }

    /// <summary>
    ///   <para>Draw a mesh.</para>
    /// </summary>
    /// <param name="mesh">The Mesh to draw.</param>
    /// <param name="position">Position of the mesh.</param>
    /// <param name="rotation">Rotation of the mesh.</param>
    /// <param name="matrix">Transformation matrix of the mesh (combines position, rotation and other transformations).</param>
    /// <param name="material">Material to use.</param>
    /// <param name="layer"> to use.</param>
    /// <param name="camera">If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.</param>
    /// <param name="submeshIndex">Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.</param>
    /// <param name="properties">Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.</param>
    /// <param name="castShadows">Should the mesh cast shadows?</param>
    /// <param name="receiveShadows">Should the mesh receive shadows?</param>
    /// <param name="materialIndex"></param>
    /// <param name="probeAnchor"></param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Method DrawMesh has been deprecated. Use Graphics.DrawMeshNow instead (UnityUpgradable) -> DrawMeshNow(*)", true)]
    public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation)
    {
    }

    /// <summary>
    ///   <para>Draw a mesh.</para>
    /// </summary>
    /// <param name="mesh">The Mesh to draw.</param>
    /// <param name="position">Position of the mesh.</param>
    /// <param name="rotation">Rotation of the mesh.</param>
    /// <param name="matrix">Transformation matrix of the mesh (combines position, rotation and other transformations).</param>
    /// <param name="material">Material to use.</param>
    /// <param name="layer"> to use.</param>
    /// <param name="camera">If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.</param>
    /// <param name="submeshIndex">Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.</param>
    /// <param name="properties">Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.</param>
    /// <param name="castShadows">Should the mesh cast shadows?</param>
    /// <param name="receiveShadows">Should the mesh receive shadows?</param>
    /// <param name="materialIndex"></param>
    /// <param name="probeAnchor"></param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Method DrawMesh has been deprecated. Use Graphics.DrawMeshNow instead (UnityUpgradable) -> DrawMeshNow(*)", true)]
    public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, int materialIndex)
    {
    }

    /// <summary>
    ///   <para>Draw a mesh.</para>
    /// </summary>
    /// <param name="mesh">The Mesh to draw.</param>
    /// <param name="position">Position of the mesh.</param>
    /// <param name="rotation">Rotation of the mesh.</param>
    /// <param name="matrix">Transformation matrix of the mesh (combines position, rotation and other transformations).</param>
    /// <param name="material">Material to use.</param>
    /// <param name="layer"> to use.</param>
    /// <param name="camera">If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.</param>
    /// <param name="submeshIndex">Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.</param>
    /// <param name="properties">Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.</param>
    /// <param name="castShadows">Should the mesh cast shadows?</param>
    /// <param name="receiveShadows">Should the mesh receive shadows?</param>
    /// <param name="materialIndex"></param>
    /// <param name="probeAnchor"></param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Method DrawMesh has been deprecated. Use Graphics.DrawMeshNow instead (UnityUpgradable) -> DrawMeshNow(*)", true)]
    public static void DrawMesh(Mesh mesh, Matrix4x4 matrix)
    {
    }

    /// <summary>
    ///   <para>Draw a mesh.</para>
    /// </summary>
    /// <param name="mesh">The Mesh to draw.</param>
    /// <param name="position">Position of the mesh.</param>
    /// <param name="rotation">Rotation of the mesh.</param>
    /// <param name="matrix">Transformation matrix of the mesh (combines position, rotation and other transformations).</param>
    /// <param name="material">Material to use.</param>
    /// <param name="layer"> to use.</param>
    /// <param name="camera">If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.</param>
    /// <param name="submeshIndex">Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.</param>
    /// <param name="properties">Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.</param>
    /// <param name="castShadows">Should the mesh cast shadows?</param>
    /// <param name="receiveShadows">Should the mesh receive shadows?</param>
    /// <param name="materialIndex"></param>
    /// <param name="probeAnchor"></param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Method DrawMesh has been deprecated. Use Graphics.DrawMeshNow instead (UnityUpgradable) -> DrawMeshNow(*)", true)]
    public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, int materialIndex)
    {
    }
  }
}

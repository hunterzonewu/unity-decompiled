// Decompiled with JetBrains decompiler
// Type: UnityEngine.MeshSubsetCombineUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  internal class MeshSubsetCombineUtility
  {
    public struct MeshInstance
    {
      public int meshInstanceID;
      public int rendererInstanceID;
      public int additionalVertexStreamsMeshInstanceID;
      public Matrix4x4 transform;
      public Vector4 lightmapScaleOffset;
      public Vector4 realtimeLightmapScaleOffset;
    }

    public struct SubMeshInstance
    {
      public int meshInstanceID;
      public int vertexOffset;
      public int gameObjectInstanceID;
      public int subMeshIndex;
      public Matrix4x4 transform;
    }
  }
}

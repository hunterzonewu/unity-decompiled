// Decompiled with JetBrains decompiler
// Type: UnityEditor.TerrainInspectorUtil
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  internal sealed class TerrainInspectorUtil
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float GetTreePlacementSize(TerrainData terrainData, int prototypeIndex, float spacing, float treeCount);

    public static bool CheckTreeDistance(TerrainData terrainData, Vector3 position, int treeIndex, float distanceBias)
    {
      return TerrainInspectorUtil.INTERNAL_CALL_CheckTreeDistance(terrainData, ref position, treeIndex, distanceBias);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_CheckTreeDistance(TerrainData terrainData, ref Vector3 position, int treeIndex, float distanceBias);

    public static Vector3 GetPrototypeExtent(TerrainData terrainData, int prototypeIndex)
    {
      Vector3 vector3;
      TerrainInspectorUtil.INTERNAL_CALL_GetPrototypeExtent(terrainData, prototypeIndex, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetPrototypeExtent(TerrainData terrainData, int prototypeIndex, out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetPrototypeCount(TerrainData terrainData);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool PrototypeIsRenderable(TerrainData terrainData, int prototypeIndex);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RefreshPhysicsInEditMode();
  }
}

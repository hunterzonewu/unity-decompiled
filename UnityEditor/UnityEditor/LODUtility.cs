// Decompiled with JetBrains decompiler
// Type: UnityEditor.LODUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>LOD Utility Helpers.</para>
  /// </summary>
  public sealed class LODUtility
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern LODVisualizationInformation CalculateVisualizationData(Camera camera, LODGroup group, int lodLevel);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern float CalculateDistance(Camera camera, float relativeScreenHeight, LODGroup group);

    /// <summary>
    ///   <para>Recalculate the bounding region for the given LODGroup.</para>
    /// </summary>
    /// <param name="group"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CalculateLODGroupBoundingBox(LODGroup group);
  }
}

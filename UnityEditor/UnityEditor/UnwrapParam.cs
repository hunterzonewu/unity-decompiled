// Decompiled with JetBrains decompiler
// Type: UnityEditor.UnwrapParam
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Unwrapping settings.</para>
  /// </summary>
  public struct UnwrapParam
  {
    /// <summary>
    ///   <para>Maximum allowed angle distortion (0..1).</para>
    /// </summary>
    public float angleError;
    /// <summary>
    ///   <para>Maximum allowed area distortion (0..1).</para>
    /// </summary>
    public float areaError;
    /// <summary>
    ///   <para>This angle (in degrees) or greater between triangles will cause seam to be created.</para>
    /// </summary>
    public float hardAngle;
    /// <summary>
    ///   <para>How much uv-islands will be padded.</para>
    /// </summary>
    public float packMargin;
    internal int recollectVertices;

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetDefaults(out UnwrapParam param);
  }
}

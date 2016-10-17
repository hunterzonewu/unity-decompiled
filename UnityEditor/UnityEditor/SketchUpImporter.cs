// Decompiled with JetBrains decompiler
// Type: UnityEditor.SketchUpImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Derives from AssetImporter to handle importing of SketchUp files.</para>
  /// </summary>
  public sealed class SketchUpImporter : ModelImporter
  {
    /// <summary>
    ///   <para>Retrieves the latitude Geo Coordinate imported from the SketchUp file.</para>
    /// </summary>
    public double latitude { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Retrieves the longitude Geo Coordinate imported from the SketchUp file.</para>
    /// </summary>
    public double longitude { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Retrieves the north correction value imported from the SketchUp file.</para>
    /// </summary>
    public double northCorrection { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The method returns an array of SketchUpImportScene which represents SketchUp scenes.</para>
    /// </summary>
    /// <returns>
    ///   <para>Array of scenes extracted from a SketchUp file.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public SketchUpImportScene[] GetScenes();

    /// <summary>
    ///   <para>The default camera or the camera of the active scene which the SketchUp file was saved with.</para>
    /// </summary>
    /// <returns>
    ///   <para>The default camera.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public SketchUpImportCamera GetDefaultCamera();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal SketchUpNodeInfo[] GetNodes();
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.SketchUpImportCamera
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Structure to hold camera data extracted from a SketchUp file.</para>
  /// </summary>
  [Serializable]
  public struct SketchUpImportCamera
  {
    /// <summary>
    ///   <para>The position of the camera.</para>
    /// </summary>
    public Vector3 position;
    /// <summary>
    ///   <para>The position the camera is looking at.</para>
    /// </summary>
    public Vector3 lookAt;
    /// <summary>
    ///   <para>Up vector of the camera.</para>
    /// </summary>
    public Vector3 up;
    /// <summary>
    ///   <para>Field of view of the camera.</para>
    /// </summary>
    public float fieldOfView;
    /// <summary>
    ///   <para>Aspect ratio of the camera.</para>
    /// </summary>
    public float aspectRatio;
    /// <summary>
    ///   <para>The orthogonal projection size of the camera. This value only make sense if SketchUpImportCamera.isPerspective is false.</para>
    /// </summary>
    public float orthoSize;
    /// <summary>
    ///   <para>Indicate if the camera is using a perspective or orthogonal projection.</para>
    /// </summary>
    public bool isPerspective;
  }
}

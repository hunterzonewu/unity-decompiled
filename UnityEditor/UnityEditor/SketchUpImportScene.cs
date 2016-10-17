// Decompiled with JetBrains decompiler
// Type: UnityEditor.SketchUpImportScene
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Structure to hold scene data extracted from a SketchUp file.</para>
  /// </summary>
  [Serializable]
  public struct SketchUpImportScene
  {
    /// <summary>
    ///   <para>The camera data of the SketchUp scene.</para>
    /// </summary>
    public SketchUpImportCamera camera;
    /// <summary>
    ///   <para>The name of the SketchUp scene.</para>
    /// </summary>
    public string name;
  }
}

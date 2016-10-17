// Decompiled with JetBrains decompiler
// Type: UnityEditor.StaticEditorFlags
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Static Editor Flags.</para>
  /// </summary>
  [System.Flags]
  public enum StaticEditorFlags
  {
    LightmapStatic = 1,
    OccluderStatic = 2,
    OccludeeStatic = 16,
    BatchingStatic = 4,
    NavigationStatic = 8,
    OffMeshLinkGeneration = 32,
    ReflectionProbeStatic = 64,
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.VertexChannelCompressionFlags
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>This enum is used to build a bitmask for controlling per-channel vertex compression.</para>
  /// </summary>
  [System.Flags]
  public enum VertexChannelCompressionFlags
  {
    kPosition = 1,
    kNormal = 2,
    kColor = 4,
    kUV0 = 8,
    kUV1 = 16,
    kUV2 = 32,
    kUV3 = 64,
    kTangent = 128,
  }
}

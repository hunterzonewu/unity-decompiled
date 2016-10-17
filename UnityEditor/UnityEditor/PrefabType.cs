// Decompiled with JetBrains decompiler
// Type: UnityEditor.PrefabType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>The type of a prefab object as returned by EditorUtility.GetPrefabType.</para>
  /// </summary>
  public enum PrefabType
  {
    None,
    Prefab,
    ModelPrefab,
    PrefabInstance,
    ModelPrefabInstance,
    MissingPrefabInstance,
    DisconnectedPrefabInstance,
    DisconnectedModelPrefabInstance,
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.MemoryInfoGCReason
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditorInternal
{
  public enum MemoryInfoGCReason
  {
    SceneObject = 0,
    BuiltinResource = 1,
    MarkedDontSave = 2,
    AssetMarkedDirtyInEditor = 3,
    SceneAssetReferencedByNativeCodeOnly = 5,
    SceneAssetReferenced = 6,
    AssetReferencedByNativeCodeOnly = 8,
    AssetReferenced = 9,
    NotApplicable = 10,
  }
}

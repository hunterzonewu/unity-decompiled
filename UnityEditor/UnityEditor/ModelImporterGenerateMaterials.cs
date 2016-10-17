// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporterGenerateMaterials
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Material generation options for ModelImporter.</para>
  /// </summary>
  [Obsolete("Use ModelImporterMaterialName, ModelImporter.materialName and ModelImporter.importMaterials instead")]
  public enum ModelImporterGenerateMaterials
  {
    [Obsolete("Use ModelImporter.importMaterials=false instead")] None,
    [Obsolete("Use ModelImporter.importMaterials=true and ModelImporter.materialName=ModelImporterMaterialName.BasedOnTextureName instead")] PerTexture,
    [Obsolete("Use ModelImporter.importMaterials=true and ModelImporter.materialName=ModelImporterMaterialName.BasedOnModelNameAndMaterialName instead")] PerSourceMaterial,
  }
}

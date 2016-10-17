// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporterMaterialName
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Material naming options for ModelImporter.</para>
  /// </summary>
  public enum ModelImporterMaterialName
  {
    BasedOnTextureName,
    BasedOnMaterialName,
    BasedOnModelNameAndMaterialName,
    [Obsolete("You should use ModelImporterMaterialName.BasedOnTextureName instead, because it it less complicated and behaves in more consistent way.")] BasedOnTextureName_Or_ModelNameAndMaterialName,
  }
}

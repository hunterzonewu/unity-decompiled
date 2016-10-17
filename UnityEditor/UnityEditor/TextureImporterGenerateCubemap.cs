// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureImporterGenerateCubemap
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Cubemap generation mode for TextureImporter.</para>
  /// </summary>
  public enum TextureImporterGenerateCubemap
  {
    None,
    Spheremap,
    Cylindrical,
    [Obsolete("Obscure shperemap modes are not supported any longer (use TextureImporterGenerateCubemap.Spheremap instead).")] SimpleSpheremap,
    [Obsolete("Obscure shperemap modes are not supported any longer (use TextureImporterGenerateCubemap.Spheremap instead).")] NiceSpheremap,
    FullCubemap,
    AutoCubemap,
  }
}

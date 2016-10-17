// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporterTangentSpaceMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Animation generation options for ModelImporter.</para>
  /// </summary>
  public enum ModelImporterTangentSpaceMode
  {
    [Obsolete("Use ModelImporterNormals.Import instead")] Import,
    [Obsolete("Use ModelImporterNormals.Calculate instead")] Calculate,
    [Obsolete("Use ModelImporterNormals.None instead")] None,
  }
}

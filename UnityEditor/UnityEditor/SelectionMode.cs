// Decompiled with JetBrains decompiler
// Type: UnityEditor.SelectionMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>SelectionMode can be used to tweak the selection returned by Selection.GetTransforms.</para>
  /// </summary>
  public enum SelectionMode
  {
    Unfiltered = 0,
    TopLevel = 1,
    Deep = 2,
    ExcludePrefab = 4,
    Editable = 8,
    OnlyUserModifiable = 8,
    Assets = 16,
    DeepAssets = 32,
  }
}

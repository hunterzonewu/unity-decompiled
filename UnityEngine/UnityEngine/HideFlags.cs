// Decompiled with JetBrains decompiler
// Type: UnityEngine.HideFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Bit mask that controls object destruction, saving and visibility in inspectors.</para>
  /// </summary>
  [Flags]
  public enum HideFlags
  {
    None = 0,
    HideInHierarchy = 1,
    HideInInspector = 2,
    DontSaveInEditor = 4,
    NotEditable = 8,
    DontSaveInBuild = 16,
    DontUnloadUnusedAsset = 32,
    DontSave = DontUnloadUnusedAsset | DontSaveInBuild | DontSaveInEditor,
    HideAndDontSave = DontSave | NotEditable | HideInHierarchy,
  }
}

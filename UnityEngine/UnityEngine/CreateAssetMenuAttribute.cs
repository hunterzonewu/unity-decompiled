// Decompiled with JetBrains decompiler
// Type: UnityEngine.CreateAssetMenuAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Mark a ScriptableObject-derived type to be automatically listed in the Assets/Create submenu, so that instances of the type can be easily created and stored in the project as ".asset" files.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class CreateAssetMenuAttribute : Attribute
  {
    /// <summary>
    ///   <para>The display name for this type shown in the Assets/Create menu.</para>
    /// </summary>
    public string menuName { get; set; }

    /// <summary>
    ///   <para>The default file name used by newly created instances of this type.</para>
    /// </summary>
    public string fileName { get; set; }

    /// <summary>
    ///   <para>The position of the menu item within the Assets/Create menu.</para>
    /// </summary>
    public int order { get; set; }
  }
}

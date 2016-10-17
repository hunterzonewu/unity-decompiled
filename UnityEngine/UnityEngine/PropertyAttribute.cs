// Decompiled with JetBrains decompiler
// Type: UnityEngine.PropertyAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Base class to derive custom property attributes from. Use this to create custom attributes for script variables.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public abstract class PropertyAttribute : Attribute
  {
    /// <summary>
    ///   <para>Optional field to specify the order that multiple DecorationDrawers should be drawn in.</para>
    /// </summary>
    public int order { get; set; }
  }
}

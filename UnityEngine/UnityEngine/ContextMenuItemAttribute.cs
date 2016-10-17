// Decompiled with JetBrains decompiler
// Type: UnityEngine.ContextMenuItemAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Use this attribute to add a context menu to a field that calls a  named method.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
  public class ContextMenuItemAttribute : PropertyAttribute
  {
    /// <summary>
    ///   <para>The name of the context menu item.</para>
    /// </summary>
    public readonly string name;
    /// <summary>
    ///   <para>The name of the function that should be called.</para>
    /// </summary>
    public readonly string function;

    /// <summary>
    ///   <para>Use this attribute to add a context menu to a field that calls a  named method.</para>
    /// </summary>
    /// <param name="name">The name of the context menu item.</param>
    /// <param name="function">The name of the function that should be called.</param>
    public ContextMenuItemAttribute(string name, string function)
    {
      this.name = name;
      this.function = function;
    }
  }
}

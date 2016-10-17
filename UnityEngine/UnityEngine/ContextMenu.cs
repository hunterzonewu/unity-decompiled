// Decompiled with JetBrains decompiler
// Type: UnityEngine.ContextMenu
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The ContextMenu attribute allows you to add commands to the context menu.</para>
  /// </summary>
  public sealed class ContextMenu : Attribute
  {
    private string m_ItemName;

    public string menuItem
    {
      get
      {
        return this.m_ItemName;
      }
    }

    /// <summary>
    ///   <para>Adds the function to the context menu of the component.</para>
    /// </summary>
    /// <param name="name"></param>
    public ContextMenu(string name)
    {
      this.m_ItemName = name;
    }
  }
}

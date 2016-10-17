// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.ConfigField
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>This class describes the.</para>
  /// </summary>
  public sealed class ConfigField
  {
    private IntPtr m_thisDummy;
    private string m_guid;

    /// <summary>
    ///   <para>Name of the configuration field.</para>
    /// </summary>
    public string name { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Label that is displayed next to the configuration field in the editor.</para>
    /// </summary>
    public string label { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Descrition of the configuration field.</para>
    /// </summary>
    public string description { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>This is true if the configuration field is required for the version control plugin to function correctly.</para>
    /// </summary>
    public bool isRequired { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>This is true if the configuration field is a password field.</para>
    /// </summary>
    public bool isPassword { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal ConfigField()
    {
    }

    ~ConfigField()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();
  }
}

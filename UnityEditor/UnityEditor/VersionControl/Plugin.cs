// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.Plugin
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>The plugin class describes a version control plugin and which configuratin options it has.</para>
  /// </summary>
  public sealed class Plugin
  {
    private IntPtr m_thisDummy;
    private string m_guid;

    public static extern Plugin[] availablePlugins { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string name { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Configuration fields of the plugin.</para>
    /// </summary>
    public ConfigField[] configFields { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal Plugin()
    {
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();
  }
}

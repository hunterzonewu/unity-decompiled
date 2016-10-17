// Decompiled with JetBrains decompiler
// Type: UnityEditor.PluginImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Represents plugin importer.</para>
  /// </summary>
  public sealed class PluginImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Is plugin native or managed? Note: C++ libraries with CLR support are treated as native plugins, because Unity cannot load such libraries. You can still access them via P/Invoke.</para>
    /// </summary>
    public bool isNativePlugin { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Set compatiblity with any platform.</para>
    /// </summary>
    /// <param name="enable">Is plugin compatible with any platform.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetCompatibleWithAnyPlatform(bool enable);

    /// <summary>
    ///   <para>Is plugin comptabile with any platform.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool GetCompatibleWithAnyPlatform();

    /// <summary>
    ///   <para>Set compatiblity with any editor.</para>
    /// </summary>
    /// <param name="enable">Is plugin compatible with editor.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetCompatibleWithEditor(bool enable);

    /// <summary>
    ///   <para>Is plugin compatible with editor.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool GetCompatibleWithEditor();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SetIsPreloaded(bool isPreloaded);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool GetIsPreloaded();

    /// <summary>
    ///   <para>Set compatiblity with specified platform.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="enable">Is plugin compatible with specified platform.</param>
    /// <param name="platformName">Target platform.</param>
    public void SetCompatibleWithPlatform(BuildTarget platform, bool enable)
    {
      this.SetCompatibleWithPlatform(BuildPipeline.GetBuildTargetName(platform), enable);
    }

    /// <summary>
    ///   <para>Is plugin compatible with specified platform.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="platformName"></param>
    public bool GetCompatibleWithPlatform(BuildTarget platform)
    {
      return this.GetCompatibleWithPlatform(BuildPipeline.GetBuildTargetName(platform));
    }

    /// <summary>
    ///   <para>Set compatiblity with specified platform.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="enable">Is plugin compatible with specified platform.</param>
    /// <param name="platformName">Target platform.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetCompatibleWithPlatform(string platformName, bool enable);

    /// <summary>
    ///   <para>Is plugin compatible with specified platform.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="platformName"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool GetCompatibleWithPlatform(string platformName);

    /// <summary>
    ///   <para>Set platform specific data.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="key">Key value for data.</param>
    /// <param name="value">Data.</param>
    /// <param name="platformName"></param>
    public void SetPlatformData(BuildTarget platform, string key, string value)
    {
      this.SetPlatformData(BuildPipeline.GetBuildTargetName(platform), key, value);
    }

    /// <summary>
    ///   <para>Get platform specific data.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="key">Key value for data.</param>
    /// <param name="platformName"></param>
    public string GetPlatformData(BuildTarget platform, string key)
    {
      return this.GetPlatformData(BuildPipeline.GetBuildTargetName(platform), key);
    }

    /// <summary>
    ///   <para>Set platform specific data.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="key">Key value for data.</param>
    /// <param name="value">Data.</param>
    /// <param name="platformName"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetPlatformData(string platformName, string key, string value);

    /// <summary>
    ///   <para>Get platform specific data.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="key">Key value for data.</param>
    /// <param name="platformName"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetPlatformData(string platformName, string key);

    /// <summary>
    ///   <para>Set editor specific data.</para>
    /// </summary>
    /// <param name="key">Key value for data.</param>
    /// <param name="value">Data.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetEditorData(string key, string value);

    /// <summary>
    ///   <para>Returns editor specific data for specified key.</para>
    /// </summary>
    /// <param name="key">Key value for data.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetEditorData(string key);

    /// <summary>
    ///   <para>Returns all plugin importers for all platforms.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern PluginImporter[] GetAllImporters();

    /// <summary>
    ///   <para>Returns all plugin importers for specfied platform.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="platformName">Name of the target platform.</param>
    public static PluginImporter[] GetImporters(string platformName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<PluginImporter>) PluginImporter.GetAllImporters()).Where<PluginImporter>(new Func<PluginImporter, bool>(new PluginImporter.\u003CGetImporters\u003Ec__AnonStorey16()
      {
        platformName = platformName
      }.\u003C\u003Em__1C)).ToArray<PluginImporter>();
    }

    /// <summary>
    ///   <para>Returns all plugin importers for specfied platform.</para>
    /// </summary>
    /// <param name="platform">Target platform.</param>
    /// <param name="platformName">Name of the target platform.</param>
    public static PluginImporter[] GetImporters(BuildTarget platform)
    {
      return PluginImporter.GetImporters(BuildPipeline.GetBuildTargetName(platform));
    }

    [DebuggerHidden]
    internal static IEnumerable<PluginDesc> GetExtensionPlugins(BuildTarget target)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PluginImporter.\u003CGetExtensionPlugins\u003Ec__Iterator0 pluginsCIterator0 = new PluginImporter.\u003CGetExtensionPlugins\u003Ec__Iterator0()
      {
        target = target,
        \u003C\u0024\u003Etarget = target
      };
      // ISSUE: reference to a compiler-generated field
      pluginsCIterator0.\u0024PC = -2;
      return (IEnumerable<PluginDesc>) pluginsCIterator0;
    }
  }
}

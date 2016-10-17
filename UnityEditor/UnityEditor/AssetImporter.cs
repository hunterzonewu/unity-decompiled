// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Base class from which asset importers for specific asset types derive.</para>
  /// </summary>
  public class AssetImporter : Object
  {
    /// <summary>
    ///   <para>The path name of the asset for this importer. (Read Only)</para>
    /// </summary>
    public string assetPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public ulong assetTimeStamp { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get or set any user data.</para>
    /// </summary>
    public string userData { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Get or set the AssetBundle name.</para>
    /// </summary>
    public string assetBundleName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Get or set the AssetBundle variant.</para>
    /// </summary>
    public string assetBundleVariant { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set the AssetBundle name and variant.</para>
    /// </summary>
    /// <param name="assetBundleName">AssetBundle name.</param>
    /// <param name="assetBundleVariant">AssetBundle variant.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetAssetBundleNameAndVariant(string assetBundleName, string assetBundleVariant);

    /// <summary>
    ///   <para>Retrieves the asset importer for the asset at path.</para>
    /// </summary>
    /// <param name="path"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AssetImporter GetAtPath(string path);

    /// <summary>
    ///   <para>Save asset importer settings if asset importer is dirty.</para>
    /// </summary>
    public void SaveAndReimport()
    {
      AssetDatabase.ImportAsset(this.assetPath);
    }
  }
}

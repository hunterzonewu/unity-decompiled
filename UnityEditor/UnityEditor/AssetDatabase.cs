// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetDatabase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngineInternal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>An Interface for accessing assets and performing operations on assets.</para>
  /// </summary>
  public sealed class AssetDatabase
  {
    internal static extern bool isLocked { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is object an asset?</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="instanceID"></param>
    public static bool Contains(UnityEngine.Object obj)
    {
      return AssetDatabase.Contains(obj.GetInstanceID());
    }

    /// <summary>
    ///   <para>Is object an asset?</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="instanceID"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Contains(int instanceID);

    /// <summary>
    ///   <para>Create a new folder.</para>
    /// </summary>
    /// <param name="parentFolder">The name of the parent folder.</param>
    /// <param name="newFolderName">The name of the new folder.</param>
    /// <returns>
    ///   <para>The GUID of the newly created folder.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string CreateFolder(string parentFolder, string newFolderName);

    /// <summary>
    ///   <para>Is asset a main asset in the project window?</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="instanceID"></param>
    public static bool IsMainAsset(UnityEngine.Object obj)
    {
      return AssetDatabase.IsMainAsset(obj.GetInstanceID());
    }

    /// <summary>
    ///   <para>Is asset a main asset in the project window?</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="instanceID"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsMainAsset(int instanceID);

    /// <summary>
    ///   <para>Does the asset form part of another asset?</para>
    /// </summary>
    /// <param name="obj">The asset Object to query.</param>
    /// <param name="instanceID">Instance ID of the asset Object to query.</param>
    public static bool IsSubAsset(UnityEngine.Object obj)
    {
      return AssetDatabase.IsSubAsset(obj.GetInstanceID());
    }

    /// <summary>
    ///   <para>Does the asset form part of another asset?</para>
    /// </summary>
    /// <param name="obj">The asset Object to query.</param>
    /// <param name="instanceID">Instance ID of the asset Object to query.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsSubAsset(int instanceID);

    /// <summary>
    ///   <para>Is asset a foreign asset?</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="instanceID"></param>
    public static bool IsForeignAsset(UnityEngine.Object obj)
    {
      return AssetDatabase.IsForeignAsset(obj.GetInstanceID());
    }

    /// <summary>
    ///   <para>Is asset a foreign asset?</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="instanceID"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsForeignAsset(int instanceID);

    /// <summary>
    ///   <para>Is asset a native asset?</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="instanceID"></param>
    public static bool IsNativeAsset(UnityEngine.Object obj)
    {
      return AssetDatabase.IsNativeAsset(obj.GetInstanceID());
    }

    /// <summary>
    ///   <para>Is asset a native asset?</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="instanceID"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsNativeAsset(int instanceID);

    /// <summary>
    ///   <para>Creates a new unique path for an asset.</para>
    /// </summary>
    /// <param name="path"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GenerateUniqueAssetPath(string path);

    /// <summary>
    ///   <para>Begin Asset importing. This lets you group several asset imports together into one larger import.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void StartAssetEditing();

    /// <summary>
    ///   <para>Stop Asset importing. This lets you group several asset imports together into one larger import.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void StopAssetEditing();

    /// <summary>
    ///   <para>Checks if an asset file can be moved from one folder to another. (Without actually moving the file).</para>
    /// </summary>
    /// <param name="oldPath">The path where the asset currently resides.</param>
    /// <param name="newPath">The path which the asset should be moved to.</param>
    /// <returns>
    ///   <para>An empty string if the asset can be moved, otherwise an error message.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ValidateMoveAsset(string oldPath, string newPath);

    /// <summary>
    ///   <para>Move an asset file from one folder to another.</para>
    /// </summary>
    /// <param name="oldPath">The path where the asset currently resides.</param>
    /// <param name="newPath">The path which the asset should be moved to.</param>
    /// <returns>
    ///   <para>An empty string if the asset has been successfully moved, otherwise an error message.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string MoveAsset(string oldPath, string newPath);

    /// <summary>
    ///   <para>Rename an asset file.</para>
    /// </summary>
    /// <param name="pathName">The path where the asset currently resides.</param>
    /// <param name="newName">The new name which should be given to the asset.</param>
    /// <returns>
    ///   <para>An empty string, if the asset has been successfully renamed, otherwise an error message.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string RenameAsset(string pathName, string newName);

    /// <summary>
    ///   <para>Moves the asset at path to the trash.</para>
    /// </summary>
    /// <param name="path"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool MoveAssetToTrash(string path);

    /// <summary>
    ///   <para>Deletes the asset file at path.</para>
    /// </summary>
    /// <param name="path">Filesystem path of the asset to be deleted.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool DeleteAsset(string path);

    /// <summary>
    ///   <para>Import asset at path.</para>
    /// </summary>
    /// <param name="path"></param>
    /// <param name="options"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ImportAsset(string path, [DefaultValue("ImportAssetOptions.Default")] ImportAssetOptions options);

    /// <summary>
    ///   <para>Import asset at path.</para>
    /// </summary>
    /// <param name="path"></param>
    /// <param name="options"></param>
    [ExcludeFromDocs]
    public static void ImportAsset(string path)
    {
      ImportAssetOptions options = ImportAssetOptions.Default;
      AssetDatabase.ImportAsset(path, options);
    }

    /// <summary>
    ///   <para>Duplicates the asset at path and stores it at newPath.</para>
    /// </summary>
    /// <param name="path">Filesystem path of the source asset.</param>
    /// <param name="newPath">Filesystem path of the new asset to create.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool CopyAsset(string path, string newPath);

    /// <summary>
    ///   <para>Writes the import settings to disk.</para>
    /// </summary>
    /// <param name="path"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool WriteImportSettingsIfDirty(string path);

    /// <summary>
    ///   <para>Given an absolute path to a directory, this method will return an array of all it's subdirectories.</para>
    /// </summary>
    /// <param name="path"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetSubFolders(string path);

    /// <summary>
    ///   <para>Given an absolute path to a folder, returns true if it exists, false otherwise.</para>
    /// </summary>
    /// <param name="path"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsValidFolder(string path);

    /// <summary>
    ///   <para>Creates a new asset at path.</para>
    /// </summary>
    /// <param name="asset">Object to use in creating the asset.</param>
    /// <param name="path">Filesystem path for the new asset.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CreateAsset(UnityEngine.Object asset, string path);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void CreateAssetFromObjects(UnityEngine.Object[] assets, string path);

    /// <summary>
    ///   <para>Adds objectToAdd to an existing asset at path.</para>
    /// </summary>
    /// <param name="objectToAdd">Object to add to the existing asset.</param>
    /// <param name="path">Filesystem path to the asset.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void AddObjectToAsset(UnityEngine.Object objectToAdd, string path);

    /// <summary>
    ///   <para>Adds objectToAdd to an existing asset identified by assetObject.</para>
    /// </summary>
    /// <param name="objectToAdd"></param>
    /// <param name="assetObject"></param>
    public static void AddObjectToAsset(UnityEngine.Object objectToAdd, UnityEngine.Object assetObject)
    {
      AssetDatabase.AddObjectToAsset_OBJ_Internal(objectToAdd, assetObject);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void AddInstanceIDToAssetWithRandomFileId(int instanceIDToAdd, UnityEngine.Object assetObject, bool hide);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void AddObjectToAsset_OBJ_Internal(UnityEngine.Object newAsset, UnityEngine.Object sameAssetFile);

    /// <summary>
    ///   <para>Returns the path name relative to the project folder where the asset is stored.</para>
    /// </summary>
    /// <param name="instanceID">The instance ID of the asset.</param>
    /// <param name="assetObject">A reference to the asset.</param>
    /// <returns>
    ///   <para>The asset path name, or null, or an empty string if the asset does not exist.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetAssetPath(UnityEngine.Object assetObject);

    /// <summary>
    ///   <para>Returns the path name relative to the project folder where the asset is stored.</para>
    /// </summary>
    /// <param name="instanceID">The instance ID of the asset.</param>
    /// <param name="assetObject">A reference to the asset.</param>
    /// <returns>
    ///   <para>The asset path name, or null, or an empty string if the asset does not exist.</para>
    /// </returns>
    public static string GetAssetPath(int instanceID)
    {
      return AssetDatabase.GetAssetPathFromInstanceID(instanceID);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetAssetPathFromInstanceID(int instanceID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetMainAssetInstanceID(string assetPath);

    /// <summary>
    ///   <para>Returns the path name relative to the project folder where the asset is stored.</para>
    /// </summary>
    /// <param name="assetObject"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetAssetOrScenePath(UnityEngine.Object assetObject);

    /// <summary>
    ///   <para>Gets the path to the text .meta file associated with an asset.</para>
    /// </summary>
    /// <param name="path">The path to the asset.</param>
    /// <returns>
    ///   <para>The path to the .meta text file or empty string if the file does not exist.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetTextMetaFilePathFromAssetPath(string path);

    /// <summary>
    ///   <para>Gets the path to the asset file associated with a text .meta file.</para>
    /// </summary>
    /// <param name="path"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetAssetPathFromTextMetaFilePath(string path);

    /// <summary>
    ///   <para>Returns the first asset object of type type at given path assetPath.</para>
    /// </summary>
    /// <param name="assetPath">Path of the asset to load.</param>
    /// <param name="type">Data type of the asset.</param>
    /// <returns>
    ///   <para>The asset matching the parameters</para>
    /// </returns>
    [WrapperlessIcall]
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object LoadAssetAtPath(string assetPath, System.Type type);

    public static T LoadAssetAtPath<T>(string assetPath) where T : UnityEngine.Object
    {
      return (T) AssetDatabase.LoadAssetAtPath(assetPath, typeof (T));
    }

    /// <summary>
    ///   <para>Returns the main asset object at assetPath.</para>
    /// </summary>
    /// <param name="assetPath">Filesystem path of the asset to load.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object LoadMainAssetAtPath(string assetPath);

    /// <summary>
    ///   <para>Returns all asset representations at assetPath.</para>
    /// </summary>
    /// <param name="assetPath"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object[] LoadAllAssetRepresentationsAtPath(string assetPath);

    /// <summary>
    ///   <para>Returns an array of all asset objects at assetPath.</para>
    /// </summary>
    /// <param name="assetPath">Filesystem path to the asset.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object[] LoadAllAssetsAtPath(string assetPath);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetAllAssetPaths();

    [Obsolete("Please use AssetDatabase.Refresh instead", true)]
    public static void RefreshDelayed(ImportAssetOptions options)
    {
    }

    [Obsolete("Please use AssetDatabase.Refresh instead", true)]
    public static void RefreshDelayed()
    {
    }

    /// <summary>
    ///   <para>Import any changed assets.</para>
    /// </summary>
    /// <param name="options"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Refresh([DefaultValue("ImportAssetOptions.Default")] ImportAssetOptions options);

    [ExcludeFromDocs]
    public static void Refresh()
    {
      AssetDatabase.Refresh(ImportAssetOptions.Default);
    }

    /// <summary>
    ///   <para>Opens the asset with associated application.</para>
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="lineNumber"></param>
    /// <param name="target"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool OpenAsset(int instanceID, [DefaultValue("-1")] int lineNumber);

    /// <summary>
    ///   <para>Opens the asset with associated application.</para>
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="lineNumber"></param>
    /// <param name="target"></param>
    [ExcludeFromDocs]
    public static bool OpenAsset(int instanceID)
    {
      int lineNumber = -1;
      return AssetDatabase.OpenAsset(instanceID, lineNumber);
    }

    /// <summary>
    ///   <para>Opens the asset with associated application.</para>
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="lineNumber"></param>
    /// <param name="target"></param>
    [ExcludeFromDocs]
    public static bool OpenAsset(UnityEngine.Object target)
    {
      int lineNumber = -1;
      return AssetDatabase.OpenAsset(target, lineNumber);
    }

    /// <summary>
    ///   <para>Opens the asset with associated application.</para>
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="lineNumber"></param>
    /// <param name="target"></param>
    public static bool OpenAsset(UnityEngine.Object target, [DefaultValue("-1")] int lineNumber)
    {
      if ((bool) target)
        return AssetDatabase.OpenAsset(target.GetInstanceID(), lineNumber);
      return false;
    }

    /// <summary>
    ///   <para>Opens the asset(s) with associated application(s).</para>
    /// </summary>
    /// <param name="objects"></param>
    public static bool OpenAsset(UnityEngine.Object[] objects)
    {
      bool flag = true;
      foreach (UnityEngine.Object target in objects)
      {
        if (!AssetDatabase.OpenAsset(target))
          flag = false;
      }
      return flag;
    }

    /// <summary>
    ///   <para>Get the GUID for the asset at path.</para>
    /// </summary>
    /// <param name="path">Filesystem path for the asset.</param>
    /// <returns>
    ///   <para>GUID</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string AssetPathToGUID(string path);

    /// <summary>
    ///   <para>Translate a GUID to its current asset path.</para>
    /// </summary>
    /// <param name="guid"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GUIDToAssetPath(string guid);

    /// <summary>
    ///   <para>Returns the hash of all the dependencies of an asset.</para>
    /// </summary>
    /// <param name="path">Path to the asset.</param>
    /// <returns>
    ///   <para>Aggregate hash.</para>
    /// </returns>
    public static Hash128 GetAssetDependencyHash(string path)
    {
      Hash128 hash128;
      AssetDatabase.INTERNAL_CALL_GetAssetDependencyHash(path, out hash128);
      return hash128;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetAssetDependencyHash(string path, out Hash128 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetInstanceIDFromGUID(string guid);

    /// <summary>
    ///   <para>Writes all unsaved asset changes to disk.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SaveAssets();

    /// <summary>
    ///   <para>Retrieves an icon for the asset at the given asset path.</para>
    /// </summary>
    /// <param name="path"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture GetCachedIcon(string path);

    /// <summary>
    ///   <para>Replaces that list of labels on an asset.</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="labels"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetLabels(UnityEngine.Object obj, string[] labels);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_GetAllLabels(out string[] labels, out float[] scores);

    internal static Dictionary<string, float> GetAllLabels()
    {
      string[] labels;
      float[] scores;
      AssetDatabase.INTERNAL_GetAllLabels(out labels, out scores);
      Dictionary<string, float> dictionary = new Dictionary<string, float>(labels.Length);
      for (int index = 0; index < labels.Length; ++index)
        dictionary[labels[index]] = scores[index];
      return dictionary;
    }

    /// <summary>
    ///   <para>Returns all labels attached to a given asset.</para>
    /// </summary>
    /// <param name="obj"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetLabels(UnityEngine.Object obj);

    /// <summary>
    ///   <para>Removes all labels attached to an asset.</para>
    /// </summary>
    /// <param name="obj"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearLabels(UnityEngine.Object obj);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string[] MatchLabelsPartial(UnityEngine.Object obj, string partial);

    /// <summary>
    ///   <para>Return all the AssetBundle names in the asset database.</para>
    /// </summary>
    /// <returns>
    ///   <para>Array of asset bundle names.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetAllAssetBundleNames();

    [Obsolete("Method GetAssetBundleNames has been deprecated. Use GetAllAssetBundleNames instead.")]
    public string[] GetAssetBundleNames()
    {
      return AssetDatabase.GetAllAssetBundleNames();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string[] GetAllAssetBundleNamesWithoutVariant();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string[] GetAllAssetBundleVariants();

    /// <summary>
    ///   <para>Return all the unused assetBundle names in the asset database.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetUnusedAssetBundleNames();

    /// <summary>
    ///   <para>Remove the assetBundle name from the asset database. The forceRemove flag is used to indicate if you want to remove it even it's in use.</para>
    /// </summary>
    /// <param name="assetBundleName">The assetBundle name you want to remove.</param>
    /// <param name="forceRemove">Flag to indicate if you want to remove the assetBundle name even it's in use.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool RemoveAssetBundleName(string assetBundleName, bool forceRemove);

    /// <summary>
    ///   <para>Remove all the unused assetBundle names in the asset database.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RemoveUnusedAssetBundleNames();

    /// <summary>
    ///   <para>Get the paths of the assets which have been marked with the given assetBundle name.</para>
    /// </summary>
    /// <param name="assetBundleName"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetAssetPathsFromAssetBundle(string assetBundleName);

    /// <summary>
    ///   <para>Get the asset paths from the given assetBundle name and asset name.</para>
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="assetName"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetAssetPathsFromAssetBundleAndAssetName(string assetBundleName, string assetName);

    /// <summary>
    ///   <para>Given a pathName, returns the list of all assets that it depends on.</para>
    /// </summary>
    /// <param name="pathName">The path to the asset for which dependencies are required.</param>
    /// <param name="recursive">If false, return only assets which are direct dependencies of the input; if true, include all indirect dependencies of the input. Defaults to true.</param>
    /// <returns>
    ///   <para>The paths of all assets that the input depends on.</para>
    /// </returns>
    public static string[] GetDependencies(string pathName)
    {
      return AssetDatabase.GetDependencies(pathName, true);
    }

    /// <summary>
    ///   <para>Given a pathName, returns the list of all assets that it depends on.</para>
    /// </summary>
    /// <param name="pathName">The path to the asset for which dependencies are required.</param>
    /// <param name="recursive">If false, return only assets which are direct dependencies of the input; if true, include all indirect dependencies of the input. Defaults to true.</param>
    /// <returns>
    ///   <para>The paths of all assets that the input depends on.</para>
    /// </returns>
    public static string[] GetDependencies(string pathName, bool recursive)
    {
      return AssetDatabase.GetDependencies(new string[1]{ pathName }, recursive);
    }

    /// <summary>
    ///   <para>Given an array of pathNames, returns the list of all assets that the input depend on.</para>
    /// </summary>
    /// <param name="pathNames">The path to the assets for which dependencies are required.</param>
    /// <param name="recursive">If false, return only assets which are direct dependencies of the input; if true, include all indirect dependencies of the input. Defaults to true.</param>
    /// <returns>
    ///   <para>The paths of all assets that the input depends on.</para>
    /// </returns>
    public static string[] GetDependencies(string[] pathNames)
    {
      return AssetDatabase.GetDependencies(pathNames, true);
    }

    /// <summary>
    ///   <para>Given an array of pathNames, returns the list of all assets that the input depend on.</para>
    /// </summary>
    /// <param name="pathNames">The path to the assets for which dependencies are required.</param>
    /// <param name="recursive">If false, return only assets which are direct dependencies of the input; if true, include all indirect dependencies of the input. Defaults to true.</param>
    /// <returns>
    ///   <para>The paths of all assets that the input depends on.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetDependencies(string[] pathNames, bool recursive);

    /// <summary>
    ///   <para>Exports the assets identified by assetPathNames to a unitypackage file in fileName.</para>
    /// </summary>
    /// <param name="assetPathNames"></param>
    /// <param name="fileName"></param>
    /// <param name="flags"></param>
    /// <param name="assetPathName"></param>
    public static void ExportPackage(string assetPathName, string fileName)
    {
      AssetDatabase.ExportPackage(new string[1]{ assetPathName }, fileName, ExportPackageOptions.Default);
    }

    /// <summary>
    ///   <para>Exports the assets identified by assetPathNames to a unitypackage file in fileName.</para>
    /// </summary>
    /// <param name="assetPathNames"></param>
    /// <param name="fileName"></param>
    /// <param name="flags"></param>
    /// <param name="assetPathName"></param>
    public static void ExportPackage(string assetPathName, string fileName, ExportPackageOptions flags)
    {
      AssetDatabase.ExportPackage(new string[1]{ assetPathName }, fileName, flags);
    }

    /// <summary>
    ///   <para>Exports the assets identified by assetPathNames to a unitypackage file in fileName.</para>
    /// </summary>
    /// <param name="assetPathNames"></param>
    /// <param name="fileName"></param>
    /// <param name="flags"></param>
    /// <param name="assetPathName"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ExportPackage(string[] assetPathNames, string fileName, [DefaultValue("ExportPackageOptions.Default")] ExportPackageOptions flags);

    /// <summary>
    ///   <para>Exports the assets identified by assetPathNames to a unitypackage file in fileName.</para>
    /// </summary>
    /// <param name="assetPathNames"></param>
    /// <param name="fileName"></param>
    /// <param name="flags"></param>
    /// <param name="assetPathName"></param>
    [ExcludeFromDocs]
    public static void ExportPackage(string[] assetPathNames, string fileName)
    {
      ExportPackageOptions flags = ExportPackageOptions.Default;
      AssetDatabase.ExportPackage(assetPathNames, fileName, flags);
    }

    /// <summary>
    ///   <para>Imports package at packagePath into the current project.</para>
    /// </summary>
    /// <param name="packagePath"></param>
    /// <param name="interactive"></param>
    public static void ImportPackage(string packagePath, bool interactive)
    {
      string packageIconPath;
      bool canPerformReInstall;
      ImportPackageItem[] prepareAssetList = PackageUtility.ExtractAndPrepareAssetList(packagePath, out packageIconPath, out canPerformReInstall);
      if (prepareAssetList == null)
        return;
      if (interactive)
        PackageImport.ShowImportPackage(packagePath, prepareAssetList, packageIconPath, canPerformReInstall);
      else
        PackageUtility.ImportPackageAssets(prepareAssetList, false);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetUniquePathNameAtSelectedPath(string fileName);

    /// <summary>
    ///   <para>Use IsOpenForEdit to determine if the asset is open for edit by the version control.</para>
    /// </summary>
    /// <param name="assetPath">Is the path to the asset on disk relative to project folder.</param>
    /// <param name="message">Used to give reason for not open.</param>
    /// <param name="assetObject"></param>
    /// <param name="assetOrMetaFilePath"></param>
    /// <returns>
    ///   <para>True is the asset can be edited.</para>
    /// </returns>
    public static bool IsOpenForEdit(UnityEngine.Object assetObject)
    {
      return AssetDatabase.IsOpenForEdit(AssetDatabase.GetAssetOrScenePath(assetObject));
    }

    /// <summary>
    ///   <para>Use IsOpenForEdit to determine if the asset is open for edit by the version control.</para>
    /// </summary>
    /// <param name="assetPath">Is the path to the asset on disk relative to project folder.</param>
    /// <param name="message">Used to give reason for not open.</param>
    /// <param name="assetObject"></param>
    /// <param name="assetOrMetaFilePath"></param>
    /// <returns>
    ///   <para>True is the asset can be edited.</para>
    /// </returns>
    public static bool IsOpenForEdit(string assetOrMetaFilePath)
    {
      string message;
      return AssetDatabase.IsOpenForEdit(assetOrMetaFilePath, out message);
    }

    public static bool IsOpenForEdit(UnityEngine.Object assetObject, out string message)
    {
      return AssetDatabase.IsOpenForEdit(AssetDatabase.GetAssetOrScenePath(assetObject), out message);
    }

    public static bool IsOpenForEdit(string assetOrMetaFilePath, out string message)
    {
      return AssetModificationProcessorInternal.IsOpenForEdit(assetOrMetaFilePath, out message);
    }

    public static bool IsMetaFileOpenForEdit(UnityEngine.Object assetObject)
    {
      string message;
      return AssetDatabase.IsMetaFileOpenForEdit(assetObject, out message);
    }

    public static bool IsMetaFileOpenForEdit(UnityEngine.Object assetObject, out string message)
    {
      return AssetDatabase.IsOpenForEdit(AssetDatabase.GetTextMetaFilePathFromAssetPath(AssetDatabase.GetAssetOrScenePath(assetObject)), out message);
    }

    [WrapperlessIcall]
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object GetBuiltinExtraResource(System.Type type, string path);

    public static T GetBuiltinExtraResource<T>(string path) where T : UnityEngine.Object
    {
      return (T) AssetDatabase.GetBuiltinExtraResource(typeof (T), path);
    }

    /// <summary>
    ///   <para>Gets the path to the text .meta file associated with an asset.</para>
    /// </summary>
    /// <param name="path">The path to the asset.</param>
    /// <returns>
    ///   <para>The path to the .meta text file or empty string if the file does not exist.</para>
    /// </returns>
    [Obsolete("GetTextMetaDataPathFromAssetPath has been renamed to GetTextMetaFilePathFromAssetPath (UnityUpgradable) -> GetTextMetaFilePathFromAssetPath(*)")]
    public static string GetTextMetaDataPathFromAssetPath(string path)
    {
      return (string) null;
    }

    [Obsolete("GetAssetDependencyHashFromPath has been renamed to GetAssetDependencyHash (UnityUpgradable) -> GetAssetDependencyHash(*)")]
    public static Hash128 GetAssetDependencyHashFromPath(string path)
    {
      return new Hash128();
    }

    /// <summary>
    ///   <para>Search the asset database using a search filter string.</para>
    /// </summary>
    /// <param name="filter">The filter string can contain search data for: names, asset labels and types (class names).</param>
    /// <param name="searchInFolders">Specifying one or more folders will limit the searching to these folders and their child folders (and is faster than searching all assets).</param>
    /// <returns>
    ///   <para>Array of matching asset GUIDs.</para>
    /// </returns>
    public static string[] FindAssets(string filter)
    {
      return AssetDatabase.FindAssets(filter, (string[]) null);
    }

    /// <summary>
    ///   <para>Search the asset database using a search filter string.</para>
    /// </summary>
    /// <param name="filter">The filter string can contain search data for: names, asset labels and types (class names).</param>
    /// <param name="searchInFolders">Specifying one or more folders will limit the searching to these folders and their child folders (and is faster than searching all assets).</param>
    /// <returns>
    ///   <para>Array of matching asset GUIDs.</para>
    /// </returns>
    public static string[] FindAssets(string filter, string[] searchInFolders)
    {
      SearchFilter searchFilter = new SearchFilter();
      SearchUtility.ParseSearchString(filter, searchFilter);
      if (searchInFolders != null)
        searchFilter.folders = searchInFolders;
      return AssetDatabase.FindAssets(searchFilter);
    }

    private static string[] FindAssets(SearchFilter searchFilter)
    {
      if (searchFilter.folders != null && searchFilter.folders.Length > 0)
        return AssetDatabase.SearchInFolders(searchFilter);
      return AssetDatabase.SearchAllAssets(searchFilter);
    }

    private static string[] SearchAllAssets(SearchFilter searchFilter)
    {
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      hierarchyProperty.SetSearchFilter(searchFilter);
      hierarchyProperty.Reset();
      List<string> stringList = new List<string>();
      while (hierarchyProperty.Next((int[]) null))
        stringList.Add(hierarchyProperty.guid);
      return stringList.ToArray();
    }

    private static string[] SearchInFolders(SearchFilter searchFilter)
    {
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      List<string> stringList = new List<string>();
      foreach (string folder in searchFilter.folders)
      {
        hierarchyProperty.SetSearchFilter(new SearchFilter());
        int mainAssetInstanceId = AssetDatabase.GetMainAssetInstanceID(folder);
        if (hierarchyProperty.Find(mainAssetInstanceId, (int[]) null))
        {
          hierarchyProperty.SetSearchFilter(searchFilter);
          int depth = hierarchyProperty.depth;
          int[] expanded = (int[]) null;
          while (hierarchyProperty.NextWithDepthCheck(expanded, depth + 1))
            stringList.Add(hierarchyProperty.guid);
        }
        else
          Debug.LogWarning((object) ("AssetDatabase.FindAssets: Folder not found: '" + folder + "'"));
      }
      return stringList.ToArray();
    }
  }
}

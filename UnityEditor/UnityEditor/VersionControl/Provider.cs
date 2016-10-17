// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.Provider
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>This class provides acces to the version control API.</para>
  /// </summary>
  public sealed class Provider
  {
    /// <summary>
    ///   <para>Returns true if the version control provider is enabled and a valid Unity Pro License was found.</para>
    /// </summary>
    public static extern bool enabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if a version control plugin has been selected and configured correctly.</para>
    /// </summary>
    public static extern bool isActive { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>This is true if a network connection is required by the currently selected version control plugin to perform any action.</para>
    /// </summary>
    public static extern bool requiresNetwork { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool hasChangelistSupport { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool hasCheckoutSupport { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool isVersioningFolders { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the OnlineState of the version control provider.</para>
    /// </summary>
    public static extern OnlineState onlineState { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the reason for the version control provider being offline (if it is offline).</para>
    /// </summary>
    public static extern string offlineReason { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Gets the currently executing task.</para>
    /// </summary>
    public static extern Task activeTask { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern Texture2D overlayAtlas { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern CustomCommand[] customCommands { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static Rect GetAtlasRectForState(int state)
    {
      Rect rect;
      Provider.INTERNAL_CALL_GetAtlasRectForState(state, out rect);
      return rect;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetAtlasRectForState(int state, out Rect value);

    /// <summary>
    ///   <para>Gets the currently user selected verson control plugin.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Plugin GetActivePlugin();

    /// <summary>
    ///   <para>Returns the configuration fields for the currently active version control plugin.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ConfigField[] GetActiveConfigFields();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsCustomCommandEnabled(string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Asset Internal_CacheStatus(string assetPath);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_Status(Asset[] assets, bool recursively);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_StatusStrings(string[] assetsProjectPaths, bool recursively);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_StatusAbsolutePath(string assetPath);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_CheckoutIsValid(Asset[] assets, CheckoutMode mode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_Checkout(Asset[] assets, CheckoutMode mode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_CheckoutStrings(string[] assets, CheckoutMode mode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_PromptAndCheckoutIfNeeded(string[] assets, string promptIfCheckoutIsNeeded);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_Delete(Asset[] assets);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_DeleteAtProjectPath(string assetProjectPath);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_MoveAsStrings(string from, string to);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_AddIsValid(Asset[] assets);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_Add(Asset[] assets, bool recursive);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_DeleteChangeSetsIsValid(ChangeSet[] changes);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_DeleteChangeSets(ChangeSet[] changesets);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_RevertChangeSets(ChangeSet[] changesets, RevertMode mode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_SubmitIsValid(ChangeSet changeset, Asset[] assets);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_Submit(ChangeSet changeset, Asset[] assets, string description, bool saveOnly);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_DiffIsValid(Asset[] assets);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_DiffHead(Asset[] assets, bool includingMetaFiles);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_ResolveIsValid(Asset[] assets);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_Resolve(Asset[] assets, ResolveMethod resolveMethod);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_Merge(Asset[] assets, MergeMethod method);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_LockIsValid(Asset[] assets);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_UnlockIsValid(Asset[] assets);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_Lock(Asset[] assets, bool locked);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_RevertIsValid(Asset[] assets, RevertMode mode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_Revert(Asset[] assets, RevertMode mode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_GetLatestIsValid(Asset[] assets);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_GetLatest(Asset[] assets);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_SetFileMode(Asset[] assets, FileMode mode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_SetFileModeStrings(string[] assets, FileMode mode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_ChangeSetDescription(ChangeSet changeset);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_ChangeSetStatus(ChangeSet changeset);

    /// <summary>
    ///   <para>Get a list of pending changesets owned by the current user.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Task ChangeSets();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_ChangeSetMove(Asset[] assets, ChangeSet target);

    /// <summary>
    ///   <para>Start a task for quering the version control server for incoming changes.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Task Incoming();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Task Internal_IncomingChangeSetAssets(ChangeSet changeset);

    /// <summary>
    ///   <para>Returns true if an asset can be edited.</para>
    /// </summary>
    /// <param name="asset">Asset to test.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsOpenForEdit(Asset asset);

    /// <summary>
    ///   <para>Start a task that sends the version control settings to the version control system.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Task UpdateSettings();

    /// <summary>
    ///   <para>Returns version control information about an asset.</para>
    /// </summary>
    /// <param name="unityPath">Path to asset.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Asset GetAssetByPath(string unityPath);

    /// <summary>
    ///   <para>Returns version control information about an asset.</para>
    /// </summary>
    /// <param name="guid">GUID of asset.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Asset GetAssetByGUID(string guid);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Asset[] Internal_GetAssetArrayFromSelection();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GenerateID();

    /// <summary>
    ///   <para>This will invalidate the cached state information for all assets.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearCache();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void InvalidateCache();

    internal static Asset CacheStatus(string assetPath)
    {
      return Provider.Internal_CacheStatus(assetPath);
    }

    /// <summary>
    ///   <para>Start a task that will fetch the most recent status from revision control system.</para>
    /// </summary>
    /// <param name="assets">The assets fetch new state for.</param>
    /// <param name="asset">The asset path to fetch new state for.</param>
    /// <param name="recursively">If any assets specified are folders this flag will get status for all descendants of the folder as well.</param>
    public static Task Status(AssetList assets)
    {
      return Provider.Internal_Status(assets.ToArray(), true);
    }

    /// <summary>
    ///   <para>Start a task that will fetch the most recent status from revision control system.</para>
    /// </summary>
    /// <param name="assets">The assets fetch new state for.</param>
    /// <param name="asset">The asset path to fetch new state for.</param>
    /// <param name="recursively">If any assets specified are folders this flag will get status for all descendants of the folder as well.</param>
    public static Task Status(Asset asset)
    {
      return Provider.Internal_Status(new Asset[1]{ asset }, 1 != 0);
    }

    /// <summary>
    ///   <para>Start a task that will fetch the most recent status from revision control system.</para>
    /// </summary>
    /// <param name="assets">The assets fetch new state for.</param>
    /// <param name="asset">The asset path to fetch new state for.</param>
    /// <param name="recursively">If any assets specified are folders this flag will get status for all descendants of the folder as well.</param>
    public static Task Status(AssetList assets, bool recursively)
    {
      return Provider.Internal_Status(assets.ToArray(), recursively);
    }

    /// <summary>
    ///   <para>Start a task that will fetch the most recent status from revision control system.</para>
    /// </summary>
    /// <param name="assets">The assets fetch new state for.</param>
    /// <param name="asset">The asset path to fetch new state for.</param>
    /// <param name="recursively">If any assets specified are folders this flag will get status for all descendants of the folder as well.</param>
    public static Task Status(Asset asset, bool recursively)
    {
      return Provider.Internal_Status(new Asset[1]{ asset }, (recursively ? 1 : 0) != 0);
    }

    /// <summary>
    ///   <para>Start a task that will fetch the most recent status from revision control system.</para>
    /// </summary>
    /// <param name="assets">The assets fetch new state for.</param>
    /// <param name="asset">The asset path to fetch new state for.</param>
    /// <param name="recursively">If any assets specified are folders this flag will get status for all descendants of the folder as well.</param>
    public static Task Status(string[] assets)
    {
      return Provider.Internal_StatusStrings(assets, true);
    }

    /// <summary>
    ///   <para>Start a task that will fetch the most recent status from revision control system.</para>
    /// </summary>
    /// <param name="assets">The assets fetch new state for.</param>
    /// <param name="asset">The asset path to fetch new state for.</param>
    /// <param name="recursively">If any assets specified are folders this flag will get status for all descendants of the folder as well.</param>
    public static Task Status(string[] assets, bool recursively)
    {
      return Provider.Internal_StatusStrings(assets, recursively);
    }

    /// <summary>
    ///   <para>Start a task that will fetch the most recent status from revision control system.</para>
    /// </summary>
    /// <param name="assets">The assets fetch new state for.</param>
    /// <param name="asset">The asset path to fetch new state for.</param>
    /// <param name="recursively">If any assets specified are folders this flag will get status for all descendants of the folder as well.</param>
    public static Task Status(string asset)
    {
      return Provider.Internal_StatusStrings(new string[1]{ asset }, 1 != 0);
    }

    /// <summary>
    ///   <para>Start a task that will fetch the most recent status from revision control system.</para>
    /// </summary>
    /// <param name="assets">The assets fetch new state for.</param>
    /// <param name="asset">The asset path to fetch new state for.</param>
    /// <param name="recursively">If any assets specified are folders this flag will get status for all descendants of the folder as well.</param>
    public static Task Status(string asset, bool recursively)
    {
      return Provider.Internal_StatusStrings(new string[1]{ asset }, (recursively ? 1 : 0) != 0);
    }

    /// <summary>
    ///   <para>Uses the version control plugin to move an asset from one path to another.</para>
    /// </summary>
    /// <param name="from">Path to source asset.</param>
    /// <param name="to">Path to destination.</param>
    public static Task Move(string from, string to)
    {
      return Provider.Internal_MoveAsStrings(from, to);
    }

    /// <summary>
    ///   <para>Given an asset or a  list of assets this function returns true if Checkout is a valid task to perform.</para>
    /// </summary>
    /// <param name="assets">List of assets.</param>
    /// <param name="asset">Single asset.</param>
    public static bool CheckoutIsValid(AssetList assets)
    {
      return Provider.CheckoutIsValid(assets, CheckoutMode.Exact);
    }

    public static bool CheckoutIsValid(AssetList assets, CheckoutMode mode)
    {
      return Provider.Internal_CheckoutIsValid(assets.ToArray(), mode);
    }

    /// <summary>
    ///   <para>Checkout an asset or list of asset from the version control system.</para>
    /// </summary>
    /// <param name="assets">List of assets to checkout.</param>
    /// <param name="mode">Tell the Provider to checkout the asset, the .meta file or both.</param>
    /// <param name="asset">Asset to checkout.</param>
    public static Task Checkout(AssetList assets, CheckoutMode mode)
    {
      return Provider.Internal_Checkout(assets.ToArray(), mode);
    }

    /// <summary>
    ///   <para>Checkout an asset or list of asset from the version control system.</para>
    /// </summary>
    /// <param name="assets">List of assets to checkout.</param>
    /// <param name="mode">Tell the Provider to checkout the asset, the .meta file or both.</param>
    /// <param name="asset">Asset to checkout.</param>
    public static Task Checkout(string[] assets, CheckoutMode mode)
    {
      return Provider.Internal_CheckoutStrings(assets, mode);
    }

    /// <summary>
    ///   <para>Checkout an asset or list of asset from the version control system.</para>
    /// </summary>
    /// <param name="assets">List of assets to checkout.</param>
    /// <param name="mode">Tell the Provider to checkout the asset, the .meta file or both.</param>
    /// <param name="asset">Asset to checkout.</param>
    public static Task Checkout(Object[] assets, CheckoutMode mode)
    {
      AssetList assetList = new AssetList();
      foreach (Object asset in assets)
      {
        Asset assetByPath = Provider.GetAssetByPath(AssetDatabase.GetAssetPath(asset));
        assetList.Add(assetByPath);
      }
      return Provider.Internal_Checkout(assetList.ToArray(), mode);
    }

    /// <summary>
    ///   <para>Given an asset or a  list of assets this function returns true if Checkout is a valid task to perform.</para>
    /// </summary>
    /// <param name="assets">List of assets.</param>
    /// <param name="asset">Single asset.</param>
    public static bool CheckoutIsValid(Asset asset)
    {
      return Provider.CheckoutIsValid(asset, CheckoutMode.Exact);
    }

    public static bool CheckoutIsValid(Asset asset, CheckoutMode mode)
    {
      return Provider.Internal_CheckoutIsValid(new Asset[1]{ asset }, mode);
    }

    /// <summary>
    ///   <para>Checkout an asset or list of asset from the version control system.</para>
    /// </summary>
    /// <param name="assets">List of assets to checkout.</param>
    /// <param name="mode">Tell the Provider to checkout the asset, the .meta file or both.</param>
    /// <param name="asset">Asset to checkout.</param>
    public static Task Checkout(Asset asset, CheckoutMode mode)
    {
      return Provider.Internal_Checkout(new Asset[1]{ asset }, mode);
    }

    /// <summary>
    ///   <para>Checkout an asset or list of asset from the version control system.</para>
    /// </summary>
    /// <param name="assets">List of assets to checkout.</param>
    /// <param name="mode">Tell the Provider to checkout the asset, the .meta file or both.</param>
    /// <param name="asset">Asset to checkout.</param>
    public static Task Checkout(string asset, CheckoutMode mode)
    {
      return Provider.Internal_CheckoutStrings(new string[1]{ asset }, mode);
    }

    /// <summary>
    ///   <para>Checkout an asset or list of asset from the version control system.</para>
    /// </summary>
    /// <param name="assets">List of assets to checkout.</param>
    /// <param name="mode">Tell the Provider to checkout the asset, the .meta file or both.</param>
    /// <param name="asset">Asset to checkout.</param>
    public static Task Checkout(Object asset, CheckoutMode mode)
    {
      return Provider.Internal_Checkout(new Asset[1]{ Provider.GetAssetByPath(AssetDatabase.GetAssetPath(asset)) }, mode);
    }

    internal static bool PromptAndCheckoutIfNeeded(string[] assets, string promptIfCheckoutIsNeeded)
    {
      return Provider.Internal_PromptAndCheckoutIfNeeded(assets, promptIfCheckoutIsNeeded);
    }

    /// <summary>
    ///   <para>This will statt a task for deleting an asset or assets both from disk and from version control system.</para>
    /// </summary>
    /// <param name="assetProjectPath">Project path of asset.</param>
    /// <param name="assets">List of assets to delete.</param>
    /// <param name="asset">Asset to delete.</param>
    public static Task Delete(string assetProjectPath)
    {
      return Provider.Internal_DeleteAtProjectPath(assetProjectPath);
    }

    /// <summary>
    ///   <para>This will statt a task for deleting an asset or assets both from disk and from version control system.</para>
    /// </summary>
    /// <param name="assetProjectPath">Project path of asset.</param>
    /// <param name="assets">List of assets to delete.</param>
    /// <param name="asset">Asset to delete.</param>
    public static Task Delete(AssetList assets)
    {
      return Provider.Internal_Delete(assets.ToArray());
    }

    /// <summary>
    ///   <para>This will statt a task for deleting an asset or assets both from disk and from version control system.</para>
    /// </summary>
    /// <param name="assetProjectPath">Project path of asset.</param>
    /// <param name="assets">List of assets to delete.</param>
    /// <param name="asset">Asset to delete.</param>
    public static Task Delete(Asset asset)
    {
      return Provider.Internal_Delete(new Asset[1]{ asset });
    }

    /// <summary>
    ///   <para>Given a list of assets this function returns true if Add is a valid task to perform.</para>
    /// </summary>
    /// <param name="assets">List of assets to test.</param>
    public static bool AddIsValid(AssetList assets)
    {
      return Provider.Internal_AddIsValid(assets.ToArray());
    }

    /// <summary>
    ///   <para>Adds an assets or list of assets to version control.</para>
    /// </summary>
    /// <param name="assets">List of assets to add to version control system.</param>
    /// <param name="recursive">Set this true if adding should be done recursively into subfolders.</param>
    /// <param name="asset">Single asset to add to version control system.</param>
    public static Task Add(AssetList assets, bool recursive)
    {
      return Provider.Internal_Add(assets.ToArray(), recursive);
    }

    /// <summary>
    ///   <para>Adds an assets or list of assets to version control.</para>
    /// </summary>
    /// <param name="assets">List of assets to add to version control system.</param>
    /// <param name="recursive">Set this true if adding should be done recursively into subfolders.</param>
    /// <param name="asset">Single asset to add to version control system.</param>
    public static Task Add(Asset asset, bool recursive)
    {
      return Provider.Internal_Add(new Asset[1]{ asset }, (recursive ? 1 : 0) != 0);
    }

    /// <summary>
    ///   <para>Test if deleting a changeset is a valid task to perform.</para>
    /// </summary>
    /// <param name="changesets">Changeset to test.</param>
    public static bool DeleteChangeSetsIsValid(ChangeSets changesets)
    {
      return Provider.Internal_DeleteChangeSetsIsValid(changesets.ToArray());
    }

    /// <summary>
    ///   <para>Starts a task that will attempt to delete the given changeset.</para>
    /// </summary>
    /// <param name="changesets">List of changetsets.</param>
    public static Task DeleteChangeSets(ChangeSets changesets)
    {
      return Provider.Internal_DeleteChangeSets(changesets.ToArray());
    }

    internal static Task RevertChangeSets(ChangeSets changesets, RevertMode mode)
    {
      return Provider.Internal_RevertChangeSets(changesets.ToArray(), mode);
    }

    /// <summary>
    ///   <para>Returns true if submitting the assets is a valid operation.</para>
    /// </summary>
    /// <param name="changeset">The changeset to submit.</param>
    /// <param name="assets">The asset to submit.</param>
    public static bool SubmitIsValid(ChangeSet changeset, AssetList assets)
    {
      return Provider.Internal_SubmitIsValid(changeset, assets == null ? (Asset[]) null : assets.ToArray());
    }

    /// <summary>
    ///   <para>Start a task that submits the assets to version control.</para>
    /// </summary>
    /// <param name="changeset">The changeset to submit.</param>
    /// <param name="list">The list of assets to submit.</param>
    /// <param name="description">The description of the changeset.</param>
    /// <param name="saveOnly">If true then only save the changeset to be submitted later.</param>
    public static Task Submit(ChangeSet changeset, AssetList list, string description, bool saveOnly)
    {
      return Provider.Internal_Submit(changeset, list == null ? (Asset[]) null : list.ToArray(), description, saveOnly);
    }

    /// <summary>
    ///   <para>Return true is starting a Diff task is a valid operation.</para>
    /// </summary>
    /// <param name="assets">List of assets.</param>
    public static bool DiffIsValid(AssetList assets)
    {
      return Provider.Internal_DiffIsValid(assets.ToArray());
    }

    /// <summary>
    ///   <para>Starts a task for showing a diff of the given assest versus their head revision.</para>
    /// </summary>
    /// <param name="assets">List of assets.</param>
    /// <param name="includingMetaFiles">Whether or not to include .meta.</param>
    public static Task DiffHead(AssetList assets, bool includingMetaFiles)
    {
      return Provider.Internal_DiffHead(assets.ToArray(), includingMetaFiles);
    }

    /// <summary>
    ///   <para>Tests if any of the assets in the list is resolvable.</para>
    /// </summary>
    /// <param name="assets">The list of asset to be resolved.</param>
    public static bool ResolveIsValid(AssetList assets)
    {
      return Provider.Internal_ResolveIsValid(assets.ToArray());
    }

    /// <summary>
    ///   <para>Start a task that will resolve conflicting assets in version control.</para>
    /// </summary>
    /// <param name="assets">The list of asset to mark as resolved.</param>
    /// <param name="resolveMethod">How the assets should be resolved.</param>
    public static Task Resolve(AssetList assets, ResolveMethod resolveMethod)
    {
      return Provider.Internal_Resolve(assets.ToArray(), resolveMethod);
    }

    /// <summary>
    ///   <para>This method will initiate a merge task handle merging of the conflicting assets.</para>
    /// </summary>
    /// <param name="assets">The list of conflicting assets to be merged.</param>
    /// <param name="method">How to merge the assets.</param>
    public static Task Merge(AssetList assets, MergeMethod method)
    {
      return Provider.Internal_Merge(assets.ToArray(), method);
    }

    /// <summary>
    ///   <para>Return true if the task can be executed.</para>
    /// </summary>
    /// <param name="assets">List of assets to test.</param>
    /// <param name="asset">Asset to test.</param>
    public static bool LockIsValid(AssetList assets)
    {
      return Provider.Internal_LockIsValid(assets.ToArray());
    }

    /// <summary>
    ///   <para>Return true if the task can be executed.</para>
    /// </summary>
    /// <param name="assets">List of assets to test.</param>
    /// <param name="asset">Asset to test.</param>
    public static bool LockIsValid(Asset asset)
    {
      return Provider.Internal_LockIsValid(new Asset[1]{ asset });
    }

    /// <summary>
    ///   <para>Returns true if locking the assets is a valid operation.</para>
    /// </summary>
    /// <param name="assets">The assets to lock.</param>
    /// <param name="asset">The asset to lock.</param>
    public static bool UnlockIsValid(AssetList assets)
    {
      return Provider.Internal_UnlockIsValid(assets.ToArray());
    }

    /// <summary>
    ///   <para>Returns true if locking the assets is a valid operation.</para>
    /// </summary>
    /// <param name="assets">The assets to lock.</param>
    /// <param name="asset">The asset to lock.</param>
    public static bool UnlockIsValid(Asset asset)
    {
      return Provider.Internal_UnlockIsValid(new Asset[1]{ asset });
    }

    /// <summary>
    ///   <para>Attempt to lock an asset for exclusive editing.</para>
    /// </summary>
    /// <param name="assets">List of assets to lock/unlock.</param>
    /// <param name="locked">True to lock assets, false to unlock assets.</param>
    /// <param name="asset">Asset to lock/unlock.</param>
    public static Task Lock(AssetList assets, bool locked)
    {
      return Provider.Internal_Lock(assets.ToArray(), locked);
    }

    /// <summary>
    ///   <para>Attempt to lock an asset for exclusive editing.</para>
    /// </summary>
    /// <param name="assets">List of assets to lock/unlock.</param>
    /// <param name="locked">True to lock assets, false to unlock assets.</param>
    /// <param name="asset">Asset to lock/unlock.</param>
    public static Task Lock(Asset asset, bool locked)
    {
      return Provider.Internal_Lock(new Asset[1]{ asset }, (locked ? 1 : 0) != 0);
    }

    /// <summary>
    ///   <para>Return true if Revert is a valid task to perform.</para>
    /// </summary>
    /// <param name="assets">List of assets to test.</param>
    /// <param name="mode">Revert mode to test for.</param>
    /// <param name="asset">Asset to test.</param>
    public static bool RevertIsValid(AssetList assets, RevertMode mode)
    {
      return Provider.Internal_RevertIsValid(assets.ToArray(), mode);
    }

    /// <summary>
    ///   <para>Reverts the specified assets by undoing any changes done since last time you synced.</para>
    /// </summary>
    /// <param name="assets">The list of assets to be reverted.</param>
    /// <param name="mode">How to revert the assets.</param>
    /// <param name="asset">The asset to be reverted.</param>
    public static Task Revert(AssetList assets, RevertMode mode)
    {
      return Provider.Internal_Revert(assets.ToArray(), mode);
    }

    /// <summary>
    ///   <para>Return true if Revert is a valid task to perform.</para>
    /// </summary>
    /// <param name="assets">List of assets to test.</param>
    /// <param name="mode">Revert mode to test for.</param>
    /// <param name="asset">Asset to test.</param>
    public static bool RevertIsValid(Asset asset, RevertMode mode)
    {
      return Provider.Internal_RevertIsValid(new Asset[1]{ asset }, mode);
    }

    /// <summary>
    ///   <para>Reverts the specified assets by undoing any changes done since last time you synced.</para>
    /// </summary>
    /// <param name="assets">The list of assets to be reverted.</param>
    /// <param name="mode">How to revert the assets.</param>
    /// <param name="asset">The asset to be reverted.</param>
    public static Task Revert(Asset asset, RevertMode mode)
    {
      return Provider.Internal_Revert(new Asset[1]{ asset }, mode);
    }

    /// <summary>
    ///   <para>Returns true if getting the latest version of an asset is a valid operation.</para>
    /// </summary>
    /// <param name="assets">List of assets to test.</param>
    /// <param name="asset">Asset to test.</param>
    public static bool GetLatestIsValid(AssetList assets)
    {
      return Provider.Internal_GetLatestIsValid(assets.ToArray());
    }

    /// <summary>
    ///   <para>Returns true if getting the latest version of an asset is a valid operation.</para>
    /// </summary>
    /// <param name="assets">List of assets to test.</param>
    /// <param name="asset">Asset to test.</param>
    public static bool GetLatestIsValid(Asset asset)
    {
      return Provider.Internal_GetLatestIsValid(new Asset[1]{ asset });
    }

    /// <summary>
    ///   <para>Start a task for getting the latest version of an asset from the version control server.</para>
    /// </summary>
    /// <param name="assets">List of assets to update.</param>
    /// <param name="asset">Asset to update.</param>
    public static Task GetLatest(AssetList assets)
    {
      return Provider.Internal_GetLatest(assets.ToArray());
    }

    /// <summary>
    ///   <para>Start a task for getting the latest version of an asset from the version control server.</para>
    /// </summary>
    /// <param name="assets">List of assets to update.</param>
    /// <param name="asset">Asset to update.</param>
    public static Task GetLatest(Asset asset)
    {
      return Provider.Internal_GetLatest(new Asset[1]{ asset });
    }

    internal static Task SetFileMode(AssetList assets, FileMode mode)
    {
      return Provider.Internal_SetFileMode(assets.ToArray(), mode);
    }

    internal static Task SetFileMode(string[] assets, FileMode mode)
    {
      return Provider.Internal_SetFileModeStrings(assets, mode);
    }

    /// <summary>
    ///   <para>Given a changeset only containing the changeset ID, this will start a task for quering the description of the changeset.</para>
    /// </summary>
    /// <param name="changeset">Changeset to query description of.</param>
    public static Task ChangeSetDescription(ChangeSet changeset)
    {
      return Provider.Internal_ChangeSetDescription(changeset);
    }

    /// <summary>
    ///   <para>Retrieves the list of assets belonging to a changeset.</para>
    /// </summary>
    /// <param name="changeset">Changeset to query for assets.</param>
    /// <param name="changesetID">ChangesetID to query for assets.</param>
    public static Task ChangeSetStatus(ChangeSet changeset)
    {
      return Provider.Internal_ChangeSetStatus(changeset);
    }

    /// <summary>
    ///   <para>Retrieves the list of assets belonging to a changeset.</para>
    /// </summary>
    /// <param name="changeset">Changeset to query for assets.</param>
    /// <param name="changesetID">ChangesetID to query for assets.</param>
    public static Task ChangeSetStatus(string changesetID)
    {
      return Provider.Internal_ChangeSetStatus(new ChangeSet(string.Empty, changesetID));
    }

    /// <summary>
    ///   <para>Given an incoming changeset this will start a task to query the version control server for which assets are part of the changeset.</para>
    /// </summary>
    /// <param name="changeset">Incoming changeset.</param>
    /// <param name="changesetID">Incoming changesetid.</param>
    public static Task IncomingChangeSetAssets(ChangeSet changeset)
    {
      return Provider.Internal_IncomingChangeSetAssets(changeset);
    }

    /// <summary>
    ///   <para>Given an incoming changeset this will start a task to query the version control server for which assets are part of the changeset.</para>
    /// </summary>
    /// <param name="changeset">Incoming changeset.</param>
    /// <param name="changesetID">Incoming changesetid.</param>
    public static Task IncomingChangeSetAssets(string changesetID)
    {
      return Provider.Internal_IncomingChangeSetAssets(new ChangeSet(string.Empty, changesetID));
    }

    /// <summary>
    ///   <para>Move an asset or list of assets from their current changeset to a new changeset.</para>
    /// </summary>
    /// <param name="assets">List of asset to move to changeset.</param>
    /// <param name="changeset">Changeset to move asset to.</param>
    /// <param name="asset">Asset to move to changeset.</param>
    /// <param name="changesetID">ChangesetID to move asset to.</param>
    public static Task ChangeSetMove(AssetList assets, ChangeSet changeset)
    {
      return Provider.Internal_ChangeSetMove(assets.ToArray(), changeset);
    }

    /// <summary>
    ///   <para>Move an asset or list of assets from their current changeset to a new changeset.</para>
    /// </summary>
    /// <param name="assets">List of asset to move to changeset.</param>
    /// <param name="changeset">Changeset to move asset to.</param>
    /// <param name="asset">Asset to move to changeset.</param>
    /// <param name="changesetID">ChangesetID to move asset to.</param>
    public static Task ChangeSetMove(Asset asset, ChangeSet changeset)
    {
      return Provider.Internal_ChangeSetMove(new Asset[1]{ asset }, changeset);
    }

    /// <summary>
    ///   <para>Move an asset or list of assets from their current changeset to a new changeset.</para>
    /// </summary>
    /// <param name="assets">List of asset to move to changeset.</param>
    /// <param name="changeset">Changeset to move asset to.</param>
    /// <param name="asset">Asset to move to changeset.</param>
    /// <param name="changesetID">ChangesetID to move asset to.</param>
    public static Task ChangeSetMove(AssetList assets, string changesetID)
    {
      ChangeSet target = new ChangeSet(string.Empty, changesetID);
      return Provider.Internal_ChangeSetMove(assets.ToArray(), target);
    }

    /// <summary>
    ///   <para>Move an asset or list of assets from their current changeset to a new changeset.</para>
    /// </summary>
    /// <param name="assets">List of asset to move to changeset.</param>
    /// <param name="changeset">Changeset to move asset to.</param>
    /// <param name="asset">Asset to move to changeset.</param>
    /// <param name="changesetID">ChangesetID to move asset to.</param>
    public static Task ChangeSetMove(Asset asset, string changesetID)
    {
      ChangeSet target = new ChangeSet(string.Empty, changesetID);
      return Provider.Internal_ChangeSetMove(new Asset[1]{ asset }, target);
    }

    /// <summary>
    ///   <para>Return version control information about the currently selected assets.</para>
    /// </summary>
    public static AssetList GetAssetListFromSelection()
    {
      AssetList assetList = new AssetList();
      foreach (Asset asset in Provider.Internal_GetAssetArrayFromSelection())
        assetList.Add(asset);
      return assetList;
    }
  }
}

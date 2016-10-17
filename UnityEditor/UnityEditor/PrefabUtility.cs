// Decompiled with JetBrains decompiler
// Type: UnityEditor.PrefabUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Utility class for any prefab related operations.</para>
  /// </summary>
  public sealed class PrefabUtility
  {
    /// <summary>
    ///   <para>Called after prefab instances in the scene have been updated.</para>
    /// </summary>
    public static PrefabUtility.PrefabInstanceUpdated prefabInstanceUpdated;

    /// <summary>
    ///   <para>Returns the parent asset object of source, or null if it can't be found.</para>
    /// </summary>
    /// <param name="source"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Object GetPrefabParent(Object source);

    /// <summary>
    ///   <para>Retrieves the enclosing prefab for any object contained within.</para>
    /// </summary>
    /// <param name="targetObject">An object contained within a prefab object.</param>
    /// <returns>
    ///   <para>The prefab the object is contained in.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Object GetPrefabObject(Object targetObject);

    /// <summary>
    ///   <para>Extract all modifications that are applied to the prefab instance compared to the parent prefab.</para>
    /// </summary>
    /// <param name="targetPrefab"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern PropertyModification[] GetPropertyModifications(Object targetPrefab);

    /// <summary>
    ///   <para>Assigns all modifications that are applied to the prefab instance compared to the parent prefab.</para>
    /// </summary>
    /// <param name="targetPrefab"></param>
    /// <param name="modifications"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetPropertyModifications(Object targetPrefab, PropertyModification[] modifications);

    /// <summary>
    ///   <para>Instantiate an asset that is referenced by a prefab and use it on the prefab instance.</para>
    /// </summary>
    /// <param name="targetObject"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Object InstantiateAttachedAsset(Object targetObject);

    /// <summary>
    ///   <para>Force record property modifications by comparing against the parent prefab.</para>
    /// </summary>
    /// <param name="targetObject"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RecordPrefabInstancePropertyModifications(Object targetObject);

    /// <summary>
    ///   <para>Force re-merging all prefab instances of this prefab.</para>
    /// </summary>
    /// <param name="targetObject"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void MergeAllPrefabInstances(Object targetObject);

    /// <summary>
    ///   <para>Disconnects the prefab instance from its parent prefab.</para>
    /// </summary>
    /// <param name="targetObject"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DisconnectPrefabInstance(Object targetObject);

    /// <summary>
    ///   <para>Instantiates the given prefab in a given scene.</para>
    /// </summary>
    /// <param name="target">Prefab asset to instantiate.</param>
    /// <param name="destinationScene">Scene to instantiate the prefab in.</param>
    /// <returns>
    ///   <para>The GameObject at the root of the prefab.</para>
    /// </returns>
    public static Object InstantiatePrefab(Object target)
    {
      return PrefabUtility.InternalInstantiatePrefab(target, SceneManager.GetActiveScene());
    }

    /// <summary>
    ///   <para>Instantiates the given prefab in a given scene.</para>
    /// </summary>
    /// <param name="target">Prefab asset to instantiate.</param>
    /// <param name="destinationScene">Scene to instantiate the prefab in.</param>
    /// <returns>
    ///   <para>The GameObject at the root of the prefab.</para>
    /// </returns>
    public static Object InstantiatePrefab(Object target, Scene destinationScene)
    {
      return PrefabUtility.InternalInstantiatePrefab(target, destinationScene);
    }

    private static Object InternalInstantiatePrefab(Object target, Scene destinationScene)
    {
      return PrefabUtility.INTERNAL_CALL_InternalInstantiatePrefab(target, ref destinationScene);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Object INTERNAL_CALL_InternalInstantiatePrefab(Object target, ref Scene destinationScene);

    /// <summary>
    ///   <para>Creates an empty prefab at given path.</para>
    /// </summary>
    /// <param name="path"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Object CreateEmptyPrefab(string path);

    /// <summary>
    ///   <para>Creates a prefab from a game object hierarchy.</para>
    /// </summary>
    /// <param name="path"></param>
    /// <param name="go"></param>
    /// <param name="options"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject CreatePrefab(string path, GameObject go, [DefaultValue("ReplacePrefabOptions.Default")] ReplacePrefabOptions options);

    /// <summary>
    ///   <para>Creates a prefab from a game object hierarchy.</para>
    /// </summary>
    /// <param name="path"></param>
    /// <param name="go"></param>
    /// <param name="options"></param>
    [ExcludeFromDocs]
    public static GameObject CreatePrefab(string path, GameObject go)
    {
      ReplacePrefabOptions options = ReplacePrefabOptions.Default;
      return PrefabUtility.CreatePrefab(path, go, options);
    }

    /// <summary>
    ///   <para>Replaces the targetPrefab with a copy of the game object hierarchy go.</para>
    /// </summary>
    /// <param name="go"></param>
    /// <param name="targetPrefab"></param>
    /// <param name="options"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject ReplacePrefab(GameObject go, Object targetPrefab, [DefaultValue("ReplacePrefabOptions.Default")] ReplacePrefabOptions options);

    /// <summary>
    ///   <para>Replaces the targetPrefab with a copy of the game object hierarchy go.</para>
    /// </summary>
    /// <param name="go"></param>
    /// <param name="targetPrefab"></param>
    /// <param name="options"></param>
    [ExcludeFromDocs]
    public static GameObject ReplacePrefab(GameObject go, Object targetPrefab)
    {
      ReplacePrefabOptions options = ReplacePrefabOptions.Default;
      return PrefabUtility.ReplacePrefab(go, targetPrefab, options);
    }

    /// <summary>
    ///   <para>Connects the source prefab to the game object.</para>
    /// </summary>
    /// <param name="go"></param>
    /// <param name="sourcePrefab"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject ConnectGameObjectToPrefab(GameObject go, GameObject sourcePrefab);

    /// <summary>
    ///   <para>Returns the topmost game object that has the same prefab parent as target.</para>
    /// </summary>
    /// <param name="target"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject FindRootGameObjectWithSameParentPrefab(GameObject target);

    /// <summary>
    ///   <para>Returns root game object of the prefab instance if that root prefab instance is a parent of the prefab.</para>
    /// </summary>
    /// <param name="target"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject FindValidUploadPrefabInstanceRoot(GameObject target);

    /// <summary>
    ///   <para>Connects the game object to the prefab that it was last connected to.</para>
    /// </summary>
    /// <param name="go"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool ReconnectToLastPrefab(GameObject go);

    /// <summary>
    ///   <para>Resets the properties of the component or game object to the parent prefab state.</para>
    /// </summary>
    /// <param name="obj"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool ResetToPrefabState(Object obj);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsComponentAddedToPrefabInstance(Object source);

    /// <summary>
    ///   <para>Resets the properties of all objects in the prefab, including child game objects and components that were added to the prefab instance.</para>
    /// </summary>
    /// <param name="go"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool RevertPrefabInstance(GameObject go);

    /// <summary>
    ///   <para>Given an object, returns its prefab type (None, if it's not a prefab).</para>
    /// </summary>
    /// <param name="target"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern PrefabType GetPrefabType(Object target);

    /// <summary>
    ///   <para>Helper function to find the prefab root of an object (used for picking niceness).</para>
    /// </summary>
    /// <param name="source"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject FindPrefabRoot(GameObject source);

    private static void Internal_CallPrefabInstanceUpdated(GameObject instance)
    {
      if (PrefabUtility.prefabInstanceUpdated == null)
        return;
      PrefabUtility.prefabInstanceUpdated(instance);
    }

    /// <summary>
    ///   <para>Delegate for method that is called after prefab instances in the scene have been updated.</para>
    /// </summary>
    /// <param name="instance"></param>
    public delegate void PrefabInstanceUpdated(GameObject instance);
  }
}

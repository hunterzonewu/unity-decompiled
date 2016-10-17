// Decompiled with JetBrains decompiler
// Type: UnityEditor.Undo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Lets you register undo operations on specific objects you are about to perform changes on.</para>
  /// </summary>
  public sealed class Undo
  {
    /// <summary>
    ///   <para>Callback that is triggered after an undo or redo was executed.</para>
    /// </summary>
    public static Undo.UndoRedoCallback undoRedoPerformed;
    /// <summary>
    ///   <para>Invoked before the Undo system performs a flush.</para>
    /// </summary>
    public static Undo.WillFlushUndoRecord willFlushUndoRecord;
    public static Undo.PostprocessModifications postprocessModifications;

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetRecordsInternal(object undoRecords, object redoRecords);

    internal static void GetRecords(List<string> undoRecords, List<string> redoRecords)
    {
      Undo.GetRecordsInternal((object) undoRecords, (object) redoRecords);
    }

    /// <summary>
    ///   <para>Stores a copy of the object states on the undo stack.</para>
    /// </summary>
    /// <param name="objectToUndo">The object whose state changes need to be undone.</param>
    /// <param name="name">The name of the undo operation.</param>
    public static void RegisterCompleteObjectUndo(UnityEngine.Object objectToUndo, string name)
    {
      UnityEngine.Object[] objectsToUndo = new UnityEngine.Object[1]{ objectToUndo };
      Undo.RegisterCompleteObjectUndoMultiple(objectToUndo, objectsToUndo, name, 0);
    }

    /// <summary>
    ///   <para>This is equivalent to calling the first overload mutiple times, save for the fact that only one undo operation will be generated for this one.</para>
    /// </summary>
    /// <param name="objectsToUndo">An array of objects whose state changes need to be undone.</param>
    /// <param name="name">The name of the undo operation.</param>
    public static void RegisterCompleteObjectUndo(UnityEngine.Object[] objectsToUndo, string name)
    {
      if (objectsToUndo.Length <= 0)
        return;
      Undo.RegisterCompleteObjectUndoMultiple(objectsToUndo[0], objectsToUndo, name, 0);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void RegisterCompleteObjectUndoMultiple(UnityEngine.Object identifier, UnityEngine.Object[] objectsToUndo, string name, int namePriority);

    /// <summary>
    ///   <para>Sets the parent of transform to the new parent and records an undo operation.</para>
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="newParent"></param>
    /// <param name="name"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetTransformParent(Transform transform, Transform newParent, string name);

    /// <summary>
    ///         <para>Move a GameObject from its current scene to a new scene.
    /// It is required that the GameObject is at the root of its current scene.</para>
    ///       </summary>
    /// <param name="go">GameObject to move.</param>
    /// <param name="scene">Scene to move the GameObject into.</param>
    /// <param name="name">Name of the undo action.</param>
    public static void MoveGameObjectToScene(GameObject go, Scene scene, string name)
    {
      Undo.INTERNAL_CALL_MoveGameObjectToScene(go, ref scene, name);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_MoveGameObjectToScene(GameObject go, ref Scene scene, string name);

    /// <summary>
    ///   <para>Register an undo operations for a newly created object.</para>
    /// </summary>
    /// <param name="objectToUndo">The object that was created.</param>
    /// <param name="name">The name of the action to undo. Think "Undo ...." in the main menu.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RegisterCreatedObjectUndo(UnityEngine.Object objectToUndo, string name);

    /// <summary>
    ///   <para>Destroys the object and records an undo operation so that it can be recreated.</para>
    /// </summary>
    /// <param name="objectToUndo">The object that will be destroyed.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DestroyObjectImmediate(UnityEngine.Object objectToUndo);

    /// <summary>
    ///   <para>Adds a component to the game object and registers an undo operation for this action.</para>
    /// </summary>
    /// <param name="gameObject">The game object you want to add the component to.</param>
    /// <param name="type">The type of component you want to add.</param>
    /// <returns>
    ///   <para>The newly added component.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Component AddComponent(GameObject gameObject, System.Type type);

    public static T AddComponent<T>(GameObject gameObject) where T : Component
    {
      return Undo.AddComponent(gameObject, typeof (T)) as T;
    }

    /// <summary>
    ///   <para>Copy the states of a hierarchy of objects onto the undo stack.</para>
    /// </summary>
    /// <param name="objectToUndo">The object used to determine a hierarchy of objects whose state changes need to be undone.</param>
    /// <param name="name">The name of the undo operation.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RegisterFullObjectHierarchyUndo(UnityEngine.Object objectToUndo, string name);

    /// <summary>
    ///   <para>This overload is deprecated. Use Undo.RegisterFullObjectHierarchyUndo(Object, string) instead.</para>
    /// </summary>
    /// <param name="objectToUndo"></param>
    [Obsolete("Use Undo.RegisterFullObjectHierarchyUndo(Object, string) instead")]
    public static void RegisterFullObjectHierarchyUndo(UnityEngine.Object objectToUndo)
    {
      Undo.RegisterFullObjectHierarchyUndo(objectToUndo, "Full Object Hierarchy");
    }

    /// <summary>
    ///   <para>Records any changes done on the object after the RecordObject function.</para>
    /// </summary>
    /// <param name="objectToUndo"></param>
    /// <param name="name"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RecordObject(UnityEngine.Object objectToUndo, string name);

    /// <summary>
    ///   <para>Records multiple undoable objects in a single call. This is the same as calling Undo.RecordObject multiple times.</para>
    /// </summary>
    /// <param name="objectsToUndo"></param>
    /// <param name="name"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RecordObjects(UnityEngine.Object[] objectsToUndo, string name);

    /// <summary>
    ///   <para>Removes all Undo operation for the identifier object registered using Undo.RegisterCompleteObjectUndo from the undo stack.</para>
    /// </summary>
    /// <param name="identifier"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearUndo(UnityEngine.Object identifier);

    /// <summary>
    ///   <para>Perform an Undo operation.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void PerformUndo();

    /// <summary>
    ///   <para>Perform an Redo operation.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void PerformRedo();

    /// <summary>
    ///   <para>Unity automatically groups undo operations by the current group index.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void IncrementCurrentGroup();

    /// <summary>
    ///   <para>Unity automatically groups undo operations by the current group index.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetCurrentGroup();

    /// <summary>
    ///   <para>Get the name that will be shown in the UI for the current undo group.</para>
    /// </summary>
    /// <returns>
    ///   <para>Name of the current group or an empty string if the current group is empty.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetCurrentGroupName();

    /// <summary>
    ///   <para>Set the name of the current undo group.</para>
    /// </summary>
    /// <param name="name">New name of the current undo group.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetCurrentGroupName(string name);

    /// <summary>
    ///   <para>Performs the last undo operation but does not record a redo operation.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RevertAllInCurrentGroup();

    /// <summary>
    ///   <para>Performs all undo operations up to the group index without storing a redo operation in the process.</para>
    /// </summary>
    /// <param name="group"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RevertAllDownToGroup(int group);

    /// <summary>
    ///   <para>Collapses all undo operation up to group index together into one step.</para>
    /// </summary>
    /// <param name="groupIndex"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CollapseUndoOperations(int groupIndex);

    [Obsolete("Use Undo.RecordObject instead")]
    public static void RegisterUndo(UnityEngine.Object objectToUndo, string name)
    {
      Undo.RegisterCompleteObjectUndo(objectToUndo, name);
    }

    [Obsolete("Use Undo.RecordObjects instead")]
    public static void RegisterUndo(UnityEngine.Object[] objectsToUndo, string name)
    {
      Undo.RegisterCompleteObjectUndo(objectsToUndo, name);
    }

    /// <summary>
    ///   <para>Ensure objects recorded using RecordObject or ::ref:RecordObjects are registered as an undoable action. In most cases there is no reason to invoke FlushUndoRecordObjects since it's automatically done right after mouse-up and certain other events that conventionally marks the end of an action.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void FlushUndoRecordObjects();

    private static UndoPropertyModification[] InvokePostprocessModifications(UndoPropertyModification[] modifications)
    {
      if (Undo.postprocessModifications != null)
        return Undo.postprocessModifications(modifications);
      return modifications;
    }

    private static void Internal_CallWillFlushUndoRecord()
    {
      if (Undo.willFlushUndoRecord == null)
        return;
      Undo.willFlushUndoRecord();
    }

    private static void Internal_CallUndoRedoPerformed()
    {
      if (Undo.undoRedoPerformed == null)
        return;
      Undo.undoRedoPerformed();
    }

    [Obsolete("Use Undo.RecordObject instead")]
    public static void SetSnapshotTarget(UnityEngine.Object objectToUndo, string name)
    {
    }

    [Obsolete("Use Undo.RecordObject instead")]
    public static void SetSnapshotTarget(UnityEngine.Object[] objectsToUndo, string name)
    {
    }

    [Obsolete("Use Undo.RecordObject instead")]
    public static void ClearSnapshotTarget()
    {
    }

    [Obsolete("Use Undo.RecordObject instead")]
    public static void CreateSnapshot()
    {
    }

    [Obsolete("Use Undo.RecordObject instead")]
    public static void RestoreSnapshot()
    {
    }

    [Obsolete("Use Undo.RecordObject instead")]
    public static void RegisterSnapshot()
    {
    }

    [Obsolete("Use DestroyObjectImmediate, RegisterCreatedObjectUndo or RegisterUndo instead.")]
    public static void RegisterSceneUndo(string name)
    {
    }

    /// <summary>
    ///   <para>Delegate used for undoRedoPerformed.</para>
    /// </summary>
    public delegate void UndoRedoCallback();

    /// <summary>
    ///   <para>Delegate used for willFlushUndoRecord.</para>
    /// </summary>
    public delegate void WillFlushUndoRecord();

    public delegate UndoPropertyModification[] PostprocessModifications(UndoPropertyModification[] modifications);
  }
}

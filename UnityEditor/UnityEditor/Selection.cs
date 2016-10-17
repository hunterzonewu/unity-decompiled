// Decompiled with JetBrains decompiler
// Type: UnityEditor.Selection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Access to the selection in the editor.</para>
  /// </summary>
  public sealed class Selection
  {
    /// <summary>
    ///   <para>Delegate callback triggered when currently active/selected item has changed.</para>
    /// </summary>
    public static System.Action selectionChanged;

    /// <summary>
    ///   <para>Returns the top level selection, excluding prefabs.</para>
    /// </summary>
    public static extern Transform[] transforms { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the active transform. (The one shown in the inspector).</para>
    /// </summary>
    public static extern Transform activeTransform { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the actual game object selection. Includes prefabs, non-modifyable objects.</para>
    /// </summary>
    public static extern GameObject[] gameObjects { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the active game object. (The one shown in the inspector).</para>
    /// </summary>
    public static extern GameObject activeGameObject { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the actual object selection. Includes prefabs, non-modifyable objects.</para>
    /// </summary>
    public static extern UnityEngine.Object activeObject { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the instanceID of the actual object selection. Includes prefabs, non-modifyable objects.</para>
    /// </summary>
    public static extern int activeInstanceID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The actual unfiltered selection from the Scene.</para>
    /// </summary>
    public static extern UnityEngine.Object[] objects { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The actual unfiltered selection from the Scene returned as instance ids instead of objects.</para>
    /// </summary>
    public static extern int[] instanceIDs { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern string[] assetGUIDsDeepSelection { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the guids of the selected assets.</para>
    /// </summary>
    public static extern string[] assetGUIDs { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns whether an object is contained in the current selection.</para>
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="obj"></param>
    public static bool Contains(int instanceID)
    {
      return Array.IndexOf<int>(Selection.instanceIDs, instanceID) != -1;
    }

    /// <summary>
    ///   <para>Returns whether an object is contained in the current selection.</para>
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="obj"></param>
    public static bool Contains(UnityEngine.Object obj)
    {
      return Selection.Contains(obj.GetInstanceID());
    }

    internal static void Add(int instanceID)
    {
      List<int> intList = new List<int>((IEnumerable<int>) Selection.instanceIDs);
      if (intList.IndexOf(instanceID) >= 0)
        return;
      intList.Add(instanceID);
      Selection.instanceIDs = intList.ToArray();
    }

    internal static void Add(UnityEngine.Object obj)
    {
      if (!(obj != (UnityEngine.Object) null))
        return;
      Selection.Add(obj.GetInstanceID());
    }

    internal static void Remove(int instanceID)
    {
      List<int> intList = new List<int>((IEnumerable<int>) Selection.instanceIDs);
      intList.Remove(instanceID);
      Selection.instanceIDs = intList.ToArray();
    }

    internal static void Remove(UnityEngine.Object obj)
    {
      if (!(obj != (UnityEngine.Object) null))
        return;
      Selection.Remove(obj.GetInstanceID());
    }

    /// <summary>
    ///   <para>Allows for fine grained control of the selection type using the SelectionMode bitmask.</para>
    /// </summary>
    /// <param name="mode">Options for refining the selection.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Transform[] GetTransforms(SelectionMode mode);

    /// <summary>
    ///   <para>Returns the current selection filtered by type and mode.</para>
    /// </summary>
    /// <param name="type">Only objects of this type will be retrieved.</param>
    /// <param name="mode">Further options to refine the selection.</param>
    public static UnityEngine.Object[] GetFiltered(System.Type type, SelectionMode mode)
    {
      ArrayList arrayList = new ArrayList();
      if (type == typeof (Component) || type.IsSubclassOf(typeof (Component)))
      {
        foreach (Component transform in Selection.GetTransforms(mode))
        {
          Component component = transform.GetComponent(type);
          if ((bool) ((UnityEngine.Object) component))
            arrayList.Add((object) component);
        }
      }
      else if (type == typeof (GameObject) || type.IsSubclassOf(typeof (GameObject)))
      {
        foreach (Transform transform in Selection.GetTransforms(mode))
          arrayList.Add((object) transform.gameObject);
      }
      else
      {
        foreach (UnityEngine.Object @object in Selection.GetObjectsMode(mode))
        {
          if (@object != (UnityEngine.Object) null && (@object.GetType() == type || @object.GetType().IsSubclassOf(type)))
            arrayList.Add((object) @object);
        }
      }
      return (UnityEngine.Object[]) arrayList.ToArray(typeof (UnityEngine.Object));
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern UnityEngine.Object[] GetObjectsMode(SelectionMode mode);

    private static void Internal_CallSelectionChanged()
    {
      if (Selection.selectionChanged == null)
        return;
      Selection.selectionChanged();
    }
  }
}

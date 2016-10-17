// Decompiled with JetBrains decompiler
// Type: UnityEditor.ObjectNames
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Helper class for constructing displayable names for objects.</para>
  /// </summary>
  public sealed class ObjectNames
  {
    /// <summary>
    ///   <para>Make a displayable name for a variable.</para>
    /// </summary>
    /// <param name="name"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string NicifyVariableName(string name);

    /// <summary>
    ///   <para>Inspector title for an object.</para>
    /// </summary>
    /// <param name="obj"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetInspectorTitle(UnityEngine.Object obj);

    /// <summary>
    ///   <para>Class name of an object.</para>
    /// </summary>
    /// <param name="obj"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetClassName(UnityEngine.Object obj);

    internal static string GetTypeName(UnityEngine.Object obj)
    {
      if (obj == (UnityEngine.Object) null)
        return "Object";
      string lower = AssetDatabase.GetAssetPath(obj).ToLower();
      if (lower.EndsWith(".unity"))
        return "Scene";
      if (lower.EndsWith(".guiskin"))
        return "GUI Skin";
      if (Directory.Exists(AssetDatabase.GetAssetPath(obj)))
        return "Folder";
      if (obj.GetType() == typeof (UnityEngine.Object))
        return Path.GetExtension(lower) + " File";
      return ObjectNames.GetClassName(obj);
    }

    /// <summary>
    ///   <para>Drag and drop title for an object.</para>
    /// </summary>
    /// <param name="obj"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetDragAndDropTitle(UnityEngine.Object obj);

    /// <summary>
    ///   <para>Sets the name of an Object.</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetNameSmart(UnityEngine.Object obj, string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetNameSmartWithInstanceID(int instanceID, string name);

    [Obsolete("Please use NicifyVariableName instead")]
    public static string MangleVariableName(string name)
    {
      return ObjectNames.NicifyVariableName(name);
    }

    [Obsolete("Please use GetInspectorTitle instead")]
    public static string GetPropertyEditorTitle(UnityEngine.Object obj)
    {
      return ObjectNames.GetInspectorTitle(obj);
    }
  }
}

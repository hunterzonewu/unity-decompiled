// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoScript
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Representation of Script assets.</para>
  /// </summary>
  public sealed class MonoScript : TextAsset
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public MonoScript();

    /// <summary>
    ///   <para>Returns the System.Type object of the class implemented by this script.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public System.Type GetClass();

    /// <summary>
    ///   <para>Returns the MonoScript object containing specified MonoBehaviour.</para>
    /// </summary>
    /// <param name="behaviour">The MonoBehaviour whose MonoScript should be returned.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern MonoScript FromMonoBehaviour(MonoBehaviour behaviour);

    /// <summary>
    ///   <para>Returns the MonoScript object containing specified ScriptableObject.</para>
    /// </summary>
    /// <param name="scriptableObject">The ScriptableObject whose MonoScript should be returned.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern MonoScript FromScriptableObject(ScriptableObject scriptableObject);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool GetScriptTypeWasJustCreatedFromComponentMenu();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SetScriptTypeWasJustCreatedFromComponentMenu();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void Init(string scriptContents, string className, string nameSpace, string assemblyName, bool isEditorScript);
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEngine.ScriptableObject
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A class you can derive from if you want to create objects that don't need to be attached to game objects.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public class ScriptableObject : Object
  {
    public ScriptableObject()
    {
      ScriptableObject.Internal_CreateScriptableObject(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateScriptableObject([Writable] ScriptableObject self);

    [Obsolete("Use EditorUtility.SetDirty instead")]
    public void SetDirty()
    {
      ScriptableObject.INTERNAL_CALL_SetDirty(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetDirty(ScriptableObject self);

    /// <summary>
    ///   <para>Creates an instance of a scriptable object.</para>
    /// </summary>
    /// <param name="className">The type of the ScriptableObject to create, as the name of the type.</param>
    /// <param name="type">The type of the ScriptableObject to create, as a System.Type instance.</param>
    /// <returns>
    ///   <para>The created ScriptableObject.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ScriptableObject CreateInstance(string className);

    /// <summary>
    ///   <para>Creates an instance of a scriptable object.</para>
    /// </summary>
    /// <param name="className">The type of the ScriptableObject to create, as the name of the type.</param>
    /// <param name="type">The type of the ScriptableObject to create, as a System.Type instance.</param>
    /// <returns>
    ///   <para>The created ScriptableObject.</para>
    /// </returns>
    public static ScriptableObject CreateInstance(System.Type type)
    {
      return ScriptableObject.CreateInstanceFromType(type);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern ScriptableObject CreateInstanceFromType(System.Type type);

    public static T CreateInstance<T>() where T : ScriptableObject
    {
      return (T) ScriptableObject.CreateInstance(typeof (T));
    }
  }
}

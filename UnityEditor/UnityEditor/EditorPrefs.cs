// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorPrefs
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Stores and accesses Unity editor preferences.</para>
  /// </summary>
  public sealed class EditorPrefs
  {
    /// <summary>
    ///   <para>Sets the value of the preference identified by key as an integer.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetInt(string key, int value);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetInt(string key, [DefaultValue("0")] int defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public static int GetInt(string key)
    {
      int defaultValue = 0;
      return EditorPrefs.GetInt(key, defaultValue);
    }

    /// <summary>
    ///   <para>Sets the value of the preference identified by key.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetFloat(string key, float value);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float GetFloat(string key, [DefaultValue("0.0F")] float defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public static float GetFloat(string key)
    {
      float defaultValue = 0.0f;
      return EditorPrefs.GetFloat(key, defaultValue);
    }

    /// <summary>
    ///   <para>Sets the value of the preference identified by key.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetString(string key, string value);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetString(string key, [DefaultValue("\"\"")] string defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public static string GetString(string key)
    {
      string empty = string.Empty;
      return EditorPrefs.GetString(key, empty);
    }

    /// <summary>
    ///   <para>Sets the value of the preference identified by key.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetBool(string key, bool value);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetBool(string key, [DefaultValue("false")] bool defaultValue);

    /// <summary>
    ///   <para>Returns the value corresponding to key in the preference file if it exists.</para>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    [ExcludeFromDocs]
    public static bool GetBool(string key)
    {
      bool defaultValue = false;
      return EditorPrefs.GetBool(key, defaultValue);
    }

    /// <summary>
    ///   <para>Returns true if key exists in the preferences.</para>
    /// </summary>
    /// <param name="key"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasKey(string key);

    /// <summary>
    ///   <para>Removes key and its corresponding value from the preferences.</para>
    /// </summary>
    /// <param name="key"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DeleteKey(string key);

    /// <summary>
    ///   <para>Removes all keys and values from the preferences. Use with caution.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DeleteAll();
  }
}

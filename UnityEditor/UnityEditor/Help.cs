// Decompiled with JetBrains decompiler
// Type: UnityEditor.Help
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Helper class to access Unity documentation.</para>
  /// </summary>
  public sealed class Help
  {
    /// <summary>
    ///   <para>Is there a help page for this object?</para>
    /// </summary>
    /// <param name="obj"></param>
    public static bool HasHelpForObject(Object obj)
    {
      return Help.HasHelpForObject(obj, true);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasHelpForObject(Object obj, bool defaultToMonoBehaviour);

    internal static string GetNiceHelpNameForObject(Object obj)
    {
      return Help.GetNiceHelpNameForObject(obj, true);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetNiceHelpNameForObject(Object obj, bool defaultToMonoBehaviour);

    /// <summary>
    ///   <para>Get the URL for this object's documentation.</para>
    /// </summary>
    /// <param name="obj">The object to retrieve documentation for.</param>
    /// <returns>
    ///   <para>The documentation URL for the object. Note that this could use the http: or file: schemas.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetHelpURLForObject(Object obj);

    /// <summary>
    ///   <para>Show help page for this object.</para>
    /// </summary>
    /// <param name="obj"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ShowHelpForObject(Object obj);

    /// <summary>
    ///   <para>Show a help page.</para>
    /// </summary>
    /// <param name="page"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ShowHelpPage(string page);

    /// <summary>
    ///   <para>Open url in the default web browser.</para>
    /// </summary>
    /// <param name="url"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void BrowseURL(string url);
  }
}

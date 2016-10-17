// Decompiled with JetBrains decompiler
// Type: UnityEditor.Menu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Menu class to manipulate the menu item.</para>
  /// </summary>
  public sealed class Menu
  {
    /// <summary>
    ///   <para>Set the check status of the given menu.</para>
    /// </summary>
    /// <param name="menuPath"></param>
    /// <param name="isChecked"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetChecked(string menuPath, bool isChecked);

    /// <summary>
    ///   <para>Get the check status of the given menu.</para>
    /// </summary>
    /// <param name="menuPath"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetChecked(string menuPath);
  }
}

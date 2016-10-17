// Decompiled with JetBrains decompiler
// Type: UnityEditor.OSColorPicker
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  internal sealed class OSColorPicker
  {
    public static extern bool visible { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static Color color
    {
      get
      {
        Color color;
        OSColorPicker.INTERNAL_get_color(out color);
        return color;
      }
      set
      {
        OSColorPicker.INTERNAL_set_color(ref value);
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Show(bool showAlpha);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Close();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_color(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_color(ref Color value);
  }
}

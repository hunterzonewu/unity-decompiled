// Decompiled with JetBrains decompiler
// Type: UnityEngine.Tizen.Window
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Tizen
{
  /// <summary>
  ///   <para>Interface into Tizen specific functionality.</para>
  /// </summary>
  public sealed class Window
  {
    /// <summary>
    ///   <para>Get pointer to the native window handle.</para>
    /// </summary>
    public static extern IntPtr windowHandle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEngine.AndroidInput
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>AndroidInput provides support for off-screen touch input, such as a touchpad.</para>
  /// </summary>
  public sealed class AndroidInput
  {
    /// <summary>
    ///   <para>Number of secondary touches. Guaranteed not to change throughout the frame. (Read Only).</para>
    /// </summary>
    public static extern int touchCountSecondary { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Property indicating whether the system provides secondary touch input.</para>
    /// </summary>
    public static extern bool secondaryTouchEnabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Property indicating the width of the secondary touchpad.</para>
    /// </summary>
    public static extern int secondaryTouchWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Property indicating the height of the secondary touchpad.</para>
    /// </summary>
    public static extern int secondaryTouchHeight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    private AndroidInput()
    {
    }

    /// <summary>
    ///   <para>Returns object representing status of a specific touch on a secondary touchpad (Does not allocate temporary variables).</para>
    /// </summary>
    /// <param name="index"></param>
    public static Touch GetSecondaryTouch(int index)
    {
      Touch touch;
      AndroidInput.INTERNAL_CALL_GetSecondaryTouch(index, out touch);
      return touch;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSecondaryTouch(int index, out Touch value);
  }
}

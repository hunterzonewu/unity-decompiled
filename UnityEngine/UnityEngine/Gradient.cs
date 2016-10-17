// Decompiled with JetBrains decompiler
// Type: UnityEngine.Gradient
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
  ///   <para>Gradient used for animating colors.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class Gradient
  {
    internal IntPtr m_Ptr;

    /// <summary>
    ///   <para>All color keys defined in the gradient.</para>
    /// </summary>
    public GradientColorKey[] colorKeys { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>All alpha keys defined in the gradient.</para>
    /// </summary>
    public GradientAlphaKey[] alphaKeys { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal Color constantColor
    {
      get
      {
        Color color;
        this.INTERNAL_get_constantColor(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_constantColor(ref value);
      }
    }

    /// <summary>
    ///   <para>Create a new Gradient object.</para>
    /// </summary>
    [RequiredByNativeCode]
    public Gradient()
    {
      this.Init();
    }

    ~Gradient()
    {
      this.Cleanup();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Init();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Cleanup();

    /// <summary>
    ///   <para>Calculate color at a given time.</para>
    /// </summary>
    /// <param name="time">Time of the key (0 - 1).</param>
    public Color Evaluate(float time)
    {
      Color color;
      Gradient.INTERNAL_CALL_Evaluate(this, time, out color);
      return color;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Evaluate(Gradient self, float time, out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_constantColor(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_constantColor(ref Color value);

    /// <summary>
    ///   <para>Setup Gradient with an array of color keys and alpha keys.</para>
    /// </summary>
    /// <param name="colorKeys">Color keys of the gradient (maximum 8 color keys).</param>
    /// <param name="alphaKeys">Alpha keys of the gradient (maximum 8 alpha keys).</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys);
  }
}

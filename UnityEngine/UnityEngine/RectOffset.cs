// Decompiled with JetBrains decompiler
// Type: UnityEngine.RectOffset
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
  ///   <para>Offsets for rectangles, borders, etc.</para>
  /// </summary>
  [UsedByNativeCode]
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class RectOffset
  {
    [NonSerialized]
    internal IntPtr m_Ptr;
    private readonly GUIStyle m_SourceStyle;

    /// <summary>
    ///   <para>Left edge size.</para>
    /// </summary>
    public int left { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Right edge size.</para>
    /// </summary>
    public int right { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Top edge size.</para>
    /// </summary>
    public int top { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Bottom edge size.</para>
    /// </summary>
    public int bottom { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Shortcut for left + right. (Read Only)</para>
    /// </summary>
    public int horizontal { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Shortcut for top + bottom. (Read Only)</para>
    /// </summary>
    public int vertical { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Creates a new rectangle with offsets.</para>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="top"></param>
    /// <param name="bottom"></param>
    public RectOffset()
    {
      this.Init();
    }

    internal RectOffset(GUIStyle sourceStyle, IntPtr source)
    {
      this.m_SourceStyle = sourceStyle;
      this.m_Ptr = source;
    }

    /// <summary>
    ///   <para>Creates a new rectangle with offsets.</para>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="top"></param>
    /// <param name="bottom"></param>
    public RectOffset(int left, int right, int top, int bottom)
    {
      this.Init();
      this.left = left;
      this.right = right;
      this.top = top;
      this.bottom = bottom;
    }

    ~RectOffset()
    {
      if (this.m_SourceStyle != null)
        return;
      this.Cleanup();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Init();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Cleanup();

    /// <summary>
    ///   <para>Add the border offsets to a rect.</para>
    /// </summary>
    /// <param name="rect"></param>
    public Rect Add(Rect rect)
    {
      Rect rect1;
      RectOffset.INTERNAL_CALL_Add(this, ref rect, out rect1);
      return rect1;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Add(RectOffset self, ref Rect rect, out Rect value);

    /// <summary>
    ///   <para>Remove the border offsets from a rect.</para>
    /// </summary>
    /// <param name="rect"></param>
    public Rect Remove(Rect rect)
    {
      Rect rect1;
      RectOffset.INTERNAL_CALL_Remove(this, ref rect, out rect1);
      return rect1;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Remove(RectOffset self, ref Rect rect, out Rect value);

    public override string ToString()
    {
      return UnityString.Format("RectOffset (l:{0} r:{1} t:{2} b:{3})", (object) this.left, (object) this.right, (object) this.top, (object) this.bottom);
    }
  }
}

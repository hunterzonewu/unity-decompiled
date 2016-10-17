// Decompiled with JetBrains decompiler
// Type: UnityEngine.Sprites.DataUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine.Sprites
{
  /// <summary>
  ///   <para>Helper utilities for accessing Sprite data.</para>
  /// </summary>
  public sealed class DataUtility
  {
    /// <summary>
    ///   <para>Inner UV's of the Sprite.</para>
    /// </summary>
    /// <param name="sprite"></param>
    public static Vector4 GetInnerUV(Sprite sprite)
    {
      Vector4 vector4;
      DataUtility.INTERNAL_CALL_GetInnerUV(sprite, out vector4);
      return vector4;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetInnerUV(Sprite sprite, out Vector4 value);

    /// <summary>
    ///   <para>Outer UV's of the Sprite.</para>
    /// </summary>
    /// <param name="sprite"></param>
    public static Vector4 GetOuterUV(Sprite sprite)
    {
      Vector4 vector4;
      DataUtility.INTERNAL_CALL_GetOuterUV(sprite, out vector4);
      return vector4;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetOuterUV(Sprite sprite, out Vector4 value);

    /// <summary>
    ///   <para>Return the padding on the sprite.</para>
    /// </summary>
    /// <param name="sprite"></param>
    public static Vector4 GetPadding(Sprite sprite)
    {
      Vector4 vector4;
      DataUtility.INTERNAL_CALL_GetPadding(sprite, out vector4);
      return vector4;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetPadding(Sprite sprite, out Vector4 value);

    /// <summary>
    ///   <para>Minimum width and height of the Sprite.</para>
    /// </summary>
    /// <param name="sprite"></param>
    public static Vector2 GetMinSize(Sprite sprite)
    {
      Vector2 output;
      DataUtility.Internal_GetMinSize(sprite, out output);
      return output;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_GetMinSize(Sprite sprite, out Vector2 output);
  }
}

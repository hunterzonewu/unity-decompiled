// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimationCurve
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
  ///   <para>A collection of curves form an AnimationClip.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class AnimationCurve
  {
    internal IntPtr m_Ptr;

    /// <summary>
    ///   <para>All keys defined in the animation curve.</para>
    /// </summary>
    public Keyframe[] keys
    {
      get
      {
        return this.GetKeys();
      }
      set
      {
        this.SetKeys(value);
      }
    }

    public Keyframe this[int index]
    {
      get
      {
        return this.GetKey_Internal(index);
      }
    }

    /// <summary>
    ///   <para>The number of keys in the curve. (Read Only)</para>
    /// </summary>
    public int length { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The behaviour of the animation before the first keyframe.</para>
    /// </summary>
    public WrapMode preWrapMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The behaviour of the animation after the last keyframe.</para>
    /// </summary>
    public WrapMode postWrapMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Creates an animation curve from arbitrary number of keyframes.</para>
    /// </summary>
    /// <param name="keys"></param>
    public AnimationCurve(params Keyframe[] keys)
    {
      this.Init(keys);
    }

    /// <summary>
    ///   <para>Creates an empty animation curve.</para>
    /// </summary>
    [RequiredByNativeCode]
    public AnimationCurve()
    {
      this.Init((Keyframe[]) null);
    }

    ~AnimationCurve()
    {
      this.Cleanup();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Cleanup();

    /// <summary>
    ///   <para>Evaluate the curve at time.</para>
    /// </summary>
    /// <param name="time">The time within the curve you want to evaluate (the horizontal axis in the curve graph).</param>
    /// <returns>
    ///   <para>The value of the curve, at the point in time specified.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float Evaluate(float time);

    /// <summary>
    ///   <para>Add a new key to the curve.</para>
    /// </summary>
    /// <param name="time">The time at which to add the key (horizontal axis in the curve graph).</param>
    /// <param name="value">The value for the key (vertical axis in the curve graph).</param>
    /// <returns>
    ///   <para>The index of the added key, or -1 if the key could not be added.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int AddKey(float time, float value);

    /// <summary>
    ///   <para>Add a new key to the curve.</para>
    /// </summary>
    /// <param name="key">The key to add to the curve.</param>
    /// <returns>
    ///   <para>The index of the added key, or -1 if the key could not be added.</para>
    /// </returns>
    public int AddKey(Keyframe key)
    {
      return this.AddKey_Internal(key);
    }

    private int AddKey_Internal(Keyframe key)
    {
      return AnimationCurve.INTERNAL_CALL_AddKey_Internal(this, ref key);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int INTERNAL_CALL_AddKey_Internal(AnimationCurve self, ref Keyframe key);

    /// <summary>
    ///   <para>Removes the keyframe at index and inserts key.</para>
    /// </summary>
    /// <param name="index">The index of the key to move.</param>
    /// <param name="key">The key (with its new time) to insert.</param>
    /// <returns>
    ///   <para>The index of the keyframe after moving it.</para>
    /// </returns>
    public int MoveKey(int index, Keyframe key)
    {
      return AnimationCurve.INTERNAL_CALL_MoveKey(this, index, ref key);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int INTERNAL_CALL_MoveKey(AnimationCurve self, int index, ref Keyframe key);

    /// <summary>
    ///   <para>Removes a key.</para>
    /// </summary>
    /// <param name="index">The index of the key to remove.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void RemoveKey(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetKeys(Keyframe[] keys);

    private Keyframe GetKey_Internal(int index)
    {
      Keyframe keyframe;
      AnimationCurve.INTERNAL_CALL_GetKey_Internal(this, index, out keyframe);
      return keyframe;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetKey_Internal(AnimationCurve self, int index, out Keyframe value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Keyframe[] GetKeys();

    /// <summary>
    ///   <para>Smooth the in and out tangents of the keyframe at index.</para>
    /// </summary>
    /// <param name="index">The index of the keyframe to be smoothed.</param>
    /// <param name="weight">The smoothing weight to apply to the keyframe's tangents.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SmoothTangents(int index, float weight);

    /// <summary>
    ///   <para>A straight Line starting at timeStart, valueStart and ending at timeEnd, valueEnd.</para>
    /// </summary>
    /// <param name="timeStart">The start time for the linear curve.</param>
    /// <param name="valueStart">The start value for the linear curve.</param>
    /// <param name="timeEnd">The end time for the linear curve.</param>
    /// <param name="valueEnd">The end value for the linear curve.</param>
    /// <returns>
    ///   <para>The (straight) curve created from the values specified.</para>
    /// </returns>
    public static AnimationCurve Linear(float timeStart, float valueStart, float timeEnd, float valueEnd)
    {
      float num = (float) (((double) valueEnd - (double) valueStart) / ((double) timeEnd - (double) timeStart));
      return new AnimationCurve(new Keyframe[2]{ new Keyframe(timeStart, valueStart, 0.0f, num), new Keyframe(timeEnd, valueEnd, num, 0.0f) });
    }

    /// <summary>
    ///   <para>Creates an ease-in and out curve starting at timeStart, valueStart and ending at timeEnd, valueEnd.</para>
    /// </summary>
    /// <param name="timeStart">The start time for the ease curve.</param>
    /// <param name="valueStart">The start value for the ease curve.</param>
    /// <param name="timeEnd">The end time for the ease curve.</param>
    /// <param name="valueEnd">The end value for the ease curve.</param>
    /// <returns>
    ///   <para>The ease-in and out curve generated from the specified values.</para>
    /// </returns>
    public static AnimationCurve EaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
    {
      return new AnimationCurve(new Keyframe[2]{ new Keyframe(timeStart, valueStart, 0.0f, 0.0f), new Keyframe(timeEnd, valueEnd, 0.0f, 0.0f) });
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Init(Keyframe[] keys);
  }
}

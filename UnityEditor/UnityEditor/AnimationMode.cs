// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>AnimationMode is used by the AnimationWindow to store properties modifed by the AnimationClip playback.</para>
  /// </summary>
  public sealed class AnimationMode
  {
    private static bool s_InAnimationPlaybackMode = false;
    private static Color s_AnimatedPropertyColorLight = new Color(1f, 0.65f, 0.6f, 1f);
    private static Color s_AnimatedPropertyColorDark = new Color(1f, 0.55f, 0.5f, 1f);

    /// <summary>
    ///   <para>The color used to show that a property is currently being animated.</para>
    /// </summary>
    public static Color animatedPropertyColor
    {
      get
      {
        if (EditorGUIUtility.isProSkin)
          return AnimationMode.s_AnimatedPropertyColorDark;
        return AnimationMode.s_AnimatedPropertyColorLight;
      }
    }

    /// <summary>
    ///   <para>Is the specified property currently in animation mode and being animated?</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="propertyPath"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsPropertyAnimated(Object target, string propertyPath);

    /// <summary>
    ///   <para>Stops Animation mode, reverts all properties that were animated in animation mode.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void StopAnimationMode();

    /// <summary>
    ///   <para>Are we currently in AnimationMode.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool InAnimationMode();

    /// <summary>
    ///   <para>Starts the animation mode.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void StartAnimationMode();

    internal static void StopAnimationPlaybackMode()
    {
      AnimationMode.s_InAnimationPlaybackMode = false;
    }

    internal static bool InAnimationPlaybackMode()
    {
      return AnimationMode.s_InAnimationPlaybackMode;
    }

    internal static void StartAnimationPlaybackMode()
    {
      AnimationMode.s_InAnimationPlaybackMode = true;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void BeginSampling();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void EndSampling();

    /// <summary>
    ///   <para>Samples an AnimationClip on the object and also records any modified properties in AnimationMode.</para>
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="clip"></param>
    /// <param name="time"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SampleAnimationClip(GameObject gameObject, AnimationClip clip, float time);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void AddPropertyModification(EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride);
  }
}

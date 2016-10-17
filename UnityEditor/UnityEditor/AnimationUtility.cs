// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Editor utility functions for modifying animation clips.</para>
  /// </summary>
  public sealed class AnimationUtility
  {
    /// <summary>
    ///   <para>Triggered when an animation curve inside an animation clip has been modified.</para>
    /// </summary>
    public static AnimationUtility.OnCurveWasModified onCurveWasModified;

    /// <summary>
    ///   <para>Returns the array of AnimationClips that are referenced in the Animation component.</para>
    /// </summary>
    /// <param name="component"></param>
    [Obsolete("GetAnimationClips(Animation) is deprecated. Use GetAnimationClips(GameObject) instead.")]
    public static AnimationClip[] GetAnimationClips(Animation component)
    {
      return AnimationUtility.GetAnimationClips(component.gameObject);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AnimationClip[] GetAnimationClips(GameObject gameObject);

    /// <summary>
    ///   <para>Sets the array of AnimationClips to be referenced in the Animation component.</para>
    /// </summary>
    /// <param name="animation"></param>
    /// <param name="clips"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetAnimationClips(Animation animation, AnimationClip[] clips);

    /// <summary>
    ///   <para>Returns all the animatable bindings that a specific game object has.</para>
    /// </summary>
    /// <param name="targetObject"></param>
    /// <param name="root"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern EditorCurveBinding[] GetAnimatableBindings(GameObject targetObject, GameObject root);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetFloatValue(GameObject root, EditorCurveBinding binding, out float data);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern System.Type GetEditorCurveValueType(GameObject root, EditorCurveBinding binding);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetObjectReferenceValue(GameObject root, EditorCurveBinding binding, out UnityEngine.Object targetObject);

    /// <summary>
    ///   <para>Returns the animated object that the binding is pointing to.</para>
    /// </summary>
    /// <param name="root"></param>
    /// <param name="binding"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object GetAnimatedObject(GameObject root, EditorCurveBinding binding);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern System.Type PropertyModificationToEditorCurveBinding(PropertyModification modification, GameObject gameObject, out EditorCurveBinding binding);

    /// <summary>
    ///   <para>Returns all the float curve bindings currently stored in the clip.</para>
    /// </summary>
    /// <param name="clip"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern EditorCurveBinding[] GetCurveBindings(AnimationClip clip);

    /// <summary>
    ///   <para>Returns all the object reference curve bindings currently stored in the clip.</para>
    /// </summary>
    /// <param name="clip"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern EditorCurveBinding[] GetObjectReferenceCurveBindings(AnimationClip clip);

    /// <summary>
    ///   <para>Return the object reference curve that the binding is pointing to.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="binding"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ObjectReferenceKeyframe[] GetObjectReferenceCurve(AnimationClip clip, EditorCurveBinding binding);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetObjectReferenceCurve(AnimationClip clip, EditorCurveBinding binding, ObjectReferenceKeyframe[] keyframes);

    /// <summary>
    ///   <para>Return the float curve that the binding is pointing to.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="relativePath"></param>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <param name="binding"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AnimationCurve GetEditorCurve(AnimationClip clip, EditorCurveBinding binding);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetEditorCurve(AnimationClip clip, EditorCurveBinding binding, AnimationCurve curve);

    [RequiredByNativeCode]
    private static void Internal_CallAnimationClipAwake(AnimationClip clip)
    {
      if (AnimationUtility.onCurveWasModified == null)
        return;
      AnimationUtility.onCurveWasModified(clip, new EditorCurveBinding(), AnimationUtility.CurveModifiedType.ClipModified);
    }

    /// <summary>
    ///   <para>Adds, modifies or removes an editor float curve in a given clip.</para>
    /// </summary>
    /// <param name="clip">The animation clip to which the curve will be added.</param>
    /// <param name="binding">The bindings which defines the path and the property of the curve.</param>
    /// <param name="curve">The curve to add. Setting this to null will remove the curve.</param>
    public static void SetEditorCurve(AnimationClip clip, EditorCurveBinding binding, AnimationCurve curve)
    {
      AnimationUtility.Internal_SetEditorCurve(clip, binding, curve);
      if (AnimationUtility.onCurveWasModified == null)
        return;
      AnimationUtility.onCurveWasModified(clip, binding, curve == null ? AnimationUtility.CurveModifiedType.CurveDeleted : AnimationUtility.CurveModifiedType.CurveModified);
    }

    /// <summary>
    ///   <para>Adds, modifies or removes an object reference curve in a given clip.</para>
    /// </summary>
    /// <param name="keyframes">Setting this to null will remove the curve.</param>
    /// <param name="clip"></param>
    /// <param name="binding"></param>
    public static void SetObjectReferenceCurve(AnimationClip clip, EditorCurveBinding binding, ObjectReferenceKeyframe[] keyframes)
    {
      AnimationUtility.Internal_SetObjectReferenceCurve(clip, binding, keyframes);
      if (AnimationUtility.onCurveWasModified == null)
        return;
      AnimationUtility.onCurveWasModified(clip, binding, keyframes == null ? AnimationUtility.CurveModifiedType.CurveDeleted : AnimationUtility.CurveModifiedType.CurveModified);
    }

    /// <summary>
    ///   <para>Retrieves all curves from a specific animation clip.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="includeCurveData"></param>
    [ExcludeFromDocs]
    [Obsolete("GetAllCurves is deprecated. Use GetCurveBindings and GetObjectReferenceCurveBindings instead.")]
    public static AnimationClipCurveData[] GetAllCurves(AnimationClip clip)
    {
      bool includeCurveData = true;
      return AnimationUtility.GetAllCurves(clip, includeCurveData);
    }

    /// <summary>
    ///   <para>Retrieves all curves from a specific animation clip.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="includeCurveData"></param>
    [Obsolete("GetAllCurves is deprecated. Use GetCurveBindings and GetObjectReferenceCurveBindings instead.")]
    public static AnimationClipCurveData[] GetAllCurves(AnimationClip clip, [DefaultValue("true")] bool includeCurveData)
    {
      EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
      AnimationClipCurveData[] animationClipCurveDataArray = new AnimationClipCurveData[curveBindings.Length];
      for (int index = 0; index < animationClipCurveDataArray.Length; ++index)
      {
        animationClipCurveDataArray[index] = new AnimationClipCurveData(curveBindings[index]);
        if (includeCurveData)
          animationClipCurveDataArray[index].curve = AnimationUtility.GetEditorCurve(clip, curveBindings[index]);
      }
      return animationClipCurveDataArray;
    }

    [Obsolete("This overload is deprecated. Use the one with EditorCurveBinding instead.")]
    public static bool GetFloatValue(GameObject root, string relativePath, System.Type type, string propertyName, out float data)
    {
      return AnimationUtility.GetFloatValue(root, EditorCurveBinding.FloatCurve(relativePath, type, propertyName), out data);
    }

    [Obsolete("This overload is deprecated. Use the one with EditorCurveBinding instead.")]
    public static void SetEditorCurve(AnimationClip clip, string relativePath, System.Type type, string propertyName, AnimationCurve curve)
    {
      AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(relativePath, type, propertyName), curve);
    }

    /// <summary>
    ///   <para>Return the float curve that the binding is pointing to.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="relativePath"></param>
    /// <param name="type"></param>
    /// <param name="propertyName"></param>
    /// <param name="binding"></param>
    [Obsolete("This overload is deprecated. Use the one with EditorCurveBinding instead.")]
    public static AnimationCurve GetEditorCurve(AnimationClip clip, string relativePath, System.Type type, string propertyName)
    {
      return AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(relativePath, type, propertyName));
    }

    /// <summary>
    ///   <para>Retrieves all animation events associated with the animation clip.</para>
    /// </summary>
    /// <param name="clip"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AnimationEvent[] GetAnimationEvents(AnimationClip clip);

    /// <summary>
    ///   <para>Replaces all animation events in the animation clip.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="events"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetAnimationEvents(AnimationClip clip, AnimationEvent[] events);

    /// <summary>
    ///   <para>Calculates path from root transform to target transform.</para>
    /// </summary>
    /// <param name="targetTransform"></param>
    /// <param name="root"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string CalculateTransformPath(Transform targetTransform, Transform root);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AnimationClipSettings GetAnimationClipSettings(AnimationClip clip);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetAnimationClipSettings(AnimationClip clip, AnimationClipSettings srcClipInfo);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetAnimationClipSettingsNoDirty(AnimationClip clip, AnimationClipSettings srcClipInfo);

    /// <summary>
    ///   <para>Set the additive reference pose from referenceClip at time for animation clip clip.</para>
    /// </summary>
    /// <param name="clip">The animation clip to be used.</param>
    /// <param name="referenceClip">The animation clip containing the reference pose.</param>
    /// <param name="time">Time that defines the reference pose in referenceClip.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetAdditiveReferencePose(AnimationClip clip, AnimationClip referenceClip, float time);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsValidPolynomialCurve(AnimationCurve curve);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ConstrainToPolynomialCurve(AnimationCurve curve);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CurveSupportsProcedural(AnimationCurve curve);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern AnimationClipStats GetAnimationClipStats(AnimationClip clip);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetGenerateMotionCurves(AnimationClip clip);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetGenerateMotionCurves(AnimationClip clip, bool value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasGenericRootTransform(AnimationClip clip);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasMotionFloatCurves(AnimationClip clip);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasMotionCurves(AnimationClip clip);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasRootCurves(AnimationClip clip);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool AmbiguousBinding(string path, int classID, Transform root);

    [Obsolete("Use AnimationMode.InAnimationMode instead")]
    public static bool InAnimationMode()
    {
      return AnimationMode.InAnimationMode();
    }

    [Obsolete("Use AnimationMode.StartAnimationmode instead")]
    public static void StartAnimationMode(UnityEngine.Object[] objects)
    {
      Debug.LogWarning((object) "AnimationUtility.StartAnimationMode is deprecated. Use AnimationMode.StartAnimationMode with the new APIs. The objects passed to this function will no longer be reverted automatically. See AnimationMode.AddPropertyModification");
      AnimationMode.StartAnimationMode();
    }

    [Obsolete("Use AnimationMode.StopAnimationMode instead")]
    public static void StopAnimationMode()
    {
      AnimationMode.StopAnimationMode();
    }

    [Obsolete("SetAnimationType is no longer supported", true)]
    public static void SetAnimationType(AnimationClip clip, ModelImporterAnimationType type)
    {
    }

    /// <summary>
    ///   <para>Describes the type of modification that caused OnCurveWasModified to fire.</para>
    /// </summary>
    public enum CurveModifiedType
    {
      CurveDeleted,
      CurveModified,
      ClipModified,
    }

    /// <summary>
    ///   <para>Triggered when an animation curve inside an animation clip has been modified.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="binding"></param>
    /// <param name="deleted"></param>
    public delegate void OnCurveWasModified(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType deleted);
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationWindowStyles
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class AnimationWindowStyles
  {
    public static Texture2D pointIcon = EditorGUIUtility.LoadIcon("animationkeyframe");
    public static GUIContent playContent = EditorGUIUtility.IconContent("Animation.Play", "|Play the animation clip, looping in the shown range.");
    public static GUIContent recordContent = EditorGUIUtility.IconContent("Animation.Record", "|Are scene and inspector changes recorded into the animation curves?");
    public static GUIContent prevKeyContent = EditorGUIUtility.IconContent("Animation.PrevKey", "|Go to previous key frame.");
    public static GUIContent nextKeyContent = EditorGUIUtility.IconContent("Animation.NextKey", "|Go to next key frame.");
    public static GUIContent addKeyframeContent = EditorGUIUtility.IconContent("Animation.AddKeyframe", "|Add Keyframe.");
    public static GUIContent addEventContent = EditorGUIUtility.IconContent("Animation.AddEvent", "|Add Event.");
    public static GUIContent noAnimatableObjectSelectedText = EditorGUIUtility.TextContent("No animatable object selected.");
    public static GUIContent formatIsMissing = EditorGUIUtility.TextContent("To begin animating {0}, create {1}.");
    public static GUIContent animatorAndAnimationClip = EditorGUIUtility.TextContent("an Animator and an Animation Clip");
    public static GUIContent animationClip = EditorGUIUtility.TextContent("an Animation Clip");
    public static GUIContent create = EditorGUIUtility.TextContent("Create");
    public static GUIContent dopesheet = EditorGUIUtility.TextContent("Dopesheet");
    public static GUIContent curves = EditorGUIUtility.TextContent("Curves");
    public static GUIContent samples = EditorGUIUtility.TextContent("Samples");
    public static GUIContent createNewClip = EditorGUIUtility.TextContent("Create New Clip...");
    public static GUIContent animatorOptimizedText = EditorGUIUtility.TextContent("Editing and playback of animations on optimized game object hierarchy is not supported.\nPlease select a game object that does not have 'Optimize Game Objects' applied.");
    public static GUIStyle curveEditorBackground = (GUIStyle) "AnimationCurveEditorBackground";
    public static GUIStyle eventBackground = (GUIStyle) "AnimationEventBackground";
    public static GUIStyle keyframeBackground = (GUIStyle) "AnimationKeyframeBackground";
    public static GUIStyle rowOdd = (GUIStyle) "AnimationRowEven";
    public static GUIStyle rowEven = (GUIStyle) "AnimationRowOdd";
    public static GUIStyle TimelineTick = (GUIStyle) "AnimationTimelineTick";
    public static GUIStyle miniToolbar = new GUIStyle(EditorStyles.toolbar);
    public static GUIStyle miniToolbarButton = new GUIStyle(EditorStyles.toolbarButton);
    public static GUIStyle toolbarLabel = new GUIStyle(EditorStyles.toolbarPopup);

    public static void Initialize()
    {
      AnimationWindowStyles.toolbarLabel.normal.background = (Texture2D) null;
      AnimationWindowStyles.miniToolbarButton.padding.top = 0;
      AnimationWindowStyles.miniToolbarButton.padding.bottom = 3;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace UnityEditorInternal
{
  internal static class AnimationWindowUtility
  {
    internal static string s_LastPathUsedForNewClip;

    public static void CreateDefaultCurves(AnimationWindowState state, EditorCurveBinding[] properties)
    {
      AnimationClip activeAnimationClip = state.activeAnimationClip;
      GameObject activeRootGameObject = state.activeRootGameObject;
      properties = RotationCurveInterpolation.ConvertRotationPropertiesToDefaultInterpolation(state.activeAnimationClip, properties);
      foreach (EditorCurveBinding property in properties)
        state.SaveCurve(AnimationWindowUtility.CreateDefaultCurve(activeAnimationClip, activeRootGameObject, property));
    }

    public static AnimationWindowCurve CreateDefaultCurve(AnimationClip clip, GameObject rootGameObject, EditorCurveBinding binding)
    {
      System.Type editorCurveValueType = CurveBindingUtility.GetEditorCurveValueType(rootGameObject, binding);
      AnimationWindowCurve curve = new AnimationWindowCurve(clip, binding, editorCurveValueType);
      object currentValue = CurveBindingUtility.GetCurrentValue(rootGameObject, binding);
      if ((double) clip.length == 0.0)
      {
        AnimationWindowUtility.AddKeyframeToCurve(curve, currentValue, editorCurveValueType, AnimationKeyTime.Time(0.0f, clip.frameRate));
      }
      else
      {
        AnimationWindowUtility.AddKeyframeToCurve(curve, currentValue, editorCurveValueType, AnimationKeyTime.Time(0.0f, clip.frameRate));
        AnimationWindowUtility.AddKeyframeToCurve(curve, currentValue, editorCurveValueType, AnimationKeyTime.Time(clip.length, clip.frameRate));
      }
      return curve;
    }

    public static bool ShouldShowAnimationWindowCurve(EditorCurveBinding curveBinding)
    {
      if (AnimationWindowUtility.IsTransformType(curveBinding.type))
        return !curveBinding.propertyName.EndsWith(".w");
      return true;
    }

    public static bool IsNodeLeftOverCurve(AnimationWindowHierarchyNode node, GameObject rootGameObject)
    {
      if ((UnityEngine.Object) rootGameObject == (UnityEngine.Object) null)
        return false;
      if (node.binding.HasValue)
        return AnimationUtility.GetEditorCurveValueType(rootGameObject, node.binding.Value) == null;
      if (node.hasChildren)
      {
        using (List<TreeViewItem>.Enumerator enumerator = node.children.GetEnumerator())
        {
          if (enumerator.MoveNext())
            return AnimationWindowUtility.IsNodeLeftOverCurve(enumerator.Current as AnimationWindowHierarchyNode, rootGameObject);
        }
      }
      return false;
    }

    public static bool IsNodeAmbiguous(AnimationWindowHierarchyNode node, GameObject rootGameObject)
    {
      if ((UnityEngine.Object) rootGameObject == (UnityEngine.Object) null)
        return false;
      if (node.binding.HasValue)
        return AnimationUtility.AmbiguousBinding(node.binding.Value.path, node.binding.Value.m_ClassID, rootGameObject.transform);
      if (node.hasChildren)
      {
        using (List<TreeViewItem>.Enumerator enumerator = node.children.GetEnumerator())
        {
          if (enumerator.MoveNext())
            return AnimationWindowUtility.IsNodeAmbiguous(enumerator.Current as AnimationWindowHierarchyNode, rootGameObject);
        }
      }
      return false;
    }

    public static void AddSelectedKeyframes(AnimationWindowState state, AnimationKeyTime time)
    {
      if (state.activeCurves.Count > 0)
      {
        using (List<AnimationWindowCurve>.Enumerator enumerator = state.activeCurves.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnimationWindowCurve current = enumerator.Current;
            AnimationWindowUtility.AddKeyframeToCurve(state, current, time);
          }
        }
      }
      else
      {
        using (List<AnimationWindowCurve>.Enumerator enumerator = state.allCurves.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnimationWindowCurve current = enumerator.Current;
            AnimationWindowUtility.AddKeyframeToCurve(state, current, time);
          }
        }
      }
    }

    public static AnimationWindowKeyframe AddKeyframeToCurve(AnimationWindowState state, AnimationWindowCurve curve, AnimationKeyTime time)
    {
      object currentValue = CurveBindingUtility.GetCurrentValue(state.activeRootGameObject, curve.binding);
      System.Type editorCurveValueType = CurveBindingUtility.GetEditorCurveValueType(state.activeRootGameObject, curve.binding);
      AnimationWindowKeyframe curve1 = AnimationWindowUtility.AddKeyframeToCurve(curve, currentValue, editorCurveValueType, time);
      state.SaveCurve(curve);
      return curve1;
    }

    public static AnimationWindowKeyframe AddKeyframeToCurve(AnimationWindowCurve curve, object value, System.Type type, AnimationKeyTime time)
    {
      AnimationWindowKeyframe keyAtTime = curve.FindKeyAtTime(time);
      if (keyAtTime != null)
      {
        keyAtTime.value = value;
        return keyAtTime;
      }
      AnimationWindowKeyframe key1 = new AnimationWindowKeyframe();
      key1.time = time.time;
      if (curve.isPPtrCurve)
      {
        key1.value = value;
        key1.curve = curve;
        curve.AddKeyframe(key1, time);
      }
      else if (type == typeof (bool) || type == typeof (float))
      {
        AnimationCurve animationCurve = curve.ToAnimationCurve();
        Keyframe key2 = new Keyframe(time.time, (float) value);
        if (type == typeof (bool))
        {
          CurveUtility.SetKeyTangentMode(ref key2, 0, TangentMode.Stepped);
          CurveUtility.SetKeyTangentMode(ref key2, 1, TangentMode.Stepped);
          CurveUtility.SetKeyBroken(ref key2, true);
          key1.m_TangentMode = key2.tangentMode;
          key1.m_InTangent = float.PositiveInfinity;
          key1.m_OutTangent = float.PositiveInfinity;
        }
        else
        {
          int keyIndex = animationCurve.AddKey(key2);
          if (keyIndex != -1)
          {
            CurveUtility.SetKeyModeFromContext(animationCurve, keyIndex);
            key1.m_TangentMode = animationCurve[keyIndex].tangentMode;
          }
        }
        key1.value = value;
        key1.curve = curve;
        curve.AddKeyframe(key1, time);
      }
      return key1;
    }

    public static List<AnimationWindowCurve> FilterCurves(AnimationWindowCurve[] curves, string path, bool entireHierarchy)
    {
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      foreach (AnimationWindowCurve curve in curves)
      {
        if (curve.path.Equals(path) || entireHierarchy && curve.path.Contains(path))
          animationWindowCurveList.Add(curve);
      }
      return animationWindowCurveList;
    }

    public static List<AnimationWindowCurve> FilterCurves(AnimationWindowCurve[] curves, string path, System.Type animatableObjectType)
    {
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      foreach (AnimationWindowCurve curve in curves)
      {
        if (curve.path.Equals(path) && curve.type == animatableObjectType)
          animationWindowCurveList.Add(curve);
      }
      return animationWindowCurveList;
    }

    public static bool IsCurveCreated(AnimationClip clip, EditorCurveBinding binding)
    {
      if (binding.isPPtrCurve)
        return AnimationUtility.GetObjectReferenceCurve(clip, binding) != null;
      if (AnimationWindowUtility.IsRectTransformPosition(binding))
        binding.propertyName = binding.propertyName.Replace(".x", ".z").Replace(".y", ".z");
      return AnimationUtility.GetEditorCurve(clip, binding) != null;
    }

    public static bool IsRectTransformPosition(EditorCurveBinding curveBinding)
    {
      if (curveBinding.type == typeof (RectTransform))
        return AnimationWindowUtility.GetPropertyGroupName(curveBinding.propertyName) == "m_LocalPosition";
      return false;
    }

    public static bool ContainsFloatKeyframes(List<AnimationWindowKeyframe> keyframes)
    {
      if (keyframes == null || keyframes.Count == 0)
        return false;
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = keyframes.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (!enumerator.Current.isPPtrCurve)
            return true;
        }
      }
      return false;
    }

    public static List<AnimationWindowCurve> FilterCurves(AnimationWindowCurve[] curves, string path, System.Type animatableObjectType, string propertyName)
    {
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      string propertyGroupName = AnimationWindowUtility.GetPropertyGroupName(propertyName);
      bool flag1 = propertyGroupName == propertyName;
      foreach (AnimationWindowCurve curve in curves)
      {
        bool flag2 = !flag1 ? curve.propertyName.Equals(propertyName) : AnimationWindowUtility.GetPropertyGroupName(curve.propertyName).Equals(propertyGroupName);
        if (curve.path.Equals(path) && curve.type == animatableObjectType && flag2)
          animationWindowCurveList.Add(curve);
      }
      return animationWindowCurveList;
    }

    public static object GetCurrentValue(GameObject rootGameObject, EditorCurveBinding curveBinding)
    {
      if (curveBinding.isPPtrCurve)
      {
        UnityEngine.Object targetObject;
        AnimationUtility.GetObjectReferenceValue(rootGameObject, curveBinding, out targetObject);
        return (object) targetObject;
      }
      float data;
      AnimationUtility.GetFloatValue(rootGameObject, curveBinding, out data);
      return (object) data;
    }

    public static List<EditorCurveBinding> GetAnimatableProperties(GameObject root, GameObject gameObject, System.Type valueType)
    {
      EditorCurveBinding[] animatableBindings = AnimationUtility.GetAnimatableBindings(root, gameObject);
      List<EditorCurveBinding> editorCurveBindingList = new List<EditorCurveBinding>();
      foreach (EditorCurveBinding binding in animatableBindings)
      {
        if (AnimationUtility.GetEditorCurveValueType(root, binding) == valueType)
          editorCurveBindingList.Add(binding);
      }
      return editorCurveBindingList;
    }

    public static List<EditorCurveBinding> GetAnimatableProperties(GameObject root, GameObject gameObject, System.Type objectType, System.Type valueType)
    {
      EditorCurveBinding[] animatableBindings = AnimationUtility.GetAnimatableBindings(root, gameObject);
      List<EditorCurveBinding> editorCurveBindingList = new List<EditorCurveBinding>();
      foreach (EditorCurveBinding binding in animatableBindings)
      {
        if (binding.type == objectType && AnimationUtility.GetEditorCurveValueType(root, binding) == valueType)
          editorCurveBindingList.Add(binding);
      }
      return editorCurveBindingList;
    }

    public static bool CurveExists(EditorCurveBinding binding, AnimationWindowCurve[] curves)
    {
      foreach (AnimationWindowCurve curve in curves)
      {
        if (binding.propertyName == curve.binding.propertyName && binding.type == curve.binding.type && binding.path == curve.binding.path)
          return true;
      }
      return false;
    }

    public static EditorCurveBinding GetRenamedBinding(EditorCurveBinding binding, string newPath)
    {
      return new EditorCurveBinding() { path = newPath, propertyName = binding.propertyName, type = binding.type };
    }

    public static void RenameCurvePath(AnimationWindowCurve curve, EditorCurveBinding newBinding, AnimationClip clip)
    {
      AnimationUtility.SetEditorCurve(clip, curve.binding, (AnimationCurve) null);
      AnimationUtility.SetEditorCurve(clip, newBinding, curve.ToAnimationCurve());
    }

    public static string GetPropertyDisplayName(string propertyName)
    {
      propertyName = propertyName.Replace("m_LocalPosition", "Position");
      propertyName = propertyName.Replace("m_LocalScale", "Scale");
      propertyName = propertyName.Replace("m_LocalRotation", "Rotation");
      propertyName = propertyName.Replace("localEulerAnglesBaked", "Rotation");
      propertyName = propertyName.Replace("localEulerAnglesRaw", "Rotation");
      propertyName = propertyName.Replace("localEulerAngles", "Rotation");
      propertyName = propertyName.Replace("m_Materials.Array.data", "Material Reference");
      propertyName = ObjectNames.NicifyVariableName(propertyName);
      propertyName = propertyName.Replace("m_", string.Empty);
      return propertyName;
    }

    public static bool ShouldPrefixWithTypeName(System.Type animatableObjectType, string propertyName)
    {
      return animatableObjectType != typeof (Transform) && animatableObjectType != typeof (RectTransform) && (animatableObjectType != typeof (SpriteRenderer) || !(propertyName == "m_Sprite"));
    }

    public static string GetNicePropertyDisplayName(System.Type animatableObjectType, string propertyName)
    {
      if (AnimationWindowUtility.ShouldPrefixWithTypeName(animatableObjectType, propertyName))
        return ObjectNames.NicifyVariableName(animatableObjectType.Name) + "." + AnimationWindowUtility.GetPropertyDisplayName(propertyName);
      return AnimationWindowUtility.GetPropertyDisplayName(propertyName);
    }

    public static string GetNicePropertyGroupDisplayName(System.Type animatableObjectType, string propertyGroupName)
    {
      if (AnimationWindowUtility.ShouldPrefixWithTypeName(animatableObjectType, propertyGroupName))
        return ObjectNames.NicifyVariableName(animatableObjectType.Name) + "." + AnimationWindowUtility.NicifyPropertyGroupName(animatableObjectType, propertyGroupName);
      return AnimationWindowUtility.NicifyPropertyGroupName(animatableObjectType, propertyGroupName);
    }

    public static string NicifyPropertyGroupName(System.Type animatableObjectType, string propertyGroupName)
    {
      string str = AnimationWindowUtility.GetPropertyGroupName(AnimationWindowUtility.GetPropertyDisplayName(propertyGroupName));
      if (animatableObjectType == typeof (RectTransform) & str.Equals("Position"))
        str = "Position (Z)";
      return str;
    }

    public static int GetComponentIndex(string name)
    {
      if (name.Length < 3 || (int) name[name.Length - 2] != 46)
        return -1;
      char ch = name[name.Length - 1];
      switch (ch)
      {
        case 'r':
          return 0;
        case 'w':
          return 3;
        case 'x':
          return 0;
        case 'y':
          return 1;
        case 'z':
          return 2;
        default:
          if ((int) ch == 97)
            return 3;
          if ((int) ch == 98)
            return 2;
          return (int) ch == 103 ? 1 : -1;
      }
    }

    public static string GetPropertyGroupName(string propertyName)
    {
      if (AnimationWindowUtility.GetComponentIndex(propertyName) != -1)
        return propertyName.Substring(0, propertyName.Length - 2);
      return propertyName;
    }

    public static float GetNextKeyframeTime(AnimationWindowCurve[] curves, float currentTime, float frameRate)
    {
      float num = float.MaxValue;
      float val2 = currentTime + 1f / frameRate;
      bool flag = false;
      foreach (AnimationWindowCurve curve in curves)
      {
        using (List<AnimationWindowKeyframe>.Enumerator enumerator = curve.m_Keyframes.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnimationWindowKeyframe current = enumerator.Current;
            if ((double) current.time < (double) num && (double) current.time > (double) currentTime)
            {
              num = Math.Max(current.time, val2);
              flag = true;
            }
          }
        }
      }
      if (flag)
        return num;
      return currentTime;
    }

    public static float GetPreviousKeyframeTime(AnimationWindowCurve[] curves, float currentTime, float frameRate)
    {
      float num = float.MinValue;
      float b = Mathf.Max(0.0f, currentTime - 1f / frameRate);
      bool flag = false;
      foreach (AnimationWindowCurve curve in curves)
      {
        using (List<AnimationWindowKeyframe>.Enumerator enumerator = curve.m_Keyframes.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnimationWindowKeyframe current = enumerator.Current;
            if ((double) current.time > (double) num && (double) current.time < (double) currentTime)
            {
              num = Mathf.Min(current.time, b);
              flag = true;
            }
          }
        }
      }
      if (flag)
        return num;
      return currentTime;
    }

    public static bool GameObjectIsAnimatable(GameObject gameObject, AnimationClip animationClip)
    {
      return !((UnityEngine.Object) gameObject == (UnityEngine.Object) null) && (gameObject.hideFlags & HideFlags.NotEditable) == HideFlags.None && !EditorUtility.IsPersistent((UnityEngine.Object) gameObject) && (!((UnityEngine.Object) animationClip != (UnityEngine.Object) null) || (animationClip.hideFlags & HideFlags.NotEditable) == HideFlags.None && AssetDatabase.IsOpenForEdit((UnityEngine.Object) animationClip));
    }

    public static bool InitializeGameobjectForAnimation(GameObject animatedObject)
    {
      Component componentInParents = AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(animatedObject.transform);
      if (!((UnityEngine.Object) componentInParents == (UnityEngine.Object) null))
        return AnimationWindowUtility.EnsureAnimationPlayerHasClip(componentInParents);
      AnimationClip newClip = AnimationWindowUtility.CreateNewClip(animatedObject.name);
      if ((UnityEngine.Object) newClip == (UnityEngine.Object) null)
        return false;
      Component animationPlayer = AnimationWindowUtility.EnsureActiveAnimationPlayer(animatedObject);
      bool animationPlayerComponent = AnimationWindowUtility.AddClipToAnimationPlayerComponent(animationPlayer, newClip);
      if (!animationPlayerComponent)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) animationPlayer);
      return animationPlayerComponent;
    }

    public static Component EnsureActiveAnimationPlayer(GameObject animatedObject)
    {
      Component componentInParents = AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(animatedObject.transform);
      if ((UnityEngine.Object) componentInParents == (UnityEngine.Object) null)
        return (Component) animatedObject.AddComponent<Animator>();
      return componentInParents;
    }

    private static bool EnsureAnimationPlayerHasClip(Component animationPlayer)
    {
      if ((UnityEngine.Object) animationPlayer == (UnityEngine.Object) null)
        return false;
      if (AnimationUtility.GetAnimationClips(animationPlayer.gameObject).Length > 0)
        return true;
      AnimationClip newClip = AnimationWindowUtility.CreateNewClip(animationPlayer.gameObject.name);
      if ((UnityEngine.Object) newClip == (UnityEngine.Object) null)
        return false;
      AnimationMode.StopAnimationMode();
      return AnimationWindowUtility.AddClipToAnimationPlayerComponent(animationPlayer, newClip);
    }

    public static bool AddClipToAnimationPlayerComponent(Component animationPlayer, AnimationClip newClip)
    {
      if (animationPlayer is Animator)
        return AnimationWindowUtility.AddClipToAnimatorComponent(animationPlayer as Animator, newClip);
      if (animationPlayer is Animation)
        return AnimationWindowUtility.AddClipToAnimationComponent(animationPlayer as Animation, newClip);
      return false;
    }

    public static bool AddClipToAnimatorComponent(Animator animator, AnimationClip newClip)
    {
      UnityEditor.Animations.AnimatorController animatorController = UnityEditor.Animations.AnimatorController.GetEffectiveAnimatorController(animator);
      if ((UnityEngine.Object) animatorController == (UnityEngine.Object) null)
      {
        UnityEditor.Animations.AnimatorController controllerForClip = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerForClip(newClip, animator.gameObject);
        UnityEditor.Animations.AnimatorController.SetAnimatorController(animator, controllerForClip);
        return (UnityEngine.Object) controllerForClip != (UnityEngine.Object) null;
      }
      ChildAnimatorState state = animatorController.layers[0].stateMachine.FindState(newClip.name);
      if (state.Equals((object) new ChildAnimatorState()))
        animatorController.AddMotion((Motion) newClip);
      else if ((bool) ((UnityEngine.Object) state.state) && (UnityEngine.Object) state.state.motion == (UnityEngine.Object) null)
        state.state.motion = (Motion) newClip;
      else if ((bool) ((UnityEngine.Object) state.state) && (UnityEngine.Object) state.state.motion != (UnityEngine.Object) newClip)
        animatorController.AddMotion((Motion) newClip);
      return true;
    }

    public static bool AddClipToAnimationComponent(Animation animation, AnimationClip newClip)
    {
      AnimationWindowUtility.SetClipAsLegacy(newClip);
      animation.AddClip(newClip, newClip.name);
      return true;
    }

    internal static AnimationClip CreateNewClip(string gameObjectName)
    {
      string message = string.Format("Create a new animation for the game object '{0}':", (object) gameObjectName);
      string path = ProjectWindowUtil.GetActiveFolderPath();
      if (AnimationWindowUtility.s_LastPathUsedForNewClip != null)
      {
        string directoryName = Path.GetDirectoryName(AnimationWindowUtility.s_LastPathUsedForNewClip);
        if (directoryName != null && Directory.Exists(directoryName))
          path = directoryName;
      }
      string clipPath = EditorUtility.SaveFilePanelInProject("Create New Animation", "New Animation", "anim", message, path);
      if (clipPath == string.Empty)
        return (AnimationClip) null;
      return AnimationWindowUtility.CreateNewClipAtPath(clipPath);
    }

    internal static AnimationClip CreateNewClipAtPath(string clipPath)
    {
      AnimationWindowUtility.s_LastPathUsedForNewClip = clipPath;
      AnimationClip clip = new AnimationClip();
      AnimationClipSettings animationClipSettings = AnimationUtility.GetAnimationClipSettings(clip);
      animationClipSettings.loopTime = true;
      AnimationUtility.SetAnimationClipSettingsNoDirty(clip, animationClipSettings);
      AnimationClip animationClip = AssetDatabase.LoadMainAssetAtPath(clipPath) as AnimationClip;
      if ((bool) ((UnityEngine.Object) animationClip))
      {
        EditorUtility.CopySerialized((UnityEngine.Object) clip, (UnityEngine.Object) animationClip);
        AssetDatabase.SaveAssets();
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) clip);
        return animationClip;
      }
      AssetDatabase.CreateAsset((UnityEngine.Object) clip, clipPath);
      return clip;
    }

    private static void SetClipAsLegacy(AnimationClip clip)
    {
      SerializedObject serializedObject = new SerializedObject((UnityEngine.Object) clip);
      serializedObject.FindProperty("m_Legacy").boolValue = true;
      serializedObject.ApplyModifiedProperties();
    }

    internal static AnimationClip AllocateAndSetupClip(bool useAnimator)
    {
      AnimationClip clip = new AnimationClip();
      if (useAnimator)
      {
        AnimationClipSettings animationClipSettings = AnimationUtility.GetAnimationClipSettings(clip);
        animationClipSettings.loopTime = true;
        AnimationUtility.SetAnimationClipSettingsNoDirty(clip, animationClipSettings);
      }
      return clip;
    }

    public static int GetPropertyNodeID(string path, System.Type type, string propertyName)
    {
      return (path + type.Name + propertyName).GetHashCode();
    }

    public static Component GetClosestAnimationPlayerComponentInParents(Transform tr)
    {
      Animator animatorInParents = AnimationWindowUtility.GetClosestAnimatorInParents(tr);
      if ((UnityEngine.Object) animatorInParents != (UnityEngine.Object) null)
        return (Component) animatorInParents;
      Animation animationInParents = AnimationWindowUtility.GetClosestAnimationInParents(tr);
      if ((UnityEngine.Object) animationInParents != (UnityEngine.Object) null)
        return (Component) animationInParents;
      return (Component) null;
    }

    public static Animator GetClosestAnimatorInParents(Transform tr)
    {
      for (; !((UnityEngine.Object) tr.GetComponent<Animator>() != (UnityEngine.Object) null); tr = tr.parent)
      {
        if ((UnityEngine.Object) tr == (UnityEngine.Object) tr.root)
          return (Animator) null;
      }
      return tr.GetComponent<Animator>();
    }

    public static Animation GetClosestAnimationInParents(Transform tr)
    {
      for (; !((UnityEngine.Object) tr.GetComponent<Animation>() != (UnityEngine.Object) null); tr = tr.parent)
      {
        if ((UnityEngine.Object) tr == (UnityEngine.Object) tr.root)
          return (Animation) null;
      }
      return tr.GetComponent<Animation>();
    }

    public static void SyncTimeArea(TimeArea from, TimeArea to)
    {
      to.SetDrawRectHack(from.drawRect);
      to.m_Scale = new Vector2(from.m_Scale.x, to.m_Scale.y);
      to.m_Translation = new Vector2(from.m_Translation.x, to.m_Translation.y);
      to.EnforceScaleAndRange();
    }

    public static void DrawEndOfClip(Rect rect, float endOfClipPixel)
    {
      Rect rect1 = new Rect(Mathf.Max(endOfClipPixel, rect.xMin), rect.yMin, rect.width, rect.height);
      Vector3[] corners = new Vector3[4]{ new Vector3(rect1.xMin, rect1.yMin), new Vector3(rect1.xMax, rect1.yMin), new Vector3(rect1.xMax, rect1.yMax), new Vector3(rect1.xMin, rect1.yMax) };
      Color color1 = !EditorGUIUtility.isProSkin ? Color.gray.AlphaMultiplied(0.32f) : Color.gray.RGBMultiplied(0.3f).AlphaMultiplied(0.5f);
      Color color2 = !EditorGUIUtility.isProSkin ? Color.white.RGBMultiplied(0.4f) : Color.white.RGBMultiplied(0.4f);
      AnimationWindowUtility.DrawRect(corners, color1);
      TimeArea.DrawVerticalLine(corners[0].x, corners[0].y, corners[3].y, color2);
      AnimationWindowUtility.DrawLine((Vector2) corners[0], (Vector2) (corners[3] + new Vector3(0.0f, -1f, 0.0f)), color2);
    }

    public static void DrawPlayHead(float positionX, float minY, float maxY, float alpha)
    {
      TimeArea.DrawVerticalLine(positionX, minY, maxY, Color.red.AlphaMultiplied(alpha));
    }

    public static void DrawVerticalSplitLine(Vector2 start, Vector2 end)
    {
      TimeArea.DrawVerticalLine(start.x, start.y, end.y, !EditorGUIUtility.isProSkin ? Color.white.RGBMultiplied(0.6f) : Color.white.RGBMultiplied(0.15f));
    }

    public static CurveWrapper GetCurveWrapper(AnimationWindowCurve curve, AnimationClip clip)
    {
      CurveWrapper curveWrapper = new CurveWrapper();
      curveWrapper.renderer = (CurveRenderer) new NormalCurveRenderer(curve.ToAnimationCurve());
      curveWrapper.renderer.SetWrap(WrapMode.Once, !clip.isLooping ? WrapMode.Once : WrapMode.Loop);
      curveWrapper.renderer.SetCustomRange(clip.startTime, clip.stopTime);
      curveWrapper.binding = curve.binding;
      curveWrapper.id = CurveUtility.GetCurveID(clip, curve.binding);
      curveWrapper.color = CurveUtility.GetPropertyColor(curve.propertyName);
      curveWrapper.hidden = false;
      return curveWrapper;
    }

    public static AnimationWindowKeyframe CurveSelectionToAnimationWindowKeyframe(CurveSelection curveSelection, List<AnimationWindowCurve> allCurves)
    {
      using (List<AnimationWindowCurve>.Enumerator enumerator = allCurves.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowCurve current = enumerator.Current;
          if (current.binding == curveSelection.curveWrapper.binding && current.m_Keyframes.Count > curveSelection.key)
            return current.m_Keyframes[curveSelection.key];
        }
      }
      return (AnimationWindowKeyframe) null;
    }

    public static CurveSelection AnimationWindowKeyframeToCurveSelection(AnimationWindowKeyframe keyframe, CurveEditor curveEditor)
    {
      foreach (CurveWrapper animationCurve in curveEditor.animationCurves)
      {
        if (animationCurve.binding == keyframe.curve.binding && keyframe.GetIndex() >= 0)
          return new CurveSelection(animationCurve.id, curveEditor, keyframe.GetIndex());
      }
      return (CurveSelection) null;
    }

    public static AnimationWindowCurve BestMatchForPaste(EditorCurveBinding binding, List<AnimationWindowCurve> curves)
    {
      using (List<AnimationWindowCurve>.Enumerator enumerator = curves.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowCurve current = enumerator.Current;
          if (current.binding == binding)
            return current;
        }
      }
      using (List<AnimationWindowCurve>.Enumerator enumerator = curves.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowCurve current = enumerator.Current;
          if (current.binding.propertyName == binding.propertyName)
            return current;
        }
      }
      if (curves.Count == 1)
        return curves[0];
      return (AnimationWindowCurve) null;
    }

    internal static Rect FromToRect(Vector2 start, Vector2 end)
    {
      Rect rect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);
      if ((double) rect.width < 0.0)
      {
        rect.x += rect.width;
        rect.width = -rect.width;
      }
      if ((double) rect.height < 0.0)
      {
        rect.y += rect.height;
        rect.height = -rect.height;
      }
      return rect;
    }

    private static void DrawLine(Vector2 p1, Vector2 p2, Color color)
    {
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(Handles.matrix);
      GL.Begin(1);
      GL.Color(color);
      GL.Vertex((Vector3) p1);
      GL.Vertex((Vector3) p2);
      GL.End();
      GL.PopMatrix();
    }

    private static void DrawRect(Vector3[] corners, Color color)
    {
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(Handles.matrix);
      GL.Begin(7);
      GL.Color(color);
      GL.Vertex(corners[0]);
      GL.Vertex(corners[1]);
      GL.Vertex(corners[2]);
      GL.Vertex(corners[3]);
      GL.End();
      GL.PopMatrix();
    }

    public static bool IsTransformType(System.Type type)
    {
      if (type != typeof (Transform))
        return type == typeof (RectTransform);
      return true;
    }

    public static bool ForceGrouping(EditorCurveBinding binding)
    {
      if (binding.type == typeof (Transform))
        return true;
      if (binding.type == typeof (RectTransform))
      {
        string propertyGroupName = AnimationWindowUtility.GetPropertyGroupName(binding.propertyName);
        if (!(propertyGroupName == "m_LocalPosition") && !(propertyGroupName == "m_LocalScale") && (!(propertyGroupName == "m_LocalRotation") && !(propertyGroupName == "localEulerAnglesBaked")) && !(propertyGroupName == "localEulerAngles"))
          return propertyGroupName == "localEulerAnglesRaw";
        return true;
      }
      if (typeof (Renderer).IsAssignableFrom(binding.type))
        return AnimationWindowUtility.GetPropertyGroupName(binding.propertyName) == "material._Color";
      return false;
    }

    public static void ControllerChanged()
    {
      using (List<AnimEditor>.Enumerator enumerator = AnimEditor.GetAllAnimationWindows().GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.OnControllerChange();
      }
    }
  }
}

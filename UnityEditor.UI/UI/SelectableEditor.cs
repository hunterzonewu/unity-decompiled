// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.SelectableEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.AnimatedValues;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the Selectable Component.</para>
  /// </summary>
  [CustomEditor(typeof (Selectable), true)]
  public class SelectableEditor : Editor
  {
    private static List<SelectableEditor> s_Editors = new List<SelectableEditor>();
    private static bool s_ShowNavigation = false;
    private static string s_ShowNavigationKey = "SelectableEditor.ShowNavigation";
    private GUIContent m_VisualizeNavigation = new GUIContent("Visualize", "Show navigation flows between selectable UI elements.");
    private AnimBool m_ShowColorTint = new AnimBool();
    private AnimBool m_ShowSpriteTrasition = new AnimBool();
    private AnimBool m_ShowAnimTransition = new AnimBool();
    private const float kArrowThickness = 2.5f;
    private const float kArrowHeadSize = 1.2f;
    private SerializedProperty m_Script;
    private SerializedProperty m_InteractableProperty;
    private SerializedProperty m_TargetGraphicProperty;
    private SerializedProperty m_TransitionProperty;
    private SerializedProperty m_ColorBlockProperty;
    private SerializedProperty m_SpriteStateProperty;
    private SerializedProperty m_AnimTriggerProperty;
    private SerializedProperty m_NavigationProperty;
    private string[] m_PropertyPathToExcludeForChildClasses;

    protected virtual void OnEnable()
    {
      this.m_Script = this.serializedObject.FindProperty("m_Script");
      this.m_InteractableProperty = this.serializedObject.FindProperty("m_Interactable");
      this.m_TargetGraphicProperty = this.serializedObject.FindProperty("m_TargetGraphic");
      this.m_TransitionProperty = this.serializedObject.FindProperty("m_Transition");
      this.m_ColorBlockProperty = this.serializedObject.FindProperty("m_Colors");
      this.m_SpriteStateProperty = this.serializedObject.FindProperty("m_SpriteState");
      this.m_AnimTriggerProperty = this.serializedObject.FindProperty("m_AnimationTriggers");
      this.m_NavigationProperty = this.serializedObject.FindProperty("m_Navigation");
      this.m_PropertyPathToExcludeForChildClasses = new string[8]
      {
        this.m_Script.propertyPath,
        this.m_NavigationProperty.propertyPath,
        this.m_TransitionProperty.propertyPath,
        this.m_ColorBlockProperty.propertyPath,
        this.m_SpriteStateProperty.propertyPath,
        this.m_AnimTriggerProperty.propertyPath,
        this.m_InteractableProperty.propertyPath,
        this.m_TargetGraphicProperty.propertyPath
      };
      Selectable.Transition transition = SelectableEditor.GetTransition(this.m_TransitionProperty);
      this.m_ShowColorTint.value = transition == Selectable.Transition.ColorTint;
      this.m_ShowSpriteTrasition.value = transition == Selectable.Transition.SpriteSwap;
      this.m_ShowAnimTransition.value = transition == Selectable.Transition.Animation;
      this.m_ShowColorTint.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowSpriteTrasition.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      SelectableEditor.s_Editors.Add(this);
      this.RegisterStaticOnSceneGUI();
      SelectableEditor.s_ShowNavigation = EditorPrefs.GetBool(SelectableEditor.s_ShowNavigationKey);
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected virtual void OnDisable()
    {
      this.m_ShowColorTint.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowSpriteTrasition.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      SelectableEditor.s_Editors.Remove(this);
      this.RegisterStaticOnSceneGUI();
    }

    private void RegisterStaticOnSceneGUI()
    {
      SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(SelectableEditor.StaticOnSceneGUI);
      if (SelectableEditor.s_Editors.Count <= 0)
        return;
      SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(SelectableEditor.StaticOnSceneGUI);
    }

    private static Selectable.Transition GetTransition(SerializedProperty transition)
    {
      return (Selectable.Transition) transition.enumValueIndex;
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      if (!this.IsDerivedSelectableEditor())
        EditorGUILayout.PropertyField(this.m_Script);
      EditorGUILayout.PropertyField(this.m_InteractableProperty);
      Selectable.Transition transition = SelectableEditor.GetTransition(this.m_TransitionProperty);
      Graphic graphic = this.m_TargetGraphicProperty.objectReferenceValue as Graphic;
      if ((UnityEngine.Object) graphic == (UnityEngine.Object) null)
        graphic = (this.target as Selectable).GetComponent<Graphic>();
      Animator behavior = (this.target as Selectable).GetComponent<Animator>();
      this.m_ShowColorTint.target = !this.m_TransitionProperty.hasMultipleDifferentValues && transition == Selectable.Transition.ColorTint;
      this.m_ShowSpriteTrasition.target = !this.m_TransitionProperty.hasMultipleDifferentValues && transition == Selectable.Transition.SpriteSwap;
      this.m_ShowAnimTransition.target = !this.m_TransitionProperty.hasMultipleDifferentValues && transition == Selectable.Transition.Animation;
      EditorGUILayout.PropertyField(this.m_TransitionProperty);
      ++EditorGUI.indentLevel;
      if (transition == Selectable.Transition.ColorTint || transition == Selectable.Transition.SpriteSwap)
        EditorGUILayout.PropertyField(this.m_TargetGraphicProperty);
      switch (transition)
      {
        case Selectable.Transition.ColorTint:
          if ((UnityEngine.Object) graphic == (UnityEngine.Object) null)
          {
            EditorGUILayout.HelpBox("You must have a Graphic target in order to use a color transition.", MessageType.Warning);
            break;
          }
          break;
        case Selectable.Transition.SpriteSwap:
          if ((UnityEngine.Object) (graphic as Image) == (UnityEngine.Object) null)
          {
            EditorGUILayout.HelpBox("You must have a Image target in order to use a sprite swap transition.", MessageType.Warning);
            break;
          }
          break;
      }
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowColorTint.faded))
        EditorGUILayout.PropertyField(this.m_ColorBlockProperty);
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowSpriteTrasition.faded))
        EditorGUILayout.PropertyField(this.m_SpriteStateProperty);
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowAnimTransition.faded))
      {
        EditorGUILayout.PropertyField(this.m_AnimTriggerProperty);
        if ((UnityEngine.Object) behavior == (UnityEngine.Object) null || (UnityEngine.Object) behavior.runtimeAnimatorController == (UnityEngine.Object) null)
        {
          Rect controlRect = EditorGUILayout.GetControlRect();
          controlRect.xMin += EditorGUIUtility.labelWidth;
          if (GUI.Button(controlRect, "Auto Generate Animation", EditorStyles.miniButton))
          {
            AnimatorController animatorContoller = SelectableEditor.GenerateSelectableAnimatorContoller((this.target as Selectable).animationTriggers, this.target as Selectable);
            if ((UnityEngine.Object) animatorContoller != (UnityEngine.Object) null)
            {
              if ((UnityEngine.Object) behavior == (UnityEngine.Object) null)
                behavior = (this.target as Selectable).gameObject.AddComponent<Animator>();
              AnimatorController.SetAnimatorController(behavior, animatorContoller);
            }
          }
        }
      }
      EditorGUILayout.EndFadeGroup();
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_NavigationProperty);
      EditorGUI.BeginChangeCheck();
      Rect controlRect1 = EditorGUILayout.GetControlRect();
      controlRect1.xMin += EditorGUIUtility.labelWidth;
      SelectableEditor.s_ShowNavigation = GUI.Toggle(controlRect1, SelectableEditor.s_ShowNavigation, this.m_VisualizeNavigation, EditorStyles.miniButton);
      if (EditorGUI.EndChangeCheck())
      {
        EditorPrefs.SetBool(SelectableEditor.s_ShowNavigationKey, SelectableEditor.s_ShowNavigation);
        SceneView.RepaintAll();
      }
      this.ChildClassPropertiesGUI();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void ChildClassPropertiesGUI()
    {
      if (this.IsDerivedSelectableEditor())
        return;
      Editor.DrawPropertiesExcluding(this.serializedObject, this.m_PropertyPathToExcludeForChildClasses);
    }

    private bool IsDerivedSelectableEditor()
    {
      return this.GetType() != typeof (SelectableEditor);
    }

    private static AnimatorController GenerateSelectableAnimatorContoller(AnimationTriggers animationTriggers, Selectable target)
    {
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return (AnimatorController) null;
      string saveControllerPath = SelectableEditor.GetSaveControllerPath(target);
      if (string.IsNullOrEmpty(saveControllerPath))
        return (AnimatorController) null;
      string name1 = !string.IsNullOrEmpty(animationTriggers.normalTrigger) ? animationTriggers.normalTrigger : "Normal";
      string name2 = !string.IsNullOrEmpty(animationTriggers.highlightedTrigger) ? animationTriggers.highlightedTrigger : "Highlighted";
      string name3 = !string.IsNullOrEmpty(animationTriggers.pressedTrigger) ? animationTriggers.pressedTrigger : "Pressed";
      string name4 = !string.IsNullOrEmpty(animationTriggers.disabledTrigger) ? animationTriggers.disabledTrigger : "Disabled";
      AnimatorController controllerAtPath = AnimatorController.CreateAnimatorControllerAtPath(saveControllerPath);
      SelectableEditor.GenerateTriggerableTransition(name1, controllerAtPath);
      SelectableEditor.GenerateTriggerableTransition(name2, controllerAtPath);
      SelectableEditor.GenerateTriggerableTransition(name3, controllerAtPath);
      SelectableEditor.GenerateTriggerableTransition(name4, controllerAtPath);
      AssetDatabase.ImportAsset(saveControllerPath);
      return controllerAtPath;
    }

    private static string GetSaveControllerPath(Selectable target)
    {
      string name = target.gameObject.name;
      string message = string.Format("Create a new animator for the game object '{0}':", (object) name);
      return EditorUtility.SaveFilePanelInProject("New Animation Contoller", name, "controller", message);
    }

    private static void SetUpCurves(AnimationClip highlightedClip, AnimationClip pressedClip, string animationPath)
    {
      string[] strArray = new string[3]
      {
        "m_LocalScale.x",
        "m_LocalScale.y",
        "m_LocalScale.z"
      };
      AnimationCurve curve1 = new AnimationCurve(new Keyframe[3]
      {
        new Keyframe(0.0f, 1f),
        new Keyframe(0.5f, 1.1f),
        new Keyframe(1f, 1f)
      });
      foreach (string inPropertyName in strArray)
        AnimationUtility.SetEditorCurve(highlightedClip, EditorCurveBinding.FloatCurve(animationPath, typeof (Transform), inPropertyName), curve1);
      AnimationCurve curve2 = new AnimationCurve(new Keyframe[1]
      {
        new Keyframe(0.0f, 1.15f)
      });
      foreach (string inPropertyName in strArray)
        AnimationUtility.SetEditorCurve(pressedClip, EditorCurveBinding.FloatCurve(animationPath, typeof (Transform), inPropertyName), curve2);
    }

    private static string BuildAnimationPath(Selectable target)
    {
      Graphic targetGraphic = target.targetGraphic;
      if ((UnityEngine.Object) targetGraphic == (UnityEngine.Object) null)
        return string.Empty;
      GameObject gameObject1 = targetGraphic.gameObject;
      GameObject gameObject2 = target.gameObject;
      Stack<string> stringStack = new Stack<string>();
      for (; (UnityEngine.Object) gameObject2 != (UnityEngine.Object) gameObject1; gameObject1 = gameObject1.transform.parent.gameObject)
      {
        stringStack.Push(gameObject1.name);
        if ((UnityEngine.Object) gameObject1.transform.parent == (UnityEngine.Object) null)
          return string.Empty;
      }
      StringBuilder stringBuilder = new StringBuilder();
      if (stringStack.Count > 0)
        stringBuilder.Append(stringStack.Pop());
      while (stringStack.Count > 0)
        stringBuilder.Append("/").Append(stringStack.Pop());
      return stringBuilder.ToString();
    }

    private static AnimationClip GenerateTriggerableTransition(string name, AnimatorController controller)
    {
      AnimationClip animationClip = AnimatorController.AllocateAnimatorClip(name);
      AssetDatabase.AddObjectToAsset((UnityEngine.Object) animationClip, (UnityEngine.Object) controller);
      AnimatorState destinationState = controller.AddMotion((Motion) animationClip);
      controller.AddParameter(name, AnimatorControllerParameterType.Trigger);
      controller.layers[0].stateMachine.AddAnyStateTransition(destinationState).AddCondition(AnimatorConditionMode.If, 0.0f, name);
      return animationClip;
    }

    private static void StaticOnSceneGUI(SceneView view)
    {
      if (!SelectableEditor.s_ShowNavigation)
        return;
      for (int index = 0; index < Selectable.allSelectables.Count; ++index)
        SelectableEditor.DrawNavigationForSelectable(Selectable.allSelectables[index]);
    }

    private static void DrawNavigationForSelectable(Selectable sel)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SelectableEditor.\u003CDrawNavigationForSelectable\u003Ec__AnonStorey1 selectableCAnonStorey1 = new SelectableEditor.\u003CDrawNavigationForSelectable\u003Ec__AnonStorey1();
      if ((UnityEngine.Object) sel == (UnityEngine.Object) null)
        return;
      // ISSUE: reference to a compiler-generated field
      selectableCAnonStorey1.transform = sel.transform;
      // ISSUE: reference to a compiler-generated method
      Handles.color = new Color(1f, 0.9f, 0.1f, !((IEnumerable<Transform>) Selection.transforms).Any<Transform>(new Func<Transform, bool>(selectableCAnonStorey1.\u003C\u003Em__0)) ? 0.4f : 1f);
      SelectableEditor.DrawNavigationArrow(-Vector2.right, sel, sel.FindSelectableOnLeft());
      SelectableEditor.DrawNavigationArrow(Vector2.right, sel, sel.FindSelectableOnRight());
      SelectableEditor.DrawNavigationArrow(Vector2.up, sel, sel.FindSelectableOnUp());
      SelectableEditor.DrawNavigationArrow(-Vector2.up, sel, sel.FindSelectableOnDown());
    }

    private static void DrawNavigationArrow(Vector2 direction, Selectable fromObj, Selectable toObj)
    {
      if ((UnityEngine.Object) fromObj == (UnityEngine.Object) null || (UnityEngine.Object) toObj == (UnityEngine.Object) null)
        return;
      Transform transform1 = fromObj.transform;
      Transform transform2 = toObj.transform;
      Vector2 vector2 = new Vector2(direction.y, -direction.x);
      Vector3 position1 = transform1.TransformPoint(SelectableEditor.GetPointOnRectEdge(transform1 as RectTransform, direction));
      Vector3 position2 = transform2.TransformPoint(SelectableEditor.GetPointOnRectEdge(transform2 as RectTransform, -direction));
      float num1 = HandleUtility.GetHandleSize(position1) * 0.05f;
      float num2 = HandleUtility.GetHandleSize(position2) * 0.05f;
      Vector3 vector3_1 = position1 + transform1.TransformDirection((Vector3) vector2) * num1;
      Vector3 vector3_2 = position2 + transform2.TransformDirection((Vector3) vector2) * num2;
      float num3 = Vector3.Distance(vector3_1, vector3_2);
      Vector3 vector3_3 = transform1.rotation * (Vector3) direction * num3 * 0.3f;
      Vector3 vector3_4 = transform2.rotation * (Vector3) (-direction) * num3 * 0.3f;
      Handles.DrawBezier(vector3_1, vector3_2, vector3_1 + vector3_3, vector3_2 + vector3_4, Handles.color, (Texture2D) null, 2.5f);
      Handles.DrawAAPolyLine(2.5f, new Vector3[2]
      {
        vector3_2,
        vector3_2 + transform2.rotation * (Vector3) (-direction - vector2) * num2 * 1.2f
      });
      Handles.DrawAAPolyLine(2.5f, new Vector3[2]
      {
        vector3_2,
        vector3_2 + transform2.rotation * (Vector3) (-direction + vector2) * num2 * 1.2f
      });
    }

    private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
    {
      if ((UnityEngine.Object) rect == (UnityEngine.Object) null)
        return Vector3.zero;
      if (dir != Vector2.zero)
        dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
      dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
      return (Vector3) dir;
    }
  }
}

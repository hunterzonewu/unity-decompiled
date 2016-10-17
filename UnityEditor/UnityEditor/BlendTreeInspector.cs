// Decompiled with JetBrains decompiler
// Type: UnityEditor.BlendTreeInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (UnityEditor.Animations.BlendTree))]
  internal class BlendTreeInspector : Editor
  {
    internal static UnityEditor.Animations.AnimatorController currentController = (UnityEditor.Animations.AnimatorController) null;
    internal static Animator currentAnimator = (Animator) null;
    internal static UnityEditor.Animations.BlendTree parentBlendTree = (UnityEditor.Animations.BlendTree) null;
    internal static System.Action<UnityEditor.Animations.BlendTree> blendParameterInputChanged = (System.Action<UnityEditor.Animations.BlendTree>) null;
    private static Color s_VisBgColor = EditorGUIUtility.isProSkin ? new Color(0.2f, 0.2f, 0.2f) : new Color(0.95f, 0.95f, 1f);
    private static Color s_VisWeightColor = EditorGUIUtility.isProSkin ? new Color(0.65f, 0.75f, 1f, 0.65f) : new Color(0.5f, 0.6f, 0.9f, 0.8f);
    private static Color s_VisWeightShapeColor = EditorGUIUtility.isProSkin ? new Color(0.4f, 0.65f, 1f, 0.12f) : new Color(0.4f, 0.65f, 1f, 0.15f);
    private static Color s_VisWeightLineColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.6f) : new Color(0.0f, 0.0f, 0.0f, 0.3f);
    private static Color s_VisPointColor = EditorGUIUtility.isProSkin ? new Color(0.5f, 0.7f, 1f) : new Color(0.5f, 0.7f, 1f);
    private static Color s_VisPointEmptyColor = EditorGUIUtility.isProSkin ? new Color(0.6f, 0.6f, 0.6f) : new Color(0.8f, 0.8f, 0.8f);
    private static Color s_VisPointOverlayColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.4f) : new Color(0.0f, 0.0f, 0.0f, 0.2f);
    private static Color s_VisSamplerColor = EditorGUIUtility.isProSkin ? new Color(1f, 0.4f, 0.4f) : new Color(1f, 0.4f, 0.4f);
    private readonly int m_BlendAnimationID = "BlendAnimationIDHash".GetHashCode();
    private readonly int m_ClickDragFloatID = "ClickDragFloatIDHash".GetHashCode();
    private AnimBool m_ShowGraph = new AnimBool();
    private AnimBool m_ShowCompute = new AnimBool();
    private AnimBool m_ShowAdjust = new AnimBool();
    private List<Texture2D> m_WeightTexs = new List<Texture2D>();
    private int m_SelectedPoint = -1;
    private int kNumCirclePoints = 20;
    private const int kVisResolution = 64;
    private static BlendTreeInspector.Styles styles;
    private ReorderableList m_ReorderableList;
    private SerializedProperty m_Childs;
    private SerializedProperty m_BlendParameter;
    private SerializedProperty m_BlendParameterY;
    private UnityEditor.Animations.BlendTree m_BlendTree;
    private SerializedProperty m_UseAutomaticThresholds;
    private SerializedProperty m_NormalizedBlendValues;
    private SerializedProperty m_MinThreshold;
    private SerializedProperty m_MaxThreshold;
    private SerializedProperty m_Name;
    private SerializedProperty m_BlendType;
    private bool m_ShowGraphValue;
    private float[] m_Weights;
    private Texture2D m_BlendTex;
    private string m_WarningMessage;
    private PreviewBlendTree m_PreviewBlendTree;
    private VisualizationBlendTree m_VisBlendTree;
    private GameObject m_VisInstance;
    private static bool s_ClickDragFloatDragged;
    private static float s_ClickDragFloatDistance;
    private Rect m_BlendRect;
    private bool s_DraggingPoint;

    private int ParameterCount
    {
      get
      {
        if (this.m_BlendType.intValue <= 0)
          return 1;
        return this.m_BlendType.intValue < 4 ? 2 : 0;
      }
    }

    public void OnEnable()
    {
      this.m_Name = this.serializedObject.FindProperty("m_Name");
      this.m_BlendParameter = this.serializedObject.FindProperty("m_BlendParameter");
      this.m_BlendParameterY = this.serializedObject.FindProperty("m_BlendParameterY");
      this.m_UseAutomaticThresholds = this.serializedObject.FindProperty("m_UseAutomaticThresholds");
      this.m_NormalizedBlendValues = this.serializedObject.FindProperty("m_NormalizedBlendValues");
      this.m_MinThreshold = this.serializedObject.FindProperty("m_MinThreshold");
      this.m_MaxThreshold = this.serializedObject.FindProperty("m_MaxThreshold");
      this.m_BlendType = this.serializedObject.FindProperty("m_BlendType");
    }

    private void Init()
    {
      if (BlendTreeInspector.styles == null)
        BlendTreeInspector.styles = new BlendTreeInspector.Styles();
      if ((UnityEngine.Object) this.m_BlendTree == (UnityEngine.Object) null)
        this.m_BlendTree = this.target as UnityEditor.Animations.BlendTree;
      if (BlendTreeInspector.styles == null)
        BlendTreeInspector.styles = new BlendTreeInspector.Styles();
      if (this.m_PreviewBlendTree == null)
        this.m_PreviewBlendTree = new PreviewBlendTree();
      if (this.m_VisBlendTree == null)
        this.m_VisBlendTree = new VisualizationBlendTree();
      if (this.m_Childs == null)
      {
        this.m_Childs = this.serializedObject.FindProperty("m_Childs");
        this.m_ReorderableList = new ReorderableList(this.serializedObject, this.m_Childs);
        this.m_ReorderableList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawHeader);
        this.m_ReorderableList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawChild);
        this.m_ReorderableList.onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.EndDragChild);
        this.m_ReorderableList.onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate(this.AddButton);
        this.m_ReorderableList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveButton);
        if (this.m_BlendType.intValue == 0)
          this.SortByThreshold();
        this.m_ShowGraphValue = this.m_BlendType.intValue != 4 ? this.m_Childs.arraySize >= 2 : this.m_Childs.arraySize >= 1;
        this.m_ShowGraph.value = this.m_ShowGraphValue;
        this.m_ShowAdjust.value = this.AllMotions();
        this.m_ShowCompute.value = !this.m_UseAutomaticThresholds.boolValue;
        this.m_ShowGraph.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
        this.m_ShowAdjust.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
        this.m_ShowCompute.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      }
      this.m_PreviewBlendTree.Init(this.m_BlendTree, BlendTreeInspector.currentAnimator);
      bool flag = false;
      if ((UnityEngine.Object) this.m_VisInstance == (UnityEngine.Object) null)
      {
        this.m_VisInstance = EditorUtility.InstantiateForAnimatorPreview(EditorGUIUtility.Load("Avatar/DefaultAvatar.fbx"));
        foreach (Renderer componentsInChild in this.m_VisInstance.GetComponentsInChildren<Renderer>())
          componentsInChild.enabled = false;
        flag = true;
      }
      this.m_VisBlendTree.Init(this.m_BlendTree, this.m_VisInstance.GetComponent<Animator>());
      if (!flag || this.m_BlendType.intValue != 1 && this.m_BlendType.intValue != 2 && this.m_BlendType.intValue != 3)
        return;
      this.UpdateBlendVisualization();
      this.ValidatePositions();
    }

    internal override void OnHeaderIconGUI(Rect iconRect)
    {
      Texture2D miniThumbnail = AssetPreview.GetMiniThumbnail(this.target);
      GUI.Label(iconRect, (Texture) miniThumbnail);
    }

    internal override void OnHeaderTitleGUI(Rect titleRect, string header)
    {
      this.serializedObject.Update();
      Rect position = titleRect;
      position.height = 16f;
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_Name.hasMultipleDifferentValues;
      string name = EditorGUI.DelayedTextField(position, this.m_Name.stringValue, EditorStyles.textField);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck() && !string.IsNullOrEmpty(name))
      {
        foreach (UnityEngine.Object target in this.targets)
          ObjectNames.SetNameSmart(target, name);
      }
      this.serializedObject.ApplyModifiedProperties();
    }

    internal override void OnHeaderControlsGUI()
    {
      EditorGUIUtility.labelWidth = 80f;
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_BlendType);
      this.serializedObject.ApplyModifiedProperties();
    }

    private List<string> CollectParameters(UnityEditor.Animations.AnimatorController controller)
    {
      List<string> stringList = new List<string>();
      if ((UnityEngine.Object) controller != (UnityEngine.Object) null)
      {
        foreach (UnityEngine.AnimatorControllerParameter parameter in controller.parameters)
        {
          if (parameter.type == UnityEngine.AnimatorControllerParameterType.Float)
            stringList.Add(parameter.name);
        }
      }
      return stringList;
    }

    private void ParameterGUI()
    {
      EditorGUILayout.BeginHorizontal();
      if (this.ParameterCount > 1)
        EditorGUILayout.PrefixLabel(EditorGUIUtility.TempContent("Parameters"));
      else
        EditorGUILayout.PrefixLabel(EditorGUIUtility.TempContent("Parameter"));
      this.serializedObject.Update();
      string blendParameter = this.m_BlendTree.blendParameter;
      string blendParameterY = this.m_BlendTree.blendParameterY;
      List<string> stringList = this.CollectParameters(BlendTreeInspector.currentController);
      EditorGUI.BeginChangeCheck();
      string str1 = EditorGUILayout.DelayedTextFieldDropDown(blendParameter, stringList.ToArray());
      if (EditorGUI.EndChangeCheck())
        this.m_BlendParameter.stringValue = str1;
      if (this.ParameterCount > 1)
      {
        EditorGUI.BeginChangeCheck();
        string str2 = EditorGUILayout.TextFieldDropDown(blendParameterY, stringList.ToArray());
        if (EditorGUI.EndChangeCheck())
          this.m_BlendParameterY.stringValue = str2;
      }
      this.serializedObject.ApplyModifiedProperties();
      EditorGUILayout.EndHorizontal();
    }

    public override void OnInspectorGUI()
    {
      this.Init();
      this.serializedObject.Update();
      if (this.m_BlendType.intValue != 4)
        this.ParameterGUI();
      this.m_ShowGraphValue = this.m_BlendType.intValue != 4 ? this.m_Childs.arraySize >= 2 : this.m_Childs.arraySize >= 1;
      this.m_ShowGraph.target = this.m_ShowGraphValue;
      this.m_UseAutomaticThresholds = this.serializedObject.FindProperty("m_UseAutomaticThresholds");
      GUI.enabled = true;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowGraph.faded))
      {
        if (this.m_BlendType.intValue == 0)
        {
          this.BlendGraph(EditorGUILayout.GetControlRect(false, 40f, BlendTreeInspector.styles.background, new GUILayoutOption[0]));
          this.ThresholdValues();
        }
        else if (this.m_BlendType.intValue == 4)
        {
          for (int index = 0; index < this.m_BlendTree.recursiveBlendParameterCount; ++index)
          {
            string recursiveBlendParameter = this.m_BlendTree.GetRecursiveBlendParameter(index);
            float blendParameterMin = this.m_BlendTree.GetRecursiveBlendParameterMin(index);
            float blendParameterMax = this.m_BlendTree.GetRecursiveBlendParameterMax(index);
            EditorGUI.BeginChangeCheck();
            float num = EditorGUILayout.Slider(recursiveBlendParameter, this.m_BlendTree.GetInputBlendValue(recursiveBlendParameter), blendParameterMin, blendParameterMax, new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck())
            {
              if ((bool) ((UnityEngine.Object) BlendTreeInspector.parentBlendTree))
              {
                BlendTreeInspector.parentBlendTree.SetInputBlendValue(recursiveBlendParameter, num);
                if (BlendTreeInspector.blendParameterInputChanged != null)
                  BlendTreeInspector.blendParameterInputChanged(BlendTreeInspector.parentBlendTree);
              }
              this.m_BlendTree.SetInputBlendValue(recursiveBlendParameter, num);
              if (BlendTreeInspector.blendParameterInputChanged != null)
                BlendTreeInspector.blendParameterInputChanged(this.m_BlendTree);
            }
          }
        }
        else
        {
          GUILayout.Space(1f);
          GUILayout.BeginHorizontal();
          GUILayout.FlexibleSpace();
          Rect aspectRect = GUILayoutUtility.GetAspectRect(1f, new GUILayoutOption[1]{ GUILayout.MaxWidth(235f) });
          GUI.Label(new Rect(aspectRect.x - 1f, aspectRect.y - 1f, aspectRect.width + 2f, aspectRect.height + 2f), GUIContent.none, EditorStyles.textField);
          GUI.BeginGroup(aspectRect);
          aspectRect.x = 0.0f;
          aspectRect.y = 0.0f;
          this.BlendGraph2D(aspectRect);
          GUI.EndGroup();
          GUILayout.FlexibleSpace();
          GUILayout.EndHorizontal();
        }
        GUILayout.Space(5f);
      }
      EditorGUILayout.EndFadeGroup();
      if (this.m_ReorderableList != null)
        this.m_ReorderableList.DoLayoutList();
      if (this.m_BlendType.intValue == 4)
        EditorGUILayout.PropertyField(this.m_NormalizedBlendValues, EditorGUIUtility.TempContent("Normalized Blend Values"), new GUILayoutOption[0]);
      if (this.m_ShowGraphValue)
      {
        GUILayout.Space(10f);
        this.AutoCompute();
      }
      this.serializedObject.ApplyModifiedProperties();
    }

    private void SetMinMaxThresholds()
    {
      float num1 = float.PositiveInfinity;
      float num2 = float.NegativeInfinity;
      for (int index = 0; index < this.m_Childs.arraySize; ++index)
      {
        SerializedProperty propertyRelative = this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Threshold");
        num1 = (double) propertyRelative.floatValue >= (double) num1 ? num1 : propertyRelative.floatValue;
        num2 = (double) propertyRelative.floatValue <= (double) num2 ? num2 : propertyRelative.floatValue;
      }
      this.m_MinThreshold.floatValue = this.m_Childs.arraySize <= 0 ? 0.0f : num1;
      this.m_MaxThreshold.floatValue = this.m_Childs.arraySize <= 0 ? 1f : num2;
    }

    private void ThresholdValues()
    {
      Rect controlRect = EditorGUILayout.GetControlRect();
      Rect position1 = controlRect;
      Rect position2 = controlRect;
      position1.width /= 4f;
      position2.width /= 4f;
      position2.x = controlRect.x + controlRect.width - position2.width;
      float floatValue1 = this.m_MinThreshold.floatValue;
      float floatValue2 = this.m_MaxThreshold.floatValue;
      EditorGUI.BeginChangeCheck();
      float a = this.ClickDragFloat(position1, floatValue1);
      float b = this.ClickDragFloat(position2, floatValue2, true);
      if (!EditorGUI.EndChangeCheck())
        return;
      if (this.m_Childs.arraySize >= 2)
      {
        SerializedProperty arrayElementAtIndex1 = this.m_Childs.GetArrayElementAtIndex(0);
        SerializedProperty arrayElementAtIndex2 = this.m_Childs.GetArrayElementAtIndex(this.m_Childs.arraySize - 1);
        SerializedProperty propertyRelative1 = arrayElementAtIndex1.FindPropertyRelative("m_Threshold");
        SerializedProperty propertyRelative2 = arrayElementAtIndex2.FindPropertyRelative("m_Threshold");
        propertyRelative1.floatValue = Mathf.Min(a, b);
        propertyRelative2.floatValue = Mathf.Max(a, b);
      }
      if (!this.m_UseAutomaticThresholds.boolValue)
      {
        for (int index = 0; index < this.m_Childs.arraySize; ++index)
        {
          SerializedProperty propertyRelative = this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Threshold");
          float t = Mathf.InverseLerp(this.m_MinThreshold.floatValue, this.m_MaxThreshold.floatValue, propertyRelative.floatValue);
          propertyRelative.floatValue = Mathf.Lerp(Mathf.Min(a, b), Mathf.Max(a, b), t);
        }
      }
      this.m_MinThreshold.floatValue = Mathf.Min(a, b);
      this.m_MaxThreshold.floatValue = Mathf.Max(a, b);
    }

    public float ClickDragFloat(Rect position, float value)
    {
      return this.ClickDragFloat(position, value, false);
    }

    public float ClickDragFloat(Rect position, float value, bool alignRight)
    {
      string allowedletters = "inftynaeINFTYNAE0123456789.,-";
      int controlId = GUIUtility.GetControlID(this.m_ClickDragFloatID, FocusType.Keyboard, position);
      Event current = Event.current;
      switch (current.type)
      {
        case EventType.MouseDown:
          if ((GUIUtility.keyboardControl != controlId || !EditorGUIUtility.editingTextField) && position.Contains(current.mousePosition))
          {
            current.Use();
            BlendTreeInspector.s_ClickDragFloatDragged = false;
            BlendTreeInspector.s_ClickDragFloatDistance = 0.0f;
            GUIUtility.hotControl = controlId;
            GUIUtility.keyboardControl = controlId;
            EditorGUIUtility.editingTextField = false;
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            current.Use();
            if (position.Contains(current.mousePosition) && !BlendTreeInspector.s_ClickDragFloatDragged)
            {
              EditorGUIUtility.editingTextField = true;
              break;
            }
            GUIUtility.keyboardControl = 0;
            GUIUtility.hotControl = 0;
            BlendTreeInspector.s_ClickDragFloatDragged = false;
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId && !EditorGUIUtility.editingTextField)
          {
            BlendTreeInspector.s_ClickDragFloatDistance += Mathf.Abs(HandleUtility.niceMouseDelta);
            if ((double) BlendTreeInspector.s_ClickDragFloatDistance >= 5.0)
            {
              BlendTreeInspector.s_ClickDragFloatDragged = true;
              value += HandleUtility.niceMouseDelta * 0.03f;
              value = MathUtils.RoundBasedOnMinimumDifference(value, 0.03f);
              GUI.changed = true;
            }
            current.Use();
            break;
          }
          break;
      }
      GUIStyle style = GUIUtility.keyboardControl != controlId || !EditorGUIUtility.editingTextField ? (!alignRight ? BlendTreeInspector.styles.clickDragFloatLabelLeft : BlendTreeInspector.styles.clickDragFloatLabelRight) : (!alignRight ? BlendTreeInspector.styles.clickDragFloatFieldLeft : BlendTreeInspector.styles.clickDragFloatFieldRight);
      if (GUIUtility.keyboardControl == controlId)
      {
        string text;
        if (!EditorGUI.s_RecycledEditor.IsEditingControl(controlId))
        {
          text = EditorGUI.s_RecycledCurrentEditingString = value.ToString("g7");
        }
        else
        {
          text = EditorGUI.s_RecycledCurrentEditingString;
          if (current.type == EventType.ValidateCommand && current.commandName == "UndoRedoPerformed")
            text = value.ToString("g7");
        }
        bool changed;
        string str = EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, controlId, position, text, style, allowedletters, out changed, false, false, false);
        if (changed)
        {
          GUI.changed = true;
          EditorGUI.s_RecycledCurrentEditingString = str;
          string lower = str.ToLower();
          if (lower == "inf" || lower == "infinity")
            value = float.PositiveInfinity;
          else if (lower == "-inf" || lower == "-infinity")
          {
            value = float.NegativeInfinity;
          }
          else
          {
            if (!float.TryParse(str.Replace(',', '.'), NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out value))
            {
              EditorGUI.s_RecycledCurrentEditingFloat = 0.0;
              value = 0.0f;
              return value;
            }
            if (float.IsNaN(value))
              value = 0.0f;
            EditorGUI.s_RecycledCurrentEditingFloat = (double) value;
          }
        }
      }
      else
      {
        string text = value.ToString("g7");
        bool changed;
        EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, controlId, position, text, style, allowedletters, out changed, false, false, false);
      }
      return value;
    }

    private void BlendGraph(Rect area)
    {
      ++area.xMin;
      --area.xMax;
      int controlId = GUIUtility.GetControlID(this.m_BlendAnimationID, FocusType.Passive);
      int arraySize = this.m_Childs.arraySize;
      float[] numArray = new float[arraySize];
      for (int index = 0; index < arraySize; ++index)
      {
        SerializedProperty propertyRelative = this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Threshold");
        numArray[index] = propertyRelative.floatValue;
      }
      float a = Mathf.Min(numArray);
      float b = Mathf.Max(numArray);
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = area.x + Mathf.InverseLerp(a, b, numArray[index]) * area.width;
      string blendParameter = this.m_BlendTree.blendParameter;
      Rect position = new Rect(area.x + Mathf.InverseLerp(a, b, this.m_BlendTree.GetInputBlendValue(blendParameter)) * area.width - 4f, area.y, 9f, 42f);
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (position.Contains(current.mousePosition))
          {
            current.Use();
            GUIUtility.hotControl = controlId;
            this.m_ReorderableList.index = -1;
            this.m_ReorderableList.index = -1;
            float t = Mathf.InverseLerp(0.0f, area.width, current.mousePosition.x - 4f);
            float num = Mathf.Lerp(a, b, t);
            this.m_BlendTree.SetInputBlendValue(blendParameter, num);
            if ((bool) ((UnityEngine.Object) BlendTreeInspector.parentBlendTree))
            {
              BlendTreeInspector.parentBlendTree.SetInputBlendValue(blendParameter, num);
              if (BlendTreeInspector.blendParameterInputChanged != null)
                BlendTreeInspector.blendParameterInputChanged(BlendTreeInspector.parentBlendTree);
            }
            if (BlendTreeInspector.blendParameterInputChanged == null)
              break;
            BlendTreeInspector.blendParameterInputChanged(this.m_BlendTree);
            break;
          }
          if (!area.Contains(current.mousePosition))
            break;
          current.Use();
          GUIUtility.hotControl = controlId;
          GUIUtility.keyboardControl = controlId;
          float x1 = current.mousePosition.x;
          float num1 = float.PositiveInfinity;
          for (int index = 0; index < numArray.Length; ++index)
          {
            float num2 = index != 0 ? numArray[index - 1] : numArray[index];
            float num3 = index != numArray.Length - 1 ? numArray[index + 1] : numArray[index];
            if ((double) Mathf.Abs(x1 - numArray[index]) < (double) num1 && (double) x1 < (double) num3 && (double) x1 > (double) num2)
            {
              num1 = Mathf.Abs(x1 - numArray[index]);
              this.m_ReorderableList.index = index;
            }
          }
          this.m_UseAutomaticThresholds.boolValue = false;
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          current.Use();
          GUIUtility.hotControl = 0;
          this.m_ReorderableList.index = -1;
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != controlId)
            break;
          current.Use();
          if (this.m_ReorderableList.index == -1)
          {
            float t = Mathf.InverseLerp(0.0f, area.width, current.mousePosition.x - 4f);
            float num2 = Mathf.Lerp(a, b, t);
            this.m_BlendTree.SetInputBlendValue(blendParameter, num2);
            if ((bool) ((UnityEngine.Object) BlendTreeInspector.parentBlendTree))
            {
              BlendTreeInspector.parentBlendTree.SetInputBlendValue(blendParameter, num2);
              if (BlendTreeInspector.blendParameterInputChanged != null)
                BlendTreeInspector.blendParameterInputChanged(BlendTreeInspector.parentBlendTree);
            }
            if (BlendTreeInspector.blendParameterInputChanged == null)
              break;
            BlendTreeInspector.blendParameterInputChanged(this.m_BlendTree);
            break;
          }
          float t1 = Mathf.InverseLerp(0.0f, area.width, current.mousePosition.x);
          Mathf.Lerp(a, b, t1);
          SerializedProperty arrayElementAtIndex = this.m_Childs.GetArrayElementAtIndex(this.m_ReorderableList.index);
          SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("m_Threshold");
          SerializedProperty serializedProperty1 = this.m_ReorderableList.index > 0 ? this.m_Childs.GetArrayElementAtIndex(this.m_ReorderableList.index - 1) : arrayElementAtIndex;
          SerializedProperty serializedProperty2 = this.m_ReorderableList.index != this.m_Childs.arraySize - 1 ? this.m_Childs.GetArrayElementAtIndex(this.m_ReorderableList.index + 1) : arrayElementAtIndex;
          SerializedProperty propertyRelative2 = serializedProperty1.FindPropertyRelative("m_Threshold");
          SerializedProperty propertyRelative3 = serializedProperty2.FindPropertyRelative("m_Threshold");
          float num4 = (b - a) / area.width;
          float x2 = current.delta.x;
          propertyRelative1.floatValue += x2 * num4;
          if ((double) propertyRelative1.floatValue < (double) propertyRelative2.floatValue && this.m_ReorderableList.index != 0)
          {
            this.m_Childs.MoveArrayElement(this.m_ReorderableList.index, this.m_ReorderableList.index - 1);
            --this.m_ReorderableList.index;
          }
          if ((double) propertyRelative1.floatValue > (double) propertyRelative3.floatValue && this.m_ReorderableList.index < this.m_Childs.arraySize - 1)
          {
            this.m_Childs.MoveArrayElement(this.m_ReorderableList.index, this.m_ReorderableList.index + 1);
            ++this.m_ReorderableList.index;
          }
          float num5 = (float) (3.0 * (((double) b - (double) a) / (double) area.width));
          if ((double) propertyRelative1.floatValue - (double) propertyRelative2.floatValue <= (double) num5)
            propertyRelative1.floatValue = propertyRelative2.floatValue;
          else if ((double) propertyRelative3.floatValue - (double) propertyRelative1.floatValue <= (double) num5)
            propertyRelative1.floatValue = propertyRelative3.floatValue;
          this.SetMinMaxThresholds();
          break;
        case EventType.Repaint:
          BlendTreeInspector.styles.background.Draw(area, GUIContent.none, false, false, false, false);
          if (this.m_Childs.arraySize >= 2)
          {
            for (int index = 0; index < numArray.Length; ++index)
            {
              float min = index != 0 ? numArray[index - 1] : numArray[index];
              float max = index != numArray.Length - 1 ? numArray[index + 1] : numArray[index];
              bool selected = this.m_ReorderableList.index == index;
              this.DrawAnimation(numArray[index], min, max, selected, area);
            }
            Color color = Handles.color;
            Handles.color = new Color(0.0f, 0.0f, 0.0f, 0.25f);
            Handles.DrawLine(new Vector3(area.x, area.y + area.height, 0.0f), new Vector3(area.x + area.width, area.y + area.height, 0.0f));
            Handles.color = color;
            BlendTreeInspector.styles.blendPosition.Draw(position, GUIContent.none, false, false, false, false);
            break;
          }
          GUI.Label(area, EditorGUIUtility.TempContent("Please Add Motion Fields or Blend Trees"), BlendTreeInspector.styles.errorStyle);
          break;
      }
    }

    private void UpdateBlendVisualization()
    {
      Vector2[] activeMotionPositions = this.GetActiveMotionPositions();
      if ((UnityEngine.Object) this.m_BlendTex == (UnityEngine.Object) null)
      {
        this.m_BlendTex = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        this.m_BlendTex.hideFlags = HideFlags.HideAndDontSave;
        this.m_BlendTex.wrapMode = TextureWrapMode.Clamp;
      }
      while (this.m_WeightTexs.Count < activeMotionPositions.Length)
      {
        Texture2D texture2D = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        texture2D.wrapMode = TextureWrapMode.Clamp;
        texture2D.hideFlags = HideFlags.HideAndDontSave;
        this.m_WeightTexs.Add(texture2D);
      }
      while (this.m_WeightTexs.Count > activeMotionPositions.Length)
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_WeightTexs[this.m_WeightTexs.Count - 1]);
        this.m_WeightTexs.RemoveAt(this.m_WeightTexs.Count - 1);
      }
      if (GUIUtility.hotControl == 0)
        this.m_BlendRect = this.Get2DBlendRect(this.GetMotionPositions());
      this.m_VisBlendTree.Reset();
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      Texture2D[] array = this.m_WeightTexs.ToArray();
      if (GUIUtility.hotControl != 0 && this.m_ReorderableList.index >= 0)
      {
        int[] activeMotionIndices = this.GetMotionToActiveMotionIndices();
        for (int index = 0; index < array.Length; ++index)
        {
          if (activeMotionIndices[this.m_ReorderableList.index] != index)
            array[index] = (Texture2D) null;
        }
      }
      BlendTreePreviewUtility.CalculateBlendTexture(this.m_VisBlendTree.animator, 0, this.m_VisBlendTree.animator.GetCurrentAnimatorStateInfo(0).fullPathHash, this.m_BlendTex, array, this.m_BlendRect);
      stopwatch.Stop();
    }

    private Vector2[] GetMotionPositions()
    {
      int arraySize = this.m_Childs.arraySize;
      Vector2[] vector2Array = new Vector2[arraySize];
      for (int index = 0; index < arraySize; ++index)
      {
        SerializedProperty propertyRelative = this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Position");
        vector2Array[index] = propertyRelative.vector2Value;
      }
      return vector2Array;
    }

    private Vector2[] GetActiveMotionPositions()
    {
      List<Vector2> vector2List = new List<Vector2>();
      int arraySize = this.m_Childs.arraySize;
      for (int index = 0; index < arraySize; ++index)
      {
        SerializedProperty arrayElementAtIndex = this.m_Childs.GetArrayElementAtIndex(index);
        if (arrayElementAtIndex.FindPropertyRelative("m_Motion").objectReferenceValue != (UnityEngine.Object) null)
        {
          SerializedProperty propertyRelative = arrayElementAtIndex.FindPropertyRelative("m_Position");
          vector2List.Add(propertyRelative.vector2Value);
        }
      }
      return vector2List.ToArray();
    }

    private int[] GetMotionToActiveMotionIndices()
    {
      int arraySize = this.m_Childs.arraySize;
      int[] numArray = new int[arraySize];
      int num = 0;
      for (int index = 0; index < arraySize; ++index)
      {
        if (this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Motion").objectReferenceValue == (UnityEngine.Object) null)
        {
          numArray[index] = -1;
        }
        else
        {
          numArray[index] = num;
          ++num;
        }
      }
      return numArray;
    }

    private Rect Get2DBlendRect(Vector2[] points)
    {
      Vector2 vector2 = Vector2.zero;
      float a = 0.0f;
      if (points.Length == 0)
        return new Rect();
      if (this.m_BlendType.intValue == 3)
      {
        Vector2 point1 = points[0];
        Vector2 point2 = points[0];
        for (int index = 1; index < points.Length; ++index)
        {
          point2.x = Mathf.Max(point2.x, points[index].x);
          point2.y = Mathf.Max(point2.y, points[index].y);
          point1.x = Mathf.Min(point1.x, points[index].x);
          point1.y = Mathf.Min(point1.y, points[index].y);
        }
        vector2 = (point1 + point2) * 0.5f;
        a = Mathf.Max(point2.x - point1.x, point2.y - point1.y) * 0.5f;
      }
      else
      {
        for (int index = 0; index < points.Length; ++index)
          a = Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(a, points[index].x), -points[index].x), points[index].y), -points[index].y);
      }
      if ((double) a == 0.0)
        a = 1f;
      float num = a * 1.35f;
      return new Rect(vector2.x - num, vector2.y - num, num * 2f, num * 2f);
    }

    private float ConvertFloat(float input, float fromMin, float fromMax, float toMin, float toMax)
    {
      float num = (float) (((double) input - (double) fromMin) / ((double) fromMax - (double) fromMin));
      return (float) ((double) toMin * (1.0 - (double) num) + (double) toMax * (double) num);
    }

    private void BlendGraph2D(Rect area)
    {
      if (this.m_VisBlendTree.controllerDirty)
      {
        this.UpdateBlendVisualization();
        this.ValidatePositions();
      }
      Vector2[] motionPositions = this.GetMotionPositions();
      int[] activeMotionIndices = this.GetMotionToActiveMotionIndices();
      Vector2 vector2_1 = new Vector2(this.m_BlendRect.xMin, this.m_BlendRect.yMin);
      Vector2 vector2_2 = new Vector2(this.m_BlendRect.xMax, this.m_BlendRect.yMax);
      for (int index = 0; index < motionPositions.Length; ++index)
      {
        motionPositions[index].x = this.ConvertFloat(motionPositions[index].x, vector2_1.x, vector2_2.x, area.xMin, area.xMax);
        motionPositions[index].y = this.ConvertFloat(motionPositions[index].y, vector2_1.y, vector2_2.y, area.yMax, area.yMin);
      }
      string blendParameter = this.m_BlendTree.blendParameter;
      string blendParameterY = this.m_BlendTree.blendParameterY;
      float inputBlendValue1 = this.m_BlendTree.GetInputBlendValue(blendParameter);
      float inputBlendValue2 = this.m_BlendTree.GetInputBlendValue(blendParameterY);
      int length = this.GetActiveMotionPositions().Length;
      if (this.m_Weights == null || length != this.m_Weights.Length)
        this.m_Weights = new float[length];
      BlendTreePreviewUtility.CalculateRootBlendTreeChildWeights(this.m_VisBlendTree.animator, 0, this.m_VisBlendTree.animator.GetCurrentAnimatorStateInfo(0).fullPathHash, this.m_Weights, inputBlendValue1, inputBlendValue2);
      Rect position1 = new Rect(area.x + Mathf.InverseLerp(vector2_1.x, vector2_2.x, inputBlendValue1) * area.width - 5f, area.y + (1f - Mathf.InverseLerp(vector2_1.y, vector2_2.y, inputBlendValue2)) * area.height - 5f, 11f, 11f);
      int controlId = GUIUtility.GetControlID(this.m_BlendAnimationID, FocusType.Native);
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (position1.Contains(current.mousePosition))
          {
            current.Use();
            GUIUtility.hotControl = controlId;
            this.m_SelectedPoint = -1;
            break;
          }
          if (area.Contains(current.mousePosition))
          {
            this.m_ReorderableList.index = -1;
            for (int index = 0; index < motionPositions.Length; ++index)
            {
              if (new Rect(motionPositions[index].x - 4f, motionPositions[index].y - 4f, 9f, 9f).Contains(current.mousePosition))
              {
                current.Use();
                GUIUtility.hotControl = controlId;
                this.m_SelectedPoint = index;
                this.m_ReorderableList.index = index;
              }
            }
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            current.Use();
            GUIUtility.hotControl = 0;
            this.s_DraggingPoint = false;
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            if (this.m_SelectedPoint == -1)
            {
              Vector2 vector2_3;
              vector2_3.x = this.ConvertFloat(current.mousePosition.x, area.xMin, area.xMax, vector2_1.x, vector2_2.x);
              vector2_3.y = this.ConvertFloat(current.mousePosition.y, area.yMax, area.yMin, vector2_1.y, vector2_2.y);
              this.m_BlendTree.SetInputBlendValue(blendParameter, vector2_3.x);
              this.m_BlendTree.SetInputBlendValue(blendParameterY, vector2_3.y);
              if ((bool) ((UnityEngine.Object) BlendTreeInspector.parentBlendTree))
              {
                BlendTreeInspector.parentBlendTree.SetInputBlendValue(blendParameter, vector2_3.x);
                BlendTreeInspector.parentBlendTree.SetInputBlendValue(blendParameterY, vector2_3.y);
                if (BlendTreeInspector.blendParameterInputChanged != null)
                  BlendTreeInspector.blendParameterInputChanged(BlendTreeInspector.parentBlendTree);
              }
              if (BlendTreeInspector.blendParameterInputChanged != null)
                BlendTreeInspector.blendParameterInputChanged(this.m_BlendTree);
              current.Use();
              break;
            }
            for (int index = 0; index < motionPositions.Length; ++index)
            {
              if (this.m_SelectedPoint == index)
              {
                Vector2 vector2_3;
                vector2_3.x = this.ConvertFloat(current.mousePosition.x, area.xMin, area.xMax, vector2_1.x, vector2_2.x);
                vector2_3.y = this.ConvertFloat(current.mousePosition.y, area.yMax, area.yMin, vector2_1.y, vector2_2.y);
                float minDifference = (vector2_2.x - vector2_1.x) / area.width;
                vector2_3.x = MathUtils.RoundBasedOnMinimumDifference(vector2_3.x, minDifference);
                vector2_3.y = MathUtils.RoundBasedOnMinimumDifference(vector2_3.y, minDifference);
                vector2_3.x = Mathf.Clamp(vector2_3.x, -10000f, 10000f);
                vector2_3.y = Mathf.Clamp(vector2_3.y, -10000f, 10000f);
                this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Position").vector2Value = vector2_3;
                current.Use();
                this.s_DraggingPoint = true;
              }
            }
            break;
          }
          break;
        case EventType.Repaint:
          GUI.color = BlendTreeInspector.s_VisBgColor;
          GUI.DrawTexture(area, (Texture) EditorGUIUtility.whiteTexture);
          if (this.m_ReorderableList.index < 0)
          {
            Color visWeightColor = BlendTreeInspector.s_VisWeightColor;
            visWeightColor.a *= 0.75f;
            GUI.color = visWeightColor;
            GUI.DrawTexture(area, (Texture) this.m_BlendTex);
          }
          else if (activeMotionIndices[this.m_ReorderableList.index] >= 0)
          {
            GUI.color = BlendTreeInspector.s_VisWeightColor;
            GUI.DrawTexture(area, (Texture) this.m_WeightTexs[activeMotionIndices[this.m_ReorderableList.index]]);
          }
          GUI.color = Color.white;
          if (!this.s_DraggingPoint)
          {
            for (int index = 0; index < motionPositions.Length; ++index)
            {
              if (activeMotionIndices[index] >= 0)
                this.DrawWeightShape(motionPositions[index], this.m_Weights[activeMotionIndices[index]], 0);
            }
            for (int index = 0; index < motionPositions.Length; ++index)
            {
              if (activeMotionIndices[index] >= 0)
                this.DrawWeightShape(motionPositions[index], this.m_Weights[activeMotionIndices[index]], 1);
            }
          }
          for (int index = 0; index < motionPositions.Length; ++index)
          {
            Rect position2 = new Rect(motionPositions[index].x - 6f, motionPositions[index].y - 6f, 13f, 13f);
            bool flag = this.m_ReorderableList.index == index;
            GUI.color = activeMotionIndices[index] >= 0 ? BlendTreeInspector.s_VisPointColor : BlendTreeInspector.s_VisPointEmptyColor;
            GUI.DrawTexture(position2, !flag ? (Texture) BlendTreeInspector.styles.pointIcon : (Texture) BlendTreeInspector.styles.pointIconSelected);
            if (flag)
            {
              GUI.color = BlendTreeInspector.s_VisPointOverlayColor;
              GUI.DrawTexture(position2, (Texture) BlendTreeInspector.styles.pointIconOverlay);
            }
          }
          if (!this.s_DraggingPoint)
          {
            GUI.color = BlendTreeInspector.s_VisSamplerColor;
            GUI.DrawTexture(position1, (Texture) BlendTreeInspector.styles.samplerIcon);
          }
          GUI.color = Color.white;
          break;
      }
      if (this.m_ReorderableList.index >= 0 && activeMotionIndices[this.m_ReorderableList.index] < 0)
      {
        this.ShowHelp(area, EditorGUIUtility.TempContent("The selected child has no Motion assigned."));
      }
      else
      {
        if (this.m_WarningMessage == null)
          return;
        this.ShowHelp(area, EditorGUIUtility.TempContent(this.m_WarningMessage));
      }
    }

    private void ShowHelp(Rect area, GUIContent content)
    {
      float height = EditorStyles.helpBox.CalcHeight(content, area.width);
      GUI.Label(new Rect(area.x, area.y, area.width, height), content, EditorStyles.helpBox);
    }

    private void ValidatePositions()
    {
      this.m_WarningMessage = (string) null;
      Vector2[] motionPositions = this.GetMotionPositions();
      bool flag1 = (double) this.m_BlendRect.width == 0.0 || (double) this.m_BlendRect.height == 0.0;
      for (int index1 = 0; index1 < motionPositions.Length; ++index1)
      {
        for (int index2 = 0; index2 < index1 && !flag1; ++index2)
        {
          if ((double) ((motionPositions[index1] - motionPositions[index2]) / this.m_BlendRect.height).sqrMagnitude < 9.99999974737875E-05)
          {
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
        this.m_WarningMessage = "Two or more of the positions are too close to each other.";
      else if (this.m_BlendType.intValue == 1)
      {
        List<float> list = ((IEnumerable<Vector2>) motionPositions).Where<Vector2>((Func<Vector2, bool>) (e => e != Vector2.zero)).Select<Vector2, float>((Func<Vector2, float>) (e => Mathf.Atan2(e.y, e.x))).OrderBy<float, float>((Func<float, float>) (e => e)).ToList<float>();
        float num1 = 0.0f;
        float num2 = 180f;
        for (int index = 0; index < list.Count; ++index)
        {
          float num3 = list[(index + 1) % list.Count] - list[index];
          if (index == list.Count - 1)
            num3 += 6.283185f;
          if ((double) num3 > (double) num1)
            num1 = num3;
          if ((double) num3 < (double) num2)
            num2 = num3;
        }
        if ((double) num1 * 57.2957801818848 >= 180.0)
        {
          this.m_WarningMessage = "Simple Directional blend should have motions with directions less than 180 degrees apart.";
        }
        else
        {
          if ((double) num2 * 57.2957801818848 >= 2.0)
            return;
          this.m_WarningMessage = "Simple Directional blend should not have multiple motions in almost the same direction.";
        }
      }
      else
      {
        if (this.m_BlendType.intValue != 2)
          return;
        bool flag2 = false;
        for (int index = 0; index < motionPositions.Length; ++index)
        {
          if (motionPositions[index] == Vector2.zero)
          {
            flag2 = true;
            break;
          }
        }
        if (flag2)
          return;
        this.m_WarningMessage = "Freeform Directional blend should have one motion at position (0,0) to avoid discontinuities.";
      }
    }

    private void DrawWeightShape(Vector2 point, float weight, int pass)
    {
      if ((double) weight <= 0.0)
        return;
      point.x = Mathf.Round(point.x);
      point.y = Mathf.Round(point.y);
      float radius = 20f * Mathf.Sqrt(weight);
      Vector3[] vector3Array = new Vector3[this.kNumCirclePoints + 2];
      for (int index = 0; index < this.kNumCirclePoints; ++index)
      {
        float num = (float) index / (float) this.kNumCirclePoints;
        vector3Array[index + 1] = new Vector3(point.x + 0.5f, point.y + 0.5f, 0.0f) + new Vector3(Mathf.Sin((float) ((double) num * 2.0 * 3.14159274101257)), Mathf.Cos((float) ((double) num * 2.0 * 3.14159274101257)), 0.0f) * radius;
      }
      vector3Array[0] = vector3Array[this.kNumCirclePoints + 1] = (vector3Array[1] + vector3Array[this.kNumCirclePoints]) * 0.5f;
      if (pass == 0)
      {
        Handles.color = BlendTreeInspector.s_VisWeightShapeColor;
        Handles.DrawSolidDisc((Vector3) (point + new Vector2(0.5f, 0.5f)), -Vector3.forward, radius);
      }
      else
      {
        Handles.color = BlendTreeInspector.s_VisWeightLineColor;
        Handles.DrawAAPolyLine(vector3Array);
      }
    }

    private void DrawAnimation(float val, float min, float max, bool selected, Rect area)
    {
      float y = area.y;
      Rect position1 = new Rect(min, y, val - min, area.height);
      Rect position2 = new Rect(val, y, max - val, area.height);
      BlendTreeInspector.styles.triangleLeft.Draw(position1, selected, selected, false, false);
      BlendTreeInspector.styles.triangleRight.Draw(position2, selected, selected, false, false);
      --area.height;
      Color color = Handles.color;
      Handles.color = !selected ? new Color(1f, 1f, 1f, 0.4f) : new Color(1f, 1f, 1f, 0.6f);
      if (selected)
        Handles.DrawLine(new Vector3(val, y, 0.0f), new Vector3(val, y + area.height, 0.0f));
      Handles.DrawAAPolyLine(new Vector3[2]
      {
        new Vector3(min, y + area.height, 0.0f),
        new Vector3(val, y, 0.0f)
      });
      Handles.DrawAAPolyLine(new Vector3[2]
      {
        new Vector3(val, y, 0.0f),
        new Vector3(max, y + area.height, 0.0f)
      });
      Handles.color = color;
    }

    public void EndDragChild(ReorderableList list)
    {
      List<float> floatList = new List<float>();
      for (int index = 0; index < this.m_Childs.arraySize; ++index)
      {
        SerializedProperty propertyRelative = this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Threshold");
        floatList.Add(propertyRelative.floatValue);
      }
      floatList.Sort();
      for (int index = 0; index < this.m_Childs.arraySize; ++index)
        this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Threshold").floatValue = floatList[index];
      this.serializedObject.ApplyModifiedProperties();
    }

    private void DrawHeader(Rect headerRect)
    {
      headerRect.xMin += 14f;
      ++headerRect.y;
      headerRect.height = 16f;
      Rect[] rowRects = this.GetRowRects(headerRect, this.m_BlendType.intValue);
      int index1 = 0;
      rowRects[index1].xMin = rowRects[index1].xMin - 14f;
      GUI.Label(rowRects[index1], EditorGUIUtility.TempContent("Motion"), EditorStyles.label);
      int index2 = index1 + 1;
      if (this.m_Childs.arraySize < 1)
        return;
      int index3;
      if (this.m_BlendType.intValue == 0)
      {
        GUI.Label(rowRects[index2], EditorGUIUtility.TempContent("Threshold"), EditorStyles.label);
        index3 = index2 + 1;
      }
      else if (this.m_BlendType.intValue == 4)
      {
        GUI.Label(rowRects[index2], EditorGUIUtility.TempContent("Parameter"), EditorStyles.label);
        index3 = index2 + 1;
      }
      else
      {
        GUI.Label(rowRects[index2], EditorGUIUtility.TempContent("Pos X"), EditorStyles.label);
        int index4 = index2 + 1;
        GUI.Label(rowRects[index4], EditorGUIUtility.TempContent("Pos Y"), EditorStyles.label);
        index3 = index4 + 1;
      }
      GUI.Label(rowRects[index3], BlendTreeInspector.styles.speedIcon, BlendTreeInspector.styles.headerIcon);
      int index5 = index3 + 1;
      GUI.Label(rowRects[index5], BlendTreeInspector.styles.mirrorIcon, BlendTreeInspector.styles.headerIcon);
    }

    public void AddButton(Rect rect, ReorderableList list)
    {
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(new GUIContent("Add Motion Field"), false, new GenericMenu.MenuFunction(this.AddChildAnimation));
      genericMenu.AddItem(EditorGUIUtility.TempContent("New Blend Tree"), false, new GenericMenu.MenuFunction(this.AddBlendTreeCallback));
      genericMenu.Popup(rect, 0);
    }

    public static bool DeleteBlendTreeDialog(string toDelete)
    {
      return EditorUtility.DisplayDialog("Delete selected Blend Tree asset?", toDelete, "Delete", "Cancel");
    }

    public void RemoveButton(ReorderableList list)
    {
      Motion objectReferenceValue = this.m_Childs.GetArrayElementAtIndex(list.index).FindPropertyRelative("m_Motion").objectReferenceValue as Motion;
      if (!((UnityEngine.Object) objectReferenceValue == (UnityEngine.Object) null) && !BlendTreeInspector.DeleteBlendTreeDialog(objectReferenceValue.name))
        return;
      this.m_Childs.DeleteArrayElementAtIndex(list.index);
      if (list.index >= this.m_Childs.arraySize)
        list.index = this.m_Childs.arraySize - 1;
      this.SetMinMaxThresholds();
    }

    private Rect[] GetRowRects(Rect r, int blendType)
    {
      int num1 = blendType <= 0 || blendType >= 4 ? 1 : 2;
      Rect[] rectArray = new Rect[3 + num1];
      float width1 = r.width;
      float width2 = 16f;
      float num2 = width1 - width2 - (float) (24 + 4 * (num1 - 1));
      float width3 = (float) Mathf.FloorToInt(num2 * 0.2f);
      float width4 = num2 - width3 * (float) (num1 + 1);
      float x1 = r.x;
      int index1 = 0;
      rectArray[index1] = new Rect(x1, r.y, width4, r.height);
      float x2 = x1 + (width4 + 8f);
      int index2 = index1 + 1;
      for (int index3 = 0; index3 < num1; ++index3)
      {
        rectArray[index2] = new Rect(x2, r.y, width3, r.height);
        x2 += width3 + 4f;
        ++index2;
      }
      float x3 = x2 + 4f;
      rectArray[index2] = new Rect(x3, r.y, width3, r.height);
      float x4 = x3 + (width3 + 8f);
      int index4 = index2 + 1;
      rectArray[index4] = new Rect(x4, r.y, width2, r.height);
      return rectArray;
    }

    public void DrawChild(Rect r, int index, bool isActive, bool isFocused)
    {
      SerializedProperty arrayElementAtIndex = this.m_Childs.GetArrayElementAtIndex(index);
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("m_Motion");
      ++r.y;
      r.height = 16f;
      Rect[] rowRects = this.GetRowRects(r, this.m_BlendType.intValue);
      int index1 = 0;
      EditorGUI.BeginChangeCheck();
      Motion motion = this.m_BlendTree.children[index].motion;
      EditorGUI.PropertyField(rowRects[index1], propertyRelative1, GUIContent.none);
      int index2 = index1 + 1;
      if (EditorGUI.EndChangeCheck() && motion is UnityEditor.Animations.BlendTree && (UnityEngine.Object) motion != (UnityEngine.Object) (propertyRelative1.objectReferenceValue as Motion))
      {
        if (EditorUtility.DisplayDialog("Changing BlendTree will delete previous BlendTree", "You cannot undo this action.", "Delete", "Cancel"))
          MecanimUtilities.DestroyBlendTreeRecursive(motion as UnityEditor.Animations.BlendTree);
        else
          propertyRelative1.objectReferenceValue = (UnityEngine.Object) motion;
      }
      if (this.m_BlendType.intValue == 0)
      {
        SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("m_Threshold");
        EditorGUI.BeginDisabledGroup(this.m_UseAutomaticThresholds.boolValue);
        float result = propertyRelative2.floatValue;
        EditorGUI.BeginChangeCheck();
        string s = EditorGUI.DelayedTextFieldInternal(rowRects[index2], result.ToString(), "inftynaeINFTYNAE0123456789.,-", EditorStyles.textField);
        ++index2;
        if (EditorGUI.EndChangeCheck() && float.TryParse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result))
        {
          propertyRelative2.floatValue = result;
          this.serializedObject.ApplyModifiedProperties();
          this.m_BlendTree.SortChildren();
          this.SetMinMaxThresholds();
          GUI.changed = true;
        }
        EditorGUI.EndDisabledGroup();
      }
      else if (this.m_BlendType.intValue == 4)
      {
        List<string> stringList = this.CollectParameters(BlendTreeInspector.currentController);
        ChildMotion[] children = this.m_BlendTree.children;
        string directBlendParameter = children[index].directBlendParameter;
        EditorGUI.BeginChangeCheck();
        string str = EditorGUI.TextFieldDropDown(rowRects[index2], directBlendParameter, stringList.ToArray());
        ++index2;
        if (EditorGUI.EndChangeCheck())
        {
          children[index].directBlendParameter = str;
          this.m_BlendTree.children = children;
        }
      }
      else
      {
        SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("m_Position");
        Vector2 vector2Value = propertyRelative2.vector2Value;
        for (int index3 = 0; index3 < 2; ++index3)
        {
          EditorGUI.BeginChangeCheck();
          string s = EditorGUI.DelayedTextFieldInternal(rowRects[index2], vector2Value[index3].ToString(), "inftynaeINFTYNAE0123456789.,-", EditorStyles.textField);
          ++index2;
          float result;
          if (EditorGUI.EndChangeCheck() && float.TryParse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result))
          {
            vector2Value[index3] = Mathf.Clamp(result, -10000f, 10000f);
            propertyRelative2.vector2Value = vector2Value;
            this.serializedObject.ApplyModifiedProperties();
            GUI.changed = true;
          }
        }
      }
      if (propertyRelative1.objectReferenceValue is AnimationClip)
      {
        SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("m_TimeScale");
        EditorGUI.PropertyField(rowRects[index2], propertyRelative2, GUIContent.none);
      }
      else
      {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.IntField(rowRects[index2], 1);
        EditorGUI.EndDisabledGroup();
      }
      int index4 = index2 + 1;
      if (propertyRelative1.objectReferenceValue is AnimationClip && (propertyRelative1.objectReferenceValue as AnimationClip).isHumanMotion)
      {
        SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("m_Mirror");
        EditorGUI.PropertyField(rowRects[index4], propertyRelative2, GUIContent.none);
        arrayElementAtIndex.FindPropertyRelative("m_CycleOffset").floatValue = !propertyRelative2.boolValue ? 0.0f : 0.5f;
      }
      else
      {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.Toggle(rowRects[index4], false);
        EditorGUI.EndDisabledGroup();
      }
    }

    private bool AllMotions()
    {
      bool flag = true;
      for (int index = 0; index < this.m_Childs.arraySize && flag; ++index)
        flag = this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Motion").objectReferenceValue is AnimationClip;
      return flag;
    }

    private void AutoCompute()
    {
      if (this.m_BlendType.intValue == 0)
      {
        EditorGUILayout.PropertyField(this.m_UseAutomaticThresholds, EditorGUIUtility.TempContent("Automate Thresholds"), new GUILayoutOption[0]);
        this.m_ShowCompute.target = !this.m_UseAutomaticThresholds.boolValue;
      }
      else if (this.m_BlendType.intValue == 4)
        this.m_ShowCompute.target = false;
      else
        this.m_ShowCompute.target = true;
      this.m_ShowAdjust.target = this.AllMotions();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowCompute.faded))
      {
        Rect position = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), 0, this.ParameterCount != 1 ? EditorGUIUtility.TempContent("Compute Positions") : EditorGUIUtility.TempContent("Compute Thresholds"));
        if (EditorGUI.ButtonMouseDown(position, EditorGUIUtility.TempContent("Select"), FocusType.Passive, EditorStyles.popup))
        {
          GenericMenu menu = new GenericMenu();
          if (this.ParameterCount == 1)
          {
            this.AddComputeMenuItems(menu, string.Empty, BlendTreeInspector.ChildPropertyToCompute.Threshold);
          }
          else
          {
            menu.AddItem(new GUIContent("Velocity XZ"), false, new GenericMenu.MenuFunction(this.ComputePositionsFromVelocity));
            menu.AddItem(new GUIContent("Speed And Angular Speed"), false, new GenericMenu.MenuFunction(this.ComputePositionsFromSpeedAndAngularSpeed));
            this.AddComputeMenuItems(menu, "X Position From/", BlendTreeInspector.ChildPropertyToCompute.PositionX);
            this.AddComputeMenuItems(menu, "Y Position From/", BlendTreeInspector.ChildPropertyToCompute.PositionY);
          }
          menu.DropDown(position);
        }
      }
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowAdjust.faded))
      {
        Rect position = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), 0, EditorGUIUtility.TempContent("Adjust Time Scale"));
        if (EditorGUI.ButtonMouseDown(position, EditorGUIUtility.TempContent("Select"), FocusType.Passive, EditorStyles.popup))
        {
          GenericMenu genericMenu = new GenericMenu();
          genericMenu.AddItem(new GUIContent("Homogeneous Speed"), false, new GenericMenu.MenuFunction(this.ComputeTimeScaleFromSpeed));
          genericMenu.AddItem(new GUIContent("Reset Time Scale"), false, new GenericMenu.MenuFunction(this.ResetTimeScale));
          genericMenu.DropDown(position);
        }
      }
      EditorGUILayout.EndFadeGroup();
    }

    private void AddComputeMenuItems(GenericMenu menu, string menuItemPrefix, BlendTreeInspector.ChildPropertyToCompute prop)
    {
      menu.AddItem(new GUIContent(menuItemPrefix + "Speed"), false, new GenericMenu.MenuFunction2(this.ComputeFromSpeed), (object) prop);
      menu.AddItem(new GUIContent(menuItemPrefix + "Velocity X"), false, new GenericMenu.MenuFunction2(this.ComputeFromVelocityX), (object) prop);
      menu.AddItem(new GUIContent(menuItemPrefix + "Velocity Y"), false, new GenericMenu.MenuFunction2(this.ComputeFromVelocityY), (object) prop);
      menu.AddItem(new GUIContent(menuItemPrefix + "Velocity Z"), false, new GenericMenu.MenuFunction2(this.ComputeFromVelocityZ), (object) prop);
      menu.AddItem(new GUIContent(menuItemPrefix + "Angular Speed (Rad)"), false, new GenericMenu.MenuFunction2(this.ComputeFromAngularSpeedRadians), (object) prop);
      menu.AddItem(new GUIContent(menuItemPrefix + "Angular Speed (Deg)"), false, new GenericMenu.MenuFunction2(this.ComputeFromAngularSpeedDegrees), (object) prop);
    }

    private void ComputeFromSpeed(object obj)
    {
      BlendTreeInspector.ChildPropertyToCompute prop = (BlendTreeInspector.ChildPropertyToCompute) obj;
      this.ComputeProperty((BlendTreeInspector.GetFloatFromMotion) ((m, mirrorMultiplier) => m.apparentSpeed), prop);
    }

    private void ComputeFromVelocityX(object obj)
    {
      BlendTreeInspector.ChildPropertyToCompute prop = (BlendTreeInspector.ChildPropertyToCompute) obj;
      this.ComputeProperty((BlendTreeInspector.GetFloatFromMotion) ((m, mirrorMultiplier) => m.averageSpeed.x * mirrorMultiplier), prop);
    }

    private void ComputeFromVelocityY(object obj)
    {
      BlendTreeInspector.ChildPropertyToCompute prop = (BlendTreeInspector.ChildPropertyToCompute) obj;
      this.ComputeProperty((BlendTreeInspector.GetFloatFromMotion) ((m, mirrorMultiplier) => m.averageSpeed.y), prop);
    }

    private void ComputeFromVelocityZ(object obj)
    {
      BlendTreeInspector.ChildPropertyToCompute prop = (BlendTreeInspector.ChildPropertyToCompute) obj;
      this.ComputeProperty((BlendTreeInspector.GetFloatFromMotion) ((m, mirrorMultiplier) => m.averageSpeed.z), prop);
    }

    private void ComputeFromAngularSpeedDegrees(object obj)
    {
      BlendTreeInspector.ChildPropertyToCompute prop = (BlendTreeInspector.ChildPropertyToCompute) obj;
      this.ComputeProperty((BlendTreeInspector.GetFloatFromMotion) ((m, mirrorMultiplier) => (float) ((double) m.averageAngularSpeed * 180.0 / 3.14159274101257) * mirrorMultiplier), prop);
    }

    private void ComputeFromAngularSpeedRadians(object obj)
    {
      BlendTreeInspector.ChildPropertyToCompute prop = (BlendTreeInspector.ChildPropertyToCompute) obj;
      this.ComputeProperty((BlendTreeInspector.GetFloatFromMotion) ((m, mirrorMultiplier) => m.averageAngularSpeed * mirrorMultiplier), prop);
    }

    private void ComputeProperty(BlendTreeInspector.GetFloatFromMotion func, BlendTreeInspector.ChildPropertyToCompute prop)
    {
      float num1 = 0.0f;
      float[] numArray = new float[this.m_Childs.arraySize];
      this.m_UseAutomaticThresholds.boolValue = false;
      for (int index = 0; index < this.m_Childs.arraySize; ++index)
      {
        SerializedProperty propertyRelative1 = this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Motion");
        SerializedProperty propertyRelative2 = this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Mirror");
        Motion objectReferenceValue = propertyRelative1.objectReferenceValue as Motion;
        if ((UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null)
        {
          float num2 = func(objectReferenceValue, !propertyRelative2.boolValue ? 1f : -1f);
          numArray[index] = num2;
          num1 += num2;
          if (prop == BlendTreeInspector.ChildPropertyToCompute.Threshold)
          {
            this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Threshold").floatValue = num2;
          }
          else
          {
            SerializedProperty propertyRelative3 = this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Position");
            Vector2 vector2Value = propertyRelative3.vector2Value;
            if (prop == BlendTreeInspector.ChildPropertyToCompute.PositionX)
              vector2Value.x = num2;
            else
              vector2Value.y = num2;
            propertyRelative3.vector2Value = vector2Value;
          }
        }
      }
      float num3 = num1 / (float) this.m_Childs.arraySize;
      float num4 = 0.0f;
      for (int index = 0; index < numArray.Length; ++index)
        num4 += Mathf.Pow(numArray[index] - num3, 2f);
      if ((double) (num4 / (float) numArray.Length) < (double) Mathf.Epsilon)
      {
        UnityEngine.Debug.LogWarning((object) ("Could not compute threshold for '" + this.m_BlendTree.name + "' there is not enough data"));
        this.m_SerializedObject.Update();
      }
      else
      {
        this.m_SerializedObject.ApplyModifiedProperties();
        if (prop != BlendTreeInspector.ChildPropertyToCompute.Threshold)
          return;
        this.SortByThreshold();
        this.SetMinMaxThreshold();
      }
    }

    private void ComputePositionsFromVelocity()
    {
      this.ComputeFromVelocityX((object) BlendTreeInspector.ChildPropertyToCompute.PositionX);
      this.ComputeFromVelocityZ((object) BlendTreeInspector.ChildPropertyToCompute.PositionY);
    }

    private void ComputePositionsFromSpeedAndAngularSpeed()
    {
      this.ComputeFromAngularSpeedRadians((object) BlendTreeInspector.ChildPropertyToCompute.PositionX);
      this.ComputeFromSpeed((object) BlendTreeInspector.ChildPropertyToCompute.PositionY);
    }

    private void ComputeTimeScaleFromSpeed()
    {
      float apparentSpeed = this.m_BlendTree.apparentSpeed;
      for (int index = 0; index < this.m_Childs.arraySize; ++index)
      {
        AnimationClip objectReferenceValue = this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Motion").objectReferenceValue as AnimationClip;
        if ((UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null)
        {
          if (!objectReferenceValue.legacy)
          {
            if ((double) objectReferenceValue.apparentSpeed < (double) Mathf.Epsilon)
              UnityEngine.Debug.LogWarning((object) ("Could not adjust time scale for " + objectReferenceValue.name + " because it has no speed"));
            else
              this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_TimeScale").floatValue = apparentSpeed / objectReferenceValue.apparentSpeed;
          }
          else
            UnityEngine.Debug.LogWarning((object) ("Could not adjust time scale for " + objectReferenceValue.name + " because it is not a muscle clip"));
        }
      }
      this.m_SerializedObject.ApplyModifiedProperties();
    }

    private void ResetTimeScale()
    {
      for (int index = 0; index < this.m_Childs.arraySize; ++index)
      {
        AnimationClip objectReferenceValue = this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Motion").objectReferenceValue as AnimationClip;
        if ((UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null && !objectReferenceValue.legacy)
          this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_TimeScale").floatValue = 1f;
      }
      this.m_SerializedObject.ApplyModifiedProperties();
    }

    private void SortByThreshold()
    {
      this.m_SerializedObject.Update();
      for (int dstIndex = 0; dstIndex < this.m_Childs.arraySize; ++dstIndex)
      {
        float num = float.PositiveInfinity;
        int srcIndex = -1;
        for (int index = dstIndex; index < this.m_Childs.arraySize; ++index)
        {
          float floatValue = this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Threshold").floatValue;
          if ((double) floatValue < (double) num)
          {
            num = floatValue;
            srcIndex = index;
          }
        }
        if (srcIndex != dstIndex)
          this.m_Childs.MoveArrayElement(srcIndex, dstIndex);
      }
      this.m_SerializedObject.ApplyModifiedProperties();
    }

    private void SetMinMaxThreshold()
    {
      this.m_SerializedObject.Update();
      SerializedProperty propertyRelative1 = this.m_Childs.GetArrayElementAtIndex(0).FindPropertyRelative("m_Threshold");
      SerializedProperty propertyRelative2 = this.m_Childs.GetArrayElementAtIndex(this.m_Childs.arraySize - 1).FindPropertyRelative("m_Threshold");
      this.m_MinThreshold.floatValue = Mathf.Min(propertyRelative1.floatValue, propertyRelative2.floatValue);
      this.m_MaxThreshold.floatValue = Mathf.Max(propertyRelative1.floatValue, propertyRelative2.floatValue);
      this.m_SerializedObject.ApplyModifiedProperties();
    }

    private void AddChildAnimation()
    {
      this.m_BlendTree.AddChild((Motion) null);
      int length = this.m_BlendTree.children.Length;
      this.m_BlendTree.SetDirectBlendTreeParameter(length - 1, BlendTreeInspector.currentController.GetDefaultBlendTreeParameter());
      this.SetNewThresholdAndPosition(length - 1);
      this.m_ReorderableList.index = length - 1;
    }

    private void AddBlendTreeCallback()
    {
      UnityEditor.Animations.BlendTree blendTreeChild = this.m_BlendTree.CreateBlendTreeChild(0.0f);
      int length = this.m_BlendTree.children.Length;
      if ((UnityEngine.Object) BlendTreeInspector.currentController != (UnityEngine.Object) null)
      {
        blendTreeChild.blendParameter = this.m_BlendTree.blendParameter;
        this.m_BlendTree.SetDirectBlendTreeParameter(length - 1, BlendTreeInspector.currentController.GetDefaultBlendTreeParameter());
      }
      this.SetNewThresholdAndPosition(length - 1);
      this.m_ReorderableList.index = this.m_Childs.arraySize - 1;
    }

    private void SetNewThresholdAndPosition(int index)
    {
      this.serializedObject.Update();
      if (!this.m_UseAutomaticThresholds.boolValue)
      {
        float num;
        if (this.m_Childs.arraySize >= 3 && index == this.m_Childs.arraySize - 1)
        {
          float floatValue1 = this.m_Childs.GetArrayElementAtIndex(index - 2).FindPropertyRelative("m_Threshold").floatValue;
          float floatValue2 = this.m_Childs.GetArrayElementAtIndex(index - 1).FindPropertyRelative("m_Threshold").floatValue;
          num = floatValue2 + (floatValue2 - floatValue1);
        }
        else
          num = this.m_Childs.arraySize != 1 ? this.m_Childs.GetArrayElementAtIndex(this.m_Childs.arraySize - 1).FindPropertyRelative("m_Threshold").floatValue + 1f : 0.0f;
        this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Threshold").floatValue = num;
        this.SetMinMaxThresholds();
      }
      Vector2 b = Vector2.zero;
      if (this.m_Childs.arraySize >= 1)
      {
        Vector2 center = this.m_BlendRect.center;
        Vector2[] motionPositions = this.GetMotionPositions();
        float num = this.m_BlendRect.width * 0.07f;
        for (int index1 = 0; index1 < 24; ++index1)
        {
          bool flag = true;
          for (int index2 = 0; index2 < motionPositions.Length && flag; ++index2)
          {
            if (index2 != index && (double) Vector2.Distance(motionPositions[index2], b) < (double) num)
              flag = false;
          }
          if (!flag)
          {
            float f = (float) (index1 * 15) * ((float) Math.PI / 180f);
            b = center + new Vector2(-Mathf.Cos(f), Mathf.Sin(f)) * 0.37f * this.m_BlendRect.width;
            b.x = MathUtils.RoundBasedOnMinimumDifference(b.x, this.m_BlendRect.width * 0.005f);
            b.y = MathUtils.RoundBasedOnMinimumDifference(b.y, this.m_BlendRect.width * 0.005f);
          }
          else
            break;
        }
      }
      this.m_Childs.GetArrayElementAtIndex(index).FindPropertyRelative("m_Position").vector2Value = b;
      this.serializedObject.ApplyModifiedProperties();
    }

    public override bool HasPreviewGUI()
    {
      if (this.m_PreviewBlendTree != null)
        return this.m_PreviewBlendTree.HasPreviewGUI();
      return false;
    }

    public override void OnPreviewSettings()
    {
      if (this.m_PreviewBlendTree == null)
        return;
      this.m_PreviewBlendTree.OnPreviewSettings();
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      if (this.m_PreviewBlendTree == null)
        return;
      this.m_PreviewBlendTree.OnInteractivePreviewGUI(r, background);
    }

    public void OnDisable()
    {
      if (this.m_PreviewBlendTree == null)
        return;
      this.m_PreviewBlendTree.OnDisable();
    }

    public void OnDestroy()
    {
      if (this.m_PreviewBlendTree != null)
        this.m_PreviewBlendTree.OnDestroy();
      if (this.m_VisBlendTree != null)
        this.m_VisBlendTree.Destroy();
      if ((UnityEngine.Object) this.m_VisInstance != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_VisInstance);
      for (int index = 0; index < this.m_WeightTexs.Count; ++index)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_WeightTexs[index]);
      if (!((UnityEngine.Object) this.m_BlendTex != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_BlendTex);
    }

    private class Styles
    {
      public readonly GUIStyle background = (GUIStyle) "MeBlendBackground";
      public readonly GUIStyle triangleLeft = (GUIStyle) "MeBlendTriangleLeft";
      public readonly GUIStyle triangleRight = (GUIStyle) "MeBlendTriangleRight";
      public readonly GUIStyle blendPosition = (GUIStyle) "MeBlendPosition";
      public GUIStyle clickDragFloatFieldLeft = new GUIStyle(EditorStyles.miniTextField);
      public GUIStyle clickDragFloatFieldRight = new GUIStyle(EditorStyles.miniTextField);
      public GUIStyle clickDragFloatLabelLeft = new GUIStyle(EditorStyles.miniLabel);
      public GUIStyle clickDragFloatLabelRight = new GUIStyle(EditorStyles.miniLabel);
      public GUIStyle headerIcon = new GUIStyle();
      public GUIStyle errorStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
      public GUIContent speedIcon = new GUIContent(EditorGUIUtility.IconContent("SpeedScale"));
      public GUIContent mirrorIcon = new GUIContent(EditorGUIUtility.IconContent("Mirror"));
      public Texture2D pointIcon = EditorGUIUtility.LoadIcon("blendKey");
      public Texture2D pointIconSelected = EditorGUIUtility.LoadIcon("blendKeySelected");
      public Texture2D pointIconOverlay = EditorGUIUtility.LoadIcon("blendKeyOverlay");
      public Texture2D samplerIcon = EditorGUIUtility.LoadIcon("blendSampler");

      public Styles()
      {
        this.errorStyle.alignment = TextAnchor.MiddleCenter;
        this.speedIcon.tooltip = "Changes animation speed.";
        this.mirrorIcon.tooltip = "Mirror animation.";
        this.headerIcon.alignment = TextAnchor.MiddleCenter;
        this.clickDragFloatFieldLeft.alignment = TextAnchor.MiddleLeft;
        this.clickDragFloatFieldRight.alignment = TextAnchor.MiddleRight;
        this.clickDragFloatLabelLeft.alignment = TextAnchor.MiddleLeft;
        this.clickDragFloatLabelRight.alignment = TextAnchor.MiddleRight;
      }
    }

    private enum ChildPropertyToCompute
    {
      Threshold,
      PositionX,
      PositionY,
    }

    private delegate float GetFloatFromMotion(Motion motion, float mirrorMultiplier);
  }
}

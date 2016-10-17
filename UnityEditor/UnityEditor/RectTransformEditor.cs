// Decompiled with JetBrains decompiler
// Type: UnityEditor.RectTransformEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (RectTransform))]
  [CanEditMultipleObjects]
  internal class RectTransformEditor : Editor
  {
    private static Vector2 kShadowOffset = new Vector2(1f, -1f);
    private static Color kShadowColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    private static float kDropdownSize = 49f;
    private static Color kRectInParentSpaceColor = new Color(1f, 1f, 1f, 0.4f);
    private static Color kParentColor = new Color(1f, 1f, 1f, 0.6f);
    private static Color kSiblingColor = new Color(1f, 1f, 1f, 0.2f);
    private static Color kAnchorColor = new Color(1f, 1f, 1f, 1f);
    private static Color kAnchorLineColor = new Color(1f, 1f, 1f, 0.6f);
    private static Vector3[] s_Corners = new Vector3[4];
    private static int s_FoldoutHash = "Foldout".GetHashCode();
    private static int s_FloatFieldHash = "EditorTextField".GetHashCode();
    private static int s_ParentRectPreviewHandlesHash = "ParentRectPreviewDragHandles".GetHashCode();
    private static GUIContent[] s_XYLabels = new GUIContent[2]{ new GUIContent("X"), new GUIContent("Y") };
    private static GUIContent[] s_XYZLabels = new GUIContent[3]{ new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z") };
    private static bool[] s_ScaleDisabledMask = new bool[3];
    private static RectTransformEditor.AnchorFusedState s_AnchorFusedState = RectTransformEditor.AnchorFusedState.None;
    private static float s_ParentDragTime = 0.0f;
    private static float s_ParentDragId = 0.0f;
    private static Rect s_ParentDragOrigRect = new Rect();
    private static Rect s_ParentDragPreviewRect = new Rect();
    private static RectTransform s_ParentDragRectTransform = (RectTransform) null;
    private Dictionary<int, AnimBool> m_KeyboardControlIDs = new Dictionary<int, AnimBool>();
    private AnimBool m_ChangingAnchors = new AnimBool();
    private AnimBool m_ChangingPivot = new AnimBool();
    private AnimBool m_ChangingWidth = new AnimBool();
    private AnimBool m_ChangingHeight = new AnimBool();
    private AnimBool m_ChangingPosX = new AnimBool();
    private AnimBool m_ChangingPosY = new AnimBool();
    private AnimBool m_ChangingLeft = new AnimBool();
    private AnimBool m_ChangingRight = new AnimBool();
    private AnimBool m_ChangingTop = new AnimBool();
    private AnimBool m_ChangingBottom = new AnimBool();
    private const string kShowAnchorPropsPrefName = "RectTransformEditor.showAnchorProperties";
    private const string kLockRectPrefName = "RectTransformEditor.lockRect";
    private const float kDottedLineSize = 5f;
    private static RectTransformEditor.Styles s_Styles;
    private static Vector3 s_StartMouseWorldPos;
    private static Vector3 s_StartPosition;
    private static Vector2 s_StartMousePos;
    private static bool s_DragAnchorsTogether;
    private static Vector2 s_StartDragAnchorMin;
    private static Vector2 s_StartDragAnchorMax;
    private SerializedProperty m_AnchorMin;
    private SerializedProperty m_AnchorMax;
    private SerializedProperty m_AnchoredPosition;
    private SerializedProperty m_SizeDelta;
    private SerializedProperty m_Pivot;
    private SerializedProperty m_LocalPositionZ;
    private SerializedProperty m_LocalScale;
    private TransformRotationGUI m_RotationGUI;
    private bool m_ShowLayoutOptions;
    private bool m_RawEditMode;
    private int m_TargetCount;
    private LayoutDropdownWindow m_DropdownWindow;

    private static RectTransformEditor.Styles styles
    {
      get
      {
        if (RectTransformEditor.s_Styles == null)
          RectTransformEditor.s_Styles = new RectTransformEditor.Styles();
        return RectTransformEditor.s_Styles;
      }
    }

    private void OnEnable()
    {
      this.m_AnchorMin = this.serializedObject.FindProperty("m_AnchorMin");
      this.m_AnchorMax = this.serializedObject.FindProperty("m_AnchorMax");
      this.m_AnchoredPosition = this.serializedObject.FindProperty("m_AnchoredPosition");
      this.m_SizeDelta = this.serializedObject.FindProperty("m_SizeDelta");
      this.m_Pivot = this.serializedObject.FindProperty("m_Pivot");
      this.m_TargetCount = this.targets.Length;
      this.m_LocalPositionZ = this.serializedObject.FindProperty("m_LocalPosition.z");
      this.m_LocalScale = this.serializedObject.FindProperty("m_LocalScale");
      if (this.m_RotationGUI == null)
        this.m_RotationGUI = new TransformRotationGUI();
      this.m_RotationGUI.OnEnable(this.serializedObject.FindProperty("m_LocalRotation"), new GUIContent("Rotation"));
      this.m_ShowLayoutOptions = EditorPrefs.GetBool("RectTransformEditor.showAnchorProperties", false);
      this.m_RawEditMode = EditorPrefs.GetBool("RectTransformEditor.lockRect", false);
      this.m_ChangingAnchors.valueChanged.AddListener(new UnityAction(this.RepaintScene));
      this.m_ChangingPivot.valueChanged.AddListener(new UnityAction(this.RepaintScene));
      this.m_ChangingWidth.valueChanged.AddListener(new UnityAction(this.RepaintScene));
      this.m_ChangingHeight.valueChanged.AddListener(new UnityAction(this.RepaintScene));
      this.m_ChangingPosX.valueChanged.AddListener(new UnityAction(this.RepaintScene));
      this.m_ChangingPosY.valueChanged.AddListener(new UnityAction(this.RepaintScene));
      this.m_ChangingLeft.valueChanged.AddListener(new UnityAction(this.RepaintScene));
      this.m_ChangingRight.valueChanged.AddListener(new UnityAction(this.RepaintScene));
      this.m_ChangingTop.valueChanged.AddListener(new UnityAction(this.RepaintScene));
      this.m_ChangingBottom.valueChanged.AddListener(new UnityAction(this.RepaintScene));
      ManipulationToolUtility.handleDragChange += new ManipulationToolUtility.HandleDragChange(this.HandleDragChange);
    }

    private void OnDisable()
    {
      this.m_ChangingAnchors.valueChanged.RemoveListener(new UnityAction(this.RepaintScene));
      this.m_ChangingPivot.valueChanged.RemoveListener(new UnityAction(this.RepaintScene));
      this.m_ChangingWidth.valueChanged.RemoveListener(new UnityAction(this.RepaintScene));
      this.m_ChangingHeight.valueChanged.RemoveListener(new UnityAction(this.RepaintScene));
      this.m_ChangingPosX.valueChanged.RemoveListener(new UnityAction(this.RepaintScene));
      this.m_ChangingPosY.valueChanged.RemoveListener(new UnityAction(this.RepaintScene));
      this.m_ChangingLeft.valueChanged.RemoveListener(new UnityAction(this.RepaintScene));
      this.m_ChangingRight.valueChanged.RemoveListener(new UnityAction(this.RepaintScene));
      this.m_ChangingTop.valueChanged.RemoveListener(new UnityAction(this.RepaintScene));
      this.m_ChangingBottom.valueChanged.RemoveListener(new UnityAction(this.RepaintScene));
      ManipulationToolUtility.handleDragChange -= new ManipulationToolUtility.HandleDragChange(this.HandleDragChange);
      if (this.m_DropdownWindow == null || !((UnityEngine.Object) this.m_DropdownWindow.editorWindow != (UnityEngine.Object) null))
        return;
      this.m_DropdownWindow.editorWindow.Close();
    }

    private void HandleDragChange(string handleName, bool dragging)
    {
      string key = handleName;
      AnimBool animBool;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (RectTransformEditor.\u003C\u003Ef__switch\u0024map1D == null)
        {
          // ISSUE: reference to a compiler-generated field
          RectTransformEditor.\u003C\u003Ef__switch\u0024map1D = new Dictionary<string, int>(9)
          {
            {
              "ChangingLeft",
              0
            },
            {
              "ChangingRight",
              1
            },
            {
              "ChangingPosY",
              2
            },
            {
              "ChangingWidth",
              3
            },
            {
              "ChangingBottom",
              4
            },
            {
              "ChangingTop",
              5
            },
            {
              "ChangingPosX",
              6
            },
            {
              "ChangingHeight",
              7
            },
            {
              "ChangingPivot",
              8
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (RectTransformEditor.\u003C\u003Ef__switch\u0024map1D.TryGetValue(key, out num))
        {
          switch (num)
          {
            case 0:
              animBool = this.m_ChangingLeft;
              goto label_16;
            case 1:
              animBool = this.m_ChangingRight;
              goto label_16;
            case 2:
              animBool = this.m_ChangingPosY;
              goto label_16;
            case 3:
              animBool = this.m_ChangingWidth;
              goto label_16;
            case 4:
              animBool = this.m_ChangingBottom;
              goto label_16;
            case 5:
              animBool = this.m_ChangingTop;
              goto label_16;
            case 6:
              animBool = this.m_ChangingPosX;
              goto label_16;
            case 7:
              animBool = this.m_ChangingHeight;
              goto label_16;
            case 8:
              animBool = this.m_ChangingPivot;
              goto label_16;
          }
        }
      }
      animBool = (AnimBool) null;
label_16:
      if (animBool == null)
        return;
      animBool.target = dragging;
    }

    private void SetFadingBasedOnMouseDownUp(ref AnimBool animBool, Event eventBefore)
    {
      if (eventBefore.type == EventType.MouseDrag && Event.current.type != EventType.MouseDrag)
      {
        animBool.value = true;
      }
      else
      {
        if (eventBefore.type != EventType.MouseUp || Event.current.type == EventType.MouseUp)
          return;
        animBool.target = false;
      }
    }

    private void SetFadingBasedOnControlID(ref AnimBool animBool, int id)
    {
      GUIView guiView = !((UnityEngine.Object) EditorWindow.focusedWindow == (UnityEngine.Object) null) ? (GUIView) EditorWindow.focusedWindow.m_Parent : (GUIView) null;
      if (GUIUtility.keyboardControl == id && (UnityEngine.Object) GUIView.current == (UnityEngine.Object) guiView)
      {
        animBool.value = true;
        this.m_KeyboardControlIDs[id] = animBool;
      }
      else
      {
        if (GUIUtility.keyboardControl == id && !((UnityEngine.Object) GUIView.current != (UnityEngine.Object) guiView) || !this.m_KeyboardControlIDs.ContainsKey(id))
          return;
        this.m_KeyboardControlIDs.Remove(id);
        if (this.m_KeyboardControlIDs.ContainsValue(animBool))
          return;
        animBool.target = false;
      }
    }

    private void RepaintScene()
    {
      SceneView.RepaintAll();
    }

    private static bool ShouldDoIntSnapping(RectTransform rect)
    {
      Canvas componentInParent = rect.gameObject.GetComponentInParent<Canvas>();
      if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null)
        return componentInParent.renderMode != UnityEngine.RenderMode.WorldSpace;
      return false;
    }

    public override void OnInspectorGUI()
    {
      if (!EditorGUIUtility.wideMode)
      {
        EditorGUIUtility.wideMode = true;
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212f;
      }
      bool flag = false;
      bool anyDrivenX = false;
      bool anyDrivenY = false;
      bool anyWithoutParent = false;
      foreach (RectTransform target in this.targets)
      {
        if (target.drivenByObject != (UnityEngine.Object) null)
        {
          flag = true;
          if ((target.drivenProperties & (DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.SizeDeltaX)) != DrivenTransformProperties.None)
            anyDrivenX = true;
          if ((target.drivenProperties & (DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.SizeDeltaY)) != DrivenTransformProperties.None)
            anyDrivenY = true;
        }
        PrefabType prefabType = PrefabUtility.GetPrefabType((UnityEngine.Object) target.gameObject);
        if (((UnityEngine.Object) target.transform.parent == (UnityEngine.Object) null || (UnityEngine.Object) target.transform.parent.GetComponent<RectTransform>() == (UnityEngine.Object) null) && (prefabType != PrefabType.Prefab && prefabType != PrefabType.ModelPrefab))
          anyWithoutParent = true;
      }
      if (flag)
      {
        if (this.targets.Length == 1)
          EditorGUILayout.HelpBox("Some values driven by " + (this.target as RectTransform).drivenByObject.GetType().Name + ".", MessageType.None);
        else
          EditorGUILayout.HelpBox("Some values in some or all objects are driven.", MessageType.None);
      }
      this.serializedObject.Update();
      this.LayoutDropdownButton(anyWithoutParent);
      this.SmartPositionAndSizeFields(anyWithoutParent, anyDrivenX, anyDrivenY);
      this.SmartAnchorFields();
      this.SmartPivotField();
      EditorGUILayout.Space();
      this.m_RotationGUI.RotationField(((IEnumerable<UnityEngine.Object>) this.targets).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => ((x as RectTransform).drivenProperties & DrivenTransformProperties.Rotation) != DrivenTransformProperties.None)));
      RectTransformEditor.s_ScaleDisabledMask[0] = ((IEnumerable<UnityEngine.Object>) this.targets).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => ((x as RectTransform).drivenProperties & DrivenTransformProperties.ScaleX) != DrivenTransformProperties.None));
      RectTransformEditor.s_ScaleDisabledMask[1] = ((IEnumerable<UnityEngine.Object>) this.targets).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => ((x as RectTransform).drivenProperties & DrivenTransformProperties.ScaleY) != DrivenTransformProperties.None));
      RectTransformEditor.s_ScaleDisabledMask[2] = ((IEnumerable<UnityEngine.Object>) this.targets).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => ((x as RectTransform).drivenProperties & DrivenTransformProperties.ScaleZ) != DrivenTransformProperties.None));
      RectTransformEditor.Vector3FieldWithDisabledMash(EditorGUILayout.GetControlRect(), this.m_LocalScale, RectTransformEditor.styles.transformScaleContent, RectTransformEditor.s_ScaleDisabledMask);
      this.serializedObject.ApplyModifiedProperties();
    }

    private static void Vector3FieldWithDisabledMash(Rect position, SerializedProperty property, GUIContent label, bool[] disabledMask)
    {
      int controlId = GUIUtility.GetControlID(RectTransformEditor.s_FoldoutHash, EditorGUIUtility.native, position);
      position = EditorGUI.MultiFieldPrefixLabel(position, controlId, label, 3);
      position.height = EditorGUIUtility.singleLineHeight;
      SerializedProperty valuesIterator = property.Copy();
      valuesIterator.NextVisible(true);
      EditorGUI.MultiPropertyField(position, RectTransformEditor.s_XYZLabels, valuesIterator, 13f, disabledMask);
    }

    private void LayoutDropdownButton(bool anyWithoutParent)
    {
      Rect rect = GUILayoutUtility.GetRect(0.0f, 0.0f);
      rect.x += 2f;
      rect.y += 17f;
      rect.height = RectTransformEditor.kDropdownSize;
      rect.width = RectTransformEditor.kDropdownSize;
      EditorGUI.BeginDisabledGroup(anyWithoutParent);
      Color color = GUI.color;
      GUI.color = new Color(1f, 1f, 1f, 0.6f) * color;
      if (EditorGUI.ButtonMouseDown(rect, GUIContent.none, FocusType.Passive, (GUIStyle) "box"))
      {
        GUIUtility.keyboardControl = 0;
        this.m_DropdownWindow = new LayoutDropdownWindow(this.serializedObject);
        PopupWindow.Show(rect, (PopupWindowContent) this.m_DropdownWindow);
      }
      GUI.color = color;
      EditorGUI.EndDisabledGroup();
      if (anyWithoutParent)
        return;
      LayoutDropdownWindow.DrawLayoutMode(new RectOffset(7, 7, 7, 7).Remove(rect), this.m_AnchorMin, this.m_AnchorMax, this.m_AnchoredPosition, this.m_SizeDelta);
      LayoutDropdownWindow.DrawLayoutModeHeadersOutsideRect(rect, this.m_AnchorMin, this.m_AnchorMax, this.m_AnchoredPosition, this.m_SizeDelta);
    }

    private void SmartPositionAndSizeFields(bool anyWithoutParent, bool anyDrivenX, bool anyDrivenY)
    {
      Rect controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight * 4f, new GUILayoutOption[0]);
      controlRect.height = EditorGUIUtility.singleLineHeight * 2f;
      bool flag1 = ((IEnumerable<UnityEngine.Object>) this.targets).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => (double) (x as RectTransform).anchorMin.x != (double) (x as RectTransform).anchorMax.x));
      bool flag2 = ((IEnumerable<UnityEngine.Object>) this.targets).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => (double) (x as RectTransform).anchorMin.y != (double) (x as RectTransform).anchorMax.y));
      bool flag3 = ((IEnumerable<UnityEngine.Object>) this.targets).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => (double) (x as RectTransform).anchorMin.x == (double) (x as RectTransform).anchorMax.x));
      bool flag4 = ((IEnumerable<UnityEngine.Object>) this.targets).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (x => (double) (x as RectTransform).anchorMin.y == (double) (x as RectTransform).anchorMax.y));
      Rect columnRect1 = this.GetColumnRect(controlRect, 0);
      if (flag3 || anyWithoutParent || anyDrivenX)
      {
        EditorGUI.BeginProperty(columnRect1, (GUIContent) null, this.m_AnchoredPosition.FindPropertyRelative("x"));
        this.FloatFieldLabelAbove(columnRect1, (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.anchoredPosition.x), (RectTransformEditor.FloatSetter) ((rectTransform, val) => rectTransform.anchoredPosition = new Vector2(val, rectTransform.anchoredPosition.y)), DrivenTransformProperties.AnchoredPositionX, new GUIContent("Pos X"));
        this.SetFadingBasedOnControlID(ref this.m_ChangingPosX, EditorGUIUtility.s_LastControlID);
        EditorGUI.EndProperty();
      }
      else
      {
        EditorGUI.BeginProperty(columnRect1, (GUIContent) null, this.m_AnchoredPosition.FindPropertyRelative("x"));
        EditorGUI.BeginProperty(columnRect1, (GUIContent) null, this.m_SizeDelta.FindPropertyRelative("x"));
        this.FloatFieldLabelAbove(columnRect1, (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.offsetMin.x), (RectTransformEditor.FloatSetter) ((rectTransform, val) => rectTransform.offsetMin = new Vector2(val, rectTransform.offsetMin.y)), DrivenTransformProperties.None, new GUIContent("Left"));
        this.SetFadingBasedOnControlID(ref this.m_ChangingLeft, EditorGUIUtility.s_LastControlID);
        EditorGUI.EndProperty();
        EditorGUI.EndProperty();
      }
      Rect columnRect2 = this.GetColumnRect(controlRect, 1);
      if (flag4 || anyWithoutParent || anyDrivenY)
      {
        EditorGUI.BeginProperty(columnRect2, (GUIContent) null, this.m_AnchoredPosition.FindPropertyRelative("y"));
        this.FloatFieldLabelAbove(columnRect2, (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.anchoredPosition.y), (RectTransformEditor.FloatSetter) ((rectTransform, val) => rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, val)), DrivenTransformProperties.AnchoredPositionY, new GUIContent("Pos Y"));
        this.SetFadingBasedOnControlID(ref this.m_ChangingPosY, EditorGUIUtility.s_LastControlID);
        EditorGUI.EndProperty();
      }
      else
      {
        EditorGUI.BeginProperty(columnRect2, (GUIContent) null, this.m_AnchoredPosition.FindPropertyRelative("y"));
        EditorGUI.BeginProperty(columnRect2, (GUIContent) null, this.m_SizeDelta.FindPropertyRelative("y"));
        this.FloatFieldLabelAbove(columnRect2, (RectTransformEditor.FloatGetter) (rectTransform => -rectTransform.offsetMax.y), (RectTransformEditor.FloatSetter) ((rectTransform, val) => rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -val)), DrivenTransformProperties.None, new GUIContent("Top"));
        this.SetFadingBasedOnControlID(ref this.m_ChangingTop, EditorGUIUtility.s_LastControlID);
        EditorGUI.EndProperty();
        EditorGUI.EndProperty();
      }
      Rect columnRect3 = this.GetColumnRect(controlRect, 2);
      EditorGUI.BeginProperty(columnRect3, (GUIContent) null, this.m_LocalPositionZ);
      this.FloatFieldLabelAbove(columnRect3, (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.transform.localPosition.z), (RectTransformEditor.FloatSetter) ((rectTransform, val) => rectTransform.transform.localPosition = new Vector3(rectTransform.transform.localPosition.x, rectTransform.transform.localPosition.y, val)), DrivenTransformProperties.AnchoredPositionZ, new GUIContent("Pos Z"));
      EditorGUI.EndProperty();
      controlRect.y += EditorGUIUtility.singleLineHeight * 2f;
      Rect columnRect4 = this.GetColumnRect(controlRect, 0);
      if (flag3 || anyWithoutParent || anyDrivenX)
      {
        EditorGUI.BeginProperty(columnRect4, (GUIContent) null, this.m_SizeDelta.FindPropertyRelative("x"));
        this.FloatFieldLabelAbove(columnRect4, (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.sizeDelta.x), (RectTransformEditor.FloatSetter) ((rectTransform, val) => rectTransform.sizeDelta = new Vector2(val, rectTransform.sizeDelta.y)), DrivenTransformProperties.SizeDeltaX, !flag1 ? new GUIContent("Width") : new GUIContent("W Delta"));
        this.SetFadingBasedOnControlID(ref this.m_ChangingWidth, EditorGUIUtility.s_LastControlID);
        EditorGUI.EndProperty();
      }
      else
      {
        EditorGUI.BeginProperty(columnRect4, (GUIContent) null, this.m_AnchoredPosition.FindPropertyRelative("x"));
        EditorGUI.BeginProperty(columnRect4, (GUIContent) null, this.m_SizeDelta.FindPropertyRelative("x"));
        this.FloatFieldLabelAbove(columnRect4, (RectTransformEditor.FloatGetter) (rectTransform => -rectTransform.offsetMax.x), (RectTransformEditor.FloatSetter) ((rectTransform, val) => rectTransform.offsetMax = new Vector2(-val, rectTransform.offsetMax.y)), DrivenTransformProperties.None, new GUIContent("Right"));
        this.SetFadingBasedOnControlID(ref this.m_ChangingRight, EditorGUIUtility.s_LastControlID);
        EditorGUI.EndProperty();
        EditorGUI.EndProperty();
      }
      Rect columnRect5 = this.GetColumnRect(controlRect, 1);
      if (flag4 || anyWithoutParent || anyDrivenY)
      {
        EditorGUI.BeginProperty(columnRect5, (GUIContent) null, this.m_SizeDelta.FindPropertyRelative("y"));
        this.FloatFieldLabelAbove(columnRect5, (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.sizeDelta.y), (RectTransformEditor.FloatSetter) ((rectTransform, val) => rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, val)), DrivenTransformProperties.SizeDeltaY, !flag2 ? new GUIContent("Height") : new GUIContent("H Delta"));
        this.SetFadingBasedOnControlID(ref this.m_ChangingHeight, EditorGUIUtility.s_LastControlID);
        EditorGUI.EndProperty();
      }
      else
      {
        EditorGUI.BeginProperty(columnRect5, (GUIContent) null, this.m_AnchoredPosition.FindPropertyRelative("y"));
        EditorGUI.BeginProperty(columnRect5, (GUIContent) null, this.m_SizeDelta.FindPropertyRelative("y"));
        this.FloatFieldLabelAbove(columnRect5, (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.offsetMin.y), (RectTransformEditor.FloatSetter) ((rectTransform, val) => rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, val)), DrivenTransformProperties.None, new GUIContent("Bottom"));
        this.SetFadingBasedOnControlID(ref this.m_ChangingBottom, EditorGUIUtility.s_LastControlID);
        EditorGUI.EndProperty();
        EditorGUI.EndProperty();
      }
      Rect position = controlRect;
      position.height = EditorGUIUtility.singleLineHeight;
      position.y += EditorGUIUtility.singleLineHeight;
      position.yMin -= 2f;
      position.xMin = position.xMax - 26f;
      position.x -= position.width;
      this.BlueprintButton(position);
      position.x += position.width;
      this.RawButton(position);
    }

    private void SmartAnchorFields()
    {
      Rect controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight * (!this.m_ShowLayoutOptions ? 1f : 3f), new GUILayoutOption[0]);
      controlRect.height = EditorGUIUtility.singleLineHeight;
      EditorGUI.BeginChangeCheck();
      this.m_ShowLayoutOptions = EditorGUI.Foldout(controlRect, this.m_ShowLayoutOptions, RectTransformEditor.styles.anchorsContent);
      if (EditorGUI.EndChangeCheck())
        EditorPrefs.SetBool("RectTransformEditor.showAnchorProperties", this.m_ShowLayoutOptions);
      if (!this.m_ShowLayoutOptions)
        return;
      ++EditorGUI.indentLevel;
      controlRect.y += EditorGUIUtility.singleLineHeight;
      this.Vector2Field(controlRect, (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.anchorMin.x), (RectTransformEditor.FloatSetter) ((rectTransform, val) => RectTransformEditor.SetAnchorSmart(rectTransform, val, 0, false, !this.m_RawEditMode, true)), (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.anchorMin.y), (RectTransformEditor.FloatSetter) ((rectTransform, val) => RectTransformEditor.SetAnchorSmart(rectTransform, val, 1, false, !this.m_RawEditMode, true)), DrivenTransformProperties.AnchorMinX, DrivenTransformProperties.AnchorMinY, this.m_AnchorMin.FindPropertyRelative("x"), this.m_AnchorMin.FindPropertyRelative("y"), RectTransformEditor.styles.anchorMinContent);
      controlRect.y += EditorGUIUtility.singleLineHeight;
      this.Vector2Field(controlRect, (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.anchorMax.x), (RectTransformEditor.FloatSetter) ((rectTransform, val) => RectTransformEditor.SetAnchorSmart(rectTransform, val, 0, true, !this.m_RawEditMode, true)), (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.anchorMax.y), (RectTransformEditor.FloatSetter) ((rectTransform, val) => RectTransformEditor.SetAnchorSmart(rectTransform, val, 1, true, !this.m_RawEditMode, true)), DrivenTransformProperties.AnchorMaxX, DrivenTransformProperties.AnchorMaxY, this.m_AnchorMax.FindPropertyRelative("x"), this.m_AnchorMax.FindPropertyRelative("y"), RectTransformEditor.styles.anchorMaxContent);
      --EditorGUI.indentLevel;
    }

    private void SmartPivotField()
    {
      this.Vector2Field(EditorGUILayout.GetControlRect(), (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.pivot.x), (RectTransformEditor.FloatSetter) ((rectTransform, val) => RectTransformEditor.SetPivotSmart(rectTransform, val, 0, !this.m_RawEditMode, false)), (RectTransformEditor.FloatGetter) (rectTransform => rectTransform.pivot.y), (RectTransformEditor.FloatSetter) ((rectTransform, val) => RectTransformEditor.SetPivotSmart(rectTransform, val, 1, !this.m_RawEditMode, false)), DrivenTransformProperties.PivotX, DrivenTransformProperties.PivotY, this.m_Pivot.FindPropertyRelative("x"), this.m_Pivot.FindPropertyRelative("y"), RectTransformEditor.styles.pivotContent);
    }

    private void RawButton(Rect position)
    {
      EditorGUI.BeginChangeCheck();
      this.m_RawEditMode = GUI.Toggle(position, this.m_RawEditMode, RectTransformEditor.styles.rawEditContent, (GUIStyle) "ButtonRight");
      if (!EditorGUI.EndChangeCheck())
        return;
      EditorPrefs.SetBool("RectTransformEditor.lockRect", this.m_RawEditMode);
    }

    private void BlueprintButton(Rect position)
    {
      EditorGUI.BeginChangeCheck();
      bool flag = GUI.Toggle(position, Tools.rectBlueprintMode, RectTransformEditor.styles.blueprintContent, (GUIStyle) "ButtonLeft");
      if (!EditorGUI.EndChangeCheck())
        return;
      Tools.rectBlueprintMode = flag;
      Tools.RepaintAllToolViews();
    }

    private void FloatFieldLabelAbove(Rect position, RectTransformEditor.FloatGetter getter, RectTransformEditor.FloatSetter setter, DrivenTransformProperties driven, GUIContent label)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RectTransformEditor.\u003CFloatFieldLabelAbove\u003Ec__AnonStorey98 aboveCAnonStorey98 = new RectTransformEditor.\u003CFloatFieldLabelAbove\u003Ec__AnonStorey98();
      // ISSUE: reference to a compiler-generated field
      aboveCAnonStorey98.driven = driven;
      // ISSUE: reference to a compiler-generated field
      aboveCAnonStorey98.getter = getter;
      // ISSUE: reference to a compiler-generated method
      EditorGUI.BeginDisabledGroup(((IEnumerable<UnityEngine.Object>) this.targets).Any<UnityEngine.Object>(new Func<UnityEngine.Object, bool>(aboveCAnonStorey98.\u003C\u003Em__1AD)));
      // ISSUE: reference to a compiler-generated field
      float num = aboveCAnonStorey98.getter(this.target as RectTransform);
      // ISSUE: reference to a compiler-generated method
      EditorGUI.showMixedValue = ((IEnumerable<UnityEngine.Object>) this.targets).Select<UnityEngine.Object, float>(new Func<UnityEngine.Object, float>(aboveCAnonStorey98.\u003C\u003Em__1AE)).Distinct<float>().Count<float>() >= 2;
      EditorGUI.BeginChangeCheck();
      int controlId = GUIUtility.GetControlID(RectTransformEditor.s_FloatFieldHash, FocusType.Keyboard, position);
      Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
      Rect position1 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
      EditorGUI.HandlePrefixLabel(position, rect, label, controlId);
      float f = EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, position1, rect, controlId, num, EditorGUI.kFloatFieldFormatString, EditorStyles.textField, true);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObjects(this.targets, "Inspector");
        foreach (RectTransform target in this.targets)
          setter(target, f);
      }
      EditorGUI.EndDisabledGroup();
    }

    private void Vector2Field(Rect position, RectTransformEditor.FloatGetter xGetter, RectTransformEditor.FloatSetter xSetter, RectTransformEditor.FloatGetter yGetter, RectTransformEditor.FloatSetter ySetter, DrivenTransformProperties xDriven, DrivenTransformProperties yDriven, SerializedProperty xProperty, SerializedProperty yProperty, GUIContent label)
    {
      EditorGUI.PrefixLabel(position, -1, label);
      float labelWidth = EditorGUIUtility.labelWidth;
      int indentLevel = EditorGUI.indentLevel;
      Rect columnRect1 = this.GetColumnRect(position, 0);
      Rect columnRect2 = this.GetColumnRect(position, 1);
      EditorGUIUtility.labelWidth = 13f;
      EditorGUI.indentLevel = 0;
      EditorGUI.BeginProperty(columnRect1, RectTransformEditor.s_XYLabels[0], xProperty);
      this.FloatField(columnRect1, xGetter, xSetter, xDriven, RectTransformEditor.s_XYLabels[0]);
      EditorGUI.EndProperty();
      EditorGUI.BeginProperty(columnRect1, RectTransformEditor.s_XYLabels[1], yProperty);
      this.FloatField(columnRect2, yGetter, ySetter, yDriven, RectTransformEditor.s_XYLabels[1]);
      EditorGUI.EndProperty();
      EditorGUIUtility.labelWidth = labelWidth;
      EditorGUI.indentLevel = indentLevel;
    }

    private void FloatField(Rect position, RectTransformEditor.FloatGetter getter, RectTransformEditor.FloatSetter setter, DrivenTransformProperties driven, GUIContent label)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RectTransformEditor.\u003CFloatField\u003Ec__AnonStorey99 fieldCAnonStorey99 = new RectTransformEditor.\u003CFloatField\u003Ec__AnonStorey99();
      // ISSUE: reference to a compiler-generated field
      fieldCAnonStorey99.driven = driven;
      // ISSUE: reference to a compiler-generated field
      fieldCAnonStorey99.getter = getter;
      // ISSUE: reference to a compiler-generated method
      EditorGUI.BeginDisabledGroup(((IEnumerable<UnityEngine.Object>) this.targets).Any<UnityEngine.Object>(new Func<UnityEngine.Object, bool>(fieldCAnonStorey99.\u003C\u003Em__1AF)));
      // ISSUE: reference to a compiler-generated field
      float num = fieldCAnonStorey99.getter(this.target as RectTransform);
      // ISSUE: reference to a compiler-generated method
      EditorGUI.showMixedValue = ((IEnumerable<UnityEngine.Object>) this.targets).Select<UnityEngine.Object, float>(new Func<UnityEngine.Object, float>(fieldCAnonStorey99.\u003C\u003Em__1B0)).Distinct<float>().Count<float>() >= 2;
      EditorGUI.BeginChangeCheck();
      float f = EditorGUI.FloatField(position, label, num);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObjects(this.targets, "Inspector");
        foreach (RectTransform target in this.targets)
          setter(target, f);
      }
      EditorGUI.EndDisabledGroup();
    }

    private Rect GetColumnRect(Rect totalRect, int column)
    {
      totalRect.xMin += EditorGUIUtility.labelWidth - 1f;
      Rect rect = totalRect;
      rect.xMin += (float) (((double) totalRect.width - 4.0) * ((double) column / 3.0)) + (float) (column * 2);
      rect.width = (float) (((double) totalRect.width - 4.0) / 3.0);
      return rect;
    }

    private int AnchorPopup(Rect position, string label, int selected, string[] displayedOptions)
    {
      EditorGUIUtility.labelWidth = 12f;
      int num = EditorGUI.Popup(position, label, selected, displayedOptions);
      EditorGUIUtility.labelWidth = 0.0f;
      return num;
    }

    private void DrawRect(Rect rect, Transform space, bool dotted)
    {
      Vector3 vector3_1 = space.TransformPoint((Vector3) new Vector2(rect.x, rect.y));
      Vector3 vector3_2 = space.TransformPoint((Vector3) new Vector2(rect.x, rect.yMax));
      Vector3 vector3_3 = space.TransformPoint((Vector3) new Vector2(rect.xMax, rect.yMax));
      Vector3 vector3_4 = space.TransformPoint((Vector3) new Vector2(rect.xMax, rect.y));
      if (!dotted)
      {
        Handles.DrawLine(vector3_1, vector3_2);
        Handles.DrawLine(vector3_2, vector3_3);
        Handles.DrawLine(vector3_3, vector3_4);
        Handles.DrawLine(vector3_4, vector3_1);
      }
      else
      {
        RectHandles.DrawDottedLineWithShadow(RectTransformEditor.kShadowColor, RectTransformEditor.kShadowOffset, vector3_1, vector3_2, 5f);
        RectHandles.DrawDottedLineWithShadow(RectTransformEditor.kShadowColor, RectTransformEditor.kShadowOffset, vector3_2, vector3_3, 5f);
        RectHandles.DrawDottedLineWithShadow(RectTransformEditor.kShadowColor, RectTransformEditor.kShadowOffset, vector3_3, vector3_4, 5f);
        RectHandles.DrawDottedLineWithShadow(RectTransformEditor.kShadowColor, RectTransformEditor.kShadowOffset, vector3_4, vector3_1, 5f);
      }
    }

    private void OnSceneGUI()
    {
      RectTransform target = this.target as RectTransform;
      Rect rect1 = target.rect;
      Rect rectInUserSpace = rect1;
      Rect rect2 = rect1;
      Transform transform1 = target.transform;
      Transform userSpace = transform1;
      Transform transform2 = transform1;
      RectTransform rectTransform = (RectTransform) null;
      if ((UnityEngine.Object) transform1.parent != (UnityEngine.Object) null)
      {
        transform2 = transform1.parent;
        rect2.x += transform1.localPosition.x;
        rect2.y += transform1.localPosition.y;
        rectTransform = transform2.GetComponent<RectTransform>();
      }
      if (Tools.rectBlueprintMode)
      {
        userSpace = transform2;
        rectInUserSpace = rect2;
      }
      float num = Mathf.Max(this.m_ChangingAnchors.faded, this.m_ChangingPivot.faded);
      if (target.anchorMin != target.anchorMax)
        num = Mathf.Max(num, this.m_ChangingPosX.faded, this.m_ChangingPosY.faded, this.m_ChangingLeft.faded, this.m_ChangingRight.faded, this.m_ChangingTop.faded, this.m_ChangingBottom.faded);
      Color parentSpaceColor = RectTransformEditor.kRectInParentSpaceColor;
      parentSpaceColor.a *= num;
      Handles.color = parentSpaceColor;
      this.DrawRect(rect2, transform2, true);
      if (this.m_TargetCount != 1)
        return;
      RectTransformSnapping.OnGUI();
      if ((UnityEngine.Object) rectTransform != (UnityEngine.Object) null)
        this.AllAnchorsSceneGUI(target, rectTransform, transform2, transform1);
      this.DrawSizes(rectInUserSpace, userSpace, rect2, transform2, target, rectTransform);
      RectTransformSnapping.DrawGuides();
      if (Tools.current != Tool.Rect)
        return;
      this.ParentRectPreviewDragHandles(rectTransform, transform2);
    }

    private void ParentRectPreviewDragHandles(RectTransform gui, Transform space)
    {
      if ((UnityEngine.Object) gui == (UnityEngine.Object) null)
        return;
      float size = 0.05f * HandleUtility.GetHandleSize(space.position);
      Rect rect1 = gui.rect;
      for (int index1 = 0; index1 <= 2; ++index1)
      {
        for (int index2 = 0; index2 <= 2; ++index2)
        {
          if (index1 == 1 != (index2 == 1))
          {
            Vector3 zero = (Vector3) Vector2.zero;
            for (int index3 = 0; index3 < 2; ++index3)
              zero[index3] = Mathf.Lerp(rect1.min[index3], rect1.max[index3], (float) ((index3 != 0 ? (double) index2 : (double) index1) * 0.5));
            Vector3 position1 = space.TransformPoint(zero);
            int controlId = GUIUtility.GetControlID(RectTransformEditor.s_ParentRectPreviewHandlesHash, FocusType.Native);
            Vector3 sideVector = index1 != 1 ? space.up * rect1.height : space.right * rect1.width;
            Vector3 direction = index1 != 1 ? space.right : space.up;
            EditorGUI.BeginChangeCheck();
            Vector3 position2 = RectHandles.SideSlider(controlId, position1, sideVector, direction, size, (Handles.DrawCapFunction) null, 0.0f, -3f);
            if (EditorGUI.EndChangeCheck())
            {
              Vector2 vector2_1 = (Vector2) space.InverseTransformPoint(position1);
              Vector2 vector2_2 = (Vector2) space.InverseTransformPoint(position2);
              Rect rect2 = rect1;
              Vector2 vector2_3 = vector2_2 - vector2_1;
              if (index1 == 0)
                rect2.min = new Vector2(rect2.min.x + vector2_3.x, rect2.min.y);
              if (index1 == 2)
                rect2.max = new Vector2(rect2.max.x + vector2_3.x, rect2.max.y);
              if (index2 == 0)
                rect2.min = new Vector2(rect2.min.x, rect2.min.y + vector2_3.y);
              if (index2 == 2)
                rect2.max = new Vector2(rect2.max.x, rect2.max.y + vector2_3.y);
              this.SetTemporaryRect(gui, rect2, controlId);
            }
            if (GUIUtility.hotControl == controlId)
            {
              Handles.BeginGUI();
              EditorGUI.DropShadowLabel(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 60f, 16f), "Preview");
              Handles.EndGUI();
            }
          }
        }
      }
    }

    private void SetTemporaryRect(RectTransform gui, Rect rect, int id)
    {
      if ((UnityEngine.Object) RectTransformEditor.s_ParentDragRectTransform == (UnityEngine.Object) null)
      {
        RectTransformEditor.s_ParentDragRectTransform = gui;
        RectTransformEditor.s_ParentDragOrigRect = gui.rect;
        RectTransformEditor.s_ParentDragId = (float) id;
      }
      else if ((UnityEngine.Object) RectTransformEditor.s_ParentDragRectTransform != (UnityEngine.Object) gui)
        return;
      RectTransformEditor.s_ParentDragPreviewRect = rect;
      RectTransformEditor.s_ParentDragTime = Time.realtimeSinceStartup;
      InternalEditorUtility.SetRectTransformTemporaryRect(gui, rect);
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.UpdateTemporaryRect);
      EditorApplication.update += new EditorApplication.CallbackFunction(this.UpdateTemporaryRect);
    }

    private void UpdateTemporaryRect()
    {
      if ((UnityEngine.Object) RectTransformEditor.s_ParentDragRectTransform == (UnityEngine.Object) null)
        return;
      if ((double) GUIUtility.hotControl == (double) RectTransformEditor.s_ParentDragId)
      {
        RectTransformEditor.s_ParentDragTime = Time.realtimeSinceStartup;
        Canvas.ForceUpdateCanvases();
        GameView.RepaintAll();
      }
      else
      {
        float t = Mathf.Clamp01((float) (1.0 - (double) (Time.realtimeSinceStartup - RectTransformEditor.s_ParentDragTime) * 8.0));
        if ((double) t > 0.0)
        {
          InternalEditorUtility.SetRectTransformTemporaryRect(RectTransformEditor.s_ParentDragRectTransform, new Rect()
          {
            position = Vector2.Lerp(RectTransformEditor.s_ParentDragOrigRect.position, RectTransformEditor.s_ParentDragPreviewRect.position, t),
            size = Vector2.Lerp(RectTransformEditor.s_ParentDragOrigRect.size, RectTransformEditor.s_ParentDragPreviewRect.size, t)
          });
        }
        else
        {
          InternalEditorUtility.SetRectTransformTemporaryRect(RectTransformEditor.s_ParentDragRectTransform, new Rect());
          EditorApplication.update -= new EditorApplication.CallbackFunction(this.UpdateTemporaryRect);
          RectTransformEditor.s_ParentDragRectTransform = (RectTransform) null;
        }
        Canvas.ForceUpdateCanvases();
        SceneView.RepaintAll();
        GameView.RepaintAll();
      }
    }

    private void AllAnchorsSceneGUI(RectTransform gui, RectTransform guiParent, Transform parentSpace, Transform transform)
    {
      Handles.color = RectTransformEditor.kParentColor;
      this.DrawRect(guiParent.rect, parentSpace, false);
      Handles.color = RectTransformEditor.kSiblingColor;
      foreach (Transform transform1 in parentSpace)
      {
        if (transform1.gameObject.activeInHierarchy)
        {
          RectTransform component = transform1.GetComponent<RectTransform>();
          if ((bool) ((UnityEngine.Object) component))
          {
            Rect rect = component.rect;
            rect.x += component.transform.localPosition.x;
            rect.y += component.transform.localPosition.y;
            this.DrawRect(component.rect, (Transform) component, false);
            if ((UnityEngine.Object) component != (UnityEngine.Object) transform)
              this.AnchorsSceneGUI(component, guiParent, parentSpace, false);
          }
        }
      }
      Handles.color = RectTransformEditor.kAnchorColor;
      this.AnchorsSceneGUI(gui, guiParent, parentSpace, true);
    }

    private Vector3 GetAnchorLocal(RectTransform guiParent, Vector2 anchor)
    {
      return (Vector3) RectTransformEditor.NormalizedToPointUnclamped(guiParent.rect, anchor);
    }

    private static Vector2 NormalizedToPointUnclamped(Rect rectangle, Vector2 normalizedRectCoordinates)
    {
      return new Vector2(Mathf.LerpUnclamped(rectangle.x, rectangle.xMax, normalizedRectCoordinates.x), Mathf.LerpUnclamped(rectangle.y, rectangle.yMax, normalizedRectCoordinates.y));
    }

    private static bool AnchorAllowedOutsideParent(int axis, int minmax)
    {
      if (EditorGUI.actionKey || GUIUtility.hotControl == 0)
        return true;
      float num = minmax != 0 ? RectTransformEditor.s_StartDragAnchorMax[axis] : RectTransformEditor.s_StartDragAnchorMin[axis];
      if ((double) num >= -1.0 / 1000.0)
        return (double) num > 1.00100004673004;
      return true;
    }

    private void AnchorsSceneGUI(RectTransform gui, RectTransform guiParent, Transform parentSpace, bool interactive)
    {
      if (Event.current.type == EventType.MouseDown)
      {
        RectTransformEditor.s_AnchorFusedState = RectTransformEditor.AnchorFusedState.None;
        if (gui.anchorMin == gui.anchorMax)
          RectTransformEditor.s_AnchorFusedState = RectTransformEditor.AnchorFusedState.All;
        else if ((double) gui.anchorMin.x == (double) gui.anchorMax.x)
          RectTransformEditor.s_AnchorFusedState = RectTransformEditor.AnchorFusedState.Horizontal;
        else if ((double) gui.anchorMin.y == (double) gui.anchorMax.y)
          RectTransformEditor.s_AnchorFusedState = RectTransformEditor.AnchorFusedState.Vertical;
      }
      this.AnchorSceneGUI(gui, guiParent, parentSpace, interactive, 0, 0, GUIUtility.GetControlID(FocusType.Passive));
      this.AnchorSceneGUI(gui, guiParent, parentSpace, interactive, 0, 1, GUIUtility.GetControlID(FocusType.Passive));
      this.AnchorSceneGUI(gui, guiParent, parentSpace, interactive, 1, 0, GUIUtility.GetControlID(FocusType.Passive));
      this.AnchorSceneGUI(gui, guiParent, parentSpace, interactive, 1, 1, GUIUtility.GetControlID(FocusType.Passive));
      if (!interactive)
        return;
      int controlId1 = GUIUtility.GetControlID(FocusType.Passive);
      int controlId2 = GUIUtility.GetControlID(FocusType.Passive);
      int controlId3 = GUIUtility.GetControlID(FocusType.Passive);
      int controlId4 = GUIUtility.GetControlID(FocusType.Passive);
      int controlId5 = GUIUtility.GetControlID(FocusType.Passive);
      if (RectTransformEditor.s_AnchorFusedState == RectTransformEditor.AnchorFusedState.All)
        this.AnchorSceneGUI(gui, guiParent, parentSpace, interactive, 2, 2, controlId1);
      if (RectTransformEditor.s_AnchorFusedState == RectTransformEditor.AnchorFusedState.Horizontal)
      {
        this.AnchorSceneGUI(gui, guiParent, parentSpace, interactive, 2, 0, controlId2);
        this.AnchorSceneGUI(gui, guiParent, parentSpace, interactive, 2, 1, controlId3);
      }
      if (RectTransformEditor.s_AnchorFusedState != RectTransformEditor.AnchorFusedState.Vertical)
        return;
      this.AnchorSceneGUI(gui, guiParent, parentSpace, interactive, 0, 2, controlId4);
      this.AnchorSceneGUI(gui, guiParent, parentSpace, interactive, 1, 2, controlId5);
    }

    private void AnchorSceneGUI(RectTransform gui, RectTransform guiParent, Transform parentSpace, bool interactive, int minmaxX, int minmaxY, int id)
    {
      Vector3 vector3 = (Vector3) new Vector2();
      vector3.x = minmaxX != 0 ? gui.anchorMax.x : gui.anchorMin.x;
      vector3.y = minmaxY != 0 ? gui.anchorMax.y : gui.anchorMin.y;
      vector3 = this.GetAnchorLocal(guiParent, (Vector2) vector3);
      vector3 = parentSpace.TransformPoint(vector3);
      float handleSize = 0.05f * HandleUtility.GetHandleSize(vector3);
      if (minmaxX < 2)
        vector3 += parentSpace.right * handleSize * (float) (minmaxX * 2 - 1);
      if (minmaxY < 2)
        vector3 += parentSpace.up * handleSize * (float) (minmaxY * 2 - 1);
      if (minmaxX < 2 && minmaxY < 2)
        this.DrawAnchor(vector3, parentSpace.right * handleSize * 2f * (float) (minmaxX * 2 - 1), parentSpace.up * handleSize * 2f * (float) (minmaxY * 2 - 1));
      if (!interactive)
        return;
      Event eventBefore = new Event(Event.current);
      EditorGUI.BeginChangeCheck();
      Vector3 position = Handles.Slider2D(id, vector3, parentSpace.forward, parentSpace.right, parentSpace.up, handleSize, (Handles.DrawCapFunction) null, Vector2.zero);
      if (eventBefore.type == EventType.MouseDown && GUIUtility.hotControl == id)
      {
        RectTransformEditor.s_DragAnchorsTogether = EditorGUI.actionKey;
        RectTransformEditor.s_StartDragAnchorMin = gui.anchorMin;
        RectTransformEditor.s_StartDragAnchorMax = gui.anchorMax;
        RectTransformSnapping.CalculateAnchorSnapValues(parentSpace, gui.transform, gui, minmaxX, minmaxY);
      }
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject((UnityEngine.Object) gui, "Move Rectangle Anchors");
        Vector2 vector2 = (Vector2) parentSpace.InverseTransformVector(position - vector3);
        for (int axis = 0; axis <= 1; ++axis)
        {
          // ISSUE: variable of a reference type
          Vector2& local;
          int index;
          // ISSUE: explicit reference operation
          // ISSUE: explicit reference operation
          double num1 = (double) (^(local = @vector2))[index = axis] / (double) guiParent.rect.size[axis];
          // ISSUE: explicit reference operation
          (^local)[index] = (float) num1;
          int minmax = axis != 0 ? minmaxY : minmaxX;
          bool isMax = minmax == 1;
          float num2 = (!isMax ? gui.anchorMin[axis] : gui.anchorMax[axis]) + vector2[axis];
          float a = num2;
          if (!RectTransformEditor.AnchorAllowedOutsideParent(axis, minmax))
            a = Mathf.Clamp01(a);
          if (minmax == 0)
            a = Mathf.Min(a, gui.anchorMax[axis]);
          if (minmax == 1)
            a = Mathf.Max(a, gui.anchorMin[axis]);
          float snapDistance = HandleUtility.GetHandleSize(position) * 0.05f / guiParent.rect.size[axis] * parentSpace.InverseTransformVector(axis != 0 ? Vector3.up : Vector3.right)[axis];
          float guides = RectTransformSnapping.SnapToGuides(a, snapDistance, axis);
          bool enforceExactValue = (double) guides != (double) num2;
          float num3 = guides;
          if (minmax == 2)
          {
            RectTransformEditor.SetAnchorSmart(gui, num3, axis, false, !eventBefore.shift, enforceExactValue, false, RectTransformEditor.s_DragAnchorsTogether);
            RectTransformEditor.SetAnchorSmart(gui, num3, axis, true, !eventBefore.shift, enforceExactValue, false, RectTransformEditor.s_DragAnchorsTogether);
          }
          else
            RectTransformEditor.SetAnchorSmart(gui, num3, axis, isMax, !eventBefore.shift, enforceExactValue, true, RectTransformEditor.s_DragAnchorsTogether);
          EditorUtility.SetDirty((UnityEngine.Object) gui);
          if (gui.drivenByObject != (UnityEngine.Object) null)
            RectTransform.SendReapplyDrivenProperties(gui);
        }
      }
      this.SetFadingBasedOnMouseDownUp(ref this.m_ChangingAnchors, eventBefore);
    }

    private static float Round(float value)
    {
      return Mathf.Floor(0.5f + value);
    }

    private static int RoundToInt(float value)
    {
      return Mathf.FloorToInt(0.5f + value);
    }

    private void DrawSizes(Rect rectInUserSpace, Transform userSpace, Rect rectInParentSpace, Transform parentSpace, RectTransform gui, RectTransform guiParent)
    {
      float size = 0.05f * HandleUtility.GetHandleSize(parentSpace.position);
      bool flag1 = (double) gui.anchorMin.x != (double) gui.anchorMax.x;
      bool flag2 = (double) gui.anchorMin.y != (double) gui.anchorMax.y;
      float alpha1 = Mathf.Max(this.m_ChangingPosX.faded, this.m_ChangingLeft.faded, this.m_ChangingRight.faded, this.m_ChangingAnchors.faded);
      this.DrawAnchorRect(parentSpace, gui, guiParent, 0, alpha1);
      float alpha2 = Mathf.Max(this.m_ChangingPosY.faded, this.m_ChangingTop.faded, this.m_ChangingBottom.faded, this.m_ChangingAnchors.faded);
      this.DrawAnchorRect(parentSpace, gui, guiParent, 1, alpha2);
      this.DrawAnchorDistances(parentSpace, gui, guiParent, size, this.m_ChangingAnchors.faded);
      if (flag1)
      {
        this.DrawPositionDistances(userSpace, rectInParentSpace, parentSpace, gui, guiParent, size, 0, 1, this.m_ChangingLeft.faded);
        this.DrawPositionDistances(userSpace, rectInParentSpace, parentSpace, gui, guiParent, size, 0, 2, this.m_ChangingRight.faded);
      }
      else
      {
        this.DrawPositionDistances(userSpace, rectInParentSpace, parentSpace, gui, guiParent, size, 0, 0, this.m_ChangingPosX.faded);
        this.DrawSizeDistances(userSpace, rectInParentSpace, parentSpace, gui, guiParent, size, 0, this.m_ChangingWidth.faded);
      }
      if (flag2)
      {
        this.DrawPositionDistances(userSpace, rectInParentSpace, parentSpace, gui, guiParent, size, 1, 1, this.m_ChangingBottom.faded);
        this.DrawPositionDistances(userSpace, rectInParentSpace, parentSpace, gui, guiParent, size, 1, 2, this.m_ChangingTop.faded);
      }
      else
      {
        this.DrawPositionDistances(userSpace, rectInParentSpace, parentSpace, gui, guiParent, size, 1, 0, this.m_ChangingPosY.faded);
        this.DrawSizeDistances(userSpace, rectInParentSpace, parentSpace, gui, guiParent, size, 1, this.m_ChangingHeight.faded);
      }
    }

    private void DrawSizeDistances(Transform userSpace, Rect rectInParentSpace, Transform parentSpace, RectTransform gui, RectTransform guiParent, float size, int axis, float alpha)
    {
      if ((double) alpha <= 0.0)
        return;
      Color kAnchorColor = RectTransformEditor.kAnchorColor;
      kAnchorColor.a *= alpha;
      GUI.color = kAnchorColor;
      if ((UnityEngine.Object) userSpace == (UnityEngine.Object) gui.transform)
      {
        gui.GetWorldCorners(RectTransformEditor.s_Corners);
      }
      else
      {
        gui.GetLocalCorners(RectTransformEditor.s_Corners);
        for (int index = 0; index < 4; ++index)
        {
          RectTransformEditor.s_Corners[index] += gui.transform.localPosition;
          RectTransformEditor.s_Corners[index] = userSpace.TransformPoint(RectTransformEditor.s_Corners[index]);
        }
      }
      GUIContent label = new GUIContent(gui.sizeDelta[axis].ToString());
      Vector3 vector3 = (axis != 0 ? userSpace.right : userSpace.up) * size * 2f;
      this.DrawLabelBetweenPoints(RectTransformEditor.s_Corners[0] + vector3, RectTransformEditor.s_Corners[axis != 0 ? 1 : 3] + vector3, label);
    }

    private void DrawPositionDistances(Transform userSpace, Rect rectInParentSpace, Transform parentSpace, RectTransform gui, RectTransform guiParent, float size, int axis, int side, float alpha)
    {
      if ((UnityEngine.Object) guiParent == (UnityEngine.Object) null || (double) alpha <= 0.0)
        return;
      Color kAnchorLineColor = RectTransformEditor.kAnchorLineColor;
      kAnchorLineColor.a *= alpha;
      Handles.color = kAnchorLineColor;
      Color kAnchorColor = RectTransformEditor.kAnchorColor;
      kAnchorColor.a *= alpha;
      GUI.color = kAnchorColor;
      Vector3 position1;
      Vector3 position2;
      float num;
      if (side == 0)
      {
        Vector2 point = Rect.NormalizedToPoint(rectInParentSpace, gui.pivot);
        position1 = (Vector3) point;
        position2 = (Vector3) point;
        position1[axis] = Mathf.LerpUnclamped(guiParent.rect.min[axis], guiParent.rect.max[axis], gui.anchorMin[axis]);
        num = gui.anchoredPosition[axis];
      }
      else
      {
        Vector2 center = rectInParentSpace.center;
        position1 = (Vector3) center;
        position2 = (Vector3) center;
        if (side == 1)
        {
          position1[axis] = Mathf.LerpUnclamped(guiParent.rect.min[axis], guiParent.rect.max[axis], gui.anchorMin[axis]);
          position2[axis] = rectInParentSpace.min[axis];
          num = gui.offsetMin[axis];
        }
        else
        {
          position1[axis] = Mathf.LerpUnclamped(guiParent.rect.min[axis], guiParent.rect.max[axis], gui.anchorMax[axis]);
          position2[axis] = rectInParentSpace.max[axis];
          num = -gui.offsetMax[axis];
        }
      }
      Vector3 vector3_1 = parentSpace.TransformPoint(position1);
      Vector3 vector3_2 = parentSpace.TransformPoint(position2);
      RectHandles.DrawDottedLineWithShadow(RectTransformEditor.kShadowColor, RectTransformEditor.kShadowOffset, vector3_1, vector3_2, 5f);
      GUIContent label = new GUIContent(num.ToString());
      this.DrawLabelBetweenPoints(vector3_1, vector3_2, label);
    }

    private float LerpUnclamped(float a, float b, float t)
    {
      return (float) ((double) a * (1.0 - (double) t) + (double) b * (double) t);
    }

    private void DrawAnchorDistances(Transform parentSpace, RectTransform gui, RectTransform guiParent, float size, float alpha)
    {
      if ((UnityEngine.Object) guiParent == (UnityEngine.Object) null || (double) alpha <= 0.0)
        return;
      Color kAnchorColor = RectTransformEditor.kAnchorColor;
      kAnchorColor.a *= alpha;
      GUI.color = kAnchorColor;
      Vector3[,] vector3Array = new Vector3[2, 4];
      for (int index1 = 0; index1 < 2; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          Vector3 vector3_1 = Vector3.zero;
          switch (index2)
          {
            case 0:
              vector3_1 = Vector3.zero;
              break;
            case 1:
              vector3_1 = (Vector3) gui.anchorMin;
              break;
            case 2:
              vector3_1 = (Vector3) gui.anchorMax;
              break;
            case 3:
              vector3_1 = Vector3.one;
              break;
          }
          vector3_1[index1] = gui.anchorMin[index1];
          Vector3 vector3_2 = parentSpace.TransformPoint(this.GetAnchorLocal(guiParent, (Vector2) vector3_1));
          vector3Array[index1, index2] = vector3_2;
        }
      }
      for (int index = 0; index < 2; ++index)
      {
        Vector3 vector3 = (index != 0 ? parentSpace.up : parentSpace.right) * size * 2f;
        int num1 = RectTransformEditor.RoundToInt(gui.anchorMin[1 - index] * 100f);
        int num2 = RectTransformEditor.RoundToInt((float) (((double) gui.anchorMax[1 - index] - (double) gui.anchorMin[1 - index]) * 100.0));
        int num3 = RectTransformEditor.RoundToInt((float) ((1.0 - (double) gui.anchorMax[1 - index]) * 100.0));
        if (num1 > 0)
          this.DrawLabelBetweenPoints(vector3Array[index, 0] - vector3, vector3Array[index, 1] - vector3, GUIContent.Temp(num1.ToString() + "%"));
        if (num2 > 0)
          this.DrawLabelBetweenPoints(vector3Array[index, 1] - vector3, vector3Array[index, 2] - vector3, GUIContent.Temp(num2.ToString() + "%"));
        if (num3 > 0)
          this.DrawLabelBetweenPoints(vector3Array[index, 2] - vector3, vector3Array[index, 3] - vector3, GUIContent.Temp(num3.ToString() + "%"));
      }
    }

    private void DrawAnchorRect(Transform parentSpace, RectTransform gui, RectTransform guiParent, int axis, float alpha)
    {
      if ((UnityEngine.Object) guiParent == (UnityEngine.Object) null || (double) alpha <= 0.0)
        return;
      Color kAnchorLineColor = RectTransformEditor.kAnchorLineColor;
      kAnchorLineColor.a *= alpha;
      Handles.color = kAnchorLineColor;
      Vector3[,] vector3Array = new Vector3[2, 2];
      for (int index1 = 0; index1 < 2; ++index1)
      {
        if (index1 != 1 || (double) gui.anchorMin[axis] != (double) gui.anchorMax[axis])
        {
          vector3Array[index1, 0][1 - axis] = Mathf.Min(0.0f, gui.anchorMin[1 - axis]);
          vector3Array[index1, 1][1 - axis] = Mathf.Max(1f, gui.anchorMax[1 - axis]);
          for (int index2 = 0; index2 < 2; ++index2)
          {
            vector3Array[index1, index2][axis] = index1 != 0 ? gui.anchorMax[axis] : gui.anchorMin[axis];
            vector3Array[index1, index2] = parentSpace.TransformPoint(this.GetAnchorLocal(guiParent, (Vector2) vector3Array[index1, index2]));
          }
          RectHandles.DrawDottedLineWithShadow(RectTransformEditor.kShadowColor, RectTransformEditor.kShadowOffset, vector3Array[index1, 0], vector3Array[index1, 1], 5f);
        }
      }
    }

    private void DrawLabelBetweenPoints(Vector3 pA, Vector3 pB, GUIContent label)
    {
      if (pA == pB)
        return;
      Vector2 guiPoint1 = HandleUtility.WorldToGUIPoint(pA);
      Vector2 guiPoint2 = HandleUtility.WorldToGUIPoint(pB);
      Vector2 pivotPoint = (guiPoint1 + guiPoint2) * 0.5f;
      pivotPoint.x = RectTransformEditor.Round(pivotPoint.x);
      pivotPoint.y = RectTransformEditor.Round(pivotPoint.y);
      float angle = Mathf.Repeat(Mathf.Atan2(guiPoint2.y - guiPoint1.y, guiPoint2.x - guiPoint1.x) * 57.29578f + 89f, 180f) - 89f;
      Handles.BeginGUI();
      Matrix4x4 matrix = GUI.matrix;
      GUIStyle measuringLabelStyle = RectTransformEditor.styles.measuringLabelStyle;
      measuringLabelStyle.alignment = TextAnchor.MiddleCenter;
      GUIUtility.RotateAroundPivot(angle, pivotPoint);
      EditorGUI.DropShadowLabel(new Rect(pivotPoint.x - 50f, pivotPoint.y - 9f, 100f, 16f), label, measuringLabelStyle);
      GUI.matrix = matrix;
      Handles.EndGUI();
    }

    private static Vector3 GetRectReferenceCorner(RectTransform gui, bool worldSpace)
    {
      if (!worldSpace)
        return (Vector3) gui.rect.min + gui.transform.localPosition;
      Transform transform = gui.transform;
      gui.GetWorldCorners(RectTransformEditor.s_Corners);
      if ((bool) ((UnityEngine.Object) transform.parent))
        return transform.parent.InverseTransformPoint(RectTransformEditor.s_Corners[0]);
      return RectTransformEditor.s_Corners[0];
    }

    private void DrawAnchor(Vector3 pos, Vector3 right, Vector3 up)
    {
      pos -= up * 0.5f;
      pos -= right * 0.5f;
      up *= 1.4f;
      right *= 1.4f;
      RectHandles.DrawPolyLineWithShadow(RectTransformEditor.kShadowColor, RectTransformEditor.kShadowOffset, pos, pos + up + right * 0.5f, pos + right + up * 0.5f, pos);
    }

    public static void SetPivotSmart(RectTransform rect, float value, int axis, bool smart, bool parentSpace)
    {
      Vector3 rectReferenceCorner = RectTransformEditor.GetRectReferenceCorner(rect, !parentSpace);
      Vector2 pivot = rect.pivot;
      pivot[axis] = value;
      rect.pivot = pivot;
      if (!smart)
        return;
      Vector3 vector3 = RectTransformEditor.GetRectReferenceCorner(rect, !parentSpace) - rectReferenceCorner;
      rect.anchoredPosition -= (Vector2) vector3;
      Vector3 position = rect.transform.position;
      position.z -= vector3.z;
      rect.transform.position = position;
    }

    public static void SetAnchorSmart(RectTransform rect, float value, int axis, bool isMax, bool smart)
    {
      RectTransformEditor.SetAnchorSmart(rect, value, axis, isMax, smart, false, false, false);
    }

    public static void SetAnchorSmart(RectTransform rect, float value, int axis, bool isMax, bool smart, bool enforceExactValue)
    {
      RectTransformEditor.SetAnchorSmart(rect, value, axis, isMax, smart, enforceExactValue, false, false);
    }

    public static void SetAnchorSmart(RectTransform rect, float value, int axis, bool isMax, bool smart, bool enforceExactValue, bool enforceMinNoLargerThanMax, bool moveTogether)
    {
      RectTransform rectTransform = (RectTransform) null;
      if ((UnityEngine.Object) rect.transform.parent == (UnityEngine.Object) null)
      {
        smart = false;
      }
      else
      {
        rectTransform = rect.transform.parent.GetComponent<RectTransform>();
        if ((UnityEngine.Object) rectTransform == (UnityEngine.Object) null)
          smart = false;
      }
      bool flag = !RectTransformEditor.AnchorAllowedOutsideParent(axis, !isMax ? 0 : 1);
      if (flag)
        value = Mathf.Clamp01(value);
      if (enforceMinNoLargerThanMax)
        value = !isMax ? Mathf.Min(value, rect.anchorMax[axis]) : Mathf.Max(value, rect.anchorMin[axis]);
      float num1 = 0.0f;
      float num2 = 0.0f;
      if (smart)
      {
        float num3 = !isMax ? rect.anchorMin[axis] : rect.anchorMax[axis];
        float f = (value - num3) * rectTransform.rect.size[axis];
        float num4 = 0.0f;
        if (RectTransformEditor.ShouldDoIntSnapping(rect))
          num4 = Mathf.Round(f) - f;
        num1 = f + num4;
        if (!enforceExactValue)
        {
          value += num4 / rectTransform.rect.size[axis];
          if ((double) Mathf.Abs(RectTransformEditor.Round(value * 1000f) - value * 1000f) < 0.100000001490116)
            value = RectTransformEditor.Round(value * 1000f) * (1f / 1000f);
          if (flag)
            value = Mathf.Clamp01(value);
          if (enforceMinNoLargerThanMax)
            value = !isMax ? Mathf.Min(value, rect.anchorMax[axis]) : Mathf.Max(value, rect.anchorMin[axis]);
        }
        num2 = !moveTogether ? (!isMax ? num1 * (1f - rect.pivot[axis]) : num1 * rect.pivot[axis]) : num1;
      }
      if (isMax)
      {
        Vector2 anchorMax = rect.anchorMax;
        anchorMax[axis] = value;
        rect.anchorMax = anchorMax;
        Vector2 anchorMin = rect.anchorMin;
        if (moveTogether)
          anchorMin[axis] = RectTransformEditor.s_StartDragAnchorMin[axis] + anchorMax[axis] - RectTransformEditor.s_StartDragAnchorMax[axis];
        rect.anchorMin = anchorMin;
      }
      else
      {
        Vector2 anchorMin = rect.anchorMin;
        anchorMin[axis] = value;
        rect.anchorMin = anchorMin;
        Vector2 anchorMax = rect.anchorMax;
        if (moveTogether)
          anchorMax[axis] = RectTransformEditor.s_StartDragAnchorMax[axis] + anchorMin[axis] - RectTransformEditor.s_StartDragAnchorMin[axis];
        rect.anchorMax = anchorMax;
      }
      if (!smart)
        return;
      Vector2 anchoredPosition = rect.anchoredPosition;
      anchoredPosition[axis] -= num2;
      rect.anchoredPosition = anchoredPosition;
      if (moveTogether)
        return;
      Vector2 sizeDelta = rect.sizeDelta;
      // ISSUE: variable of a reference type
      Vector2& local;
      int index;
      // ISSUE: explicit reference operation
      // ISSUE: explicit reference operation
      double num5 = (double) (^(local = @sizeDelta))[index = axis] + (double) num1 * (!isMax ? 1.0 : -1.0);
      // ISSUE: explicit reference operation
      (^local)[index] = (float) num5;
      rect.sizeDelta = sizeDelta;
    }

    private class Styles
    {
      public GUIStyle lockStyle = EditorStyles.miniButton;
      public GUIStyle measuringLabelStyle = new GUIStyle((GUIStyle) "PreOverlayLabel");
      public GUIContent anchorsContent = new GUIContent("Anchors");
      public GUIContent anchorMinContent = new GUIContent("Min", "The normalized position in the parent rectangle that the lower left corner is anchored to.");
      public GUIContent anchorMaxContent = new GUIContent("Max", "The normalized position in the parent rectangle that the upper right corner is anchored to.");
      public GUIContent positionContent = new GUIContent("Position", "The local position of the rectangle. The position specifies this rectangle's pivot relative to the anchor reference point.");
      public GUIContent sizeContent = new GUIContent("Size", "The size of the rectangle.");
      public GUIContent pivotContent = new GUIContent("Pivot", "The pivot point specified in normalized values between 0 and 1. The pivot point is the origin of this rectangle. Rotation and scaling is around this point.");
      public GUIContent transformScaleContent = new GUIContent("Scale", "The local scaling of this Game Object relative to the parent. This scales everything including image borders and text.");
      public GUIContent transformPositionZContent = new GUIContent("Pos Z", "Distance to offset the rectangle along the Z axis of the parent. The effect is visible if the Canvas uses a perspective camera, or if a parent RectTransform is rotated along the X or Y axis.");
      public GUIContent rawEditContent;
      public GUIContent blueprintContent;

      public Styles()
      {
        this.rawEditContent = EditorGUIUtility.IconContent("RectTransformRaw", "Raw edit mode. When enabled, editing pivot and anchor values will not counter-adjust the position and size of the rectangle in order to make it stay in place.");
        this.blueprintContent = EditorGUIUtility.IconContent("RectTransformBlueprint", "Blueprint mode. Edit RectTransforms as if they were not rotated and scaled. This enables snapping too.");
      }
    }

    private enum AnchorFusedState
    {
      None,
      All,
      Horizontal,
      Vertical,
    }

    private delegate float FloatGetter(RectTransform rect);

    private delegate void FloatSetter(RectTransform rect, float f);
  }
}

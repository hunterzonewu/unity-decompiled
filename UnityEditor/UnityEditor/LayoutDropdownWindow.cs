// Decompiled with JetBrains decompiler
// Type: UnityEditor.LayoutDropdownWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class LayoutDropdownWindow : PopupWindowContent
  {
    private static float[] kPivotsForModes = new float[5]{ 0.0f, 0.5f, 1f, 0.5f, 0.5f };
    private static string[] kHLabels = new string[6]{ "custom", "left", "center", "right", "stretch", "%" };
    private static string[] kVLabels = new string[6]{ "custom", "top", "middle", "bottom", "stretch", "%" };
    private const int kTopPartHeight = 38;
    private static LayoutDropdownWindow.Styles s_Styles;
    private SerializedProperty m_AnchorMin;
    private SerializedProperty m_AnchorMax;
    private SerializedProperty m_Position;
    private SerializedProperty m_SizeDelta;
    private SerializedProperty m_Pivot;
    private Vector2[,] m_InitValues;

    public LayoutDropdownWindow(SerializedObject so)
    {
      this.m_AnchorMin = so.FindProperty("m_AnchorMin");
      this.m_AnchorMax = so.FindProperty("m_AnchorMax");
      this.m_Position = so.FindProperty("m_Position");
      this.m_SizeDelta = so.FindProperty("m_SizeDelta");
      this.m_Pivot = so.FindProperty("m_Pivot");
      this.m_InitValues = new Vector2[so.targetObjects.Length, 4];
      for (int index = 0; index < so.targetObjects.Length; ++index)
      {
        RectTransform targetObject = so.targetObjects[index] as RectTransform;
        this.m_InitValues[index, 0] = targetObject.anchorMin;
        this.m_InitValues[index, 1] = targetObject.anchorMax;
        this.m_InitValues[index, 2] = targetObject.anchoredPosition;
        this.m_InitValues[index, 3] = targetObject.sizeDelta;
      }
    }

    public override void OnOpen()
    {
      EditorApplication.modifierKeysChanged += new EditorApplication.CallbackFunction(this.editorWindow.Repaint);
    }

    public override void OnClose()
    {
      EditorApplication.modifierKeysChanged -= new EditorApplication.CallbackFunction(this.editorWindow.Repaint);
    }

    public override Vector2 GetWindowSize()
    {
      return new Vector2(262f, 300f);
    }

    public override void OnGUI(Rect rect)
    {
      if (LayoutDropdownWindow.s_Styles == null)
        LayoutDropdownWindow.s_Styles = new LayoutDropdownWindow.Styles();
      if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
        this.editorWindow.Close();
      GUI.Label(new Rect(rect.x + 5f, rect.y + 3f, rect.width - 10f, 16f), new GUIContent("Anchor Presets"), EditorStyles.boldLabel);
      GUI.Label(new Rect(rect.x + 5f, (float) ((double) rect.y + 3.0 + 16.0), rect.width - 10f, 16f), new GUIContent("Shift: Also set pivot     Alt: Also set position"), EditorStyles.label);
      Color color = GUI.color;
      GUI.color = LayoutDropdownWindow.s_Styles.tableLineColor * color;
      GUI.DrawTexture(new Rect(0.0f, 37f, 400f, 1f), (Texture) EditorGUIUtility.whiteTexture);
      GUI.color = color;
      GUI.BeginGroup(new Rect(rect.x, rect.y + 38f, rect.width, rect.height - 38f));
      this.TableGUI(rect);
      GUI.EndGroup();
    }

    private static LayoutDropdownWindow.LayoutMode SwappedVMode(LayoutDropdownWindow.LayoutMode vMode)
    {
      if (vMode == LayoutDropdownWindow.LayoutMode.Min)
        return LayoutDropdownWindow.LayoutMode.Max;
      if (vMode == LayoutDropdownWindow.LayoutMode.Max)
        return LayoutDropdownWindow.LayoutMode.Min;
      return vMode;
    }

    internal static void DrawLayoutModeHeadersOutsideRect(Rect rect, SerializedProperty anchorMin, SerializedProperty anchorMax, SerializedProperty position, SerializedProperty sizeDelta)
    {
      LayoutDropdownWindow.LayoutMode layoutModeForAxis = LayoutDropdownWindow.GetLayoutModeForAxis(anchorMin, anchorMax, position, sizeDelta, 0);
      LayoutDropdownWindow.LayoutMode mode = LayoutDropdownWindow.SwappedVMode(LayoutDropdownWindow.GetLayoutModeForAxis(anchorMin, anchorMax, position, sizeDelta, 1));
      LayoutDropdownWindow.DrawLayoutModeHeaderOutsideRect(rect, 0, layoutModeForAxis);
      LayoutDropdownWindow.DrawLayoutModeHeaderOutsideRect(rect, 1, mode);
    }

    internal static void DrawLayoutModeHeaderOutsideRect(Rect position, int axis, LayoutDropdownWindow.LayoutMode mode)
    {
      Rect position1 = new Rect(position.x, position.y - 16f, position.width, 16f);
      Matrix4x4 matrix = GUI.matrix;
      if (axis == 1)
        GUIUtility.RotateAroundPivot(-90f, position.center);
      int index = (int) (mode + 1);
      GUI.Label(position1, axis != 0 ? LayoutDropdownWindow.kVLabels[index] : LayoutDropdownWindow.kHLabels[index], LayoutDropdownWindow.s_Styles.label);
      GUI.matrix = matrix;
    }

    private void TableGUI(Rect rect)
    {
      int num1 = 6;
      int num2 = 31 + num1 * 2;
      int num3 = 0;
      int[] numArray = new int[6]{ 15, 30, 30, 30, 45, 45 };
      Color color = GUI.color;
      int num4 = 62;
      GUI.color = LayoutDropdownWindow.s_Styles.tableHeaderColor * color;
      GUI.DrawTexture(new Rect(0.0f, 0.0f, 400f, (float) num4), (Texture) EditorGUIUtility.whiteTexture);
      GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) num4, 400f), (Texture) EditorGUIUtility.whiteTexture);
      GUI.color = LayoutDropdownWindow.s_Styles.tableLineColor * color;
      GUI.DrawTexture(new Rect(0.0f, (float) num4, 400f, 1f), (Texture) EditorGUIUtility.whiteTexture);
      GUI.DrawTexture(new Rect((float) num4, 0.0f, 1f, 400f), (Texture) EditorGUIUtility.whiteTexture);
      GUI.color = color;
      LayoutDropdownWindow.LayoutMode layoutModeForAxis = LayoutDropdownWindow.GetLayoutModeForAxis(this.m_AnchorMin, this.m_AnchorMax, this.m_Position, this.m_SizeDelta, 0);
      LayoutDropdownWindow.LayoutMode layoutMode1 = LayoutDropdownWindow.SwappedVMode(LayoutDropdownWindow.GetLayoutModeForAxis(this.m_AnchorMin, this.m_AnchorMax, this.m_Position, this.m_SizeDelta, 1));
      bool shift = Event.current.shift;
      bool alt = Event.current.alt;
      int num5 = 5;
      for (int index1 = 0; index1 < num5; ++index1)
      {
        LayoutDropdownWindow.LayoutMode layoutMode2 = (LayoutDropdownWindow.LayoutMode) (index1 - 1);
        for (int index2 = 0; index2 < num5; ++index2)
        {
          LayoutDropdownWindow.LayoutMode layoutMode3 = (LayoutDropdownWindow.LayoutMode) (index2 - 1);
          if (index1 != 0 || index2 != 0 || (layoutMode1 < LayoutDropdownWindow.LayoutMode.Min || layoutModeForAxis < LayoutDropdownWindow.LayoutMode.Min))
          {
            Rect position = new Rect((float) (index1 * (num2 + num3) + numArray[index1]), (float) (index2 * (num2 + num3) + numArray[index2]), (float) num2, (float) num2);
            if (index2 == 0 && (index1 != 0 || layoutModeForAxis == LayoutDropdownWindow.LayoutMode.Undefined))
              LayoutDropdownWindow.DrawLayoutModeHeaderOutsideRect(position, 0, layoutMode2);
            if (index1 == 0 && (index2 != 0 || layoutMode1 == LayoutDropdownWindow.LayoutMode.Undefined))
              LayoutDropdownWindow.DrawLayoutModeHeaderOutsideRect(position, 1, layoutMode3);
            bool flag1 = layoutMode2 == layoutModeForAxis && layoutMode3 == layoutMode1;
            bool flag2 = false;
            if (index1 == 0 && layoutMode3 == layoutMode1)
              flag2 = true;
            if (index2 == 0 && layoutMode2 == layoutModeForAxis)
              flag2 = true;
            if (Event.current.type == EventType.Repaint)
            {
              if (flag1)
              {
                GUI.color = Color.white * color;
                LayoutDropdownWindow.s_Styles.frame.Draw(position, false, false, false, false);
              }
              else if (flag2)
              {
                GUI.color = new Color(1f, 1f, 1f, 0.7f) * color;
                LayoutDropdownWindow.s_Styles.frame.Draw(position, false, false, false, false);
              }
            }
            LayoutDropdownWindow.DrawLayoutMode(new Rect(position.x + (float) num1, position.y + (float) num1, position.width - (float) (num1 * 2), position.height - (float) (num1 * 2)), layoutMode2, layoutMode3, shift, alt);
            int clickCount = Event.current.clickCount;
            if (GUI.Button(position, GUIContent.none, GUIStyle.none))
            {
              LayoutDropdownWindow.SetLayoutModeForAxis(this.m_AnchorMin, this.m_AnchorMax, this.m_Position, this.m_SizeDelta, this.m_Pivot, 0, layoutMode2, shift, alt, this.m_InitValues);
              LayoutDropdownWindow.SetLayoutModeForAxis(this.m_AnchorMin, this.m_AnchorMax, this.m_Position, this.m_SizeDelta, this.m_Pivot, 1, LayoutDropdownWindow.SwappedVMode(layoutMode3), shift, alt, this.m_InitValues);
              if (clickCount == 2)
                this.editorWindow.Close();
              else
                this.editorWindow.Repaint();
            }
          }
        }
      }
      GUI.color = color;
    }

    private static LayoutDropdownWindow.LayoutMode GetLayoutModeForAxis(SerializedProperty anchorMin, SerializedProperty anchorMax, SerializedProperty position, SerializedProperty sizeDelta, int axis)
    {
      if ((double) anchorMin.vector2Value[axis] == 0.0 && (double) anchorMax.vector2Value[axis] == 0.0)
        return LayoutDropdownWindow.LayoutMode.Min;
      if ((double) anchorMin.vector2Value[axis] == 0.5 && (double) anchorMax.vector2Value[axis] == 0.5)
        return LayoutDropdownWindow.LayoutMode.Middle;
      if ((double) anchorMin.vector2Value[axis] == 1.0 && (double) anchorMax.vector2Value[axis] == 1.0)
        return LayoutDropdownWindow.LayoutMode.Max;
      return (double) anchorMin.vector2Value[axis] == 0.0 && (double) anchorMax.vector2Value[axis] == 1.0 ? LayoutDropdownWindow.LayoutMode.Stretch : LayoutDropdownWindow.LayoutMode.Undefined;
    }

    private static void SetLayoutModeForAxis(SerializedProperty anchorMin, SerializedProperty anchorMax, SerializedProperty position, SerializedProperty sizeDelta, SerializedProperty pivot, int axis, LayoutDropdownWindow.LayoutMode layoutMode)
    {
      LayoutDropdownWindow.SetLayoutModeForAxis(anchorMin, anchorMax, position, sizeDelta, pivot, axis, layoutMode, false, false, (Vector2[,]) null);
    }

    private static void SetLayoutModeForAxis(SerializedProperty anchorMin, SerializedProperty anchorMax, SerializedProperty position, SerializedProperty sizeDelta, SerializedProperty pivot, int axis, LayoutDropdownWindow.LayoutMode layoutMode, bool doPivot)
    {
      LayoutDropdownWindow.SetLayoutModeForAxis(anchorMin, anchorMax, position, sizeDelta, pivot, axis, layoutMode, doPivot, false, (Vector2[,]) null);
    }

    private static void SetLayoutModeForAxis(SerializedProperty anchorMin, SerializedProperty anchorMax, SerializedProperty position, SerializedProperty sizeDelta, SerializedProperty pivot, int axis, LayoutDropdownWindow.LayoutMode layoutMode, bool doPivot, bool doPosition)
    {
      LayoutDropdownWindow.SetLayoutModeForAxis(anchorMin, anchorMax, position, sizeDelta, pivot, axis, layoutMode, doPivot, doPosition, (Vector2[,]) null);
    }

    private static void SetLayoutModeForAxis(SerializedProperty anchorMin, SerializedProperty anchorMax, SerializedProperty position, SerializedProperty sizeDelta, SerializedProperty pivot, int axis, LayoutDropdownWindow.LayoutMode layoutMode, bool doPivot, bool doPosition, Vector2[,] defaultValues)
    {
      anchorMin.serializedObject.ApplyModifiedProperties();
      for (int index = 0; index < anchorMin.serializedObject.targetObjects.Length; ++index)
      {
        RectTransform targetObject = anchorMin.serializedObject.targetObjects[index] as RectTransform;
        Undo.RecordObject((Object) targetObject, "Change Rectangle Anchors");
        if (doPosition && defaultValues != null && defaultValues.Length > index)
        {
          Vector2 vector2 = targetObject.anchorMin;
          vector2[axis] = defaultValues[index, 0][axis];
          targetObject.anchorMin = vector2;
          vector2 = targetObject.anchorMax;
          vector2[axis] = defaultValues[index, 1][axis];
          targetObject.anchorMax = vector2;
          vector2 = targetObject.anchoredPosition;
          vector2[axis] = defaultValues[index, 2][axis];
          targetObject.anchoredPosition = vector2;
          vector2 = targetObject.sizeDelta;
          vector2[axis] = defaultValues[index, 3][axis];
          targetObject.sizeDelta = vector2;
        }
        if (doPivot && layoutMode != LayoutDropdownWindow.LayoutMode.Undefined)
          RectTransformEditor.SetPivotSmart(targetObject, LayoutDropdownWindow.kPivotsForModes[(int) layoutMode], axis, true, true);
        Vector2 vector2_1 = Vector2.zero;
        switch (layoutMode)
        {
          case LayoutDropdownWindow.LayoutMode.Min:
            RectTransformEditor.SetAnchorSmart(targetObject, 0.0f, axis, false, true, true);
            RectTransformEditor.SetAnchorSmart(targetObject, 0.0f, axis, true, true, true);
            vector2_1 = targetObject.offsetMin;
            EditorUtility.SetDirty((Object) targetObject);
            break;
          case LayoutDropdownWindow.LayoutMode.Middle:
            RectTransformEditor.SetAnchorSmart(targetObject, 0.5f, axis, false, true, true);
            RectTransformEditor.SetAnchorSmart(targetObject, 0.5f, axis, true, true, true);
            vector2_1 = (targetObject.offsetMin + targetObject.offsetMax) * 0.5f;
            EditorUtility.SetDirty((Object) targetObject);
            break;
          case LayoutDropdownWindow.LayoutMode.Max:
            RectTransformEditor.SetAnchorSmart(targetObject, 1f, axis, false, true, true);
            RectTransformEditor.SetAnchorSmart(targetObject, 1f, axis, true, true, true);
            vector2_1 = targetObject.offsetMax;
            EditorUtility.SetDirty((Object) targetObject);
            break;
          case LayoutDropdownWindow.LayoutMode.Stretch:
            RectTransformEditor.SetAnchorSmart(targetObject, 0.0f, axis, false, true, true);
            RectTransformEditor.SetAnchorSmart(targetObject, 1f, axis, true, true, true);
            vector2_1 = (targetObject.offsetMin + targetObject.offsetMax) * 0.5f;
            EditorUtility.SetDirty((Object) targetObject);
            break;
        }
        if (doPosition)
        {
          Vector2 anchoredPosition = targetObject.anchoredPosition;
          anchoredPosition[axis] -= vector2_1[axis];
          targetObject.anchoredPosition = anchoredPosition;
          if (layoutMode == LayoutDropdownWindow.LayoutMode.Stretch)
          {
            Vector2 sizeDelta1 = targetObject.sizeDelta;
            sizeDelta1[axis] = 0.0f;
            targetObject.sizeDelta = sizeDelta1;
          }
        }
      }
      anchorMin.serializedObject.Update();
    }

    internal static void DrawLayoutMode(Rect rect, SerializedProperty anchorMin, SerializedProperty anchorMax, SerializedProperty position, SerializedProperty sizeDelta)
    {
      LayoutDropdownWindow.LayoutMode layoutModeForAxis = LayoutDropdownWindow.GetLayoutModeForAxis(anchorMin, anchorMax, position, sizeDelta, 0);
      LayoutDropdownWindow.LayoutMode vMode = LayoutDropdownWindow.SwappedVMode(LayoutDropdownWindow.GetLayoutModeForAxis(anchorMin, anchorMax, position, sizeDelta, 1));
      LayoutDropdownWindow.DrawLayoutMode(rect, layoutModeForAxis, vMode);
    }

    internal static void DrawLayoutMode(Rect position, LayoutDropdownWindow.LayoutMode hMode, LayoutDropdownWindow.LayoutMode vMode)
    {
      LayoutDropdownWindow.DrawLayoutMode(position, hMode, vMode, false, false);
    }

    internal static void DrawLayoutMode(Rect position, LayoutDropdownWindow.LayoutMode hMode, LayoutDropdownWindow.LayoutMode vMode, bool doPivot)
    {
      LayoutDropdownWindow.DrawLayoutMode(position, hMode, vMode, doPivot, false);
    }

    internal static void DrawLayoutMode(Rect position, LayoutDropdownWindow.LayoutMode hMode, LayoutDropdownWindow.LayoutMode vMode, bool doPivot, bool doPosition)
    {
      if (LayoutDropdownWindow.s_Styles == null)
        LayoutDropdownWindow.s_Styles = new LayoutDropdownWindow.Styles();
      Color color = GUI.color;
      int num1 = (int) Mathf.Min(position.width, position.height);
      if (num1 % 2 == 0)
        --num1;
      int num2 = num1 / 2;
      if (num2 % 2 == 0)
        ++num2;
      Vector2 vector2_1 = (float) num1 * Vector2.one;
      Vector2 vector2_2 = (float) num2 * Vector2.one;
      Vector2 vector2_3 = (position.size - vector2_1) / 2f;
      vector2_3.x = Mathf.Floor(vector2_3.x);
      vector2_3.y = Mathf.Floor(vector2_3.y);
      Vector2 vector2_4 = (position.size - vector2_2) / 2f;
      vector2_4.x = Mathf.Floor(vector2_4.x);
      vector2_4.y = Mathf.Floor(vector2_4.y);
      Rect position1 = new Rect(position.x + vector2_3.x, position.y + vector2_3.y, vector2_1.x, vector2_1.y);
      Rect position2 = new Rect(position.x + vector2_4.x, position.y + vector2_4.y, vector2_2.x, vector2_2.y);
      if (doPosition)
      {
        for (int index1 = 0; index1 < 2; ++index1)
        {
          LayoutDropdownWindow.LayoutMode layoutMode = index1 != 0 ? vMode : hMode;
          if (layoutMode == LayoutDropdownWindow.LayoutMode.Min)
          {
            Vector2 center = position2.center;
            // ISSUE: variable of a reference type
            Vector2& local;
            int index2;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            double num3 = (double) (^(local = @center))[index2 = index1] + ((double) position1.min[index1] - (double) position2.min[index1]);
            // ISSUE: explicit reference operation
            (^local)[index2] = (float) num3;
            position2.center = center;
          }
          if (layoutMode != LayoutDropdownWindow.LayoutMode.Middle)
            ;
          if (layoutMode == LayoutDropdownWindow.LayoutMode.Max)
          {
            Vector2 center = position2.center;
            // ISSUE: variable of a reference type
            Vector2& local;
            int index2;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            double num3 = (double) (^(local = @center))[index2 = index1] + ((double) position1.max[index1] - (double) position2.max[index1]);
            // ISSUE: explicit reference operation
            (^local)[index2] = (float) num3;
            position2.center = center;
          }
          if (layoutMode == LayoutDropdownWindow.LayoutMode.Stretch)
          {
            Vector2 min = position2.min;
            Vector2 max = position2.max;
            min[index1] = position1.min[index1];
            max[index1] = position1.max[index1];
            position2.min = min;
            position2.max = max;
          }
        }
      }
      Rect rect = new Rect();
      Vector2 zero1 = Vector2.zero;
      Vector2 zero2 = Vector2.zero;
      for (int index = 0; index < 2; ++index)
      {
        LayoutDropdownWindow.LayoutMode layoutMode = index != 0 ? vMode : hMode;
        if (layoutMode == LayoutDropdownWindow.LayoutMode.Min)
        {
          zero1[index] = position1.min[index] + 0.5f;
          zero2[index] = position1.min[index] + 0.5f;
        }
        if (layoutMode == LayoutDropdownWindow.LayoutMode.Middle)
        {
          zero1[index] = position1.center[index];
          zero2[index] = position1.center[index];
        }
        if (layoutMode == LayoutDropdownWindow.LayoutMode.Max)
        {
          zero1[index] = position1.max[index] - 0.5f;
          zero2[index] = position1.max[index] - 0.5f;
        }
        if (layoutMode == LayoutDropdownWindow.LayoutMode.Stretch)
        {
          zero1[index] = position1.min[index] + 0.5f;
          zero2[index] = position1.max[index] - 0.5f;
        }
      }
      rect.min = zero1;
      rect.max = zero2;
      if (Event.current.type == EventType.Repaint)
      {
        GUI.color = LayoutDropdownWindow.s_Styles.parentColor * color;
        LayoutDropdownWindow.s_Styles.frame.Draw(position1, false, false, false, false);
      }
      if (hMode != LayoutDropdownWindow.LayoutMode.Undefined && hMode != LayoutDropdownWindow.LayoutMode.Stretch)
      {
        GUI.color = LayoutDropdownWindow.s_Styles.simpleAnchorColor * color;
        GUI.DrawTexture(new Rect(rect.xMin - 0.5f, position1.y + 1f, 1f, position1.height - 2f), (Texture) EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMax - 0.5f, position1.y + 1f, 1f, position1.height - 2f), (Texture) EditorGUIUtility.whiteTexture);
      }
      if (vMode != LayoutDropdownWindow.LayoutMode.Undefined && vMode != LayoutDropdownWindow.LayoutMode.Stretch)
      {
        GUI.color = LayoutDropdownWindow.s_Styles.simpleAnchorColor * color;
        GUI.DrawTexture(new Rect(position1.x + 1f, rect.yMin - 0.5f, position1.width - 2f, 1f), (Texture) EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(position1.x + 1f, rect.yMax - 0.5f, position1.width - 2f, 1f), (Texture) EditorGUIUtility.whiteTexture);
      }
      if (hMode == LayoutDropdownWindow.LayoutMode.Stretch)
      {
        GUI.color = LayoutDropdownWindow.s_Styles.stretchAnchorColor * color;
        LayoutDropdownWindow.DrawArrow(new Rect(position2.x + 1f, position2.center.y - 0.5f, position2.width - 2f, 1f));
      }
      if (vMode == LayoutDropdownWindow.LayoutMode.Stretch)
      {
        GUI.color = LayoutDropdownWindow.s_Styles.stretchAnchorColor * color;
        LayoutDropdownWindow.DrawArrow(new Rect(position2.center.x - 0.5f, position2.y + 1f, 1f, position2.height - 2f));
      }
      if (Event.current.type == EventType.Repaint)
      {
        GUI.color = LayoutDropdownWindow.s_Styles.selfColor * color;
        LayoutDropdownWindow.s_Styles.frame.Draw(position2, false, false, false, false);
      }
      if (doPivot && hMode != LayoutDropdownWindow.LayoutMode.Undefined && vMode != LayoutDropdownWindow.LayoutMode.Undefined)
      {
        Vector2 vector2_5 = new Vector2(Mathf.Lerp(position2.xMin + 0.5f, position2.xMax - 0.5f, LayoutDropdownWindow.kPivotsForModes[(int) hMode]), Mathf.Lerp(position2.yMin + 0.5f, position2.yMax - 0.5f, LayoutDropdownWindow.kPivotsForModes[(int) vMode]));
        GUI.color = LayoutDropdownWindow.s_Styles.pivotColor * color;
        GUI.DrawTexture(new Rect(vector2_5.x - 2.5f, vector2_5.y - 1.5f, 5f, 3f), (Texture) EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(vector2_5.x - 1.5f, vector2_5.y - 2.5f, 3f, 5f), (Texture) EditorGUIUtility.whiteTexture);
      }
      if (hMode != LayoutDropdownWindow.LayoutMode.Undefined && vMode != LayoutDropdownWindow.LayoutMode.Undefined)
      {
        GUI.color = LayoutDropdownWindow.s_Styles.anchorCornerColor * color;
        GUI.DrawTexture(new Rect(rect.xMin - 1.5f, rect.yMin - 1.5f, 2f, 2f), (Texture) EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMax - 0.5f, rect.yMin - 1.5f, 2f, 2f), (Texture) EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMin - 1.5f, rect.yMax - 0.5f, 2f, 2f), (Texture) EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMax - 0.5f, rect.yMax - 0.5f, 2f, 2f), (Texture) EditorGUIUtility.whiteTexture);
      }
      GUI.color = color;
    }

    private static void DrawArrow(Rect lineRect)
    {
      GUI.DrawTexture(lineRect, (Texture) EditorGUIUtility.whiteTexture);
      if ((double) lineRect.width == 1.0)
      {
        GUI.DrawTexture(new Rect(lineRect.x - 1f, lineRect.y + 1f, 3f, 1f), (Texture) EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(lineRect.x - 2f, lineRect.y + 2f, 5f, 1f), (Texture) EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(lineRect.x - 1f, lineRect.yMax - 2f, 3f, 1f), (Texture) EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(lineRect.x - 2f, lineRect.yMax - 3f, 5f, 1f), (Texture) EditorGUIUtility.whiteTexture);
      }
      else
      {
        GUI.DrawTexture(new Rect(lineRect.x + 1f, lineRect.y - 1f, 1f, 3f), (Texture) EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(lineRect.x + 2f, lineRect.y - 2f, 1f, 5f), (Texture) EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(lineRect.xMax - 2f, lineRect.y - 1f, 1f, 3f), (Texture) EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(lineRect.xMax - 3f, lineRect.y - 2f, 1f, 5f), (Texture) EditorGUIUtility.whiteTexture);
      }
    }

    private class Styles
    {
      public GUIStyle label = new GUIStyle(EditorStyles.miniLabel);
      public Color tableHeaderColor;
      public Color tableLineColor;
      public Color parentColor;
      public Color selfColor;
      public Color simpleAnchorColor;
      public Color stretchAnchorColor;
      public Color anchorCornerColor;
      public Color pivotColor;
      public GUIStyle frame;

      public Styles()
      {
        this.frame = new GUIStyle();
        Texture2D texture2D = new Texture2D(4, 4);
        texture2D.SetPixels(new Color[16]
        {
          Color.white,
          Color.white,
          Color.white,
          Color.white,
          Color.white,
          Color.clear,
          Color.clear,
          Color.white,
          Color.white,
          Color.clear,
          Color.clear,
          Color.white,
          Color.white,
          Color.white,
          Color.white,
          Color.white
        });
        texture2D.filterMode = UnityEngine.FilterMode.Point;
        texture2D.Apply();
        texture2D.hideFlags = HideFlags.HideAndDontSave;
        this.frame.normal.background = texture2D;
        this.frame.border = new RectOffset(2, 2, 2, 2);
        this.label.alignment = TextAnchor.LowerCenter;
        if (EditorGUIUtility.isProSkin)
        {
          this.tableHeaderColor = new Color(0.18f, 0.18f, 0.18f, 1f);
          this.tableLineColor = new Color(1f, 1f, 1f, 0.3f);
          this.parentColor = new Color(0.4f, 0.4f, 0.4f, 1f);
          this.selfColor = new Color(0.6f, 0.6f, 0.6f, 1f);
          this.simpleAnchorColor = new Color(0.7f, 0.3f, 0.3f, 1f);
          this.stretchAnchorColor = new Color(0.0f, 0.6f, 0.8f, 1f);
          this.anchorCornerColor = new Color(0.8f, 0.6f, 0.0f, 1f);
          this.pivotColor = new Color(0.0f, 0.6f, 0.8f, 1f);
        }
        else
        {
          this.tableHeaderColor = new Color(0.8f, 0.8f, 0.8f, 1f);
          this.tableLineColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
          this.parentColor = new Color(0.55f, 0.55f, 0.55f, 1f);
          this.selfColor = new Color(0.2f, 0.2f, 0.2f, 1f);
          this.simpleAnchorColor = new Color(0.8f, 0.3f, 0.3f, 1f);
          this.stretchAnchorColor = new Color(0.2f, 0.5f, 0.9f, 1f);
          this.anchorCornerColor = new Color(0.6f, 0.4f, 0.0f, 1f);
          this.pivotColor = new Color(0.2f, 0.5f, 0.9f, 1f);
        }
      }
    }

    public enum LayoutMode
    {
      Undefined = -1,
      Min = 0,
      Middle = 1,
      Max = 2,
      Stretch = 3,
    }
  }
}

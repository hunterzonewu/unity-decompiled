// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class ModuleUI : SerializedModule
  {
    protected static readonly bool kUseSignedRange = true;
    protected static readonly Rect kUnsignedRange = new Rect(0.0f, 0.0f, 1f, 1f);
    protected static readonly Rect kSignedRange = new Rect(0.0f, -1f, 1f, 2f);
    public static float k_CompactFixedModuleWidth = 195f;
    public static float k_SpaceBetweenModules = 5f;
    private static readonly GUIStyle s_ControlRectStyle = new GUIStyle() { margin = new RectOffset(0, 0, 2, 2) };
    protected string m_ToolTip = string.Empty;
    public List<SerializedProperty> m_ModuleCurves = new List<SerializedProperty>();
    private List<SerializedProperty> m_CurvesRemovedWhenFolded = new List<SerializedProperty>();
    protected const int kSingleLineHeight = 13;
    protected const float k_minMaxToggleWidth = 13f;
    protected const float k_toggleWidth = 9f;
    protected const float kDragSpace = 20f;
    protected const int kPlusAddRemoveButtonWidth = 12;
    protected const int kPlusAddRemoveButtonSpacing = 5;
    protected const int kSpacingSubLabel = 4;
    protected const int kSubLabelWidth = 10;
    protected const string kFormatString = "g7";
    public ParticleSystemUI m_ParticleSystemUI;
    private string m_DisplayName;
    private SerializedProperty m_Enabled;
    private ModuleUI.VisibilityState m_VisibilityState;

    public bool visibleUI
    {
      get
      {
        return this.m_VisibilityState != ModuleUI.VisibilityState.NotVisible;
      }
      set
      {
        this.SetVisibilityState(!value ? ModuleUI.VisibilityState.NotVisible : ModuleUI.VisibilityState.VisibleAndFolded);
      }
    }

    public bool foldout
    {
      get
      {
        return this.m_VisibilityState == ModuleUI.VisibilityState.VisibleAndFoldedOut;
      }
      set
      {
        this.SetVisibilityState(!value ? ModuleUI.VisibilityState.VisibleAndFolded : ModuleUI.VisibilityState.VisibleAndFoldedOut);
      }
    }

    public bool enabled
    {
      get
      {
        return this.m_Enabled.boolValue;
      }
      set
      {
        if (this.m_Enabled.boolValue == value)
          return;
        this.m_Enabled.boolValue = value;
        if (value)
          this.OnModuleEnable();
        else
          this.OnModuleDisable();
      }
    }

    public string displayName
    {
      get
      {
        return this.m_DisplayName;
      }
    }

    public string toolTip
    {
      get
      {
        return this.m_ToolTip;
      }
    }

    public ModuleUI(ParticleSystemUI owner, SerializedObject o, string name, string displayName)
      : base(o, name)
    {
      this.Setup(owner, o, name, displayName, ModuleUI.VisibilityState.NotVisible);
    }

    public ModuleUI(ParticleSystemUI owner, SerializedObject o, string name, string displayName, ModuleUI.VisibilityState initialVisibilityState)
      : base(o, name)
    {
      this.Setup(owner, o, name, displayName, initialVisibilityState);
    }

    private void Setup(ParticleSystemUI owner, SerializedObject o, string name, string displayName, ModuleUI.VisibilityState defaultVisibilityState)
    {
      this.m_ParticleSystemUI = owner;
      this.m_DisplayName = displayName;
      this.m_Enabled = !(this is RendererModuleUI) ? this.GetProperty("enabled") : this.GetProperty0("m_Enabled");
      this.m_VisibilityState = (ModuleUI.VisibilityState) SessionState.GetInt(this.GetUniqueModuleName(), (int) defaultVisibilityState);
      this.CheckVisibilityState();
      if (!this.foldout)
        return;
      this.Init();
    }

    protected abstract void Init();

    public virtual void Validate()
    {
    }

    public virtual float GetXAxisScalar()
    {
      return 1f;
    }

    public abstract void OnInspectorGUI(ParticleSystem s);

    public virtual void OnSceneGUI(ParticleSystem s, InitialModuleUI initial)
    {
    }

    public virtual void UpdateCullingSupportedString(ref string text)
    {
    }

    protected virtual void OnModuleEnable()
    {
      this.Init();
    }

    protected virtual void OnModuleDisable()
    {
      ParticleSystemCurveEditor systemCurveEditor = this.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor();
      using (List<SerializedProperty>.Enumerator enumerator = this.m_ModuleCurves.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SerializedProperty current = enumerator.Current;
          if (systemCurveEditor.IsAdded(current))
            systemCurveEditor.RemoveCurve(current);
        }
      }
    }

    internal void CheckVisibilityState()
    {
      if (!(this is RendererModuleUI) && !this.m_Enabled.boolValue && !ParticleEffectUI.GetAllModulesVisible())
        this.SetVisibilityState(ModuleUI.VisibilityState.NotVisible);
      if (!this.m_Enabled.boolValue || this.visibleUI)
        return;
      this.SetVisibilityState(ModuleUI.VisibilityState.VisibleAndFolded);
    }

    protected virtual void SetVisibilityState(ModuleUI.VisibilityState newState)
    {
      if (newState == this.m_VisibilityState)
        return;
      if (newState == ModuleUI.VisibilityState.VisibleAndFolded)
      {
        ParticleSystemCurveEditor systemCurveEditor = this.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor();
        using (List<SerializedProperty>.Enumerator enumerator = this.m_ModuleCurves.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            SerializedProperty current = enumerator.Current;
            if (systemCurveEditor.IsAdded(current))
            {
              this.m_CurvesRemovedWhenFolded.Add(current);
              systemCurveEditor.SetVisible(current, false);
            }
          }
        }
        systemCurveEditor.Refresh();
      }
      else if (newState == ModuleUI.VisibilityState.VisibleAndFoldedOut)
      {
        ParticleSystemCurveEditor systemCurveEditor = this.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor();
        using (List<SerializedProperty>.Enumerator enumerator = this.m_CurvesRemovedWhenFolded.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            SerializedProperty current = enumerator.Current;
            systemCurveEditor.SetVisible(current, true);
          }
        }
        this.m_CurvesRemovedWhenFolded.Clear();
        systemCurveEditor.Refresh();
      }
      this.m_VisibilityState = newState;
      SessionState.SetInt(this.GetUniqueModuleName(), (int) this.m_VisibilityState);
      if (newState != ModuleUI.VisibilityState.VisibleAndFoldedOut)
        return;
      this.Init();
    }

    protected ParticleSystem GetParticleSystem()
    {
      return this.m_Enabled.serializedObject.targetObject as ParticleSystem;
    }

    public ParticleSystemCurveEditor GetParticleSystemCurveEditor()
    {
      return this.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor();
    }

    public void AddToModuleCurves(SerializedProperty curveProp)
    {
      this.m_ModuleCurves.Add(curveProp);
      if (this.foldout)
        return;
      this.m_CurvesRemovedWhenFolded.Add(curveProp);
    }

    private static void Label(Rect rect, GUIContent guiContent)
    {
      GUI.Label(rect, guiContent, ParticleSystemStyles.Get().label);
    }

    protected static Rect GetControlRect(int height)
    {
      return GUILayoutUtility.GetRect(0.0f, (float) height, ModuleUI.s_ControlRectStyle);
    }

    private static Rect PrefixLabel(Rect totalPosition, GUIContent label)
    {
      Rect labelPosition = new Rect(totalPosition.x + EditorGUI.indent, totalPosition.y, EditorGUIUtility.labelWidth - EditorGUI.indent, 13f);
      Rect rect = new Rect(totalPosition.x + EditorGUIUtility.labelWidth, totalPosition.y, totalPosition.width - EditorGUIUtility.labelWidth, totalPosition.height);
      EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, 0, ParticleSystemStyles.Get().label);
      return rect;
    }

    private static Rect SubtractPopupWidth(Rect position)
    {
      position.width -= 14f;
      return position;
    }

    private static Rect GetPopupRect(Rect position)
    {
      position.xMin = position.xMax - 13f;
      return position;
    }

    protected static bool PlusButton(Rect position)
    {
      return GUI.Button(new Rect(position.x - 2f, position.y - 2f, 12f, 13f), GUIContent.none, (GUIStyle) "OL Plus");
    }

    protected static bool MinusButton(Rect position)
    {
      return GUI.Button(new Rect(position.x - 2f, position.y - 2f, 12f, 13f), GUIContent.none, (GUIStyle) "OL Minus");
    }

    private static float FloatDraggable(Rect rect, SerializedProperty floatProp, float remap, float dragWidth)
    {
      return ModuleUI.FloatDraggable(rect, floatProp, remap, dragWidth, "g7");
    }

    public static float FloatDraggable(Rect rect, float floatValue, float remap, float dragWidth, string formatString)
    {
      int controlId = GUIUtility.GetControlID(1658656233, FocusType.Keyboard, rect);
      Rect dragHotZone = rect;
      dragHotZone.width = dragWidth;
      Rect position = rect;
      position.xMin += dragWidth;
      return EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, position, dragHotZone, controlId, floatValue * remap, formatString, ParticleSystemStyles.Get().numberField, true) / remap;
    }

    public static float FloatDraggable(Rect rect, SerializedProperty floatProp, float remap, float dragWidth, string formatString)
    {
      Color color = GUI.color;
      if (floatProp.isAnimated)
        GUI.color = AnimationMode.animatedPropertyColor;
      float floatValue = floatProp.floatValue;
      float num = ModuleUI.FloatDraggable(rect, floatValue, remap, dragWidth, formatString);
      if ((double) num != (double) floatValue)
        floatProp.floatValue = num;
      GUI.color = color;
      return num;
    }

    public static Vector3 GUIVector3Field(GUIContent guiContent, SerializedProperty vecProp)
    {
      Rect rect = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13), guiContent);
      GUIContent[] guiContentArray = new GUIContent[3]{ new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z") };
      float num = (float) (((double) rect.width - 8.0) / 3.0);
      rect.width = num;
      Vector3 vector3Value = vecProp.vector3Value;
      for (int index = 0; index < 3; ++index)
      {
        ModuleUI.Label(rect, guiContentArray[index]);
        vector3Value[index] = ModuleUI.FloatDraggable(rect, vector3Value[index], 1f, 25f, "g5");
        rect.x += num + 4f;
      }
      vecProp.vector3Value = vector3Value;
      return vector3Value;
    }

    public static float GUIFloat(string label, SerializedProperty floatProp)
    {
      return ModuleUI.GUIFloat(GUIContent.Temp(label), floatProp);
    }

    public static float GUIFloat(GUIContent guiContent, SerializedProperty floatProp)
    {
      return ModuleUI.GUIFloat(guiContent, floatProp, "g7");
    }

    public static float GUIFloat(GUIContent guiContent, SerializedProperty floatProp, string formatString)
    {
      Rect controlRect = ModuleUI.GetControlRect(13);
      ModuleUI.PrefixLabel(controlRect, guiContent);
      return ModuleUI.FloatDraggable(controlRect, floatProp, 1f, EditorGUIUtility.labelWidth, formatString);
    }

    public static float GUIFloat(GUIContent guiContent, float floatValue, string formatString)
    {
      Rect controlRect = ModuleUI.GetControlRect(13);
      ModuleUI.PrefixLabel(controlRect, guiContent);
      return ModuleUI.FloatDraggable(controlRect, floatValue, 1f, EditorGUIUtility.labelWidth, formatString);
    }

    private static bool Toggle(Rect rect, SerializedProperty boolProp)
    {
      Color color = GUI.color;
      if (boolProp.isAnimated)
        GUI.color = AnimationMode.animatedPropertyColor;
      bool boolValue = boolProp.boolValue;
      bool flag = EditorGUI.Toggle(rect, boolValue, ParticleSystemStyles.Get().toggle);
      if (boolValue != flag)
        boolProp.boolValue = flag;
      GUI.color = color;
      return flag;
    }

    public static bool GUIToggle(string label, SerializedProperty boolProp)
    {
      return ModuleUI.GUIToggle(GUIContent.Temp(label), boolProp);
    }

    public static bool GUIToggle(GUIContent guiContent, SerializedProperty boolProp)
    {
      return ModuleUI.Toggle(ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13), guiContent), boolProp);
    }

    public static void GUILayerMask(GUIContent guiContent, SerializedProperty boolProp)
    {
      EditorGUI.LayerMaskField(ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13), guiContent), boolProp, GUIContent.none, ParticleSystemStyles.Get().popup);
    }

    public static bool GUIToggle(GUIContent guiContent, bool boolValue)
    {
      boolValue = EditorGUI.Toggle(ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13), guiContent), boolValue, ParticleSystemStyles.Get().toggle);
      return boolValue;
    }

    public static void GUIToggleWithFloatField(string name, SerializedProperty boolProp, SerializedProperty floatProp, bool invertToggle)
    {
      ModuleUI.GUIToggleWithFloatField(EditorGUIUtility.TempContent(name), boolProp, floatProp, invertToggle);
    }

    public static void GUIToggleWithFloatField(GUIContent guiContent, SerializedProperty boolProp, SerializedProperty floatProp, bool invertToggle)
    {
      Rect rect1 = ModuleUI.PrefixLabel(GUILayoutUtility.GetRect(0.0f, 13f), guiContent);
      Rect rect2 = rect1;
      rect2.xMax = rect2.x + 9f;
      bool flag = ModuleUI.Toggle(rect2, boolProp);
      if (!(!invertToggle ? flag : !flag))
        return;
      float dragWidth = 25f;
      double num = (double) ModuleUI.FloatDraggable(new Rect((float) ((double) rect1.x + (double) EditorGUIUtility.labelWidth + 9.0), rect1.y, rect1.width - 9f, rect1.height), floatProp, 1f, dragWidth);
    }

    public static void GUIToggleWithIntField(string name, SerializedProperty boolProp, SerializedProperty floatProp, bool invertToggle)
    {
      ModuleUI.GUIToggleWithIntField(EditorGUIUtility.TempContent(name), boolProp, floatProp, invertToggle);
    }

    public static void GUIToggleWithIntField(GUIContent guiContent, SerializedProperty boolProp, SerializedProperty intProp, bool invertToggle)
    {
      Rect rect1 = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13), guiContent);
      Rect rect2 = rect1;
      rect2.xMax = rect2.x + 9f;
      bool flag = ModuleUI.Toggle(rect2, boolProp);
      if (!(!invertToggle ? flag : !flag))
        return;
      float dragWidth = 25f;
      Rect rect3 = new Rect((float) ((double) rect1.x + (double) EditorGUIUtility.labelWidth + 9.0), rect1.y, rect1.width - 9f, rect1.height);
      intProp.intValue = ModuleUI.IntDraggable(rect3, (GUIContent) null, intProp.intValue, dragWidth);
    }

    public static void GUIObject(GUIContent label, SerializedProperty objectProp)
    {
      EditorGUI.ObjectField(ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13), label), objectProp, (System.Type) null, GUIContent.none, ParticleSystemStyles.Get().objectField);
    }

    public static void GUIObjectFieldAndToggle(GUIContent label, SerializedProperty objectProp, SerializedProperty boolProp)
    {
      Rect rect = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13), label);
      rect.xMax -= 19f;
      EditorGUI.ObjectField(rect, objectProp, GUIContent.none);
      if (boolProp == null)
        return;
      rect.x += rect.width + 10f;
      rect.width = 9f;
      ModuleUI.Toggle(rect, boolProp);
    }

    internal UnityEngine.Object ParticleSystemValidator(UnityEngine.Object[] references, System.Type objType, SerializedProperty property)
    {
      foreach (UnityEngine.Object reference in references)
      {
        if (reference != (UnityEngine.Object) null)
        {
          GameObject gameObject = reference as GameObject;
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          {
            ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
            if ((bool) ((UnityEngine.Object) component))
              return (UnityEngine.Object) component;
          }
        }
      }
      return (UnityEngine.Object) null;
    }

    public int GUIListOfFloatObjectToggleFields(GUIContent label, SerializedProperty[] objectProps, EditorGUI.ObjectFieldValidator validator, GUIContent buttonTooltip, bool allowCreation)
    {
      int num1 = -1;
      int length = objectProps.Length;
      Rect rect1 = GUILayoutUtility.GetRect(0.0f, (float) (15 * length));
      rect1.height = 13f;
      float num2 = 10f;
      float num3 = 35f;
      float num4 = 10f;
      float width = (float) ((double) rect1.width - (double) num2 - (double) num3 - (double) num4 * 2.0 - 9.0);
      ModuleUI.PrefixLabel(rect1, label);
      for (int index = 0; index < length; ++index)
      {
        SerializedProperty objectProp = objectProps[index];
        Rect rect2 = new Rect(rect1.x + num2 + num3 + num4, rect1.y, width, rect1.height);
        int controlId = GUIUtility.GetControlID(1235498, EditorGUIUtility.native, rect2);
        EditorGUI.DoObjectField(rect2, rect2, controlId, (UnityEngine.Object) null, (System.Type) null, objectProp, validator, true, ParticleSystemStyles.Get().objectField);
        if (objectProp.objectReferenceValue == (UnityEngine.Object) null)
        {
          rect2 = new Rect(rect1.xMax - 9f, rect1.y + 3f, 9f, 9f);
          if (!allowCreation || GUI.Button(rect2, buttonTooltip ?? GUIContent.none, ParticleSystemStyles.Get().plus))
            num1 = index;
        }
        rect1.y += 15f;
      }
      return num1;
    }

    public static int GUIIntDraggable(GUIContent label, SerializedProperty intProp)
    {
      return ModuleUI.GUIIntDraggable(label, intProp.intValue);
    }

    public static int GUIIntDraggable(GUIContent label, int intValue)
    {
      return ModuleUI.IntDraggable(GUILayoutUtility.GetRect(0.0f, 13f), label, intValue, EditorGUIUtility.labelWidth);
    }

    public static void GUIIntDraggableX2(GUIContent mainLabel, GUIContent label1, SerializedProperty intProp1, GUIContent label2, SerializedProperty intProp2)
    {
      Rect rect1 = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13), mainLabel);
      float width = (float) (((double) rect1.width - 4.0) * 0.5);
      Rect rect2 = new Rect(rect1.x, rect1.y, width, rect1.height);
      ModuleUI.IntDraggable(rect2, label1, intProp1, 10f);
      rect2.x += width + 4f;
      ModuleUI.IntDraggable(rect2, label2, intProp2, 10f);
    }

    public static int IntDraggable(Rect rect, GUIContent label, SerializedProperty intProp, float dragWidth)
    {
      intProp.intValue = ModuleUI.IntDraggable(rect, label, intProp.intValue, dragWidth);
      return intProp.intValue;
    }

    public static int GUIInt(GUIContent guiContent, int intValue)
    {
      Rect rect = GUILayoutUtility.GetRect(0.0f, 13f);
      ModuleUI.PrefixLabel(rect, guiContent);
      return ModuleUI.IntDraggable(rect, (GUIContent) null, intValue, EditorGUIUtility.labelWidth);
    }

    public static void GUIInt(GUIContent guiContent, SerializedProperty intProp)
    {
      intProp.intValue = ModuleUI.GUIInt(guiContent, intProp.intValue);
    }

    public static int IntDraggable(Rect rect, GUIContent label, int value, float dragWidth)
    {
      float width = rect.width;
      Rect position1 = rect;
      position1.width = width;
      int controlId = GUIUtility.GetControlID(16586232, FocusType.Keyboard, position1);
      Rect rect1 = position1;
      rect1.width = dragWidth;
      if (label != null && !string.IsNullOrEmpty(label.text))
        ModuleUI.Label(rect1, label);
      Rect position2 = position1;
      position2.x += dragWidth;
      position2.width = width - dragWidth;
      float dragSensitivity = Mathf.Max(1f, Mathf.Pow(Mathf.Abs((float) value), 0.5f) * 0.03f);
      return (int) EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, position2, rect1, controlId, (float) value, EditorGUI.kIntFieldFormatString, ParticleSystemStyles.Get().numberField, true, dragSensitivity);
    }

    public static void GUIMinMaxRange(GUIContent label, SerializedProperty vec2Prop)
    {
      Rect rect = ModuleUI.PrefixLabel(ModuleUI.SubtractPopupWidth(ModuleUI.GetControlRect(13)), label);
      float num = (float) (((double) rect.width - 20.0) * 0.5);
      Vector2 vector2Value = vec2Prop.vector2Value;
      rect.width = num;
      rect.xMin -= 20f;
      vector2Value.x = ModuleUI.FloatDraggable(rect, vector2Value.x, 1f, 20f, "g7");
      vector2Value.x = Mathf.Clamp(vector2Value.x, 0.0f, vector2Value.y - 0.01f);
      rect.x += num + 20f;
      vector2Value.y = ModuleUI.FloatDraggable(rect, vector2Value.y, 1f, 20f, "g7");
      vector2Value.y = Mathf.Max(vector2Value.x + 0.01f, vector2Value.y);
      vec2Prop.vector2Value = vector2Value;
    }

    public static void GUISlider(SerializedProperty floatProp, float a, float b, float remap)
    {
      ModuleUI.GUISlider(string.Empty, floatProp, a, b, remap);
    }

    public static void GUISlider(string name, SerializedProperty floatProp, float a, float b, float remap)
    {
      floatProp.floatValue = (float) ((double) EditorGUILayout.Slider(name, (float) ((double) floatProp.floatValue * (double) remap), a, b, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(300f)
      }) / (double) remap);
    }

    public static void GUIMinMaxSlider(GUIContent label, SerializedProperty vec2Prop, float a, float b)
    {
      Rect controlRect = ModuleUI.GetControlRect(26);
      controlRect.height = 13f;
      controlRect.y += 3f;
      ModuleUI.PrefixLabel(controlRect, label);
      Vector2 vector2Value = vec2Prop.vector2Value;
      controlRect.y += 13f;
      EditorGUI.MinMaxSlider(controlRect, ref vector2Value.x, ref vector2Value.y, a, b);
      vec2Prop.vector2Value = vector2Value;
    }

    public static bool GUIBoolAsPopup(GUIContent label, SerializedProperty boolProp, string[] options)
    {
      Rect position = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13), label);
      int selectedIndex = !boolProp.boolValue ? 0 : 1;
      int num = EditorGUI.Popup(position, selectedIndex, options, ParticleSystemStyles.Get().popup);
      if (num != selectedIndex)
        boolProp.boolValue = num > 0;
      return num > 0;
    }

    public static int GUIPopup(string name, SerializedProperty intProp, string[] options)
    {
      return ModuleUI.GUIPopup(GUIContent.Temp(name), intProp, options);
    }

    public static int GUIPopup(GUIContent label, SerializedProperty intProp, string[] options)
    {
      Rect position = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13), label);
      intProp.intValue = EditorGUI.Popup(position, intProp.intValue, options, ParticleSystemStyles.Get().popup);
      return intProp.intValue;
    }

    public static int GUIPopup(GUIContent label, int intValue, string[] options)
    {
      return EditorGUI.Popup(ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13), label), intValue, options, ParticleSystemStyles.Get().popup);
    }

    private static Color GetColor(SerializedMinMaxCurve mmCurve)
    {
      return mmCurve.m_Module.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor().GetCurveColor(mmCurve.maxCurve);
    }

    private static void GUICurveField(Rect position, SerializedProperty maxCurve, SerializedProperty minCurve, Color color, Rect ranges, ModuleUI.CurveFieldMouseDownCallback mouseDownCallback)
    {
      int controlId = GUIUtility.GetControlID(1321321231, EditorGUIUtility.native, position);
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (!position.Contains(current.mousePosition) || mouseDownCallback == null || !mouseDownCallback(current.button, position, ranges))
            break;
          current.Use();
          break;
        case EventType.Repaint:
          Rect position1 = position;
          if (minCurve == null)
            EditorGUIUtility.DrawCurveSwatch(position1, (AnimationCurve) null, maxCurve, color, EditorGUI.kCurveBGColor, ranges);
          else
            EditorGUIUtility.DrawRegionSwatch(position1, maxCurve, minCurve, color, EditorGUI.kCurveBGColor, ranges);
          EditorStyles.colorPickerBox.Draw(position1, GUIContent.none, controlId, false);
          break;
        case EventType.ValidateCommand:
          if (!(current.commandName == "UndoRedoPerformed"))
            break;
          AnimationCurvePreviewCache.ClearCache();
          break;
      }
    }

    public static void GUIMinMaxCurve(string label, SerializedMinMaxCurve mmCurve)
    {
      ModuleUI.GUIMinMaxCurve(GUIContent.Temp(label), mmCurve);
    }

    public static void GUIMinMaxCurve(GUIContent label, SerializedMinMaxCurve mmCurve)
    {
      Rect controlRect = ModuleUI.GetControlRect(13);
      Rect popupRect = ModuleUI.GetPopupRect(controlRect);
      Rect rect1 = ModuleUI.SubtractPopupWidth(controlRect);
      Rect position = ModuleUI.PrefixLabel(rect1, label);
      MinMaxCurveState state = mmCurve.state;
      switch (state)
      {
        case MinMaxCurveState.k_Scalar:
          float a = ModuleUI.FloatDraggable(rect1, mmCurve.scalar, mmCurve.m_RemapValue, EditorGUIUtility.labelWidth);
          if (!mmCurve.signedRange)
          {
            mmCurve.scalar.floatValue = Mathf.Max(a, 0.0f);
            break;
          }
          break;
        case MinMaxCurveState.k_TwoScalars:
          Rect rect2 = position;
          rect2.width = (float) (((double) position.width - 20.0) * 0.5);
          float minConstant = mmCurve.minConstant;
          float maxConstant = mmCurve.maxConstant;
          Rect rect3 = rect2;
          rect3.xMin -= 20f;
          EditorGUI.BeginChangeCheck();
          float num1 = ModuleUI.FloatDraggable(rect3, minConstant, mmCurve.m_RemapValue, 20f, "g5");
          if (EditorGUI.EndChangeCheck())
            mmCurve.minConstant = num1;
          rect3.x += rect2.width + 20f;
          EditorGUI.BeginChangeCheck();
          float num2 = ModuleUI.FloatDraggable(rect3, maxConstant, mmCurve.m_RemapValue, 20f, "g5");
          if (EditorGUI.EndChangeCheck())
          {
            mmCurve.maxConstant = num2;
            break;
          }
          break;
        default:
          Rect ranges = !mmCurve.signedRange ? ModuleUI.kUnsignedRange : ModuleUI.kSignedRange;
          SerializedProperty minCurve = state != MinMaxCurveState.k_TwoCurves ? (SerializedProperty) null : mmCurve.minCurve;
          ModuleUI.GUICurveField(position, mmCurve.maxCurve, minCurve, ModuleUI.GetColor(mmCurve), ranges, new ModuleUI.CurveFieldMouseDownCallback(mmCurve.OnCurveAreaMouseDown));
          break;
      }
      ModuleUI.GUIMMCurveStateList(popupRect, mmCurve);
    }

    public void GUIMinMaxGradient(GUIContent label, SerializedMinMaxGradient minMaxGradient)
    {
      MinMaxGradientState state = minMaxGradient.state;
      Rect rect1 = GUILayoutUtility.GetRect(0.0f, state < MinMaxGradientState.k_RandomBetweenTwoColors ? 13f : 26f);
      Rect popupRect = ModuleUI.GetPopupRect(rect1);
      Rect rect2 = ModuleUI.PrefixLabel(ModuleUI.SubtractPopupWidth(rect1), label);
      rect2.height = 13f;
      switch (state)
      {
        case MinMaxGradientState.k_Color:
          ModuleUI.GUIColor(rect2, minMaxGradient.m_MaxColor);
          break;
        case MinMaxGradientState.k_Gradient:
          EditorGUI.GradientField(rect2, minMaxGradient.m_MaxGradient);
          break;
        case MinMaxGradientState.k_RandomBetweenTwoColors:
          ModuleUI.GUIColor(rect2, minMaxGradient.m_MaxColor);
          rect2.y += rect2.height;
          ModuleUI.GUIColor(rect2, minMaxGradient.m_MinColor);
          break;
        case MinMaxGradientState.k_RandomBetweenTwoGradients:
          EditorGUI.GradientField(rect2, minMaxGradient.m_MaxGradient);
          rect2.y += rect2.height;
          EditorGUI.GradientField(rect2, minMaxGradient.m_MinGradient);
          break;
      }
      ModuleUI.GUIMMGradientPopUp(popupRect, minMaxGradient);
    }

    private static void GUIGradientAsColor(Rect rect, SerializedProperty gradientProp)
    {
      bool changed = GUI.changed;
      GUI.changed = false;
      Color gradientAsColor = SerializedMinMaxGradient.GetGradientAsColor(gradientProp);
      Color color = EditorGUI.ColorField(rect, gradientAsColor, false, true);
      if (GUI.changed)
        SerializedMinMaxGradient.SetGradientAsColor(gradientProp, color);
      GUI.changed |= changed;
    }

    private static void GUIColor(Rect rect, SerializedProperty colorProp)
    {
      colorProp.colorValue = EditorGUI.ColorField(rect, colorProp.colorValue, false, true);
    }

    public void GUIMinMaxColor(GUIContent label, SerializedMinMaxColor minMaxColor)
    {
      Rect rect1 = ModuleUI.PrefixLabel(ModuleUI.GetControlRect(13), label);
      float width = (float) ((double) rect1.width - 13.0 - 5.0);
      if (!minMaxColor.minMax.boolValue)
      {
        ModuleUI.GUIColor(new Rect(rect1.x, rect1.y, width, rect1.height), minMaxColor.maxColor);
      }
      else
      {
        Rect rect2 = new Rect(rect1.x, rect1.y, (float) ((double) width * 0.5 - 2.0), rect1.height);
        ModuleUI.GUIColor(rect2, minMaxColor.minColor);
        rect2.x += rect2.width + 4f;
        ModuleUI.GUIColor(rect2, minMaxColor.maxColor);
      }
      ModuleUI.GUIMMColorPopUp(new Rect(rect1.xMax - 13f, rect1.y, 13f, 13f), minMaxColor.minMax);
    }

    public void GUITripleMinMaxCurve(GUIContent label, GUIContent x, SerializedMinMaxCurve xCurve, GUIContent y, SerializedMinMaxCurve yCurve, GUIContent z, SerializedMinMaxCurve zCurve, SerializedProperty randomizePerFrame)
    {
      MinMaxCurveState state = xCurve.state;
      bool flag = label != GUIContent.none;
      int num1 = !flag ? 1 : 2;
      if (state == MinMaxCurveState.k_TwoScalars)
        ++num1;
      Rect controlRect = ModuleUI.GetControlRect(13 * num1);
      Rect popupRect = ModuleUI.GetPopupRect(controlRect);
      Rect totalPosition = ModuleUI.SubtractPopupWidth(controlRect);
      Rect rect = totalPosition;
      float num2 = (float) (((double) totalPosition.width - 8.0) / 3.0);
      if (num1 > 1)
        rect.height = 13f;
      if (flag)
      {
        ModuleUI.PrefixLabel(totalPosition, label);
        rect.y += rect.height;
      }
      rect.width = num2;
      GUIContent[] guiContentArray = new GUIContent[3]{ x, y, z };
      SerializedMinMaxCurve[] minMaxCurves = new SerializedMinMaxCurve[3]{ xCurve, yCurve, zCurve };
      if (state == MinMaxCurveState.k_Scalar)
      {
        for (int index = 0; index < minMaxCurves.Length; ++index)
        {
          ModuleUI.Label(rect, guiContentArray[index]);
          float a = ModuleUI.FloatDraggable(rect, minMaxCurves[index].scalar, minMaxCurves[index].m_RemapValue, 10f);
          if (!minMaxCurves[index].signedRange)
            minMaxCurves[index].scalar.floatValue = Mathf.Max(a, 0.0f);
          rect.x += num2 + 4f;
        }
      }
      else if (state == MinMaxCurveState.k_TwoScalars)
      {
        for (int index = 0; index < minMaxCurves.Length; ++index)
        {
          ModuleUI.Label(rect, guiContentArray[index]);
          float minConstant = minMaxCurves[index].minConstant;
          float maxConstant = minMaxCurves[index].maxConstant;
          EditorGUI.BeginChangeCheck();
          float num3 = ModuleUI.FloatDraggable(rect, maxConstant, minMaxCurves[index].m_RemapValue, 10f, "g5");
          if (EditorGUI.EndChangeCheck())
            minMaxCurves[index].maxConstant = num3;
          rect.y += 13f;
          EditorGUI.BeginChangeCheck();
          float num4 = ModuleUI.FloatDraggable(rect, minConstant, minMaxCurves[index].m_RemapValue, 10f, "g5");
          if (EditorGUI.EndChangeCheck())
            minMaxCurves[index].minConstant = num4;
          rect.x += num2 + 4f;
          rect.y -= 13f;
        }
      }
      else
      {
        rect.width = num2;
        Rect ranges = !xCurve.signedRange ? ModuleUI.kUnsignedRange : ModuleUI.kSignedRange;
        for (int index = 0; index < minMaxCurves.Length; ++index)
        {
          ModuleUI.Label(rect, guiContentArray[index]);
          Rect position = rect;
          position.xMin += 10f;
          SerializedProperty minCurve = state != MinMaxCurveState.k_TwoCurves ? (SerializedProperty) null : minMaxCurves[index].minCurve;
          ModuleUI.GUICurveField(position, minMaxCurves[index].maxCurve, minCurve, ModuleUI.GetColor(minMaxCurves[index]), ranges, new ModuleUI.CurveFieldMouseDownCallback(minMaxCurves[index].OnCurveAreaMouseDown));
          rect.x += num2 + 4f;
        }
      }
      ModuleUI.GUIMMCurveStateList(popupRect, minMaxCurves);
    }

    private static void SelectMinMaxCurveStateCallback(object obj)
    {
      ModuleUI.CurveStateCallbackData stateCallbackData = (ModuleUI.CurveStateCallbackData) obj;
      foreach (SerializedMinMaxCurve minMaxCurve in stateCallbackData.minMaxCurves)
        minMaxCurve.state = stateCallbackData.selectedState;
    }

    public static void GUIMMCurveStateList(Rect rect, SerializedMinMaxCurve minMaxCurves)
    {
      SerializedMinMaxCurve[] minMaxCurves1 = new SerializedMinMaxCurve[1]{ minMaxCurves };
      ModuleUI.GUIMMCurveStateList(rect, minMaxCurves1);
    }

    public static void GUIMMCurveStateList(Rect rect, SerializedMinMaxCurve[] minMaxCurves)
    {
      if (!EditorGUI.ButtonMouseDown(rect, GUIContent.none, FocusType.Passive, ParticleSystemStyles.Get().minMaxCurveStateDropDown) || minMaxCurves.Length == 0)
        return;
      GUIContent[] guiContentArray = new GUIContent[4]{ new GUIContent("Constant"), new GUIContent("Curve"), new GUIContent("Random Between Two Constants"), new GUIContent("Random Between Two Curves") };
      MinMaxCurveState[] minMaxCurveStateArray = new MinMaxCurveState[4]{ MinMaxCurveState.k_Scalar, MinMaxCurveState.k_Curve, MinMaxCurveState.k_TwoScalars, MinMaxCurveState.k_TwoCurves };
      bool[] flagArray = new bool[4]{ (minMaxCurves[0].m_AllowConstant ? 1 : 0) != 0, (minMaxCurves[0].m_AllowCurves ? 1 : 0) != 0, (minMaxCurves[0].m_AllowRandom ? 1 : 0) != 0, (!minMaxCurves[0].m_AllowRandom ? 0 : (minMaxCurves[0].m_AllowCurves ? 1 : 0)) != 0 };
      GenericMenu genericMenu = new GenericMenu();
      for (int index = 0; index < guiContentArray.Length; ++index)
      {
        if (flagArray[index])
          genericMenu.AddItem(guiContentArray[index], minMaxCurves[0].state == minMaxCurveStateArray[index], new GenericMenu.MenuFunction2(ModuleUI.SelectMinMaxCurveStateCallback), (object) new ModuleUI.CurveStateCallbackData(minMaxCurveStateArray[index], minMaxCurves));
      }
      genericMenu.DropDown(rect);
      Event.current.Use();
    }

    private static void SelectMinMaxGradientStateCallback(object obj)
    {
      ModuleUI.GradientCallbackData gradientCallbackData = (ModuleUI.GradientCallbackData) obj;
      gradientCallbackData.gradientProp.state = gradientCallbackData.selectedState;
    }

    public static void GUIMMGradientPopUp(Rect rect, SerializedMinMaxGradient gradientProp)
    {
      if (!EditorGUI.ButtonMouseDown(rect, GUIContent.none, FocusType.Passive, ParticleSystemStyles.Get().minMaxCurveStateDropDown))
        return;
      GUIContent[] guiContentArray = new GUIContent[4]{ new GUIContent("Color"), new GUIContent("Gradient"), new GUIContent("Random Between Two Colors"), new GUIContent("Random Between Two Gradients") };
      MinMaxGradientState[] maxGradientStateArray = new MinMaxGradientState[4]{ MinMaxGradientState.k_Color, MinMaxGradientState.k_Gradient, MinMaxGradientState.k_RandomBetweenTwoColors, MinMaxGradientState.k_RandomBetweenTwoGradients };
      bool[] flagArray = new bool[4]{ (gradientProp.m_AllowColor ? 1 : 0) != 0, (gradientProp.m_AllowGradient ? 1 : 0) != 0, (gradientProp.m_AllowRandomBetweenTwoColors ? 1 : 0) != 0, (gradientProp.m_AllowRandomBetweenTwoGradients ? 1 : 0) != 0 };
      GenericMenu genericMenu = new GenericMenu();
      for (int index = 0; index < guiContentArray.Length; ++index)
      {
        if (flagArray[index])
          genericMenu.AddItem(guiContentArray[index], gradientProp.state == maxGradientStateArray[index], new GenericMenu.MenuFunction2(ModuleUI.SelectMinMaxGradientStateCallback), (object) new ModuleUI.GradientCallbackData(maxGradientStateArray[index], gradientProp));
      }
      genericMenu.ShowAsContext();
      Event.current.Use();
    }

    private static void SelectMinMaxColorStateCallback(object obj)
    {
      ModuleUI.ColorCallbackData colorCallbackData = (ModuleUI.ColorCallbackData) obj;
      colorCallbackData.boolProp.boolValue = colorCallbackData.selectedState;
    }

    public static void GUIMMColorPopUp(Rect rect, SerializedProperty boolProp)
    {
      if (!EditorGUI.ButtonMouseDown(rect, GUIContent.none, FocusType.Passive, ParticleSystemStyles.Get().minMaxCurveStateDropDown))
        return;
      GenericMenu genericMenu = new GenericMenu();
      GUIContent[] guiContentArray = new GUIContent[2]{ new GUIContent("Constant Color"), new GUIContent("Random Between Two Colors") };
      bool[] flagArray = new bool[2]{ false, true };
      for (int index = 0; index < guiContentArray.Length; ++index)
        genericMenu.AddItem(guiContentArray[index], boolProp.boolValue == flagArray[index], new GenericMenu.MenuFunction2(ModuleUI.SelectMinMaxColorStateCallback), (object) new ModuleUI.ColorCallbackData(flagArray[index], boolProp));
      genericMenu.ShowAsContext();
      Event.current.Use();
    }

    public enum VisibilityState
    {
      NotVisible,
      VisibleAndFolded,
      VisibleAndFoldedOut,
    }

    private class CurveStateCallbackData
    {
      public SerializedMinMaxCurve[] minMaxCurves;
      public MinMaxCurveState selectedState;

      public CurveStateCallbackData(MinMaxCurveState state, SerializedMinMaxCurve[] curves)
      {
        this.minMaxCurves = curves;
        this.selectedState = state;
      }
    }

    private class GradientCallbackData
    {
      public SerializedMinMaxGradient gradientProp;
      public MinMaxGradientState selectedState;

      public GradientCallbackData(MinMaxGradientState state, SerializedMinMaxGradient p)
      {
        this.gradientProp = p;
        this.selectedState = state;
      }
    }

    private class ColorCallbackData
    {
      public SerializedProperty boolProp;
      public bool selectedState;

      public ColorCallbackData(bool state, SerializedProperty bp)
      {
        this.boolProp = bp;
        this.selectedState = state;
      }
    }

    public delegate bool CurveFieldMouseDownCallback(int button, Rect drawRect, Rect curveRanges);
  }
}

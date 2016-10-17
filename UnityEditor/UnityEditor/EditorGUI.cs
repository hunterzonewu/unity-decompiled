// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>These work pretty much like the normal GUI functions - and also have matching implementations in EditorGUILayout.</para>
  /// </summary>
  public sealed class EditorGUI
  {
    internal static EditorGUI.DelayedTextEditor s_DelayedTextEditor = new EditorGUI.DelayedTextEditor();
    internal static EditorGUI.RecycledTextEditor s_RecycledEditor = new EditorGUI.RecycledTextEditor();
    internal static string s_OriginalText = string.Empty;
    private static bool bKeyEventActive = false;
    internal static bool s_DragToPosition = true;
    internal static bool s_Dragged = false;
    internal static bool s_PostPoneMove = false;
    internal static bool s_SelectAllOnMouseUp = true;
    private static int s_DragUpdatedOverID = 0;
    private static int s_FoldoutHash = "Foldout".GetHashCode();
    private static int s_TagFieldHash = "s_TagFieldHash".GetHashCode();
    private static int s_PPtrHash = "s_PPtrHash".GetHashCode();
    private static int s_ObjectFieldHash = "s_ObjectFieldHash".GetHashCode();
    private static int s_ToggleHash = "s_ToggleHash".GetHashCode();
    private static int s_ColorHash = "s_ColorHash".GetHashCode();
    private static int s_CurveHash = "s_CurveHash".GetHashCode();
    private static int s_LayerMaskField = "s_LayerMaskField".GetHashCode();
    private static int s_MaskField = "s_MaskField".GetHashCode();
    private static int s_GenericField = "s_GenericField".GetHashCode();
    private static int s_PopupHash = "EditorPopup".GetHashCode();
    private static int s_KeyEventFieldHash = "KeyEventField".GetHashCode();
    private static int s_TextFieldHash = "EditorTextField".GetHashCode();
    private static int s_SearchFieldHash = "EditorSearchField".GetHashCode();
    private static int s_TextAreaHash = "EditorTextField".GetHashCode();
    private static int s_PasswordFieldHash = "PasswordField".GetHashCode();
    private static int s_FloatFieldHash = "EditorTextField".GetHashCode();
    private static int s_DelayedTextFieldHash = "DelayedEditorTextField".GetHashCode();
    private static int s_ArraySizeFieldHash = "ArraySizeField".GetHashCode();
    private static int s_SliderHash = "EditorSlider".GetHashCode();
    private static int s_SliderKnobHash = "EditorSliderKnob".GetHashCode();
    private static int s_MinMaxSliderHash = "EditorMinMaxSlider".GetHashCode();
    private static int s_TitlebarHash = "GenericTitlebar".GetHashCode();
    private static int s_ProgressBarHash = "s_ProgressBarHash".GetHashCode();
    private static int s_SelectableLabelHash = "s_SelectableLabel".GetHashCode();
    private static int s_SortingLayerFieldHash = "s_SortingLayerFieldHash".GetHashCode();
    private static int s_TextFieldDropDownHash = "s_TextFieldDropDown".GetHashCode();
    private static int s_DragCandidateState = 0;
    private static float kDragDeadzone = 16f;
    private static double s_DragStartValue = 0.0;
    private static long s_DragStartIntValue = 0;
    private static double s_DragSensitivity = 0.0;
    internal static string kFloatFieldFormatString = "g7";
    internal static string kDoubleFieldFormatString = "g15";
    internal static string kIntFieldFormatString = "#######0";
    internal static int ms_IndentLevel = 0;
    internal static string s_UnitString = string.Empty;
    private static float[] s_Vector2Floats = new float[2];
    private static GUIContent[] s_XYLabels = new GUIContent[2]
    {
      EditorGUIUtility.TextContent("X"),
      EditorGUIUtility.TextContent("Y")
    };
    private static float[] s_Vector3Floats = new float[3];
    private static GUIContent[] s_XYZLabels = new GUIContent[3]
    {
      EditorGUIUtility.TextContent("X"),
      EditorGUIUtility.TextContent("Y"),
      EditorGUIUtility.TextContent("Z")
    };
    private static float[] s_Vector4Floats = new float[4];
    private static GUIContent[] s_XYZWLabels = new GUIContent[4]
    {
      EditorGUIUtility.TextContent("X"),
      EditorGUIUtility.TextContent("Y"),
      EditorGUIUtility.TextContent("Z"),
      EditorGUIUtility.TextContent("W")
    };
    private static GUIContent[] s_WHLabels = new GUIContent[2]
    {
      EditorGUIUtility.TextContent("W"),
      EditorGUIUtility.TextContent("H")
    };
    internal static readonly GUIContent s_ClipingPlanesLabel = EditorGUIUtility.TextContent("Clipping Planes");
    internal static readonly GUIContent[] s_NearAndFarLabels = new GUIContent[2]
    {
      EditorGUIUtility.TextContent("Near"),
      EditorGUIUtility.TextContent("Far")
    };
    internal static Color kCurveColor = Color.green;
    internal static Color kCurveBGColor = new Color(0.337f, 0.337f, 0.337f, 1f);
    private static GUIContent s_PropertyFieldTempContent = new GUIContent();
    internal static GUIContent s_PrefixLabel = new GUIContent((string) null);
    private static GUIContent s_MixedValueContent = EditorGUIUtility.TextContent("—|Mixed Values");
    private static Color s_MixedValueContentColor = new Color(1f, 1f, 1f, 0.5f);
    private static Color s_MixedValueContentColorTemp = Color.white;
    private static Stack<PropertyGUIData> s_PropertyStack = new Stack<PropertyGUIData>();
    private static Stack<bool> s_EnabledStack = new Stack<bool>();
    private static Stack<bool> s_ChangedStack = new Stack<bool>();
    internal static readonly string s_AllowedCharactersForFloat = "inftynaeINFTYNAE0123456789.,-*/+%^()";
    internal static readonly string s_AllowedCharactersForInt = "0123456789-*/+%^()";
    private static SerializedProperty s_PendingPropertyKeyboardHandling = (SerializedProperty) null;
    private static SerializedProperty s_PendingPropertyDelete = (SerializedProperty) null;
    private static readonly int s_GradientHash = "s_GradientHash".GetHashCode();
    private static readonly GUIContent s_HDRWarning = new GUIContent(string.Empty, (Texture) EditorGUIUtility.warningIcon, LocalizationDatabase.GetLocalizedString("For HDR colors the normalized LDR hex color value is shown"));
    private static int s_ButtonMouseDownHash = "ButtonMouseDown".GetHashCode();
    private static int s_MouseDeltaReaderHash = "MouseDeltaReader".GetHashCode();
    private const double kFoldoutExpandTimeout = 0.7;
    private const float kDragSensitivity = 0.03f;
    internal const float kMiniLabelW = 13f;
    internal const float kLabelW = 80f;
    internal const float kSpacing = 5f;
    internal const float kSpacingSubLabel = 2f;
    internal const float kSliderMinW = 60f;
    internal const float kSliderMaxW = 100f;
    internal const float kSingleLineHeight = 16f;
    internal const float kStructHeaderLineHeight = 16f;
    internal const float kObjectFieldThumbnailHeight = 64f;
    internal const float kObjectFieldMiniThumbnailHeight = 18f;
    internal const float kObjectFieldMiniThumbnailWidth = 32f;
    private const float kIndentPerLevel = 15f;
    internal const int kControlVerticalSpacing = 2;
    internal const int kInspTitlebarIconWidth = 16;
    internal const float kNearFarLabelsWidth = 35f;
    private const int kInspTitlebarToggleWidth = 16;
    private const int kInspTitlebarSpacing = 2;
    private const string kEmptyDropDownElement = "--empty--";
    private static EditorGUI.RecycledTextEditor activeEditor;
    internal static string s_RecycledCurrentEditingString;
    internal static double s_RecycledCurrentEditingFloat;
    internal static long s_RecycledCurrentEditingInt;
    private static double s_FoldoutDestTime;
    private static bool s_WasBoldDefaultFont;
    private static Vector2 s_DragStartPos;
    private static int s_ColorPickID;
    private static int s_CurveID;
    private static GUIContent s_IconDropDown;
    private static Material s_IconTextureInactive;
    internal static Rect s_PrefixTotalRect;
    internal static Rect s_PrefixRect;
    internal static GUIStyle s_PrefixStyle;
    private static bool s_ShowMixedValue;
    internal static bool s_CollectingToolTips;
    private static int s_GradientID;
    private static Vector2 s_MouseDeltaReaderLastPos;

    /// <summary>
    ///   <para>Makes the following controls give the appearance of editing multiple different values.</para>
    /// </summary>
    public static bool showMixedValue
    {
      get
      {
        return EditorGUI.s_ShowMixedValue;
      }
      set
      {
        EditorGUI.s_ShowMixedValue = value;
      }
    }

    internal static GUIContent mixedValueContent
    {
      get
      {
        return EditorGUI.s_MixedValueContent;
      }
    }

    /// <summary>
    ///   <para>Is the platform-dependent "action" modifier key held down? (Read Only)</para>
    /// </summary>
    public static bool actionKey
    {
      get
      {
        if (Event.current == null)
          return false;
        if (Application.platform == RuntimePlatform.OSXEditor)
          return Event.current.command;
        return Event.current.control;
      }
    }

    /// <summary>
    ///   <para>The indent level of the field labels.</para>
    /// </summary>
    public static int indentLevel
    {
      get
      {
        return EditorGUI.ms_IndentLevel;
      }
      set
      {
        EditorGUI.ms_IndentLevel = value;
      }
    }

    internal static float indent
    {
      get
      {
        return (float) EditorGUI.indentLevel * 15f;
      }
    }

    internal static Material alphaMaterial
    {
      get
      {
        return EditorGUIUtility.LoadRequired("Previews/PreviewAlphaMaterial.mat") as Material;
      }
    }

    internal static Material transparentMaterial
    {
      get
      {
        return EditorGUIUtility.LoadRequired("Previews/PreviewTransparentMaterial.mat") as Material;
      }
    }

    internal static Texture2D transparentCheckerTexture
    {
      get
      {
        if (EditorGUIUtility.isProSkin)
          return EditorGUIUtility.LoadRequired("Previews/Textures/textureCheckerDark.png") as Texture2D;
        return EditorGUIUtility.LoadRequired("Previews/Textures/textureChecker.png") as Texture2D;
      }
    }

    internal static Material lightmapRGBMMaterial
    {
      get
      {
        return EditorGUIUtility.LoadRequired("Previews/PreviewEncodedLightmapRGBMMaterial.mat") as Material;
      }
    }

    internal static Material lightmapDoubleLDRMaterial
    {
      get
      {
        return EditorGUIUtility.LoadRequired("Previews/PreviewEncodedLightmapDoubleLDRMaterial.mat") as Material;
      }
    }

    internal static Material normalmapMaterial
    {
      get
      {
        return EditorGUIUtility.LoadRequired("Previews/PreviewEncodedNormalsMaterial.mat") as Material;
      }
    }

    internal static bool isCollectingTooltips
    {
      get
      {
        return EditorGUI.s_CollectingToolTips;
      }
      set
      {
        EditorGUI.s_CollectingToolTips = value;
      }
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the label field.</param>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="style">Style information (color, etc) for displaying the label.</param>
    [ExcludeFromDocs]
    public static void LabelField(Rect position, string label)
    {
      GUIStyle label1 = EditorStyles.label;
      EditorGUI.LabelField(position, label, label1);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the label field.</param>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="style">Style information (color, etc) for displaying the label.</param>
    public static void LabelField(Rect position, string label, [DefaultValue("EditorStyles.label")] GUIStyle style)
    {
      EditorGUI.LabelField(position, GUIContent.none, EditorGUIUtility.TempContent(label), style);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the label field.</param>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="style">Style information (color, etc) for displaying the label.</param>
    [ExcludeFromDocs]
    public static void LabelField(Rect position, GUIContent label)
    {
      GUIStyle label1 = EditorStyles.label;
      EditorGUI.LabelField(position, label, label1);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the label field.</param>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="style">Style information (color, etc) for displaying the label.</param>
    public static void LabelField(Rect position, GUIContent label, [DefaultValue("EditorStyles.label")] GUIStyle style)
    {
      EditorGUI.LabelField(position, GUIContent.none, label, style);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the label field.</param>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="style">Style information (color, etc) for displaying the label.</param>
    [ExcludeFromDocs]
    public static void LabelField(Rect position, string label, string label2)
    {
      GUIStyle label1 = EditorStyles.label;
      EditorGUI.LabelField(position, label, label2, label1);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the label field.</param>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="style">Style information (color, etc) for displaying the label.</param>
    public static void LabelField(Rect position, string label, string label2, [DefaultValue("EditorStyles.label")] GUIStyle style)
    {
      EditorGUI.LabelField(position, new GUIContent(label), EditorGUIUtility.TempContent(label2), style);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the label field.</param>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="style">Style information (color, etc) for displaying the label.</param>
    [ExcludeFromDocs]
    public static void LabelField(Rect position, GUIContent label, GUIContent label2)
    {
      GUIStyle label1 = EditorStyles.label;
      EditorGUI.LabelField(position, label, label2, label1);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the label field.</param>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="style">Style information (color, etc) for displaying the label.</param>
    public static void LabelField(Rect position, GUIContent label, GUIContent label2, [DefaultValue("EditorStyles.label")] GUIStyle style)
    {
      EditorGUI.LabelFieldInternal(position, label, label2, style);
    }

    /// <summary>
    ///   <para>Make a toggle field where the toggle is to the left and the label immediately to the right of it.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the toggle.</param>
    /// <param name="label">Label to display next to the toggle.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="labelStyle">Optional GUIStyle to use for the label.</param>
    /// <returns>
    ///   <para>The value set by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static bool ToggleLeft(Rect position, string label, bool value)
    {
      GUIStyle label1 = EditorStyles.label;
      return EditorGUI.ToggleLeft(position, label, value, label1);
    }

    /// <summary>
    ///   <para>Make a toggle field where the toggle is to the left and the label immediately to the right of it.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the toggle.</param>
    /// <param name="label">Label to display next to the toggle.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="labelStyle">Optional GUIStyle to use for the label.</param>
    /// <returns>
    ///   <para>The value set by the user.</para>
    /// </returns>
    public static bool ToggleLeft(Rect position, string label, bool value, [DefaultValue("EditorStyles.label")] GUIStyle labelStyle)
    {
      return EditorGUI.ToggleLeft(position, EditorGUIUtility.TempContent(label), value, labelStyle);
    }

    /// <summary>
    ///   <para>Make a toggle field where the toggle is to the left and the label immediately to the right of it.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the toggle.</param>
    /// <param name="label">Label to display next to the toggle.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="labelStyle">Optional GUIStyle to use for the label.</param>
    /// <returns>
    ///   <para>The value set by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static bool ToggleLeft(Rect position, GUIContent label, bool value)
    {
      GUIStyle label1 = EditorStyles.label;
      return EditorGUI.ToggleLeft(position, label, value, label1);
    }

    /// <summary>
    ///   <para>Make a toggle field where the toggle is to the left and the label immediately to the right of it.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the toggle.</param>
    /// <param name="label">Label to display next to the toggle.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="labelStyle">Optional GUIStyle to use for the label.</param>
    /// <returns>
    ///   <para>The value set by the user.</para>
    /// </returns>
    public static bool ToggleLeft(Rect position, GUIContent label, bool value, [DefaultValue("EditorStyles.label")] GUIStyle labelStyle)
    {
      return EditorGUI.ToggleLeftInternal(position, label, value, labelStyle);
    }

    /// <summary>
    ///   <para>Make a text field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the text field.</param>
    /// <param name="label">Optional label to display in front of the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static string TextField(Rect position, string text)
    {
      GUIStyle textField = EditorStyles.textField;
      return EditorGUI.TextField(position, text, textField);
    }

    /// <summary>
    ///   <para>Make a text field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the text field.</param>
    /// <param name="label">Optional label to display in front of the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    public static string TextField(Rect position, string text, [DefaultValue("EditorStyles.textField")] GUIStyle style)
    {
      return EditorGUI.TextFieldInternal(position, text, style);
    }

    /// <summary>
    ///   <para>Make a text field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the text field.</param>
    /// <param name="label">Optional label to display in front of the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static string TextField(Rect position, string label, string text)
    {
      GUIStyle textField = EditorStyles.textField;
      return EditorGUI.TextField(position, label, text, textField);
    }

    /// <summary>
    ///   <para>Make a text field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the text field.</param>
    /// <param name="label">Optional label to display in front of the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    public static string TextField(Rect position, string label, string text, [DefaultValue("EditorStyles.textField")] GUIStyle style)
    {
      return EditorGUI.TextField(position, EditorGUIUtility.TempContent(label), text, style);
    }

    /// <summary>
    ///   <para>Make a text field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the text field.</param>
    /// <param name="label">Optional label to display in front of the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static string TextField(Rect position, GUIContent label, string text)
    {
      GUIStyle textField = EditorStyles.textField;
      return EditorGUI.TextField(position, label, text, textField);
    }

    /// <summary>
    ///   <para>Make a text field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the text field.</param>
    /// <param name="label">Optional label to display in front of the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    public static string TextField(Rect position, GUIContent label, string text, [DefaultValue("EditorStyles.textField")] GUIStyle style)
    {
      return EditorGUI.TextFieldInternal(position, label, text, style);
    }

    [ExcludeFromDocs]
    public static string DelayedTextField(Rect position, string text)
    {
      GUIStyle textField = EditorStyles.textField;
      return EditorGUI.DelayedTextField(position, text, textField);
    }

    /// <summary>
    ///   <para>Make a delayed text field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the text field.</param>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="text">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the text field.</para>
    /// </returns>
    public static string DelayedTextField(Rect position, string text, [DefaultValue("EditorStyles.textField")] GUIStyle style)
    {
      return EditorGUI.DelayedTextField(position, GUIContent.none, text, style);
    }

    [ExcludeFromDocs]
    public static string DelayedTextField(Rect position, string label, string text)
    {
      GUIStyle textField = EditorStyles.textField;
      return EditorGUI.DelayedTextField(position, label, text, textField);
    }

    /// <summary>
    ///   <para>Make a delayed text field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the text field.</param>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="text">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the text field.</para>
    /// </returns>
    public static string DelayedTextField(Rect position, string label, string text, [DefaultValue("EditorStyles.textField")] GUIStyle style)
    {
      return EditorGUI.DelayedTextField(position, EditorGUIUtility.TempContent(label), text, style);
    }

    [ExcludeFromDocs]
    public static string DelayedTextField(Rect position, GUIContent label, string text)
    {
      GUIStyle textField = EditorStyles.textField;
      return EditorGUI.DelayedTextField(position, label, text, textField);
    }

    /// <summary>
    ///   <para>Make a delayed text field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the text field.</param>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="text">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the text field.</para>
    /// </returns>
    public static string DelayedTextField(Rect position, GUIContent label, string text, [DefaultValue("EditorStyles.textField")] GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TextFieldHash, FocusType.Keyboard, position);
      return EditorGUI.DelayedTextFieldInternal(position, controlId, label, text, (string) null, style);
    }

    [ExcludeFromDocs]
    public static void DelayedTextField(Rect position, SerializedProperty property)
    {
      GUIContent label = (GUIContent) null;
      EditorGUI.DelayedTextField(position, property, label);
    }

    /// <summary>
    ///   <para>Make a delayed text field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the text field.</param>
    /// <param name="property">The text property to edit.</param>
    /// <param name="label">Optional label to display in front of the int field. Pass GUIContent.none to hide label.</param>
    public static void DelayedTextField(Rect position, SerializedProperty property, [DefaultValue("null")] GUIContent label)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TextFieldHash, FocusType.Keyboard, position);
      EditorGUI.DelayedTextFieldInternal(position, controlId, property, (string) null, label);
    }

    /// <summary>
    ///   <para>Make a text area.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static string TextArea(Rect position, string text)
    {
      GUIStyle textField = EditorStyles.textField;
      return EditorGUI.TextArea(position, text, textField);
    }

    /// <summary>
    ///   <para>Make a text area.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    public static string TextArea(Rect position, string text, [DefaultValue("EditorStyles.textField")] GUIStyle style)
    {
      return EditorGUI.TextAreaInternal(position, text, style);
    }

    /// <summary>
    ///   <para>Make a selectable label field. (Useful for showing read-only info that can be copy-pasted.)</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the label.</param>
    /// <param name="text">The text to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    [ExcludeFromDocs]
    public static void SelectableLabel(Rect position, string text)
    {
      GUIStyle label = EditorStyles.label;
      EditorGUI.SelectableLabel(position, text, label);
    }

    /// <summary>
    ///   <para>Make a selectable label field. (Useful for showing read-only info that can be copy-pasted.)</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the label.</param>
    /// <param name="text">The text to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    public static void SelectableLabel(Rect position, string text, [DefaultValue("EditorStyles.label")] GUIStyle style)
    {
      EditorGUI.SelectableLabelInternal(position, text, style);
    }

    /// <summary>
    ///   <para>Make a text field where the user can enter a password.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the password field.</param>
    /// <param name="label">Optional label to display in front of the password field.</param>
    /// <param name="password">The password to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The password entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static string PasswordField(Rect position, string password)
    {
      GUIStyle textField = EditorStyles.textField;
      return EditorGUI.PasswordField(position, password, textField);
    }

    /// <summary>
    ///   <para>Make a text field where the user can enter a password.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the password field.</param>
    /// <param name="label">Optional label to display in front of the password field.</param>
    /// <param name="password">The password to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The password entered by the user.</para>
    /// </returns>
    public static string PasswordField(Rect position, string password, [DefaultValue("EditorStyles.textField")] GUIStyle style)
    {
      return EditorGUI.PasswordFieldInternal(position, password, style);
    }

    /// <summary>
    ///   <para>Make a text field where the user can enter a password.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the password field.</param>
    /// <param name="label">Optional label to display in front of the password field.</param>
    /// <param name="password">The password to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The password entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static string PasswordField(Rect position, string label, string password)
    {
      GUIStyle textField = EditorStyles.textField;
      return EditorGUI.PasswordField(position, label, password, textField);
    }

    /// <summary>
    ///   <para>Make a text field where the user can enter a password.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the password field.</param>
    /// <param name="label">Optional label to display in front of the password field.</param>
    /// <param name="password">The password to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The password entered by the user.</para>
    /// </returns>
    public static string PasswordField(Rect position, string label, string password, [DefaultValue("EditorStyles.textField")] GUIStyle style)
    {
      return EditorGUI.PasswordField(position, EditorGUIUtility.TempContent(label), password, style);
    }

    /// <summary>
    ///   <para>Make a text field where the user can enter a password.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the password field.</param>
    /// <param name="label">Optional label to display in front of the password field.</param>
    /// <param name="password">The password to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The password entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static string PasswordField(Rect position, GUIContent label, string password)
    {
      GUIStyle textField = EditorStyles.textField;
      return EditorGUI.PasswordField(position, label, password, textField);
    }

    /// <summary>
    ///   <para>Make a text field where the user can enter a password.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the password field.</param>
    /// <param name="label">Optional label to display in front of the password field.</param>
    /// <param name="password">The password to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The password entered by the user.</para>
    /// </returns>
    public static string PasswordField(Rect position, GUIContent label, string password, [DefaultValue("EditorStyles.textField")] GUIStyle style)
    {
      return EditorGUI.PasswordFieldInternal(position, label, password, style);
    }

    [ExcludeFromDocs]
    public static float FloatField(Rect position, float value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.FloatField(position, value, numberField);
    }

    /// <summary>
    ///   <para>Make a text field for entering floats.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the float field.</param>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static float FloatField(Rect position, float value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.FloatFieldInternal(position, value, style);
    }

    [ExcludeFromDocs]
    public static float FloatField(Rect position, string label, float value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.FloatField(position, label, value, numberField);
    }

    /// <summary>
    ///   <para>Make a text field for entering floats.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the float field.</param>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static float FloatField(Rect position, string label, float value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.FloatField(position, EditorGUIUtility.TempContent(label), value, style);
    }

    [ExcludeFromDocs]
    public static float FloatField(Rect position, GUIContent label, float value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.FloatField(position, label, value, numberField);
    }

    /// <summary>
    ///   <para>Make a text field for entering floats.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the float field.</param>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static float FloatField(Rect position, GUIContent label, float value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.FloatFieldInternal(position, label, value, style);
    }

    [ExcludeFromDocs]
    public static float DelayedFloatField(Rect position, float value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.DelayedFloatField(position, value, numberField);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering floats.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the float field.</param>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the float field.</para>
    /// </returns>
    public static float DelayedFloatField(Rect position, float value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.DelayedFloatField(position, GUIContent.none, value, style);
    }

    [ExcludeFromDocs]
    public static float DelayedFloatField(Rect position, string label, float value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.DelayedFloatField(position, label, value, numberField);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering floats.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the float field.</param>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the float field.</para>
    /// </returns>
    public static float DelayedFloatField(Rect position, string label, float value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.DelayedFloatField(position, EditorGUIUtility.TempContent(label), value, style);
    }

    [ExcludeFromDocs]
    public static float DelayedFloatField(Rect position, GUIContent label, float value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.DelayedFloatField(position, label, value, numberField);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering floats.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the float field.</param>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the float field.</para>
    /// </returns>
    public static float DelayedFloatField(Rect position, GUIContent label, float value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.DelayedFloatFieldInternal(position, label, value, style);
    }

    [ExcludeFromDocs]
    public static void DelayedFloatField(Rect position, SerializedProperty property)
    {
      GUIContent label = (GUIContent) null;
      EditorGUI.DelayedFloatField(position, property, label);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering floats.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the float field.</param>
    /// <param name="property">The float property to edit.</param>
    /// <param name="label">Optional label to display in front of the float field. Pass GUIContent.none to hide label.</param>
    public static void DelayedFloatField(Rect position, SerializedProperty property, [DefaultValue("null")] GUIContent label)
    {
      EditorGUI.DelayedFloatFieldInternal(position, property, label);
    }

    [ExcludeFromDocs]
    public static double DoubleField(Rect position, double value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.DoubleField(position, value, numberField);
    }

    /// <summary>
    ///   <para>Make a text field for entering doubles.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the double field.</param>
    /// <param name="label">Optional label to display in front of the double field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static double DoubleField(Rect position, double value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.DoubleFieldInternal(position, value, style);
    }

    [ExcludeFromDocs]
    public static double DoubleField(Rect position, string label, double value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.DoubleField(position, label, value, numberField);
    }

    /// <summary>
    ///   <para>Make a text field for entering doubles.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the double field.</param>
    /// <param name="label">Optional label to display in front of the double field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static double DoubleField(Rect position, string label, double value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.DoubleField(position, EditorGUIUtility.TempContent(label), value, style);
    }

    [ExcludeFromDocs]
    public static double DoubleField(Rect position, GUIContent label, double value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.DoubleField(position, label, value, numberField);
    }

    /// <summary>
    ///   <para>Make a text field for entering doubles.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the double field.</param>
    /// <param name="label">Optional label to display in front of the double field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static double DoubleField(Rect position, GUIContent label, double value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.DoubleFieldInternal(position, label, value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the int field.</param>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int IntField(Rect position, int value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.IntField(position, value, numberField);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the int field.</param>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static int IntField(Rect position, int value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.IntFieldInternal(position, value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the int field.</param>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int IntField(Rect position, string label, int value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.IntField(position, label, value, numberField);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the int field.</param>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static int IntField(Rect position, string label, int value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.IntField(position, EditorGUIUtility.TempContent(label), value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the int field.</param>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int IntField(Rect position, GUIContent label, int value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.IntField(position, label, value, numberField);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the int field.</param>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static int IntField(Rect position, GUIContent label, int value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.IntFieldInternal(position, label, value, style);
    }

    [ExcludeFromDocs]
    public static int DelayedIntField(Rect position, int value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.DelayedIntField(position, value, numberField);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the int field.</param>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the int field.</para>
    /// </returns>
    public static int DelayedIntField(Rect position, int value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.DelayedIntField(position, GUIContent.none, value, style);
    }

    [ExcludeFromDocs]
    public static int DelayedIntField(Rect position, string label, int value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.DelayedIntField(position, label, value, numberField);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the int field.</param>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the int field.</para>
    /// </returns>
    public static int DelayedIntField(Rect position, string label, int value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.DelayedIntField(position, EditorGUIUtility.TempContent(label), value, style);
    }

    [ExcludeFromDocs]
    public static int DelayedIntField(Rect position, GUIContent label, int value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.DelayedIntField(position, label, value, numberField);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the int field.</param>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the int field.</para>
    /// </returns>
    public static int DelayedIntField(Rect position, GUIContent label, int value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.DelayedIntFieldInternal(position, label, value, style);
    }

    [ExcludeFromDocs]
    public static void DelayedIntField(Rect position, SerializedProperty property)
    {
      GUIContent label = (GUIContent) null;
      EditorGUI.DelayedIntField(position, property, label);
    }

    /// <summary>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the int field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the int field.</param>
    /// <param name="property">The int property to edit.</param>
    /// <param name="label">Optional label to display in front of the int field. Pass GUIContent.none to hide label.</param>
    public static void DelayedIntField(Rect position, SerializedProperty property, [DefaultValue("null")] GUIContent label)
    {
      EditorGUI.DelayedIntFieldInternal(position, property, label);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the long field.</param>
    /// <param name="label">Optional label to display in front of the long field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static long LongField(Rect position, long value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.LongField(position, value, numberField);
    }

    public static long LongField(Rect position, long value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.LongFieldInternal(position, value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the long field.</param>
    /// <param name="label">Optional label to display in front of the long field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static long LongField(Rect position, string label, long value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.LongField(position, label, value, numberField);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the long field.</param>
    /// <param name="label">Optional label to display in front of the long field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static long LongField(Rect position, string label, long value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.LongField(position, EditorGUIUtility.TempContent(label), value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the long field.</param>
    /// <param name="label">Optional label to display in front of the long field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static long LongField(Rect position, GUIContent label, long value)
    {
      GUIStyle numberField = EditorStyles.numberField;
      return EditorGUI.LongField(position, label, value, numberField);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the long field.</param>
    /// <param name="label">Optional label to display in front of the long field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static long LongField(Rect position, GUIContent label, long value, [DefaultValue("EditorStyles.numberField")] GUIStyle style)
    {
      return EditorGUI.LongFieldInternal(position, label, value, style);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int Popup(Rect position, int selectedIndex, string[] displayedOptions)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.Popup(position, selectedIndex, displayedOptions, popup);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    public static int Popup(Rect position, int selectedIndex, string[] displayedOptions, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.DoPopup(EditorGUI.IndentedRect(position), GUIUtility.GetControlID(EditorGUI.s_PopupHash, EditorGUIUtility.native, position), selectedIndex, EditorGUIUtility.TempContent(displayedOptions), style);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int Popup(Rect position, int selectedIndex, GUIContent[] displayedOptions)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.Popup(position, selectedIndex, displayedOptions, popup);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    public static int Popup(Rect position, int selectedIndex, GUIContent[] displayedOptions, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.DoPopup(EditorGUI.IndentedRect(position), GUIUtility.GetControlID(EditorGUI.s_PopupHash, EditorGUIUtility.native, position), selectedIndex, displayedOptions, style);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int Popup(Rect position, string label, int selectedIndex, string[] displayedOptions)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.Popup(position, label, selectedIndex, displayedOptions, popup);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    public static int Popup(Rect position, string label, int selectedIndex, string[] displayedOptions, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.PopupInternal(position, EditorGUIUtility.TempContent(label), selectedIndex, EditorGUIUtility.TempContent(displayedOptions), style);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int Popup(Rect position, GUIContent label, int selectedIndex, GUIContent[] displayedOptions)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.Popup(position, label, selectedIndex, displayedOptions, popup);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    public static int Popup(Rect position, GUIContent label, int selectedIndex, GUIContent[] displayedOptions, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.PopupInternal(position, label, selectedIndex, displayedOptions, style);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum option the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The enum option that has been selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static Enum EnumPopup(Rect position, Enum selected)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.EnumPopup(position, selected, popup);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum option the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The enum option that has been selected by the user.</para>
    /// </returns>
    public static Enum EnumPopup(Rect position, Enum selected, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.EnumPopup(position, GUIContent.none, selected, style);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum option the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The enum option that has been selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static Enum EnumPopup(Rect position, string label, Enum selected)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.EnumPopup(position, label, selected, popup);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum option the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The enum option that has been selected by the user.</para>
    /// </returns>
    public static Enum EnumPopup(Rect position, string label, Enum selected, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.EnumPopup(position, EditorGUIUtility.TempContent(label), selected, style);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum option the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The enum option that has been selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static Enum EnumPopup(Rect position, GUIContent label, Enum selected)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.EnumPopup(position, label, selected, popup);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum option the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The enum option that has been selected by the user.</para>
    /// </returns>
    public static Enum EnumPopup(Rect position, GUIContent label, Enum selected, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.EnumPopupInternal(position, label, selected, style);
    }

    [ExcludeFromDocs]
    internal static Enum EnumMaskPopup(Rect position, GUIContent label, Enum selected, out int changedFlags, out bool changedToValue)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.EnumMaskPopup(position, label, selected, out changedFlags, out changedToValue, popup);
    }

    internal static Enum EnumMaskPopup(Rect position, GUIContent label, Enum selected, out int changedFlags, out bool changedToValue, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.EnumMaskPopupInternal(position, label, selected, out changedFlags, out changedToValue, style);
    }

    [ExcludeFromDocs]
    public static Enum EnumMaskPopup(Rect position, GUIContent label, Enum selected)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.EnumMaskPopup(position, label, selected, popup);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field for a bitmask.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum options the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The enum options that has been selected by the user.</para>
    /// </returns>
    public static Enum EnumMaskPopup(Rect position, GUIContent label, Enum selected, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      int changedFlags;
      bool changedToValue;
      return EditorGUI.EnumMaskPopupInternal(position, label, selected, out changedFlags, out changedToValue, style);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option. If optionValues a direct mapping of selectedValue to displayedOptions is assumed.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int IntPopup(Rect position, int selectedValue, string[] displayedOptions, int[] optionValues)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.IntPopup(position, selectedValue, displayedOptions, optionValues, popup);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option. If optionValues a direct mapping of selectedValue to displayedOptions is assumed.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    public static int IntPopup(Rect position, int selectedValue, string[] displayedOptions, int[] optionValues, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.IntPopup(position, GUIContent.none, selectedValue, EditorGUIUtility.TempContent(displayedOptions), optionValues, style);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option. If optionValues a direct mapping of selectedValue to displayedOptions is assumed.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int IntPopup(Rect position, int selectedValue, GUIContent[] displayedOptions, int[] optionValues)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.IntPopup(position, selectedValue, displayedOptions, optionValues, popup);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option. If optionValues a direct mapping of selectedValue to displayedOptions is assumed.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    public static int IntPopup(Rect position, int selectedValue, GUIContent[] displayedOptions, int[] optionValues, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.IntPopup(position, GUIContent.none, selectedValue, displayedOptions, optionValues, style);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option. If optionValues a direct mapping of selectedValue to displayedOptions is assumed.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int IntPopup(Rect position, GUIContent label, int selectedValue, GUIContent[] displayedOptions, int[] optionValues)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.IntPopup(position, label, selectedValue, displayedOptions, optionValues, popup);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option. If optionValues a direct mapping of selectedValue to displayedOptions is assumed.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    public static int IntPopup(Rect position, GUIContent label, int selectedValue, GUIContent[] displayedOptions, int[] optionValues, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.IntPopupInternal(position, label, selectedValue, displayedOptions, optionValues, style);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="property">The SerializedProperty to use for the control.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option. If optionValues a direct	mapping of selectedValue to displayedOptions is assumed.</param>
    /// <param name="label">Optional label in front of the field.</param>
    [ExcludeFromDocs]
    public static void IntPopup(Rect position, SerializedProperty property, GUIContent[] displayedOptions, int[] optionValues)
    {
      GUIContent label = (GUIContent) null;
      EditorGUI.IntPopup(position, property, displayedOptions, optionValues, label);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="property">The SerializedProperty to use for the control.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option. If optionValues a direct	mapping of selectedValue to displayedOptions is assumed.</param>
    /// <param name="label">Optional label in front of the field.</param>
    public static void IntPopup(Rect position, SerializedProperty property, GUIContent[] displayedOptions, int[] optionValues, [DefaultValue("null")] GUIContent label)
    {
      EditorGUI.IntPopupInternal(position, property, displayedOptions, optionValues, label);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option. If optionValues a direct mapping of selectedValue to displayedOptions is assumed.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int IntPopup(Rect position, string label, int selectedValue, string[] displayedOptions, int[] optionValues)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.IntPopup(position, label, selectedValue, displayedOptions, optionValues, popup);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option. If optionValues a direct mapping of selectedValue to displayedOptions is assumed.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    public static int IntPopup(Rect position, string label, int selectedValue, string[] displayedOptions, int[] optionValues, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.IntPopupInternal(position, EditorGUIUtility.TempContent(label), selectedValue, EditorGUIUtility.TempContent(displayedOptions), optionValues, style);
    }

    /// <summary>
    ///   <para>Make a tag selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="tag">The tag the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The tag selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static string TagField(Rect position, string tag)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.TagField(position, tag, popup);
    }

    /// <summary>
    ///   <para>Make a tag selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="tag">The tag the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The tag selected by the user.</para>
    /// </returns>
    public static string TagField(Rect position, string tag, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.TagFieldInternal(position, EditorGUIUtility.TempContent(string.Empty), tag, style);
    }

    /// <summary>
    ///   <para>Make a tag selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="tag">The tag the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The tag selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static string TagField(Rect position, string label, string tag)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.TagField(position, label, tag, popup);
    }

    /// <summary>
    ///   <para>Make a tag selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="tag">The tag the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The tag selected by the user.</para>
    /// </returns>
    public static string TagField(Rect position, string label, string tag, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.TagFieldInternal(position, EditorGUIUtility.TempContent(label), tag, style);
    }

    /// <summary>
    ///   <para>Make a tag selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="tag">The tag the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The tag selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static string TagField(Rect position, GUIContent label, string tag)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.TagField(position, label, tag, popup);
    }

    /// <summary>
    ///   <para>Make a tag selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="tag">The tag the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The tag selected by the user.</para>
    /// </returns>
    public static string TagField(Rect position, GUIContent label, string tag, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.TagFieldInternal(position, label, tag, style);
    }

    /// <summary>
    ///   <para>Make a layer selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="layer">The layer shown in the field.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The layer selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int LayerField(Rect position, int layer)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.LayerField(position, layer, popup);
    }

    /// <summary>
    ///   <para>Make a layer selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="layer">The layer shown in the field.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The layer selected by the user.</para>
    /// </returns>
    public static int LayerField(Rect position, int layer, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.LayerFieldInternal(position, GUIContent.none, layer, style);
    }

    /// <summary>
    ///   <para>Make a layer selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="layer">The layer shown in the field.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The layer selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int LayerField(Rect position, string label, int layer)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.LayerField(position, label, layer, popup);
    }

    /// <summary>
    ///   <para>Make a layer selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="layer">The layer shown in the field.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The layer selected by the user.</para>
    /// </returns>
    public static int LayerField(Rect position, string label, int layer, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.LayerFieldInternal(position, EditorGUIUtility.TempContent(label), layer, style);
    }

    /// <summary>
    ///   <para>Make a layer selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="layer">The layer shown in the field.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The layer selected by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int LayerField(Rect position, GUIContent label, int layer)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.LayerField(position, label, layer, popup);
    }

    /// <summary>
    ///   <para>Make a layer selection field.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="layer">The layer shown in the field.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The layer selected by the user.</para>
    /// </returns>
    public static int LayerField(Rect position, GUIContent label, int layer, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.LayerFieldInternal(position, label, layer, style);
    }

    /// <summary>
    ///   <para>Make a field for masks.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for this control.</param>
    /// <param name="label">Label for the field.</param>
    /// <param name="mask">The current mask to display.</param>
    /// <param name="displayedOption">A string array containing the labels for each flag.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="displayedOptions">A string array containing the labels for each flag.</param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int MaskField(Rect position, GUIContent label, int mask, string[] displayedOptions)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.MaskField(position, label, mask, displayedOptions, popup);
    }

    /// <summary>
    ///   <para>Make a field for masks.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for this control.</param>
    /// <param name="label">Label for the field.</param>
    /// <param name="mask">The current mask to display.</param>
    /// <param name="displayedOption">A string array containing the labels for each flag.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="displayedOptions">A string array containing the labels for each flag.</param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static int MaskField(Rect position, GUIContent label, int mask, string[] displayedOptions, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.MaskFieldInternal(position, label, mask, displayedOptions, style);
    }

    /// <summary>
    ///   <para>Make a field for masks.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for this control.</param>
    /// <param name="label">Label for the field.</param>
    /// <param name="mask">The current mask to display.</param>
    /// <param name="displayedOption">A string array containing the labels for each flag.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="displayedOptions">A string array containing the labels for each flag.</param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int MaskField(Rect position, string label, int mask, string[] displayedOptions)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.MaskField(position, label, mask, displayedOptions, popup);
    }

    /// <summary>
    ///   <para>Make a field for masks.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for this control.</param>
    /// <param name="label">Label for the field.</param>
    /// <param name="mask">The current mask to display.</param>
    /// <param name="displayedOption">A string array containing the labels for each flag.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="displayedOptions">A string array containing the labels for each flag.</param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static int MaskField(Rect position, string label, int mask, string[] displayedOptions, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.MaskFieldInternal(position, GUIContent.Temp(label), mask, displayedOptions, style);
    }

    /// <summary>
    ///   <para>Make a field for masks.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for this control.</param>
    /// <param name="label">Label for the field.</param>
    /// <param name="mask">The current mask to display.</param>
    /// <param name="displayedOption">A string array containing the labels for each flag.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="displayedOptions">A string array containing the labels for each flag.</param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int MaskField(Rect position, int mask, string[] displayedOptions)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.MaskField(position, mask, displayedOptions, popup);
    }

    /// <summary>
    ///   <para>Make a field for masks.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for this control.</param>
    /// <param name="label">Label for the field.</param>
    /// <param name="mask">The current mask to display.</param>
    /// <param name="displayedOption">A string array containing the labels for each flag.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="displayedOptions">A string array containing the labels for each flag.</param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static int MaskField(Rect position, int mask, string[] displayedOptions, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.MaskFieldInternal(position, mask, displayedOptions, style);
    }

    /// <summary>
    ///   <para>Make a field for enum based masks.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for this control.</param>
    /// <param name="enumValue">Enum to use for the flags.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="label">Caption/label for the control.</param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static Enum EnumMaskField(Rect position, GUIContent label, Enum enumValue)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.EnumMaskField(position, label, enumValue, popup);
    }

    /// <summary>
    ///   <para>Make a field for enum based masks.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for this control.</param>
    /// <param name="enumValue">Enum to use for the flags.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="label">Caption/label for the control.</param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static Enum EnumMaskField(Rect position, GUIContent label, Enum enumValue, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.EnumMaskFieldInternal(position, label, enumValue, style);
    }

    /// <summary>
    ///   <para>Make a field for enum based masks.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for this control.</param>
    /// <param name="enumValue">Enum to use for the flags.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="label">Caption/label for the control.</param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static Enum EnumMaskField(Rect position, string label, Enum enumValue)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.EnumMaskField(position, label, enumValue, popup);
    }

    /// <summary>
    ///   <para>Make a field for enum based masks.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for this control.</param>
    /// <param name="enumValue">Enum to use for the flags.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="label">Caption/label for the control.</param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static Enum EnumMaskField(Rect position, string label, Enum enumValue, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.EnumMaskFieldInternal(position, EditorGUIUtility.TempContent(label), enumValue, style);
    }

    /// <summary>
    ///   <para>Make a field for enum based masks.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for this control.</param>
    /// <param name="enumValue">Enum to use for the flags.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="label">Caption/label for the control.</param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static Enum EnumMaskField(Rect position, Enum enumValue)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUI.EnumMaskField(position, enumValue, popup);
    }

    /// <summary>
    ///   <para>Make a field for enum based masks.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for this control.</param>
    /// <param name="enumValue">Enum to use for the flags.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="label">Caption/label for the control.</param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static Enum EnumMaskField(Rect position, Enum enumValue, [DefaultValue("EditorStyles.popup")] GUIStyle style)
    {
      return EditorGUI.EnumMaskFieldInternal(position, enumValue, style);
    }

    /// <summary>
    ///   <para>Make a label with a foldout arrow to the left of it.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the arrow and label.</param>
    /// <param name="foldout">The shown foldout state.</param>
    /// <param name="content">The label to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="toggleOnLabelClick">Should the label be a clickable part of the control?</param>
    /// <returns>
    ///   <para>The foldout state selected by the user. If true, you should render sub-objects.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static bool Foldout(Rect position, bool foldout, string content)
    {
      GUIStyle foldout1 = EditorStyles.foldout;
      return EditorGUI.Foldout(position, foldout, content, foldout1);
    }

    /// <summary>
    ///   <para>Make a label with a foldout arrow to the left of it.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the arrow and label.</param>
    /// <param name="foldout">The shown foldout state.</param>
    /// <param name="content">The label to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="toggleOnLabelClick">Should the label be a clickable part of the control?</param>
    /// <returns>
    ///   <para>The foldout state selected by the user. If true, you should render sub-objects.</para>
    /// </returns>
    public static bool Foldout(Rect position, bool foldout, string content, [DefaultValue("EditorStyles.foldout")] GUIStyle style)
    {
      return EditorGUI.FoldoutInternal(position, foldout, EditorGUIUtility.TempContent(content), false, style);
    }

    /// <summary>
    ///   <para>Make a label with a foldout arrow to the left of it.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the arrow and label.</param>
    /// <param name="foldout">The shown foldout state.</param>
    /// <param name="content">The label to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="toggleOnLabelClick">Should the label be a clickable part of the control?</param>
    /// <returns>
    ///   <para>The foldout state selected by the user. If true, you should render sub-objects.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static bool Foldout(Rect position, bool foldout, string content, bool toggleOnLabelClick)
    {
      GUIStyle foldout1 = EditorStyles.foldout;
      return EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick, foldout1);
    }

    /// <summary>
    ///   <para>Make a label with a foldout arrow to the left of it.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the arrow and label.</param>
    /// <param name="foldout">The shown foldout state.</param>
    /// <param name="content">The label to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="toggleOnLabelClick">Should the label be a clickable part of the control?</param>
    /// <returns>
    ///   <para>The foldout state selected by the user. If true, you should render sub-objects.</para>
    /// </returns>
    public static bool Foldout(Rect position, bool foldout, string content, bool toggleOnLabelClick, [DefaultValue("EditorStyles.foldout")] GUIStyle style)
    {
      return EditorGUI.FoldoutInternal(position, foldout, EditorGUIUtility.TempContent(content), toggleOnLabelClick, style);
    }

    /// <summary>
    ///   <para>Make a label with a foldout arrow to the left of it.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the arrow and label.</param>
    /// <param name="foldout">The shown foldout state.</param>
    /// <param name="content">The label to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="toggleOnLabelClick">Should the label be a clickable part of the control?</param>
    /// <returns>
    ///   <para>The foldout state selected by the user. If true, you should render sub-objects.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static bool Foldout(Rect position, bool foldout, GUIContent content)
    {
      GUIStyle foldout1 = EditorStyles.foldout;
      return EditorGUI.Foldout(position, foldout, content, foldout1);
    }

    /// <summary>
    ///   <para>Make a label with a foldout arrow to the left of it.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the arrow and label.</param>
    /// <param name="foldout">The shown foldout state.</param>
    /// <param name="content">The label to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="toggleOnLabelClick">Should the label be a clickable part of the control?</param>
    /// <returns>
    ///   <para>The foldout state selected by the user. If true, you should render sub-objects.</para>
    /// </returns>
    public static bool Foldout(Rect position, bool foldout, GUIContent content, [DefaultValue("EditorStyles.foldout")] GUIStyle style)
    {
      return EditorGUI.FoldoutInternal(position, foldout, content, false, style);
    }

    /// <summary>
    ///   <para>Make a label with a foldout arrow to the left of it.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the arrow and label.</param>
    /// <param name="foldout">The shown foldout state.</param>
    /// <param name="content">The label to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="toggleOnLabelClick">Should the label be a clickable part of the control?</param>
    /// <returns>
    ///   <para>The foldout state selected by the user. If true, you should render sub-objects.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static bool Foldout(Rect position, bool foldout, GUIContent content, bool toggleOnLabelClick)
    {
      GUIStyle foldout1 = EditorStyles.foldout;
      return EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick, foldout1);
    }

    /// <summary>
    ///   <para>Make a label with a foldout arrow to the left of it.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the arrow and label.</param>
    /// <param name="foldout">The shown foldout state.</param>
    /// <param name="content">The label to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="toggleOnLabelClick">Should the label be a clickable part of the control?</param>
    /// <returns>
    ///   <para>The foldout state selected by the user. If true, you should render sub-objects.</para>
    /// </returns>
    public static bool Foldout(Rect position, bool foldout, GUIContent content, bool toggleOnLabelClick, [DefaultValue("EditorStyles.foldout")] GUIStyle style)
    {
      return EditorGUI.FoldoutInternal(position, foldout, content, toggleOnLabelClick, style);
    }

    /// <summary>
    ///   <para>Make a label for some control.</para>
    /// </summary>
    /// <param name="totalPosition">Rectangle on the screen to use in total for both the label and the control.</param>
    /// <param name="labelPosition">Rectangle on the screen to use for the label.</param>
    /// <param name="label">Label to show for the control.</param>
    /// <param name="id">The unique ID of the control. If none specified, the ID of the following control is used.</param>
    /// <param name="style">Optional GUIStyle to use for the label.</param>
    [ExcludeFromDocs]
    public static void HandlePrefixLabel(Rect totalPosition, Rect labelPosition, GUIContent label, int id)
    {
      GUIStyle label1 = EditorStyles.label;
      EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, id, label1);
    }

    /// <summary>
    ///   <para>Make a label for some control.</para>
    /// </summary>
    /// <param name="totalPosition">Rectangle on the screen to use in total for both the label and the control.</param>
    /// <param name="labelPosition">Rectangle on the screen to use for the label.</param>
    /// <param name="label">Label to show for the control.</param>
    /// <param name="id">The unique ID of the control. If none specified, the ID of the following control is used.</param>
    /// <param name="style">Optional GUIStyle to use for the label.</param>
    [ExcludeFromDocs]
    public static void HandlePrefixLabel(Rect totalPosition, Rect labelPosition, GUIContent label)
    {
      GUIStyle label1 = EditorStyles.label;
      int id = 0;
      EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, id, label1);
    }

    /// <summary>
    ///   <para>Make a label for some control.</para>
    /// </summary>
    /// <param name="totalPosition">Rectangle on the screen to use in total for both the label and the control.</param>
    /// <param name="labelPosition">Rectangle on the screen to use for the label.</param>
    /// <param name="label">Label to show for the control.</param>
    /// <param name="id">The unique ID of the control. If none specified, the ID of the following control is used.</param>
    /// <param name="style">Optional GUIStyle to use for the label.</param>
    public static void HandlePrefixLabel(Rect totalPosition, Rect labelPosition, GUIContent label, [DefaultValue("0")] int id, [DefaultValue("EditorStyles.label")] GUIStyle style)
    {
      EditorGUI.HandlePrefixLabelInternal(totalPosition, labelPosition, label, id, style);
    }

    /// <summary>
    ///   <para>Draws the alpha channel of a texture within a rectangle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to draw the texture within.</param>
    /// <param name="image">Texture to display.</param>
    /// <param name="scaleMode">How to scale the image when the aspect ratio of it doesn't fit the aspect ratio to be drawn within.</param>
    /// <param name="imageAspect">Aspect ratio to use for the source image. If 0 (the default), the aspect ratio from the image is used.</param>
    [ExcludeFromDocs]
    public static void DrawTextureAlpha(Rect position, Texture image, ScaleMode scaleMode)
    {
      float imageAspect = 0.0f;
      EditorGUI.DrawTextureAlpha(position, image, scaleMode, imageAspect);
    }

    /// <summary>
    ///   <para>Draws the alpha channel of a texture within a rectangle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to draw the texture within.</param>
    /// <param name="image">Texture to display.</param>
    /// <param name="scaleMode">How to scale the image when the aspect ratio of it doesn't fit the aspect ratio to be drawn within.</param>
    /// <param name="imageAspect">Aspect ratio to use for the source image. If 0 (the default), the aspect ratio from the image is used.</param>
    [ExcludeFromDocs]
    public static void DrawTextureAlpha(Rect position, Texture image)
    {
      float imageAspect = 0.0f;
      ScaleMode scaleMode = ScaleMode.StretchToFill;
      EditorGUI.DrawTextureAlpha(position, image, scaleMode, imageAspect);
    }

    /// <summary>
    ///   <para>Draws the alpha channel of a texture within a rectangle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to draw the texture within.</param>
    /// <param name="image">Texture to display.</param>
    /// <param name="scaleMode">How to scale the image when the aspect ratio of it doesn't fit the aspect ratio to be drawn within.</param>
    /// <param name="imageAspect">Aspect ratio to use for the source image. If 0 (the default), the aspect ratio from the image is used.</param>
    public static void DrawTextureAlpha(Rect position, Texture image, [DefaultValue("ScaleMode.StretchToFill")] ScaleMode scaleMode, [DefaultValue("0")] float imageAspect)
    {
      EditorGUI.DrawTextureAlphaInternal(position, image, scaleMode, imageAspect);
    }

    [ExcludeFromDocs]
    public static void DrawTextureTransparent(Rect position, Texture image, ScaleMode scaleMode)
    {
      float imageAspect = 0.0f;
      EditorGUI.DrawTextureTransparent(position, image, scaleMode, imageAspect);
    }

    [ExcludeFromDocs]
    public static void DrawTextureTransparent(Rect position, Texture image)
    {
      float imageAspect = 0.0f;
      ScaleMode scaleMode = ScaleMode.StretchToFill;
      EditorGUI.DrawTextureTransparent(position, image, scaleMode, imageAspect);
    }

    public static void DrawTextureTransparent(Rect position, Texture image, [DefaultValue("ScaleMode.StretchToFill")] ScaleMode scaleMode, [DefaultValue("0")] float imageAspect)
    {
      EditorGUI.DrawTextureTransparentInternal(position, image, scaleMode, imageAspect);
    }

    /// <summary>
    ///   <para>Draws the texture within a rectangle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to draw the texture within.</param>
    /// <param name="image">Texture to display.</param>
    /// <param name="mat">Material to be used when drawing the texture.</param>
    /// <param name="scaleMode">How to scale the image when the aspect ratio of it doesn't fit the aspect ratio to be drawn within.</param>
    /// <param name="imageAspect">Aspect ratio to use for the source image. If 0 (the default), the aspect ratio from the image is used.</param>
    [ExcludeFromDocs]
    public static void DrawPreviewTexture(Rect position, Texture image, Material mat, ScaleMode scaleMode)
    {
      float imageAspect = 0.0f;
      EditorGUI.DrawPreviewTexture(position, image, mat, scaleMode, imageAspect);
    }

    /// <summary>
    ///   <para>Draws the texture within a rectangle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to draw the texture within.</param>
    /// <param name="image">Texture to display.</param>
    /// <param name="mat">Material to be used when drawing the texture.</param>
    /// <param name="scaleMode">How to scale the image when the aspect ratio of it doesn't fit the aspect ratio to be drawn within.</param>
    /// <param name="imageAspect">Aspect ratio to use for the source image. If 0 (the default), the aspect ratio from the image is used.</param>
    [ExcludeFromDocs]
    public static void DrawPreviewTexture(Rect position, Texture image, Material mat)
    {
      float imageAspect = 0.0f;
      ScaleMode scaleMode = ScaleMode.StretchToFill;
      EditorGUI.DrawPreviewTexture(position, image, mat, scaleMode, imageAspect);
    }

    /// <summary>
    ///   <para>Draws the texture within a rectangle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to draw the texture within.</param>
    /// <param name="image">Texture to display.</param>
    /// <param name="mat">Material to be used when drawing the texture.</param>
    /// <param name="scaleMode">How to scale the image when the aspect ratio of it doesn't fit the aspect ratio to be drawn within.</param>
    /// <param name="imageAspect">Aspect ratio to use for the source image. If 0 (the default), the aspect ratio from the image is used.</param>
    [ExcludeFromDocs]
    public static void DrawPreviewTexture(Rect position, Texture image)
    {
      float imageAspect = 0.0f;
      ScaleMode scaleMode = ScaleMode.StretchToFill;
      Material mat = (Material) null;
      EditorGUI.DrawPreviewTexture(position, image, mat, scaleMode, imageAspect);
    }

    /// <summary>
    ///   <para>Draws the texture within a rectangle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to draw the texture within.</param>
    /// <param name="image">Texture to display.</param>
    /// <param name="mat">Material to be used when drawing the texture.</param>
    /// <param name="scaleMode">How to scale the image when the aspect ratio of it doesn't fit the aspect ratio to be drawn within.</param>
    /// <param name="imageAspect">Aspect ratio to use for the source image. If 0 (the default), the aspect ratio from the image is used.</param>
    public static void DrawPreviewTexture(Rect position, Texture image, [DefaultValue("null")] Material mat, [DefaultValue("ScaleMode.StretchToFill")] ScaleMode scaleMode, [DefaultValue("0")] float imageAspect)
    {
      EditorGUI.DrawPreviewTextureInternal(position, image, mat, scaleMode, imageAspect);
    }

    /// <summary>
    ///   <para>Get the height needed for a PropertyField control.</para>
    /// </summary>
    /// <param name="property">Height of the property area.</param>
    /// <param name="label">Descriptive text or image.</param>
    /// <param name="includeChildren">Should the returned height include the height of child properties?</param>
    [ExcludeFromDocs]
    public static float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      bool includeChildren = true;
      return EditorGUI.GetPropertyHeight(property, label, includeChildren);
    }

    /// <summary>
    ///   <para>Get the height needed for a PropertyField control.</para>
    /// </summary>
    /// <param name="property">Height of the property area.</param>
    /// <param name="label">Descriptive text or image.</param>
    /// <param name="includeChildren">Should the returned height include the height of child properties?</param>
    [ExcludeFromDocs]
    public static float GetPropertyHeight(SerializedProperty property)
    {
      bool includeChildren = true;
      GUIContent label = (GUIContent) null;
      return EditorGUI.GetPropertyHeight(property, label, includeChildren);
    }

    /// <summary>
    ///   <para>Get the height needed for a PropertyField control.</para>
    /// </summary>
    /// <param name="property">Height of the property area.</param>
    /// <param name="label">Descriptive text or image.</param>
    /// <param name="includeChildren">Should the returned height include the height of child properties?</param>
    public static float GetPropertyHeight(SerializedProperty property, [DefaultValue("null")] GUIContent label, [DefaultValue("true")] bool includeChildren)
    {
      return EditorGUI.GetPropertyHeightInternal(property, label, includeChildren);
    }

    [ExcludeFromDocs]
    public static bool PropertyField(Rect position, SerializedProperty property)
    {
      bool includeChildren = false;
      return EditorGUI.PropertyField(position, property, includeChildren);
    }

    /// <summary>
    ///   <para>Make a field for SerializedProperty.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the property field.</param>
    /// <param name="property">The SerializedProperty to make a field for.</param>
    /// <param name="label">Optional label to use. If not specified the label of the property itself is used. Use GUIContent.none to not display a label at all.</param>
    /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
    /// <returns>
    ///   <para>True if the property has children and is expanded and includeChildren was set to false; otherwise false.</para>
    /// </returns>
    public static bool PropertyField(Rect position, SerializedProperty property, [DefaultValue("false")] bool includeChildren)
    {
      return EditorGUI.PropertyFieldInternal(position, property, (GUIContent) null, includeChildren);
    }

    [ExcludeFromDocs]
    public static bool PropertyField(Rect position, SerializedProperty property, GUIContent label)
    {
      bool includeChildren = false;
      return EditorGUI.PropertyField(position, property, label, includeChildren);
    }

    /// <summary>
    ///   <para>Make a field for SerializedProperty.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the property field.</param>
    /// <param name="property">The SerializedProperty to make a field for.</param>
    /// <param name="label">Optional label to use. If not specified the label of the property itself is used. Use GUIContent.none to not display a label at all.</param>
    /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
    /// <returns>
    ///   <para>True if the property has children and is expanded and includeChildren was set to false; otherwise false.</para>
    /// </returns>
    public static bool PropertyField(Rect position, SerializedProperty property, GUIContent label, [DefaultValue("false")] bool includeChildren)
    {
      return EditorGUI.PropertyFieldInternal(position, property, label, includeChildren);
    }

    internal static void BeginHandleMixedValueContentColor()
    {
      EditorGUI.s_MixedValueContentColorTemp = GUI.contentColor;
      GUI.contentColor = !EditorGUI.showMixedValue ? GUI.contentColor : GUI.contentColor * EditorGUI.s_MixedValueContentColor;
    }

    internal static void EndHandleMixedValueContentColor()
    {
      GUI.contentColor = EditorGUI.s_MixedValueContentColorTemp;
    }

    internal static bool IsEditingTextField()
    {
      return EditorGUI.RecycledTextEditor.s_ActuallyEditing;
    }

    internal static void EndEditingActiveTextField()
    {
      if (EditorGUI.activeEditor == null)
        return;
      EditorGUI.activeEditor.EndEditing();
    }

    /// <summary>
    ///   <para>Move keyboard focus to a named text field and begin editing of the content.</para>
    /// </summary>
    /// <param name="name">Name set using GUI.SetNextControlName.</param>
    public static void FocusTextInControl(string name)
    {
      GUI.FocusControl(name);
      EditorGUIUtility.editingTextField = true;
    }

    internal static void ClearStacks()
    {
      EditorGUI.s_EnabledStack.Clear();
      EditorGUI.s_ChangedStack.Clear();
      EditorGUI.s_PropertyStack.Clear();
      ScriptAttributeUtility.s_DrawerStack.Clear();
    }

    /// <summary>
    ///   <para>Create a group of controls that can be disabled.</para>
    /// </summary>
    /// <param name="disabled">Boolean specifying if the controls inside the group should be disabled.</param>
    public static void BeginDisabledGroup(bool disabled)
    {
      EditorGUI.s_EnabledStack.Push(GUI.enabled);
      GUI.enabled &= !disabled;
    }

    /// <summary>
    ///   <para>Ends a disabled group started with BeginDisabledGroup.</para>
    /// </summary>
    public static void EndDisabledGroup()
    {
      GUI.enabled = EditorGUI.s_EnabledStack.Pop();
    }

    /// <summary>
    ///   <para>Check if any control was changed inside a block of code.</para>
    /// </summary>
    public static void BeginChangeCheck()
    {
      EditorGUI.s_ChangedStack.Push(GUI.changed);
      GUI.changed = false;
    }

    /// <summary>
    ///   <para>Ends a change check started with BeginChangeCheck ().</para>
    /// </summary>
    /// <returns>
    ///   <para>True if GUI.changed was set to true, otherwise false.</para>
    /// </returns>
    public static bool EndChangeCheck()
    {
      bool changed = GUI.changed;
      GUI.changed |= EditorGUI.s_ChangedStack.Pop();
      return changed;
    }

    private static void ShowTextEditorPopupMenu()
    {
      GenericMenu genericMenu = new GenericMenu();
      if (EditorGUI.s_RecycledEditor.hasSelection && !EditorGUI.s_RecycledEditor.isPasswordField)
      {
        if (EditorGUI.RecycledTextEditor.s_AllowContextCutOrPaste)
          genericMenu.AddItem(EditorGUIUtility.TextContent("Cut"), false, new GenericMenu.MenuFunction(new EditorGUI.PopupMenuEvent("Cut", GUIView.current).SendEvent));
        genericMenu.AddItem(EditorGUIUtility.TextContent("Copy"), false, new GenericMenu.MenuFunction(new EditorGUI.PopupMenuEvent("Copy", GUIView.current).SendEvent));
      }
      else
      {
        if (EditorGUI.RecycledTextEditor.s_AllowContextCutOrPaste)
          genericMenu.AddDisabledItem(EditorGUIUtility.TextContent("Cut"));
        genericMenu.AddDisabledItem(EditorGUIUtility.TextContent("Copy"));
      }
      if (EditorGUI.s_RecycledEditor.CanPaste() && EditorGUI.RecycledTextEditor.s_AllowContextCutOrPaste)
        genericMenu.AddItem(EditorGUIUtility.TextContent("Paste"), false, new GenericMenu.MenuFunction(new EditorGUI.PopupMenuEvent("Paste", GUIView.current).SendEvent));
      genericMenu.ShowAsContext();
    }

    internal static void BeginCollectTooltips()
    {
      EditorGUI.isCollectingTooltips = true;
    }

    internal static void EndCollectTooltips()
    {
      EditorGUI.isCollectingTooltips = false;
    }

    /// <summary>
    ///   <para>Draws a label with a drop shadow.</para>
    /// </summary>
    /// <param name="position">Where to show the label.</param>
    /// <param name="content">Text to show
    /// @style style to use.</param>
    /// <param name="text"></param>
    /// <param name="style"></param>
    public static void DropShadowLabel(Rect position, string text)
    {
      EditorGUI.DoDropShadowLabel(position, EditorGUIUtility.TempContent(text), (GUIStyle) "PreOverlayLabel", 0.6f);
    }

    /// <summary>
    ///   <para>Draws a label with a drop shadow.</para>
    /// </summary>
    /// <param name="position">Where to show the label.</param>
    /// <param name="content">Text to show
    /// @style style to use.</param>
    /// <param name="text"></param>
    /// <param name="style"></param>
    public static void DropShadowLabel(Rect position, GUIContent content)
    {
      EditorGUI.DoDropShadowLabel(position, content, (GUIStyle) "PreOverlayLabel", 0.6f);
    }

    /// <summary>
    ///   <para>Draws a label with a drop shadow.</para>
    /// </summary>
    /// <param name="position">Where to show the label.</param>
    /// <param name="content">Text to show
    /// @style style to use.</param>
    /// <param name="text"></param>
    /// <param name="style"></param>
    public static void DropShadowLabel(Rect position, string text, GUIStyle style)
    {
      EditorGUI.DoDropShadowLabel(position, EditorGUIUtility.TempContent(text), style, 0.6f);
    }

    /// <summary>
    ///   <para>Draws a label with a drop shadow.</para>
    /// </summary>
    /// <param name="position">Where to show the label.</param>
    /// <param name="content">Text to show
    /// @style style to use.</param>
    /// <param name="text"></param>
    /// <param name="style"></param>
    public static void DropShadowLabel(Rect position, GUIContent content, GUIStyle style)
    {
      EditorGUI.DoDropShadowLabel(position, content, style, 0.6f);
    }

    internal static void DoDropShadowLabel(Rect position, GUIContent content, GUIStyle style, float shadowOpa)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      EditorGUI.DrawLabelShadow(position, content, style, shadowOpa);
      style.Draw(position, content, false, false, false, false);
    }

    internal static void DrawLabelShadow(Rect position, GUIContent content, GUIStyle style, float shadowOpa)
    {
      Color color = GUI.color;
      Color contentColor = GUI.contentColor;
      Color backgroundColor = GUI.backgroundColor;
      GUI.contentColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      style.Draw(position, content, false, false, false, false);
      ++position.y;
      GUI.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      GUI.contentColor = contentColor;
      EditorGUI.Draw4(position, content, 1f, GUI.color.a * shadowOpa, style);
      EditorGUI.Draw4(position, content, 2f, (float) ((double) GUI.color.a * (double) shadowOpa * 0.419999986886978), style);
      GUI.color = color;
      GUI.backgroundColor = backgroundColor;
    }

    private static void Draw4(Rect position, GUIContent content, float offset, float alpha, GUIStyle style)
    {
      GUI.color = new Color(0.0f, 0.0f, 0.0f, alpha);
      position.y -= offset;
      style.Draw(position, content, false, false, false, false);
      position.y += offset * 2f;
      style.Draw(position, content, false, false, false, false);
      position.y -= offset;
      position.x -= offset;
      style.Draw(position, content, false, false, false, false);
      position.x += offset * 2f;
      style.Draw(position, content, false, false, false, false);
    }

    internal static string DoTextField(EditorGUI.RecycledTextEditor editor, int id, Rect position, string text, GUIStyle style, string allowedletters, out bool changed, bool reset, bool multiline, bool passwordField)
    {
      Event current = Event.current;
      string str1 = text;
      if (text == null)
        text = string.Empty;
      if (EditorGUI.showMixedValue)
        text = string.Empty;
      if (EditorGUI.HasKeyboardFocus(id) && Event.current.type != EventType.Layout)
      {
        if (editor.IsEditingControl(id))
        {
          editor.position = position;
          editor.style = style;
          editor.controlID = id;
          editor.multiline = multiline;
          editor.isPasswordField = passwordField;
          editor.DetectFocusChange();
        }
        else if (EditorGUIUtility.editingTextField)
        {
          editor.BeginEditing(id, text, position, style, multiline, passwordField);
          if ((double) GUI.skin.settings.cursorColor.a > 0.0)
            editor.SelectAll();
        }
      }
      if (editor.controlID == id && GUIUtility.keyboardControl != id)
        editor.controlID = 0;
      bool flag1 = false;
      string text1 = editor.text;
      EventType typeForControl = current.GetTypeForControl(id);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (position.Contains(current.mousePosition) && current.button == 0)
          {
            if (editor.IsEditingControl(id))
            {
              if (Event.current.clickCount == 2 && GUI.skin.settings.doubleClickSelectsWord)
              {
                editor.MoveCursorToPosition(Event.current.mousePosition);
                editor.SelectCurrentWord();
                editor.MouseDragSelectsWholeWords(true);
                editor.DblClickSnap(TextEditor.DblClickSnapping.WORDS);
                EditorGUI.s_DragToPosition = false;
              }
              else if (Event.current.clickCount == 3 && GUI.skin.settings.tripleClickSelectsLine)
              {
                editor.MoveCursorToPosition(Event.current.mousePosition);
                editor.SelectCurrentParagraph();
                editor.MouseDragSelectsWholeWords(true);
                editor.DblClickSnap(TextEditor.DblClickSnapping.PARAGRAPHS);
                EditorGUI.s_DragToPosition = false;
              }
              else
              {
                editor.MoveCursorToPosition(Event.current.mousePosition);
                EditorGUI.s_SelectAllOnMouseUp = false;
              }
            }
            else
            {
              GUIUtility.keyboardControl = id;
              editor.BeginEditing(id, text, position, style, multiline, passwordField);
              editor.MoveCursorToPosition(Event.current.mousePosition);
              if ((double) GUI.skin.settings.cursorColor.a > 0.0)
                EditorGUI.s_SelectAllOnMouseUp = true;
            }
            GUIUtility.hotControl = id;
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id)
          {
            if (EditorGUI.s_Dragged && EditorGUI.s_DragToPosition)
            {
              editor.MoveSelectionToAltCursor();
              flag1 = true;
            }
            else if (EditorGUI.s_PostPoneMove)
              editor.MoveCursorToPosition(Event.current.mousePosition);
            else if (EditorGUI.s_SelectAllOnMouseUp)
            {
              if ((double) GUI.skin.settings.cursorColor.a > 0.0)
                editor.SelectAll();
              EditorGUI.s_SelectAllOnMouseUp = false;
            }
            editor.MouseDragSelectsWholeWords(false);
            EditorGUI.s_DragToPosition = true;
            EditorGUI.s_Dragged = false;
            EditorGUI.s_PostPoneMove = false;
            if (current.button == 0)
            {
              GUIUtility.hotControl = 0;
              current.Use();
              break;
            }
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == id)
          {
            if (!current.shift && editor.hasSelection && EditorGUI.s_DragToPosition)
            {
              editor.MoveAltCursorToPosition(Event.current.mousePosition);
            }
            else
            {
              if (current.shift)
                editor.MoveCursorToPosition(Event.current.mousePosition);
              else
                editor.SelectToPosition(Event.current.mousePosition);
              EditorGUI.s_DragToPosition = false;
              EditorGUI.s_SelectAllOnMouseUp = !editor.hasSelection;
            }
            EditorGUI.s_Dragged = true;
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.keyboardControl == id)
          {
            char character = current.character;
            if (editor.IsEditingControl(id) && editor.HandleKeyEvent(current))
            {
              current.Use();
              flag1 = true;
              break;
            }
            if (current.keyCode == KeyCode.Escape)
            {
              if (editor.IsEditingControl(id))
              {
                if (style == EditorStyles.toolbarSearchField || style == EditorStyles.searchField)
                  EditorGUI.s_OriginalText = string.Empty;
                editor.text = EditorGUI.s_OriginalText;
                editor.EndEditing();
                flag1 = true;
                break;
              }
              break;
            }
            if ((int) character == 10 || (int) character == 3)
            {
              if (!editor.IsEditingControl(id))
              {
                editor.BeginEditing(id, text, position, style, multiline, passwordField);
                editor.SelectAll();
              }
              else if (!multiline || current.alt || (current.shift || current.control))
              {
                editor.EndEditing();
              }
              else
              {
                editor.Insert(character);
                flag1 = true;
                break;
              }
              current.Use();
              break;
            }
            if ((int) character == 9 || current.keyCode == KeyCode.Tab)
            {
              if (multiline && editor.IsEditingControl(id))
              {
                bool flag2 = allowedletters == null || allowedletters.IndexOf(character) != -1;
                if (!current.alt && !current.shift && !current.control && (int) character == 9 && flag2)
                {
                  editor.Insert(character);
                  flag1 = true;
                  break;
                }
                break;
              }
              break;
            }
            if ((int) character != 25 && (int) character != 27 && editor.IsEditingControl(id))
            {
              if ((allowedletters == null || allowedletters.IndexOf(character) != -1) && (int) character != 0)
              {
                editor.Insert(character);
                flag1 = true;
                break;
              }
              if (Input.compositionString != string.Empty)
              {
                editor.ReplaceSelection(string.Empty);
                flag1 = true;
              }
              current.Use();
              break;
            }
            break;
          }
          break;
        case EventType.Repaint:
          string str2 = !editor.IsEditingControl(id) ? (!EditorGUI.showMixedValue ? (!passwordField ? text : string.Empty.PadRight(text.Length, '*')) : EditorGUI.s_MixedValueContent.text) : (!passwordField ? editor.text : string.Empty.PadRight(editor.text.Length, '*'));
          if (!string.IsNullOrEmpty(EditorGUI.s_UnitString) && !passwordField)
            str2 = str2 + " " + EditorGUI.s_UnitString;
          if (GUIUtility.hotControl == 0)
            EditorGUIUtility.AddCursorRect(position, MouseCursor.Text);
          if (!editor.IsEditingControl(id))
          {
            EditorGUI.BeginHandleMixedValueContentColor();
            style.Draw(position, EditorGUIUtility.TempContent(str2), id, false);
            EditorGUI.EndHandleMixedValueContentColor();
            break;
          }
          editor.DrawCursor(str2);
          break;
        default:
          switch (typeForControl - 13)
          {
            case EventType.MouseDown:
              if (GUIUtility.keyboardControl == id)
              {
                string commandName = current.commandName;
                if (commandName != null)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (EditorGUI.\u003C\u003Ef__switch\u0024map0 == null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    EditorGUI.\u003C\u003Ef__switch\u0024map0 = new Dictionary<string, int>(6)
                    {
                      {
                        "Cut",
                        0
                      },
                      {
                        "Copy",
                        0
                      },
                      {
                        "Paste",
                        1
                      },
                      {
                        "SelectAll",
                        2
                      },
                      {
                        "UndoRedoPerformed",
                        3
                      },
                      {
                        "Delete",
                        4
                      }
                    };
                  }
                  int num;
                  // ISSUE: reference to a compiler-generated field
                  if (EditorGUI.\u003C\u003Ef__switch\u0024map0.TryGetValue(commandName, out num))
                  {
                    switch (num)
                    {
                      case 0:
                        if (editor.hasSelection)
                        {
                          current.Use();
                          break;
                        }
                        break;
                      case 1:
                        if (editor.CanPaste())
                        {
                          current.Use();
                          break;
                        }
                        break;
                      case 2:
                        current.Use();
                        break;
                      case 3:
                        editor.text = text;
                        current.Use();
                        break;
                      case 4:
                        current.Use();
                        break;
                    }
                  }
                  else
                    break;
                }
                else
                  break;
              }
              else
                break;
            case EventType.MouseUp:
              if (GUIUtility.keyboardControl == id)
              {
                string commandName = current.commandName;
                if (commandName != null)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (EditorGUI.\u003C\u003Ef__switch\u0024map1 == null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    EditorGUI.\u003C\u003Ef__switch\u0024map1 = new Dictionary<string, int>(6)
                    {
                      {
                        "OnLostFocus",
                        0
                      },
                      {
                        "Cut",
                        1
                      },
                      {
                        "Copy",
                        2
                      },
                      {
                        "Paste",
                        3
                      },
                      {
                        "SelectAll",
                        4
                      },
                      {
                        "Delete",
                        5
                      }
                    };
                  }
                  int num;
                  // ISSUE: reference to a compiler-generated field
                  if (EditorGUI.\u003C\u003Ef__switch\u0024map1.TryGetValue(commandName, out num))
                  {
                    switch (num)
                    {
                      case 0:
                        if (EditorGUI.activeEditor != null)
                          EditorGUI.activeEditor.EndEditing();
                        current.Use();
                        break;
                      case 1:
                        editor.BeginEditing(id, text, position, style, multiline, passwordField);
                        editor.Cut();
                        flag1 = true;
                        break;
                      case 2:
                        editor.Copy();
                        current.Use();
                        break;
                      case 3:
                        editor.BeginEditing(id, text, position, style, multiline, passwordField);
                        editor.Paste();
                        flag1 = true;
                        break;
                      case 4:
                        editor.SelectAll();
                        current.Use();
                        break;
                      case 5:
                        editor.BeginEditing(id, text, position, style, multiline, passwordField);
                        if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer || (Application.platform == RuntimePlatform.OSXDashboardPlayer || Application.platform == RuntimePlatform.OSXEditor) || Application.platform == RuntimePlatform.WebGLPlayer && SystemInfo.operatingSystem.StartsWith("Mac"))
                          editor.Delete();
                        else
                          editor.Cut();
                        flag1 = true;
                        current.Use();
                        break;
                    }
                  }
                  else
                    break;
                }
                else
                  break;
              }
              else
                break;
            case EventType.MouseDrag:
              if (position.Contains(current.mousePosition))
              {
                if (!editor.IsEditingControl(id))
                {
                  GUIUtility.keyboardControl = id;
                  editor.BeginEditing(id, text, position, style, multiline, passwordField);
                  editor.MoveCursorToPosition(Event.current.mousePosition);
                }
                EditorGUI.ShowTextEditorPopupMenu();
                Event.current.Use();
                break;
              }
              break;
          }
      }
      if (GUIUtility.keyboardControl == id)
        GUIUtility.textFieldInput = true;
      editor.UpdateScrollOffsetIfNeeded();
      changed = false;
      if (flag1)
      {
        changed = text1 != editor.text;
        current.Use();
      }
      if (changed)
      {
        GUI.changed = true;
        return editor.text;
      }
      EditorGUI.RecycledTextEditor.s_AllowContextCutOrPaste = true;
      return str1;
    }

    internal static Event KeyEventField(Rect position, Event evt)
    {
      return EditorGUI.DoKeyEventField(position, evt, GUI.skin.textField);
    }

    internal static Event DoKeyEventField(Rect position, Event _event, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_KeyEventFieldHash, FocusType.Native, position);
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (position.Contains(current.mousePosition))
          {
            GUIUtility.hotControl = controlId;
            current.Use();
            EditorGUI.bKeyEventActive = !EditorGUI.bKeyEventActive;
          }
          return _event;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            GUIUtility.hotControl = controlId;
            current.Use();
          }
          return _event;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl == controlId && EditorGUI.bKeyEventActive && ((int) current.character != 0 || (!current.alt || current.keyCode != KeyCode.AltGr && current.keyCode != KeyCode.LeftAlt && current.keyCode != KeyCode.RightAlt) && (!current.control || current.keyCode != KeyCode.LeftControl && current.keyCode != KeyCode.RightControl) && ((!current.command || current.keyCode != KeyCode.LeftCommand && current.keyCode != KeyCode.RightCommand && (current.keyCode != KeyCode.LeftWindows && current.keyCode != KeyCode.RightWindows)) && (!current.shift || current.keyCode != KeyCode.LeftShift && current.keyCode != KeyCode.RightShift && current.keyCode != KeyCode.None))))
          {
            EditorGUI.bKeyEventActive = false;
            GUI.changed = true;
            GUIUtility.hotControl = 0;
            Event @event = new Event(current);
            current.Use();
            return @event;
          }
          break;
        case EventType.Repaint:
          if (EditorGUI.bKeyEventActive)
          {
            GUIContent content = EditorGUIUtility.TempContent("[Please press a key]");
            style.Draw(position, content, controlId);
            break;
          }
          string t = InternalEditorUtility.TextifyEvent(_event);
          style.Draw(position, EditorGUIUtility.TempContent(t), controlId);
          break;
      }
      return _event;
    }

    internal static Rect GetInspectorTitleBarObjectFoldoutRenderRect(Rect rect)
    {
      return new Rect(rect.x + 3f, rect.y + 3f, 16f, 16f);
    }

    internal static bool DoObjectMouseInteraction(bool foldout, Rect interactionRect, UnityEngine.Object[] targetObjs, int id)
    {
      bool enabled = GUI.enabled;
      GUI.enabled = true;
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(id);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (interactionRect.Contains(current.mousePosition))
          {
            if (current.button == 1 && targetObjs[0] != (UnityEngine.Object) null)
            {
              EditorUtility.DisplayObjectContextMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f), targetObjs, 0);
              current.Use();
              break;
            }
            if (current.button == 0 && (Application.platform != RuntimePlatform.OSXEditor || !current.control))
            {
              GUIUtility.hotControl = id;
              GUIUtility.keyboardControl = id;
              ((DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), id)).mouseDownPosition = current.mousePosition;
              current.Use();
              break;
            }
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id)
          {
            GUIUtility.hotControl = 0;
            current.Use();
            if (interactionRect.Contains(current.mousePosition))
            {
              GUI.changed = true;
              foldout = !foldout;
              break;
            }
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == id)
          {
            if (((DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), id)).CanStartDrag())
            {
              GUIUtility.hotControl = 0;
              DragAndDrop.PrepareStartDrag();
              DragAndDrop.objectReferences = targetObjs;
              if (targetObjs.Length > 1)
                DragAndDrop.StartDrag("<Multiple>");
              else
                DragAndDrop.StartDrag(ObjectNames.GetDragAndDropTitle(targetObjs[0]));
            }
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.keyboardControl == id)
          {
            if (current.keyCode == KeyCode.LeftArrow)
            {
              foldout = false;
              current.Use();
            }
            if (current.keyCode == KeyCode.RightArrow)
            {
              foldout = true;
              current.Use();
              break;
            }
            break;
          }
          break;
        case EventType.DragUpdated:
          if (EditorGUI.s_DragUpdatedOverID == id)
          {
            if (interactionRect.Contains(current.mousePosition))
            {
              if ((double) Time.realtimeSinceStartup > EditorGUI.s_FoldoutDestTime)
              {
                foldout = true;
                HandleUtility.Repaint();
              }
            }
            else
              EditorGUI.s_DragUpdatedOverID = 0;
          }
          else if (interactionRect.Contains(current.mousePosition))
          {
            EditorGUI.s_DragUpdatedOverID = id;
            EditorGUI.s_FoldoutDestTime = (double) Time.realtimeSinceStartup + 0.7;
          }
          if (interactionRect.Contains(current.mousePosition))
          {
            DragAndDrop.visualMode = InternalEditorUtility.InspectorWindowDrag(targetObjs, false);
            Event.current.Use();
            break;
          }
          break;
        case EventType.DragPerform:
          if (interactionRect.Contains(current.mousePosition))
          {
            DragAndDrop.visualMode = InternalEditorUtility.InspectorWindowDrag(targetObjs, true);
            DragAndDrop.AcceptDrag();
            Event.current.Use();
            break;
          }
          break;
        default:
          if (typeForControl == EventType.ContextClick && interactionRect.Contains(current.mousePosition) && targetObjs[0] != (UnityEngine.Object) null)
          {
            EditorUtility.DisplayObjectContextMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f), targetObjs, 0);
            current.Use();
            break;
          }
          break;
      }
      GUI.enabled = enabled;
      return foldout;
    }

    private static void DoObjectFoldoutInternal(bool foldout, Rect interactionRect, Rect renderRect, UnityEngine.Object[] targetObjs, int id)
    {
      bool enabled = GUI.enabled;
      GUI.enabled = true;
      if (Event.current.GetTypeForControl(id) == EventType.Repaint)
      {
        bool flag = GUIUtility.hotControl == id;
        EditorStyles.foldout.Draw(renderRect, flag, flag, foldout, false);
      }
      GUI.enabled = enabled;
    }

    internal static bool DoObjectFoldout(bool foldout, Rect interactionRect, Rect renderRect, UnityEngine.Object[] targetObjs, int id)
    {
      foldout = EditorGUI.DoObjectMouseInteraction(foldout, interactionRect, targetObjs, id);
      EditorGUI.DoObjectFoldoutInternal(foldout, interactionRect, renderRect, targetObjs, id);
      return foldout;
    }

    internal static void LabelFieldInternal(Rect position, GUIContent label, GUIContent label2, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FloatFieldHash, FocusType.Passive, position);
      position = EditorGUI.PrefixLabel(position, controlId, label);
      if (Event.current.type != EventType.Repaint)
        return;
      style.Draw(position, label2, controlId);
    }

    /// <summary>
    ///   <para>Make a toggle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the toggle.</param>
    /// <param name="label">Optional label in front of the toggle.</param>
    /// <param name="value">The shown state of the toggle.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The selected state of the toggle.</para>
    /// </returns>
    public static bool Toggle(Rect position, bool value)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ToggleHash, EditorGUIUtility.native, position);
      return EditorGUIInternal.DoToggleForward(EditorGUI.IndentedRect(position), controlId, value, GUIContent.none, EditorStyles.toggle);
    }

    /// <summary>
    ///   <para>Make a toggle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the toggle.</param>
    /// <param name="label">Optional label in front of the toggle.</param>
    /// <param name="value">The shown state of the toggle.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The selected state of the toggle.</para>
    /// </returns>
    public static bool Toggle(Rect position, string label, bool value)
    {
      return EditorGUI.Toggle(position, EditorGUIUtility.TempContent(label), value);
    }

    /// <summary>
    ///   <para>Make a toggle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the toggle.</param>
    /// <param name="label">Optional label in front of the toggle.</param>
    /// <param name="value">The shown state of the toggle.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The selected state of the toggle.</para>
    /// </returns>
    public static bool Toggle(Rect position, bool value, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ToggleHash, EditorGUIUtility.native, position);
      return EditorGUIInternal.DoToggleForward(position, controlId, value, GUIContent.none, style);
    }

    /// <summary>
    ///   <para>Make a toggle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the toggle.</param>
    /// <param name="label">Optional label in front of the toggle.</param>
    /// <param name="value">The shown state of the toggle.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The selected state of the toggle.</para>
    /// </returns>
    public static bool Toggle(Rect position, string label, bool value, GUIStyle style)
    {
      return EditorGUI.Toggle(position, EditorGUIUtility.TempContent(label), value, style);
    }

    /// <summary>
    ///   <para>Make a toggle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the toggle.</param>
    /// <param name="label">Optional label in front of the toggle.</param>
    /// <param name="value">The shown state of the toggle.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The selected state of the toggle.</para>
    /// </returns>
    public static bool Toggle(Rect position, GUIContent label, bool value)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ToggleHash, EditorGUIUtility.native, position);
      return EditorGUIInternal.DoToggleForward(EditorGUI.PrefixLabel(position, controlId, label), controlId, value, GUIContent.none, EditorStyles.toggle);
    }

    /// <summary>
    ///   <para>Make a toggle.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the toggle.</param>
    /// <param name="label">Optional label in front of the toggle.</param>
    /// <param name="value">The shown state of the toggle.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The selected state of the toggle.</para>
    /// </returns>
    public static bool Toggle(Rect position, GUIContent label, bool value, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ToggleHash, EditorGUIUtility.native, position);
      return EditorGUIInternal.DoToggleForward(EditorGUI.PrefixLabel(position, controlId, label), controlId, value, GUIContent.none, style);
    }

    internal static bool ToggleLeftInternal(Rect position, GUIContent label, bool value, GUIStyle labelStyle)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ToggleHash, EditorGUIUtility.native, position);
      Rect position1 = EditorGUI.IndentedRect(position);
      Rect labelPosition = EditorGUI.IndentedRect(position);
      labelPosition.xMin += 13f;
      EditorGUI.HandlePrefixLabel(position, labelPosition, label, controlId, labelStyle);
      return EditorGUIInternal.DoToggleForward(position1, controlId, value, GUIContent.none, EditorStyles.toggle);
    }

    internal static bool DoToggle(Rect position, int id, bool value, GUIContent content, GUIStyle style)
    {
      return EditorGUIInternal.DoToggleForward(position, id, value, content, style);
    }

    internal static string TextFieldInternal(Rect position, string text, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TextFieldHash, FocusType.Keyboard, position);
      bool changed;
      text = EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, controlId, EditorGUI.IndentedRect(position), text, style, (string) null, out changed, false, false, false);
      return text;
    }

    internal static string TextFieldInternal(Rect position, GUIContent label, string text, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TextFieldHash, FocusType.Keyboard, position);
      bool changed;
      text = EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, controlId, EditorGUI.PrefixLabel(position, controlId, label), text, style, (string) null, out changed, false, false, false);
      return text;
    }

    internal static string ToolbarSearchField(int id, Rect position, string text, bool showWithPopupArrow)
    {
      Rect position1 = position;
      position1.width -= 14f;
      bool changed;
      text = EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, id, position1, text, !showWithPopupArrow ? EditorStyles.toolbarSearchField : EditorStyles.toolbarSearchFieldPopup, (string) null, out changed, false, false, false);
      Rect position2 = position;
      position2.x += position.width - 14f;
      position2.width = 14f;
      if (GUI.Button(position2, GUIContent.none, !(text != string.Empty) ? EditorStyles.toolbarSearchFieldCancelButtonEmpty : EditorStyles.toolbarSearchFieldCancelButton) && text != string.Empty)
      {
        EditorGUI.s_RecycledEditor.text = text = string.Empty;
        GUIUtility.keyboardControl = 0;
      }
      return text;
    }

    internal static string ToolbarSearchField(Rect position, string[] searchModes, ref int searchMode, string text)
    {
      return EditorGUI.ToolbarSearchField(GUIUtility.GetControlID(EditorGUI.s_SearchFieldHash, FocusType.Keyboard, position), position, searchModes, ref searchMode, text);
    }

    internal static string ToolbarSearchField(int id, Rect position, string[] searchModes, ref int searchMode, string text)
    {
      bool showWithPopupArrow = searchModes != null;
      if (showWithPopupArrow)
      {
        searchMode = EditorGUI.PopupCallbackInfo.GetSelectedValueForControl(id, searchMode);
        Rect rect = position;
        rect.width = 20f;
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
          EditorGUI.PopupCallbackInfo.instance = new EditorGUI.PopupCallbackInfo(id);
          EditorUtility.DisplayCustomMenu(position, EditorGUIUtility.TempContent(searchModes), searchMode, new EditorUtility.SelectMenuItemFunction(EditorGUI.PopupCallbackInfo.instance.SetEnumValueDelegate), (object) null);
          if (EditorGUI.s_RecycledEditor.IsEditingControl(id))
            Event.current.Use();
        }
      }
      text = EditorGUI.ToolbarSearchField(id, position, text, showWithPopupArrow);
      if (showWithPopupArrow && text == string.Empty && (!EditorGUI.s_RecycledEditor.IsEditingControl(id) && Event.current.type == EventType.Repaint))
      {
        position.width -= 14f;
        EditorGUI.BeginDisabledGroup(true);
        EditorStyles.toolbarSearchFieldPopup.Draw(position, EditorGUIUtility.TempContent(searchModes[searchMode]), id, false);
        EditorGUI.EndDisabledGroup();
      }
      return text;
    }

    internal static string SearchField(Rect position, string text)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_SearchFieldHash, FocusType.Keyboard, position);
      Rect position1 = position;
      position1.width -= 15f;
      bool changed;
      text = EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, controlId, position1, text, EditorStyles.searchField, (string) null, out changed, false, false, false);
      Rect position2 = position;
      position2.x += position.width - 15f;
      position2.width = 15f;
      if (GUI.Button(position2, GUIContent.none, !(text != string.Empty) ? EditorStyles.searchFieldCancelButtonEmpty : EditorStyles.searchFieldCancelButton) && text != string.Empty)
      {
        EditorGUI.s_RecycledEditor.text = text = string.Empty;
        GUIUtility.keyboardControl = 0;
      }
      return text;
    }

    internal static string ScrollableTextAreaInternal(Rect position, string text, ref Vector2 scrollPosition, GUIStyle style)
    {
      if (Event.current.type == EventType.Layout)
        return text;
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TextAreaHash, FocusType.Keyboard, position);
      float height1 = style.CalcHeight(GUIContent.Temp(text), position.width);
      Rect rect = new Rect(0.0f, 0.0f, position.width, height1);
      Vector2 contentOffset = style.contentOffset;
      if ((double) position.height < (double) rect.height)
      {
        Rect position1 = position;
        position1.width = GUI.skin.verticalScrollbar.fixedWidth;
        position1.height -= 2f;
        ++position1.y;
        position1.x = position.x + position.width - position1.width;
        position.width -= position1.width;
        float height2 = style.CalcHeight(GUIContent.Temp(text), position.width);
        rect = new Rect(0.0f, 0.0f, position.width, height2);
        if (position.Contains(Event.current.mousePosition) && Event.current.type == EventType.ScrollWheel)
        {
          float num = scrollPosition.y + Event.current.delta.y * 10f;
          scrollPosition.y = Mathf.Clamp(num, 0.0f, rect.height);
          Event.current.Use();
        }
        scrollPosition.y = GUI.VerticalScrollbar(position1, scrollPosition.y, position.height, 0.0f, rect.height);
        if (!EditorGUI.s_RecycledEditor.IsEditingControl(controlId))
        {
          style.contentOffset -= scrollPosition;
          style.Internal_clipOffset = scrollPosition;
        }
        else
          EditorGUI.s_RecycledEditor.scrollOffset.y = scrollPosition.y;
      }
      EventType type = Event.current.type;
      bool changed;
      string str = EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, controlId, EditorGUI.IndentedRect(position), text, style, (string) null, out changed, false, true, false);
      if (type != Event.current.type)
        scrollPosition = EditorGUI.s_RecycledEditor.scrollOffset;
      style.contentOffset = contentOffset;
      style.Internal_clipOffset = Vector2.zero;
      return str;
    }

    internal static string TextAreaInternal(Rect position, string text, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TextAreaHash, FocusType.Keyboard, position);
      bool changed;
      text = EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, controlId, EditorGUI.IndentedRect(position), text, style, (string) null, out changed, false, true, false);
      return text;
    }

    internal static void SelectableLabelInternal(Rect position, string text, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_SelectableLabelHash, FocusType.Keyboard, position);
      Event current = Event.current;
      if (GUIUtility.keyboardControl == controlId && current.GetTypeForControl(controlId) == EventType.KeyDown)
      {
        KeyCode keyCode = current.keyCode;
        switch (keyCode)
        {
          case KeyCode.UpArrow:
          case KeyCode.DownArrow:
          case KeyCode.RightArrow:
          case KeyCode.LeftArrow:
          case KeyCode.Home:
          case KeyCode.End:
          case KeyCode.PageUp:
          case KeyCode.PageDown:
            break;
          default:
            if (keyCode == KeyCode.Space)
            {
              GUIUtility.hotControl = 0;
              GUIUtility.keyboardControl = 0;
              break;
            }
            if ((int) current.character != 9)
            {
              current.Use();
              break;
            }
            break;
        }
      }
      if (current.type == EventType.ExecuteCommand && (current.commandName == "Paste" || current.commandName == "Cut") && GUIUtility.keyboardControl == controlId)
        current.Use();
      Color cursorColor = GUI.skin.settings.cursorColor;
      GUI.skin.settings.cursorColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      EditorGUI.RecycledTextEditor.s_AllowContextCutOrPaste = false;
      bool changed;
      text = EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, controlId, EditorGUI.IndentedRect(position), text, style, string.Empty, out changed, false, true, false);
      GUI.skin.settings.cursorColor = cursorColor;
    }

    public static string DoPasswordField(int id, Rect position, string password, GUIStyle style)
    {
      bool changed;
      return EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, id, position, password, style, (string) null, out changed, false, false, true);
    }

    public static string DoPasswordField(int id, Rect position, GUIContent label, string password, GUIStyle style)
    {
      bool changed;
      return EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, id, EditorGUI.PrefixLabel(position, id, label), password, style, (string) null, out changed, false, false, true);
    }

    internal static string PasswordFieldInternal(Rect position, string password, GUIStyle style)
    {
      return EditorGUI.DoPasswordField(GUIUtility.GetControlID(EditorGUI.s_PasswordFieldHash, FocusType.Keyboard, position), EditorGUI.IndentedRect(position), password, style);
    }

    internal static string PasswordFieldInternal(Rect position, GUIContent label, string password, GUIStyle style)
    {
      return EditorGUI.DoPasswordField(GUIUtility.GetControlID(EditorGUI.s_PasswordFieldHash, FocusType.Keyboard, position), position, label, password, style);
    }

    internal static float FloatFieldInternal(Rect position, float value, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FloatFieldHash, FocusType.Keyboard, position);
      return EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, EditorGUI.IndentedRect(position), new Rect(0.0f, 0.0f, 0.0f, 0.0f), controlId, value, EditorGUI.kFloatFieldFormatString, style, false);
    }

    internal static float FloatFieldInternal(Rect position, GUIContent label, float value, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FloatFieldHash, FocusType.Keyboard, position);
      Rect position1 = EditorGUI.PrefixLabel(position, controlId, label);
      position.xMax = position1.x;
      return EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, position1, position, controlId, value, EditorGUI.kFloatFieldFormatString, style, true);
    }

    internal static double DoubleFieldInternal(Rect position, double value, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FloatFieldHash, FocusType.Keyboard, position);
      return EditorGUI.DoDoubleField(EditorGUI.s_RecycledEditor, EditorGUI.IndentedRect(position), new Rect(0.0f, 0.0f, 0.0f, 0.0f), controlId, value, EditorGUI.kDoubleFieldFormatString, style, false);
    }

    internal static double DoubleFieldInternal(Rect position, GUIContent label, double value, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FloatFieldHash, FocusType.Keyboard, position);
      Rect position1 = EditorGUI.PrefixLabel(position, controlId, label);
      position.xMax = position1.x;
      return EditorGUI.DoDoubleField(EditorGUI.s_RecycledEditor, position1, position, controlId, value, EditorGUI.kDoubleFieldFormatString, style, true);
    }

    private static double CalculateFloatDragSensitivity(double value)
    {
      if (double.IsInfinity(value) || double.IsNaN(value))
        return 0.0;
      return Math.Max(1.0, Math.Pow(Math.Abs(value), 0.5)) * 0.0299999993294477;
    }

    private static long CalculateIntDragSensitivity(long value)
    {
      return (long) Math.Max(1.0, Math.Pow(Math.Abs((double) value), 0.5) * 0.0299999993294477);
    }

    private static void DragNumberValue(EditorGUI.RecycledTextEditor editor, Rect position, Rect dragHotZone, int id, bool isDouble, ref double doubleVal, ref long longVal, string formatString, GUIStyle style, double dragSensitivity)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (!dragHotZone.Contains(current.mousePosition) || current.button != 0)
            break;
          EditorGUIUtility.editingTextField = false;
          GUIUtility.hotControl = id;
          if (EditorGUI.activeEditor != null)
            EditorGUI.activeEditor.EndEditing();
          current.Use();
          GUIUtility.keyboardControl = id;
          EditorGUI.s_DragCandidateState = 1;
          EditorGUI.s_DragStartValue = doubleVal;
          EditorGUI.s_DragStartIntValue = longVal;
          EditorGUI.s_DragStartPos = current.mousePosition;
          EditorGUI.s_DragSensitivity = dragSensitivity;
          current.Use();
          EditorGUIUtility.SetWantsMouseJumping(1);
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != id || EditorGUI.s_DragCandidateState == 0)
            break;
          GUIUtility.hotControl = 0;
          EditorGUI.s_DragCandidateState = 0;
          current.Use();
          EditorGUIUtility.SetWantsMouseJumping(0);
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != id)
            break;
          switch (EditorGUI.s_DragCandidateState)
          {
            case 1:
              if ((double) (Event.current.mousePosition - EditorGUI.s_DragStartPos).sqrMagnitude > (double) EditorGUI.kDragDeadzone)
              {
                EditorGUI.s_DragCandidateState = 2;
                GUIUtility.keyboardControl = id;
              }
              current.Use();
              return;
            case 2:
              if (isDouble)
              {
                doubleVal = doubleVal + (double) HandleUtility.niceMouseDelta * EditorGUI.s_DragSensitivity;
                doubleVal = MathUtils.RoundBasedOnMinimumDifference(doubleVal, EditorGUI.s_DragSensitivity);
              }
              else
                longVal = longVal + (long) Math.Round((double) HandleUtility.niceMouseDelta * EditorGUI.s_DragSensitivity);
              GUI.changed = true;
              current.Use();
              return;
            default:
              return;
          }
        case EventType.KeyDown:
          if (GUIUtility.hotControl != id || current.keyCode != KeyCode.Escape || EditorGUI.s_DragCandidateState == 0)
            break;
          doubleVal = EditorGUI.s_DragStartValue;
          longVal = EditorGUI.s_DragStartIntValue;
          GUI.changed = true;
          GUIUtility.hotControl = 0;
          current.Use();
          break;
        case EventType.Repaint:
          EditorGUIUtility.AddCursorRect(dragHotZone, MouseCursor.SlideArrow);
          break;
      }
    }

    internal static float DoFloatField(EditorGUI.RecycledTextEditor editor, Rect position, Rect dragHotZone, int id, float value, string formatString, GUIStyle style, bool draggable)
    {
      return EditorGUI.DoFloatField(editor, position, dragHotZone, id, value, formatString, style, draggable, Event.current.GetTypeForControl(id) != EventType.MouseDown ? 0.0f : (float) EditorGUI.CalculateFloatDragSensitivity(EditorGUI.s_DragStartValue));
    }

    internal static float DoFloatField(EditorGUI.RecycledTextEditor editor, Rect position, Rect dragHotZone, int id, float value, string formatString, GUIStyle style, bool draggable, float dragSensitivity)
    {
      long longVal = 0;
      double doubleVal = (double) value;
      EditorGUI.DoNumberField(editor, position, dragHotZone, id, true, ref doubleVal, ref longVal, formatString, style, draggable, (double) dragSensitivity);
      return MathUtils.ClampToFloat(doubleVal);
    }

    internal static int DoIntField(EditorGUI.RecycledTextEditor editor, Rect position, Rect dragHotZone, int id, int value, string formatString, GUIStyle style, bool draggable, float dragSensitivity)
    {
      double doubleVal = 0.0;
      long longVal = (long) value;
      EditorGUI.DoNumberField(editor, position, dragHotZone, id, false, ref doubleVal, ref longVal, formatString, style, draggable, (double) dragSensitivity);
      return MathUtils.ClampToInt(longVal);
    }

    internal static double DoDoubleField(EditorGUI.RecycledTextEditor editor, Rect position, Rect dragHotZone, int id, double value, string formatString, GUIStyle style, bool draggable)
    {
      return EditorGUI.DoDoubleField(editor, position, dragHotZone, id, value, formatString, style, draggable, Event.current.GetTypeForControl(id) != EventType.MouseDown ? 0.0 : EditorGUI.CalculateFloatDragSensitivity(EditorGUI.s_DragStartValue));
    }

    internal static double DoDoubleField(EditorGUI.RecycledTextEditor editor, Rect position, Rect dragHotZone, int id, double value, string formatString, GUIStyle style, bool draggable, double dragSensitivity)
    {
      long longVal = 0;
      EditorGUI.DoNumberField(editor, position, dragHotZone, id, true, ref value, ref longVal, formatString, style, draggable, dragSensitivity);
      return value;
    }

    internal static long DoLongField(EditorGUI.RecycledTextEditor editor, Rect position, Rect dragHotZone, int id, long value, string formatString, GUIStyle style, bool draggable, double dragSensitivity)
    {
      double doubleVal = 0.0;
      EditorGUI.DoNumberField(editor, position, dragHotZone, id, false, ref doubleVal, ref value, formatString, style, draggable, dragSensitivity);
      return value;
    }

    private static bool HasKeyboardFocus(int controlID)
    {
      if (GUIUtility.keyboardControl == controlID)
        return GUIView.current.hasFocus;
      return false;
    }

    internal static void DoNumberField(EditorGUI.RecycledTextEditor editor, Rect position, Rect dragHotZone, int id, bool isDouble, ref double doubleVal, ref long longVal, string formatString, GUIStyle style, bool draggable, double dragSensitivity)
    {
      string allowedletters = !isDouble ? EditorGUI.s_AllowedCharactersForInt : EditorGUI.s_AllowedCharactersForFloat;
      if (draggable)
        EditorGUI.DragNumberValue(editor, position, dragHotZone, id, isDouble, ref doubleVal, ref longVal, formatString, style, dragSensitivity);
      Event current = Event.current;
      string text;
      if (EditorGUI.HasKeyboardFocus(id) || current.type == EventType.MouseDown && current.button == 0 && position.Contains(current.mousePosition))
      {
        if (!editor.IsEditingControl(id))
        {
          text = EditorGUI.s_RecycledCurrentEditingString = !isDouble ? longVal.ToString(formatString) : doubleVal.ToString(formatString);
        }
        else
        {
          text = EditorGUI.s_RecycledCurrentEditingString;
          if (current.type == EventType.ValidateCommand && current.commandName == "UndoRedoPerformed")
            text = !isDouble ? longVal.ToString(formatString) : doubleVal.ToString(formatString);
        }
      }
      else
        text = !isDouble ? longVal.ToString(formatString) : doubleVal.ToString(formatString);
      if (GUIUtility.keyboardControl == id)
      {
        bool changed;
        string str1 = EditorGUI.DoTextField(editor, id, position, text, style, allowedletters, out changed, false, false, false);
        if (!changed)
          return;
        GUI.changed = true;
        EditorGUI.s_RecycledCurrentEditingString = str1;
        if (isDouble)
        {
          string lower = str1.ToLower();
          if (lower == "inf" || lower == "infinity")
            doubleVal = double.PositiveInfinity;
          else if (lower == "-inf" || lower == "-infinity")
          {
            doubleVal = double.NegativeInfinity;
          }
          else
          {
            string str2 = str1.Replace(',', '.');
            if (!double.TryParse(str2, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out doubleVal))
            {
              doubleVal = EditorGUI.s_RecycledCurrentEditingFloat = ExpressionEvaluator.Evaluate<double>(str2);
            }
            else
            {
              if (double.IsNaN(doubleVal))
                doubleVal = 0.0;
              EditorGUI.s_RecycledCurrentEditingFloat = doubleVal;
            }
          }
        }
        else if (!long.TryParse(str1, out longVal))
          longVal = EditorGUI.s_RecycledCurrentEditingInt = ExpressionEvaluator.Evaluate<long>(str1);
        else
          EditorGUI.s_RecycledCurrentEditingInt = longVal;
      }
      else
      {
        bool changed;
        EditorGUI.DoTextField(editor, id, position, text, style, allowedletters, out changed, false, false, false);
      }
    }

    internal static int ArraySizeField(Rect position, GUIContent label, int value, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ArraySizeFieldHash, FocusType.Keyboard, position);
      EditorGUI.BeginChangeCheck();
      string s = EditorGUI.DelayedTextFieldInternal(position, controlId, label, value.ToString(EditorGUI.kIntFieldFormatString), "0123456789-", style);
      if (EditorGUI.EndChangeCheck())
      {
        try
        {
          value = int.Parse(s, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
        }
        catch (FormatException ex)
        {
        }
      }
      return value;
    }

    internal static string DelayedTextFieldInternal(Rect position, string value, string allowedLetters, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_DelayedTextFieldHash, FocusType.Keyboard, position);
      return EditorGUI.DelayedTextFieldInternal(position, controlId, GUIContent.none, value, allowedLetters, style);
    }

    internal static string DelayedTextFieldInternal(Rect position, int id, GUIContent label, string value, string allowedLetters, GUIStyle style)
    {
      string str1;
      if (EditorGUI.HasKeyboardFocus(id))
      {
        str1 = EditorGUI.s_DelayedTextEditor.IsEditingControl(id) ? EditorGUI.s_RecycledCurrentEditingString : (EditorGUI.s_RecycledCurrentEditingString = value);
        Event current = Event.current;
        if (current.type == EventType.ValidateCommand && current.commandName == "UndoRedoPerformed")
          str1 = value;
      }
      else
        str1 = value;
      bool changed1 = GUI.changed;
      bool changed2;
      string text = EditorGUI.s_DelayedTextEditor.OnGUI(id, str1, out changed2);
      GUI.changed = false;
      if (!changed2)
      {
        string str2 = EditorGUI.DoTextField((EditorGUI.RecycledTextEditor) EditorGUI.s_DelayedTextEditor, id, EditorGUI.PrefixLabel(position, id, label), text, style, allowedLetters, out changed2, false, false, false);
        GUI.changed = false;
        if (GUIUtility.keyboardControl == id)
        {
          if (!EditorGUI.s_DelayedTextEditor.IsEditingControl(id))
          {
            if (value != str2)
            {
              GUI.changed = true;
              value = str2;
            }
          }
          else
            EditorGUI.s_RecycledCurrentEditingString = str2;
        }
      }
      else
      {
        GUI.changed = true;
        value = text;
      }
      GUI.changed |= changed1;
      return value;
    }

    internal static void DelayedTextFieldInternal(Rect position, int id, SerializedProperty property, string allowedLetters, GUIContent label)
    {
      label = EditorGUI.BeginProperty(position, label, property);
      EditorGUI.BeginChangeCheck();
      string str = EditorGUI.DelayedTextFieldInternal(position, id, label, property.stringValue, allowedLetters, EditorStyles.textField);
      if (EditorGUI.EndChangeCheck())
        property.stringValue = str;
      EditorGUI.EndProperty();
    }

    internal static float DelayedFloatFieldInternal(Rect position, GUIContent label, float value, GUIStyle style)
    {
      float num = value;
      float result = num;
      EditorGUI.BeginChangeCheck();
      int controlId = GUIUtility.GetControlID(EditorGUI.s_DelayedTextFieldHash, FocusType.Keyboard, position);
      if (EditorGUI.EndChangeCheck() && float.TryParse(EditorGUI.DelayedTextFieldInternal(position, controlId, label, num.ToString(), EditorGUI.s_AllowedCharactersForFloat, style), NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result) && (double) result != (double) num)
      {
        value = result;
        GUI.changed = true;
      }
      return result;
    }

    internal static void DelayedFloatFieldInternal(Rect position, SerializedProperty property, GUIContent label)
    {
      label = EditorGUI.BeginProperty(position, label, property);
      EditorGUI.BeginChangeCheck();
      float num = EditorGUI.DelayedFloatFieldInternal(position, label, property.floatValue, EditorStyles.numberField);
      if (EditorGUI.EndChangeCheck())
        property.floatValue = num;
      EditorGUI.EndProperty();
    }

    internal static int DelayedIntFieldInternal(Rect position, GUIContent label, int value, GUIStyle style)
    {
      int num = value;
      int result = num;
      EditorGUI.BeginChangeCheck();
      int controlId = GUIUtility.GetControlID(EditorGUI.s_DelayedTextFieldHash, FocusType.Keyboard, position);
      if (EditorGUI.EndChangeCheck() && int.TryParse(EditorGUI.DelayedTextFieldInternal(position, controlId, label, num.ToString(), EditorGUI.s_AllowedCharactersForInt, style), out result) && result != num)
      {
        value = result;
        GUI.changed = true;
      }
      return result;
    }

    internal static void DelayedIntFieldInternal(Rect position, SerializedProperty property, GUIContent label)
    {
      label = EditorGUI.BeginProperty(position, label, property);
      EditorGUI.BeginChangeCheck();
      int num = EditorGUI.DelayedIntFieldInternal(position, label, property.intValue, EditorStyles.numberField);
      if (EditorGUI.EndChangeCheck())
        property.intValue = num;
      EditorGUI.EndProperty();
    }

    internal static int IntFieldInternal(Rect position, int value, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FloatFieldHash, FocusType.Keyboard, position);
      return EditorGUI.DoIntField(EditorGUI.s_RecycledEditor, EditorGUI.IndentedRect(position), new Rect(0.0f, 0.0f, 0.0f, 0.0f), controlId, value, EditorGUI.kIntFieldFormatString, style, false, (float) EditorGUI.CalculateIntDragSensitivity((long) value));
    }

    internal static int IntFieldInternal(Rect position, GUIContent label, int value, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FloatFieldHash, FocusType.Keyboard, position);
      Rect position1 = EditorGUI.PrefixLabel(position, controlId, label);
      position.xMax = position1.x;
      return EditorGUI.DoIntField(EditorGUI.s_RecycledEditor, position1, position, controlId, value, EditorGUI.kIntFieldFormatString, style, true, (float) EditorGUI.CalculateIntDragSensitivity((long) value));
    }

    internal static long LongFieldInternal(Rect position, long value, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FloatFieldHash, FocusType.Keyboard, position);
      return EditorGUI.DoLongField(EditorGUI.s_RecycledEditor, EditorGUI.IndentedRect(position), new Rect(0.0f, 0.0f, 0.0f, 0.0f), controlId, value, EditorGUI.kIntFieldFormatString, style, false, (double) EditorGUI.CalculateIntDragSensitivity(value));
    }

    internal static long LongFieldInternal(Rect position, GUIContent label, long value, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FloatFieldHash, FocusType.Keyboard, position);
      Rect position1 = EditorGUI.PrefixLabel(position, controlId, label);
      position.xMax = position1.x;
      return EditorGUI.DoLongField(EditorGUI.s_RecycledEditor, position1, position, controlId, value, EditorGUI.kIntFieldFormatString, style, true, (double) EditorGUI.CalculateIntDragSensitivity(value));
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the slider.</param>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <returns>
    ///   <para>The value that has been set by the user.</para>
    /// </returns>
    public static float Slider(Rect position, float value, float leftValue, float rightValue)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_SliderHash, EditorGUIUtility.native, position);
      return EditorGUI.DoSlider(EditorGUI.IndentedRect(position), EditorGUIUtility.DragZoneRect(position), controlId, value, leftValue, rightValue, EditorGUI.kFloatFieldFormatString);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the slider.</param>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <returns>
    ///   <para>The value that has been set by the user.</para>
    /// </returns>
    public static float Slider(Rect position, string label, float value, float leftValue, float rightValue)
    {
      return EditorGUI.Slider(position, EditorGUIUtility.TempContent(label), value, leftValue, rightValue);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the slider.</param>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <returns>
    ///   <para>The value that has been set by the user.</para>
    /// </returns>
    public static float Slider(Rect position, GUIContent label, float value, float leftValue, float rightValue)
    {
      return EditorGUI.PowerSlider(position, label, value, leftValue, rightValue, 1f);
    }

    internal static float PowerSlider(Rect position, string label, float sliderValue, float leftValue, float rightValue, float power)
    {
      return EditorGUI.PowerSlider(position, EditorGUIUtility.TempContent(label), sliderValue, leftValue, rightValue, power);
    }

    internal static float PowerSlider(Rect position, GUIContent label, float sliderValue, float leftValue, float rightValue, float power)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_SliderHash, EditorGUIUtility.native, position);
      return EditorGUI.DoSlider(EditorGUI.PrefixLabel(position, controlId, label), !EditorGUI.LabelHasContent(label) ? new Rect() : EditorGUIUtility.DragZoneRect(position), controlId, sliderValue, leftValue, rightValue, EditorGUI.kFloatFieldFormatString, power);
    }

    private static float PowPreserveSign(float f, float p)
    {
      float num = Mathf.Pow(Mathf.Abs(f), p);
      if ((double) f < 0.0)
        return -num;
      return num;
    }

    private static void DoPropertyContextMenu(SerializedProperty property)
    {
      GenericMenu menu = new GenericMenu();
      SerializedProperty property1 = property.serializedObject.FindProperty(property.propertyPath);
      ScriptAttributeUtility.GetHandler(property).AddMenuItems(property, menu);
      if (property.hasMultipleDifferentValues && !property.hasVisibleChildren)
        TargetChoiceHandler.AddSetToValueOfTargetMenuItems(menu, property1, new TargetChoiceHandler.TargetChoiceMenuFunction(TargetChoiceHandler.SetToValueOfTarget));
      if (property.serializedObject.targetObjects.Length == 1 && property.isInstantiatedPrefab)
        menu.AddItem(EditorGUIUtility.TextContent("Revert Value to Prefab"), false, new GenericMenu.MenuFunction2(TargetChoiceHandler.SetPrefabOverride), (object) property1);
      if (property.propertyPath.LastIndexOf(']') == property.propertyPath.Length - 1)
      {
        if (menu.GetItemCount() > 0)
          menu.AddSeparator(string.Empty);
        menu.AddItem(EditorGUIUtility.TextContent("Duplicate Array Element"), false, new GenericMenu.MenuFunction2(TargetChoiceHandler.DuplicateArrayElement), (object) property1);
        menu.AddItem(EditorGUIUtility.TextContent("Delete Array Element"), false, new GenericMenu.MenuFunction2(TargetChoiceHandler.DeleteArrayElement), (object) property1);
      }
      if (Event.current.shift)
      {
        if (menu.GetItemCount() > 0)
          menu.AddSeparator(string.Empty);
        menu.AddItem(EditorGUIUtility.TextContent("Print Property Path"), false, (GenericMenu.MenuFunction2) (e => Debug.Log((object) ((SerializedProperty) e).propertyPath)), (object) property1);
      }
      Event.current.Use();
      if (menu.GetItemCount() == 0)
        return;
      menu.ShowAsContext();
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the slider.</param>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="property">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    public static void Slider(Rect position, SerializedProperty property, float leftValue, float rightValue)
    {
      EditorGUI.Slider(position, property, leftValue, rightValue, property.displayName);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the slider.</param>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="property">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    public static void Slider(Rect position, SerializedProperty property, float leftValue, float rightValue, string label)
    {
      EditorGUI.Slider(position, property, leftValue, rightValue, EditorGUIUtility.TempContent(label));
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the slider.</param>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="property">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    public static void Slider(Rect position, SerializedProperty property, float leftValue, float rightValue, GUIContent label)
    {
      label = EditorGUI.BeginProperty(position, label, property);
      EditorGUI.BeginChangeCheck();
      float num = EditorGUI.Slider(position, label, property.floatValue, leftValue, rightValue);
      if (EditorGUI.EndChangeCheck())
        property.floatValue = num;
      EditorGUI.EndProperty();
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change an integer value between a min and a max.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the slider.</param>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <returns>
    ///   <para>The value that has been set by the user.</para>
    /// </returns>
    public static int IntSlider(Rect position, int value, int leftValue, int rightValue)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_SliderHash, EditorGUIUtility.native, position);
      return Mathf.RoundToInt(EditorGUI.DoSlider(EditorGUI.IndentedRect(position), EditorGUIUtility.DragZoneRect(position), controlId, (float) value, (float) leftValue, (float) rightValue, EditorGUI.kIntFieldFormatString));
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change an integer value between a min and a max.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the slider.</param>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <returns>
    ///   <para>The value that has been set by the user.</para>
    /// </returns>
    public static int IntSlider(Rect position, string label, int value, int leftValue, int rightValue)
    {
      return EditorGUI.IntSlider(position, EditorGUIUtility.TempContent(label), value, leftValue, rightValue);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change an integer value between a min and a max.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the slider.</param>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <returns>
    ///   <para>The value that has been set by the user.</para>
    /// </returns>
    public static int IntSlider(Rect position, GUIContent label, int value, int leftValue, int rightValue)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_SliderHash, EditorGUIUtility.native, position);
      return Mathf.RoundToInt(EditorGUI.DoSlider(EditorGUI.PrefixLabel(position, controlId, label), EditorGUIUtility.DragZoneRect(position), controlId, (float) value, (float) leftValue, (float) rightValue, EditorGUI.kIntFieldFormatString));
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the slider.</param>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="property">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    public static void IntSlider(Rect position, SerializedProperty property, int leftValue, int rightValue)
    {
      EditorGUI.IntSlider(position, property, leftValue, rightValue, property.displayName);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the slider.</param>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="property">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    public static void IntSlider(Rect position, SerializedProperty property, int leftValue, int rightValue, string label)
    {
      EditorGUI.IntSlider(position, property, leftValue, rightValue, EditorGUIUtility.TempContent(label));
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the slider.</param>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="property">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    public static void IntSlider(Rect position, SerializedProperty property, int leftValue, int rightValue, GUIContent label)
    {
      label = EditorGUI.BeginProperty(position, label, property);
      EditorGUI.BeginChangeCheck();
      int num = EditorGUI.IntSlider(position, label, property.intValue, leftValue, rightValue);
      if (EditorGUI.EndChangeCheck())
        property.intValue = num;
      EditorGUI.EndProperty();
    }

    internal static void DoTwoLabels(Rect rect, GUIContent leftLabel, GUIContent rightLabel, GUIStyle labelStyle)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      TextAnchor alignment = labelStyle.alignment;
      labelStyle.alignment = TextAnchor.UpperLeft;
      GUI.Label(rect, leftLabel, labelStyle);
      labelStyle.alignment = TextAnchor.UpperRight;
      GUI.Label(rect, rightLabel, labelStyle);
      labelStyle.alignment = alignment;
    }

    private static float DoSlider(Rect position, Rect dragZonePosition, int id, float value, float left, float right, string formatString)
    {
      return EditorGUI.DoSlider(position, dragZonePosition, id, value, left, right, formatString, 1f);
    }

    private static float DoSlider(Rect position, Rect dragZonePosition, int id, float value, float left, float right, string formatString, float power)
    {
      float width1 = position.width;
      if ((double) width1 >= 65.0 + (double) EditorGUIUtility.fieldWidth)
      {
        float width2 = width1 - 5f - EditorGUIUtility.fieldWidth;
        EditorGUI.BeginChangeCheck();
        int controlId = GUIUtility.GetControlID(EditorGUI.s_SliderKnobHash, FocusType.Native, position);
        if (GUIUtility.keyboardControl == id && !EditorGUI.s_RecycledEditor.IsEditingControl(id))
          GUIUtility.keyboardControl = controlId;
        float start = left;
        float end = right;
        if ((double) power != 1.0)
        {
          start = EditorGUI.PowPreserveSign(left, 1f / power);
          end = EditorGUI.PowPreserveSign(right, 1f / power);
          value = EditorGUI.PowPreserveSign(value, 1f / power);
        }
        Rect position1 = new Rect(position.x, position.y, width2, position.height);
        value = GUI.Slider(position1, value, 0.0f, start, end, GUI.skin.horizontalSlider, !EditorGUI.showMixedValue ? GUI.skin.horizontalSliderThumb : (GUIStyle) "SliderMixed", true, controlId);
        if ((double) power != 1.0)
          value = EditorGUI.PowPreserveSign(value, power);
        if (EditorGUIUtility.sliderLabels.HasLabels())
        {
          Color color = GUI.color;
          GUI.color *= new Color(1f, 1f, 1f, 0.5f);
          EditorGUI.DoTwoLabels(new Rect(position1.x, position1.y + 10f, position1.width, position1.height), EditorGUIUtility.sliderLabels.leftLabel, EditorGUIUtility.sliderLabels.rightLabel, EditorStyles.miniLabel);
          GUI.color = color;
          EditorGUIUtility.sliderLabels.SetLabels((GUIContent) null, (GUIContent) null);
        }
        if (GUIUtility.keyboardControl == controlId || GUIUtility.hotControl == controlId)
          GUIUtility.keyboardControl = id;
        if (GUIUtility.keyboardControl == id && Event.current.type == EventType.KeyDown && !EditorGUI.s_RecycledEditor.IsEditingControl(id) && (Event.current.keyCode == KeyCode.LeftArrow || Event.current.keyCode == KeyCode.RightArrow))
        {
          float roundingValue = MathUtils.GetClosestPowerOfTen(Mathf.Abs((float) (((double) right - (double) left) * 0.00999999977648258)));
          if (formatString == EditorGUI.kIntFieldFormatString && (double) roundingValue < 1.0)
            roundingValue = 1f;
          if (Event.current.shift)
            roundingValue *= 10f;
          if (Event.current.keyCode == KeyCode.LeftArrow)
            value -= roundingValue * 0.5001f;
          else
            value += roundingValue * 0.5001f;
          value = MathUtils.RoundToMultipleOf(value, roundingValue);
          GUI.changed = true;
          Event.current.Use();
        }
        if (EditorGUI.EndChangeCheck())
        {
          float f = (float) (((double) right - (double) left) / ((double) width2 - (double) GUI.skin.horizontalSlider.padding.horizontal - (double) GUI.skin.horizontalSliderThumb.fixedWidth));
          value = MathUtils.RoundBasedOnMinimumDifference(value, Mathf.Abs(f));
          if (EditorGUI.s_RecycledEditor.IsEditingControl(id))
            EditorGUI.s_RecycledEditor.EndEditing();
        }
        value = EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, new Rect((float) ((double) position.x + (double) width2 + 5.0), position.y, EditorGUIUtility.fieldWidth, position.height), dragZonePosition, id, value, formatString, EditorStyles.numberField, true);
      }
      else
      {
        float num = Mathf.Min(EditorGUIUtility.fieldWidth, width1);
        position.x = position.xMax - num;
        position.width = num;
        value = EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, position, dragZonePosition, id, value, formatString, EditorStyles.numberField, true);
      }
      value = Mathf.Clamp(value, Mathf.Min(left, right), Mathf.Max(left, right));
      return value;
    }

    public static void MinMaxSlider(GUIContent label, Rect position, ref float minValue, ref float maxValue, float minLimit, float maxLimit)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_MinMaxSliderHash, FocusType.Native);
      EditorGUI.DoMinMaxSlider(EditorGUI.PrefixLabel(position, controlId, label), controlId, ref minValue, ref maxValue, minLimit, maxLimit);
    }

    public static void MinMaxSlider(Rect position, ref float minValue, ref float maxValue, float minLimit, float maxLimit)
    {
      EditorGUI.DoMinMaxSlider(EditorGUI.IndentedRect(position), GUIUtility.GetControlID(EditorGUI.s_MinMaxSliderHash, FocusType.Native), ref minValue, ref maxValue, minLimit, maxLimit);
    }

    private static void DoMinMaxSlider(Rect position, int id, ref float minValue, ref float maxValue, float minLimit, float maxLimit)
    {
      float size = maxValue - minValue;
      EditorGUI.BeginChangeCheck();
      EditorGUIExt.DoMinMaxSlider(position, id, ref minValue, ref size, minLimit, maxLimit, minLimit, maxLimit, GUI.skin.horizontalSlider, EditorStyles.minMaxHorizontalSliderThumb, true);
      if (!EditorGUI.EndChangeCheck())
        return;
      maxValue = minValue + size;
    }

    private static int PopupInternal(Rect position, string label, int selectedIndex, string[] displayedOptions, GUIStyle style)
    {
      return EditorGUI.PopupInternal(position, EditorGUIUtility.TempContent(label), selectedIndex, EditorGUIUtility.TempContent(displayedOptions), style);
    }

    private static int PopupInternal(Rect position, GUIContent label, int selectedIndex, GUIContent[] displayedOptions, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_PopupHash, EditorGUIUtility.native, position);
      return EditorGUI.DoPopup(EditorGUI.PrefixLabel(position, controlId, label), controlId, selectedIndex, displayedOptions, style);
    }

    private static void Popup(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginChangeCheck();
      int num = EditorGUI.Popup(position, label, !property.hasMultipleDifferentValues ? property.enumValueIndex : -1, EditorGUIUtility.TempContent(property.enumDisplayNames));
      if (!EditorGUI.EndChangeCheck())
        return;
      property.enumValueIndex = num;
    }

    internal static void Popup(Rect position, SerializedProperty property, GUIContent[] displayedOptions, GUIContent label)
    {
      label = EditorGUI.BeginProperty(position, label, property);
      EditorGUI.BeginChangeCheck();
      int num = EditorGUI.Popup(position, label, !property.hasMultipleDifferentValues ? property.intValue : -1, displayedOptions);
      if (EditorGUI.EndChangeCheck())
        property.intValue = num;
      EditorGUI.EndProperty();
    }

    private static Enum EnumPopupInternal(Rect position, GUIContent label, Enum selected, GUIStyle style)
    {
      System.Type type = selected.GetType();
      if (!type.IsEnum)
        throw new Exception("parameter _enum must be of type System.Enum");
      Enum[] array = Enum.GetValues(type).Cast<Enum>().ToArray<Enum>();
      string[] names = Enum.GetNames(type);
      int selectedIndex = Array.IndexOf<Enum>(array, selected);
      int index = EditorGUI.Popup(position, label, selectedIndex, EditorGUIUtility.TempContent(((IEnumerable<string>) names).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>()), style);
      if (index < 0 || index >= names.Length)
        return selected;
      return array[index];
    }

    private static Enum EnumMaskPopupInternal(Rect position, GUIContent label, Enum selected, out int changedFlags, out bool changedToValue, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_MaskField, EditorGUIUtility.native, position);
      return EditorGUI.EnumMaskField(EditorGUI.PrefixLabel(position, controlId, label), selected, style, out changedFlags, out changedToValue);
    }

    private static int IntPopupInternal(Rect position, GUIContent label, int selectedValue, GUIContent[] displayedOptions, int[] optionValues, GUIStyle style)
    {
      int selectedIndex;
      if (optionValues != null)
      {
        selectedIndex = 0;
        while (selectedIndex < optionValues.Length && selectedValue != optionValues[selectedIndex])
          ++selectedIndex;
      }
      else
        selectedIndex = selectedValue;
      int index = EditorGUI.PopupInternal(position, label, selectedIndex, displayedOptions, style);
      if (optionValues == null)
        return index;
      if (index < 0 || index >= optionValues.Length)
        return selectedValue;
      return optionValues[index];
    }

    internal static void IntPopupInternal(Rect position, SerializedProperty property, GUIContent[] displayedOptions, int[] optionValues, GUIContent label)
    {
      label = EditorGUI.BeginProperty(position, label, property);
      EditorGUI.BeginChangeCheck();
      int num = EditorGUI.IntPopupInternal(position, label, property.intValue, displayedOptions, optionValues, EditorStyles.popup);
      if (EditorGUI.EndChangeCheck())
        property.intValue = num;
      EditorGUI.EndProperty();
    }

    internal static void SortingLayerField(Rect position, GUIContent label, SerializedProperty layerID, GUIStyle style, GUIStyle labelStyle)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_SortingLayerFieldHash, EditorGUIUtility.native, position);
      position = EditorGUI.PrefixLabel(position, controlId, label, labelStyle);
      Event current = Event.current;
      int selectedValueForControl = EditorGUI.PopupCallbackInfo.GetSelectedValueForControl(controlId, -1);
      if (selectedValueForControl != -1)
      {
        int[] sortingLayerUniqueIds = InternalEditorUtility.sortingLayerUniqueIDs;
        if (selectedValueForControl >= sortingLayerUniqueIds.Length)
        {
          ((TagManager) EditorApplication.tagManager).m_DefaultExpandedFoldout = "SortingLayers";
          Selection.activeObject = EditorApplication.tagManager;
        }
        else
          layerID.intValue = sortingLayerUniqueIds[selectedValueForControl];
      }
      if (current.type == EventType.MouseDown && position.Contains(current.mousePosition) || current.MainActionKeyForControl(controlId))
      {
        int[] sortingLayerUniqueIds = InternalEditorUtility.sortingLayerUniqueIDs;
        string[] sortingLayerNames = InternalEditorUtility.sortingLayerNames;
        int selected = 0;
        while (selected < sortingLayerUniqueIds.Length && sortingLayerUniqueIds[selected] != layerID.intValue)
          ++selected;
        ArrayUtility.Add<string>(ref sortingLayerNames, string.Empty);
        ArrayUtility.Add<string>(ref sortingLayerNames, "Add Sorting Layer...");
        EditorGUI.DoPopup(position, controlId, selected, EditorGUIUtility.TempContent(sortingLayerNames), style);
      }
      else
      {
        if (Event.current.type != EventType.Repaint)
          return;
        GUIContent content = !layerID.hasMultipleDifferentValues ? EditorGUIUtility.TempContent(InternalEditorUtility.GetSortingLayerNameFromUniqueID(layerID.intValue)) : EditorGUI.mixedValueContent;
        EditorGUI.showMixedValue = layerID.hasMultipleDifferentValues;
        EditorGUI.BeginHandleMixedValueContentColor();
        style.Draw(position, content, controlId, false);
        EditorGUI.EndHandleMixedValueContentColor();
        EditorGUI.showMixedValue = false;
      }
    }

    internal static int DoPopup(Rect position, int controlID, int selected, GUIContent[] popupValues, GUIStyle style)
    {
      selected = EditorGUI.PopupCallbackInfo.GetSelectedValueForControl(controlID, selected);
      GUIContent content = (GUIContent) null ?? (!EditorGUI.showMixedValue ? (selected < 0 || selected >= popupValues.Length ? GUIContent.none : popupValues[selected]) : EditorGUI.s_MixedValueContent);
      Event current = Event.current;
      EventType type = current.type;
      switch (type)
      {
        case EventType.KeyDown:
          if (current.MainActionKeyForControl(controlID))
          {
            if (Application.platform == RuntimePlatform.OSXEditor)
              position.y = (float) ((double) position.y - (double) (selected * 16) - 19.0);
            EditorGUI.PopupCallbackInfo.instance = new EditorGUI.PopupCallbackInfo(controlID);
            EditorUtility.DisplayCustomMenu(position, popupValues, !EditorGUI.showMixedValue ? selected : -1, new EditorUtility.SelectMenuItemFunction(EditorGUI.PopupCallbackInfo.instance.SetEnumValueDelegate), (object) null);
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          Font font = style.font;
          if ((bool) ((UnityEngine.Object) font) && EditorGUIUtility.GetBoldDefaultFont() && (UnityEngine.Object) font == (UnityEngine.Object) EditorStyles.miniFont)
            style.font = EditorStyles.miniBoldFont;
          EditorGUI.BeginHandleMixedValueContentColor();
          style.Draw(position, content, controlID, false);
          EditorGUI.EndHandleMixedValueContentColor();
          style.font = font;
          break;
        default:
          if (type == EventType.MouseDown && current.button == 0 && position.Contains(current.mousePosition))
          {
            if (Application.platform == RuntimePlatform.OSXEditor)
              position.y = (float) ((double) position.y - (double) (selected * 16) - 19.0);
            EditorGUI.PopupCallbackInfo.instance = new EditorGUI.PopupCallbackInfo(controlID);
            EditorUtility.DisplayCustomMenu(position, popupValues, !EditorGUI.showMixedValue ? selected : -1, new EditorUtility.SelectMenuItemFunction(EditorGUI.PopupCallbackInfo.instance.SetEnumValueDelegate), (object) null);
            GUIUtility.keyboardControl = controlID;
            current.Use();
            break;
          }
          break;
      }
      return selected;
    }

    internal static string TagFieldInternal(Rect position, string tag, GUIStyle style)
    {
      position = EditorGUI.IndentedRect(position);
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TagFieldHash, EditorGUIUtility.native, position);
      Event current = Event.current;
      int selectedValueForControl = EditorGUI.PopupCallbackInfo.GetSelectedValueForControl(controlId, -1);
      if (selectedValueForControl != -1)
      {
        string[] tags = InternalEditorUtility.tags;
        if (selectedValueForControl >= tags.Length)
        {
          ((TagManager) EditorApplication.tagManager).m_DefaultExpandedFoldout = "Tags";
          Selection.activeObject = EditorApplication.tagManager;
        }
        else
          tag = tags[selectedValueForControl];
      }
      if (current.type == EventType.MouseDown && position.Contains(current.mousePosition) || current.MainActionKeyForControl(controlId))
      {
        string[] tags = InternalEditorUtility.tags;
        int selected = 0;
        while (selected < tags.Length && !(tags[selected] == tag))
          ++selected;
        ArrayUtility.Add<string>(ref tags, string.Empty);
        ArrayUtility.Add<string>(ref tags, "Add Tag...");
        EditorGUI.DoPopup(position, controlId, selected, EditorGUIUtility.TempContent(tags), style);
        return tag;
      }
      if (Event.current.type == EventType.Repaint)
      {
        EditorGUI.BeginHandleMixedValueContentColor();
        style.Draw(position, !EditorGUI.showMixedValue ? EditorGUIUtility.TempContent(tag) : EditorGUI.s_MixedValueContent, controlId, false);
        EditorGUI.EndHandleMixedValueContentColor();
      }
      return tag;
    }

    internal static string TagFieldInternal(Rect position, GUIContent label, string tag, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TagFieldHash, EditorGUIUtility.native, position);
      position = EditorGUI.PrefixLabel(position, controlId, label);
      Event current = Event.current;
      int selectedValueForControl = EditorGUI.PopupCallbackInfo.GetSelectedValueForControl(controlId, -1);
      if (selectedValueForControl != -1)
      {
        string[] tags = InternalEditorUtility.tags;
        if (selectedValueForControl >= tags.Length)
        {
          ((TagManager) EditorApplication.tagManager).m_DefaultExpandedFoldout = "Tags";
          Selection.activeObject = EditorApplication.tagManager;
        }
        else
          tag = tags[selectedValueForControl];
      }
      if (current.type == EventType.MouseDown && position.Contains(current.mousePosition) || current.MainActionKeyForControl(controlId))
      {
        string[] tags = InternalEditorUtility.tags;
        int selected = 0;
        while (selected < tags.Length && !(tags[selected] == tag))
          ++selected;
        ArrayUtility.Add<string>(ref tags, string.Empty);
        ArrayUtility.Add<string>(ref tags, "Add Tag...");
        EditorGUI.DoPopup(position, controlId, selected, EditorGUIUtility.TempContent(tags), style);
        return tag;
      }
      if (Event.current.type == EventType.Repaint)
        style.Draw(position, EditorGUIUtility.TempContent(tag), controlId, false);
      return tag;
    }

    internal static int LayerFieldInternal(Rect position, GUIContent label, int layer, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TagFieldHash, EditorGUIUtility.native, position);
      position = EditorGUI.PrefixLabel(position, controlId, label);
      Event current = Event.current;
      bool changed = GUI.changed;
      int selectedValueForControl = EditorGUI.PopupCallbackInfo.GetSelectedValueForControl(controlId, -1);
      if (selectedValueForControl != -1)
      {
        if (selectedValueForControl >= InternalEditorUtility.layers.Length)
        {
          ((TagManager) EditorApplication.tagManager).m_DefaultExpandedFoldout = "Layers";
          Selection.activeObject = EditorApplication.tagManager;
          GUI.changed = changed;
        }
        else
        {
          int num = 0;
          for (int layer1 = 0; layer1 < 32; ++layer1)
          {
            if (InternalEditorUtility.GetLayerName(layer1).Length != 0)
            {
              if (num == selectedValueForControl)
              {
                layer = layer1;
                break;
              }
              ++num;
            }
          }
        }
      }
      if (current.type == EventType.MouseDown && position.Contains(current.mousePosition) || current.MainActionKeyForControl(controlId))
      {
        int selected = 0;
        for (int layer1 = 0; layer1 < 32; ++layer1)
        {
          if (InternalEditorUtility.GetLayerName(layer1).Length != 0)
          {
            if (layer1 != layer)
              ++selected;
            else
              break;
          }
        }
        string[] layers = InternalEditorUtility.layers;
        ArrayUtility.Add<string>(ref layers, string.Empty);
        ArrayUtility.Add<string>(ref layers, "Add Layer...");
        EditorGUI.DoPopup(position, controlId, selected, EditorGUIUtility.TempContent(layers), style);
        Event.current.Use();
        return layer;
      }
      if (current.type == EventType.Repaint)
        style.Draw(position, EditorGUIUtility.TempContent(InternalEditorUtility.GetLayerName(layer)), controlId, false);
      return layer;
    }

    internal static int MaskFieldInternal(Rect position, GUIContent label, int mask, string[] displayedOptions, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_MaskField, EditorGUIUtility.native, position);
      position = EditorGUI.PrefixLabel(position, controlId, label);
      return MaskFieldGUI.DoMaskField(position, controlId, mask, displayedOptions, style);
    }

    internal static int MaskFieldInternal(Rect position, int mask, string[] displayedOptions, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_MaskField, EditorGUIUtility.native, position);
      return MaskFieldGUI.DoMaskField(EditorGUI.IndentedRect(position), controlId, mask, displayedOptions, style);
    }

    internal static Enum EnumMaskFieldInternal(Rect position, GUIContent label, Enum enumValue, GUIStyle style)
    {
      System.Type type = enumValue.GetType();
      if (!type.IsEnum)
        throw new Exception("parameter _enum must be of type System.Enum");
      int controlId = GUIUtility.GetControlID(EditorGUI.s_MaskField, EditorGUIUtility.native, position);
      Rect position1 = EditorGUI.PrefixLabel(position, controlId, label);
      position.xMax = position1.x;
      string[] array = ((IEnumerable<string>) Enum.GetNames(enumValue.GetType())).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>();
      int num = MaskFieldGUI.DoMaskField(position1, controlId, Convert.ToInt32((object) enumValue), array, style);
      return EditorGUI.EnumFlagsToInt(type, num);
    }

    internal static Enum EnumMaskFieldInternal(Rect position, Enum enumValue, GUIStyle style)
    {
      System.Type type = enumValue.GetType();
      if (!type.IsEnum)
        throw new Exception("parameter _enum must be of type System.Enum");
      string[] array = ((IEnumerable<string>) Enum.GetNames(enumValue.GetType())).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>();
      int num = MaskFieldGUI.DoMaskField(EditorGUI.IndentedRect(position), GUIUtility.GetControlID(EditorGUI.s_MaskField, EditorGUIUtility.native, position), Convert.ToInt32((object) enumValue), array, style);
      return EditorGUI.EnumFlagsToInt(type, num);
    }

    internal static Enum EnumMaskField(Rect position, Enum enumValue, GUIStyle style, out int changedFlags, out bool changedToValue)
    {
      System.Type type = enumValue.GetType();
      if (!type.IsEnum)
        throw new Exception("parameter _enum must be of type System.Enum");
      string[] array = ((IEnumerable<string>) Enum.GetNames(enumValue.GetType())).Select<string, string>((Func<string, string>) (x => ObjectNames.NicifyVariableName(x))).ToArray<string>();
      int num = MaskFieldGUI.DoMaskField(EditorGUI.IndentedRect(position), GUIUtility.GetControlID(EditorGUI.s_MaskField, EditorGUIUtility.native, position), Convert.ToInt32((object) enumValue), array, style, out changedFlags, out changedToValue);
      return EditorGUI.EnumFlagsToInt(type, num);
    }

    public static void ObjectField(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.ObjectField(position, property, (System.Type) null, label, EditorStyles.objectField);
    }

    public static void ObjectField(Rect position, SerializedProperty property, System.Type objType, GUIContent label)
    {
      EditorGUI.ObjectField(position, property, objType, label, EditorStyles.objectField);
    }

    internal static void ObjectField(Rect position, SerializedProperty property, System.Type objType, GUIContent label, GUIStyle style)
    {
      label = EditorGUI.BeginProperty(position, label, property);
      EditorGUI.ObjectFieldInternal(position, property, objType, label, style);
      EditorGUI.EndProperty();
    }

    private static void ObjectFieldInternal(Rect position, SerializedProperty property, System.Type objType, GUIContent label, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_PPtrHash, EditorGUIUtility.native, position);
      position = EditorGUI.PrefixLabel(position, controlId, label);
      bool allowSceneObjects = false;
      if (property != null)
      {
        UnityEngine.Object targetObject = property.serializedObject.targetObject;
        if (targetObject != (UnityEngine.Object) null && !EditorUtility.IsPersistent(targetObject))
          allowSceneObjects = true;
      }
      EditorGUI.DoObjectField(position, position, controlId, (UnityEngine.Object) null, (System.Type) null, property, (EditorGUI.ObjectFieldValidator) null, allowSceneObjects, style);
    }

    /// <summary>
    ///   <para>Make an object field. You can assign objects either by drag and drop objects or by selecting an object using the Object Picker.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="obj">The object the field shows.</param>
    /// <param name="objType">The type of the objects that can be assigned.</param>
    /// <param name="allowSceneObjects">Allow assigning scene objects. See Description for more info.</param>
    /// <returns>
    ///   <para>The object that has been set by the user.</para>
    /// </returns>
    public static UnityEngine.Object ObjectField(Rect position, UnityEngine.Object obj, System.Type objType, bool allowSceneObjects)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ObjectFieldHash, EditorGUIUtility.native, position);
      return EditorGUI.DoObjectField(EditorGUI.IndentedRect(position), EditorGUI.IndentedRect(position), controlId, obj, objType, (SerializedProperty) null, (EditorGUI.ObjectFieldValidator) null, allowSceneObjects);
    }

    /// <summary>
    ///   <para>Make an object field. You can assign objects either by drag and drop objects or by selecting an object using the Object Picker.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="obj">The object the field shows.</param>
    /// <param name="objType">The type of the objects that can be assigned.</param>
    /// <param name="allowSceneObjects">Allow assigning scene objects. See Description for more info.</param>
    /// <returns>
    ///   <para>The object that has been set by the user.</para>
    /// </returns>
    [Obsolete("Check the docs for the usage of the new parameter 'allowSceneObjects'.")]
    public static UnityEngine.Object ObjectField(Rect position, UnityEngine.Object obj, System.Type objType)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ObjectFieldHash, EditorGUIUtility.native, position);
      return EditorGUI.DoObjectField(position, position, controlId, obj, objType, (SerializedProperty) null, (EditorGUI.ObjectFieldValidator) null, true);
    }

    /// <summary>
    ///   <para>Make an object field. You can assign objects either by drag and drop objects or by selecting an object using the Object Picker.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="obj">The object the field shows.</param>
    /// <param name="objType">The type of the objects that can be assigned.</param>
    /// <param name="allowSceneObjects">Allow assigning scene objects. See Description for more info.</param>
    /// <returns>
    ///   <para>The object that has been set by the user.</para>
    /// </returns>
    public static UnityEngine.Object ObjectField(Rect position, string label, UnityEngine.Object obj, System.Type objType, bool allowSceneObjects)
    {
      return EditorGUI.ObjectField(position, EditorGUIUtility.TempContent(label), obj, objType, allowSceneObjects);
    }

    /// <summary>
    ///   <para>Make an object field. You can assign objects either by drag and drop objects or by selecting an object using the Object Picker.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="obj">The object the field shows.</param>
    /// <param name="objType">The type of the objects that can be assigned.</param>
    /// <param name="allowSceneObjects">Allow assigning scene objects. See Description for more info.</param>
    /// <returns>
    ///   <para>The object that has been set by the user.</para>
    /// </returns>
    [Obsolete("Check the docs for the usage of the new parameter 'allowSceneObjects'.")]
    public static UnityEngine.Object ObjectField(Rect position, string label, UnityEngine.Object obj, System.Type objType)
    {
      return EditorGUI.ObjectField(position, EditorGUIUtility.TempContent(label), obj, objType, true);
    }

    /// <summary>
    ///   <para>Make an object field. You can assign objects either by drag and drop objects or by selecting an object using the Object Picker.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="obj">The object the field shows.</param>
    /// <param name="objType">The type of the objects that can be assigned.</param>
    /// <param name="allowSceneObjects">Allow assigning scene objects. See Description for more info.</param>
    /// <returns>
    ///   <para>The object that has been set by the user.</para>
    /// </returns>
    public static UnityEngine.Object ObjectField(Rect position, GUIContent label, UnityEngine.Object obj, System.Type objType, bool allowSceneObjects)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ObjectFieldHash, EditorGUIUtility.native, position);
      position = EditorGUI.PrefixLabel(position, controlId, label);
      if (EditorGUIUtility.HasObjectThumbnail(objType) && (double) position.height > 16.0)
      {
        float num = Mathf.Min(position.width, position.height);
        position.height = num;
        position.xMin = position.xMax - num;
      }
      return EditorGUI.DoObjectField(position, position, controlId, obj, objType, (SerializedProperty) null, (EditorGUI.ObjectFieldValidator) null, allowSceneObjects);
    }

    internal static void GetRectsForMiniThumbnailField(Rect position, out Rect thumbRect, out Rect labelRect)
    {
      thumbRect = EditorGUI.IndentedRect(position);
      --thumbRect.y;
      thumbRect.height = 18f;
      thumbRect.width = 32f;
      float x = thumbRect.x + 30f;
      labelRect = new Rect(x, position.y, thumbRect.x + EditorGUIUtility.labelWidth - x, position.height);
    }

    internal static UnityEngine.Object MiniThumbnailObjectField(Rect position, GUIContent label, UnityEngine.Object obj, System.Type objType, EditorGUI.ObjectFieldValidator validator)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ObjectFieldHash, EditorGUIUtility.native, position);
      Rect thumbRect;
      Rect labelRect;
      EditorGUI.GetRectsForMiniThumbnailField(position, out thumbRect, out labelRect);
      EditorGUI.HandlePrefixLabel(position, labelRect, label, controlId, EditorStyles.label);
      return EditorGUI.DoObjectField(thumbRect, thumbRect, controlId, obj, objType, (SerializedProperty) null, validator, false);
    }

    /// <summary>
    ///   <para>Make an object field. You can assign objects either by drag and drop objects or by selecting an object using the Object Picker.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="obj">The object the field shows.</param>
    /// <param name="objType">The type of the objects that can be assigned.</param>
    /// <param name="allowSceneObjects">Allow assigning scene objects. See Description for more info.</param>
    /// <returns>
    ///   <para>The object that has been set by the user.</para>
    /// </returns>
    [Obsolete("Check the docs for the usage of the new parameter 'allowSceneObjects'.")]
    public static UnityEngine.Object ObjectField(Rect position, GUIContent label, UnityEngine.Object obj, System.Type objType)
    {
      return EditorGUI.ObjectField(position, label, obj, objType, true);
    }

    internal static UnityEngine.Object ValidateObjectFieldAssignment(UnityEngine.Object[] references, System.Type objType, SerializedProperty property)
    {
      if (references.Length > 0)
      {
        bool flag1 = DragAndDrop.objectReferences.Length > 0;
        bool flag2 = references[0] != (UnityEngine.Object) null && references[0].GetType() == typeof (Texture2D);
        if (objType == typeof (Sprite) && flag2 && flag1)
          return (UnityEngine.Object) SpriteUtility.TextureToSprite(references[0] as Texture2D);
        if (property != null)
        {
          if (references[0] != (UnityEngine.Object) null && property.ValidateObjectReferenceValue(references[0]))
            return references[0];
          if ((property.type == "PPtr<Sprite>" || property.type == "PPtr<$Sprite>" || property.type == "vector") && (flag2 && flag1))
            return (UnityEngine.Object) SpriteUtility.TextureToSprite(references[0] as Texture2D);
        }
        else
        {
          if (references[0] != (UnityEngine.Object) null && references[0].GetType() == typeof (GameObject) && typeof (Component).IsAssignableFrom(objType))
            references = (UnityEngine.Object[]) ((GameObject) references[0]).GetComponents(typeof (Component));
          foreach (UnityEngine.Object reference in references)
          {
            if (reference != (UnityEngine.Object) null && objType.IsAssignableFrom(reference.GetType()))
              return reference;
          }
        }
      }
      return (UnityEngine.Object) null;
    }

    private static UnityEngine.Object HandleTextureToSprite(Texture2D tex)
    {
      UnityEngine.Object[] objectArray = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) tex));
      for (int index = 0; index < objectArray.Length; ++index)
      {
        if (objectArray[index].GetType() == typeof (Sprite))
          return objectArray[index];
      }
      return (UnityEngine.Object) tex;
    }

    public static Rect IndentedRect(Rect source)
    {
      float indent = EditorGUI.indent;
      return new Rect(source.x + indent, source.y, source.width - indent, source.height);
    }

    /// <summary>
    ///   <para>Make an X &amp; Y field for entering a Vector2.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Vector2 Vector2Field(Rect position, string label, Vector2 value)
    {
      return EditorGUI.Vector2Field(position, EditorGUIUtility.TempContent(label), value);
    }

    /// <summary>
    ///   <para>Make an X &amp; Y field for entering a Vector2.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Vector2 Vector2Field(Rect position, GUIContent label, Vector2 value)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      position = EditorGUI.MultiFieldPrefixLabel(position, controlId, label, 2);
      position.height = 16f;
      return EditorGUI.Vector2Field(position, value);
    }

    private static Vector2 Vector2Field(Rect position, Vector2 value)
    {
      EditorGUI.s_Vector2Floats[0] = value.x;
      EditorGUI.s_Vector2Floats[1] = value.y;
      position.height = 16f;
      EditorGUI.BeginChangeCheck();
      EditorGUI.MultiFloatField(position, EditorGUI.s_XYLabels, EditorGUI.s_Vector2Floats);
      if (EditorGUI.EndChangeCheck())
      {
        value.x = EditorGUI.s_Vector2Floats[0];
        value.y = EditorGUI.s_Vector2Floats[1];
      }
      return value;
    }

    /// <summary>
    ///   <para>Make an X, Y &amp; Z field for entering a Vector3.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Vector3 Vector3Field(Rect position, string label, Vector3 value)
    {
      return EditorGUI.Vector3Field(position, EditorGUIUtility.TempContent(label), value);
    }

    /// <summary>
    ///   <para>Make an X, Y &amp; Z field for entering a Vector3.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Vector3 Vector3Field(Rect position, GUIContent label, Vector3 value)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      position = EditorGUI.MultiFieldPrefixLabel(position, controlId, label, 3);
      position.height = 16f;
      return EditorGUI.Vector3Field(position, value);
    }

    private static Vector3 Vector3Field(Rect position, Vector3 value)
    {
      EditorGUI.s_Vector3Floats[0] = value.x;
      EditorGUI.s_Vector3Floats[1] = value.y;
      EditorGUI.s_Vector3Floats[2] = value.z;
      position.height = 16f;
      EditorGUI.BeginChangeCheck();
      EditorGUI.MultiFloatField(position, EditorGUI.s_XYZLabels, EditorGUI.s_Vector3Floats);
      if (EditorGUI.EndChangeCheck())
      {
        value.x = EditorGUI.s_Vector3Floats[0];
        value.y = EditorGUI.s_Vector3Floats[1];
        value.z = EditorGUI.s_Vector3Floats[2];
      }
      return value;
    }

    private static void Vector2Field(Rect position, SerializedProperty property, GUIContent label)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      position = EditorGUI.MultiFieldPrefixLabel(position, controlId, label, 2);
      position.height = 16f;
      SerializedProperty valuesIterator = property.Copy();
      valuesIterator.NextVisible(true);
      EditorGUI.MultiPropertyField(position, EditorGUI.s_XYLabels, valuesIterator);
    }

    private static void Vector3Field(Rect position, SerializedProperty property, GUIContent label)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      position = EditorGUI.MultiFieldPrefixLabel(position, controlId, label, 3);
      position.height = 16f;
      SerializedProperty valuesIterator = property.Copy();
      valuesIterator.NextVisible(true);
      EditorGUI.MultiPropertyField(position, EditorGUI.s_XYZLabels, valuesIterator);
    }

    private static void Vector4Field(Rect position, SerializedProperty property, GUIContent label)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      position = EditorGUI.MultiFieldPrefixLabel(position, controlId, label, 4);
      position.height = 16f;
      SerializedProperty valuesIterator = property.Copy();
      valuesIterator.NextVisible(true);
      EditorGUI.MultiPropertyField(position, EditorGUI.s_XYZWLabels, valuesIterator);
    }

    /// <summary>
    ///   <para>Make an X, Y, Z &amp; W field for entering a Vector4.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Vector4 Vector4Field(Rect position, string label, Vector4 value)
    {
      EditorGUI.s_Vector4Floats[0] = value.x;
      EditorGUI.s_Vector4Floats[1] = value.y;
      EditorGUI.s_Vector4Floats[2] = value.z;
      EditorGUI.s_Vector4Floats[3] = value.w;
      position.height = 16f;
      GUI.Label(EditorGUI.IndentedRect(position), label, EditorStyles.label);
      position.y += 16f;
      EditorGUI.BeginChangeCheck();
      ++EditorGUI.indentLevel;
      EditorGUI.MultiFloatField(position, EditorGUI.s_XYZWLabels, EditorGUI.s_Vector4Floats);
      --EditorGUI.indentLevel;
      if (EditorGUI.EndChangeCheck())
      {
        value.x = EditorGUI.s_Vector4Floats[0];
        value.y = EditorGUI.s_Vector4Floats[1];
        value.z = EditorGUI.s_Vector4Floats[2];
        value.w = EditorGUI.s_Vector4Floats[3];
      }
      return value;
    }

    /// <summary>
    ///   <para>Make an X, Y, W &amp; H field for entering a Rect.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Rect RectField(Rect position, Rect value)
    {
      position.height = 16f;
      EditorGUI.s_Vector2Floats[0] = value.x;
      EditorGUI.s_Vector2Floats[1] = value.y;
      EditorGUI.BeginChangeCheck();
      EditorGUI.MultiFloatField(position, EditorGUI.s_XYLabels, EditorGUI.s_Vector2Floats);
      if (EditorGUI.EndChangeCheck())
      {
        value.x = EditorGUI.s_Vector2Floats[0];
        value.y = EditorGUI.s_Vector2Floats[1];
      }
      position.y += 16f;
      EditorGUI.s_Vector2Floats[0] = value.width;
      EditorGUI.s_Vector2Floats[1] = value.height;
      EditorGUI.BeginChangeCheck();
      EditorGUI.MultiFloatField(position, EditorGUI.s_WHLabels, EditorGUI.s_Vector2Floats);
      if (EditorGUI.EndChangeCheck())
      {
        value.width = EditorGUI.s_Vector2Floats[0];
        value.height = EditorGUI.s_Vector2Floats[1];
      }
      return value;
    }

    /// <summary>
    ///   <para>Make an X, Y, W &amp; H field for entering a Rect.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Rect RectField(Rect position, string label, Rect value)
    {
      return EditorGUI.RectField(position, EditorGUIUtility.TempContent(label), value);
    }

    /// <summary>
    ///   <para>Make an X, Y, W &amp; H field for entering a Rect.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Rect RectField(Rect position, GUIContent label, Rect value)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      position = EditorGUI.MultiFieldPrefixLabel(position, controlId, label, 2);
      position.height = 16f;
      return EditorGUI.RectField(position, value);
    }

    private static void RectField(Rect position, SerializedProperty property, GUIContent label)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      position = EditorGUI.MultiFieldPrefixLabel(position, controlId, label, 2);
      position.height = 16f;
      SerializedProperty valuesIterator = property.Copy();
      valuesIterator.NextVisible(true);
      EditorGUI.MultiPropertyField(position, EditorGUI.s_XYLabels, valuesIterator);
      position.y += 16f;
      EditorGUI.MultiPropertyField(position, EditorGUI.s_WHLabels, valuesIterator);
    }

    private static Rect DrawBoundsFieldLabelsAndAdjustPositionForValues(Rect position, bool drawOutside)
    {
      if (drawOutside)
        position.xMin -= 53f;
      GUI.Label(EditorGUI.IndentedRect(position), "Center:", EditorStyles.label);
      position.y += 16f;
      GUI.Label(EditorGUI.IndentedRect(position), "Extents:", EditorStyles.label);
      position.y -= 16f;
      position.xMin += 53f;
      return position;
    }

    /// <summary>
    ///   <para>Make Center &amp; Extents field for entering a Bounds.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Bounds BoundsField(Rect position, Bounds value)
    {
      return EditorGUI.BoundsField(position, GUIContent.none, value);
    }

    /// <summary>
    ///   <para>Make Center &amp; Extents field for entering a Bounds.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Bounds BoundsField(Rect position, GUIContent label, Bounds value)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      position = EditorGUI.MultiFieldPrefixLabel(position, controlId, label, 3);
      if (EditorGUIUtility.wideMode && EditorGUI.LabelHasContent(label))
        position.y += 16f;
      position.height = 16f;
      position = EditorGUI.DrawBoundsFieldLabelsAndAdjustPositionForValues(position, EditorGUIUtility.wideMode && EditorGUI.LabelHasContent(label));
      value.center = EditorGUI.Vector3Field(position, value.center);
      position.y += 16f;
      value.extents = EditorGUI.Vector3Field(position, value.extents);
      return value;
    }

    private static void BoundsField(Rect position, SerializedProperty property, GUIContent label)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      position = EditorGUI.MultiFieldPrefixLabel(position, controlId, label, 3);
      if (EditorGUIUtility.wideMode && EditorGUI.LabelHasContent(label))
        position.y += 16f;
      position.height = 16f;
      position = EditorGUI.DrawBoundsFieldLabelsAndAdjustPositionForValues(position, EditorGUIUtility.wideMode && EditorGUI.LabelHasContent(label));
      SerializedProperty valuesIterator = property.Copy();
      valuesIterator.NextVisible(true);
      valuesIterator.NextVisible(true);
      EditorGUI.MultiPropertyField(position, EditorGUI.s_XYZLabels, valuesIterator);
      valuesIterator.NextVisible(true);
      position.y += 16f;
      EditorGUI.MultiPropertyField(position, EditorGUI.s_XYZLabels, valuesIterator);
    }

    /// <summary>
    ///   <para>Make a multi-control with text fields for entering multiple floats in the same line.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the float field.</param>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="subLabels">Array with small labels to show in front of each float field. There is room for one letter per field only.</param>
    /// <param name="values">Array with the values to edit.</param>
    public static void MultiFloatField(Rect position, GUIContent label, GUIContent[] subLabels, float[] values)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      position = EditorGUI.MultiFieldPrefixLabel(position, controlId, label, subLabels.Length);
      position.height = 16f;
      EditorGUI.MultiFloatField(position, subLabels, values);
    }

    /// <summary>
    ///   <para>Make a multi-control with text fields for entering multiple floats in the same line.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the float field.</param>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="subLabels">Array with small labels to show in front of each float field. There is room for one letter per field only.</param>
    /// <param name="values">Array with the values to edit.</param>
    public static void MultiFloatField(Rect position, GUIContent[] subLabels, float[] values)
    {
      EditorGUI.MultiFloatField(position, subLabels, values, 13f);
    }

    internal static void MultiFloatField(Rect position, GUIContent[] subLabels, float[] values, float labelWidth)
    {
      int length = values.Length;
      float num = (position.width - (float) (length - 1) * 2f) / (float) length;
      Rect position1 = new Rect(position);
      position1.width = num;
      float labelWidth1 = EditorGUIUtility.labelWidth;
      int indentLevel = EditorGUI.indentLevel;
      EditorGUIUtility.labelWidth = labelWidth;
      EditorGUI.indentLevel = 0;
      for (int index = 0; index < values.Length; ++index)
      {
        values[index] = EditorGUI.FloatField(position1, subLabels[index], values[index]);
        position1.x += num + 2f;
      }
      EditorGUIUtility.labelWidth = labelWidth1;
      EditorGUI.indentLevel = indentLevel;
    }

    /// <summary>
    ///   <para>Make a multi-control with several property fields in the same line.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the multi-property field.</param>
    /// <param name="valuesIterator">The SerializedProperty of the first property to make a control for.</param>
    /// <param name="label">Optional label to use. If not specified the label of the property itself is used. Use GUIContent.none to not display a label at all.</param>
    /// <param name="subLabels">Array with small labels to show in front of each float field. There is room for one letter per field only.</param>
    public static void MultiPropertyField(Rect position, GUIContent[] subLabels, SerializedProperty valuesIterator, GUIContent label)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      position = EditorGUI.MultiFieldPrefixLabel(position, controlId, label, subLabels.Length);
      position.height = 16f;
      EditorGUI.MultiPropertyField(position, subLabels, valuesIterator);
    }

    /// <summary>
    ///   <para>Make a multi-control with several property fields in the same line.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the multi-property field.</param>
    /// <param name="valuesIterator">The SerializedProperty of the first property to make a control for.</param>
    /// <param name="label">Optional label to use. If not specified the label of the property itself is used. Use GUIContent.none to not display a label at all.</param>
    /// <param name="subLabels">Array with small labels to show in front of each float field. There is room for one letter per field only.</param>
    public static void MultiPropertyField(Rect position, GUIContent[] subLabels, SerializedProperty valuesIterator)
    {
      EditorGUI.MultiPropertyField(position, subLabels, valuesIterator, 13f, (bool[]) null);
    }

    internal static void MultiPropertyField(Rect position, GUIContent[] subLabels, SerializedProperty valuesIterator, float labelWidth, bool[] disabledMask)
    {
      int length = subLabels.Length;
      float num = (position.width - (float) (length - 1) * 2f) / (float) length;
      Rect position1 = new Rect(position);
      position1.width = num;
      float labelWidth1 = EditorGUIUtility.labelWidth;
      int indentLevel = EditorGUI.indentLevel;
      EditorGUIUtility.labelWidth = labelWidth;
      EditorGUI.indentLevel = 0;
      for (int index = 0; index < subLabels.Length; ++index)
      {
        if (disabledMask != null)
          EditorGUI.BeginDisabledGroup(disabledMask[index]);
        EditorGUI.PropertyField(position1, valuesIterator, subLabels[index]);
        if (disabledMask != null)
          EditorGUI.EndDisabledGroup();
        position1.x += num + 2f;
        valuesIterator.NextVisible(false);
      }
      EditorGUIUtility.labelWidth = labelWidth1;
      EditorGUI.indentLevel = indentLevel;
    }

    internal static void PropertiesField(Rect position, GUIContent label, SerializedProperty[] properties, GUIContent[] propertyLabels, float propertyLabelsWidth)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      Rect position1 = EditorGUI.PrefixLabel(position, controlId, label);
      position1.height = 16f;
      float labelWidth = EditorGUIUtility.labelWidth;
      int indentLevel = EditorGUI.indentLevel;
      EditorGUIUtility.labelWidth = propertyLabelsWidth;
      EditorGUI.indentLevel = 0;
      for (int index = 0; index < properties.Length; ++index)
      {
        EditorGUI.PropertyField(position1, properties[index], propertyLabels[index]);
        position1.y += 16f;
      }
      EditorGUI.indentLevel = indentLevel;
      EditorGUIUtility.labelWidth = labelWidth;
    }

    internal static int CycleButton(Rect position, int selected, GUIContent[] options, GUIStyle style)
    {
      if (selected >= options.Length || selected < 0)
      {
        selected = 0;
        GUI.changed = true;
      }
      if (GUI.Button(position, options[selected], style))
      {
        ++selected;
        GUI.changed = true;
        if (selected >= options.Length)
          selected = 0;
      }
      return selected;
    }

    /// <summary>
    ///   <para>Make a field for selecting a Color.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The color to edit.</param>
    /// <param name="showEyedropper">If true, the color picker should show the eyedropper control. If false, don't show it.</param>
    /// <param name="showAlpha">If true, allow the user to set an alpha value for the color. If false, hide the alpha component.</param>
    /// <param name="hdr">If true, treat the color as an HDR value. If false, treat it as a standard LDR value.</param>
    /// <param name="hdrConfig">An object that sets the presentation parameters for an HDR color. If not using an HDR color, set this to null.</param>
    /// <returns>
    ///   <para>The color selected by the user.</para>
    /// </returns>
    public static Color ColorField(Rect position, Color value)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ColorHash, EditorGUIUtility.native, position);
      return EditorGUI.DoColorField(EditorGUI.IndentedRect(position), controlId, value, true, true, false, (ColorPickerHDRConfig) null);
    }

    internal static Color ColorField(Rect position, Color value, bool showEyedropper, bool showAlpha)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ColorHash, EditorGUIUtility.native, position);
      return EditorGUI.DoColorField(position, controlId, value, showEyedropper, showAlpha, false, (ColorPickerHDRConfig) null);
    }

    /// <summary>
    ///   <para>Make a field for selecting a Color.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The color to edit.</param>
    /// <param name="showEyedropper">If true, the color picker should show the eyedropper control. If false, don't show it.</param>
    /// <param name="showAlpha">If true, allow the user to set an alpha value for the color. If false, hide the alpha component.</param>
    /// <param name="hdr">If true, treat the color as an HDR value. If false, treat it as a standard LDR value.</param>
    /// <param name="hdrConfig">An object that sets the presentation parameters for an HDR color. If not using an HDR color, set this to null.</param>
    /// <returns>
    ///   <para>The color selected by the user.</para>
    /// </returns>
    public static Color ColorField(Rect position, string label, Color value)
    {
      return EditorGUI.ColorField(position, EditorGUIUtility.TempContent(label), value);
    }

    /// <summary>
    ///   <para>Make a field for selecting a Color.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The color to edit.</param>
    /// <param name="showEyedropper">If true, the color picker should show the eyedropper control. If false, don't show it.</param>
    /// <param name="showAlpha">If true, allow the user to set an alpha value for the color. If false, hide the alpha component.</param>
    /// <param name="hdr">If true, treat the color as an HDR value. If false, treat it as a standard LDR value.</param>
    /// <param name="hdrConfig">An object that sets the presentation parameters for an HDR color. If not using an HDR color, set this to null.</param>
    /// <returns>
    ///   <para>The color selected by the user.</para>
    /// </returns>
    public static Color ColorField(Rect position, GUIContent label, Color value)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ColorHash, EditorGUIUtility.native, position);
      return EditorGUI.DoColorField(EditorGUI.PrefixLabel(position, controlId, label), controlId, value, true, true, false, (ColorPickerHDRConfig) null);
    }

    internal static Color ColorField(Rect position, GUIContent label, Color value, bool showEyedropper, bool showAlpha)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ColorHash, EditorGUIUtility.native, position);
      return EditorGUI.DoColorField(EditorGUI.PrefixLabel(position, controlId, label), controlId, value, showEyedropper, showAlpha, false, (ColorPickerHDRConfig) null);
    }

    /// <summary>
    ///   <para>Make a field for selecting a Color.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The color to edit.</param>
    /// <param name="showEyedropper">If true, the color picker should show the eyedropper control. If false, don't show it.</param>
    /// <param name="showAlpha">If true, allow the user to set an alpha value for the color. If false, hide the alpha component.</param>
    /// <param name="hdr">If true, treat the color as an HDR value. If false, treat it as a standard LDR value.</param>
    /// <param name="hdrConfig">An object that sets the presentation parameters for an HDR color. If not using an HDR color, set this to null.</param>
    /// <returns>
    ///   <para>The color selected by the user.</para>
    /// </returns>
    public static Color ColorField(Rect position, GUIContent label, Color value, bool showEyedropper, bool showAlpha, bool hdr, ColorPickerHDRConfig hdrConfig)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_ColorHash, EditorGUIUtility.native, position);
      return EditorGUI.DoColorField(EditorGUI.PrefixLabel(position, controlId, label), controlId, value, showEyedropper, showAlpha, hdr, hdrConfig);
    }

    private static Color DoColorField(Rect position, int id, Color value, bool showEyedropper, bool showAlpha, bool hdr, ColorPickerHDRConfig hdrConfig)
    {
      Event current = Event.current;
      GUIStyle colorField = EditorStyles.colorField;
      Color color1 = value;
      value = !EditorGUI.showMixedValue ? value : Color.white;
      EventType typeForControl = current.GetTypeForControl(id);
      switch (typeForControl)
      {
        case EventType.KeyDown:
          if (current.MainActionKeyForControl(id))
          {
            Event.current.Use();
            EditorGUI.showMixedValue = false;
            ColorPicker.Show(GUIView.current, value, showAlpha, hdr, hdrConfig);
            GUIUtility.ExitGUI();
            break;
          }
          break;
        case EventType.Repaint:
          Rect position1 = !showEyedropper ? position : colorField.padding.Remove(position);
          if (showEyedropper && EditorGUI.s_ColorPickID == id)
          {
            Color pickedColor = EyeDropper.GetPickedColor();
            pickedColor.a = value.a;
            EditorGUIUtility.DrawColorSwatch(position1, pickedColor, showAlpha, hdr);
          }
          else
            EditorGUIUtility.DrawColorSwatch(position1, value, showAlpha, hdr);
          if (showEyedropper)
          {
            colorField.Draw(position, GUIContent.none, id);
            break;
          }
          EditorStyles.colorPickerBox.Draw(position, GUIContent.none, id);
          break;
        default:
          if (typeForControl != EventType.ValidateCommand)
          {
            if (typeForControl != EventType.ExecuteCommand)
            {
              if (typeForControl == EventType.MouseDown)
              {
                if (showEyedropper)
                  position.width -= 20f;
                if (position.Contains(current.mousePosition))
                {
                  switch (current.button)
                  {
                    case 0:
                      GUIUtility.keyboardControl = id;
                      EditorGUI.showMixedValue = false;
                      ColorPicker.Show(GUIView.current, value, showAlpha, hdr, hdrConfig);
                      GUIUtility.ExitGUI();
                      break;
                    case 1:
                      GUIUtility.keyboardControl = id;
                      string[] options1 = new string[2]
                      {
                        "Copy",
                        "Paste"
                      };
                      bool[] enabled = new bool[2]
                      {
                        true,
                        (ColorClipboard.HasColor() ? 1 : 0) != 0
                      };
                      EditorUtility.DisplayCustomMenu(position, options1, enabled, (int[]) null, (EditorUtility.SelectMenuItemFunction) ((data, options, selected) =>
                      {
                        if (selected == 0)
                        {
                          GUIView.current.SendEvent(EditorGUIUtility.CommandEvent("Copy"));
                        }
                        else
                        {
                          if (selected != 1)
                            return;
                          GUIView.current.SendEvent(EditorGUIUtility.CommandEvent("Paste"));
                        }
                      }), (object) null);
                      return color1;
                  }
                }
                if (showEyedropper)
                {
                  position.width += 20f;
                  if (position.Contains(current.mousePosition))
                  {
                    GUIUtility.keyboardControl = id;
                    EyeDropper.Start(GUIView.current);
                    EditorGUI.s_ColorPickID = id;
                    GUIUtility.ExitGUI();
                    break;
                  }
                  break;
                }
                break;
              }
              break;
            }
            if (GUIUtility.keyboardControl == id)
            {
              string commandName = current.commandName;
              if (commandName != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (EditorGUI.\u003C\u003Ef__switch\u0024map3 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  EditorGUI.\u003C\u003Ef__switch\u0024map3 = new Dictionary<string, int>(6)
                  {
                    {
                      "EyeDropperUpdate",
                      0
                    },
                    {
                      "EyeDropperClicked",
                      1
                    },
                    {
                      "EyeDropperCancelled",
                      2
                    },
                    {
                      "ColorPickerChanged",
                      3
                    },
                    {
                      "Copy",
                      4
                    },
                    {
                      "Paste",
                      5
                    }
                  };
                }
                int num;
                // ISSUE: reference to a compiler-generated field
                if (EditorGUI.\u003C\u003Ef__switch\u0024map3.TryGetValue(commandName, out num))
                {
                  switch (num)
                  {
                    case 0:
                      HandleUtility.Repaint();
                      break;
                    case 1:
                      GUI.changed = true;
                      HandleUtility.Repaint();
                      Color lastPickedColor = EyeDropper.GetLastPickedColor();
                      lastPickedColor.a = value.a;
                      EditorGUI.s_ColorPickID = 0;
                      return lastPickedColor;
                    case 2:
                      HandleUtility.Repaint();
                      EditorGUI.s_ColorPickID = 0;
                      break;
                    case 3:
                      GUI.changed = true;
                      HandleUtility.Repaint();
                      return ColorPicker.color;
                    case 4:
                      ColorClipboard.SetColor(value);
                      current.Use();
                      break;
                    case 5:
                      Color color2;
                      if (ColorClipboard.TryGetColor(hdr, out color2))
                      {
                        if (!showAlpha)
                          color2.a = color1.a;
                        color1 = color2;
                        GUI.changed = true;
                        current.Use();
                        break;
                      }
                      break;
                  }
                }
                else
                  break;
              }
              else
                break;
            }
            else
              break;
          }
          else
          {
            string commandName = current.commandName;
            if (commandName != null)
            {
              // ISSUE: reference to a compiler-generated field
              if (EditorGUI.\u003C\u003Ef__switch\u0024map2 == null)
              {
                // ISSUE: reference to a compiler-generated field
                EditorGUI.\u003C\u003Ef__switch\u0024map2 = new Dictionary<string, int>(3)
                {
                  {
                    "UndoRedoPerformed",
                    0
                  },
                  {
                    "Copy",
                    1
                  },
                  {
                    "Paste",
                    1
                  }
                };
              }
              int num;
              // ISSUE: reference to a compiler-generated field
              if (EditorGUI.\u003C\u003Ef__switch\u0024map2.TryGetValue(commandName, out num))
              {
                if (num != 0)
                {
                  if (num == 1)
                  {
                    current.Use();
                    break;
                  }
                  break;
                }
                if (GUIUtility.keyboardControl == id && ColorPicker.visible)
                {
                  ColorPicker.color = value;
                  break;
                }
                break;
              }
              break;
            }
            break;
          }
      }
      return color1;
    }

    internal static Color ColorSelector(Rect activatorRect, Rect renderRect, int id, Color value)
    {
      Event current = Event.current;
      Color color = value;
      value = !EditorGUI.showMixedValue ? value : Color.white;
      EventType typeForControl = current.GetTypeForControl(id);
      switch (typeForControl)
      {
        case EventType.KeyDown:
          if (current.MainActionKeyForControl(id))
          {
            current.Use();
            EditorGUI.showMixedValue = false;
            ColorPicker.Show(GUIView.current, value, false, false, (ColorPickerHDRConfig) null);
            GUIUtility.ExitGUI();
            break;
          }
          break;
        case EventType.Repaint:
          if ((double) renderRect.height > 0.0 && (double) renderRect.width > 0.0)
          {
            EditorGUI.DrawRect(renderRect, value);
            break;
          }
          break;
        default:
          if (typeForControl != EventType.ValidateCommand)
          {
            if (typeForControl != EventType.ExecuteCommand)
            {
              if (typeForControl == EventType.MouseDown && activatorRect.Contains(current.mousePosition))
              {
                current.Use();
                GUIUtility.keyboardControl = id;
                EditorGUI.showMixedValue = false;
                ColorPicker.Show(GUIView.current, value, false, false, (ColorPickerHDRConfig) null);
                GUIUtility.ExitGUI();
                break;
              }
              break;
            }
            if (GUIUtility.keyboardControl == id)
            {
              string commandName = current.commandName;
              if (commandName != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (EditorGUI.\u003C\u003Ef__switch\u0024map4 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  EditorGUI.\u003C\u003Ef__switch\u0024map4 = new Dictionary<string, int>(1)
                  {
                    {
                      "ColorPickerChanged",
                      0
                    }
                  };
                }
                int num;
                // ISSUE: reference to a compiler-generated field
                if (EditorGUI.\u003C\u003Ef__switch\u0024map4.TryGetValue(commandName, out num) && num == 0)
                {
                  current.Use();
                  GUI.changed = true;
                  HandleUtility.Repaint();
                  return ColorPicker.color;
                }
                break;
              }
              break;
            }
            break;
          }
          if (current.commandName == "UndoRedoPerformed" && GUIUtility.keyboardControl == id && ColorPicker.visible)
          {
            ColorPicker.color = value;
            break;
          }
          break;
      }
      return color;
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <returns>
    ///   <para>The curve edited by the user.</para>
    /// </returns>
    public static AnimationCurve CurveField(Rect position, AnimationCurve value)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_CurveHash, EditorGUIUtility.native, position);
      return EditorGUI.DoCurveField(EditorGUI.IndentedRect(position), controlId, value, EditorGUI.kCurveColor, new Rect(), (SerializedProperty) null);
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <returns>
    ///   <para>The curve edited by the user.</para>
    /// </returns>
    public static AnimationCurve CurveField(Rect position, string label, AnimationCurve value)
    {
      return EditorGUI.CurveField(position, EditorGUIUtility.TempContent(label), value);
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <returns>
    ///   <para>The curve edited by the user.</para>
    /// </returns>
    public static AnimationCurve CurveField(Rect position, GUIContent label, AnimationCurve value)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_CurveHash, EditorGUIUtility.native, position);
      return EditorGUI.DoCurveField(EditorGUI.PrefixLabel(position, controlId, label), controlId, value, EditorGUI.kCurveColor, new Rect(), (SerializedProperty) null);
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <returns>
    ///   <para>The curve edited by the user.</para>
    /// </returns>
    public static AnimationCurve CurveField(Rect position, AnimationCurve value, Color color, Rect ranges)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_CurveHash, EditorGUIUtility.native, position);
      return EditorGUI.DoCurveField(EditorGUI.IndentedRect(position), controlId, value, color, ranges, (SerializedProperty) null);
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <returns>
    ///   <para>The curve edited by the user.</para>
    /// </returns>
    public static AnimationCurve CurveField(Rect position, string label, AnimationCurve value, Color color, Rect ranges)
    {
      return EditorGUI.CurveField(position, EditorGUIUtility.TempContent(label), value, color, ranges);
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <returns>
    ///   <para>The curve edited by the user.</para>
    /// </returns>
    public static AnimationCurve CurveField(Rect position, GUIContent label, AnimationCurve value, Color color, Rect ranges)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_CurveHash, EditorGUIUtility.native, position);
      return EditorGUI.DoCurveField(EditorGUI.PrefixLabel(position, controlId, label), controlId, value, color, ranges, (SerializedProperty) null);
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the field.</param>
    /// <param name="property">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <param name="value"></param>
    public static void CurveField(Rect position, SerializedProperty value, Color color, Rect ranges)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_CurveHash, EditorGUIUtility.native, position);
      EditorGUI.DoCurveField(position, controlId, (AnimationCurve) null, color, ranges, value);
    }

    private static void SetCurveEditorWindowCurve(AnimationCurve value, SerializedProperty property, Color color)
    {
      CurveEditorWindow.curve = property == null ? value : (!property.hasMultipleDifferentValues ? property.animationCurveValue : new AnimationCurve());
      CurveEditorWindow.color = color;
    }

    private static AnimationCurve DoCurveField(Rect position, int id, AnimationCurve value, Color color, Rect ranges, SerializedProperty property)
    {
      Event current = Event.current;
      position.width = Mathf.Max(position.width, 2f);
      position.height = Mathf.Max(position.height, 2f);
      if (GUIUtility.keyboardControl == id && Event.current.type != EventType.Layout)
      {
        if (EditorGUI.s_CurveID != id)
        {
          EditorGUI.s_CurveID = id;
          if (CurveEditorWindow.visible)
          {
            EditorGUI.SetCurveEditorWindowCurve(value, property, color);
            EditorGUI.ShowCurvePopup(GUIView.current, ranges);
          }
        }
        else if (CurveEditorWindow.visible && Event.current.type == EventType.Repaint)
        {
          EditorGUI.SetCurveEditorWindowCurve(value, property, color);
          CurveEditorWindow.instance.Repaint();
        }
      }
      EventType typeForControl = current.GetTypeForControl(id);
      switch (typeForControl)
      {
        case EventType.KeyDown:
          if (current.MainActionKeyForControl(id))
          {
            EditorGUI.s_CurveID = id;
            EditorGUI.SetCurveEditorWindowCurve(value, property, color);
            EditorGUI.ShowCurvePopup(GUIView.current, ranges);
            current.Use();
            GUIUtility.ExitGUI();
            break;
          }
          break;
        case EventType.Repaint:
          Rect position1 = position;
          ++position1.y;
          --position1.height;
          if (ranges != new Rect())
            EditorGUIUtility.DrawCurveSwatch(position1, value, property, color, EditorGUI.kCurveBGColor, ranges);
          else
            EditorGUIUtility.DrawCurveSwatch(position1, value, property, color, EditorGUI.kCurveBGColor);
          EditorStyles.colorPickerBox.Draw(position1, GUIContent.none, id, false);
          break;
        default:
          if (typeForControl != EventType.MouseDown)
          {
            if (typeForControl == EventType.ExecuteCommand && EditorGUI.s_CurveID == id)
            {
              string commandName = current.commandName;
              if (commandName != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (EditorGUI.\u003C\u003Ef__switch\u0024map5 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  EditorGUI.\u003C\u003Ef__switch\u0024map5 = new Dictionary<string, int>(1)
                  {
                    {
                      "CurveChanged",
                      0
                    }
                  };
                }
                int num;
                // ISSUE: reference to a compiler-generated field
                if (EditorGUI.\u003C\u003Ef__switch\u0024map5.TryGetValue(commandName, out num) && num == 0)
                {
                  GUI.changed = true;
                  AnimationCurvePreviewCache.ClearCache();
                  HandleUtility.Repaint();
                  if (property != null)
                  {
                    property.animationCurveValue = CurveEditorWindow.curve;
                    if (property.hasMultipleDifferentValues)
                      Debug.LogError((object) "AnimationCurve SerializedProperty hasMultipleDifferentValues is true after writing.");
                  }
                  return CurveEditorWindow.curve;
                }
                break;
              }
              break;
            }
            break;
          }
          if (position.Contains(current.mousePosition))
          {
            EditorGUI.s_CurveID = id;
            GUIUtility.keyboardControl = id;
            EditorGUI.SetCurveEditorWindowCurve(value, property, color);
            EditorGUI.ShowCurvePopup(GUIView.current, ranges);
            current.Use();
            GUIUtility.ExitGUI();
            break;
          }
          break;
      }
      return value;
    }

    private static void ShowCurvePopup(GUIView viewToUpdate, Rect ranges)
    {
      CurveEditorSettings settings = new CurveEditorSettings();
      if ((double) ranges.width > 0.0 && (double) ranges.height > 0.0 && ((double) ranges.width != double.PositiveInfinity && (double) ranges.height != double.PositiveInfinity))
      {
        settings.hRangeMin = ranges.xMin;
        settings.hRangeMax = ranges.xMax;
        settings.vRangeMin = ranges.yMin;
        settings.vRangeMax = ranges.yMax;
      }
      CurveEditorWindow.instance.Show(GUIView.current, settings);
    }

    private static bool ValidTargetForIconSelection(UnityEngine.Object[] targets)
    {
      if ((bool) ((UnityEngine.Object) (targets[0] as MonoScript)) || (bool) ((UnityEngine.Object) (targets[0] as GameObject)))
        return targets.Length == 1;
      return false;
    }

    internal static void ObjectIconDropDown(Rect position, UnityEngine.Object[] targets, bool showLabelIcons, Texture2D nullIcon, SerializedProperty iconProperty)
    {
      if ((UnityEngine.Object) EditorGUI.s_IconTextureInactive == (UnityEngine.Object) null)
        EditorGUI.s_IconTextureInactive = (Material) EditorGUIUtility.LoadRequired("Inspectors/InactiveGUI.mat");
      if (Event.current.type == EventType.Repaint)
      {
        Texture2D texture2D = (Texture2D) null;
        if (!iconProperty.hasMultipleDifferentValues)
          texture2D = AssetPreview.GetMiniThumbnail(targets[0]);
        if ((UnityEngine.Object) texture2D == (UnityEngine.Object) null)
          texture2D = nullIcon;
        Vector2 vector2 = new Vector2(position.width, position.height);
        if ((bool) ((UnityEngine.Object) texture2D))
        {
          vector2.x = Mathf.Min((float) texture2D.width, vector2.x);
          vector2.y = Mathf.Min((float) texture2D.height, vector2.y);
        }
        Rect rect = new Rect((float) ((double) position.x + (double) position.width / 2.0 - (double) vector2.x / 2.0), (float) ((double) position.y + (double) position.height / 2.0 - (double) vector2.y / 2.0), vector2.x, vector2.y);
        GameObject target = targets[0] as GameObject;
        if ((bool) ((UnityEngine.Object) target) && (!EditorUtility.IsPersistent(targets[0]) && (!target.activeSelf || !target.activeInHierarchy)))
          Graphics.DrawTexture(rect, (Texture) texture2D, new Rect(0.0f, 0.0f, 1f, 1f), 0, 0, 0, 0, new Color(0.5f, 0.5f, 0.5f, 1f), EditorGUI.s_IconTextureInactive);
        else
          GUI.DrawTexture(rect, (Texture) texture2D);
        if (EditorGUI.ValidTargetForIconSelection(targets))
        {
          if (EditorGUI.s_IconDropDown == null)
            EditorGUI.s_IconDropDown = EditorGUIUtility.IconContent("Icon Dropdown");
          GUIStyle.none.Draw(new Rect(Mathf.Max(position.x + 2f, rect.x - 6f), rect.yMax - rect.height * 0.2f, 13f, 8f), EditorGUI.s_IconDropDown, false, false, false, false);
        }
      }
      if (!EditorGUI.ButtonMouseDown(position, GUIContent.none, FocusType.Passive, GUIStyle.none) || !EditorGUI.ValidTargetForIconSelection(targets) || !IconSelector.ShowAtPosition(targets[0], position, showLabelIcons))
        return;
      GUIUtility.ExitGUI();
    }

    public static void InspectorTitlebar(Rect position, UnityEngine.Object[] targetObjs)
    {
      GUIStyle none = GUIStyle.none;
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TitlebarHash, EditorGUIUtility.native, position);
      EditorGUI.DoInspectorTitlebar(position, controlId, true, targetObjs, none);
    }

    /// <summary>
    ///   <para>Make an inspector-window-like titlebar.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the titlebar.</param>
    /// <param name="foldout">The foldout state shown with the arrow.</param>
    /// <param name="targetObj">The object (for example a component) that the titlebar is for.</param>
    /// <param name="targetObjs">The objects that the titlebar is for.</param>
    /// <param name="expandable">Whether this editor should display a foldout arrow in order to toggle the display of its properties.</param>
    /// <returns>
    ///   <para>The foldout state selected by the user.</para>
    /// </returns>
    public static bool InspectorTitlebar(Rect position, bool foldout, UnityEngine.Object targetObj, bool expandable)
    {
      return EditorGUI.InspectorTitlebar(position, (foldout ? 1 : 0) != 0, new UnityEngine.Object[1]{ targetObj }, (expandable ? 1 : 0) != 0);
    }

    /// <summary>
    ///   <para>Make an inspector-window-like titlebar.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the titlebar.</param>
    /// <param name="foldout">The foldout state shown with the arrow.</param>
    /// <param name="targetObj">The object (for example a component) that the titlebar is for.</param>
    /// <param name="targetObjs">The objects that the titlebar is for.</param>
    /// <param name="expandable">Whether this editor should display a foldout arrow in order to toggle the display of its properties.</param>
    /// <returns>
    ///   <para>The foldout state selected by the user.</para>
    /// </returns>
    public static bool InspectorTitlebar(Rect position, bool foldout, UnityEngine.Object[] targetObjs, bool expandable)
    {
      GUIStyle inspectorTitlebar = EditorStyles.inspectorTitlebar;
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TitlebarHash, EditorGUIUtility.native, position);
      EditorGUI.DoInspectorTitlebar(position, controlId, foldout, targetObjs, inspectorTitlebar);
      foldout = EditorGUI.DoObjectMouseInteraction(foldout, position, targetObjs, controlId);
      if (expandable)
      {
        Rect foldoutRenderRect = EditorGUI.GetInspectorTitleBarObjectFoldoutRenderRect(position);
        EditorGUI.DoObjectFoldoutInternal(foldout, position, foldoutRenderRect, targetObjs, controlId);
      }
      return foldout;
    }

    internal static void DoInspectorTitlebar(Rect position, int id, bool foldout, UnityEngine.Object[] targetObjs, GUIStyle baseStyle)
    {
      GUIStyle inspectorTitlebarText = EditorStyles.inspectorTitlebarText;
      Rect rect = new Rect(position.x + (float) baseStyle.padding.left, position.y + (float) baseStyle.padding.top, 16f, 16f);
      Rect position1 = new Rect((float) ((double) position.xMax - (double) baseStyle.padding.right - 2.0 - 16.0), rect.y, 16f, 16f);
      Rect position2 = new Rect((float) ((double) rect.xMax + 2.0 + 2.0 + 16.0), rect.y, 100f, rect.height);
      position2.xMax = position1.xMin - 2f;
      int num = -1;
      foreach (UnityEngine.Object targetObj in targetObjs)
      {
        int objectEnabled = EditorUtility.GetObjectEnabled(targetObj);
        if (num == -1)
          num = objectEnabled;
        else if (num != objectEnabled)
          num = -2;
      }
      if (num != -1)
      {
        bool flag1 = AnimationMode.IsPropertyAnimated(targetObjs[0], "m_Enabled");
        bool flag2 = num != 0;
        EditorGUI.showMixedValue = num == -2;
        Rect position3 = rect;
        position3.x = rect.xMax + 2f;
        EditorGUI.BeginChangeCheck();
        Color color = GUI.color;
        if (flag1)
          GUI.color = AnimationMode.animatedPropertyColor;
        int controlId = GUIUtility.GetControlID(EditorGUI.s_TitlebarHash, EditorGUIUtility.native, position);
        bool enabled = EditorGUIInternal.DoToggleForward(position3, controlId, flag2, GUIContent.none, EditorStyles.toggle);
        if (flag1)
          GUI.color = color;
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObjects(targetObjs, (!enabled ? "Disable" : "Enable") + " Component" + (targetObjs.Length <= 1 ? string.Empty : "s"));
          foreach (UnityEngine.Object targetObj in targetObjs)
            EditorUtility.SetObjectEnabled(targetObj, enabled);
        }
        EditorGUI.showMixedValue = false;
      }
      Rect position4 = position1;
      position4.x -= 18f;
      if (EditorGUI.HelpIconButton(position4, targetObjs[0]))
        position2.xMax = position4.xMin - 2f;
      Event current = Event.current;
      Texture2D texture2D = (Texture2D) null;
      if (current.type == EventType.Repaint)
        texture2D = AssetPreview.GetMiniThumbnail(targetObjs[0]);
      if (EditorGUI.ButtonMouseDown(rect, EditorGUIUtility.TempContent((Texture) texture2D), FocusType.Passive, GUIStyle.none) && (UnityEngine.Object) (targetObjs[0] as MonoScript) != (UnityEngine.Object) null && IconSelector.ShowAtPosition(targetObjs[0], rect, true))
        GUIUtility.ExitGUI();
      switch (current.type)
      {
        case EventType.MouseDown:
          if (!position1.Contains(current.mousePosition))
            break;
          EditorUtility.DisplayObjectContextMenu(position1, targetObjs, 0);
          current.Use();
          break;
        case EventType.Repaint:
          baseStyle.Draw(position, GUIContent.none, id, foldout);
          position = baseStyle.padding.Remove(position);
          inspectorTitlebarText.Draw(position2, EditorGUIUtility.TempContent(ObjectNames.GetInspectorTitle(targetObjs[0])), id, foldout);
          inspectorTitlebarText.Draw(position1, EditorGUI.GUIContents.titleSettingsIcon, id, foldout);
          break;
      }
    }

    internal static bool ToggleTitlebar(Rect position, GUIContent label, bool foldout, ref bool toggleValue)
    {
      int controlId1 = GUIUtility.GetControlID(EditorGUI.s_TitlebarHash, EditorGUIUtility.native, position);
      GUIStyle inspectorTitlebar = EditorStyles.inspectorTitlebar;
      GUIStyle inspectorTitlebarText = EditorStyles.inspectorTitlebarText;
      GUIStyle foldout1 = EditorStyles.foldout;
      Rect position1 = new Rect(position.x + (float) inspectorTitlebar.padding.left, position.y + (float) inspectorTitlebar.padding.top, 16f, 16f);
      Rect position2 = new Rect(position1.xMax + 2f, position1.y, 200f, 16f);
      int controlId2 = GUIUtility.GetControlID(EditorGUI.s_TitlebarHash, EditorGUIUtility.native, position);
      toggleValue = EditorGUIInternal.DoToggleForward(position1, controlId2, toggleValue, GUIContent.none, EditorStyles.toggle);
      if (Event.current.type == EventType.Repaint)
      {
        inspectorTitlebar.Draw(position, GUIContent.none, controlId1, foldout);
        foldout1.Draw(EditorGUI.GetInspectorTitleBarObjectFoldoutRenderRect(position), GUIContent.none, controlId1, foldout);
        position = inspectorTitlebar.padding.Remove(position);
        inspectorTitlebarText.Draw(position2, label, controlId1, foldout);
      }
      return EditorGUIInternal.DoToggleForward(EditorGUI.IndentedRect(position), controlId1, foldout, GUIContent.none, GUIStyle.none);
    }

    internal static bool FoldoutTitlebar(Rect position, GUIContent label, bool foldout)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TitlebarHash, EditorGUIUtility.native, position);
      if (Event.current.type == EventType.Repaint)
      {
        GUIStyle inspectorTitlebar = EditorStyles.inspectorTitlebar;
        GUIStyle inspectorTitlebarText = EditorStyles.inspectorTitlebarText;
        GUIStyle foldout1 = EditorStyles.foldout;
        Rect position1 = new Rect((float) ((double) position.x + (double) inspectorTitlebar.padding.left + 2.0 + 16.0), position.y + (float) inspectorTitlebar.padding.top, 200f, 16f);
        inspectorTitlebar.Draw(position, GUIContent.none, controlId, foldout);
        foldout1.Draw(EditorGUI.GetInspectorTitleBarObjectFoldoutRenderRect(position), GUIContent.none, controlId, foldout);
        position = inspectorTitlebar.padding.Remove(position);
        inspectorTitlebarText.Draw(position1, EditorGUIUtility.TempContent(label.text), controlId, foldout);
      }
      return EditorGUIInternal.DoToggleForward(EditorGUI.IndentedRect(position), controlId, foldout, GUIContent.none, GUIStyle.none);
    }

    internal static bool HelpIconButton(Rect position, UnityEngine.Object obj)
    {
      bool flag1 = Unsupported.IsDeveloperBuild();
      bool defaultToMonoBehaviour = !flag1 || obj.GetType().Assembly.ToString().StartsWith("Assembly-");
      bool flag2 = Help.HasHelpForObject(obj, defaultToMonoBehaviour);
      if (!flag2 && !flag1)
        return false;
      Color color = GUI.color;
      GUIContent content = new GUIContent(EditorGUI.GUIContents.helpIcon);
      string helpNameForObject = Help.GetNiceHelpNameForObject(obj, defaultToMonoBehaviour);
      if (flag1 && !flag2)
      {
        GUI.color = Color.yellow;
        string str = (!(obj is MonoBehaviour) ? "sealed partial class-" : "script-") + helpNameForObject;
        content.tooltip = string.Format("Could not find Reference page for {0} ({1}).\nDocs for this object is missing or all docs are missing.\nThis warning only shows up in development builds.", (object) helpNameForObject, (object) str);
      }
      else
        content.tooltip = string.Format("Open Reference for {0}.", (object) helpNameForObject);
      GUIStyle inspectorTitlebarText = EditorStyles.inspectorTitlebarText;
      if (GUI.Button(position, content, inspectorTitlebarText))
        Help.ShowHelpForObject(obj);
      GUI.color = color;
      return true;
    }

    internal static bool FoldoutInternal(Rect position, bool foldout, GUIContent content, bool toggleOnLabelClick, GUIStyle style)
    {
      Rect rect1 = position;
      if (EditorGUIUtility.hierarchyMode)
      {
        int num = EditorStyles.foldout.padding.left - EditorStyles.label.padding.left;
        position.xMin -= (float) num;
      }
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FoldoutHash, EditorGUIUtility.native, position);
      EventType eventType1 = Event.current.type;
      if (!GUI.enabled && GUIClip.enabled && (Event.current.rawType == EventType.MouseDown || Event.current.rawType == EventType.MouseDrag || Event.current.rawType == EventType.MouseUp))
        eventType1 = Event.current.rawType;
      EventType eventType2 = eventType1;
      switch (eventType2)
      {
        case EventType.MouseDown:
          if (position.Contains(Event.current.mousePosition) && Event.current.button == 0)
          {
            int num = controlId;
            GUIUtility.hotControl = num;
            GUIUtility.keyboardControl = num;
            Event.current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            GUIUtility.hotControl = 0;
            Event.current.Use();
            Rect rect2 = position;
            if (!toggleOnLabelClick)
            {
              rect2.width = (float) style.padding.left;
              rect2.x += EditorGUI.indent;
            }
            if (rect2.Contains(Event.current.mousePosition))
            {
              GUI.changed = true;
              return !foldout;
            }
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            Event.current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.keyboardControl == controlId)
          {
            KeyCode keyCode = Event.current.keyCode;
            if (keyCode == KeyCode.LeftArrow && foldout || keyCode == KeyCode.RightArrow && !foldout)
            {
              foldout = !foldout;
              GUI.changed = true;
              Event.current.Use();
              break;
            }
            break;
          }
          break;
        case EventType.Repaint:
          EditorStyles.foldoutSelected.Draw(position, GUIContent.none, controlId, EditorGUI.s_DragUpdatedOverID == controlId);
          Rect position1 = new Rect(position.x + EditorGUI.indent, position.y, EditorGUIUtility.labelWidth - EditorGUI.indent, position.height);
          if (EditorGUI.showMixedValue && !foldout)
          {
            style.Draw(position1, content, controlId, foldout);
            EditorGUI.BeginHandleMixedValueContentColor();
            Rect position2 = rect1;
            position2.xMin += EditorGUIUtility.labelWidth;
            EditorStyles.label.Draw(position2, EditorGUI.s_MixedValueContent, controlId, false);
            EditorGUI.EndHandleMixedValueContentColor();
            break;
          }
          style.Draw(position1, content, controlId, foldout);
          break;
        case EventType.DragUpdated:
          if (EditorGUI.s_DragUpdatedOverID == controlId)
          {
            if (position.Contains(Event.current.mousePosition))
            {
              if ((double) Time.realtimeSinceStartup > EditorGUI.s_FoldoutDestTime)
              {
                foldout = true;
                Event.current.Use();
                break;
              }
              break;
            }
            EditorGUI.s_DragUpdatedOverID = 0;
            break;
          }
          if (position.Contains(Event.current.mousePosition))
          {
            EditorGUI.s_DragUpdatedOverID = controlId;
            EditorGUI.s_FoldoutDestTime = (double) Time.realtimeSinceStartup + 0.7;
            Event.current.Use();
            break;
          }
          break;
        default:
          if (eventType2 == EventType.DragExited && EditorGUI.s_DragUpdatedOverID == controlId)
          {
            EditorGUI.s_DragUpdatedOverID = 0;
            Event.current.Use();
            break;
          }
          break;
      }
      return foldout;
    }

    /// <summary>
    ///   <para>Make a progress bar.</para>
    /// </summary>
    /// <param name="totalPosition">Rectangle on the screen to use in total for both the control.</param>
    /// <param name="value">Value that is shown.</param>
    /// <param name="position"></param>
    /// <param name="text"></param>
    public static void ProgressBar(Rect position, float value, string text)
    {
      if (Event.current.GetTypeForControl(GUIUtility.GetControlID(EditorGUI.s_ProgressBarHash, EditorGUIUtility.native, position)) != EventType.Repaint)
        return;
      EditorStyles.progressBarBack.Draw(position, false, false, false, false);
      Rect position1 = new Rect(position);
      value = Mathf.Clamp01(value);
      position1.width *= value;
      EditorStyles.progressBarBar.Draw(position1, false, false, false, false);
      EditorStyles.progressBarText.Draw(position, text, false, false, false, false);
    }

    /// <summary>
    ///   <para>Make a help box with a message to the user.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to draw the help box within.</param>
    /// <param name="message">The message text.</param>
    /// <param name="type">The type of message.</param>
    public static void HelpBox(Rect position, string message, MessageType type)
    {
      GUI.Label(position, EditorGUIUtility.TempContent(message, (Texture) EditorGUIUtility.GetHelpIcon(type)), EditorStyles.helpBox);
    }

    internal static bool LabelHasContent(GUIContent label)
    {
      if (label == null || label.text != string.Empty)
        return true;
      return (UnityEngine.Object) label.image != (UnityEngine.Object) null;
    }

    private static void DrawTextDebugHelpers(Rect labelPosition)
    {
      Color color = GUI.color;
      GUI.color = Color.white;
      GUI.DrawTexture(new Rect(labelPosition.x, labelPosition.y, labelPosition.width, 4f), (Texture) EditorGUIUtility.whiteTexture);
      GUI.color = Color.cyan;
      GUI.DrawTexture(new Rect(labelPosition.x, labelPosition.yMax - 4f, labelPosition.width, 4f), (Texture) EditorGUIUtility.whiteTexture);
      GUI.color = color;
    }

    internal static void HandlePrefixLabelInternal(Rect totalPosition, Rect labelPosition, GUIContent label, int id, GUIStyle style)
    {
      if (id == 0 && label != null)
      {
        EditorGUI.s_PrefixLabel.text = label.text;
        EditorGUI.s_PrefixLabel.image = label.image;
        EditorGUI.s_PrefixLabel.tooltip = label.tooltip;
        EditorGUI.s_PrefixTotalRect = totalPosition;
        EditorGUI.s_PrefixRect = labelPosition;
        EditorGUI.s_PrefixStyle = style;
      }
      else
      {
        if (Highlighter.searchMode == HighlightSearchMode.PrefixLabel || Highlighter.searchMode == HighlightSearchMode.Auto)
          Highlighter.Handle(totalPosition, label.text);
        switch (Event.current.type)
        {
          case EventType.MouseDown:
            if (Event.current.button == 0 && labelPosition.Contains(Event.current.mousePosition))
            {
              if (EditorGUIUtility.CanHaveKeyboardFocus(id))
                GUIUtility.keyboardControl = id;
              EditorGUIUtility.editingTextField = false;
              HandleUtility.Repaint();
              break;
            }
            break;
          case EventType.Repaint:
            ++labelPosition.width;
            style.DrawPrefixLabel(labelPosition, label, id);
            break;
        }
        EditorGUI.s_PrefixLabel.text = (string) null;
      }
    }

    /// <summary>
    ///   <para>Make a label in front of some control.</para>
    /// </summary>
    /// <param name="totalPosition">Rectangle on the screen to use in total for both the label and the control.</param>
    /// <param name="id">The unique ID of the control. If none specified, the ID of the following control is used.</param>
    /// <param name="label">Label to show in front of the control.</param>
    /// <param name="style">Style to use for the label.</param>
    /// <returns>
    ///   <para>Rectangle on the screen to use just for the control itself.</para>
    /// </returns>
    public static Rect PrefixLabel(Rect totalPosition, GUIContent label)
    {
      return EditorGUI.PrefixLabel(totalPosition, 0, label, EditorStyles.label);
    }

    /// <summary>
    ///   <para>Make a label in front of some control.</para>
    /// </summary>
    /// <param name="totalPosition">Rectangle on the screen to use in total for both the label and the control.</param>
    /// <param name="id">The unique ID of the control. If none specified, the ID of the following control is used.</param>
    /// <param name="label">Label to show in front of the control.</param>
    /// <param name="style">Style to use for the label.</param>
    /// <returns>
    ///   <para>Rectangle on the screen to use just for the control itself.</para>
    /// </returns>
    public static Rect PrefixLabel(Rect totalPosition, GUIContent label, GUIStyle style)
    {
      return EditorGUI.PrefixLabel(totalPosition, 0, label, style);
    }

    /// <summary>
    ///   <para>Make a label in front of some control.</para>
    /// </summary>
    /// <param name="totalPosition">Rectangle on the screen to use in total for both the label and the control.</param>
    /// <param name="id">The unique ID of the control. If none specified, the ID of the following control is used.</param>
    /// <param name="label">Label to show in front of the control.</param>
    /// <param name="style">Style to use for the label.</param>
    /// <returns>
    ///   <para>Rectangle on the screen to use just for the control itself.</para>
    /// </returns>
    public static Rect PrefixLabel(Rect totalPosition, int id, GUIContent label)
    {
      return EditorGUI.PrefixLabel(totalPosition, id, label, EditorStyles.label);
    }

    /// <summary>
    ///   <para>Make a label in front of some control.</para>
    /// </summary>
    /// <param name="totalPosition">Rectangle on the screen to use in total for both the label and the control.</param>
    /// <param name="id">The unique ID of the control. If none specified, the ID of the following control is used.</param>
    /// <param name="label">Label to show in front of the control.</param>
    /// <param name="style">Style to use for the label.</param>
    /// <returns>
    ///   <para>Rectangle on the screen to use just for the control itself.</para>
    /// </returns>
    public static Rect PrefixLabel(Rect totalPosition, int id, GUIContent label, GUIStyle style)
    {
      if (!EditorGUI.LabelHasContent(label))
        return EditorGUI.IndentedRect(totalPosition);
      Rect labelPosition = new Rect(totalPosition.x + EditorGUI.indent, totalPosition.y, EditorGUIUtility.labelWidth - EditorGUI.indent, 16f);
      Rect rect = new Rect(totalPosition.x + EditorGUIUtility.labelWidth, totalPosition.y, totalPosition.width - EditorGUIUtility.labelWidth, totalPosition.height);
      EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, id, style);
      return rect;
    }

    internal static Rect MultiFieldPrefixLabel(Rect totalPosition, int id, GUIContent label, int columns)
    {
      if (!EditorGUI.LabelHasContent(label))
        return EditorGUI.IndentedRect(totalPosition);
      if (EditorGUIUtility.wideMode)
      {
        Rect labelPosition = new Rect(totalPosition.x + EditorGUI.indent, totalPosition.y, EditorGUIUtility.labelWidth - EditorGUI.indent, 16f);
        Rect rect = totalPosition;
        rect.xMin += EditorGUIUtility.labelWidth;
        if (columns > 1)
        {
          --labelPosition.width;
          --rect.xMin;
        }
        if (columns == 2)
        {
          float num = (float) (((double) rect.width - 4.0) / 3.0);
          rect.xMax -= num + 2f;
        }
        EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, id);
        return rect;
      }
      Rect labelPosition1 = new Rect(totalPosition.x + EditorGUI.indent, totalPosition.y, totalPosition.width - EditorGUI.indent, 16f);
      Rect rect1 = totalPosition;
      rect1.xMin += EditorGUI.indent + 15f;
      rect1.yMin += 16f;
      EditorGUI.HandlePrefixLabel(totalPosition, labelPosition1, label, id);
      return rect1;
    }

    /// <summary>
    ///   <para>Create a Property wrapper, useful for making regular GUI controls work with SerializedProperty.</para>
    /// </summary>
    /// <param name="totalPosition">Rectangle on the screen to use for the control, including label if applicable.</param>
    /// <param name="label">Optional label in front of the slider. Use null to use the name from the SerializedProperty. Use GUIContent.none to not display a label.</param>
    /// <param name="property">The SerializedProperty to use for the control.</param>
    /// <returns>
    ///   <para>The actual label to use for the control.</para>
    /// </returns>
    public static GUIContent BeginProperty(Rect totalPosition, GUIContent label, SerializedProperty property)
    {
      Highlighter.HighlightIdentifier(totalPosition, property.propertyPath);
      if (EditorGUI.s_PendingPropertyKeyboardHandling != null)
        EditorGUI.DoPropertyFieldKeyboardHandling(EditorGUI.s_PendingPropertyKeyboardHandling);
      EditorGUI.s_PendingPropertyKeyboardHandling = property;
      if (property == null)
      {
        string message = (label != null ? label.text + ": " : string.Empty) + "SerializedProperty is null";
        EditorGUI.HelpBox(totalPosition, "null", MessageType.Error);
        throw new NullReferenceException(message);
      }
      EditorGUI.s_PropertyFieldTempContent.text = LocalizationDatabase.GetLocalizedString(label != null ? label.text : property.displayName);
      EditorGUI.s_PropertyFieldTempContent.tooltip = !EditorGUI.isCollectingTooltips ? (string) null : (label != null ? label.tooltip : property.tooltip);
      string tooltip = ScriptAttributeUtility.GetHandler(property).tooltip;
      if (tooltip != null)
        EditorGUI.s_PropertyFieldTempContent.tooltip = tooltip;
      EditorGUI.s_PropertyFieldTempContent.image = label != null ? label.image : (Texture) null;
      if (Event.current.alt && property.serializedObject.inspectorMode != InspectorMode.Normal)
      {
        GUIContent fieldTempContent = EditorGUI.s_PropertyFieldTempContent;
        string propertyPath = property.propertyPath;
        EditorGUI.s_PropertyFieldTempContent.text = propertyPath;
        string str = propertyPath;
        fieldTempContent.tooltip = str;
      }
      bool boldDefaultFont = EditorGUIUtility.GetBoldDefaultFont();
      if (property.serializedObject.targetObjects.Length == 1 && property.isInstantiatedPrefab)
        EditorGUIUtility.SetBoldDefaultFont(property.prefabOverride);
      EditorGUI.s_PropertyStack.Push(new PropertyGUIData(property, totalPosition, boldDefaultFont, GUI.enabled, GUI.color));
      EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
      if (property.isAnimated)
      {
        Color animatedPropertyColor = AnimationMode.animatedPropertyColor;
        animatedPropertyColor.a *= GUI.color.a;
        GUI.color = animatedPropertyColor;
      }
      GUI.enabled &= property.editable;
      return EditorGUI.s_PropertyFieldTempContent;
    }

    /// <summary>
    ///   <para>Ends a Property wrapper started with BeginProperty.</para>
    /// </summary>
    public static void EndProperty()
    {
      EditorGUI.showMixedValue = false;
      PropertyGUIData propertyGuiData = EditorGUI.s_PropertyStack.Pop();
      if (Event.current.type == EventType.ContextClick && propertyGuiData.totalPosition.Contains(Event.current.mousePosition))
        EditorGUI.DoPropertyContextMenu(propertyGuiData.property);
      EditorGUIUtility.SetBoldDefaultFont(propertyGuiData.wasBoldDefaultFont);
      GUI.enabled = propertyGuiData.wasEnabled;
      GUI.color = propertyGuiData.color;
      if (EditorGUI.s_PendingPropertyKeyboardHandling != null)
        EditorGUI.DoPropertyFieldKeyboardHandling(EditorGUI.s_PendingPropertyKeyboardHandling);
      if (EditorGUI.s_PendingPropertyDelete == null || EditorGUI.s_PropertyStack.Count != 0)
        return;
      if (EditorGUI.s_PendingPropertyDelete.propertyPath == propertyGuiData.property.propertyPath)
        propertyGuiData.property.DeleteCommand();
      else
        EditorGUI.s_PendingPropertyDelete.DeleteCommand();
      EditorGUI.s_PendingPropertyDelete = (SerializedProperty) null;
    }

    private static void DoPropertyFieldKeyboardHandling(SerializedProperty property)
    {
      if (Event.current.type == EventType.ExecuteCommand || Event.current.type == EventType.ValidateCommand)
      {
        if (GUIUtility.keyboardControl == EditorGUIUtility.s_LastControlID && (Event.current.commandName == "Delete" || Event.current.commandName == "SoftDelete"))
        {
          if (Event.current.type == EventType.ExecuteCommand)
            EditorGUI.s_PendingPropertyDelete = property.Copy();
          Event.current.Use();
        }
        if (GUIUtility.keyboardControl == EditorGUIUtility.s_LastControlID && Event.current.commandName == "Duplicate")
        {
          if (Event.current.type == EventType.ExecuteCommand)
            property.DuplicateCommand();
          Event.current.Use();
        }
      }
      EditorGUI.s_PendingPropertyKeyboardHandling = (SerializedProperty) null;
    }

    internal static void LayerMaskField(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.LayerMaskField(position, property, label, EditorStyles.layerMaskField);
    }

    internal static void LayerMaskField(Rect position, SerializedProperty property, GUIContent label, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_LayerMaskField, EditorGUIUtility.native, position);
      position = EditorGUI.PrefixLabel(position, controlId, label);
      Event current = Event.current;
      if (current.type == EventType.Repaint)
      {
        if (EditorGUI.showMixedValue)
        {
          EditorGUI.BeginHandleMixedValueContentColor();
          style.Draw(position, EditorGUI.s_MixedValueContent, controlId, false);
          EditorGUI.EndHandleMixedValueContentColor();
        }
        else
          style.Draw(position, EditorGUIUtility.TempContent(property.layerMaskStringValue), controlId, false);
      }
      else
      {
        if ((current.type != EventType.MouseDown || !position.Contains(current.mousePosition)) && !current.MainActionKeyForControl(controlId))
          return;
        SerializedProperty property1 = property.serializedObject.FindProperty(property.propertyPath);
        EditorUtility.DisplayCustomMenu(position, property.GetLayerMaskNames(), !property.hasMultipleDifferentValues ? property.GetLayerMaskSelectedIndex() : new int[0], new EditorUtility.SelectMenuItemFunction(EditorGUI.SetLayerMaskValueDelegate), (object) property1);
        Event.current.Use();
      }
    }

    internal static void SetLayerMaskValueDelegate(object userData, string[] options, int selected)
    {
      SerializedProperty serializedProperty = (SerializedProperty) userData;
      serializedProperty.ToggleLayerMaskAtIndex(selected);
      serializedProperty.serializedObject.ApplyModifiedProperties();
    }

    internal static void ShowRepaints()
    {
      if (!Unsupported.IsDeveloperBuild())
        return;
      Color backgroundColor = GUI.backgroundColor;
      GUI.backgroundColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
      GUI.Label(new Rect(0.0f, 0.0f, 10f, 10f), string.Empty, GUI.skin.button);
      GUI.backgroundColor = backgroundColor;
    }

    internal static void DrawTextureAlphaInternal(Rect position, Texture image, ScaleMode scaleMode, float imageAspect)
    {
      EditorGUI.DrawPreviewTextureInternal(position, image, EditorGUI.alphaMaterial, scaleMode, imageAspect);
    }

    internal static void DrawTextureTransparentInternal(Rect position, Texture image, ScaleMode scaleMode, float imageAspect)
    {
      if ((double) imageAspect == 0.0 && (UnityEngine.Object) image == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Please specify an image or a imageAspect");
      }
      else
      {
        if ((double) imageAspect == 0.0)
          imageAspect = (float) image.width / (float) image.height;
        EditorGUI.DrawTransparencyCheckerTexture(position, scaleMode, imageAspect);
        if (!((UnityEngine.Object) image != (UnityEngine.Object) null))
          return;
        EditorGUI.DrawPreviewTexture(position, image, EditorGUI.transparentMaterial, scaleMode, imageAspect);
      }
    }

    internal static void DrawTransparencyCheckerTexture(Rect position, ScaleMode scaleMode, float imageAspect)
    {
      Rect outScreenRect = new Rect();
      Rect outSourceRect = new Rect();
      GUI.CalculateScaledTextureRects(position, scaleMode, imageAspect, ref outScreenRect, ref outSourceRect);
      GUI.DrawTextureWithTexCoords(outScreenRect, (Texture) EditorGUI.transparentCheckerTexture, new Rect(outScreenRect.width * -0.5f / (float) EditorGUI.transparentCheckerTexture.width, outScreenRect.height * -0.5f / (float) EditorGUI.transparentCheckerTexture.height, outScreenRect.width / (float) EditorGUI.transparentCheckerTexture.width, outScreenRect.height / (float) EditorGUI.transparentCheckerTexture.height), false);
    }

    internal static void DrawPreviewTextureInternal(Rect position, Texture image, Material mat, ScaleMode scaleMode, float imageAspect)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if ((double) imageAspect == 0.0)
        imageAspect = (float) image.width / (float) image.height;
      if ((UnityEngine.Object) mat == (UnityEngine.Object) null)
        mat = EditorGUI.GetMaterialForSpecialTexture(image);
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear && !TextureUtil.GetLinearSampled(image);
      if ((UnityEngine.Object) mat == (UnityEngine.Object) null)
      {
        GUI.DrawTexture(position, image, scaleMode, false, imageAspect);
        GL.sRGBWrite = false;
      }
      else
      {
        Rect outScreenRect = new Rect();
        Rect outSourceRect = new Rect();
        GUI.CalculateScaledTextureRects(position, scaleMode, imageAspect, ref outScreenRect, ref outSourceRect);
        Texture2D texture2D = image as Texture2D;
        if ((UnityEngine.Object) texture2D != (UnityEngine.Object) null && TextureUtil.GetUsageMode(image) == TextureUsageMode.AlwaysPadded)
        {
          outSourceRect.width *= (float) texture2D.width / (float) TextureUtil.GetGLWidth((Texture) texture2D);
          outSourceRect.height *= (float) texture2D.height / (float) TextureUtil.GetGLHeight((Texture) texture2D);
        }
        Graphics.DrawTexture(outScreenRect, image, outSourceRect, 0, 0, 0, 0, GUI.color, mat);
        GL.sRGBWrite = false;
      }
    }

    internal static Material GetMaterialForSpecialTexture(Texture t)
    {
      if (!(bool) ((UnityEngine.Object) t))
        return (Material) null;
      switch (TextureUtil.GetUsageMode(t))
      {
        case TextureUsageMode.LightmapRGBM:
        case TextureUsageMode.RGBMEncoded:
          return EditorGUI.lightmapRGBMMaterial;
        case TextureUsageMode.LightmapDoubleLDR:
          return EditorGUI.lightmapDoubleLDRMaterial;
        case TextureUsageMode.NormalmapPlain:
        case TextureUsageMode.NormalmapDXT5nm:
          return EditorGUI.normalmapMaterial;
        default:
          if (TextureUtil.IsAlphaOnlyTextureFormat(TextureUtil.GetTextureFormat(t)))
            return EditorGUI.alphaMaterial;
          return (Material) null;
      }
    }

    private static void SetExpandedRecurse(SerializedProperty property, bool expanded)
    {
      SerializedProperty serializedProperty = property.Copy();
      serializedProperty.isExpanded = expanded;
      int depth = serializedProperty.depth;
      while (serializedProperty.NextVisible(true) && serializedProperty.depth > depth)
      {
        if (serializedProperty.hasVisibleChildren)
          serializedProperty.isExpanded = expanded;
      }
    }

    internal static float GetSinglePropertyHeight(SerializedProperty property, GUIContent label)
    {
      if (property == null)
        return 16f;
      switch (property.propertyType)
      {
        case SerializedPropertyType.Vector3:
        case SerializedPropertyType.Vector2:
          return (float) ((!EditorGUI.LabelHasContent(label) || EditorGUIUtility.wideMode ? 0.0 : 16.0) + 16.0);
        case SerializedPropertyType.Rect:
          return (float) ((!EditorGUI.LabelHasContent(label) || EditorGUIUtility.wideMode ? 0.0 : 16.0) + 32.0);
        case SerializedPropertyType.Bounds:
          return (float) ((EditorGUI.LabelHasContent(label) ? 16.0 : 0.0) + 32.0);
        default:
          return 16f;
      }
    }

    internal static float GetPropertyHeightInternal(SerializedProperty property, GUIContent label, bool includeChildren)
    {
      return ScriptAttributeUtility.GetHandler(property).GetHeight(property, label, includeChildren);
    }

    internal static bool HasVisibleChildFields(SerializedProperty property)
    {
      switch (property.propertyType)
      {
        case SerializedPropertyType.Vector2:
        case SerializedPropertyType.Vector3:
        case SerializedPropertyType.Rect:
        case SerializedPropertyType.Bounds:
          return false;
        default:
          return property.hasVisibleChildren;
      }
    }

    internal static bool PropertyFieldInternal(Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
    {
      return ScriptAttributeUtility.GetHandler(property).OnGUI(position, property, label, includeChildren);
    }

    internal static bool DefaultPropertyField(Rect position, SerializedProperty property, GUIContent label)
    {
      label = EditorGUI.BeginProperty(position, label, property);
      SerializedPropertyType propertyType = property.propertyType;
      bool flag1 = false;
      if (!EditorGUI.HasVisibleChildFields(property))
      {
        switch (propertyType)
        {
          case SerializedPropertyType.Integer:
            EditorGUI.BeginChangeCheck();
            long num1 = EditorGUI.LongField(position, label, property.longValue);
            if (EditorGUI.EndChangeCheck())
            {
              property.longValue = num1;
              break;
            }
            break;
          case SerializedPropertyType.Boolean:
            EditorGUI.BeginChangeCheck();
            bool flag2 = EditorGUI.Toggle(position, label, property.boolValue);
            if (EditorGUI.EndChangeCheck())
            {
              property.boolValue = flag2;
              break;
            }
            break;
          case SerializedPropertyType.Float:
            EditorGUI.BeginChangeCheck();
            double num2 = !(property.type == "float") ? EditorGUI.DoubleField(position, label, property.doubleValue) : (double) EditorGUI.FloatField(position, label, property.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
              property.doubleValue = num2;
              break;
            }
            break;
          case SerializedPropertyType.String:
            EditorGUI.BeginChangeCheck();
            string str1 = EditorGUI.TextField(position, label, property.stringValue);
            if (EditorGUI.EndChangeCheck())
            {
              property.stringValue = str1;
              break;
            }
            break;
          case SerializedPropertyType.Color:
            EditorGUI.BeginChangeCheck();
            Color color = EditorGUI.ColorField(position, label, property.colorValue);
            if (EditorGUI.EndChangeCheck())
            {
              property.colorValue = color;
              break;
            }
            break;
          case SerializedPropertyType.ObjectReference:
            EditorGUI.ObjectFieldInternal(position, property, (System.Type) null, label, EditorStyles.objectField);
            break;
          case SerializedPropertyType.LayerMask:
            EditorGUI.LayerMaskField(position, property, label);
            break;
          case SerializedPropertyType.Enum:
            EditorGUI.Popup(position, property, label);
            break;
          case SerializedPropertyType.Vector2:
            EditorGUI.Vector2Field(position, property, label);
            break;
          case SerializedPropertyType.Vector3:
            EditorGUI.Vector3Field(position, property, label);
            break;
          case SerializedPropertyType.Vector4:
            EditorGUI.Vector4Field(position, property, label);
            break;
          case SerializedPropertyType.Rect:
            EditorGUI.RectField(position, property, label);
            break;
          case SerializedPropertyType.ArraySize:
            EditorGUI.BeginChangeCheck();
            int num3 = EditorGUI.ArraySizeField(position, label, property.intValue, EditorStyles.numberField);
            if (EditorGUI.EndChangeCheck())
            {
              property.intValue = num3;
              break;
            }
            break;
          case SerializedPropertyType.Character:
            char[] chArray = new char[1]
            {
              (char) property.intValue
            };
            bool changed = GUI.changed;
            GUI.changed = false;
            string str2 = EditorGUI.TextField(position, label, new string(chArray));
            if (GUI.changed)
            {
              if (str2.Length == 1)
                property.intValue = (int) str2[0];
              else
                GUI.changed = false;
            }
            GUI.changed |= changed;
            break;
          case SerializedPropertyType.AnimationCurve:
            int controlId1 = GUIUtility.GetControlID(EditorGUI.s_CurveHash, EditorGUIUtility.native, position);
            EditorGUI.DoCurveField(EditorGUI.PrefixLabel(position, controlId1, label), controlId1, (AnimationCurve) null, EditorGUI.kCurveColor, new Rect(), property);
            break;
          case SerializedPropertyType.Bounds:
            EditorGUI.BoundsField(position, property, label);
            break;
          case SerializedPropertyType.Gradient:
            int controlId2 = GUIUtility.GetControlID(EditorGUI.s_CurveHash, EditorGUIUtility.native, position);
            EditorGUI.DoGradientField(EditorGUI.PrefixLabel(position, controlId2, label), controlId2, (Gradient) null, property);
            break;
          default:
            int controlId3 = GUIUtility.GetControlID(EditorGUI.s_GenericField, FocusType.Keyboard, position);
            EditorGUI.PrefixLabel(position, controlId3, label);
            break;
        }
      }
      else
      {
        Event @event = new Event(Event.current);
        bool isExpanded = property.isExpanded;
        EditorGUI.BeginDisabledGroup(!property.editable);
        GUIStyle style = DragAndDrop.activeControlID != -10 ? EditorStyles.foldout : EditorStyles.foldoutPreDrop;
        bool expanded = EditorGUI.Foldout(position, isExpanded, EditorGUI.s_PropertyFieldTempContent, true, style);
        EditorGUI.EndDisabledGroup();
        if (expanded != isExpanded)
        {
          if (Event.current.alt)
            EditorGUI.SetExpandedRecurse(property, expanded);
          else
            property.isExpanded = expanded;
        }
        flag1 = expanded;
        int lastControlId = EditorGUIUtility.s_LastControlID;
        switch (@event.type)
        {
          case EventType.DragUpdated:
          case EventType.DragPerform:
            if (position.Contains(@event.mousePosition) && GUI.enabled)
            {
              UnityEngine.Object[] objectReferences = DragAndDrop.objectReferences;
              UnityEngine.Object[] references = new UnityEngine.Object[1];
              bool flag3 = false;
              foreach (UnityEngine.Object object1 in objectReferences)
              {
                references[0] = object1;
                UnityEngine.Object object2 = EditorGUI.ValidateObjectFieldAssignment(references, (System.Type) null, property);
                if (object2 != (UnityEngine.Object) null)
                {
                  DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                  if (@event.type == EventType.DragPerform)
                  {
                    property.AppendFoldoutPPtrValue(object2);
                    flag3 = true;
                    DragAndDrop.activeControlID = 0;
                  }
                  else
                    DragAndDrop.activeControlID = lastControlId;
                }
              }
              if (flag3)
              {
                GUI.changed = true;
                DragAndDrop.AcceptDrag();
                break;
              }
              break;
            }
            break;
          case EventType.DragExited:
            if (GUI.enabled)
            {
              HandleUtility.Repaint();
              break;
            }
            break;
        }
      }
      EditorGUI.EndProperty();
      return flag1;
    }

    internal static void DrawLegend(Rect position, Color color, string label, bool enabled)
    {
      position = new Rect(position.x + 2f, position.y + 2f, position.width - 2f, position.height - 2f);
      Color backgroundColor = GUI.backgroundColor;
      GUI.backgroundColor = !enabled ? new Color(0.5f, 0.5f, 0.5f, 0.45f) : color;
      GUI.Label(position, label, (GUIStyle) "ProfilerPaneSubLabel");
      GUI.backgroundColor = backgroundColor;
    }

    internal static string TextFieldDropDown(Rect position, string text, string[] dropDownElement)
    {
      return EditorGUI.TextFieldDropDown(position, GUIContent.none, text, dropDownElement);
    }

    internal static string TextFieldDropDown(Rect position, GUIContent label, string text, string[] dropDownElement)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TextFieldDropDownHash, FocusType.Keyboard, position);
      return EditorGUI.DoTextFieldDropDown(EditorGUI.PrefixLabel(position, controlId, label), controlId, text, dropDownElement, false);
    }

    internal static string DelayedTextFieldDropDown(Rect position, string text, string[] dropDownElement)
    {
      return EditorGUI.DelayedTextFieldDropDown(position, GUIContent.none, text, dropDownElement);
    }

    internal static string DelayedTextFieldDropDown(Rect position, GUIContent label, string text, string[] dropDownElement)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TextFieldDropDownHash, FocusType.Keyboard, position);
      return EditorGUI.DoTextFieldDropDown(EditorGUI.PrefixLabel(position, controlId, label), controlId, text, dropDownElement, true);
    }

    private static Enum EnumFlagsToInt(System.Type type, int value)
    {
      return Enum.Parse(type, value.ToString()) as Enum;
    }

    internal static Color ColorBrightnessField(Rect r, GUIContent label, Color value, float minBrightness, float maxBrightness)
    {
      return EditorGUI.ColorBrightnessFieldInternal(r, label, value, minBrightness, maxBrightness, EditorStyles.numberField);
    }

    internal static Color ColorBrightnessFieldInternal(Rect position, GUIContent label, Color value, float minBrightness, float maxBrightness, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FloatFieldHash, FocusType.Keyboard, position);
      Rect rect = EditorGUI.PrefixLabel(position, controlId, label);
      position.xMax = rect.x;
      return EditorGUI.DoColorBrightnessField(rect, position, value, minBrightness, maxBrightness, style);
    }

    internal static Color DoColorBrightnessField(Rect rect, Rect dragRect, Color col, float minBrightness, float maxBrightness, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(18975602, FocusType.Keyboard);
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (current.button == 0 && dragRect.Contains(Event.current.mousePosition) && GUIUtility.hotControl == 0)
          {
            EditorGUI.ColorBrightnessFieldStateObject stateObject = GUIUtility.GetStateObject(typeof (EditorGUI.ColorBrightnessFieldStateObject), controlId) as EditorGUI.ColorBrightnessFieldStateObject;
            if (stateObject != null)
              Color.RGBToHSV(col, out stateObject.m_Hue, out stateObject.m_Saturation, out stateObject.m_Brightness);
            GUIUtility.keyboardControl = 0;
            GUIUtility.hotControl = controlId;
            GUI.changed = true;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(1);
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            GUIUtility.hotControl = 0;
            EditorGUIUtility.SetWantsMouseJumping(0);
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            EditorGUI.ColorBrightnessFieldStateObject fieldStateObject = GUIUtility.QueryStateObject(typeof (EditorGUI.ColorBrightnessFieldStateObject), controlId) as EditorGUI.ColorBrightnessFieldStateObject;
            float num1 = HandleUtility.niceMouseDelta * Mathf.Clamp01(Mathf.Max(1f, Mathf.Pow(Mathf.Abs(col.maxColorComponent), 0.5f)) * 0.004f);
            float num2 = Mathf.Clamp(fieldStateObject.m_Brightness + num1, minBrightness, maxBrightness);
            fieldStateObject.m_Brightness = (float) Math.Round((double) num2, 3);
            col = Color.HSVToRGB(fieldStateObject.m_Hue, fieldStateObject.m_Saturation, fieldStateObject.m_Brightness, (double) maxBrightness > 1.0);
            GUIUtility.keyboardControl = 0;
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (GUIUtility.hotControl == 0)
          {
            EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.SlideArrow);
            break;
          }
          break;
      }
      EditorGUI.BeginChangeCheck();
      float num = EditorGUI.DelayedFloatField(rect, col.maxColorComponent, style);
      if (EditorGUI.EndChangeCheck())
      {
        float H;
        float S;
        float V1;
        Color.RGBToHSV(col, out H, out S, out V1);
        float V2 = Mathf.Clamp(num, minBrightness, maxBrightness);
        col = Color.HSVToRGB(H, S, V2, (double) maxBrightness > 1.0);
      }
      return col;
    }

    internal static Gradient GradientField(Rect position, Gradient gradient)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_GradientHash, EditorGUIUtility.native, position);
      return EditorGUI.DoGradientField(position, controlId, gradient, (SerializedProperty) null);
    }

    internal static Gradient GradientField(string label, Rect position, Gradient gradient)
    {
      return EditorGUI.GradientField(EditorGUIUtility.TempContent(label), position, gradient);
    }

    internal static Gradient GradientField(GUIContent label, Rect position, Gradient gradient)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_GradientHash, EditorGUIUtility.native, position);
      return EditorGUI.DoGradientField(EditorGUI.PrefixLabel(position, controlId, label), controlId, gradient, (SerializedProperty) null);
    }

    internal static Gradient GradientField(Rect position, SerializedProperty gradient)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_GradientHash, EditorGUIUtility.native, position);
      return EditorGUI.DoGradientField(position, controlId, (Gradient) null, gradient);
    }

    internal static Gradient GradientField(string label, Rect position, SerializedProperty property)
    {
      return EditorGUI.GradientField(EditorGUIUtility.TempContent(label), position, property);
    }

    internal static Gradient GradientField(GUIContent label, Rect position, SerializedProperty property)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_GradientHash, EditorGUIUtility.native, position);
      return EditorGUI.DoGradientField(EditorGUI.PrefixLabel(position, controlId, label), controlId, (Gradient) null, property);
    }

    internal static Gradient DoGradientField(Rect position, int id, Gradient value, SerializedProperty property)
    {
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(id);
      switch (typeForControl)
      {
        case EventType.KeyDown:
          if (GUIUtility.keyboardControl == id && (current.keyCode == KeyCode.Space || current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter))
          {
            Event.current.Use();
            GradientPicker.Show(property == null ? value : property.gradientValue);
            GUIUtility.ExitGUI();
            break;
          }
          break;
        case EventType.Repaint:
          Rect position1 = new Rect(position.x + 1f, position.y + 1f, position.width - 2f, position.height - 2f);
          if (property != null)
            GradientEditor.DrawGradientSwatch(position1, property, Color.white);
          else
            GradientEditor.DrawGradientSwatch(position1, value, Color.white);
          EditorStyles.colorPickerBox.Draw(position, GUIContent.none, id);
          break;
        default:
          if (typeForControl != EventType.ValidateCommand)
          {
            if (typeForControl != EventType.ExecuteCommand)
            {
              if (typeForControl == EventType.MouseDown && position.Contains(current.mousePosition))
              {
                if (current.button == 0)
                {
                  EditorGUI.s_GradientID = id;
                  GUIUtility.keyboardControl = id;
                  GradientPicker.Show(property == null ? value : property.gradientValue);
                  GUIUtility.ExitGUI();
                  break;
                }
                if (current.button == 1 && property != null)
                {
                  GradientContextMenu.Show(property);
                  break;
                }
                break;
              }
              break;
            }
            if (EditorGUI.s_GradientID == id && current.commandName == "GradientPickerChanged")
            {
              GUI.changed = true;
              GradientPreviewCache.ClearCache();
              HandleUtility.Repaint();
              if (property != null)
                property.gradientValue = GradientPicker.gradient;
              return GradientPicker.gradient;
            }
            break;
          }
          if (EditorGUI.s_GradientID == id && current.commandName == "UndoRedoPerformed")
          {
            if (property != null)
              GradientPicker.SetCurrentGradient(property.gradientValue);
            GradientPreviewCache.ClearCache();
            return value;
          }
          break;
      }
      return value;
    }

    internal static Color HexColorTextField(Rect rect, GUIContent label, Color color, bool showAlpha)
    {
      return EditorGUI.HexColorTextField(rect, label, color, showAlpha, EditorStyles.textField);
    }

    internal static Color HexColorTextField(Rect rect, GUIContent label, Color color, bool showAlpha, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_FloatFieldHash, FocusType.Keyboard, rect);
      return EditorGUI.DoHexColorTextField(EditorGUI.PrefixLabel(rect, controlId, label), color, showAlpha, style);
    }

    internal static Color DoHexColorTextField(Rect rect, Color color, bool showAlpha, GUIStyle style)
    {
      bool flag = false;
      if ((double) color.maxColorComponent > 1.0)
      {
        color = color.RGBMultiplied(1f / color.maxColorComponent);
        flag = true;
      }
      Rect position = new Rect(rect.x, rect.y, 14f, rect.height);
      rect.xMin += 14f;
      GUI.Label(position, GUIContent.Temp("#"));
      string text = !showAlpha ? ColorUtility.ToHtmlStringRGB(color) : ColorUtility.ToHtmlStringRGBA(color);
      EditorGUI.BeginChangeCheck();
      int controlId = GUIUtility.GetControlID(EditorGUI.s_TextFieldHash, FocusType.Keyboard, rect);
      bool changed;
      string str = EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, controlId, rect, text, style, "0123456789ABCDEFabcdef", out changed, false, false, false);
      if (EditorGUI.EndChangeCheck())
      {
        EditorGUI.s_RecycledEditor.text = EditorGUI.s_RecycledEditor.text.ToUpper();
        Color color1;
        if (ColorUtility.TryParseHtmlString("#" + str, out color1))
          color = new Color(color1.r, color1.g, color1.b, !showAlpha ? color.a : color1.a);
      }
      if (flag)
      {
        EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
        GUI.Label(new Rect(position.x - 20f, rect.y, 20f, 20f), EditorGUI.s_HDRWarning);
        EditorGUIUtility.SetIconSize(Vector2.zero);
      }
      return color;
    }

    internal static bool ButtonMouseDown(Rect position, GUIContent content, FocusType focusType, GUIStyle style)
    {
      return EditorGUI.ButtonMouseDown(GUIUtility.GetControlID(EditorGUI.s_ButtonMouseDownHash, focusType, position), position, content, style);
    }

    internal static bool ButtonMouseDown(int id, Rect position, GUIContent content, GUIStyle style)
    {
      Event current = Event.current;
      EventType type = current.type;
      switch (type)
      {
        case EventType.KeyDown:
          if (GUIUtility.keyboardControl == id && (int) current.character == 32)
          {
            Event.current.Use();
            return true;
          }
          break;
        case EventType.Repaint:
          if (EditorGUI.showMixedValue)
          {
            EditorGUI.BeginHandleMixedValueContentColor();
            style.Draw(position, EditorGUI.s_MixedValueContent, id, false);
            EditorGUI.EndHandleMixedValueContentColor();
            break;
          }
          style.Draw(position, content, id, false);
          break;
        default:
          if (type == EventType.MouseDown && position.Contains(current.mousePosition) && current.button == 0)
          {
            Event.current.Use();
            return true;
          }
          break;
      }
      return false;
    }

    internal static bool IconButton(int id, Rect position, GUIContent content, GUIStyle style)
    {
      GUIUtility.CheckOnGUI();
      switch (Event.current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (!position.Contains(Event.current.mousePosition))
            return false;
          GUIUtility.hotControl = id;
          Event.current.Use();
          return true;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != id)
            return false;
          GUIUtility.hotControl = 0;
          Event.current.Use();
          return position.Contains(Event.current.mousePosition);
        case EventType.MouseDrag:
          if (position.Contains(Event.current.mousePosition))
          {
            GUIUtility.hotControl = id;
            Event.current.Use();
            return true;
          }
          break;
        case EventType.Repaint:
          style.Draw(position, content, id);
          break;
      }
      return false;
    }

    internal static Vector2 MouseDeltaReader(Rect position, bool activated)
    {
      int controlId = GUIUtility.GetControlID(EditorGUI.s_MouseDeltaReaderHash, FocusType.Passive, position);
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (activated && GUIUtility.hotControl == 0 && (position.Contains(current.mousePosition) && current.button == 0))
          {
            GUIUtility.hotControl = controlId;
            GUIUtility.keyboardControl = 0;
            EditorGUI.s_MouseDeltaReaderLastPos = GUIClip.Unclip(current.mousePosition);
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId && current.button == 0)
          {
            GUIUtility.hotControl = 0;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            Vector2 vector2_1 = GUIClip.Unclip(current.mousePosition);
            Vector2 vector2_2 = vector2_1 - EditorGUI.s_MouseDeltaReaderLastPos;
            EditorGUI.s_MouseDeltaReaderLastPos = vector2_1;
            current.Use();
            return vector2_2;
          }
          break;
      }
      return Vector2.zero;
    }

    internal static bool ButtonWithDropdownList(string buttonName, string[] buttonNames, GenericMenu.MenuFunction2 callback, params GUILayoutOption[] options)
    {
      return EditorGUI.ButtonWithDropdownList(EditorGUIUtility.TempContent(buttonName), buttonNames, callback, options);
    }

    internal static bool ButtonWithDropdownList(GUIContent content, string[] buttonNames, GenericMenu.MenuFunction2 callback, params GUILayoutOption[] options)
    {
      Rect rect1 = GUILayoutUtility.GetRect(content, EditorStyles.dropDownList, options);
      Rect rect2 = rect1;
      rect2.xMin = rect2.xMax - 20f;
      if (Event.current.type != EventType.MouseDown || !rect2.Contains(Event.current.mousePosition))
        return GUI.Button(rect1, content, EditorStyles.dropDownList);
      GenericMenu genericMenu = new GenericMenu();
      for (int index = 0; index != buttonNames.Length; ++index)
        genericMenu.AddItem(new GUIContent(buttonNames[index]), false, callback, (object) index);
      genericMenu.DropDown(rect1);
      Event.current.Use();
      return false;
    }

    internal static void GameViewSizePopup(Rect buttonRect, GameViewSizeGroupType groupType, int selectedIndex, System.Action<int, object> itemClickedCallback, GUIStyle guiStyle)
    {
      GameViewSizeGroup group = ScriptableSingleton<GameViewSizes>.instance.GetGroup(groupType);
      string t = selectedIndex < 0 || selectedIndex >= group.GetTotalCount() ? string.Empty : group.GetGameViewSize(selectedIndex).displayText;
      if (!EditorGUI.ButtonMouseDown(buttonRect, GUIContent.Temp(t), FocusType.Passive, guiStyle))
        return;
      FlexibleMenu flexibleMenu = new FlexibleMenu((IFlexibleMenuItemProvider) new GameViewSizesMenuItemProvider(groupType), selectedIndex, (FlexibleMenuModifyItemUI) new GameViewSizesMenuModifyItemUI(), itemClickedCallback);
      PopupWindow.Show(buttonRect, (PopupWindowContent) flexibleMenu);
    }

    /// <summary>
    ///   <para>Draws a filled rectangle of color at the specified position and size within the current editor window.</para>
    /// </summary>
    /// <param name="rect">The position and size of the rectangle to draw.</param>
    /// <param name="color">The color of the rectange.</param>
    public static void DrawRect(Rect rect, Color color)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color color1 = GUI.color;
      GUI.color *= color;
      GUI.DrawTexture(rect, (Texture) EditorGUIUtility.whiteTexture);
      GUI.color = color1;
    }

    internal static float Knob(Rect position, Vector2 knobSize, float currentValue, float start, float end, string unit, Color backgroundColor, Color activeColor, bool showValue, int id)
    {
      return new EditorGUI.KnobContext(position, knobSize, currentValue, start, end, unit, backgroundColor, activeColor, showValue, id).Handle();
    }

    internal static float OffsetKnob(Rect position, float currentValue, float start, float end, float median, string unit, Color backgroundColor, Color activeColor, GUIStyle knob, int id)
    {
      return 0.0f;
    }

    internal static UnityEngine.Object DoObjectField(Rect position, Rect dropRect, int id, UnityEngine.Object obj, System.Type objType, SerializedProperty property, EditorGUI.ObjectFieldValidator validator, bool allowSceneObjects)
    {
      return EditorGUI.DoObjectField(position, dropRect, id, obj, objType, property, validator, allowSceneObjects, EditorStyles.objectField);
    }

    internal static UnityEngine.Object DoObjectField(Rect position, Rect dropRect, int id, UnityEngine.Object obj, System.Type objType, SerializedProperty property, EditorGUI.ObjectFieldValidator validator, bool allowSceneObjects, GUIStyle style)
    {
      if (validator == null)
        validator = new EditorGUI.ObjectFieldValidator(EditorGUI.ValidateObjectFieldAssignment);
      Event current = Event.current;
      EventType eventType1 = current.type;
      if (!GUI.enabled && GUIClip.enabled && Event.current.rawType == EventType.MouseDown)
        eventType1 = Event.current.rawType;
      bool flag1 = EditorGUIUtility.HasObjectThumbnail(objType);
      EditorGUI.ObjectFieldVisualType objectFieldVisualType = EditorGUI.ObjectFieldVisualType.IconAndText;
      if (flag1 && (double) position.height <= 18.0 && (double) position.width <= 32.0)
        objectFieldVisualType = EditorGUI.ObjectFieldVisualType.MiniPreivew;
      else if (flag1 && (double) position.height > 16.0)
        objectFieldVisualType = EditorGUI.ObjectFieldVisualType.LargePreview;
      Vector2 iconSize = EditorGUIUtility.GetIconSize();
      if (objectFieldVisualType == EditorGUI.ObjectFieldVisualType.IconAndText)
        EditorGUIUtility.SetIconSize(new Vector2(12f, 12f));
      else if (objectFieldVisualType == EditorGUI.ObjectFieldVisualType.LargePreview)
        EditorGUIUtility.SetIconSize(new Vector2(64f, 64f));
      EventType eventType2 = eventType1;
      switch (eventType2)
      {
        case EventType.KeyDown:
          if (GUIUtility.keyboardControl == id)
          {
            if (current.keyCode == KeyCode.Backspace || current.keyCode == KeyCode.Delete)
            {
              if (property != null)
                property.objectReferenceValue = (UnityEngine.Object) null;
              else
                obj = (UnityEngine.Object) null;
              GUI.changed = true;
              current.Use();
            }
            if (current.MainActionKeyForControl(id))
            {
              ObjectSelector.get.Show(obj, objType, property, allowSceneObjects);
              ObjectSelector.get.objectSelectorID = id;
              current.Use();
              GUIUtility.ExitGUI();
              break;
            }
            break;
          }
          break;
        case EventType.Repaint:
          GUIContent content;
          if (EditorGUI.showMixedValue)
            content = EditorGUI.s_MixedValueContent;
          else if (property != null)
          {
            content = EditorGUIUtility.TempContent(property.objectReferenceStringValue, (Texture) AssetPreview.GetMiniThumbnail(property.objectReferenceValue));
            obj = property.objectReferenceValue;
            if (obj != (UnityEngine.Object) null)
            {
              UnityEngine.Object[] references = new UnityEngine.Object[1]{ obj };
              if (validator(references, objType, property) == (UnityEngine.Object) null)
                content = EditorGUIUtility.TempContent("Type mismatch");
            }
          }
          else
            content = EditorGUIUtility.ObjectContent(obj, objType);
          switch (objectFieldVisualType)
          {
            case EditorGUI.ObjectFieldVisualType.IconAndText:
              EditorGUI.BeginHandleMixedValueContentColor();
              style.Draw(position, content, id, DragAndDrop.activeControlID == id);
              EditorGUI.EndHandleMixedValueContentColor();
              break;
            case EditorGUI.ObjectFieldVisualType.LargePreview:
              EditorGUI.DrawObjectFieldLargeThumb(position, id, obj, content);
              break;
            case EditorGUI.ObjectFieldVisualType.MiniPreivew:
              EditorGUI.DrawObjectFieldMiniThumb(position, id, obj, content);
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        case EventType.DragUpdated:
        case EventType.DragPerform:
          if (dropRect.Contains(Event.current.mousePosition) && GUI.enabled)
          {
            UnityEngine.Object[] objectReferences = DragAndDrop.objectReferences;
            UnityEngine.Object target = validator(objectReferences, objType, property);
            if (target != (UnityEngine.Object) null && !allowSceneObjects && !EditorUtility.IsPersistent(target))
              target = (UnityEngine.Object) null;
            if (target != (UnityEngine.Object) null)
            {
              DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
              if (eventType1 == EventType.DragPerform)
              {
                if (property != null)
                  property.objectReferenceValue = target;
                else
                  obj = target;
                GUI.changed = true;
                DragAndDrop.AcceptDrag();
                DragAndDrop.activeControlID = 0;
              }
              else
                DragAndDrop.activeControlID = id;
              Event.current.Use();
              break;
            }
            break;
          }
          break;
        case EventType.ExecuteCommand:
          if (current.commandName == "ObjectSelectorUpdated" && ObjectSelector.get.objectSelectorID == id && GUIUtility.keyboardControl == id)
          {
            UnityEngine.Object[] references = new UnityEngine.Object[1]
            {
              ObjectSelector.GetCurrentObject()
            };
            UnityEngine.Object @object = validator(references, objType, property);
            if (property != null)
              property.objectReferenceValue = @object;
            GUI.changed = true;
            current.Use();
            return @object;
          }
          break;
        case EventType.DragExited:
          if (GUI.enabled)
          {
            HandleUtility.Repaint();
            break;
          }
          break;
        default:
          if (eventType2 == EventType.MouseDown && Event.current.button == 0 && position.Contains(Event.current.mousePosition))
          {
            Rect rect;
            switch (objectFieldVisualType)
            {
              case EditorGUI.ObjectFieldVisualType.IconAndText:
                rect = new Rect(position.xMax - 15f, position.y, 15f, position.height);
                break;
              case EditorGUI.ObjectFieldVisualType.LargePreview:
                rect = new Rect(position.xMax - 36f, position.yMax - 14f, 36f, 14f);
                break;
              case EditorGUI.ObjectFieldVisualType.MiniPreivew:
                rect = new Rect(position.xMax - 15f, position.y, 15f, position.height);
                break;
              default:
                throw new ArgumentOutOfRangeException();
            }
            EditorGUIUtility.editingTextField = false;
            if (rect.Contains(Event.current.mousePosition))
            {
              if (GUI.enabled)
              {
                GUIUtility.keyboardControl = id;
                ObjectSelector.get.Show(obj, objType, property, allowSceneObjects);
                ObjectSelector.get.objectSelectorID = id;
                current.Use();
                GUIUtility.ExitGUI();
                break;
              }
              break;
            }
            UnityEngine.Object @object = property == null ? obj : property.objectReferenceValue;
            Component component = @object as Component;
            if ((bool) ((UnityEngine.Object) component))
              @object = (UnityEngine.Object) component.gameObject;
            if (EditorGUI.showMixedValue)
              @object = (UnityEngine.Object) null;
            if (Event.current.clickCount == 1)
            {
              GUIUtility.keyboardControl = id;
              if ((bool) @object)
              {
                bool flag2 = current.shift || current.control;
                if (!flag2)
                  EditorGUIUtility.PingObject(@object);
                if (flag2 && @object is Texture)
                  PopupWindowWithoutFocus.Show(new RectOffset(6, 3, 0, 3).Add(position), (PopupWindowContent) new ObjectPreviewPopup(@object), new PopupLocationHelper.PopupLocation[3]
                  {
                    PopupLocationHelper.PopupLocation.Left,
                    PopupLocationHelper.PopupLocation.Below,
                    PopupLocationHelper.PopupLocation.Right
                  });
              }
              current.Use();
              break;
            }
            if (Event.current.clickCount == 2)
            {
              if ((bool) @object)
              {
                AssetDatabase.OpenAsset(@object);
                GUIUtility.ExitGUI();
              }
              current.Use();
              break;
            }
            break;
          }
          break;
      }
      EditorGUIUtility.SetIconSize(iconSize);
      return obj;
    }

    private static void DrawObjectFieldLargeThumb(Rect position, int id, UnityEngine.Object obj, GUIContent content)
    {
      GUIStyle objectFieldThumb = EditorStyles.objectFieldThumb;
      objectFieldThumb.Draw(position, GUIContent.none, id, DragAndDrop.activeControlID == id);
      if (obj != (UnityEngine.Object) null && !EditorGUI.showMixedValue)
      {
        bool flag1 = obj is Cubemap;
        bool flag2 = obj is Sprite;
        Rect position1 = objectFieldThumb.padding.Remove(position);
        if (flag1 || flag2)
        {
          Texture2D assetPreview = AssetPreview.GetAssetPreview(obj);
          if ((UnityEngine.Object) assetPreview != (UnityEngine.Object) null)
          {
            if (flag2 || assetPreview.alphaIsTransparency)
              EditorGUI.DrawTextureTransparent(position1, (Texture) assetPreview);
            else
              EditorGUI.DrawPreviewTexture(position1, (Texture) assetPreview);
          }
          else
          {
            position1.x += (float) (((double) position1.width - (double) content.image.width) / 2.0);
            position1.y += (float) (((double) position1.height - (double) content.image.width) / 2.0);
            GUIStyle.none.Draw(position1, content.image, false, false, false, false);
            HandleUtility.Repaint();
          }
        }
        else
        {
          Texture2D image = content.image as Texture2D;
          if ((UnityEngine.Object) image != (UnityEngine.Object) null && image.alphaIsTransparency)
            EditorGUI.DrawTextureTransparent(position1, (Texture) image);
          else
            EditorGUI.DrawPreviewTexture(position1, content.image);
        }
      }
      else
      {
        GUIStyle guiStyle = (GUIStyle) (objectFieldThumb.name + "Overlay");
        EditorGUI.BeginHandleMixedValueContentColor();
        guiStyle.Draw(position, content, id);
        EditorGUI.EndHandleMixedValueContentColor();
      }
      (GUIStyle) (objectFieldThumb.name + "Overlay2").Draw(position, EditorGUIUtility.TempContent("Select"), id);
    }

    private static void DrawObjectFieldMiniThumb(Rect position, int id, UnityEngine.Object obj, GUIContent content)
    {
      GUIStyle objectFieldMiniThumb = EditorStyles.objectFieldMiniThumb;
      position.width = 32f;
      EditorGUI.BeginHandleMixedValueContentColor();
      bool isHover = obj != (UnityEngine.Object) null;
      bool on = DragAndDrop.activeControlID == id;
      bool hasKeyboardFocus = GUIUtility.keyboardControl == id;
      objectFieldMiniThumb.Draw(position, isHover, false, on, hasKeyboardFocus);
      EditorGUI.EndHandleMixedValueContentColor();
      if (!(obj != (UnityEngine.Object) null) || EditorGUI.showMixedValue)
        return;
      Rect position1 = new Rect(position.x + 1f, position.y + 1f, position.height - 2f, position.height - 2f);
      Texture2D image = content.image as Texture2D;
      if ((UnityEngine.Object) image != (UnityEngine.Object) null && image.alphaIsTransparency)
        EditorGUI.DrawTextureTransparent(position1, (Texture) image);
      else
        EditorGUI.DrawPreviewTexture(position1, content.image);
      if (!position1.Contains(Event.current.mousePosition))
        return;
      GUI.Label(position1, GUIContent.Temp(string.Empty, "Ctrl + Click to show preview"));
    }

    internal static UnityEngine.Object DoDropField(Rect position, int id, System.Type objType, EditorGUI.ObjectFieldValidator validator, bool allowSceneObjects, GUIStyle style)
    {
      if (validator == null)
        validator = new EditorGUI.ObjectFieldValidator(EditorGUI.ValidateObjectFieldAssignment);
      EventType eventType1 = Event.current.type;
      if (!GUI.enabled && GUIClip.enabled && Event.current.rawType == EventType.MouseDown)
        eventType1 = Event.current.rawType;
      EventType eventType2 = eventType1;
      switch (eventType2)
      {
        case EventType.Repaint:
          style.Draw(position, GUIContent.none, id, DragAndDrop.activeControlID == id);
          break;
        case EventType.DragUpdated:
        case EventType.DragPerform:
          if (position.Contains(Event.current.mousePosition) && GUI.enabled)
          {
            UnityEngine.Object[] objectReferences = DragAndDrop.objectReferences;
            UnityEngine.Object target = validator(objectReferences, objType, (SerializedProperty) null);
            if (target != (UnityEngine.Object) null && !allowSceneObjects && !EditorUtility.IsPersistent(target))
              target = (UnityEngine.Object) null;
            if (target != (UnityEngine.Object) null)
            {
              DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
              if (eventType1 == EventType.DragPerform)
              {
                GUI.changed = true;
                DragAndDrop.AcceptDrag();
                DragAndDrop.activeControlID = 0;
                Event.current.Use();
                return target;
              }
              DragAndDrop.activeControlID = id;
              Event.current.Use();
              break;
            }
            break;
          }
          break;
        default:
          if (eventType2 == EventType.DragExited && GUI.enabled)
          {
            HandleUtility.Repaint();
            break;
          }
          break;
      }
      return (UnityEngine.Object) null;
    }

    internal static void TargetChoiceField(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.TargetChoiceField(position, property, label, new TargetChoiceHandler.TargetChoiceMenuFunction(TargetChoiceHandler.SetToValueOfTarget));
    }

    internal static void TargetChoiceField(Rect position, SerializedProperty property, GUIContent label, TargetChoiceHandler.TargetChoiceMenuFunction func)
    {
      EditorGUI.BeginProperty(position, label, property);
      position = EditorGUI.PrefixLabel(position, 0, label);
      EditorGUI.BeginHandleMixedValueContentColor();
      if (GUI.Button(position, EditorGUI.mixedValueContent, EditorStyles.popup))
      {
        GenericMenu menu = new GenericMenu();
        TargetChoiceHandler.AddSetToValueOfTargetMenuItems(menu, property, func);
        menu.DropDown(position);
      }
      EditorGUI.EndHandleMixedValueContentColor();
      EditorGUI.EndProperty();
    }

    internal static string DoTextFieldDropDown(Rect rect, int id, string text, string[] dropDownElements, bool delayed)
    {
      Rect position1 = new Rect(rect.x, rect.y, rect.width - EditorStyles.textFieldDropDown.fixedWidth, rect.height);
      Rect rect1 = new Rect(position1.xMax, position1.y, EditorStyles.textFieldDropDown.fixedWidth, rect.height);
      bool changed;
      text = !delayed ? EditorGUI.DoTextField(EditorGUI.s_RecycledEditor, id, position1, text, EditorStyles.textFieldDropDownText, (string) null, out changed, false, false, false) : EditorGUI.DelayedTextField(position1, text, EditorStyles.textFieldDropDownText);
      EditorGUI.BeginChangeCheck();
      int indentLevel = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;
      Rect position2 = rect1;
      string empty = string.Empty;
      int selectedIndex = -1;
      string[] displayedOptions;
      if (dropDownElements.Length > 0)
        displayedOptions = dropDownElements;
      else
        displayedOptions = new string[1]{ "--empty--" };
      GUIStyle textFieldDropDown = EditorStyles.textFieldDropDown;
      int index = EditorGUI.Popup(position2, empty, selectedIndex, displayedOptions, textFieldDropDown);
      if (EditorGUI.EndChangeCheck() && dropDownElements.Length > 0)
        text = dropDownElements[index];
      EditorGUI.indentLevel = indentLevel;
      return text;
    }

    /// <summary>
    ///   <para>Create a group of controls that can be disabled.</para>
    /// </summary>
    public class DisabledGroupScope : GUI.Scope
    {
      /// <summary>
      ///   <para>Create a new DisabledGroupScope and begin the corresponding group.</para>
      /// </summary>
      /// <param name="disabled">Boolean specifying if the controls inside the group should be disabled.</param>
      public DisabledGroupScope(bool disabled)
      {
        EditorGUI.BeginDisabledGroup(disabled);
      }

      protected override void CloseScope()
      {
        EditorGUI.EndDisabledGroup();
      }
    }

    internal class RecycledTextEditor : TextEditor
    {
      internal static bool s_AllowContextCutOrPaste = true;
      internal static bool s_ActuallyEditing;

      internal bool IsEditingControl(int id)
      {
        if (GUIUtility.keyboardControl == id && this.controlID == id && EditorGUI.RecycledTextEditor.s_ActuallyEditing)
          return GUIView.current.hasFocus;
        return false;
      }

      public virtual void BeginEditing(int id, string newText, Rect position, GUIStyle style, bool multiline, bool passwordField)
      {
        if (this.IsEditingControl(id))
          return;
        if (EditorGUI.activeEditor != null)
          EditorGUI.activeEditor.EndEditing();
        EditorGUI.activeEditor = this;
        this.controlID = id;
        this.text = EditorGUI.s_OriginalText = newText;
        this.multiline = multiline;
        this.style = style;
        this.position = position;
        this.isPasswordField = passwordField;
        EditorGUI.RecycledTextEditor.s_ActuallyEditing = true;
        this.scrollOffset = Vector2.zero;
        Undo.IncrementCurrentGroup();
      }

      public virtual void EndEditing()
      {
        if (EditorGUI.activeEditor == this)
          EditorGUI.activeEditor = (EditorGUI.RecycledTextEditor) null;
        this.controlID = 0;
        EditorGUI.RecycledTextEditor.s_ActuallyEditing = false;
        EditorGUI.RecycledTextEditor.s_AllowContextCutOrPaste = true;
        Undo.IncrementCurrentGroup();
      }
    }

    internal sealed class DelayedTextEditor : EditorGUI.RecycledTextEditor
    {
      internal string controlThatHadFocusValue = string.Empty;
      private int controlThatHadFocus;
      private int messageControl;
      private GUIView viewThatHadFocus;
      private int controlThatLostFocus;
      private bool m_IgnoreBeginGUI;

      public void BeginGUI()
      {
        if (this.m_IgnoreBeginGUI)
          return;
        if (GUIUtility.keyboardControl == this.controlID)
        {
          this.controlThatHadFocus = GUIUtility.keyboardControl;
          this.controlThatHadFocusValue = this.text;
          this.viewThatHadFocus = GUIView.current;
        }
        else
          this.controlThatHadFocus = 0;
      }

      public void EndGUI(EventType type)
      {
        int num = 0;
        if (this.controlThatLostFocus != 0)
        {
          this.messageControl = this.controlThatLostFocus;
          this.controlThatLostFocus = 0;
        }
        if (this.controlThatHadFocus != 0 && this.controlThatHadFocus != GUIUtility.keyboardControl)
        {
          num = this.controlThatHadFocus;
          this.controlThatHadFocus = 0;
        }
        if (num == 0)
          return;
        this.messageControl = num;
        this.m_IgnoreBeginGUI = true;
        this.viewThatHadFocus.SendEvent(EditorGUIUtility.CommandEvent("DelayedControlShouldCommit"));
        this.m_IgnoreBeginGUI = false;
        this.messageControl = 0;
      }

      public override void EndEditing()
      {
        base.EndEditing();
        this.messageControl = 0;
      }

      public string OnGUI(int id, string value, out bool changed)
      {
        Event current = Event.current;
        if (current.type == EventType.ExecuteCommand && current.commandName == "DelayedControlShouldCommit" && id == this.messageControl)
        {
          changed = value != this.controlThatHadFocusValue;
          current.Use();
          return this.controlThatHadFocusValue;
        }
        changed = false;
        return value;
      }
    }

    internal sealed class PopupMenuEvent
    {
      public string commandName;
      public GUIView receiver;

      public PopupMenuEvent(string cmd, GUIView v)
      {
        this.commandName = cmd;
        this.receiver = v;
      }

      public void SendEvent()
      {
        if ((bool) ((UnityEngine.Object) this.receiver))
          this.receiver.SendEvent(EditorGUIUtility.CommandEvent(this.commandName));
        else
          Debug.LogError((object) "BUG: We don't have a receiver set up, please report");
      }
    }

    internal sealed class PopupCallbackInfo
    {
      internal const string kPopupMenuChangedMessage = "PopupMenuChanged";
      public static EditorGUI.PopupCallbackInfo instance;
      private int m_ControlID;
      private int m_SelectedIndex;
      private GUIView m_SourceView;

      public PopupCallbackInfo(int controlID)
      {
        this.m_ControlID = controlID;
        this.m_SourceView = GUIView.current;
      }

      public static int GetSelectedValueForControl(int controlID, int selected)
      {
        Event current = Event.current;
        if (current.type == EventType.ExecuteCommand && current.commandName == "PopupMenuChanged")
        {
          if (EditorGUI.PopupCallbackInfo.instance == null)
          {
            Debug.LogError((object) "Popup menu has no instance");
            return selected;
          }
          if (EditorGUI.PopupCallbackInfo.instance.m_ControlID == controlID)
          {
            selected = EditorGUI.PopupCallbackInfo.instance.m_SelectedIndex;
            EditorGUI.PopupCallbackInfo.instance = (EditorGUI.PopupCallbackInfo) null;
            GUI.changed = true;
            current.Use();
          }
        }
        return selected;
      }

      internal void SetEnumValueDelegate(object userData, string[] options, int selected)
      {
        this.m_SelectedIndex = selected;
        if (!(bool) ((UnityEngine.Object) this.m_SourceView))
          return;
        this.m_SourceView.SendEvent(EditorGUIUtility.CommandEvent("PopupMenuChanged"));
      }
    }

    /// <summary>
    ///   <para>Create a Property wrapper, useful for making regular GUI controls work with SerializedProperty.</para>
    /// </summary>
    public class PropertyScope : GUI.Scope
    {
      /// <summary>
      ///   <para>The actual label to use for the control.</para>
      /// </summary>
      public GUIContent content { get; protected set; }

      /// <summary>
      ///   <para>Create a new PropertyScope and begin the corresponding property.</para>
      /// </summary>
      /// <param name="totalPosition">Rectangle on the screen to use for the control, including label if applicable.</param>
      /// <param name="label">Label in front of the slider. Use null to use the name from the SerializedProperty. Use GUIContent.none to not display a label.</param>
      /// <param name="property">The SerializedProperty to use for the control.</param>
      public PropertyScope(Rect totalPosition, GUIContent label, SerializedProperty property)
      {
        this.content = EditorGUI.BeginProperty(totalPosition, label, property);
      }

      protected override void CloseScope()
      {
        EditorGUI.EndProperty();
      }
    }

    private class ColorBrightnessFieldStateObject
    {
      public float m_Hue;
      public float m_Saturation;
      public float m_Brightness;
    }

    internal sealed class GUIContents
    {
      [EditorGUI.GUIContents.IconName("_Popup")]
      internal static GUIContent titleSettingsIcon { get; private set; }

      [EditorGUI.GUIContents.IconName("_Help")]
      internal static GUIContent helpIcon { get; private set; }

      static GUIContents()
      {
        foreach (PropertyInfo property in typeof (EditorGUI.GUIContents).GetProperties(BindingFlags.Static | BindingFlags.NonPublic))
        {
          EditorGUI.GUIContents.IconName[] customAttributes = (EditorGUI.GUIContents.IconName[]) property.GetCustomAttributes(typeof (EditorGUI.GUIContents.IconName), false);
          if (customAttributes.Length > 0)
          {
            GUIContent guiContent = EditorGUIUtility.IconContent(customAttributes[0].name);
            property.SetValue((object) null, (object) guiContent, (object[]) null);
          }
        }
      }

      private class IconName : Attribute
      {
        private string m_Name;

        public virtual string name
        {
          get
          {
            return this.m_Name;
          }
        }

        public IconName(string name)
        {
          this.m_Name = name;
        }
      }
    }

    internal struct KnobContext
    {
      private const int kPixelRange = 250;
      private readonly Rect position;
      private readonly Vector2 knobSize;
      private readonly float currentValue;
      private readonly float start;
      private readonly float end;
      private readonly string unit;
      private readonly Color activeColor;
      private readonly Color backgroundColor;
      private readonly bool showValue;
      private readonly int id;
      private static Material knobMaterial;

      public KnobContext(Rect position, Vector2 knobSize, float currentValue, float start, float end, string unit, Color backgroundColor, Color activeColor, bool showValue, int id)
      {
        this.position = position;
        this.knobSize = knobSize;
        this.currentValue = currentValue;
        this.start = start;
        this.end = end;
        this.unit = unit;
        this.activeColor = activeColor;
        this.backgroundColor = backgroundColor;
        this.showValue = showValue;
        this.id = id;
      }

      public float Handle()
      {
        if (this.KnobState().isEditing && this.CurrentEventType() != EventType.Repaint)
          return this.DoKeyboardInput();
        switch (this.CurrentEventType())
        {
          case EventType.MouseDown:
            return this.OnMouseDown();
          case EventType.MouseUp:
            return this.OnMouseUp();
          case EventType.MouseDrag:
            return this.OnMouseDrag();
          case EventType.Repaint:
            return this.OnRepaint();
          default:
            return this.currentValue;
        }
      }

      private EventType CurrentEventType()
      {
        return this.CurrentEvent().type;
      }

      private bool IsEmptyKnob()
      {
        return (double) this.start == (double) this.end;
      }

      private Event CurrentEvent()
      {
        return Event.current;
      }

      private float Clamp(float value)
      {
        return Mathf.Clamp(value, this.MinValue(), this.MaxValue());
      }

      private float ClampedCurrentValue()
      {
        return this.Clamp(this.currentValue);
      }

      private float MaxValue()
      {
        return Mathf.Max(this.start, this.end);
      }

      private float MinValue()
      {
        return Mathf.Min(this.start, this.end);
      }

      private float GetCurrentValuePercent()
      {
        return (float) (((double) this.ClampedCurrentValue() - (double) this.MinValue()) / ((double) this.MaxValue() - (double) this.MinValue()));
      }

      private float MousePosition()
      {
        return this.CurrentEvent().mousePosition.y - this.position.y;
      }

      private bool WasDoubleClick()
      {
        if (this.CurrentEventType() == EventType.MouseDown)
          return this.CurrentEvent().clickCount == 2;
        return false;
      }

      private float ValuesPerPixel()
      {
        return (float) (250.0 / ((double) this.MaxValue() - (double) this.MinValue()));
      }

      private KnobState KnobState()
      {
        return (KnobState) GUIUtility.GetStateObject(typeof (KnobState), this.id);
      }

      private void StartDraggingWithValue(float dragStartValue)
      {
        KnobState knobState = this.KnobState();
        knobState.dragStartPos = this.MousePosition();
        knobState.dragStartValue = dragStartValue;
        knobState.isDragging = true;
      }

      private float OnMouseDown()
      {
        if (!this.position.Contains(this.CurrentEvent().mousePosition) || this.KnobState().isEditing || this.IsEmptyKnob())
          return this.currentValue;
        GUIUtility.hotControl = this.id;
        if (this.WasDoubleClick())
          this.KnobState().isEditing = true;
        else
          this.StartDraggingWithValue(this.ClampedCurrentValue());
        this.CurrentEvent().Use();
        return this.currentValue;
      }

      private float OnMouseDrag()
      {
        if (GUIUtility.hotControl != this.id)
          return this.currentValue;
        KnobState knobState = this.KnobState();
        if (!knobState.isDragging)
          return this.currentValue;
        GUI.changed = true;
        this.CurrentEvent().Use();
        float num = knobState.dragStartPos - this.MousePosition();
        return this.Clamp(knobState.dragStartValue + num / this.ValuesPerPixel());
      }

      private float OnMouseUp()
      {
        if (GUIUtility.hotControl == this.id)
        {
          this.CurrentEvent().Use();
          GUIUtility.hotControl = 0;
          this.KnobState().isDragging = false;
        }
        return this.currentValue;
      }

      private void PrintValue()
      {
        GUI.Label(new Rect((float) ((double) this.position.x + (double) this.knobSize.x / 2.0 - 8.0), (float) ((double) this.position.y + (double) this.knobSize.y / 2.0 - 8.0), this.position.width, 20f), this.currentValue.ToString("0.##") + " " + this.unit);
      }

      private float DoKeyboardInput()
      {
        GUI.SetNextControlName("KnobInput");
        GUI.FocusControl("KnobInput");
        EditorGUI.BeginChangeCheck();
        Rect position = new Rect((float) ((double) this.position.x + (double) this.knobSize.x / 2.0 - 6.0), (float) ((double) this.position.y + (double) this.knobSize.y / 2.0 - 7.0), this.position.width, 20f);
        GUIStyle none = GUIStyle.none;
        none.normal.textColor = new Color(0.703f, 0.703f, 0.703f, 1f);
        none.fontStyle = FontStyle.Normal;
        string s = EditorGUI.DelayedTextField(position, this.currentValue.ToString("0.##"), none);
        if (EditorGUI.EndChangeCheck() && !string.IsNullOrEmpty(s))
        {
          this.KnobState().isEditing = false;
          float result;
          if (float.TryParse(s, out result) && (double) result != (double) this.currentValue)
            return this.Clamp(result);
        }
        return this.currentValue;
      }

      private static void CreateKnobMaterial()
      {
        if ((bool) ((UnityEngine.Object) EditorGUI.KnobContext.knobMaterial))
          return;
        EditorGUI.KnobContext.knobMaterial = new Material(Resources.GetBuiltinResource(typeof (Shader), "Internal-GUITextureClip.shader") as Shader);
        EditorGUI.KnobContext.knobMaterial.hideFlags = HideFlags.HideAndDontSave;
        EditorGUI.KnobContext.knobMaterial.mainTexture = (Texture) EditorGUIUtility.FindTexture("KnobCShape");
        EditorGUI.KnobContext.knobMaterial.name = "Knob Material";
        if (!((UnityEngine.Object) EditorGUI.KnobContext.knobMaterial.mainTexture == (UnityEngine.Object) null))
          return;
        Debug.Log((object) "Did not find 'KnobCShape'");
      }

      private Vector3 GetUVForPoint(Vector3 point)
      {
        return new Vector3((point.x - this.position.x) / this.knobSize.x, (float) (((double) point.y - (double) this.position.y - (double) this.knobSize.y) / -(double) this.knobSize.y));
      }

      private void VertexPointWithUV(Vector3 point)
      {
        GL.TexCoord(this.GetUVForPoint(point));
        GL.Vertex(point);
      }

      private void DrawValueArc(float angle)
      {
        EditorGUI.KnobContext.CreateKnobMaterial();
        Vector3 point1 = new Vector3(this.position.x + this.knobSize.x / 2f, this.position.y + this.knobSize.y / 2f, 0.0f);
        Vector3 point2 = new Vector3(this.position.x + this.knobSize.x, this.position.y + this.knobSize.y / 2f, 0.0f);
        Vector3 point3 = new Vector3(this.position.x + this.knobSize.x, this.position.y + this.knobSize.y, 0.0f);
        Vector3 point4 = new Vector3(this.position.x, this.position.y + this.knobSize.y, 0.0f);
        Vector3 point5 = new Vector3(this.position.x, this.position.y, 0.0f);
        Vector3 point6 = new Vector3(this.position.x + this.knobSize.x, this.position.y, 0.0f);
        EditorGUI.KnobContext.knobMaterial.SetPass(0);
        GL.Begin(7);
        GL.Color(this.backgroundColor);
        this.VertexPointWithUV(point5);
        this.VertexPointWithUV(point6);
        this.VertexPointWithUV(point3);
        this.VertexPointWithUV(point4);
        GL.End();
        GL.Begin(4);
        GL.Color(this.activeColor * (!GUI.enabled ? 0.5f : 1f));
        if ((double) angle > 0.0)
        {
          this.VertexPointWithUV(point1);
          this.VertexPointWithUV(point2);
          this.VertexPointWithUV(point3);
          Vector3 point7 = point3;
          if ((double) angle > 1.57079637050629)
          {
            this.VertexPointWithUV(point1);
            this.VertexPointWithUV(point3);
            this.VertexPointWithUV(point4);
            point7 = point4;
            if ((double) angle > 3.14159274101257)
            {
              this.VertexPointWithUV(point1);
              this.VertexPointWithUV(point4);
              this.VertexPointWithUV(point5);
              point7 = point5;
            }
          }
          if ((double) angle == 4.71238899230957)
          {
            this.VertexPointWithUV(point1);
            this.VertexPointWithUV(point5);
            this.VertexPointWithUV(point6);
            this.VertexPointWithUV(point1);
            this.VertexPointWithUV(point6);
            this.VertexPointWithUV(point2);
          }
          else
          {
            float num = angle + 0.7853982f;
            Vector3 point8 = (double) angle >= 1.57079637050629 ? ((double) angle >= 3.14159274101257 ? point5 + new Vector3((float) (-((double) this.knobSize.y / 2.0 * (double) Mathf.Tan(4.712389f - num)) + (double) this.knobSize.x / 2.0), 0.0f, 0.0f) : point4 + new Vector3(0.0f, (float) ((double) this.knobSize.x / 2.0 * (double) Mathf.Tan(3.141593f - num) - (double) this.knobSize.y / 2.0), 0.0f)) : point3 + new Vector3((float) ((double) this.knobSize.y / 2.0 * (double) Mathf.Tan(1.570796f - num) - (double) this.knobSize.x / 2.0), 0.0f, 0.0f);
            this.VertexPointWithUV(point1);
            this.VertexPointWithUV(point7);
            this.VertexPointWithUV(point8);
          }
        }
        GL.End();
      }

      private float OnRepaint()
      {
        this.DrawValueArc((float) ((double) this.GetCurrentValuePercent() * 3.14159274101257 * 1.5));
        if (this.KnobState().isEditing)
          return this.DoKeyboardInput();
        if (this.showValue)
          this.PrintValue();
        return this.currentValue;
      }
    }

    internal enum ObjectFieldVisualType
    {
      IconAndText,
      LargePreview,
      MiniPreivew,
    }

    internal class VUMeter
    {
      private const float VU_SPLIT = 0.9f;
      private static Texture2D s_VerticalVUTexture;
      private static Texture2D s_HorizontalVUTexture;

      public static Texture2D verticalVUTexture
      {
        get
        {
          if ((UnityEngine.Object) EditorGUI.VUMeter.s_VerticalVUTexture == (UnityEngine.Object) null)
            EditorGUI.VUMeter.s_VerticalVUTexture = EditorGUIUtility.LoadIcon("VUMeterTextureVertical");
          return EditorGUI.VUMeter.s_VerticalVUTexture;
        }
      }

      public static Texture2D horizontalVUTexture
      {
        get
        {
          if ((UnityEngine.Object) EditorGUI.VUMeter.s_HorizontalVUTexture == (UnityEngine.Object) null)
            EditorGUI.VUMeter.s_HorizontalVUTexture = EditorGUIUtility.LoadIcon("VUMeterTextureHorizontal");
          return EditorGUI.VUMeter.s_HorizontalVUTexture;
        }
      }

      public static void HorizontalMeter(Rect position, float value, float peak, Texture2D foregroundTexture, Color peakColor)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        Color color = GUI.color;
        EditorStyles.progressBarBack.Draw(position, false, false, false, false);
        GUI.color = new Color(1f, 1f, 1f, !GUI.enabled ? 0.5f : 1f);
        float width = (float) ((double) position.width * (double) value - 2.0);
        if ((double) width < 2.0)
          width = 2f;
        Rect position1 = new Rect(position.x + 1f, position.y + 1f, width, position.height - 2f);
        Rect texCoords = new Rect(0.0f, 0.0f, value, 1f);
        GUI.DrawTextureWithTexCoords(position1, (Texture) foregroundTexture, texCoords);
        GUI.color = peakColor;
        float num = (float) ((double) position.width * (double) peak - 2.0);
        if ((double) num < 2.0)
          num = 2f;
        position1 = new Rect(position.x + num, position.y + 1f, 1f, position.height - 2f);
        GUI.DrawTexture(position1, (Texture) EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill);
        GUI.color = color;
      }

      public static void VerticalMeter(Rect position, float value, float peak, Texture2D foregroundTexture, Color peakColor)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        Color color = GUI.color;
        EditorStyles.progressBarBack.Draw(position, false, false, false, false);
        GUI.color = new Color(1f, 1f, 1f, !GUI.enabled ? 0.5f : 1f);
        float height = (position.height - 2f) * value;
        if ((double) height < 2.0)
          height = 2f;
        Rect position1 = new Rect(position.x + 1f, (float) ((double) position.y + (double) position.height - 1.0) - height, position.width - 2f, height);
        Rect texCoords = new Rect(0.0f, 0.0f, 1f, value);
        GUI.DrawTextureWithTexCoords(position1, (Texture) foregroundTexture, texCoords);
        GUI.color = peakColor;
        float num = (position.height - 2f) * peak;
        if ((double) num < 2.0)
          num = 2f;
        position1 = new Rect(position.x + 1f, (float) ((double) position.y + (double) position.height - 1.0) - num, position.width - 2f, 1f);
        GUI.DrawTexture(position1, (Texture) EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill);
        GUI.color = color;
      }

      public static void HorizontalMeter(Rect position, float value, ref EditorGUI.VUMeter.SmoothingData data, Texture2D foregroundTexture, Color peakColor)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        float renderValue;
        float renderPeak;
        EditorGUI.VUMeter.SmoothVUMeterData(ref value, ref data, out renderValue, out renderPeak);
        EditorGUI.VUMeter.HorizontalMeter(position, renderValue, renderPeak, foregroundTexture, peakColor);
      }

      public static void VerticalMeter(Rect position, float value, ref EditorGUI.VUMeter.SmoothingData data, Texture2D foregroundTexture, Color peakColor)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        float renderValue;
        float renderPeak;
        EditorGUI.VUMeter.SmoothVUMeterData(ref value, ref data, out renderValue, out renderPeak);
        EditorGUI.VUMeter.VerticalMeter(position, renderValue, renderPeak, foregroundTexture, peakColor);
      }

      private static void SmoothVUMeterData(ref float value, ref EditorGUI.VUMeter.SmoothingData data, out float renderValue, out float renderPeak)
      {
        if ((double) value <= (double) data.lastValue)
        {
          value = Mathf.Lerp(data.lastValue, value, Time.smoothDeltaTime * 7f);
        }
        else
        {
          value = Mathf.Lerp(value, data.lastValue, Time.smoothDeltaTime * 2f);
          data.peakValue = value;
          data.peakValueTime = Time.realtimeSinceStartup;
        }
        if ((double) value > 1.11111116409302)
          value = 1.111111f;
        if ((double) data.peakValue > 1.11111116409302)
          data.peakValue = 1.111111f;
        renderValue = value * 0.9f;
        renderPeak = data.peakValue * 0.9f;
        data.lastValue = value;
      }

      public struct SmoothingData
      {
        public float lastValue;
        public float peakValue;
        public float peakValueTime;
      }
    }

    internal delegate UnityEngine.Object ObjectFieldValidator(UnityEngine.Object[] references, System.Type objType, SerializedProperty property);
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorStyles
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Common GUIStyles used for EditorGUI controls.</para>
  /// </summary>
  public sealed class EditorStyles
  {
    private static EditorStyles[] s_CachedStyles = new EditorStyles[2];
    private Vector2 m_KnobSize = new Vector2(40f, 40f);
    private Vector2 m_MiniKnobSize = new Vector2(29f, 29f);
    internal GUIStyle m_Label;
    private GUIStyle m_MiniLabel;
    private GUIStyle m_LargeLabel;
    private GUIStyle m_BoldLabel;
    private GUIStyle m_MiniBoldLabel;
    private GUIStyle m_CenteredGreyMiniLabel;
    private GUIStyle m_WordWrappedMiniLabel;
    private GUIStyle m_WordWrappedLabel;
    private GUIStyle m_LinkLabel;
    private GUIStyle m_WhiteLabel;
    private GUIStyle m_WhiteMiniLabel;
    private GUIStyle m_WhiteLargeLabel;
    private GUIStyle m_WhiteBoldLabel;
    private GUIStyle m_RadioButton;
    private GUIStyle m_MiniButton;
    private GUIStyle m_MiniButtonLeft;
    private GUIStyle m_MiniButtonMid;
    private GUIStyle m_MiniButtonRight;
    internal GUIStyle m_TextField;
    internal GUIStyle m_TextArea;
    private GUIStyle m_MiniTextField;
    private GUIStyle m_NumberField;
    private GUIStyle m_Popup;
    private GUIStyle m_ObjectField;
    private GUIStyle m_ObjectFieldThumb;
    private GUIStyle m_ObjectFieldMiniThumb;
    private GUIStyle m_ColorField;
    private GUIStyle m_LayerMaskField;
    private GUIStyle m_Toggle;
    private GUIStyle m_ToggleMixed;
    private GUIStyle m_Foldout;
    private GUIStyle m_FoldoutPreDrop;
    private GUIStyle m_ToggleGroup;
    private GUIStyle m_TextFieldDropDown;
    private GUIStyle m_TextFieldDropDownText;
    internal Font m_StandardFont;
    internal Font m_BoldFont;
    internal Font m_MiniFont;
    internal Font m_MiniBoldFont;
    private GUIStyle m_Toolbar;
    private GUIStyle m_ToolbarButton;
    private GUIStyle m_ToolbarPopup;
    private GUIStyle m_ToolbarDropDown;
    private GUIStyle m_ToolbarTextField;
    private GUIStyle m_InspectorDefaultMargins;
    private GUIStyle m_InspectorFullWidthMargins;
    private GUIStyle m_HelpBox;
    private GUIStyle m_ToolbarSearchField;
    private GUIStyle m_ToolbarSearchFieldPopup;
    private GUIStyle m_ToolbarSearchFieldCancelButton;
    private GUIStyle m_ToolbarSearchFieldCancelButtonEmpty;
    private GUIStyle m_ColorPickerBox;
    private GUIStyle m_InspectorBig;
    private GUIStyle m_InspectorTitlebar;
    private GUIStyle m_InspectorTitlebarText;
    private GUIStyle m_FoldoutSelected;
    private GUIStyle m_Tooltip;
    private GUIStyle m_NotificationText;
    private GUIStyle m_NotificationBackground;
    private GUIStyle m_AssetLabel;
    private GUIStyle m_AssetLabelPartial;
    private GUIStyle m_AssetLabelIcon;
    private GUIStyle m_SearchField;
    private GUIStyle m_SearchFieldCancelButton;
    private GUIStyle m_SearchFieldCancelButtonEmpty;
    private GUIStyle m_SelectionRect;
    private GUIStyle m_MinMaxHorizontalSliderThumb;
    private GUIStyle m_DropDownList;
    private GUIStyle m_ProgressBarBar;
    private GUIStyle m_ProgressBarText;
    private GUIStyle m_ProgressBarBack;
    internal static EditorStyles s_Current;

    /// <summary>
    ///   <para>Style used for the labelled on all EditorGUI overloads that take a prefix label.</para>
    /// </summary>
    public static GUIStyle label
    {
      get
      {
        return EditorStyles.s_Current.m_Label;
      }
    }

    /// <summary>
    ///   <para>Style for label with small font.</para>
    /// </summary>
    public static GUIStyle miniLabel
    {
      get
      {
        return EditorStyles.s_Current.m_MiniLabel;
      }
    }

    /// <summary>
    ///   <para>Style for label with large font.</para>
    /// </summary>
    public static GUIStyle largeLabel
    {
      get
      {
        return EditorStyles.s_Current.m_LargeLabel;
      }
    }

    /// <summary>
    ///   <para>Style for bold label.</para>
    /// </summary>
    public static GUIStyle boldLabel
    {
      get
      {
        return EditorStyles.s_Current.m_BoldLabel;
      }
    }

    /// <summary>
    ///   <para>Style for mini bold label.</para>
    /// </summary>
    public static GUIStyle miniBoldLabel
    {
      get
      {
        return EditorStyles.s_Current.m_MiniBoldLabel;
      }
    }

    /// <summary>
    ///   <para>Style for label with small font which is centered and grey.</para>
    /// </summary>
    public static GUIStyle centeredGreyMiniLabel
    {
      get
      {
        return EditorStyles.s_Current.m_CenteredGreyMiniLabel;
      }
    }

    /// <summary>
    ///   <para>Style for word wrapped mini label.</para>
    /// </summary>
    public static GUIStyle wordWrappedMiniLabel
    {
      get
      {
        return EditorStyles.s_Current.m_WordWrappedMiniLabel;
      }
    }

    /// <summary>
    ///   <para>Style for word wrapped label.</para>
    /// </summary>
    public static GUIStyle wordWrappedLabel
    {
      get
      {
        return EditorStyles.s_Current.m_WordWrappedLabel;
      }
    }

    internal static GUIStyle linkLabel
    {
      get
      {
        return EditorStyles.s_Current.m_LinkLabel;
      }
    }

    /// <summary>
    ///   <para>Style for white label.</para>
    /// </summary>
    public static GUIStyle whiteLabel
    {
      get
      {
        return EditorStyles.s_Current.m_WhiteLabel;
      }
    }

    /// <summary>
    ///   <para>Style for white mini label.</para>
    /// </summary>
    public static GUIStyle whiteMiniLabel
    {
      get
      {
        return EditorStyles.s_Current.m_WhiteMiniLabel;
      }
    }

    /// <summary>
    ///   <para>Style for white large label.</para>
    /// </summary>
    public static GUIStyle whiteLargeLabel
    {
      get
      {
        return EditorStyles.s_Current.m_WhiteLargeLabel;
      }
    }

    /// <summary>
    ///   <para>Style for white bold label.</para>
    /// </summary>
    public static GUIStyle whiteBoldLabel
    {
      get
      {
        return EditorStyles.s_Current.m_WhiteBoldLabel;
      }
    }

    /// <summary>
    ///   <para>Style used for a radio button.</para>
    /// </summary>
    public static GUIStyle radioButton
    {
      get
      {
        return EditorStyles.s_Current.m_RadioButton;
      }
    }

    /// <summary>
    ///   <para>Style used for a standalone small button.</para>
    /// </summary>
    public static GUIStyle miniButton
    {
      get
      {
        return EditorStyles.s_Current.m_MiniButton;
      }
    }

    /// <summary>
    ///   <para>Style used for the leftmost button in a horizontal button group.</para>
    /// </summary>
    public static GUIStyle miniButtonLeft
    {
      get
      {
        return EditorStyles.s_Current.m_MiniButtonLeft;
      }
    }

    /// <summary>
    ///   <para>Style used for the middle buttons in a horizontal group.</para>
    /// </summary>
    public static GUIStyle miniButtonMid
    {
      get
      {
        return EditorStyles.s_Current.m_MiniButtonMid;
      }
    }

    /// <summary>
    ///   <para>Style used for the rightmost button in a horizontal group.</para>
    /// </summary>
    public static GUIStyle miniButtonRight
    {
      get
      {
        return EditorStyles.s_Current.m_MiniButtonRight;
      }
    }

    /// <summary>
    ///   <para>Style used for EditorGUI.TextField.</para>
    /// </summary>
    public static GUIStyle textField
    {
      get
      {
        return EditorStyles.s_Current.m_TextField;
      }
    }

    /// <summary>
    ///   <para>Style used for EditorGUI.TextArea.</para>
    /// </summary>
    public static GUIStyle textArea
    {
      get
      {
        return EditorStyles.s_Current.m_TextArea;
      }
    }

    /// <summary>
    ///   <para>Smaller text field.</para>
    /// </summary>
    public static GUIStyle miniTextField
    {
      get
      {
        return EditorStyles.s_Current.m_MiniTextField;
      }
    }

    /// <summary>
    ///   <para>Style used for field editors for numbers.</para>
    /// </summary>
    public static GUIStyle numberField
    {
      get
      {
        return EditorStyles.s_Current.m_NumberField;
      }
    }

    /// <summary>
    ///   <para>Style used for EditorGUI.Popup, EditorGUI.EnumPopup,.</para>
    /// </summary>
    public static GUIStyle popup
    {
      get
      {
        return EditorStyles.s_Current.m_Popup;
      }
    }

    [Obsolete("structHeadingLabel is deprecated, use EditorStyles.label instead.")]
    public static GUIStyle structHeadingLabel
    {
      get
      {
        return EditorStyles.s_Current.m_Label;
      }
    }

    /// <summary>
    ///   <para>Style used for headings for object fields.</para>
    /// </summary>
    public static GUIStyle objectField
    {
      get
      {
        return EditorStyles.s_Current.m_ObjectField;
      }
    }

    /// <summary>
    ///   <para>Style used for headings for the Select button in object fields.</para>
    /// </summary>
    public static GUIStyle objectFieldThumb
    {
      get
      {
        return EditorStyles.s_Current.m_ObjectFieldThumb;
      }
    }

    /// <summary>
    ///   <para>Style used for object fields that have a thumbnail (e.g Textures). </para>
    /// </summary>
    public static GUIStyle objectFieldMiniThumb
    {
      get
      {
        return EditorStyles.s_Current.m_ObjectFieldMiniThumb;
      }
    }

    /// <summary>
    ///   <para>Style used for headings for Color fields.</para>
    /// </summary>
    public static GUIStyle colorField
    {
      get
      {
        return EditorStyles.s_Current.m_ColorField;
      }
    }

    /// <summary>
    ///   <para>Style used for headings for Layer masks.</para>
    /// </summary>
    public static GUIStyle layerMaskField
    {
      get
      {
        return EditorStyles.s_Current.m_LayerMaskField;
      }
    }

    /// <summary>
    ///   <para>Style used for headings for EditorGUI.Toggle.</para>
    /// </summary>
    public static GUIStyle toggle
    {
      get
      {
        return EditorStyles.s_Current.m_Toggle;
      }
    }

    internal static GUIStyle toggleMixed
    {
      get
      {
        return EditorStyles.s_Current.m_ToggleMixed;
      }
    }

    /// <summary>
    ///   <para>Style used for headings for EditorGUI.Foldout.</para>
    /// </summary>
    public static GUIStyle foldout
    {
      get
      {
        return EditorStyles.s_Current.m_Foldout;
      }
    }

    /// <summary>
    ///   <para>Style used for headings for EditorGUI.Foldout.</para>
    /// </summary>
    public static GUIStyle foldoutPreDrop
    {
      get
      {
        return EditorStyles.s_Current.m_FoldoutPreDrop;
      }
    }

    /// <summary>
    ///   <para>Style used for headings for EditorGUILayout.BeginToggleGroup.</para>
    /// </summary>
    public static GUIStyle toggleGroup
    {
      get
      {
        return EditorStyles.s_Current.m_ToggleGroup;
      }
    }

    internal static GUIStyle textFieldDropDown
    {
      get
      {
        return EditorStyles.s_Current.m_TextFieldDropDown;
      }
    }

    internal static GUIStyle textFieldDropDownText
    {
      get
      {
        return EditorStyles.s_Current.m_TextFieldDropDownText;
      }
    }

    /// <summary>
    ///   <para>Standard font.</para>
    /// </summary>
    public static Font standardFont
    {
      get
      {
        return EditorStyles.s_Current.m_StandardFont;
      }
    }

    /// <summary>
    ///   <para>Bold font.</para>
    /// </summary>
    public static Font boldFont
    {
      get
      {
        return EditorStyles.s_Current.m_BoldFont;
      }
    }

    /// <summary>
    ///   <para>Mini font.</para>
    /// </summary>
    public static Font miniFont
    {
      get
      {
        return EditorStyles.s_Current.m_MiniFont;
      }
    }

    /// <summary>
    ///   <para>Mini Bold font.</para>
    /// </summary>
    public static Font miniBoldFont
    {
      get
      {
        return EditorStyles.s_Current.m_MiniBoldFont;
      }
    }

    /// <summary>
    ///   <para>Toolbar background from top of windows.</para>
    /// </summary>
    public static GUIStyle toolbar
    {
      get
      {
        return EditorStyles.s_Current.m_Toolbar;
      }
    }

    /// <summary>
    ///   <para>Style for Button and Toggles in toolbars.</para>
    /// </summary>
    public static GUIStyle toolbarButton
    {
      get
      {
        return EditorStyles.s_Current.m_ToolbarButton;
      }
    }

    /// <summary>
    ///   <para>Toolbar Popup.</para>
    /// </summary>
    public static GUIStyle toolbarPopup
    {
      get
      {
        return EditorStyles.s_Current.m_ToolbarPopup;
      }
    }

    /// <summary>
    ///   <para>Toolbar Dropdown.</para>
    /// </summary>
    public static GUIStyle toolbarDropDown
    {
      get
      {
        return EditorStyles.s_Current.m_ToolbarDropDown;
      }
    }

    /// <summary>
    ///   <para>Toolbar text field.</para>
    /// </summary>
    public static GUIStyle toolbarTextField
    {
      get
      {
        return EditorStyles.s_Current.m_ToolbarTextField;
      }
    }

    /// <summary>
    ///   <para>Wrap content in a vertical group with this style to get the default margins used in the Inspector.</para>
    /// </summary>
    public static GUIStyle inspectorDefaultMargins
    {
      get
      {
        return EditorStyles.s_Current.m_InspectorDefaultMargins;
      }
    }

    /// <summary>
    ///   <para>Wrap content in a vertical group with this style to get full width margins in the Inspector.</para>
    /// </summary>
    public static GUIStyle inspectorFullWidthMargins
    {
      get
      {
        return EditorStyles.s_Current.m_InspectorFullWidthMargins;
      }
    }

    /// <summary>
    ///   <para>Style used for background box for EditorGUI.HelpBox.</para>
    /// </summary>
    public static GUIStyle helpBox
    {
      get
      {
        return EditorStyles.s_Current.m_HelpBox;
      }
    }

    internal static GUIStyle toolbarSearchField
    {
      get
      {
        return EditorStyles.s_Current.m_ToolbarSearchField;
      }
    }

    internal static GUIStyle toolbarSearchFieldPopup
    {
      get
      {
        return EditorStyles.s_Current.m_ToolbarSearchFieldPopup;
      }
    }

    internal static GUIStyle toolbarSearchFieldCancelButton
    {
      get
      {
        return EditorStyles.s_Current.m_ToolbarSearchFieldCancelButton;
      }
    }

    internal static GUIStyle toolbarSearchFieldCancelButtonEmpty
    {
      get
      {
        return EditorStyles.s_Current.m_ToolbarSearchFieldCancelButtonEmpty;
      }
    }

    internal static GUIStyle colorPickerBox
    {
      get
      {
        return EditorStyles.s_Current.m_ColorPickerBox;
      }
    }

    internal static GUIStyle inspectorBig
    {
      get
      {
        return EditorStyles.s_Current.m_InspectorBig;
      }
    }

    internal static GUIStyle inspectorTitlebar
    {
      get
      {
        return EditorStyles.s_Current.m_InspectorTitlebar;
      }
    }

    internal static GUIStyle inspectorTitlebarText
    {
      get
      {
        return EditorStyles.s_Current.m_InspectorTitlebarText;
      }
    }

    internal static GUIStyle foldoutSelected
    {
      get
      {
        return EditorStyles.s_Current.m_FoldoutSelected;
      }
    }

    internal static GUIStyle tooltip
    {
      get
      {
        return EditorStyles.s_Current.m_Tooltip;
      }
    }

    internal static GUIStyle notificationText
    {
      get
      {
        return EditorStyles.s_Current.m_NotificationText;
      }
    }

    internal static GUIStyle notificationBackground
    {
      get
      {
        return EditorStyles.s_Current.m_NotificationBackground;
      }
    }

    internal static GUIStyle assetLabel
    {
      get
      {
        return EditorStyles.s_Current.m_AssetLabel;
      }
    }

    internal static GUIStyle assetLabelPartial
    {
      get
      {
        return EditorStyles.s_Current.m_AssetLabelPartial;
      }
    }

    internal static GUIStyle assetLabelIcon
    {
      get
      {
        return EditorStyles.s_Current.m_AssetLabelIcon;
      }
    }

    internal static GUIStyle searchField
    {
      get
      {
        return EditorStyles.s_Current.m_SearchField;
      }
    }

    internal static GUIStyle searchFieldCancelButton
    {
      get
      {
        return EditorStyles.s_Current.m_SearchFieldCancelButton;
      }
    }

    internal static GUIStyle searchFieldCancelButtonEmpty
    {
      get
      {
        return EditorStyles.s_Current.m_SearchFieldCancelButtonEmpty;
      }
    }

    internal static GUIStyle selectionRect
    {
      get
      {
        return EditorStyles.s_Current.m_SelectionRect;
      }
    }

    internal static GUIStyle minMaxHorizontalSliderThumb
    {
      get
      {
        return EditorStyles.s_Current.m_MinMaxHorizontalSliderThumb;
      }
    }

    internal static GUIStyle dropDownList
    {
      get
      {
        return EditorStyles.s_Current.m_DropDownList;
      }
    }

    internal static GUIStyle progressBarBack
    {
      get
      {
        return EditorStyles.s_Current.m_ProgressBarBack;
      }
    }

    internal static GUIStyle progressBarBar
    {
      get
      {
        return EditorStyles.s_Current.m_ProgressBarBar;
      }
    }

    internal static GUIStyle progressBarText
    {
      get
      {
        return EditorStyles.s_Current.m_ProgressBarText;
      }
    }

    internal static Vector2 knobSize
    {
      get
      {
        return EditorStyles.s_Current.m_KnobSize;
      }
    }

    internal static Vector2 miniKnobSize
    {
      get
      {
        return EditorStyles.s_Current.m_MiniKnobSize;
      }
    }

    internal static void UpdateSkinCache()
    {
      EditorStyles.UpdateSkinCache(EditorGUIUtility.skinIndex);
    }

    internal static void UpdateSkinCache(int skinIndex)
    {
      if (GUIUtility.s_SkinMode == 0)
        return;
      if (EditorStyles.s_CachedStyles[skinIndex] == null)
      {
        EditorStyles.s_CachedStyles[skinIndex] = new EditorStyles();
        EditorStyles.s_CachedStyles[skinIndex].InitSharedStyles();
      }
      EditorStyles.s_Current = EditorStyles.s_CachedStyles[skinIndex];
      EditorGUIUtility.s_FontIsBold = -1;
      EditorGUIUtility.SetBoldDefaultFont(false);
    }

    private void InitSharedStyles()
    {
      this.m_ColorPickerBox = this.GetStyle("ColorPickerBox");
      this.m_InspectorBig = this.GetStyle("In BigTitle");
      this.m_MiniLabel = this.GetStyle("miniLabel");
      this.m_LargeLabel = this.GetStyle("LargeLabel");
      this.m_BoldLabel = this.GetStyle("BoldLabel");
      this.m_MiniBoldLabel = this.GetStyle("MiniBoldLabel");
      this.m_WordWrappedLabel = this.GetStyle("WordWrappedLabel");
      this.m_WordWrappedMiniLabel = this.GetStyle("WordWrappedMiniLabel");
      this.m_WhiteLabel = this.GetStyle("WhiteLabel");
      this.m_WhiteMiniLabel = this.GetStyle("WhiteMiniLabel");
      this.m_WhiteLargeLabel = this.GetStyle("WhiteLargeLabel");
      this.m_WhiteBoldLabel = this.GetStyle("WhiteBoldLabel");
      this.m_MiniTextField = this.GetStyle("MiniTextField");
      this.m_RadioButton = this.GetStyle("Radio");
      this.m_MiniButton = this.GetStyle("miniButton");
      this.m_MiniButtonLeft = this.GetStyle("miniButtonLeft");
      this.m_MiniButtonMid = this.GetStyle("miniButtonMid");
      this.m_MiniButtonRight = this.GetStyle("miniButtonRight");
      this.m_Toolbar = this.GetStyle("toolbar");
      this.m_ToolbarButton = this.GetStyle("toolbarbutton");
      this.m_ToolbarPopup = this.GetStyle("toolbarPopup");
      this.m_ToolbarDropDown = this.GetStyle("toolbarDropDown");
      this.m_ToolbarTextField = this.GetStyle("toolbarTextField");
      this.m_ToolbarSearchField = this.GetStyle("ToolbarSeachTextField");
      this.m_ToolbarSearchFieldPopup = this.GetStyle("ToolbarSeachTextFieldPopup");
      this.m_ToolbarSearchFieldCancelButton = this.GetStyle("ToolbarSeachCancelButton");
      this.m_ToolbarSearchFieldCancelButtonEmpty = this.GetStyle("ToolbarSeachCancelButtonEmpty");
      this.m_SearchField = this.GetStyle("SearchTextField");
      this.m_SearchFieldCancelButton = this.GetStyle("SearchCancelButton");
      this.m_SearchFieldCancelButtonEmpty = this.GetStyle("SearchCancelButtonEmpty");
      this.m_HelpBox = this.GetStyle("HelpBox");
      this.m_AssetLabel = this.GetStyle("AssetLabel");
      this.m_AssetLabelPartial = this.GetStyle("AssetLabel Partial");
      this.m_AssetLabelIcon = this.GetStyle("AssetLabel Icon");
      this.m_SelectionRect = this.GetStyle("selectionRect");
      this.m_MinMaxHorizontalSliderThumb = this.GetStyle("MinMaxHorizontalSliderThumb");
      this.m_DropDownList = this.GetStyle("DropDownButton");
      this.m_BoldFont = this.GetStyle("BoldLabel").font;
      this.m_StandardFont = this.GetStyle("Label").font;
      this.m_MiniFont = this.GetStyle("MiniLabel").font;
      this.m_MiniBoldFont = this.GetStyle("MiniBoldLabel").font;
      this.m_ProgressBarBack = this.GetStyle("ProgressBarBack");
      this.m_ProgressBarBar = this.GetStyle("ProgressBarBar");
      this.m_ProgressBarText = this.GetStyle("ProgressBarText");
      this.m_FoldoutPreDrop = this.GetStyle("FoldoutPreDrop");
      this.m_InspectorTitlebar = this.GetStyle("IN Title");
      this.m_InspectorTitlebarText = this.GetStyle("IN TitleText");
      this.m_ToggleGroup = this.GetStyle("BoldToggle");
      this.m_Tooltip = this.GetStyle("Tooltip");
      this.m_NotificationText = this.GetStyle("NotificationText");
      this.m_NotificationBackground = this.GetStyle("NotificationBackground");
      this.m_Popup = this.m_LayerMaskField = this.GetStyle("MiniPopup");
      this.m_TextField = this.m_NumberField = this.GetStyle("textField");
      this.m_Label = this.GetStyle("ControlLabel");
      this.m_ObjectField = this.GetStyle("ObjectField");
      this.m_ObjectFieldThumb = this.GetStyle("ObjectFieldThumb");
      this.m_ObjectFieldMiniThumb = this.GetStyle("ObjectFieldMiniThumb");
      this.m_Toggle = this.GetStyle("Toggle");
      this.m_ToggleMixed = this.GetStyle("ToggleMixed");
      this.m_ColorField = this.GetStyle("ColorField");
      this.m_Foldout = this.GetStyle("Foldout");
      this.m_FoldoutSelected = GUIStyle.none;
      this.m_TextFieldDropDown = this.GetStyle("TextFieldDropDown");
      this.m_TextFieldDropDownText = this.GetStyle("TextFieldDropDownText");
      this.m_LinkLabel = new GUIStyle(this.m_Label);
      this.m_LinkLabel.normal.textColor = new Color(0.25f, 0.5f, 0.9f, 1f);
      this.m_LinkLabel.stretchWidth = false;
      this.m_TextArea = new GUIStyle(this.m_TextField);
      this.m_TextArea.wordWrap = true;
      this.m_InspectorDefaultMargins = new GUIStyle();
      this.m_InspectorDefaultMargins.padding = new RectOffset(14, 4, 0, 0);
      this.m_InspectorFullWidthMargins = new GUIStyle();
      this.m_InspectorFullWidthMargins.padding = new RectOffset(5, 4, 0, 0);
      this.m_CenteredGreyMiniLabel = new GUIStyle(this.m_MiniLabel);
      this.m_CenteredGreyMiniLabel.alignment = TextAnchor.MiddleCenter;
      this.m_CenteredGreyMiniLabel.normal.textColor = Color.grey;
    }

    private GUIStyle GetStyle(string styleName)
    {
      GUIStyle guiStyle = GUI.skin.FindStyle(styleName) ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);
      if (guiStyle == null)
      {
        Debug.LogError((object) ("Missing built-in guistyle " + styleName));
        guiStyle = GUISkin.error;
      }
      return guiStyle;
    }
  }
}

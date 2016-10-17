// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorGUILayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Auto-layouted version of EditorGUI.</para>
  /// </summary>
  public sealed class EditorGUILayout
  {
    internal static SavedBool s_SelectedDefault = new SavedBool("Platform.ShownDefaultTab", true);
    internal const float kPlatformTabWidth = 30f;
    internal static Rect s_LastRect;

    internal static float kLabelFloatMinW
    {
      get
      {
        return (float) ((double) EditorGUIUtility.labelWidth + (double) EditorGUIUtility.fieldWidth + 5.0);
      }
    }

    internal static float kLabelFloatMaxW
    {
      get
      {
        return (float) ((double) EditorGUIUtility.labelWidth + (double) EditorGUIUtility.fieldWidth + 5.0);
      }
    }

    /// <summary>
    ///   <para>Make a label with a foldout arrow to the left of it.</para>
    /// </summary>
    /// <param name="foldout">The shown foldout state.</param>
    /// <param name="content">The label to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The foldout state selected by the user. If true, you should render sub-objects.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static bool Foldout(bool foldout, string content)
    {
      GUIStyle foldout1 = EditorStyles.foldout;
      return EditorGUILayout.Foldout(foldout, content, foldout1);
    }

    /// <summary>
    ///   <para>Make a label with a foldout arrow to the left of it.</para>
    /// </summary>
    /// <param name="foldout">The shown foldout state.</param>
    /// <param name="content">The label to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The foldout state selected by the user. If true, you should render sub-objects.</para>
    /// </returns>
    public static bool Foldout(bool foldout, string content, [DefaultValue("EditorStyles.foldout")] GUIStyle style)
    {
      return EditorGUILayout.Foldout(foldout, EditorGUIUtility.TempContent(content), style);
    }

    /// <summary>
    ///   <para>Make a label with a foldout arrow to the left of it.</para>
    /// </summary>
    /// <param name="foldout">The shown foldout state.</param>
    /// <param name="content">The label to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The foldout state selected by the user. If true, you should render sub-objects.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static bool Foldout(bool foldout, GUIContent content)
    {
      GUIStyle foldout1 = EditorStyles.foldout;
      return EditorGUILayout.Foldout(foldout, content, foldout1);
    }

    /// <summary>
    ///   <para>Make a label with a foldout arrow to the left of it.</para>
    /// </summary>
    /// <param name="foldout">The shown foldout state.</param>
    /// <param name="content">The label to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <returns>
    ///   <para>The foldout state selected by the user. If true, you should render sub-objects.</para>
    /// </returns>
    public static bool Foldout(bool foldout, GUIContent content, [DefaultValue("EditorStyles.foldout")] GUIStyle style)
    {
      return EditorGUILayout.FoldoutInternal(foldout, content, style);
    }

    /// <summary>
    ///   <para>Make a label in front of some control.</para>
    /// </summary>
    /// <param name="label">Label to show in front of the control.</param>
    /// <param name="followingStyle"></param>
    /// <param name="labelStyle"></param>
    [ExcludeFromDocs]
    public static void PrefixLabel(string label)
    {
      GUIStyle followingStyle = (GUIStyle) "Button";
      EditorGUILayout.PrefixLabel(label, followingStyle);
    }

    /// <summary>
    ///   <para>Make a label in front of some control.</para>
    /// </summary>
    /// <param name="label">Label to show in front of the control.</param>
    /// <param name="followingStyle"></param>
    /// <param name="labelStyle"></param>
    public static void PrefixLabel(string label, [DefaultValue("\"Button\"")] GUIStyle followingStyle)
    {
      EditorGUILayout.PrefixLabel(EditorGUIUtility.TempContent(label), followingStyle, EditorStyles.label);
    }

    /// <summary>
    ///   <para>Make a label in front of some control.</para>
    /// </summary>
    /// <param name="label">Label to show in front of the control.</param>
    /// <param name="followingStyle"></param>
    /// <param name="labelStyle"></param>
    public static void PrefixLabel(string label, GUIStyle followingStyle, GUIStyle labelStyle)
    {
      EditorGUILayout.PrefixLabel(EditorGUIUtility.TempContent(label), followingStyle, labelStyle);
    }

    /// <summary>
    ///   <para>Make a label in front of some control.</para>
    /// </summary>
    /// <param name="label">Label to show in front of the control.</param>
    /// <param name="followingStyle"></param>
    /// <param name="labelStyle"></param>
    [ExcludeFromDocs]
    public static void PrefixLabel(GUIContent label)
    {
      GUIStyle followingStyle = (GUIStyle) "Button";
      EditorGUILayout.PrefixLabel(label, followingStyle);
    }

    /// <summary>
    ///   <para>Make a label in front of some control.</para>
    /// </summary>
    /// <param name="label">Label to show in front of the control.</param>
    /// <param name="followingStyle"></param>
    /// <param name="labelStyle"></param>
    public static void PrefixLabel(GUIContent label, [DefaultValue("\"Button\"")] GUIStyle followingStyle)
    {
      EditorGUILayout.PrefixLabel(label, followingStyle, EditorStyles.label);
    }

    /// <summary>
    ///   <para>Make a label in front of some control.</para>
    /// </summary>
    /// <param name="label">Label to show in front of the control.</param>
    /// <param name="followingStyle"></param>
    /// <param name="labelStyle"></param>
    public static void PrefixLabel(GUIContent label, GUIStyle followingStyle, GUIStyle labelStyle)
    {
      EditorGUILayout.PrefixLabelInternal(label, followingStyle, labelStyle);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    public static void LabelField(string label, params GUILayoutOption[] options)
    {
      EditorGUILayout.LabelField(GUIContent.none, EditorGUIUtility.TempContent(label), EditorStyles.label, options);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    public static void LabelField(string label, GUIStyle style, params GUILayoutOption[] options)
    {
      EditorGUILayout.LabelField(GUIContent.none, EditorGUIUtility.TempContent(label), style, options);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    public static void LabelField(GUIContent label, params GUILayoutOption[] options)
    {
      EditorGUILayout.LabelField(GUIContent.none, label, EditorStyles.label, options);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    public static void LabelField(GUIContent label, GUIStyle style, params GUILayoutOption[] options)
    {
      EditorGUILayout.LabelField(GUIContent.none, label, style, options);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    public static void LabelField(string label, string label2, params GUILayoutOption[] options)
    {
      EditorGUILayout.LabelField(new GUIContent(label), EditorGUIUtility.TempContent(label2), EditorStyles.label, options);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    public static void LabelField(string label, string label2, GUIStyle style, params GUILayoutOption[] options)
    {
      EditorGUILayout.LabelField(new GUIContent(label), EditorGUIUtility.TempContent(label2), style, options);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    public static void LabelField(GUIContent label, GUIContent label2, params GUILayoutOption[] options)
    {
      EditorGUILayout.LabelField(label, label2, EditorStyles.label, options);
    }

    /// <summary>
    ///   <para>Make a label field. (Useful for showing read-only info.)</para>
    /// </summary>
    /// <param name="label">Label in front of the label field.</param>
    /// <param name="label2">The label to show to the right.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    public static void LabelField(GUIContent label, GUIContent label2, GUIStyle style, params GUILayoutOption[] options)
    {
      if (!style.wordWrap)
      {
        EditorGUI.LabelField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, options), label, label2, style);
      }
      else
      {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label, style);
        Rect rect = GUILayoutUtility.GetRect(label2, style, options);
        int indentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        EditorGUI.LabelField(rect, label2, style);
        EditorGUI.indentLevel = indentLevel;
        EditorGUILayout.EndHorizontal();
      }
    }

    internal static bool LinkLabel(string label, params GUILayoutOption[] options)
    {
      return EditorGUILayout.LinkLabel(EditorGUIUtility.TempContent(label), options);
    }

    internal static bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
    {
      Rect position = EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(label, EditorStyles.linkLabel, options);
      Handles.BeginGUI();
      Handles.color = EditorStyles.linkLabel.normal.textColor;
      Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
      Handles.color = Color.white;
      Handles.EndGUI();
      EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);
      return GUI.Button(position, label, EditorStyles.linkLabel);
    }

    /// <summary>
    ///   <para>Make a toggle.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the toggle.</param>
    /// <param name="value">The shown state of the toggle.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The selected state of the toggle.</para>
    /// </returns>
    public static bool Toggle(bool value, params GUILayoutOption[] options)
    {
      return EditorGUI.Toggle(EditorGUILayout.s_LastRect = EditorGUILayout.GetToggleRect(false, options), value);
    }

    /// <summary>
    ///   <para>Make a toggle.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the toggle.</param>
    /// <param name="value">The shown state of the toggle.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The selected state of the toggle.</para>
    /// </returns>
    public static bool Toggle(string label, bool value, params GUILayoutOption[] options)
    {
      return EditorGUILayout.Toggle(EditorGUIUtility.TempContent(label), value, options);
    }

    /// <summary>
    ///   <para>Make a toggle.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the toggle.</param>
    /// <param name="value">The shown state of the toggle.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The selected state of the toggle.</para>
    /// </returns>
    public static bool Toggle(GUIContent label, bool value, params GUILayoutOption[] options)
    {
      return EditorGUI.Toggle(EditorGUILayout.s_LastRect = EditorGUILayout.GetToggleRect(true, options), label, value);
    }

    /// <summary>
    ///   <para>Make a toggle.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the toggle.</param>
    /// <param name="value">The shown state of the toggle.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The selected state of the toggle.</para>
    /// </returns>
    public static bool Toggle(bool value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.Toggle(EditorGUILayout.s_LastRect = EditorGUILayout.GetToggleRect(false, options), value, style);
    }

    /// <summary>
    ///   <para>Make a toggle.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the toggle.</param>
    /// <param name="value">The shown state of the toggle.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The selected state of the toggle.</para>
    /// </returns>
    public static bool Toggle(string label, bool value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUILayout.Toggle(EditorGUIUtility.TempContent(label), value, style, options);
    }

    /// <summary>
    ///   <para>Make a toggle.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the toggle.</param>
    /// <param name="value">The shown state of the toggle.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The selected state of the toggle.</para>
    /// </returns>
    public static bool Toggle(GUIContent label, bool value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.Toggle(EditorGUILayout.s_LastRect = EditorGUILayout.GetToggleRect(true, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a toggle field where the toggle is to the left and the label immediately to the right of it.</para>
    /// </summary>
    /// <param name="label">Label to display next to the toggle.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="labelStyle">Optional GUIStyle to use for the label.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static bool ToggleLeft(string label, bool value, params GUILayoutOption[] options)
    {
      return EditorGUILayout.ToggleLeft(EditorGUIUtility.TempContent(label), value, options);
    }

    /// <summary>
    ///   <para>Make a toggle field where the toggle is to the left and the label immediately to the right of it.</para>
    /// </summary>
    /// <param name="label">Label to display next to the toggle.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="labelStyle">Optional GUIStyle to use for the label.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static bool ToggleLeft(GUIContent label, bool value, params GUILayoutOption[] options)
    {
      return EditorGUI.ToggleLeft(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, options), label, value);
    }

    /// <summary>
    ///   <para>Make a toggle field where the toggle is to the left and the label immediately to the right of it.</para>
    /// </summary>
    /// <param name="label">Label to display next to the toggle.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="labelStyle">Optional GUIStyle to use for the label.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static bool ToggleLeft(string label, bool value, GUIStyle labelStyle, params GUILayoutOption[] options)
    {
      return EditorGUILayout.ToggleLeft(EditorGUIUtility.TempContent(label), value, labelStyle, options);
    }

    /// <summary>
    ///   <para>Make a toggle field where the toggle is to the left and the label immediately to the right of it.</para>
    /// </summary>
    /// <param name="label">Label to display next to the toggle.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="labelStyle">Optional GUIStyle to use for the label.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static bool ToggleLeft(GUIContent label, bool value, GUIStyle labelStyle, params GUILayoutOption[] options)
    {
      return EditorGUI.ToggleLeft(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, options), label, value, labelStyle);
    }

    /// <summary>
    ///   <para>Make a text field.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    public static string TextField(string text, params GUILayoutOption[] options)
    {
      return EditorGUI.TextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.textField, options), text);
    }

    /// <summary>
    ///   <para>Make a text field.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    public static string TextField(string text, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.TextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), text, style);
    }

    /// <summary>
    ///   <para>Make a text field.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    public static string TextField(string label, string text, params GUILayoutOption[] options)
    {
      return EditorGUI.TextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.textField, options), label, text);
    }

    /// <summary>
    ///   <para>Make a text field.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    public static string TextField(string label, string text, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.TextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, text, style);
    }

    /// <summary>
    ///   <para>Make a text field.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    public static string TextField(GUIContent label, string text, params GUILayoutOption[] options)
    {
      return EditorGUI.TextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.textField, options), label, text);
    }

    /// <summary>
    ///   <para>Make a text field.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the text field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    public static string TextField(GUIContent label, string text, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.TextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, text, style);
    }

    /// <summary>
    ///   <para>Make a delayed text field.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the text field.</para>
    /// </returns>
    public static string DelayedTextField(string text, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedTextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.textField, options), text);
    }

    /// <summary>
    ///   <para>Make a delayed text field.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the text field.</para>
    /// </returns>
    public static string DelayedTextField(string text, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedTextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), text, style);
    }

    /// <summary>
    ///   <para>Make a delayed text field.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the text field.</para>
    /// </returns>
    public static string DelayedTextField(string label, string text, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedTextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.textField, options), label, text);
    }

    /// <summary>
    ///   <para>Make a delayed text field.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the text field.</para>
    /// </returns>
    public static string DelayedTextField(string label, string text, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedTextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, text, style);
    }

    /// <summary>
    ///   <para>Make a delayed text field.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the text field.</para>
    /// </returns>
    public static string DelayedTextField(GUIContent label, string text, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedTextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.textField, options), label, text);
    }

    /// <summary>
    ///   <para>Make a delayed text field.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the text field.</para>
    /// </returns>
    public static string DelayedTextField(GUIContent label, string text, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedTextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, text, style);
    }

    /// <summary>
    ///   <para>Make a delayed text field.</para>
    /// </summary>
    /// <param name="property">The text property to edit.</param>
    /// <param name="label">Optional label to display in front of the int field. Pass GUIContent.none to hide label.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void DelayedTextField(SerializedProperty property, params GUILayoutOption[] options)
    {
      EditorGUILayout.DelayedTextField(property, (GUIContent) null, options);
    }

    /// <summary>
    ///   <para>Make a delayed text field.</para>
    /// </summary>
    /// <param name="property">The text property to edit.</param>
    /// <param name="label">Optional label to display in front of the int field. Pass GUIContent.none to hide label.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void DelayedTextField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
    {
      EditorGUI.DelayedTextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(EditorGUI.LabelHasContent(label), 16f, EditorStyles.textField, options), property, label);
    }

    internal static string ToolbarSearchField(string text, params GUILayoutOption[] options)
    {
      Rect position = EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(0.0f, EditorGUILayout.kLabelFloatMaxW * 1.5f, 16f, 16f, EditorStyles.toolbarSearchField, options);
      int searchMode = 0;
      return EditorGUI.ToolbarSearchField(position, (string[]) null, ref searchMode, text);
    }

    internal static string ToolbarSearchField(string text, string[] searchModes, ref int searchMode, params GUILayoutOption[] options)
    {
      return EditorGUI.ToolbarSearchField(EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(0.0f, EditorGUILayout.kLabelFloatMaxW * 1.5f, 16f, 16f, EditorStyles.toolbarSearchField, options), searchModes, ref searchMode, text);
    }

    /// <summary>
    ///   <para>Make a text area.</para>
    /// </summary>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    public static string TextArea(string text, params GUILayoutOption[] options)
    {
      return EditorGUILayout.TextArea(text, EditorStyles.textField, options);
    }

    /// <summary>
    ///   <para>Make a text area.</para>
    /// </summary>
    /// <param name="text">The text to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The text entered by the user.</para>
    /// </returns>
    public static string TextArea(string text, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.TextArea(EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(EditorGUIUtility.TempContent(text), style, options), text, style);
    }

    /// <summary>
    ///   <para>Make a selectable label field. (Useful for showing read-only info that can be copy-pasted.)</para>
    /// </summary>
    /// <param name="text">The text to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void SelectableLabel(string text, params GUILayoutOption[] options)
    {
      EditorGUILayout.SelectableLabel(text, EditorStyles.label, options);
    }

    /// <summary>
    ///   <para>Make a selectable label field. (Useful for showing read-only info that can be copy-pasted.)</para>
    /// </summary>
    /// <param name="text">The text to show.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void SelectableLabel(string text, GUIStyle style, params GUILayoutOption[] options)
    {
      EditorGUI.SelectableLabel(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 32f, style, options), text, style);
    }

    internal static Event KeyEventField(Event e, params GUILayoutOption[] options)
    {
      return EditorGUI.KeyEventField(GUILayoutUtility.GetRect(EditorGUIUtility.TempContent("[Please press a key]"), GUI.skin.textField, options), e);
    }

    /// <summary>
    ///   <para>Make a text field where the user can enter a password.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the password field.</param>
    /// <param name="password">The password to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The password entered by the user.</para>
    /// </returns>
    public static string PasswordField(string password, params GUILayoutOption[] options)
    {
      return EditorGUILayout.PasswordField(password, EditorStyles.textField, options);
    }

    /// <summary>
    ///   <para>Make a text field where the user can enter a password.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the password field.</param>
    /// <param name="password">The password to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The password entered by the user.</para>
    /// </returns>
    public static string PasswordField(string password, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.PasswordField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), password, style);
    }

    /// <summary>
    ///   <para>Make a text field where the user can enter a password.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the password field.</param>
    /// <param name="password">The password to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The password entered by the user.</para>
    /// </returns>
    public static string PasswordField(string label, string password, params GUILayoutOption[] options)
    {
      return EditorGUILayout.PasswordField(EditorGUIUtility.TempContent(label), password, EditorStyles.textField, options);
    }

    /// <summary>
    ///   <para>Make a text field where the user can enter a password.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the password field.</param>
    /// <param name="password">The password to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The password entered by the user.</para>
    /// </returns>
    public static string PasswordField(string label, string password, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUILayout.PasswordField(EditorGUIUtility.TempContent(label), password, style, options);
    }

    /// <summary>
    ///   <para>Make a text field where the user can enter a password.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the password field.</param>
    /// <param name="password">The password to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The password entered by the user.</para>
    /// </returns>
    public static string PasswordField(GUIContent label, string password, params GUILayoutOption[] options)
    {
      return EditorGUILayout.PasswordField(label, password, EditorStyles.textField, options);
    }

    /// <summary>
    ///   <para>Make a text field where the user can enter a password.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the password field.</param>
    /// <param name="password">The password to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The password entered by the user.</para>
    /// </returns>
    public static string PasswordField(GUIContent label, string password, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.PasswordField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, password, style);
    }

    internal static void VUMeterHorizontal(float value, float peak, params GUILayoutOption[] options)
    {
      EditorGUI.VUMeter.HorizontalMeter(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.numberField, options), value, peak, EditorGUI.VUMeter.horizontalVUTexture, Color.grey);
    }

    internal static void VUMeterHorizontal(float value, ref EditorGUI.VUMeter.SmoothingData data, params GUILayoutOption[] options)
    {
      EditorGUI.VUMeter.HorizontalMeter(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.numberField, options), value, ref data, EditorGUI.VUMeter.horizontalVUTexture, Color.grey);
    }

    /// <summary>
    ///   <para>Make a text field for entering float values.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static float FloatField(float value, params GUILayoutOption[] options)
    {
      return EditorGUI.FloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.numberField, options), value);
    }

    /// <summary>
    ///   <para>Make a text field for entering float values.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static float FloatField(float value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.FloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering float values.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static float FloatField(string label, float value, params GUILayoutOption[] options)
    {
      return EditorGUI.FloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.numberField, options), label, value);
    }

    /// <summary>
    ///   <para>Make a text field for entering float values.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static float FloatField(string label, float value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.FloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering float values.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static float FloatField(GUIContent label, float value, params GUILayoutOption[] options)
    {
      return EditorGUI.FloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.numberField, options), label, value);
    }

    /// <summary>
    ///   <para>Make a text field for entering float values.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static float FloatField(GUIContent label, float value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.FloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering floats.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the float field.</para>
    /// </returns>
    public static float DelayedFloatField(float value, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedFloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.numberField, options), value);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering floats.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the float field.</para>
    /// </returns>
    public static float DelayedFloatField(float value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedFloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), value, style);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering floats.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the float field.</para>
    /// </returns>
    public static float DelayedFloatField(string label, float value, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedFloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.numberField, options), label, value);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering floats.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the float field.</para>
    /// </returns>
    public static float DelayedFloatField(string label, float value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedFloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering floats.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the float field.</para>
    /// </returns>
    public static float DelayedFloatField(GUIContent label, float value, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedFloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.numberField, options), label, value);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering floats.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the float field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the float field.</para>
    /// </returns>
    public static float DelayedFloatField(GUIContent label, float value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedFloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering floats.</para>
    /// </summary>
    /// <param name="property">The float property to edit.</param>
    /// <param name="label">Optional label to display in front of the float field. Pass GUIContent.none to hide label.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void DelayedFloatField(SerializedProperty property, params GUILayoutOption[] options)
    {
      EditorGUILayout.DelayedFloatField(property, (GUIContent) null, options);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering floats.</para>
    /// </summary>
    /// <param name="property">The float property to edit.</param>
    /// <param name="label">Optional label to display in front of the float field. Pass GUIContent.none to hide label.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void DelayedFloatField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
    {
      EditorGUI.DelayedFloatField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(EditorGUI.LabelHasContent(label), 16f, EditorStyles.numberField, options), property, label);
    }

    /// <summary>
    ///   <para>Make a text field for entering double values.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the double field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static double DoubleField(double value, params GUILayoutOption[] options)
    {
      return EditorGUI.DoubleField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.numberField, options), value);
    }

    /// <summary>
    ///   <para>Make a text field for entering double values.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the double field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static double DoubleField(double value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.DoubleField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering double values.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the double field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static double DoubleField(string label, double value, params GUILayoutOption[] options)
    {
      return EditorGUI.DoubleField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.numberField, options), label, value);
    }

    /// <summary>
    ///   <para>Make a text field for entering double values.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the double field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static double DoubleField(string label, double value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.DoubleField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering double values.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the double field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static double DoubleField(GUIContent label, double value, params GUILayoutOption[] options)
    {
      return EditorGUI.DoubleField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.numberField, options), label, value);
    }

    /// <summary>
    ///   <para>Make a text field for entering double values.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the double field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static double DoubleField(GUIContent label, double value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.DoubleField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static int IntField(int value, params GUILayoutOption[] options)
    {
      return EditorGUILayout.IntField(value, EditorStyles.numberField, options);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static int IntField(int value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.IntField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static int IntField(string label, int value, params GUILayoutOption[] options)
    {
      return EditorGUILayout.IntField(label, value, EditorStyles.numberField, options);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static int IntField(string label, int value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.IntField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static int IntField(GUIContent label, int value, params GUILayoutOption[] options)
    {
      return EditorGUILayout.IntField(label, value, EditorStyles.numberField, options);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static int IntField(GUIContent label, int value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.IntField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the int field.</para>
    /// </returns>
    public static int DelayedIntField(int value, params GUILayoutOption[] options)
    {
      return EditorGUILayout.DelayedIntField(value, EditorStyles.numberField, options);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the int field.</para>
    /// </returns>
    public static int DelayedIntField(int value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedIntField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), value, style);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the int field.</para>
    /// </returns>
    public static int DelayedIntField(string label, int value, params GUILayoutOption[] options)
    {
      return EditorGUILayout.DelayedIntField(label, value, EditorStyles.numberField, options);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the int field.</para>
    /// </returns>
    public static int DelayedIntField(string label, int value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedIntField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the int field.</para>
    /// </returns>
    public static int DelayedIntField(GUIContent label, int value, params GUILayoutOption[] options)
    {
      return EditorGUILayout.DelayedIntField(label, value, EditorStyles.numberField, options);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the int field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user. Note that the return value will not change until the user has pressed enter or focus is moved away from the int field.</para>
    /// </returns>
    public static int DelayedIntField(GUIContent label, int value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.DelayedIntField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering integers.</para>
    /// </summary>
    /// <param name="property">The int property to edit.</param>
    /// <param name="label">Optional label to display in front of the int field. Pass GUIContent.none to hide label.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void DelayedIntField(SerializedProperty property, params GUILayoutOption[] options)
    {
      EditorGUILayout.DelayedIntField(property, (GUIContent) null, options);
    }

    /// <summary>
    ///   <para>Make a delayed text field for entering integers.</para>
    /// </summary>
    /// <param name="property">The int property to edit.</param>
    /// <param name="label">Optional label to display in front of the int field. Pass GUIContent.none to hide label.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void DelayedIntField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
    {
      EditorGUI.DelayedIntField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(EditorGUI.LabelHasContent(label), 16f, EditorStyles.numberField, options), property, label);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the long field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static long LongField(long value, params GUILayoutOption[] options)
    {
      return EditorGUILayout.LongField(value, EditorStyles.numberField, options);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the long field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static long LongField(long value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.LongField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the long field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static long LongField(string label, long value, params GUILayoutOption[] options)
    {
      return EditorGUILayout.LongField(label, value, EditorStyles.numberField, options);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the long field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static long LongField(string label, long value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.LongField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the long field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static long LongField(GUIContent label, long value, params GUILayoutOption[] options)
    {
      return EditorGUILayout.LongField(label, value, EditorStyles.numberField, options);
    }

    /// <summary>
    ///   <para>Make a text field for entering integers.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the long field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static long LongField(GUIContent label, long value, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.LongField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, value, style);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value that has been set by the user.</para>
    /// </returns>
    public static float Slider(float value, float leftValue, float rightValue, params GUILayoutOption[] options)
    {
      return EditorGUI.Slider(EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(false, options), value, leftValue, rightValue);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value that has been set by the user.</para>
    /// </returns>
    public static float Slider(string label, float value, float leftValue, float rightValue, params GUILayoutOption[] options)
    {
      return EditorGUILayout.Slider(EditorGUIUtility.TempContent(label), value, leftValue, rightValue, options);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value that has been set by the user.</para>
    /// </returns>
    public static float Slider(GUIContent label, float value, float leftValue, float rightValue, params GUILayoutOption[] options)
    {
      return EditorGUI.Slider(EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(true, options), label, value, leftValue, rightValue);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="property">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void Slider(SerializedProperty property, float leftValue, float rightValue, params GUILayoutOption[] options)
    {
      EditorGUI.Slider(EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(false, options), property, leftValue, rightValue);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="property">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void Slider(SerializedProperty property, float leftValue, float rightValue, string label, params GUILayoutOption[] options)
    {
      EditorGUILayout.Slider(property, leftValue, rightValue, EditorGUIUtility.TempContent(label), options);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change a value between a min and a max.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="property">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void Slider(SerializedProperty property, float leftValue, float rightValue, GUIContent label, params GUILayoutOption[] options)
    {
      EditorGUI.Slider(EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(true, options), property, leftValue, rightValue, label);
    }

    internal static float PowerSlider(string label, float value, float leftValue, float rightValue, float power, params GUILayoutOption[] options)
    {
      return EditorGUILayout.PowerSlider(EditorGUIUtility.TempContent(label), value, leftValue, rightValue, power, options);
    }

    internal static float PowerSlider(GUIContent label, float value, float leftValue, float rightValue, float power, params GUILayoutOption[] options)
    {
      return EditorGUI.PowerSlider(EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(true, options), label, value, leftValue, rightValue, power);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change an integer value between a min and a max.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value that has been set by the user.</para>
    /// </returns>
    public static int IntSlider(int value, int leftValue, int rightValue, params GUILayoutOption[] options)
    {
      return EditorGUI.IntSlider(EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(false, options), value, leftValue, rightValue);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change an integer value between a min and a max.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value that has been set by the user.</para>
    /// </returns>
    public static int IntSlider(string label, int value, int leftValue, int rightValue, params GUILayoutOption[] options)
    {
      return EditorGUI.IntSlider(EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(true, options), label, value, leftValue, rightValue);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change an integer value between a min and a max.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value that has been set by the user.</para>
    /// </returns>
    public static int IntSlider(GUIContent label, int value, int leftValue, int rightValue, params GUILayoutOption[] options)
    {
      return EditorGUI.IntSlider(EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(true, options), label, value, leftValue, rightValue);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change an integer value between a min and a max.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="property">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void IntSlider(SerializedProperty property, int leftValue, int rightValue, params GUILayoutOption[] options)
    {
      EditorGUI.IntSlider(EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(false, options), property, leftValue, rightValue, property.displayName);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change an integer value between a min and a max.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="property">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void IntSlider(SerializedProperty property, int leftValue, int rightValue, string label, params GUILayoutOption[] options)
    {
      EditorGUILayout.IntSlider(property, leftValue, rightValue, EditorGUIUtility.TempContent(label), options);
    }

    /// <summary>
    ///   <para>Make a slider the user can drag to change an integer value between a min and a max.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the slider.</param>
    /// <param name="property">The value the slider shows. This determines the position of the draggable thumb.</param>
    /// <param name="leftValue">The value at the left end of the slider.</param>
    /// <param name="rightValue">The value at the right end of the slider.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static void IntSlider(SerializedProperty property, int leftValue, int rightValue, GUIContent label, params GUILayoutOption[] options)
    {
      EditorGUI.IntSlider(EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(true, options), property, leftValue, rightValue, label);
    }

    public static void MinMaxSlider(ref float minValue, ref float maxValue, float minLimit, float maxLimit, params GUILayoutOption[] options)
    {
      EditorGUI.MinMaxSlider(EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(false, options), ref minValue, ref maxValue, minLimit, maxLimit);
    }

    public static void MinMaxSlider(GUIContent label, ref float minValue, ref float maxValue, float minLimit, float maxLimit, params GUILayoutOption[] options)
    {
      Rect position = EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(true, options);
      EditorGUI.MinMaxSlider(label, position, ref minValue, ref maxValue, minLimit, maxLimit);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    public static int Popup(int selectedIndex, string[] displayedOptions, params GUILayoutOption[] options)
    {
      return EditorGUILayout.Popup(selectedIndex, displayedOptions, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    public static int Popup(int selectedIndex, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.Popup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), selectedIndex, displayedOptions, style);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    public static int Popup(int selectedIndex, GUIContent[] displayedOptions, params GUILayoutOption[] options)
    {
      return EditorGUILayout.Popup(selectedIndex, displayedOptions, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    public static int Popup(int selectedIndex, GUIContent[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.Popup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), selectedIndex, displayedOptions, style);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    public static int Popup(string label, int selectedIndex, string[] displayedOptions, params GUILayoutOption[] options)
    {
      return EditorGUILayout.Popup(label, selectedIndex, displayedOptions, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    public static int Popup(string label, int selectedIndex, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.Popup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, selectedIndex, displayedOptions, style);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    public static int Popup(GUIContent label, int selectedIndex, GUIContent[] displayedOptions, params GUILayoutOption[] options)
    {
      return EditorGUILayout.Popup(label, selectedIndex, displayedOptions, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make a generic popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedIndex">The index of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the options shown in the popup.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The index of the option that has been selected by the user.</para>
    /// </returns>
    public static int Popup(GUIContent label, int selectedIndex, GUIContent[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.Popup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, selectedIndex, displayedOptions, style);
    }

    internal static void Popup(SerializedProperty property, GUIContent[] displayedOptions, params GUILayoutOption[] options)
    {
      EditorGUILayout.Popup(property, displayedOptions, (GUIContent) null, options);
    }

    internal static void Popup(SerializedProperty property, GUIContent[] displayedOptions, GUIContent label, params GUILayoutOption[] options)
    {
      EditorGUI.Popup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.popup, options), property, displayedOptions, label);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum option the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The enum option that has been selected by the user.</para>
    /// </returns>
    public static Enum EnumPopup(Enum selected, params GUILayoutOption[] options)
    {
      return EditorGUILayout.EnumPopup(selected, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum option the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The enum option that has been selected by the user.</para>
    /// </returns>
    public static Enum EnumPopup(Enum selected, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.EnumPopup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), selected, style);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum option the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The enum option that has been selected by the user.</para>
    /// </returns>
    public static Enum EnumPopup(string label, Enum selected, params GUILayoutOption[] options)
    {
      return EditorGUILayout.EnumPopup(label, selected, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum option the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The enum option that has been selected by the user.</para>
    /// </returns>
    public static Enum EnumPopup(string label, Enum selected, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.EnumPopup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), GUIContent.Temp(label), selected, style);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum option the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The enum option that has been selected by the user.</para>
    /// </returns>
    public static Enum EnumPopup(GUIContent label, Enum selected, params GUILayoutOption[] options)
    {
      return EditorGUILayout.EnumPopup(label, selected, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum option the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The enum option that has been selected by the user.</para>
    /// </returns>
    public static Enum EnumPopup(GUIContent label, Enum selected, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.EnumPopup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, selected, style);
    }

    /// <summary>
    ///   <para>Make an enum popup selection field for a bitmask.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selected">The enum options the field shows.</param>
    /// <param name="options">Optional layout options.</param>
    /// <returns>
    ///   <para>The enum options that has been selected by the user.</para>
    /// </returns>
    public static Enum EnumMaskPopup(GUIContent label, Enum selected, params GUILayoutOption[] options)
    {
      int changedFlags;
      bool changedToValue;
      return EditorGUILayout.EnumMaskPopup(label, selected, out changedFlags, out changedToValue);
    }

    internal static Enum EnumMaskPopup(GUIContent label, Enum selected, out int changedFlags, out bool changedToValue, params GUILayoutOption[] options)
    {
      GUIStyle popup = EditorStyles.popup;
      return EditorGUILayout.EnumMaskPopup(label, selected, out changedFlags, out changedToValue, popup, new GUILayoutOption[0]);
    }

    internal static Enum EnumMaskPopup(GUIContent label, Enum selected, out int changedFlags, out bool changedToValue, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.EnumMaskPopup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, selected, out changedFlags, out changedToValue, style);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    public static int IntPopup(int selectedValue, string[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
    {
      return EditorGUILayout.IntPopup(selectedValue, displayedOptions, optionValues, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    public static int IntPopup(int selectedValue, string[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.IntPopup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), selectedValue, displayedOptions, optionValues, style);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    public static int IntPopup(int selectedValue, GUIContent[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
    {
      return EditorGUILayout.IntPopup(selectedValue, displayedOptions, optionValues, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    public static int IntPopup(int selectedValue, GUIContent[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.IntPopup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), GUIContent.none, selectedValue, displayedOptions, optionValues, style);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    public static int IntPopup(string label, int selectedValue, string[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
    {
      return EditorGUILayout.IntPopup(label, selectedValue, displayedOptions, optionValues, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    public static int IntPopup(string label, int selectedValue, string[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.IntPopup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, selectedValue, displayedOptions, optionValues, style);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    public static int IntPopup(GUIContent label, int selectedValue, GUIContent[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
    {
      return EditorGUILayout.IntPopup(label, selectedValue, displayedOptions, optionValues, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="selectedValue">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value of the option that has been selected by the user.</para>
    /// </returns>
    public static int IntPopup(GUIContent label, int selectedValue, GUIContent[] displayedOptions, int[] optionValues, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.IntPopup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, selectedValue, displayedOptions, optionValues, style);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="property">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    public static void IntPopup(SerializedProperty property, GUIContent[] displayedOptions, int[] optionValues, params GUILayoutOption[] options)
    {
      EditorGUILayout.IntPopup(property, displayedOptions, optionValues, (GUIContent) null, options);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="property">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    public static void IntPopup(SerializedProperty property, GUIContent[] displayedOptions, int[] optionValues, GUIContent label, params GUILayoutOption[] options)
    {
      EditorGUI.IntPopup(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.popup, options), property, displayedOptions, optionValues, label);
    }

    /// <summary>
    ///   <para>Make an integer popup selection field.</para>
    /// </summary>
    /// <param name="property">The value of the option the field shows.</param>
    /// <param name="displayedOptions">An array with the displayed options the user can choose from.</param>
    /// <param name="optionValues">An array with the values for each option.</param>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    [Obsolete("This function is obsolete and the style is not used.")]
    public static void IntPopup(SerializedProperty property, GUIContent[] displayedOptions, int[] optionValues, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
    {
      EditorGUILayout.IntPopup(property, displayedOptions, optionValues, label, options);
    }

    /// <summary>
    ///   <para>Make a tag selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="tag">The tag the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The tag selected by the user.</para>
    /// </returns>
    public static string TagField(string tag, params GUILayoutOption[] options)
    {
      return EditorGUILayout.TagField(tag, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make a tag selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="tag">The tag the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The tag selected by the user.</para>
    /// </returns>
    public static string TagField(string tag, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.TagField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), tag, style);
    }

    /// <summary>
    ///   <para>Make a tag selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="tag">The tag the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The tag selected by the user.</para>
    /// </returns>
    public static string TagField(string label, string tag, params GUILayoutOption[] options)
    {
      return EditorGUILayout.TagField(EditorGUIUtility.TempContent(label), tag, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make a tag selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="tag">The tag the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The tag selected by the user.</para>
    /// </returns>
    public static string TagField(string label, string tag, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUILayout.TagField(EditorGUIUtility.TempContent(label), tag, style, options);
    }

    /// <summary>
    ///   <para>Make a tag selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="tag">The tag the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The tag selected by the user.</para>
    /// </returns>
    public static string TagField(GUIContent label, string tag, params GUILayoutOption[] options)
    {
      return EditorGUILayout.TagField(label, tag, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make a tag selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="tag">The tag the field shows.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The tag selected by the user.</para>
    /// </returns>
    public static string TagField(GUIContent label, string tag, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.TagField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, tag, style);
    }

    /// <summary>
    ///   <para>Make a layer selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="layer">The layer shown in the field.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The layer selected by the user.</para>
    /// </returns>
    public static int LayerField(int layer, params GUILayoutOption[] options)
    {
      return EditorGUILayout.LayerField(layer, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make a layer selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="layer">The layer shown in the field.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The layer selected by the user.</para>
    /// </returns>
    public static int LayerField(int layer, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.LayerField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), layer, style);
    }

    /// <summary>
    ///   <para>Make a layer selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="layer">The layer shown in the field.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The layer selected by the user.</para>
    /// </returns>
    public static int LayerField(string label, int layer, params GUILayoutOption[] options)
    {
      return EditorGUILayout.LayerField(EditorGUIUtility.TempContent(label), layer, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make a layer selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="layer">The layer shown in the field.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The layer selected by the user.</para>
    /// </returns>
    public static int LayerField(string label, int layer, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUILayout.LayerField(EditorGUIUtility.TempContent(label), layer, style, options);
    }

    /// <summary>
    ///   <para>Make a layer selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="layer">The layer shown in the field.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The layer selected by the user.</para>
    /// </returns>
    public static int LayerField(GUIContent label, int layer, params GUILayoutOption[] options)
    {
      return EditorGUILayout.LayerField(label, layer, EditorStyles.popup, options);
    }

    /// <summary>
    ///   <para>Make a layer selection field.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="layer">The layer shown in the field.</param>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The layer selected by the user.</para>
    /// </returns>
    public static int LayerField(GUIContent label, int layer, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.LayerField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, layer, style);
    }

    /// <summary>
    ///   <para>Make a field for masks.</para>
    /// </summary>
    /// <param name="label">Prefix label of the field.</param>
    /// <param name="mask">The current mask to display.</param>
    /// <param name="displayedOption">A string array containing the labels for each flag.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="displayedOptions"></param>
    /// <param name="style"></param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static int MaskField(GUIContent label, int mask, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.MaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, mask, displayedOptions, style);
    }

    /// <summary>
    ///   <para>Make a field for masks.</para>
    /// </summary>
    /// <param name="label">Prefix label of the field.</param>
    /// <param name="mask">The current mask to display.</param>
    /// <param name="displayedOption">A string array containing the labels for each flag.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="displayedOptions"></param>
    /// <param name="style"></param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static int MaskField(string label, int mask, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.MaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, mask, displayedOptions, style);
    }

    /// <summary>
    ///   <para>Make a field for masks.</para>
    /// </summary>
    /// <param name="label">Prefix label of the field.</param>
    /// <param name="mask">The current mask to display.</param>
    /// <param name="displayedOption">A string array containing the labels for each flag.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="displayedOptions"></param>
    /// <param name="style"></param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static int MaskField(GUIContent label, int mask, string[] displayedOptions, params GUILayoutOption[] options)
    {
      return EditorGUI.MaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.popup, options), label, mask, displayedOptions, EditorStyles.popup);
    }

    /// <summary>
    ///   <para>Make a field for masks.</para>
    /// </summary>
    /// <param name="label">Prefix label of the field.</param>
    /// <param name="mask">The current mask to display.</param>
    /// <param name="displayedOption">A string array containing the labels for each flag.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="displayedOptions"></param>
    /// <param name="style"></param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static int MaskField(string label, int mask, string[] displayedOptions, params GUILayoutOption[] options)
    {
      return EditorGUI.MaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.popup, options), label, mask, displayedOptions, EditorStyles.popup);
    }

    /// <summary>
    ///   <para>Make a field for masks.</para>
    /// </summary>
    /// <param name="label">Prefix label of the field.</param>
    /// <param name="mask">The current mask to display.</param>
    /// <param name="displayedOption">A string array containing the labels for each flag.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="displayedOptions"></param>
    /// <param name="style"></param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static int MaskField(int mask, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.MaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), mask, displayedOptions, style);
    }

    /// <summary>
    ///   <para>Make a field for masks.</para>
    /// </summary>
    /// <param name="label">Prefix label of the field.</param>
    /// <param name="mask">The current mask to display.</param>
    /// <param name="displayedOption">A string array containing the labels for each flag.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="displayedOptions"></param>
    /// <param name="style"></param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static int MaskField(int mask, string[] displayedOptions, params GUILayoutOption[] options)
    {
      return EditorGUI.MaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.popup, options), mask, displayedOptions, EditorStyles.popup);
    }

    /// <summary>
    ///   <para>Make a field for enum based masks.</para>
    /// </summary>
    /// <param name="label">Prefix label for this field.</param>
    /// <param name="enumValue">Enum to use for the flags.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static Enum EnumMaskField(GUIContent label, Enum enumValue, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.EnumMaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, enumValue, style);
    }

    /// <summary>
    ///   <para>Make a field for enum based masks.</para>
    /// </summary>
    /// <param name="label">Prefix label for this field.</param>
    /// <param name="enumValue">Enum to use for the flags.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static Enum EnumMaskField(string label, Enum enumValue, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.EnumMaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options), label, enumValue, style);
    }

    /// <summary>
    ///   <para>Make a field for enum based masks.</para>
    /// </summary>
    /// <param name="label">Prefix label for this field.</param>
    /// <param name="enumValue">Enum to use for the flags.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static Enum EnumMaskField(GUIContent label, Enum enumValue, params GUILayoutOption[] options)
    {
      return EditorGUI.EnumMaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.popup, options), label, enumValue, EditorStyles.popup);
    }

    /// <summary>
    ///   <para>Make a field for enum based masks.</para>
    /// </summary>
    /// <param name="label">Prefix label for this field.</param>
    /// <param name="enumValue">Enum to use for the flags.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static Enum EnumMaskField(string label, Enum enumValue, params GUILayoutOption[] options)
    {
      return EditorGUI.EnumMaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.popup, options), label, enumValue, EditorStyles.popup);
    }

    /// <summary>
    ///   <para>Make a field for enum based masks.</para>
    /// </summary>
    /// <param name="label">Prefix label for this field.</param>
    /// <param name="enumValue">Enum to use for the flags.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static Enum EnumMaskField(Enum enumValue, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.EnumMaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options), enumValue, style);
    }

    /// <summary>
    ///   <para>Make a field for enum based masks.</para>
    /// </summary>
    /// <param name="label">Prefix label for this field.</param>
    /// <param name="enumValue">Enum to use for the flags.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <param name="style"></param>
    /// <returns>
    ///   <para>The value modified by the user.</para>
    /// </returns>
    public static Enum EnumMaskField(Enum enumValue, params GUILayoutOption[] options)
    {
      return EditorGUI.EnumMaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.popup, options), enumValue, EditorStyles.popup);
    }

    [Obsolete("Check the docs for the usage of the new parameter 'allowSceneObjects'.")]
    public static UnityEngine.Object ObjectField(UnityEngine.Object obj, System.Type objType, params GUILayoutOption[] options)
    {
      return EditorGUILayout.ObjectField(obj, objType, true, options);
    }

    /// <summary>
    ///   <para>Make a field to receive any object type.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="obj">The object the field shows.</param>
    /// <param name="objType">The type of the objects that can be assigned.</param>
    /// <param name="allowSceneObjects">Allow assigning scene objects. See Description for more info.</param>
    /// <param name="options">An optional list of layout options that specify extra layout properties. Any values passed in here will override settings defined by the style.
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The object that has been set by the user.</para>
    /// </returns>
    public static UnityEngine.Object ObjectField(UnityEngine.Object obj, System.Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
    {
      return EditorGUI.ObjectField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, options), obj, objType, allowSceneObjects);
    }

    [Obsolete("Check the docs for the usage of the new parameter 'allowSceneObjects'.")]
    public static UnityEngine.Object ObjectField(string label, UnityEngine.Object obj, System.Type objType, params GUILayoutOption[] options)
    {
      return EditorGUILayout.ObjectField(label, obj, objType, true, options);
    }

    /// <summary>
    ///   <para>Make a field to receive any object type.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="obj">The object the field shows.</param>
    /// <param name="objType">The type of the objects that can be assigned.</param>
    /// <param name="allowSceneObjects">Allow assigning scene objects. See Description for more info.</param>
    /// <param name="options">An optional list of layout options that specify extra layout properties. Any values passed in here will override settings defined by the style.
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The object that has been set by the user.</para>
    /// </returns>
    public static UnityEngine.Object ObjectField(string label, UnityEngine.Object obj, System.Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
    {
      return EditorGUILayout.ObjectField(EditorGUIUtility.TempContent(label), obj, objType, allowSceneObjects, options);
    }

    [Obsolete("Check the docs for the usage of the new parameter 'allowSceneObjects'.")]
    public static UnityEngine.Object ObjectField(GUIContent label, UnityEngine.Object obj, System.Type objType, params GUILayoutOption[] options)
    {
      return EditorGUILayout.ObjectField(label, obj, objType, true, options);
    }

    /// <summary>
    ///   <para>Make a field to receive any object type.</para>
    /// </summary>
    /// <param name="label">Optional label in front of the field.</param>
    /// <param name="obj">The object the field shows.</param>
    /// <param name="objType">The type of the objects that can be assigned.</param>
    /// <param name="allowSceneObjects">Allow assigning scene objects. See Description for more info.</param>
    /// <param name="options">An optional list of layout options that specify extra layout properties. Any values passed in here will override settings defined by the style.
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The object that has been set by the user.</para>
    /// </returns>
    public static UnityEngine.Object ObjectField(GUIContent label, UnityEngine.Object obj, System.Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
    {
      float height = !EditorGUIUtility.HasObjectThumbnail(objType) ? 16f : 64f;
      return EditorGUI.ObjectField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, height, options), label, obj, objType, allowSceneObjects);
    }

    internal static UnityEngine.Object MiniThumbnailObjectField(GUIContent label, UnityEngine.Object obj, System.Type objType, EditorGUI.ObjectFieldValidator validator, params GUILayoutOption[] options)
    {
      return EditorGUI.MiniThumbnailObjectField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, options), label, obj, objType, validator);
    }

    /// <summary>
    ///   <para>Make an X &amp; Y field for entering a Vector2.</para>
    /// </summary>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Vector2 Vector2Field(string label, Vector2 value, params GUILayoutOption[] options)
    {
      int num1 = 1;
      double num2 = (!EditorGUIUtility.wideMode ? 16.0 : 0.0) + 16.0;
      GUIStyle numberField = EditorStyles.numberField;
      GUILayoutOption[] guiLayoutOptionArray = options;
      return EditorGUI.Vector2Field(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(num1 != 0, (float) num2, numberField, guiLayoutOptionArray), label, value);
    }

    /// <summary>
    ///   <para>Make an X &amp; Y field for entering a Vector2.</para>
    /// </summary>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Vector2 Vector2Field(GUIContent label, Vector2 value, params GUILayoutOption[] options)
    {
      int num1 = 1;
      double num2 = (!EditorGUIUtility.wideMode ? 16.0 : 0.0) + 16.0;
      GUIStyle numberField = EditorStyles.numberField;
      GUILayoutOption[] guiLayoutOptionArray = options;
      return EditorGUI.Vector2Field(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(num1 != 0, (float) num2, numberField, guiLayoutOptionArray), label, value);
    }

    /// <summary>
    ///   <para>Make an X, Y &amp; Z field for entering a Vector3.</para>
    /// </summary>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Vector3 Vector3Field(string label, Vector3 value, params GUILayoutOption[] options)
    {
      int num1 = 1;
      double num2 = (!EditorGUIUtility.wideMode ? 16.0 : 0.0) + 16.0;
      GUIStyle numberField = EditorStyles.numberField;
      GUILayoutOption[] guiLayoutOptionArray = options;
      return EditorGUI.Vector3Field(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(num1 != 0, (float) num2, numberField, guiLayoutOptionArray), label, value);
    }

    /// <summary>
    ///   <para>Make an X, Y &amp; Z field for entering a Vector3.</para>
    /// </summary>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Vector3 Vector3Field(GUIContent label, Vector3 value, params GUILayoutOption[] options)
    {
      int num1 = 1;
      double num2 = (!EditorGUIUtility.wideMode ? 16.0 : 0.0) + 16.0;
      GUIStyle numberField = EditorStyles.numberField;
      GUILayoutOption[] guiLayoutOptionArray = options;
      return EditorGUI.Vector3Field(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(num1 != 0, (float) num2, numberField, guiLayoutOptionArray), label, value);
    }

    /// <summary>
    ///   <para>Make an X, Y, Z &amp; W field for entering a Vector4.</para>
    /// </summary>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Vector4 Vector4Field(string label, Vector4 value, params GUILayoutOption[] options)
    {
      return EditorGUI.Vector4Field(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 32f, EditorStyles.numberField, options), label, value);
    }

    /// <summary>
    ///   <para>Make an X, Y, W &amp; H field for entering a Rect.</para>
    /// </summary>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Rect RectField(Rect value, params GUILayoutOption[] options)
    {
      return EditorGUI.RectField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 32f, EditorStyles.numberField, options), value);
    }

    /// <summary>
    ///   <para>Make an X, Y, W &amp; H field for entering a Rect.</para>
    /// </summary>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Rect RectField(string label, Rect value, params GUILayoutOption[] options)
    {
      int num1 = 1;
      double num2 = (!EditorGUIUtility.wideMode ? 16.0 : 0.0) + 32.0;
      GUIStyle numberField = EditorStyles.numberField;
      GUILayoutOption[] guiLayoutOptionArray = options;
      return EditorGUI.RectField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(num1 != 0, (float) num2, numberField, guiLayoutOptionArray), label, value);
    }

    /// <summary>
    ///   <para>Make an X, Y, W &amp; H field for entering a Rect.</para>
    /// </summary>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Rect RectField(GUIContent label, Rect value, params GUILayoutOption[] options)
    {
      bool hasLabel = EditorGUI.LabelHasContent(label);
      float height = (float) ((!hasLabel || EditorGUIUtility.wideMode ? 0.0 : 16.0) + 32.0);
      return EditorGUI.RectField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(hasLabel, height, EditorStyles.numberField, options), label, value);
    }

    /// <summary>
    ///   <para>Make Center &amp; Extents field for entering a Bounds.</para>
    /// </summary>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Bounds BoundsField(Bounds value, params GUILayoutOption[] options)
    {
      return EditorGUI.BoundsField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 32f, EditorStyles.numberField, options), value);
    }

    /// <summary>
    ///   <para>Make Center &amp; Extents field for entering a Bounds.</para>
    /// </summary>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Bounds BoundsField(string label, Bounds value, params GUILayoutOption[] options)
    {
      return EditorGUILayout.BoundsField(new GUIContent(label), value, options);
    }

    /// <summary>
    ///   <para>Make Center &amp; Extents field for entering a Bounds.</para>
    /// </summary>
    /// <param name="label">Label to display above the field.</param>
    /// <param name="value">The value to edit.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The value entered by the user.</para>
    /// </returns>
    public static Bounds BoundsField(GUIContent label, Bounds value, params GUILayoutOption[] options)
    {
      bool hasLabel = EditorGUI.LabelHasContent(label);
      float height = (float) ((hasLabel ? 16.0 : 0.0) + 32.0);
      return EditorGUI.BoundsField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(hasLabel, height, EditorStyles.numberField, options), label, value);
    }

    internal static void PropertiesField(GUIContent label, SerializedProperty[] properties, GUIContent[] propertyLabels, float propertyLabelsWidth, params GUILayoutOption[] options)
    {
      bool hasLabel = EditorGUI.LabelHasContent(label);
      float height = 16f * (float) properties.Length;
      EditorGUI.PropertiesField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(hasLabel, height, EditorStyles.numberField, options), label, properties, propertyLabels, propertyLabelsWidth);
    }

    internal static int CycleButton(int selected, GUIContent[] options, GUIStyle style)
    {
      if (GUILayout.Button(options[selected], style, new GUILayoutOption[0]))
      {
        ++selected;
        if (selected >= options.Length)
          selected = 0;
      }
      return selected;
    }

    /// <summary>
    ///   <para>Make a field for selecting a Color.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The color to edit.</param>
    /// <param name="showEyedropper">If true, the color picker should show the eyedropper control. If false, don't show it.</param>
    /// <param name="showAlpha">If true, allow the user to set an alpha value for the color. If false, hide the alpha component.</param>
    /// <param name="hdr">If true, treat the color as an HDR value. If false, treat it as a standard LDR value.</param>
    /// <param name="hdrConfig">An object that sets the presentation parameters for an HDR color. If not using an HDR color, set this to null.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The color selected by the user.</para>
    /// </returns>
    public static Color ColorField(Color value, params GUILayoutOption[] options)
    {
      return EditorGUI.ColorField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.colorField, options), value);
    }

    /// <summary>
    ///   <para>Make a field for selecting a Color.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The color to edit.</param>
    /// <param name="showEyedropper">If true, the color picker should show the eyedropper control. If false, don't show it.</param>
    /// <param name="showAlpha">If true, allow the user to set an alpha value for the color. If false, hide the alpha component.</param>
    /// <param name="hdr">If true, treat the color as an HDR value. If false, treat it as a standard LDR value.</param>
    /// <param name="hdrConfig">An object that sets the presentation parameters for an HDR color. If not using an HDR color, set this to null.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The color selected by the user.</para>
    /// </returns>
    public static Color ColorField(string label, Color value, params GUILayoutOption[] options)
    {
      return EditorGUI.ColorField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.colorField, options), label, value);
    }

    /// <summary>
    ///   <para>Make a field for selecting a Color.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The color to edit.</param>
    /// <param name="showEyedropper">If true, the color picker should show the eyedropper control. If false, don't show it.</param>
    /// <param name="showAlpha">If true, allow the user to set an alpha value for the color. If false, hide the alpha component.</param>
    /// <param name="hdr">If true, treat the color as an HDR value. If false, treat it as a standard LDR value.</param>
    /// <param name="hdrConfig">An object that sets the presentation parameters for an HDR color. If not using an HDR color, set this to null.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The color selected by the user.</para>
    /// </returns>
    public static Color ColorField(GUIContent label, Color value, params GUILayoutOption[] options)
    {
      return EditorGUI.ColorField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.colorField, options), label, value);
    }

    /// <summary>
    ///   <para>Make a field for selecting a Color.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The color to edit.</param>
    /// <param name="showEyedropper">If true, the color picker should show the eyedropper control. If false, don't show it.</param>
    /// <param name="showAlpha">If true, allow the user to set an alpha value for the color. If false, hide the alpha component.</param>
    /// <param name="hdr">If true, treat the color as an HDR value. If false, treat it as a standard LDR value.</param>
    /// <param name="hdrConfig">An object that sets the presentation parameters for an HDR color. If not using an HDR color, set this to null.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The color selected by the user.</para>
    /// </returns>
    public static Color ColorField(GUIContent label, Color value, bool showEyedropper, bool showAlpha, bool hdr, ColorPickerHDRConfig hdrConfig, params GUILayoutOption[] options)
    {
      return EditorGUI.ColorField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.colorField, options), label, value, showEyedropper, showAlpha, hdr, hdrConfig);
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The curve edited by the user.</para>
    /// </returns>
    public static AnimationCurve CurveField(AnimationCurve value, params GUILayoutOption[] options)
    {
      return EditorGUI.CurveField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.colorField, options), value);
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The curve edited by the user.</para>
    /// </returns>
    public static AnimationCurve CurveField(string label, AnimationCurve value, params GUILayoutOption[] options)
    {
      return EditorGUI.CurveField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.colorField, options), label, value);
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The curve edited by the user.</para>
    /// </returns>
    public static AnimationCurve CurveField(GUIContent label, AnimationCurve value, params GUILayoutOption[] options)
    {
      return EditorGUI.CurveField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.colorField, options), label, value);
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The curve edited by the user.</para>
    /// </returns>
    public static AnimationCurve CurveField(AnimationCurve value, Color color, Rect ranges, params GUILayoutOption[] options)
    {
      return EditorGUI.CurveField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.colorField, options), value, color, ranges);
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The curve edited by the user.</para>
    /// </returns>
    public static AnimationCurve CurveField(string label, AnimationCurve value, Color color, Rect ranges, params GUILayoutOption[] options)
    {
      return EditorGUI.CurveField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.colorField, options), label, value, color, ranges);
    }

    /// <summary>
    ///   <para>Make a field for editing an AnimationCurve.</para>
    /// </summary>
    /// <param name="label">Optional label to display in front of the field.</param>
    /// <param name="value">The curve to edit.</param>
    /// <param name="color">The color to show the curve with.</param>
    /// <param name="ranges">Optional rectangle that the curve is restrained within.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>The curve edited by the user.</para>
    /// </returns>
    public static AnimationCurve CurveField(GUIContent label, AnimationCurve value, Color color, Rect ranges, params GUILayoutOption[] options)
    {
      return EditorGUI.CurveField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.colorField, options), label, value, color, ranges);
    }

    internal static void CurveField(SerializedProperty value, Color color, Rect ranges, params GUILayoutOption[] options)
    {
      EditorGUI.CurveField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, EditorStyles.colorField, options), value, color, ranges);
    }

    /// <summary>
    ///   <para>Make an inspector-window-like titlebar.</para>
    /// </summary>
    /// <param name="foldout">The foldout state shown with the arrow.</param>
    /// <param name="targetObj">The object (for example a component) or objects that the titlebar is for.</param>
    /// <param name="targetObjs"></param>
    /// <returns>
    ///   <para>The foldout state selected by the user.</para>
    /// </returns>
    public static bool InspectorTitlebar(bool foldout, UnityEngine.Object targetObj)
    {
      return EditorGUILayout.InspectorTitlebar(foldout, targetObj, true);
    }

    public static bool InspectorTitlebar(bool foldout, UnityEngine.Object targetObj, bool expandable)
    {
      return EditorGUI.InspectorTitlebar(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.inspectorTitlebar), foldout, targetObj, expandable);
    }

    /// <summary>
    ///   <para>Make an inspector-window-like titlebar.</para>
    /// </summary>
    /// <param name="foldout">The foldout state shown with the arrow.</param>
    /// <param name="targetObj">The object (for example a component) or objects that the titlebar is for.</param>
    /// <param name="targetObjs"></param>
    /// <returns>
    ///   <para>The foldout state selected by the user.</para>
    /// </returns>
    public static bool InspectorTitlebar(bool foldout, UnityEngine.Object[] targetObjs)
    {
      return EditorGUILayout.InspectorTitlebar(foldout, targetObjs, true);
    }

    public static bool InspectorTitlebar(bool foldout, UnityEngine.Object[] targetObjs, bool expandable)
    {
      return EditorGUI.InspectorTitlebar(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.inspectorTitlebar), foldout, targetObjs, expandable);
    }

    public static void InspectorTitlebar(UnityEngine.Object[] targetObjs)
    {
      EditorGUI.InspectorTitlebar(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.inspectorTitlebar), targetObjs);
    }

    internal static bool ToggleTitlebar(bool foldout, GUIContent label, ref bool toggleValue)
    {
      return EditorGUI.ToggleTitlebar(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.inspectorTitlebar), label, foldout, ref toggleValue);
    }

    internal static bool ToggleTitlebar(bool foldout, GUIContent label, SerializedProperty property)
    {
      bool boolValue = property.boolValue;
      EditorGUI.BeginChangeCheck();
      foldout = EditorGUI.ToggleTitlebar(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.inspectorTitlebar), label, foldout, ref boolValue);
      if (EditorGUI.EndChangeCheck())
        property.boolValue = boolValue;
      return foldout;
    }

    internal static bool FoldoutTitlebar(bool foldout, GUIContent label)
    {
      return EditorGUI.FoldoutTitlebar(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.inspectorTitlebar), label, foldout);
    }

    internal static bool FoldoutInternal(bool foldout, GUIContent content, GUIStyle style)
    {
      return EditorGUI.Foldout(EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUIUtility.fieldWidth, 16f, 16f, style), foldout, content, style);
    }

    internal static void ObjectField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
    {
      EditorGUI.ObjectField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.objectField, options), property, label);
    }

    internal static void LayerMaskField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
    {
      EditorGUI.LayerMaskField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, options), property, label);
    }

    /// <summary>
    ///   <para>Make a help box with a message to the user.</para>
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="type">The type of message.</param>
    /// <param name="wide">If true, the box will cover the whole width of the window; otherwise it will cover the controls part only.</param>
    public static void HelpBox(string message, MessageType type)
    {
      EditorGUILayout.LabelField(GUIContent.none, EditorGUIUtility.TempContent(message, (Texture) EditorGUIUtility.GetHelpIcon(type)), EditorStyles.helpBox, new GUILayoutOption[0]);
    }

    /// <summary>
    ///   <para>Make a help box with a message to the user.</para>
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="type">The type of message.</param>
    /// <param name="wide">If true, the box will cover the whole width of the window; otherwise it will cover the controls part only.</param>
    public static void HelpBox(string message, MessageType type, bool wide)
    {
      EditorGUILayout.LabelField(!wide ? EditorGUIUtility.blankContent : GUIContent.none, EditorGUIUtility.TempContent(message, (Texture) EditorGUIUtility.GetHelpIcon(type)), EditorStyles.helpBox, new GUILayoutOption[0]);
    }

    internal static void PrefixLabelInternal(GUIContent label, GUIStyle followingStyle, GUIStyle labelStyle)
    {
      float left = (float) followingStyle.margin.left;
      if (!EditorGUI.LabelHasContent(label))
      {
        GUILayoutUtility.GetRect((float) ((double) EditorGUI.indent - (double) left), 16f, followingStyle, new GUILayoutOption[1]
        {
          GUILayout.ExpandWidth(false)
        });
      }
      else
      {
        Rect rect = GUILayoutUtility.GetRect((float) ((double) EditorGUIUtility.labelWidth - (double) left), 16f, followingStyle, new GUILayoutOption[1]
        {
          GUILayout.ExpandWidth(false)
        });
        rect.xMin += EditorGUI.indent;
        EditorGUI.HandlePrefixLabel(rect, rect, label, 0, labelStyle);
      }
    }

    /// <summary>
    ///   <para>Make a small space between the previous control and the following.</para>
    /// </summary>
    public static void Space()
    {
      GUILayoutUtility.GetRect(6f, 6f);
    }

    public static void Separator()
    {
      EditorGUILayout.Space();
    }

    /// <summary>
    ///   <para>Begin a vertical group with a toggle to enable or disable all the controls within at once.</para>
    /// </summary>
    /// <param name="label">Label to show above the toggled controls.</param>
    /// <param name="toggle">Enabled state of the toggle group.</param>
    /// <returns>
    ///   <para>The enabled state selected by the user.</para>
    /// </returns>
    public static bool BeginToggleGroup(string label, bool toggle)
    {
      return EditorGUILayout.BeginToggleGroup(EditorGUIUtility.TempContent(label), toggle);
    }

    /// <summary>
    ///   <para>Begin a vertical group with a toggle to enable or disable all the controls within at once.</para>
    /// </summary>
    /// <param name="label">Label to show above the toggled controls.</param>
    /// <param name="toggle">Enabled state of the toggle group.</param>
    /// <returns>
    ///   <para>The enabled state selected by the user.</para>
    /// </returns>
    public static bool BeginToggleGroup(GUIContent label, bool toggle)
    {
      toggle = EditorGUILayout.ToggleLeft(label, toggle, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUI.BeginDisabledGroup(!toggle);
      GUILayout.BeginVertical();
      return toggle;
    }

    /// <summary>
    ///   <para>Close a group started with BeginToggleGroup.</para>
    /// </summary>
    public static void EndToggleGroup()
    {
      GUILayout.EndVertical();
      EditorGUI.EndDisabledGroup();
    }

    /// <summary>
    ///   <para>Begin a horizontal group and get its rect back.</para>
    /// </summary>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static Rect BeginHorizontal(params GUILayoutOption[] options)
    {
      return EditorGUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, options);
    }

    /// <summary>
    ///   <para>Begin a horizontal group and get its rect back.</para>
    /// </summary>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static Rect BeginHorizontal(GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUILayout.BeginHorizontal(GUIContent.none, style, options);
    }

    internal static Rect BeginHorizontal(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
    {
      GUILayoutUtility.BeginGroup("GUILayout.EndVertical");
      GUILayoutGroup guiLayoutGroup = GUILayoutUtility.BeginLayoutGroup(style, options, typeof (GUILayoutGroup));
      guiLayoutGroup.isVertical = false;
      if (style != GUIStyle.none || content != GUIContent.none)
        GUI.Box(guiLayoutGroup.rect, GUIContent.none, style);
      return guiLayoutGroup.rect;
    }

    /// <summary>
    ///   <para>Close a group started with BeginHorizontal.</para>
    /// </summary>
    public static void EndHorizontal()
    {
      GUILayout.EndHorizontal();
    }

    /// <summary>
    ///   <para>Begin a vertical group and get its rect back.</para>
    /// </summary>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static Rect BeginVertical(params GUILayoutOption[] options)
    {
      return EditorGUILayout.BeginVertical(GUIContent.none, GUIStyle.none, options);
    }

    /// <summary>
    ///   <para>Begin a vertical group and get its rect back.</para>
    /// </summary>
    /// <param name="style">Optional GUIStyle.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static Rect BeginVertical(GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUILayout.BeginVertical(GUIContent.none, style, options);
    }

    internal static Rect BeginVertical(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
    {
      GUILayoutUtility.BeginGroup("GUILayout.EndVertical");
      GUILayoutGroup guiLayoutGroup = GUILayoutUtility.BeginLayoutGroup(style, options, typeof (GUILayoutGroup));
      guiLayoutGroup.isVertical = true;
      if (style != GUIStyle.none || content != GUIContent.none)
        GUI.Box(guiLayoutGroup.rect, GUIContent.none, style);
      return guiLayoutGroup.rect;
    }

    /// <summary>
    ///   <para>Close a group started with BeginVertical.</para>
    /// </summary>
    public static void EndVertical()
    {
      GUILayout.EndVertical();
    }

    /// <summary>
    ///   <para>Begin an automatically layouted scrollview.</para>
    /// </summary>
    /// <param name="scrollPosition">The position to use display.</param>
    /// <param name="alwayShowHorizontal">Optional parameter to always show the horizontal scrollbar. If false or left out, it is only shown when the content inside the ScrollView is wider than the scrollview itself.</param>
    /// <param name="alwayShowVertical">Optional parameter to always show the vertical scrollbar. If false or left out, it is only shown when content inside the ScrollView is taller than the scrollview itself.</param>
    /// <param name="horizontalScrollbar">Optional GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current GUISkin is used.</param>
    /// <param name="verticalScrollbar">Optional GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current GUISkin is used.</param>
    /// <param name="options"></param>
    /// <param name="alwaysShowHorizontal"></param>
    /// <param name="alwaysShowVertical"></param>
    /// <param name="background"></param>
    /// <returns>
    ///   <para>The modified scrollPosition. Feed this back into the variable you pass in, as shown in the example.</para>
    /// </returns>
    public static Vector2 BeginScrollView(Vector2 scrollPosition, params GUILayoutOption[] options)
    {
      return EditorGUILayout.BeginScrollView(scrollPosition, false, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
    }

    /// <summary>
    ///   <para>Begin an automatically layouted scrollview.</para>
    /// </summary>
    /// <param name="scrollPosition">The position to use display.</param>
    /// <param name="alwayShowHorizontal">Optional parameter to always show the horizontal scrollbar. If false or left out, it is only shown when the content inside the ScrollView is wider than the scrollview itself.</param>
    /// <param name="alwayShowVertical">Optional parameter to always show the vertical scrollbar. If false or left out, it is only shown when content inside the ScrollView is taller than the scrollview itself.</param>
    /// <param name="horizontalScrollbar">Optional GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current GUISkin is used.</param>
    /// <param name="verticalScrollbar">Optional GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current GUISkin is used.</param>
    /// <param name="options"></param>
    /// <param name="alwaysShowHorizontal"></param>
    /// <param name="alwaysShowVertical"></param>
    /// <param name="background"></param>
    /// <returns>
    ///   <para>The modified scrollPosition. Feed this back into the variable you pass in, as shown in the example.</para>
    /// </returns>
    public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
    {
      return EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
    }

    /// <summary>
    ///   <para>Begin an automatically layouted scrollview.</para>
    /// </summary>
    /// <param name="scrollPosition">The position to use display.</param>
    /// <param name="alwayShowHorizontal">Optional parameter to always show the horizontal scrollbar. If false or left out, it is only shown when the content inside the ScrollView is wider than the scrollview itself.</param>
    /// <param name="alwayShowVertical">Optional parameter to always show the vertical scrollbar. If false or left out, it is only shown when content inside the ScrollView is taller than the scrollview itself.</param>
    /// <param name="horizontalScrollbar">Optional GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current GUISkin is used.</param>
    /// <param name="verticalScrollbar">Optional GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current GUISkin is used.</param>
    /// <param name="options"></param>
    /// <param name="alwaysShowHorizontal"></param>
    /// <param name="alwaysShowVertical"></param>
    /// <param name="background"></param>
    /// <returns>
    ///   <para>The modified scrollPosition. Feed this back into the variable you pass in, as shown in the example.</para>
    /// </returns>
    public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
    {
      return EditorGUILayout.BeginScrollView(scrollPosition, false, false, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, options);
    }

    public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
    {
      string name = style.name;
      GUIStyle verticalScrollbar = GUI.skin.FindStyle(name + "VerticalScrollbar") ?? GUI.skin.verticalScrollbar;
      GUIStyle horizontalScrollbar = GUI.skin.FindStyle(name + "HorizontalScrollbar") ?? GUI.skin.horizontalScrollbar;
      return EditorGUILayout.BeginScrollView(scrollPosition, false, false, horizontalScrollbar, verticalScrollbar, style, options);
    }

    internal static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
    {
      return EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, options);
    }

    /// <summary>
    ///   <para>Begin an automatically layouted scrollview.</para>
    /// </summary>
    /// <param name="scrollPosition">The position to use display.</param>
    /// <param name="alwayShowHorizontal">Optional parameter to always show the horizontal scrollbar. If false or left out, it is only shown when the content inside the ScrollView is wider than the scrollview itself.</param>
    /// <param name="alwayShowVertical">Optional parameter to always show the vertical scrollbar. If false or left out, it is only shown when content inside the ScrollView is taller than the scrollview itself.</param>
    /// <param name="horizontalScrollbar">Optional GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current GUISkin is used.</param>
    /// <param name="verticalScrollbar">Optional GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current GUISkin is used.</param>
    /// <param name="options"></param>
    /// <param name="alwaysShowHorizontal"></param>
    /// <param name="alwaysShowVertical"></param>
    /// <param name="background"></param>
    /// <returns>
    ///   <para>The modified scrollPosition. Feed this back into the variable you pass in, as shown in the example.</para>
    /// </returns>
    public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options)
    {
      GUIScrollGroup guiScrollGroup = (GUIScrollGroup) GUILayoutUtility.BeginLayoutGroup(background, (GUILayoutOption[]) null, typeof (GUIScrollGroup));
      if (Event.current.type == EventType.Layout)
      {
        guiScrollGroup.resetCoords = true;
        guiScrollGroup.isVertical = true;
        guiScrollGroup.stretchWidth = 1;
        guiScrollGroup.stretchHeight = 1;
        guiScrollGroup.verticalScrollbar = verticalScrollbar;
        guiScrollGroup.horizontalScrollbar = horizontalScrollbar;
        guiScrollGroup.ApplyOptions(options);
      }
      return EditorGUIInternal.DoBeginScrollViewForward(guiScrollGroup.rect, scrollPosition, new Rect(0.0f, 0.0f, guiScrollGroup.clientWidth, guiScrollGroup.clientHeight), alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background);
    }

    internal static Vector2 BeginVerticalScrollView(Vector2 scrollPosition, params GUILayoutOption[] options)
    {
      return EditorGUILayout.BeginVerticalScrollView(scrollPosition, false, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
    }

    internal static Vector2 BeginVerticalScrollView(Vector2 scrollPosition, bool alwaysShowVertical, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options)
    {
      GUIScrollGroup guiScrollGroup = (GUIScrollGroup) GUILayoutUtility.BeginLayoutGroup(background, (GUILayoutOption[]) null, typeof (GUIScrollGroup));
      if (Event.current.type == EventType.Layout)
      {
        guiScrollGroup.resetCoords = true;
        guiScrollGroup.isVertical = true;
        guiScrollGroup.stretchWidth = 1;
        guiScrollGroup.stretchHeight = 1;
        guiScrollGroup.verticalScrollbar = verticalScrollbar;
        guiScrollGroup.horizontalScrollbar = GUIStyle.none;
        guiScrollGroup.allowHorizontalScroll = false;
        guiScrollGroup.ApplyOptions(options);
      }
      return EditorGUIInternal.DoBeginScrollViewForward(guiScrollGroup.rect, scrollPosition, new Rect(0.0f, 0.0f, guiScrollGroup.clientWidth, guiScrollGroup.clientHeight), false, alwaysShowVertical, GUI.skin.horizontalScrollbar, verticalScrollbar, background);
    }

    internal static Vector2 BeginHorizontalScrollView(Vector2 scrollPosition, params GUILayoutOption[] options)
    {
      return EditorGUILayout.BeginHorizontalScrollView(scrollPosition, false, GUI.skin.horizontalScrollbar, GUI.skin.scrollView, options);
    }

    internal static Vector2 BeginHorizontalScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, GUIStyle horizontalScrollbar, GUIStyle background, params GUILayoutOption[] options)
    {
      GUIScrollGroup guiScrollGroup = (GUIScrollGroup) GUILayoutUtility.BeginLayoutGroup(background, (GUILayoutOption[]) null, typeof (GUIScrollGroup));
      if (Event.current.type == EventType.Layout)
      {
        guiScrollGroup.resetCoords = true;
        guiScrollGroup.isVertical = true;
        guiScrollGroup.stretchWidth = 1;
        guiScrollGroup.stretchHeight = 1;
        guiScrollGroup.verticalScrollbar = GUIStyle.none;
        guiScrollGroup.horizontalScrollbar = horizontalScrollbar;
        guiScrollGroup.allowHorizontalScroll = true;
        guiScrollGroup.allowVerticalScroll = false;
        guiScrollGroup.ApplyOptions(options);
      }
      return EditorGUIInternal.DoBeginScrollViewForward(guiScrollGroup.rect, scrollPosition, new Rect(0.0f, 0.0f, guiScrollGroup.clientWidth, guiScrollGroup.clientHeight), alwaysShowHorizontal, false, horizontalScrollbar, GUI.skin.verticalScrollbar, background);
    }

    /// <summary>
    ///   <para>Ends a scrollview started with a call to BeginScrollView.</para>
    /// </summary>
    public static void EndScrollView()
    {
      GUILayout.EndScrollView(true);
    }

    internal static void EndScrollView(bool handleScrollWheel)
    {
      GUILayout.EndScrollView(handleScrollWheel);
    }

    /// <summary>
    ///   <para>Make a field for SerializedProperty.</para>
    /// </summary>
    /// <param name="property">The SerializedProperty to make a field for.</param>
    /// <param name="label">Optional label to use. If not specified the label of the property itself is used. Use GUIContent.none to not display a label at all.</param>
    /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>True if the property has children and is expanded and includeChildren was set to false; otherwise false.</para>
    /// </returns>
    public static bool PropertyField(SerializedProperty property, params GUILayoutOption[] options)
    {
      return EditorGUILayout.PropertyField(property, (GUIContent) null, false, options);
    }

    /// <summary>
    ///   <para>Make a field for SerializedProperty.</para>
    /// </summary>
    /// <param name="property">The SerializedProperty to make a field for.</param>
    /// <param name="label">Optional label to use. If not specified the label of the property itself is used. Use GUIContent.none to not display a label at all.</param>
    /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>True if the property has children and is expanded and includeChildren was set to false; otherwise false.</para>
    /// </returns>
    public static bool PropertyField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
    {
      return EditorGUILayout.PropertyField(property, label, false, options);
    }

    /// <summary>
    ///   <para>Make a field for SerializedProperty.</para>
    /// </summary>
    /// <param name="property">The SerializedProperty to make a field for.</param>
    /// <param name="label">Optional label to use. If not specified the label of the property itself is used. Use GUIContent.none to not display a label at all.</param>
    /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>True if the property has children and is expanded and includeChildren was set to false; otherwise false.</para>
    /// </returns>
    public static bool PropertyField(SerializedProperty property, bool includeChildren, params GUILayoutOption[] options)
    {
      return EditorGUILayout.PropertyField(property, (GUIContent) null, includeChildren, options);
    }

    /// <summary>
    ///   <para>Make a field for SerializedProperty.</para>
    /// </summary>
    /// <param name="property">The SerializedProperty to make a field for.</param>
    /// <param name="label">Optional label to use. If not specified the label of the property itself is used. Use GUIContent.none to not display a label at all.</param>
    /// <param name="includeChildren">If true the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    /// <returns>
    ///   <para>True if the property has children and is expanded and includeChildren was set to false; otherwise false.</para>
    /// </returns>
    public static bool PropertyField(SerializedProperty property, GUIContent label, bool includeChildren, params GUILayoutOption[] options)
    {
      return ScriptAttributeUtility.GetHandler(property).OnGUILayout(property, label, includeChildren, options);
    }

    /// <summary>
    ///   <para>Get a rect for an Editor control.</para>
    /// </summary>
    /// <param name="hasLabel">Optional boolean to specify if the control has a label. Default is true.</param>
    /// <param name="height">The height in pixels of the control. Default is EditorGUIUtility.singleLineHeight.</param>
    /// <param name="style">Optional GUIStyle to use for the control.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static Rect GetControlRect(params GUILayoutOption[] options)
    {
      return EditorGUILayout.GetControlRect(true, 16f, EditorStyles.layerMaskField, options);
    }

    /// <summary>
    ///   <para>Get a rect for an Editor control.</para>
    /// </summary>
    /// <param name="hasLabel">Optional boolean to specify if the control has a label. Default is true.</param>
    /// <param name="height">The height in pixels of the control. Default is EditorGUIUtility.singleLineHeight.</param>
    /// <param name="style">Optional GUIStyle to use for the control.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static Rect GetControlRect(bool hasLabel, params GUILayoutOption[] options)
    {
      return EditorGUILayout.GetControlRect(hasLabel, 16f, EditorStyles.layerMaskField, options);
    }

    /// <summary>
    ///   <para>Get a rect for an Editor control.</para>
    /// </summary>
    /// <param name="hasLabel">Optional boolean to specify if the control has a label. Default is true.</param>
    /// <param name="height">The height in pixels of the control. Default is EditorGUIUtility.singleLineHeight.</param>
    /// <param name="style">Optional GUIStyle to use for the control.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static Rect GetControlRect(bool hasLabel, float height, params GUILayoutOption[] options)
    {
      return EditorGUILayout.GetControlRect(hasLabel, height, EditorStyles.layerMaskField, options);
    }

    /// <summary>
    ///   <para>Get a rect for an Editor control.</para>
    /// </summary>
    /// <param name="hasLabel">Optional boolean to specify if the control has a label. Default is true.</param>
    /// <param name="height">The height in pixels of the control. Default is EditorGUIUtility.singleLineHeight.</param>
    /// <param name="style">Optional GUIStyle to use for the control.</param>
    /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.
    /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
    /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
    public static Rect GetControlRect(bool hasLabel, float height, GUIStyle style, params GUILayoutOption[] options)
    {
      return GUILayoutUtility.GetRect(!hasLabel ? EditorGUIUtility.fieldWidth : EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, height, height, style, options);
    }

    internal static Rect GetSliderRect(bool hasLabel, params GUILayoutOption[] options)
    {
      return GUILayoutUtility.GetRect(!hasLabel ? EditorGUIUtility.fieldWidth : EditorGUILayout.kLabelFloatMinW, (float) ((double) EditorGUILayout.kLabelFloatMaxW + 5.0 + 100.0), 16f, 16f, EditorStyles.numberField, options);
    }

    internal static Rect GetToggleRect(bool hasLabel, params GUILayoutOption[] options)
    {
      float num = 10f - EditorGUIUtility.fieldWidth;
      return GUILayoutUtility.GetRect(!hasLabel ? EditorGUIUtility.fieldWidth + num : EditorGUILayout.kLabelFloatMinW + num, EditorGUILayout.kLabelFloatMaxW + num, 16f, 16f, EditorStyles.numberField, options);
    }

    /// <summary>
    ///   <para>Begins a group that can be be hidden/shown and the transition will be animated.</para>
    /// </summary>
    /// <param name="value">A value between 0 and 1, 0 being hidden, and 1 being fully visible.</param>
    /// <returns>
    ///   <para>If the group is visible or not.</para>
    /// </returns>
    public static bool BeginFadeGroup(float value)
    {
      if ((double) value == 0.0)
        return false;
      if ((double) value == 1.0)
        return true;
      GUILayoutFadeGroup guiLayoutFadeGroup = (GUILayoutFadeGroup) GUILayoutUtility.BeginLayoutGroup(GUIStyle.none, (GUILayoutOption[]) null, typeof (GUILayoutFadeGroup));
      guiLayoutFadeGroup.isVertical = true;
      guiLayoutFadeGroup.resetCoords = true;
      guiLayoutFadeGroup.fadeValue = value;
      guiLayoutFadeGroup.wasGUIEnabled = GUI.enabled;
      guiLayoutFadeGroup.guiColor = GUI.color;
      if ((double) value != 0.0 && (double) value != 1.0 && Event.current.type == EventType.MouseDown)
        Event.current.Use();
      EditorGUIUtility.LockContextWidth();
      GUI.BeginGroup(guiLayoutFadeGroup.rect);
      return (double) value != 0.0;
    }

    /// <summary>
    ///   <para>Closes a group started with BeginFadeGroup.</para>
    /// </summary>
    public static void EndFadeGroup()
    {
      GUILayoutFadeGroup topLevel = EditorGUILayoutUtilityInternal.topLevel as GUILayoutFadeGroup;
      if (topLevel == null)
        return;
      GUI.EndGroup();
      EditorGUIUtility.UnlockContextWidth();
      GUI.enabled = topLevel.wasGUIEnabled;
      GUI.color = topLevel.guiColor;
      GUILayoutUtility.EndGroup("GUILayout.EndVertical");
      GUILayoutUtility.EndLayoutGroup();
    }

    internal static int BeginPlatformGrouping(BuildPlayerWindow.BuildPlatform[] platforms, GUIContent defaultTab)
    {
      int num1 = -1;
      for (int index = 0; index < platforms.Length; ++index)
      {
        if (platforms[index].targetGroup == EditorUserBuildSettings.selectedBuildTargetGroup)
          num1 = index;
      }
      if (num1 == -1)
      {
        EditorGUILayout.s_SelectedDefault.value = true;
        num1 = 0;
      }
      int index1 = defaultTab != null ? (!EditorGUILayout.s_SelectedDefault.value ? num1 : -1) : num1;
      bool enabled = GUI.enabled;
      GUI.enabled = true;
      EditorGUI.BeginChangeCheck();
      Rect rect = EditorGUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
      --rect.width;
      int length = platforms.Length;
      int num2 = 18;
      GUIStyle toolbarButton = EditorStyles.toolbarButton;
      if (defaultTab != null && GUI.Toggle(new Rect(rect.x, rect.y, rect.width - (float) length * 30f, (float) num2), index1 == -1, defaultTab, toolbarButton))
        index1 = -1;
      for (int index2 = 0; index2 < length; ++index2)
      {
        Rect position;
        if (defaultTab != null)
        {
          position = new Rect(rect.xMax - (float) (length - index2) * 30f, rect.y, 30f, (float) num2);
        }
        else
        {
          int num3 = Mathf.RoundToInt((float) index2 * rect.width / (float) length);
          int num4 = Mathf.RoundToInt((float) (index2 + 1) * rect.width / (float) length);
          position = new Rect(rect.x + (float) num3, rect.y, (float) (num4 - num3), (float) num2);
        }
        if (GUI.Toggle(position, index1 == index2, new GUIContent((Texture) platforms[index2].smallIcon, platforms[index2].tooltip), toolbarButton))
          index1 = index2;
      }
      GUILayoutUtility.GetRect(10f, (float) num2);
      GUI.enabled = enabled;
      if (EditorGUI.EndChangeCheck())
      {
        if (defaultTab == null)
          EditorUserBuildSettings.selectedBuildTargetGroup = platforms[index1].targetGroup;
        else if (index1 < 0)
        {
          EditorGUILayout.s_SelectedDefault.value = true;
        }
        else
        {
          EditorUserBuildSettings.selectedBuildTargetGroup = platforms[index1].targetGroup;
          EditorGUILayout.s_SelectedDefault.value = false;
        }
        foreach (UnityEngine.Object @object in Resources.FindObjectsOfTypeAll(typeof (BuildPlayerWindow)))
        {
          BuildPlayerWindow buildPlayerWindow = @object as BuildPlayerWindow;
          if ((UnityEngine.Object) buildPlayerWindow != (UnityEngine.Object) null)
            buildPlayerWindow.Repaint();
        }
      }
      return index1;
    }

    internal static void EndPlatformGrouping()
    {
      EditorGUILayout.EndVertical();
    }

    internal static void MultiSelectionObjectTitleBar(UnityEngine.Object[] objects)
    {
      string t = objects[0].name + " (" + ObjectNames.NicifyVariableName(ObjectNames.GetTypeName(objects[0])) + ")";
      if (objects.Length > 1)
        t = t + " and " + (object) (objects.Length - 1) + " other" + (objects.Length <= 2 ? string.Empty : "s");
      GUILayoutOption[] guiLayoutOptionArray = new GUILayoutOption[1]
      {
        GUILayout.Height(16f)
      };
      GUILayout.Label(EditorGUIUtility.TempContent(t, (Texture) AssetPreview.GetMiniThumbnail(objects[0])), EditorStyles.boldLabel, guiLayoutOptionArray);
    }

    internal static bool BitToggleField(string label, SerializedProperty bitFieldProperty, int flag)
    {
      bool flag1 = (bitFieldProperty.intValue & flag) != 0;
      bool flag2 = (bitFieldProperty.hasMultipleDifferentValuesBitwise & flag) != 0;
      EditorGUI.showMixedValue = flag2;
      EditorGUI.BeginChangeCheck();
      bool flag3 = EditorGUILayout.Toggle(label, flag1, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        if (flag2)
          flag3 = true;
        flag2 = false;
        int index1 = -1;
        for (int index2 = 0; index2 < 32; ++index2)
        {
          if ((1 << index2 & flag) != 0)
          {
            index1 = index2;
            break;
          }
        }
        bitFieldProperty.SetBitAtIndexForAllTargetsImmediate(index1, flag3);
      }
      EditorGUI.showMixedValue = false;
      if (flag3)
        return !flag2;
      return false;
    }

    internal static void SortingLayerField(GUIContent label, SerializedProperty layerID, GUIStyle style)
    {
      EditorGUI.SortingLayerField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, new GUILayoutOption[0]), label, layerID, style, EditorStyles.label);
    }

    internal static string TextFieldDropDown(string text, string[] dropDownElement)
    {
      return EditorGUILayout.TextFieldDropDown(GUIContent.none, text, dropDownElement);
    }

    internal static string TextFieldDropDown(GUIContent label, string text, string[] dropDownElement)
    {
      return EditorGUI.TextFieldDropDown(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.textField), label, text, dropDownElement);
    }

    internal static string DelayedTextFieldDropDown(string text, string[] dropDownElement)
    {
      return EditorGUILayout.DelayedTextFieldDropDown(GUIContent.none, text, dropDownElement);
    }

    internal static string DelayedTextFieldDropDown(GUIContent label, string text, string[] dropDownElement)
    {
      return EditorGUI.DelayedTextFieldDropDown(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.textFieldDropDownText), label, text, dropDownElement);
    }

    internal static Color ColorBrightnessField(GUIContent label, Color value, float minBrightness, float maxBrightness, params GUILayoutOption[] options)
    {
      return EditorGUI.ColorBrightnessField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.numberField, options), label, value, minBrightness, maxBrightness);
    }

    internal static Gradient GradientField(Gradient value, params GUILayoutOption[] options)
    {
      return EditorGUI.GradientField(EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.colorField, options), value);
    }

    internal static Gradient GradientField(string label, Gradient value, params GUILayoutOption[] options)
    {
      Rect position = EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.colorField, options);
      return EditorGUI.GradientField(label, position, value);
    }

    internal static Gradient GradientField(GUIContent label, Gradient value, params GUILayoutOption[] options)
    {
      Rect position = EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.colorField, options);
      return EditorGUI.GradientField(label, position, value);
    }

    internal static Gradient GradientField(SerializedProperty value, params GUILayoutOption[] options)
    {
      return EditorGUI.GradientField(EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.colorField, options), value);
    }

    internal static Gradient GradientField(string label, SerializedProperty value, params GUILayoutOption[] options)
    {
      Rect position = EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.colorField, options);
      return EditorGUI.GradientField(label, position, value);
    }

    internal static Gradient GradientField(GUIContent label, SerializedProperty value, params GUILayoutOption[] options)
    {
      Rect position = EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.colorField, options);
      return EditorGUI.GradientField(label, position, value);
    }

    internal static Color HexColorTextField(GUIContent label, Color color, bool showAlpha, params GUILayoutOption[] options)
    {
      return EditorGUILayout.HexColorTextField(label, color, showAlpha, EditorStyles.textField, options);
    }

    internal static Color HexColorTextField(GUIContent label, Color color, bool showAlpha, GUIStyle style, params GUILayoutOption[] options)
    {
      return EditorGUI.HexColorTextField(EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.numberField, options), label, color, showAlpha, style);
    }

    internal static bool ButtonMouseDown(GUIContent content, FocusType focusType, GUIStyle style, params GUILayoutOption[] options)
    {
      EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(content, style, options);
      return EditorGUI.ButtonMouseDown(EditorGUILayout.s_LastRect, content, focusType, style);
    }

    internal static bool IconButton(int id, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
    {
      EditorGUILayout.s_LastRect = GUILayoutUtility.GetRect(content, style, options);
      return EditorGUI.IconButton(id, EditorGUILayout.s_LastRect, content, style);
    }

    internal static void GameViewSizePopup(GameViewSizeGroupType groupType, int selectedIndex, System.Action<int, object> itemClickedCallback, GUIStyle style, params GUILayoutOption[] options)
    {
      EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, options);
      EditorGUI.GameViewSizePopup(EditorGUILayout.s_LastRect, groupType, selectedIndex, itemClickedCallback, style);
    }

    internal static void SortingLayerField(GUIContent label, SerializedProperty layerID, GUIStyle style, GUIStyle labelStyle)
    {
      EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, 16f, style, new GUILayoutOption[0]);
      EditorGUI.SortingLayerField(EditorGUILayout.s_LastRect, label, layerID, style, labelStyle);
    }

    public static float Knob(Vector2 knobSize, float value, float minValue, float maxValue, string unit, Color backgroundColor, Color activeColor, bool showValue, params GUILayoutOption[] options)
    {
      Rect position = EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(false, knobSize.y, options);
      return EditorGUI.Knob(position, knobSize, value, minValue, maxValue, unit, backgroundColor, activeColor, showValue, GUIUtility.GetControlID("Knob".GetHashCode(), FocusType.Native, position));
    }

    internal static void TargetChoiceField(SerializedProperty property, GUIContent label, params GUILayoutOption[] options)
    {
      EditorGUILayout.TargetChoiceField(property, label, new TargetChoiceHandler.TargetChoiceMenuFunction(TargetChoiceHandler.SetToValueOfTarget), options);
    }

    internal static void TargetChoiceField(SerializedProperty property, GUIContent label, TargetChoiceHandler.TargetChoiceMenuFunction func, params GUILayoutOption[] options)
    {
      EditorGUI.TargetChoiceField(GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.popup, options), property, label, func);
    }

    /// <summary>
    ///   <para>Begin a vertical group with a toggle to enable or disable all the controls within at once.</para>
    /// </summary>
    public class ToggleGroupScope : GUI.Scope
    {
      /// <summary>
      ///   <para>The enabled state selected by the user.</para>
      /// </summary>
      public bool enabled { get; protected set; }

      /// <summary>
      ///   <para></para>
      /// </summary>
      /// <param name="label">Label to show above the toggled controls.</param>
      /// <param name="toggle">Enabled state of the toggle group.</param>
      public ToggleGroupScope(string label, bool toggle)
      {
        this.enabled = EditorGUILayout.BeginToggleGroup(label, toggle);
      }

      /// <summary>
      ///   <para></para>
      /// </summary>
      /// <param name="label">Label to show above the toggled controls.</param>
      /// <param name="toggle">Enabled state of the toggle group.</param>
      public ToggleGroupScope(GUIContent label, bool toggle)
      {
        this.enabled = EditorGUILayout.BeginToggleGroup(label, toggle);
      }

      protected override void CloseScope()
      {
        EditorGUILayout.EndToggleGroup();
      }
    }

    /// <summary>
    ///   <para>Disposable helper class for managing BeginHorizontal / EndHorizontal.</para>
    /// </summary>
    public class HorizontalScope : GUI.Scope
    {
      /// <summary>
      ///   <para>The rect of the horizontal group.</para>
      /// </summary>
      public Rect rect { get; protected set; }

      /// <summary>
      ///   <para>Create a new HorizontalScope and begin the corresponding horizontal group.</para>
      /// </summary>
      /// <param name="style">The style to use for background image and padding values. If left out, the background is transparent.</param>
      /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
      /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
      /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
      public HorizontalScope(params GUILayoutOption[] options)
      {
        this.rect = EditorGUILayout.BeginHorizontal(options);
      }

      /// <summary>
      ///   <para>Create a new HorizontalScope and begin the corresponding horizontal group.</para>
      /// </summary>
      /// <param name="style">The style to use for background image and padding values. If left out, the background is transparent.</param>
      /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
      /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
      /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
      public HorizontalScope(GUIStyle style, params GUILayoutOption[] options)
      {
        this.rect = EditorGUILayout.BeginHorizontal(style, options);
      }

      internal HorizontalScope(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
      {
        this.rect = EditorGUILayout.BeginHorizontal(content, style, options);
      }

      protected override void CloseScope()
      {
        EditorGUILayout.EndHorizontal();
      }
    }

    /// <summary>
    ///   <para>Disposable helper class for managing BeginVertical / EndVertical.</para>
    /// </summary>
    public class VerticalScope : GUI.Scope
    {
      /// <summary>
      ///   <para>The rect of the vertical group.</para>
      /// </summary>
      public Rect rect { get; protected set; }

      /// <summary>
      ///   <para>Create a new VerticalScope and begin the corresponding vertical group.</para>
      /// </summary>
      /// <param name="style">The style to use for background image and padding values. If left out, the background is transparent.</param>
      /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
      /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
      /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
      public VerticalScope(params GUILayoutOption[] options)
      {
        this.rect = EditorGUILayout.BeginVertical(options);
      }

      /// <summary>
      ///   <para>Create a new VerticalScope and begin the corresponding vertical group.</para>
      /// </summary>
      /// <param name="style">The style to use for background image and padding values. If left out, the background is transparent.</param>
      /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
      /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
      /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
      public VerticalScope(GUIStyle style, params GUILayoutOption[] options)
      {
        this.rect = EditorGUILayout.BeginVertical(style, options);
      }

      internal VerticalScope(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
      {
        this.rect = EditorGUILayout.BeginVertical(content, style, options);
      }

      protected override void CloseScope()
      {
        EditorGUILayout.EndVertical();
      }
    }

    /// <summary>
    ///   <para>Disposable helper class for managing BeginScrollView / EndScrollView.</para>
    /// </summary>
    public class ScrollViewScope : GUI.Scope
    {
      /// <summary>
      ///   <para>The modified scrollPosition. Feed this back into the variable you pass in, as shown in the example.</para>
      /// </summary>
      public Vector2 scrollPosition { get; protected set; }

      /// <summary>
      ///   <para>Whether this ScrollView should handle scroll wheel events. (default: true).</para>
      /// </summary>
      public bool handleScrollWheel { get; set; }

      /// <summary>
      ///   <para>Create a new ScrollViewScope and begin the corresponding ScrollView.</para>
      /// </summary>
      /// <param name="scrollPosition">The scroll position to use.</param>
      /// <param name="alwaysShowHorizontal">Whether to always show the horizontal scrollbar. If false, it is only shown when the content inside the ScrollView is wider than the scrollview itself.</param>
      /// <param name="alwaysShowVertical">Whether to always show the vertical scrollbar. If false, it is only shown when the content inside the ScrollView is higher than the scrollview itself.</param>
      /// <param name="horizontalScrollbar">Optional GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current GUISkin is used.</param>
      /// <param name="verticalScrollbar">Optional GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current GUISkin is used.</param>
      /// <param name="options"></param>
      /// <param name="style"></param>
      /// <param name="background"></param>
      public ScrollViewScope(Vector2 scrollPosition, params GUILayoutOption[] options)
      {
        this.handleScrollWheel = true;
        this.scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, options);
      }

      /// <summary>
      ///   <para>Create a new ScrollViewScope and begin the corresponding ScrollView.</para>
      /// </summary>
      /// <param name="scrollPosition">The scroll position to use.</param>
      /// <param name="alwaysShowHorizontal">Whether to always show the horizontal scrollbar. If false, it is only shown when the content inside the ScrollView is wider than the scrollview itself.</param>
      /// <param name="alwaysShowVertical">Whether to always show the vertical scrollbar. If false, it is only shown when the content inside the ScrollView is higher than the scrollview itself.</param>
      /// <param name="horizontalScrollbar">Optional GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current GUISkin is used.</param>
      /// <param name="verticalScrollbar">Optional GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current GUISkin is used.</param>
      /// <param name="options"></param>
      /// <param name="style"></param>
      /// <param name="background"></param>
      public ScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
      {
        this.handleScrollWheel = true;
        this.scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, options);
      }

      /// <summary>
      ///   <para>Create a new ScrollViewScope and begin the corresponding ScrollView.</para>
      /// </summary>
      /// <param name="scrollPosition">The scroll position to use.</param>
      /// <param name="alwaysShowHorizontal">Whether to always show the horizontal scrollbar. If false, it is only shown when the content inside the ScrollView is wider than the scrollview itself.</param>
      /// <param name="alwaysShowVertical">Whether to always show the vertical scrollbar. If false, it is only shown when the content inside the ScrollView is higher than the scrollview itself.</param>
      /// <param name="horizontalScrollbar">Optional GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current GUISkin is used.</param>
      /// <param name="verticalScrollbar">Optional GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current GUISkin is used.</param>
      /// <param name="options"></param>
      /// <param name="style"></param>
      /// <param name="background"></param>
      public ScrollViewScope(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
      {
        this.handleScrollWheel = true;
        this.scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar, options);
      }

      /// <summary>
      ///   <para>Create a new ScrollViewScope and begin the corresponding ScrollView.</para>
      /// </summary>
      /// <param name="scrollPosition">The scroll position to use.</param>
      /// <param name="alwaysShowHorizontal">Whether to always show the horizontal scrollbar. If false, it is only shown when the content inside the ScrollView is wider than the scrollview itself.</param>
      /// <param name="alwaysShowVertical">Whether to always show the vertical scrollbar. If false, it is only shown when the content inside the ScrollView is higher than the scrollview itself.</param>
      /// <param name="horizontalScrollbar">Optional GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current GUISkin is used.</param>
      /// <param name="verticalScrollbar">Optional GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current GUISkin is used.</param>
      /// <param name="options"></param>
      /// <param name="style"></param>
      /// <param name="background"></param>
      public ScrollViewScope(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
      {
        this.handleScrollWheel = true;
        this.scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, style, options);
      }

      /// <summary>
      ///   <para>Create a new ScrollViewScope and begin the corresponding ScrollView.</para>
      /// </summary>
      /// <param name="scrollPosition">The scroll position to use.</param>
      /// <param name="alwaysShowHorizontal">Whether to always show the horizontal scrollbar. If false, it is only shown when the content inside the ScrollView is wider than the scrollview itself.</param>
      /// <param name="alwaysShowVertical">Whether to always show the vertical scrollbar. If false, it is only shown when the content inside the ScrollView is higher than the scrollview itself.</param>
      /// <param name="horizontalScrollbar">Optional GUIStyle to use for the horizontal scrollbar. If left out, the horizontalScrollbar style from the current GUISkin is used.</param>
      /// <param name="verticalScrollbar">Optional GUIStyle to use for the vertical scrollbar. If left out, the verticalScrollbar style from the current GUISkin is used.</param>
      /// <param name="options"></param>
      /// <param name="style"></param>
      /// <param name="background"></param>
      public ScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options)
      {
        this.handleScrollWheel = true;
        this.scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, options);
      }

      internal ScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
      {
        this.handleScrollWheel = true;
        this.scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, options);
      }

      protected override void CloseScope()
      {
        EditorGUILayout.EndScrollView(this.handleScrollWheel);
      }
    }

    internal class VerticalScrollViewScope : GUI.Scope
    {
      public Vector2 scrollPosition { get; protected set; }

      public bool handleScrollWheel { get; set; }

      public VerticalScrollViewScope(Vector2 scrollPosition, params GUILayoutOption[] options)
      {
        this.handleScrollWheel = true;
        this.scrollPosition = EditorGUILayout.BeginVerticalScrollView(scrollPosition, options);
      }

      public VerticalScrollViewScope(Vector2 scrollPosition, bool alwaysShowVertical, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options)
      {
        this.handleScrollWheel = true;
        this.scrollPosition = EditorGUILayout.BeginVerticalScrollView(scrollPosition, alwaysShowVertical, verticalScrollbar, background, options);
      }

      protected override void CloseScope()
      {
        EditorGUILayout.EndScrollView(this.handleScrollWheel);
      }
    }

    internal class HorizontalScrollViewScope : GUI.Scope
    {
      public Vector2 scrollPosition { get; protected set; }

      public bool handleScrollWheel { get; set; }

      public HorizontalScrollViewScope(Vector2 scrollPosition, params GUILayoutOption[] options)
      {
        this.handleScrollWheel = true;
        this.scrollPosition = EditorGUILayout.BeginHorizontalScrollView(scrollPosition, options);
      }

      public HorizontalScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, GUIStyle horizontalScrollbar, GUIStyle background, params GUILayoutOption[] options)
      {
        this.handleScrollWheel = true;
        this.scrollPosition = EditorGUILayout.BeginHorizontalScrollView(scrollPosition, alwaysShowHorizontal, horizontalScrollbar, background, options);
      }

      protected override void CloseScope()
      {
        EditorGUILayout.EndScrollView(this.handleScrollWheel);
      }
    }

    /// <summary>
    ///   <para>Begins a group that can be be hidden/shown and the transition will be animated.</para>
    /// </summary>
    public class FadeGroupScope : GUI.Scope
    {
      /// <summary>
      ///   <para>Whether the group is visible.</para>
      /// </summary>
      public bool visible { get; protected set; }

      /// <summary>
      ///   <para>Create a new FadeGroupScope and begin the corresponding group.</para>
      /// </summary>
      /// <param name="value">A value between 0 and 1, 0 being hidden, and 1 being fully visible.</param>
      public FadeGroupScope(float value)
      {
        this.visible = EditorGUILayout.BeginFadeGroup(value);
      }

      protected override void CloseScope()
      {
        EditorGUILayout.EndFadeGroup();
      }
    }
  }
}

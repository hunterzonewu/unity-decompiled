// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.FontDataDrawer
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>PropertyDrawer for FontData.</para>
  /// </summary>
  [CustomPropertyDrawer(typeof (FontData), true)]
  public class FontDataDrawer : PropertyDrawer
  {
    private static int s_TextAlignmentHash = "DoTextAligmentControl".GetHashCode();
    private const int kAlignmentButtonWidth = 20;
    private SerializedProperty m_SupportEncoding;
    private SerializedProperty m_Font;
    private SerializedProperty m_FontSize;
    private SerializedProperty m_LineSpacing;
    private SerializedProperty m_FontStyle;
    private SerializedProperty m_ResizeTextForBestFit;
    private SerializedProperty m_ResizeTextMinSize;
    private SerializedProperty m_ResizeTextMaxSize;
    private SerializedProperty m_HorizontalOverflow;
    private SerializedProperty m_VerticalOverflow;
    private SerializedProperty m_Alignment;
    private SerializedProperty m_AlignByGeometry;
    private float m_FontFieldfHeight;
    private float m_FontStyleHeight;
    private float m_FontSizeHeight;
    private float m_LineSpacingHeight;
    private float m_EncodingHeight;
    private float m_ResizeTextForBestFitHeight;
    private float m_ResizeTextMinSizeHeight;
    private float m_ResizeTextMaxSizeHeight;
    private float m_HorizontalOverflowHeight;
    private float m_VerticalOverflowHeight;
    private float m_AlignByGeometryHeight;

    /// <summary>
    ///   <para>Initialize the serialized properties for the drawer.</para>
    /// </summary>
    /// <param name="property"></param>
    protected void Init(SerializedProperty property)
    {
      this.m_SupportEncoding = property.FindPropertyRelative("m_RichText");
      this.m_Font = property.FindPropertyRelative("m_Font");
      this.m_FontSize = property.FindPropertyRelative("m_FontSize");
      this.m_LineSpacing = property.FindPropertyRelative("m_LineSpacing");
      this.m_FontStyle = property.FindPropertyRelative("m_FontStyle");
      this.m_ResizeTextForBestFit = property.FindPropertyRelative("m_BestFit");
      this.m_ResizeTextMinSize = property.FindPropertyRelative("m_MinSize");
      this.m_ResizeTextMaxSize = property.FindPropertyRelative("m_MaxSize");
      this.m_HorizontalOverflow = property.FindPropertyRelative("m_HorizontalOverflow");
      this.m_VerticalOverflow = property.FindPropertyRelative("m_VerticalOverflow");
      this.m_Alignment = property.FindPropertyRelative("m_Alignment");
      this.m_AlignByGeometry = property.FindPropertyRelative("m_AlignByGeometry");
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      this.Init(property);
      this.m_FontFieldfHeight = EditorGUI.GetPropertyHeight(this.m_Font);
      this.m_FontStyleHeight = EditorGUI.GetPropertyHeight(this.m_FontStyle);
      this.m_FontSizeHeight = EditorGUI.GetPropertyHeight(this.m_FontSize);
      this.m_LineSpacingHeight = EditorGUI.GetPropertyHeight(this.m_LineSpacing);
      this.m_EncodingHeight = EditorGUI.GetPropertyHeight(this.m_SupportEncoding);
      this.m_ResizeTextForBestFitHeight = EditorGUI.GetPropertyHeight(this.m_ResizeTextForBestFit);
      this.m_ResizeTextMinSizeHeight = EditorGUI.GetPropertyHeight(this.m_ResizeTextMinSize);
      this.m_ResizeTextMaxSizeHeight = EditorGUI.GetPropertyHeight(this.m_ResizeTextMaxSize);
      this.m_HorizontalOverflowHeight = EditorGUI.GetPropertyHeight(this.m_HorizontalOverflow);
      this.m_VerticalOverflowHeight = EditorGUI.GetPropertyHeight(this.m_VerticalOverflow);
      this.m_AlignByGeometryHeight = EditorGUI.GetPropertyHeight(this.m_AlignByGeometry);
      float num = (float) ((double) this.m_FontFieldfHeight + (double) this.m_FontStyleHeight + (double) this.m_FontSizeHeight + (double) this.m_LineSpacingHeight + (double) this.m_EncodingHeight + (double) this.m_ResizeTextForBestFitHeight + (double) this.m_HorizontalOverflowHeight + (double) this.m_VerticalOverflowHeight + (double) EditorGUIUtility.singleLineHeight * 3.0 + (double) EditorGUIUtility.standardVerticalSpacing * 10.0) + this.m_AlignByGeometryHeight;
      if (this.m_ResizeTextForBestFit.boolValue)
        num += (float) ((double) this.m_ResizeTextMinSizeHeight + (double) this.m_ResizeTextMaxSizeHeight + (double) EditorGUIUtility.standardVerticalSpacing * 2.0);
      return num;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      this.Init(property);
      Rect position1 = position;
      position1.height = EditorGUIUtility.singleLineHeight;
      EditorGUI.LabelField(position1, "Character", EditorStyles.boldLabel);
      position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
      ++EditorGUI.indentLevel;
      Font objectReferenceValue = this.m_Font.objectReferenceValue as Font;
      position1.height = this.m_FontFieldfHeight;
      EditorGUI.BeginChangeCheck();
      EditorGUI.PropertyField(position1, this.m_Font);
      if (EditorGUI.EndChangeCheck())
      {
        objectReferenceValue = this.m_Font.objectReferenceValue as Font;
        if ((Object) objectReferenceValue != (Object) null && !objectReferenceValue.dynamic)
          this.m_FontSize.intValue = objectReferenceValue.fontSize;
      }
      position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
      position1.height = this.m_FontStyleHeight;
      EditorGUI.BeginDisabledGroup(!this.m_Font.hasMultipleDifferentValues && (Object) objectReferenceValue != (Object) null && !objectReferenceValue.dynamic);
      EditorGUI.PropertyField(position1, this.m_FontStyle);
      EditorGUI.EndDisabledGroup();
      position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
      position1.height = this.m_FontSizeHeight;
      EditorGUI.PropertyField(position1, this.m_FontSize);
      position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
      position1.height = this.m_LineSpacingHeight;
      EditorGUI.PropertyField(position1, this.m_LineSpacing);
      position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
      position1.height = this.m_EncodingHeight;
      EditorGUI.PropertyField(position1, this.m_SupportEncoding, FontDataDrawer.Styles.m_EncodingContent);
      --EditorGUI.indentLevel;
      position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
      position1.height = EditorGUIUtility.singleLineHeight;
      EditorGUI.LabelField(position1, "Paragraph", EditorStyles.boldLabel);
      position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
      ++EditorGUI.indentLevel;
      position1.height = EditorGUIUtility.singleLineHeight;
      this.DoTextAligmentControl(position1, this.m_Alignment);
      position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
      position1.height = this.m_HorizontalOverflowHeight;
      EditorGUI.PropertyField(position1, this.m_AlignByGeometry);
      position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
      position1.height = this.m_HorizontalOverflowHeight;
      EditorGUI.PropertyField(position1, this.m_HorizontalOverflow);
      position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
      position1.height = this.m_VerticalOverflowHeight;
      EditorGUI.PropertyField(position1, this.m_VerticalOverflow);
      position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
      position1.height = this.m_ResizeTextMaxSizeHeight;
      EditorGUI.PropertyField(position1, this.m_ResizeTextForBestFit);
      if (this.m_ResizeTextForBestFit.boolValue)
      {
        EditorGUILayout.EndFadeGroup();
        position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
        position1.height = this.m_ResizeTextMinSizeHeight;
        EditorGUI.PropertyField(position1, this.m_ResizeTextMinSize);
        position1.y += position1.height + EditorGUIUtility.standardVerticalSpacing;
        position1.height = this.m_ResizeTextMaxSizeHeight;
        EditorGUI.PropertyField(position1, this.m_ResizeTextMaxSize);
      }
      --EditorGUI.indentLevel;
    }

    private void DoTextAligmentControl(Rect position, SerializedProperty alignment)
    {
      GUIContent label = new GUIContent("Alignment");
      int controlId = GUIUtility.GetControlID(FontDataDrawer.s_TextAlignmentHash, EditorGUIUtility.native, position);
      EditorGUIUtility.SetIconSize(new Vector2(15f, 15f));
      EditorGUI.BeginProperty(position, label, alignment);
      Rect rect = EditorGUI.PrefixLabel(position, controlId, label);
      float width = 60f;
      float num = Mathf.Clamp(rect.width - width * 2f, 2f, 10f);
      Rect position1 = new Rect(rect.x, rect.y, width, rect.height);
      Rect position2 = new Rect(position1.xMax + num, rect.y, width, rect.height);
      FontDataDrawer.DoHorizontalAligmentControl(position1, alignment);
      FontDataDrawer.DoVerticalAligmentControl(position2, alignment);
      EditorGUI.EndProperty();
      EditorGUIUtility.SetIconSize(Vector2.zero);
    }

    private static void DoHorizontalAligmentControl(Rect position, SerializedProperty alignment)
    {
      FontDataDrawer.HorizontalTextAligment horizontalAlignment1 = FontDataDrawer.GetHorizontalAlignment((TextAnchor) alignment.intValue);
      bool flag1 = horizontalAlignment1 == FontDataDrawer.HorizontalTextAligment.Left;
      bool flag2 = horizontalAlignment1 == FontDataDrawer.HorizontalTextAligment.Center;
      bool flag3 = horizontalAlignment1 == FontDataDrawer.HorizontalTextAligment.Right;
      if (alignment.hasMultipleDifferentValues)
      {
        foreach (Object targetObject in alignment.serializedObject.targetObjects)
        {
          FontDataDrawer.HorizontalTextAligment horizontalAlignment2 = FontDataDrawer.GetHorizontalAlignment((targetObject as Text).alignment);
          flag1 = flag1 || horizontalAlignment2 == FontDataDrawer.HorizontalTextAligment.Left;
          flag2 = flag2 || horizontalAlignment2 == FontDataDrawer.HorizontalTextAligment.Center;
          flag3 = flag3 || horizontalAlignment2 == FontDataDrawer.HorizontalTextAligment.Right;
        }
      }
      position.width = 20f;
      EditorGUI.BeginChangeCheck();
      FontDataDrawer.EditorToggle(position, flag1, !flag1 ? FontDataDrawer.Styles.m_LeftAlignText : FontDataDrawer.Styles.m_LeftAlignTextActive, FontDataDrawer.Styles.alignmentButtonLeft);
      if (EditorGUI.EndChangeCheck())
        FontDataDrawer.SetHorizontalAlignment(alignment, FontDataDrawer.HorizontalTextAligment.Left);
      position.x += position.width;
      EditorGUI.BeginChangeCheck();
      FontDataDrawer.EditorToggle(position, flag2, !flag2 ? FontDataDrawer.Styles.m_CenterAlignText : FontDataDrawer.Styles.m_CenterAlignTextActive, FontDataDrawer.Styles.alignmentButtonMid);
      if (EditorGUI.EndChangeCheck())
        FontDataDrawer.SetHorizontalAlignment(alignment, FontDataDrawer.HorizontalTextAligment.Center);
      position.x += position.width;
      EditorGUI.BeginChangeCheck();
      FontDataDrawer.EditorToggle(position, flag3, !flag3 ? FontDataDrawer.Styles.m_RightAlignText : FontDataDrawer.Styles.m_RightAlignTextActive, FontDataDrawer.Styles.alignmentButtonRight);
      if (!EditorGUI.EndChangeCheck())
        return;
      FontDataDrawer.SetHorizontalAlignment(alignment, FontDataDrawer.HorizontalTextAligment.Right);
    }

    private static void DoVerticalAligmentControl(Rect position, SerializedProperty alignment)
    {
      FontDataDrawer.VerticalTextAligment verticalAlignment1 = FontDataDrawer.GetVerticalAlignment((TextAnchor) alignment.intValue);
      bool flag1 = verticalAlignment1 == FontDataDrawer.VerticalTextAligment.Top;
      bool flag2 = verticalAlignment1 == FontDataDrawer.VerticalTextAligment.Middle;
      bool flag3 = verticalAlignment1 == FontDataDrawer.VerticalTextAligment.Bottom;
      if (alignment.hasMultipleDifferentValues)
      {
        foreach (Object targetObject in alignment.serializedObject.targetObjects)
        {
          FontDataDrawer.VerticalTextAligment verticalAlignment2 = FontDataDrawer.GetVerticalAlignment((targetObject as Text).alignment);
          flag1 = flag1 || verticalAlignment2 == FontDataDrawer.VerticalTextAligment.Top;
          flag2 = flag2 || verticalAlignment2 == FontDataDrawer.VerticalTextAligment.Middle;
          flag3 = flag3 || verticalAlignment2 == FontDataDrawer.VerticalTextAligment.Bottom;
        }
      }
      position.width = 20f;
      EditorGUI.BeginChangeCheck();
      FontDataDrawer.EditorToggle(position, flag1, !flag1 ? FontDataDrawer.Styles.m_TopAlignText : FontDataDrawer.Styles.m_TopAlignTextActive, FontDataDrawer.Styles.alignmentButtonLeft);
      if (EditorGUI.EndChangeCheck())
        FontDataDrawer.SetVerticalAlignment(alignment, FontDataDrawer.VerticalTextAligment.Top);
      position.x += position.width;
      EditorGUI.BeginChangeCheck();
      FontDataDrawer.EditorToggle(position, flag2, !flag2 ? FontDataDrawer.Styles.m_MiddleAlignText : FontDataDrawer.Styles.m_MiddleAlignTextActive, FontDataDrawer.Styles.alignmentButtonMid);
      if (EditorGUI.EndChangeCheck())
        FontDataDrawer.SetVerticalAlignment(alignment, FontDataDrawer.VerticalTextAligment.Middle);
      position.x += position.width;
      EditorGUI.BeginChangeCheck();
      FontDataDrawer.EditorToggle(position, flag3, !flag3 ? FontDataDrawer.Styles.m_BottomAlignText : FontDataDrawer.Styles.m_BottomAlignTextActive, FontDataDrawer.Styles.alignmentButtonRight);
      if (!EditorGUI.EndChangeCheck())
        return;
      FontDataDrawer.SetVerticalAlignment(alignment, FontDataDrawer.VerticalTextAligment.Bottom);
    }

    private static bool EditorToggle(Rect position, bool value, GUIContent content, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID("AlignToggle".GetHashCode(), EditorGUIUtility.native, position);
      Event current = Event.current;
      if (GUIUtility.keyboardControl == controlId && current.type == EventType.KeyDown && (current.keyCode == KeyCode.Space || current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter))
      {
        value = !value;
        current.Use();
        GUI.changed = true;
      }
      if (current.type == EventType.KeyDown && Event.current.button == 0 && position.Contains(Event.current.mousePosition))
      {
        GUIUtility.keyboardControl = controlId;
        EditorGUIUtility.editingTextField = false;
        HandleUtility.Repaint();
      }
      return GUI.Toggle(position, controlId, value, content, style);
    }

    private static FontDataDrawer.HorizontalTextAligment GetHorizontalAlignment(TextAnchor ta)
    {
      switch (ta)
      {
        case TextAnchor.UpperLeft:
        case TextAnchor.MiddleLeft:
        case TextAnchor.LowerLeft:
          return FontDataDrawer.HorizontalTextAligment.Left;
        case TextAnchor.UpperCenter:
        case TextAnchor.MiddleCenter:
        case TextAnchor.LowerCenter:
          return FontDataDrawer.HorizontalTextAligment.Center;
        case TextAnchor.UpperRight:
        case TextAnchor.MiddleRight:
        case TextAnchor.LowerRight:
          return FontDataDrawer.HorizontalTextAligment.Right;
        default:
          return FontDataDrawer.HorizontalTextAligment.Left;
      }
    }

    private static FontDataDrawer.VerticalTextAligment GetVerticalAlignment(TextAnchor ta)
    {
      switch (ta)
      {
        case TextAnchor.UpperLeft:
        case TextAnchor.UpperCenter:
        case TextAnchor.UpperRight:
          return FontDataDrawer.VerticalTextAligment.Top;
        case TextAnchor.MiddleLeft:
        case TextAnchor.MiddleCenter:
        case TextAnchor.MiddleRight:
          return FontDataDrawer.VerticalTextAligment.Middle;
        case TextAnchor.LowerLeft:
        case TextAnchor.LowerCenter:
        case TextAnchor.LowerRight:
          return FontDataDrawer.VerticalTextAligment.Bottom;
        default:
          return FontDataDrawer.VerticalTextAligment.Top;
      }
    }

    private static void SetHorizontalAlignment(SerializedProperty alignment, FontDataDrawer.HorizontalTextAligment horizontalAlignment)
    {
      foreach (Object targetObject in alignment.serializedObject.targetObjects)
      {
        Text text = targetObject as Text;
        FontDataDrawer.VerticalTextAligment verticalAlignment = FontDataDrawer.GetVerticalAlignment(text.alignment);
        Undo.RecordObject((Object) text, "Horizontal Alignment");
        text.alignment = FontDataDrawer.GetAnchor(verticalAlignment, horizontalAlignment);
        EditorUtility.SetDirty(targetObject);
      }
    }

    private static void SetVerticalAlignment(SerializedProperty alignment, FontDataDrawer.VerticalTextAligment verticalAlignment)
    {
      foreach (Object targetObject in alignment.serializedObject.targetObjects)
      {
        Text text = targetObject as Text;
        FontDataDrawer.HorizontalTextAligment horizontalAlignment = FontDataDrawer.GetHorizontalAlignment(text.alignment);
        Undo.RecordObject((Object) text, "Vertical Alignment");
        text.alignment = FontDataDrawer.GetAnchor(verticalAlignment, horizontalAlignment);
        EditorUtility.SetDirty(targetObject);
      }
    }

    private static TextAnchor GetAnchor(FontDataDrawer.VerticalTextAligment verticalTextAligment, FontDataDrawer.HorizontalTextAligment horizontalTextAligment)
    {
      switch (horizontalTextAligment)
      {
        case FontDataDrawer.HorizontalTextAligment.Left:
          switch (verticalTextAligment)
          {
            case FontDataDrawer.VerticalTextAligment.Middle:
              return TextAnchor.MiddleLeft;
            case FontDataDrawer.VerticalTextAligment.Bottom:
              return TextAnchor.LowerLeft;
            default:
              return TextAnchor.UpperLeft;
          }
        case FontDataDrawer.HorizontalTextAligment.Center:
          switch (verticalTextAligment)
          {
            case FontDataDrawer.VerticalTextAligment.Middle:
              return TextAnchor.MiddleCenter;
            case FontDataDrawer.VerticalTextAligment.Bottom:
              return TextAnchor.LowerCenter;
            default:
              return TextAnchor.UpperCenter;
          }
        default:
          switch (verticalTextAligment)
          {
            case FontDataDrawer.VerticalTextAligment.Middle:
              return TextAnchor.MiddleRight;
            case FontDataDrawer.VerticalTextAligment.Bottom:
              return TextAnchor.LowerRight;
            default:
              return TextAnchor.UpperRight;
          }
      }
    }

    private static class Styles
    {
      public static GUIStyle alignmentButtonLeft = new GUIStyle(EditorStyles.miniButtonLeft);
      public static GUIStyle alignmentButtonMid = new GUIStyle(EditorStyles.miniButtonMid);
      public static GUIStyle alignmentButtonRight = new GUIStyle(EditorStyles.miniButtonRight);
      public static GUIContent m_EncodingContent = new GUIContent("Rich Text", "Use emoticons and colors");
      public static GUIContent m_LeftAlignText = EditorGUIUtility.IconContent("GUISystem/align_horizontally_left", "Left Align");
      public static GUIContent m_CenterAlignText = EditorGUIUtility.IconContent("GUISystem/align_horizontally_center", "Center Align");
      public static GUIContent m_RightAlignText = EditorGUIUtility.IconContent("GUISystem/align_horizontally_right", "Right Align");
      public static GUIContent m_LeftAlignTextActive = EditorGUIUtility.IconContent("GUISystem/align_horizontally_left_active", "Left Align");
      public static GUIContent m_CenterAlignTextActive = EditorGUIUtility.IconContent("GUISystem/align_horizontally_center_active", "Center Align");
      public static GUIContent m_RightAlignTextActive = EditorGUIUtility.IconContent("GUISystem/align_horizontally_right_active", "Right Align");
      public static GUIContent m_TopAlignText = EditorGUIUtility.IconContent("GUISystem/align_vertically_top", "Top Align");
      public static GUIContent m_MiddleAlignText = EditorGUIUtility.IconContent("GUISystem/align_vertically_center", "Middle Align");
      public static GUIContent m_BottomAlignText = EditorGUIUtility.IconContent("GUISystem/align_vertically_bottom", "Bottom Align");
      public static GUIContent m_TopAlignTextActive = EditorGUIUtility.IconContent("GUISystem/align_vertically_top_active", "Top Align");
      public static GUIContent m_MiddleAlignTextActive = EditorGUIUtility.IconContent("GUISystem/align_vertically_center_active", "Middle Align");
      public static GUIContent m_BottomAlignTextActive = EditorGUIUtility.IconContent("GUISystem/align_vertically_bottom_active", "Bottom Align");

      static Styles()
      {
        FontDataDrawer.Styles.FixAlignmentButtonStyles(FontDataDrawer.Styles.alignmentButtonLeft, FontDataDrawer.Styles.alignmentButtonMid, FontDataDrawer.Styles.alignmentButtonRight);
      }

      private static void FixAlignmentButtonStyles(params GUIStyle[] styles)
      {
        foreach (GUIStyle style in styles)
        {
          style.padding.left = 2;
          style.padding.right = 2;
        }
      }
    }

    private enum VerticalTextAligment
    {
      Top,
      Middle,
      Bottom,
    }

    private enum HorizontalTextAligment
    {
      Left,
      Center,
      Right,
    }
  }
}

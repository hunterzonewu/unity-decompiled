// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.ImageEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the Image Component.</para>
  /// </summary>
  [CustomEditor(typeof (Image), true)]
  [CanEditMultipleObjects]
  public class ImageEditor : GraphicEditor
  {
    private SerializedProperty m_FillMethod;
    private SerializedProperty m_FillOrigin;
    private SerializedProperty m_FillAmount;
    private SerializedProperty m_FillClockwise;
    private SerializedProperty m_Type;
    private SerializedProperty m_FillCenter;
    private SerializedProperty m_Sprite;
    private SerializedProperty m_PreserveAspect;
    private GUIContent m_SpriteContent;
    private GUIContent m_SpriteTypeContent;
    private GUIContent m_ClockwiseContent;
    private AnimBool m_ShowSlicedOrTiled;
    private AnimBool m_ShowSliced;
    private AnimBool m_ShowFilled;
    private AnimBool m_ShowType;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_SpriteContent = new GUIContent("Source Image");
      this.m_SpriteTypeContent = new GUIContent("Image Type");
      this.m_ClockwiseContent = new GUIContent("Clockwise");
      this.m_Sprite = this.serializedObject.FindProperty("m_Sprite");
      this.m_Type = this.serializedObject.FindProperty("m_Type");
      this.m_FillCenter = this.serializedObject.FindProperty("m_FillCenter");
      this.m_FillMethod = this.serializedObject.FindProperty("m_FillMethod");
      this.m_FillOrigin = this.serializedObject.FindProperty("m_FillOrigin");
      this.m_FillClockwise = this.serializedObject.FindProperty("m_FillClockwise");
      this.m_FillAmount = this.serializedObject.FindProperty("m_FillAmount");
      this.m_PreserveAspect = this.serializedObject.FindProperty("m_PreserveAspect");
      this.m_ShowType = new AnimBool(this.m_Sprite.objectReferenceValue != (UnityEngine.Object) null);
      this.m_ShowType.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      Image.Type enumValueIndex = (Image.Type) this.m_Type.enumValueIndex;
      this.m_ShowSlicedOrTiled = new AnimBool(!this.m_Type.hasMultipleDifferentValues && enumValueIndex == Image.Type.Sliced);
      this.m_ShowSliced = new AnimBool(!this.m_Type.hasMultipleDifferentValues && enumValueIndex == Image.Type.Sliced);
      this.m_ShowFilled = new AnimBool(!this.m_Type.hasMultipleDifferentValues && enumValueIndex == Image.Type.Filled);
      this.m_ShowSlicedOrTiled.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowSliced.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowFilled.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.SetShowNativeSize(true);
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected override void OnDisable()
    {
      this.m_ShowType.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowSlicedOrTiled.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowSliced.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowFilled.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    /// <summary>
    ///   <para>Implement specific ImageEditor inspector GUI code here. If you want to simply extend the existing editor call the base OnInspectorGUI () before doing any custom GUI code.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.SpriteGUI();
      this.AppearanceControlsGUI();
      this.RaycastControlsGUI();
      this.m_ShowType.target = this.m_Sprite.objectReferenceValue != (UnityEngine.Object) null;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowType.faded))
        this.TypeGUI();
      EditorGUILayout.EndFadeGroup();
      this.SetShowNativeSize(false);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowNativeSize.faded))
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(this.m_PreserveAspect);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
      this.NativeSizeButtonGUI();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void SetShowNativeSize(bool instant)
    {
      Image.Type enumValueIndex = (Image.Type) this.m_Type.enumValueIndex;
      this.SetShowNativeSize(enumValueIndex == Image.Type.Simple || enumValueIndex == Image.Type.Filled, instant);
    }

    /// <summary>
    ///   <para>GUI for showing the Sprite property.</para>
    /// </summary>
    protected void SpriteGUI()
    {
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_Sprite, this.m_SpriteContent, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      Sprite objectReferenceValue = this.m_Sprite.objectReferenceValue as Sprite;
      if (!(bool) ((UnityEngine.Object) objectReferenceValue))
        return;
      Image.Type enumValueIndex = (Image.Type) this.m_Type.enumValueIndex;
      if ((double) objectReferenceValue.border.SqrMagnitude() > 0.0)
      {
        this.m_Type.enumValueIndex = 1;
      }
      else
      {
        if (enumValueIndex != Image.Type.Sliced)
          return;
        this.m_Type.enumValueIndex = 0;
      }
    }

    /// <summary>
    ///   <para>GUI for showing the image type and associated settings.</para>
    /// </summary>
    protected void TypeGUI()
    {
      EditorGUILayout.PropertyField(this.m_Type, this.m_SpriteTypeContent, new GUILayoutOption[0]);
      ++EditorGUI.indentLevel;
      Image.Type enumValueIndex = (Image.Type) this.m_Type.enumValueIndex;
      bool flag = !this.m_Type.hasMultipleDifferentValues && (enumValueIndex == Image.Type.Sliced || enumValueIndex == Image.Type.Tiled);
      if (flag && this.targets.Length > 1)
        flag = ((IEnumerable<UnityEngine.Object>) this.targets).Select<UnityEngine.Object, Image>((Func<UnityEngine.Object, Image>) (obj => obj as Image)).All<Image>((Func<Image, bool>) (img => img.hasBorder));
      this.m_ShowSlicedOrTiled.target = flag;
      this.m_ShowSliced.target = flag && !this.m_Type.hasMultipleDifferentValues && enumValueIndex == Image.Type.Sliced;
      this.m_ShowFilled.target = !this.m_Type.hasMultipleDifferentValues && enumValueIndex == Image.Type.Filled;
      Image target = this.target as Image;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowSlicedOrTiled.faded) && target.hasBorder)
        EditorGUILayout.PropertyField(this.m_FillCenter);
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowSliced.faded) && (UnityEngine.Object) target.sprite != (UnityEngine.Object) null && !target.hasBorder)
        EditorGUILayout.HelpBox("This Image doesn't have a border.", MessageType.Warning);
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowFilled.faded))
      {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(this.m_FillMethod);
        if (EditorGUI.EndChangeCheck())
          this.m_FillOrigin.intValue = 0;
        switch (this.m_FillMethod.enumValueIndex)
        {
          case 0:
            this.m_FillOrigin.intValue = (int) EditorGUILayout.EnumPopup("Fill Origin", (Enum) (Image.OriginHorizontal) this.m_FillOrigin.intValue, new GUILayoutOption[0]);
            break;
          case 1:
            this.m_FillOrigin.intValue = (int) EditorGUILayout.EnumPopup("Fill Origin", (Enum) (Image.OriginVertical) this.m_FillOrigin.intValue, new GUILayoutOption[0]);
            break;
          case 2:
            this.m_FillOrigin.intValue = (int) EditorGUILayout.EnumPopup("Fill Origin", (Enum) (Image.Origin90) this.m_FillOrigin.intValue, new GUILayoutOption[0]);
            break;
          case 3:
            this.m_FillOrigin.intValue = (int) EditorGUILayout.EnumPopup("Fill Origin", (Enum) (Image.Origin180) this.m_FillOrigin.intValue, new GUILayoutOption[0]);
            break;
          case 4:
            this.m_FillOrigin.intValue = (int) EditorGUILayout.EnumPopup("Fill Origin", (Enum) (Image.Origin360) this.m_FillOrigin.intValue, new GUILayoutOption[0]);
            break;
        }
        EditorGUILayout.PropertyField(this.m_FillAmount);
        if (this.m_FillMethod.enumValueIndex > 1)
          EditorGUILayout.PropertyField(this.m_FillClockwise, this.m_ClockwiseContent, new GUILayoutOption[0]);
      }
      EditorGUILayout.EndFadeGroup();
      --EditorGUI.indentLevel;
    }

    /// <summary>
    ///   <para>Can this component be Previewed in its current state?</para>
    /// </summary>
    /// <returns>
    ///   <para>True if this component can be Previewed in its current state.</para>
    /// </returns>
    public override bool HasPreviewGUI()
    {
      return true;
    }

    /// <summary>
    ///   <para>Custom preview for Image component.</para>
    /// </summary>
    /// <param name="rect">Rectangle in which to draw the preview.</param>
    /// <param name="background">Background image.</param>
    public override void OnPreviewGUI(Rect rect, GUIStyle background)
    {
      Image target = this.target as Image;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      Sprite sprite = target.sprite;
      if ((UnityEngine.Object) sprite == (UnityEngine.Object) null)
        return;
      SpriteDrawUtility.DrawSprite(sprite, rect, target.canvasRenderer.GetColor());
    }

    /// <summary>
    ///   <para>A string cointaining the Image details to be used as a overlay on the component Preview.</para>
    /// </summary>
    /// <returns>
    ///   <para>The Image details.</para>
    /// </returns>
    public override string GetInfoString()
    {
      Sprite sprite = (this.target as Image).sprite;
      return string.Format("Image Size: {0}x{1}", (object) (!((UnityEngine.Object) sprite != (UnityEngine.Object) null) ? 0 : Mathf.RoundToInt(sprite.rect.width)), (object) (!((UnityEngine.Object) sprite != (UnityEngine.Object) null) ? 0 : Mathf.RoundToInt(sprite.rect.height)));
    }
  }
}

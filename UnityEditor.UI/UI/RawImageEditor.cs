// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.RawImageEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom editor for RawImage.</para>
  /// </summary>
  [CustomEditor(typeof (RawImage), true)]
  [CanEditMultipleObjects]
  public class RawImageEditor : GraphicEditor
  {
    private SerializedProperty m_Texture;
    private SerializedProperty m_UVRect;
    private GUIContent m_UVRectContent;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_UVRectContent = new GUIContent("UV Rect");
      this.m_Texture = this.serializedObject.FindProperty("m_Texture");
      this.m_UVRect = this.serializedObject.FindProperty("m_UVRect");
      this.SetShowNativeSize(true);
    }

    /// <summary>
    ///   <para>Implement specific RawImage inspector GUI code here. If you want to simply extend the existing editor call the base OnInspectorGUI () before doing any custom GUI code.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Texture);
      this.AppearanceControlsGUI();
      this.RaycastControlsGUI();
      EditorGUILayout.PropertyField(this.m_UVRect, this.m_UVRectContent, new GUILayoutOption[0]);
      this.SetShowNativeSize(false);
      this.NativeSizeButtonGUI();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void SetShowNativeSize(bool instant)
    {
      this.SetShowNativeSize(this.m_Texture.objectReferenceValue != (Object) null, instant);
    }

    private static Rect Outer(RawImage rawImage)
    {
      Rect uvRect = rawImage.uvRect;
      uvRect.xMin *= rawImage.rectTransform.rect.width;
      uvRect.xMax *= rawImage.rectTransform.rect.width;
      uvRect.yMin *= rawImage.rectTransform.rect.height;
      uvRect.yMax *= rawImage.rectTransform.rect.height;
      return uvRect;
    }

    /// <summary>
    ///   <para>Can this component be Previewed in its current state?</para>
    /// </summary>
    /// <returns>
    ///   <para>True if this component can be Previewed in its current state.</para>
    /// </returns>
    public override bool HasPreviewGUI()
    {
      RawImage target = this.target as RawImage;
      if ((Object) target == (Object) null)
        return false;
      Rect rect = RawImageEditor.Outer(target);
      if ((double) rect.width > 0.0)
        return (double) rect.height > 0.0;
      return false;
    }

    /// <summary>
    ///   <para>Custom preview for Image component.</para>
    /// </summary>
    /// <param name="rect">Rectangle in which to draw the preview.</param>
    /// <param name="background">Background image.</param>
    public override void OnPreviewGUI(Rect rect, GUIStyle background)
    {
      RawImage target = this.target as RawImage;
      Texture mainTexture = target.mainTexture;
      if ((Object) mainTexture == (Object) null)
        return;
      Rect outer = RawImageEditor.Outer(target);
      SpriteDrawUtility.DrawSprite(mainTexture, rect, outer, target.uvRect, target.canvasRenderer.GetColor());
    }

    /// <summary>
    ///   <para>A string cointaining the Image details to be used as a overlay on the component Preview.</para>
    /// </summary>
    /// <returns>
    ///   <para>The RawImage details.</para>
    /// </returns>
    public override string GetInfoString()
    {
      RawImage target = this.target as RawImage;
      return string.Format("RawImage Size: {0}x{1}", (object) Mathf.RoundToInt(Mathf.Abs(target.rectTransform.rect.width)), (object) Mathf.RoundToInt(Mathf.Abs(target.rectTransform.rect.height)));
    }
  }
}

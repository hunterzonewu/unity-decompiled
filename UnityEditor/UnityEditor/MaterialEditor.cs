// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Utils;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>The Unity Material Editor.</para>
  /// </summary>
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Material))]
  public class MaterialEditor : Editor
  {
    private static int s_ControlHash = "EditorTextField".GetHashCode();
    private static string s_PropBlockWarning = EditorGUIUtility.TextContent("MaterialPropertyBlock is used to modify these values").text;
    private static readonly GUIContent s_TilingText = new GUIContent("Tiling");
    private static readonly GUIContent s_OffsetText = new GUIContent("Offset");
    private static readonly Mesh[] s_Meshes = new Mesh[5];
    private static readonly GUIContent[] s_MeshIcons = new GUIContent[5];
    private static readonly GUIContent[] s_LightIcons = new GUIContent[2];
    private static readonly GUIContent[] s_TimeIcons = new GUIContent[2];
    private Vector2 m_PreviewDir = new Vector2(0.0f, -20f);
    private int m_LightMode = 1;
    private MaterialEditor.ReflectionProbePicker m_ReflectionProbePicker = new MaterialEditor.ReflectionProbePicker();
    private const float kSpacingUnderTexture = 6f;
    private const float kWarningMessageHeight = 33f;
    private const float kMiniWarningMessageHeight = 27f;
    /// <summary>
    ///   <para>Useful for indenting shader properties that need the same indent as mini texture field.</para>
    /// </summary>
    public const int kMiniTextureFieldLabelIndentLevel = 2;
    private const float kSpaceBetweenFlexibleAreaAndField = 5f;
    private bool m_IsVisible;
    private bool m_CheckSetup;
    private Shader m_Shader;
    private string m_InfoMessage;
    private int m_SelectedMesh;
    private int m_TimeUpdate;
    private ShaderGUI m_CustomShaderGUI;
    [NonSerialized]
    private bool m_TriedCreatingCustomGUI;
    private bool m_InsidePropertiesGUI;
    private Renderer m_RendererForAnimationMode;
    private Color m_PreviousGUIColor;
    private MaterialProperty.TexDim m_DesiredTexdim;
    private PreviewRenderUtility m_PreviewUtility;
    private static Mesh s_PlaneMesh;

    internal bool forceVisible { get; set; }

    /// <summary>
    ///   <para>Is the current material expanded.</para>
    /// </summary>
    public bool isVisible
    {
      get
      {
        if (!this.forceVisible)
          return this.m_IsVisible;
        return true;
      }
    }

    private static MaterialEditor.PreviewType GetPreviewType(Material mat)
    {
      if ((UnityEngine.Object) mat == (UnityEngine.Object) null)
        return MaterialEditor.PreviewType.Mesh;
      string lower = mat.GetTag("PreviewType", false, string.Empty).ToLower();
      if (lower == "plane")
        return MaterialEditor.PreviewType.Plane;
      return lower == "skybox" || (UnityEngine.Object) mat.shader != (UnityEngine.Object) null && mat.shader.name.Contains("Skybox") ? MaterialEditor.PreviewType.Skybox : MaterialEditor.PreviewType.Mesh;
    }

    private static bool DoesPreviewAllowRotation(MaterialEditor.PreviewType type)
    {
      return type != MaterialEditor.PreviewType.Plane;
    }

    /// <summary>
    ///   <para>Set the shader of the material.</para>
    /// </summary>
    /// <param name="shader">Shader to set.</param>
    /// <param name="registerUndo">Should undo be registered.</param>
    /// <param name="newShader"></param>
    public void SetShader(Shader shader)
    {
      this.SetShader(shader, true);
    }

    /// <summary>
    ///   <para>Set the shader of the material.</para>
    /// </summary>
    /// <param name="shader">Shader to set.</param>
    /// <param name="registerUndo">Should undo be registered.</param>
    /// <param name="newShader"></param>
    public void SetShader(Shader newShader, bool registerUndo)
    {
      bool flag = false;
      ShaderGUI customShaderGui = this.m_CustomShaderGUI;
      string oldEditorName = !((UnityEngine.Object) this.m_Shader != (UnityEngine.Object) null) ? string.Empty : this.m_Shader.customEditor;
      this.CreateCustomShaderGUI(newShader, oldEditorName);
      this.m_Shader = newShader;
      if (customShaderGui != this.m_CustomShaderGUI)
        flag = true;
      foreach (Material target in this.targets)
      {
        Shader shader = target.shader;
        Undo.RecordObject((UnityEngine.Object) target, "Assign shader");
        if (this.m_CustomShaderGUI != null)
          this.m_CustomShaderGUI.AssignNewShaderToMaterial(target, shader, newShader);
        else
          target.shader = newShader;
        EditorMaterialUtility.ResetDefaultTextures(target, false);
        MaterialEditor.ApplyMaterialPropertyDrawers(target);
      }
      if (!flag || ActiveEditorTracker.sharedTracker == null)
        return;
      foreach (InspectorWindow allInspectorWindow in InspectorWindow.GetAllInspectorWindows())
        allInspectorWindow.GetTracker().ForceRebuild();
    }

    internal void OnSelectedShaderPopup(string command, Shader shader)
    {
      this.serializedObject.Update();
      if ((UnityEngine.Object) shader != (UnityEngine.Object) null)
        this.SetShader(shader);
      this.PropertiesChanged();
    }

    private bool HasMultipleMixedShaderValues()
    {
      bool flag = false;
      Shader shader = (this.targets[0] as Material).shader;
      for (int index = 1; index < this.targets.Length; ++index)
      {
        if ((UnityEngine.Object) shader != (UnityEngine.Object) (this.targets[index] as Material).shader)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private void ShaderPopup(GUIStyle style)
    {
      bool enabled = GUI.enabled;
      Rect position = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), 47385, EditorGUIUtility.TempContent("Shader"));
      EditorGUI.showMixedValue = this.HasMultipleMixedShaderValues();
      GUIContent content = EditorGUIUtility.TempContent(!((UnityEngine.Object) this.m_Shader != (UnityEngine.Object) null) ? "No Shader Selected" : this.m_Shader.name);
      if (EditorGUI.ButtonMouseDown(position, content, EditorGUIUtility.native, style))
      {
        EditorGUI.showMixedValue = false;
        Vector2 screenPoint = GUIUtility.GUIToScreenPoint(new Vector2(position.x, position.y));
        InternalEditorUtility.SetupShaderMenu(this.target as Material);
        EditorUtility.Internal_DisplayPopupMenu(new Rect(screenPoint.x, screenPoint.y, position.width, position.height), "CONTEXT/ShaderPopup", (UnityEngine.Object) this, 0);
        Event.current.Use();
      }
      EditorGUI.showMixedValue = false;
      GUI.enabled = enabled;
    }

    /// <summary>
    ///   <para>Called when the Editor is woken up.</para>
    /// </summary>
    public virtual void Awake()
    {
      this.m_IsVisible = InternalEditorUtility.GetIsInspectorExpanded(this.target);
      if (MaterialEditor.GetPreviewType(this.target as Material) != MaterialEditor.PreviewType.Skybox)
        return;
      this.m_PreviewDir = new Vector2(0.0f, 50f);
    }

    private void DetectShaderChanged()
    {
      Material target = this.target as Material;
      if (!((UnityEngine.Object) target.shader != (UnityEngine.Object) this.m_Shader))
        return;
      string oldEditorName = !((UnityEngine.Object) this.m_Shader != (UnityEngine.Object) null) ? string.Empty : this.m_Shader.customEditor;
      this.CreateCustomShaderGUI(target.shader, oldEditorName);
      this.m_Shader = target.shader;
      InspectorWindow.RepaintAllInspectors();
    }

    /// <summary>
    ///   <para>Implement specific MaterialEditor GUI code here. If you want to simply extend the existing editor call the base OnInspectorGUI () before doing any custom GUI code.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.CheckSetup();
      this.DetectShaderChanged();
      if (!this.isVisible || !((UnityEngine.Object) this.m_Shader != (UnityEngine.Object) null) || (this.HasMultipleMixedShaderValues() || !this.PropertiesGUI()))
        return;
      this.PropertiesChanged();
    }

    private void CheckSetup()
    {
      if (!this.m_CheckSetup || (UnityEngine.Object) this.m_Shader == (UnityEngine.Object) null)
        return;
      this.m_CheckSetup = false;
      if (this.m_CustomShaderGUI != null || this.IsMaterialEditor(this.m_Shader.customEditor))
        return;
      Debug.LogWarningFormat("Could not create a custom UI for the shader '{0}'. The shader has the following: 'CustomEditor = {1}'. Does the custom editor specified include its namespace? And does the class either derive from ShaderGUI or MaterialEditor?", new object[2]
      {
        (object) this.m_Shader.name,
        (object) this.m_Shader.customEditor
      });
    }

    internal override void OnAssetStoreInspectorGUI()
    {
      this.OnInspectorGUI();
    }

    /// <summary>
    ///   <para>Whenever a material property is changed call this function. This will rebuild the inspector and validate the properties.</para>
    /// </summary>
    public void PropertiesChanged()
    {
      this.m_InfoMessage = (string) null;
      if (this.targets.Length != 1)
        return;
      this.m_InfoMessage = PerformanceChecks.CheckMaterial(this.target as Material, EditorUserBuildSettings.activeBuildTarget);
    }

    protected override void OnHeaderGUI()
    {
      Rect rect = Editor.DrawHeaderGUI((Editor) this, this.targetTitle, !this.forceVisible ? 10f : 0.0f);
      int controlId = GUIUtility.GetControlID(45678, FocusType.Passive);
      if (this.forceVisible)
        return;
      Rect foldoutRenderRect = EditorGUI.GetInspectorTitleBarObjectFoldoutRenderRect(rect);
      foldoutRenderRect.y = rect.yMax - 17f;
      bool isExpanded = EditorGUI.DoObjectFoldout(this.m_IsVisible, rect, foldoutRenderRect, this.targets, controlId);
      if (isExpanded == this.m_IsVisible)
        return;
      this.m_IsVisible = isExpanded;
      InternalEditorUtility.SetIsInspectorExpanded(this.target, isExpanded);
    }

    internal override void OnHeaderControlsGUI()
    {
      this.serializedObject.Update();
      EditorGUI.BeginDisabledGroup(!this.IsEnabled());
      EditorGUIUtility.labelWidth = 50f;
      this.ShaderPopup((GUIStyle) "MiniPulldown");
      if ((UnityEngine.Object) this.m_Shader != (UnityEngine.Object) null && this.HasMultipleMixedShaderValues() && (this.m_Shader.hideFlags & HideFlags.DontSave) == HideFlags.None)
      {
        if (GUILayout.Button("Edit...", EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
          AssetDatabase.OpenAsset((UnityEngine.Object) this.m_Shader);
      }
      EditorGUI.EndDisabledGroup();
    }

    [Obsolete("Use GetMaterialProperty instead.")]
    public float GetFloat(string propertyName, out bool hasMixedValue)
    {
      hasMixedValue = false;
      float num = ((Material) this.targets[0]).GetFloat(propertyName);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        if ((double) ((Material) this.targets[index]).GetFloat(propertyName) != (double) num)
        {
          hasMixedValue = true;
          break;
        }
      }
      return num;
    }

    [Obsolete("Use MaterialProperty instead.")]
    public void SetFloat(string propertyName, float value)
    {
      foreach (Material target in this.targets)
        target.SetFloat(propertyName, value);
    }

    [Obsolete("Use GetMaterialProperty instead.")]
    public Color GetColor(string propertyName, out bool hasMixedValue)
    {
      hasMixedValue = false;
      Color color = ((Material) this.targets[0]).GetColor(propertyName);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        if (((Material) this.targets[index]).GetColor(propertyName) != color)
        {
          hasMixedValue = true;
          break;
        }
      }
      return color;
    }

    [Obsolete("Use MaterialProperty instead.")]
    public void SetColor(string propertyName, Color value)
    {
      foreach (Material target in this.targets)
        target.SetColor(propertyName, value);
    }

    [Obsolete("Use GetMaterialProperty instead.")]
    public Vector4 GetVector(string propertyName, out bool hasMixedValue)
    {
      hasMixedValue = false;
      Vector4 vector = ((Material) this.targets[0]).GetVector(propertyName);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        if (((Material) this.targets[index]).GetVector(propertyName) != vector)
        {
          hasMixedValue = true;
          break;
        }
      }
      return vector;
    }

    [Obsolete("Use MaterialProperty instead.")]
    public void SetVector(string propertyName, Vector4 value)
    {
      foreach (Material target in this.targets)
        target.SetVector(propertyName, value);
    }

    [Obsolete("Use GetMaterialProperty instead.")]
    public Texture GetTexture(string propertyName, out bool hasMixedValue)
    {
      hasMixedValue = false;
      Texture texture = ((Material) this.targets[0]).GetTexture(propertyName);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        if ((UnityEngine.Object) ((Material) this.targets[index]).GetTexture(propertyName) != (UnityEngine.Object) texture)
        {
          hasMixedValue = true;
          break;
        }
      }
      return texture;
    }

    [Obsolete("Use MaterialProperty instead.")]
    public void SetTexture(string propertyName, Texture value)
    {
      foreach (Material target in this.targets)
        target.SetTexture(propertyName, value);
    }

    [Obsolete("Use MaterialProperty instead.")]
    public Vector2 GetTextureScale(string propertyName, out bool hasMixedValueX, out bool hasMixedValueY)
    {
      hasMixedValueX = false;
      hasMixedValueY = false;
      Vector2 textureScale1 = ((Material) this.targets[0]).GetTextureScale(propertyName);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        Vector2 textureScale2 = ((Material) this.targets[index]).GetTextureScale(propertyName);
        if ((double) textureScale2.x != (double) textureScale1.x)
          hasMixedValueX = true;
        if ((double) textureScale2.y != (double) textureScale1.y)
          hasMixedValueY = true;
        if (hasMixedValueX && hasMixedValueY)
          break;
      }
      return textureScale1;
    }

    [Obsolete("Use MaterialProperty instead.")]
    public Vector2 GetTextureOffset(string propertyName, out bool hasMixedValueX, out bool hasMixedValueY)
    {
      hasMixedValueX = false;
      hasMixedValueY = false;
      Vector2 textureOffset1 = ((Material) this.targets[0]).GetTextureOffset(propertyName);
      for (int index = 1; index < this.targets.Length; ++index)
      {
        Vector2 textureOffset2 = ((Material) this.targets[index]).GetTextureOffset(propertyName);
        if ((double) textureOffset2.x != (double) textureOffset1.x)
          hasMixedValueX = true;
        if ((double) textureOffset2.y != (double) textureOffset1.y)
          hasMixedValueY = true;
        if (hasMixedValueX && hasMixedValueY)
          break;
      }
      return textureOffset1;
    }

    /// <summary>
    ///   <para>Set the scale of a given texture property.</para>
    /// </summary>
    /// <param name="propertyName">Name of the texture property that you wish to modify the scale of.</param>
    /// <param name="value">Scale to set.</param>
    /// <param name="coord">Set the x or y component of the scale (0 for x, 1 for y).</param>
    [Obsolete("Use MaterialProperty instead.")]
    public void SetTextureScale(string propertyName, Vector2 value, int coord)
    {
      foreach (Material target in this.targets)
      {
        Vector2 textureScale = target.GetTextureScale(propertyName);
        textureScale[coord] = value[coord];
        target.SetTextureScale(propertyName, textureScale);
      }
    }

    /// <summary>
    ///   <para>Set the offset of a given texture property.</para>
    /// </summary>
    /// <param name="propertyName">Name of the texture property that you wish to modify the offset of.</param>
    /// <param name="value">Scale to set.</param>
    /// <param name="coord">Set the x or y component of the offset (0 for x, 1 for y).</param>
    [Obsolete("Use MaterialProperty instead.")]
    public void SetTextureOffset(string propertyName, Vector2 value, int coord)
    {
      foreach (Material target in this.targets)
      {
        Vector2 textureOffset = target.GetTextureOffset(propertyName);
        textureOffset[coord] = value[coord];
        target.SetTextureOffset(propertyName, textureOffset);
      }
    }

    /// <summary>
    ///   <para>Draw a range slider for a range shader property.</para>
    /// </summary>
    /// <param name="label">Label for the property.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    public float RangeProperty(MaterialProperty prop, string label)
    {
      return this.RangeProperty(this.GetPropertyRect(prop, label, true), prop, label);
    }

    /// <summary>
    ///   <para>Draw a range slider for a range shader property.</para>
    /// </summary>
    /// <param name="label">Label for the property.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    public float RangeProperty(Rect position, MaterialProperty prop, string label)
    {
      float power = !(prop.name == "_Shininess") ? 1f : 5f;
      return MaterialEditor.DoPowerRangeProperty(position, prop, label, power);
    }

    internal static float DoPowerRangeProperty(Rect position, MaterialProperty prop, string label, float power)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = prop.hasMixedValue;
      float labelWidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = 0.0f;
      float num = EditorGUI.PowerSlider(position, label, prop.floatValue, prop.rangeLimits.x, prop.rangeLimits.y, power);
      EditorGUI.showMixedValue = false;
      EditorGUIUtility.labelWidth = labelWidth;
      if (EditorGUI.EndChangeCheck())
        prop.floatValue = num;
      return prop.floatValue;
    }

    /// <summary>
    ///   <para>Draw a property field for a float shader property.</para>
    /// </summary>
    /// <param name="label">Label for the property.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    public float FloatProperty(MaterialProperty prop, string label)
    {
      return this.FloatProperty(this.GetPropertyRect(prop, label, true), prop, label);
    }

    /// <summary>
    ///   <para>Draw a property field for a float shader property.</para>
    /// </summary>
    /// <param name="label">Label for the property.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    public float FloatProperty(Rect position, MaterialProperty prop, string label)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = prop.hasMixedValue;
      float num = EditorGUI.FloatField(position, label, prop.floatValue);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        prop.floatValue = num;
      return prop.floatValue;
    }

    /// <summary>
    ///   <para>Draw a property field for a color shader property.</para>
    /// </summary>
    /// <param name="label">Label for the property.</param>
    /// <param name="position"></param>
    /// <param name="prop"></param>
    public Color ColorProperty(MaterialProperty prop, string label)
    {
      return this.ColorProperty(this.GetPropertyRect(prop, label, true), prop, label);
    }

    /// <summary>
    ///   <para>Draw a property field for a color shader property.</para>
    /// </summary>
    /// <param name="label">Label for the property.</param>
    /// <param name="position"></param>
    /// <param name="prop"></param>
    public Color ColorProperty(Rect position, MaterialProperty prop, string label)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = prop.hasMixedValue;
      bool hdr = (prop.flags & MaterialProperty.PropFlags.HDR) != MaterialProperty.PropFlags.None;
      bool showAlpha = true;
      Color color = EditorGUI.ColorField(position, GUIContent.Temp(label), prop.colorValue, true, showAlpha, hdr, (ColorPickerHDRConfig) null);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        prop.colorValue = color;
      return prop.colorValue;
    }

    /// <summary>
    ///   <para>Draw a property field for a vector shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    public Vector4 VectorProperty(MaterialProperty prop, string label)
    {
      return this.VectorProperty(this.GetPropertyRect(prop, label, true), prop, label);
    }

    /// <summary>
    ///   <para>Draw a property field for a vector shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    public Vector4 VectorProperty(Rect position, MaterialProperty prop, string label)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = prop.hasMixedValue;
      Vector4 vector4 = EditorGUI.Vector4Field(position, label, prop.vectorValue);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        prop.vectorValue = vector4;
      return prop.vectorValue;
    }

    public void TextureScaleOffsetProperty(MaterialProperty property)
    {
      double num = (double) this.TextureScaleOffsetProperty(EditorGUILayout.GetControlRect(true, 32f, EditorStyles.layerMaskField, new GUILayoutOption[0]), property, false);
    }

    /// <summary>
    ///   <para>Draws tiling and offset properties for a texture.</para>
    /// </summary>
    /// <param name="position">Rect to draw this control in.</param>
    /// <param name="property">Property to draw.</param>
    /// <param name="partOfTexturePropertyControl">If this control should be rendered under large texture property control use 'true'. If this control should be shown seperately use 'false'.</param>
    public float TextureScaleOffsetProperty(Rect position, MaterialProperty property)
    {
      return this.TextureScaleOffsetProperty(position, property, true);
    }

    /// <summary>
    ///   <para>Draws tiling and offset properties for a texture.</para>
    /// </summary>
    /// <param name="position">Rect to draw this control in.</param>
    /// <param name="property">Property to draw.</param>
    /// <param name="partOfTexturePropertyControl">If this control should be rendered under large texture property control use 'true'. If this control should be shown seperately use 'false'.</param>
    public float TextureScaleOffsetProperty(Rect position, MaterialProperty property, bool partOfTexturePropertyControl)
    {
      this.BeginAnimatedCheck(property);
      EditorGUI.BeginChangeCheck();
      int mixedValueMask = property.mixedValueMask >> 1;
      Vector4 vector4 = MaterialEditor.TextureScaleOffsetProperty(position, property.textureScaleAndOffset, mixedValueMask, partOfTexturePropertyControl);
      if (EditorGUI.EndChangeCheck())
        property.textureScaleAndOffset = vector4;
      this.EndAnimatedCheck();
      return 32f;
    }

    private Texture TexturePropertyBody(Rect position, MaterialProperty prop)
    {
      if (prop.type != MaterialProperty.PropType.Texture)
        throw new ArgumentException(string.Format("The MaterialProperty '{0}' should be of type 'Texture' (its type is '{1})'", (object) prop.name, (object) prop.type));
      this.m_DesiredTexdim = prop.textureDimension;
      System.Type objType;
      switch (this.m_DesiredTexdim)
      {
        case MaterialProperty.TexDim.Tex2D:
          objType = typeof (Texture);
          break;
        case MaterialProperty.TexDim.Tex3D:
          objType = typeof (Texture3D);
          break;
        case MaterialProperty.TexDim.Cube:
          objType = typeof (Cubemap);
          break;
        case MaterialProperty.TexDim.Any:
          objType = typeof (Texture);
          break;
        default:
          objType = (System.Type) null;
          break;
      }
      bool enabled = GUI.enabled;
      EditorGUI.BeginChangeCheck();
      if ((prop.flags & MaterialProperty.PropFlags.PerRendererData) != MaterialProperty.PropFlags.None)
        GUI.enabled = false;
      EditorGUI.showMixedValue = prop.hasMixedValue;
      int controlId = GUIUtility.GetControlID(12354, EditorGUIUtility.native, position);
      Texture texture = EditorGUI.DoObjectField(position, position, controlId, (UnityEngine.Object) prop.textureValue, objType, (SerializedProperty) null, new EditorGUI.ObjectFieldValidator(this.TextureValidator), false) as Texture;
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        prop.textureValue = texture;
      GUI.enabled = enabled;
      return prop.textureValue;
    }

    /// <summary>
    ///   <para>Draw a property field for a texture shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="scaleOffset">Draw scale / offset.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    /// <param name="tooltip"></param>
    public Texture TextureProperty(MaterialProperty prop, string label)
    {
      bool scaleOffset = (prop.flags & MaterialProperty.PropFlags.NoScaleOffset) == MaterialProperty.PropFlags.None;
      return this.TextureProperty(prop, label, scaleOffset);
    }

    /// <summary>
    ///   <para>Draw a property field for a texture shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="scaleOffset">Draw scale / offset.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    /// <param name="tooltip"></param>
    public Texture TextureProperty(MaterialProperty prop, string label, bool scaleOffset)
    {
      return this.TextureProperty(this.GetPropertyRect(prop, label, true), prop, label, scaleOffset);
    }

    /// <summary>
    ///   <para>Make a help box with a message and button. Returns true, if button was pressed.</para>
    /// </summary>
    /// <param name="messageContent">The message text.</param>
    /// <param name="buttonContent">The button text.</param>
    /// <returns>
    ///   <para>Returns true, if button was pressed.</para>
    /// </returns>
    public bool HelpBoxWithButton(GUIContent messageContent, GUIContent buttonContent)
    {
      Rect rect = GUILayoutUtility.GetRect(messageContent, EditorStyles.helpBox);
      GUILayoutUtility.GetRect(1f, 25f);
      rect.height += 25f;
      GUI.Label(rect, messageContent, EditorStyles.helpBox);
      return GUI.Button(new Rect((float) ((double) rect.xMax - 60.0 - 4.0), (float) ((double) rect.yMax - 20.0 - 4.0), 60f, 20f), buttonContent);
    }

    /// <summary>
    ///   <para>Checks if particular property has incorrect type of texture specified by the material, displays appropriate warning and suggests the user to automatically fix the problem.</para>
    /// </summary>
    /// <param name="prop">The texture property to check and display warning for, if necessary.</param>
    public void TextureCompatibilityWarning(MaterialProperty prop)
    {
      if (InternalEditorUtility.BumpMapTextureNeedsFixing(prop) && this.HelpBoxWithButton(EditorGUIUtility.TextContent("This texture is not marked as a normal map"), EditorGUIUtility.TextContent("Fix Now")))
        InternalEditorUtility.FixNormalmapTexture(prop);
      bool canBeFixedAutomatically = false;
      if (!InternalEditorUtility.HDRTextureNeedsFixing(prop, out canBeFixedAutomatically))
        return;
      if (canBeFixedAutomatically)
      {
        if (!this.HelpBoxWithButton(EditorGUIUtility.TextContent("This texture contains alpha, but is not RGBM (incompatible with HDR)"), EditorGUIUtility.TextContent("Fix Now")))
          return;
        InternalEditorUtility.FixHDRTexture(prop);
      }
      else
        EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("This texture contains alpha, but is not RGBM (incompatible with HDR)").text, MessageType.Warning);
    }

    /// <summary>
    ///   <para>Draw a property field for a texture shader property that only takes up a single line height.</para>
    /// </summary>
    /// <param name="position">Rect that this control should be rendered in.</param>
    /// <param name="label">Label for the field.</param>
    /// <param name="prop"></param>
    /// <param name="tooltip"></param>
    /// <returns>
    ///   <para>Returns total height used by this control.</para>
    /// </returns>
    public Texture TexturePropertyMiniThumbnail(Rect position, MaterialProperty prop, string label, string tooltip)
    {
      this.BeginAnimatedCheck(prop);
      Rect thumbRect;
      Rect labelRect;
      EditorGUI.GetRectsForMiniThumbnailField(position, out thumbRect, out labelRect);
      EditorGUI.HandlePrefixLabel(position, labelRect, GUIContent.Temp(label, tooltip), 0, EditorStyles.label);
      this.EndAnimatedCheck();
      Texture texture = this.TexturePropertyBody(thumbRect, prop);
      Rect rect = position;
      rect.y += position.height;
      rect.height = 27f;
      this.TextureCompatibilityWarning(prop);
      return texture;
    }

    /// <summary>
    ///   <para>Returns the free rect below the label and before the large thumb object field. Is used for e.g. tiling and offset properties.</para>
    /// </summary>
    /// <param name="position">The total rect of the texture property.</param>
    public Rect GetTexturePropertyCustomArea(Rect position)
    {
      ++EditorGUI.indentLevel;
      position.height = MaterialEditor.GetTextureFieldHeight();
      Rect source = position;
      source.yMin += 16f;
      source.xMax -= EditorGUIUtility.fieldWidth + 2f;
      Rect rect = EditorGUI.IndentedRect(source);
      --EditorGUI.indentLevel;
      return rect;
    }

    /// <summary>
    ///   <para>Draw a property field for a texture shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="scaleOffset">Draw scale / offset.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    /// <param name="tooltip"></param>
    public Texture TextureProperty(Rect position, MaterialProperty prop, string label)
    {
      bool scaleOffset = (prop.flags & MaterialProperty.PropFlags.NoScaleOffset) == MaterialProperty.PropFlags.None;
      return this.TextureProperty(position, prop, label, scaleOffset);
    }

    /// <summary>
    ///   <para>Draw a property field for a texture shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="scaleOffset">Draw scale / offset.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    /// <param name="tooltip"></param>
    public Texture TextureProperty(Rect position, MaterialProperty prop, string label, bool scaleOffset)
    {
      return this.TextureProperty(position, prop, label, string.Empty, scaleOffset);
    }

    /// <summary>
    ///   <para>Draw a property field for a texture shader property.</para>
    /// </summary>
    /// <param name="label">Label for the field.</param>
    /// <param name="scaleOffset">Draw scale / offset.</param>
    /// <param name="prop"></param>
    /// <param name="position"></param>
    /// <param name="tooltip"></param>
    public Texture TextureProperty(Rect position, MaterialProperty prop, string label, string tooltip, bool scaleOffset)
    {
      EditorGUI.PrefixLabel(position, GUIContent.Temp(label, tooltip));
      position.height = MaterialEditor.GetTextureFieldHeight();
      Rect position1 = position;
      position1.xMin = position1.xMax - EditorGUIUtility.fieldWidth;
      Texture texture = this.TexturePropertyBody(position1, prop);
      if (scaleOffset)
      {
        double num = (double) this.TextureScaleOffsetProperty(this.GetTexturePropertyCustomArea(position), prop);
      }
      GUILayout.Space(-6f);
      this.TextureCompatibilityWarning(prop);
      GUILayout.Space(6f);
      return texture;
    }

    /// <summary>
    ///   <para>TODO.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="scaleOffset"></param>
    /// <param name="partOfTexturePropertyControl"></param>
    public static Vector4 TextureScaleOffsetProperty(Rect position, Vector4 scaleOffset)
    {
      return MaterialEditor.TextureScaleOffsetProperty(position, scaleOffset, 0, false);
    }

    /// <summary>
    ///   <para>TODO.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="scaleOffset"></param>
    /// <param name="partOfTexturePropertyControl"></param>
    public static Vector4 TextureScaleOffsetProperty(Rect position, Vector4 scaleOffset, bool partOfTexturePropertyControl)
    {
      return MaterialEditor.TextureScaleOffsetProperty(position, scaleOffset, 0, partOfTexturePropertyControl);
    }

    internal static Vector4 TextureScaleOffsetProperty(Rect position, Vector4 scaleOffset, int mixedValueMask, bool partOfTexturePropertyControl)
    {
      Vector2 vector2_1 = new Vector2(scaleOffset.x, scaleOffset.y);
      Vector2 vector2_2 = new Vector2(scaleOffset.z, scaleOffset.w);
      float width = EditorGUIUtility.labelWidth;
      float x1 = position.x + width;
      float x2 = position.x + EditorGUI.indent;
      if (partOfTexturePropertyControl)
      {
        width = 65f;
        x1 = position.x + width;
        x2 = position.x;
        position.y = position.yMax - 32f;
      }
      Rect totalPosition = new Rect(x2, position.y, width, 16f);
      Rect position1 = new Rect(x1, position.y, position.width - width, 16f);
      EditorGUI.PrefixLabel(totalPosition, MaterialEditor.s_TilingText);
      Vector2 vector2_3 = EditorGUI.Vector2Field(position1, GUIContent.none, vector2_1);
      totalPosition.y += 16f;
      position1.y += 16f;
      EditorGUI.PrefixLabel(totalPosition, MaterialEditor.s_OffsetText);
      Vector2 vector2_4 = EditorGUI.Vector2Field(position1, GUIContent.none, vector2_2);
      return new Vector4(vector2_3.x, vector2_3.y, vector2_4.x, vector2_4.y);
    }

    /// <summary>
    ///   <para>Calculate height needed for the property.</para>
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    public float GetPropertyHeight(MaterialProperty prop)
    {
      return this.GetPropertyHeight(prop, prop.displayName);
    }

    /// <summary>
    ///   <para>Calculate height needed for the property.</para>
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    public float GetPropertyHeight(MaterialProperty prop, string label)
    {
      float num = 0.0f;
      MaterialPropertyHandler handler = MaterialPropertyHandler.GetHandler(((Material) this.target).shader, prop.name);
      if (handler != null)
      {
        num = handler.GetPropertyHeight(prop, label ?? prop.displayName, this);
        if (handler.propertyDrawer != null)
          return num;
      }
      return num + MaterialEditor.GetDefaultPropertyHeight(prop);
    }

    private static float GetTextureFieldHeight()
    {
      return 64f;
    }

    /// <summary>
    ///   <para>Calculate height needed for the property, ignoring custom drawers.</para>
    /// </summary>
    /// <param name="prop"></param>
    public static float GetDefaultPropertyHeight(MaterialProperty prop)
    {
      if (prop.type == MaterialProperty.PropType.Vector)
        return 32f;
      if (prop.type == MaterialProperty.PropType.Texture)
        return MaterialEditor.GetTextureFieldHeight() + 6f;
      return 16f;
    }

    private Rect GetPropertyRect(MaterialProperty prop, string label, bool ignoreDrawer)
    {
      float height = 0.0f;
      if (!ignoreDrawer)
      {
        MaterialPropertyHandler handler = MaterialPropertyHandler.GetHandler(((Material) this.target).shader, prop.name);
        if (handler != null)
        {
          height = handler.GetPropertyHeight(prop, label ?? prop.displayName, this);
          if (handler.propertyDrawer != null)
            return EditorGUILayout.GetControlRect(true, height, EditorStyles.layerMaskField, new GUILayoutOption[0]);
        }
      }
      return EditorGUILayout.GetControlRect(true, height + MaterialEditor.GetDefaultPropertyHeight(prop), EditorStyles.layerMaskField, new GUILayoutOption[0]);
    }

    public void BeginAnimatedCheck(MaterialProperty prop)
    {
      if ((UnityEngine.Object) this.m_RendererForAnimationMode == (UnityEngine.Object) null)
        return;
      this.m_PreviousGUIColor = GUI.color;
      if (!MaterialAnimationUtility.IsAnimated(prop, this.m_RendererForAnimationMode))
        return;
      GUI.color = AnimationMode.animatedPropertyColor;
    }

    public void EndAnimatedCheck()
    {
      if ((UnityEngine.Object) this.m_RendererForAnimationMode == (UnityEngine.Object) null)
        return;
      GUI.color = this.m_PreviousGUIColor;
    }

    /// <summary>
    ///   <para>Handes UI for one shader property.</para>
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    /// <param name="position"></param>
    public void ShaderProperty(MaterialProperty prop, string label)
    {
      this.ShaderProperty(prop, label, 0);
    }

    public void ShaderProperty(MaterialProperty prop, string label, int labelIndent)
    {
      this.ShaderProperty(this.GetPropertyRect(prop, label, false), prop, label, labelIndent);
    }

    /// <summary>
    ///   <para>Handes UI for one shader property.</para>
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    /// <param name="position"></param>
    public void ShaderProperty(Rect position, MaterialProperty prop, string label)
    {
      this.ShaderProperty(position, prop, label, 0);
    }

    public void ShaderProperty(Rect position, MaterialProperty prop, string label, int labelIndent)
    {
      this.BeginAnimatedCheck(prop);
      EditorGUI.indentLevel += labelIndent;
      this.ShaderPropertyInternal(position, prop, label);
      EditorGUI.indentLevel -= labelIndent;
      this.EndAnimatedCheck();
    }

    /// <summary>
    ///         <para>This function will draw the UI for the lightmap emission property. (None, Realtime, baked)
    /// 
    /// See Also: MaterialLightmapFlags.</para>
    ///       </summary>
    public void LightmapEmissionProperty()
    {
      this.LightmapEmissionProperty(0);
    }

    public void LightmapEmissionProperty(int labelIndent)
    {
      this.LightmapEmissionProperty(EditorGUILayout.GetControlRect(true, 16f, EditorStyles.layerMaskField, new GUILayoutOption[0]), labelIndent);
    }

    private static int GetGlobalIlluminationInt(MaterialGlobalIlluminationFlags flags)
    {
      int num = 0;
      if ((flags & MaterialGlobalIlluminationFlags.RealtimeEmissive) != MaterialGlobalIlluminationFlags.None)
        num = 1;
      else if ((flags & MaterialGlobalIlluminationFlags.BakedEmissive) != MaterialGlobalIlluminationFlags.None)
        num = 2;
      return num;
    }

    public void LightmapEmissionProperty(Rect position, int labelIndent)
    {
      EditorGUI.indentLevel += labelIndent;
      UnityEngine.Object[] targets = this.targets;
      int globalIlluminationInt = MaterialEditor.GetGlobalIlluminationInt(((Material) this.target).globalIlluminationFlags);
      bool flag = false;
      for (int index = 1; index < targets.Length; ++index)
      {
        if (MaterialEditor.GetGlobalIlluminationInt(((Material) targets[index]).globalIlluminationFlags) != globalIlluminationInt)
          flag = true;
      }
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = flag;
      int num = EditorGUI.IntPopup(position, MaterialEditor.Styles.lightmapEmissiveLabel, globalIlluminationInt, MaterialEditor.Styles.lightmapEmissiveStrings, MaterialEditor.Styles.lightmapEmissiveValues);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        foreach (Material material in targets)
        {
          MaterialGlobalIlluminationFlags illuminationFlags = material.globalIlluminationFlags & ~(MaterialGlobalIlluminationFlags.RealtimeEmissive | MaterialGlobalIlluminationFlags.BakedEmissive) | (MaterialGlobalIlluminationFlags) num;
          material.globalIlluminationFlags = illuminationFlags;
        }
      }
      EditorGUI.indentLevel -= labelIndent;
    }

    private void ShaderPropertyInternal(Rect position, MaterialProperty prop, string label)
    {
      MaterialPropertyHandler handler = MaterialPropertyHandler.GetHandler(((Material) this.target).shader, prop.name);
      if (handler != null)
      {
        handler.OnGUI(ref position, prop, label ?? prop.displayName, this);
        if (handler.propertyDrawer != null)
          return;
      }
      this.DefaultShaderProperty(position, prop, label);
    }

    /// <summary>
    ///   <para>Handles UI for one shader property ignoring any custom drawers.</para>
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    /// <param name="position"></param>
    public void DefaultShaderProperty(MaterialProperty prop, string label)
    {
      this.DefaultShaderProperty(this.GetPropertyRect(prop, label, true), prop, label);
    }

    /// <summary>
    ///   <para>Handles UI for one shader property ignoring any custom drawers.</para>
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    /// <param name="position"></param>
    public void DefaultShaderProperty(Rect position, MaterialProperty prop, string label)
    {
      switch (prop.type)
      {
        case MaterialProperty.PropType.Color:
          this.ColorProperty(position, prop, label);
          break;
        case MaterialProperty.PropType.Vector:
          this.VectorProperty(position, prop, label);
          break;
        case MaterialProperty.PropType.Float:
          double num1 = (double) this.FloatProperty(position, prop, label);
          break;
        case MaterialProperty.PropType.Range:
          double num2 = (double) this.RangeProperty(position, prop, label);
          break;
        case MaterialProperty.PropType.Texture:
          this.TextureProperty(position, prop, label);
          break;
        default:
          GUI.Label(position, "Unknown property type: " + prop.name + ": " + (object) prop.type);
          break;
      }
    }

    [Obsolete("Use RangeProperty with MaterialProperty instead.")]
    public float RangeProperty(string propertyName, string label, float v2, float v3)
    {
      return this.RangeProperty(MaterialEditor.GetMaterialProperty(this.targets, propertyName), label);
    }

    [Obsolete("Use FloatProperty with MaterialProperty instead.")]
    public float FloatProperty(string propertyName, string label)
    {
      return this.FloatProperty(MaterialEditor.GetMaterialProperty(this.targets, propertyName), label);
    }

    [Obsolete("Use ColorProperty with MaterialProperty instead.")]
    public Color ColorProperty(string propertyName, string label)
    {
      return this.ColorProperty(MaterialEditor.GetMaterialProperty(this.targets, propertyName), label);
    }

    [Obsolete("Use VectorProperty with MaterialProperty instead.")]
    public Vector4 VectorProperty(string propertyName, string label)
    {
      return this.VectorProperty(MaterialEditor.GetMaterialProperty(this.targets, propertyName), label);
    }

    [Obsolete("Use TextureProperty with MaterialProperty instead.")]
    public Texture TextureProperty(string propertyName, string label, ShaderUtil.ShaderPropertyTexDim texDim)
    {
      return this.TextureProperty(MaterialEditor.GetMaterialProperty(this.targets, propertyName), label);
    }

    [Obsolete("Use TextureProperty with MaterialProperty instead.")]
    public Texture TextureProperty(string propertyName, string label, ShaderUtil.ShaderPropertyTexDim texDim, bool scaleOffset)
    {
      return this.TextureProperty(MaterialEditor.GetMaterialProperty(this.targets, propertyName), label, scaleOffset);
    }

    [Obsolete("Use ShaderProperty that takes MaterialProperty parameter instead.")]
    public void ShaderProperty(Shader shader, int propertyIndex)
    {
      MaterialProperty materialProperty = MaterialEditor.GetMaterialProperty(this.targets, propertyIndex);
      this.ShaderProperty(materialProperty, materialProperty.displayName);
    }

    /// <summary>
    ///   <para>Get shader property information of the passed materials.</para>
    /// </summary>
    /// <param name="mats"></param>
    public static MaterialProperty[] GetMaterialProperties(UnityEngine.Object[] mats)
    {
      return ShaderUtil.GetMaterialProperties(mats);
    }

    /// <summary>
    ///   <para>Get information about a single shader property.</para>
    /// </summary>
    /// <param name="mats">Selected materials.</param>
    /// <param name="name">Property name.</param>
    /// <param name="propertyIndex">Property index.</param>
    public static MaterialProperty GetMaterialProperty(UnityEngine.Object[] mats, string name)
    {
      return ShaderUtil.GetMaterialProperty(mats, name);
    }

    /// <summary>
    ///   <para>Get information about a single shader property.</para>
    /// </summary>
    /// <param name="mats">Selected materials.</param>
    /// <param name="name">Property name.</param>
    /// <param name="propertyIndex">Property index.</param>
    public static MaterialProperty GetMaterialProperty(UnityEngine.Object[] mats, int propertyIndex)
    {
      return ShaderUtil.GetMaterialProperty_Index(mats, propertyIndex);
    }

    private static Renderer GetAssociatedRenderFromInspector()
    {
      if ((bool) ((UnityEngine.Object) InspectorWindow.s_CurrentInspectorWindow))
      {
        foreach (Editor activeEditor in InspectorWindow.s_CurrentInspectorWindow.GetTracker().activeEditors)
        {
          Renderer target = activeEditor.target as Renderer;
          if ((bool) ((UnityEngine.Object) target))
            return target;
        }
      }
      return (Renderer) null;
    }

    public static Renderer PrepareMaterialPropertiesForAnimationMode(MaterialProperty[] properties, bool isMaterialEditable)
    {
      bool flag = AnimationMode.InAnimationMode();
      Renderer renderFromInspector = MaterialEditor.GetAssociatedRenderFromInspector();
      if ((UnityEngine.Object) renderFromInspector != (UnityEngine.Object) null)
      {
        MaterialEditor.ForwardApplyMaterialModification materialModification = new MaterialEditor.ForwardApplyMaterialModification(renderFromInspector, isMaterialEditable);
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        renderFromInspector.GetPropertyBlock(materialPropertyBlock);
        foreach (MaterialProperty property in properties)
        {
          property.ReadFromMaterialPropertyBlock(materialPropertyBlock);
          if (flag)
            property.applyPropertyCallback = new MaterialProperty.ApplyPropertyCallback(materialModification.DidModifyAnimationModeMaterialProperty);
        }
      }
      if (flag)
        return renderFromInspector;
      return (Renderer) null;
    }

    /// <summary>
    ///   <para>Set EditorGUIUtility.fieldWidth and labelWidth to the default values that PropertiesGUI uses.</para>
    /// </summary>
    public void SetDefaultGUIWidths()
    {
      EditorGUIUtility.fieldWidth = 64f;
      EditorGUIUtility.labelWidth = (float) ((double) GUIClip.visibleRect.width - (double) EditorGUIUtility.fieldWidth - 17.0);
    }

    private bool IsMaterialEditor(string customEditorName)
    {
      string str = "UnityEditor." + customEditorName;
      foreach (Assembly loadedAssembly in EditorAssemblies.loadedAssemblies)
      {
        foreach (System.Type c in AssemblyHelper.GetTypesFromAssembly(loadedAssembly))
        {
          if ((c.FullName.Equals(customEditorName, StringComparison.Ordinal) || c.FullName.Equals(str, StringComparison.Ordinal)) && typeof (MaterialEditor).IsAssignableFrom(c))
            return true;
        }
      }
      return false;
    }

    private void CreateCustomShaderGUI(Shader shader, string oldEditorName)
    {
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null || string.IsNullOrEmpty(shader.customEditor))
      {
        this.m_CustomShaderGUI = (ShaderGUI) null;
      }
      else
      {
        if (oldEditorName == shader.customEditor)
          return;
        this.m_CustomShaderGUI = ShaderGUIUtility.CreateShaderGUI(shader.customEditor);
        this.m_CheckSetup = true;
      }
    }

    /// <summary>
    ///   <para>Render the standard material properties. This method will either render properties using a IShaderGUI instance if found otherwise it uses PropertiesDefaultGUI.</para>
    /// </summary>
    /// <returns>
    ///   <para>Returns true if any value was changed.</para>
    /// </returns>
    public bool PropertiesGUI()
    {
      if (this.m_InsidePropertiesGUI)
      {
        Debug.LogWarning((object) "PropertiesGUI() is being called recursivly. If you want to render the default gui for shader properties then call PropertiesDefaultGUI() instead");
        return false;
      }
      EditorGUI.BeginChangeCheck();
      MaterialProperty[] materialProperties = MaterialEditor.GetMaterialProperties(this.targets);
      this.m_RendererForAnimationMode = MaterialEditor.PrepareMaterialPropertiesForAnimationMode(materialProperties, GUI.enabled);
      bool enabled = GUI.enabled;
      if ((UnityEngine.Object) this.m_RendererForAnimationMode != (UnityEngine.Object) null)
        GUI.enabled = true;
      this.m_InsidePropertiesGUI = true;
      try
      {
        if (this.m_CustomShaderGUI != null)
          this.m_CustomShaderGUI.OnGUI(this, materialProperties);
        else
          this.PropertiesDefaultGUI(materialProperties);
        Renderer renderFromInspector = MaterialEditor.GetAssociatedRenderFromInspector();
        if ((UnityEngine.Object) renderFromInspector != (UnityEngine.Object) null)
        {
          MaterialPropertyBlock dest = new MaterialPropertyBlock();
          renderFromInspector.GetPropertyBlock(dest);
          if (!dest.isEmpty)
            EditorGUILayout.HelpBox(MaterialEditor.s_PropBlockWarning, MessageType.Warning);
        }
      }
      catch (Exception ex)
      {
        GUI.enabled = enabled;
        this.m_InsidePropertiesGUI = false;
        this.m_RendererForAnimationMode = (Renderer) null;
        throw;
      }
      GUI.enabled = enabled;
      this.m_InsidePropertiesGUI = false;
      this.m_RendererForAnimationMode = (Renderer) null;
      return EditorGUI.EndChangeCheck();
    }

    /// <summary>
    ///   <para>Default rendering of shader properties.</para>
    /// </summary>
    /// <param name="props">Array of material properties.</param>
    public void PropertiesDefaultGUI(MaterialProperty[] props)
    {
      this.SetDefaultGUIWidths();
      if (this.m_InfoMessage != null)
        EditorGUILayout.HelpBox(this.m_InfoMessage, MessageType.Info);
      else
        GUIUtility.GetControlID(MaterialEditor.s_ControlHash, FocusType.Passive, new Rect(0.0f, 0.0f, 0.0f, 0.0f));
      for (int index = 0; index < props.Length; ++index)
      {
        if ((props[index].flags & (MaterialProperty.PropFlags.HideInInspector | MaterialProperty.PropFlags.PerRendererData)) == MaterialProperty.PropFlags.None)
          this.ShaderProperty(EditorGUILayout.GetControlRect(true, this.GetPropertyHeight(props[index], props[index].displayName), EditorStyles.layerMaskField, new GUILayoutOption[0]), props[index], props[index].displayName);
      }
    }

    /// <summary>
    ///   <para>Apply initial MaterialPropertyDrawer values.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="targets"></param>
    public static void ApplyMaterialPropertyDrawers(Material material)
    {
      MaterialEditor.ApplyMaterialPropertyDrawers(new UnityEngine.Object[1]
      {
        (UnityEngine.Object) material
      });
    }

    /// <summary>
    ///   <para>Apply initial MaterialPropertyDrawer values.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="targets"></param>
    public static void ApplyMaterialPropertyDrawers(UnityEngine.Object[] targets)
    {
      if (targets == null || targets.Length == 0)
        return;
      Material target = targets[0] as Material;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      Shader shader = target.shader;
      MaterialProperty[] materialProperties = MaterialEditor.GetMaterialProperties(targets);
      for (int index = 0; index < materialProperties.Length; ++index)
      {
        MaterialPropertyHandler handler = MaterialPropertyHandler.GetHandler(shader, materialProperties[index].name);
        if (handler != null && handler.propertyDrawer != null)
          handler.propertyDrawer.Apply(materialProperties[index]);
      }
    }

    /// <summary>
    ///   <para>Call this when you change a material property. It will add an undo for the action.</para>
    /// </summary>
    /// <param name="label">Undo Label.</param>
    public void RegisterPropertyChangeUndo(string label)
    {
      Undo.RecordObjects(this.targets, "Modify " + label + " of " + this.targetTitle);
    }

    private UnityEngine.Object TextureValidator(UnityEngine.Object[] references, System.Type objType, SerializedProperty property)
    {
      foreach (UnityEngine.Object reference in references)
      {
        Texture t = reference as Texture;
        if ((bool) ((UnityEngine.Object) t) && ((MaterialProperty.TexDim) ShaderUtil.GetTextureDimension(t) == this.m_DesiredTexdim || this.m_DesiredTexdim == MaterialProperty.TexDim.Any))
          return (UnityEngine.Object) t;
      }
      return (UnityEngine.Object) null;
    }

    private void Init()
    {
      if (this.m_PreviewUtility == null)
      {
        this.m_PreviewUtility = new PreviewRenderUtility();
        EditorUtility.SetCameraAnimateMaterials(this.m_PreviewUtility.m_Camera, true);
      }
      if (!((UnityEngine.Object) MaterialEditor.s_Meshes[0] == (UnityEngine.Object) null))
        return;
      GameObject gameObject = (GameObject) EditorGUIUtility.LoadRequired("Previews/PreviewMaterials.fbx");
      gameObject.SetActive(false);
      foreach (Transform transform in gameObject.transform)
      {
        MeshFilter component = transform.GetComponent<MeshFilter>();
        string name = transform.name;
        if (name != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (MaterialEditor.\u003C\u003Ef__switch\u0024map1C == null)
          {
            // ISSUE: reference to a compiler-generated field
            MaterialEditor.\u003C\u003Ef__switch\u0024map1C = new Dictionary<string, int>(4)
            {
              {
                "sphere",
                0
              },
              {
                "cube",
                1
              },
              {
                "cylinder",
                2
              },
              {
                "torus",
                3
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (MaterialEditor.\u003C\u003Ef__switch\u0024map1C.TryGetValue(name, out num))
          {
            switch (num)
            {
              case 0:
                MaterialEditor.s_Meshes[0] = component.sharedMesh;
                continue;
              case 1:
                MaterialEditor.s_Meshes[1] = component.sharedMesh;
                continue;
              case 2:
                MaterialEditor.s_Meshes[2] = component.sharedMesh;
                continue;
              case 3:
                MaterialEditor.s_Meshes[3] = component.sharedMesh;
                continue;
            }
          }
        }
        Debug.Log((object) ("Something is wrong, weird object found: " + transform.name));
      }
      MaterialEditor.s_MeshIcons[0] = EditorGUIUtility.IconContent("PreMatSphere");
      MaterialEditor.s_MeshIcons[1] = EditorGUIUtility.IconContent("PreMatCube");
      MaterialEditor.s_MeshIcons[2] = EditorGUIUtility.IconContent("PreMatCylinder");
      MaterialEditor.s_MeshIcons[3] = EditorGUIUtility.IconContent("PreMatTorus");
      MaterialEditor.s_MeshIcons[4] = EditorGUIUtility.IconContent("PreMatQuad");
      MaterialEditor.s_LightIcons[0] = EditorGUIUtility.IconContent("PreMatLight0");
      MaterialEditor.s_LightIcons[1] = EditorGUIUtility.IconContent("PreMatLight1");
      MaterialEditor.s_TimeIcons[0] = EditorGUIUtility.IconContent("PlayButton");
      MaterialEditor.s_TimeIcons[1] = EditorGUIUtility.IconContent("PauseButton");
      Mesh builtinResource = Resources.GetBuiltinResource(typeof (Mesh), "Quad.fbx") as Mesh;
      MaterialEditor.s_Meshes[4] = builtinResource;
      MaterialEditor.s_PlaneMesh = builtinResource;
    }

    public override void OnPreviewSettings()
    {
      if (this.m_CustomShaderGUI != null)
        this.m_CustomShaderGUI.OnMaterialPreviewSettingsGUI(this);
      else
        this.DefaultPreviewSettingsGUI();
    }

    private bool PreviewSettingsMenuButton(out Rect buttonRect)
    {
      buttonRect = GUILayoutUtility.GetRect(14f, 24f, 14f, 20f);
      Rect position = new Rect(buttonRect.x + (float) (((double) buttonRect.width - 16.0) / 2.0), buttonRect.y + (float) (((double) buttonRect.height - 6.0) / 2.0), 16f, 6f);
      if (Event.current.type == EventType.Repaint)
        MaterialEditor.Styles.kReflectionProbePickerStyle.Draw(position, false, false, false, false);
      return EditorGUI.ButtonMouseDown(buttonRect, GUIContent.none, FocusType.Passive, GUIStyle.none);
    }

    /// <summary>
    ///   <para>Default toolbar for material preview area.</para>
    /// </summary>
    public void DefaultPreviewSettingsGUI()
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return;
      this.Init();
      if (this.targets.Length <= 1 && MaterialEditor.GetPreviewType(this.target as Material) != MaterialEditor.PreviewType.Mesh)
        return;
      this.m_TimeUpdate = PreviewGUI.CycleButton(this.m_TimeUpdate, MaterialEditor.s_TimeIcons);
      this.m_SelectedMesh = PreviewGUI.CycleButton(this.m_SelectedMesh, MaterialEditor.s_MeshIcons);
      this.m_LightMode = PreviewGUI.CycleButton(this.m_LightMode, MaterialEditor.s_LightIcons);
      Rect buttonRect;
      if (!this.PreviewSettingsMenuButton(out buttonRect))
        return;
      PopupWindow.Show(buttonRect, (PopupWindowContent) this.m_ReflectionProbePicker);
    }

    public override sealed Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return (Texture2D) null;
      this.Init();
      this.m_PreviewUtility.BeginStaticPreview(new Rect(0.0f, 0.0f, (float) width, (float) height));
      this.DoRenderPreview();
      return this.m_PreviewUtility.EndStaticPreview();
    }

    private void DoRenderPreview()
    {
      if (this.m_PreviewUtility.m_RenderTexture.width <= 0 || this.m_PreviewUtility.m_RenderTexture.height <= 0)
        return;
      Material target = this.target as Material;
      MaterialEditor.PreviewType previewType = MaterialEditor.GetPreviewType(target);
      this.m_PreviewUtility.m_Camera.transform.position = -Vector3.forward * 5f;
      this.m_PreviewUtility.m_Camera.transform.rotation = Quaternion.identity;
      Color ambient;
      if (this.m_LightMode == 0)
      {
        this.m_PreviewUtility.m_Light[0].intensity = 1f;
        this.m_PreviewUtility.m_Light[0].transform.rotation = Quaternion.Euler(30f, 30f, 0.0f);
        this.m_PreviewUtility.m_Light[1].intensity = 0.0f;
        ambient = new Color(0.2f, 0.2f, 0.2f, 0.0f);
      }
      else
      {
        this.m_PreviewUtility.m_Light[0].intensity = 1f;
        this.m_PreviewUtility.m_Light[0].transform.rotation = Quaternion.Euler(50f, 50f, 0.0f);
        this.m_PreviewUtility.m_Light[1].intensity = 1f;
        ambient = new Color(0.2f, 0.2f, 0.2f, 0.0f);
      }
      InternalEditorUtility.SetCustomLighting(this.m_PreviewUtility.m_Light, ambient);
      Quaternion quaternion = Quaternion.identity;
      if (MaterialEditor.DoesPreviewAllowRotation(previewType))
        quaternion = Quaternion.Euler(this.m_PreviewDir.y, 0.0f, 0.0f) * Quaternion.Euler(0.0f, this.m_PreviewDir.x, 0.0f);
      Mesh mesh = MaterialEditor.s_Meshes[this.m_SelectedMesh];
      switch (previewType)
      {
        case MaterialEditor.PreviewType.Mesh:
          this.m_PreviewUtility.m_Camera.transform.position = Quaternion.Inverse(quaternion) * this.m_PreviewUtility.m_Camera.transform.position;
          this.m_PreviewUtility.m_Camera.transform.LookAt(Vector3.zero);
          quaternion = Quaternion.identity;
          break;
        case MaterialEditor.PreviewType.Plane:
          mesh = MaterialEditor.s_PlaneMesh;
          break;
        case MaterialEditor.PreviewType.Skybox:
          mesh = (Mesh) null;
          this.m_PreviewUtility.m_Camera.transform.rotation = Quaternion.Inverse(quaternion);
          this.m_PreviewUtility.m_Camera.fieldOfView = 120f;
          break;
      }
      if ((UnityEngine.Object) mesh != (UnityEngine.Object) null)
        this.m_PreviewUtility.DrawMesh(mesh, Vector3.zero, quaternion, target, 0, (MaterialPropertyBlock) null, this.m_ReflectionProbePicker.Target);
      bool fog = RenderSettings.fog;
      Unsupported.SetRenderSettingsUseFogNoDirty(false);
      this.m_PreviewUtility.m_Camera.Render();
      if (previewType == MaterialEditor.PreviewType.Skybox)
      {
        GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
        InternalEditorUtility.DrawSkyboxMaterial(target, this.m_PreviewUtility.m_Camera);
        GL.sRGBWrite = false;
      }
      Unsupported.SetRenderSettingsUseFogNoDirty(fog);
      InternalEditorUtility.RemoveCustomLighting();
    }

    /// <summary>
    ///   <para>Can this component be Previewed in its current state?</para>
    /// </summary>
    /// <returns>
    ///   <para>True if this component can be Previewed in its current state.</para>
    /// </returns>
    public override sealed bool HasPreviewGUI()
    {
      return true;
    }

    /// <summary>
    ///   <para>Does this edit require to be repainted constantly in its current state?</para>
    /// </summary>
    public override bool RequiresConstantRepaint()
    {
      return this.m_TimeUpdate == 1;
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      if (this.m_CustomShaderGUI != null)
        this.m_CustomShaderGUI.OnMaterialInteractivePreviewGUI(this, r, background);
      else
        base.OnInteractivePreviewGUI(r, background);
    }

    /// <summary>
    ///   <para>Custom preview for Image component.</para>
    /// </summary>
    /// <param name="r">Rectangle in which to draw the preview.</param>
    /// <param name="background">Background image.</param>
    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (this.m_CustomShaderGUI != null)
        this.m_CustomShaderGUI.OnMaterialPreviewGUI(this, r, background);
      else
        this.DefaultPreviewGUI(r, background);
    }

    /// <summary>
    ///   <para>Default handling of preview area for materials.</para>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="background"></param>
    public void DefaultPreviewGUI(Rect r, GUIStyle background)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y, r.width, 40f), "Material preview \nnot available");
      }
      else
      {
        this.Init();
        if (MaterialEditor.DoesPreviewAllowRotation(MaterialEditor.GetPreviewType(this.target as Material)))
          this.m_PreviewDir = PreviewGUI.Drag2D(this.m_PreviewDir, r);
        if (Event.current.type != EventType.Repaint)
          return;
        this.m_PreviewUtility.BeginPreview(r, background);
        this.DoRenderPreview();
        this.m_PreviewUtility.EndAndDrawPreview(r);
      }
    }

    /// <summary>
    ///   <para>Called when the editor is enabled, if overridden please call the base OnEnable() to ensure that the material inspector is set up properly.</para>
    /// </summary>
    public virtual void OnEnable()
    {
      this.m_Shader = this.serializedObject.FindProperty("m_Shader").objectReferenceValue as Shader;
      this.CreateCustomShaderGUI(this.m_Shader, string.Empty);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      this.PropertiesChanged();
      this.m_ReflectionProbePicker.OnEnable();
    }

    public virtual void UndoRedoPerformed()
    {
      if (ActiveEditorTracker.sharedTracker != null)
        ActiveEditorTracker.sharedTracker.ForceRebuild();
      this.PropertiesChanged();
    }

    /// <summary>
    ///   <para>Called when the editor is disabled, if overridden please call the base OnDisable() to ensure that the material inspector is set up properly.</para>
    /// </summary>
    public virtual void OnDisable()
    {
      this.m_ReflectionProbePicker.OnDisable();
      if (this.m_PreviewUtility != null)
      {
        this.m_PreviewUtility.Cleanup();
        this.m_PreviewUtility = (PreviewRenderUtility) null;
      }
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    internal void OnSceneDrag(SceneView sceneView)
    {
      Event current = Event.current;
      if (current.type == EventType.Repaint)
        return;
      int materialIndex = -1;
      GameObject go = HandleUtility.PickGameObject(current.mousePosition, out materialIndex);
      if (EditorMaterialUtility.IsBackgroundMaterial(this.target as Material))
      {
        this.HandleSkybox(go, current);
      }
      else
      {
        if (!(bool) ((UnityEngine.Object) go) || !(bool) ((UnityEngine.Object) go.GetComponent<Renderer>()))
          return;
        this.HandleRenderer(go.GetComponent<Renderer>(), materialIndex, current);
      }
    }

    internal void HandleSkybox(GameObject go, Event evt)
    {
      bool flag1 = !(bool) ((UnityEngine.Object) go);
      bool flag2 = false;
      if (!flag1 || evt.type == EventType.DragExited)
      {
        evt.Use();
      }
      else
      {
        switch (evt.type)
        {
          case EventType.DragUpdated:
            DragAndDrop.visualMode = DragAndDropVisualMode.Link;
            flag2 = true;
            break;
          case EventType.DragPerform:
            DragAndDrop.AcceptDrag();
            flag2 = true;
            break;
        }
      }
      if (!flag2)
        return;
      Undo.RecordObject((UnityEngine.Object) UnityEngine.Object.FindObjectOfType<RenderSettings>(), "Assign Skybox Material");
      RenderSettings.skybox = this.target as Material;
      evt.Use();
    }

    internal void HandleRenderer(Renderer r, int materialIndex, Event evt)
    {
      bool flag = false;
      switch (evt.type)
      {
        case EventType.DragUpdated:
          DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
          flag = true;
          break;
        case EventType.DragPerform:
          DragAndDrop.AcceptDrag();
          flag = true;
          break;
      }
      if (!flag)
        return;
      Undo.RecordObject((UnityEngine.Object) r, "Assign Material");
      Material[] sharedMaterials = r.sharedMaterials;
      if (!evt.alt && (materialIndex >= 0 && materialIndex < r.sharedMaterials.Length))
      {
        sharedMaterials[materialIndex] = this.target as Material;
      }
      else
      {
        for (int index = 0; index < sharedMaterials.Length; ++index)
          sharedMaterials[index] = this.target as Material;
      }
      r.sharedMaterials = sharedMaterials;
      evt.Use();
    }

    /// <summary>
    ///   <para>Method for showing a texture property control with additional inlined properites.</para>
    /// </summary>
    /// <param name="label">The label used for the texture property.</param>
    /// <param name="textureProp">The texture property.</param>
    /// <param name="extraProperty1">First optional property inlined after the texture property.</param>
    /// <param name="extraProperty2">Second optional property inlined after the extraProperty1.</param>
    /// <returns>
    ///   <para>Returns the Rect used.</para>
    /// </returns>
    public Rect TexturePropertySingleLine(GUIContent label, MaterialProperty textureProp)
    {
      return this.TexturePropertySingleLine(label, textureProp, (MaterialProperty) null, (MaterialProperty) null);
    }

    /// <summary>
    ///   <para>Method for showing a texture property control with additional inlined properites.</para>
    /// </summary>
    /// <param name="label">The label used for the texture property.</param>
    /// <param name="textureProp">The texture property.</param>
    /// <param name="extraProperty1">First optional property inlined after the texture property.</param>
    /// <param name="extraProperty2">Second optional property inlined after the extraProperty1.</param>
    /// <returns>
    ///   <para>Returns the Rect used.</para>
    /// </returns>
    public Rect TexturePropertySingleLine(GUIContent label, MaterialProperty textureProp, MaterialProperty extraProperty1)
    {
      return this.TexturePropertySingleLine(label, textureProp, extraProperty1, (MaterialProperty) null);
    }

    /// <summary>
    ///   <para>Method for showing a texture property control with additional inlined properites.</para>
    /// </summary>
    /// <param name="label">The label used for the texture property.</param>
    /// <param name="textureProp">The texture property.</param>
    /// <param name="extraProperty1">First optional property inlined after the texture property.</param>
    /// <param name="extraProperty2">Second optional property inlined after the extraProperty1.</param>
    /// <returns>
    ///   <para>Returns the Rect used.</para>
    /// </returns>
    public Rect TexturePropertySingleLine(GUIContent label, MaterialProperty textureProp, MaterialProperty extraProperty1, MaterialProperty extraProperty2)
    {
      Rect rectForSingleLine = this.GetControlRectForSingleLine();
      this.TexturePropertyMiniThumbnail(rectForSingleLine, textureProp, label.text, label.tooltip);
      if (extraProperty1 == null && extraProperty2 == null)
        return rectForSingleLine;
      if (extraProperty1 == null || extraProperty2 == null)
      {
        MaterialProperty property = extraProperty1 ?? extraProperty2;
        if (property.type == MaterialProperty.PropType.Color)
          this.ExtraPropertyAfterTexture(MaterialEditor.GetLeftAlignedFieldRect(rectForSingleLine), property);
        else
          this.ExtraPropertyAfterTexture(MaterialEditor.GetRectAfterLabelWidth(rectForSingleLine), property);
      }
      else if (extraProperty1.type == MaterialProperty.PropType.Color)
      {
        this.ExtraPropertyAfterTexture(MaterialEditor.GetFlexibleRectBetweenFieldAndRightEdge(rectForSingleLine), extraProperty2);
        this.ExtraPropertyAfterTexture(MaterialEditor.GetLeftAlignedFieldRect(rectForSingleLine), extraProperty1);
      }
      else
      {
        this.ExtraPropertyAfterTexture(MaterialEditor.GetRightAlignedFieldRect(rectForSingleLine), extraProperty2);
        this.ExtraPropertyAfterTexture(MaterialEditor.GetFlexibleRectBetweenLabelAndField(rectForSingleLine), extraProperty1);
      }
      return rectForSingleLine;
    }

    /// <summary>
    ///   <para>Method for showing a texture property control with a HDR color field and its color brightness float field.</para>
    /// </summary>
    /// <param name="label">The label used for the texture property.</param>
    /// <param name="textureProp">The texture property.</param>
    /// <param name="colorProperty">The color property (will be treated as a HDR color).</param>
    /// <param name="hdrConfig">The HDR color configuration used by the HDR Color Picker.</param>
    /// <param name="showAlpha">If false then the alpha channel information will be hidden in the GUI.</param>
    /// <returns>
    ///   <para>Return the Rect used.</para>
    /// </returns>
    public Rect TexturePropertyWithHDRColor(GUIContent label, MaterialProperty textureProp, MaterialProperty colorProperty, ColorPickerHDRConfig hdrConfig, bool showAlpha)
    {
      Rect rectForSingleLine = this.GetControlRectForSingleLine();
      this.TexturePropertyMiniThumbnail(rectForSingleLine, textureProp, label.text, label.tooltip);
      if (colorProperty.type != MaterialProperty.PropType.Color)
      {
        Debug.LogError((object) ("Assuming MaterialProperty.PropType.Color (was " + (object) colorProperty.type + ")"));
        return rectForSingleLine;
      }
      this.BeginAnimatedCheck(colorProperty);
      ColorPickerHDRConfig hdrConfig1 = hdrConfig ?? ColorPicker.defaultHDRConfig;
      Rect alignedFieldRect = MaterialEditor.GetLeftAlignedFieldRect(rectForSingleLine);
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = colorProperty.hasMixedValue;
      Color color1 = EditorGUI.ColorField(alignedFieldRect, GUIContent.none, colorProperty.colorValue, true, showAlpha, true, hdrConfig1);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        colorProperty.colorValue = color1;
      Rect fieldAndRightEdge = MaterialEditor.GetFlexibleRectBetweenFieldAndRightEdge(rectForSingleLine);
      float labelWidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = fieldAndRightEdge.width - EditorGUIUtility.fieldWidth;
      EditorGUI.BeginChangeCheck();
      Color color2 = EditorGUI.ColorBrightnessField(fieldAndRightEdge, GUIContent.Temp(" "), colorProperty.colorValue, hdrConfig1.minBrightness, hdrConfig1.maxBrightness);
      if (EditorGUI.EndChangeCheck())
        colorProperty.colorValue = color2;
      EditorGUIUtility.labelWidth = labelWidth;
      this.EndAnimatedCheck();
      return rectForSingleLine;
    }

    /// <summary>
    ///   <para>Method for showing a compact layout of properties.</para>
    /// </summary>
    /// <param name="label">The label used for the texture property.</param>
    /// <param name="textureProp">The texture property.</param>
    /// <param name="extraProperty1">First extra property inlined after the texture property.</param>
    /// <param name="label2">Label for the second extra property (on a new line and indented).</param>
    /// <param name="extraProperty2">Second property on a new line below the texture.</param>
    /// <returns>
    ///   <para>Returns the Rect used.</para>
    /// </returns>
    public Rect TexturePropertyTwoLines(GUIContent label, MaterialProperty textureProp, MaterialProperty extraProperty1, GUIContent label2, MaterialProperty extraProperty2)
    {
      if (extraProperty2 == null)
        return this.TexturePropertySingleLine(label, textureProp, extraProperty1);
      Rect rectForSingleLine1 = this.GetControlRectForSingleLine();
      this.TexturePropertyMiniThumbnail(rectForSingleLine1, textureProp, label.text, label.tooltip);
      Rect r = MaterialEditor.GetRectAfterLabelWidth(rectForSingleLine1);
      if (extraProperty1.type == MaterialProperty.PropType.Color)
        r = MaterialEditor.GetLeftAlignedFieldRect(rectForSingleLine1);
      this.ExtraPropertyAfterTexture(r, extraProperty1);
      Rect rectForSingleLine2 = this.GetControlRectForSingleLine();
      this.ShaderProperty(rectForSingleLine2, extraProperty2, label2.text, 3);
      rectForSingleLine1.height += rectForSingleLine2.height;
      return rectForSingleLine1;
    }

    private Rect GetControlRectForSingleLine()
    {
      return EditorGUILayout.GetControlRect(true, 18f, EditorStyles.layerMaskField, new GUILayoutOption[0]);
    }

    private void ExtraPropertyAfterTexture(Rect r, MaterialProperty property)
    {
      if ((property.type == MaterialProperty.PropType.Float || property.type == MaterialProperty.PropType.Color) && (double) r.width > (double) EditorGUIUtility.fieldWidth)
      {
        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = r.width - EditorGUIUtility.fieldWidth;
        this.ShaderProperty(r, property, " ");
        EditorGUIUtility.labelWidth = labelWidth;
      }
      else
        this.ShaderProperty(r, property, string.Empty);
    }

    /// <summary>
    ///   <para>Utility method for GUI layouting ShaderGUI.</para>
    /// </summary>
    /// <param name="r">Field Rect.</param>
    /// <returns>
    ///   <para>A sub rect of the input Rect.</para>
    /// </returns>
    public static Rect GetRightAlignedFieldRect(Rect r)
    {
      return new Rect(r.xMax - EditorGUIUtility.fieldWidth, r.y, EditorGUIUtility.fieldWidth, EditorGUIUtility.singleLineHeight);
    }

    /// <summary>
    ///   <para>Utility method for GUI layouting ShaderGUI.</para>
    /// </summary>
    /// <param name="r">Field Rect.</param>
    /// <returns>
    ///   <para>A sub rect of the input Rect.</para>
    /// </returns>
    public static Rect GetLeftAlignedFieldRect(Rect r)
    {
      return new Rect(r.x + EditorGUIUtility.labelWidth, r.y, EditorGUIUtility.fieldWidth, EditorGUIUtility.singleLineHeight);
    }

    /// <summary>
    ///   <para>Utility method for GUI layouting ShaderGUI.</para>
    /// </summary>
    /// <param name="r">Field Rect.</param>
    /// <returns>
    ///   <para>A sub rect of the input Rect.</para>
    /// </returns>
    public static Rect GetFlexibleRectBetweenLabelAndField(Rect r)
    {
      return new Rect(r.x + EditorGUIUtility.labelWidth, r.y, (float) ((double) r.width - (double) EditorGUIUtility.labelWidth - (double) EditorGUIUtility.fieldWidth - 5.0), EditorGUIUtility.singleLineHeight);
    }

    /// <summary>
    ///   <para>Utility method for GUI layouting ShaderGUI. Used e.g for the rect after a left aligned Color field.</para>
    /// </summary>
    /// <param name="r">Field Rect.</param>
    /// <returns>
    ///   <para>A sub rect of the input Rect.</para>
    /// </returns>
    public static Rect GetFlexibleRectBetweenFieldAndRightEdge(Rect r)
    {
      Rect rectAfterLabelWidth = MaterialEditor.GetRectAfterLabelWidth(r);
      rectAfterLabelWidth.xMin += EditorGUIUtility.fieldWidth + 5f;
      return rectAfterLabelWidth;
    }

    /// <summary>
    ///   <para>Utility method for GUI layouting ShaderGUI. This is the rect after the label which can be used for multiple properties. The input rect can be fetched by calling: EditorGUILayout.GetControlRect.</para>
    /// </summary>
    /// <param name="r">Line Rect.</param>
    /// <returns>
    ///   <para>A sub rect of the input Rect.</para>
    /// </returns>
    public static Rect GetRectAfterLabelWidth(Rect r)
    {
      return new Rect(r.x + EditorGUIUtility.labelWidth, r.y, r.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
    }

    private static class Styles
    {
      public static readonly GUIStyle kReflectionProbePickerStyle = (GUIStyle) "PaneOptions";
      public static string[] lightmapEmissiveStrings = new string[3]{ "None", "Realtime", "Baked" };
      public static int[] lightmapEmissiveValues = new int[3]{ 0, 1, 2 };
      public static string lightmapEmissiveLabel = "Global Illumination";
    }

    private enum PreviewType
    {
      Mesh,
      Plane,
      Skybox,
    }

    internal class ReflectionProbePicker : PopupWindowContent
    {
      private ReflectionProbe m_SelectedReflectionProbe;

      public Transform Target
      {
        get
        {
          if ((UnityEngine.Object) this.m_SelectedReflectionProbe != (UnityEngine.Object) null)
            return this.m_SelectedReflectionProbe.transform;
          return (Transform) null;
        }
      }

      public override Vector2 GetWindowSize()
      {
        return new Vector2(170f, 48f);
      }

      public void OnEnable()
      {
        this.m_SelectedReflectionProbe = EditorUtility.InstanceIDToObject(SessionState.GetInt("PreviewReflectionProbe", 0)) as ReflectionProbe;
      }

      public void OnDisable()
      {
        SessionState.SetInt("PreviewReflectionProbe", !(bool) ((UnityEngine.Object) this.m_SelectedReflectionProbe) ? 0 : this.m_SelectedReflectionProbe.GetInstanceID());
      }

      public override void OnGUI(Rect rc)
      {
        EditorGUILayout.LabelField("Select Reflection Probe", EditorStyles.boldLabel, new GUILayoutOption[0]);
        EditorGUILayout.Space();
        this.m_SelectedReflectionProbe = EditorGUILayout.ObjectField(string.Empty, (UnityEngine.Object) this.m_SelectedReflectionProbe, typeof (ReflectionProbe), true, new GUILayoutOption[0]) as ReflectionProbe;
      }
    }

    private class ForwardApplyMaterialModification
    {
      private readonly Renderer renderer;
      private bool isMaterialEditable;

      public ForwardApplyMaterialModification(Renderer r, bool inIsMaterialEditable)
      {
        this.renderer = r;
        this.isMaterialEditable = inIsMaterialEditable;
      }

      public bool DidModifyAnimationModeMaterialProperty(MaterialProperty property, int changedMask, object previousValue)
      {
        if (MaterialAnimationUtility.ApplyMaterialModificationToAnimationRecording(property, changedMask, this.renderer, previousValue))
          return true;
        return !this.isMaterialEditable;
      }
    }
  }
}

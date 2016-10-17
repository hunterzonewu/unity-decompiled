// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (Texture2D))]
  [CanEditMultipleObjects]
  internal class TextureInspector : Editor
  {
    private CubemapPreview m_CubemapPreview = new CubemapPreview();
    private static TextureInspector.Styles s_Styles;
    private bool m_ShowAlpha;
    protected SerializedProperty m_WrapMode;
    protected SerializedProperty m_FilterMode;
    protected SerializedProperty m_Aniso;
    [SerializeField]
    protected Vector2 m_Pos;
    [SerializeField]
    private float m_MipLevel;

    public float mipLevel
    {
      get
      {
        if (this.IsCubemap())
          return this.m_CubemapPreview.mipLevel;
        return this.m_MipLevel;
      }
      set
      {
        this.m_CubemapPreview.mipLevel = value;
        this.m_MipLevel = value;
      }
    }

    public static bool IsNormalMap(Texture t)
    {
      TextureUsageMode usageMode = TextureUtil.GetUsageMode(t);
      if (usageMode != TextureUsageMode.NormalmapPlain)
        return usageMode == TextureUsageMode.NormalmapDXT5nm;
      return true;
    }

    protected virtual void OnEnable()
    {
      this.m_WrapMode = this.serializedObject.FindProperty("m_TextureSettings.m_WrapMode");
      this.m_FilterMode = this.serializedObject.FindProperty("m_TextureSettings.m_FilterMode");
      this.m_Aniso = this.serializedObject.FindProperty("m_TextureSettings.m_Aniso");
    }

    protected virtual void OnDisable()
    {
      this.m_CubemapPreview.OnDisable();
    }

    internal void SetCubemapIntensity(float intensity)
    {
      if (this.m_CubemapPreview == null)
        return;
      this.m_CubemapPreview.SetIntensity(intensity);
    }

    public float GetMipLevelForRendering()
    {
      if (this.target == (UnityEngine.Object) null)
        return 0.0f;
      if (this.IsCubemap())
        return this.m_CubemapPreview.GetMipLevelForRendering(this.target as Texture);
      return Mathf.Min(this.m_MipLevel, (float) (TextureUtil.CountMipmaps(this.target as Texture) - 1));
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.DoWrapModePopup();
      this.DoFilterModePopup();
      this.DoAnisoLevelSlider();
      this.serializedObject.ApplyModifiedProperties();
    }

    protected void DoWrapModePopup()
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_WrapMode.hasMultipleDifferentValues;
      TextureWrapMode textureWrapMode = (TextureWrapMode) EditorGUILayout.EnumPopup(EditorGUIUtility.TempContent("Wrap Mode"), (Enum) (TextureWrapMode) this.m_WrapMode.intValue, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_WrapMode.intValue = (int) textureWrapMode;
    }

    protected void DoFilterModePopup()
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_FilterMode.hasMultipleDifferentValues;
      UnityEngine.FilterMode filterMode = (UnityEngine.FilterMode) EditorGUILayout.EnumPopup(EditorGUIUtility.TempContent("Filter Mode"), (Enum) (UnityEngine.FilterMode) this.m_FilterMode.intValue, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_FilterMode.intValue = (int) filterMode;
    }

    protected void DoAnisoLevelSlider()
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_Aniso.hasMultipleDifferentValues;
      int anisoLevel = EditorGUILayout.IntSlider("Aniso Level", this.m_Aniso.intValue, 0, 16, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        this.m_Aniso.intValue = anisoLevel;
      TextureInspector.DoAnisoGlobalSettingNote(anisoLevel);
    }

    internal static void DoAnisoGlobalSettingNote(int anisoLevel)
    {
      if (anisoLevel <= 1)
        return;
      if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable)
      {
        EditorGUILayout.HelpBox("Anisotropic filtering is disabled for all textures in Quality Settings.", MessageType.Info);
      }
      else
      {
        if (QualitySettings.anisotropicFiltering != AnisotropicFiltering.ForceEnable)
          return;
        EditorGUILayout.HelpBox("Anisotropic filtering is enabled for all textures in Quality Settings.", MessageType.Info);
      }
    }

    private bool IsCubemap()
    {
      RenderTexture target = this.target as RenderTexture;
      return (UnityEngine.Object) target != (UnityEngine.Object) null && target.isCubemap || (UnityEngine.Object) (this.target as Cubemap) != (UnityEngine.Object) null;
    }

    public override void OnPreviewSettings()
    {
      if (this.IsCubemap())
      {
        this.m_CubemapPreview.OnPreviewSettings(this.targets);
      }
      else
      {
        if (TextureInspector.s_Styles == null)
          TextureInspector.s_Styles = new TextureInspector.Styles();
        Texture target1 = this.target as Texture;
        bool flag1 = true;
        bool flag2 = false;
        bool flag3 = true;
        int a = 1;
        if (this.target is Texture2D || this.target is ProceduralTexture)
        {
          flag2 = true;
          flag3 = false;
        }
        foreach (Texture target2 in this.targets)
        {
          TextureFormat format = (TextureFormat) 0;
          bool flag4 = false;
          if (target2 is Texture2D)
          {
            format = (target2 as Texture2D).format;
            flag4 = true;
          }
          else if (target2 is ProceduralTexture)
          {
            format = (target2 as ProceduralTexture).format;
            flag4 = true;
          }
          if (flag4)
          {
            if (!TextureUtil.IsAlphaOnlyTextureFormat(format))
              flag2 = false;
            if (TextureUtil.HasAlphaTextureFormat(format) && TextureUtil.GetUsageMode(target2) == TextureUsageMode.Default)
              flag3 = true;
          }
          a = Mathf.Max(a, TextureUtil.CountMipmaps(target2));
        }
        if (flag2)
        {
          this.m_ShowAlpha = true;
          flag1 = false;
        }
        else if (!flag3)
        {
          this.m_ShowAlpha = false;
          flag1 = false;
        }
        if (flag1 && !TextureInspector.IsNormalMap(target1))
          this.m_ShowAlpha = GUILayout.Toggle(this.m_ShowAlpha, !this.m_ShowAlpha ? TextureInspector.s_Styles.RGBIcon : TextureInspector.s_Styles.alphaIcon, TextureInspector.s_Styles.previewButton, new GUILayoutOption[0]);
        GUI.enabled = a != 1;
        GUILayout.Box(TextureInspector.s_Styles.smallZoom, TextureInspector.s_Styles.previewLabel, new GUILayoutOption[0]);
        GUI.changed = false;
        this.m_MipLevel = Mathf.Round(GUILayout.HorizontalSlider(this.m_MipLevel, (float) (a - 1), 0.0f, TextureInspector.s_Styles.previewSlider, TextureInspector.s_Styles.previewSliderThumb, GUILayout.MaxWidth(64f)));
        GUILayout.Box(TextureInspector.s_Styles.largeZoom, TextureInspector.s_Styles.previewLabel, new GUILayoutOption[0]);
        GUI.enabled = true;
      }
    }

    public override bool HasPreviewGUI()
    {
      return this.target != (UnityEngine.Object) null;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (Event.current.type == EventType.Repaint)
        background.Draw(r, false, false, false, false);
      Texture target = this.target as Texture;
      RenderTexture renderTexture = target as RenderTexture;
      if ((UnityEngine.Object) renderTexture != (UnityEngine.Object) null)
      {
        if (!SystemInfo.SupportsRenderTextureFormat(renderTexture.format))
          return;
        renderTexture.Create();
      }
      if (this.IsCubemap())
      {
        this.m_CubemapPreview.OnPreviewGUI(target, r, background);
      }
      else
      {
        int num1 = Mathf.Max(target.width, 1);
        int num2 = Mathf.Max(target.height, 1);
        float levelForRendering = this.GetMipLevelForRendering();
        float num3 = Mathf.Min(Mathf.Min(r.width / (float) num1, r.height / (float) num2), 1f);
        Rect rect1 = new Rect(r.x, r.y, (float) num1 * num3, (float) num2 * num3);
        PreviewGUI.BeginScrollView(r, this.m_Pos, rect1, (GUIStyle) "PreHorizontalScrollbar", (GUIStyle) "PreHorizontalScrollbarThumb");
        float mipMapBias = target.mipMapBias;
        TextureUtil.SetMipMapBiasNoDirty(target, levelForRendering - this.Log2((float) num1 / rect1.width));
        UnityEngine.FilterMode filterMode = target.filterMode;
        TextureUtil.SetFilterModeNoDirty(target, UnityEngine.FilterMode.Point);
        if (this.m_ShowAlpha)
        {
          EditorGUI.DrawTextureAlpha(rect1, target);
        }
        else
        {
          Texture2D texture2D = target as Texture2D;
          if ((UnityEngine.Object) texture2D != (UnityEngine.Object) null && texture2D.alphaIsTransparency)
            EditorGUI.DrawTextureTransparent(rect1, target);
          else
            EditorGUI.DrawPreviewTexture(rect1, target);
        }
        if ((double) rect1.width > 32.0 && (double) rect1.height > 32.0)
        {
          TextureImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) target)) as TextureImporter;
          SpriteMetaData[] spriteMetaDataArray = !((UnityEngine.Object) atPath != (UnityEngine.Object) null) ? (SpriteMetaData[]) null : atPath.spritesheet;
          if (spriteMetaDataArray != null && atPath.spriteImportMode == SpriteImportMode.Multiple)
          {
            Rect outScreenRect = new Rect();
            Rect outSourceRect = new Rect();
            GUI.CalculateScaledTextureRects(rect1, ScaleMode.StretchToFill, (float) target.width / (float) target.height, ref outScreenRect, ref outSourceRect);
            int width = target.width;
            int height = target.height;
            atPath.GetWidthAndHeight(ref width, ref height);
            float num4 = (float) target.width / (float) width;
            HandleUtility.ApplyWireMaterial();
            GL.PushMatrix();
            GL.MultMatrix(Handles.matrix);
            GL.Begin(1);
            GL.Color(new Color(1f, 1f, 1f, 0.5f));
            foreach (SpriteMetaData spriteMetaData in spriteMetaDataArray)
            {
              Rect rect2 = spriteMetaData.rect;
              this.DrawRect(new Rect()
              {
                xMin = outScreenRect.xMin + outScreenRect.width * (rect2.xMin / (float) target.width * num4),
                xMax = outScreenRect.xMin + outScreenRect.width * (rect2.xMax / (float) target.width * num4),
                yMin = outScreenRect.yMin + outScreenRect.height * (float) (1.0 - (double) rect2.yMin / (double) target.height * (double) num4),
                yMax = outScreenRect.yMin + outScreenRect.height * (float) (1.0 - (double) rect2.yMax / (double) target.height * (double) num4)
              });
            }
            GL.End();
            GL.PopMatrix();
          }
        }
        TextureUtil.SetMipMapBiasNoDirty(target, mipMapBias);
        TextureUtil.SetFilterModeNoDirty(target, filterMode);
        this.m_Pos = PreviewGUI.EndScrollView();
        if ((double) levelForRendering == 0.0)
          return;
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y, r.width, 20f), "Mip " + (object) levelForRendering);
      }
    }

    private void DrawRect(Rect rect)
    {
      GL.Vertex(new Vector3(rect.xMin, rect.yMin, 0.0f));
      GL.Vertex(new Vector3(rect.xMax, rect.yMin, 0.0f));
      GL.Vertex(new Vector3(rect.xMax, rect.yMin, 0.0f));
      GL.Vertex(new Vector3(rect.xMax, rect.yMax, 0.0f));
      GL.Vertex(new Vector3(rect.xMax, rect.yMax, 0.0f));
      GL.Vertex(new Vector3(rect.xMin, rect.yMax, 0.0f));
      GL.Vertex(new Vector3(rect.xMin, rect.yMax, 0.0f));
      GL.Vertex(new Vector3(rect.xMin, rect.yMin, 0.0f));
    }

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return (Texture2D) null;
      Texture target1 = this.target as Texture;
      if (this.IsCubemap())
        return this.m_CubemapPreview.RenderStaticPreview(target1, width, height);
      TextureImporter atPath = AssetImporter.GetAtPath(assetPath) as TextureImporter;
      if ((UnityEngine.Object) atPath != (UnityEngine.Object) null && atPath.spriteImportMode == SpriteImportMode.Polygon)
      {
        Sprite subAsset = subAssets[0] as Sprite;
        if ((bool) ((UnityEngine.Object) subAsset))
          return SpriteInspector.BuildPreviewTexture(width, height, subAsset, (Material) null, true);
      }
      PreviewHelpers.AdjustWidthAndHeightForStaticPreview(target1.width, target1.height, ref width, ref height);
      RenderTexture active = RenderTexture.active;
      Rect rawViewportRect = ShaderUtil.rawViewportRect;
      bool flag = !TextureUtil.GetLinearSampled(target1);
      RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.Default, !flag ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB);
      Material material = EditorGUI.GetMaterialForSpecialTexture(target1);
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      if ((bool) ((UnityEngine.Object) material))
      {
        if (Unsupported.IsDeveloperBuild())
          material = new Material(material);
        Graphics.Blit(target1, temporary, material);
      }
      else
        Graphics.Blit(target1, temporary);
      GL.sRGBWrite = false;
      RenderTexture.active = temporary;
      Texture2D target2 = this.target as Texture2D;
      Texture2D texture2D = !((UnityEngine.Object) target2 != (UnityEngine.Object) null) || !target2.alphaIsTransparency ? new Texture2D(width, height, TextureFormat.RGB24, false) : new Texture2D(width, height, TextureFormat.ARGB32, false);
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) width, (float) height), 0, 0);
      texture2D.Apply();
      RenderTexture.ReleaseTemporary(temporary);
      EditorGUIUtility.SetRenderTextureNoViewport(active);
      ShaderUtil.rawViewportRect = rawViewportRect;
      if ((bool) ((UnityEngine.Object) material) && Unsupported.IsDeveloperBuild())
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) material);
      return texture2D;
    }

    private float Log2(float x)
    {
      return (float) (Math.Log((double) x) / Math.Log(2.0));
    }

    public override string GetInfoString()
    {
      Texture target1 = this.target as Texture;
      Texture2D target2 = this.target as Texture2D;
      TextureImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) target1)) as TextureImporter;
      string str1 = target1.width.ToString() + "x" + target1.height.ToString();
      if (QualitySettings.desiredColorSpace == ColorSpace.Linear)
        str1 = str1 + " " + TextureUtil.GetTextureColorSpaceString(target1);
      bool flag1 = (bool) ((UnityEngine.Object) atPath) && atPath.qualifiesForSpritePacking;
      bool flag2 = TextureInspector.IsNormalMap(target1);
      bool beCompressed = TextureUtil.DoesTextureStillNeedToBeCompressed(AssetDatabase.GetAssetPath((UnityEngine.Object) target1));
      bool flag3 = (UnityEngine.Object) target2 != (UnityEngine.Object) null && TextureUtil.IsNonPowerOfTwo(target2);
      TextureFormat textureFormat1 = TextureUtil.GetTextureFormat(target1);
      bool flag4 = !beCompressed;
      if (flag3)
        str1 += " (NPOT)";
      string str2;
      if (beCompressed)
        str2 = str1 + " (Not yet compressed)";
      else if (flag2)
      {
        TextureFormat textureFormat2 = textureFormat1;
        switch (textureFormat2)
        {
          case TextureFormat.ARGB4444:
            str2 = str1 + "  Nm 16 bit";
            break;
          case TextureFormat.ARGB32:
            str2 = str1 + "  Nm 32 bit";
            break;
          default:
            str2 = textureFormat2 == TextureFormat.DXT5 ? str1 + "  DXTnm" : str1 + "  " + TextureUtil.GetTextureFormatString(textureFormat1);
            break;
        }
      }
      else if (flag1)
      {
        TextureFormat desiredFormat;
        ColorSpace colorSpace;
        int compressionQuality;
        atPath.ReadTextureImportInstructions(EditorUserBuildSettings.activeBuildTarget, out desiredFormat, out colorSpace, out compressionQuality);
        str2 = str1 + "\n " + TextureUtil.GetTextureFormatString(textureFormat1) + "(Original) " + TextureUtil.GetTextureFormatString(desiredFormat) + "(Atlas)";
      }
      else
        str2 = str1 + "  " + TextureUtil.GetTextureFormatString(textureFormat1);
      if (flag4)
        str2 = str2 + "\n" + EditorUtility.FormatBytes(TextureUtil.GetStorageMemorySize(target1));
      if (TextureUtil.GetUsageMode(target1) == TextureUsageMode.AlwaysPadded)
      {
        int glWidth = TextureUtil.GetGLWidth(target1);
        int glHeight = TextureUtil.GetGLHeight(target1);
        if (target1.width != glWidth || target1.height != glHeight)
          str2 += string.Format("\nPadded to {0}x{1}", (object) glWidth, (object) glHeight);
      }
      return str2;
    }

    private class Styles
    {
      public GUIContent smallZoom;
      public GUIContent largeZoom;
      public GUIContent alphaIcon;
      public GUIContent RGBIcon;
      public GUIStyle previewButton;
      public GUIStyle previewSlider;
      public GUIStyle previewSliderThumb;
      public GUIStyle previewLabel;

      public Styles()
      {
        this.smallZoom = EditorGUIUtility.IconContent("PreTextureMipMapLow");
        this.largeZoom = EditorGUIUtility.IconContent("PreTextureMipMapHigh");
        this.alphaIcon = EditorGUIUtility.IconContent("PreTextureAlpha");
        this.RGBIcon = EditorGUIUtility.IconContent("PreTextureRGB");
        this.previewButton = (GUIStyle) "preButton";
        this.previewSlider = (GUIStyle) "preSlider";
        this.previewSliderThumb = (GUIStyle) "preSliderThumb";
        this.previewLabel = new GUIStyle((GUIStyle) "preLabel");
        this.previewLabel.alignment = TextAnchor.UpperCenter;
      }
    }
  }
}

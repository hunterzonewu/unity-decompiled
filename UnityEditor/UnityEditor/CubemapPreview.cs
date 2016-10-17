// Decompiled with JetBrains decompiler
// Type: UnityEditor.CubemapPreview
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class CubemapPreview
  {
    private float m_Intensity = 1f;
    public Vector2 m_PreviewDir = new Vector2(0.0f, 0.0f);
    [SerializeField]
    private CubemapPreview.PreviewType m_PreviewType;
    [SerializeField]
    private float m_MipLevel;
    private PreviewRenderUtility m_PreviewUtility;
    private Mesh m_Mesh;

    public float mipLevel
    {
      get
      {
        return this.m_MipLevel;
      }
      set
      {
        this.m_MipLevel = value;
      }
    }

    public void OnDisable()
    {
      if (this.m_PreviewUtility == null)
        return;
      this.m_PreviewUtility.Cleanup();
      this.m_PreviewUtility = (PreviewRenderUtility) null;
    }

    public float GetMipLevelForRendering(Texture texture)
    {
      return Mathf.Min(this.m_MipLevel, (float) TextureUtil.CountMipmaps(texture));
    }

    public void SetIntensity(float intensity)
    {
      this.m_Intensity = intensity;
    }

    private void InitPreview()
    {
      if (this.m_PreviewUtility != null)
        return;
      this.m_PreviewUtility = new PreviewRenderUtility()
      {
        m_CameraFieldOfView = 15f
      };
      this.m_Mesh = PreviewRenderUtility.GetPreviewSphere();
    }

    public void OnPreviewSettings(Object[] targets)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return;
      GUI.enabled = true;
      this.InitPreview();
      bool flag1 = true;
      bool flag2 = true;
      bool flag3 = false;
      int a = 8;
      foreach (Texture target in targets)
      {
        a = Mathf.Max(a, TextureUtil.CountMipmaps(target));
        Cubemap cubemap = target as Cubemap;
        if ((bool) ((Object) cubemap))
        {
          TextureFormat format = cubemap.format;
          if (!TextureUtil.IsAlphaOnlyTextureFormat(format))
            flag2 = false;
          if (TextureUtil.HasAlphaTextureFormat(format) && TextureUtil.GetUsageMode(target) == TextureUsageMode.Default)
            flag3 = true;
        }
        else
        {
          flag3 = true;
          flag2 = false;
        }
      }
      if (flag2)
      {
        this.m_PreviewType = CubemapPreview.PreviewType.Alpha;
        flag1 = false;
      }
      else if (!flag3)
      {
        this.m_PreviewType = CubemapPreview.PreviewType.RGB;
        flag1 = false;
      }
      if (flag1)
      {
        GUIContent[] guiContentArray = new GUIContent[2]{ CubemapPreview.Styles.RGBIcon, CubemapPreview.Styles.alphaIcon };
        int previewType = (int) this.m_PreviewType;
        if (GUILayout.Button(guiContentArray[previewType], CubemapPreview.Styles.preButton, new GUILayoutOption[0]))
        {
          int num;
          this.m_PreviewType = (CubemapPreview.PreviewType) ((num = previewType + 1) % guiContentArray.Length);
        }
      }
      GUI.enabled = a != 1;
      GUILayout.Box(CubemapPreview.Styles.smallZoom, CubemapPreview.Styles.preLabel, new GUILayoutOption[0]);
      GUI.changed = false;
      this.m_MipLevel = Mathf.Round(GUILayout.HorizontalSlider(this.m_MipLevel, (float) (a - 1), 0.0f, CubemapPreview.Styles.preSlider, CubemapPreview.Styles.preSliderThumb, GUILayout.MaxWidth(64f)));
      GUILayout.Box(CubemapPreview.Styles.largeZoom, CubemapPreview.Styles.preLabel, new GUILayoutOption[0]);
      GUI.enabled = true;
    }

    public void OnPreviewGUI(Texture t, Rect r, GUIStyle background)
    {
      if ((Object) t == (Object) null)
        return;
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y, r.width, 40f), "Cubemap preview requires\nrender texture support");
      }
      else
      {
        this.m_PreviewDir = PreviewGUI.Drag2D(this.m_PreviewDir, r);
        if (Event.current.type != EventType.Repaint)
          return;
        this.InitPreview();
        this.m_PreviewUtility.BeginPreview(r, background);
        this.RenderCubemap(t, this.m_PreviewDir, 6f);
        Texture image = this.m_PreviewUtility.EndPreview();
        GUI.DrawTexture(r, image, ScaleMode.StretchToFill, false);
        if ((double) this.mipLevel == 0.0)
          return;
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y, r.width, 20f), "Mip " + (object) this.mipLevel);
      }
    }

    public Texture2D RenderStaticPreview(Texture t, int width, int height)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return (Texture2D) null;
      this.InitPreview();
      this.m_PreviewUtility.BeginStaticPreview(new Rect(0.0f, 0.0f, (float) width, (float) height));
      Vector2 previewDir = new Vector2(0.0f, 0.0f);
      this.RenderCubemap(t, previewDir, 5.3f);
      return this.m_PreviewUtility.EndStaticPreview();
    }

    private void RenderCubemap(Texture t, Vector2 previewDir, float previewDistance)
    {
      bool fog = RenderSettings.fog;
      Unsupported.SetRenderSettingsUseFogNoDirty(false);
      this.m_PreviewUtility.m_Camera.transform.position = -Vector3.forward * previewDistance;
      this.m_PreviewUtility.m_Camera.transform.rotation = Quaternion.identity;
      Quaternion quaternion = Quaternion.Euler(previewDir.y, 0.0f, 0.0f) * Quaternion.Euler(0.0f, previewDir.x, 0.0f);
      Material mat = EditorGUIUtility.LoadRequired("Previews/PreviewCubemapMaterial.mat") as Material;
      mat.mainTexture = t;
      mat.SetMatrix("_CubemapRotation", Matrix4x4.TRS(Vector3.zero, quaternion, Vector3.one));
      float levelForRendering = this.GetMipLevelForRendering(t);
      mat.SetFloat("_Mip", levelForRendering);
      mat.SetFloat("_Alpha", this.m_PreviewType != CubemapPreview.PreviewType.Alpha ? 0.0f : 1f);
      mat.SetFloat("_Intensity", this.m_Intensity);
      this.m_PreviewUtility.DrawMesh(this.m_Mesh, Vector3.zero, quaternion, mat, 0);
      this.m_PreviewUtility.m_Camera.Render();
      Unsupported.SetRenderSettingsUseFogNoDirty(fog);
    }

    private enum PreviewType
    {
      RGB,
      Alpha,
    }

    private static class Styles
    {
      public static GUIStyle preButton = (GUIStyle) "preButton";
      public static GUIStyle preSlider = (GUIStyle) "preSlider";
      public static GUIStyle preSliderThumb = (GUIStyle) "preSliderThumb";
      public static GUIStyle preLabel = (GUIStyle) "preLabel";
      public static GUIContent smallZoom = EditorGUIUtility.IconContent("PreTextureMipMapLow");
      public static GUIContent largeZoom = EditorGUIUtility.IconContent("PreTextureMipMapHigh");
      public static GUIContent alphaIcon = EditorGUIUtility.IconContent("PreTextureAlpha");
      public static GUIContent RGBIcon = EditorGUIUtility.IconContent("PreTextureRGB");
    }
  }
}

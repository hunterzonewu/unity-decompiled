// Decompiled with JetBrains decompiler
// Type: UnityEditor.Texture3DInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Texture3D))]
  internal class Texture3DInspector : TextureInspector
  {
    public Vector2 m_PreviewDir = new Vector2(0.0f, 0.0f);
    private PreviewRenderUtility m_PreviewUtility;
    private Material m_Material;
    private Mesh m_Mesh;

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
    }

    public override string GetInfoString()
    {
      Texture3D target = this.target as Texture3D;
      return string.Format("{0}x{1}x{2} {3} {4}", (object) target.width, (object) target.height, (object) target.depth, (object) TextureUtil.GetTextureFormatString(target.format), (object) EditorUtility.FormatBytes(TextureUtil.GetRuntimeMemorySize((Texture) target)));
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      if (this.m_PreviewUtility == null)
        return;
      this.m_PreviewUtility.Cleanup();
      this.m_PreviewUtility = (PreviewRenderUtility) null;
    }

    public override void OnPreviewSettings()
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture || !SystemInfo.supports3DTextures)
        return;
      GUI.enabled = true;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture || !SystemInfo.supports3DTextures)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y, r.width, 40f), "3D texture preview not supported");
      }
      else
      {
        this.m_PreviewDir = PreviewGUI.Drag2D(this.m_PreviewDir, r);
        if (Event.current.type != EventType.Repaint)
          return;
        this.InitPreview();
        this.m_Material.mainTexture = this.target as Texture;
        this.m_PreviewUtility.BeginPreview(r, background);
        bool fog = RenderSettings.fog;
        Unsupported.SetRenderSettingsUseFogNoDirty(false);
        this.m_PreviewUtility.m_Camera.transform.position = -Vector3.forward * 3f;
        this.m_PreviewUtility.m_Camera.transform.rotation = Quaternion.identity;
        this.m_PreviewUtility.DrawMesh(this.m_Mesh, Vector3.zero, Quaternion.Euler(this.m_PreviewDir.y, 0.0f, 0.0f) * Quaternion.Euler(0.0f, this.m_PreviewDir.x, 0.0f), this.m_Material, 0);
        this.m_PreviewUtility.m_Camera.Render();
        Unsupported.SetRenderSettingsUseFogNoDirty(fog);
        this.m_PreviewUtility.EndAndDrawPreview(r);
      }
    }

    private void InitPreview()
    {
      if (this.m_PreviewUtility == null)
      {
        this.m_PreviewUtility = new PreviewRenderUtility();
        this.m_PreviewUtility.m_CameraFieldOfView = 30f;
      }
      if ((Object) this.m_Mesh == (Object) null)
        this.m_Mesh = PreviewRenderUtility.GetPreviewSphere();
      if (!((Object) this.m_Material == (Object) null))
        return;
      this.m_Material = EditorGUIUtility.LoadRequired("Previews/Preview3DTextureMaterial.mat") as Material;
    }
  }
}

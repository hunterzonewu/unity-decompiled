// Decompiled with JetBrains decompiler
// Type: UnityEditor.PreviewRenderUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  public class PreviewRenderUtility
  {
    public float m_CameraFieldOfView = 15f;
    public Light[] m_Light = new Light[2];
    public Camera m_Camera;
    internal RenderTexture m_RenderTexture;
    private Rect m_TargetRect;
    private SavedRenderTargetState m_SavedState;

    public PreviewRenderUtility()
      : this(false)
    {
    }

    public PreviewRenderUtility(bool renderFullScene)
    {
      this.m_Camera = EditorUtility.CreateGameObjectWithHideFlags("PreRenderCamera", HideFlags.HideAndDontSave, typeof (Camera)).GetComponent<Camera>();
      this.m_Camera.cameraType = CameraType.Preview;
      this.m_Camera.fieldOfView = this.m_CameraFieldOfView;
      this.m_Camera.enabled = false;
      this.m_Camera.clearFlags = CameraClearFlags.Depth;
      this.m_Camera.farClipPlane = 10f;
      this.m_Camera.nearClipPlane = 2f;
      this.m_Camera.backgroundColor = new Color(0.1921569f, 0.1921569f, 0.1921569f, 1f);
      this.m_Camera.renderingPath = RenderingPath.Forward;
      this.m_Camera.useOcclusionCulling = false;
      if (!renderFullScene)
        Handles.SetCameraOnlyDrawMesh(this.m_Camera);
      for (int index = 0; index < 2; ++index)
      {
        GameObject objectWithHideFlags = EditorUtility.CreateGameObjectWithHideFlags("PreRenderLight", HideFlags.HideAndDontSave, typeof (Light));
        this.m_Light[index] = objectWithHideFlags.GetComponent<Light>();
        this.m_Light[index].type = LightType.Directional;
        this.m_Light[index].intensity = 1f;
        this.m_Light[index].enabled = false;
      }
      this.m_Light[0].color = SceneView.kSceneViewFrontLight;
      this.m_Light[1].transform.rotation = Quaternion.Euler(340f, 218f, 177f);
      this.m_Light[1].color = new Color(0.4f, 0.4f, 0.45f, 0.0f) * 0.7f;
    }

    public void Cleanup()
    {
      if ((bool) ((UnityEngine.Object) this.m_Camera))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Camera.gameObject, true);
      if ((bool) ((UnityEngine.Object) this.m_RenderTexture))
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_RenderTexture);
        this.m_RenderTexture = (RenderTexture) null;
      }
      foreach (Light light in this.m_Light)
      {
        if ((bool) ((UnityEngine.Object) light))
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) light.gameObject, true);
      }
    }

    private void InitPreview(Rect r)
    {
      this.m_TargetRect = r;
      int width = (int) r.width;
      int height = (int) r.height;
      if (!(bool) ((UnityEngine.Object) this.m_RenderTexture) || this.m_RenderTexture.width != width || this.m_RenderTexture.height != height)
      {
        if ((bool) ((UnityEngine.Object) this.m_RenderTexture))
        {
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_RenderTexture);
          this.m_RenderTexture = (RenderTexture) null;
        }
        float scaleFactor = this.GetScaleFactor((float) width, (float) height);
        this.m_RenderTexture = new RenderTexture((int) ((double) width * (double) scaleFactor), (int) ((double) height * (double) scaleFactor), 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        this.m_RenderTexture.hideFlags = HideFlags.HideAndDontSave;
        this.m_Camera.targetTexture = this.m_RenderTexture;
      }
      this.m_Camera.fieldOfView = (float) ((double) Mathf.Atan((this.m_RenderTexture.width > 0 ? Mathf.Max(1f, (float) this.m_RenderTexture.height / (float) this.m_RenderTexture.width) : 1f) * Mathf.Tan((float) ((double) this.m_CameraFieldOfView * 0.5 * (Math.PI / 180.0)))) * 57.2957801818848 * 2.0);
      this.m_SavedState = new SavedRenderTargetState();
      EditorGUIUtility.SetRenderTextureNoViewport(this.m_RenderTexture);
      GL.LoadOrtho();
      GL.LoadPixelMatrix(0.0f, (float) this.m_RenderTexture.width, (float) this.m_RenderTexture.height, 0.0f);
      ShaderUtil.rawViewportRect = new Rect(0.0f, 0.0f, (float) this.m_RenderTexture.width, (float) this.m_RenderTexture.height);
      ShaderUtil.rawScissorRect = new Rect(0.0f, 0.0f, (float) this.m_RenderTexture.width, (float) this.m_RenderTexture.height);
      GL.Clear(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
    }

    public float GetScaleFactor(float width, float height)
    {
      return Mathf.Min(Mathf.Max(Mathf.Min(width * 2f, 1024f), width) / width, Mathf.Max(Mathf.Min(height * 2f, 1024f), height) / height) * EditorGUIUtility.pixelsPerPoint;
    }

    public void BeginStaticPreview(Rect r)
    {
      this.InitPreview(r);
      Color color = new Color(0.3215686f, 0.3215686f, 0.3215686f, 1f);
      Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, true, true);
      texture2D.SetPixel(0, 0, color);
      texture2D.Apply();
      Graphics.DrawTexture(new Rect(0.0f, 0.0f, (float) this.m_RenderTexture.width, (float) this.m_RenderTexture.height), (Texture) texture2D);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) texture2D);
    }

    public void BeginPreview(Rect r, GUIStyle previewBackground)
    {
      this.InitPreview(r);
      if (previewBackground == null || previewBackground == GUIStyle.none)
        return;
      Graphics.DrawTexture(previewBackground.overflow.Add(new Rect(0.0f, 0.0f, (float) this.m_RenderTexture.width, (float) this.m_RenderTexture.height)), (Texture) previewBackground.normal.background, new Rect(0.0f, 0.0f, 1f, 1f), previewBackground.border.left, previewBackground.border.right, previewBackground.border.top, previewBackground.border.bottom, new Color(0.5f, 0.5f, 0.5f, 1f), (Material) null);
    }

    public Texture EndPreview()
    {
      this.m_SavedState.Restore();
      return (Texture) this.m_RenderTexture;
    }

    public void EndAndDrawPreview(Rect r)
    {
      Texture image = this.EndPreview();
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      GUI.DrawTexture(r, image, ScaleMode.StretchToFill, false);
      GL.sRGBWrite = false;
    }

    public Texture2D EndStaticPreview()
    {
      RenderTexture temporary = RenderTexture.GetTemporary((int) this.m_TargetRect.width, (int) this.m_TargetRect.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      Graphics.Blit((Texture) this.m_RenderTexture, temporary);
      GL.sRGBWrite = false;
      RenderTexture.active = temporary;
      Texture2D texture2D = new Texture2D((int) this.m_TargetRect.width, (int) this.m_TargetRect.height, TextureFormat.RGB24, false, true);
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, this.m_TargetRect.width, this.m_TargetRect.height), 0, 0);
      texture2D.Apply();
      RenderTexture.ReleaseTemporary(temporary);
      this.m_SavedState.Restore();
      return texture2D;
    }

    public void DrawMesh(Mesh mesh, Vector3 pos, Quaternion rot, Material mat, int subMeshIndex)
    {
      this.DrawMesh(mesh, pos, rot, mat, subMeshIndex, (MaterialPropertyBlock) null);
    }

    public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material mat, int subMeshIndex)
    {
      this.DrawMesh(mesh, matrix, mat, subMeshIndex, (MaterialPropertyBlock) null);
    }

    public void DrawMesh(Mesh mesh, Vector3 pos, Quaternion rot, Material mat, int subMeshIndex, MaterialPropertyBlock customProperties)
    {
      Graphics.DrawMesh(mesh, pos, rot, mat, 1, this.m_Camera, subMeshIndex, customProperties);
    }

    public void DrawMesh(Mesh mesh, Vector3 pos, Quaternion rot, Material mat, int subMeshIndex, MaterialPropertyBlock customProperties, Transform probeAnchor)
    {
      Graphics.DrawMesh(mesh, pos, rot, mat, 1, this.m_Camera, subMeshIndex, customProperties, ShadowCastingMode.Off, false, probeAnchor);
    }

    public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material mat, int subMeshIndex, MaterialPropertyBlock customProperties)
    {
      Graphics.DrawMesh(mesh, matrix, mat, 1, this.m_Camera, subMeshIndex, customProperties);
    }

    internal static Mesh GetPreviewSphere()
    {
      GameObject gameObject = (GameObject) EditorGUIUtility.LoadRequired("Previews/PreviewMaterials.fbx");
      gameObject.SetActive(false);
      foreach (Transform transform in gameObject.transform)
      {
        if (transform.name == "sphere")
          return transform.GetComponent<MeshFilter>().sharedMesh;
      }
      return (Mesh) null;
    }
  }
}

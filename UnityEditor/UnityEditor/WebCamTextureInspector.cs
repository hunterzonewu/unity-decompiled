// Decompiled with JetBrains decompiler
// Type: UnityEditor.WebCamTextureInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (WebCamTexture))]
  internal class WebCamTextureInspector : Editor
  {
    private static GUIContent[] s_PlayIcons = new GUIContent[2];
    private Vector2 m_Pos;

    public override void OnInspectorGUI()
    {
      WebCamTexture target = this.target as WebCamTexture;
      EditorGUILayout.LabelField("Requested FPS", target.requestedFPS.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Requested Width", target.requestedWidth.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Requested Height", target.requestedHeight.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Device Name", target.deviceName, new GUILayoutOption[0]);
    }

    private static void Init()
    {
      WebCamTextureInspector.s_PlayIcons[0] = EditorGUIUtility.IconContent("preAudioPlayOff");
      WebCamTextureInspector.s_PlayIcons[1] = EditorGUIUtility.IconContent("preAudioPlayOn");
    }

    public override bool HasPreviewGUI()
    {
      return this.target != (Object) null;
    }

    public override void OnPreviewSettings()
    {
      WebCamTextureInspector.Init();
      GUI.enabled = !Application.isPlaying;
      WebCamTexture target = this.target as WebCamTexture;
      bool flag = PreviewGUI.CycleButton(!target.isPlaying ? 0 : 1, WebCamTextureInspector.s_PlayIcons) != 0;
      if (flag != target.isPlaying)
      {
        if (flag)
        {
          target.Stop();
          target.Play();
        }
        else
          target.Pause();
      }
      GUI.enabled = true;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (Event.current.type == EventType.Repaint)
        background.Draw(r, false, false, false, false);
      WebCamTexture target = this.target as WebCamTexture;
      float num = Mathf.Min(Mathf.Min(r.width / (float) target.width, r.height / (float) target.height), 1f);
      Rect rect = new Rect(r.x, r.y, (float) target.width * num, (float) target.height * num);
      PreviewGUI.BeginScrollView(r, this.m_Pos, rect, (GUIStyle) "PreHorizontalScrollbar", (GUIStyle) "PreHorizontalScrollbarThumb");
      GUI.DrawTexture(rect, (Texture) target, ScaleMode.StretchToFill, false);
      this.m_Pos = PreviewGUI.EndScrollView();
      if (target.isPlaying)
        GUIView.current.Repaint();
      if (!Application.isPlaying)
        return;
      if (target.isPlaying)
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y + 10f, r.width, 20f), "Can't pause preview when in play mode");
      else
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y + 10f, r.width, 20f), "Can't start preview when in play mode");
    }

    public void OnDisable()
    {
      WebCamTexture target = this.target as WebCamTexture;
      if (Application.isPlaying || !((Object) target != (Object) null))
        return;
      target.Stop();
    }

    public override string GetInfoString()
    {
      Texture target = this.target as Texture;
      return target.width.ToString() + "x" + target.height.ToString() + "  " + TextureUtil.GetTextureFormatString(TextureUtil.GetTextureFormat(target));
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.MovieTextureInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (MovieTexture))]
  internal class MovieTextureInspector : TextureInspector
  {
    private static GUIContent[] s_PlayIcons = new GUIContent[2];

    private static void Init()
    {
      MovieTextureInspector.s_PlayIcons[0] = EditorGUIUtility.IconContent("preAudioPlayOff");
      MovieTextureInspector.s_PlayIcons[1] = EditorGUIUtility.IconContent("preAudioPlayOn");
    }

    protected override void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
    }

    public override void OnPreviewSettings()
    {
      MovieTextureInspector.Init();
      EditorGUI.BeginDisabledGroup(Application.isPlaying || this.targets.Length > 1);
      MovieTexture target = this.target as MovieTexture;
      AudioClip audioClip = target.audioClip;
      bool flag = PreviewGUI.CycleButton(!target.isPlaying ? 0 : 1, MovieTextureInspector.s_PlayIcons) != 0;
      if (flag != target.isPlaying)
      {
        if (flag)
        {
          target.Stop();
          target.Play();
          if ((Object) audioClip != (Object) null)
            AudioUtil.PlayClip(audioClip);
        }
        else
        {
          target.Pause();
          if ((Object) audioClip != (Object) null)
            AudioUtil.PauseClip(audioClip);
        }
      }
      EditorGUI.EndDisabledGroup();
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (Event.current.type == EventType.Repaint)
        background.Draw(r, false, false, false, false);
      MovieTexture target = this.target as MovieTexture;
      float num = Mathf.Min(Mathf.Min(r.width / (float) target.width, r.height / (float) target.height), 1f);
      Rect rect = new Rect(r.x, r.y, (float) target.width * num, (float) target.height * num);
      PreviewGUI.BeginScrollView(r, this.m_Pos, rect, (GUIStyle) "PreHorizontalScrollbar", (GUIStyle) "PreHorizontalScrollbarThumb");
      EditorGUI.DrawPreviewTexture(rect, (Texture) target, (Material) null, ScaleMode.StretchToFill);
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

    protected override void OnDisable()
    {
      base.OnDisable();
      MovieTexture target = this.target as MovieTexture;
      if (Application.isPlaying || !((Object) target != (Object) null))
        return;
      AudioClip audioClip = target.audioClip;
      target.Stop();
      if (!((Object) audioClip != (Object) null))
        return;
      AudioUtil.StopClip(audioClip);
    }

    public override string GetInfoString()
    {
      string infoString = base.GetInfoString();
      if (!(this.target as MovieTexture).isReadyToPlay)
        infoString += "/nNot ready to play yet.";
      return infoString;
    }
  }
}

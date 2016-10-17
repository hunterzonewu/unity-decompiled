// Decompiled with JetBrains decompiler
// Type: UnityEditor.MovieImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (MovieImporter))]
  internal class MovieImporterInspector : AssetImporterInspector
  {
    private static GUIContent linearTextureContent = EditorGUIUtility.TextContent("Bypass sRGB Sampling|Texture will not be converted from gamma space to linear when sampled. Enable for IMGUI textures and non-color textures.");
    private float m_quality;
    private float m_duration;
    private bool m_linearTexture;

    internal override bool showImportedObject
    {
      get
      {
        return false;
      }
    }

    internal override bool HasModified()
    {
      MovieImporter target = this.target as MovieImporter;
      if ((double) target.quality == (double) this.m_quality)
        return target.linearTexture != this.m_linearTexture;
      return true;
    }

    internal override void ResetValues()
    {
      MovieImporter target = this.target as MovieImporter;
      this.m_quality = target.quality;
      this.m_linearTexture = target.linearTexture;
      this.m_duration = target.duration;
    }

    internal override void Apply()
    {
      MovieImporter target = this.target as MovieImporter;
      target.quality = this.m_quality;
      target.linearTexture = this.m_linearTexture;
    }

    public override void OnInspectorGUI()
    {
      if ((Object) (this.target as MovieImporter) != (Object) null)
      {
        GUILayout.BeginVertical();
        this.m_linearTexture = EditorGUILayout.Toggle(MovieImporterInspector.linearTextureContent, this.m_linearTexture, new GUILayoutOption[0]);
        int num1 = (int) (this.GetVideoBitrateForQuality((double) this.m_quality) + this.GetAudioBitrateForQuality((double) this.m_quality));
        float num2 = (float) (num1 / 8) * this.m_duration;
        float num3 = 1048576f;
        this.m_quality = EditorGUILayout.Slider("Quality", this.m_quality, 0.0f, 1f, new GUILayoutOption[0]);
        GUILayout.Label(string.Format("Approx. {0:0.00} " + ((double) num2 >= (double) num3 ? "MB" : "kB") + ", {1} kbps", (object) (float) ((double) num2 / ((double) num2 >= (double) num3 ? (double) num3 : 1024.0)), (object) (num1 / 1000)), EditorStyles.helpBox, new GUILayoutOption[0]);
        GUILayout.EndVertical();
      }
      this.ApplyRevertGUI();
      MovieTexture target = this.assetEditor.target as MovieTexture;
      if (!(bool) ((Object) target) || !target.loop)
        return;
      EditorGUILayout.Space();
      target.loop = EditorGUILayout.Toggle("Loop", target.loop, new GUILayoutOption[0]);
      GUILayout.Label("The Loop setting in the Inspector is obsolete. Use the Scripting API to control looping instead.\n\nThe loop setting will be disabled on next re-import or by disabling it above.", EditorStyles.helpBox, new GUILayoutOption[0]);
    }

    private double GetAudioBitrateForQuality(double f)
    {
      return 56000.0 + 200000.0 * f;
    }

    private double GetVideoBitrateForQuality(double f)
    {
      return 100000.0 + 8000000.0 * f;
    }

    private double GetAudioQualityForBitrate(double f)
    {
      return (f - 56000.0) / 200000.0;
    }

    private double GetVideoQualityForBitrate(double f)
    {
      return (f - 100000.0) / 8000000.0;
    }
  }
}

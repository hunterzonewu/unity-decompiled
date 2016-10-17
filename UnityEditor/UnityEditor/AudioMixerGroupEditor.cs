// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerGroupEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Audio;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AudioMixerGroupController))]
  internal class AudioMixerGroupEditor : Editor
  {
    public static readonly string kPrefKeyForShowCpuUsage = "AudioMixerShowCPU";
    private readonly TickTimerHelper m_Ticker = new TickTimerHelper(0.05);
    private AudioMixerEffectView m_EffectView;

    private void OnEnable()
    {
      EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
    }

    private void OnDisable()
    {
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
    }

    public void Update()
    {
      if (!EditorApplication.isPlaying || !this.m_Ticker.DoTick())
        return;
      this.Repaint();
    }

    public override void OnInspectorGUI()
    {
      AudioMixerDrawUtils.InitStyles();
      if (this.m_EffectView == null)
        this.m_EffectView = new AudioMixerEffectView();
      this.m_EffectView.OnGUI(this.target as AudioMixerGroupController);
    }

    public override bool UseDefaultMargins()
    {
      return false;
    }

    internal override void DrawHeaderHelpAndSettingsGUI(Rect r)
    {
      if (this.m_EffectView == null)
        return;
      AudioMixerGroupController target = this.target as AudioMixerGroupController;
      base.DrawHeaderHelpAndSettingsGUI(r);
      GUI.Label(new Rect(r.x + 44f, r.yMax - 20f, r.width - 50f, 15f), GUIContent.Temp(target.controller.name), EditorStyles.miniLabel);
    }

    [MenuItem("CONTEXT/AudioMixerGroupController/Copy all effect settings to all snapshots")]
    private static void CopyAllEffectToSnapshots(MenuCommand command)
    {
      AudioMixerGroupController context = command.context as AudioMixerGroupController;
      AudioMixerController controller = context.controller;
      if ((Object) controller == (Object) null)
        return;
      Undo.RecordObject((Object) controller, "Copy all effect settings to all snapshots");
      controller.CopyAllSettingsToAllSnapshots(context, controller.TargetSnapshot);
    }

    [MenuItem("CONTEXT/AudioMixerGroupController/Toggle CPU usage display (only available on first editor instance)")]
    private static void ShowCPUUsage(MenuCommand command)
    {
      bool flag = EditorPrefs.GetBool(AudioMixerGroupEditor.kPrefKeyForShowCpuUsage, false);
      EditorPrefs.SetBool(AudioMixerGroupEditor.kPrefKeyForShowCpuUsage, !flag);
    }
  }
}

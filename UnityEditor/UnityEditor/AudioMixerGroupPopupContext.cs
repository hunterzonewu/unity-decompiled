// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerGroupPopupContext
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Audio;

namespace UnityEditor
{
  internal class AudioMixerGroupPopupContext
  {
    public AudioMixerController controller;
    public AudioMixerGroupController[] groups;

    public AudioMixerGroupPopupContext(AudioMixerController controller, AudioMixerGroupController group)
    {
      this.controller = controller;
      this.groups = new AudioMixerGroupController[1]
      {
        group
      };
    }

    public AudioMixerGroupPopupContext(AudioMixerController controller, AudioMixerGroupController[] groups)
    {
      this.controller = controller;
      this.groups = groups;
    }
  }
}

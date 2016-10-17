// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor.Audio;

namespace UnityEditor
{
  internal class AudioMixerItem : TreeViewItem
  {
    private const string kSuspendedText = " - Inactive";

    public AudioMixerController mixer { get; set; }

    public string infoText { get; set; }

    public float labelWidth { get; set; }

    private bool lastSuspendedState { get; set; }

    public AudioMixerItem(int id, int depth, TreeViewItem parent, string displayName, AudioMixerController mixer, string infoText)
      : base(id, depth, parent, displayName)
    {
      this.mixer = mixer;
      this.infoText = infoText;
      this.UpdateSuspendedString(true);
    }

    public void UpdateSuspendedString(bool force)
    {
      bool isSuspended = this.mixer.isSuspended;
      if (this.lastSuspendedState == isSuspended && !force)
        return;
      this.lastSuspendedState = isSuspended;
      if (isSuspended)
        this.AddSuspendedText();
      else
        this.RemoveSuspendedText();
    }

    private void RemoveSuspendedText()
    {
      int startIndex = this.infoText.IndexOf(" - Inactive", StringComparison.Ordinal);
      if (startIndex < 0)
        return;
      this.infoText = this.infoText.Remove(startIndex, " - Inactive".Length);
    }

    private void AddSuspendedText()
    {
      if (this.infoText.IndexOf(" - Inactive", StringComparison.Ordinal) >= 0)
        return;
      this.infoText += " - Inactive";
    }
  }
}

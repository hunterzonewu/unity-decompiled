// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEditor.Audio;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioMixerSelection
  {
    private AudioMixerController m_Controller;

    public List<AudioMixerGroupController> ChannelStripSelection { get; private set; }

    public AudioMixerSelection(AudioMixerController controller)
    {
      this.m_Controller = controller;
      this.ChannelStripSelection = new List<AudioMixerGroupController>();
      this.SyncToUnitySelection();
    }

    public void SyncToUnitySelection()
    {
      if (!((Object) this.m_Controller != (Object) null))
        return;
      this.RefreshCachedChannelStripSelection();
    }

    public void SetChannelStrips(List<AudioMixerGroupController> newSelection)
    {
      Selection.objects = (Object[]) newSelection.ToArray();
    }

    public void SetSingleChannelStrip(AudioMixerGroupController group)
    {
      Selection.objects = (Object[]) new AudioMixerGroupController[1]
      {
        group
      };
    }

    public void ToggleChannelStrip(AudioMixerGroupController group)
    {
      List<Object> objectList = new List<Object>((IEnumerable<Object>) Selection.objects);
      if (objectList.Contains((Object) group))
        objectList.Remove((Object) group);
      else
        objectList.Add((Object) group);
      Selection.objects = objectList.ToArray();
    }

    public void ClearChannelStrips()
    {
      Selection.objects = new Object[0];
    }

    public bool HasSingleChannelStripSelection()
    {
      return this.ChannelStripSelection.Count == 1;
    }

    private void RefreshCachedChannelStripSelection()
    {
      Object[] filtered = Selection.GetFiltered(typeof (AudioMixerGroupController), SelectionMode.Deep);
      this.ChannelStripSelection = new List<AudioMixerGroupController>();
      using (List<AudioMixerGroupController>.Enumerator enumerator = this.m_Controller.GetAllAudioGroupsSlow().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          if (((IEnumerable<Object>) filtered).Contains<Object>((Object) current))
            this.ChannelStripSelection.Add(current);
        }
      }
    }

    public void Sanitize()
    {
      this.RefreshCachedChannelStripSelection();
    }
  }
}

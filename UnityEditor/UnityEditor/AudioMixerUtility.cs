// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.Audio;

namespace UnityEditor
{
  internal class AudioMixerUtility
  {
    public static void RepaintAudioMixerAndInspectors()
    {
      InspectorWindow.RepaintAllInspectors();
      AudioMixerWindow.RepaintAudioMixerWindow();
    }

    public static void VisitGroupsRecursivly(AudioMixerGroupController group, System.Action<AudioMixerGroupController> visitorCallback)
    {
      foreach (AudioMixerGroupController child in group.children)
        AudioMixerUtility.VisitGroupsRecursivly(child, visitorCallback);
      if (visitorCallback == null)
        return;
      visitorCallback(group);
    }

    public class VisitorFetchInstanceIDs
    {
      public List<int> instanceIDs = new List<int>();

      public void Visitor(AudioMixerGroupController group)
      {
        this.instanceIDs.Add(group.GetInstanceID());
      }
    }
  }
}

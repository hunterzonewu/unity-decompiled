// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerTreeViewNode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Audio;

namespace UnityEditor
{
  internal class AudioMixerTreeViewNode : TreeViewItem
  {
    public AudioMixerGroupController group { get; set; }

    public AudioMixerTreeViewNode(int instanceID, int depth, TreeViewItem parent, string displayName, AudioMixerGroupController group)
      : base(instanceID, depth, parent, displayName)
    {
      this.group = group;
    }
  }
}

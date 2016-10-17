// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.MixerGroupControllerCompareByName
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;

namespace UnityEditor.Audio
{
  internal class MixerGroupControllerCompareByName : IComparer<AudioMixerGroupController>
  {
    public int Compare(AudioMixerGroupController x, AudioMixerGroupController y)
    {
      return StringComparer.InvariantCultureIgnoreCase.Compare(x.GetDisplayString(), y.GetDisplayString());
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.AudioMixerGroupController
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityEditor.Audio
{
  internal sealed class AudioMixerGroupController : AudioMixerGroup
  {
    public GUID groupID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int userColorIndex { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public AudioMixerController controller { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public AudioMixerGroupController[] children { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public AudioMixerEffectController[] effects { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public bool mute { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public bool solo { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public bool bypassEffects { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public AudioMixerGroupController(AudioMixer owner)
    {
      AudioMixerGroupController.Internal_CreateAudioMixerGroupController(this, owner);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateAudioMixerGroupController(AudioMixerGroupController mono, AudioMixer owner);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void PreallocateGUIDs();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public GUID GetGUIDForVolume();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float GetValueForVolume(AudioMixerController controller, AudioMixerSnapshotController snapshot);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetValueForVolume(AudioMixerController controller, AudioMixerSnapshotController snapshot, float value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public GUID GetGUIDForPitch();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float GetValueForPitch(AudioMixerController controller, AudioMixerSnapshotController snapshot);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetValueForPitch(AudioMixerController controller, AudioMixerSnapshotController snapshot, float value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool HasDependentMixers();

    public void InsertEffect(AudioMixerEffectController effect, int index)
    {
      List<AudioMixerEffectController> effectControllerList = new List<AudioMixerEffectController>((IEnumerable<AudioMixerEffectController>) this.effects);
      effectControllerList.Add((AudioMixerEffectController) null);
      for (int index1 = effectControllerList.Count - 1; index1 > index; --index1)
        effectControllerList[index1] = effectControllerList[index1 - 1];
      effectControllerList[index] = effect;
      this.effects = effectControllerList.ToArray();
    }

    public bool HasAttenuation()
    {
      foreach (AudioMixerEffectController effect in this.effects)
      {
        if (effect.IsAttenuation())
          return true;
      }
      return false;
    }

    public void DumpHierarchy(string title, int level)
    {
      if (title != string.Empty)
        Console.WriteLine(title);
      string empty = string.Empty;
      int num = level;
      while (num-- > 0)
        empty += "  ";
      Console.WriteLine(empty + "name=" + this.name);
      string str = empty + "  ";
      foreach (AudioMixerEffectController effect in this.effects)
        Console.WriteLine(str + "effect=" + effect.ToString());
      foreach (AudioMixerGroupController child in this.children)
        child.DumpHierarchy(string.Empty, level + 1);
    }

    public string GetDisplayString()
    {
      return this.name;
    }

    public override string ToString()
    {
      return this.name;
    }
  }
}

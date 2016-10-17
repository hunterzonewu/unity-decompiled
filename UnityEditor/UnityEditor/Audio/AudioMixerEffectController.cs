// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.AudioMixerEffectController
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Audio
{
  internal sealed class AudioMixerEffectController : Object
  {
    private int m_LastCachedGroupDisplayNameID;
    private string m_DisplayName;

    public GUID effectID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string effectName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public AudioMixerEffectController sendTarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public bool enableWetMix { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public bool bypass { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public AudioMixerEffectController(string name)
    {
      AudioMixerEffectController.Internal_CreateAudioMixerEffectController(this, name);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateAudioMixerEffectController(AudioMixerEffectController mono, string name);

    public bool IsSend()
    {
      return this.effectName == "Send";
    }

    public bool IsReceive()
    {
      return this.effectName == "Receive";
    }

    public bool IsDuckVolume()
    {
      return this.effectName == "Duck Volume";
    }

    public bool IsAttenuation()
    {
      return this.effectName == "Attenuation";
    }

    public bool DisallowsBypass()
    {
      if (!this.IsSend() && !this.IsReceive() && !this.IsDuckVolume())
        return this.IsAttenuation();
      return true;
    }

    public void ClearCachedDisplayName()
    {
      this.m_DisplayName = (string) null;
    }

    public string GetDisplayString(Dictionary<AudioMixerEffectController, AudioMixerGroupController> effectMap)
    {
      AudioMixerGroupController effect = effectMap[this];
      if (effect.GetInstanceID() != this.m_LastCachedGroupDisplayNameID || this.m_DisplayName == null)
      {
        this.m_DisplayName = effect.GetDisplayString() + AudioMixerController.s_GroupEffectDisplaySeperator + AudioMixerController.FixNameForPopupMenu(this.effectName);
        this.m_LastCachedGroupDisplayNameID = effect.GetInstanceID();
      }
      return this.m_DisplayName;
    }

    public string GetSendTargetDisplayString(Dictionary<AudioMixerEffectController, AudioMixerGroupController> effectMap)
    {
      if ((Object) this.sendTarget != (Object) null)
        return this.sendTarget.GetDisplayString(effectMap);
      return string.Empty;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void PreallocateGUIDs();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public GUID GetGUIDForMixLevel();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float GetValueForMixLevel(AudioMixerController controller, AudioMixerSnapshotController snapshot);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetValueForMixLevel(AudioMixerController controller, AudioMixerSnapshotController snapshot, float value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public GUID GetGUIDForParameter(string parameterName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float GetValueForParameter(AudioMixerController controller, AudioMixerSnapshotController snapshot, string parameterName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetValueForParameter(AudioMixerController controller, AudioMixerSnapshotController snapshot, string parameterName, float value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool GetFloatBuffer(AudioMixerController controller, string name, out float[] data, int numsamples);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float GetCPUUsage(AudioMixerController controller);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool ContainsParameterGUID(GUID guid);
  }
}

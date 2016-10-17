// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.AudioEffectParameterPath
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Audio
{
  internal sealed class AudioEffectParameterPath : AudioGroupParameterPath
  {
    public AudioMixerEffectController effect;

    public AudioEffectParameterPath(AudioMixerGroupController group, AudioMixerEffectController effect, GUID parameter)
      : base(group, parameter)
    {
      this.effect = effect;
    }

    public override string ResolveStringPath(bool getOnlyBasePath)
    {
      if (getOnlyBasePath)
        return this.GetBasePath(this.group.GetDisplayString(), this.effect.effectName);
      if (this.effect.GetGUIDForMixLevel() == this.parameter)
        return "Mix Level" + this.GetBasePath(this.group.GetDisplayString(), this.effect.effectName);
      MixerParameterDefinition[] effectParameters = MixerEffectDefinitions.GetEffectParameters(this.effect.effectName);
      for (int index = 0; index < effectParameters.Length; ++index)
      {
        if (this.effect.GetGUIDForParameter(effectParameters[index].name) == this.parameter)
          return effectParameters[index].name + this.GetBasePath(this.group.GetDisplayString(), this.effect.effectName);
      }
      return "Error finding Parameter path.";
    }
  }
}

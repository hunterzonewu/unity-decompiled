// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.MixerEffectDefinitions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Audio
{
  internal sealed class MixerEffectDefinitions
  {
    private static readonly List<MixerEffectDefinition> s_MixerEffectDefinitions = new List<MixerEffectDefinition>();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ClearDefinitionsRuntime();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void AddDefinitionRuntime(string name, MixerParameterDefinition[] parameters);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetAudioEffectNames();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern MixerParameterDefinition[] GetAudioEffectParameterDesc(string effectName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool EffectCanBeSidechainTarget(AudioMixerEffectController effect);

    public static void Refresh()
    {
      MixerEffectDefinitions.ClearDefinitions();
      MixerEffectDefinitions.RegisterAudioMixerEffect("Attenuation", new MixerParameterDefinition[0]);
      MixerEffectDefinitions.RegisterAudioMixerEffect("Send", new MixerParameterDefinition[0]);
      MixerEffectDefinitions.RegisterAudioMixerEffect("Receive", new MixerParameterDefinition[0]);
      MixerParameterDefinition[] parameterDefinitionArray = new MixerParameterDefinition[7]{ new MixerParameterDefinition() { name = "Threshold", units = "dB", displayScale = 1f, displayExponent = 1f, minRange = -80f, maxRange = 0.0f, defaultValue = -10f, description = "Threshold of side-chain level detector" }, new MixerParameterDefinition() { name = "Ratio", units = "%", displayScale = 100f, displayExponent = 1f, minRange = 0.2f, maxRange = 10f, defaultValue = 2f, description = "Ratio of compression applied when side-chain signal exceeds threshold" }, new MixerParameterDefinition() { name = "Attack Time", units = "ms", displayScale = 1000f, displayExponent = 3f, minRange = 0.0f, maxRange = 10f, defaultValue = 0.1f, description = "Level detector attack time" }, new MixerParameterDefinition() { name = "Release Time", units = "ms", displayScale = 1000f, displayExponent = 3f, minRange = 0.0f, maxRange = 10f, defaultValue = 0.1f, description = "Level detector release time" }, new MixerParameterDefinition() { name = "Make-up Gain", units = "dB", displayScale = 1f, displayExponent = 1f, minRange = -80f, maxRange = 40f, defaultValue = 0.0f, description = "Make-up gain" }, new MixerParameterDefinition() { name = "Knee", units = "dB", displayScale = 1f, displayExponent = 1f, minRange = 0.0f, maxRange = 50f, defaultValue = 10f, description = "Sharpness of compression curve knee" }, new MixerParameterDefinition() { name = "Sidechain Mix", units = "%", displayScale = 100f, displayExponent = 1f, minRange = 0.0f, maxRange = 1f, defaultValue = 1f, description = "Sidechain/source mix. If set to 100% the compressor detects level entirely from sidechain signal." } };
      MixerEffectDefinitions.RegisterAudioMixerEffect("Duck Volume", parameterDefinitionArray);
      MixerEffectDefinitions.AddDefinitionRuntime("Duck Volume", parameterDefinitionArray);
      foreach (string audioEffectName in MixerEffectDefinitions.GetAudioEffectNames())
      {
        MixerParameterDefinition[] effectParameterDesc = MixerEffectDefinitions.GetAudioEffectParameterDesc(audioEffectName);
        MixerEffectDefinitions.RegisterAudioMixerEffect(audioEffectName, effectParameterDesc);
      }
    }

    public static bool EffectExists(string name)
    {
      using (List<MixerEffectDefinition>.Enumerator enumerator = MixerEffectDefinitions.s_MixerEffectDefinitions.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.name == name)
            return true;
        }
      }
      return false;
    }

    public static string[] GetEffectList()
    {
      string[] strArray = new string[MixerEffectDefinitions.s_MixerEffectDefinitions.Count];
      for (int index = 0; index < MixerEffectDefinitions.s_MixerEffectDefinitions.Count; ++index)
        strArray[index] = MixerEffectDefinitions.s_MixerEffectDefinitions[index].name;
      return strArray;
    }

    public static void ClearDefinitions()
    {
      MixerEffectDefinitions.s_MixerEffectDefinitions.Clear();
      MixerEffectDefinitions.ClearDefinitionsRuntime();
    }

    public static MixerParameterDefinition[] GetEffectParameters(string effect)
    {
      using (List<MixerEffectDefinition>.Enumerator enumerator = MixerEffectDefinitions.s_MixerEffectDefinitions.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          MixerEffectDefinition current = enumerator.Current;
          if (current.name == effect)
            return current.parameters;
        }
      }
      return new MixerParameterDefinition[0];
    }

    public static bool RegisterAudioMixerEffect(string name, MixerParameterDefinition[] definitions)
    {
      using (List<MixerEffectDefinition>.Enumerator enumerator = MixerEffectDefinitions.s_MixerEffectDefinitions.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.name == name)
            return false;
        }
      }
      MixerEffectDefinition effectDefinition = new MixerEffectDefinition(name, definitions);
      MixerEffectDefinitions.s_MixerEffectDefinitions.Add(effectDefinition);
      MixerEffectDefinitions.ClearDefinitionsRuntime();
      using (List<MixerEffectDefinition>.Enumerator enumerator = MixerEffectDefinitions.s_MixerEffectDefinitions.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          MixerEffectDefinition current = enumerator.Current;
          MixerEffectDefinitions.AddDefinitionRuntime(current.name, current.parameters);
        }
      }
      return true;
    }
  }
}

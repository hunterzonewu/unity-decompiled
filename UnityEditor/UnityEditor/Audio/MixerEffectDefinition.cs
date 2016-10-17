// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.MixerEffectDefinition
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor.Audio
{
  internal class MixerEffectDefinition
  {
    private readonly string m_EffectName;
    private readonly MixerParameterDefinition[] m_Parameters;

    public string name
    {
      get
      {
        return this.m_EffectName;
      }
    }

    public MixerParameterDefinition[] parameters
    {
      get
      {
        return this.m_Parameters;
      }
    }

    public MixerEffectDefinition(string name, MixerParameterDefinition[] parameters)
    {
      this.m_EffectName = name;
      this.m_Parameters = new MixerParameterDefinition[parameters.Length];
      Array.Copy((Array) parameters, (Array) this.m_Parameters, parameters.Length);
    }
  }
}

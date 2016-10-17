// Decompiled with JetBrains decompiler
// Type: UnityEditor.IAudioEffectPlugin
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  public abstract class IAudioEffectPlugin
  {
    public abstract bool SetFloatParameter(string name, float value);

    public abstract bool GetFloatParameter(string name, out float value);

    public abstract bool GetFloatParameterInfo(string name, out float minRange, out float maxRange, out float defaultValue);

    public abstract bool GetFloatBuffer(string name, out float[] data, int numsamples);

    public abstract int GetSampleRate();

    public abstract bool IsPluginEditableAndEnabled();
  }
}

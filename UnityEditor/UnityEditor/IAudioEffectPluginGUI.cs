// Decompiled with JetBrains decompiler
// Type: UnityEditor.IAudioEffectPluginGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  public abstract class IAudioEffectPluginGUI
  {
    public abstract string Name { get; }

    public abstract string Description { get; }

    public abstract string Vendor { get; }

    public abstract bool OnGUI(IAudioEffectPlugin plugin);
  }
}

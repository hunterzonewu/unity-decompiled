// Decompiled with JetBrains decompiler
// Type: UnityEditor.Hardware.DevDevice
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor.Hardware
{
  [RequiredByNativeCode]
  public struct DevDevice
  {
    public readonly string id;
    public readonly string name;
    public readonly string type;
    public readonly string module;
    public readonly DevDeviceState state;
    public readonly DevDeviceFeatures features;

    public bool isConnected
    {
      get
      {
        return this.state == DevDeviceState.Connected;
      }
    }

    public static DevDevice none
    {
      get
      {
        return new DevDevice("None", "None", "none", "internal", DevDeviceState.Disconnected, DevDeviceFeatures.None);
      }
    }

    public DevDevice(string id, string name, string type, string module, DevDeviceState state, DevDeviceFeatures features)
    {
      this.id = id;
      this.name = name;
      this.type = type;
      this.module = module;
      this.state = state;
      this.features = features;
    }

    public override string ToString()
    {
      return this.name + " (id:" + this.id + ", type: " + this.type + ", module: " + this.module + ", state: " + (object) this.state + ", features: " + (object) this.features + ")";
    }
  }
}

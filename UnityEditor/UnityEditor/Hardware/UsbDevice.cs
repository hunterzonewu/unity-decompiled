// Decompiled with JetBrains decompiler
// Type: UnityEditor.Hardware.UsbDevice
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Hardware
{
  public struct UsbDevice
  {
    public readonly int vendorId;
    public readonly int productId;
    public readonly int revision;
    public readonly string udid;
    public readonly string name;

    public override string ToString()
    {
      return this.name + " (udid:" + this.udid + ", vid: " + this.vendorId.ToString("X4") + ", pid: " + this.productId.ToString("X4") + ", rev: " + this.revision.ToString("X4") + ")";
    }
  }
}

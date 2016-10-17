// Decompiled with JetBrains decompiler
// Type: UnityEditor.Hardware.Usb
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Hardware
{
  public sealed class Usb
  {
    public static event Usb.OnDevicesChangedHandler DevicesChanged;

    public static void OnDevicesChanged(UsbDevice[] devices)
    {
      if (Usb.DevicesChanged == null || devices == null)
        return;
      Usb.DevicesChanged(devices);
    }

    public delegate void OnDevicesChangedHandler(UsbDevice[] devices);
  }
}

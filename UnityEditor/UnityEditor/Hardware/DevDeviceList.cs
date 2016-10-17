// Decompiled with JetBrains decompiler
// Type: UnityEditor.Hardware.DevDeviceList
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Hardware
{
  public sealed class DevDeviceList
  {
    public static event DevDeviceList.OnChangedHandler Changed;

    public static void OnChanged()
    {
      if (DevDeviceList.Changed == null)
        return;
      DevDeviceList.Changed();
    }

    public static bool FindDevice(string deviceId, out DevDevice device)
    {
      foreach (DevDevice device1 in DevDeviceList.GetDevices())
      {
        if (device1.id == deviceId)
        {
          device = device1;
          return true;
        }
      }
      device = new DevDevice();
      return false;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern DevDevice[] GetDevices();

    internal static void Update(string target, DevDevice[] devices)
    {
      DevDeviceList.UpdateInternal(target, devices);
      DevDeviceList.OnChanged();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateInternal(string target, DevDevice[] devices);

    public delegate void OnChangedHandler();
  }
}

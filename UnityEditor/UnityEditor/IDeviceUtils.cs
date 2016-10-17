// Decompiled with JetBrains decompiler
// Type: UnityEditor.IDeviceUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Modules;

namespace UnityEditor
{
  internal static class IDeviceUtils
  {
    internal static RemoteAddress StartRemoteSupport(string deviceId)
    {
      return ModuleManager.GetDevice(deviceId).StartRemoteSupport();
    }

    internal static void StopRemoteSupport(string deviceId)
    {
      ModuleManager.GetDevice(deviceId).StopRemoteSupport();
    }

    internal static RemoteAddress StartPlayerConnectionSupport(string deviceId)
    {
      return ModuleManager.GetDevice(deviceId).StartPlayerConnectionSupport();
    }

    internal static void StopPlayerConnectionSupport(string deviceId)
    {
      ModuleManager.GetDevice(deviceId).StopPlayerConnectionSupport();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEngine.Cloud.Service.CloudService
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Cloud.Service
{
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class CloudService : IDisposable
  {
    [NonSerialized]
    internal IntPtr m_Ptr;

    public string serviceFolderName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public CloudService(CloudServiceType serviceType)
    {
      this.InternalCreate(serviceType);
    }

    ~CloudService()
    {
      this.InternalDestroy();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void InternalCreate(CloudServiceType serviceType);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void InternalDestroy();

    public void Dispose()
    {
      this.InternalDestroy();
      GC.SuppressFinalize((object) this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool Initialize(string projectId);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool StartEventHandler(string sessionInfo, int maxNumberOfEventInQueue, int maxEventTimeoutInSec);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool PauseEventHandler(bool flushEvents);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool StopEventHandler();

    public bool StartEventDispatcher(CloudServiceConfig serviceConfig, Dictionary<string, string> headers)
    {
      return this.InternalStartEventDispatcher(serviceConfig, CloudService.FlattenedHeadersFrom(headers));
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private bool InternalStartEventDispatcher(CloudServiceConfig serviceConfig, string[] headers);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool PauseEventDispatcher();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool StopEventDispatcher();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ResetNetworkRetryIndex();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool QueueEvent(string eventData, CloudEventFlags flags);

    public bool SaveFileFromServer(string fileName, string url, Dictionary<string, string> headers, object d, string methodName)
    {
      if (methodName == null)
        methodName = string.Empty;
      return this.InternalSaveFileFromServer(fileName, url, CloudService.FlattenedHeadersFrom(headers), d, methodName);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private bool InternalSaveFileFromServer(string fileName, string url, string[] headers, object d, string methodName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool SaveFile(string fileName, string data);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string RestoreFile(string fileName);

    private static string[] FlattenedHeadersFrom(Dictionary<string, string> headers)
    {
      if (headers == null)
        return (string[]) null;
      string[] strArray1 = new string[headers.Count * 2];
      int num1 = 0;
      using (Dictionary<string, string>.Enumerator enumerator = headers.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, string> current = enumerator.Current;
          string[] strArray2 = strArray1;
          int index1 = num1;
          int num2 = 1;
          int num3 = index1 + num2;
          string str1 = current.Key.ToString();
          strArray2[index1] = str1;
          string[] strArray3 = strArray1;
          int index2 = num3;
          int num4 = 1;
          num1 = index2 + num4;
          string str2 = current.Value.ToString();
          strArray3[index2] = str2;
        }
      }
      return strArray1;
    }
  }
}

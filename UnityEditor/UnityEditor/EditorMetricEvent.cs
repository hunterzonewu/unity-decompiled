// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorMetricEvent
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEditor
{
  [StructLayout(LayoutKind.Sequential)]
  internal class EditorMetricEvent : IDisposable
  {
    internal IntPtr m_Ptr;

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public EditorMetricEvent(EditorMetricCollectionType en);

    ~EditorMetricEvent()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void AddValueStr(string key, string value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void AddValueInt(string key, int value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void AddChildValueBool(string parent, string key, bool value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void AddChildValueInt(string parent, string key, int value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void AddChildValueStr(string parent, string key, string value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Send();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Destroy();

    public void Dispose()
    {
      this.Destroy();
      this.m_Ptr = IntPtr.Zero;
    }

    internal void AddValue(string key, string value)
    {
      this.AddValueStr(key, value);
    }

    internal void AddValue(string key, int value)
    {
      this.AddValueInt(key, value);
    }

    internal void AddChildValue(string parent, string key, int value)
    {
      this.AddChildValueInt(parent, key, value);
    }

    internal void AddChildValue(string parent, string key, string value)
    {
      this.AddChildValueStr(parent, key, value);
    }

    internal void AddChildValue(string parent, string key, bool value)
    {
      this.AddChildValueBool(parent, key, value);
    }
  }
}

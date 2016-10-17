// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.Request
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.RestService
{
  internal sealed class Request
  {
    private IntPtr m_nativeRequestPtr;

    public string Payload { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string Url { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int MessageType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int Depth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool Info { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetParam(string paramName);
  }
}

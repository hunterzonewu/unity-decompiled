// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.WriteResponseDelegateHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.RestService
{
  internal sealed class WriteResponseDelegateHelper
  {
    internal static WriteResponse MakeDelegateFor(IntPtr response, IntPtr callbackData)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new WriteResponse(new WriteResponseDelegateHelper.\u003CMakeDelegateFor\u003Ec__AnonStorey2A() { response = response, callbackData = callbackData }.\u003C\u003Em__3D);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DoWriteResponse(IntPtr cppResponse, HttpStatusCode resultCode, string payload, IntPtr callbackData);
  }
}

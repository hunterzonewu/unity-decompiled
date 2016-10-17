// Decompiled with JetBrains decompiler
// Type: UnityEditor.WebViewV8CallbackCSharp
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal sealed class WebViewV8CallbackCSharp
  {
    [SerializeField]
    private IntPtr m_thisDummy;

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Callback(string result);

    public void OnDestroy()
    {
      this.DestroyCallBack();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void DestroyCallBack();
  }
}

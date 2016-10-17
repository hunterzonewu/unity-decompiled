// Decompiled with JetBrains decompiler
// Type: UnityEngine.Coroutine
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>MonoBehaviour.StartCoroutine returns a Coroutine. Instances of this class are only used to reference these coroutines and do not hold any exposed properties or functions.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class Coroutine : YieldInstruction
  {
    internal IntPtr m_Ptr;

    private Coroutine()
    {
    }

    ~Coroutine()
    {
      this.ReleaseCoroutine();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void ReleaseCoroutine();
  }
}

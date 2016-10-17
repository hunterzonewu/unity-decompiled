// Decompiled with JetBrains decompiler
// Type: UnityEngine.AsyncOperation
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
  ///   <para>Asynchronous operation coroutine.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public class AsyncOperation : YieldInstruction
  {
    internal IntPtr m_Ptr;

    /// <summary>
    ///   <para>Has the operation finished? (Read Only)</para>
    /// </summary>
    public bool isDone { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>What's the operation's progress. (Read Only)</para>
    /// </summary>
    public float progress { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Priority lets you tweak in which order async operation calls will be performed.</para>
    /// </summary>
    public int priority { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Allow scenes to be activated as soon as it is ready.</para>
    /// </summary>
    public bool allowSceneActivation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    ~AsyncOperation()
    {
      this.InternalDestroy();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InternalDestroy();
  }
}

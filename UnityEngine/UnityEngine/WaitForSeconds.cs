// Decompiled with JetBrains decompiler
// Type: UnityEngine.WaitForSeconds
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Suspends the coroutine execution for the given amount of seconds.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class WaitForSeconds : YieldInstruction
  {
    internal float m_Seconds;

    /// <summary>
    ///   <para>Creates a yield instruction to wait for a given number of seconds.</para>
    /// </summary>
    /// <param name="seconds"></param>
    public WaitForSeconds(float seconds)
    {
      this.m_Seconds = seconds;
    }
  }
}

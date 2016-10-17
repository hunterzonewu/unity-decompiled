// Decompiled with JetBrains decompiler
// Type: UnityEngine.WaitUntil
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Suspends the coroutine execution until the supplied delegate evaluates to true.</para>
  /// </summary>
  public sealed class WaitUntil : CustomYieldInstruction
  {
    private Func<bool> m_Predicate;

    public override bool keepWaiting
    {
      get
      {
        return !this.m_Predicate();
      }
    }

    public WaitUntil(Func<bool> predicate)
    {
      this.m_Predicate = predicate;
    }
  }
}

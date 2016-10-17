// Decompiled with JetBrains decompiler
// Type: UnityEngine.CustomYieldInstruction
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Collections;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Base class for custom yield instructions to suspend coroutines.</para>
  /// </summary>
  public abstract class CustomYieldInstruction : IEnumerator
  {
    /// <summary>
    ///   <para>Indicates if coroutine should be kept suspended.</para>
    /// </summary>
    public abstract bool keepWaiting { get; }

    public object Current
    {
      get
      {
        return (object) null;
      }
    }

    public bool MoveNext()
    {
      return this.keepWaiting;
    }

    public void Reset()
    {
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.CachedInvokableCall`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Reflection;

namespace UnityEngine.Events
{
  internal class CachedInvokableCall<T> : InvokableCall<T>
  {
    private readonly object[] m_Arg1 = new object[1];

    public CachedInvokableCall(Object target, MethodInfo theFunction, T argument)
      : base((object) target, theFunction)
    {
      this.m_Arg1[0] = (object) argument;
    }

    public override void Invoke(object[] args)
    {
      base.Invoke(this.m_Arg1);
    }
  }
}

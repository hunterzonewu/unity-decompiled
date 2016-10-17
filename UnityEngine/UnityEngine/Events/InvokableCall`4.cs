// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.InvokableCall`4
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
  internal class InvokableCall<T1, T2, T3, T4> : BaseInvokableCall
  {
    protected event UnityAction<T1, T2, T3, T4> Delegate;

    public InvokableCall(object target, MethodInfo theFunction)
      : base(target, theFunction)
    {
      this.Delegate = (UnityAction<T1, T2, T3, T4>) theFunction.CreateDelegate(typeof (UnityAction<T1, T2, T3, T4>), target);
    }

    public InvokableCall(UnityAction<T1, T2, T3, T4> action)
    {
      this.Delegate += action;
    }

    public override void Invoke(object[] args)
    {
      if (args.Length != 4)
        throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
      BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
      BaseInvokableCall.ThrowOnInvalidArg<T2>(args[1]);
      BaseInvokableCall.ThrowOnInvalidArg<T3>(args[2]);
      BaseInvokableCall.ThrowOnInvalidArg<T4>(args[3]);
      if (!BaseInvokableCall.AllowInvoke((System.Delegate) this.Delegate))
        return;
      this.Delegate((T1) args[0], (T2) args[1], (T3) args[2], (T4) args[3]);
    }

    public override bool Find(object targetObj, MethodInfo method)
    {
      if (this.Delegate.Target == targetObj)
        return this.Delegate.GetMethodInfo() == method;
      return false;
    }
  }
}

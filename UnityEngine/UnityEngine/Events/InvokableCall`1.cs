// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.InvokableCall`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
  internal class InvokableCall<T1> : BaseInvokableCall
  {
    protected event UnityAction<T1> Delegate;

    public InvokableCall(object target, MethodInfo theFunction)
      : base(target, theFunction)
    {
      InvokableCall<T1> invokableCall = this;
      UnityAction<T1> unityAction = (UnityAction<T1>) System.Delegate.Combine((System.Delegate) invokableCall.Delegate, theFunction.CreateDelegate(typeof (UnityAction<T1>), target));
      invokableCall.Delegate = unityAction;
    }

    public InvokableCall(UnityAction<T1> action)
    {
      this.Delegate += action;
    }

    public override void Invoke(object[] args)
    {
      if (args.Length != 1)
        throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
      BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
      if (!BaseInvokableCall.AllowInvoke((System.Delegate) this.Delegate))
        return;
      this.Delegate((T1) args[0]);
    }

    public override bool Find(object targetObj, MethodInfo method)
    {
      if (this.Delegate.Target == targetObj)
        return this.Delegate.GetMethodInfo() == method;
      return false;
    }
  }
}

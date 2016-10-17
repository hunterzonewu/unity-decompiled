// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.InvokableCall
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
  internal class InvokableCall : BaseInvokableCall
  {
    private event UnityAction Delegate;

    public InvokableCall(object target, MethodInfo theFunction)
      : base(target, theFunction)
    {
      InvokableCall invokableCall = this;
      UnityAction unityAction = (UnityAction) System.Delegate.Combine((System.Delegate) invokableCall.Delegate, theFunction.CreateDelegate(typeof (UnityAction), target));
      invokableCall.Delegate = unityAction;
    }

    public InvokableCall(UnityAction action)
    {
      this.Delegate += action;
    }

    public override void Invoke(object[] args)
    {
      if (!BaseInvokableCall.AllowInvoke((System.Delegate) this.Delegate))
        return;
      this.Delegate();
    }

    public override bool Find(object targetObj, MethodInfo method)
    {
      if (this.Delegate.Target == targetObj)
        return this.Delegate.GetMethodInfo() == method;
      return false;
    }
  }
}

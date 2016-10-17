// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.UnityEvent`4
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Reflection;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine.Events
{
  /// <summary>
  ///   <para>Four argument version of UnityEvent.</para>
  /// </summary>
  [Serializable]
  public abstract class UnityEvent<T0, T1, T2, T3> : UnityEventBase
  {
    private readonly object[] m_InvokeArray = new object[4];

    [RequiredByNativeCode]
    public UnityEvent()
    {
    }

    public void AddListener(UnityAction<T0, T1, T2, T3> call)
    {
      this.AddCall(UnityEvent<T0, T1, T2, T3>.GetDelegate(call));
    }

    public void RemoveListener(UnityAction<T0, T1, T2, T3> call)
    {
      this.RemoveListener(call.Target, call.GetMethodInfo());
    }

    protected override MethodInfo FindMethod_Impl(string name, object targetObj)
    {
      return UnityEventBase.GetValidMethodInfo(targetObj, name, new System.Type[4]{ typeof (T0), typeof (T1), typeof (T2), typeof (T3) });
    }

    internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
    {
      return (BaseInvokableCall) new InvokableCall<T0, T1, T2, T3>(target, theFunction);
    }

    private static BaseInvokableCall GetDelegate(UnityAction<T0, T1, T2, T3> action)
    {
      return (BaseInvokableCall) new InvokableCall<T0, T1, T2, T3>(action);
    }

    public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
      this.m_InvokeArray[0] = (object) arg0;
      this.m_InvokeArray[1] = (object) arg1;
      this.m_InvokeArray[2] = (object) arg2;
      this.m_InvokeArray[3] = (object) arg3;
      this.Invoke(this.m_InvokeArray);
    }

    internal void AddPersistentListener(UnityAction<T0, T1, T2, T3> call)
    {
      this.AddPersistentListener(call, UnityEventCallState.RuntimeOnly);
    }

    internal void AddPersistentListener(UnityAction<T0, T1, T2, T3> call, UnityEventCallState callState)
    {
      int persistentEventCount = this.GetPersistentEventCount();
      this.AddPersistentListener();
      this.RegisterPersistentListener(persistentEventCount, call);
      this.SetPersistentListenerState(persistentEventCount, callState);
    }

    internal void RegisterPersistentListener(int index, UnityAction<T0, T1, T2, T3> call)
    {
      if (call == null)
        Debug.LogWarning((object) "Registering a Listener requires an action");
      else
        this.RegisterPersistentListener(index, (object) (call.Target as UnityEngine.Object), call.Method);
    }
  }
}

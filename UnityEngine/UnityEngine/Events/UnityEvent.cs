// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.UnityEvent
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
  ///   <para>A zero argument persistent callback that can be saved with the scene.</para>
  /// </summary>
  [Serializable]
  public class UnityEvent : UnityEventBase
  {
    private readonly object[] m_InvokeArray = new object[0];

    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    [RequiredByNativeCode]
    public UnityEvent()
    {
    }

    /// <summary>
    ///   <para>Add a non persistent listener to the UnityEvent.</para>
    /// </summary>
    /// <param name="call">Callback function.</param>
    public void AddListener(UnityAction call)
    {
      this.AddCall(UnityEvent.GetDelegate(call));
    }

    /// <summary>
    ///   <para>Remove a non persistent listener from the UnityEvent.</para>
    /// </summary>
    /// <param name="call">Callback function.</param>
    public void RemoveListener(UnityAction call)
    {
      this.RemoveListener(call.Target, call.GetMethodInfo());
    }

    protected override MethodInfo FindMethod_Impl(string name, object targetObj)
    {
      return UnityEventBase.GetValidMethodInfo(targetObj, name, new System.Type[0]);
    }

    internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
    {
      return (BaseInvokableCall) new InvokableCall(target, theFunction);
    }

    private static BaseInvokableCall GetDelegate(UnityAction action)
    {
      return (BaseInvokableCall) new InvokableCall(action);
    }

    /// <summary>
    ///   <para>Invoke all registered callbacks (runtime and peristent).</para>
    /// </summary>
    public void Invoke()
    {
      this.Invoke(this.m_InvokeArray);
    }

    internal void AddPersistentListener(UnityAction call)
    {
      this.AddPersistentListener(call, UnityEventCallState.RuntimeOnly);
    }

    internal void AddPersistentListener(UnityAction call, UnityEventCallState callState)
    {
      int persistentEventCount = this.GetPersistentEventCount();
      this.AddPersistentListener();
      this.RegisterPersistentListener(persistentEventCount, call);
      this.SetPersistentListenerState(persistentEventCount, callState);
    }

    internal void RegisterPersistentListener(int index, UnityAction call)
    {
      if (call == null)
        Debug.LogWarning((object) "Registering a Listener requires an action");
      else
        this.RegisterPersistentListener(index, (object) (call.Target as UnityEngine.Object), call.Method);
    }
  }
}

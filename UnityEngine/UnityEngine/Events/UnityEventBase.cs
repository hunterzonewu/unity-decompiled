// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.UnityEventBase
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Reflection;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
  /// <summary>
  ///   <para>Abstract base class for UnityEvents.</para>
  /// </summary>
  [Serializable]
  public abstract class UnityEventBase : ISerializationCallbackReceiver
  {
    private bool m_CallsDirty = true;
    private InvokableCallList m_Calls;
    [SerializeField]
    [FormerlySerializedAs("m_PersistentListeners")]
    private PersistentCallGroup m_PersistentCalls;
    [SerializeField]
    private string m_TypeName;

    protected UnityEventBase()
    {
      this.m_Calls = new InvokableCallList();
      this.m_PersistentCalls = new PersistentCallGroup();
      this.m_TypeName = this.GetType().AssemblyQualifiedName;
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
      this.DirtyPersistentCalls();
      this.m_TypeName = this.GetType().AssemblyQualifiedName;
    }

    protected abstract MethodInfo FindMethod_Impl(string name, object targetObj);

    internal abstract BaseInvokableCall GetDelegate(object target, MethodInfo theFunction);

    internal MethodInfo FindMethod(PersistentCall call)
    {
      System.Type argumentType = typeof (UnityEngine.Object);
      if (!string.IsNullOrEmpty(call.arguments.unityObjectArgumentAssemblyTypeName))
        argumentType = System.Type.GetType(call.arguments.unityObjectArgumentAssemblyTypeName, false) ?? typeof (UnityEngine.Object);
      return this.FindMethod(call.methodName, (object) call.target, call.mode, argumentType);
    }

    internal MethodInfo FindMethod(string name, object listener, PersistentListenerMode mode, System.Type argumentType)
    {
      switch (mode)
      {
        case PersistentListenerMode.EventDefined:
          return this.FindMethod_Impl(name, listener);
        case PersistentListenerMode.Void:
          return UnityEventBase.GetValidMethodInfo(listener, name, new System.Type[0]);
        case PersistentListenerMode.Object:
          return UnityEventBase.GetValidMethodInfo(listener, name, new System.Type[1]{ argumentType ?? typeof (UnityEngine.Object) });
        case PersistentListenerMode.Int:
          return UnityEventBase.GetValidMethodInfo(listener, name, new System.Type[1]{ typeof (int) });
        case PersistentListenerMode.Float:
          return UnityEventBase.GetValidMethodInfo(listener, name, new System.Type[1]{ typeof (float) });
        case PersistentListenerMode.String:
          return UnityEventBase.GetValidMethodInfo(listener, name, new System.Type[1]{ typeof (string) });
        case PersistentListenerMode.Bool:
          return UnityEventBase.GetValidMethodInfo(listener, name, new System.Type[1]{ typeof (bool) });
        default:
          return (MethodInfo) null;
      }
    }

    /// <summary>
    ///   <para>Get the number of registered persistent listeners.</para>
    /// </summary>
    public int GetPersistentEventCount()
    {
      return this.m_PersistentCalls.Count;
    }

    /// <summary>
    ///   <para>Get the target component of the listener at index index.</para>
    /// </summary>
    /// <param name="index">Index of the listener to query.</param>
    public UnityEngine.Object GetPersistentTarget(int index)
    {
      PersistentCall listener = this.m_PersistentCalls.GetListener(index);
      if (listener != null)
        return listener.target;
      return (UnityEngine.Object) null;
    }

    /// <summary>
    ///   <para>Get the target method name of the listener at index index.</para>
    /// </summary>
    /// <param name="index">Index of the listener to query.</param>
    public string GetPersistentMethodName(int index)
    {
      PersistentCall listener = this.m_PersistentCalls.GetListener(index);
      if (listener != null)
        return listener.methodName;
      return string.Empty;
    }

    private void DirtyPersistentCalls()
    {
      this.m_Calls.ClearPersistent();
      this.m_CallsDirty = true;
    }

    private void RebuildPersistentCallsIfNeeded()
    {
      if (!this.m_CallsDirty)
        return;
      this.m_PersistentCalls.Initialize(this.m_Calls, this);
      this.m_CallsDirty = false;
    }

    /// <summary>
    ///   <para>Modify the execution state of a persistent listener.</para>
    /// </summary>
    /// <param name="index">Index of the listener to query.</param>
    /// <param name="state">State to set.</param>
    public void SetPersistentListenerState(int index, UnityEventCallState state)
    {
      PersistentCall listener = this.m_PersistentCalls.GetListener(index);
      if (listener != null)
        listener.callState = state;
      this.DirtyPersistentCalls();
    }

    protected void AddListener(object targetObj, MethodInfo method)
    {
      this.m_Calls.AddListener(this.GetDelegate(targetObj, method));
    }

    internal void AddCall(BaseInvokableCall call)
    {
      this.m_Calls.AddListener(call);
    }

    protected void RemoveListener(object targetObj, MethodInfo method)
    {
      this.m_Calls.RemoveListener(targetObj, method);
    }

    /// <summary>
    ///   <para>Remove all listeners from the event.</para>
    /// </summary>
    public void RemoveAllListeners()
    {
      this.m_Calls.Clear();
    }

    protected void Invoke(object[] parameters)
    {
      this.RebuildPersistentCallsIfNeeded();
      this.m_Calls.Invoke(parameters);
    }

    public override string ToString()
    {
      return base.ToString() + " " + this.GetType().FullName;
    }

    /// <summary>
    ///   <para>Given an object, function name, and a list of argument types; find the method that matches.</para>
    /// </summary>
    /// <param name="obj">Object to search for the method.</param>
    /// <param name="functionName">Function name to search for.</param>
    /// <param name="argumentTypes">Argument types for the function.</param>
    public static MethodInfo GetValidMethodInfo(object obj, string functionName, System.Type[] argumentTypes)
    {
      for (System.Type type = obj.GetType(); type != typeof (object) && type != null; type = type.BaseType)
      {
        MethodInfo method = type.GetMethod(functionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, argumentTypes, (ParameterModifier[]) null);
        if (method != null)
        {
          ParameterInfo[] parameters = method.GetParameters();
          bool flag = true;
          int index = 0;
          foreach (ParameterInfo parameterInfo in parameters)
          {
            flag = argumentTypes[index].IsPrimitive == parameterInfo.ParameterType.IsPrimitive;
            if (flag)
              ++index;
            else
              break;
          }
          if (flag)
            return method;
        }
      }
      return (MethodInfo) null;
    }

    protected bool ValidateRegistration(MethodInfo method, object targetObj, PersistentListenerMode mode)
    {
      return this.ValidateRegistration(method, targetObj, mode, typeof (UnityEngine.Object));
    }

    protected bool ValidateRegistration(MethodInfo method, object targetObj, PersistentListenerMode mode, System.Type argumentType)
    {
      if (method == null)
        throw new ArgumentNullException("method", UnityString.Format("Can not register null method on {0} for callback!", targetObj));
      UnityEngine.Object @object = targetObj as UnityEngine.Object;
      if (@object == (UnityEngine.Object) null || @object.GetInstanceID() == 0)
        throw new ArgumentException(UnityString.Format("Could not register callback {0} on {1}. The class {2} does not derive from UnityEngine.Object", (object) method.Name, targetObj, (object) (targetObj != null ? targetObj.GetType().ToString() : "null")));
      if (method.IsStatic)
        throw new ArgumentException(UnityString.Format("Could not register listener {0} on {1} static functions are not supported.", (object) method, (object) this.GetType()));
      if (this.FindMethod(method.Name, targetObj, mode, argumentType) != null)
        return true;
      Debug.LogWarning((object) UnityString.Format("Could not register listener {0}.{1} on {2} the method could not be found.", targetObj, (object) method, (object) this.GetType()));
      return false;
    }

    internal void AddPersistentListener()
    {
      this.m_PersistentCalls.AddListener();
    }

    protected void RegisterPersistentListener(int index, object targetObj, MethodInfo method)
    {
      if (!this.ValidateRegistration(method, targetObj, PersistentListenerMode.EventDefined))
        return;
      this.m_PersistentCalls.RegisterEventPersistentListener(index, targetObj as UnityEngine.Object, method.Name);
      this.DirtyPersistentCalls();
    }

    internal void RemovePersistentListener(UnityEngine.Object target, MethodInfo method)
    {
      if (method == null || method.IsStatic || (target == (UnityEngine.Object) null || target.GetInstanceID() == 0))
        return;
      this.m_PersistentCalls.RemoveListeners(target, method.Name);
      this.DirtyPersistentCalls();
    }

    internal void RemovePersistentListener(int index)
    {
      this.m_PersistentCalls.RemoveListener(index);
      this.DirtyPersistentCalls();
    }

    internal void UnregisterPersistentListener(int index)
    {
      this.m_PersistentCalls.UnregisterPersistentListener(index);
      this.DirtyPersistentCalls();
    }

    internal void AddVoidPersistentListener(UnityAction call)
    {
      int persistentEventCount = this.GetPersistentEventCount();
      this.AddPersistentListener();
      this.RegisterVoidPersistentListener(persistentEventCount, call);
    }

    internal void RegisterVoidPersistentListener(int index, UnityAction call)
    {
      if (call == null)
      {
        Debug.LogWarning((object) "Registering a Listener requires an action");
      }
      else
      {
        if (!this.ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Void))
          return;
        this.m_PersistentCalls.RegisterVoidPersistentListener(index, call.Target as UnityEngine.Object, call.Method.Name);
        this.DirtyPersistentCalls();
      }
    }

    internal void AddIntPersistentListener(UnityAction<int> call, int argument)
    {
      int persistentEventCount = this.GetPersistentEventCount();
      this.AddPersistentListener();
      this.RegisterIntPersistentListener(persistentEventCount, call, argument);
    }

    internal void RegisterIntPersistentListener(int index, UnityAction<int> call, int argument)
    {
      if (call == null)
      {
        Debug.LogWarning((object) "Registering a Listener requires an action");
      }
      else
      {
        if (!this.ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Int))
          return;
        this.m_PersistentCalls.RegisterIntPersistentListener(index, call.Target as UnityEngine.Object, argument, call.Method.Name);
        this.DirtyPersistentCalls();
      }
    }

    internal void AddFloatPersistentListener(UnityAction<float> call, float argument)
    {
      int persistentEventCount = this.GetPersistentEventCount();
      this.AddPersistentListener();
      this.RegisterFloatPersistentListener(persistentEventCount, call, argument);
    }

    internal void RegisterFloatPersistentListener(int index, UnityAction<float> call, float argument)
    {
      if (call == null)
      {
        Debug.LogWarning((object) "Registering a Listener requires an action");
      }
      else
      {
        if (!this.ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Float))
          return;
        this.m_PersistentCalls.RegisterFloatPersistentListener(index, call.Target as UnityEngine.Object, argument, call.Method.Name);
        this.DirtyPersistentCalls();
      }
    }

    internal void AddBoolPersistentListener(UnityAction<bool> call, bool argument)
    {
      int persistentEventCount = this.GetPersistentEventCount();
      this.AddPersistentListener();
      this.RegisterBoolPersistentListener(persistentEventCount, call, argument);
    }

    internal void RegisterBoolPersistentListener(int index, UnityAction<bool> call, bool argument)
    {
      if (call == null)
      {
        Debug.LogWarning((object) "Registering a Listener requires an action");
      }
      else
      {
        if (!this.ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Bool))
          return;
        this.m_PersistentCalls.RegisterBoolPersistentListener(index, call.Target as UnityEngine.Object, argument, call.Method.Name);
        this.DirtyPersistentCalls();
      }
    }

    internal void AddStringPersistentListener(UnityAction<string> call, string argument)
    {
      int persistentEventCount = this.GetPersistentEventCount();
      this.AddPersistentListener();
      this.RegisterStringPersistentListener(persistentEventCount, call, argument);
    }

    internal void RegisterStringPersistentListener(int index, UnityAction<string> call, string argument)
    {
      if (call == null)
      {
        Debug.LogWarning((object) "Registering a Listener requires an action");
      }
      else
      {
        if (!this.ValidateRegistration(call.Method, call.Target, PersistentListenerMode.String))
          return;
        this.m_PersistentCalls.RegisterStringPersistentListener(index, call.Target as UnityEngine.Object, argument, call.Method.Name);
        this.DirtyPersistentCalls();
      }
    }

    internal void AddObjectPersistentListener<T>(UnityAction<T> call, T argument) where T : UnityEngine.Object
    {
      int persistentEventCount = this.GetPersistentEventCount();
      this.AddPersistentListener();
      this.RegisterObjectPersistentListener<T>(persistentEventCount, call, argument);
    }

    internal void RegisterObjectPersistentListener<T>(int index, UnityAction<T> call, T argument) where T : UnityEngine.Object
    {
      if (call == null)
        throw new ArgumentNullException("call", "Registering a Listener requires a non null call");
      if (!this.ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Object, !((UnityEngine.Object) argument == (UnityEngine.Object) null) ? argument.GetType() : typeof (UnityEngine.Object)))
        return;
      this.m_PersistentCalls.RegisterObjectPersistentListener(index, call.Target as UnityEngine.Object, (UnityEngine.Object) argument, call.Method.Name);
      this.DirtyPersistentCalls();
    }
  }
}

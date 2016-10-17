// Decompiled with JetBrains decompiler
// Type: UnityEditor.Events.UnityEventTools
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor.Events
{
  /// <summary>
  ///   <para>Editor tools for working with persistent UnityEvents.</para>
  /// </summary>
  public static class UnityEventTools
  {
    /// <summary>
    ///   <para>Adds a persistent, call to the listener. Will be invoked with the arguments as defined by the Event and sent from the call location.</para>
    /// </summary>
    /// <param name="unityEvent">Event to modify.</param>
    /// <param name="call">Function to call.</param>
    public static void AddPersistentListener(UnityEventBase unityEvent)
    {
      unityEvent.AddPersistentListener();
    }

    /// <summary>
    ///   <para>Removes the given function from the event.</para>
    /// </summary>
    /// <param name="unityEvent">Event to modify.</param>
    /// <param name="index">Index to remove (if specified).</param>
    /// <param name="call">Function to remove (if specified).</param>
    public static void RemovePersistentListener(UnityEventBase unityEvent, int index)
    {
      unityEvent.RemovePersistentListener(index);
    }

    /// <summary>
    ///   <para>Adds a persistent, call to the listener. Will be invoked with the arguments as defined by the Event and sent from the call location.</para>
    /// </summary>
    /// <param name="unityEvent">Event to modify.</param>
    /// <param name="call">Function to call.</param>
    public static void AddPersistentListener(UnityEvent unityEvent, UnityAction call)
    {
      unityEvent.AddPersistentListener(call);
    }

    public static void AddPersistentListener<T0>(UnityEvent<T0> unityEvent, UnityAction<T0> call)
    {
      unityEvent.AddPersistentListener(call);
    }

    public static void AddPersistentListener<T0, T1>(UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> call)
    {
      unityEvent.AddPersistentListener(call);
    }

    public static void AddPersistentListener<T0, T1, T2>(UnityEvent<T0, T1, T2> unityEvent, UnityAction<T0, T1, T2> call)
    {
      unityEvent.AddPersistentListener(call);
    }

    public static void AddPersistentListener<T0, T1, T2, T3>(UnityEvent<T0, T1, T2, T3> unityEvent, UnityAction<T0, T1, T2, T3> call)
    {
      unityEvent.AddPersistentListener(call);
    }

    /// <summary>
    ///   <para>Modifies the event at the given index.</para>
    /// </summary>
    /// <param name="unityEvent">Event to modify.</param>
    /// <param name="index">Index to modify.</param>
    /// <param name="call">Function to call.</param>
    public static void RegisterPersistentListener(UnityEvent unityEvent, int index, UnityAction call)
    {
      unityEvent.RegisterPersistentListener(index, call);
    }

    public static void RegisterPersistentListener<T0>(UnityEvent<T0> unityEvent, int index, UnityAction<T0> call)
    {
      unityEvent.RegisterPersistentListener(index, call);
    }

    public static void RegisterPersistentListener<T0, T1>(UnityEvent<T0, T1> unityEvent, int index, UnityAction<T0, T1> call)
    {
      unityEvent.RegisterPersistentListener(index, call);
    }

    public static void RegisterPersistentListener<T0, T1, T2>(UnityEvent<T0, T1, T2> unityEvent, int index, UnityAction<T0, T1, T2> call)
    {
      unityEvent.RegisterPersistentListener(index, call);
    }

    public static void RegisterPersistentListener<T0, T1, T2, T3>(UnityEvent<T0, T1, T2, T3> unityEvent, int index, UnityAction<T0, T1, T2, T3> call)
    {
      unityEvent.RegisterPersistentListener(index, call);
    }

    /// <summary>
    ///   <para>Removes the given function from the event.</para>
    /// </summary>
    /// <param name="unityEvent">Event to modify.</param>
    /// <param name="index">Index to remove (if specified).</param>
    /// <param name="call">Function to remove (if specified).</param>
    public static void RemovePersistentListener(UnityEventBase unityEvent, UnityAction call)
    {
      unityEvent.RemovePersistentListener(call.Target as Object, call.Method);
    }

    public static void RemovePersistentListener<T0>(UnityEventBase unityEvent, UnityAction<T0> call)
    {
      unityEvent.RemovePersistentListener(call.Target as Object, call.Method);
    }

    public static void RemovePersistentListener<T0, T1>(UnityEventBase unityEvent, UnityAction<T0, T1> call)
    {
      unityEvent.RemovePersistentListener(call.Target as Object, call.Method);
    }

    public static void RemovePersistentListener<T0, T1, T2>(UnityEventBase unityEvent, UnityAction<T0, T1, T2> call)
    {
      unityEvent.RemovePersistentListener(call.Target as Object, call.Method);
    }

    public static void RemovePersistentListener<T0, T1, T2, T3>(UnityEventBase unityEvent, UnityAction<T0, T1, T2, T3> call)
    {
      unityEvent.RemovePersistentListener(call.Target as Object, call.Method);
    }

    /// <summary>
    ///   <para>Unregisters the given listener at the specified index.</para>
    /// </summary>
    /// <param name="unityEvent">Event to modify.</param>
    /// <param name="index">Index to unregister.</param>
    public static void UnregisterPersistentListener(UnityEventBase unityEvent, int index)
    {
      unityEvent.UnregisterPersistentListener(index);
    }

    /// <summary>
    ///   <para>Adds a persistent, preset call to the listener.</para>
    /// </summary>
    /// <param name="unityEvent">Event to modify.</param>
    /// <param name="call">Function to call.</param>
    public static void AddVoidPersistentListener(UnityEventBase unityEvent, UnityAction call)
    {
      unityEvent.AddVoidPersistentListener(call);
    }

    /// <summary>
    ///   <para>Modifies the event at the given index.</para>
    /// </summary>
    /// <param name="unityEvent">Event to modify.</param>
    /// <param name="index">Index to modify.</param>
    /// <param name="call">Function to call.</param>
    public static void RegisterVoidPersistentListener(UnityEventBase unityEvent, int index, UnityAction call)
    {
      unityEvent.RegisterVoidPersistentListener(index, call);
    }

    public static void AddIntPersistentListener(UnityEventBase unityEvent, UnityAction<int> call, int argument)
    {
      unityEvent.AddIntPersistentListener(call, argument);
    }

    public static void RegisterIntPersistentListener(UnityEventBase unityEvent, int index, UnityAction<int> call, int argument)
    {
      unityEvent.RegisterIntPersistentListener(index, call, argument);
    }

    public static void AddFloatPersistentListener(UnityEventBase unityEvent, UnityAction<float> call, float argument)
    {
      unityEvent.AddFloatPersistentListener(call, argument);
    }

    public static void RegisterFloatPersistentListener(UnityEventBase unityEvent, int index, UnityAction<float> call, float argument)
    {
      unityEvent.RegisterFloatPersistentListener(index, call, argument);
    }

    public static void AddBoolPersistentListener(UnityEventBase unityEvent, UnityAction<bool> call, bool argument)
    {
      unityEvent.AddBoolPersistentListener(call, argument);
    }

    public static void RegisterBoolPersistentListener(UnityEventBase unityEvent, int index, UnityAction<bool> call, bool argument)
    {
      unityEvent.RegisterBoolPersistentListener(index, call, argument);
    }

    public static void AddStringPersistentListener(UnityEventBase unityEvent, UnityAction<string> call, string argument)
    {
      unityEvent.AddStringPersistentListener(call, argument);
    }

    public static void RegisterStringPersistentListener(UnityEventBase unityEvent, int index, UnityAction<string> call, string argument)
    {
      unityEvent.RegisterStringPersistentListener(index, call, argument);
    }

    public static void AddObjectPersistentListener<T>(UnityEventBase unityEvent, UnityAction<T> call, T argument) where T : Object
    {
      unityEvent.AddObjectPersistentListener<T>(call, argument);
    }

    public static void RegisterObjectPersistentListener<T>(UnityEventBase unityEvent, int index, UnityAction<T> call, T argument) where T : Object
    {
      unityEvent.RegisterObjectPersistentListener<T>(index, call, argument);
    }
  }
}

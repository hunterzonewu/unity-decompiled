// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.ExecuteEvents
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>Helper class that can be used to send IEventSystemHandler events to GameObjects.</para>
  /// </summary>
  public static class ExecuteEvents
  {
    private static readonly ExecuteEvents.EventFunction<IPointerEnterHandler> s_PointerEnterHandler = new ExecuteEvents.EventFunction<IPointerEnterHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IPointerExitHandler> s_PointerExitHandler = new ExecuteEvents.EventFunction<IPointerExitHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IPointerDownHandler> s_PointerDownHandler = new ExecuteEvents.EventFunction<IPointerDownHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IPointerUpHandler> s_PointerUpHandler = new ExecuteEvents.EventFunction<IPointerUpHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IPointerClickHandler> s_PointerClickHandler = new ExecuteEvents.EventFunction<IPointerClickHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IInitializePotentialDragHandler> s_InitializePotentialDragHandler = new ExecuteEvents.EventFunction<IInitializePotentialDragHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IBeginDragHandler> s_BeginDragHandler = new ExecuteEvents.EventFunction<IBeginDragHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IDragHandler> s_DragHandler = new ExecuteEvents.EventFunction<IDragHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IEndDragHandler> s_EndDragHandler = new ExecuteEvents.EventFunction<IEndDragHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IDropHandler> s_DropHandler = new ExecuteEvents.EventFunction<IDropHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IScrollHandler> s_ScrollHandler = new ExecuteEvents.EventFunction<IScrollHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IUpdateSelectedHandler> s_UpdateSelectedHandler = new ExecuteEvents.EventFunction<IUpdateSelectedHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<ISelectHandler> s_SelectHandler = new ExecuteEvents.EventFunction<ISelectHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IDeselectHandler> s_DeselectHandler = new ExecuteEvents.EventFunction<IDeselectHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<IMoveHandler> s_MoveHandler = new ExecuteEvents.EventFunction<IMoveHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<ISubmitHandler> s_SubmitHandler = new ExecuteEvents.EventFunction<ISubmitHandler>(ExecuteEvents.Execute);
    private static readonly ExecuteEvents.EventFunction<ICancelHandler> s_CancelHandler = new ExecuteEvents.EventFunction<ICancelHandler>(ExecuteEvents.Execute);
    private static readonly ObjectPool<List<IEventSystemHandler>> s_HandlerListPool = new ObjectPool<List<IEventSystemHandler>>((UnityAction<List<IEventSystemHandler>>) null, (UnityAction<List<IEventSystemHandler>>) (l => l.Clear()));
    private static readonly List<Transform> s_InternalTransformList = new List<Transform>(30);

    /// <summary>
    ///   <para>IPointerEnterHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IPointerEnterHandler> pointerEnterHandler
    {
      get
      {
        return ExecuteEvents.s_PointerEnterHandler;
      }
    }

    /// <summary>
    ///   <para>IPointerExitHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IPointerExitHandler> pointerExitHandler
    {
      get
      {
        return ExecuteEvents.s_PointerExitHandler;
      }
    }

    /// <summary>
    ///   <para>IPointerDownHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IPointerDownHandler> pointerDownHandler
    {
      get
      {
        return ExecuteEvents.s_PointerDownHandler;
      }
    }

    /// <summary>
    ///   <para>IPointerUpHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IPointerUpHandler> pointerUpHandler
    {
      get
      {
        return ExecuteEvents.s_PointerUpHandler;
      }
    }

    /// <summary>
    ///   <para>IPointerClickHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IPointerClickHandler> pointerClickHandler
    {
      get
      {
        return ExecuteEvents.s_PointerClickHandler;
      }
    }

    /// <summary>
    ///   <para>IInitializePotentialDragHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IInitializePotentialDragHandler> initializePotentialDrag
    {
      get
      {
        return ExecuteEvents.s_InitializePotentialDragHandler;
      }
    }

    /// <summary>
    ///   <para>IBeginDragHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IBeginDragHandler> beginDragHandler
    {
      get
      {
        return ExecuteEvents.s_BeginDragHandler;
      }
    }

    /// <summary>
    ///   <para>IDragHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IDragHandler> dragHandler
    {
      get
      {
        return ExecuteEvents.s_DragHandler;
      }
    }

    /// <summary>
    ///   <para>IEndDragHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IEndDragHandler> endDragHandler
    {
      get
      {
        return ExecuteEvents.s_EndDragHandler;
      }
    }

    /// <summary>
    ///   <para>IDropHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IDropHandler> dropHandler
    {
      get
      {
        return ExecuteEvents.s_DropHandler;
      }
    }

    /// <summary>
    ///   <para>IScrollHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IScrollHandler> scrollHandler
    {
      get
      {
        return ExecuteEvents.s_ScrollHandler;
      }
    }

    /// <summary>
    ///   <para>IUpdateSelectedHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IUpdateSelectedHandler> updateSelectedHandler
    {
      get
      {
        return ExecuteEvents.s_UpdateSelectedHandler;
      }
    }

    /// <summary>
    ///   <para>ISelectHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<ISelectHandler> selectHandler
    {
      get
      {
        return ExecuteEvents.s_SelectHandler;
      }
    }

    /// <summary>
    ///   <para>IDeselectHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IDeselectHandler> deselectHandler
    {
      get
      {
        return ExecuteEvents.s_DeselectHandler;
      }
    }

    /// <summary>
    ///   <para>IMoveHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<IMoveHandler> moveHandler
    {
      get
      {
        return ExecuteEvents.s_MoveHandler;
      }
    }

    /// <summary>
    ///   <para>ISubmitHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<ISubmitHandler> submitHandler
    {
      get
      {
        return ExecuteEvents.s_SubmitHandler;
      }
    }

    /// <summary>
    ///   <para>ICancelHandler execute helper function.</para>
    /// </summary>
    public static ExecuteEvents.EventFunction<ICancelHandler> cancelHandler
    {
      get
      {
        return ExecuteEvents.s_CancelHandler;
      }
    }

    public static T ValidateEventData<T>(BaseEventData data) where T : class
    {
      if ((object) ((object) data as T) == null)
        throw new ArgumentException(string.Format("Invalid type: {0} passed to event expecting {1}", (object) data.GetType(), (object) typeof (T)));
      return (object) data as T;
    }

    private static void Execute(IPointerEnterHandler handler, BaseEventData eventData)
    {
      handler.OnPointerEnter(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IPointerExitHandler handler, BaseEventData eventData)
    {
      handler.OnPointerExit(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IPointerDownHandler handler, BaseEventData eventData)
    {
      handler.OnPointerDown(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IPointerUpHandler handler, BaseEventData eventData)
    {
      handler.OnPointerUp(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IPointerClickHandler handler, BaseEventData eventData)
    {
      handler.OnPointerClick(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IInitializePotentialDragHandler handler, BaseEventData eventData)
    {
      handler.OnInitializePotentialDrag(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IBeginDragHandler handler, BaseEventData eventData)
    {
      handler.OnBeginDrag(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IDragHandler handler, BaseEventData eventData)
    {
      handler.OnDrag(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IEndDragHandler handler, BaseEventData eventData)
    {
      handler.OnEndDrag(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IDropHandler handler, BaseEventData eventData)
    {
      handler.OnDrop(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IScrollHandler handler, BaseEventData eventData)
    {
      handler.OnScroll(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
    }

    private static void Execute(IUpdateSelectedHandler handler, BaseEventData eventData)
    {
      handler.OnUpdateSelected(eventData);
    }

    private static void Execute(ISelectHandler handler, BaseEventData eventData)
    {
      handler.OnSelect(eventData);
    }

    private static void Execute(IDeselectHandler handler, BaseEventData eventData)
    {
      handler.OnDeselect(eventData);
    }

    private static void Execute(IMoveHandler handler, BaseEventData eventData)
    {
      handler.OnMove(ExecuteEvents.ValidateEventData<AxisEventData>(eventData));
    }

    private static void Execute(ISubmitHandler handler, BaseEventData eventData)
    {
      handler.OnSubmit(eventData);
    }

    private static void Execute(ICancelHandler handler, BaseEventData eventData)
    {
      handler.OnCancel(eventData);
    }

    private static void GetEventChain(GameObject root, IList<Transform> eventChain)
    {
      eventChain.Clear();
      if ((UnityEngine.Object) root == (UnityEngine.Object) null)
        return;
      for (Transform transform = root.transform; (UnityEngine.Object) transform != (UnityEngine.Object) null; transform = transform.parent)
        eventChain.Add(transform);
    }

    public static bool Execute<T>(GameObject target, BaseEventData eventData, ExecuteEvents.EventFunction<T> functor) where T : IEventSystemHandler
    {
      List<IEventSystemHandler> element = ExecuteEvents.s_HandlerListPool.Get();
      ExecuteEvents.GetEventList<T>(target, (IList<IEventSystemHandler>) element);
      for (int index = 0; index < element.Count; ++index)
      {
        T handler;
        try
        {
          handler = (T) element[index];
        }
        catch (Exception ex)
        {
          Debug.LogException(new Exception(string.Format("Type {0} expected {1} received.", (object) typeof (T).Name, (object) element[index].GetType().Name), ex));
          continue;
        }
        try
        {
          functor(handler, eventData);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
      }
      int count = element.Count;
      ExecuteEvents.s_HandlerListPool.Release(element);
      return count > 0;
    }

    public static GameObject ExecuteHierarchy<T>(GameObject root, BaseEventData eventData, ExecuteEvents.EventFunction<T> callbackFunction) where T : IEventSystemHandler
    {
      ExecuteEvents.GetEventChain(root, (IList<Transform>) ExecuteEvents.s_InternalTransformList);
      for (int index = 0; index < ExecuteEvents.s_InternalTransformList.Count; ++index)
      {
        Transform internalTransform = ExecuteEvents.s_InternalTransformList[index];
        if (ExecuteEvents.Execute<T>(internalTransform.gameObject, eventData, callbackFunction))
          return internalTransform.gameObject;
      }
      return (GameObject) null;
    }

    private static bool ShouldSendToComponent<T>(Component component) where T : IEventSystemHandler
    {
      if (!(component is T))
        return false;
      Behaviour behaviour = component as Behaviour;
      if ((UnityEngine.Object) behaviour != (UnityEngine.Object) null)
        return behaviour.isActiveAndEnabled;
      return true;
    }

    private static void GetEventList<T>(GameObject go, IList<IEventSystemHandler> results) where T : IEventSystemHandler
    {
      if (results == null)
        throw new ArgumentException("Results array is null", "results");
      if ((UnityEngine.Object) go == (UnityEngine.Object) null || !go.activeInHierarchy)
        return;
      List<Component> componentList = ListPool<Component>.Get();
      go.GetComponents<Component>(componentList);
      for (int index = 0; index < componentList.Count; ++index)
      {
        if (ExecuteEvents.ShouldSendToComponent<T>(componentList[index]))
          results.Add(componentList[index] as IEventSystemHandler);
      }
      ListPool<Component>.Release(componentList);
    }

    public static bool CanHandleEvent<T>(GameObject go) where T : IEventSystemHandler
    {
      List<IEventSystemHandler> element = ExecuteEvents.s_HandlerListPool.Get();
      ExecuteEvents.GetEventList<T>(go, (IList<IEventSystemHandler>) element);
      int count = element.Count;
      ExecuteEvents.s_HandlerListPool.Release(element);
      return count != 0;
    }

    public static GameObject GetEventHandler<T>(GameObject root) where T : IEventSystemHandler
    {
      if ((UnityEngine.Object) root == (UnityEngine.Object) null)
        return (GameObject) null;
      for (Transform transform = root.transform; (UnityEngine.Object) transform != (UnityEngine.Object) null; transform = transform.parent)
      {
        if (ExecuteEvents.CanHandleEvent<T>(transform.gameObject))
          return transform.gameObject;
      }
      return (GameObject) null;
    }

    public delegate void EventFunction<T1>(T1 handler, BaseEventData eventData);
  }
}

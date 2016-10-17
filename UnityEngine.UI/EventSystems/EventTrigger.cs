// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.EventTrigger
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>Receives events from the EventSystem and calls registered functions for each event.</para>
  /// </summary>
  [AddComponentMenu("Event/Event Trigger")]
  public class EventTrigger : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler
  {
    [SerializeField]
    [FormerlySerializedAs("delegates")]
    private List<EventTrigger.Entry> m_Delegates;
    /// <summary>
    ///   <para>All the functions registered in this EventTrigger.</para>
    /// </summary>
    [Obsolete("Please use triggers instead (UnityUpgradable) -> triggers", true)]
    public List<EventTrigger.Entry> delegates;

    /// <summary>
    ///   <para>All the functions registered in this EventTrigger.</para>
    /// </summary>
    public List<EventTrigger.Entry> triggers
    {
      get
      {
        if (this.m_Delegates == null)
          this.m_Delegates = new List<EventTrigger.Entry>();
        return this.m_Delegates;
      }
      set
      {
        this.m_Delegates = value;
      }
    }

    protected EventTrigger()
    {
    }

    private void Execute(EventTriggerType id, BaseEventData eventData)
    {
      int index = 0;
      for (int count = this.triggers.Count; index < count; ++index)
      {
        EventTrigger.Entry trigger = this.triggers[index];
        if (trigger.eventID == id && trigger.callback != null)
          trigger.callback.Invoke(eventData);
      }
    }

    /// <summary>
    ///   <para>See IPointerEnterHandler.OnPointerEnter.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.PointerEnter, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>See IPointerExitHandler.OnPointerExit.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.PointerExit, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>See IDragHandler.OnDrag.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnDrag(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.Drag, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>See IDropHandler.OnDrop.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnDrop(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.Drop, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>See IPointerDownHandler.OnPointerDown.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.PointerDown, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>See IPointerUpHandler.OnPointerUp.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.PointerUp, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>See IPointerClickHandler.OnPointerClick.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.PointerClick, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>See ISelectHandler.OnSelect.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnSelect(BaseEventData eventData)
    {
      this.Execute(EventTriggerType.Select, eventData);
    }

    /// <summary>
    ///   <para>See IDeselectHandler.OnDeselect.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnDeselect(BaseEventData eventData)
    {
      this.Execute(EventTriggerType.Deselect, eventData);
    }

    /// <summary>
    ///   <para>See IScrollHandler.OnScroll.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnScroll(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.Scroll, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>See IMoveHandler.OnMove.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnMove(AxisEventData eventData)
    {
      this.Execute(EventTriggerType.Move, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>See IUpdateSelectedHandler.OnUpdateSelected.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnUpdateSelected(BaseEventData eventData)
    {
      this.Execute(EventTriggerType.UpdateSelected, eventData);
    }

    /// <summary>
    ///   <para>Called by a BaseInputModule when a drag has been found but before it is valid to begin the drag.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnInitializePotentialDrag(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.InitializePotentialDrag, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>See IBeginDragHandler.OnBeginDrag.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.BeginDrag, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>See See IBeginDragHandler.OnEndDrag.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnEndDrag(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.EndDrag, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>See ISubmitHandler.OnSubmit.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnSubmit(BaseEventData eventData)
    {
      this.Execute(EventTriggerType.Submit, eventData);
    }

    /// <summary>
    ///   <para>See ICancelHandler.OnCancel.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnCancel(BaseEventData eventData)
    {
      this.Execute(EventTriggerType.Cancel, eventData);
    }

    /// <summary>
    ///   <para>UnityEvent class for Triggers.</para>
    /// </summary>
    [Serializable]
    public class TriggerEvent : UnityEvent<BaseEventData>
    {
    }

    /// <summary>
    ///   <para>An Entry in the EventSystem delegates list.</para>
    /// </summary>
    [Serializable]
    public class Entry
    {
      /// <summary>
      ///   <para>What type of event is the associated callback listening for.</para>
      /// </summary>
      public EventTriggerType eventID = EventTriggerType.PointerClick;
      /// <summary>
      ///   <para>The desired UnityEvent to be Invoked.</para>
      /// </summary>
      public EventTrigger.TriggerEvent callback = new EventTrigger.TriggerEvent();
    }
  }
}

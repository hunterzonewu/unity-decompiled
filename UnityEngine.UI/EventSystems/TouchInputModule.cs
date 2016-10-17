// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.TouchInputModule
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>Input module used for touch based input.</para>
  /// </summary>
  [AddComponentMenu("Event/Touch Input Module")]
  [Obsolete("TouchInputModule is no longer required as Touch input is now handled in StandaloneInputModule.")]
  public class TouchInputModule : PointerInputModule
  {
    private Vector2 m_LastMousePosition;
    private Vector2 m_MousePosition;
    [SerializeField]
    [FormerlySerializedAs("m_AllowActivationOnStandalone")]
    private bool m_ForceModuleActive;

    /// <summary>
    ///   <para>Can this module be activated on a standalone platform?</para>
    /// </summary>
    [Obsolete("allowActivationOnStandalone has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
    public bool allowActivationOnStandalone
    {
      get
      {
        return this.m_ForceModuleActive;
      }
      set
      {
        this.m_ForceModuleActive = value;
      }
    }

    /// <summary>
    ///   <para>Force this module to be active.</para>
    /// </summary>
    public bool forceModuleActive
    {
      get
      {
        return this.m_ForceModuleActive;
      }
      set
      {
        this.m_ForceModuleActive = value;
      }
    }

    protected TouchInputModule()
    {
    }

    /// <summary>
    ///   <para>See BaseInputModule.</para>
    /// </summary>
    public override void UpdateModule()
    {
      this.m_LastMousePosition = this.m_MousePosition;
      this.m_MousePosition = (Vector2) Input.mousePosition;
    }

    /// <summary>
    ///   <para>See BaseInputModule.</para>
    /// </summary>
    /// <returns>
    ///   <para>Supported.</para>
    /// </returns>
    public override bool IsModuleSupported()
    {
      if (!this.forceModuleActive)
        return Input.touchSupported;
      return true;
    }

    /// <summary>
    ///   <para>See BaseInputModule.</para>
    /// </summary>
    /// <returns>
    ///   <para>Should activate.</para>
    /// </returns>
    public override bool ShouldActivateModule()
    {
      if (!base.ShouldActivateModule())
        return false;
      if (this.m_ForceModuleActive)
        return true;
      if (this.UseFakeInput())
        return Input.GetMouseButtonDown(0) | (double) (this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0.0;
      return Input.touchCount > 0;
    }

    private bool UseFakeInput()
    {
      return !Input.touchSupported;
    }

    /// <summary>
    ///   <para>See BaseInputModule.</para>
    /// </summary>
    public override void Process()
    {
      if (this.UseFakeInput())
        this.FakeTouches();
      else
        this.ProcessTouchEvents();
    }

    private void FakeTouches()
    {
      PointerInputModule.MouseButtonEventData eventData = this.GetMousePointerEventData(0).GetButtonState(PointerEventData.InputButton.Left).eventData;
      if (eventData.PressedThisFrame())
        eventData.buttonData.delta = Vector2.zero;
      this.ProcessTouchPress(eventData.buttonData, eventData.PressedThisFrame(), eventData.ReleasedThisFrame());
      if (!Input.GetMouseButton(0))
        return;
      this.ProcessMove(eventData.buttonData);
      this.ProcessDrag(eventData.buttonData);
    }

    private void ProcessTouchEvents()
    {
      for (int index = 0; index < Input.touchCount; ++index)
      {
        Touch touch = Input.GetTouch(index);
        if (touch.type != TouchType.Indirect)
        {
          bool pressed;
          bool released;
          PointerEventData pointerEventData = this.GetTouchPointerEventData(touch, out pressed, out released);
          this.ProcessTouchPress(pointerEventData, pressed, released);
          if (!released)
          {
            this.ProcessMove(pointerEventData);
            this.ProcessDrag(pointerEventData);
          }
          else
            this.RemovePointerData(pointerEventData);
        }
      }
    }

    private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
    {
      GameObject gameObject1 = pointerEvent.pointerCurrentRaycast.gameObject;
      if (pressed)
      {
        pointerEvent.eligibleForClick = true;
        pointerEvent.delta = Vector2.zero;
        pointerEvent.dragging = false;
        pointerEvent.useDragThreshold = true;
        pointerEvent.pressPosition = pointerEvent.position;
        pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
        this.DeselectIfSelectionChanged(gameObject1, (BaseEventData) pointerEvent);
        if ((UnityEngine.Object) pointerEvent.pointerEnter != (UnityEngine.Object) gameObject1)
        {
          this.HandlePointerExitAndEnter(pointerEvent, gameObject1);
          pointerEvent.pointerEnter = gameObject1;
        }
        GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData) pointerEvent, ExecuteEvents.pointerDownHandler);
        if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null)
          gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
        float unscaledTime = Time.unscaledTime;
        if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) pointerEvent.lastPress)
        {
          if ((double) (unscaledTime - pointerEvent.clickTime) < 0.300000011920929)
            ++pointerEvent.clickCount;
          else
            pointerEvent.clickCount = 1;
          pointerEvent.clickTime = unscaledTime;
        }
        else
          pointerEvent.clickCount = 1;
        pointerEvent.pointerPress = gameObject2;
        pointerEvent.rawPointerPress = gameObject1;
        pointerEvent.clickTime = unscaledTime;
        pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1);
        if ((UnityEngine.Object) pointerEvent.pointerDrag != (UnityEngine.Object) null)
          ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, (BaseEventData) pointerEvent, ExecuteEvents.initializePotentialDrag);
      }
      if (!released)
        return;
      ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, (BaseEventData) pointerEvent, ExecuteEvents.pointerUpHandler);
      GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
      if ((UnityEngine.Object) pointerEvent.pointerPress == (UnityEngine.Object) eventHandler && pointerEvent.eligibleForClick)
        ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, (BaseEventData) pointerEvent, ExecuteEvents.pointerClickHandler);
      else if ((UnityEngine.Object) pointerEvent.pointerDrag != (UnityEngine.Object) null && pointerEvent.dragging)
        ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject1, (BaseEventData) pointerEvent, ExecuteEvents.dropHandler);
      pointerEvent.eligibleForClick = false;
      pointerEvent.pointerPress = (GameObject) null;
      pointerEvent.rawPointerPress = (GameObject) null;
      if ((UnityEngine.Object) pointerEvent.pointerDrag != (UnityEngine.Object) null && pointerEvent.dragging)
        ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, (BaseEventData) pointerEvent, ExecuteEvents.endDragHandler);
      pointerEvent.dragging = false;
      pointerEvent.pointerDrag = (GameObject) null;
      if ((UnityEngine.Object) pointerEvent.pointerDrag != (UnityEngine.Object) null)
        ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, (BaseEventData) pointerEvent, ExecuteEvents.endDragHandler);
      pointerEvent.pointerDrag = (GameObject) null;
      ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, (BaseEventData) pointerEvent, ExecuteEvents.pointerExitHandler);
      pointerEvent.pointerEnter = (GameObject) null;
    }

    /// <summary>
    ///   <para>See BaseInputModule.</para>
    /// </summary>
    public override void DeactivateModule()
    {
      base.DeactivateModule();
      this.ClearSelection();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(!this.UseFakeInput() ? "Input: Touch" : "Input: Faked");
      if (this.UseFakeInput())
      {
        PointerEventData pointerEventData = this.GetLastPointerEventData(-1);
        if (pointerEventData != null)
          stringBuilder.AppendLine(pointerEventData.ToString());
      }
      else
      {
        using (Dictionary<int, PointerEventData>.Enumerator enumerator = this.m_PointerData.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, PointerEventData> current = enumerator.Current;
            stringBuilder.AppendLine(current.ToString());
          }
        }
      }
      return stringBuilder.ToString();
    }
  }
}

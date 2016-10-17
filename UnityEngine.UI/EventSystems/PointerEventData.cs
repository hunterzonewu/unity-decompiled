// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.PointerEventData
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>Event payload associated with pointer (mouse / touch) events.</para>
  /// </summary>
  public class PointerEventData : BaseEventData
  {
    /// <summary>
    ///   <para>List of objects in the hover stack.</para>
    /// </summary>
    public List<GameObject> hovered = new List<GameObject>();
    private GameObject m_PointerPress;

    /// <summary>
    ///   <para>The object that received 'OnPointerEnter'.</para>
    /// </summary>
    public GameObject pointerEnter { get; set; }

    /// <summary>
    ///   <para>The GameObject for the last press event.</para>
    /// </summary>
    public GameObject lastPress { get; private set; }

    /// <summary>
    ///   <para>The object that the press happened on even if it can not handle the press event.</para>
    /// </summary>
    public GameObject rawPointerPress { get; set; }

    /// <summary>
    ///   <para>The object that is receiving 'OnDrag'.</para>
    /// </summary>
    public GameObject pointerDrag { get; set; }

    /// <summary>
    ///   <para>RaycastResult associated with the current event.</para>
    /// </summary>
    public RaycastResult pointerCurrentRaycast { get; set; }

    /// <summary>
    ///   <para>RaycastResult associated with the pointer press.</para>
    /// </summary>
    public RaycastResult pointerPressRaycast { get; set; }

    public bool eligibleForClick { get; set; }

    /// <summary>
    ///   <para>Id of the pointer (touch id).</para>
    /// </summary>
    public int pointerId { get; set; }

    /// <summary>
    ///   <para>Current pointer position.</para>
    /// </summary>
    public Vector2 position { get; set; }

    /// <summary>
    ///   <para>Pointer delta since last update.</para>
    /// </summary>
    public Vector2 delta { get; set; }

    /// <summary>
    ///   <para>Position of the press.</para>
    /// </summary>
    public Vector2 pressPosition { get; set; }

    [Obsolete("Use either pointerCurrentRaycast.worldPosition or pointerPressRaycast.worldPosition")]
    public Vector3 worldPosition { get; set; }

    [Obsolete("Use either pointerCurrentRaycast.worldNormal or pointerPressRaycast.worldNormal")]
    public Vector3 worldNormal { get; set; }

    /// <summary>
    ///   <para>The last time a click event was sent.</para>
    /// </summary>
    public float clickTime { get; set; }

    /// <summary>
    ///   <para>Number of clicks in a row.</para>
    /// </summary>
    public int clickCount { get; set; }

    /// <summary>
    ///   <para>The amount of scroll since the last update.</para>
    /// </summary>
    public Vector2 scrollDelta { get; set; }

    /// <summary>
    ///   <para>Should a drag threshold be used?</para>
    /// </summary>
    public bool useDragThreshold { get; set; }

    /// <summary>
    ///   <para>Is a drag operation currently occuring.</para>
    /// </summary>
    public bool dragging { get; set; }

    /// <summary>
    ///   <para>The EventSystems.PointerEventData.InputButton for this event.</para>
    /// </summary>
    public PointerEventData.InputButton button { get; set; }

    /// <summary>
    ///   <para>The camera associated with the last OnPointerEnter event.</para>
    /// </summary>
    public Camera enterEventCamera
    {
      get
      {
        if ((UnityEngine.Object) this.pointerCurrentRaycast.module == (UnityEngine.Object) null)
          return (Camera) null;
        return this.pointerCurrentRaycast.module.eventCamera;
      }
    }

    /// <summary>
    ///   <para>The camera associated with the last OnPointerPress event.</para>
    /// </summary>
    public Camera pressEventCamera
    {
      get
      {
        if ((UnityEngine.Object) this.pointerPressRaycast.module == (UnityEngine.Object) null)
          return (Camera) null;
        return this.pointerPressRaycast.module.eventCamera;
      }
    }

    /// <summary>
    ///   <para>The GameObject that received the OnPointerDown.</para>
    /// </summary>
    public GameObject pointerPress
    {
      get
      {
        return this.m_PointerPress;
      }
      set
      {
        if ((UnityEngine.Object) this.m_PointerPress == (UnityEngine.Object) value)
          return;
        this.lastPress = this.m_PointerPress;
        this.m_PointerPress = value;
      }
    }

    public PointerEventData(EventSystem eventSystem)
      : base(eventSystem)
    {
      this.eligibleForClick = false;
      this.pointerId = -1;
      this.position = Vector2.zero;
      this.delta = Vector2.zero;
      this.pressPosition = Vector2.zero;
      this.clickTime = 0.0f;
      this.clickCount = 0;
      this.scrollDelta = Vector2.zero;
      this.useDragThreshold = true;
      this.dragging = false;
      this.button = PointerEventData.InputButton.Left;
    }

    /// <summary>
    ///   <para>Is the pointer moving.</para>
    /// </summary>
    /// <returns>
    ///   <para>Moving.</para>
    /// </returns>
    public bool IsPointerMoving()
    {
      return (double) this.delta.sqrMagnitude > 0.0;
    }

    /// <summary>
    ///   <para>Is scroll being used on the input device.</para>
    /// </summary>
    /// <returns>
    ///   <para>Scrolling.</para>
    /// </returns>
    public bool IsScrolling()
    {
      return (double) this.scrollDelta.sqrMagnitude > 0.0;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("<b>Position</b>: " + (object) this.position);
      stringBuilder.AppendLine("<b>delta</b>: " + (object) this.delta);
      stringBuilder.AppendLine("<b>eligibleForClick</b>: " + (object) this.eligibleForClick);
      stringBuilder.AppendLine("<b>pointerEnter</b>: " + (object) this.pointerEnter);
      stringBuilder.AppendLine("<b>pointerPress</b>: " + (object) this.pointerPress);
      stringBuilder.AppendLine("<b>lastPointerPress</b>: " + (object) this.lastPress);
      stringBuilder.AppendLine("<b>pointerDrag</b>: " + (object) this.pointerDrag);
      stringBuilder.AppendLine("<b>Use Drag Threshold</b>: " + (object) this.useDragThreshold);
      stringBuilder.AppendLine("<b>Current Rayast:</b>");
      stringBuilder.AppendLine(this.pointerCurrentRaycast.ToString());
      stringBuilder.AppendLine("<b>Press Rayast:</b>");
      stringBuilder.AppendLine(this.pointerPressRaycast.ToString());
      return stringBuilder.ToString();
    }

    /// <summary>
    ///   <para>Input press tracking.</para>
    /// </summary>
    public enum InputButton
    {
      Left,
      Right,
      Middle,
    }

    /// <summary>
    ///   <para>The state of a press for the given frame.</para>
    /// </summary>
    public enum FramePressState
    {
      Pressed,
      Released,
      PressedAndReleased,
      NotChanged,
    }
  }
}

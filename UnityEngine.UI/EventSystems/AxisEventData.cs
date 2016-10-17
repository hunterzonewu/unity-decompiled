// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.AxisEventData
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>Event Data associated with Axis Events (Controller / Keyboard).</para>
  /// </summary>
  public class AxisEventData : BaseEventData
  {
    /// <summary>
    ///   <para>Raw input vector associated with this event.</para>
    /// </summary>
    public Vector2 moveVector { get; set; }

    /// <summary>
    ///   <para>MoveDirection for this event.</para>
    /// </summary>
    public MoveDirection moveDir { get; set; }

    public AxisEventData(EventSystem eventSystem)
      : base(eventSystem)
    {
      this.moveVector = Vector2.zero;
      this.moveDir = MoveDirection.None;
    }
  }
}

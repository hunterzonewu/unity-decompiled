// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.IDragHandler
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.EventSystems
{
  public interface IDragHandler : IEventSystemHandler
  {
    /// <summary>
    ///   <para>When draging is occuring this will be called every time the cursor is moved.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    void OnDrag(PointerEventData eventData);
  }
}

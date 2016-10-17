// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.IDropHandler
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.EventSystems
{
  public interface IDropHandler : IEventSystemHandler
  {
    /// <summary>
    ///   <para>Called by a BaseInputModule on a target that can accept a drop.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    void OnDrop(PointerEventData eventData);
  }
}

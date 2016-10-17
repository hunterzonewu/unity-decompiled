// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.IInitializePotentialDragHandler
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.EventSystems
{
  public interface IInitializePotentialDragHandler : IEventSystemHandler
  {
    /// <summary>
    ///   <para>Called by a BaseInputModule when a drag has been found but before it is valid to begin the drag.</para>
    /// </summary>
    /// <param name="eventData"></param>
    void OnInitializePotentialDrag(PointerEventData eventData);
  }
}

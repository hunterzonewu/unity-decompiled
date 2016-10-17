// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.EventSystem
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
  ///   <para>Handles input, raycasting, and sending events.</para>
  /// </summary>
  [AddComponentMenu("Event/Event System")]
  public class EventSystem : UIBehaviour
  {
    private static readonly Comparison<RaycastResult> s_RaycastComparer = new Comparison<RaycastResult>(EventSystem.RaycastComparer);
    private List<BaseInputModule> m_SystemInputModules = new List<BaseInputModule>();
    [SerializeField]
    private bool m_sendNavigationEvents = true;
    [SerializeField]
    private int m_DragThreshold = 5;
    private BaseInputModule m_CurrentInputModule;
    [SerializeField]
    [FormerlySerializedAs("m_Selected")]
    private GameObject m_FirstSelected;
    private GameObject m_CurrentSelected;
    private bool m_SelectionGuard;
    private BaseEventData m_DummyData;

    /// <summary>
    ///   <para>Return the current EventSystem.</para>
    /// </summary>
    public static EventSystem current { get; set; }

    /// <summary>
    ///   <para>Should the EventSystem allow navigation events (move  submit  cancel).</para>
    /// </summary>
    public bool sendNavigationEvents
    {
      get
      {
        return this.m_sendNavigationEvents;
      }
      set
      {
        this.m_sendNavigationEvents = value;
      }
    }

    /// <summary>
    ///   <para>The soft area for dragging in pixels.</para>
    /// </summary>
    public int pixelDragThreshold
    {
      get
      {
        return this.m_DragThreshold;
      }
      set
      {
        this.m_DragThreshold = value;
      }
    }

    /// <summary>
    ///   <para>The currently active EventSystems.BaseInputModule.</para>
    /// </summary>
    public BaseInputModule currentInputModule
    {
      get
      {
        return this.m_CurrentInputModule;
      }
    }

    /// <summary>
    ///   <para>The GameObject that was selected first.</para>
    /// </summary>
    public GameObject firstSelectedGameObject
    {
      get
      {
        return this.m_FirstSelected;
      }
      set
      {
        this.m_FirstSelected = value;
      }
    }

    /// <summary>
    ///   <para>The GameObject currently considered active by the EventSystem.</para>
    /// </summary>
    public GameObject currentSelectedGameObject
    {
      get
      {
        return this.m_CurrentSelected;
      }
    }

    [Obsolete("lastSelectedGameObject is no longer supported")]
    public GameObject lastSelectedGameObject
    {
      get
      {
        return (GameObject) null;
      }
    }

    /// <summary>
    ///   <para>Returns true if the EventSystem is already in a SetSelectedGameObject.</para>
    /// </summary>
    public bool alreadySelecting
    {
      get
      {
        return this.m_SelectionGuard;
      }
    }

    private BaseEventData baseEventDataCache
    {
      get
      {
        if (this.m_DummyData == null)
          this.m_DummyData = new BaseEventData(this);
        return this.m_DummyData;
      }
    }

    protected EventSystem()
    {
    }

    /// <summary>
    ///   <para>Recalculate the internal list of BaseInputModules.</para>
    /// </summary>
    public void UpdateModules()
    {
      this.GetComponents<BaseInputModule>(this.m_SystemInputModules);
      for (int index = this.m_SystemInputModules.Count - 1; index >= 0; --index)
      {
        if (!(bool) ((UnityEngine.Object) this.m_SystemInputModules[index]) || !this.m_SystemInputModules[index].IsActive())
          this.m_SystemInputModules.RemoveAt(index);
      }
    }

    /// <summary>
    ///   <para>Set the object as selected. Will send an OnDeselect the the old selected object and OnSelect to the new selected object.</para>
    /// </summary>
    /// <param name="selected">GameObject to select.</param>
    /// <param name="pointer">Associated EventData.</param>
    public void SetSelectedGameObject(GameObject selected, BaseEventData pointer)
    {
      if (this.m_SelectionGuard)
      {
        Debug.LogError((object) ("Attempting to select " + (object) selected + "while already selecting an object."));
      }
      else
      {
        this.m_SelectionGuard = true;
        if ((UnityEngine.Object) selected == (UnityEngine.Object) this.m_CurrentSelected)
        {
          this.m_SelectionGuard = false;
        }
        else
        {
          ExecuteEvents.Execute<IDeselectHandler>(this.m_CurrentSelected, pointer, ExecuteEvents.deselectHandler);
          this.m_CurrentSelected = selected;
          ExecuteEvents.Execute<ISelectHandler>(this.m_CurrentSelected, pointer, ExecuteEvents.selectHandler);
          this.m_SelectionGuard = false;
        }
      }
    }

    public void SetSelectedGameObject(GameObject selected)
    {
      this.SetSelectedGameObject(selected, this.baseEventDataCache);
    }

    private static int RaycastComparer(RaycastResult lhs, RaycastResult rhs)
    {
      if ((UnityEngine.Object) lhs.module != (UnityEngine.Object) rhs.module)
      {
        if ((UnityEngine.Object) lhs.module.eventCamera != (UnityEngine.Object) null && (UnityEngine.Object) rhs.module.eventCamera != (UnityEngine.Object) null && (double) lhs.module.eventCamera.depth != (double) rhs.module.eventCamera.depth)
        {
          if ((double) lhs.module.eventCamera.depth < (double) rhs.module.eventCamera.depth)
            return 1;
          return (double) lhs.module.eventCamera.depth == (double) rhs.module.eventCamera.depth ? 0 : -1;
        }
        if (lhs.module.sortOrderPriority != rhs.module.sortOrderPriority)
          return rhs.module.sortOrderPriority.CompareTo(lhs.module.sortOrderPriority);
        if (lhs.module.renderOrderPriority != rhs.module.renderOrderPriority)
          return rhs.module.renderOrderPriority.CompareTo(lhs.module.renderOrderPriority);
      }
      if (lhs.sortingLayer != rhs.sortingLayer)
        return SortingLayer.GetLayerValueFromID(rhs.sortingLayer).CompareTo(SortingLayer.GetLayerValueFromID(lhs.sortingLayer));
      if (lhs.sortingOrder != rhs.sortingOrder)
        return rhs.sortingOrder.CompareTo(lhs.sortingOrder);
      if (lhs.depth != rhs.depth)
        return rhs.depth.CompareTo(lhs.depth);
      if ((double) lhs.distance != (double) rhs.distance)
        return lhs.distance.CompareTo(rhs.distance);
      return lhs.index.CompareTo(rhs.index);
    }

    public void RaycastAll(PointerEventData eventData, List<RaycastResult> raycastResults)
    {
      raycastResults.Clear();
      List<BaseRaycaster> raycasters = RaycasterManager.GetRaycasters();
      for (int index = 0; index < raycasters.Count; ++index)
      {
        BaseRaycaster baseRaycaster = raycasters[index];
        if (!((UnityEngine.Object) baseRaycaster == (UnityEngine.Object) null) && baseRaycaster.IsActive())
          baseRaycaster.Raycast(eventData, raycastResults);
      }
      raycastResults.Sort(EventSystem.s_RaycastComparer);
    }

    /// <summary>
    ///   <para>Is the pointer with the given ID over an EventSystem object?</para>
    /// </summary>
    /// <param name="pointerId">Pointer (touch / mouse) ID.</param>
    public bool IsPointerOverGameObject()
    {
      return this.IsPointerOverGameObject(-1);
    }

    /// <summary>
    ///   <para>Is the pointer with the given ID over an EventSystem object?</para>
    /// </summary>
    /// <param name="pointerId">Pointer (touch / mouse) ID.</param>
    public bool IsPointerOverGameObject(int pointerId)
    {
      if ((UnityEngine.Object) this.m_CurrentInputModule == (UnityEngine.Object) null)
        return false;
      return this.m_CurrentInputModule.IsPointerOverGameObject(pointerId);
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      if ((UnityEngine.Object) EventSystem.current == (UnityEngine.Object) null)
        EventSystem.current = this;
      else
        Debug.LogWarning((object) "Multiple EventSystems in scene... this is not supported");
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected override void OnDisable()
    {
      if ((UnityEngine.Object) this.m_CurrentInputModule != (UnityEngine.Object) null)
      {
        this.m_CurrentInputModule.DeactivateModule();
        this.m_CurrentInputModule = (BaseInputModule) null;
      }
      if ((UnityEngine.Object) EventSystem.current == (UnityEngine.Object) this)
        EventSystem.current = (EventSystem) null;
      base.OnDisable();
    }

    private void TickModules()
    {
      for (int index = 0; index < this.m_SystemInputModules.Count; ++index)
      {
        if ((UnityEngine.Object) this.m_SystemInputModules[index] != (UnityEngine.Object) null)
          this.m_SystemInputModules[index].UpdateModule();
      }
    }

    protected virtual void Update()
    {
      if ((UnityEngine.Object) EventSystem.current != (UnityEngine.Object) this)
        return;
      this.TickModules();
      bool flag = false;
      for (int index = 0; index < this.m_SystemInputModules.Count; ++index)
      {
        BaseInputModule systemInputModule = this.m_SystemInputModules[index];
        if (systemInputModule.IsModuleSupported() && systemInputModule.ShouldActivateModule())
        {
          if ((UnityEngine.Object) this.m_CurrentInputModule != (UnityEngine.Object) systemInputModule)
          {
            this.ChangeEventModule(systemInputModule);
            flag = true;
            break;
          }
          break;
        }
      }
      if ((UnityEngine.Object) this.m_CurrentInputModule == (UnityEngine.Object) null)
      {
        for (int index = 0; index < this.m_SystemInputModules.Count; ++index)
        {
          BaseInputModule systemInputModule = this.m_SystemInputModules[index];
          if (systemInputModule.IsModuleSupported())
          {
            this.ChangeEventModule(systemInputModule);
            flag = true;
            break;
          }
        }
      }
      if (flag || !((UnityEngine.Object) this.m_CurrentInputModule != (UnityEngine.Object) null))
        return;
      this.m_CurrentInputModule.Process();
    }

    private void ChangeEventModule(BaseInputModule module)
    {
      if ((UnityEngine.Object) this.m_CurrentInputModule == (UnityEngine.Object) module)
        return;
      if ((UnityEngine.Object) this.m_CurrentInputModule != (UnityEngine.Object) null)
        this.m_CurrentInputModule.DeactivateModule();
      if ((UnityEngine.Object) module != (UnityEngine.Object) null)
        module.ActivateModule();
      this.m_CurrentInputModule = module;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("<b>Selected:</b>" + (object) this.currentSelectedGameObject);
      stringBuilder.AppendLine();
      stringBuilder.AppendLine();
      stringBuilder.AppendLine(!((UnityEngine.Object) this.m_CurrentInputModule != (UnityEngine.Object) null) ? "No module" : this.m_CurrentInputModule.ToString());
      return stringBuilder.ToString();
    }
  }
}

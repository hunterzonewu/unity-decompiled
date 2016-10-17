// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.CanvasUpdateRegistry
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>A place where CanvasElements can register themselves for rebuilding.</para>
  /// </summary>
  public class CanvasUpdateRegistry
  {
    private static readonly Comparison<ICanvasElement> s_SortLayoutFunction = new Comparison<ICanvasElement>(CanvasUpdateRegistry.SortLayoutList);
    private readonly IndexedSet<ICanvasElement> m_LayoutRebuildQueue = new IndexedSet<ICanvasElement>();
    private readonly IndexedSet<ICanvasElement> m_GraphicRebuildQueue = new IndexedSet<ICanvasElement>();
    private static CanvasUpdateRegistry s_Instance;
    private bool m_PerformingLayoutUpdate;
    private bool m_PerformingGraphicUpdate;

    /// <summary>
    ///   <para>Get the singleton registry.</para>
    /// </summary>
    public static CanvasUpdateRegistry instance
    {
      get
      {
        if (CanvasUpdateRegistry.s_Instance == null)
          CanvasUpdateRegistry.s_Instance = new CanvasUpdateRegistry();
        return CanvasUpdateRegistry.s_Instance;
      }
    }

    protected CanvasUpdateRegistry()
    {
      Canvas.willRenderCanvases += new Canvas.WillRenderCanvases(this.PerformUpdate);
    }

    private bool ObjectValidForUpdate(ICanvasElement element)
    {
      bool flag = element != null;
      if (element is UnityEngine.Object)
        flag = element as UnityEngine.Object != (UnityEngine.Object) null;
      return flag;
    }

    private void CleanInvalidItems()
    {
      for (int index = this.m_LayoutRebuildQueue.Count - 1; index >= 0; --index)
      {
        ICanvasElement layoutRebuild = this.m_LayoutRebuildQueue[index];
        if (layoutRebuild == null)
          this.m_LayoutRebuildQueue.RemoveAt(index);
        else if (layoutRebuild.IsDestroyed())
        {
          this.m_LayoutRebuildQueue.RemoveAt(index);
          layoutRebuild.LayoutComplete();
        }
      }
      for (int index = this.m_GraphicRebuildQueue.Count - 1; index >= 0; --index)
      {
        ICanvasElement graphicRebuild = this.m_GraphicRebuildQueue[index];
        if (graphicRebuild == null)
          this.m_GraphicRebuildQueue.RemoveAt(index);
        else if (graphicRebuild.IsDestroyed())
        {
          this.m_GraphicRebuildQueue.RemoveAt(index);
          graphicRebuild.GraphicUpdateComplete();
        }
      }
    }

    private void PerformUpdate()
    {
      this.CleanInvalidItems();
      this.m_PerformingLayoutUpdate = true;
      this.m_LayoutRebuildQueue.Sort(CanvasUpdateRegistry.s_SortLayoutFunction);
      for (int index1 = 0; index1 <= 2; ++index1)
      {
        for (int index2 = 0; index2 < this.m_LayoutRebuildQueue.Count; ++index2)
        {
          ICanvasElement layoutRebuild = CanvasUpdateRegistry.instance.m_LayoutRebuildQueue[index2];
          try
          {
            if (this.ObjectValidForUpdate(layoutRebuild))
              layoutRebuild.Rebuild((CanvasUpdate) index1);
          }
          catch (Exception ex)
          {
            Debug.LogException(ex, (UnityEngine.Object) layoutRebuild.transform);
          }
        }
      }
      for (int index = 0; index < this.m_LayoutRebuildQueue.Count; ++index)
        this.m_LayoutRebuildQueue[index].LayoutComplete();
      CanvasUpdateRegistry.instance.m_LayoutRebuildQueue.Clear();
      this.m_PerformingLayoutUpdate = false;
      ClipperRegistry.instance.Cull();
      this.m_PerformingGraphicUpdate = true;
      for (int index1 = 3; index1 < 5; ++index1)
      {
        for (int index2 = 0; index2 < CanvasUpdateRegistry.instance.m_GraphicRebuildQueue.Count; ++index2)
        {
          try
          {
            ICanvasElement graphicRebuild = CanvasUpdateRegistry.instance.m_GraphicRebuildQueue[index2];
            if (this.ObjectValidForUpdate(graphicRebuild))
              graphicRebuild.Rebuild((CanvasUpdate) index1);
          }
          catch (Exception ex)
          {
            Debug.LogException(ex, (UnityEngine.Object) CanvasUpdateRegistry.instance.m_GraphicRebuildQueue[index2].transform);
          }
        }
      }
      for (int index = 0; index < this.m_GraphicRebuildQueue.Count; ++index)
        this.m_GraphicRebuildQueue[index].LayoutComplete();
      CanvasUpdateRegistry.instance.m_GraphicRebuildQueue.Clear();
      this.m_PerformingGraphicUpdate = false;
    }

    private static int ParentCount(Transform child)
    {
      if ((UnityEngine.Object) child == (UnityEngine.Object) null)
        return 0;
      Transform parent = child.parent;
      int num = 0;
      for (; (UnityEngine.Object) parent != (UnityEngine.Object) null; parent = parent.parent)
        ++num;
      return num;
    }

    private static int SortLayoutList(ICanvasElement x, ICanvasElement y)
    {
      return CanvasUpdateRegistry.ParentCount(x.transform) - CanvasUpdateRegistry.ParentCount(y.transform);
    }

    /// <summary>
    ///   <para>Rebuild the layout of the given element.</para>
    /// </summary>
    /// <param name="element">Element to rebuild.</param>
    public static void RegisterCanvasElementForLayoutRebuild(ICanvasElement element)
    {
      CanvasUpdateRegistry.instance.InternalRegisterCanvasElementForLayoutRebuild(element);
    }

    /// <summary>
    ///   <para>Was the element scheduled.</para>
    /// </summary>
    /// <param name="element">Element to rebuild.</param>
    /// <returns>
    ///   <para>Was the element scheduled.</para>
    /// </returns>
    public static bool TryRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
    {
      return CanvasUpdateRegistry.instance.InternalRegisterCanvasElementForLayoutRebuild(element);
    }

    private bool InternalRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
    {
      if (this.m_LayoutRebuildQueue.Contains(element))
        return false;
      return this.m_LayoutRebuildQueue.AddUnique(element);
    }

    /// <summary>
    ///   <para>Rebuild the graphics of the given element.</para>
    /// </summary>
    /// <param name="element">Element to rebuild.</param>
    public static void RegisterCanvasElementForGraphicRebuild(ICanvasElement element)
    {
      CanvasUpdateRegistry.instance.InternalRegisterCanvasElementForGraphicRebuild(element);
    }

    /// <summary>
    ///   <para>Rebuild the layout of the given element.</para>
    /// </summary>
    /// <param name="element">Element to rebuild.</param>
    /// <returns>
    ///   <para>Was the element scheduled.</para>
    /// </returns>
    public static bool TryRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
    {
      return CanvasUpdateRegistry.instance.InternalRegisterCanvasElementForGraphicRebuild(element);
    }

    private bool InternalRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
    {
      if (!this.m_PerformingGraphicUpdate)
        return this.m_GraphicRebuildQueue.AddUnique(element);
      Debug.LogError((object) string.Format("Trying to add {0} for graphic rebuild while we are already inside a graphic rebuild loop. This is not supported.", (object) element));
      return false;
    }

    /// <summary>
    ///   <para>Remove the given element from rebuild.</para>
    /// </summary>
    /// <param name="element">Element to remove.</param>
    public static void UnRegisterCanvasElementForRebuild(ICanvasElement element)
    {
      CanvasUpdateRegistry.instance.InternalUnRegisterCanvasElementForLayoutRebuild(element);
      CanvasUpdateRegistry.instance.InternalUnRegisterCanvasElementForGraphicRebuild(element);
    }

    private void InternalUnRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
    {
      if (this.m_PerformingLayoutUpdate)
      {
        Debug.LogError((object) string.Format("Trying to remove {0} from rebuild list while we are already inside a rebuild loop. This is not supported.", (object) element));
      }
      else
      {
        element.LayoutComplete();
        CanvasUpdateRegistry.instance.m_LayoutRebuildQueue.Remove(element);
      }
    }

    private void InternalUnRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
    {
      if (this.m_PerformingGraphicUpdate)
      {
        Debug.LogError((object) string.Format("Trying to remove {0} from rebuild list while we are already inside a rebuild loop. This is not supported.", (object) element));
      }
      else
      {
        element.GraphicUpdateComplete();
        CanvasUpdateRegistry.instance.m_GraphicRebuildQueue.Remove(element);
      }
    }

    /// <summary>
    ///   <para>Is layout being rebuilt?</para>
    /// </summary>
    /// <returns>
    ///   <para>Rebuilding layout.</para>
    /// </returns>
    public static bool IsRebuildingLayout()
    {
      return CanvasUpdateRegistry.instance.m_PerformingLayoutUpdate;
    }

    /// <summary>
    ///   <para>Are graphics being rebuild.</para>
    /// </summary>
    /// <returns>
    ///   <para>Rebuilding graphics.</para>
    /// </returns>
    public static bool IsRebuildingGraphics()
    {
      return CanvasUpdateRegistry.instance.m_PerformingGraphicUpdate;
    }
  }
}

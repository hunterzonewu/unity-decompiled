// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.MaskUtilities
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System.Collections.Generic;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Mask related utility class.</para>
  /// </summary>
  public class MaskUtilities
  {
    /// <summary>
    ///   <para>Notify all IClippables under the given component that they need to recalculate clipping.</para>
    /// </summary>
    /// <param name="mask"></param>
    public static void Notify2DMaskStateChanged(Component mask)
    {
      List<Component> componentList = ListPool<Component>.Get();
      mask.GetComponentsInChildren<Component>(componentList);
      for (int index = 0; index < componentList.Count; ++index)
      {
        if (!((Object) componentList[index] == (Object) null) && !((Object) componentList[index].gameObject == (Object) mask.gameObject))
        {
          IClippable clippable = componentList[index] as IClippable;
          if (clippable != null)
            clippable.RecalculateClipping();
        }
      }
      ListPool<Component>.Release(componentList);
    }

    /// <summary>
    ///   <para>Notify all IMaskable under the given component that they need to recalculate masking.</para>
    /// </summary>
    /// <param name="mask"></param>
    public static void NotifyStencilStateChanged(Component mask)
    {
      List<Component> componentList = ListPool<Component>.Get();
      mask.GetComponentsInChildren<Component>(componentList);
      for (int index = 0; index < componentList.Count; ++index)
      {
        if (!((Object) componentList[index] == (Object) null) && !((Object) componentList[index].gameObject == (Object) mask.gameObject))
        {
          IMaskable maskable = componentList[index] as IMaskable;
          if (maskable != null)
            maskable.RecalculateMasking();
        }
      }
      ListPool<Component>.Release(componentList);
    }

    /// <summary>
    ///   <para>Find a root Canvas.</para>
    /// </summary>
    /// <param name="start">Search start.</param>
    /// <returns>
    ///   <para>Canvas transform.</para>
    /// </returns>
    public static Transform FindRootSortOverrideCanvas(Transform start)
    {
      List<Canvas> canvasList = ListPool<Canvas>.Get();
      start.GetComponentsInParent<Canvas>(false, canvasList);
      Canvas canvas = (Canvas) null;
      for (int index = 0; index < canvasList.Count; ++index)
      {
        canvas = canvasList[index];
        if (canvas.overrideSorting)
          break;
      }
      ListPool<Canvas>.Release(canvasList);
      if ((Object) canvas != (Object) null)
        return canvas.transform;
      return (Transform) null;
    }

    /// <summary>
    ///   <para>Find the stencil depth for a given element.</para>
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="stopAfter"></param>
    public static int GetStencilDepth(Transform transform, Transform stopAfter)
    {
      int num = 0;
      if ((Object) transform == (Object) stopAfter)
        return num;
      Transform parent = transform.parent;
      List<Mask> maskList = ListPool<Mask>.Get();
      for (; (Object) parent != (Object) null; parent = parent.parent)
      {
        parent.GetComponents<Mask>(maskList);
        for (int index = 0; index < maskList.Count; ++index)
        {
          if ((Object) maskList[index] != (Object) null && maskList[index].IsActive() && ((Object) maskList[index].graphic != (Object) null && maskList[index].graphic.IsActive()))
          {
            ++num;
            break;
          }
        }
        if ((Object) parent == (Object) stopAfter)
          break;
      }
      ListPool<Mask>.Release(maskList);
      return num;
    }

    /// <summary>
    ///   <para>Find the correct RectMask2D for a given IClippable.</para>
    /// </summary>
    /// <param name="transform">Clippable to search from.</param>
    public static RectMask2D GetRectMaskForClippable(IClippable transform)
    {
      List<RectMask2D> rectMask2DList = ListPool<RectMask2D>.Get();
      RectMask2D rectMask2D = (RectMask2D) null;
      transform.rectTransform.GetComponentsInParent<RectMask2D>(false, rectMask2DList);
      if (rectMask2DList.Count > 0)
        rectMask2D = rectMask2DList[0];
      ListPool<RectMask2D>.Release(rectMask2DList);
      return rectMask2D;
    }

    public static void GetRectMasksForClip(RectMask2D clipper, List<RectMask2D> masks)
    {
      masks.Clear();
      clipper.transform.GetComponentsInParent<RectMask2D>(false, masks);
    }
  }
}

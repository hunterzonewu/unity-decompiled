// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.LayoutUtility
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Utility functions for querying layout elements for their minimum, preferred, and flexible sizes.</para>
  /// </summary>
  public static class LayoutUtility
  {
    /// <summary>
    ///   <para>Returns the minimum size of the layout element.</para>
    /// </summary>
    /// <param name="rect">The RectTransform of the layout element to query.</param>
    /// <param name="axis">The axis to query. This can be 0 or 1.</param>
    public static float GetMinSize(RectTransform rect, int axis)
    {
      if (axis == 0)
        return LayoutUtility.GetMinWidth(rect);
      return LayoutUtility.GetMinHeight(rect);
    }

    /// <summary>
    ///   <para>Returns the preferred size of the layout element.</para>
    /// </summary>
    /// <param name="rect">The RectTransform of the layout element to query.</param>
    /// <param name="axis">The axis to query. This can be 0 or 1.</param>
    public static float GetPreferredSize(RectTransform rect, int axis)
    {
      if (axis == 0)
        return LayoutUtility.GetPreferredWidth(rect);
      return LayoutUtility.GetPreferredHeight(rect);
    }

    /// <summary>
    ///   <para>Returns the flexible size of the layout element.</para>
    /// </summary>
    /// <param name="rect">The RectTransform of the layout element to query.</param>
    /// <param name="axis">The axis to query. This can be 0 or 1.</param>
    public static float GetFlexibleSize(RectTransform rect, int axis)
    {
      if (axis == 0)
        return LayoutUtility.GetFlexibleWidth(rect);
      return LayoutUtility.GetFlexibleHeight(rect);
    }

    /// <summary>
    ///   <para>Returns the minimum width of the layout element.</para>
    /// </summary>
    /// <param name="rect">The RectTransform of the layout element to query.</param>
    public static float GetMinWidth(RectTransform rect)
    {
      return LayoutUtility.GetLayoutProperty(rect, (Func<ILayoutElement, float>) (e => e.minWidth), 0.0f);
    }

    /// <summary>
    ///   <para>Returns the preferred width of the layout element.</para>
    /// </summary>
    /// <param name="rect">The RectTransform of the layout element to query.</param>
    public static float GetPreferredWidth(RectTransform rect)
    {
      return Mathf.Max(LayoutUtility.GetLayoutProperty(rect, (Func<ILayoutElement, float>) (e => e.minWidth), 0.0f), LayoutUtility.GetLayoutProperty(rect, (Func<ILayoutElement, float>) (e => e.preferredWidth), 0.0f));
    }

    /// <summary>
    ///   <para>Returns the flexible width of the layout element.</para>
    /// </summary>
    /// <param name="rect">The RectTransform of the layout element to query.</param>
    public static float GetFlexibleWidth(RectTransform rect)
    {
      return LayoutUtility.GetLayoutProperty(rect, (Func<ILayoutElement, float>) (e => e.flexibleWidth), 0.0f);
    }

    /// <summary>
    ///   <para>Returns the minimum height of the layout element.</para>
    /// </summary>
    /// <param name="rect">The RectTransform of the layout element to query.</param>
    public static float GetMinHeight(RectTransform rect)
    {
      return LayoutUtility.GetLayoutProperty(rect, (Func<ILayoutElement, float>) (e => e.minHeight), 0.0f);
    }

    /// <summary>
    ///   <para>Returns the preferred height of the layout element.</para>
    /// </summary>
    /// <param name="rect">The RectTransform of the layout element to query.</param>
    public static float GetPreferredHeight(RectTransform rect)
    {
      return Mathf.Max(LayoutUtility.GetLayoutProperty(rect, (Func<ILayoutElement, float>) (e => e.minHeight), 0.0f), LayoutUtility.GetLayoutProperty(rect, (Func<ILayoutElement, float>) (e => e.preferredHeight), 0.0f));
    }

    /// <summary>
    ///   <para>Returns the flexible height of the layout element.</para>
    /// </summary>
    /// <param name="rect">The RectTransform of the layout element to query.</param>
    public static float GetFlexibleHeight(RectTransform rect)
    {
      return LayoutUtility.GetLayoutProperty(rect, (Func<ILayoutElement, float>) (e => e.flexibleHeight), 0.0f);
    }

    public static float GetLayoutProperty(RectTransform rect, Func<ILayoutElement, float> property, float defaultValue)
    {
      ILayoutElement source;
      return LayoutUtility.GetLayoutProperty(rect, property, defaultValue, out source);
    }

    public static float GetLayoutProperty(RectTransform rect, Func<ILayoutElement, float> property, float defaultValue, out ILayoutElement source)
    {
      source = (ILayoutElement) null;
      if ((UnityEngine.Object) rect == (UnityEngine.Object) null)
        return 0.0f;
      float num1 = defaultValue;
      int num2 = int.MinValue;
      List<Component> componentList = ListPool<Component>.Get();
      rect.GetComponents(typeof (ILayoutElement), componentList);
      for (int index = 0; index < componentList.Count; ++index)
      {
        ILayoutElement layoutElement = componentList[index] as ILayoutElement;
        if (!(layoutElement is Behaviour) || ((Behaviour) layoutElement).isActiveAndEnabled)
        {
          int layoutPriority = layoutElement.layoutPriority;
          if (layoutPriority >= num2)
          {
            float num3 = property(layoutElement);
            if ((double) num3 >= 0.0)
            {
              if (layoutPriority > num2)
              {
                num1 = num3;
                num2 = layoutPriority;
                source = layoutElement;
              }
              else if ((double) num3 > (double) num1)
              {
                num1 = num3;
                source = layoutElement;
              }
            }
          }
        }
      }
      ListPool<Component>.Release(componentList);
      return num1;
    }
  }
}

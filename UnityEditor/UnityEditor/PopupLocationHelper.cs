// Decompiled with JetBrains decompiler
// Type: UnityEditor.PopupLocationHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal static class PopupLocationHelper
  {
    private static float k_SpaceFromBottom
    {
      get
      {
        return Application.platform == RuntimePlatform.OSXEditor ? 10f : 0.0f;
      }
    }

    public static Rect GetDropDownRect(Rect buttonRect, Vector2 minSize, Vector2 maxSize, ContainerWindow popupContainerWindow)
    {
      return PopupLocationHelper.GetDropDownRect(buttonRect, minSize, maxSize, popupContainerWindow, (PopupLocationHelper.PopupLocation[]) null);
    }

    public static Rect GetDropDownRect(Rect buttonRect, Vector2 minSize, Vector2 maxSize, ContainerWindow popupContainerWindow, PopupLocationHelper.PopupLocation[] locationPriorityOrder)
    {
      if (locationPriorityOrder == null)
        locationPriorityOrder = new PopupLocationHelper.PopupLocation[2]
        {
          PopupLocationHelper.PopupLocation.Below,
          PopupLocationHelper.PopupLocation.Above
        };
      List<Rect> rects = new List<Rect>();
      Rect resultRect;
      foreach (int num in locationPriorityOrder)
      {
        switch (num)
        {
          case 0:
            if (PopupLocationHelper.PopupBelow(buttonRect, minSize, maxSize, popupContainerWindow, out resultRect))
              return resultRect;
            rects.Add(resultRect);
            break;
          case 1:
            if (PopupLocationHelper.PopupAbove(buttonRect, minSize, maxSize, popupContainerWindow, out resultRect))
              return resultRect;
            rects.Add(resultRect);
            break;
          case 2:
            if (PopupLocationHelper.PopupLeft(buttonRect, minSize, maxSize, popupContainerWindow, out resultRect))
              return resultRect;
            rects.Add(resultRect);
            break;
          case 3:
            if (PopupLocationHelper.PopupRight(buttonRect, minSize, maxSize, popupContainerWindow, out resultRect))
              return resultRect;
            rects.Add(resultRect);
            break;
        }
      }
      return PopupLocationHelper.GetLargestRect(rects);
    }

    private static Rect FitRect(Rect rect, ContainerWindow popupContainerWindow)
    {
      if ((bool) ((Object) popupContainerWindow))
        return popupContainerWindow.FitWindowRectToScreen(rect, true, true);
      return ContainerWindow.FitRectToScreen(rect, true, true);
    }

    private static bool PopupRight(Rect buttonRect, Vector2 minSize, Vector2 maxSize, ContainerWindow popupContainerWindow, out Rect resultRect)
    {
      Rect rect1 = new Rect(buttonRect.xMax, buttonRect.y, maxSize.x, maxSize.y);
      float num = 0.0f;
      rect1.xMax += num;
      rect1.height += PopupLocationHelper.k_SpaceFromBottom;
      Rect rect2 = PopupLocationHelper.FitRect(rect1, popupContainerWindow);
      float a = Mathf.Max(rect2.xMax - buttonRect.xMax - num, 0.0f);
      float width = Mathf.Min(a, maxSize.x);
      resultRect = new Rect(rect2.x, rect2.y, width, rect2.height - PopupLocationHelper.k_SpaceFromBottom);
      return (double) a >= (double) minSize.x;
    }

    private static bool PopupLeft(Rect buttonRect, Vector2 minSize, Vector2 maxSize, ContainerWindow popupContainerWindow, out Rect resultRect)
    {
      Rect rect1 = new Rect(buttonRect.x - maxSize.x, buttonRect.y, maxSize.x, maxSize.y);
      float num = 0.0f;
      rect1.xMin -= num;
      rect1.height += PopupLocationHelper.k_SpaceFromBottom;
      Rect rect2 = PopupLocationHelper.FitRect(rect1, popupContainerWindow);
      float a = Mathf.Max(buttonRect.x - rect2.x - num, 0.0f);
      float width = Mathf.Min(a, maxSize.x);
      resultRect = new Rect(rect2.x, rect2.y, width, rect2.height - PopupLocationHelper.k_SpaceFromBottom);
      return (double) a >= (double) minSize.x;
    }

    private static bool PopupAbove(Rect buttonRect, Vector2 minSize, Vector2 maxSize, ContainerWindow popupContainerWindow, out Rect resultRect)
    {
      Rect rect1 = new Rect(buttonRect.x, buttonRect.y - maxSize.y, maxSize.x, maxSize.y);
      float num1 = 0.0f;
      rect1.yMin -= num1;
      Rect rect2 = PopupLocationHelper.FitRect(rect1, popupContainerWindow);
      float num2 = Mathf.Max(buttonRect.y - rect2.y - num1, 0.0f);
      if ((double) num2 >= (double) minSize.y)
      {
        float height = Mathf.Min(num2, maxSize.y);
        resultRect = new Rect(rect2.x, buttonRect.y - height, rect2.width, height);
        return true;
      }
      resultRect = new Rect(rect2.x, buttonRect.y - num2, rect2.width, num2);
      return false;
    }

    private static bool PopupBelow(Rect buttonRect, Vector2 minSize, Vector2 maxSize, ContainerWindow popupContainerWindow, out Rect resultRect)
    {
      Rect rect1 = new Rect(buttonRect.x, buttonRect.yMax, maxSize.x, maxSize.y);
      rect1.height += PopupLocationHelper.k_SpaceFromBottom;
      Rect rect2 = PopupLocationHelper.FitRect(rect1, popupContainerWindow);
      float num = Mathf.Max(rect2.yMax - buttonRect.yMax - PopupLocationHelper.k_SpaceFromBottom, 0.0f);
      if ((double) num >= (double) minSize.y)
      {
        float height = Mathf.Min(num, maxSize.y);
        resultRect = new Rect(rect2.x, buttonRect.yMax, rect2.width, height);
        return true;
      }
      resultRect = new Rect(rect2.x, buttonRect.yMax, rect2.width, num);
      return false;
    }

    private static Rect GetLargestRect(List<Rect> rects)
    {
      Rect rect = new Rect();
      using (List<Rect>.Enumerator enumerator = rects.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Rect current = enumerator.Current;
          if ((double) current.height * (double) current.width > (double) rect.height * (double) rect.width)
            rect = current;
        }
      }
      return rect;
    }

    public enum PopupLocation
    {
      Below,
      Above,
      Left,
      Right,
    }
  }
}

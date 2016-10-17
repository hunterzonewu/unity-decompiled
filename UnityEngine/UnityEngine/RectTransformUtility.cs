// Decompiled with JetBrains decompiler
// Type: UnityEngine.RectTransformUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Utility class containing helper methods for working with  RectTransform.</para>
  /// </summary>
  public sealed class RectTransformUtility
  {
    private static Vector3[] s_Corners = new Vector3[4];

    private RectTransformUtility()
    {
    }

    public static bool RectangleContainsScreenPoint(RectTransform rect, Vector2 screenPoint)
    {
      return RectTransformUtility.RectangleContainsScreenPoint(rect, screenPoint, (Camera) null);
    }

    /// <summary>
    ///   <para>Does the RectTransform contain the screen point as seen from the given camera?</para>
    /// </summary>
    /// <param name="rect">The RectTransform to test with.</param>
    /// <param name="screenPoint">The screen point to test.</param>
    /// <param name="cam">The camera from which the test is performed from.</param>
    /// <returns>
    ///   <para>True if the point is inside the rectangle.</para>
    /// </returns>
    public static bool RectangleContainsScreenPoint(RectTransform rect, Vector2 screenPoint, Camera cam)
    {
      return RectTransformUtility.INTERNAL_CALL_RectangleContainsScreenPoint(rect, ref screenPoint, cam);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_RectangleContainsScreenPoint(RectTransform rect, ref Vector2 screenPoint, Camera cam);

    /// <summary>
    ///   <para>Convert a given point in screen space into a pixel correct point.</para>
    /// </summary>
    /// <param name="point"></param>
    /// <param name="elementTransform"></param>
    /// <param name="canvas"></param>
    /// <returns>
    ///   <para>Pixel adjusted point.</para>
    /// </returns>
    public static Vector2 PixelAdjustPoint(Vector2 point, Transform elementTransform, Canvas canvas)
    {
      Vector2 output;
      RectTransformUtility.PixelAdjustPoint(point, elementTransform, canvas, out output);
      return output;
    }

    private static void PixelAdjustPoint(Vector2 point, Transform elementTransform, Canvas canvas, out Vector2 output)
    {
      RectTransformUtility.INTERNAL_CALL_PixelAdjustPoint(ref point, elementTransform, canvas, out output);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_PixelAdjustPoint(ref Vector2 point, Transform elementTransform, Canvas canvas, out Vector2 output);

    /// <summary>
    ///   <para>Given a rect transform, return the corner points in pixel accurate coordinates.</para>
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="canvas"></param>
    /// <returns>
    ///   <para>Pixel adjusted rect.</para>
    /// </returns>
    public static Rect PixelAdjustRect(RectTransform rectTransform, Canvas canvas)
    {
      Rect rect;
      RectTransformUtility.INTERNAL_CALL_PixelAdjustRect(rectTransform, canvas, out rect);
      return rect;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_PixelAdjustRect(RectTransform rectTransform, Canvas canvas, out Rect value);

    public static bool ScreenPointToWorldPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
    {
      worldPoint = (Vector3) Vector2.zero;
      Ray ray = RectTransformUtility.ScreenPointToRay(cam, screenPoint);
      float enter;
      if (!new Plane(rect.rotation * Vector3.back, rect.position).Raycast(ray, out enter))
        return false;
      worldPoint = ray.GetPoint(enter);
      return true;
    }

    public static bool ScreenPointToLocalPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector2 localPoint)
    {
      localPoint = Vector2.zero;
      Vector3 worldPoint;
      if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPoint, cam, out worldPoint))
        return false;
      localPoint = (Vector2) rect.InverseTransformPoint(worldPoint);
      return true;
    }

    public static Ray ScreenPointToRay(Camera cam, Vector2 screenPos)
    {
      if ((Object) cam != (Object) null)
        return cam.ScreenPointToRay((Vector3) screenPos);
      Vector3 origin = (Vector3) screenPos;
      origin.z -= 100f;
      return new Ray(origin, Vector3.forward);
    }

    public static Vector2 WorldToScreenPoint(Camera cam, Vector3 worldPoint)
    {
      if ((Object) cam == (Object) null)
        return new Vector2(worldPoint.x, worldPoint.y);
      return (Vector2) cam.WorldToScreenPoint(worldPoint);
    }

    public static Bounds CalculateRelativeRectTransformBounds(Transform root, Transform child)
    {
      RectTransform[] componentsInChildren = child.GetComponentsInChildren<RectTransform>(false);
      if (componentsInChildren.Length <= 0)
        return new Bounds(Vector3.zero, Vector3.zero);
      Vector3 vector3_1 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
      Vector3 vector3_2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
      Matrix4x4 worldToLocalMatrix = root.worldToLocalMatrix;
      int index1 = 0;
      for (int length = componentsInChildren.Length; index1 < length; ++index1)
      {
        componentsInChildren[index1].GetWorldCorners(RectTransformUtility.s_Corners);
        for (int index2 = 0; index2 < 4; ++index2)
        {
          Vector3 lhs = worldToLocalMatrix.MultiplyPoint3x4(RectTransformUtility.s_Corners[index2]);
          vector3_1 = Vector3.Min(lhs, vector3_1);
          vector3_2 = Vector3.Max(lhs, vector3_2);
        }
      }
      Bounds bounds = new Bounds(vector3_1, Vector3.zero);
      bounds.Encapsulate(vector3_2);
      return bounds;
    }

    public static Bounds CalculateRelativeRectTransformBounds(Transform trans)
    {
      return RectTransformUtility.CalculateRelativeRectTransformBounds(trans, trans);
    }

    /// <summary>
    ///   <para>Flips the alignment of the RectTransform along the horizontal or vertical axis, and optionally its children as well.</para>
    /// </summary>
    /// <param name="rect">The RectTransform to flip.</param>
    /// <param name="keepPositioning">Flips around the pivot if true. Flips within the parent rect if false.</param>
    /// <param name="recursive">Flip the children as well?</param>
    /// <param name="axis">The axis to flip along. 0 is horizontal and 1 is vertical.</param>
    public static void FlipLayoutOnAxis(RectTransform rect, int axis, bool keepPositioning, bool recursive)
    {
      if ((Object) rect == (Object) null)
        return;
      if (recursive)
      {
        for (int index = 0; index < rect.childCount; ++index)
        {
          RectTransform child = rect.GetChild(index) as RectTransform;
          if ((Object) child != (Object) null)
            RectTransformUtility.FlipLayoutOnAxis(child, axis, false, true);
        }
      }
      Vector2 pivot = rect.pivot;
      pivot[axis] = 1f - pivot[axis];
      rect.pivot = pivot;
      if (keepPositioning)
        return;
      Vector2 anchoredPosition = rect.anchoredPosition;
      anchoredPosition[axis] = -anchoredPosition[axis];
      rect.anchoredPosition = anchoredPosition;
      Vector2 anchorMin = rect.anchorMin;
      Vector2 anchorMax = rect.anchorMax;
      float num = anchorMin[axis];
      anchorMin[axis] = 1f - anchorMax[axis];
      anchorMax[axis] = 1f - num;
      rect.anchorMin = anchorMin;
      rect.anchorMax = anchorMax;
    }

    /// <summary>
    ///   <para>Flips the horizontal and vertical axes of the RectTransform size and alignment, and optionally its children as well.</para>
    /// </summary>
    /// <param name="rect">The RectTransform to flip.</param>
    /// <param name="keepPositioning">Flips around the pivot if true. Flips within the parent rect if false.</param>
    /// <param name="recursive">Flip the children as well?</param>
    public static void FlipLayoutAxes(RectTransform rect, bool keepPositioning, bool recursive)
    {
      if ((Object) rect == (Object) null)
        return;
      if (recursive)
      {
        for (int index = 0; index < rect.childCount; ++index)
        {
          RectTransform child = rect.GetChild(index) as RectTransform;
          if ((Object) child != (Object) null)
            RectTransformUtility.FlipLayoutAxes(child, false, true);
        }
      }
      rect.pivot = RectTransformUtility.GetTransposed(rect.pivot);
      rect.sizeDelta = RectTransformUtility.GetTransposed(rect.sizeDelta);
      if (keepPositioning)
        return;
      rect.anchoredPosition = RectTransformUtility.GetTransposed(rect.anchoredPosition);
      rect.anchorMin = RectTransformUtility.GetTransposed(rect.anchorMin);
      rect.anchorMax = RectTransformUtility.GetTransposed(rect.anchorMax);
    }

    private static Vector2 GetTransposed(Vector2 input)
    {
      return new Vector2(input.y, input.x);
    }
  }
}

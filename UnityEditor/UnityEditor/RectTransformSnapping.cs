// Decompiled with JetBrains decompiler
// Type: UnityEditor.RectTransformSnapping
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class RectTransformSnapping
  {
    private static SnapGuideCollection[] s_SnapGuides = new SnapGuideCollection[2]{ new SnapGuideCollection(), new SnapGuideCollection() };
    private static float[] kSidesAndMiddle = new float[3]{ 0.0f, 0.5f, 1f };
    private static Vector3[] s_Corners = new Vector3[4];
    internal const float kSnapThreshold = 0.05f;

    internal static void OnGUI()
    {
      RectTransformSnapping.s_SnapGuides[0].OnGUI();
      RectTransformSnapping.s_SnapGuides[1].OnGUI();
    }

    internal static void DrawGuides()
    {
      if (EditorGUI.actionKey)
        return;
      RectTransformSnapping.s_SnapGuides[0].DrawGuides();
      RectTransformSnapping.s_SnapGuides[1].DrawGuides();
    }

    private static Vector3 GetInterpolatedCorner(Vector3[] corners, int mainAxis, float alongMainAxis, float alongOtherAxis)
    {
      if (mainAxis != 0)
      {
        float num = alongMainAxis;
        alongMainAxis = alongOtherAxis;
        alongOtherAxis = num;
      }
      return corners[0] * (1f - alongMainAxis) * (1f - alongOtherAxis) + corners[1] * (1f - alongMainAxis) * alongOtherAxis + corners[3] * alongMainAxis * (1f - alongOtherAxis) + corners[2] * alongMainAxis * alongOtherAxis;
    }

    internal static void CalculatePivotSnapValues(Rect rect, Vector3 pivot, Quaternion rotation)
    {
      for (int axis = 0; axis < 2; ++axis)
      {
        RectTransformSnapping.s_SnapGuides[axis].Clear();
        for (int index = 0; index < RectTransformSnapping.kSidesAndMiddle.Length; ++index)
          RectTransformSnapping.s_SnapGuides[axis].AddGuide(new SnapGuide(RectTransformSnapping.kSidesAndMiddle[index], RectTransformSnapping.GetGuideLineForRect(rect, pivot, rotation, axis, RectTransformSnapping.kSidesAndMiddle[index])));
      }
    }

    internal static void CalculateAnchorSnapValues(Transform parentSpace, Transform self, RectTransform gui, int minmaxX, int minmaxY)
    {
      for (int mainAxis = 0; mainAxis < 2; ++mainAxis)
      {
        RectTransformSnapping.s_SnapGuides[mainAxis].Clear();
        parentSpace.GetComponent<RectTransform>().GetWorldCorners(RectTransformSnapping.s_Corners);
        for (int index = 0; index < RectTransformSnapping.kSidesAndMiddle.Length; ++index)
        {
          float alongMainAxis = RectTransformSnapping.kSidesAndMiddle[index];
          RectTransformSnapping.s_SnapGuides[mainAxis].AddGuide(new SnapGuide(alongMainAxis, new Vector3[2]
          {
            RectTransformSnapping.GetInterpolatedCorner(RectTransformSnapping.s_Corners, mainAxis, alongMainAxis, 0.0f),
            RectTransformSnapping.GetInterpolatedCorner(RectTransformSnapping.s_Corners, mainAxis, alongMainAxis, 1f)
          }));
        }
        foreach (Transform transform in parentSpace)
        {
          if (!((Object) transform == (Object) self))
          {
            RectTransform component = transform.GetComponent<RectTransform>();
            if ((bool) ((Object) component))
            {
              RectTransformSnapping.s_SnapGuides[mainAxis].AddGuide(new SnapGuide(component.anchorMin[mainAxis], new Vector3[0]));
              RectTransformSnapping.s_SnapGuides[mainAxis].AddGuide(new SnapGuide(component.anchorMax[mainAxis], new Vector3[0]));
            }
          }
        }
        int num = mainAxis != 0 ? minmaxY : minmaxX;
        if (num == 0)
          RectTransformSnapping.s_SnapGuides[mainAxis].AddGuide(new SnapGuide(gui.anchorMax[mainAxis], new Vector3[0]));
        if (num == 1)
          RectTransformSnapping.s_SnapGuides[mainAxis].AddGuide(new SnapGuide(gui.anchorMin[mainAxis], new Vector3[0]));
      }
    }

    internal static void CalculateOffsetSnapValues(Transform parentSpace, Transform self, RectTransform parentRect, RectTransform rect, int xHandle, int yHandle)
    {
      for (int index = 0; index < 2; ++index)
        RectTransformSnapping.s_SnapGuides[index].Clear();
      if ((Object) parentSpace == (Object) null)
        return;
      for (int axis = 0; axis < 2; ++axis)
      {
        int side = axis != 0 ? yHandle : xHandle;
        if (side != 1)
        {
          using (List<SnapGuide>.Enumerator enumerator = RectTransformSnapping.GetSnapGuides(parentSpace, self, parentRect, rect, axis, side).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              SnapGuide current = enumerator.Current;
              RectTransformSnapping.s_SnapGuides[axis].AddGuide(current);
            }
          }
        }
      }
    }

    internal static void CalculatePositionSnapValues(Transform parentSpace, Transform self, RectTransform parentRect, RectTransform rect)
    {
      for (int index = 0; index < 2; ++index)
        RectTransformSnapping.s_SnapGuides[index].Clear();
      if ((Object) parentSpace == (Object) null)
        return;
      for (int axis = 0; axis < 2; ++axis)
      {
        for (int side = 0; side < RectTransformSnapping.kSidesAndMiddle.Length; ++side)
        {
          using (List<SnapGuide>.Enumerator enumerator = RectTransformSnapping.GetSnapGuides(parentSpace, self, parentRect, rect, axis, side).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              SnapGuide current = enumerator.Current;
              current.value = RectTransformSnapping.GetGuideValueForRect(rect, current.value, axis, RectTransformSnapping.kSidesAndMiddle[side]);
              RectTransformSnapping.s_SnapGuides[axis].AddGuide(current);
            }
          }
        }
      }
    }

    private static List<SnapGuide> GetSnapGuides(Transform parentSpace, Transform self, RectTransform parentRect, RectTransform rect, int axis, int side)
    {
      List<SnapGuide> snapGuideList = new List<SnapGuide>();
      if ((Object) parentRect != (Object) null)
      {
        float num1 = RectTransformSnapping.kSidesAndMiddle[side];
        float side1 = Mathf.Lerp(rect.anchorMin[axis], rect.anchorMax[axis], num1);
        snapGuideList.Add(new SnapGuide(side1 * parentRect.rect.size[axis], RectTransformSnapping.GetGuideLineForRect(parentRect, axis, side1)));
        float num2 = Mathf.Lerp(rect.anchorMin[axis], rect.anchorMax[axis], num1);
        if ((double) num1 != (double) num2)
          snapGuideList.Add(new SnapGuide(num1 * parentRect.rect.size[axis], false, RectTransformSnapping.GetGuideLineForRect(parentRect, axis, num1)));
      }
      foreach (Transform transform in parentSpace)
      {
        if (!((Object) transform == (Object) self))
        {
          RectTransform component = transform.GetComponent<RectTransform>();
          if ((bool) ((Object) component))
          {
            if (side == 0)
            {
              bool safe1 = (double) component.anchorMin[axis] == (double) rect.anchorMin[axis];
              snapGuideList.Add(new SnapGuide(component.GetRectInParentSpace().min[axis], safe1, RectTransformSnapping.GetGuideLineForRect(component, axis, 0.0f)));
              bool safe2 = (double) component.anchorMax[axis] == (double) rect.anchorMin[axis];
              snapGuideList.Add(new SnapGuide(component.GetRectInParentSpace().max[axis], safe2, RectTransformSnapping.GetGuideLineForRect(component, axis, 1f)));
            }
            if (side == 2)
            {
              bool safe1 = (double) component.anchorMax[axis] == (double) rect.anchorMax[axis];
              snapGuideList.Add(new SnapGuide(component.GetRectInParentSpace().max[axis], safe1, RectTransformSnapping.GetGuideLineForRect(component, axis, 1f)));
              bool safe2 = (double) component.anchorMin[axis] == (double) rect.anchorMax[axis];
              snapGuideList.Add(new SnapGuide(component.GetRectInParentSpace().min[axis], safe2, RectTransformSnapping.GetGuideLineForRect(component, axis, 0.0f)));
            }
            if (side == 1)
            {
              bool safe = (double) component.anchorMin[axis] - (double) rect.anchorMin[axis] == -((double) component.anchorMax[axis] - (double) rect.anchorMax[axis]);
              snapGuideList.Add(new SnapGuide(component.GetRectInParentSpace().center[axis], safe, RectTransformSnapping.GetGuideLineForRect(component, axis, 0.5f)));
            }
          }
        }
      }
      return snapGuideList;
    }

    private static Vector3[] GetGuideLineForRect(RectTransform rect, int axis, float side)
    {
      Vector3[] vector3Array = new Vector3[2];
      vector3Array[0][1 - axis] = rect.rect.min[1 - axis];
      vector3Array[1][1 - axis] = rect.rect.max[1 - axis];
      vector3Array[0][axis] = Mathf.Lerp(rect.rect.min[axis], rect.rect.max[axis], side);
      vector3Array[1][axis] = vector3Array[0][axis];
      vector3Array[0] = rect.transform.TransformPoint(vector3Array[0]);
      vector3Array[1] = rect.transform.TransformPoint(vector3Array[1]);
      return vector3Array;
    }

    private static Vector3[] GetGuideLineForRect(Rect rect, Vector3 pivot, Quaternion rotation, int axis, float side)
    {
      Vector3[] vector3Array = new Vector3[2];
      vector3Array[0][1 - axis] = rect.min[1 - axis];
      vector3Array[1][1 - axis] = rect.max[1 - axis];
      vector3Array[0][axis] = Mathf.Lerp(rect.min[axis], rect.max[axis], side);
      vector3Array[1][axis] = vector3Array[0][axis];
      vector3Array[0] = rotation * vector3Array[0] + pivot;
      vector3Array[1] = rotation * vector3Array[1] + pivot;
      return vector3Array;
    }

    private static float GetGuideValueForRect(RectTransform rect, float value, int axis, float side)
    {
      RectTransform component = rect.transform.parent.GetComponent<RectTransform>();
      float num1 = !(bool) ((Object) component) ? 0.0f : component.rect.size[axis];
      float num2 = Mathf.Lerp(rect.anchorMin[axis], rect.anchorMax[axis], rect.pivot[axis]) * num1;
      float num3 = rect.rect.size[axis] * (rect.pivot[axis] - side);
      return value - num2 + num3;
    }

    internal static Vector2 SnapToGuides(Vector2 value, Vector2 snapDistance)
    {
      return new Vector2(RectTransformSnapping.SnapToGuides(value.x, snapDistance.x, 0), RectTransformSnapping.SnapToGuides(value.y, snapDistance.y, 1));
    }

    internal static float SnapToGuides(float value, float snapDistance, int axis)
    {
      if (EditorGUI.actionKey)
        return value;
      return (axis != 0 ? RectTransformSnapping.s_SnapGuides[1] : RectTransformSnapping.s_SnapGuides[0]).SnapToGuides(value, snapDistance);
    }
  }
}

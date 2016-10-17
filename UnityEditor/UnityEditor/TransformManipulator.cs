// Decompiled with JetBrains decompiler
// Type: UnityEditor.TransformManipulator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class TransformManipulator
  {
    private static EventType s_EventTypeBefore = EventType.Ignore;
    private static TransformManipulator.TransformData[] s_MouseDownState = (TransformManipulator.TransformData[]) null;
    private static Vector3 s_StartHandlePosition = Vector3.zero;
    private static Vector3 s_StartLocalHandleOffset = Vector3.zero;
    private static int s_HotControl = 0;
    private static bool s_LockHandle = false;

    public static Vector3 mouseDownHandlePosition
    {
      get
      {
        return TransformManipulator.s_StartHandlePosition;
      }
    }

    public static bool active
    {
      get
      {
        return TransformManipulator.s_MouseDownState != null;
      }
    }

    public static bool individualSpace
    {
      get
      {
        if (Tools.pivotRotation == PivotRotation.Local)
          return Tools.pivotMode == PivotMode.Pivot;
        return false;
      }
    }

    private static void BeginEventCheck()
    {
      TransformManipulator.s_EventTypeBefore = Event.current.GetTypeForControl(TransformManipulator.s_HotControl);
    }

    private static EventType EndEventCheck()
    {
      EventType eventType = TransformManipulator.s_EventTypeBefore == Event.current.GetTypeForControl(TransformManipulator.s_HotControl) ? EventType.Ignore : TransformManipulator.s_EventTypeBefore;
      TransformManipulator.s_EventTypeBefore = EventType.Ignore;
      if (eventType == EventType.MouseDown)
        TransformManipulator.s_HotControl = GUIUtility.hotControl;
      else if (eventType == EventType.MouseUp)
        TransformManipulator.s_HotControl = 0;
      return eventType;
    }

    public static void BeginManipulationHandling(bool lockHandleWhileDragging)
    {
      TransformManipulator.BeginEventCheck();
      TransformManipulator.s_LockHandle = lockHandleWhileDragging;
    }

    public static EventType EndManipulationHandling()
    {
      EventType eventType = TransformManipulator.EndEventCheck();
      if (eventType == EventType.MouseDown)
      {
        TransformManipulator.RecordMouseDownState(Selection.transforms);
        TransformManipulator.s_StartHandlePosition = Tools.handlePosition;
        TransformManipulator.s_StartLocalHandleOffset = Tools.localHandleOffset;
        if (TransformManipulator.s_LockHandle)
          Tools.LockHandlePosition();
        Tools.LockHandleRectRotation();
      }
      else if (TransformManipulator.s_MouseDownState != null && (eventType == EventType.MouseUp || GUIUtility.hotControl != TransformManipulator.s_HotControl))
      {
        TransformManipulator.s_MouseDownState = (TransformManipulator.TransformData[]) null;
        if (TransformManipulator.s_LockHandle)
          Tools.UnlockHandlePosition();
        Tools.UnlockHandleRectRotation();
        ManipulationToolUtility.DisableMinDragDifference();
      }
      return eventType;
    }

    private static void RecordMouseDownState(Transform[] transforms)
    {
      TransformManipulator.s_MouseDownState = new TransformManipulator.TransformData[transforms.Length];
      for (int index = 0; index < transforms.Length; ++index)
        TransformManipulator.s_MouseDownState[index] = TransformManipulator.TransformData.GetData(transforms[index]);
    }

    private static void SetLocalHandleOffsetScaleDelta(Vector3 scaleDelta, Quaternion pivotRotation)
    {
      Quaternion quaternion = Quaternion.Inverse(Tools.handleRotation) * pivotRotation;
      Tools.localHandleOffset = Vector3.Scale(Vector3.Scale(TransformManipulator.s_StartLocalHandleOffset, quaternion * scaleDelta), quaternion * Vector3.one);
    }

    public static void SetScaleDelta(Vector3 scaleDelta, Quaternion pivotRotation)
    {
      if (TransformManipulator.s_MouseDownState == null)
        return;
      TransformManipulator.SetLocalHandleOffsetScaleDelta(scaleDelta, pivotRotation);
      for (int index = 0; index < TransformManipulator.s_MouseDownState.Length; ++index)
        Undo.RecordObject((Object) TransformManipulator.s_MouseDownState[index].transform, "Scale");
      Vector3 scalePivot = Tools.handlePosition;
      for (int index = 0; index < TransformManipulator.s_MouseDownState.Length; ++index)
      {
        if (Tools.pivotMode == PivotMode.Pivot)
          scalePivot = TransformManipulator.s_MouseDownState[index].position;
        if (TransformManipulator.individualSpace)
          pivotRotation = TransformManipulator.s_MouseDownState[index].rotation;
        TransformManipulator.s_MouseDownState[index].SetScaleDelta(scaleDelta, scalePivot, pivotRotation, false);
      }
    }

    public static void SetResizeDelta(Vector3 scaleDelta, Vector3 pivotPosition, Quaternion pivotRotation)
    {
      if (TransformManipulator.s_MouseDownState == null)
        return;
      TransformManipulator.SetLocalHandleOffsetScaleDelta(scaleDelta, pivotRotation);
      for (int index = 0; index < TransformManipulator.s_MouseDownState.Length; ++index)
      {
        TransformManipulator.TransformData transformData = TransformManipulator.s_MouseDownState[index];
        Undo.RecordObject(!((Object) transformData.rectTransform != (Object) null) ? (Object) transformData.transform : (Object) transformData.rectTransform, "Resize");
      }
      for (int index = 0; index < TransformManipulator.s_MouseDownState.Length; ++index)
        TransformManipulator.s_MouseDownState[index].SetScaleDelta(scaleDelta, pivotPosition, pivotRotation, true);
    }

    public static void SetPositionDelta(Vector3 positionDelta)
    {
      if (TransformManipulator.s_MouseDownState == null)
        return;
      for (int index = 0; index < TransformManipulator.s_MouseDownState.Length; ++index)
      {
        TransformManipulator.TransformData transformData = TransformManipulator.s_MouseDownState[index];
        Undo.RecordObject(!((Object) transformData.rectTransform != (Object) null) ? (Object) transformData.transform : (Object) transformData.rectTransform, "Move");
      }
      for (int index = 0; index < TransformManipulator.s_MouseDownState.Length; ++index)
        TransformManipulator.s_MouseDownState[index].SetPositionDelta(positionDelta);
    }

    public static void DebugAlignment(Quaternion targetRotation)
    {
      if (TransformManipulator.s_MouseDownState == null)
        return;
      for (int index = 0; index < TransformManipulator.s_MouseDownState.Length; ++index)
        TransformManipulator.s_MouseDownState[index].DebugAlignment(targetRotation);
    }

    private struct TransformData
    {
      public static Quaternion[] s_Alignments = new Quaternion[6]{ Quaternion.LookRotation(Vector3.right, Vector3.up), Quaternion.LookRotation(Vector3.right, Vector3.forward), Quaternion.LookRotation(Vector3.up, Vector3.forward), Quaternion.LookRotation(Vector3.up, Vector3.right), Quaternion.LookRotation(Vector3.forward, Vector3.right), Quaternion.LookRotation(Vector3.forward, Vector3.up) };
      public Transform transform;
      public Vector3 position;
      public Vector3 localPosition;
      public Quaternion rotation;
      public Vector3 scale;
      public RectTransform rectTransform;
      public Rect rect;
      public Vector2 anchoredPosition;
      public Vector2 sizeDelta;

      public static TransformManipulator.TransformData GetData(Transform t)
      {
        TransformManipulator.TransformData transformData = new TransformManipulator.TransformData();
        transformData.transform = t;
        transformData.position = t.position;
        transformData.localPosition = t.localPosition;
        transformData.rotation = t.rotation;
        transformData.scale = t.localScale;
        transformData.rectTransform = t.GetComponent<RectTransform>();
        if ((Object) transformData.rectTransform != (Object) null)
        {
          transformData.sizeDelta = transformData.rectTransform.sizeDelta;
          transformData.rect = transformData.rectTransform.rect;
          transformData.anchoredPosition = transformData.rectTransform.anchoredPosition;
        }
        return transformData;
      }

      private Quaternion GetRefAlignment(Quaternion targetRotation, Quaternion ownRotation)
      {
        float num1 = float.NegativeInfinity;
        Quaternion quaternion = Quaternion.identity;
        for (int index = 0; index < TransformManipulator.TransformData.s_Alignments.Length; ++index)
        {
          float num2 = Mathf.Min(Mathf.Abs(Vector3.Dot(targetRotation * Vector3.right, ownRotation * TransformManipulator.TransformData.s_Alignments[index] * Vector3.right)), Mathf.Abs(Vector3.Dot(targetRotation * Vector3.up, ownRotation * TransformManipulator.TransformData.s_Alignments[index] * Vector3.up)), Mathf.Abs(Vector3.Dot(targetRotation * Vector3.forward, ownRotation * TransformManipulator.TransformData.s_Alignments[index] * Vector3.forward)));
          if ((double) num2 > (double) num1)
          {
            num1 = num2;
            quaternion = TransformManipulator.TransformData.s_Alignments[index];
          }
        }
        return quaternion;
      }

      public void SetScaleDelta(Vector3 scaleDelta, Vector3 scalePivot, Quaternion scaleRotation, bool preferRectResize)
      {
        this.SetPosition(scaleRotation * Vector3.Scale(Quaternion.Inverse(scaleRotation) * (this.position - scalePivot), scaleDelta) + scalePivot);
        Vector3 minDragDifference = ManipulationToolUtility.minDragDifference;
        if ((Object) this.transform.parent != (Object) null)
        {
          minDragDifference.x /= this.transform.parent.lossyScale.x;
          minDragDifference.y /= this.transform.parent.lossyScale.y;
          minDragDifference.z /= this.transform.parent.lossyScale.z;
        }
        Quaternion ownRotation = !Tools.rectBlueprintMode || !InternalEditorUtility.SupportsRectLayout(this.transform) ? this.rotation : this.transform.parent.rotation;
        Quaternion refAlignment = this.GetRefAlignment(scaleRotation, ownRotation);
        scaleDelta = refAlignment * scaleDelta;
        scaleDelta = Vector3.Scale(scaleDelta, refAlignment * Vector3.one);
        if (preferRectResize && (Object) this.rectTransform != (Object) null)
        {
          Vector2 vector2 = this.sizeDelta + Vector2.Scale(this.rect.size, (Vector2) scaleDelta) - this.rect.size;
          vector2.x = MathUtils.RoundBasedOnMinimumDifference(vector2.x, minDragDifference.x);
          vector2.y = MathUtils.RoundBasedOnMinimumDifference(vector2.y, minDragDifference.y);
          this.rectTransform.sizeDelta = vector2;
          if (!(this.rectTransform.drivenByObject != (Object) null))
            return;
          RectTransform.SendReapplyDrivenProperties(this.rectTransform);
        }
        else
          this.transform.localScale = Vector3.Scale(this.scale, scaleDelta);
      }

      private void SetPosition(Vector3 newPosition)
      {
        this.SetPositionDelta(newPosition - this.position);
      }

      public void SetPositionDelta(Vector3 positionDelta)
      {
        Vector3 vector = positionDelta;
        Vector3 minDragDifference = ManipulationToolUtility.minDragDifference;
        if ((Object) this.transform.parent != (Object) null)
        {
          vector = this.transform.parent.InverseTransformVector(vector);
          minDragDifference.x /= this.transform.parent.lossyScale.x;
          minDragDifference.y /= this.transform.parent.lossyScale.y;
          minDragDifference.z /= this.transform.parent.lossyScale.z;
        }
        bool flag1 = Mathf.Approximately(vector.x, 0.0f);
        bool flag2 = Mathf.Approximately(vector.y, 0.0f);
        bool flag3 = Mathf.Approximately(vector.z, 0.0f);
        if ((Object) this.rectTransform == (Object) null)
        {
          Vector3 vector3 = this.localPosition + vector;
          vector3.x = !flag1 ? MathUtils.RoundBasedOnMinimumDifference(vector3.x, minDragDifference.x) : this.localPosition.x;
          vector3.y = !flag2 ? MathUtils.RoundBasedOnMinimumDifference(vector3.y, minDragDifference.y) : this.localPosition.y;
          vector3.z = !flag3 ? MathUtils.RoundBasedOnMinimumDifference(vector3.z, minDragDifference.z) : this.localPosition.z;
          this.transform.localPosition = vector3;
        }
        else
        {
          Vector3 vector3 = this.localPosition + vector;
          vector3.z = !flag3 ? MathUtils.RoundBasedOnMinimumDifference(vector3.z, minDragDifference.z) : this.localPosition.z;
          this.transform.localPosition = vector3;
          Vector2 vector2 = this.anchoredPosition + (Vector2) vector;
          vector2.x = !flag1 ? MathUtils.RoundBasedOnMinimumDifference(vector2.x, minDragDifference.x) : this.anchoredPosition.x;
          vector2.y = !flag2 ? MathUtils.RoundBasedOnMinimumDifference(vector2.y, minDragDifference.y) : this.anchoredPosition.y;
          this.rectTransform.anchoredPosition = vector2;
          if (!(this.rectTransform.drivenByObject != (Object) null))
            return;
          RectTransform.SendReapplyDrivenProperties(this.rectTransform);
        }
      }

      public void DebugAlignment(Quaternion targetRotation)
      {
        Quaternion quaternion = Quaternion.identity;
        if (!TransformManipulator.individualSpace)
          quaternion = this.GetRefAlignment(targetRotation, this.rotation);
        Vector3 position = this.transform.position;
        float num = HandleUtility.GetHandleSize(position) * 0.25f;
        Color color = Handles.color;
        Handles.color = Color.red;
        Vector3 vector3_1 = this.rotation * quaternion * Vector3.right * num;
        Handles.DrawLine(position - vector3_1, position + vector3_1);
        Handles.color = Color.green;
        Vector3 vector3_2 = this.rotation * quaternion * Vector3.up * num;
        Handles.DrawLine(position - vector3_2, position + vector3_2);
        Handles.color = Color.blue;
        Vector3 vector3_3 = this.rotation * quaternion * Vector3.forward * num;
        Handles.DrawLine(position - vector3_3, position + vector3_3);
        Handles.color = color;
      }
    }
  }
}

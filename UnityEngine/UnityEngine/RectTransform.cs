// Decompiled with JetBrains decompiler
// Type: UnityEngine.RectTransform
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Position, size, anchor and pivot information for a rectangle.</para>
  /// </summary>
  public sealed class RectTransform : Transform
  {
    /// <summary>
    ///   <para>The calculated rectangle in the local space of the Transform.</para>
    /// </summary>
    public Rect rect
    {
      get
      {
        Rect rect;
        this.INTERNAL_get_rect(out rect);
        return rect;
      }
    }

    /// <summary>
    ///   <para>The normalized position in the parent RectTransform that the lower left corner is anchored to.</para>
    /// </summary>
    public Vector2 anchorMin
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_anchorMin(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_anchorMin(ref value);
      }
    }

    /// <summary>
    ///   <para>The normalized position in the parent RectTransform that the upper right corner is anchored to.</para>
    /// </summary>
    public Vector2 anchorMax
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_anchorMax(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_anchorMax(ref value);
      }
    }

    /// <summary>
    ///   <para>The 3D position of the pivot of this RectTransform relative to the anchor reference point.</para>
    /// </summary>
    public Vector3 anchoredPosition3D
    {
      get
      {
        Vector2 anchoredPosition = this.anchoredPosition;
        return new Vector3(anchoredPosition.x, anchoredPosition.y, this.localPosition.z);
      }
      set
      {
        this.anchoredPosition = new Vector2(value.x, value.y);
        Vector3 localPosition = this.localPosition;
        localPosition.z = value.z;
        this.localPosition = localPosition;
      }
    }

    /// <summary>
    ///   <para>The position of the pivot of this RectTransform relative to the anchor reference point.</para>
    /// </summary>
    public Vector2 anchoredPosition
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_anchoredPosition(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_anchoredPosition(ref value);
      }
    }

    /// <summary>
    ///   <para>The size of this RectTransform relative to the distances between the anchors.</para>
    /// </summary>
    public Vector2 sizeDelta
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_sizeDelta(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_sizeDelta(ref value);
      }
    }

    /// <summary>
    ///   <para>The normalized position in this RectTransform that it rotates around.</para>
    /// </summary>
    public Vector2 pivot
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_pivot(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_pivot(ref value);
      }
    }

    internal Object drivenByObject { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal DrivenTransformProperties drivenProperties { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The offset of the lower left corner of the rectangle relative to the lower left anchor.</para>
    /// </summary>
    public Vector2 offsetMin
    {
      get
      {
        return this.anchoredPosition - Vector2.Scale(this.sizeDelta, this.pivot);
      }
      set
      {
        Vector2 a = value - this.anchoredPosition - Vector2.Scale(this.sizeDelta, this.pivot);
        this.sizeDelta -= a;
        this.anchoredPosition += Vector2.Scale(a, Vector2.one - this.pivot);
      }
    }

    /// <summary>
    ///   <para>The offset of the upper right corner of the rectangle relative to the upper right anchor.</para>
    /// </summary>
    public Vector2 offsetMax
    {
      get
      {
        return this.anchoredPosition + Vector2.Scale(this.sizeDelta, Vector2.one - this.pivot);
      }
      set
      {
        Vector2 a = value - this.anchoredPosition + Vector2.Scale(this.sizeDelta, Vector2.one - this.pivot);
        this.sizeDelta += a;
        this.anchoredPosition += Vector2.Scale(a, this.pivot);
      }
    }

    public static event RectTransform.ReapplyDrivenProperties reapplyDrivenProperties;

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_rect(out Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_anchorMin(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_anchorMin(ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_anchorMax(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_anchorMax(ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_anchoredPosition(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_anchoredPosition(ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_sizeDelta(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_sizeDelta(ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_pivot(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_pivot(ref Vector2 value);

    [RequiredByNativeCode]
    internal static void SendReapplyDrivenProperties(RectTransform driven)
    {
      if (RectTransform.reapplyDrivenProperties == null)
        return;
      RectTransform.reapplyDrivenProperties(driven);
    }

    /// <summary>
    ///   <para>Get the corners of the calculated rectangle in the local space of its Transform.</para>
    /// </summary>
    /// <param name="fourCornersArray">Array that corners should be filled into.</param>
    public void GetLocalCorners(Vector3[] fourCornersArray)
    {
      if (fourCornersArray == null || fourCornersArray.Length < 4)
      {
        Debug.LogError((object) "Calling GetLocalCorners with an array that is null or has less than 4 elements.");
      }
      else
      {
        Rect rect = this.rect;
        float x = rect.x;
        float y = rect.y;
        float xMax = rect.xMax;
        float yMax = rect.yMax;
        fourCornersArray[0] = new Vector3(x, y, 0.0f);
        fourCornersArray[1] = new Vector3(x, yMax, 0.0f);
        fourCornersArray[2] = new Vector3(xMax, yMax, 0.0f);
        fourCornersArray[3] = new Vector3(xMax, y, 0.0f);
      }
    }

    /// <summary>
    ///   <para>Get the corners of the calculated rectangle in world space.</para>
    /// </summary>
    /// <param name="fourCornersArray">Array that corners should be filled into.</param>
    public void GetWorldCorners(Vector3[] fourCornersArray)
    {
      if (fourCornersArray == null || fourCornersArray.Length < 4)
      {
        Debug.LogError((object) "Calling GetWorldCorners with an array that is null or has less than 4 elements.");
      }
      else
      {
        this.GetLocalCorners(fourCornersArray);
        Transform transform = this.transform;
        for (int index = 0; index < 4; ++index)
          fourCornersArray[index] = transform.TransformPoint(fourCornersArray[index]);
      }
    }

    internal Rect GetRectInParentSpace()
    {
      Rect rect = this.rect;
      Vector2 vector2 = this.offsetMin + Vector2.Scale(this.pivot, rect.size);
      Transform parent = this.transform.parent;
      if ((bool) ((Object) parent))
      {
        RectTransform component = parent.GetComponent<RectTransform>();
        if ((bool) ((Object) component))
          vector2 += Vector2.Scale(this.anchorMin, component.rect.size);
      }
      rect.x += vector2.x;
      rect.y += vector2.y;
      return rect;
    }

    public void SetInsetAndSizeFromParentEdge(RectTransform.Edge edge, float inset, float size)
    {
      int index = edge == RectTransform.Edge.Top || edge == RectTransform.Edge.Bottom ? 1 : 0;
      bool flag = edge == RectTransform.Edge.Top || edge == RectTransform.Edge.Right;
      float num = !flag ? 0.0f : 1f;
      Vector2 vector2 = this.anchorMin;
      vector2[index] = num;
      this.anchorMin = vector2;
      vector2 = this.anchorMax;
      vector2[index] = num;
      this.anchorMax = vector2;
      Vector2 sizeDelta = this.sizeDelta;
      sizeDelta[index] = size;
      this.sizeDelta = sizeDelta;
      Vector2 anchoredPosition = this.anchoredPosition;
      anchoredPosition[index] = !flag ? inset + size * this.pivot[index] : (float) (-(double) inset - (double) size * (1.0 - (double) this.pivot[index]));
      this.anchoredPosition = anchoredPosition;
    }

    public void SetSizeWithCurrentAnchors(RectTransform.Axis axis, float size)
    {
      int index = (int) axis;
      Vector2 sizeDelta = this.sizeDelta;
      sizeDelta[index] = size - this.GetParentSize()[index] * (this.anchorMax[index] - this.anchorMin[index]);
      this.sizeDelta = sizeDelta;
    }

    private Vector2 GetParentSize()
    {
      RectTransform parent = this.parent as RectTransform;
      if (!(bool) ((Object) parent))
        return Vector2.zero;
      return parent.rect.size;
    }

    /// <summary>
    ///   <para>Enum used to specify one edge of a rectangle.</para>
    /// </summary>
    public enum Edge
    {
      Left,
      Right,
      Top,
      Bottom,
    }

    /// <summary>
    ///   <para>An axis that can be horizontal or vertical.</para>
    /// </summary>
    public enum Axis
    {
      Horizontal,
      Vertical,
    }

    /// <summary>
    ///   <para>Delegate used for the reapplyDrivenProperties event.</para>
    /// </summary>
    /// <param name="driven"></param>
    public delegate void ReapplyDrivenProperties(RectTransform driven);
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.AspectRatioFitter
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Resizes a RectTransform to fit a specified aspect ratio.</para>
  /// </summary>
  [RequireComponent(typeof (RectTransform))]
  [ExecuteInEditMode]
  [AddComponentMenu("Layout/Aspect Ratio Fitter", 142)]
  public class AspectRatioFitter : UIBehaviour, ILayoutController, ILayoutSelfController
  {
    [SerializeField]
    private float m_AspectRatio = 1f;
    [SerializeField]
    private AspectRatioFitter.AspectMode m_AspectMode;
    [NonSerialized]
    private RectTransform m_Rect;
    private DrivenRectTransformTracker m_Tracker;

    /// <summary>
    ///   <para>The mode to use to enforce the aspect ratio.</para>
    /// </summary>
    public AspectRatioFitter.AspectMode aspectMode
    {
      get
      {
        return this.m_AspectMode;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<AspectRatioFitter.AspectMode>(ref this.m_AspectMode, value))
          return;
        this.SetDirty();
      }
    }

    /// <summary>
    ///   <para>The aspect ratio to enforce. This means width divided by height.</para>
    /// </summary>
    public float aspectRatio
    {
      get
      {
        return this.m_AspectRatio;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this.m_AspectRatio, value))
          return;
        this.SetDirty();
      }
    }

    private RectTransform rectTransform
    {
      get
      {
        if ((UnityEngine.Object) this.m_Rect == (UnityEngine.Object) null)
          this.m_Rect = this.GetComponent<RectTransform>();
        return this.m_Rect;
      }
    }

    protected AspectRatioFitter()
    {
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.SetDirty();
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected override void OnDisable()
    {
      this.m_Tracker.Clear();
      LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
      base.OnDisable();
    }

    protected override void OnRectTransformDimensionsChange()
    {
      this.UpdateRect();
    }

    private void UpdateRect()
    {
      if (!this.IsActive())
        return;
      this.m_Tracker.Clear();
      switch (this.m_AspectMode)
      {
        case AspectRatioFitter.AspectMode.None:
          if (Application.isPlaying)
            break;
          this.m_AspectRatio = Mathf.Clamp(this.rectTransform.rect.width / this.rectTransform.rect.height, 1f / 1000f, 1000f);
          break;
        case AspectRatioFitter.AspectMode.WidthControlsHeight:
          this.m_Tracker.Add((UnityEngine.Object) this, this.rectTransform, DrivenTransformProperties.SizeDeltaY);
          this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.rectTransform.rect.width / this.m_AspectRatio);
          break;
        case AspectRatioFitter.AspectMode.HeightControlsWidth:
          this.m_Tracker.Add((UnityEngine.Object) this, this.rectTransform, DrivenTransformProperties.SizeDeltaX);
          this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.rectTransform.rect.height * this.m_AspectRatio);
          break;
        case AspectRatioFitter.AspectMode.FitInParent:
        case AspectRatioFitter.AspectMode.EnvelopeParent:
          this.m_Tracker.Add((UnityEngine.Object) this, this.rectTransform, DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.SizeDelta);
          this.rectTransform.anchorMin = Vector2.zero;
          this.rectTransform.anchorMax = Vector2.one;
          this.rectTransform.anchoredPosition = Vector2.zero;
          Vector2 zero = Vector2.zero;
          Vector2 parentSize = this.GetParentSize();
          if ((double) parentSize.y * (double) this.aspectRatio < (double) parentSize.x ^ this.m_AspectMode == AspectRatioFitter.AspectMode.FitInParent)
            zero.y = this.GetSizeDeltaToProduceSize(parentSize.x / this.aspectRatio, 1);
          else
            zero.x = this.GetSizeDeltaToProduceSize(parentSize.y * this.aspectRatio, 0);
          this.rectTransform.sizeDelta = zero;
          break;
      }
    }

    private float GetSizeDeltaToProduceSize(float size, int axis)
    {
      return size - this.GetParentSize()[axis] * (this.rectTransform.anchorMax[axis] - this.rectTransform.anchorMin[axis]);
    }

    private Vector2 GetParentSize()
    {
      RectTransform parent = this.rectTransform.parent as RectTransform;
      if (!(bool) ((UnityEngine.Object) parent))
        return Vector2.zero;
      return parent.rect.size;
    }

    /// <summary>
    ///   <para>Method called by the layout system.</para>
    /// </summary>
    public virtual void SetLayoutHorizontal()
    {
    }

    /// <summary>
    ///   <para>Method called by the layout system.</para>
    /// </summary>
    public virtual void SetLayoutVertical()
    {
    }

    /// <summary>
    ///   <para>Mark the AspectRatioFitter as dirty.</para>
    /// </summary>
    protected void SetDirty()
    {
      if (!this.IsActive())
        return;
      this.UpdateRect();
    }

    protected override void OnValidate()
    {
      this.m_AspectRatio = Mathf.Clamp(this.m_AspectRatio, 1f / 1000f, 1000f);
      this.SetDirty();
    }

    /// <summary>
    ///   <para>Specifies a mode to use to enforce an aspect ratio.</para>
    /// </summary>
    public enum AspectMode
    {
      None,
      WidthControlsHeight,
      HeightControlsWidth,
      FitInParent,
      EnvelopeParent,
    }
  }
}

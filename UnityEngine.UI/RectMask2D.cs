using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>A 2D rectangular mask that allows for clipping / masking of areas outside the mask.</para>
	/// </summary>
	[AddComponentMenu("UI/2D Rect Mask", 13), DisallowMultipleComponent, ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
	public class RectMask2D : UIBehaviour, ICanvasRaycastFilter, IClipper
	{
		[NonSerialized]
		private readonly RectangularVertexClipper m_VertexClipper = new RectangularVertexClipper();

		[NonSerialized]
		private RectTransform m_RectTransform;

		[NonSerialized]
		private List<IClippable> m_ClipTargets = new List<IClippable>();

		[NonSerialized]
		private bool m_ShouldRecalculateClipRects;

		[NonSerialized]
		private List<RectMask2D> m_Clippers = new List<RectMask2D>();

		[NonSerialized]
		private Rect m_LastClipRectCanvasSpace;

		[NonSerialized]
		private bool m_LastClipRectValid;

		/// <summary>
		///   <para>Get the Rect for the mask in canvas space.</para>
		/// </summary>
		public Rect canvasRect
		{
			get
			{
				Canvas c = null;
				List<Canvas> list = ListPool<Canvas>.Get();
				base.gameObject.GetComponentsInParent<Canvas>(false, list);
				if (list.Count > 0)
				{
					c = list[0];
				}
				ListPool<Canvas>.Release(list);
				return this.m_VertexClipper.GetCanvasRect(this.rectTransform, c);
			}
		}

		/// <summary>
		///   <para>Get the RectTransform for the mask.</para>
		/// </summary>
		public RectTransform rectTransform
		{
			get
			{
				RectTransform arg_1C_0;
				if ((arg_1C_0 = this.m_RectTransform) == null)
				{
					arg_1C_0 = (this.m_RectTransform = base.GetComponent<RectTransform>());
				}
				return arg_1C_0;
			}
		}

		protected RectMask2D()
		{
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this.m_ShouldRecalculateClipRects = true;
			ClipperRegistry.Register(this);
			MaskUtilities.Notify2DMaskStateChanged(this);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			this.m_ClipTargets.Clear();
			this.m_Clippers.Clear();
			ClipperRegistry.Unregister(this);
			MaskUtilities.Notify2DMaskStateChanged(this);
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			this.m_ShouldRecalculateClipRects = true;
			if (!this.IsActive())
			{
				return;
			}
			MaskUtilities.Notify2DMaskStateChanged(this);
		}

		/// <summary>
		///   <para>See:ICanvasRaycastFilter.</para>
		/// </summary>
		/// <param name="sp"></param>
		/// <param name="eventCamera"></param>
		public virtual bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			return !base.isActiveAndEnabled || RectTransformUtility.RectangleContainsScreenPoint(this.rectTransform, sp, eventCamera);
		}

		/// <summary>
		///   <para>See: IClipper.PerformClipping.</para>
		/// </summary>
		public virtual void PerformClipping()
		{
			if (this.m_ShouldRecalculateClipRects)
			{
				MaskUtilities.GetRectMasksForClip(this, this.m_Clippers);
				this.m_ShouldRecalculateClipRects = false;
			}
			bool flag = true;
			Rect rect = Clipping.FindCullAndClipWorldRect(this.m_Clippers, out flag);
			if (rect != this.m_LastClipRectCanvasSpace)
			{
				for (int i = 0; i < this.m_ClipTargets.Count; i++)
				{
					this.m_ClipTargets[i].SetClipRect(rect, flag);
				}
				this.m_LastClipRectCanvasSpace = rect;
				this.m_LastClipRectValid = flag;
			}
			for (int j = 0; j < this.m_ClipTargets.Count; j++)
			{
				this.m_ClipTargets[j].Cull(this.m_LastClipRectCanvasSpace, this.m_LastClipRectValid);
			}
		}

		/// <summary>
		///   <para>Add a [IClippable]] to be tracked by the mask.</para>
		/// </summary>
		/// <param name="clippable"></param>
		public void AddClippable(IClippable clippable)
		{
			if (clippable == null)
			{
				return;
			}
			if (!this.m_ClipTargets.Contains(clippable))
			{
				this.m_ClipTargets.Add(clippable);
			}
			clippable.SetClipRect(this.m_LastClipRectCanvasSpace, this.m_LastClipRectValid);
			clippable.Cull(this.m_LastClipRectCanvasSpace, this.m_LastClipRectValid);
		}

		/// <summary>
		///   <para>Remove an IClippable from being tracked by the mask.</para>
		/// </summary>
		/// <param name="clippable"></param>
		public void RemoveClippable(IClippable clippable)
		{
			if (clippable == null)
			{
				return;
			}
			clippable.SetClipRect(default(Rect), false);
			this.m_ClipTargets.Remove(clippable);
		}

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.m_ShouldRecalculateClipRects = true;
		}

		protected override void OnCanvasHierarchyChanged()
		{
			base.OnCanvasHierarchyChanged();
			this.m_ShouldRecalculateClipRects = true;
		}
	}
}

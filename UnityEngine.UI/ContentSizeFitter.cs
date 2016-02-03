using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Resizes a RectTransform to fit the size of its content.</para>
	/// </summary>
	[AddComponentMenu("Layout/Content Size Fitter", 141), ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
	public class ContentSizeFitter : UIBehaviour, ILayoutController, ILayoutSelfController
	{
		/// <summary>
		///   <para>The size fit mode to use.</para>
		/// </summary>
		public enum FitMode
		{
			/// <summary>
			///   <para>Don't perform any resizing.</para>
			/// </summary>
			Unconstrained,
			/// <summary>
			///   <para>Resize to the minimum size of the content.</para>
			/// </summary>
			MinSize,
			/// <summary>
			///   <para>Resize to the preferred size of the content.</para>
			/// </summary>
			PreferredSize
		}

		[SerializeField]
		protected ContentSizeFitter.FitMode m_HorizontalFit;

		[SerializeField]
		protected ContentSizeFitter.FitMode m_VerticalFit;

		[NonSerialized]
		private RectTransform m_Rect;

		private DrivenRectTransformTracker m_Tracker;

		/// <summary>
		///   <para>The fit mode to use to determine the width.</para>
		/// </summary>
		public ContentSizeFitter.FitMode horizontalFit
		{
			get
			{
				return this.m_HorizontalFit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<ContentSizeFitter.FitMode>(ref this.m_HorizontalFit, value))
				{
					this.SetDirty();
				}
			}
		}

		/// <summary>
		///   <para>The fit mode to use to determine the height.</para>
		/// </summary>
		public ContentSizeFitter.FitMode verticalFit
		{
			get
			{
				return this.m_VerticalFit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<ContentSizeFitter.FitMode>(ref this.m_VerticalFit, value))
				{
					this.SetDirty();
				}
			}
		}

		private RectTransform rectTransform
		{
			get
			{
				if (this.m_Rect == null)
				{
					this.m_Rect = base.GetComponent<RectTransform>();
				}
				return this.m_Rect;
			}
		}

		protected ContentSizeFitter()
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
			this.SetDirty();
		}

		private void HandleSelfFittingAlongAxis(int axis)
		{
			ContentSizeFitter.FitMode fitMode = (axis != 0) ? this.verticalFit : this.horizontalFit;
			if (fitMode == ContentSizeFitter.FitMode.Unconstrained)
			{
				return;
			}
			this.m_Tracker.Add(this, this.rectTransform, (axis != 0) ? DrivenTransformProperties.SizeDeltaY : DrivenTransformProperties.SizeDeltaX);
			if (fitMode == ContentSizeFitter.FitMode.MinSize)
			{
				this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetMinSize(this.m_Rect, axis));
			}
			else
			{
				this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetPreferredSize(this.m_Rect, axis));
			}
		}

		/// <summary>
		///   <para>Method called by the layout system.</para>
		/// </summary>
		public virtual void SetLayoutHorizontal()
		{
			this.m_Tracker.Clear();
			this.HandleSelfFittingAlongAxis(0);
		}

		/// <summary>
		///   <para>Method called by the layout system.</para>
		/// </summary>
		public virtual void SetLayoutVertical()
		{
			this.HandleSelfFittingAlongAxis(1);
		}

		/// <summary>
		///   <para>Mark the ContentSizeFitter as dirty.</para>
		/// </summary>
		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		}

		protected override void OnValidate()
		{
			this.SetDirty();
		}
	}
}

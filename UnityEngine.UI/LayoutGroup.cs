using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Abstract base class to use for layout groups.</para>
	/// </summary>
	[DisallowMultipleComponent, ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
	public abstract class LayoutGroup : UIBehaviour, ILayoutElement, ILayoutController, ILayoutGroup
	{
		[SerializeField]
		protected RectOffset m_Padding = new RectOffset();

		[FormerlySerializedAs("m_Alignment"), SerializeField]
		protected TextAnchor m_ChildAlignment;

		[NonSerialized]
		private RectTransform m_Rect;

		protected DrivenRectTransformTracker m_Tracker;

		private Vector2 m_TotalMinSize = Vector2.zero;

		private Vector2 m_TotalPreferredSize = Vector2.zero;

		private Vector2 m_TotalFlexibleSize = Vector2.zero;

		[NonSerialized]
		private List<RectTransform> m_RectChildren = new List<RectTransform>();

		/// <summary>
		///   <para>The padding to add around the child layout elements.</para>
		/// </summary>
		public RectOffset padding
		{
			get
			{
				return this.m_Padding;
			}
			set
			{
				this.SetProperty<RectOffset>(ref this.m_Padding, value);
			}
		}

		/// <summary>
		///   <para>The alignment to use for the child layout elements in the layout group.</para>
		/// </summary>
		public TextAnchor childAlignment
		{
			get
			{
				return this.m_ChildAlignment;
			}
			set
			{
				this.SetProperty<TextAnchor>(ref this.m_ChildAlignment, value);
			}
		}

		protected RectTransform rectTransform
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

		protected List<RectTransform> rectChildren
		{
			get
			{
				return this.m_RectChildren;
			}
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual float minWidth
		{
			get
			{
				return this.GetTotalMinSize(0);
			}
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual float preferredWidth
		{
			get
			{
				return this.GetTotalPreferredSize(0);
			}
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual float flexibleWidth
		{
			get
			{
				return this.GetTotalFlexibleSize(0);
			}
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual float minHeight
		{
			get
			{
				return this.GetTotalMinSize(1);
			}
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual float preferredHeight
		{
			get
			{
				return this.GetTotalPreferredSize(1);
			}
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual float flexibleHeight
		{
			get
			{
				return this.GetTotalFlexibleSize(1);
			}
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual int layoutPriority
		{
			get
			{
				return 0;
			}
		}

		private bool isRootLayoutGroup
		{
			get
			{
				Transform parent = base.transform.parent;
				return parent == null || base.transform.parent.GetComponent(typeof(ILayoutGroup)) == null;
			}
		}

		protected LayoutGroup()
		{
			if (this.m_Padding == null)
			{
				this.m_Padding = new RectOffset();
			}
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual void CalculateLayoutInputHorizontal()
		{
			this.m_RectChildren.Clear();
			List<Component> list = ListPool<Component>.Get();
			for (int i = 0; i < this.rectTransform.childCount; i++)
			{
				RectTransform rectTransform = this.rectTransform.GetChild(i) as RectTransform;
				if (!(rectTransform == null) && rectTransform.gameObject.activeInHierarchy)
				{
					rectTransform.GetComponents(typeof(ILayoutIgnorer), list);
					if (list.Count == 0)
					{
						this.m_RectChildren.Add(rectTransform);
					}
					else
					{
						for (int j = 0; j < list.Count; j++)
						{
							ILayoutIgnorer layoutIgnorer = (ILayoutIgnorer)list[j];
							if (!layoutIgnorer.ignoreLayout)
							{
								this.m_RectChildren.Add(rectTransform);
								break;
							}
						}
					}
				}
			}
			ListPool<Component>.Release(list);
			this.m_Tracker.Clear();
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public abstract void CalculateLayoutInputVertical();

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public abstract void SetLayoutHorizontal();

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public abstract void SetLayoutVertical();

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

		/// <summary>
		///   <para>Callback for when properties have been changed by animation.</para>
		/// </summary>
		protected override void OnDidApplyAnimationProperties()
		{
			this.SetDirty();
		}

		/// <summary>
		///   <para>The min size for the layout group on the given axis.</para>
		/// </summary>
		/// <param name="axis">The axis index. 0 is horizontal and 1 is vertical.</param>
		/// <returns>
		///   <para>The min size.</para>
		/// </returns>
		protected float GetTotalMinSize(int axis)
		{
			return this.m_TotalMinSize[axis];
		}

		/// <summary>
		///   <para>The preferred size for the layout group on the given axis.</para>
		/// </summary>
		/// <param name="axis">The axis index. 0 is horizontal and 1 is vertical.</param>
		/// <returns>
		///   <para>The preferred size.</para>
		/// </returns>
		protected float GetTotalPreferredSize(int axis)
		{
			return this.m_TotalPreferredSize[axis];
		}

		/// <summary>
		///   <para>The flexible size for the layout group on the given axis.</para>
		/// </summary>
		/// <param name="axis">The axis index. 0 is horizontal and 1 is vertical.</param>
		/// <returns>
		///   <para>The flexible size.</para>
		/// </returns>
		protected float GetTotalFlexibleSize(int axis)
		{
			return this.m_TotalFlexibleSize[axis];
		}

		/// <summary>
		///   <para>Returns the calculated position of the first child layout element along the given axis.</para>
		/// </summary>
		/// <param name="axis">The axis index. 0 is horizontal and 1 is vertical.</param>
		/// <param name="requiredSpaceWithoutPadding">The total space required on the given axis for all the layout elements including spacing and excluding padding.</param>
		/// <returns>
		///   <para>The position of the first child along the given axis.</para>
		/// </returns>
		protected float GetStartOffset(int axis, float requiredSpaceWithoutPadding)
		{
			float num = requiredSpaceWithoutPadding + (float)((axis != 0) ? this.padding.vertical : this.padding.horizontal);
			float num2 = this.rectTransform.rect.size[axis];
			float num3 = num2 - num;
			float num4;
			if (axis == 0)
			{
				num4 = (float)(this.childAlignment % TextAnchor.MiddleLeft) * 0.5f;
			}
			else
			{
				num4 = (float)(this.childAlignment / TextAnchor.MiddleLeft) * 0.5f;
			}
			return (float)((axis != 0) ? this.padding.top : this.padding.left) + num3 * num4;
		}

		/// <summary>
		///   <para>Used to set the calculated layout properties for the given axis.</para>
		/// </summary>
		/// <param name="totalMin">The min size for the layout group.</param>
		/// <param name="totalPreferred">The preferred size for the layout group.</param>
		/// <param name="totalFlexible">The flexible size for the layout group.</param>
		/// <param name="axis">The axis to set sizes for. 0 is horizontal and 1 is vertical.</param>
		protected void SetLayoutInputForAxis(float totalMin, float totalPreferred, float totalFlexible, int axis)
		{
			this.m_TotalMinSize[axis] = totalMin;
			this.m_TotalPreferredSize[axis] = totalPreferred;
			this.m_TotalFlexibleSize[axis] = totalFlexible;
		}

		/// <summary>
		///   <para>Set the position and size of a child layout element along the given axis.</para>
		/// </summary>
		/// <param name="rect">The RectTransform of the child layout element.</param>
		/// <param name="axis">The axis to set the position and size along. 0 is horizontal and 1 is vertical.</param>
		/// <param name="pos">The position from the left side or top.</param>
		/// <param name="size">The size.</param>
		protected void SetChildAlongAxis(RectTransform rect, int axis, float pos, float size)
		{
			if (rect == null)
			{
				return;
			}
			this.m_Tracker.Add(this, rect, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
			rect.SetInsetAndSizeFromParentEdge((axis != 0) ? RectTransform.Edge.Top : RectTransform.Edge.Left, pos, size);
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (this.isRootLayoutGroup)
			{
				this.SetDirty();
			}
		}

		protected virtual void OnTransformChildrenChanged()
		{
			this.SetDirty();
		}

		protected void SetProperty<T>(ref T currentValue, T newValue)
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return;
			}
			currentValue = newValue;
			this.SetDirty();
		}

		/// <summary>
		///   <para>Mark the LayoutGroup as dirty.</para>
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

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
			{
				return LayoutUtility.GetMinWidth(rect);
			}
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
			{
				return LayoutUtility.GetPreferredWidth(rect);
			}
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
			{
				return LayoutUtility.GetFlexibleWidth(rect);
			}
			return LayoutUtility.GetFlexibleHeight(rect);
		}

		/// <summary>
		///   <para>Returns the minimum width of the layout element.</para>
		/// </summary>
		/// <param name="rect">The RectTransform of the layout element to query.</param>
		public static float GetMinWidth(RectTransform rect)
		{
			return LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.minWidth, 0f);
		}

		/// <summary>
		///   <para>Returns the preferred width of the layout element.</para>
		/// </summary>
		/// <param name="rect">The RectTransform of the layout element to query.</param>
		public static float GetPreferredWidth(RectTransform rect)
		{
			return Mathf.Max(LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.minWidth, 0f), LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.preferredWidth, 0f));
		}

		/// <summary>
		///   <para>Returns the flexible width of the layout element.</para>
		/// </summary>
		/// <param name="rect">The RectTransform of the layout element to query.</param>
		public static float GetFlexibleWidth(RectTransform rect)
		{
			return LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.flexibleWidth, 0f);
		}

		/// <summary>
		///   <para>Returns the minimum height of the layout element.</para>
		/// </summary>
		/// <param name="rect">The RectTransform of the layout element to query.</param>
		public static float GetMinHeight(RectTransform rect)
		{
			return LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.minHeight, 0f);
		}

		/// <summary>
		///   <para>Returns the preferred height of the layout element.</para>
		/// </summary>
		/// <param name="rect">The RectTransform of the layout element to query.</param>
		public static float GetPreferredHeight(RectTransform rect)
		{
			return Mathf.Max(LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.minHeight, 0f), LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.preferredHeight, 0f));
		}

		/// <summary>
		///   <para>Returns the flexible height of the layout element.</para>
		/// </summary>
		/// <param name="rect">The RectTransform of the layout element to query.</param>
		public static float GetFlexibleHeight(RectTransform rect)
		{
			return LayoutUtility.GetLayoutProperty(rect, (ILayoutElement e) => e.flexibleHeight, 0f);
		}

		public static float GetLayoutProperty(RectTransform rect, Func<ILayoutElement, float> property, float defaultValue)
		{
			ILayoutElement layoutElement;
			return LayoutUtility.GetLayoutProperty(rect, property, defaultValue, out layoutElement);
		}

		public static float GetLayoutProperty(RectTransform rect, Func<ILayoutElement, float> property, float defaultValue, out ILayoutElement source)
		{
			source = null;
			if (rect == null)
			{
				return 0f;
			}
			float num = defaultValue;
			int num2 = -2147483648;
			List<Component> list = ListPool<Component>.Get();
			rect.GetComponents(typeof(ILayoutElement), list);
			for (int i = 0; i < list.Count; i++)
			{
				ILayoutElement layoutElement = list[i] as ILayoutElement;
				if (!(layoutElement is Behaviour) || ((Behaviour)layoutElement).isActiveAndEnabled)
				{
					int layoutPriority = layoutElement.layoutPriority;
					if (layoutPriority >= num2)
					{
						float num3 = property(layoutElement);
						if (num3 >= 0f)
						{
							if (layoutPriority > num2)
							{
								num = num3;
								num2 = layoutPriority;
								source = layoutElement;
							}
							else if (num3 > num)
							{
								num = num3;
								source = layoutElement;
							}
						}
					}
				}
			}
			ListPool<Component>.Release(list);
			return num;
		}
	}
}

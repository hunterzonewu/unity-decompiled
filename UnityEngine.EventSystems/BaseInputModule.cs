using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
	/// <summary>
	///   <para>A base module that raises events and sends them to GameObjects.</para>
	/// </summary>
	[RequireComponent(typeof(EventSystem))]
	public abstract class BaseInputModule : UIBehaviour
	{
		[NonSerialized]
		protected List<RaycastResult> m_RaycastResultCache = new List<RaycastResult>();

		private AxisEventData m_AxisEventData;

		private EventSystem m_EventSystem;

		private BaseEventData m_BaseEventData;

		protected EventSystem eventSystem
		{
			get
			{
				return this.m_EventSystem;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this.m_EventSystem = base.GetComponent<EventSystem>();
			this.m_EventSystem.UpdateModules();
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnDisable.</para>
		/// </summary>
		protected override void OnDisable()
		{
			this.m_EventSystem.UpdateModules();
			base.OnDisable();
		}

		/// <summary>
		///   <para>Process the current tick for the module.</para>
		/// </summary>
		public abstract void Process();

		protected static RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
		{
			for (int i = 0; i < candidates.Count; i++)
			{
				if (!(candidates[i].gameObject == null))
				{
					return candidates[i];
				}
			}
			return default(RaycastResult);
		}

		/// <summary>
		///   <para>Given an input movement, determine the best MoveDirection.</para>
		/// </summary>
		/// <param name="x">X movement.</param>
		/// <param name="y">Y movement.</param>
		/// <param name="deadZone">Dead zone.</param>
		protected static MoveDirection DetermineMoveDirection(float x, float y)
		{
			return BaseInputModule.DetermineMoveDirection(x, y, 0.6f);
		}

		/// <summary>
		///   <para>Given an input movement, determine the best MoveDirection.</para>
		/// </summary>
		/// <param name="x">X movement.</param>
		/// <param name="y">Y movement.</param>
		/// <param name="deadZone">Dead zone.</param>
		protected static MoveDirection DetermineMoveDirection(float x, float y, float deadZone)
		{
			Vector2 vector = new Vector2(x, y);
			if (vector.sqrMagnitude < deadZone * deadZone)
			{
				return MoveDirection.None;
			}
			if (Mathf.Abs(x) > Mathf.Abs(y))
			{
				if (x > 0f)
				{
					return MoveDirection.Right;
				}
				return MoveDirection.Left;
			}
			else
			{
				if (y > 0f)
				{
					return MoveDirection.Up;
				}
				return MoveDirection.Down;
			}
		}

		/// <summary>
		///   <para>Given 2 GameObjects, return a common root GameObject (or null).</para>
		/// </summary>
		/// <param name="g1"></param>
		/// <param name="g2"></param>
		protected static GameObject FindCommonRoot(GameObject g1, GameObject g2)
		{
			if (g1 == null || g2 == null)
			{
				return null;
			}
			Transform transform = g1.transform;
			while (transform != null)
			{
				Transform transform2 = g2.transform;
				while (transform2 != null)
				{
					if (transform == transform2)
					{
						return transform.gameObject;
					}
					transform2 = transform2.parent;
				}
				transform = transform.parent;
			}
			return null;
		}

		/// <summary>
		///   <para>Handle sending enter and exit events when a new enter targer is found.</para>
		/// </summary>
		/// <param name="currentPointerData"></param>
		/// <param name="newEnterTarget"></param>
		protected void HandlePointerExitAndEnter(PointerEventData currentPointerData, GameObject newEnterTarget)
		{
			if (newEnterTarget == null || currentPointerData.pointerEnter == null)
			{
				for (int i = 0; i < currentPointerData.hovered.Count; i++)
				{
					ExecuteEvents.Execute<IPointerExitHandler>(currentPointerData.hovered[i], currentPointerData, ExecuteEvents.pointerExitHandler);
				}
				currentPointerData.hovered.Clear();
				if (newEnterTarget == null)
				{
					currentPointerData.pointerEnter = newEnterTarget;
					return;
				}
			}
			if (currentPointerData.pointerEnter == newEnterTarget && newEnterTarget)
			{
				return;
			}
			GameObject gameObject = BaseInputModule.FindCommonRoot(currentPointerData.pointerEnter, newEnterTarget);
			if (currentPointerData.pointerEnter != null)
			{
				Transform transform = currentPointerData.pointerEnter.transform;
				while (transform != null)
				{
					if (gameObject != null && gameObject.transform == transform)
					{
						break;
					}
					ExecuteEvents.Execute<IPointerExitHandler>(transform.gameObject, currentPointerData, ExecuteEvents.pointerExitHandler);
					currentPointerData.hovered.Remove(transform.gameObject);
					transform = transform.parent;
				}
			}
			currentPointerData.pointerEnter = newEnterTarget;
			if (newEnterTarget != null)
			{
				Transform transform2 = newEnterTarget.transform;
				while (transform2 != null && transform2.gameObject != gameObject)
				{
					ExecuteEvents.Execute<IPointerEnterHandler>(transform2.gameObject, currentPointerData, ExecuteEvents.pointerEnterHandler);
					currentPointerData.hovered.Add(transform2.gameObject);
					transform2 = transform2.parent;
				}
			}
		}

		/// <summary>
		///   <para>Given some input data generate an AxisEventData that can be used by the event system.</para>
		/// </summary>
		/// <param name="x">X movement.</param>
		/// <param name="y">Y movement.</param>
		/// <param name="moveDeadZone">Dead Zone.</param>
		protected virtual AxisEventData GetAxisEventData(float x, float y, float moveDeadZone)
		{
			if (this.m_AxisEventData == null)
			{
				this.m_AxisEventData = new AxisEventData(this.eventSystem);
			}
			this.m_AxisEventData.Reset();
			this.m_AxisEventData.moveVector = new Vector2(x, y);
			this.m_AxisEventData.moveDir = BaseInputModule.DetermineMoveDirection(x, y, moveDeadZone);
			return this.m_AxisEventData;
		}

		/// <summary>
		///   <para>Generate a BaseEventData that can be used by the EventSystem.</para>
		/// </summary>
		protected virtual BaseEventData GetBaseEventData()
		{
			if (this.m_BaseEventData == null)
			{
				this.m_BaseEventData = new BaseEventData(this.eventSystem);
			}
			this.m_BaseEventData.Reset();
			return this.m_BaseEventData;
		}

		/// <summary>
		///   <para>Is the pointer with the given ID over an EventSystem object?</para>
		/// </summary>
		/// <param name="pointerId">Pointer ID.</param>
		public virtual bool IsPointerOverGameObject(int pointerId)
		{
			return false;
		}

		/// <summary>
		///   <para>Should be activated.</para>
		/// </summary>
		/// <returns>
		///   <para>Should the module be activated.</para>
		/// </returns>
		public virtual bool ShouldActivateModule()
		{
			return base.enabled && base.gameObject.activeInHierarchy;
		}

		/// <summary>
		///   <para>Called when the module is deactivated. Override this if you want custom code to execute when you deactivate your module.</para>
		/// </summary>
		public virtual void DeactivateModule()
		{
		}

		/// <summary>
		///   <para>Called when the module is activated. Override this if you want custom code to execute when you activate your module.</para>
		/// </summary>
		public virtual void ActivateModule()
		{
		}

		/// <summary>
		///   <para>Update the internal state of the Module.</para>
		/// </summary>
		public virtual void UpdateModule()
		{
		}

		/// <summary>
		///   <para>Check to see if the module is supported. Override this if you have a platfrom specific module (eg. TouchInputModule that you do not want to activate on standalone.</para>
		/// </summary>
		/// <returns>
		///   <para>Is the module supported.</para>
		/// </returns>
		public virtual bool IsModuleSupported()
		{
			return true;
		}
	}
}

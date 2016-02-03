using System;

namespace UnityEngine.EventSystems
{
	/// <summary>
	///   <para>Base behaviour that has protected implementations of Unity lifecycle functions.</para>
	/// </summary>
	public abstract class UIBehaviour : MonoBehaviour
	{
		/// <summary>
		///   <para>See MonoBehaviour.Awake.</para>
		/// </summary>
		protected virtual void Awake()
		{
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnEnable.</para>
		/// </summary>
		protected virtual void OnEnable()
		{
		}

		/// <summary>
		///   <para>See MonoBehaviour.Start.</para>
		/// </summary>
		protected virtual void Start()
		{
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnDisable.</para>
		/// </summary>
		protected virtual void OnDisable()
		{
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnDestroy.</para>
		/// </summary>
		protected virtual void OnDestroy()
		{
		}

		/// <summary>
		///   <para>Returns true if the GameObject and the Component are active.</para>
		/// </summary>
		/// <returns>
		///   <para>Active.</para>
		/// </returns>
		public virtual bool IsActive()
		{
			return base.isActiveAndEnabled;
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnValidate.</para>
		/// </summary>
		protected virtual void OnValidate()
		{
		}

		/// <summary>
		///   <para>See MonoBehaviour.Reset.</para>
		/// </summary>
		protected virtual void Reset()
		{
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnRectTransformDimensionsChange.</para>
		/// </summary>
		protected virtual void OnRectTransformDimensionsChange()
		{
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnBeforeTransformParentChanged.</para>
		/// </summary>
		protected virtual void OnBeforeTransformParentChanged()
		{
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnRectTransformParentChanged.</para>
		/// </summary>
		protected virtual void OnTransformParentChanged()
		{
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnDidApplyAnimationProperties.</para>
		/// </summary>
		protected virtual void OnDidApplyAnimationProperties()
		{
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnCanvasGroupChanged.</para>
		/// </summary>
		protected virtual void OnCanvasGroupChanged()
		{
		}

		/// <summary>
		///   <para>Called when the state of the parent Canvas is changed.</para>
		/// </summary>
		protected virtual void OnCanvasHierarchyChanged()
		{
		}

		/// <summary>
		///   <para>Returns true if the native representation of the behaviour has been destroyed.</para>
		/// </summary>
		/// <returns>
		///   <para>True if Destroyed.</para>
		/// </returns>
		public bool IsDestroyed()
		{
			return this == null;
		}
	}
}

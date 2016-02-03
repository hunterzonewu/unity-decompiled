using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>A standard button that can be clicked in order to trigger an event.</para>
	/// </summary>
	[AddComponentMenu("UI/Button", 30)]
	public class Button : Selectable, IEventSystemHandler, IPointerClickHandler, ISubmitHandler
	{
		/// <summary>
		///   <para>Function definition for a button click event.</para>
		/// </summary>
		[Serializable]
		public class ButtonClickedEvent : UnityEvent
		{
		}

		[FormerlySerializedAs("onClick"), SerializeField]
		private Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();

		/// <summary>
		///   <para>UnityEvent to be fired when the buttons is pressed.</para>
		/// </summary>
		public Button.ButtonClickedEvent onClick
		{
			get
			{
				return this.m_OnClick;
			}
			set
			{
				this.m_OnClick = value;
			}
		}

		protected Button()
		{
		}

		private void Press()
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			this.m_OnClick.Invoke();
		}

		/// <summary>
		///   <para>Registered IPointerClickHandler callback.</para>
		/// </summary>
		/// <param name="eventData">Data passed in (Typically by the event system).</param>
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.Press();
		}

		/// <summary>
		///   <para>Registered ISubmitHandler callback.</para>
		/// </summary>
		/// <param name="eventData">Data passed in (Typically by the event system).</param>
		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.Press();
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			this.DoStateTransition(Selectable.SelectionState.Pressed, false);
			base.StartCoroutine(this.OnFinishSubmit());
		}

		[DebuggerHidden]
		private IEnumerator OnFinishSubmit()
		{
			Button.<OnFinishSubmit>c__Iterator1 <OnFinishSubmit>c__Iterator = new Button.<OnFinishSubmit>c__Iterator1();
			<OnFinishSubmit>c__Iterator.<>f__this = this;
			return <OnFinishSubmit>c__Iterator;
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Turn a simple label into a interactable input field.</para>
	/// </summary>
	[AddComponentMenu("UI/Input Field", 31)]
	public class InputField : Selectable, IEventSystemHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IUpdateSelectedHandler, ISubmitHandler, ICanvasElement
	{
		/// <summary>
		///   <para>Specifies the type of the input text content.</para>
		/// </summary>
		public enum ContentType
		{
			/// <summary>
			///   <para>Allows all input.</para>
			/// </summary>
			Standard,
			/// <summary>
			///   <para>Allows all input and performs auto-correction on platforms that support it.</para>
			/// </summary>
			Autocorrected,
			/// <summary>
			///   <para>Allow whole numbers (positive or negative).</para>
			/// </summary>
			IntegerNumber,
			/// <summary>
			///   <para>Allows decimal numbers (positive or negative).</para>
			/// </summary>
			DecimalNumber,
			/// <summary>
			///   <para>Allows letters A-Z, a-z and numbers 0-9.</para>
			/// </summary>
			Alphanumeric,
			/// <summary>
			///   <para>The InputField is used for typing in a name and enforces capitalization.</para>
			/// </summary>
			Name,
			/// <summary>
			///   <para>The input is used for typing in an email address.</para>
			/// </summary>
			EmailAddress,
			/// <summary>
			///   <para>Allows all input and hides the typed characters by showing them as asterisks characters.</para>
			/// </summary>
			Password,
			/// <summary>
			///   <para>Allows integer numbers and hides the typed characters by showing them as asterisks characters.</para>
			/// </summary>
			Pin,
			/// <summary>
			///   <para>Custom types that allows user-defined settings.</para>
			/// </summary>
			Custom
		}

		/// <summary>
		///   <para>Type of data expected by the input field.</para>
		/// </summary>
		public enum InputType
		{
			/// <summary>
			///   <para>The standard mobile keyboard.</para>
			/// </summary>
			Standard,
			/// <summary>
			///   <para>The mobile autocorrect keyboard.</para>
			/// </summary>
			AutoCorrect,
			/// <summary>
			///   <para>The mobile password keyboard.</para>
			/// </summary>
			Password
		}

		/// <summary>
		///   <para>The type of characters that are allowed to be added to the string.</para>
		/// </summary>
		public enum CharacterValidation
		{
			/// <summary>
			///   <para>No validation. Any input is valid.</para>
			/// </summary>
			None,
			/// <summary>
			///   <para>Allow whole numbers (positive or negative).</para>
			/// </summary>
			Integer,
			/// <summary>
			///   <para>Allows decimal numbers (positive or negative).
			/// Characters 0-9, . (dot), and - (dash / minus sign) are allowed. The dash is only allowed as the first character. Only one dot in the string is allowed.</para>
			/// </summary>
			Decimal,
			/// <summary>
			///   <para>Allows letters A-Z, a-z and numbers 0-9.</para>
			/// </summary>
			Alphanumeric,
			/// <summary>
			///   <para>Only allow names and enforces capitalization.</para>
			/// </summary>
			Name,
			/// <summary>
			///   <para>Allows the characters that are allowed in an email address.</para>
			/// </summary>
			EmailAddress
		}

		/// <summary>
		///   <para>The LineType is used to describe the behavior of the InputField.</para>
		/// </summary>
		public enum LineType
		{
			/// <summary>
			///   <para>Only allows 1 line to be entered. Has horizontal scrolling and no word wrap. Pressing enter will submit the InputField.</para>
			/// </summary>
			SingleLine,
			/// <summary>
			///   <para>Is a multiline InputField with vertical scrolling and overflow. Pressing the return key will submit.</para>
			/// </summary>
			MultiLineSubmit,
			/// <summary>
			///   <para>Is a multiline InputField with vertical scrolling and overflow. Pressing the return key will insert a new line character.</para>
			/// </summary>
			MultiLineNewline
		}

		/// <summary>
		///   <para>Unity Event with a inputfield as a param.</para>
		/// </summary>
		[Serializable]
		public class SubmitEvent : UnityEvent<string>
		{
		}

		/// <summary>
		///   <para>The callback sent anytime the Inputfield is updated.</para>
		/// </summary>
		[Serializable]
		public class OnChangeEvent : UnityEvent<string>
		{
		}

		protected enum EditState
		{
			Continue,
			Finish
		}

		/// <summary>
		///   <para>Custom validation callback.</para>
		/// </summary>
		/// <param name="text"></param>
		/// <param name="charIndex"></param>
		/// <param name="addedChar"></param>
		public delegate char OnValidateInput(string text, int charIndex, char addedChar);

		private const float kHScrollSpeed = 0.05f;

		private const float kVScrollSpeed = 0.1f;

		private const string kEmailSpecialCharacters = "!#$%&'*+-/=?^_`{|}~";

		protected TouchScreenKeyboard m_Keyboard;

		private static readonly char[] kSeparators = new char[]
		{
			' ',
			'.',
			','
		};

		[FormerlySerializedAs("text"), SerializeField]
		protected Text m_TextComponent;

		[SerializeField]
		protected Graphic m_Placeholder;

		[SerializeField]
		private InputField.ContentType m_ContentType;

		[FormerlySerializedAs("inputType"), SerializeField]
		private InputField.InputType m_InputType;

		[FormerlySerializedAs("asteriskChar"), SerializeField]
		private char m_AsteriskChar = '*';

		[FormerlySerializedAs("keyboardType"), SerializeField]
		private TouchScreenKeyboardType m_KeyboardType;

		[SerializeField]
		private InputField.LineType m_LineType;

		[FormerlySerializedAs("hideMobileInput"), SerializeField]
		private bool m_HideMobileInput;

		[FormerlySerializedAs("validation"), SerializeField]
		private InputField.CharacterValidation m_CharacterValidation;

		[FormerlySerializedAs("characterLimit"), SerializeField]
		private int m_CharacterLimit;

		[FormerlySerializedAs("m_OnSubmit"), FormerlySerializedAs("m_EndEdit"), FormerlySerializedAs("onSubmit"), SerializeField]
		private InputField.SubmitEvent m_OnEndEdit = new InputField.SubmitEvent();

		[FormerlySerializedAs("m_OnValueChange"), FormerlySerializedAs("onValueChange"), SerializeField]
		private InputField.OnChangeEvent m_OnValueChanged = new InputField.OnChangeEvent();

		[FormerlySerializedAs("onValidateInput"), SerializeField]
		private InputField.OnValidateInput m_OnValidateInput;

		[SerializeField]
		private Color m_CaretColor = new Color(0.196078435f, 0.196078435f, 0.196078435f, 1f);

		[SerializeField]
		private bool m_CustomCaretColor;

		[FormerlySerializedAs("selectionColor"), SerializeField]
		private Color m_SelectionColor = new Color(0.65882355f, 0.807843149f, 1f, 0.7529412f);

		[FormerlySerializedAs("mValue"), SerializeField]
		protected string m_Text = string.Empty;

		[Range(0f, 4f), SerializeField]
		private float m_CaretBlinkRate = 0.85f;

		[Range(1f, 5f), SerializeField]
		private int m_CaretWidth = 1;

		[SerializeField]
		private bool m_ReadOnly;

		protected int m_CaretPosition;

		protected int m_CaretSelectPosition;

		private RectTransform caretRectTrans;

		protected UIVertex[] m_CursorVerts;

		private TextGenerator m_InputTextCache;

		private CanvasRenderer m_CachedInputRenderer;

		private bool m_PreventFontCallback;

		[NonSerialized]
		protected Mesh m_Mesh;

		private bool m_AllowInput;

		private bool m_ShouldActivateNextUpdate;

		private bool m_UpdateDrag;

		private bool m_DragPositionOutOfBounds;

		protected bool m_CaretVisible;

		private Coroutine m_BlinkCoroutine;

		private float m_BlinkStartTime;

		protected int m_DrawStart;

		protected int m_DrawEnd;

		private Coroutine m_DragCoroutine;

		private string m_OriginalText = string.Empty;

		private bool m_WasCanceled;

		private bool m_HasDoneFocusTransition;

		private Event m_ProcessingEvent = new Event();

		protected Mesh mesh
		{
			get
			{
				if (this.m_Mesh == null)
				{
					this.m_Mesh = new Mesh();
				}
				return this.m_Mesh;
			}
		}

		protected TextGenerator cachedInputTextGenerator
		{
			get
			{
				if (this.m_InputTextCache == null)
				{
					this.m_InputTextCache = new TextGenerator();
				}
				return this.m_InputTextCache;
			}
		}

		/// <summary>
		///   <para>Should the mobile keyboard input be hidden.</para>
		/// </summary>
		public bool shouldHideMobileInput
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				switch (platform)
				{
				case RuntimePlatform.IPhonePlayer:
				case RuntimePlatform.Android:
					goto IL_3B;
				case RuntimePlatform.PS3:
				case RuntimePlatform.XBOX360:
					IL_1E:
					if (platform != RuntimePlatform.BB10Player && platform != RuntimePlatform.TizenPlayer && platform != RuntimePlatform.tvOS)
					{
						return true;
					}
					goto IL_3B;
				}
				goto IL_1E;
				IL_3B:
				return this.m_HideMobileInput;
			}
			set
			{
				SetPropertyUtility.SetStruct<bool>(ref this.m_HideMobileInput, value);
			}
		}

		private bool shouldActivateOnSelect
		{
			get
			{
				return Application.platform != RuntimePlatform.tvOS;
			}
		}

		/// <summary>
		///   <para>The current value of the input field.</para>
		/// </summary>
		public string text
		{
			get
			{
				if (this.m_Keyboard != null && this.m_Keyboard.active && !this.InPlaceEditing() && EventSystem.current.currentSelectedGameObject == base.gameObject)
				{
					return this.m_Keyboard.text;
				}
				return this.m_Text;
			}
			set
			{
				if (this.text == value)
				{
					return;
				}
				this.m_Text = value;
				if (!Application.isPlaying)
				{
					this.SendOnValueChangedAndUpdateLabel();
					return;
				}
				if (this.m_Keyboard != null)
				{
					this.m_Keyboard.text = this.m_Text;
				}
				if (this.m_CaretPosition > this.m_Text.Length)
				{
					this.m_CaretPosition = (this.m_CaretSelectPosition = this.m_Text.Length);
				}
				this.SendOnValueChangedAndUpdateLabel();
			}
		}

		/// <summary>
		///   <para>Does the InputField currently have focus and is able to process events.</para>
		/// </summary>
		public bool isFocused
		{
			get
			{
				return this.m_AllowInput;
			}
		}

		/// <summary>
		///   <para>The blinking rate of the input caret, defined as the number of times the blink cycle occurs per second.</para>
		/// </summary>
		public float caretBlinkRate
		{
			get
			{
				return this.m_CaretBlinkRate;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_CaretBlinkRate, value) && this.m_AllowInput)
				{
					this.SetCaretActive();
				}
			}
		}

		/// <summary>
		///   <para>The width of the caret in pixels.</para>
		/// </summary>
		public int caretWidth
		{
			get
			{
				return this.m_CaretWidth;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<int>(ref this.m_CaretWidth, value))
				{
					this.MarkGeometryAsDirty();
				}
			}
		}

		/// <summary>
		///   <para>The Text component that is going to be used to render the text to screen.</para>
		/// </summary>
		public Text textComponent
		{
			get
			{
				return this.m_TextComponent;
			}
			set
			{
				SetPropertyUtility.SetClass<Text>(ref this.m_TextComponent, value);
			}
		}

		/// <summary>
		///   <para>Optional graphic to show when the InputField text is empty.
		/// A placeholder graphic can be used to show subtle hints or make it more obvious that the control is an InputField.</para>
		/// </summary>
		public Graphic placeholder
		{
			get
			{
				return this.m_Placeholder;
			}
			set
			{
				SetPropertyUtility.SetClass<Graphic>(ref this.m_Placeholder, value);
			}
		}

		/// <summary>
		///   <para>The custom caret color used if customCaretColor is set.</para>
		/// </summary>
		public Color caretColor
		{
			get
			{
				return (!this.customCaretColor) ? this.textComponent.color : this.m_CaretColor;
			}
			set
			{
				if (SetPropertyUtility.SetColor(ref this.m_CaretColor, value))
				{
					this.MarkGeometryAsDirty();
				}
			}
		}

		/// <summary>
		///   <para>Should a custom caret color be used or should the textComponent.color be used.</para>
		/// </summary>
		public bool customCaretColor
		{
			get
			{
				return this.m_CustomCaretColor;
			}
			set
			{
				if (this.m_CustomCaretColor != value)
				{
					this.m_CustomCaretColor = value;
					this.MarkGeometryAsDirty();
				}
			}
		}

		/// <summary>
		///   <para>The color of the highlight to show which characters are selected.</para>
		/// </summary>
		public Color selectionColor
		{
			get
			{
				return this.m_SelectionColor;
			}
			set
			{
				if (SetPropertyUtility.SetColor(ref this.m_SelectionColor, value))
				{
					this.MarkGeometryAsDirty();
				}
			}
		}

		/// <summary>
		///   <para>The Unity Event to call when editing has ended.</para>
		/// </summary>
		public InputField.SubmitEvent onEndEdit
		{
			get
			{
				return this.m_OnEndEdit;
			}
			set
			{
				SetPropertyUtility.SetClass<InputField.SubmitEvent>(ref this.m_OnEndEdit, value);
			}
		}

		/// <summary>
		///   <para>Accessor to the OnChangeEvent.</para>
		/// </summary>
		[Obsolete("onValueChange has been renamed to onValueChanged")]
		public InputField.OnChangeEvent onValueChange
		{
			get
			{
				return this.onValueChanged;
			}
			set
			{
				this.onValueChanged = value;
			}
		}

		/// <summary>
		///   <para>Accessor to the OnChangeEvent.</para>
		/// </summary>
		public InputField.OnChangeEvent onValueChanged
		{
			get
			{
				return this.m_OnValueChanged;
			}
			set
			{
				SetPropertyUtility.SetClass<InputField.OnChangeEvent>(ref this.m_OnValueChanged, value);
			}
		}

		/// <summary>
		///   <para>The function to call to validate the input characters.</para>
		/// </summary>
		public InputField.OnValidateInput onValidateInput
		{
			get
			{
				return this.m_OnValidateInput;
			}
			set
			{
				SetPropertyUtility.SetClass<InputField.OnValidateInput>(ref this.m_OnValidateInput, value);
			}
		}

		/// <summary>
		///   <para>How many characters the input field is limited to. 0 = infinite.</para>
		/// </summary>
		public int characterLimit
		{
			get
			{
				return this.m_CharacterLimit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<int>(ref this.m_CharacterLimit, Math.Max(0, value)))
				{
					this.UpdateLabel();
				}
			}
		}

		/// <summary>
		///   <para>Specifies the type of the input text content.</para>
		/// </summary>
		public InputField.ContentType contentType
		{
			get
			{
				return this.m_ContentType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<InputField.ContentType>(ref this.m_ContentType, value))
				{
					this.EnforceContentType();
				}
			}
		}

		/// <summary>
		///   <para>The LineType used by the InputField.</para>
		/// </summary>
		public InputField.LineType lineType
		{
			get
			{
				return this.m_LineType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<InputField.LineType>(ref this.m_LineType, value))
				{
					this.SetToCustomIfContentTypeIsNot(new InputField.ContentType[]
					{
						InputField.ContentType.Standard,
						InputField.ContentType.Autocorrected
					});
				}
			}
		}

		/// <summary>
		///   <para>The type of input expected. See InputField.InputType.</para>
		/// </summary>
		public InputField.InputType inputType
		{
			get
			{
				return this.m_InputType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<InputField.InputType>(ref this.m_InputType, value))
				{
					this.SetToCustom();
				}
			}
		}

		/// <summary>
		///   <para>They type of mobile keyboard that will be used.</para>
		/// </summary>
		public TouchScreenKeyboardType keyboardType
		{
			get
			{
				return this.m_KeyboardType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<TouchScreenKeyboardType>(ref this.m_KeyboardType, value))
				{
					this.SetToCustom();
				}
			}
		}

		/// <summary>
		///   <para>The type of validation to perform on a character.</para>
		/// </summary>
		public InputField.CharacterValidation characterValidation
		{
			get
			{
				return this.m_CharacterValidation;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<InputField.CharacterValidation>(ref this.m_CharacterValidation, value))
				{
					this.SetToCustom();
				}
			}
		}

		/// <summary>
		///   <para>Set the InputField to be read only.</para>
		/// </summary>
		public bool readOnly
		{
			get
			{
				return this.m_ReadOnly;
			}
			set
			{
				this.m_ReadOnly = value;
			}
		}

		/// <summary>
		///   <para>If the input field supports multiple lines.</para>
		/// </summary>
		public bool multiLine
		{
			get
			{
				return this.m_LineType == InputField.LineType.MultiLineNewline || this.lineType == InputField.LineType.MultiLineSubmit;
			}
		}

		/// <summary>
		///   <para>The character used for password fields.</para>
		/// </summary>
		public char asteriskChar
		{
			get
			{
				return this.m_AsteriskChar;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<char>(ref this.m_AsteriskChar, value))
				{
					this.UpdateLabel();
				}
			}
		}

		/// <summary>
		///   <para>If the UI.InputField was canceled and will revert back to the original text upon DeactivateInputField.</para>
		/// </summary>
		public bool wasCanceled
		{
			get
			{
				return this.m_WasCanceled;
			}
		}

		protected int caretPositionInternal
		{
			get
			{
				return this.m_CaretPosition + Input.compositionString.Length;
			}
			set
			{
				this.m_CaretPosition = value;
				this.ClampPos(ref this.m_CaretPosition);
			}
		}

		protected int caretSelectPositionInternal
		{
			get
			{
				return this.m_CaretSelectPosition + Input.compositionString.Length;
			}
			set
			{
				this.m_CaretSelectPosition = value;
				this.ClampPos(ref this.m_CaretSelectPosition);
			}
		}

		private bool hasSelection
		{
			get
			{
				return this.caretPositionInternal != this.caretSelectPositionInternal;
			}
		}

		/// <summary>
		///   <para>Current InputField selection head.</para>
		/// </summary>
		[Obsolete("caretSelectPosition has been deprecated. Use selectionFocusPosition instead (UnityUpgradable) -> selectionFocusPosition", true)]
		public int caretSelectPosition
		{
			get
			{
				return this.selectionFocusPosition;
			}
			protected set
			{
				this.selectionFocusPosition = value;
			}
		}

		/// <summary>
		///   <para>Current InputField caret position (also selection tail).</para>
		/// </summary>
		public int caretPosition
		{
			get
			{
				return this.m_CaretSelectPosition + Input.compositionString.Length;
			}
			set
			{
				this.selectionAnchorPosition = value;
				this.selectionFocusPosition = value;
			}
		}

		/// <summary>
		///   <para>The beginning point of the selection.</para>
		/// </summary>
		public int selectionAnchorPosition
		{
			get
			{
				return this.m_CaretPosition + Input.compositionString.Length;
			}
			set
			{
				if (Input.compositionString.Length != 0)
				{
					return;
				}
				this.m_CaretPosition = value;
				this.ClampPos(ref this.m_CaretPosition);
			}
		}

		/// <summary>
		///   <para>The the end point of the selection.</para>
		/// </summary>
		public int selectionFocusPosition
		{
			get
			{
				return this.m_CaretSelectPosition + Input.compositionString.Length;
			}
			set
			{
				if (Input.compositionString.Length != 0)
				{
					return;
				}
				this.m_CaretSelectPosition = value;
				this.ClampPos(ref this.m_CaretSelectPosition);
			}
		}

		private static string clipboard
		{
			get
			{
				return GUIUtility.systemCopyBuffer;
			}
			set
			{
				GUIUtility.systemCopyBuffer = value;
			}
		}

		protected InputField()
		{
		}

		protected void ClampPos(ref int pos)
		{
			if (pos < 0)
			{
				pos = 0;
			}
			else if (pos > this.text.Length)
			{
				pos = this.text.Length;
			}
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			this.EnforceContentType();
			this.m_CharacterLimit = Math.Max(0, this.m_CharacterLimit);
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateLabel();
			if (this.m_AllowInput)
			{
				this.SetCaretActive();
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.m_Text == null)
			{
				this.m_Text = string.Empty;
			}
			this.m_DrawStart = 0;
			this.m_DrawEnd = this.m_Text.Length;
			if (this.m_CachedInputRenderer != null)
			{
				this.m_CachedInputRenderer.SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);
			}
			if (this.m_TextComponent != null)
			{
				this.m_TextComponent.RegisterDirtyVerticesCallback(new UnityAction(this.MarkGeometryAsDirty));
				this.m_TextComponent.RegisterDirtyVerticesCallback(new UnityAction(this.UpdateLabel));
				this.UpdateLabel();
			}
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnDisable.</para>
		/// </summary>
		protected override void OnDisable()
		{
			this.m_BlinkCoroutine = null;
			this.DeactivateInputField();
			if (this.m_TextComponent != null)
			{
				this.m_TextComponent.UnregisterDirtyVerticesCallback(new UnityAction(this.MarkGeometryAsDirty));
				this.m_TextComponent.UnregisterDirtyVerticesCallback(new UnityAction(this.UpdateLabel));
			}
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (this.m_CachedInputRenderer != null)
			{
				this.m_CachedInputRenderer.Clear();
			}
			if (this.m_Mesh != null)
			{
				Object.DestroyImmediate(this.m_Mesh);
			}
			this.m_Mesh = null;
			base.OnDisable();
		}

		[DebuggerHidden]
		private IEnumerator CaretBlink()
		{
			InputField.<CaretBlink>c__Iterator3 <CaretBlink>c__Iterator = new InputField.<CaretBlink>c__Iterator3();
			<CaretBlink>c__Iterator.<>f__this = this;
			return <CaretBlink>c__Iterator;
		}

		private void SetCaretVisible()
		{
			if (!this.m_AllowInput)
			{
				return;
			}
			this.m_CaretVisible = true;
			this.m_BlinkStartTime = Time.unscaledTime;
			this.SetCaretActive();
		}

		private void SetCaretActive()
		{
			if (!this.m_AllowInput)
			{
				return;
			}
			if (this.m_CaretBlinkRate > 0f)
			{
				if (this.m_BlinkCoroutine == null)
				{
					this.m_BlinkCoroutine = base.StartCoroutine(this.CaretBlink());
				}
			}
			else
			{
				this.m_CaretVisible = true;
			}
		}

		/// <summary>
		///   <para>Focus the input field initializing properties.</para>
		/// </summary>
		protected void OnFocus()
		{
			this.SelectAll();
		}

		/// <summary>
		///   <para>Highlight the whole InputField.</para>
		/// </summary>
		protected void SelectAll()
		{
			this.caretPositionInternal = this.text.Length;
			this.caretSelectPositionInternal = 0;
		}

		/// <summary>
		///   <para>Move the caret index to end of text.</para>
		/// </summary>
		/// <param name="shift">Only move the selectionPosition.</param>
		public void MoveTextEnd(bool shift)
		{
			int length = this.text.Length;
			if (shift)
			{
				this.caretSelectPositionInternal = length;
			}
			else
			{
				this.caretPositionInternal = length;
				this.caretSelectPositionInternal = this.caretPositionInternal;
			}
			this.UpdateLabel();
		}

		/// <summary>
		///   <para>Move the caret index to start of text.</para>
		/// </summary>
		/// <param name="shift">Only move the selectionPosition.</param>
		public void MoveTextStart(bool shift)
		{
			int num = 0;
			if (shift)
			{
				this.caretSelectPositionInternal = num;
			}
			else
			{
				this.caretPositionInternal = num;
				this.caretSelectPositionInternal = this.caretPositionInternal;
			}
			this.UpdateLabel();
		}

		private bool InPlaceEditing()
		{
			return !TouchScreenKeyboard.isSupported;
		}

		protected virtual void LateUpdate()
		{
			if (this.m_ShouldActivateNextUpdate)
			{
				if (!this.isFocused)
				{
					this.ActivateInputFieldInternal();
					this.m_ShouldActivateNextUpdate = false;
					return;
				}
				this.m_ShouldActivateNextUpdate = false;
			}
			if (this.InPlaceEditing() || !this.isFocused)
			{
				return;
			}
			this.AssignPositioningIfNeeded();
			if (this.m_Keyboard == null || !this.m_Keyboard.active)
			{
				if (this.m_Keyboard != null)
				{
					if (!this.m_ReadOnly)
					{
						this.text = this.m_Keyboard.text;
					}
					if (this.m_Keyboard.wasCanceled)
					{
						this.m_WasCanceled = true;
					}
				}
				this.OnDeselect(null);
				return;
			}
			string text = this.m_Keyboard.text;
			if (this.m_Text != text)
			{
				if (this.m_ReadOnly)
				{
					this.m_Keyboard.text = this.m_Text;
				}
				else
				{
					this.m_Text = string.Empty;
					for (int i = 0; i < text.Length; i++)
					{
						char c = text[i];
						if (c == '\r' || c == '\u0003')
						{
							c = '\n';
						}
						if (this.onValidateInput != null)
						{
							c = this.onValidateInput(this.m_Text, this.m_Text.Length, c);
						}
						else if (this.characterValidation != InputField.CharacterValidation.None)
						{
							c = this.Validate(this.m_Text, this.m_Text.Length, c);
						}
						if (this.lineType == InputField.LineType.MultiLineSubmit && c == '\n')
						{
							this.m_Keyboard.text = this.m_Text;
							this.OnDeselect(null);
							return;
						}
						if (c != '\0')
						{
							this.m_Text += c;
						}
					}
					if (this.characterLimit > 0 && this.m_Text.Length > this.characterLimit)
					{
						this.m_Text = this.m_Text.Substring(0, this.characterLimit);
					}
					int length = this.m_Text.Length;
					this.caretSelectPositionInternal = length;
					this.caretPositionInternal = length;
					if (this.m_Text != text)
					{
						this.m_Keyboard.text = this.m_Text;
					}
					this.SendOnValueChangedAndUpdateLabel();
				}
			}
			if (this.m_Keyboard.done)
			{
				if (this.m_Keyboard.wasCanceled)
				{
					this.m_WasCanceled = true;
				}
				this.OnDeselect(null);
			}
		}

		/// <summary>
		///   <para>Convert screen space into input field local space.</para>
		/// </summary>
		/// <param name="screen"></param>
		public Vector2 ScreenToLocal(Vector2 screen)
		{
			Canvas canvas = this.m_TextComponent.canvas;
			if (canvas == null)
			{
				return screen;
			}
			Vector3 vector = Vector3.zero;
			if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				vector = this.m_TextComponent.transform.InverseTransformPoint(screen);
			}
			else if (canvas.worldCamera != null)
			{
				Ray ray = canvas.worldCamera.ScreenPointToRay(screen);
				Plane plane = new Plane(this.m_TextComponent.transform.forward, this.m_TextComponent.transform.position);
				float distance;
				plane.Raycast(ray, out distance);
				vector = this.m_TextComponent.transform.InverseTransformPoint(ray.GetPoint(distance));
			}
			return new Vector2(vector.x, vector.y);
		}

		private int GetUnclampedCharacterLineFromPosition(Vector2 pos, TextGenerator generator)
		{
			if (!this.multiLine)
			{
				return 0;
			}
			float num = pos.y * this.m_TextComponent.pixelsPerUnit;
			float num2 = 0f;
			int i = 0;
			while (i < generator.lineCount)
			{
				float topY = generator.lines[i].topY;
				float num3 = topY - (float)generator.lines[i].height;
				if (num > topY)
				{
					float num4 = topY - num2;
					if (num > topY - 0.5f * num4)
					{
						return i - 1;
					}
					return i;
				}
				else
				{
					if (num > num3)
					{
						return i;
					}
					num2 = num3;
					i++;
				}
			}
			return generator.lineCount;
		}

		/// <summary>
		///   <para>The character that is under the mouse.</para>
		/// </summary>
		/// <param name="pos">Mouse position.</param>
		/// <returns>
		///   <para>Character index with in value.</para>
		/// </returns>
		protected int GetCharacterIndexFromPosition(Vector2 pos)
		{
			TextGenerator cachedTextGenerator = this.m_TextComponent.cachedTextGenerator;
			if (cachedTextGenerator.lineCount == 0)
			{
				return 0;
			}
			int unclampedCharacterLineFromPosition = this.GetUnclampedCharacterLineFromPosition(pos, cachedTextGenerator);
			if (unclampedCharacterLineFromPosition < 0)
			{
				return 0;
			}
			if (unclampedCharacterLineFromPosition >= cachedTextGenerator.lineCount)
			{
				return cachedTextGenerator.characterCountVisible;
			}
			int startCharIdx = cachedTextGenerator.lines[unclampedCharacterLineFromPosition].startCharIdx;
			int lineEndPosition = InputField.GetLineEndPosition(cachedTextGenerator, unclampedCharacterLineFromPosition);
			for (int i = startCharIdx; i < lineEndPosition; i++)
			{
				if (i >= cachedTextGenerator.characterCountVisible)
				{
					break;
				}
				UICharInfo uICharInfo = cachedTextGenerator.characters[i];
				Vector2 vector = uICharInfo.cursorPos / this.m_TextComponent.pixelsPerUnit;
				float num = pos.x - vector.x;
				float num2 = vector.x + uICharInfo.charWidth / this.m_TextComponent.pixelsPerUnit - pos.x;
				if (num < num2)
				{
					return i;
				}
			}
			return lineEndPosition;
		}

		private bool MayDrag(PointerEventData eventData)
		{
			return this.IsActive() && this.IsInteractable() && eventData.button == PointerEventData.InputButton.Left && this.m_TextComponent != null && this.m_Keyboard == null;
		}

		/// <summary>
		///   <para>Capture the OnBeginDrag callback from the EventSystem and ensure we should listen to the drag events to follow.</para>
		/// </summary>
		/// <param name="eventData">The data passed by the EventSystem.</param>
		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			this.m_UpdateDrag = true;
		}

		/// <summary>
		///   <para>What to do when the event system sends a Drag Event.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			Vector2 pos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.textComponent.rectTransform, eventData.position, eventData.pressEventCamera, out pos);
			this.caretSelectPositionInternal = this.GetCharacterIndexFromPosition(pos) + this.m_DrawStart;
			this.MarkGeometryAsDirty();
			this.m_DragPositionOutOfBounds = !RectTransformUtility.RectangleContainsScreenPoint(this.textComponent.rectTransform, eventData.position, eventData.pressEventCamera);
			if (this.m_DragPositionOutOfBounds && this.m_DragCoroutine == null)
			{
				this.m_DragCoroutine = base.StartCoroutine(this.MouseDragOutsideRect(eventData));
			}
			eventData.Use();
		}

		[DebuggerHidden]
		private IEnumerator MouseDragOutsideRect(PointerEventData eventData)
		{
			InputField.<MouseDragOutsideRect>c__Iterator4 <MouseDragOutsideRect>c__Iterator = new InputField.<MouseDragOutsideRect>c__Iterator4();
			<MouseDragOutsideRect>c__Iterator.eventData = eventData;
			<MouseDragOutsideRect>c__Iterator.<$>eventData = eventData;
			<MouseDragOutsideRect>c__Iterator.<>f__this = this;
			return <MouseDragOutsideRect>c__Iterator;
		}

		/// <summary>
		///   <para>Capture the OnEndDrag callback from the EventSystem and cancel the listening of drag events.</para>
		/// </summary>
		/// <param name="eventData">The eventData sent by the EventSystem.</param>
		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			this.m_UpdateDrag = false;
		}

		/// <summary>
		///   <para>What to do when the event system sends a pointer down Event.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(base.gameObject, eventData);
			bool allowInput = this.m_AllowInput;
			base.OnPointerDown(eventData);
			if (!this.InPlaceEditing() && (this.m_Keyboard == null || !this.m_Keyboard.active))
			{
				this.OnSelect(eventData);
				return;
			}
			if (allowInput)
			{
				Vector2 pos = this.ScreenToLocal(eventData.position);
				int num = this.GetCharacterIndexFromPosition(pos) + this.m_DrawStart;
				this.caretPositionInternal = num;
				this.caretSelectPositionInternal = num;
			}
			this.UpdateLabel();
			eventData.Use();
		}

		/// <summary>
		///   <para>Process the Event and perform the appropriate action for that key.</para>
		/// </summary>
		/// <param name="evt">The Event that is currently being processed.</param>
		/// <returns>
		///   <para>If we should continue processing events or we have hit an end condition.</para>
		/// </returns>
		protected InputField.EditState KeyPressed(Event evt)
		{
			EventModifiers modifiers = evt.modifiers;
			RuntimePlatform platform = Application.platform;
			bool flag = platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.OSXWebPlayer;
			bool flag2 = (!flag) ? ((modifiers & EventModifiers.Control) != EventModifiers.None) : ((modifiers & EventModifiers.Command) != EventModifiers.None);
			bool flag3 = (modifiers & EventModifiers.Shift) != EventModifiers.None;
			bool flag4 = (modifiers & EventModifiers.Alt) != EventModifiers.None;
			bool flag5 = flag2 && !flag4 && !flag3;
			KeyCode keyCode = evt.keyCode;
			switch (keyCode)
			{
			case KeyCode.KeypadEnter:
				goto IL_1E9;
			case KeyCode.KeypadEquals:
			case KeyCode.Insert:
				IL_A6:
				switch (keyCode)
				{
				case KeyCode.A:
					if (flag5)
					{
						this.SelectAll();
						return InputField.EditState.Continue;
					}
					goto IL_205;
				case KeyCode.B:
					IL_BC:
					switch (keyCode)
					{
					case KeyCode.V:
						if (flag5)
						{
							this.Append(InputField.clipboard);
							return InputField.EditState.Continue;
						}
						goto IL_205;
					case KeyCode.W:
						IL_D2:
						if (keyCode == KeyCode.Backspace)
						{
							this.Backspace();
							return InputField.EditState.Continue;
						}
						if (keyCode == KeyCode.Return)
						{
							goto IL_1E9;
						}
						if (keyCode == KeyCode.Escape)
						{
							this.m_WasCanceled = true;
							return InputField.EditState.Finish;
						}
						if (keyCode != KeyCode.Delete)
						{
							goto IL_205;
						}
						this.ForwardSpace();
						return InputField.EditState.Continue;
					case KeyCode.X:
						if (flag5)
						{
							if (this.inputType != InputField.InputType.Password)
							{
								InputField.clipboard = this.GetSelectedString();
							}
							else
							{
								InputField.clipboard = string.Empty;
							}
							this.Delete();
							this.SendOnValueChangedAndUpdateLabel();
							return InputField.EditState.Continue;
						}
						goto IL_205;
					}
					goto IL_D2;
				case KeyCode.C:
					if (flag5)
					{
						if (this.inputType != InputField.InputType.Password)
						{
							InputField.clipboard = this.GetSelectedString();
						}
						else
						{
							InputField.clipboard = string.Empty;
						}
						return InputField.EditState.Continue;
					}
					goto IL_205;
				}
				goto IL_BC;
			case KeyCode.UpArrow:
				this.MoveUp(flag3);
				return InputField.EditState.Continue;
			case KeyCode.DownArrow:
				this.MoveDown(flag3);
				return InputField.EditState.Continue;
			case KeyCode.RightArrow:
				this.MoveRight(flag3, flag2);
				return InputField.EditState.Continue;
			case KeyCode.LeftArrow:
				this.MoveLeft(flag3, flag2);
				return InputField.EditState.Continue;
			case KeyCode.Home:
				this.MoveTextStart(flag3);
				return InputField.EditState.Continue;
			case KeyCode.End:
				this.MoveTextEnd(flag3);
				return InputField.EditState.Continue;
			}
			goto IL_A6;
			IL_1E9:
			if (this.lineType != InputField.LineType.MultiLineNewline)
			{
				return InputField.EditState.Finish;
			}
			IL_205:
			char c = evt.character;
			if (!this.multiLine && (c == '\t' || c == '\r' || c == '\n'))
			{
				return InputField.EditState.Continue;
			}
			if (c == '\r' || c == '\u0003')
			{
				c = '\n';
			}
			if (this.IsValidChar(c))
			{
				this.Append(c);
			}
			if (c == '\0' && Input.compositionString.Length > 0)
			{
				this.UpdateLabel();
			}
			return InputField.EditState.Continue;
		}

		private bool IsValidChar(char c)
		{
			return c != '\u007f' && (c == '\t' || c == '\n' || this.m_TextComponent.font.HasCharacter(c));
		}

		/// <summary>
		///   <para>Helper function to allow separate events to be processed by the InputField.</para>
		/// </summary>
		/// <param name="e">The Event to be processed.</param>
		public void ProcessEvent(Event e)
		{
			this.KeyPressed(e);
		}

		/// <summary>
		///   <para>What to do when the event system sends a Update selected Event.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public virtual void OnUpdateSelected(BaseEventData eventData)
		{
			if (!this.isFocused)
			{
				return;
			}
			bool flag = false;
			while (Event.PopEvent(this.m_ProcessingEvent))
			{
				if (this.m_ProcessingEvent.rawType == EventType.KeyDown)
				{
					flag = true;
					InputField.EditState editState = this.KeyPressed(this.m_ProcessingEvent);
					if (editState == InputField.EditState.Finish)
					{
						this.DeactivateInputField();
						break;
					}
				}
				EventType type = this.m_ProcessingEvent.type;
				if (type == EventType.ValidateCommand || type == EventType.ExecuteCommand)
				{
					string commandName = this.m_ProcessingEvent.commandName;
					if (commandName != null)
					{
						if (InputField.<>f__switch$map0 == null)
						{
							InputField.<>f__switch$map0 = new Dictionary<string, int>(1)
							{
								{
									"SelectAll",
									0
								}
							};
						}
						int num;
						if (InputField.<>f__switch$map0.TryGetValue(commandName, out num))
						{
							if (num == 0)
							{
								this.SelectAll();
								flag = true;
							}
						}
					}
				}
			}
			if (flag)
			{
				this.UpdateLabel();
			}
			eventData.Use();
		}

		private string GetSelectedString()
		{
			if (!this.hasSelection)
			{
				return string.Empty;
			}
			int num = this.caretPositionInternal;
			int num2 = this.caretSelectPositionInternal;
			if (num > num2)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			return this.text.Substring(num, num2 - num);
		}

		private int FindtNextWordBegin()
		{
			if (this.caretSelectPositionInternal + 1 >= this.text.Length)
			{
				return this.text.Length;
			}
			int num = this.text.IndexOfAny(InputField.kSeparators, this.caretSelectPositionInternal + 1);
			if (num == -1)
			{
				num = this.text.Length;
			}
			else
			{
				num++;
			}
			return num;
		}

		private void MoveRight(bool shift, bool ctrl)
		{
			if (this.hasSelection && !shift)
			{
				int num = Mathf.Max(this.caretPositionInternal, this.caretSelectPositionInternal);
				this.caretSelectPositionInternal = num;
				this.caretPositionInternal = num;
				return;
			}
			int num2;
			if (ctrl)
			{
				num2 = this.FindtNextWordBegin();
			}
			else
			{
				num2 = this.caretSelectPositionInternal + 1;
			}
			if (shift)
			{
				this.caretSelectPositionInternal = num2;
			}
			else
			{
				int num = num2;
				this.caretPositionInternal = num;
				this.caretSelectPositionInternal = num;
			}
		}

		private int FindtPrevWordBegin()
		{
			if (this.caretSelectPositionInternal - 2 < 0)
			{
				return 0;
			}
			int num = this.text.LastIndexOfAny(InputField.kSeparators, this.caretSelectPositionInternal - 2);
			if (num == -1)
			{
				num = 0;
			}
			else
			{
				num++;
			}
			return num;
		}

		private void MoveLeft(bool shift, bool ctrl)
		{
			if (this.hasSelection && !shift)
			{
				int num = Mathf.Min(this.caretPositionInternal, this.caretSelectPositionInternal);
				this.caretSelectPositionInternal = num;
				this.caretPositionInternal = num;
				return;
			}
			int num2;
			if (ctrl)
			{
				num2 = this.FindtPrevWordBegin();
			}
			else
			{
				num2 = this.caretSelectPositionInternal - 1;
			}
			if (shift)
			{
				this.caretSelectPositionInternal = num2;
			}
			else
			{
				int num = num2;
				this.caretPositionInternal = num;
				this.caretSelectPositionInternal = num;
			}
		}

		private int DetermineCharacterLine(int charPos, TextGenerator generator)
		{
			for (int i = 0; i < generator.lineCount - 1; i++)
			{
				if (generator.lines[i + 1].startCharIdx > charPos)
				{
					return i;
				}
			}
			return generator.lineCount - 1;
		}

		private int LineUpCharacterPosition(int originalPos, bool goToFirstChar)
		{
			if (originalPos >= this.cachedInputTextGenerator.characterCountVisible)
			{
				return 0;
			}
			UICharInfo uICharInfo = this.cachedInputTextGenerator.characters[originalPos];
			int num = this.DetermineCharacterLine(originalPos, this.cachedInputTextGenerator);
			if (num - 1 < 0)
			{
				return (!goToFirstChar) ? originalPos : 0;
			}
			int num2 = this.cachedInputTextGenerator.lines[num].startCharIdx - 1;
			for (int i = this.cachedInputTextGenerator.lines[num - 1].startCharIdx; i < num2; i++)
			{
				if (this.cachedInputTextGenerator.characters[i].cursorPos.x >= uICharInfo.cursorPos.x)
				{
					return i;
				}
			}
			return num2;
		}

		private int LineDownCharacterPosition(int originalPos, bool goToLastChar)
		{
			if (originalPos >= this.cachedInputTextGenerator.characterCountVisible)
			{
				return this.text.Length;
			}
			UICharInfo uICharInfo = this.cachedInputTextGenerator.characters[originalPos];
			int num = this.DetermineCharacterLine(originalPos, this.cachedInputTextGenerator);
			if (num + 1 >= this.cachedInputTextGenerator.lineCount)
			{
				return (!goToLastChar) ? originalPos : this.text.Length;
			}
			int lineEndPosition = InputField.GetLineEndPosition(this.cachedInputTextGenerator, num + 1);
			for (int i = this.cachedInputTextGenerator.lines[num + 1].startCharIdx; i < lineEndPosition; i++)
			{
				if (this.cachedInputTextGenerator.characters[i].cursorPos.x >= uICharInfo.cursorPos.x)
				{
					return i;
				}
			}
			return lineEndPosition;
		}

		private void MoveDown(bool shift)
		{
			this.MoveDown(shift, true);
		}

		private void MoveDown(bool shift, bool goToLastChar)
		{
			if (this.hasSelection && !shift)
			{
				int num = Mathf.Max(this.caretPositionInternal, this.caretSelectPositionInternal);
				this.caretSelectPositionInternal = num;
				this.caretPositionInternal = num;
			}
			int num2 = (!this.multiLine) ? this.text.Length : this.LineDownCharacterPosition(this.caretSelectPositionInternal, goToLastChar);
			if (shift)
			{
				this.caretSelectPositionInternal = num2;
			}
			else
			{
				int num = num2;
				this.caretSelectPositionInternal = num;
				this.caretPositionInternal = num;
			}
		}

		private void MoveUp(bool shift)
		{
			this.MoveUp(shift, true);
		}

		private void MoveUp(bool shift, bool goToFirstChar)
		{
			if (this.hasSelection && !shift)
			{
				int num = Mathf.Min(this.caretPositionInternal, this.caretSelectPositionInternal);
				this.caretSelectPositionInternal = num;
				this.caretPositionInternal = num;
			}
			int num2 = (!this.multiLine) ? 0 : this.LineUpCharacterPosition(this.caretSelectPositionInternal, goToFirstChar);
			if (shift)
			{
				this.caretSelectPositionInternal = num2;
			}
			else
			{
				int num = num2;
				this.caretPositionInternal = num;
				this.caretSelectPositionInternal = num;
			}
		}

		private void Delete()
		{
			if (this.m_ReadOnly)
			{
				return;
			}
			if (this.caretPositionInternal == this.caretSelectPositionInternal)
			{
				return;
			}
			if (this.caretPositionInternal < this.caretSelectPositionInternal)
			{
				this.m_Text = this.text.Substring(0, this.caretPositionInternal) + this.text.Substring(this.caretSelectPositionInternal, this.text.Length - this.caretSelectPositionInternal);
				this.caretSelectPositionInternal = this.caretPositionInternal;
			}
			else
			{
				this.m_Text = this.text.Substring(0, this.caretSelectPositionInternal) + this.text.Substring(this.caretPositionInternal, this.text.Length - this.caretPositionInternal);
				this.caretPositionInternal = this.caretSelectPositionInternal;
			}
		}

		private void ForwardSpace()
		{
			if (this.m_ReadOnly)
			{
				return;
			}
			if (this.hasSelection)
			{
				this.Delete();
				this.SendOnValueChangedAndUpdateLabel();
			}
			else if (this.caretPositionInternal < this.text.Length)
			{
				this.m_Text = this.text.Remove(this.caretPositionInternal, 1);
				this.SendOnValueChangedAndUpdateLabel();
			}
		}

		private void Backspace()
		{
			if (this.m_ReadOnly)
			{
				return;
			}
			if (this.hasSelection)
			{
				this.Delete();
				this.SendOnValueChangedAndUpdateLabel();
			}
			else if (this.caretPositionInternal > 0)
			{
				this.m_Text = this.text.Remove(this.caretPositionInternal - 1, 1);
				int num = this.caretPositionInternal - 1;
				this.caretPositionInternal = num;
				this.caretSelectPositionInternal = num;
				this.SendOnValueChangedAndUpdateLabel();
			}
		}

		private void Insert(char c)
		{
			if (this.m_ReadOnly)
			{
				return;
			}
			string text = c.ToString();
			this.Delete();
			if (this.characterLimit > 0 && this.text.Length >= this.characterLimit)
			{
				return;
			}
			this.m_Text = this.text.Insert(this.m_CaretPosition, text);
			this.caretSelectPositionInternal = (this.caretPositionInternal += text.Length);
			this.SendOnValueChanged();
		}

		private void SendOnValueChangedAndUpdateLabel()
		{
			this.SendOnValueChanged();
			this.UpdateLabel();
		}

		private void SendOnValueChanged()
		{
			if (this.onValueChanged != null)
			{
				this.onValueChanged.Invoke(this.text);
			}
		}

		/// <summary>
		///   <para>Convenience function to make functionality to send the SubmitEvent easier.</para>
		/// </summary>
		protected void SendOnSubmit()
		{
			if (this.onEndEdit != null)
			{
				this.onEndEdit.Invoke(this.m_Text);
			}
		}

		/// <summary>
		///   <para>Append a character to the input field.</para>
		/// </summary>
		/// <param name="input">Character / string to append.</param>
		protected virtual void Append(string input)
		{
			if (this.m_ReadOnly)
			{
				return;
			}
			if (!this.InPlaceEditing())
			{
				return;
			}
			int i = 0;
			int length = input.Length;
			while (i < length)
			{
				char c = input[i];
				if (c >= ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\n')
				{
					this.Append(c);
				}
				i++;
			}
		}

		/// <summary>
		///   <para>Append a character to the input field.</para>
		/// </summary>
		/// <param name="input">Character / string to append.</param>
		protected virtual void Append(char input)
		{
			if (this.m_ReadOnly)
			{
				return;
			}
			if (!this.InPlaceEditing())
			{
				return;
			}
			if (this.onValidateInput != null)
			{
				input = this.onValidateInput(this.text, this.caretPositionInternal, input);
			}
			else if (this.characterValidation != InputField.CharacterValidation.None)
			{
				input = this.Validate(this.text, this.caretPositionInternal, input);
			}
			if (input == '\0')
			{
				return;
			}
			this.Insert(input);
		}

		/// <summary>
		///   <para>Update the Text associated with this input field.</para>
		/// </summary>
		protected void UpdateLabel()
		{
			if (this.m_TextComponent != null && this.m_TextComponent.font != null && !this.m_PreventFontCallback)
			{
				this.m_PreventFontCallback = true;
				string text;
				if (Input.compositionString.Length > 0)
				{
					text = this.text.Substring(0, this.m_CaretPosition) + Input.compositionString + this.text.Substring(this.m_CaretPosition);
				}
				else
				{
					text = this.text;
				}
				string text2;
				if (this.inputType == InputField.InputType.Password)
				{
					text2 = new string(this.asteriskChar, text.Length);
				}
				else
				{
					text2 = text;
				}
				bool flag = string.IsNullOrEmpty(text);
				if (this.m_Placeholder != null)
				{
					this.m_Placeholder.enabled = (flag && !this.isFocused);
				}
				if (!this.m_AllowInput)
				{
					this.m_DrawStart = 0;
					this.m_DrawEnd = this.m_Text.Length;
				}
				if (!flag)
				{
					Vector2 size = this.m_TextComponent.rectTransform.rect.size;
					TextGenerationSettings generationSettings = this.m_TextComponent.GetGenerationSettings(size);
					generationSettings.generateOutOfBounds = true;
					this.cachedInputTextGenerator.Populate(text2, generationSettings);
					this.SetDrawRangeToContainCaretPosition(this.caretSelectPositionInternal);
					text2 = text2.Substring(this.m_DrawStart, Mathf.Min(this.m_DrawEnd, text2.Length) - this.m_DrawStart);
					this.SetCaretVisible();
				}
				this.m_TextComponent.text = text2;
				this.MarkGeometryAsDirty();
				this.m_PreventFontCallback = false;
			}
		}

		private bool IsSelectionVisible()
		{
			return this.m_DrawStart <= this.caretPositionInternal && this.m_DrawStart <= this.caretSelectPositionInternal && this.m_DrawEnd >= this.caretPositionInternal && this.m_DrawEnd >= this.caretSelectPositionInternal;
		}

		private static int GetLineStartPosition(TextGenerator gen, int line)
		{
			line = Mathf.Clamp(line, 0, gen.lines.Count - 1);
			return gen.lines[line].startCharIdx;
		}

		private static int GetLineEndPosition(TextGenerator gen, int line)
		{
			line = Mathf.Max(line, 0);
			if (line + 1 < gen.lines.Count)
			{
				return gen.lines[line + 1].startCharIdx;
			}
			return gen.characterCountVisible;
		}

		private void SetDrawRangeToContainCaretPosition(int caretPos)
		{
			if (this.cachedInputTextGenerator.lineCount <= 0)
			{
				return;
			}
			Vector2 size = this.cachedInputTextGenerator.rectExtents.size;
			if (this.multiLine)
			{
				IList<UILineInfo> lines = this.cachedInputTextGenerator.lines;
				int num = this.DetermineCharacterLine(caretPos, this.cachedInputTextGenerator);
				int num2 = (int)size.y;
				if (this.m_DrawEnd <= caretPos)
				{
					this.m_DrawEnd = InputField.GetLineEndPosition(this.cachedInputTextGenerator, num);
					int num3 = num;
					while (num3 >= 0 && num3 < lines.Count)
					{
						num2 -= lines[num3].height;
						if (num2 < 0)
						{
							break;
						}
						this.m_DrawStart = InputField.GetLineStartPosition(this.cachedInputTextGenerator, num3);
						num3--;
					}
				}
				else
				{
					if (this.m_DrawStart > caretPos)
					{
						this.m_DrawStart = InputField.GetLineStartPosition(this.cachedInputTextGenerator, num);
					}
					int num4 = this.DetermineCharacterLine(this.m_DrawStart, this.cachedInputTextGenerator);
					int num5 = num4;
					this.m_DrawEnd = InputField.GetLineEndPosition(this.cachedInputTextGenerator, num5);
					num2 -= lines[num5].height;
					while (true)
					{
						if (num5 < lines.Count - 1)
						{
							num5++;
							if (num2 < lines[num5].height)
							{
								break;
							}
							this.m_DrawEnd = InputField.GetLineEndPosition(this.cachedInputTextGenerator, num5);
							num2 -= lines[num5].height;
						}
						else
						{
							if (num4 <= 0)
							{
								break;
							}
							num4--;
							if (num2 < lines[num4].height)
							{
								break;
							}
							this.m_DrawStart = InputField.GetLineStartPosition(this.cachedInputTextGenerator, num4);
							num2 -= lines[num4].height;
						}
					}
				}
			}
			else
			{
				IList<UICharInfo> characters = this.cachedInputTextGenerator.characters;
				if (this.m_DrawEnd > this.cachedInputTextGenerator.characterCountVisible)
				{
					this.m_DrawEnd = this.cachedInputTextGenerator.characterCountVisible;
				}
				float num6 = 0f;
				if (caretPos > this.m_DrawEnd || (caretPos == this.m_DrawEnd && this.m_DrawStart > 0))
				{
					this.m_DrawEnd = caretPos;
					this.m_DrawStart = this.m_DrawEnd - 1;
					while (this.m_DrawStart >= 0)
					{
						if (num6 + characters[this.m_DrawStart].charWidth > size.x)
						{
							break;
						}
						num6 += characters[this.m_DrawStart].charWidth;
						this.m_DrawStart--;
					}
					this.m_DrawStart++;
				}
				else
				{
					if (caretPos < this.m_DrawStart)
					{
						this.m_DrawStart = caretPos;
					}
					this.m_DrawEnd = this.m_DrawStart;
				}
				while (this.m_DrawEnd < this.cachedInputTextGenerator.characterCountVisible)
				{
					num6 += characters[this.m_DrawEnd].charWidth;
					if (num6 > size.x)
					{
						break;
					}
					this.m_DrawEnd++;
				}
			}
		}

		/// <summary>
		///   <para>Force the label to update immediatly. This will recalculate the positioning of the caret and the visible text.</para>
		/// </summary>
		public void ForceLabelUpdate()
		{
			this.UpdateLabel();
		}

		private void MarkGeometryAsDirty()
		{
			if (!Application.isPlaying || PrefabUtility.GetPrefabObject(base.gameObject) != null)
			{
				return;
			}
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
		}

		/// <summary>
		///   <para>Rebuild the input fields geometry. (caret and highlight).</para>
		/// </summary>
		/// <param name="update"></param>
		public virtual void Rebuild(CanvasUpdate update)
		{
			if (update == CanvasUpdate.LatePreRender)
			{
				this.UpdateGeometry();
			}
		}

		/// <summary>
		///   <para>See ICanvasElement.LayoutComplete.</para>
		/// </summary>
		public virtual void LayoutComplete()
		{
		}

		/// <summary>
		///   <para>See ICanvasElement.GraphicUpdateComplete.</para>
		/// </summary>
		public virtual void GraphicUpdateComplete()
		{
		}

		private void UpdateGeometry()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (!this.shouldHideMobileInput)
			{
				return;
			}
			if (this.m_CachedInputRenderer == null && this.m_TextComponent != null)
			{
				GameObject gameObject = new GameObject(base.transform.name + " Input Caret");
				gameObject.hideFlags = HideFlags.DontSave;
				gameObject.transform.SetParent(this.m_TextComponent.transform.parent);
				gameObject.transform.SetAsFirstSibling();
				gameObject.layer = base.gameObject.layer;
				this.caretRectTrans = gameObject.AddComponent<RectTransform>();
				this.m_CachedInputRenderer = gameObject.AddComponent<CanvasRenderer>();
				this.m_CachedInputRenderer.SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);
				gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
				this.AssignPositioningIfNeeded();
			}
			if (this.m_CachedInputRenderer == null)
			{
				return;
			}
			this.OnFillVBO(this.mesh);
			this.m_CachedInputRenderer.SetMesh(this.mesh);
		}

		private void AssignPositioningIfNeeded()
		{
			if (this.m_TextComponent != null && this.caretRectTrans != null && (this.caretRectTrans.localPosition != this.m_TextComponent.rectTransform.localPosition || this.caretRectTrans.localRotation != this.m_TextComponent.rectTransform.localRotation || this.caretRectTrans.localScale != this.m_TextComponent.rectTransform.localScale || this.caretRectTrans.anchorMin != this.m_TextComponent.rectTransform.anchorMin || this.caretRectTrans.anchorMax != this.m_TextComponent.rectTransform.anchorMax || this.caretRectTrans.anchoredPosition != this.m_TextComponent.rectTransform.anchoredPosition || this.caretRectTrans.sizeDelta != this.m_TextComponent.rectTransform.sizeDelta || this.caretRectTrans.pivot != this.m_TextComponent.rectTransform.pivot))
			{
				this.caretRectTrans.localPosition = this.m_TextComponent.rectTransform.localPosition;
				this.caretRectTrans.localRotation = this.m_TextComponent.rectTransform.localRotation;
				this.caretRectTrans.localScale = this.m_TextComponent.rectTransform.localScale;
				this.caretRectTrans.anchorMin = this.m_TextComponent.rectTransform.anchorMin;
				this.caretRectTrans.anchorMax = this.m_TextComponent.rectTransform.anchorMax;
				this.caretRectTrans.anchoredPosition = this.m_TextComponent.rectTransform.anchoredPosition;
				this.caretRectTrans.sizeDelta = this.m_TextComponent.rectTransform.sizeDelta;
				this.caretRectTrans.pivot = this.m_TextComponent.rectTransform.pivot;
			}
		}

		private void OnFillVBO(Mesh vbo)
		{
			using (VertexHelper vertexHelper = new VertexHelper())
			{
				if (!this.isFocused)
				{
					vertexHelper.FillMesh(vbo);
				}
				else
				{
					Rect rect = this.m_TextComponent.rectTransform.rect;
					Vector2 size = rect.size;
					Vector2 textAnchorPivot = Text.GetTextAnchorPivot(this.m_TextComponent.alignment);
					Vector2 zero = Vector2.zero;
					zero.x = Mathf.Lerp(rect.xMin, rect.xMax, textAnchorPivot.x);
					zero.y = Mathf.Lerp(rect.yMin, rect.yMax, textAnchorPivot.y);
					Vector2 a = this.m_TextComponent.PixelAdjustPoint(zero);
					Vector2 roundingOffset = a - zero + Vector2.Scale(size, textAnchorPivot);
					roundingOffset.x -= Mathf.Floor(0.5f + roundingOffset.x);
					roundingOffset.y -= Mathf.Floor(0.5f + roundingOffset.y);
					if (!this.hasSelection)
					{
						this.GenerateCaret(vertexHelper, roundingOffset);
					}
					else
					{
						this.GenerateHightlight(vertexHelper, roundingOffset);
					}
					vertexHelper.FillMesh(vbo);
				}
			}
		}

		private void GenerateCaret(VertexHelper vbo, Vector2 roundingOffset)
		{
			if (!this.m_CaretVisible)
			{
				return;
			}
			if (this.m_CursorVerts == null)
			{
				this.CreateCursorVerts();
			}
			float num = (float)this.m_CaretWidth;
			int num2 = Mathf.Max(0, this.caretPositionInternal - this.m_DrawStart);
			TextGenerator cachedTextGenerator = this.m_TextComponent.cachedTextGenerator;
			if (cachedTextGenerator == null)
			{
				return;
			}
			if (cachedTextGenerator.lineCount == 0)
			{
				return;
			}
			Vector2 zero = Vector2.zero;
			if (cachedTextGenerator.characterCountVisible + 1 > num2 || num2 == 0)
			{
				zero.x = cachedTextGenerator.characters[num2].cursorPos.x;
			}
			zero.x /= this.m_TextComponent.pixelsPerUnit;
			if (zero.x > this.m_TextComponent.rectTransform.rect.xMax)
			{
				zero.x = this.m_TextComponent.rectTransform.rect.xMax;
			}
			int index = this.DetermineCharacterLine(num2, cachedTextGenerator);
			zero.y = cachedTextGenerator.lines[index].topY / this.m_TextComponent.pixelsPerUnit;
			float num3 = (float)cachedTextGenerator.lines[index].height / this.m_TextComponent.pixelsPerUnit;
			for (int i = 0; i < this.m_CursorVerts.Length; i++)
			{
				this.m_CursorVerts[i].color = this.caretColor;
			}
			this.m_CursorVerts[0].position = new Vector3(zero.x, zero.y - num3, 0f);
			this.m_CursorVerts[1].position = new Vector3(zero.x + num, zero.y - num3, 0f);
			this.m_CursorVerts[2].position = new Vector3(zero.x + num, zero.y, 0f);
			this.m_CursorVerts[3].position = new Vector3(zero.x, zero.y, 0f);
			if (roundingOffset != Vector2.zero)
			{
				for (int j = 0; j < this.m_CursorVerts.Length; j++)
				{
					UIVertex uIVertex = this.m_CursorVerts[j];
					uIVertex.position.x = uIVertex.position.x + roundingOffset.x;
					uIVertex.position.y = uIVertex.position.y + roundingOffset.y;
				}
			}
			vbo.AddUIVertexQuad(this.m_CursorVerts);
			int height = Screen.height;
			zero.y = (float)height - zero.y;
			Input.compositionCursorPos = zero;
		}

		private void CreateCursorVerts()
		{
			this.m_CursorVerts = new UIVertex[4];
			for (int i = 0; i < this.m_CursorVerts.Length; i++)
			{
				this.m_CursorVerts[i] = UIVertex.simpleVert;
				this.m_CursorVerts[i].uv0 = Vector2.zero;
			}
		}

		private void GenerateHightlight(VertexHelper vbo, Vector2 roundingOffset)
		{
			int num = Mathf.Max(0, this.caretPositionInternal - this.m_DrawStart);
			int num2 = Mathf.Max(0, this.caretSelectPositionInternal - this.m_DrawStart);
			if (num > num2)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			num2--;
			TextGenerator cachedTextGenerator = this.m_TextComponent.cachedTextGenerator;
			if (cachedTextGenerator.lineCount <= 0)
			{
				return;
			}
			int num4 = this.DetermineCharacterLine(num, cachedTextGenerator);
			int lineEndPosition = InputField.GetLineEndPosition(cachedTextGenerator, num4);
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.uv0 = Vector2.zero;
			simpleVert.color = this.selectionColor;
			int num5 = num;
			while (num5 <= num2 && num5 < cachedTextGenerator.characterCountVisible)
			{
				if (num5 + 1 == lineEndPosition || num5 == num2)
				{
					UICharInfo uICharInfo = cachedTextGenerator.characters[num];
					UICharInfo uICharInfo2 = cachedTextGenerator.characters[num5];
					Vector2 vector = new Vector2(uICharInfo.cursorPos.x / this.m_TextComponent.pixelsPerUnit, cachedTextGenerator.lines[num4].topY / this.m_TextComponent.pixelsPerUnit);
					Vector2 vector2 = new Vector2((uICharInfo2.cursorPos.x + uICharInfo2.charWidth) / this.m_TextComponent.pixelsPerUnit, vector.y - (float)cachedTextGenerator.lines[num4].height / this.m_TextComponent.pixelsPerUnit);
					if (vector2.x > this.m_TextComponent.rectTransform.rect.xMax || vector2.x < this.m_TextComponent.rectTransform.rect.xMin)
					{
						vector2.x = this.m_TextComponent.rectTransform.rect.xMax;
					}
					int currentVertCount = vbo.currentVertCount;
					simpleVert.position = new Vector3(vector.x, vector2.y, 0f) + roundingOffset;
					vbo.AddVert(simpleVert);
					simpleVert.position = new Vector3(vector2.x, vector2.y, 0f) + roundingOffset;
					vbo.AddVert(simpleVert);
					simpleVert.position = new Vector3(vector2.x, vector.y, 0f) + roundingOffset;
					vbo.AddVert(simpleVert);
					simpleVert.position = new Vector3(vector.x, vector.y, 0f) + roundingOffset;
					vbo.AddVert(simpleVert);
					vbo.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
					vbo.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
					num = num5 + 1;
					num4++;
					lineEndPosition = InputField.GetLineEndPosition(cachedTextGenerator, num4);
				}
				num5++;
			}
		}

		/// <summary>
		///   <para>Predefined validation functionality for different characterValidation types.</para>
		/// </summary>
		/// <param name="text">The whole text string to validate.</param>
		/// <param name="pos">The position at which the current character is being inserted.</param>
		/// <param name="ch">The character that is being inserted.</param>
		/// <returns>
		///   <para>The character that should be inserted.</para>
		/// </returns>
		protected char Validate(string text, int pos, char ch)
		{
			if (this.characterValidation == InputField.CharacterValidation.None || !base.enabled)
			{
				return ch;
			}
			if (this.characterValidation == InputField.CharacterValidation.Integer || this.characterValidation == InputField.CharacterValidation.Decimal)
			{
				bool flag = pos == 0 && text.Length > 0 && text[0] == '-';
				bool flag2 = this.caretPositionInternal == 0 || this.caretSelectPositionInternal == 0;
				if (!flag)
				{
					if (ch >= '0' && ch <= '9')
					{
						return ch;
					}
					if (ch == '-' && (pos == 0 || flag2))
					{
						return ch;
					}
					if (ch == '.' && this.characterValidation == InputField.CharacterValidation.Decimal && !text.Contains("."))
					{
						return ch;
					}
				}
			}
			else if (this.characterValidation == InputField.CharacterValidation.Alphanumeric)
			{
				if (ch >= 'A' && ch <= 'Z')
				{
					return ch;
				}
				if (ch >= 'a' && ch <= 'z')
				{
					return ch;
				}
				if (ch >= '0' && ch <= '9')
				{
					return ch;
				}
			}
			else if (this.characterValidation == InputField.CharacterValidation.Name)
			{
				char c = (text.Length <= 0) ? ' ' : text[Mathf.Clamp(pos, 0, text.Length - 1)];
				char c2 = (text.Length <= 0) ? '\n' : text[Mathf.Clamp(pos + 1, 0, text.Length - 1)];
				if (char.IsLetter(ch))
				{
					if (char.IsLower(ch) && c == ' ')
					{
						return char.ToUpper(ch);
					}
					if (char.IsUpper(ch) && c != ' ' && c != '\'')
					{
						return char.ToLower(ch);
					}
					return ch;
				}
				else if (ch == '\'')
				{
					if (c != ' ' && c != '\'' && c2 != '\'' && !text.Contains("'"))
					{
						return ch;
					}
				}
				else if (ch == ' ' && c != ' ' && c != '\'' && c2 != ' ' && c2 != '\'')
				{
					return ch;
				}
			}
			else if (this.characterValidation == InputField.CharacterValidation.EmailAddress)
			{
				if (ch >= 'A' && ch <= 'Z')
				{
					return ch;
				}
				if (ch >= 'a' && ch <= 'z')
				{
					return ch;
				}
				if (ch >= '0' && ch <= '9')
				{
					return ch;
				}
				if (ch == '@' && text.IndexOf('@') == -1)
				{
					return ch;
				}
				if ("!#$%&'*+-/=?^_`{|}~".IndexOf(ch) != -1)
				{
					return ch;
				}
				if (ch == '.')
				{
					char c3 = (text.Length <= 0) ? ' ' : text[Mathf.Clamp(pos, 0, text.Length - 1)];
					char c4 = (text.Length <= 0) ? '\n' : text[Mathf.Clamp(pos + 1, 0, text.Length - 1)];
					if (c3 != '.' && c4 != '.')
					{
						return ch;
					}
				}
			}
			return '\0';
		}

		/// <summary>
		///   <para>Function to activate the InputField to begin processing Events.</para>
		/// </summary>
		public void ActivateInputField()
		{
			if (this.m_TextComponent == null || this.m_TextComponent.font == null || !this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			if (this.isFocused && this.m_Keyboard != null && !this.m_Keyboard.active)
			{
				this.m_Keyboard.active = true;
				this.m_Keyboard.text = this.m_Text;
			}
			this.m_ShouldActivateNextUpdate = true;
		}

		private void ActivateInputFieldInternal()
		{
			if (EventSystem.current.currentSelectedGameObject != base.gameObject)
			{
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}
			if (TouchScreenKeyboard.isSupported)
			{
				if (Input.touchSupported)
				{
					TouchScreenKeyboard.hideInput = this.shouldHideMobileInput;
				}
				this.m_Keyboard = ((this.inputType != InputField.InputType.Password) ? TouchScreenKeyboard.Open(this.m_Text, this.keyboardType, this.inputType == InputField.InputType.AutoCorrect, this.multiLine) : TouchScreenKeyboard.Open(this.m_Text, this.keyboardType, false, this.multiLine, true));
				this.MoveTextEnd(false);
			}
			else
			{
				Input.imeCompositionMode = IMECompositionMode.On;
				this.OnFocus();
			}
			this.m_AllowInput = true;
			this.m_OriginalText = this.text;
			this.m_WasCanceled = false;
			this.SetCaretVisible();
			this.UpdateLabel();
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			if (this.shouldActivateOnSelect)
			{
				this.ActivateInputField();
			}
		}

		/// <summary>
		///   <para>What to do when the event system sends a pointer click Event.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.ActivateInputField();
		}

		/// <summary>
		///   <para>Function to deactivate the InputField to stop the processing of Events and send OnSubmit if not canceled.</para>
		/// </summary>
		public void DeactivateInputField()
		{
			if (!this.m_AllowInput)
			{
				return;
			}
			this.m_HasDoneFocusTransition = false;
			this.m_AllowInput = false;
			if (this.m_Placeholder != null)
			{
				this.m_Placeholder.enabled = string.IsNullOrEmpty(this.m_Text);
			}
			if (this.m_TextComponent != null && this.IsInteractable())
			{
				if (this.m_WasCanceled)
				{
					this.text = this.m_OriginalText;
				}
				if (this.m_Keyboard != null)
				{
					this.m_Keyboard.active = false;
					this.m_Keyboard = null;
				}
				this.m_CaretPosition = (this.m_CaretSelectPosition = 0);
				this.SendOnSubmit();
				Input.imeCompositionMode = IMECompositionMode.Auto;
			}
			this.MarkGeometryAsDirty();
		}

		/// <summary>
		///   <para>What to do when the event system sends a Deselect Event.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public override void OnDeselect(BaseEventData eventData)
		{
			this.DeactivateInputField();
			base.OnDeselect(eventData);
		}

		/// <summary>
		///   <para>What to do when the event system sends a submit Event.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public virtual void OnSubmit(BaseEventData eventData)
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			if (!this.isFocused)
			{
				this.m_ShouldActivateNextUpdate = true;
			}
		}

		private void EnforceContentType()
		{
			switch (this.contentType)
			{
			case InputField.ContentType.Standard:
				this.m_InputType = InputField.InputType.Standard;
				this.m_KeyboardType = TouchScreenKeyboardType.Default;
				this.m_CharacterValidation = InputField.CharacterValidation.None;
				return;
			case InputField.ContentType.Autocorrected:
				this.m_InputType = InputField.InputType.AutoCorrect;
				this.m_KeyboardType = TouchScreenKeyboardType.Default;
				this.m_CharacterValidation = InputField.CharacterValidation.None;
				return;
			case InputField.ContentType.IntegerNumber:
				this.m_LineType = InputField.LineType.SingleLine;
				this.m_InputType = InputField.InputType.Standard;
				this.m_KeyboardType = TouchScreenKeyboardType.NumberPad;
				this.m_CharacterValidation = InputField.CharacterValidation.Integer;
				return;
			case InputField.ContentType.DecimalNumber:
				this.m_LineType = InputField.LineType.SingleLine;
				this.m_InputType = InputField.InputType.Standard;
				this.m_KeyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
				this.m_CharacterValidation = InputField.CharacterValidation.Decimal;
				return;
			case InputField.ContentType.Alphanumeric:
				this.m_LineType = InputField.LineType.SingleLine;
				this.m_InputType = InputField.InputType.Standard;
				this.m_KeyboardType = TouchScreenKeyboardType.ASCIICapable;
				this.m_CharacterValidation = InputField.CharacterValidation.Alphanumeric;
				return;
			case InputField.ContentType.Name:
				this.m_LineType = InputField.LineType.SingleLine;
				this.m_InputType = InputField.InputType.Standard;
				this.m_KeyboardType = TouchScreenKeyboardType.Default;
				this.m_CharacterValidation = InputField.CharacterValidation.Name;
				return;
			case InputField.ContentType.EmailAddress:
				this.m_LineType = InputField.LineType.SingleLine;
				this.m_InputType = InputField.InputType.Standard;
				this.m_KeyboardType = TouchScreenKeyboardType.EmailAddress;
				this.m_CharacterValidation = InputField.CharacterValidation.EmailAddress;
				return;
			case InputField.ContentType.Password:
				this.m_LineType = InputField.LineType.SingleLine;
				this.m_InputType = InputField.InputType.Password;
				this.m_KeyboardType = TouchScreenKeyboardType.Default;
				this.m_CharacterValidation = InputField.CharacterValidation.None;
				return;
			case InputField.ContentType.Pin:
				this.m_LineType = InputField.LineType.SingleLine;
				this.m_InputType = InputField.InputType.Password;
				this.m_KeyboardType = TouchScreenKeyboardType.NumberPad;
				this.m_CharacterValidation = InputField.CharacterValidation.Integer;
				return;
			default:
				return;
			}
		}

		private void SetToCustomIfContentTypeIsNot(params InputField.ContentType[] allowedContentTypes)
		{
			if (this.contentType == InputField.ContentType.Custom)
			{
				return;
			}
			for (int i = 0; i < allowedContentTypes.Length; i++)
			{
				if (this.contentType == allowedContentTypes[i])
				{
					return;
				}
			}
			this.contentType = InputField.ContentType.Custom;
		}

		private void SetToCustom()
		{
			if (this.contentType == InputField.ContentType.Custom)
			{
				return;
			}
			this.contentType = InputField.ContentType.Custom;
		}

		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			if (this.m_HasDoneFocusTransition)
			{
				state = Selectable.SelectionState.Highlighted;
			}
			else if (state == Selectable.SelectionState.Pressed)
			{
				this.m_HasDoneFocusTransition = true;
			}
			base.DoStateTransition(state, instant);
		}

		virtual bool IsDestroyed()
		{
			return base.IsDestroyed();
		}

		virtual Transform get_transform()
		{
			return base.transform;
		}
	}
}

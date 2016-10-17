// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.InputField
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

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
    private static readonly char[] kSeparators = new char[6]{ ' ', '.', ',', '\t', '\r', '\n' };
    [FormerlySerializedAs("asteriskChar")]
    [SerializeField]
    private char m_AsteriskChar = '*';
    [FormerlySerializedAs("m_EndEdit")]
    [FormerlySerializedAs("onSubmit")]
    [FormerlySerializedAs("m_OnSubmit")]
    [SerializeField]
    private InputField.SubmitEvent m_OnEndEdit = new InputField.SubmitEvent();
    [FormerlySerializedAs("onValueChange")]
    [FormerlySerializedAs("m_OnValueChange")]
    [SerializeField]
    private InputField.OnChangeEvent m_OnValueChanged = new InputField.OnChangeEvent();
    [SerializeField]
    private Color m_CaretColor = new Color(0.1960784f, 0.1960784f, 0.1960784f, 1f);
    [FormerlySerializedAs("selectionColor")]
    [SerializeField]
    private Color m_SelectionColor = new Color(0.6588235f, 0.8078431f, 1f, 0.7529412f);
    [SerializeField]
    [FormerlySerializedAs("mValue")]
    protected string m_Text = string.Empty;
    [SerializeField]
    [Range(0.0f, 4f)]
    private float m_CaretBlinkRate = 0.85f;
    [SerializeField]
    [Range(1f, 5f)]
    private int m_CaretWidth = 1;
    private string m_OriginalText = string.Empty;
    private Event m_ProcessingEvent = new Event();
    private const float kHScrollSpeed = 0.05f;
    private const float kVScrollSpeed = 0.1f;
    private const string kEmailSpecialCharacters = "!#$%&'*+-/=?^_`{|}~";
    protected TouchScreenKeyboard m_Keyboard;
    [SerializeField]
    [FormerlySerializedAs("text")]
    protected Text m_TextComponent;
    [SerializeField]
    protected Graphic m_Placeholder;
    [SerializeField]
    private InputField.ContentType m_ContentType;
    [SerializeField]
    [FormerlySerializedAs("inputType")]
    private InputField.InputType m_InputType;
    [FormerlySerializedAs("keyboardType")]
    [SerializeField]
    private TouchScreenKeyboardType m_KeyboardType;
    [SerializeField]
    private InputField.LineType m_LineType;
    [FormerlySerializedAs("hideMobileInput")]
    [SerializeField]
    private bool m_HideMobileInput;
    [FormerlySerializedAs("validation")]
    [SerializeField]
    private InputField.CharacterValidation m_CharacterValidation;
    [FormerlySerializedAs("characterLimit")]
    [SerializeField]
    private int m_CharacterLimit;
    [FormerlySerializedAs("onValidateInput")]
    [SerializeField]
    private InputField.OnValidateInput m_OnValidateInput;
    [SerializeField]
    private bool m_CustomCaretColor;
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
    private bool m_WasCanceled;
    private bool m_HasDoneFocusTransition;

    protected Mesh mesh
    {
      get
      {
        if ((UnityEngine.Object) this.m_Mesh == (UnityEngine.Object) null)
          this.m_Mesh = new Mesh();
        return this.m_Mesh;
      }
    }

    protected TextGenerator cachedInputTextGenerator
    {
      get
      {
        if (this.m_InputTextCache == null)
          this.m_InputTextCache = new TextGenerator();
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
            return this.m_HideMobileInput;
          default:
            if (platform != RuntimePlatform.BB10Player && platform != RuntimePlatform.TizenPlayer && platform != RuntimePlatform.tvOS)
              return true;
            goto case RuntimePlatform.IPhonePlayer;
        }
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
        return this.m_Text;
      }
      set
      {
        if (this.text == value)
          return;
        if (this.onValidateInput != null || this.characterValidation != InputField.CharacterValidation.None)
        {
          this.m_Text = string.Empty;
          InputField.OnValidateInput onValidateInput = this.onValidateInput ?? new InputField.OnValidateInput(this.Validate);
          this.m_CaretPosition = this.m_CaretSelectPosition = value.Length;
          int num = this.characterLimit <= 0 ? value.Length : Math.Min(this.characterLimit - 1, value.Length);
          for (int index = 0; index < num; ++index)
          {
            char ch = onValidateInput(this.m_Text, this.m_Text.Length, value[index]);
            if ((int) ch != 0)
              this.m_Text += (string) (object) ch;
          }
        }
        else
          this.m_Text = this.characterLimit <= 0 || value.Length <= this.characterLimit ? value : value.Substring(0, this.characterLimit);
        if (!Application.isPlaying)
        {
          this.SendOnValueChangedAndUpdateLabel();
        }
        else
        {
          if (this.m_Keyboard != null)
            this.m_Keyboard.text = this.m_Text;
          if (this.m_CaretPosition > this.m_Text.Length)
            this.m_CaretPosition = this.m_CaretSelectPosition = this.m_Text.Length;
          this.SendOnValueChangedAndUpdateLabel();
        }
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
        if (!SetPropertyUtility.SetStruct<float>(ref this.m_CaretBlinkRate, value) || !this.m_AllowInput)
          return;
        this.SetCaretActive();
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
        if (!SetPropertyUtility.SetStruct<int>(ref this.m_CaretWidth, value))
          return;
        this.MarkGeometryAsDirty();
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
    ///         <para>This is an optional ‘empty’ graphic to show that InputField text field is empty. Note that this ‘empty' graphic still displays even when the InputField is selected (that is; when there is focus on it).
    /// 
    /// A placeholder graphic can be used to show subtle hints or make it more obvious that the control is an InputField.</para>
    ///       </summary>
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
        if (this.customCaretColor)
          return this.m_CaretColor;
        return this.textComponent.color;
      }
      set
      {
        if (!SetPropertyUtility.SetColor(ref this.m_CaretColor, value))
          return;
        this.MarkGeometryAsDirty();
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
        if (this.m_CustomCaretColor == value)
          return;
        this.m_CustomCaretColor = value;
        this.MarkGeometryAsDirty();
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
        if (!SetPropertyUtility.SetColor(ref this.m_SelectionColor, value))
          return;
        this.MarkGeometryAsDirty();
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
        if (!SetPropertyUtility.SetStruct<int>(ref this.m_CharacterLimit, Math.Max(0, value)))
          return;
        this.UpdateLabel();
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
        if (!SetPropertyUtility.SetStruct<InputField.ContentType>(ref this.m_ContentType, value))
          return;
        this.EnforceContentType();
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
        if (!SetPropertyUtility.SetStruct<InputField.LineType>(ref this.m_LineType, value))
          return;
        this.SetToCustomIfContentTypeIsNot(InputField.ContentType.Standard, InputField.ContentType.Autocorrected);
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
        if (!SetPropertyUtility.SetStruct<InputField.InputType>(ref this.m_InputType, value))
          return;
        this.SetToCustom();
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
        if (!SetPropertyUtility.SetStruct<TouchScreenKeyboardType>(ref this.m_KeyboardType, value))
          return;
        this.SetToCustom();
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
        if (!SetPropertyUtility.SetStruct<InputField.CharacterValidation>(ref this.m_CharacterValidation, value))
          return;
        this.SetToCustom();
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
        if (this.m_LineType != InputField.LineType.MultiLineNewline)
          return this.lineType == InputField.LineType.MultiLineSubmit;
        return true;
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
        if (!SetPropertyUtility.SetStruct<char>(ref this.m_AsteriskChar, value))
          return;
        this.UpdateLabel();
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
          return;
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
          return;
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
      else
      {
        if (pos <= this.text.Length)
          return;
        pos = this.text.Length;
      }
    }

    protected override void OnValidate()
    {
      base.OnValidate();
      this.EnforceContentType();
      this.m_CharacterLimit = Math.Max(0, this.m_CharacterLimit);
      if (!this.IsActive())
        return;
      this.UpdateLabel();
      if (!this.m_AllowInput)
        return;
      this.SetCaretActive();
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      if (this.m_Text == null)
        this.m_Text = string.Empty;
      this.m_DrawStart = 0;
      this.m_DrawEnd = this.m_Text.Length;
      if ((UnityEngine.Object) this.m_CachedInputRenderer != (UnityEngine.Object) null)
        this.m_CachedInputRenderer.SetMaterial(Graphic.defaultGraphicMaterial, (Texture) Texture2D.whiteTexture);
      if (!((UnityEngine.Object) this.m_TextComponent != (UnityEngine.Object) null))
        return;
      this.m_TextComponent.RegisterDirtyVerticesCallback(new UnityAction(this.MarkGeometryAsDirty));
      this.m_TextComponent.RegisterDirtyVerticesCallback(new UnityAction(this.UpdateLabel));
      this.UpdateLabel();
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected override void OnDisable()
    {
      this.m_BlinkCoroutine = (Coroutine) null;
      this.DeactivateInputField();
      if ((UnityEngine.Object) this.m_TextComponent != (UnityEngine.Object) null)
      {
        this.m_TextComponent.UnregisterDirtyVerticesCallback(new UnityAction(this.MarkGeometryAsDirty));
        this.m_TextComponent.UnregisterDirtyVerticesCallback(new UnityAction(this.UpdateLabel));
      }
      CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild((ICanvasElement) this);
      if ((UnityEngine.Object) this.m_CachedInputRenderer != (UnityEngine.Object) null)
        this.m_CachedInputRenderer.Clear();
      if ((UnityEngine.Object) this.m_Mesh != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Mesh);
      this.m_Mesh = (Mesh) null;
      base.OnDisable();
    }

    [DebuggerHidden]
    private IEnumerator CaretBlink()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new InputField.\u003CCaretBlink\u003Ec__Iterator3() { \u003C\u003Ef__this = this };
    }

    private void SetCaretVisible()
    {
      if (!this.m_AllowInput)
        return;
      this.m_CaretVisible = true;
      this.m_BlinkStartTime = Time.unscaledTime;
      this.SetCaretActive();
    }

    private void SetCaretActive()
    {
      if (!this.m_AllowInput)
        return;
      if ((double) this.m_CaretBlinkRate > 0.0)
      {
        if (this.m_BlinkCoroutine != null)
          return;
        this.m_BlinkCoroutine = this.StartCoroutine(this.CaretBlink());
      }
      else
        this.m_CaretVisible = true;
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
        return;
      this.AssignPositioningIfNeeded();
      if (this.m_Keyboard == null || !this.m_Keyboard.active)
      {
        if (this.m_Keyboard != null)
        {
          if (!this.m_ReadOnly)
            this.text = this.m_Keyboard.text;
          if (this.m_Keyboard.wasCanceled)
            this.m_WasCanceled = true;
        }
        this.OnDeselect((BaseEventData) null);
      }
      else
      {
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
            for (int index = 0; index < text.Length; ++index)
            {
              char ch = text[index];
              switch (ch)
              {
                case '\r':
                case '\x0003':
                  ch = '\n';
                  break;
              }
              if (this.onValidateInput != null)
                ch = this.onValidateInput(this.m_Text, this.m_Text.Length, ch);
              else if (this.characterValidation != InputField.CharacterValidation.None)
                ch = this.Validate(this.m_Text, this.m_Text.Length, ch);
              if (this.lineType == InputField.LineType.MultiLineSubmit && (int) ch == 10)
              {
                this.m_Keyboard.text = this.m_Text;
                this.OnDeselect((BaseEventData) null);
                return;
              }
              if ((int) ch != 0)
                this.m_Text += (string) (object) ch;
            }
            if (this.characterLimit > 0 && this.m_Text.Length > this.characterLimit)
              this.m_Text = this.m_Text.Substring(0, this.characterLimit);
            int length = this.m_Text.Length;
            this.caretSelectPositionInternal = length;
            this.caretPositionInternal = length;
            if (this.m_Text != text)
              this.m_Keyboard.text = this.m_Text;
            this.SendOnValueChangedAndUpdateLabel();
          }
        }
        if (!this.m_Keyboard.done)
          return;
        if (this.m_Keyboard.wasCanceled)
          this.m_WasCanceled = true;
        this.OnDeselect((BaseEventData) null);
      }
    }

    /// <summary>
    ///   <para>Convert screen space into input field local space.</para>
    /// </summary>
    /// <param name="screen"></param>
    [Obsolete("This function is no longer used. Please use RectTransformUtility.ScreenPointToLocalPointInRectangle() instead.")]
    public Vector2 ScreenToLocal(Vector2 screen)
    {
      Canvas canvas = this.m_TextComponent.canvas;
      if ((UnityEngine.Object) canvas == (UnityEngine.Object) null)
        return screen;
      Vector3 vector3 = Vector3.zero;
      if (canvas.renderMode == UnityEngine.RenderMode.ScreenSpaceOverlay)
        vector3 = this.m_TextComponent.transform.InverseTransformPoint((Vector3) screen);
      else if ((UnityEngine.Object) canvas.worldCamera != (UnityEngine.Object) null)
      {
        Ray ray = canvas.worldCamera.ScreenPointToRay((Vector3) screen);
        float enter;
        new Plane(this.m_TextComponent.transform.forward, this.m_TextComponent.transform.position).Raycast(ray, out enter);
        vector3 = this.m_TextComponent.transform.InverseTransformPoint(ray.GetPoint(enter));
      }
      return new Vector2(vector3.x, vector3.y);
    }

    private int GetUnclampedCharacterLineFromPosition(Vector2 pos, TextGenerator generator)
    {
      if (!this.multiLine)
        return 0;
      float num1 = pos.y * this.m_TextComponent.pixelsPerUnit;
      float num2 = 0.0f;
      for (int index = 0; index < generator.lineCount; ++index)
      {
        float topY = generator.lines[index].topY;
        float num3 = topY - (float) generator.lines[index].height;
        if ((double) num1 > (double) topY)
        {
          float num4 = topY - num2;
          if ((double) num1 > (double) topY - 0.5 * (double) num4)
            return index - 1;
          return index;
        }
        if ((double) num1 > (double) num3)
          return index;
        num2 = num3;
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
        return 0;
      int lineFromPosition = this.GetUnclampedCharacterLineFromPosition(pos, cachedTextGenerator);
      if (lineFromPosition < 0)
        return 0;
      if (lineFromPosition >= cachedTextGenerator.lineCount)
        return cachedTextGenerator.characterCountVisible;
      int startCharIdx = cachedTextGenerator.lines[lineFromPosition].startCharIdx;
      int lineEndPosition = InputField.GetLineEndPosition(cachedTextGenerator, lineFromPosition);
      for (int index = startCharIdx; index < lineEndPosition && index < cachedTextGenerator.characterCountVisible; ++index)
      {
        UICharInfo character = cachedTextGenerator.characters[index];
        Vector2 vector2 = character.cursorPos / this.m_TextComponent.pixelsPerUnit;
        if ((double) (pos.x - vector2.x) < (double) (vector2.x + character.charWidth / this.m_TextComponent.pixelsPerUnit - pos.x))
          return index;
      }
      return lineEndPosition;
    }

    private bool MayDrag(PointerEventData eventData)
    {
      if (this.IsActive() && this.IsInteractable() && (eventData.button == PointerEventData.InputButton.Left && (UnityEngine.Object) this.m_TextComponent != (UnityEngine.Object) null))
        return this.m_Keyboard == null;
      return false;
    }

    /// <summary>
    ///   <para>Capture the OnBeginDrag callback from the EventSystem and ensure we should listen to the drag events to follow.</para>
    /// </summary>
    /// <param name="eventData">The data passed by the EventSystem.</param>
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData))
        return;
      this.m_UpdateDrag = true;
    }

    /// <summary>
    ///   <para>What to do when the event system sends a Drag Event.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnDrag(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData))
        return;
      Vector2 localPoint;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this.textComponent.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
      this.caretSelectPositionInternal = this.GetCharacterIndexFromPosition(localPoint) + this.m_DrawStart;
      this.MarkGeometryAsDirty();
      this.m_DragPositionOutOfBounds = !RectTransformUtility.RectangleContainsScreenPoint(this.textComponent.rectTransform, eventData.position, eventData.pressEventCamera);
      if (this.m_DragPositionOutOfBounds && this.m_DragCoroutine == null)
        this.m_DragCoroutine = this.StartCoroutine(this.MouseDragOutsideRect(eventData));
      eventData.Use();
    }

    [DebuggerHidden]
    private IEnumerator MouseDragOutsideRect(PointerEventData eventData)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new InputField.\u003CMouseDragOutsideRect\u003Ec__Iterator4() { eventData = eventData, \u003C\u0024\u003EeventData = eventData, \u003C\u003Ef__this = this };
    }

    /// <summary>
    ///   <para>Capture the OnEndDrag callback from the EventSystem and cancel the listening of drag events.</para>
    /// </summary>
    /// <param name="eventData">The eventData sent by the EventSystem.</param>
    public virtual void OnEndDrag(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData))
        return;
      this.m_UpdateDrag = false;
    }

    /// <summary>
    ///   <para>What to do when the event system sends a pointer down Event.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerDown(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData))
        return;
      EventSystem.current.SetSelectedGameObject(this.gameObject, (BaseEventData) eventData);
      bool allowInput = this.m_AllowInput;
      base.OnPointerDown(eventData);
      if (!this.InPlaceEditing() && (this.m_Keyboard == null || !this.m_Keyboard.active))
      {
        this.OnSelect((BaseEventData) eventData);
      }
      else
      {
        if (allowInput)
        {
          Vector2 localPoint;
          RectTransformUtility.ScreenPointToLocalPointInRectangle(this.textComponent.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
          int num = this.GetCharacterIndexFromPosition(localPoint) + this.m_DrawStart;
          this.caretPositionInternal = num;
          this.caretSelectPositionInternal = num;
        }
        this.UpdateLabel();
        eventData.Use();
      }
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
      int num;
      switch (platform)
      {
        case RuntimePlatform.OSXEditor:
        case RuntimePlatform.OSXPlayer:
          num = 1;
          break;
        default:
          num = platform == RuntimePlatform.OSXWebPlayer ? 1 : 0;
          break;
      }
      bool ctrl = num == 0 ? (modifiers & EventModifiers.Control) != EventModifiers.None : (modifiers & EventModifiers.Command) != EventModifiers.None;
      bool shift = (modifiers & EventModifiers.Shift) != EventModifiers.None;
      bool flag1 = (modifiers & EventModifiers.Alt) != EventModifiers.None;
      bool flag2 = ctrl && !flag1 && !shift;
      KeyCode keyCode = evt.keyCode;
      switch (keyCode)
      {
        case KeyCode.KeypadEnter:
label_26:
          if (this.lineType != InputField.LineType.MultiLineNewline)
            return InputField.EditState.Finish;
          break;
        case KeyCode.UpArrow:
          this.MoveUp(shift);
          return InputField.EditState.Continue;
        case KeyCode.DownArrow:
          this.MoveDown(shift);
          return InputField.EditState.Continue;
        case KeyCode.RightArrow:
          this.MoveRight(shift, ctrl);
          return InputField.EditState.Continue;
        case KeyCode.LeftArrow:
          this.MoveLeft(shift, ctrl);
          return InputField.EditState.Continue;
        case KeyCode.Home:
          this.MoveTextStart(shift);
          return InputField.EditState.Continue;
        case KeyCode.End:
          this.MoveTextEnd(shift);
          return InputField.EditState.Continue;
        default:
          switch (keyCode - 97)
          {
            case KeyCode.None:
              if (flag2)
              {
                this.SelectAll();
                return InputField.EditState.Continue;
              }
              break;
            case (KeyCode) 2:
              if (flag2)
              {
                InputField.clipboard = this.inputType == InputField.InputType.Password ? string.Empty : this.GetSelectedString();
                return InputField.EditState.Continue;
              }
              break;
            default:
              switch (keyCode - 118)
              {
                case KeyCode.None:
                  if (flag2)
                  {
                    this.Append(InputField.clipboard);
                    return InputField.EditState.Continue;
                  }
                  break;
                case (KeyCode) 2:
                  if (flag2)
                  {
                    InputField.clipboard = this.inputType == InputField.InputType.Password ? string.Empty : this.GetSelectedString();
                    this.Delete();
                    this.SendOnValueChangedAndUpdateLabel();
                    return InputField.EditState.Continue;
                  }
                  break;
                default:
                  if (keyCode != KeyCode.Backspace)
                  {
                    if (keyCode != KeyCode.Return)
                    {
                      if (keyCode != KeyCode.Escape)
                      {
                        if (keyCode == KeyCode.Delete)
                        {
                          this.ForwardSpace();
                          return InputField.EditState.Continue;
                        }
                        break;
                      }
                      this.m_WasCanceled = true;
                      return InputField.EditState.Finish;
                    }
                    goto label_26;
                  }
                  else
                  {
                    this.Backspace();
                    return InputField.EditState.Continue;
                  }
              }
          }
      }
      char ch = evt.character;
      if (!this.multiLine && ((int) ch == 9 || (int) ch == 13 || (int) ch == 10))
        return InputField.EditState.Continue;
      if ((int) ch == 13 || (int) ch == 3)
        ch = '\n';
      if (this.IsValidChar(ch))
        this.Append(ch);
      if ((int) ch == 0 && Input.compositionString.Length > 0)
        this.UpdateLabel();
      return InputField.EditState.Continue;
    }

    private bool IsValidChar(char c)
    {
      if ((int) c == (int) sbyte.MaxValue)
        return false;
      if ((int) c == 9 || (int) c == 10)
        return true;
      return this.m_TextComponent.font.HasCharacter(c);
    }

    /// <summary>
    ///   <para>Helper function to allow separate events to be processed by the InputField.</para>
    /// </summary>
    /// <param name="e">The Event to be processed.</param>
    public void ProcessEvent(Event e)
    {
      int num = (int) this.KeyPressed(e);
    }

    /// <summary>
    ///   <para>What to do when the event system sends a Update selected Event.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnUpdateSelected(BaseEventData eventData)
    {
      if (!this.isFocused)
        return;
      bool flag = false;
      while (Event.PopEvent(this.m_ProcessingEvent))
      {
        if (this.m_ProcessingEvent.rawType == EventType.KeyDown)
        {
          flag = true;
          if (this.KeyPressed(this.m_ProcessingEvent) == InputField.EditState.Finish)
          {
            this.DeactivateInputField();
            break;
          }
        }
        switch (this.m_ProcessingEvent.type)
        {
          case EventType.ValidateCommand:
          case EventType.ExecuteCommand:
            string commandName = this.m_ProcessingEvent.commandName;
            if (commandName != null)
            {
              // ISSUE: reference to a compiler-generated field
              if (InputField.\u003C\u003Ef__switch\u0024map0 == null)
              {
                // ISSUE: reference to a compiler-generated field
                InputField.\u003C\u003Ef__switch\u0024map0 = new Dictionary<string, int>(1)
                {
                  {
                    "SelectAll",
                    0
                  }
                };
              }
              int num;
              // ISSUE: reference to a compiler-generated field
              if (InputField.\u003C\u003Ef__switch\u0024map0.TryGetValue(commandName, out num) && num == 0)
              {
                this.SelectAll();
                flag = true;
                continue;
              }
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      if (flag)
        this.UpdateLabel();
      eventData.Use();
    }

    private string GetSelectedString()
    {
      if (!this.hasSelection)
        return string.Empty;
      int startIndex = this.caretPositionInternal;
      int num1 = this.caretSelectPositionInternal;
      if (startIndex > num1)
      {
        int num2 = startIndex;
        startIndex = num1;
        num1 = num2;
      }
      return this.text.Substring(startIndex, num1 - startIndex);
    }

    private int FindtNextWordBegin()
    {
      if (this.caretSelectPositionInternal + 1 >= this.text.Length)
        return this.text.Length;
      int num = this.text.IndexOfAny(InputField.kSeparators, this.caretSelectPositionInternal + 1);
      return num != -1 ? num + 1 : this.text.Length;
    }

    private void MoveRight(bool shift, bool ctrl)
    {
      if (this.hasSelection && !shift)
      {
        int num = Mathf.Max(this.caretPositionInternal, this.caretSelectPositionInternal);
        this.caretSelectPositionInternal = num;
        this.caretPositionInternal = num;
      }
      else
      {
        int num1 = !ctrl ? this.caretSelectPositionInternal + 1 : this.FindtNextWordBegin();
        if (shift)
        {
          this.caretSelectPositionInternal = num1;
        }
        else
        {
          int num2 = num1;
          this.caretPositionInternal = num2;
          this.caretSelectPositionInternal = num2;
        }
      }
    }

    private int FindtPrevWordBegin()
    {
      if (this.caretSelectPositionInternal - 2 < 0)
        return 0;
      int num = this.text.LastIndexOfAny(InputField.kSeparators, this.caretSelectPositionInternal - 2);
      return num != -1 ? num + 1 : 0;
    }

    private void MoveLeft(bool shift, bool ctrl)
    {
      if (this.hasSelection && !shift)
      {
        int num = Mathf.Min(this.caretPositionInternal, this.caretSelectPositionInternal);
        this.caretSelectPositionInternal = num;
        this.caretPositionInternal = num;
      }
      else
      {
        int num1 = !ctrl ? this.caretSelectPositionInternal - 1 : this.FindtPrevWordBegin();
        if (shift)
        {
          this.caretSelectPositionInternal = num1;
        }
        else
        {
          int num2 = num1;
          this.caretPositionInternal = num2;
          this.caretSelectPositionInternal = num2;
        }
      }
    }

    private int DetermineCharacterLine(int charPos, TextGenerator generator)
    {
      for (int index = 0; index < generator.lineCount - 1; ++index)
      {
        if (generator.lines[index + 1].startCharIdx > charPos)
          return index;
      }
      return generator.lineCount - 1;
    }

    private int LineUpCharacterPosition(int originalPos, bool goToFirstChar)
    {
      if (originalPos > this.cachedInputTextGenerator.characterCountVisible)
        return 0;
      UICharInfo character = this.cachedInputTextGenerator.characters[originalPos];
      int characterLine = this.DetermineCharacterLine(originalPos, this.cachedInputTextGenerator);
      if (characterLine <= 0)
      {
        if (goToFirstChar)
          return 0;
        return originalPos;
      }
      int num = this.cachedInputTextGenerator.lines[characterLine].startCharIdx - 1;
      for (int startCharIdx = this.cachedInputTextGenerator.lines[characterLine - 1].startCharIdx; startCharIdx < num; ++startCharIdx)
      {
        if ((double) this.cachedInputTextGenerator.characters[startCharIdx].cursorPos.x >= (double) character.cursorPos.x)
          return startCharIdx;
      }
      return num;
    }

    private int LineDownCharacterPosition(int originalPos, bool goToLastChar)
    {
      if (originalPos >= this.cachedInputTextGenerator.characterCountVisible)
        return this.text.Length;
      UICharInfo character = this.cachedInputTextGenerator.characters[originalPos];
      int characterLine = this.DetermineCharacterLine(originalPos, this.cachedInputTextGenerator);
      if (characterLine + 1 >= this.cachedInputTextGenerator.lineCount)
      {
        if (goToLastChar)
          return this.text.Length;
        return originalPos;
      }
      int lineEndPosition = InputField.GetLineEndPosition(this.cachedInputTextGenerator, characterLine + 1);
      for (int startCharIdx = this.cachedInputTextGenerator.lines[characterLine + 1].startCharIdx; startCharIdx < lineEndPosition; ++startCharIdx)
      {
        if ((double) this.cachedInputTextGenerator.characters[startCharIdx].cursorPos.x >= (double) character.cursorPos.x)
          return startCharIdx;
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
      int num1 = !this.multiLine ? this.text.Length : this.LineDownCharacterPosition(this.caretSelectPositionInternal, goToLastChar);
      if (shift)
      {
        this.caretSelectPositionInternal = num1;
      }
      else
      {
        int num2 = num1;
        this.caretSelectPositionInternal = num2;
        this.caretPositionInternal = num2;
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
      int num1 = !this.multiLine ? 0 : this.LineUpCharacterPosition(this.caretSelectPositionInternal, goToFirstChar);
      if (shift)
      {
        this.caretSelectPositionInternal = num1;
      }
      else
      {
        int num2 = num1;
        this.caretPositionInternal = num2;
        this.caretSelectPositionInternal = num2;
      }
    }

    private void Delete()
    {
      if (this.m_ReadOnly || this.caretPositionInternal == this.caretSelectPositionInternal)
        return;
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
        return;
      if (this.hasSelection)
      {
        this.Delete();
        this.SendOnValueChangedAndUpdateLabel();
      }
      else
      {
        if (this.caretPositionInternal >= this.text.Length)
          return;
        this.m_Text = this.text.Remove(this.caretPositionInternal, 1);
        this.SendOnValueChangedAndUpdateLabel();
      }
    }

    private void Backspace()
    {
      if (this.m_ReadOnly)
        return;
      if (this.hasSelection)
      {
        this.Delete();
        this.SendOnValueChangedAndUpdateLabel();
      }
      else
      {
        if (this.caretPositionInternal <= 0)
          return;
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
        return;
      string str = c.ToString();
      this.Delete();
      if (this.characterLimit > 0 && this.text.Length >= this.characterLimit)
        return;
      this.m_Text = this.text.Insert(this.m_CaretPosition, str);
      this.caretSelectPositionInternal = (this.caretPositionInternal += str.Length);
      this.SendOnValueChanged();
    }

    private void SendOnValueChangedAndUpdateLabel()
    {
      this.SendOnValueChanged();
      this.UpdateLabel();
    }

    private void SendOnValueChanged()
    {
      if (this.onValueChanged == null)
        return;
      this.onValueChanged.Invoke(this.text);
    }

    /// <summary>
    ///   <para>Convenience function to make functionality to send the SubmitEvent easier.</para>
    /// </summary>
    protected void SendOnSubmit()
    {
      if (this.onEndEdit == null)
        return;
      this.onEndEdit.Invoke(this.m_Text);
    }

    /// <summary>
    ///   <para>Append a character to the input field.</para>
    /// </summary>
    /// <param name="input">Character / string to append.</param>
    protected virtual void Append(string input)
    {
      if (this.m_ReadOnly || !this.InPlaceEditing())
        return;
      int index = 0;
      for (int length = input.Length; index < length; ++index)
      {
        char input1 = input[index];
        if ((int) input1 >= 32 || (int) input1 == 9 || ((int) input1 == 13 || (int) input1 == 10) || (int) input1 == 10)
          this.Append(input1);
      }
    }

    /// <summary>
    ///   <para>Append a character to the input field.</para>
    /// </summary>
    /// <param name="input">Character / string to append.</param>
    protected virtual void Append(char input)
    {
      if (this.m_ReadOnly || !this.InPlaceEditing())
        return;
      if (this.onValidateInput != null)
        input = this.onValidateInput(this.text, this.caretPositionInternal, input);
      else if (this.characterValidation != InputField.CharacterValidation.None)
        input = this.Validate(this.text, this.caretPositionInternal, input);
      if ((int) input == 0)
        return;
      this.Insert(input);
    }

    /// <summary>
    ///   <para>Update the Text associated with this input field.</para>
    /// </summary>
    protected void UpdateLabel()
    {
      if (!((UnityEngine.Object) this.m_TextComponent != (UnityEngine.Object) null) || !((UnityEngine.Object) this.m_TextComponent.font != (UnityEngine.Object) null) || this.m_PreventFontCallback)
        return;
      this.m_PreventFontCallback = true;
      string str1 = Input.compositionString.Length <= 0 ? this.text : this.text.Substring(0, this.m_CaretPosition) + Input.compositionString + this.text.Substring(this.m_CaretPosition);
      string str2 = this.inputType != InputField.InputType.Password ? str1 : new string(this.asteriskChar, str1.Length);
      bool flag = string.IsNullOrEmpty(str1);
      if ((UnityEngine.Object) this.m_Placeholder != (UnityEngine.Object) null)
        this.m_Placeholder.enabled = flag;
      if (!this.m_AllowInput)
      {
        this.m_DrawStart = 0;
        this.m_DrawEnd = this.m_Text.Length;
      }
      if (!flag)
      {
        TextGenerationSettings generationSettings = this.m_TextComponent.GetGenerationSettings(this.m_TextComponent.rectTransform.rect.size);
        generationSettings.generateOutOfBounds = true;
        this.cachedInputTextGenerator.Populate(str2, generationSettings);
        this.SetDrawRangeToContainCaretPosition(this.caretSelectPositionInternal);
        str2 = str2.Substring(this.m_DrawStart, Mathf.Min(this.m_DrawEnd, str2.Length) - this.m_DrawStart);
        this.SetCaretVisible();
      }
      this.m_TextComponent.text = str2;
      this.MarkGeometryAsDirty();
      this.m_PreventFontCallback = false;
    }

    private bool IsSelectionVisible()
    {
      return this.m_DrawStart <= this.caretPositionInternal && this.m_DrawStart <= this.caretSelectPositionInternal && (this.m_DrawEnd >= this.caretPositionInternal && this.m_DrawEnd >= this.caretSelectPositionInternal);
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
        return gen.lines[line + 1].startCharIdx - 1;
      return gen.characterCountVisible;
    }

    private void SetDrawRangeToContainCaretPosition(int caretPos)
    {
      if (this.cachedInputTextGenerator.lineCount <= 0)
        return;
      Vector2 size = this.cachedInputTextGenerator.rectExtents.size;
      if (this.multiLine)
      {
        IList<UILineInfo> lines = this.cachedInputTextGenerator.lines;
        int characterLine1 = this.DetermineCharacterLine(caretPos, this.cachedInputTextGenerator);
        if (caretPos > this.m_DrawEnd)
        {
          this.m_DrawEnd = InputField.GetLineEndPosition(this.cachedInputTextGenerator, characterLine1);
          float num = lines[characterLine1].topY - (float) lines[characterLine1].height;
          int line = characterLine1;
          while (line > 0 && (double) lines[line - 1].topY - (double) num <= (double) size.y)
            --line;
          this.m_DrawStart = InputField.GetLineStartPosition(this.cachedInputTextGenerator, line);
        }
        else
        {
          if (caretPos < this.m_DrawStart)
            this.m_DrawStart = InputField.GetLineStartPosition(this.cachedInputTextGenerator, characterLine1);
          int characterLine2 = this.DetermineCharacterLine(this.m_DrawStart, this.cachedInputTextGenerator);
          int line = characterLine2;
          float topY = lines[characterLine2].topY;
          float num = lines[line].topY - (float) lines[line].height;
          for (; line < lines.Count - 1; ++line)
          {
            num = lines[line + 1].topY - (float) lines[line + 1].height;
            if ((double) topY - (double) num > (double) size.y)
              break;
          }
          this.m_DrawEnd = InputField.GetLineEndPosition(this.cachedInputTextGenerator, line);
          while (characterLine2 > 0 && (double) lines[characterLine2 - 1].topY - (double) num <= (double) size.y)
            --characterLine2;
          this.m_DrawStart = InputField.GetLineStartPosition(this.cachedInputTextGenerator, characterLine2);
        }
      }
      else
      {
        IList<UICharInfo> characters = this.cachedInputTextGenerator.characters;
        if (this.m_DrawEnd > this.cachedInputTextGenerator.characterCountVisible)
          this.m_DrawEnd = this.cachedInputTextGenerator.characterCountVisible;
        float num = 0.0f;
        if (caretPos > this.m_DrawEnd || caretPos == this.m_DrawEnd && this.m_DrawStart > 0)
        {
          this.m_DrawEnd = caretPos;
          for (this.m_DrawStart = this.m_DrawEnd - 1; this.m_DrawStart >= 0 && (double) num + (double) characters[this.m_DrawStart].charWidth <= (double) size.x; --this.m_DrawStart)
            num += characters[this.m_DrawStart].charWidth;
          ++this.m_DrawStart;
        }
        else
        {
          if (caretPos < this.m_DrawStart)
            this.m_DrawStart = caretPos;
          this.m_DrawEnd = this.m_DrawStart;
        }
        for (; this.m_DrawEnd < this.cachedInputTextGenerator.characterCountVisible; ++this.m_DrawEnd)
        {
          num += characters[this.m_DrawEnd].charWidth;
          if ((double) num > (double) size.x)
            break;
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
      if (!Application.isPlaying || PrefabUtility.GetPrefabObject((UnityEngine.Object) this.gameObject) != (UnityEngine.Object) null)
        return;
      CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild((ICanvasElement) this);
    }

    /// <summary>
    ///   <para>Rebuild the input fields geometry. (caret and highlight).</para>
    /// </summary>
    /// <param name="update"></param>
    public virtual void Rebuild(CanvasUpdate update)
    {
      if (update != CanvasUpdate.LatePreRender)
        return;
      this.UpdateGeometry();
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
      if (!Application.isPlaying || !this.shouldHideMobileInput)
        return;
      if ((UnityEngine.Object) this.m_CachedInputRenderer == (UnityEngine.Object) null && (UnityEngine.Object) this.m_TextComponent != (UnityEngine.Object) null)
      {
        GameObject gameObject = new GameObject(this.transform.name + " Input Caret");
        gameObject.hideFlags = HideFlags.DontSave;
        gameObject.transform.SetParent(this.m_TextComponent.transform.parent);
        gameObject.transform.SetAsFirstSibling();
        gameObject.layer = this.gameObject.layer;
        this.caretRectTrans = gameObject.AddComponent<RectTransform>();
        this.m_CachedInputRenderer = gameObject.AddComponent<CanvasRenderer>();
        this.m_CachedInputRenderer.SetMaterial(Graphic.defaultGraphicMaterial, (Texture) Texture2D.whiteTexture);
        gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
        this.AssignPositioningIfNeeded();
      }
      if ((UnityEngine.Object) this.m_CachedInputRenderer == (UnityEngine.Object) null)
        return;
      this.OnFillVBO(this.mesh);
      this.m_CachedInputRenderer.SetMesh(this.mesh);
    }

    private void AssignPositioningIfNeeded()
    {
      if (!((UnityEngine.Object) this.m_TextComponent != (UnityEngine.Object) null) || !((UnityEngine.Object) this.caretRectTrans != (UnityEngine.Object) null) || !(this.caretRectTrans.localPosition != this.m_TextComponent.rectTransform.localPosition) && !(this.caretRectTrans.localRotation != this.m_TextComponent.rectTransform.localRotation) && (!(this.caretRectTrans.localScale != this.m_TextComponent.rectTransform.localScale) && !(this.caretRectTrans.anchorMin != this.m_TextComponent.rectTransform.anchorMin)) && (!(this.caretRectTrans.anchorMax != this.m_TextComponent.rectTransform.anchorMax) && !(this.caretRectTrans.anchoredPosition != this.m_TextComponent.rectTransform.anchoredPosition) && (!(this.caretRectTrans.sizeDelta != this.m_TextComponent.rectTransform.sizeDelta) && !(this.caretRectTrans.pivot != this.m_TextComponent.rectTransform.pivot))))
        return;
      this.caretRectTrans.localPosition = this.m_TextComponent.rectTransform.localPosition;
      this.caretRectTrans.localRotation = this.m_TextComponent.rectTransform.localRotation;
      this.caretRectTrans.localScale = this.m_TextComponent.rectTransform.localScale;
      this.caretRectTrans.anchorMin = this.m_TextComponent.rectTransform.anchorMin;
      this.caretRectTrans.anchorMax = this.m_TextComponent.rectTransform.anchorMax;
      this.caretRectTrans.anchoredPosition = this.m_TextComponent.rectTransform.anchoredPosition;
      this.caretRectTrans.sizeDelta = this.m_TextComponent.rectTransform.sizeDelta;
      this.caretRectTrans.pivot = this.m_TextComponent.rectTransform.pivot;
    }

    private void OnFillVBO(Mesh vbo)
    {
      using (VertexHelper vbo1 = new VertexHelper())
      {
        if (!this.isFocused)
        {
          vbo1.FillMesh(vbo);
        }
        else
        {
          Rect rect = this.m_TextComponent.rectTransform.rect;
          Vector2 size = rect.size;
          Vector2 textAnchorPivot = Text.GetTextAnchorPivot(this.m_TextComponent.alignment);
          Vector2 zero = Vector2.zero;
          zero.x = Mathf.Lerp(rect.xMin, rect.xMax, textAnchorPivot.x);
          zero.y = Mathf.Lerp(rect.yMin, rect.yMax, textAnchorPivot.y);
          Vector2 roundingOffset = this.m_TextComponent.PixelAdjustPoint(zero) - zero + Vector2.Scale(size, textAnchorPivot);
          roundingOffset.x = roundingOffset.x - Mathf.Floor(0.5f + roundingOffset.x);
          roundingOffset.y = roundingOffset.y - Mathf.Floor(0.5f + roundingOffset.y);
          if (!this.hasSelection)
            this.GenerateCaret(vbo1, roundingOffset);
          else
            this.GenerateHightlight(vbo1, roundingOffset);
          vbo1.FillMesh(vbo);
        }
      }
    }

    private void GenerateCaret(VertexHelper vbo, Vector2 roundingOffset)
    {
      if (!this.m_CaretVisible)
        return;
      if (this.m_CursorVerts == null)
        this.CreateCursorVerts();
      float caretWidth = (float) this.m_CaretWidth;
      int charPos = Mathf.Max(0, this.caretPositionInternal - this.m_DrawStart);
      TextGenerator cachedTextGenerator = this.m_TextComponent.cachedTextGenerator;
      if (cachedTextGenerator == null || cachedTextGenerator.lineCount == 0)
        return;
      Vector2 zero = Vector2.zero;
      if (charPos < cachedTextGenerator.characters.Count)
      {
        UICharInfo character = cachedTextGenerator.characters[charPos];
        zero.x = character.cursorPos.x;
      }
      zero.x /= this.m_TextComponent.pixelsPerUnit;
      if ((double) zero.x > (double) this.m_TextComponent.rectTransform.rect.xMax)
        zero.x = this.m_TextComponent.rectTransform.rect.xMax;
      int characterLine = this.DetermineCharacterLine(charPos, cachedTextGenerator);
      zero.y = cachedTextGenerator.lines[characterLine].topY / this.m_TextComponent.pixelsPerUnit;
      float num = (float) cachedTextGenerator.lines[characterLine].height / this.m_TextComponent.pixelsPerUnit;
      for (int index = 0; index < this.m_CursorVerts.Length; ++index)
        this.m_CursorVerts[index].color = (Color32) this.caretColor;
      this.m_CursorVerts[0].position = new Vector3(zero.x, zero.y - num, 0.0f);
      this.m_CursorVerts[1].position = new Vector3(zero.x + caretWidth, zero.y - num, 0.0f);
      this.m_CursorVerts[2].position = new Vector3(zero.x + caretWidth, zero.y, 0.0f);
      this.m_CursorVerts[3].position = new Vector3(zero.x, zero.y, 0.0f);
      if (roundingOffset != Vector2.zero)
      {
        for (int index = 0; index < this.m_CursorVerts.Length; ++index)
        {
          UIVertex cursorVert = this.m_CursorVerts[index];
          cursorVert.position.x += roundingOffset.x;
          cursorVert.position.y += roundingOffset.y;
        }
      }
      vbo.AddUIVertexQuad(this.m_CursorVerts);
      int height = Screen.height;
      zero.y = (float) height - zero.y;
      Input.compositionCursorPos = zero;
    }

    private void CreateCursorVerts()
    {
      this.m_CursorVerts = new UIVertex[4];
      for (int index = 0; index < this.m_CursorVerts.Length; ++index)
      {
        this.m_CursorVerts[index] = UIVertex.simpleVert;
        this.m_CursorVerts[index].uv0 = Vector2.zero;
      }
    }

    private void GenerateHightlight(VertexHelper vbo, Vector2 roundingOffset)
    {
      int charPos = Mathf.Max(0, this.caretPositionInternal - this.m_DrawStart);
      int num1 = Mathf.Max(0, this.caretSelectPositionInternal - this.m_DrawStart);
      if (charPos > num1)
      {
        int num2 = charPos;
        charPos = num1;
        num1 = num2;
      }
      int num3 = num1 - 1;
      TextGenerator cachedTextGenerator = this.m_TextComponent.cachedTextGenerator;
      if (cachedTextGenerator.lineCount <= 0)
        return;
      int characterLine = this.DetermineCharacterLine(charPos, cachedTextGenerator);
      int lineEndPosition = InputField.GetLineEndPosition(cachedTextGenerator, characterLine);
      UIVertex simpleVert = UIVertex.simpleVert;
      simpleVert.uv0 = Vector2.zero;
      simpleVert.color = (Color32) this.selectionColor;
      for (int index = charPos; index <= num3 && index < cachedTextGenerator.characterCount; ++index)
      {
        if (index == lineEndPosition || index == num3)
        {
          UICharInfo character1 = cachedTextGenerator.characters[charPos];
          UICharInfo character2 = cachedTextGenerator.characters[index];
          Vector2 vector2_1 = new Vector2(character1.cursorPos.x / this.m_TextComponent.pixelsPerUnit, cachedTextGenerator.lines[characterLine].topY / this.m_TextComponent.pixelsPerUnit);
          Vector2 vector2_2 = new Vector2((character2.cursorPos.x + character2.charWidth) / this.m_TextComponent.pixelsPerUnit, vector2_1.y - (float) cachedTextGenerator.lines[characterLine].height / this.m_TextComponent.pixelsPerUnit);
          if ((double) vector2_2.x > (double) this.m_TextComponent.rectTransform.rect.xMax || (double) vector2_2.x < (double) this.m_TextComponent.rectTransform.rect.xMin)
            vector2_2.x = this.m_TextComponent.rectTransform.rect.xMax;
          int currentVertCount = vbo.currentVertCount;
          simpleVert.position = new Vector3(vector2_1.x, vector2_2.y, 0.0f) + (Vector3) roundingOffset;
          vbo.AddVert(simpleVert);
          simpleVert.position = new Vector3(vector2_2.x, vector2_2.y, 0.0f) + (Vector3) roundingOffset;
          vbo.AddVert(simpleVert);
          simpleVert.position = new Vector3(vector2_2.x, vector2_1.y, 0.0f) + (Vector3) roundingOffset;
          vbo.AddVert(simpleVert);
          simpleVert.position = new Vector3(vector2_1.x, vector2_1.y, 0.0f) + (Vector3) roundingOffset;
          vbo.AddVert(simpleVert);
          vbo.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
          vbo.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
          charPos = index + 1;
          ++characterLine;
          lineEndPosition = InputField.GetLineEndPosition(cachedTextGenerator, characterLine);
        }
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
      if (this.characterValidation == InputField.CharacterValidation.None || !this.enabled)
        return ch;
      if (this.characterValidation == InputField.CharacterValidation.Integer || this.characterValidation == InputField.CharacterValidation.Decimal)
      {
        bool flag1 = pos == 0 && text.Length > 0 && (int) text[0] == 45;
        bool flag2 = this.caretPositionInternal == 0 || this.caretSelectPositionInternal == 0;
        if (!flag1 && ((int) ch >= 48 && (int) ch <= 57 || (int) ch == 45 && (pos == 0 || flag2) || (int) ch == 46 && this.characterValidation == InputField.CharacterValidation.Decimal && !text.Contains(".")))
          return ch;
      }
      else if (this.characterValidation == InputField.CharacterValidation.Alphanumeric)
      {
        if ((int) ch >= 65 && (int) ch <= 90 || (int) ch >= 97 && (int) ch <= 122 || (int) ch >= 48 && (int) ch <= 57)
          return ch;
      }
      else if (this.characterValidation == InputField.CharacterValidation.Name)
      {
        char ch1 = text.Length <= 0 ? ' ' : text[Mathf.Clamp(pos, 0, text.Length - 1)];
        char ch2 = text.Length <= 0 ? '\n' : text[Mathf.Clamp(pos + 1, 0, text.Length - 1)];
        if (char.IsLetter(ch))
        {
          if (char.IsLower(ch) && (int) ch1 == 32)
            return char.ToUpper(ch);
          if (char.IsUpper(ch) && (int) ch1 != 32 && (int) ch1 != 39)
            return char.ToLower(ch);
          return ch;
        }
        if ((int) ch == 39)
        {
          if ((int) ch1 != 32 && (int) ch1 != 39 && ((int) ch2 != 39 && !text.Contains("'")))
            return ch;
        }
        else if ((int) ch == 32 && (int) ch1 != 32 && ((int) ch1 != 39 && (int) ch2 != 32) && (int) ch2 != 39)
          return ch;
      }
      else if (this.characterValidation == InputField.CharacterValidation.EmailAddress && ((int) ch >= 65 && (int) ch <= 90 || (int) ch >= 97 && (int) ch <= 122 || ((int) ch >= 48 && (int) ch <= 57 || (int) ch == 64 && text.IndexOf('@') == -1) || "!#$%&'*+-/=?^_`{|}~".IndexOf(ch) != -1 || (int) ch == 46 && ((text.Length <= 0 ? (int) ' ' : (int) text[Mathf.Clamp(pos, 0, text.Length - 1)]) != 46 && (text.Length <= 0 ? (int) '\n' : (int) text[Mathf.Clamp(pos + 1, 0, text.Length - 1)]) != 46)))
        return ch;
      return char.MinValue;
    }

    /// <summary>
    ///   <para>Function to activate the InputField to begin processing Events.</para>
    /// </summary>
    public void ActivateInputField()
    {
      if ((UnityEngine.Object) this.m_TextComponent == (UnityEngine.Object) null || (UnityEngine.Object) this.m_TextComponent.font == (UnityEngine.Object) null || (!this.IsActive() || !this.IsInteractable()))
        return;
      if (this.isFocused && this.m_Keyboard != null && !this.m_Keyboard.active)
      {
        this.m_Keyboard.active = true;
        this.m_Keyboard.text = this.m_Text;
      }
      this.m_ShouldActivateNextUpdate = true;
    }

    private void ActivateInputFieldInternal()
    {
      if ((UnityEngine.Object) EventSystem.current == (UnityEngine.Object) null)
        return;
      if ((UnityEngine.Object) EventSystem.current.currentSelectedGameObject != (UnityEngine.Object) this.gameObject)
        EventSystem.current.SetSelectedGameObject(this.gameObject);
      if (TouchScreenKeyboard.isSupported)
      {
        if (Input.touchSupported)
          TouchScreenKeyboard.hideInput = this.shouldHideMobileInput;
        this.m_Keyboard = this.inputType != InputField.InputType.Password ? TouchScreenKeyboard.Open(this.m_Text, this.keyboardType, this.inputType == InputField.InputType.AutoCorrect, this.multiLine) : TouchScreenKeyboard.Open(this.m_Text, this.keyboardType, false, this.multiLine, true);
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
      if (!this.shouldActivateOnSelect)
        return;
      this.ActivateInputField();
    }

    /// <summary>
    ///   <para>What to do when the event system sends a pointer click Event.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.ActivateInputField();
    }

    /// <summary>
    ///   <para>Function to deactivate the InputField to stop the processing of Events and send OnSubmit if not canceled.</para>
    /// </summary>
    public void DeactivateInputField()
    {
      if (!this.m_AllowInput)
        return;
      this.m_HasDoneFocusTransition = false;
      this.m_AllowInput = false;
      if ((UnityEngine.Object) this.m_Placeholder != (UnityEngine.Object) null)
        this.m_Placeholder.enabled = string.IsNullOrEmpty(this.m_Text);
      if ((UnityEngine.Object) this.m_TextComponent != (UnityEngine.Object) null && this.IsInteractable())
      {
        if (this.m_WasCanceled)
          this.text = this.m_OriginalText;
        if (this.m_Keyboard != null)
        {
          this.m_Keyboard.active = false;
          this.m_Keyboard = (TouchScreenKeyboard) null;
        }
        this.m_CaretPosition = this.m_CaretSelectPosition = 0;
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
      if (!this.IsActive() || !this.IsInteractable() || this.isFocused)
        return;
      this.m_ShouldActivateNextUpdate = true;
    }

    private void EnforceContentType()
    {
      switch (this.contentType)
      {
        case InputField.ContentType.Standard:
          this.m_InputType = InputField.InputType.Standard;
          this.m_KeyboardType = TouchScreenKeyboardType.Default;
          this.m_CharacterValidation = InputField.CharacterValidation.None;
          break;
        case InputField.ContentType.Autocorrected:
          this.m_InputType = InputField.InputType.AutoCorrect;
          this.m_KeyboardType = TouchScreenKeyboardType.Default;
          this.m_CharacterValidation = InputField.CharacterValidation.None;
          break;
        case InputField.ContentType.IntegerNumber:
          this.m_LineType = InputField.LineType.SingleLine;
          this.m_InputType = InputField.InputType.Standard;
          this.m_KeyboardType = TouchScreenKeyboardType.NumberPad;
          this.m_CharacterValidation = InputField.CharacterValidation.Integer;
          break;
        case InputField.ContentType.DecimalNumber:
          this.m_LineType = InputField.LineType.SingleLine;
          this.m_InputType = InputField.InputType.Standard;
          this.m_KeyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
          this.m_CharacterValidation = InputField.CharacterValidation.Decimal;
          break;
        case InputField.ContentType.Alphanumeric:
          this.m_LineType = InputField.LineType.SingleLine;
          this.m_InputType = InputField.InputType.Standard;
          this.m_KeyboardType = TouchScreenKeyboardType.ASCIICapable;
          this.m_CharacterValidation = InputField.CharacterValidation.Alphanumeric;
          break;
        case InputField.ContentType.Name:
          this.m_LineType = InputField.LineType.SingleLine;
          this.m_InputType = InputField.InputType.Standard;
          this.m_KeyboardType = TouchScreenKeyboardType.Default;
          this.m_CharacterValidation = InputField.CharacterValidation.Name;
          break;
        case InputField.ContentType.EmailAddress:
          this.m_LineType = InputField.LineType.SingleLine;
          this.m_InputType = InputField.InputType.Standard;
          this.m_KeyboardType = TouchScreenKeyboardType.EmailAddress;
          this.m_CharacterValidation = InputField.CharacterValidation.EmailAddress;
          break;
        case InputField.ContentType.Password:
          this.m_LineType = InputField.LineType.SingleLine;
          this.m_InputType = InputField.InputType.Password;
          this.m_KeyboardType = TouchScreenKeyboardType.Default;
          this.m_CharacterValidation = InputField.CharacterValidation.None;
          break;
        case InputField.ContentType.Pin:
          this.m_LineType = InputField.LineType.SingleLine;
          this.m_InputType = InputField.InputType.Password;
          this.m_KeyboardType = TouchScreenKeyboardType.NumberPad;
          this.m_CharacterValidation = InputField.CharacterValidation.Integer;
          break;
      }
    }

    private void SetToCustomIfContentTypeIsNot(params InputField.ContentType[] allowedContentTypes)
    {
      if (this.contentType == InputField.ContentType.Custom)
        return;
      for (int index = 0; index < allowedContentTypes.Length; ++index)
      {
        if (this.contentType == allowedContentTypes[index])
          return;
      }
      this.contentType = InputField.ContentType.Custom;
    }

    private void SetToCustom()
    {
      if (this.contentType == InputField.ContentType.Custom)
        return;
      this.contentType = InputField.ContentType.Custom;
    }

    protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
    {
      if (this.m_HasDoneFocusTransition)
        state = Selectable.SelectionState.Highlighted;
      else if (state == Selectable.SelectionState.Pressed)
        this.m_HasDoneFocusTransition = true;
      base.DoStateTransition(state, instant);
    }

    bool ICanvasElement.IsDestroyed()
    {
      return this.IsDestroyed();
    }

    Transform ICanvasElement.get_transform()
    {
      return this.transform;
    }

    /// <summary>
    ///   <para>Specifies the type of the input text content.</para>
    /// </summary>
    public enum ContentType
    {
      Standard,
      Autocorrected,
      IntegerNumber,
      DecimalNumber,
      Alphanumeric,
      Name,
      EmailAddress,
      Password,
      Pin,
      Custom,
    }

    /// <summary>
    ///   <para>Type of data expected by the input field.</para>
    /// </summary>
    public enum InputType
    {
      Standard,
      AutoCorrect,
      Password,
    }

    /// <summary>
    ///   <para>The type of characters that are allowed to be added to the string.</para>
    /// </summary>
    public enum CharacterValidation
    {
      None,
      Integer,
      Decimal,
      Alphanumeric,
      Name,
      EmailAddress,
    }

    /// <summary>
    ///   <para>The LineType is used to describe the behavior of the InputField.</para>
    /// </summary>
    public enum LineType
    {
      SingleLine,
      MultiLineSubmit,
      MultiLineNewline,
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
      Finish,
    }

    /// <summary>
    ///   <para>Custom validation callback.</para>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="charIndex"></param>
    /// <param name="addedChar"></param>
    public delegate char OnValidateInput(string text, int charIndex, char addedChar);
  }
}

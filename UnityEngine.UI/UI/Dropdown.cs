// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Dropdown
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI.CoroutineTween;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>A standard dropdown that presents a list of options when clicked, of which one can be chosen.</para>
  /// </summary>
  [RequireComponent(typeof (RectTransform))]
  [AddComponentMenu("UI/Dropdown", 35)]
  public class Dropdown : Selectable, IEventSystemHandler, IPointerClickHandler, ISubmitHandler, ICancelHandler
  {
    private static Dropdown.OptionData s_NoOptionData = new Dropdown.OptionData();
    [Space]
    [SerializeField]
    private Dropdown.OptionDataList m_Options = new Dropdown.OptionDataList();
    [Space]
    [SerializeField]
    private Dropdown.DropdownEvent m_OnValueChanged = new Dropdown.DropdownEvent();
    private List<Dropdown.DropdownItem> m_Items = new List<Dropdown.DropdownItem>();
    [SerializeField]
    private RectTransform m_Template;
    [SerializeField]
    private Text m_CaptionText;
    [SerializeField]
    private Image m_CaptionImage;
    [SerializeField]
    [Space]
    private Text m_ItemText;
    [SerializeField]
    private Image m_ItemImage;
    [SerializeField]
    [Space]
    private int m_Value;
    private GameObject m_Dropdown;
    private GameObject m_Blocker;
    private TweenRunner<FloatTween> m_AlphaTweenRunner;
    private bool validTemplate;

    /// <summary>
    ///   <para>The Rect Transform of the template for the dropdown list.</para>
    /// </summary>
    public RectTransform template
    {
      get
      {
        return this.m_Template;
      }
      set
      {
        this.m_Template = value;
        this.RefreshShownValue();
      }
    }

    /// <summary>
    ///   <para>The Text component to hold the text of the currently selected option.</para>
    /// </summary>
    public Text captionText
    {
      get
      {
        return this.m_CaptionText;
      }
      set
      {
        this.m_CaptionText = value;
        this.RefreshShownValue();
      }
    }

    /// <summary>
    ///   <para>The Image component to hold the image of the currently selected option.</para>
    /// </summary>
    public Image captionImage
    {
      get
      {
        return this.m_CaptionImage;
      }
      set
      {
        this.m_CaptionImage = value;
        this.RefreshShownValue();
      }
    }

    /// <summary>
    ///   <para>The Text component to hold the text of the item.</para>
    /// </summary>
    public Text itemText
    {
      get
      {
        return this.m_ItemText;
      }
      set
      {
        this.m_ItemText = value;
        this.RefreshShownValue();
      }
    }

    /// <summary>
    ///   <para>The Image component to hold the image of the item.</para>
    /// </summary>
    public Image itemImage
    {
      get
      {
        return this.m_ItemImage;
      }
      set
      {
        this.m_ItemImage = value;
        this.RefreshShownValue();
      }
    }

    /// <summary>
    ///   <para>The list of possible options. A text string and an image can be specified for each option.</para>
    /// </summary>
    public List<Dropdown.OptionData> options
    {
      get
      {
        return this.m_Options.options;
      }
      set
      {
        this.m_Options.options = value;
        this.RefreshShownValue();
      }
    }

    /// <summary>
    ///   <para>A UnityEvent that is invoked when when a user has clicked one of the options in the dropdown list.</para>
    /// </summary>
    public Dropdown.DropdownEvent onValueChanged
    {
      get
      {
        return this.m_OnValueChanged;
      }
      set
      {
        this.m_OnValueChanged = value;
      }
    }

    /// <summary>
    ///   <para>The index of the currently selected option. 0 is the first option, 1 is the second, and so on.</para>
    /// </summary>
    public int value
    {
      get
      {
        return this.m_Value;
      }
      set
      {
        if (Application.isPlaying && (value == this.m_Value || this.options.Count == 0))
          return;
        this.m_Value = Mathf.Clamp(value, 0, this.options.Count - 1);
        this.RefreshShownValue();
        this.m_OnValueChanged.Invoke(this.m_Value);
      }
    }

    protected Dropdown()
    {
    }

    protected override void Awake()
    {
      if (!Application.isPlaying)
        return;
      this.m_AlphaTweenRunner = new TweenRunner<FloatTween>();
      this.m_AlphaTweenRunner.Init((MonoBehaviour) this);
      if ((bool) ((UnityEngine.Object) this.m_CaptionImage))
        this.m_CaptionImage.enabled = (UnityEngine.Object) this.m_CaptionImage.sprite != (UnityEngine.Object) null;
      if (!(bool) ((UnityEngine.Object) this.m_Template))
        return;
      this.m_Template.gameObject.SetActive(false);
    }

    protected override void OnValidate()
    {
      base.OnValidate();
      if (!this.IsActive())
        return;
      this.RefreshShownValue();
    }

    /// <summary>
    ///         <para>Refreshes the text and image (if available) of the currently selected option.
    /// 
    /// If you have modified the list of options, you should call this method afterwards to ensure that the visual state of the dropdown corresponds to the updated options.</para>
    ///       </summary>
    public void RefreshShownValue()
    {
      Dropdown.OptionData optionData = Dropdown.s_NoOptionData;
      if (this.options.Count > 0)
        optionData = this.options[Mathf.Clamp(this.m_Value, 0, this.options.Count - 1)];
      if ((bool) ((UnityEngine.Object) this.m_CaptionText))
        this.m_CaptionText.text = optionData == null || optionData.text == null ? string.Empty : optionData.text;
      if (!(bool) ((UnityEngine.Object) this.m_CaptionImage))
        return;
      this.m_CaptionImage.sprite = optionData == null ? (Sprite) null : optionData.image;
      this.m_CaptionImage.enabled = (UnityEngine.Object) this.m_CaptionImage.sprite != (UnityEngine.Object) null;
    }

    public void AddOptions(List<Dropdown.OptionData> options)
    {
      this.options.AddRange((IEnumerable<Dropdown.OptionData>) options);
      this.RefreshShownValue();
    }

    public void AddOptions(List<string> options)
    {
      for (int index = 0; index < options.Count; ++index)
        this.options.Add(new Dropdown.OptionData(options[index]));
      this.RefreshShownValue();
    }

    public void AddOptions(List<Sprite> options)
    {
      for (int index = 0; index < options.Count; ++index)
        this.options.Add(new Dropdown.OptionData(options[index]));
      this.RefreshShownValue();
    }

    /// <summary>
    ///   <para>Clear the list of options in the Dropdown.</para>
    /// </summary>
    public void ClearOptions()
    {
      this.options.Clear();
      this.RefreshShownValue();
    }

    private void SetupTemplate()
    {
      this.validTemplate = false;
      if (!(bool) ((UnityEngine.Object) this.m_Template))
      {
        UnityEngine.Debug.LogError((object) "The dropdown template is not assigned. The template needs to be assigned and must have a child GameObject with a Toggle component serving as the item.", (UnityEngine.Object) this);
      }
      else
      {
        GameObject gameObject = this.m_Template.gameObject;
        gameObject.SetActive(true);
        Toggle componentInChildren = this.m_Template.GetComponentInChildren<Toggle>();
        this.validTemplate = true;
        if (!(bool) ((UnityEngine.Object) componentInChildren) || (UnityEngine.Object) componentInChildren.transform == (UnityEngine.Object) this.template)
        {
          this.validTemplate = false;
          UnityEngine.Debug.LogError((object) "The dropdown template is not valid. The template must have a child GameObject with a Toggle component serving as the item.", (UnityEngine.Object) this.template);
        }
        else if (!(componentInChildren.transform.parent is RectTransform))
        {
          this.validTemplate = false;
          UnityEngine.Debug.LogError((object) "The dropdown template is not valid. The child GameObject with a Toggle component (the item) must have a RectTransform on its parent.", (UnityEngine.Object) this.template);
        }
        else if ((UnityEngine.Object) this.itemText != (UnityEngine.Object) null && !this.itemText.transform.IsChildOf(componentInChildren.transform))
        {
          this.validTemplate = false;
          UnityEngine.Debug.LogError((object) "The dropdown template is not valid. The Item Text must be on the item GameObject or children of it.", (UnityEngine.Object) this.template);
        }
        else if ((UnityEngine.Object) this.itemImage != (UnityEngine.Object) null && !this.itemImage.transform.IsChildOf(componentInChildren.transform))
        {
          this.validTemplate = false;
          UnityEngine.Debug.LogError((object) "The dropdown template is not valid. The Item Image must be on the item GameObject or children of it.", (UnityEngine.Object) this.template);
        }
        if (!this.validTemplate)
        {
          gameObject.SetActive(false);
        }
        else
        {
          Dropdown.DropdownItem dropdownItem = componentInChildren.gameObject.AddComponent<Dropdown.DropdownItem>();
          dropdownItem.text = this.m_ItemText;
          dropdownItem.image = this.m_ItemImage;
          dropdownItem.toggle = componentInChildren;
          dropdownItem.rectTransform = (RectTransform) componentInChildren.transform;
          Canvas orAddComponent = Dropdown.GetOrAddComponent<Canvas>(gameObject);
          orAddComponent.overrideSorting = true;
          orAddComponent.sortingOrder = 30000;
          Dropdown.GetOrAddComponent<GraphicRaycaster>(gameObject);
          Dropdown.GetOrAddComponent<CanvasGroup>(gameObject);
          gameObject.SetActive(false);
          this.validTemplate = true;
        }
      }
    }

    private static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
      T obj = go.GetComponent<T>();
      if (!(bool) ((UnityEngine.Object) obj))
        obj = go.AddComponent<T>();
      return obj;
    }

    /// <summary>
    ///   <para>Handling for when the dropdown is 'clicked'.</para>
    /// </summary>
    /// <param name="eventData">Current event.</param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
      this.Show();
    }

    /// <summary>
    ///   <para>What to do when the event system sends a submit Event.</para>
    /// </summary>
    /// <param name="eventData">Current event.</param>
    public virtual void OnSubmit(BaseEventData eventData)
    {
      this.Show();
    }

    /// <summary>
    ///   <para>Called by a BaseInputModule when a Cancel event occurs.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnCancel(BaseEventData eventData)
    {
      this.Hide();
    }

    /// <summary>
    ///   <para>Show the dropdown list.</para>
    /// </summary>
    public void Show()
    {
      if (!this.IsActive() || !this.IsInteractable() || (UnityEngine.Object) this.m_Dropdown != (UnityEngine.Object) null)
        return;
      if (!this.validTemplate)
      {
        this.SetupTemplate();
        if (!this.validTemplate)
          return;
      }
      List<Canvas> canvasList = ListPool<Canvas>.Get();
      this.gameObject.GetComponentsInParent<Canvas>(false, canvasList);
      if (canvasList.Count == 0)
        return;
      Canvas rootCanvas = canvasList[0];
      ListPool<Canvas>.Release(canvasList);
      this.m_Template.gameObject.SetActive(true);
      this.m_Dropdown = this.CreateDropdownList(this.m_Template.gameObject);
      this.m_Dropdown.name = "Dropdown List";
      this.m_Dropdown.SetActive(true);
      RectTransform transform1 = this.m_Dropdown.transform as RectTransform;
      transform1.SetParent(this.m_Template.transform.parent, false);
      Dropdown.DropdownItem componentInChildren = this.m_Dropdown.GetComponentInChildren<Dropdown.DropdownItem>();
      RectTransform transform2 = componentInChildren.rectTransform.parent.gameObject.transform as RectTransform;
      componentInChildren.rectTransform.gameObject.SetActive(true);
      Rect rect1 = transform2.rect;
      Rect rect2 = componentInChildren.rectTransform.rect;
      Vector2 vector2_1 = rect2.min - rect1.min + (Vector2) componentInChildren.rectTransform.localPosition;
      Vector2 vector2_2 = rect2.max - rect1.max + (Vector2) componentInChildren.rectTransform.localPosition;
      Vector2 size = rect2.size;
      this.m_Items.Clear();
      Toggle toggle = (Toggle) null;
      for (int index = 0; index < this.options.Count; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Dropdown.\u003CShow\u003Ec__AnonStorey6 showCAnonStorey6 = new Dropdown.\u003CShow\u003Ec__AnonStorey6();
        // ISSUE: reference to a compiler-generated field
        showCAnonStorey6.\u003C\u003Ef__this = this;
        Dropdown.OptionData option = this.options[index];
        // ISSUE: reference to a compiler-generated field
        showCAnonStorey6.item = this.AddItem(option, this.value == index, componentInChildren, this.m_Items);
        // ISSUE: reference to a compiler-generated field
        if (!((UnityEngine.Object) showCAnonStorey6.item == (UnityEngine.Object) null))
        {
          // ISSUE: reference to a compiler-generated field
          showCAnonStorey6.item.toggle.isOn = this.value == index;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          showCAnonStorey6.item.toggle.onValueChanged.AddListener(new UnityAction<bool>(showCAnonStorey6.\u003C\u003Em__2));
          // ISSUE: reference to a compiler-generated field
          if (showCAnonStorey6.item.toggle.isOn)
          {
            // ISSUE: reference to a compiler-generated field
            showCAnonStorey6.item.toggle.Select();
          }
          if ((UnityEngine.Object) toggle != (UnityEngine.Object) null)
          {
            Navigation navigation1 = toggle.navigation;
            // ISSUE: reference to a compiler-generated field
            Navigation navigation2 = showCAnonStorey6.item.toggle.navigation;
            navigation1.mode = Navigation.Mode.Explicit;
            navigation2.mode = Navigation.Mode.Explicit;
            // ISSUE: reference to a compiler-generated field
            navigation1.selectOnDown = (Selectable) showCAnonStorey6.item.toggle;
            // ISSUE: reference to a compiler-generated field
            navigation1.selectOnRight = (Selectable) showCAnonStorey6.item.toggle;
            navigation2.selectOnLeft = (Selectable) toggle;
            navigation2.selectOnUp = (Selectable) toggle;
            toggle.navigation = navigation1;
            // ISSUE: reference to a compiler-generated field
            showCAnonStorey6.item.toggle.navigation = navigation2;
          }
          // ISSUE: reference to a compiler-generated field
          toggle = showCAnonStorey6.item.toggle;
        }
      }
      Vector2 sizeDelta = transform2.sizeDelta;
      sizeDelta.y = size.y * (float) this.m_Items.Count + vector2_1.y - vector2_2.y;
      transform2.sizeDelta = sizeDelta;
      float num = transform1.rect.height - transform2.rect.height;
      if ((double) num > 0.0)
        transform1.sizeDelta = new Vector2(transform1.sizeDelta.x, transform1.sizeDelta.y - num);
      Vector3[] fourCornersArray = new Vector3[4];
      transform1.GetWorldCorners(fourCornersArray);
      RectTransform transform3 = rootCanvas.transform as RectTransform;
      Rect rect3 = transform3.rect;
      for (int axis = 0; axis < 2; ++axis)
      {
        bool flag = false;
        for (int index = 0; index < 4; ++index)
        {
          Vector3 vector3 = transform3.InverseTransformPoint(fourCornersArray[index]);
          if ((double) vector3[axis] < (double) rect3.min[axis] || (double) vector3[axis] > (double) rect3.max[axis])
          {
            flag = true;
            break;
          }
        }
        if (flag)
          RectTransformUtility.FlipLayoutOnAxis(transform1, axis, false, false);
      }
      for (int index = 0; index < this.m_Items.Count; ++index)
      {
        RectTransform rectTransform = this.m_Items[index].rectTransform;
        rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 0.0f);
        rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, 0.0f);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, (float) ((double) vector2_1.y + (double) size.y * (double) (this.m_Items.Count - 1 - index) + (double) size.y * (double) rectTransform.pivot.y));
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, size.y);
      }
      this.AlphaFadeList(0.15f, 0.0f, 1f);
      this.m_Template.gameObject.SetActive(false);
      componentInChildren.gameObject.SetActive(false);
      this.m_Blocker = this.CreateBlocker(rootCanvas);
    }

    /// <summary>
    ///   <para>Override this method to implement a different way to obtain a blocker GameObject.</para>
    /// </summary>
    /// <param name="rootCanvas">The root canvas the dropdown is under.</param>
    /// <returns>
    ///   <para>The obtained blocker.</para>
    /// </returns>
    protected virtual GameObject CreateBlocker(Canvas rootCanvas)
    {
      GameObject gameObject = new GameObject("Blocker");
      RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
      rectTransform.SetParent(rootCanvas.transform, false);
      rectTransform.anchorMin = (Vector2) Vector3.zero;
      rectTransform.anchorMax = (Vector2) Vector3.one;
      rectTransform.sizeDelta = Vector2.zero;
      Canvas canvas = gameObject.AddComponent<Canvas>();
      canvas.overrideSorting = true;
      Canvas component = this.m_Dropdown.GetComponent<Canvas>();
      canvas.sortingLayerID = component.sortingLayerID;
      canvas.sortingOrder = component.sortingOrder - 1;
      gameObject.AddComponent<GraphicRaycaster>();
      gameObject.AddComponent<Image>().color = Color.clear;
      gameObject.AddComponent<Button>().onClick.AddListener(new UnityAction(this.Hide));
      return gameObject;
    }

    /// <summary>
    ///   <para>Override this method to implement a different way to dispose of a blocker GameObject that blocks clicks to other controls while the dropdown list is open.</para>
    /// </summary>
    /// <param name="blocker">The blocker to dispose of.</param>
    protected virtual void DestroyBlocker(GameObject blocker)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) blocker);
    }

    /// <summary>
    ///   <para>Override this method to implement a different way to obtain a dropdown list GameObject.</para>
    /// </summary>
    /// <param name="template">The template to create the dropdown list from.</param>
    /// <returns>
    ///   <para>The obtained dropdown list.</para>
    /// </returns>
    protected virtual GameObject CreateDropdownList(GameObject template)
    {
      return UnityEngine.Object.Instantiate<GameObject>(template);
    }

    /// <summary>
    ///   <para>Override this method to implement a different way to dispose of a dropdown list GameObject.</para>
    /// </summary>
    /// <param name="dropdownList">The dropdown list to dispose of.</param>
    protected virtual void DestroyDropdownList(GameObject dropdownList)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) dropdownList);
    }

    protected virtual Dropdown.DropdownItem CreateItem(Dropdown.DropdownItem itemTemplate)
    {
      return UnityEngine.Object.Instantiate<Dropdown.DropdownItem>(itemTemplate);
    }

    protected virtual void DestroyItem(Dropdown.DropdownItem item)
    {
    }

    private Dropdown.DropdownItem AddItem(Dropdown.OptionData data, bool selected, Dropdown.DropdownItem itemTemplate, List<Dropdown.DropdownItem> items)
    {
      Dropdown.DropdownItem dropdownItem = this.CreateItem(itemTemplate);
      dropdownItem.rectTransform.SetParent(itemTemplate.rectTransform.parent, false);
      dropdownItem.gameObject.SetActive(true);
      dropdownItem.gameObject.name = "Item " + (object) items.Count + (data.text == null ? (object) string.Empty : (object) (": " + data.text));
      if ((UnityEngine.Object) dropdownItem.toggle != (UnityEngine.Object) null)
        dropdownItem.toggle.isOn = false;
      if ((bool) ((UnityEngine.Object) dropdownItem.text))
        dropdownItem.text.text = data.text;
      if ((bool) ((UnityEngine.Object) dropdownItem.image))
      {
        dropdownItem.image.sprite = data.image;
        dropdownItem.image.enabled = (UnityEngine.Object) dropdownItem.image.sprite != (UnityEngine.Object) null;
      }
      items.Add(dropdownItem);
      return dropdownItem;
    }

    private void AlphaFadeList(float duration, float alpha)
    {
      CanvasGroup component = this.m_Dropdown.GetComponent<CanvasGroup>();
      this.AlphaFadeList(duration, component.alpha, alpha);
    }

    private void AlphaFadeList(float duration, float start, float end)
    {
      if (end.Equals(start))
        return;
      FloatTween info = new FloatTween() { duration = duration, startValue = start, targetValue = end };
      info.AddOnChangedCallback(new UnityAction<float>(this.SetAlpha));
      info.ignoreTimeScale = true;
      this.m_AlphaTweenRunner.StartTween(info);
    }

    private void SetAlpha(float alpha)
    {
      if (!(bool) ((UnityEngine.Object) this.m_Dropdown))
        return;
      this.m_Dropdown.GetComponent<CanvasGroup>().alpha = alpha;
    }

    /// <summary>
    ///   <para>Hide the dropdown list.</para>
    /// </summary>
    public void Hide()
    {
      if ((UnityEngine.Object) this.m_Dropdown != (UnityEngine.Object) null)
      {
        this.AlphaFadeList(0.15f, 0.0f);
        if (this.IsActive())
          this.StartCoroutine(this.DelayedDestroyDropdownList(0.15f));
      }
      if ((UnityEngine.Object) this.m_Blocker != (UnityEngine.Object) null)
        this.DestroyBlocker(this.m_Blocker);
      this.m_Blocker = (GameObject) null;
      this.Select();
    }

    [DebuggerHidden]
    private IEnumerator DelayedDestroyDropdownList(float delay)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Dropdown.\u003CDelayedDestroyDropdownList\u003Ec__Iterator2() { delay = delay, \u003C\u0024\u003Edelay = delay, \u003C\u003Ef__this = this };
    }

    private void OnSelectItem(Toggle toggle)
    {
      if (!toggle.isOn)
        toggle.isOn = true;
      int num = -1;
      Transform transform = toggle.transform;
      Transform parent = transform.parent;
      for (int index = 0; index < parent.childCount; ++index)
      {
        if ((UnityEngine.Object) parent.GetChild(index) == (UnityEngine.Object) transform)
        {
          num = index - 1;
          break;
        }
      }
      if (num < 0)
        return;
      this.value = num;
      this.Hide();
    }

    protected internal class DropdownItem : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, ICancelHandler
    {
      [SerializeField]
      private Text m_Text;
      [SerializeField]
      private Image m_Image;
      [SerializeField]
      private RectTransform m_RectTransform;
      [SerializeField]
      private Toggle m_Toggle;

      public Text text
      {
        get
        {
          return this.m_Text;
        }
        set
        {
          this.m_Text = value;
        }
      }

      public Image image
      {
        get
        {
          return this.m_Image;
        }
        set
        {
          this.m_Image = value;
        }
      }

      public RectTransform rectTransform
      {
        get
        {
          return this.m_RectTransform;
        }
        set
        {
          this.m_RectTransform = value;
        }
      }

      public Toggle toggle
      {
        get
        {
          return this.m_Toggle;
        }
        set
        {
          this.m_Toggle = value;
        }
      }

      public virtual void OnPointerEnter(PointerEventData eventData)
      {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
      }

      public virtual void OnCancel(BaseEventData eventData)
      {
        Dropdown componentInParent = this.GetComponentInParent<Dropdown>();
        if (!(bool) ((UnityEngine.Object) componentInParent))
          return;
        componentInParent.Hide();
      }
    }

    /// <summary>
    ///   <para>Class to store the text and/or image of a single option in the dropdown list.</para>
    /// </summary>
    [Serializable]
    public class OptionData
    {
      [SerializeField]
      private string m_Text;
      [SerializeField]
      private Sprite m_Image;

      /// <summary>
      ///   <para>The text associated with the option.</para>
      /// </summary>
      public string text
      {
        get
        {
          return this.m_Text;
        }
        set
        {
          this.m_Text = value;
        }
      }

      /// <summary>
      ///   <para>The image associated with the option.</para>
      /// </summary>
      public Sprite image
      {
        get
        {
          return this.m_Image;
        }
        set
        {
          this.m_Image = value;
        }
      }

      /// <summary>
      ///   <para>Create an object representing a single option for the dropdown list.</para>
      /// </summary>
      /// <param name="text">Optional text for the option.</param>
      /// <param name="image">Optional image for the option.</param>
      public OptionData()
      {
      }

      /// <summary>
      ///   <para>Create an object representing a single option for the dropdown list.</para>
      /// </summary>
      /// <param name="text">Optional text for the option.</param>
      /// <param name="image">Optional image for the option.</param>
      public OptionData(string text)
      {
        this.text = text;
      }

      /// <summary>
      ///   <para>Create an object representing a single option for the dropdown list.</para>
      /// </summary>
      /// <param name="text">Optional text for the option.</param>
      /// <param name="image">Optional image for the option.</param>
      public OptionData(Sprite image)
      {
        this.image = image;
      }

      /// <summary>
      ///   <para>Create an object representing a single option for the dropdown list.</para>
      /// </summary>
      /// <param name="text">Optional text for the option.</param>
      /// <param name="image">Optional image for the option.</param>
      public OptionData(string text, Sprite image)
      {
        this.text = text;
        this.image = image;
      }
    }

    /// <summary>
    ///   <para>Class used internally to store the list of options for the dropdown list.</para>
    /// </summary>
    [Serializable]
    public class OptionDataList
    {
      [SerializeField]
      private List<Dropdown.OptionData> m_Options;

      /// <summary>
      ///   <para>The list of options for the dropdown list.</para>
      /// </summary>
      public List<Dropdown.OptionData> options
      {
        get
        {
          return this.m_Options;
        }
        set
        {
          this.m_Options = value;
        }
      }

      public OptionDataList()
      {
        this.options = new List<Dropdown.OptionData>();
      }
    }

    /// <summary>
    ///   <para>UnityEvent callback for when a dropdown current option is changed.</para>
    /// </summary>
    [Serializable]
    public class DropdownEvent : UnityEvent<int>
    {
    }
  }
}

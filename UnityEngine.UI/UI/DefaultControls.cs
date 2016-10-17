// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.DefaultControls
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Utility class for creating default implementations of builtin UI controls.</para>
  /// </summary>
  public static class DefaultControls
  {
    private static Vector2 s_ThickElementSize = new Vector2(160f, 30f);
    private static Vector2 s_ThinElementSize = new Vector2(160f, 20f);
    private static Vector2 s_ImageElementSize = new Vector2(100f, 100f);
    private static Color s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
    private static Color s_PanelColor = new Color(1f, 1f, 1f, 0.392f);
    private static Color s_TextColor = new Color(0.1960784f, 0.1960784f, 0.1960784f, 1f);
    private const float kWidth = 160f;
    private const float kThickHeight = 30f;
    private const float kThinHeight = 20f;

    private static GameObject CreateUIElementRoot(string name, Vector2 size)
    {
      GameObject gameObject = new GameObject(name);
      gameObject.AddComponent<RectTransform>().sizeDelta = size;
      return gameObject;
    }

    private static GameObject CreateUIObject(string name, GameObject parent)
    {
      GameObject child = new GameObject(name);
      child.AddComponent<RectTransform>();
      DefaultControls.SetParentAndAlign(child, parent);
      return child;
    }

    private static void SetDefaultTextValues(Text lbl)
    {
      lbl.color = DefaultControls.s_TextColor;
    }

    private static void SetDefaultColorTransitionValues(Selectable slider)
    {
      ColorBlock colors = slider.colors;
      colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
      colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
      colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
    }

    private static void SetParentAndAlign(GameObject child, GameObject parent)
    {
      if ((Object) parent == (Object) null)
        return;
      child.transform.SetParent(parent.transform, false);
      DefaultControls.SetLayerRecursively(child, parent.layer);
    }

    private static void SetLayerRecursively(GameObject go, int layer)
    {
      go.layer = layer;
      Transform transform = go.transform;
      for (int index = 0; index < transform.childCount; ++index)
        DefaultControls.SetLayerRecursively(transform.GetChild(index).gameObject, layer);
    }

    public static GameObject CreatePanel(DefaultControls.Resources resources)
    {
      GameObject uiElementRoot = DefaultControls.CreateUIElementRoot("Panel", DefaultControls.s_ThickElementSize);
      RectTransform component = uiElementRoot.GetComponent<RectTransform>();
      component.anchorMin = Vector2.zero;
      component.anchorMax = Vector2.one;
      component.anchoredPosition = Vector2.zero;
      component.sizeDelta = Vector2.zero;
      Image image = uiElementRoot.AddComponent<Image>();
      image.sprite = resources.background;
      image.type = Image.Type.Sliced;
      image.color = DefaultControls.s_PanelColor;
      return uiElementRoot;
    }

    public static GameObject CreateButton(DefaultControls.Resources resources)
    {
      GameObject uiElementRoot = DefaultControls.CreateUIElementRoot("Button", DefaultControls.s_ThickElementSize);
      GameObject child = new GameObject("Text");
      DefaultControls.SetParentAndAlign(child, uiElementRoot);
      Image image = uiElementRoot.AddComponent<Image>();
      image.sprite = resources.standard;
      image.type = Image.Type.Sliced;
      image.color = DefaultControls.s_DefaultSelectableColor;
      DefaultControls.SetDefaultColorTransitionValues((Selectable) uiElementRoot.AddComponent<Button>());
      Text lbl = child.AddComponent<Text>();
      lbl.text = "Button";
      lbl.alignment = TextAnchor.MiddleCenter;
      DefaultControls.SetDefaultTextValues(lbl);
      RectTransform component = child.GetComponent<RectTransform>();
      component.anchorMin = Vector2.zero;
      component.anchorMax = Vector2.one;
      component.sizeDelta = Vector2.zero;
      return uiElementRoot;
    }

    public static GameObject CreateText(DefaultControls.Resources resources)
    {
      GameObject uiElementRoot = DefaultControls.CreateUIElementRoot("Text", DefaultControls.s_ThickElementSize);
      Text lbl = uiElementRoot.AddComponent<Text>();
      lbl.text = "New Text";
      DefaultControls.SetDefaultTextValues(lbl);
      return uiElementRoot;
    }

    public static GameObject CreateImage(DefaultControls.Resources resources)
    {
      GameObject uiElementRoot = DefaultControls.CreateUIElementRoot("Image", DefaultControls.s_ImageElementSize);
      uiElementRoot.AddComponent<Image>();
      return uiElementRoot;
    }

    public static GameObject CreateRawImage(DefaultControls.Resources resources)
    {
      GameObject uiElementRoot = DefaultControls.CreateUIElementRoot("RawImage", DefaultControls.s_ImageElementSize);
      uiElementRoot.AddComponent<RawImage>();
      return uiElementRoot;
    }

    public static GameObject CreateSlider(DefaultControls.Resources resources)
    {
      GameObject uiElementRoot = DefaultControls.CreateUIElementRoot("Slider", DefaultControls.s_ThinElementSize);
      GameObject uiObject1 = DefaultControls.CreateUIObject("Background", uiElementRoot);
      GameObject uiObject2 = DefaultControls.CreateUIObject("Fill Area", uiElementRoot);
      GameObject uiObject3 = DefaultControls.CreateUIObject("Fill", uiObject2);
      GameObject uiObject4 = DefaultControls.CreateUIObject("Handle Slide Area", uiElementRoot);
      GameObject uiObject5 = DefaultControls.CreateUIObject("Handle", uiObject4);
      Image image1 = uiObject1.AddComponent<Image>();
      image1.sprite = resources.background;
      image1.type = Image.Type.Sliced;
      image1.color = DefaultControls.s_DefaultSelectableColor;
      RectTransform component1 = uiObject1.GetComponent<RectTransform>();
      component1.anchorMin = new Vector2(0.0f, 0.25f);
      component1.anchorMax = new Vector2(1f, 0.75f);
      component1.sizeDelta = new Vector2(0.0f, 0.0f);
      RectTransform component2 = uiObject2.GetComponent<RectTransform>();
      component2.anchorMin = new Vector2(0.0f, 0.25f);
      component2.anchorMax = new Vector2(1f, 0.75f);
      component2.anchoredPosition = new Vector2(-5f, 0.0f);
      component2.sizeDelta = new Vector2(-20f, 0.0f);
      Image image2 = uiObject3.AddComponent<Image>();
      image2.sprite = resources.standard;
      image2.type = Image.Type.Sliced;
      image2.color = DefaultControls.s_DefaultSelectableColor;
      uiObject3.GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 0.0f);
      RectTransform component3 = uiObject4.GetComponent<RectTransform>();
      component3.sizeDelta = new Vector2(-20f, 0.0f);
      component3.anchorMin = new Vector2(0.0f, 0.0f);
      component3.anchorMax = new Vector2(1f, 1f);
      Image image3 = uiObject5.AddComponent<Image>();
      image3.sprite = resources.knob;
      image3.color = DefaultControls.s_DefaultSelectableColor;
      uiObject5.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 0.0f);
      Slider slider = uiElementRoot.AddComponent<Slider>();
      slider.fillRect = uiObject3.GetComponent<RectTransform>();
      slider.handleRect = uiObject5.GetComponent<RectTransform>();
      slider.targetGraphic = (Graphic) image3;
      slider.direction = Slider.Direction.LeftToRight;
      DefaultControls.SetDefaultColorTransitionValues((Selectable) slider);
      return uiElementRoot;
    }

    public static GameObject CreateScrollbar(DefaultControls.Resources resources)
    {
      GameObject uiElementRoot = DefaultControls.CreateUIElementRoot("Scrollbar", DefaultControls.s_ThinElementSize);
      GameObject uiObject1 = DefaultControls.CreateUIObject("Sliding Area", uiElementRoot);
      GameObject uiObject2 = DefaultControls.CreateUIObject("Handle", uiObject1);
      Image image1 = uiElementRoot.AddComponent<Image>();
      image1.sprite = resources.background;
      image1.type = Image.Type.Sliced;
      image1.color = DefaultControls.s_DefaultSelectableColor;
      Image image2 = uiObject2.AddComponent<Image>();
      image2.sprite = resources.standard;
      image2.type = Image.Type.Sliced;
      image2.color = DefaultControls.s_DefaultSelectableColor;
      RectTransform component1 = uiObject1.GetComponent<RectTransform>();
      component1.sizeDelta = new Vector2(-20f, -20f);
      component1.anchorMin = Vector2.zero;
      component1.anchorMax = Vector2.one;
      RectTransform component2 = uiObject2.GetComponent<RectTransform>();
      component2.sizeDelta = new Vector2(20f, 20f);
      Scrollbar scrollbar = uiElementRoot.AddComponent<Scrollbar>();
      scrollbar.handleRect = component2;
      scrollbar.targetGraphic = (Graphic) image2;
      DefaultControls.SetDefaultColorTransitionValues((Selectable) scrollbar);
      return uiElementRoot;
    }

    public static GameObject CreateToggle(DefaultControls.Resources resources)
    {
      GameObject uiElementRoot = DefaultControls.CreateUIElementRoot("Toggle", DefaultControls.s_ThinElementSize);
      GameObject uiObject1 = DefaultControls.CreateUIObject("Background", uiElementRoot);
      GameObject uiObject2 = DefaultControls.CreateUIObject("Checkmark", uiObject1);
      GameObject uiObject3 = DefaultControls.CreateUIObject("Label", uiElementRoot);
      Toggle toggle = uiElementRoot.AddComponent<Toggle>();
      toggle.isOn = true;
      Image image1 = uiObject1.AddComponent<Image>();
      image1.sprite = resources.standard;
      image1.type = Image.Type.Sliced;
      image1.color = DefaultControls.s_DefaultSelectableColor;
      Image image2 = uiObject2.AddComponent<Image>();
      image2.sprite = resources.checkmark;
      Text lbl = uiObject3.AddComponent<Text>();
      lbl.text = "Toggle";
      DefaultControls.SetDefaultTextValues(lbl);
      toggle.graphic = (Graphic) image2;
      toggle.targetGraphic = (Graphic) image1;
      DefaultControls.SetDefaultColorTransitionValues((Selectable) toggle);
      RectTransform component1 = uiObject1.GetComponent<RectTransform>();
      component1.anchorMin = new Vector2(0.0f, 1f);
      component1.anchorMax = new Vector2(0.0f, 1f);
      component1.anchoredPosition = new Vector2(10f, -10f);
      component1.sizeDelta = new Vector2(20f, 20f);
      RectTransform component2 = uiObject2.GetComponent<RectTransform>();
      component2.anchorMin = new Vector2(0.5f, 0.5f);
      component2.anchorMax = new Vector2(0.5f, 0.5f);
      component2.anchoredPosition = Vector2.zero;
      component2.sizeDelta = new Vector2(20f, 20f);
      RectTransform component3 = uiObject3.GetComponent<RectTransform>();
      component3.anchorMin = new Vector2(0.0f, 0.0f);
      component3.anchorMax = new Vector2(1f, 1f);
      component3.offsetMin = new Vector2(23f, 1f);
      component3.offsetMax = new Vector2(-5f, -2f);
      return uiElementRoot;
    }

    public static GameObject CreateInputField(DefaultControls.Resources resources)
    {
      GameObject uiElementRoot = DefaultControls.CreateUIElementRoot("InputField", DefaultControls.s_ThickElementSize);
      GameObject uiObject1 = DefaultControls.CreateUIObject("Placeholder", uiElementRoot);
      GameObject uiObject2 = DefaultControls.CreateUIObject("Text", uiElementRoot);
      Image image = uiElementRoot.AddComponent<Image>();
      image.sprite = resources.inputField;
      image.type = Image.Type.Sliced;
      image.color = DefaultControls.s_DefaultSelectableColor;
      InputField inputField = uiElementRoot.AddComponent<InputField>();
      DefaultControls.SetDefaultColorTransitionValues((Selectable) inputField);
      Text lbl = uiObject2.AddComponent<Text>();
      lbl.text = string.Empty;
      lbl.supportRichText = false;
      DefaultControls.SetDefaultTextValues(lbl);
      Text text = uiObject1.AddComponent<Text>();
      text.text = "Enter text...";
      text.fontStyle = FontStyle.Italic;
      Color color = lbl.color;
      color.a *= 0.5f;
      text.color = color;
      RectTransform component1 = uiObject2.GetComponent<RectTransform>();
      component1.anchorMin = Vector2.zero;
      component1.anchorMax = Vector2.one;
      component1.sizeDelta = Vector2.zero;
      component1.offsetMin = new Vector2(10f, 6f);
      component1.offsetMax = new Vector2(-10f, -7f);
      RectTransform component2 = uiObject1.GetComponent<RectTransform>();
      component2.anchorMin = Vector2.zero;
      component2.anchorMax = Vector2.one;
      component2.sizeDelta = Vector2.zero;
      component2.offsetMin = new Vector2(10f, 6f);
      component2.offsetMax = new Vector2(-10f, -7f);
      inputField.textComponent = lbl;
      inputField.placeholder = (Graphic) text;
      return uiElementRoot;
    }

    public static GameObject CreateDropdown(DefaultControls.Resources resources)
    {
      GameObject uiElementRoot = DefaultControls.CreateUIElementRoot("Dropdown", DefaultControls.s_ThickElementSize);
      GameObject uiObject1 = DefaultControls.CreateUIObject("Label", uiElementRoot);
      GameObject uiObject2 = DefaultControls.CreateUIObject("Arrow", uiElementRoot);
      GameObject uiObject3 = DefaultControls.CreateUIObject("Template", uiElementRoot);
      GameObject uiObject4 = DefaultControls.CreateUIObject("Viewport", uiObject3);
      GameObject uiObject5 = DefaultControls.CreateUIObject("Content", uiObject4);
      GameObject uiObject6 = DefaultControls.CreateUIObject("Item", uiObject5);
      GameObject uiObject7 = DefaultControls.CreateUIObject("Item Background", uiObject6);
      GameObject uiObject8 = DefaultControls.CreateUIObject("Item Checkmark", uiObject6);
      GameObject uiObject9 = DefaultControls.CreateUIObject("Item Label", uiObject6);
      GameObject scrollbar = DefaultControls.CreateScrollbar(resources);
      scrollbar.name = "Scrollbar";
      DefaultControls.SetParentAndAlign(scrollbar, uiObject3);
      Scrollbar component1 = scrollbar.GetComponent<Scrollbar>();
      component1.SetDirection(Scrollbar.Direction.BottomToTop, true);
      RectTransform component2 = scrollbar.GetComponent<RectTransform>();
      component2.anchorMin = Vector2.right;
      component2.anchorMax = Vector2.one;
      component2.pivot = Vector2.one;
      component2.sizeDelta = new Vector2(component2.sizeDelta.x, 0.0f);
      Text lbl1 = uiObject9.AddComponent<Text>();
      DefaultControls.SetDefaultTextValues(lbl1);
      lbl1.alignment = TextAnchor.MiddleLeft;
      Image image1 = uiObject7.AddComponent<Image>();
      image1.color = (Color) new Color32((byte) 245, (byte) 245, (byte) 245, byte.MaxValue);
      Image image2 = uiObject8.AddComponent<Image>();
      image2.sprite = resources.checkmark;
      Toggle toggle = uiObject6.AddComponent<Toggle>();
      toggle.targetGraphic = (Graphic) image1;
      toggle.graphic = (Graphic) image2;
      toggle.isOn = true;
      Image image3 = uiObject3.AddComponent<Image>();
      image3.sprite = resources.standard;
      image3.type = Image.Type.Sliced;
      ScrollRect scrollRect = uiObject3.AddComponent<ScrollRect>();
      scrollRect.content = (RectTransform) uiObject5.transform;
      scrollRect.viewport = (RectTransform) uiObject4.transform;
      scrollRect.horizontal = false;
      scrollRect.movementType = ScrollRect.MovementType.Clamped;
      scrollRect.verticalScrollbar = component1;
      scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
      scrollRect.verticalScrollbarSpacing = -3f;
      uiObject4.AddComponent<Mask>().showMaskGraphic = false;
      Image image4 = uiObject4.AddComponent<Image>();
      image4.sprite = resources.mask;
      image4.type = Image.Type.Sliced;
      Text lbl2 = uiObject1.AddComponent<Text>();
      DefaultControls.SetDefaultTextValues(lbl2);
      lbl2.alignment = TextAnchor.MiddleLeft;
      uiObject2.AddComponent<Image>().sprite = resources.dropdown;
      Image image5 = uiElementRoot.AddComponent<Image>();
      image5.sprite = resources.standard;
      image5.color = DefaultControls.s_DefaultSelectableColor;
      image5.type = Image.Type.Sliced;
      Dropdown dropdown = uiElementRoot.AddComponent<Dropdown>();
      dropdown.targetGraphic = (Graphic) image5;
      DefaultControls.SetDefaultColorTransitionValues((Selectable) dropdown);
      dropdown.template = uiObject3.GetComponent<RectTransform>();
      dropdown.captionText = lbl2;
      dropdown.itemText = lbl1;
      lbl1.text = "Option A";
      dropdown.options.Add(new Dropdown.OptionData()
      {
        text = "Option A"
      });
      dropdown.options.Add(new Dropdown.OptionData()
      {
        text = "Option B"
      });
      dropdown.options.Add(new Dropdown.OptionData()
      {
        text = "Option C"
      });
      dropdown.RefreshShownValue();
      RectTransform component3 = uiObject1.GetComponent<RectTransform>();
      component3.anchorMin = Vector2.zero;
      component3.anchorMax = Vector2.one;
      component3.offsetMin = new Vector2(10f, 6f);
      component3.offsetMax = new Vector2(-25f, -7f);
      RectTransform component4 = uiObject2.GetComponent<RectTransform>();
      component4.anchorMin = new Vector2(1f, 0.5f);
      component4.anchorMax = new Vector2(1f, 0.5f);
      component4.sizeDelta = new Vector2(20f, 20f);
      component4.anchoredPosition = new Vector2(-15f, 0.0f);
      RectTransform component5 = uiObject3.GetComponent<RectTransform>();
      component5.anchorMin = new Vector2(0.0f, 0.0f);
      component5.anchorMax = new Vector2(1f, 0.0f);
      component5.pivot = new Vector2(0.5f, 1f);
      component5.anchoredPosition = new Vector2(0.0f, 2f);
      component5.sizeDelta = new Vector2(0.0f, 150f);
      RectTransform component6 = uiObject4.GetComponent<RectTransform>();
      component6.anchorMin = new Vector2(0.0f, 0.0f);
      component6.anchorMax = new Vector2(1f, 1f);
      component6.sizeDelta = new Vector2(-18f, 0.0f);
      component6.pivot = new Vector2(0.0f, 1f);
      RectTransform component7 = uiObject5.GetComponent<RectTransform>();
      component7.anchorMin = new Vector2(0.0f, 1f);
      component7.anchorMax = new Vector2(1f, 1f);
      component7.pivot = new Vector2(0.5f, 1f);
      component7.anchoredPosition = new Vector2(0.0f, 0.0f);
      component7.sizeDelta = new Vector2(0.0f, 28f);
      RectTransform component8 = uiObject6.GetComponent<RectTransform>();
      component8.anchorMin = new Vector2(0.0f, 0.5f);
      component8.anchorMax = new Vector2(1f, 0.5f);
      component8.sizeDelta = new Vector2(0.0f, 20f);
      RectTransform component9 = uiObject7.GetComponent<RectTransform>();
      component9.anchorMin = Vector2.zero;
      component9.anchorMax = Vector2.one;
      component9.sizeDelta = Vector2.zero;
      RectTransform component10 = uiObject8.GetComponent<RectTransform>();
      component10.anchorMin = new Vector2(0.0f, 0.5f);
      component10.anchorMax = new Vector2(0.0f, 0.5f);
      component10.sizeDelta = new Vector2(20f, 20f);
      component10.anchoredPosition = new Vector2(10f, 0.0f);
      RectTransform component11 = uiObject9.GetComponent<RectTransform>();
      component11.anchorMin = Vector2.zero;
      component11.anchorMax = Vector2.one;
      component11.offsetMin = new Vector2(20f, 1f);
      component11.offsetMax = new Vector2(-10f, -2f);
      uiObject3.SetActive(false);
      return uiElementRoot;
    }

    public static GameObject CreateScrollView(DefaultControls.Resources resources)
    {
      GameObject uiElementRoot = DefaultControls.CreateUIElementRoot("Scroll View", new Vector2(200f, 200f));
      GameObject uiObject1 = DefaultControls.CreateUIObject("Viewport", uiElementRoot);
      GameObject uiObject2 = DefaultControls.CreateUIObject("Content", uiObject1);
      GameObject scrollbar1 = DefaultControls.CreateScrollbar(resources);
      scrollbar1.name = "Scrollbar Horizontal";
      DefaultControls.SetParentAndAlign(scrollbar1, uiElementRoot);
      RectTransform component1 = scrollbar1.GetComponent<RectTransform>();
      component1.anchorMin = Vector2.zero;
      component1.anchorMax = Vector2.right;
      component1.pivot = Vector2.zero;
      component1.sizeDelta = new Vector2(0.0f, component1.sizeDelta.y);
      GameObject scrollbar2 = DefaultControls.CreateScrollbar(resources);
      scrollbar2.name = "Scrollbar Vertical";
      DefaultControls.SetParentAndAlign(scrollbar2, uiElementRoot);
      scrollbar2.GetComponent<Scrollbar>().SetDirection(Scrollbar.Direction.BottomToTop, true);
      RectTransform component2 = scrollbar2.GetComponent<RectTransform>();
      component2.anchorMin = Vector2.right;
      component2.anchorMax = Vector2.one;
      component2.pivot = Vector2.one;
      component2.sizeDelta = new Vector2(component2.sizeDelta.x, 0.0f);
      RectTransform component3 = uiObject1.GetComponent<RectTransform>();
      component3.anchorMin = Vector2.zero;
      component3.anchorMax = Vector2.one;
      component3.sizeDelta = Vector2.zero;
      component3.pivot = Vector2.up;
      RectTransform component4 = uiObject2.GetComponent<RectTransform>();
      component4.anchorMin = Vector2.up;
      component4.anchorMax = Vector2.one;
      component4.sizeDelta = new Vector2(0.0f, 300f);
      component4.pivot = Vector2.up;
      ScrollRect scrollRect = uiElementRoot.AddComponent<ScrollRect>();
      scrollRect.content = component4;
      scrollRect.viewport = component3;
      scrollRect.horizontalScrollbar = scrollbar1.GetComponent<Scrollbar>();
      scrollRect.verticalScrollbar = scrollbar2.GetComponent<Scrollbar>();
      scrollRect.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
      scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
      scrollRect.horizontalScrollbarSpacing = -3f;
      scrollRect.verticalScrollbarSpacing = -3f;
      Image image1 = uiElementRoot.AddComponent<Image>();
      image1.sprite = resources.background;
      image1.type = Image.Type.Sliced;
      image1.color = DefaultControls.s_PanelColor;
      uiObject1.AddComponent<Mask>().showMaskGraphic = false;
      Image image2 = uiObject1.AddComponent<Image>();
      image2.sprite = resources.mask;
      image2.type = Image.Type.Sliced;
      return uiElementRoot;
    }

    /// <summary>
    ///   <para>Object used to pass resources to use for the default controls.</para>
    /// </summary>
    public struct Resources
    {
      /// <summary>
      ///   <para>The primary sprite to be used for graphical UI elements, used by the button, toggle, and dropdown controls, among others.</para>
      /// </summary>
      public Sprite standard;
      /// <summary>
      ///   <para>Sprite used for background elements.</para>
      /// </summary>
      public Sprite background;
      /// <summary>
      ///   <para>Sprite used as background for input fields.</para>
      /// </summary>
      public Sprite inputField;
      /// <summary>
      ///   <para>Sprite used for knobs that can be dragged, such as on a slider.</para>
      /// </summary>
      public Sprite knob;
      /// <summary>
      ///   <para>Sprite used for representation of an "on" state when present, such as a checkmark.</para>
      /// </summary>
      public Sprite checkmark;
      /// <summary>
      ///   <para>Sprite used to indicate that a button will open a dropdown when clicked.</para>
      /// </summary>
      public Sprite dropdown;
      /// <summary>
      ///   <para>Sprite used for masking purposes, for example to be used for the viewport of a scroll view.</para>
      /// </summary>
      public Sprite mask;
    }
  }
}

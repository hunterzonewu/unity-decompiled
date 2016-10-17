// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.MenuOptions
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  internal static class MenuOptions
  {
    private const string kUILayerName = "UI";
    private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
    private const string kBackgroundSpritePath = "UI/Skin/Background.psd";
    private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
    private const string kKnobPath = "UI/Skin/Knob.psd";
    private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";
    private const string kDropdownArrowPath = "UI/Skin/DropdownArrow.psd";
    private const string kMaskPath = "UI/Skin/UIMask.psd";
    private static DefaultControls.Resources s_StandardResources;

    private static DefaultControls.Resources GetStandardResources()
    {
      if ((Object) MenuOptions.s_StandardResources.standard == (Object) null)
      {
        MenuOptions.s_StandardResources.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        MenuOptions.s_StandardResources.background = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        MenuOptions.s_StandardResources.inputField = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/InputFieldBackground.psd");
        MenuOptions.s_StandardResources.knob = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        MenuOptions.s_StandardResources.checkmark = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Checkmark.psd");
        MenuOptions.s_StandardResources.dropdown = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/DropdownArrow.psd");
        MenuOptions.s_StandardResources.mask = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");
      }
      return MenuOptions.s_StandardResources;
    }

    private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
    {
      SceneView sceneView = SceneView.lastActiveSceneView;
      if ((Object) sceneView == (Object) null && SceneView.sceneViews.Count > 0)
        sceneView = SceneView.sceneViews[0] as SceneView;
      if ((Object) sceneView == (Object) null || (Object) sceneView.camera == (Object) null)
        return;
      Camera camera = sceneView.camera;
      Vector3 zero = Vector3.zero;
      Vector2 localPoint;
      if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2((float) (camera.pixelWidth / 2), (float) (camera.pixelHeight / 2)), camera, out localPoint))
      {
        localPoint.x = localPoint.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
        localPoint.y = localPoint.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;
        localPoint.x = Mathf.Clamp(localPoint.x, 0.0f, canvasRTransform.sizeDelta.x);
        localPoint.y = Mathf.Clamp(localPoint.y, 0.0f, canvasRTransform.sizeDelta.y);
        zero.x = localPoint.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
        zero.y = localPoint.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;
        Vector3 vector3_1;
        vector3_1.x = (float) ((double) canvasRTransform.sizeDelta.x * (0.0 - (double) canvasRTransform.pivot.x) + (double) itemTransform.sizeDelta.x * (double) itemTransform.pivot.x);
        vector3_1.y = (float) ((double) canvasRTransform.sizeDelta.y * (0.0 - (double) canvasRTransform.pivot.y) + (double) itemTransform.sizeDelta.y * (double) itemTransform.pivot.y);
        Vector3 vector3_2;
        vector3_2.x = (float) ((double) canvasRTransform.sizeDelta.x * (1.0 - (double) canvasRTransform.pivot.x) - (double) itemTransform.sizeDelta.x * (double) itemTransform.pivot.x);
        vector3_2.y = (float) ((double) canvasRTransform.sizeDelta.y * (1.0 - (double) canvasRTransform.pivot.y) - (double) itemTransform.sizeDelta.y * (double) itemTransform.pivot.y);
        zero.x = Mathf.Clamp(zero.x, vector3_1.x, vector3_2.x);
        zero.y = Mathf.Clamp(zero.y, vector3_1.y, vector3_2.y);
      }
      itemTransform.anchoredPosition = (Vector2) zero;
      itemTransform.localRotation = Quaternion.identity;
      itemTransform.localScale = Vector3.one;
    }

    private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
    {
      GameObject parent = menuCommand.context as GameObject;
      if ((Object) parent == (Object) null || (Object) parent.GetComponentInParent<Canvas>() == (Object) null)
        parent = MenuOptions.GetOrCreateCanvasGameObject();
      string uniqueNameForSibling = GameObjectUtility.GetUniqueNameForSibling(parent.transform, element.name);
      element.name = uniqueNameForSibling;
      Undo.RegisterCreatedObjectUndo((Object) element, "Create " + element.name);
      Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
      GameObjectUtility.SetParentAndAlign(element, parent);
      if ((Object) parent != menuCommand.context)
        MenuOptions.SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());
      Selection.activeGameObject = element;
    }

    [MenuItem("GameObject/UI/Text", false, 2000)]
    public static void AddText(MenuCommand menuCommand)
    {
      MenuOptions.PlaceUIElementRoot(DefaultControls.CreateText(MenuOptions.GetStandardResources()), menuCommand);
    }

    [MenuItem("GameObject/UI/Image", false, 2001)]
    public static void AddImage(MenuCommand menuCommand)
    {
      MenuOptions.PlaceUIElementRoot(DefaultControls.CreateImage(MenuOptions.GetStandardResources()), menuCommand);
    }

    [MenuItem("GameObject/UI/Raw Image", false, 2002)]
    public static void AddRawImage(MenuCommand menuCommand)
    {
      MenuOptions.PlaceUIElementRoot(DefaultControls.CreateRawImage(MenuOptions.GetStandardResources()), menuCommand);
    }

    [MenuItem("GameObject/UI/Button", false, 2030)]
    public static void AddButton(MenuCommand menuCommand)
    {
      MenuOptions.PlaceUIElementRoot(DefaultControls.CreateButton(MenuOptions.GetStandardResources()), menuCommand);
    }

    [MenuItem("GameObject/UI/Toggle", false, 2031)]
    public static void AddToggle(MenuCommand menuCommand)
    {
      MenuOptions.PlaceUIElementRoot(DefaultControls.CreateToggle(MenuOptions.GetStandardResources()), menuCommand);
    }

    [MenuItem("GameObject/UI/Slider", false, 2033)]
    public static void AddSlider(MenuCommand menuCommand)
    {
      MenuOptions.PlaceUIElementRoot(DefaultControls.CreateSlider(MenuOptions.GetStandardResources()), menuCommand);
    }

    [MenuItem("GameObject/UI/Scrollbar", false, 2034)]
    public static void AddScrollbar(MenuCommand menuCommand)
    {
      MenuOptions.PlaceUIElementRoot(DefaultControls.CreateScrollbar(MenuOptions.GetStandardResources()), menuCommand);
    }

    [MenuItem("GameObject/UI/Dropdown", false, 2035)]
    public static void AddDropdown(MenuCommand menuCommand)
    {
      MenuOptions.PlaceUIElementRoot(DefaultControls.CreateDropdown(MenuOptions.GetStandardResources()), menuCommand);
    }

    [MenuItem("GameObject/UI/Input Field", false, 2036)]
    public static void AddInputField(MenuCommand menuCommand)
    {
      MenuOptions.PlaceUIElementRoot(DefaultControls.CreateInputField(MenuOptions.GetStandardResources()), menuCommand);
    }

    [MenuItem("GameObject/UI/Canvas", false, 2060)]
    public static void AddCanvas(MenuCommand menuCommand)
    {
      GameObject newUi = MenuOptions.CreateNewUI();
      GameObjectUtility.SetParentAndAlign(newUi, menuCommand.context as GameObject);
      if ((bool) ((Object) (newUi.transform.parent as RectTransform)))
      {
        RectTransform transform = newUi.transform as RectTransform;
        transform.anchorMin = Vector2.zero;
        transform.anchorMax = Vector2.one;
        transform.anchoredPosition = Vector2.zero;
        transform.sizeDelta = Vector2.zero;
      }
      Selection.activeGameObject = newUi;
    }

    [MenuItem("GameObject/UI/Panel", false, 2061)]
    public static void AddPanel(MenuCommand menuCommand)
    {
      GameObject panel = DefaultControls.CreatePanel(MenuOptions.GetStandardResources());
      MenuOptions.PlaceUIElementRoot(panel, menuCommand);
      RectTransform component = panel.GetComponent<RectTransform>();
      component.anchoredPosition = Vector2.zero;
      component.sizeDelta = Vector2.zero;
    }

    [MenuItem("GameObject/UI/Scroll View", false, 2062)]
    public static void AddScrollView(MenuCommand menuCommand)
    {
      MenuOptions.PlaceUIElementRoot(DefaultControls.CreateScrollView(MenuOptions.GetStandardResources()), menuCommand);
    }

    public static GameObject CreateNewUI()
    {
      GameObject gameObject = new GameObject("Canvas");
      gameObject.layer = LayerMask.NameToLayer("UI");
      gameObject.AddComponent<Canvas>().renderMode = UnityEngine.RenderMode.ScreenSpaceOverlay;
      gameObject.AddComponent<CanvasScaler>();
      gameObject.AddComponent<GraphicRaycaster>();
      Undo.RegisterCreatedObjectUndo((Object) gameObject, "Create " + gameObject.name);
      MenuOptions.CreateEventSystem(false);
      return gameObject;
    }

    [MenuItem("GameObject/UI/Event System", false, 2100)]
    public static void CreateEventSystem(MenuCommand menuCommand)
    {
      MenuOptions.CreateEventSystem(true, menuCommand.context as GameObject);
    }

    private static void CreateEventSystem(bool select)
    {
      MenuOptions.CreateEventSystem(select, (GameObject) null);
    }

    private static void CreateEventSystem(bool select, GameObject parent)
    {
      EventSystem eventSystem = Object.FindObjectOfType<EventSystem>();
      if ((Object) eventSystem == (Object) null)
      {
        GameObject child = new GameObject("EventSystem");
        GameObjectUtility.SetParentAndAlign(child, parent);
        eventSystem = child.AddComponent<EventSystem>();
        child.AddComponent<StandaloneInputModule>();
        Undo.RegisterCreatedObjectUndo((Object) child, "Create " + child.name);
      }
      if (!select || !((Object) eventSystem != (Object) null))
        return;
      Selection.activeGameObject = eventSystem.gameObject;
    }

    public static GameObject GetOrCreateCanvasGameObject()
    {
      GameObject activeGameObject = Selection.activeGameObject;
      Canvas canvas = !((Object) activeGameObject != (Object) null) ? (Canvas) null : activeGameObject.GetComponentInParent<Canvas>();
      if ((Object) canvas != (Object) null && canvas.gameObject.activeInHierarchy)
        return canvas.gameObject;
      Canvas objectOfType = Object.FindObjectOfType(typeof (Canvas)) as Canvas;
      if ((Object) objectOfType != (Object) null && objectOfType.gameObject.activeInHierarchy)
        return objectOfType.gameObject;
      return MenuOptions.CreateNewUI();
    }
  }
}

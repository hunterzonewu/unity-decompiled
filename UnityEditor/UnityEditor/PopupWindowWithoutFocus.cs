// Decompiled with JetBrains decompiler
// Type: UnityEditor.PopupWindowWithoutFocus
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class PopupWindowWithoutFocus : EditorWindow
  {
    private Vector2 m_LastWantedSize = Vector2.zero;
    private float m_BorderWidth = 1f;
    private static PopupWindowWithoutFocus s_PopupWindowWithoutFocus;
    private static double s_LastClosedTime;
    private static Rect s_LastActivatorRect;
    private PopupWindowContent m_WindowContent;
    private PopupLocationHelper.PopupLocation[] m_LocationPriorityOrder;
    private Rect m_ActivatorRect;

    private PopupWindowWithoutFocus()
    {
      this.hideFlags = HideFlags.DontSave;
    }

    public static void Show(Rect activatorRect, PopupWindowContent windowContent)
    {
      PopupWindowWithoutFocus.Show(activatorRect, windowContent, (PopupLocationHelper.PopupLocation[]) null);
    }

    public static bool IsVisible()
    {
      return (Object) PopupWindowWithoutFocus.s_PopupWindowWithoutFocus != (Object) null;
    }

    internal static void Show(Rect activatorRect, PopupWindowContent windowContent, PopupLocationHelper.PopupLocation[] locationPriorityOrder)
    {
      if (!PopupWindowWithoutFocus.ShouldShowWindow(activatorRect))
        return;
      if ((Object) PopupWindowWithoutFocus.s_PopupWindowWithoutFocus == (Object) null)
        PopupWindowWithoutFocus.s_PopupWindowWithoutFocus = ScriptableObject.CreateInstance<PopupWindowWithoutFocus>();
      PopupWindowWithoutFocus.s_PopupWindowWithoutFocus.Init(activatorRect, windowContent, locationPriorityOrder);
    }

    public static void Hide()
    {
      if (!((Object) PopupWindowWithoutFocus.s_PopupWindowWithoutFocus != (Object) null))
        return;
      PopupWindowWithoutFocus.s_PopupWindowWithoutFocus.Close();
    }

    private void Init(Rect activatorRect, PopupWindowContent windowContent, PopupLocationHelper.PopupLocation[] locationPriorityOrder)
    {
      this.m_WindowContent = windowContent;
      this.m_WindowContent.editorWindow = (EditorWindow) this;
      this.m_ActivatorRect = GUIUtility.GUIToScreenRect(activatorRect);
      this.m_LastWantedSize = windowContent.GetWindowSize();
      this.m_LocationPriorityOrder = locationPriorityOrder;
      Vector2 vector2 = windowContent.GetWindowSize() + new Vector2(this.m_BorderWidth * 2f, this.m_BorderWidth * 2f);
      this.position = PopupLocationHelper.GetDropDownRect(this.m_ActivatorRect, vector2, vector2, (ContainerWindow) null, this.m_LocationPriorityOrder);
      this.ShowPopup();
      this.Repaint();
    }

    private void OnEnable()
    {
      PopupWindowWithoutFocus.s_PopupWindowWithoutFocus = this;
    }

    private void OnDisable()
    {
      PopupWindowWithoutFocus.s_LastClosedTime = EditorApplication.timeSinceStartup;
      if (this.m_WindowContent != null)
        this.m_WindowContent.OnClose();
      PopupWindowWithoutFocus.s_PopupWindowWithoutFocus = (PopupWindowWithoutFocus) null;
    }

    private static bool OnGlobalMouseOrKeyEvent(EventType type, KeyCode keyCode, Vector2 mousePosition)
    {
      if ((Object) PopupWindowWithoutFocus.s_PopupWindowWithoutFocus == (Object) null)
        return false;
      if (type == EventType.KeyDown && keyCode == KeyCode.Escape)
      {
        PopupWindowWithoutFocus.s_PopupWindowWithoutFocus.Close();
        return true;
      }
      if (type != EventType.MouseDown || PopupWindowWithoutFocus.s_PopupWindowWithoutFocus.position.Contains(mousePosition))
        return false;
      PopupWindowWithoutFocus.s_PopupWindowWithoutFocus.Close();
      return true;
    }

    private static bool ShouldShowWindow(Rect activatorRect)
    {
      if (EditorApplication.timeSinceStartup - PopupWindowWithoutFocus.s_LastClosedTime < 0.2 && !(activatorRect != PopupWindowWithoutFocus.s_LastActivatorRect))
        return false;
      PopupWindowWithoutFocus.s_LastActivatorRect = activatorRect;
      return true;
    }

    internal void OnGUI()
    {
      this.FitWindowToContent();
      this.m_WindowContent.OnGUI(new Rect(this.m_BorderWidth, this.m_BorderWidth, this.position.width - 2f * this.m_BorderWidth, this.position.height - 2f * this.m_BorderWidth));
      GUI.Label(new Rect(0.0f, 0.0f, this.position.width, this.position.height), GUIContent.none, (GUIStyle) "grey_border");
    }

    private void FitWindowToContent()
    {
      Vector2 windowSize = this.m_WindowContent.GetWindowSize();
      if (!(this.m_LastWantedSize != windowSize))
        return;
      this.m_LastWantedSize = windowSize;
      Vector2 vector2_1 = windowSize + new Vector2(2f * this.m_BorderWidth, 2f * this.m_BorderWidth);
      Rect dropDownRect = PopupLocationHelper.GetDropDownRect(this.m_ActivatorRect, vector2_1, vector2_1, (ContainerWindow) null, this.m_LocationPriorityOrder);
      this.m_Pos = dropDownRect;
      Vector2 vector2_2 = new Vector2(dropDownRect.width, dropDownRect.height);
      this.maxSize = vector2_2;
      this.minSize = vector2_2;
    }
  }
}

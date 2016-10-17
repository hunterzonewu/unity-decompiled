// Decompiled with JetBrains decompiler
// Type: PreviewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

internal class PreviewGUI
{
  private static int sliderHash = "Slider".GetHashCode();
  private static Rect s_ViewRect;
  private static Rect s_Position;
  private static Vector2 s_ScrollPos;

  internal static void BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar)
  {
    PreviewGUI.s_ScrollPos = scrollPosition;
    PreviewGUI.s_ViewRect = viewRect;
    PreviewGUI.s_Position = position;
    GUIClip.Push(position, new Vector2(Mathf.Round((float) (-(double) scrollPosition.x - (double) viewRect.x - ((double) viewRect.width - (double) position.width) * 0.5)), Mathf.Round((float) (-(double) scrollPosition.y - (double) viewRect.y - ((double) viewRect.height - (double) position.height) * 0.5))), Vector2.zero, false);
  }

  public static int CycleButton(int selected, GUIContent[] options)
  {
    PreviewGUI.Styles.Init();
    return EditorGUILayout.CycleButton(selected, options, PreviewGUI.Styles.preButton);
  }

  public static Vector2 EndScrollView()
  {
    GUIClip.Pop();
    Rect position1 = PreviewGUI.s_Position;
    Rect position2 = PreviewGUI.s_Position;
    Rect viewRect = PreviewGUI.s_ViewRect;
    Vector2 scrollPos = PreviewGUI.s_ScrollPos;
    switch (Event.current.type)
    {
      case EventType.Layout:
        GUIUtility.GetControlID(PreviewGUI.sliderHash, FocusType.Passive);
        GUIUtility.GetControlID(PreviewGUI.sliderHash, FocusType.Passive);
        goto case EventType.Used;
      case EventType.Used:
        return scrollPos;
      default:
        bool flag1 = false;
        bool flag2 = false;
        if (flag2 || (double) viewRect.width > (double) position1.width)
          flag2 = true;
        if (flag1 || (double) viewRect.height > (double) position1.height)
          flag1 = true;
        int controlId1 = GUIUtility.GetControlID(PreviewGUI.sliderHash, FocusType.Passive);
        if (flag2)
        {
          GUIStyle slider = (GUIStyle) "PreHorizontalScrollbar";
          GUIStyle thumb = (GUIStyle) "PreHorizontalScrollbarThumb";
          float num = (float) (((double) viewRect.width - (double) position1.width) * 0.5);
          scrollPos.x = GUI.Slider(new Rect(position2.x, position2.yMax - slider.fixedHeight, position1.width - (!flag1 ? 0.0f : slider.fixedHeight), slider.fixedHeight), scrollPos.x, position1.width + num, -num, viewRect.width, slider, thumb, true, controlId1);
        }
        else
          scrollPos.x = 0.0f;
        int controlId2 = GUIUtility.GetControlID(PreviewGUI.sliderHash, FocusType.Passive);
        if (flag1)
        {
          GUIStyle slider = (GUIStyle) "PreVerticalScrollbar";
          GUIStyle thumb = (GUIStyle) "PreVerticalScrollbarThumb";
          float num = (float) (((double) viewRect.height - (double) position1.height) * 0.5);
          scrollPos.y = GUI.Slider(new Rect(position1.xMax - slider.fixedWidth, position1.y, slider.fixedWidth, position1.height), scrollPos.y, position1.height + num, -num, viewRect.height, slider, thumb, false, controlId2);
          goto case EventType.Used;
        }
        else
        {
          scrollPos.y = 0.0f;
          goto case EventType.Used;
        }
    }
  }

  public static Vector2 Drag2D(Vector2 scrollPosition, Rect position)
  {
    int controlId = GUIUtility.GetControlID(PreviewGUI.sliderHash, FocusType.Passive);
    Event current = Event.current;
    switch (current.GetTypeForControl(controlId))
    {
      case EventType.MouseDown:
        if (position.Contains(current.mousePosition) && (double) position.width > 50.0)
        {
          GUIUtility.hotControl = controlId;
          current.Use();
          EditorGUIUtility.SetWantsMouseJumping(1);
          break;
        }
        break;
      case EventType.MouseUp:
        if (GUIUtility.hotControl == controlId)
          GUIUtility.hotControl = 0;
        EditorGUIUtility.SetWantsMouseJumping(0);
        break;
      case EventType.MouseDrag:
        if (GUIUtility.hotControl == controlId)
        {
          scrollPosition -= current.delta * (!current.shift ? 1f : 3f) / Mathf.Min(position.width, position.height) * 140f;
          scrollPosition.y = Mathf.Clamp(scrollPosition.y, -90f, 90f);
          current.Use();
          GUI.changed = true;
          break;
        }
        break;
    }
    return scrollPosition;
  }

  internal class Styles
  {
    public static GUIStyle preButton;

    public static void Init()
    {
      PreviewGUI.Styles.preButton = (GUIStyle) "preButton";
    }
  }
}

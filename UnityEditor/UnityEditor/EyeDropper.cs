// Decompiled with JetBrains decompiler
// Type: UnityEditor.EyeDropper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class EyeDropper : GUIView
  {
    private static Vector2 s_PickCoordinates = Vector2.zero;
    private const int kPixelSize = 10;
    private const int kDummyWindowSize = 10000;
    internal static Color s_LastPickedColor;
    private GUIView m_DelegateView;
    private Texture2D m_Preview;
    private static EyeDropper s_Instance;
    private bool m_Focused;
    private static EyeDropper.Styles styles;

    private static EyeDropper get
    {
      get
      {
        if (!(bool) ((Object) EyeDropper.s_Instance))
          ScriptableObject.CreateInstance<EyeDropper>();
        return EyeDropper.s_Instance;
      }
    }

    private EyeDropper()
    {
      EyeDropper.s_Instance = this;
    }

    public static void Start(GUIView viewToUpdate)
    {
      EyeDropper.get.Show(viewToUpdate);
    }

    private void Show(GUIView sourceView)
    {
      this.m_DelegateView = sourceView;
      ContainerWindow instance = ScriptableObject.CreateInstance<ContainerWindow>();
      instance.m_DontSaveToLayout = true;
      instance.title = "EyeDropper";
      instance.hideFlags = HideFlags.DontSave;
      instance.mainView = (View) this;
      instance.Show(ShowMode.PopupMenu, true, false);
      this.AddToAuxWindowList();
      instance.SetInvisible();
      this.SetMinMaxSizes(new Vector2(0.0f, 0.0f), new Vector2(10000f, 10000f));
      instance.position = new Rect(-5000f, -5000f, 10000f, 10000f);
      this.wantsMouseMove = true;
      this.StealMouseCapture();
    }

    public static Color GetPickedColor()
    {
      return InternalEditorUtility.ReadScreenPixel(EyeDropper.s_PickCoordinates, 1, 1)[0];
    }

    public static Color GetLastPickedColor()
    {
      return EyeDropper.s_LastPickedColor;
    }

    public static void DrawPreview(Rect position)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if (EyeDropper.styles == null)
        EyeDropper.styles = new EyeDropper.Styles();
      Texture2D texture2D = EyeDropper.get.m_Preview;
      int num1 = (int) Mathf.Ceil(position.width / 10f);
      int num2 = (int) Mathf.Ceil(position.height / 10f);
      if ((Object) texture2D == (Object) null)
      {
        EyeDropper.get.m_Preview = texture2D = ColorPicker.MakeTexture(num1, num2);
        texture2D.filterMode = UnityEngine.FilterMode.Point;
      }
      if (texture2D.width != num1 || texture2D.height != num2)
        texture2D.Resize(num1, num2);
      Vector2 screenPoint = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
      Vector2 pixelPos = screenPoint - new Vector2((float) (num1 / 2), (float) (num2 / 2));
      texture2D.SetPixels(InternalEditorUtility.ReadScreenPixel(pixelPos, num1, num2), 0);
      texture2D.Apply(true);
      Graphics.DrawTexture(position, (Texture) texture2D);
      float width = position.width / (float) num1;
      GUIStyle dropperVerticalLine = EyeDropper.styles.eyeDropperVerticalLine;
      float x = position.x;
      while ((double) x < (double) position.xMax)
      {
        Rect position1 = new Rect(Mathf.Round(x), position.y, width, position.height);
        dropperVerticalLine.Draw(position1, false, false, false, false);
        x += width;
      }
      float height = position.height / (float) num2;
      GUIStyle dropperHorizontalLine = EyeDropper.styles.eyeDropperHorizontalLine;
      float y = position.y;
      while ((double) y < (double) position.yMax)
      {
        Rect position1 = new Rect(position.x, Mathf.Floor(y), position.width, height);
        dropperHorizontalLine.Draw(position1, false, false, false, false);
        y += height;
      }
      Rect position2 = new Rect((screenPoint.x - pixelPos.x) * width + position.x, (screenPoint.y - pixelPos.y) * height + position.y, width, height);
      EyeDropper.styles.eyeDropperPickedPixel.Draw(position2, false, false, false, false);
    }

    private void OnGUI()
    {
      switch (Event.current.type)
      {
        case EventType.MouseDown:
          if (Event.current.button != 0)
            break;
          EyeDropper.s_PickCoordinates = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
          this.window.Close();
          EyeDropper.s_LastPickedColor = EyeDropper.GetPickedColor();
          this.SendEvent("EyeDropperClicked", true);
          break;
        case EventType.MouseMove:
          EyeDropper.s_PickCoordinates = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
          this.StealMouseCapture();
          this.SendEvent("EyeDropperUpdate", true);
          break;
        case EventType.KeyDown:
          if (Event.current.keyCode != KeyCode.Escape)
            break;
          this.window.Close();
          this.SendEvent("EyeDropperCancelled", true);
          break;
      }
    }

    private void SendEvent(string eventName, bool exitGUI)
    {
      if (!(bool) ((Object) this.m_DelegateView))
        return;
      this.m_DelegateView.SendEvent(EditorGUIUtility.CommandEvent(eventName));
      if (!exitGUI)
        return;
      GUIUtility.ExitGUI();
    }

    public new void OnDestroy()
    {
      if ((bool) ((Object) this.m_Preview))
        Object.DestroyImmediate((Object) this.m_Preview);
      if (!this.m_Focused)
        this.SendEvent("EyeDropperCancelled", false);
      base.OnDestroy();
    }

    protected override bool OnFocus()
    {
      this.m_Focused = true;
      return base.OnFocus();
    }

    private class Styles
    {
      public GUIStyle eyeDropperHorizontalLine = (GUIStyle) "EyeDropperHorizontalLine";
      public GUIStyle eyeDropperVerticalLine = (GUIStyle) "EyeDropperVerticalLine";
      public GUIStyle eyeDropperPickedPixel = (GUIStyle) "EyeDropperPickedPixel";
    }
  }
}

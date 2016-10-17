// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneFXWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SceneFXWindow : PopupWindowContent
  {
    private const float kFrameWidth = 1f;
    private static SceneFXWindow s_SceneFXWindow;
    private static SceneFXWindow.Styles s_Styles;
    private readonly SceneView m_SceneView;

    public SceneFXWindow(SceneView sceneView)
    {
      this.m_SceneView = sceneView;
    }

    public override Vector2 GetWindowSize()
    {
      return new Vector2(160f, 66f);
    }

    public override void OnGUI(Rect rect)
    {
      if ((UnityEngine.Object) this.m_SceneView == (UnityEngine.Object) null || this.m_SceneView.m_SceneViewState == null || Event.current.type == EventType.Layout)
        return;
      if (SceneFXWindow.s_Styles == null)
        SceneFXWindow.s_Styles = new SceneFXWindow.Styles();
      this.Draw(rect);
      if (Event.current.type == EventType.MouseMove)
        Event.current.Use();
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape)
        return;
      this.editorWindow.Close();
      GUIUtility.ExitGUI();
    }

    private void Draw(Rect rect)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SceneFXWindow.\u003CDraw\u003Ec__AnonStorey4C drawCAnonStorey4C = new SceneFXWindow.\u003CDraw\u003Ec__AnonStorey4C();
      if ((UnityEngine.Object) this.m_SceneView == (UnityEngine.Object) null || this.m_SceneView.m_SceneViewState == null)
        return;
      Rect rect1 = new Rect(1f, 1f, rect.width - 2f, 16f);
      // ISSUE: reference to a compiler-generated field
      drawCAnonStorey4C.state = this.m_SceneView.m_SceneViewState;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.DrawListElement(rect1, "Skybox", drawCAnonStorey4C.state.showSkybox, new System.Action<bool>(drawCAnonStorey4C.\u003C\u003Em__86));
      rect1.y += 16f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.DrawListElement(rect1, "Fog", drawCAnonStorey4C.state.showFog, new System.Action<bool>(drawCAnonStorey4C.\u003C\u003Em__87));
      rect1.y += 16f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.DrawListElement(rect1, "Flares", drawCAnonStorey4C.state.showFlares, new System.Action<bool>(drawCAnonStorey4C.\u003C\u003Em__88));
      rect1.y += 16f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.DrawListElement(rect1, "Animated Materials", drawCAnonStorey4C.state.showMaterialUpdate, new System.Action<bool>(drawCAnonStorey4C.\u003C\u003Em__89));
      rect1.y += 16f;
    }

    private void DrawListElement(Rect rect, string toggleName, bool value, System.Action<bool> setValue)
    {
      EditorGUI.BeginChangeCheck();
      bool flag = GUI.Toggle(rect, value, EditorGUIUtility.TempContent(toggleName), SceneFXWindow.s_Styles.menuItem);
      if (!EditorGUI.EndChangeCheck())
        return;
      setValue(flag);
      this.m_SceneView.Repaint();
    }

    private class Styles
    {
      public readonly GUIStyle menuItem = (GUIStyle) "MenuItem";
    }
  }
}

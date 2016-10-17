// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaximizedHostView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
  internal class MaximizedHostView : HostView
  {
    public void OnGUI()
    {
      this.ClearBackground();
      EditorGUIUtility.ResetGUIState();
      Rect rect = new Rect(-2f, 0.0f, this.position.width + 4f, this.position.height);
      this.background = (GUIStyle) "dockarea";
      GUIStyle style = (GUIStyle) "dockareaoverlay";
      Rect position1 = this.background.margin.Remove(rect);
      this.DoWindowDecorationStart();
      Rect position2 = new Rect(position1.x + 1f, position1.y, position1.width - 2f, 17f);
      if (Event.current.type == EventType.Repaint)
      {
        this.background.Draw(position1, GUIContent.none, false, false, false, false);
        (GUIStyle) "dragTab".Draw(position2, this.actualView.titleContent, false, false, true, this.hasFocus);
      }
      if (Event.current.type == EventType.ContextClick && position2.Contains(Event.current.mousePosition))
        this.PopupGenericMenu(this.actualView, new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 0.0f, 0.0f));
      this.ShowGenericMenu();
      if ((bool) ((UnityEngine.Object) this.actualView))
      {
        this.actualView.m_Pos = this.borderSize.Remove(this.screenPosition);
        if (this.actualView is GameView)
          GUI.Box(position1, GUIContent.none, style);
      }
      DockArea.BeginOffsetArea(new Rect(position1.x + 2f, position1.y + 17f, position1.width - 4f, (float) ((double) position1.height - 17.0 - 2.0)), GUIContent.none, (GUIStyle) "TabWindowBackground");
      try
      {
        this.Invoke("OnGUI");
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
      EditorGUIUtility.ResetGUIState();
      DockArea.EndOffsetArea();
      this.DoWindowDecorationEnd();
      GUI.Box(position1, GUIContent.none, style);
    }

    protected override RectOffset GetBorderSize()
    {
      this.m_BorderSize.left = 0;
      this.m_BorderSize.right = 0;
      this.m_BorderSize.top = 17;
      this.m_BorderSize.bottom = 4;
      return this.m_BorderSize;
    }

    private void Unmaximize(object userData)
    {
      WindowLayout.Unmaximize((EditorWindow) userData);
    }

    protected override void AddDefaultItemsToMenu(GenericMenu menu, EditorWindow view)
    {
      if (menu.GetItemCount() != 0)
        menu.AddSeparator(string.Empty);
      menu.AddItem(EditorGUIUtility.TextContent("Maximize"), !(this.parent is SplitView), new GenericMenu.MenuFunction2(this.Unmaximize), (object) view);
      menu.AddDisabledItem(EditorGUIUtility.TextContent("Close Tab"));
      menu.AddSeparator(string.Empty);
      System.Type[] paneTypes = this.GetPaneTypes();
      GUIContent guiContent = EditorGUIUtility.TextContent("Add Tab");
      foreach (System.Type t in paneTypes)
      {
        if (t != null)
        {
          GUIContent content = new GUIContent(EditorWindow.GetLocalizedTitleContentFromType(t));
          content.text = guiContent.text + "/" + content.text;
          menu.AddDisabledItem(content);
        }
      }
    }
  }
}

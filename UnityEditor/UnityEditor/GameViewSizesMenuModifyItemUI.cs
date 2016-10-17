// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameViewSizesMenuModifyItemUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class GameViewSizesMenuModifyItemUI : FlexibleMenuModifyItemUI
  {
    private static GameViewSizesMenuModifyItemUI.Styles s_Styles;
    private GameViewSize m_GameViewSize;

    public override void OnClose()
    {
      this.m_GameViewSize = (GameViewSize) null;
      base.OnClose();
    }

    public override Vector2 GetWindowSize()
    {
      return new Vector2(230f, 140f);
    }

    public override void OnGUI(Rect rect)
    {
      if (GameViewSizesMenuModifyItemUI.s_Styles == null)
        GameViewSizesMenuModifyItemUI.s_Styles = new GameViewSizesMenuModifyItemUI.Styles();
      GameViewSize other = this.m_Object as GameViewSize;
      if (other == null)
      {
        Debug.LogError((object) "Invalid object");
      }
      else
      {
        if (this.m_GameViewSize == null)
          this.m_GameViewSize = new GameViewSize(other);
        bool flag = this.m_GameViewSize.width > 0 && this.m_GameViewSize.height > 0;
        GUILayout.Space(3f);
        GUILayout.Label(this.m_MenuType != FlexibleMenuModifyItemUI.MenuType.Add ? GameViewSizesMenuModifyItemUI.s_Styles.headerEdit : GameViewSizesMenuModifyItemUI.s_Styles.headerAdd, EditorStyles.boldLabel, new GUILayoutOption[0]);
        FlexibleMenu.DrawRect(GUILayoutUtility.GetRect(1f, 1f), !EditorGUIUtility.isProSkin ? new Color(0.6f, 0.6f, 0.6f, 1.333f) : new Color(0.32f, 0.32f, 0.32f, 1.333f));
        GUILayout.Space(4f);
        GUILayout.BeginHorizontal();
        GUILayout.Label(GameViewSizesMenuModifyItemUI.s_Styles.optionalText, new GUILayoutOption[1]
        {
          GUILayout.Width(90f)
        });
        GUILayout.Space(10f);
        this.m_GameViewSize.baseText = EditorGUILayout.TextField(this.m_GameViewSize.baseText);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label(GameViewSizesMenuModifyItemUI.s_Styles.typeName, new GUILayoutOption[1]
        {
          GUILayout.Width(90f)
        });
        GUILayout.Space(10f);
        this.m_GameViewSize.sizeType = (GameViewSizeType) EditorGUILayout.Popup((int) this.m_GameViewSize.sizeType, GameViewSizesMenuModifyItemUI.s_Styles.typeNames);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label(GameViewSizesMenuModifyItemUI.s_Styles.widthHeightText, new GUILayoutOption[1]
        {
          GUILayout.Width(90f)
        });
        GUILayout.Space(10f);
        this.m_GameViewSize.width = EditorGUILayout.IntField(this.m_GameViewSize.width);
        GUILayout.Space(5f);
        this.m_GameViewSize.height = EditorGUILayout.IntField(this.m_GameViewSize.height);
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        float pixels = 10f;
        float cropWidth = rect.width - 2f * pixels;
        GUILayout.BeginHorizontal();
        GUILayout.Space(pixels);
        GUILayout.FlexibleSpace();
        string displayText = this.m_GameViewSize.displayText;
        EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(displayText));
        GUILayout.Label(GUIContent.Temp(!string.IsNullOrEmpty(displayText) ? this.GetCroppedText(displayText, cropWidth, EditorStyles.label) : "Result"), EditorStyles.label, new GUILayoutOption[0]);
        EditorGUI.EndDisabledGroup();
        GUILayout.FlexibleSpace();
        GUILayout.Space(pixels);
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        GUILayout.Space(10f);
        if (GUILayout.Button(GameViewSizesMenuModifyItemUI.s_Styles.cancel))
          this.editorWindow.Close();
        EditorGUI.BeginDisabledGroup(!flag);
        if (GUILayout.Button(GameViewSizesMenuModifyItemUI.s_Styles.ok))
        {
          other.Set(this.m_GameViewSize);
          this.Accepted();
          this.editorWindow.Close();
        }
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(10f);
        GUILayout.EndHorizontal();
      }
    }

    private string GetCroppedText(string fullText, float cropWidth, GUIStyle style)
    {
      int thatFitWithinWidth = style.GetNumCharactersThatFitWithinWidth(fullText, cropWidth);
      if (thatFitWithinWidth == -1 || thatFitWithinWidth <= 1 || thatFitWithinWidth == fullText.Length)
        return fullText;
      return fullText.Substring(0, thatFitWithinWidth - 1) + "…";
    }

    private class Styles
    {
      public GUIContent headerAdd = new GUIContent("Add");
      public GUIContent headerEdit = new GUIContent("Edit");
      public GUIContent typeName = new GUIContent("Type");
      public GUIContent widthHeightText = new GUIContent("Width & Height");
      public GUIContent optionalText = new GUIContent("Label");
      public GUIContent ok = new GUIContent("OK");
      public GUIContent cancel = new GUIContent("Cancel");
      public GUIContent[] typeNames = new GUIContent[2]{ new GUIContent("Aspect Ratio"), new GUIContent("Fixed Resolution") };
    }
  }
}

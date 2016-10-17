// Decompiled with JetBrains decompiler
// Type: UnityEditor.AddShaderVariantWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  internal class AddShaderVariantWindow : EditorWindow
  {
    private const float kMargin = 2f;
    private const float kSpaceHeight = 6f;
    private const float kSeparatorHeight = 3f;
    private const float kMinWindowWidth = 400f;
    private const float kMiscUIHeight = 120f;
    private const float kMinWindowHeight = 264f;
    private AddShaderVariantWindow.PopupData m_Data;
    private List<string> m_SelectedKeywords;
    private List<string> m_AvailableKeywords;
    private List<int> m_FilteredVariants;
    private List<int> m_SelectedVariants;

    public AddShaderVariantWindow()
    {
      this.position = new Rect(100f, 100f, 600f, 396f);
      this.minSize = new Vector2(400f, 264f);
      this.wantsMouseMove = true;
    }

    private void Initialize(AddShaderVariantWindow.PopupData data)
    {
      this.m_Data = data;
      this.m_SelectedKeywords = new List<string>();
      this.m_AvailableKeywords = new List<string>();
      this.m_SelectedVariants = new List<int>();
      this.m_AvailableKeywords.Sort();
      this.m_FilteredVariants = new List<int>();
      this.ApplyKeywordFilter();
    }

    public static void ShowAddVariantWindow(AddShaderVariantWindow.PopupData data)
    {
      AddShaderVariantWindow window = EditorWindow.GetWindow<AddShaderVariantWindow>(true, "Add shader " + data.shader.name + " variants to collection");
      window.Initialize(data);
      window.m_Parent.window.m_DontSaveToLayout = true;
    }

    private void ApplyKeywordFilter()
    {
      this.m_FilteredVariants.Clear();
      this.m_AvailableKeywords.Clear();
      for (int index1 = 0; index1 < this.m_Data.keywords.Length; ++index1)
      {
        bool flag = true;
        for (int index2 = 0; index2 < this.m_SelectedKeywords.Count; ++index2)
        {
          if (!((IEnumerable<string>) this.m_Data.keywords[index1]).Contains<string>(this.m_SelectedKeywords[index2]))
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          this.m_FilteredVariants.Add(index1);
          foreach (string str in this.m_Data.keywords[index1])
          {
            if (!this.m_AvailableKeywords.Contains(str) && !this.m_SelectedKeywords.Contains(str))
              this.m_AvailableKeywords.Add(str);
          }
        }
      }
      this.m_AvailableKeywords.Sort();
    }

    public void OnGUI()
    {
      if (this.m_Data == null || (UnityEngine.Object) this.m_Data.shader == (UnityEngine.Object) null || (UnityEngine.Object) this.m_Data.collection == (UnityEngine.Object) null)
      {
        this.Close();
        GUIUtility.ExitGUI();
      }
      else
      {
        if (Event.current.type == EventType.Layout)
          return;
        this.Draw(new Rect(0.0f, 0.0f, this.position.width, this.position.height));
        if (Event.current.type != EventType.MouseMove)
          return;
        Event.current.Use();
      }
    }

    private bool KeywordButton(Rect buttonRect, string k, Vector2 areaSize)
    {
      Color color = GUI.color;
      if ((double) buttonRect.yMax > (double) areaSize.y)
        GUI.color = new Color(1f, 1f, 1f, 0.4f);
      bool flag = GUI.Button(buttonRect, EditorGUIUtility.TempContent(k), EditorStyles.miniButton);
      GUI.color = color;
      return flag;
    }

    private float CalcVerticalSpaceForKeywords()
    {
      return Mathf.Floor((float) (((double) this.position.height - 120.0) / 4.0));
    }

    private float CalcVerticalSpaceForVariants()
    {
      return (float) (((double) this.position.height - 120.0) / 2.0);
    }

    private void DrawKeywordsList(ref Rect rect, List<string> keywords, bool clickingAddsToSelected)
    {
      rect.height = this.CalcVerticalSpaceForKeywords();
      List<string> list = keywords.Select<string, string>((Func<string, string>) (k =>
      {
        if (string.IsNullOrEmpty(k))
          return "<no keyword>";
        return k.ToLowerInvariant();
      })).ToList<string>();
      GUI.BeginGroup(rect);
      List<Rect> flowLayoutedRects = EditorGUIUtility.GetFlowLayoutedRects(new Rect(4f, 0.0f, rect.width, rect.height), EditorStyles.miniButton, 2f, 2f, list);
      for (int index = 0; index < list.Count; ++index)
      {
        if (this.KeywordButton(flowLayoutedRects[index], list[index], rect.size))
        {
          if (clickingAddsToSelected)
          {
            this.m_SelectedKeywords.Add(keywords[index]);
            this.m_SelectedKeywords.Sort();
          }
          else
            this.m_SelectedKeywords.Remove(keywords[index]);
          this.ApplyKeywordFilter();
          GUIUtility.ExitGUI();
        }
      }
      GUI.EndGroup();
      rect.y += rect.height;
    }

    private void DrawSectionHeader(ref Rect rect, string titleString, bool separator)
    {
      rect.y += 6f;
      if (separator)
      {
        rect.height = 3f;
        GUI.Label(rect, GUIContent.none, AddShaderVariantWindow.Styles.sSeparator);
        rect.y += rect.height;
      }
      rect.height = 16f;
      GUI.Label(rect, titleString);
      rect.y += rect.height;
    }

    private void Draw(Rect windowRect)
    {
      Rect rect = new Rect(2f, 2f, windowRect.width - 4f, 16f);
      this.DrawSectionHeader(ref rect, "Pick shader keywords to narrow down variant list:", false);
      this.DrawKeywordsList(ref rect, this.m_AvailableKeywords, true);
      this.DrawSectionHeader(ref rect, "Selected keywords:", true);
      this.DrawKeywordsList(ref rect, this.m_SelectedKeywords, false);
      this.DrawSectionHeader(ref rect, "Shader variants with these keywords (click to select):", true);
      if (this.m_FilteredVariants.Count > 0)
      {
        int b = (int) ((double) this.CalcVerticalSpaceForVariants() / 16.0);
        for (int index = 0; index < Mathf.Min(this.m_FilteredVariants.Count, b); ++index)
        {
          int filteredVariant = this.m_FilteredVariants[index];
          PassType type = (PassType) this.m_Data.types[filteredVariant];
          bool flag1 = this.m_SelectedVariants.Contains(filteredVariant);
          string text = type.ToString() + " " + string.Join(" ", this.m_Data.keywords[filteredVariant]).ToLowerInvariant();
          bool flag2 = GUI.Toggle(rect, flag1, text, AddShaderVariantWindow.Styles.sMenuItem);
          rect.y += rect.height;
          if (flag2 && !flag1)
            this.m_SelectedVariants.Add(filteredVariant);
          else if (!flag2 && flag1)
            this.m_SelectedVariants.Remove(filteredVariant);
        }
        if (this.m_FilteredVariants.Count > b)
        {
          GUI.Label(rect, string.Format("[{0} more variants skipped]", (object) (this.m_FilteredVariants.Count - b)), EditorStyles.miniLabel);
          rect.y += rect.height;
        }
      }
      else
      {
        GUI.Label(rect, "No variants with these keywords");
        rect.y += rect.height;
      }
      rect.y = (float) ((double) windowRect.height - 2.0 - 6.0 - 16.0);
      rect.height = 16f;
      EditorGUI.BeginDisabledGroup(this.m_SelectedVariants.Count == 0);
      if (GUI.Button(rect, string.Format("Add {0} selected variants", (object) this.m_SelectedVariants.Count)))
      {
        Undo.RecordObject((UnityEngine.Object) this.m_Data.collection, "Add variant");
        for (int index = 0; index < this.m_SelectedVariants.Count; ++index)
        {
          int selectedVariant = this.m_SelectedVariants[index];
          this.m_Data.collection.Add(new ShaderVariantCollection.ShaderVariant(this.m_Data.shader, (PassType) this.m_Data.types[selectedVariant], this.m_Data.keywords[selectedVariant]));
        }
        this.Close();
        GUIUtility.ExitGUI();
      }
      EditorGUI.EndDisabledGroup();
    }

    internal class PopupData
    {
      public Shader shader;
      public ShaderVariantCollection collection;
      public int[] types;
      public string[][] keywords;
    }

    private class Styles
    {
      public static readonly GUIStyle sMenuItem = (GUIStyle) "MenuItem";
      public static readonly GUIStyle sSeparator = (GUIStyle) "sv_iconselector_sep";
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.WebTemplateManagerBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class WebTemplateManagerBase
  {
    private const float kWebTemplateGridPadding = 15f;
    private const float kThumbnailSize = 80f;
    private const float kThumbnailLabelHeight = 20f;
    private const float kThumbnailPadding = 5f;
    private static WebTemplateManagerBase.Styles s_Styles;
    private WebTemplate[] s_Templates;
    private GUIContent[] s_TemplateGUIThumbnails;

    public abstract string customTemplatesFolder { get; }

    public abstract string builtinTemplatesFolder { get; }

    public abstract Texture2D defaultIcon { get; }

    public WebTemplate[] Templates
    {
      get
      {
        if (this.s_Templates == null || this.s_TemplateGUIThumbnails == null)
          this.BuildTemplateList();
        return this.s_Templates;
      }
    }

    public GUIContent[] TemplateGUIThumbnails
    {
      get
      {
        if (this.s_Templates == null || this.s_TemplateGUIThumbnails == null)
          this.BuildTemplateList();
        return this.s_TemplateGUIThumbnails;
      }
    }

    public int GetTemplateIndex(string path)
    {
      for (int index = 0; index < this.Templates.Length; ++index)
      {
        if (path.Equals(this.Templates[index].ToString()))
          return index;
      }
      return 0;
    }

    public void ClearTemplates()
    {
      this.s_Templates = (WebTemplate[]) null;
      this.s_TemplateGUIThumbnails = (GUIContent[]) null;
    }

    private void BuildTemplateList()
    {
      List<WebTemplate> webTemplateList = new List<WebTemplate>();
      if (Directory.Exists(this.customTemplatesFolder))
        webTemplateList.AddRange((IEnumerable<WebTemplate>) this.ListTemplates(this.customTemplatesFolder));
      if (Directory.Exists(this.builtinTemplatesFolder))
        webTemplateList.AddRange((IEnumerable<WebTemplate>) this.ListTemplates(this.builtinTemplatesFolder));
      else
        Debug.LogError((object) "Did not find built-in templates.");
      this.s_Templates = webTemplateList.ToArray();
      this.s_TemplateGUIThumbnails = new GUIContent[this.s_Templates.Length];
      for (int index = 0; index < this.s_TemplateGUIThumbnails.Length; ++index)
        this.s_TemplateGUIThumbnails[index] = this.s_Templates[index].ToGUIContent(this.defaultIcon);
    }

    private WebTemplate Load(string path)
    {
      if (!Directory.Exists(path) || Directory.GetFiles(path, "index.*").Length < 1)
        return (WebTemplate) null;
      string[] strArray = path.Split(new char[2]{ '/', '\\' });
      WebTemplate webTemplate = new WebTemplate();
      webTemplate.m_Name = strArray[strArray.Length - 1];
      webTemplate.m_Path = strArray.Length <= 3 || !strArray[strArray.Length - 3].Equals("Assets") ? "APPLICATION:" + webTemplate.m_Name : "PROJECT:" + webTemplate.m_Name;
      string[] files = Directory.GetFiles(path, "thumbnail.*");
      if (files.Length > 0)
      {
        webTemplate.m_Thumbnail = new Texture2D(2, 2);
        webTemplate.m_Thumbnail.LoadImage(File.ReadAllBytes(files[0]));
      }
      List<string> stringList = new List<string>();
      foreach (Capture match in new Regex("\\%UNITY_CUSTOM_([A-Z_]+)\\%").Matches(File.ReadAllText(Directory.GetFiles(path, "index.*")[0])))
      {
        string str1 = match.Value.Substring("%UNITY_CUSTOM_".Length);
        string str2 = str1.Substring(0, str1.Length - 1);
        if (!stringList.Contains(str2))
          stringList.Add(str2);
      }
      webTemplate.m_CustomKeys = stringList.ToArray();
      return webTemplate;
    }

    private List<WebTemplate> ListTemplates(string path)
    {
      List<WebTemplate> webTemplateList = new List<WebTemplate>();
      foreach (string directory in Directory.GetDirectories(path))
      {
        WebTemplate webTemplate = this.Load(directory);
        if (webTemplate != null)
          webTemplateList.Add(webTemplate);
      }
      return webTemplateList;
    }

    public void SelectionUI(SerializedProperty templateProp)
    {
      if (WebTemplateManagerBase.s_Styles == null)
        WebTemplateManagerBase.s_Styles = new WebTemplateManagerBase.Styles();
      if (this.TemplateGUIThumbnails.Length < 1)
      {
        GUILayout.Label(EditorGUIUtility.TextContent("No templates found."));
      }
      else
      {
        int maxRowItems = Mathf.Min((int) Mathf.Max((float) (((double) Screen.width - 30.0) / 80.0), 1f), this.TemplateGUIThumbnails.Length);
        int num = Mathf.Max((int) Mathf.Ceil((float) this.TemplateGUIThumbnails.Length / (float) maxRowItems), 1);
        bool changed1 = GUI.changed;
        templateProp.stringValue = this.Templates[WebTemplateManagerBase.ThumbnailList(GUILayoutUtility.GetRect((float) ((double) maxRowItems * 80.0), (float) ((double) num * 100.0), new GUILayoutOption[1]
        {
          GUILayout.ExpandWidth(false)
        }), this.GetTemplateIndex(templateProp.stringValue), this.TemplateGUIThumbnails, maxRowItems)].ToString();
        bool flag = !changed1 && GUI.changed;
        bool changed2 = GUI.changed;
        GUI.changed = false;
        foreach (string templateCustomKey in PlayerSettings.templateCustomKeys)
        {
          string templateCustomValue = PlayerSettings.GetTemplateCustomValue(templateCustomKey);
          string str = EditorGUILayout.TextField(WebTemplateManagerBase.PrettyTemplateKeyName(templateCustomKey), templateCustomValue, new GUILayoutOption[0]);
          PlayerSettings.SetTemplateCustomValue(templateCustomKey, str);
        }
        if (GUI.changed)
          templateProp.serializedObject.Update();
        GUI.changed |= changed2;
        if (!flag)
          return;
        GUIUtility.hotControl = 0;
        GUIUtility.keyboardControl = 0;
        templateProp.serializedObject.ApplyModifiedProperties();
        PlayerSettings.templateCustomKeys = this.Templates[this.GetTemplateIndex(templateProp.stringValue)].CustomKeys;
        templateProp.serializedObject.Update();
      }
    }

    private static int ThumbnailList(Rect rect, int selection, GUIContent[] thumbnails, int maxRowItems)
    {
      int num = 0;
      int index1 = 0;
      while (index1 < thumbnails.Length)
      {
        for (int index2 = 0; index2 < maxRowItems && index1 < thumbnails.Length; ++index1)
        {
          if (WebTemplateManagerBase.ThumbnailListItem(new Rect(rect.x + (float) index2 * 80f, rect.y + (float) num * 100f, 80f, 100f), index1 == selection, thumbnails[index1]))
            selection = index1;
          ++index2;
        }
        ++num;
      }
      return selection;
    }

    private static bool ThumbnailListItem(Rect rect, bool selected, GUIContent content)
    {
      switch (Event.current.type)
      {
        case EventType.MouseDown:
          if (rect.Contains(Event.current.mousePosition))
          {
            if (!selected)
              GUI.changed = true;
            selected = true;
            Event.current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          Rect position = new Rect(rect.x + 5f, rect.y + 5f, rect.width - 10f, (float) ((double) rect.height - 20.0 - 10.0));
          WebTemplateManagerBase.s_Styles.thumbnail.Draw(position, content.image, false, false, selected, selected);
          WebTemplateManagerBase.s_Styles.thumbnailLabel.Draw(new Rect(rect.x, (float) ((double) rect.y + (double) rect.height - 20.0), rect.width, 20f), content.text, false, false, selected, selected);
          break;
      }
      return selected;
    }

    private static string PrettyTemplateKeyName(string name)
    {
      string[] strArray = name.Split('_');
      strArray[0] = WebTemplateManagerBase.UppercaseFirst(strArray[0].ToLower());
      for (int index = 1; index < strArray.Length; ++index)
        strArray[index] = strArray[index].ToLower();
      return string.Join(" ", strArray);
    }

    private static string UppercaseFirst(string target)
    {
      if (string.IsNullOrEmpty(target))
        return string.Empty;
      return ((int) char.ToUpper(target[0])).ToString() + target.Substring(1);
    }

    private class Styles
    {
      public GUIStyle thumbnail = (GUIStyle) "IN ThumbnailShadow";
      public GUIStyle thumbnailLabel = (GUIStyle) "IN ThumbnailSelection";
    }
  }
}

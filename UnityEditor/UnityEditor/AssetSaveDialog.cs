// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetSaveDialog
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetSaveDialog : EditorWindow
  {
    private ListViewState m_LV = new ListViewState();
    private int m_InitialSelectedItem = -1;
    private static AssetSaveDialog.Styles s_Styles;
    private List<string> m_Assets;
    private List<string> m_AssetsToSave;
    private bool[] m_SelectedItems;
    private List<GUIContent> m_Content;

    private void SetAssets(string[] assets)
    {
      this.m_Assets = new List<string>((IEnumerable<string>) assets);
      this.RebuildLists(true);
      this.m_AssetsToSave = new List<string>();
    }

    public static void ShowWindow(string[] inAssets, out string[] assetsThatShouldBeSaved)
    {
      int length1 = 0;
      foreach (string inAsset in inAssets)
      {
        if (inAsset.EndsWith("meta"))
          ++length1;
      }
      int length2 = inAssets.Length - length1;
      if (length2 == 0)
      {
        assetsThatShouldBeSaved = inAssets;
      }
      else
      {
        string[] assets = new string[length2];
        string[] strArray = new string[length1];
        int num1 = 0;
        int num2 = 0;
        foreach (string inAsset in inAssets)
        {
          if (inAsset.EndsWith("meta"))
            strArray[num2++] = inAsset;
          else
            assets[num1++] = inAsset;
        }
        AssetSaveDialog windowDontShow = EditorWindow.GetWindowDontShow<AssetSaveDialog>();
        windowDontShow.titleContent = EditorGUIUtility.TextContent("Save Assets");
        windowDontShow.SetAssets(assets);
        windowDontShow.ShowUtility();
        windowDontShow.ShowModal();
        assetsThatShouldBeSaved = new string[windowDontShow.m_AssetsToSave.Count + num2];
        windowDontShow.m_AssetsToSave.CopyTo(assetsThatShouldBeSaved, 0);
        strArray.CopyTo((Array) assetsThatShouldBeSaved, windowDontShow.m_AssetsToSave.Count);
      }
    }

    public static GUIContent GetContentForAsset(string path)
    {
      Texture cachedIcon = AssetDatabase.GetCachedIcon(path);
      if (path.StartsWith("Library/"))
        path = ObjectNames.NicifyVariableName(AssetDatabase.LoadMainAssetAtPath(path).name);
      if (path.StartsWith("Assets/"))
        path = path.Substring(7);
      return new GUIContent(path, cachedIcon);
    }

    private void HandleKeyboard()
    {
    }

    private void OnGUI()
    {
      if (AssetSaveDialog.s_Styles == null)
      {
        AssetSaveDialog.s_Styles = new AssetSaveDialog.Styles();
        this.minSize = new Vector2(500f, 300f);
        this.position = new Rect(this.position.x, this.position.y, this.minSize.x, this.minSize.y);
      }
      this.HandleKeyboard();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      GUILayout.Label("Unity is about to save the following modified files. Unsaved changes will be lost!");
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      int row = this.m_LV.row;
      int num = 0;
      foreach (ListViewElement listViewElement in ListViewGUILayout.ListView(this.m_LV, AssetSaveDialog.s_Styles.box))
      {
        if (this.m_SelectedItems[listViewElement.row] && Event.current.type == EventType.Repaint)
        {
          Rect position = listViewElement.position;
          ++position.x;
          ++position.y;
          --position.width;
          --position.height;
          AssetSaveDialog.s_Styles.selected.Draw(position, false, false, false, false);
        }
        GUILayout.Label(this.m_Content[listViewElement.row]);
        if (ListViewGUILayout.HasMouseUp(listViewElement.position))
        {
          Event.current.command = true;
          Event.current.control = true;
          ListViewGUILayout.MultiSelection(row, listViewElement.row, ref this.m_InitialSelectedItem, ref this.m_SelectedItems);
        }
        if (this.m_SelectedItems[listViewElement.row])
          ++num;
      }
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      if (GUILayout.Button(AssetSaveDialog.s_Styles.close, AssetSaveDialog.s_Styles.button, new GUILayoutOption[1]{ GUILayout.Width(AssetSaveDialog.s_Styles.buttonWidth) }))
        this.CloseWindow();
      GUILayout.FlexibleSpace();
      GUI.enabled = num > 0;
      bool flag = num == this.m_Assets.Count;
      if (GUILayout.Button(AssetSaveDialog.s_Styles.dontSave, AssetSaveDialog.s_Styles.button, new GUILayoutOption[1]{ GUILayout.Width(AssetSaveDialog.s_Styles.buttonWidth) }))
        this.IgnoreSelectedAssets();
      if (GUILayout.Button(!flag ? AssetSaveDialog.s_Styles.saveSelected : AssetSaveDialog.s_Styles.saveAll, AssetSaveDialog.s_Styles.button, new GUILayoutOption[1]{ GUILayout.Width(AssetSaveDialog.s_Styles.buttonWidth) }))
        this.SaveSelectedAssets();
      if (this.m_Assets.Count == 0)
        this.CloseWindow();
      GUI.enabled = true;
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
    }

    private void Cancel()
    {
      this.Close();
      GUIUtility.ExitGUI();
    }

    private void CloseWindow()
    {
      this.Close();
      GUIUtility.ExitGUI();
    }

    private void SaveSelectedAssets()
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.m_SelectedItems.Length; ++index)
      {
        if (this.m_SelectedItems[index])
          this.m_AssetsToSave.Add(this.m_Assets[index]);
        else
          stringList.Add(this.m_Assets[index]);
      }
      this.m_Assets = stringList;
      this.RebuildLists(false);
    }

    private void IgnoreSelectedAssets()
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.m_SelectedItems.Length; ++index)
      {
        if (!this.m_SelectedItems[index])
          stringList.Add(this.m_Assets[index]);
      }
      this.m_Assets = stringList;
      this.RebuildLists(false);
      if (this.m_Assets.Count != 0)
        return;
      this.CloseWindow();
    }

    private void RebuildLists(bool selected)
    {
      this.m_LV.totalRows = this.m_Assets.Count;
      this.m_SelectedItems = new bool[this.m_Assets.Count];
      this.m_Content = new List<GUIContent>(this.m_Assets.Count);
      for (int index = 0; index < this.m_Assets.Count; ++index)
      {
        this.m_SelectedItems[index] = selected;
        this.m_Content.Add(AssetSaveDialog.GetContentForAsset(this.m_Assets[index]));
      }
    }

    private class Styles
    {
      public GUIStyle selected = (GUIStyle) "ServerUpdateChangesetOn";
      public GUIStyle box = (GUIStyle) "OL Box";
      public GUIStyle button = (GUIStyle) "LargeButton";
      public GUIContent saveSelected = EditorGUIUtility.TextContent("Save Selected");
      public GUIContent saveAll = EditorGUIUtility.TextContent("Save All");
      public GUIContent dontSave = EditorGUIUtility.TextContent("Don't Save");
      public GUIContent close = EditorGUIUtility.TextContent("Close");
      public float buttonWidth;

      public Styles()
      {
        this.buttonWidth = Mathf.Max(Mathf.Max(this.button.CalcSize(this.saveSelected).x, this.button.CalcSize(this.saveAll).x), this.button.CalcSize(this.dontSave).x);
      }
    }
  }
}

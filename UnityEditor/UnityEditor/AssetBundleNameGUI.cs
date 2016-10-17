// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetBundleNameGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetBundleNameGUI
  {
    private static readonly GUIContent kAssetBundleName = new GUIContent("AssetBundle");
    private static readonly int kAssetBundleNameFieldIdHash = "AssetBundleNameFieldHash".GetHashCode();
    private static readonly int kAssetBundleVariantFieldIdHash = "AssetBundleVariantFieldHash".GetHashCode();
    private bool m_ShowAssetBundleNameTextField;
    private bool m_ShowAssetBundleVariantTextField;

    public void OnAssetBundleNameGUI(IEnumerable<UnityEngine.Object> assets)
    {
      EditorGUIUtility.labelWidth = 90f;
      Rect controlRect = EditorGUILayout.GetControlRect(true, 16f, new GUILayoutOption[0]);
      Rect rect1 = controlRect;
      controlRect.width *= 0.8f;
      rect1.xMin += controlRect.width + 5f;
      int controlId1 = GUIUtility.GetControlID(AssetBundleNameGUI.kAssetBundleNameFieldIdHash, FocusType.Native, controlRect);
      Rect rect2 = EditorGUI.PrefixLabel(controlRect, controlId1, AssetBundleNameGUI.kAssetBundleName, AssetBundleNameGUI.Styles.label);
      if (this.m_ShowAssetBundleNameTextField)
        this.AssetBundleTextField(rect2, controlId1, assets, false);
      else
        this.AssetBundlePopup(rect2, controlId1, assets, false);
      int controlId2 = GUIUtility.GetControlID(AssetBundleNameGUI.kAssetBundleVariantFieldIdHash, FocusType.Native, rect1);
      if (this.m_ShowAssetBundleVariantTextField)
        this.AssetBundleTextField(rect1, controlId2, assets, true);
      else
        this.AssetBundlePopup(rect1, controlId2, assets, true);
    }

    private void ShowNewAssetBundleField(bool isVariant)
    {
      this.m_ShowAssetBundleNameTextField = !isVariant;
      this.m_ShowAssetBundleVariantTextField = isVariant;
      EditorGUIUtility.editingTextField = true;
    }

    private void AssetBundleTextField(Rect rect, int id, IEnumerable<UnityEngine.Object> assets, bool isVariant)
    {
      Color cursorColor = GUI.skin.settings.cursorColor;
      GUI.skin.settings.cursorColor = AssetBundleNameGUI.Styles.cursorColor;
      EditorGUI.BeginChangeCheck();
      string name = EditorGUI.DelayedTextFieldInternal(rect, id, GUIContent.none, string.Empty, (string) null, AssetBundleNameGUI.Styles.textField);
      if (EditorGUI.EndChangeCheck())
      {
        this.SetAssetBundleForAssets(assets, name, isVariant);
        this.ShowAssetBundlePopup();
      }
      GUI.skin.settings.cursorColor = cursorColor;
      if (EditorGUI.IsEditingTextField() || Event.current.type == EventType.Layout)
        return;
      this.ShowAssetBundlePopup();
    }

    private void ShowAssetBundlePopup()
    {
      this.m_ShowAssetBundleNameTextField = false;
      this.m_ShowAssetBundleVariantTextField = false;
    }

    private void AssetBundlePopup(Rect rect, int id, IEnumerable<UnityEngine.Object> assets, bool isVariant)
    {
      List<string> stringList = new List<string>();
      stringList.Add("None");
      stringList.Add(string.Empty);
      bool isMixed;
      IEnumerable<string> bundlesFromAssets = this.GetAssetBundlesFromAssets(assets, isVariant, out isMixed);
      string[] strArray = !isVariant ? AssetDatabase.GetAllAssetBundleNamesWithoutVariant() : AssetDatabase.GetAllAssetBundleVariants();
      stringList.AddRange((IEnumerable<string>) strArray);
      stringList.Add(string.Empty);
      int count = stringList.Count;
      stringList.Add("New...");
      int num1 = -1;
      int num2 = -1;
      if (!isVariant)
      {
        num1 = stringList.Count;
        stringList.Add("Remove Unused Names");
        num2 = stringList.Count;
        if (bundlesFromAssets.Count<string>() != 0)
          stringList.Add("Filter Selected Name" + (!isMixed ? string.Empty : "s"));
      }
      int selected = 0;
      string str = bundlesFromAssets.FirstOrDefault<string>();
      if (!string.IsNullOrEmpty(str))
        selected = stringList.IndexOf(str);
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = isMixed;
      int index = EditorGUI.DoPopup(rect, id, selected, EditorGUIUtility.TempContent(stringList.ToArray()), AssetBundleNameGUI.Styles.popup);
      EditorGUI.showMixedValue = false;
      if (!EditorGUI.EndChangeCheck())
        return;
      if (index == 0)
        this.SetAssetBundleForAssets(assets, (string) null, isVariant);
      else if (index == count)
        this.ShowNewAssetBundleField(isVariant);
      else if (index == num1)
        AssetDatabase.RemoveUnusedAssetBundleNames();
      else if (index == num2)
        this.FilterSelected(bundlesFromAssets);
      else
        this.SetAssetBundleForAssets(assets, stringList[index], isVariant);
    }

    private void FilterSelected(IEnumerable<string> assetBundleNames)
    {
      SearchFilter searchFilter = new SearchFilter();
      searchFilter.assetBundleNames = assetBundleNames.Where<string>((Func<string, bool>) (name => !string.IsNullOrEmpty(name))).ToArray<string>();
      if ((UnityEngine.Object) ProjectBrowser.s_LastInteractedProjectBrowser != (UnityEngine.Object) null)
        ProjectBrowser.s_LastInteractedProjectBrowser.SetSearch(searchFilter);
      else
        Debug.LogWarning((object) "No Project Browser found to apply AssetBundle filter.");
    }

    private IEnumerable<string> GetAssetBundlesFromAssets(IEnumerable<UnityEngine.Object> assets, bool isVariant, out bool isMixed)
    {
      HashSet<string> stringSet = new HashSet<string>();
      string str1 = (string) null;
      isMixed = false;
      foreach (UnityEngine.Object asset in assets)
      {
        if (!(asset is MonoScript))
        {
          AssetImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(asset));
          if (!((UnityEngine.Object) atPath == (UnityEngine.Object) null))
          {
            string str2 = !isVariant ? atPath.assetBundleName : atPath.assetBundleVariant;
            if (str1 != null && str1 != str2)
              isMixed = true;
            str1 = str2;
            if (!string.IsNullOrEmpty(str2))
              stringSet.Add(str2);
          }
        }
      }
      return (IEnumerable<string>) stringSet;
    }

    private void SetAssetBundleForAssets(IEnumerable<UnityEngine.Object> assets, string name, bool isVariant)
    {
      bool flag = false;
      foreach (UnityEngine.Object asset in assets)
      {
        if (!(asset is MonoScript))
        {
          AssetImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(asset));
          if (!((UnityEngine.Object) atPath == (UnityEngine.Object) null))
          {
            if (isVariant)
              atPath.assetBundleVariant = name;
            else
              atPath.assetBundleName = name;
            flag = true;
          }
        }
      }
      if (!flag)
        return;
      EditorApplication.Internal_CallAssetBundleNameChanged();
    }

    private class Styles
    {
      private static GUISkin s_DarkSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
      public static GUIStyle label = AssetBundleNameGUI.Styles.GetStyle("ControlLabel");
      public static GUIStyle popup = AssetBundleNameGUI.Styles.GetStyle("MiniPopup");
      public static GUIStyle textField = AssetBundleNameGUI.Styles.GetStyle("textField");
      public static Color cursorColor = AssetBundleNameGUI.Styles.s_DarkSkin.settings.cursorColor;

      private static GUIStyle GetStyle(string name)
      {
        return new GUIStyle(AssetBundleNameGUI.Styles.s_DarkSkin.GetStyle(name));
      }
    }
  }
}

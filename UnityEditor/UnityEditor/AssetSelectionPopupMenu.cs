// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetSelectionPopupMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetSelectionPopupMenu
  {
    public static void Show(Rect buttonRect, string[] classNames, int initialSelectedInstanceID)
    {
      GenericMenu genericMenu = new GenericMenu();
      List<UnityEngine.Object> assetsOfType = AssetSelectionPopupMenu.FindAssetsOfType(classNames);
      if (assetsOfType.Any<UnityEngine.Object>())
      {
        assetsOfType.Sort((Comparison<UnityEngine.Object>) ((result1, result2) => EditorUtility.NaturalCompare(result1.name, result2.name)));
        using (List<UnityEngine.Object>.Enumerator enumerator = assetsOfType.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            UnityEngine.Object current = enumerator.Current;
            GUIContent content = new GUIContent(current.name);
            bool on = current.GetInstanceID() == initialSelectedInstanceID;
            genericMenu.AddItem(content, on, new GenericMenu.MenuFunction2(AssetSelectionPopupMenu.SelectCallback), (object) current);
          }
        }
      }
      else
        genericMenu.AddDisabledItem(new GUIContent("No Audio Mixers found in this project"));
      genericMenu.DropDown(buttonRect);
    }

    private static void SelectCallback(object userData)
    {
      UnityEngine.Object @object = userData as UnityEngine.Object;
      if (!(@object != (UnityEngine.Object) null))
        return;
      Selection.activeInstanceID = @object.GetInstanceID();
    }

    private static List<UnityEngine.Object> FindAssetsOfType(string[] classNames)
    {
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      hierarchyProperty.SetSearchFilter(new SearchFilter()
      {
        classNames = classNames
      });
      List<UnityEngine.Object> objectList = new List<UnityEngine.Object>();
      while (hierarchyProperty.Next((int[]) null))
        objectList.Add(hierarchyProperty.pptrValue);
      return objectList;
    }
  }
}

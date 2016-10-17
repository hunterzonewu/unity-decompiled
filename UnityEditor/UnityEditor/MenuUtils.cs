// Decompiled with JetBrains decompiler
// Type: UnityEditor.MenuUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class MenuUtils
  {
    public static void MenuCallback(object callbackObject)
    {
      MenuUtils.MenuCallbackObject menuCallbackObject = callbackObject as MenuUtils.MenuCallbackObject;
      if (menuCallbackObject.onBeforeExecuteCallback != null)
        menuCallbackObject.onBeforeExecuteCallback(menuCallbackObject.menuItemPath, menuCallbackObject.temporaryContext, menuCallbackObject.userData);
      if (menuCallbackObject.temporaryContext != null)
        EditorApplication.ExecuteMenuItemWithTemporaryContext(menuCallbackObject.menuItemPath, menuCallbackObject.temporaryContext);
      else
        EditorApplication.ExecuteMenuItem(menuCallbackObject.menuItemPath);
      if (menuCallbackObject.onAfterExecuteCallback == null)
        return;
      menuCallbackObject.onAfterExecuteCallback(menuCallbackObject.menuItemPath, menuCallbackObject.temporaryContext, menuCallbackObject.userData);
    }

    public static void ExtractSubMenuWithPath(string path, GenericMenu menu, string replacementPath, UnityEngine.Object[] temporaryContext)
    {
      HashSet<string> stringSet = new HashSet<string>((IEnumerable<string>) Unsupported.GetSubmenus(path));
      foreach (string includingSeparator in Unsupported.GetSubmenusIncludingSeparators(path))
      {
        string replacementMenuString = replacementPath + includingSeparator.Substring(path.Length);
        if (stringSet.Contains(includingSeparator))
          MenuUtils.ExtractMenuItemWithPath(includingSeparator, menu, replacementMenuString, temporaryContext, -1, (System.Action<string, UnityEngine.Object[], int>) null, (System.Action<string, UnityEngine.Object[], int>) null);
      }
    }

    public static void ExtractMenuItemWithPath(string menuString, GenericMenu menu, string replacementMenuString, UnityEngine.Object[] temporaryContext, int userData, System.Action<string, UnityEngine.Object[], int> onBeforeExecuteCallback, System.Action<string, UnityEngine.Object[], int> onAfterExecuteCallback)
    {
      menu.AddItem(new GUIContent(replacementMenuString), false, new GenericMenu.MenuFunction2(MenuUtils.MenuCallback), (object) new MenuUtils.MenuCallbackObject()
      {
        menuItemPath = menuString,
        temporaryContext = temporaryContext,
        onBeforeExecuteCallback = onBeforeExecuteCallback,
        onAfterExecuteCallback = onAfterExecuteCallback,
        userData = userData
      });
    }

    private class MenuCallbackObject
    {
      public string menuItemPath;
      public UnityEngine.Object[] temporaryContext;
      public System.Action<string, UnityEngine.Object[], int> onBeforeExecuteCallback;
      public System.Action<string, UnityEngine.Object[], int> onAfterExecuteCallback;
      public int userData;
    }
  }
}

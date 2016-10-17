// Decompiled with JetBrains decompiler
// Type: UnityEditor.GenericMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>The GenericMenu lets you create a custom context and dropdown menus.</para>
  /// </summary>
  public sealed class GenericMenu
  {
    private ArrayList menuItems = new ArrayList();

    public void AddItem(GUIContent content, bool on, GenericMenu.MenuFunction func)
    {
      this.menuItems.Add((object) new GenericMenu.MenuItem(content, false, on, func));
    }

    public void AddItem(GUIContent content, bool on, GenericMenu.MenuFunction2 func, object userData)
    {
      this.menuItems.Add((object) new GenericMenu.MenuItem(content, false, on, func, userData));
    }

    /// <summary>
    ///   <para>Add a disabled item to the menu.</para>
    /// </summary>
    /// <param name="content">The GUIContent to display as a disabled menu item.</param>
    public void AddDisabledItem(GUIContent content)
    {
      this.menuItems.Add((object) new GenericMenu.MenuItem(content, false, false, (GenericMenu.MenuFunction) null));
    }

    /// <summary>
    ///   <para>Add a seperator item to the menu.</para>
    /// </summary>
    /// <param name="path">The path to the submenu, if adding a separator to a submenu. When adding a separator to the top level of a menu, use an empty string as the path.</param>
    public void AddSeparator(string path)
    {
      this.menuItems.Add((object) new GenericMenu.MenuItem(new GUIContent(path), true, false, (GenericMenu.MenuFunction) null));
    }

    /// <summary>
    ///   <para>Get number of items in the menu.</para>
    /// </summary>
    /// <returns>
    ///   <para>The number of items in the menu.</para>
    /// </returns>
    public int GetItemCount()
    {
      return this.menuItems.Count;
    }

    /// <summary>
    ///   <para>Show the menu under the mouse when right-clicked.</para>
    /// </summary>
    public void ShowAsContext()
    {
      if (Event.current == null)
        return;
      this.DropDown(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 0.0f, 0.0f));
    }

    /// <summary>
    ///   <para>Show the menu at the given screen rect.</para>
    /// </summary>
    /// <param name="position">The position at which to show the menu.</param>
    public void DropDown(Rect position)
    {
      string[] options = new string[this.menuItems.Count];
      bool[] enabled = new bool[this.menuItems.Count];
      ArrayList arrayList = new ArrayList();
      bool[] separator = new bool[this.menuItems.Count];
      for (int index = 0; index < this.menuItems.Count; ++index)
      {
        GenericMenu.MenuItem menuItem = (GenericMenu.MenuItem) this.menuItems[index];
        options[index] = menuItem.content.text;
        enabled[index] = menuItem.func != null || menuItem.func2 != null;
        separator[index] = menuItem.separator;
        if (menuItem.on)
          arrayList.Add((object) index);
      }
      EditorUtility.DisplayCustomMenuWithSeparators(position, options, enabled, separator, (int[]) arrayList.ToArray(typeof (int)), new EditorUtility.SelectMenuItemFunction(this.CatchMenu), (object) null);
    }

    internal void Popup(Rect position, int selectedIndex)
    {
      if (Application.platform == RuntimePlatform.WindowsEditor)
        this.DropDown(position);
      else
        this.DropDown(position);
    }

    private void CatchMenu(object userData, string[] options, int selected)
    {
      GenericMenu.MenuItem menuItem = (GenericMenu.MenuItem) this.menuItems[selected];
      if (menuItem.func2 != null)
      {
        menuItem.func2(menuItem.userData);
      }
      else
      {
        if (menuItem.func == null)
          return;
        menuItem.func();
      }
    }

    private sealed class MenuItem
    {
      public GUIContent content;
      public bool separator;
      public bool on;
      public GenericMenu.MenuFunction func;
      public GenericMenu.MenuFunction2 func2;
      public object userData;

      public MenuItem(GUIContent _content, bool _separator, bool _on, GenericMenu.MenuFunction _func)
      {
        this.content = _content;
        this.separator = _separator;
        this.on = _on;
        this.func = _func;
      }

      public MenuItem(GUIContent _content, bool _separator, bool _on, GenericMenu.MenuFunction2 _func, object _userData)
      {
        this.content = _content;
        this.separator = _separator;
        this.on = _on;
        this.func2 = _func;
        this.userData = _userData;
      }
    }

    /// <summary>
    ///   <para>Callback function, called when a menu item is selected.</para>
    /// </summary>
    public delegate void MenuFunction();

    /// <summary>
    ///   <para>Callback function with user data, called when a menu item is selected.</para>
    /// </summary>
    /// <param name="userData">The data to pass through to the callback function.</param>
    public delegate void MenuFunction2(object userData);
  }
}

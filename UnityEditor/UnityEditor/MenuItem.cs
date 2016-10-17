// Decompiled with JetBrains decompiler
// Type: UnityEditor.MenuItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>The MenuItem attribute allows you to add menu items to the main menu and inspector context menus.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  public sealed class MenuItem : Attribute
  {
    public string menuItem;
    public bool validate;
    public int priority;

    /// <summary>
    ///   <para>Creates a menu item and invokes the static function following it, when the menu item is selected.</para>
    /// </summary>
    /// <param name="itemName"></param>
    public MenuItem(string itemName)
      : this(itemName, false)
    {
    }

    /// <summary>
    ///   <para>Creates a menu item and invokes the static function following it, when the menu item is selected.</para>
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="isValidateFunction"></param>
    public MenuItem(string itemName, bool isValidateFunction)
      : this(itemName, isValidateFunction, !itemName.StartsWith("GameObject/Create Other") ? 1000 : 10)
    {
    }

    /// <summary>
    ///   <para>Creates a menu item and invokes the static function following it, when the menu item is selected.</para>
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="isValidateFunction"></param>
    /// <param name="priority"></param>
    public MenuItem(string itemName, bool isValidateFunction, int priority)
      : this(itemName, isValidateFunction, priority, false)
    {
    }

    internal MenuItem(string itemName, bool isValidateFunction, int priority, bool internalMenu)
    {
      this.menuItem = !internalMenu ? itemName : "internal:" + itemName;
      this.validate = isValidateFunction;
      this.priority = priority;
    }
  }
}

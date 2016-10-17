// Decompiled with JetBrains decompiler
// Type: UnityEditor.DecoratorDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Base class to derive custom decorator drawers from.</para>
  /// </summary>
  public abstract class DecoratorDrawer : GUIDrawer
  {
    internal PropertyAttribute m_Attribute;

    /// <summary>
    ///   <para>The PropertyAttribute for the decorator. (Read Only)</para>
    /// </summary>
    public PropertyAttribute attribute
    {
      get
      {
        return this.m_Attribute;
      }
    }

    /// <summary>
    ///   <para>Override this method to make your own GUI for the decorator.</para>
    /// </summary>
    /// <param name="position">Rectangle on the screen to use for the decorator GUI.</param>
    public virtual void OnGUI(Rect position)
    {
    }

    /// <summary>
    ///   <para>Override this method to specify how tall the GUI for this decorator is in pixels.</para>
    /// </summary>
    public virtual float GetHeight()
    {
      return 16f;
    }
  }
}

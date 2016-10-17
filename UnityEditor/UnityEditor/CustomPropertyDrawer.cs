// Decompiled with JetBrains decompiler
// Type: UnityEditor.CustomPropertyDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Tells a custom PropertyDrawer or DecoratorDrawer which run-time Serializable class or PropertyAttribute it's a drawer for.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
  public sealed class CustomPropertyDrawer : Attribute
  {
    internal System.Type m_Type;
    internal bool m_UseForChildren;

    /// <summary>
    ///   <para>Tells a PropertyDrawer or DecoratorDrawer class which run-time class or attribute it's a drawer for.</para>
    /// </summary>
    /// <param name="type">If the drawer is for a custom Serializable class, the type should be that class. If the drawer is for script variables with a specific PropertyAttribute, the type should be that attribute.</param>
    /// <param name="useForChildren">If true, the drawer will be used for any children of the specified class unless they define their own drawer.</param>
    public CustomPropertyDrawer(System.Type type)
    {
      this.m_Type = type;
    }

    /// <summary>
    ///   <para>Tells a PropertyDrawer or DecoratorDrawer class which run-time class or attribute it's a drawer for.</para>
    /// </summary>
    /// <param name="type">If the drawer is for a custom Serializable class, the type should be that class. If the drawer is for script variables with a specific PropertyAttribute, the type should be that attribute.</param>
    /// <param name="useForChildren">If true, the drawer will be used for any children of the specified class unless they define their own drawer.</param>
    public CustomPropertyDrawer(System.Type type, bool useForChildren)
    {
      this.m_Type = type;
      this.m_UseForChildren = useForChildren;
    }
  }
}

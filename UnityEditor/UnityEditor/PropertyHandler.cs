// Decompiled with JetBrains decompiler
// Type: UnityEditor.PropertyHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
  internal class PropertyHandler
  {
    private PropertyDrawer m_PropertyDrawer;
    private List<DecoratorDrawer> m_DecoratorDrawers;
    public string tooltip;
    public List<ContextMenuItemAttribute> contextMenuItems;

    public bool hasPropertyDrawer
    {
      get
      {
        return this.propertyDrawer != null;
      }
    }

    private PropertyDrawer propertyDrawer
    {
      get
      {
        if (this.isCurrentlyNested)
          return (PropertyDrawer) null;
        return this.m_PropertyDrawer;
      }
    }

    private bool isCurrentlyNested
    {
      get
      {
        if (this.m_PropertyDrawer != null && ScriptAttributeUtility.s_DrawerStack.Any<PropertyDrawer>())
          return this.m_PropertyDrawer == ScriptAttributeUtility.s_DrawerStack.Peek();
        return false;
      }
    }

    public bool empty
    {
      get
      {
        if (this.m_DecoratorDrawers == null && this.tooltip == null && this.propertyDrawer == null)
          return this.contextMenuItems == null;
        return false;
      }
    }

    public void HandleAttribute(PropertyAttribute attribute, System.Reflection.FieldInfo field, System.Type propertyType)
    {
      if (attribute is TooltipAttribute)
        this.tooltip = (attribute as TooltipAttribute).tooltip;
      else if (attribute is ContextMenuItemAttribute)
      {
        if (propertyType.IsArrayOrList())
          return;
        if (this.contextMenuItems == null)
          this.contextMenuItems = new List<ContextMenuItemAttribute>();
        this.contextMenuItems.Add(attribute as ContextMenuItemAttribute);
      }
      else
        this.HandleDrawnType(attribute.GetType(), propertyType, field, attribute);
    }

    public void HandleDrawnType(System.Type drawnType, System.Type propertyType, System.Reflection.FieldInfo field, PropertyAttribute attribute)
    {
      System.Type drawerTypeForType = ScriptAttributeUtility.GetDrawerTypeForType(drawnType);
      if (drawerTypeForType == null)
        return;
      if (typeof (PropertyDrawer).IsAssignableFrom(drawerTypeForType))
      {
        if (propertyType != null && propertyType.IsArrayOrList())
          return;
        this.m_PropertyDrawer = (PropertyDrawer) Activator.CreateInstance(drawerTypeForType);
        this.m_PropertyDrawer.m_FieldInfo = field;
        this.m_PropertyDrawer.m_Attribute = attribute;
      }
      else
      {
        if (!typeof (DecoratorDrawer).IsAssignableFrom(drawerTypeForType) || field != null && field.FieldType.IsArrayOrList() && !propertyType.IsArrayOrList())
          return;
        DecoratorDrawer instance = (DecoratorDrawer) Activator.CreateInstance(drawerTypeForType);
        instance.m_Attribute = attribute;
        if (this.m_DecoratorDrawers == null)
          this.m_DecoratorDrawers = new List<DecoratorDrawer>();
        this.m_DecoratorDrawers.Add(instance);
      }
    }

    public bool OnGUI(Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
    {
      float height = position.height;
      position.height = 0.0f;
      if (this.m_DecoratorDrawers != null && !this.isCurrentlyNested)
      {
        using (List<DecoratorDrawer>.Enumerator enumerator = this.m_DecoratorDrawers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            DecoratorDrawer current = enumerator.Current;
            position.height = current.GetHeight();
            float labelWidth = EditorGUIUtility.labelWidth;
            float fieldWidth = EditorGUIUtility.fieldWidth;
            current.OnGUI(position);
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUIUtility.fieldWidth = fieldWidth;
            position.y += position.height;
            height -= position.height;
          }
        }
      }
      position.height = height;
      if (this.propertyDrawer != null)
      {
        float labelWidth = EditorGUIUtility.labelWidth;
        float fieldWidth = EditorGUIUtility.fieldWidth;
        this.propertyDrawer.OnGUISafe(position, property.Copy(), label ?? EditorGUIUtility.TempContent(property.displayName));
        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUIUtility.fieldWidth = fieldWidth;
        return false;
      }
      if (!includeChildren)
        return EditorGUI.DefaultPropertyField(position, property, label);
      Vector2 iconSize = EditorGUIUtility.GetIconSize();
      bool enabled = GUI.enabled;
      int indentLevel = EditorGUI.indentLevel;
      int num = indentLevel - property.depth;
      SerializedProperty serializedProperty = property.Copy();
      SerializedProperty endProperty = serializedProperty.GetEndProperty();
      position.height = EditorGUI.GetSinglePropertyHeight(serializedProperty, label);
      EditorGUI.indentLevel = serializedProperty.depth + num;
      bool enterChildren = EditorGUI.DefaultPropertyField(position, serializedProperty, label) && EditorGUI.HasVisibleChildFields(serializedProperty);
      position.y += position.height + 2f;
      while (serializedProperty.NextVisible(enterChildren) && !SerializedProperty.EqualContents(serializedProperty, endProperty))
      {
        EditorGUI.indentLevel = serializedProperty.depth + num;
        position.height = EditorGUI.GetPropertyHeight(serializedProperty, (GUIContent) null, false);
        EditorGUI.BeginChangeCheck();
        enterChildren = ScriptAttributeUtility.GetHandler(serializedProperty).OnGUI(position, serializedProperty, (GUIContent) null, false) && EditorGUI.HasVisibleChildFields(serializedProperty);
        if (!EditorGUI.EndChangeCheck())
          position.y += position.height + 2f;
        else
          break;
      }
      GUI.enabled = enabled;
      EditorGUIUtility.SetIconSize(iconSize);
      EditorGUI.indentLevel = indentLevel;
      return false;
    }

    public bool OnGUILayout(SerializedProperty property, GUIContent label, bool includeChildren, params GUILayoutOption[] options)
    {
      Rect position = property.propertyType != SerializedPropertyType.Boolean || this.propertyDrawer != null || this.m_DecoratorDrawers != null && this.m_DecoratorDrawers.Count != 0 ? EditorGUILayout.GetControlRect(EditorGUI.LabelHasContent(label), this.GetHeight(property, label, includeChildren), options) : EditorGUILayout.GetToggleRect(true, options);
      EditorGUILayout.s_LastRect = position;
      return this.OnGUI(position, property, label, includeChildren);
    }

    public float GetHeight(SerializedProperty property, GUIContent label, bool includeChildren)
    {
      float num1 = 0.0f;
      if (this.m_DecoratorDrawers != null && !this.isCurrentlyNested)
      {
        using (List<DecoratorDrawer>.Enumerator enumerator = this.m_DecoratorDrawers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            DecoratorDrawer current = enumerator.Current;
            num1 += current.GetHeight();
          }
        }
      }
      float num2;
      if (this.propertyDrawer != null)
        num2 = num1 + this.propertyDrawer.GetPropertyHeightSafe(property.Copy(), label ?? EditorGUIUtility.TempContent(property.displayName));
      else if (!includeChildren)
      {
        num2 = num1 + EditorGUI.GetSinglePropertyHeight(property, label);
      }
      else
      {
        property = property.Copy();
        SerializedProperty endProperty = property.GetEndProperty();
        num2 = num1 + EditorGUI.GetSinglePropertyHeight(property, label);
        bool enterChildren = property.isExpanded && EditorGUI.HasVisibleChildFields(property);
        while (property.NextVisible(enterChildren) && !SerializedProperty.EqualContents(property, endProperty))
        {
          float num3 = num2 + ScriptAttributeUtility.GetHandler(property).GetHeight(property, EditorGUIUtility.TempContent(property.displayName), true);
          enterChildren = false;
          num2 = num3 + 2f;
        }
      }
      return num2;
    }

    public void AddMenuItems(SerializedProperty property, GenericMenu menu)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PropertyHandler.\u003CAddMenuItems\u003Ec__AnonStoreyAE itemsCAnonStoreyAe = new PropertyHandler.\u003CAddMenuItems\u003Ec__AnonStoreyAE();
      // ISSUE: reference to a compiler-generated field
      itemsCAnonStoreyAe.property = property;
      // ISSUE: reference to a compiler-generated field
      itemsCAnonStoreyAe.\u003C\u003Ef__this = this;
      if (this.contextMenuItems == null)
        return;
      // ISSUE: reference to a compiler-generated field
      System.Type type = itemsCAnonStoreyAe.property.serializedObject.targetObject.GetType();
      using (List<ContextMenuItemAttribute>.Enumerator enumerator = this.contextMenuItems.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ContextMenuItemAttribute current = enumerator.Current;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          PropertyHandler.\u003CAddMenuItems\u003Ec__AnonStoreyAF itemsCAnonStoreyAf = new PropertyHandler.\u003CAddMenuItems\u003Ec__AnonStoreyAF();
          // ISSUE: reference to a compiler-generated field
          itemsCAnonStoreyAf.\u003C\u003Ef__ref\u0024174 = itemsCAnonStoreyAe;
          // ISSUE: reference to a compiler-generated field
          itemsCAnonStoreyAf.\u003C\u003Ef__this = this;
          // ISSUE: reference to a compiler-generated field
          itemsCAnonStoreyAf.method = type.GetMethod(current.function, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
          // ISSUE: reference to a compiler-generated field
          if (itemsCAnonStoreyAf.method != null)
          {
            // ISSUE: reference to a compiler-generated method
            menu.AddItem(new GUIContent(current.name), false, new GenericMenu.MenuFunction(itemsCAnonStoreyAf.\u003C\u003Em__1F9));
          }
        }
      }
    }

    public void CallMenuCallback(object[] targets, MethodInfo method)
    {
      foreach (object target in targets)
        method.Invoke(target, new object[0]);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.ScriptAttributeUtility
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
  internal class ScriptAttributeUtility
  {
    internal static Stack<PropertyDrawer> s_DrawerStack = new Stack<PropertyDrawer>();
    private static Dictionary<System.Type, ScriptAttributeUtility.DrawerKeySet> s_DrawerTypeForType = (Dictionary<System.Type, ScriptAttributeUtility.DrawerKeySet>) null;
    private static Dictionary<string, List<PropertyAttribute>> s_BuiltinAttributes = (Dictionary<string, List<PropertyAttribute>>) null;
    private static PropertyHandler s_SharedNullHandler = new PropertyHandler();
    private static PropertyHandler s_NextHandler = new PropertyHandler();
    private static PropertyHandlerCache s_GlobalCache = new PropertyHandlerCache();
    private static PropertyHandlerCache s_CurrentCache = (PropertyHandlerCache) null;

    internal static PropertyHandlerCache propertyHandlerCache
    {
      get
      {
        return ScriptAttributeUtility.s_CurrentCache ?? ScriptAttributeUtility.s_GlobalCache;
      }
      set
      {
        ScriptAttributeUtility.s_CurrentCache = value;
      }
    }

    internal static void ClearGlobalCache()
    {
      ScriptAttributeUtility.s_GlobalCache.Clear();
    }

    private static void PopulateBuiltinAttributes()
    {
      ScriptAttributeUtility.s_BuiltinAttributes = new Dictionary<string, List<PropertyAttribute>>();
      ScriptAttributeUtility.AddBuiltinAttribute("GUIText", "m_Text", (PropertyAttribute) new MultilineAttribute());
      ScriptAttributeUtility.AddBuiltinAttribute("TextMesh", "m_Text", (PropertyAttribute) new MultilineAttribute());
    }

    private static void AddBuiltinAttribute(string componentTypeName, string propertyPath, PropertyAttribute attr)
    {
      string key = componentTypeName + "_" + propertyPath;
      if (!ScriptAttributeUtility.s_BuiltinAttributes.ContainsKey(key))
        ScriptAttributeUtility.s_BuiltinAttributes.Add(key, new List<PropertyAttribute>());
      ScriptAttributeUtility.s_BuiltinAttributes[key].Add(attr);
    }

    private static List<PropertyAttribute> GetBuiltinAttributes(SerializedProperty property)
    {
      if (property.serializedObject.targetObject == (UnityEngine.Object) null)
        return (List<PropertyAttribute>) null;
      System.Type type = property.serializedObject.targetObject.GetType();
      if (type == null)
        return (List<PropertyAttribute>) null;
      string key = type.Name + "_" + property.propertyPath;
      List<PropertyAttribute> propertyAttributeList = (List<PropertyAttribute>) null;
      ScriptAttributeUtility.s_BuiltinAttributes.TryGetValue(key, out propertyAttributeList);
      return propertyAttributeList;
    }

    private static void BuildDrawerTypeForTypeDictionary()
    {
      ScriptAttributeUtility.s_DrawerTypeForType = new Dictionary<System.Type, ScriptAttributeUtility.DrawerKeySet>();
      System.Type[] array = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany<Assembly, System.Type>((Func<Assembly, IEnumerable<System.Type>>) (x => (IEnumerable<System.Type>) AssemblyHelper.GetTypesFromAssembly(x))).ToArray<System.Type>();
      foreach (System.Type type in EditorAssemblies.SubclassesOf(typeof (GUIDrawer)))
      {
        object[] customAttributes = type.GetCustomAttributes(typeof (CustomPropertyDrawer), true);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ScriptAttributeUtility.\u003CBuildDrawerTypeForTypeDictionary\u003Ec__AnonStoreyB0 dictionaryCAnonStoreyB0 = new ScriptAttributeUtility.\u003CBuildDrawerTypeForTypeDictionary\u003Ec__AnonStoreyB0();
        foreach (CustomPropertyDrawer customPropertyDrawer in customAttributes)
        {
          // ISSUE: reference to a compiler-generated field
          dictionaryCAnonStoreyB0.editor = customPropertyDrawer;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ScriptAttributeUtility.s_DrawerTypeForType[dictionaryCAnonStoreyB0.editor.m_Type] = new ScriptAttributeUtility.DrawerKeySet()
          {
            drawer = type,
            type = dictionaryCAnonStoreyB0.editor.m_Type
          };
          // ISSUE: reference to a compiler-generated field
          if (dictionaryCAnonStoreyB0.editor.m_UseForChildren)
          {
            // ISSUE: reference to a compiler-generated method
            foreach (System.Type key in ((IEnumerable<System.Type>) array).Where<System.Type>(new Func<System.Type, bool>(dictionaryCAnonStoreyB0.\u003C\u003Em__1FB)))
            {
              // ISSUE: reference to a compiler-generated field
              if (!ScriptAttributeUtility.s_DrawerTypeForType.ContainsKey(key) || !dictionaryCAnonStoreyB0.editor.m_Type.IsAssignableFrom(ScriptAttributeUtility.s_DrawerTypeForType[key].type))
              {
                // ISSUE: reference to a compiler-generated field
                ScriptAttributeUtility.s_DrawerTypeForType[key] = new ScriptAttributeUtility.DrawerKeySet()
                {
                  drawer = type,
                  type = dictionaryCAnonStoreyB0.editor.m_Type
                };
              }
            }
          }
        }
      }
    }

    internal static System.Type GetDrawerTypeForType(System.Type type)
    {
      if (ScriptAttributeUtility.s_DrawerTypeForType == null)
        ScriptAttributeUtility.BuildDrawerTypeForTypeDictionary();
      ScriptAttributeUtility.DrawerKeySet drawerKeySet;
      ScriptAttributeUtility.s_DrawerTypeForType.TryGetValue(type, out drawerKeySet);
      if (drawerKeySet.drawer != null || !type.IsGenericType)
        return drawerKeySet.drawer;
      ScriptAttributeUtility.s_DrawerTypeForType.TryGetValue(type.GetGenericTypeDefinition(), out drawerKeySet);
      return drawerKeySet.drawer;
    }

    private static List<PropertyAttribute> GetFieldAttributes(System.Reflection.FieldInfo field)
    {
      if (field == null)
        return (List<PropertyAttribute>) null;
      object[] customAttributes = field.GetCustomAttributes(typeof (PropertyAttribute), true);
      if (customAttributes != null && customAttributes.Length > 0)
        return new List<PropertyAttribute>((IEnumerable<PropertyAttribute>) ((IEnumerable<object>) customAttributes).Select<object, PropertyAttribute>((Func<object, PropertyAttribute>) (e => e as PropertyAttribute)).OrderBy<PropertyAttribute, int>((Func<PropertyAttribute, int>) (e => -e.order)));
      return (List<PropertyAttribute>) null;
    }

    private static System.Reflection.FieldInfo GetFieldInfoFromProperty(SerializedProperty property, out System.Type type)
    {
      System.Type typeFromProperty = ScriptAttributeUtility.GetScriptTypeFromProperty(property);
      if (typeFromProperty != null)
        return ScriptAttributeUtility.GetFieldInfoFromPropertyPath(typeFromProperty, property.propertyPath, out type);
      type = (System.Type) null;
      return (System.Reflection.FieldInfo) null;
    }

    private static System.Type GetScriptTypeFromProperty(SerializedProperty property)
    {
      SerializedProperty property1 = property.serializedObject.FindProperty("m_Script");
      if (property1 == null)
        return (System.Type) null;
      MonoScript objectReferenceValue = property1.objectReferenceValue as MonoScript;
      if ((UnityEngine.Object) objectReferenceValue == (UnityEngine.Object) null)
        return (System.Type) null;
      return objectReferenceValue.GetClass();
    }

    private static System.Reflection.FieldInfo GetFieldInfoFromPropertyPath(System.Type host, string path, out System.Type type)
    {
      System.Reflection.FieldInfo fieldInfo1 = (System.Reflection.FieldInfo) null;
      type = host;
      string[] strArray = path.Split('.');
      for (int index = 0; index < strArray.Length; ++index)
      {
        string name = strArray[index];
        if (index < strArray.Length - 1 && name == "Array" && strArray[index + 1].StartsWith("data["))
        {
          if (type.IsArrayOrList())
            type = type.GetArrayOrListElementType();
          ++index;
        }
        else
        {
          System.Reflection.FieldInfo fieldInfo2 = (System.Reflection.FieldInfo) null;
          for (System.Type type1 = type; fieldInfo2 == null && type1 != null; type1 = type1.BaseType)
            fieldInfo2 = type1.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
          if (fieldInfo2 == null)
          {
            type = (System.Type) null;
            return (System.Reflection.FieldInfo) null;
          }
          fieldInfo1 = fieldInfo2;
          type = fieldInfo1.FieldType;
        }
      }
      return fieldInfo1;
    }

    internal static PropertyHandler GetHandler(SerializedProperty property)
    {
      if (property == null || property.serializedObject.inspectorMode != InspectorMode.Normal)
        return ScriptAttributeUtility.s_SharedNullHandler;
      PropertyHandler handler1 = ScriptAttributeUtility.propertyHandlerCache.GetHandler(property);
      if (handler1 != null)
        return handler1;
      System.Type type = (System.Type) null;
      List<PropertyAttribute> propertyAttributeList = (List<PropertyAttribute>) null;
      System.Reflection.FieldInfo field = (System.Reflection.FieldInfo) null;
      UnityEngine.Object targetObject = property.serializedObject.targetObject;
      if (targetObject is MonoBehaviour || targetObject is ScriptableObject)
      {
        field = ScriptAttributeUtility.GetFieldInfoFromProperty(property, out type);
        propertyAttributeList = ScriptAttributeUtility.GetFieldAttributes(field);
      }
      else
      {
        if (ScriptAttributeUtility.s_BuiltinAttributes == null)
          ScriptAttributeUtility.PopulateBuiltinAttributes();
        if (propertyAttributeList == null)
          propertyAttributeList = ScriptAttributeUtility.GetBuiltinAttributes(property);
      }
      PropertyHandler handler2 = ScriptAttributeUtility.s_NextHandler;
      if (propertyAttributeList != null)
      {
        for (int index = propertyAttributeList.Count - 1; index >= 0; --index)
          handler2.HandleAttribute(propertyAttributeList[index], field, type);
      }
      if (!handler2.hasPropertyDrawer && type != null)
        handler2.HandleDrawnType(type, type, field, (PropertyAttribute) null);
      if (handler2.empty)
      {
        ScriptAttributeUtility.propertyHandlerCache.SetHandler(property, ScriptAttributeUtility.s_SharedNullHandler);
        handler2 = ScriptAttributeUtility.s_SharedNullHandler;
      }
      else
      {
        ScriptAttributeUtility.propertyHandlerCache.SetHandler(property, handler2);
        ScriptAttributeUtility.s_NextHandler = new PropertyHandler();
      }
      return handler2;
    }

    private struct DrawerKeySet
    {
      public System.Type drawer;
      public System.Type type;
    }
  }
}

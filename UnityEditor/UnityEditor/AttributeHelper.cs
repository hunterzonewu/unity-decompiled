// Decompiled with JetBrains decompiler
// Type: UnityEditor.AttributeHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
  internal class AttributeHelper
  {
    private static AttributeHelper.MonoGizmoMethod[] ExtractGizmos(Assembly assembly)
    {
      List<AttributeHelper.MonoGizmoMethod> monoGizmoMethodList = new List<AttributeHelper.MonoGizmoMethod>();
      foreach (System.Type type in AssemblyHelper.GetTypesFromAssembly(assembly))
      {
        MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        for (int index = 0; index < methods.GetLength(0); ++index)
        {
          MethodInfo methodInfo = methods[index];
          foreach (DrawGizmo customAttribute in methodInfo.GetCustomAttributes(typeof (DrawGizmo), false))
          {
            System.Reflection.ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Length != 2)
            {
              UnityEngine.Debug.LogWarning((object) string.Format("Method {0}.{1} is marked with the DrawGizmo attribute but does not take parameters (ComponentType, GizmoType) so will be ignored.", (object) methodInfo.DeclaringType.FullName, (object) methodInfo.Name));
            }
            else
            {
              AttributeHelper.MonoGizmoMethod monoGizmoMethod = new AttributeHelper.MonoGizmoMethod();
              if (customAttribute.drawnType == null)
                monoGizmoMethod.drawnType = parameters[0].ParameterType;
              else if (parameters[0].ParameterType.IsAssignableFrom(customAttribute.drawnType))
              {
                monoGizmoMethod.drawnType = customAttribute.drawnType;
              }
              else
              {
                UnityEngine.Debug.LogWarning((object) string.Format("Method {0}.{1} is marked with the DrawGizmo attribute but the component type it applies to could not be determined.", (object) methodInfo.DeclaringType.FullName, (object) methodInfo.Name));
                continue;
              }
              if (parameters[1].ParameterType != typeof (GizmoType) && parameters[1].ParameterType != typeof (int))
              {
                UnityEngine.Debug.LogWarning((object) string.Format("Method {0}.{1} is marked with the DrawGizmo attribute but does not take a second parameter of type GizmoType so will be ignored.", (object) methodInfo.DeclaringType.FullName, (object) methodInfo.Name));
              }
              else
              {
                monoGizmoMethod.drawGizmo = methodInfo;
                monoGizmoMethod.options = (int) customAttribute.drawOptions;
                monoGizmoMethodList.Add(monoGizmoMethod);
              }
            }
          }
        }
      }
      return monoGizmoMethodList.ToArray();
    }

    private static AttributeHelper.MonoMenuItem[] ExtractMenuCommands(Assembly assembly)
    {
      bool flag = EditorPrefs.GetBool("InternalMode", false);
      Dictionary<string, AttributeHelper.MonoMenuItem> dictionary = new Dictionary<string, AttributeHelper.MonoMenuItem>();
      foreach (System.Type type in AssemblyHelper.GetTypesFromAssembly(assembly))
      {
        MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        for (int index = 0; index < methods.GetLength(0); ++index)
        {
          MethodInfo methodInfo = methods[index];
          foreach (MenuItem customAttribute in methodInfo.GetCustomAttributes(typeof (MenuItem), false))
          {
            AttributeHelper.MonoMenuItem monoMenuItem = !dictionary.ContainsKey(customAttribute.menuItem) ? new AttributeHelper.MonoMenuItem() : dictionary[customAttribute.menuItem];
            if (customAttribute.menuItem.StartsWith("internal:", StringComparison.Ordinal))
            {
              if (flag)
                monoMenuItem.menuItem = customAttribute.menuItem.Substring(9);
              else
                continue;
            }
            else
              monoMenuItem.menuItem = customAttribute.menuItem;
            monoMenuItem.type = type;
            if (customAttribute.validate)
            {
              monoMenuItem.validate = methodInfo.Name;
            }
            else
            {
              monoMenuItem.execute = methodInfo.Name;
              monoMenuItem.index = index;
              monoMenuItem.priority = customAttribute.priority;
            }
            dictionary[customAttribute.menuItem] = monoMenuItem;
          }
        }
      }
      AttributeHelper.MonoMenuItem[] array = dictionary.Values.ToArray<AttributeHelper.MonoMenuItem>();
      Array.Sort((Array) array, (IComparer) new AttributeHelper.CompareMenuIndex());
      return array;
    }

    private static AttributeHelper.MonoMenuItem[] ExtractContextMenu(System.Type klass)
    {
      Dictionary<string, AttributeHelper.MonoMenuItem> dictionary = new Dictionary<string, AttributeHelper.MonoMenuItem>();
      MethodInfo[] methods = klass.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      for (int index = 0; index < methods.GetLength(0); ++index)
      {
        MethodInfo methodInfo = methods[index];
        foreach (ContextMenu customAttribute in methodInfo.GetCustomAttributes(typeof (ContextMenu), false))
        {
          AttributeHelper.MonoMenuItem monoMenuItem = !dictionary.ContainsKey(customAttribute.menuItem) ? new AttributeHelper.MonoMenuItem() : dictionary[customAttribute.menuItem];
          monoMenuItem.menuItem = customAttribute.menuItem;
          monoMenuItem.type = klass;
          monoMenuItem.execute = methodInfo.Name;
          dictionary[customAttribute.menuItem] = monoMenuItem;
        }
      }
      return dictionary.Values.ToArray<AttributeHelper.MonoMenuItem>();
    }

    private static string GetComponentMenuName(System.Type klass)
    {
      object[] customAttributes = klass.GetCustomAttributes(typeof (AddComponentMenu), false);
      if (customAttributes.Length > 0)
        return ((AddComponentMenu) customAttributes[0]).componentMenu;
      return (string) null;
    }

    private static int GetComponentMenuOrdering(System.Type klass)
    {
      object[] customAttributes = klass.GetCustomAttributes(typeof (AddComponentMenu), false);
      if (customAttributes.Length > 0)
        return ((AddComponentMenu) customAttributes[0]).componentOrder;
      return 0;
    }

    private static AttributeHelper.MonoCreateAssetItem[] ExtractCreateAssetMenuItems(Assembly assembly)
    {
      List<AttributeHelper.MonoCreateAssetItem> monoCreateAssetItemList = new List<AttributeHelper.MonoCreateAssetItem>();
      foreach (System.Type type in AssemblyHelper.GetTypesFromAssembly(assembly))
      {
        CreateAssetMenuAttribute customAttribute = (CreateAssetMenuAttribute) Attribute.GetCustomAttribute((MemberInfo) type, typeof (CreateAssetMenuAttribute));
        if (customAttribute != null)
        {
          if (!type.IsSubclassOf(typeof (ScriptableObject)))
          {
            UnityEngine.Debug.LogWarningFormat("CreateAssetMenu attribute on {0} will be ignored as {0} is not derived from ScriptableObject.", (object) type.FullName);
          }
          else
          {
            string str = !string.IsNullOrEmpty(customAttribute.menuName) ? customAttribute.menuName : ObjectNames.NicifyVariableName(type.Name);
            string path = !string.IsNullOrEmpty(customAttribute.fileName) ? customAttribute.fileName : "New " + ObjectNames.NicifyVariableName(type.Name) + ".asset";
            if (!Path.HasExtension(path))
              path += ".asset";
            monoCreateAssetItemList.Add(new AttributeHelper.MonoCreateAssetItem()
            {
              menuItem = str,
              fileName = path,
              order = customAttribute.order,
              type = type
            });
          }
        }
      }
      return monoCreateAssetItemList.ToArray();
    }

    internal static ArrayList FindEditorClassesWithAttribute(System.Type attrib)
    {
      ArrayList arrayList = new ArrayList();
      foreach (System.Type loadedType in EditorAssemblies.loadedTypes)
      {
        if (loadedType.GetCustomAttributes(attrib, false).Length != 0)
          arrayList.Add((object) loadedType);
      }
      return arrayList;
    }

    internal static object InvokeMemberIfAvailable(object target, string methodName, object[] args)
    {
      MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (method != null)
        return method.Invoke(target, args);
      return (object) null;
    }

    internal static bool GameObjectContainsAttribute(GameObject go, System.Type attributeType)
    {
      foreach (Component component in go.GetComponents(typeof (Component)))
      {
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.GetType().GetCustomAttributes(attributeType, true).Length > 0)
          return true;
      }
      return false;
    }

    [DebuggerHidden]
    internal static IEnumerable<T> CallMethodsWithAttribute<T>(System.Type attributeType, params object[] arguments)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AttributeHelper.\u003CCallMethodsWithAttribute\u003Ec__Iterator3<T> attributeCIterator3 = new AttributeHelper.\u003CCallMethodsWithAttribute\u003Ec__Iterator3<T>()
      {
        attributeType = attributeType,
        arguments = arguments,
        \u003C\u0024\u003EattributeType = attributeType,
        \u003C\u0024\u003Earguments = arguments
      };
      // ISSUE: reference to a compiler-generated field
      attributeCIterator3.\u0024PC = -2;
      return (IEnumerable<T>) attributeCIterator3;
    }

    internal static string GetHelpURLFromAttribute(System.Type objectType)
    {
      HelpURLAttribute customAttribute = (HelpURLAttribute) Attribute.GetCustomAttribute((MemberInfo) objectType, typeof (HelpURLAttribute));
      if (customAttribute != null)
        return customAttribute.URL;
      return (string) null;
    }

    private struct MonoGizmoMethod
    {
      public MethodInfo drawGizmo;
      public System.Type drawnType;
      public int options;
    }

    private struct MonoMenuItem
    {
      public string menuItem;
      public string execute;
      public string validate;
      public int priority;
      public int index;
      public System.Type type;
    }

    internal class CompareMenuIndex : IComparer
    {
      int IComparer.Compare(object xo, object yo)
      {
        AttributeHelper.MonoMenuItem monoMenuItem1 = (AttributeHelper.MonoMenuItem) xo;
        AttributeHelper.MonoMenuItem monoMenuItem2 = (AttributeHelper.MonoMenuItem) yo;
        if (monoMenuItem1.priority != monoMenuItem2.priority)
          return monoMenuItem1.priority.CompareTo(monoMenuItem2.priority);
        return monoMenuItem1.index.CompareTo(monoMenuItem2.index);
      }
    }

    private struct MonoCreateAssetItem
    {
      public string menuItem;
      public string fileName;
      public int order;
      public System.Type type;
    }
  }
}

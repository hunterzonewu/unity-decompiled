// Decompiled with JetBrains decompiler
// Type: UnityEditor.CustomEditorAttributes
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
  internal class CustomEditorAttributes
  {
    private static readonly List<CustomEditorAttributes.MonoEditorType> kSCustomEditors = new List<CustomEditorAttributes.MonoEditorType>();
    private static readonly List<CustomEditorAttributes.MonoEditorType> kSCustomMultiEditors = new List<CustomEditorAttributes.MonoEditorType>();
    private static bool s_Initialized;

    internal static System.Type FindCustomEditorType(UnityEngine.Object o, bool multiEdit)
    {
      return CustomEditorAttributes.FindCustomEditorTypeByType(o.GetType(), multiEdit);
    }

    internal static System.Type FindCustomEditorTypeByType(System.Type type, bool multiEdit)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CustomEditorAttributes.\u003CFindCustomEditorTypeByType\u003Ec__AnonStorey35 typeCAnonStorey35 = new CustomEditorAttributes.\u003CFindCustomEditorTypeByType\u003Ec__AnonStorey35();
      // ISSUE: reference to a compiler-generated field
      typeCAnonStorey35.type = type;
      if (!CustomEditorAttributes.s_Initialized)
      {
        Assembly[] loadedAssemblies = EditorAssemblies.loadedAssemblies;
        for (int index = loadedAssemblies.Length - 1; index >= 0; --index)
          CustomEditorAttributes.Rebuild(loadedAssemblies[index]);
        CustomEditorAttributes.s_Initialized = true;
      }
      List<CustomEditorAttributes.MonoEditorType> source = !multiEdit ? CustomEditorAttributes.kSCustomEditors : CustomEditorAttributes.kSCustomMultiEditors;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CustomEditorAttributes.\u003CFindCustomEditorTypeByType\u003Ec__AnonStorey37 typeCAnonStorey37 = new CustomEditorAttributes.\u003CFindCustomEditorTypeByType\u003Ec__AnonStorey37();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (typeCAnonStorey37.pass = 0; typeCAnonStorey37.pass < 2; typeCAnonStorey37.pass = typeCAnonStorey37.pass + 1)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CustomEditorAttributes.\u003CFindCustomEditorTypeByType\u003Ec__AnonStorey36 typeCAnonStorey36 = new CustomEditorAttributes.\u003CFindCustomEditorTypeByType\u003Ec__AnonStorey36();
        // ISSUE: reference to a compiler-generated field
        typeCAnonStorey36.\u003C\u003Ef__ref\u002453 = typeCAnonStorey35;
        // ISSUE: reference to a compiler-generated field
        typeCAnonStorey36.\u003C\u003Ef__ref\u002455 = typeCAnonStorey37;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (typeCAnonStorey36.inspected = typeCAnonStorey35.type; typeCAnonStorey36.inspected != null; typeCAnonStorey36.inspected = typeCAnonStorey36.inspected.BaseType)
        {
          // ISSUE: reference to a compiler-generated method
          CustomEditorAttributes.MonoEditorType monoEditorType = source.FirstOrDefault<CustomEditorAttributes.MonoEditorType>(new Func<CustomEditorAttributes.MonoEditorType, bool>(typeCAnonStorey36.\u003C\u003Em__4C));
          if (monoEditorType != null)
            return monoEditorType.m_InspectorType;
        }
      }
      return (System.Type) null;
    }

    internal static void Rebuild(Assembly assembly)
    {
      foreach (System.Type type in AssemblyHelper.GetTypesFromAssembly(assembly))
      {
        foreach (CustomEditor customAttribute in type.GetCustomAttributes(typeof (CustomEditor), false))
        {
          CustomEditorAttributes.MonoEditorType monoEditorType = new CustomEditorAttributes.MonoEditorType();
          if (customAttribute.m_InspectedType == null)
            Debug.Log((object) ("Can't load custom inspector " + type.Name + " because the inspected type is null."));
          else if (!type.IsSubclassOf(typeof (Editor)))
          {
            if (!(type.FullName == "TweakMode") || !type.IsEnum || !(customAttribute.m_InspectedType.FullName == "BloomAndFlares"))
              Debug.LogWarning((object) (type.Name + " uses the CustomEditor attribute but does not inherit from Editor.\nYou must inherit from Editor. See the Editor class script documentation."));
          }
          else
          {
            monoEditorType.m_InspectedType = customAttribute.m_InspectedType;
            monoEditorType.m_InspectorType = type;
            monoEditorType.m_EditorForChildClasses = customAttribute.m_EditorForChildClasses;
            monoEditorType.m_IsFallback = customAttribute.isFallback;
            CustomEditorAttributes.kSCustomEditors.Add(monoEditorType);
            if (type.GetCustomAttributes(typeof (CanEditMultipleObjects), false).Length > 0)
              CustomEditorAttributes.kSCustomMultiEditors.Add(monoEditorType);
          }
        }
      }
    }

    private class MonoEditorType
    {
      public System.Type m_InspectedType;
      public System.Type m_InspectorType;
      public bool m_EditorForChildClasses;
      public bool m_IsFallback;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEngine.AttributeHelperEngine
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UnityEngine
{
  internal class AttributeHelperEngine
  {
    [RequiredByNativeCode]
    private static System.Type GetParentTypeDisallowingMultipleInclusion(System.Type type)
    {
      Stack<System.Type> typeStack = new Stack<System.Type>();
      for (; type != null && type != typeof (MonoBehaviour); type = type.BaseType)
        typeStack.Push(type);
      while (typeStack.Count > 0)
      {
        System.Type type1 = typeStack.Pop();
        if (type1.GetCustomAttributes(typeof (DisallowMultipleComponent), false).Length != 0)
          return type1;
      }
      return (System.Type) null;
    }

    [RequiredByNativeCode]
    private static System.Type[] GetRequiredComponents(System.Type klass)
    {
      List<System.Type> typeList = (List<System.Type>) null;
      System.Type baseType;
      for (; klass != null && klass != typeof (MonoBehaviour); klass = baseType)
      {
        RequireComponent[] customAttributes = (RequireComponent[]) klass.GetCustomAttributes(typeof (RequireComponent), false);
        baseType = klass.BaseType;
        foreach (RequireComponent requireComponent in customAttributes)
        {
          if (typeList == null && customAttributes.Length == 1 && baseType == typeof (MonoBehaviour))
            return new System.Type[3]{ requireComponent.m_Type0, requireComponent.m_Type1, requireComponent.m_Type2 };
          if (typeList == null)
            typeList = new List<System.Type>();
          if (requireComponent.m_Type0 != null)
            typeList.Add(requireComponent.m_Type0);
          if (requireComponent.m_Type1 != null)
            typeList.Add(requireComponent.m_Type1);
          if (requireComponent.m_Type2 != null)
            typeList.Add(requireComponent.m_Type2);
        }
      }
      if (typeList == null)
        return (System.Type[]) null;
      return typeList.ToArray();
    }

    [RequiredByNativeCode]
    private static bool CheckIsEditorScript(System.Type klass)
    {
      for (; klass != null && klass != typeof (MonoBehaviour); klass = klass.BaseType)
      {
        if (klass.GetCustomAttributes(typeof (ExecuteInEditMode), false).Length != 0)
          return true;
      }
      return false;
    }
  }
}

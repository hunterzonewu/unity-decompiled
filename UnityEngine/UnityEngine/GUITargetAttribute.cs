// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUITargetAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Reflection;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Allows to control for which display the OnGUI is called.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class GUITargetAttribute : Attribute
  {
    internal int displayMask;

    /// <summary>
    ///   <para>Default constructor initializes the attribute for OnGUI to be called for all available displays.</para>
    /// </summary>
    /// <param name="displayIndex">Display index.</param>
    /// <param name="displayIndex1">Display index.</param>
    /// <param name="displayIndexList">Display index list.</param>
    public GUITargetAttribute()
    {
      this.displayMask = -1;
    }

    /// <summary>
    ///   <para>Default constructor initializes the attribute for OnGUI to be called for all available displays.</para>
    /// </summary>
    /// <param name="displayIndex">Display index.</param>
    /// <param name="displayIndex1">Display index.</param>
    /// <param name="displayIndexList">Display index list.</param>
    public GUITargetAttribute(int displayIndex)
    {
      this.displayMask = 1 << displayIndex;
    }

    /// <summary>
    ///   <para>Default constructor initializes the attribute for OnGUI to be called for all available displays.</para>
    /// </summary>
    /// <param name="displayIndex">Display index.</param>
    /// <param name="displayIndex1">Display index.</param>
    /// <param name="displayIndexList">Display index list.</param>
    public GUITargetAttribute(int displayIndex, int displayIndex1)
    {
      this.displayMask = 1 << displayIndex | 1 << displayIndex1;
    }

    /// <summary>
    ///   <para>Default constructor initializes the attribute for OnGUI to be called for all available displays.</para>
    /// </summary>
    /// <param name="displayIndex">Display index.</param>
    /// <param name="displayIndex1">Display index.</param>
    /// <param name="displayIndexList">Display index list.</param>
    public GUITargetAttribute(int displayIndex, int displayIndex1, params int[] displayIndexList)
    {
      this.displayMask = 1 << displayIndex | 1 << displayIndex1;
      for (int index = 0; index < displayIndexList.Length; ++index)
        this.displayMask |= 1 << displayIndexList[index];
    }

    [RequiredByNativeCode]
    private static int GetGUITargetAttrValue(System.Type klass, string methodName)
    {
      MethodInfo method = klass.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (method != null)
      {
        object[] customAttributes = method.GetCustomAttributes(true);
        if (customAttributes != null)
        {
          for (int index = 0; index < customAttributes.Length; ++index)
          {
            if (customAttributes[index].GetType() == typeof (GUITargetAttribute))
              return (customAttributes[index] as GUITargetAttribute).displayMask;
          }
        }
      }
      return -1;
    }
  }
}

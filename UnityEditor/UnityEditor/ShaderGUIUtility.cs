// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShaderGUIUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Reflection;

namespace UnityEditor
{
  internal static class ShaderGUIUtility
  {
    internal static ShaderGUI CreateShaderGUI(string customEditorName)
    {
      string str = "UnityEditor." + customEditorName;
      Assembly[] loadedAssemblies = EditorAssemblies.loadedAssemblies;
      for (int index = loadedAssemblies.Length - 1; index >= 0; --index)
      {
        foreach (System.Type type in AssemblyHelper.GetTypesFromAssembly(loadedAssemblies[index]))
        {
          if (type.FullName.Equals(customEditorName, StringComparison.Ordinal) || type.FullName.Equals(str, StringComparison.Ordinal))
          {
            if (typeof (ShaderGUI).IsAssignableFrom(type))
              return Activator.CreateInstance(type) as ShaderGUI;
            return (ShaderGUI) null;
          }
        }
      }
      return (ShaderGUI) null;
    }
  }
}

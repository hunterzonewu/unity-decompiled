// Decompiled with JetBrains decompiler
// Type: UnityEditor.DebugUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor
{
  internal class DebugUtils
  {
    internal static string ListToString<T>(IEnumerable<T> list)
    {
      if (list == null)
        return "[null list]";
      string str1 = "[";
      int num = 0;
      foreach (T obj in list)
      {
        if (num != 0)
          str1 += ", ";
        str1 = (object) obj == null ? str1 + "'null'" : str1 + obj.ToString();
        ++num;
      }
      string str2 = str1 + "]";
      if (num == 0)
        return "[empty list]";
      return "(" + (object) num + ") " + str2;
    }
  }
}

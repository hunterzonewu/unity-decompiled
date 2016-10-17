// Decompiled with JetBrains decompiler
// Type: UnityEditor.Settings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEditor
{
  internal class Settings
  {
    private static SortedList<string, object> m_Prefs = new SortedList<string, object>();

    internal static T Get<T>(string name, T defaultValue) where T : IPrefType, new()
    {
      if ((object) defaultValue == null)
        throw new ArgumentException("default can not be null", "defaultValue");
      if (Settings.m_Prefs.ContainsKey(name))
        return (T) Settings.m_Prefs[name];
      string sstr = EditorPrefs.GetString(name, string.Empty);
      if (sstr == string.Empty)
      {
        Settings.Set<T>(name, defaultValue);
        return defaultValue;
      }
      defaultValue.FromUniqueString(sstr);
      Settings.Set<T>(name, defaultValue);
      return defaultValue;
    }

    internal static void Set<T>(string name, T value) where T : IPrefType
    {
      EditorPrefs.SetString(name, value.ToUniqueString());
      Settings.m_Prefs[name] = (object) value;
    }

    [DebuggerHidden]
    internal static IEnumerable<KeyValuePair<string, T>> Prefs<T>() where T : IPrefType
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Settings.\u003CPrefs\u003Ec__Iterator4<T> prefsCIterator4_1 = new Settings.\u003CPrefs\u003Ec__Iterator4<T>();
      // ISSUE: variable of a compiler-generated type
      Settings.\u003CPrefs\u003Ec__Iterator4<T> prefsCIterator4_2 = prefsCIterator4_1;
      int num = -2;
      // ISSUE: reference to a compiler-generated field
      prefsCIterator4_2.\u0024PC = num;
      return (IEnumerable<KeyValuePair<string, T>>) prefsCIterator4_2;
    }
  }
}

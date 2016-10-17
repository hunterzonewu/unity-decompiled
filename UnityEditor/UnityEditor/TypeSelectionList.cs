// Decompiled with JetBrains decompiler
// Type: UnityEditor.TypeSelectionList
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class TypeSelectionList
  {
    private List<TypeSelection> m_TypeSelections;

    public List<TypeSelection> typeSelections
    {
      get
      {
        return this.m_TypeSelections;
      }
    }

    public TypeSelectionList(Object[] objects)
    {
      Dictionary<string, List<Object>> dictionary = new Dictionary<string, List<Object>>();
      foreach (Object @object in objects)
      {
        string typeName = ObjectNames.GetTypeName(@object);
        if (!dictionary.ContainsKey(typeName))
          dictionary[typeName] = new List<Object>();
        dictionary[typeName].Add(@object);
      }
      this.m_TypeSelections = new List<TypeSelection>();
      using (Dictionary<string, List<Object>>.Enumerator enumerator = dictionary.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, List<Object>> current = enumerator.Current;
          this.m_TypeSelections.Add(new TypeSelection(current.Key, current.Value.ToArray()));
        }
      }
      this.m_TypeSelections.Sort();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.SavedFilter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  [Serializable]
  internal class SavedFilter
  {
    public float m_PreviewSize = -1f;
    public string m_Name;
    public int m_Depth;
    public int m_ID;
    public SearchFilter m_Filter;

    public SavedFilter(string name, SearchFilter filter, int depth, float previewSize)
    {
      this.m_Name = name;
      this.m_Depth = depth;
      this.m_Filter = filter;
      this.m_PreviewSize = previewSize;
    }
  }
}

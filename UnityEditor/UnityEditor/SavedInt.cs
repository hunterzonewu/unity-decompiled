// Decompiled with JetBrains decompiler
// Type: UnityEditor.SavedInt
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  internal class SavedInt
  {
    private int m_Value;
    private string m_Name;

    public int value
    {
      get
      {
        return this.m_Value;
      }
      set
      {
        if (this.m_Value == value)
          return;
        this.m_Value = value;
        EditorPrefs.SetInt(this.m_Name, value);
      }
    }

    public SavedInt(string name, int value)
    {
      this.m_Name = name;
      this.m_Value = EditorPrefs.GetInt(name, value);
    }

    public static implicit operator int(SavedInt s)
    {
      return s.value;
    }
  }
}

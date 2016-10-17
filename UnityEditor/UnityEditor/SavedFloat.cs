// Decompiled with JetBrains decompiler
// Type: UnityEditor.SavedFloat
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  internal class SavedFloat
  {
    private float m_Value;
    private string m_Name;

    public float value
    {
      get
      {
        return this.m_Value;
      }
      set
      {
        if ((double) this.m_Value == (double) value)
          return;
        this.m_Value = value;
        EditorPrefs.SetFloat(this.m_Name, value);
      }
    }

    public SavedFloat(string name, float value)
    {
      this.m_Name = name;
      this.m_Value = EditorPrefs.GetFloat(name, value);
    }

    public static implicit operator float(SavedFloat s)
    {
      return s.value;
    }
  }
}

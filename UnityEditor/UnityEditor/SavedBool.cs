// Decompiled with JetBrains decompiler
// Type: UnityEditor.SavedBool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  internal class SavedBool
  {
    private bool m_Value;
    private string m_Name;

    public bool value
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
        EditorPrefs.SetBool(this.m_Name, value);
      }
    }

    public SavedBool(string name, bool value)
    {
      this.m_Name = name;
      this.m_Value = EditorPrefs.GetBool(name, value);
    }

    public static implicit operator bool(SavedBool s)
    {
      return s.value;
    }
  }
}

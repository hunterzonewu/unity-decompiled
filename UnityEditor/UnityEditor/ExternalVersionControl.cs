// Decompiled with JetBrains decompiler
// Type: UnityEditor.ExternalVersionControl
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  public struct ExternalVersionControl
  {
    public static readonly string Disabled = "Hidden Meta Files";
    public static readonly string AutoDetect = "Auto detect";
    public static readonly string Generic = "Visible Meta Files";
    public static readonly string AssetServer = "Asset Server";
    private string m_Value;

    public ExternalVersionControl(string value)
    {
      this.m_Value = value;
    }

    public static implicit operator string(ExternalVersionControl d)
    {
      return d.ToString();
    }

    public static implicit operator ExternalVersionControl(string d)
    {
      return new ExternalVersionControl(d);
    }

    public override string ToString()
    {
      return this.m_Value;
    }
  }
}

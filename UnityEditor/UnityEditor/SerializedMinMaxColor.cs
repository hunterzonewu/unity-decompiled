// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializedMinMaxColor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  internal class SerializedMinMaxColor
  {
    public SerializedProperty maxColor;
    public SerializedProperty minColor;
    public SerializedProperty minMax;

    public SerializedMinMaxColor(SerializedModule m)
    {
      this.Init(m, "curve");
    }

    public SerializedMinMaxColor(SerializedModule m, string name)
    {
      this.Init(m, name);
    }

    private void Init(SerializedModule m, string name)
    {
      this.maxColor = m.GetProperty(name, "maxColor");
      this.minColor = m.GetProperty(name, "minColor");
      this.minMax = m.GetProperty(name, "minMax");
    }
  }
}

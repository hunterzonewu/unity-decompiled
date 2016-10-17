// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ChartSeries
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal
{
  internal class ChartSeries
  {
    public string identifierName;
    public string name;
    public float[] data;
    public float[] overlayData;
    public Color color;
    public bool enabled;

    public ChartSeries(string name, int len, Color clr)
    {
      this.name = name;
      this.identifierName = name;
      this.data = new float[len];
      this.overlayData = (float[]) null;
      this.color = clr;
      this.enabled = true;
    }

    public void CreateOverlayData()
    {
      this.overlayData = new float[this.data.Length];
    }
  }
}

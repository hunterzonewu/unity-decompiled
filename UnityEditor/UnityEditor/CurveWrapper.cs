// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveWrapper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class CurveWrapper
  {
    public float vRangeMin = float.NegativeInfinity;
    public float vRangeMax = float.PositiveInfinity;
    private CurveRenderer m_Renderer;
    public int id;
    public EditorCurveBinding binding;
    public int groupId;
    public int regionId;
    public Color color;
    public bool readOnly;
    public bool hidden;
    public CurveWrapper.GetAxisScalarsCallback getAxisUiScalarsCallback;
    public CurveWrapper.SetAxisScalarsCallback setAxisUiScalarsCallback;
    public bool changed;
    public CurveWrapper.SelectionMode selected;
    public int listIndex;

    public CurveRenderer renderer
    {
      get
      {
        return this.m_Renderer;
      }
      set
      {
        this.m_Renderer = value;
      }
    }

    public AnimationCurve curve
    {
      get
      {
        return this.renderer.GetCurve();
      }
    }

    public CurveWrapper()
    {
      this.id = 0;
      this.groupId = -1;
      this.regionId = -1;
      this.hidden = false;
      this.readOnly = false;
      this.listIndex = -1;
      this.getAxisUiScalarsCallback = (CurveWrapper.GetAxisScalarsCallback) null;
      this.setAxisUiScalarsCallback = (CurveWrapper.SetAxisScalarsCallback) null;
    }

    internal enum SelectionMode
    {
      None,
      Selected,
      SemiSelected,
    }

    public delegate Vector2 GetAxisScalarsCallback();

    public delegate void SetAxisScalarsCallback(Vector2 newAxisScalars);
  }
}

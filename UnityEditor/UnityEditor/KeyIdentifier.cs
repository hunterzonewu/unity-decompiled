// Decompiled with JetBrains decompiler
// Type: UnityEditor.KeyIdentifier
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class KeyIdentifier
  {
    public CurveRenderer renderer;
    public int curveId;
    public int key;
    public EditorCurveBinding binding;

    public AnimationCurve curve
    {
      get
      {
        return this.renderer.GetCurve();
      }
    }

    public Keyframe keyframe
    {
      get
      {
        return this.curve[this.key];
      }
    }

    public KeyIdentifier(CurveRenderer _renderer, int _curveId, int _keyIndex)
    {
      this.renderer = _renderer;
      this.curveId = _curveId;
      this.key = _keyIndex;
    }

    public KeyIdentifier(CurveRenderer _renderer, int _curveId, int _keyIndex, EditorCurveBinding _binding)
    {
      this.renderer = _renderer;
      this.curveId = _curveId;
      this.key = _keyIndex;
      this.binding = _binding;
    }
  }
}

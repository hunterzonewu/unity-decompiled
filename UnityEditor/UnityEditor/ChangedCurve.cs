// Decompiled with JetBrains decompiler
// Type: UnityEditor.ChangedCurve
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ChangedCurve
  {
    public AnimationCurve curve;
    public EditorCurveBinding binding;

    public ChangedCurve(AnimationCurve curve, EditorCurveBinding binding)
    {
      this.curve = curve;
      this.binding = binding;
    }

    public override int GetHashCode()
    {
      return 33 * this.curve.GetHashCode() + this.binding.GetHashCode();
    }
  }
}

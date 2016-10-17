// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUILayoutFadeGroup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal sealed class GUILayoutFadeGroup : GUILayoutGroup
  {
    public float fadeValue;
    public bool wasGUIEnabled;
    public Color guiColor;

    public override void CalcHeight()
    {
      base.CalcHeight();
      this.minHeight *= (float) (double) this.fadeValue;
      this.maxHeight *= (float) (double) this.fadeValue;
    }
  }
}

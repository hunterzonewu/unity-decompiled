// Decompiled with JetBrains decompiler
// Type: UnityEditor.SliderLabels
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal struct SliderLabels
  {
    public GUIContent leftLabel;
    public GUIContent rightLabel;

    public void SetLabels(GUIContent leftLabel, GUIContent rightLabel)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      this.leftLabel = leftLabel;
      this.rightLabel = rightLabel;
    }

    public bool HasLabels()
    {
      if (Event.current.type == EventType.Repaint && this.leftLabel != null)
        return this.rightLabel != null;
      return false;
    }
  }
}

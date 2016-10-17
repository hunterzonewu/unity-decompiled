// Decompiled with JetBrains decompiler
// Type: UnityEditor.PropertyGUIData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal struct PropertyGUIData
  {
    public SerializedProperty property;
    public Rect totalPosition;
    public bool wasBoldDefaultFont;
    public bool wasEnabled;
    public Color color;

    public PropertyGUIData(SerializedProperty property, Rect totalPosition, bool wasBoldDefaultFont, bool wasEnabled, Color color)
    {
      this.property = property;
      this.totalPosition = totalPosition;
      this.wasBoldDefaultFont = wasBoldDefaultFont;
      this.wasEnabled = wasEnabled;
      this.color = color;
    }
  }
}

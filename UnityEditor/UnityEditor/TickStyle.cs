// Decompiled with JetBrains decompiler
// Type: UnityEditor.TickStyle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class TickStyle
  {
    public Color color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
    public Color labelColor = new Color(0.0f, 0.0f, 0.0f, 1f);
    public int distMin = 10;
    public int distFull = 80;
    public int distLabel = 50;
    public string unit = string.Empty;
    public bool stubs;
    public bool centerLabel;

    public TickStyle()
    {
      if (EditorGUIUtility.isProSkin)
      {
        this.color = new Color(0.45f, 0.45f, 0.45f, 0.2f);
        this.labelColor = new Color(0.8f, 0.8f, 0.8f, 0.32f);
      }
      else
      {
        this.color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
        this.labelColor = new Color(0.0f, 0.0f, 0.0f, 0.32f);
      }
    }
  }
}

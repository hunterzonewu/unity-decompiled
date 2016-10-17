// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteRect
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class SpriteRect
  {
    [SerializeField]
    public string m_Name = string.Empty;
    [SerializeField]
    public string m_OriginalName = string.Empty;
    [SerializeField]
    public Vector2 m_Pivot = Vector2.zero;
    [SerializeField]
    public List<List<Vector2>> m_Outline = new List<List<Vector2>>();
    [SerializeField]
    public SpriteAlignment m_Alignment;
    [SerializeField]
    public Vector4 m_Border;
    [SerializeField]
    public Rect m_Rect;
  }
}

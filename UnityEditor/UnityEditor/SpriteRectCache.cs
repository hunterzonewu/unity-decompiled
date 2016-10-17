// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteRectCache
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class SpriteRectCache : ScriptableObject
  {
    [SerializeField]
    public List<SpriteRect> m_Rects;

    public int Count
    {
      get
      {
        if (this.m_Rects != null)
          return this.m_Rects.Count;
        return 0;
      }
    }

    public SpriteRect RectAt(int i)
    {
      if (i >= this.Count)
        return (SpriteRect) null;
      return this.m_Rects[i];
    }

    public void AddRect(SpriteRect r)
    {
      if (this.m_Rects == null)
        return;
      this.m_Rects.Add(r);
    }

    public void RemoveRect(SpriteRect r)
    {
      if (this.m_Rects == null)
        return;
      this.m_Rects.Remove(r);
    }

    public void ClearAll()
    {
      if (this.m_Rects == null)
        return;
      this.m_Rects.Clear();
    }

    public int GetIndex(SpriteRect spriteRect)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SpriteRectCache.\u003CGetIndex\u003Ec__AnonStoreyB4 indexCAnonStoreyB4 = new SpriteRectCache.\u003CGetIndex\u003Ec__AnonStoreyB4();
      // ISSUE: reference to a compiler-generated field
      indexCAnonStoreyB4.spriteRect = spriteRect;
      if (this.m_Rects != null)
      {
        // ISSUE: reference to a compiler-generated method
        return this.m_Rects.FindIndex(new Predicate<SpriteRect>(indexCAnonStoreyB4.\u003C\u003Em__211));
      }
      return 0;
    }

    public bool Contains(SpriteRect spriteRect)
    {
      if (this.m_Rects != null)
        return this.m_Rects.Contains(spriteRect);
      return false;
    }

    private void OnEnable()
    {
      if (this.m_Rects != null)
        return;
      this.m_Rects = new List<SpriteRect>();
    }
  }
}

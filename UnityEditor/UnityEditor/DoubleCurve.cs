// Decompiled with JetBrains decompiler
// Type: UnityEditor.DoubleCurve
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class DoubleCurve
  {
    [SerializeField]
    private AnimationCurve m_MinCurve;
    [SerializeField]
    private AnimationCurve m_MaxCurve;
    [SerializeField]
    private bool m_SignedRange;

    public AnimationCurve minCurve
    {
      get
      {
        return this.m_MinCurve;
      }
      set
      {
        this.m_MinCurve = value;
      }
    }

    public AnimationCurve maxCurve
    {
      get
      {
        return this.m_MaxCurve;
      }
      set
      {
        this.m_MaxCurve = value;
      }
    }

    public bool signedRange
    {
      get
      {
        return this.m_SignedRange;
      }
      set
      {
        this.m_SignedRange = value;
      }
    }

    public DoubleCurve(AnimationCurve minCurve, AnimationCurve maxCurve, bool signedRange)
    {
      if (minCurve != null)
        this.m_MinCurve = new AnimationCurve(minCurve.keys);
      if (maxCurve != null)
        this.m_MaxCurve = new AnimationCurve(maxCurve.keys);
      else
        Debug.LogError((object) "Ensure that maxCurve is not null when creating a double curve. The minCurve can be null for single curves");
      this.m_SignedRange = signedRange;
    }

    public bool IsSingleCurve()
    {
      if (this.minCurve != null)
        return this.minCurve.length == 0;
      return true;
    }
  }
}

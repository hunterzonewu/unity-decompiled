// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class CurveSelection : IComparable
  {
    private int m_Key = -1;
    [NonSerialized]
    internal CurveEditor m_Host;
    private int m_CurveID;
    internal bool semiSelected;
    internal CurveSelection.SelectionType type;

    internal CurveWrapper curveWrapper
    {
      get
      {
        return this.m_Host.GetCurveFromID(this.m_CurveID);
      }
    }

    internal AnimationCurve curve
    {
      get
      {
        if (this.curveWrapper != null)
          return this.curveWrapper.curve;
        return (AnimationCurve) null;
      }
    }

    public int curveID
    {
      get
      {
        return this.m_CurveID;
      }
      set
      {
        this.m_CurveID = value;
      }
    }

    public int key
    {
      get
      {
        return this.m_Key;
      }
      set
      {
        this.m_Key = value;
      }
    }

    internal Keyframe keyframe
    {
      get
      {
        if (this.validKey())
          return this.curve[this.m_Key];
        return new Keyframe();
      }
    }

    internal CurveSelection(int curveID, CurveEditor host, int keyIndex)
    {
      this.m_CurveID = curveID;
      this.m_Host = host;
      this.m_Key = keyIndex;
      this.type = CurveSelection.SelectionType.Key;
    }

    internal CurveSelection(int curveID, CurveEditor host, int keyIndex, CurveSelection.SelectionType t)
    {
      this.m_CurveID = curveID;
      this.m_Host = host;
      this.m_Key = keyIndex;
      this.type = t;
    }

    internal bool validKey()
    {
      if (this.curve != null && this.m_Key >= 0)
        return this.m_Key < this.curve.length;
      return false;
    }

    public int CompareTo(object _other)
    {
      CurveSelection curveSelection = (CurveSelection) _other;
      int num1 = this.curveID - curveSelection.curveID;
      if (num1 != 0)
        return num1;
      int num2 = this.key - curveSelection.key;
      if (num2 != 0)
        return num2;
      return this.type - curveSelection.type;
    }

    public override bool Equals(object _other)
    {
      CurveSelection curveSelection = (CurveSelection) _other;
      if (curveSelection.curveID == this.curveID && curveSelection.key == this.key)
        return curveSelection.type == this.type;
      return false;
    }

    public override int GetHashCode()
    {
      return (int) (this.curveID * 729 + this.key * 27 + this.type);
    }

    internal enum SelectionType
    {
      Key,
      InTangent,
      OutTangent,
      Count,
    }
  }
}

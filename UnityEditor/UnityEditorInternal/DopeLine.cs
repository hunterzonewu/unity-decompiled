// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.DopeLine
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class DopeLine
  {
    public static GUIStyle dopekeyStyle = (GUIStyle) "Dopesheetkeyframe";
    public Rect position;
    public AnimationWindowCurve[] m_Curves;
    public List<AnimationWindowKeyframe> keys;
    public int m_HierarchyNodeID;
    public System.Type objectType;
    public bool tallMode;
    public bool hasChildren;
    public bool isMasterDopeline;

    public System.Type valueType
    {
      get
      {
        if (this.m_Curves.Length <= 0)
          return (System.Type) null;
        System.Type valueType = this.m_Curves[0].m_ValueType;
        for (int index = 1; index < this.m_Curves.Length; ++index)
        {
          if (this.m_Curves[index].m_ValueType != valueType)
            return (System.Type) null;
        }
        return valueType;
      }
    }

    public bool isPptrDopeline
    {
      get
      {
        if (this.m_Curves.Length <= 0)
          return false;
        for (int index = 0; index < this.m_Curves.Length; ++index)
        {
          if (!this.m_Curves[index].isPPtrCurve)
            return false;
        }
        return true;
      }
    }

    public DopeLine(int hierarchyNodeId, AnimationWindowCurve[] curves)
    {
      this.m_HierarchyNodeID = hierarchyNodeId;
      this.m_Curves = curves;
      this.LoadKeyframes();
    }

    public void LoadKeyframes()
    {
      this.keys = new List<AnimationWindowKeyframe>();
      foreach (AnimationWindowCurve curve in this.m_Curves)
      {
        using (List<AnimationWindowKeyframe>.Enumerator enumerator = curve.m_Keyframes.GetEnumerator())
        {
          while (enumerator.MoveNext())
            this.keys.Add(enumerator.Current);
        }
      }
      this.keys.Sort((Comparison<AnimationWindowKeyframe>) ((a, b) => a.time.CompareTo(b.time)));
    }
  }
}

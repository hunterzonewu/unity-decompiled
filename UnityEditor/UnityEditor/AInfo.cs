// Decompiled with JetBrains decompiler
// Type: UnityEditor.AInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class AInfo : IComparable, IEquatable<AInfo>
  {
    public bool m_IconEnabled;
    public bool m_GizmoEnabled;
    public int m_ClassID;
    public string m_ScriptClass;
    public string m_DisplayText;
    public int m_Flags;

    public AInfo(bool gizmoEnabled, bool iconEnabled, int flags, int classID, string scriptClass)
    {
      this.m_GizmoEnabled = gizmoEnabled;
      this.m_IconEnabled = iconEnabled;
      this.m_ClassID = classID;
      this.m_ScriptClass = scriptClass;
      this.m_Flags = flags;
      if (this.m_ScriptClass == string.Empty)
        this.m_DisplayText = BaseObjectTools.ClassIDToString(this.m_ClassID);
      else
        this.m_DisplayText = this.m_ScriptClass;
    }

    private bool IsBitSet(byte b, int pos)
    {
      return ((int) b & 1 << pos) != 0;
    }

    public bool HasGizmo()
    {
      return (this.m_Flags & 2) > 0;
    }

    public bool HasIcon()
    {
      return (this.m_Flags & 1) > 0;
    }

    public int CompareTo(object obj)
    {
      AInfo ainfo = obj as AInfo;
      if (ainfo != null)
        return this.m_DisplayText.CompareTo(ainfo.m_DisplayText);
      throw new ArgumentException("Object is not an AInfo");
    }

    public bool Equals(AInfo other)
    {
      if (this.m_ClassID == other.m_ClassID)
        return this.m_ScriptClass == other.m_ScriptClass;
      return false;
    }

    public enum Flags
    {
      kHasIcon = 1,
      kHasGizmo = 2,
    }
  }
}

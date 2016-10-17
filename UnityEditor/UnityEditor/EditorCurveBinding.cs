// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorCurveBinding
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Defines how a curve is attached to an object that it controls.</para>
  /// </summary>
  public struct EditorCurveBinding
  {
    /// <summary>
    ///   <para>The transform path of the object that is animated.</para>
    /// </summary>
    public string path;
    private System.Type m_type;
    /// <summary>
    ///   <para>The property of the object that is animated.</para>
    /// </summary>
    public string propertyName;
    private int m_isPPtrCurve;
    internal int m_ClassID;
    internal int m_ScriptInstanceID;

    public bool isPPtrCurve
    {
      get
      {
        return this.m_isPPtrCurve != 0;
      }
    }

    public System.Type type
    {
      get
      {
        return this.m_type;
      }
      set
      {
        this.m_type = value;
        this.m_ClassID = 0;
        this.m_ScriptInstanceID = 0;
      }
    }

    public static bool operator ==(EditorCurveBinding lhs, EditorCurveBinding rhs)
    {
      if (lhs.m_ClassID != 0 && rhs.m_ClassID != 0 && (lhs.m_ClassID != rhs.m_ClassID || lhs.m_ScriptInstanceID != rhs.m_ScriptInstanceID) || (!(lhs.path == rhs.path) || lhs.type != rhs.type || !(lhs.propertyName == rhs.propertyName)))
        return false;
      return lhs.m_isPPtrCurve == rhs.m_isPPtrCurve;
    }

    public static bool operator !=(EditorCurveBinding lhs, EditorCurveBinding rhs)
    {
      return !(lhs == rhs);
    }

    public override int GetHashCode()
    {
      return this.path.GetHashCode() ^ this.type.GetHashCode() << 2 ^ this.propertyName.GetHashCode() << 4;
    }

    public override bool Equals(object other)
    {
      if (!(other is EditorCurveBinding))
        return false;
      return this == (EditorCurveBinding) other;
    }

    public static EditorCurveBinding FloatCurve(string inPath, System.Type inType, string inPropertyName)
    {
      return new EditorCurveBinding()
      {
        path = inPath,
        type = inType,
        propertyName = inPropertyName,
        m_isPPtrCurve = 0
      };
    }

    public static EditorCurveBinding PPtrCurve(string inPath, System.Type inType, string inPropertyName)
    {
      return new EditorCurveBinding()
      {
        path = inPath,
        type = inType,
        propertyName = inPropertyName,
        m_isPPtrCurve = 1
      };
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEngine.Serialization.FormerlySerializedAsAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine.Serialization
{
  /// <summary>
  ///   <para>Use this attribute to rename a field without losing its serialized value.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
  [RequiredByNativeCode]
  public class FormerlySerializedAsAttribute : Attribute
  {
    private string m_oldName;

    /// <summary>
    ///   <para>The name of the field before the rename.</para>
    /// </summary>
    public string oldName
    {
      get
      {
        return this.m_oldName;
      }
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="oldName">The name of the field before renaming.</param>
    public FormerlySerializedAsAttribute(string oldName)
    {
      this.m_oldName = oldName;
    }
  }
}

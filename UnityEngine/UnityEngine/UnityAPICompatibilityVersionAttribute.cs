// Decompiled with JetBrains decompiler
// Type: UnityEngine.UnityAPICompatibilityVersionAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Declares an assembly to be compatible (API wise) with a specific Unity API. Used by internal tools to avoid processing the assembly in order to decide whether assemblies may be using old Unity API.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
  public class UnityAPICompatibilityVersionAttribute : Attribute
  {
    private string _version;

    /// <summary>
    ///   <para>Version of Unity API.</para>
    /// </summary>
    public string version
    {
      get
      {
        return this._version;
      }
    }

    /// <summary>
    ///   <para>Initializes a new instance of UnityAPICompatibilityVersionAttribute.</para>
    /// </summary>
    /// <param name="version">Unity version that this assembly with compatible with.</param>
    public UnityAPICompatibilityVersionAttribute(string version)
    {
      this._version = version;
    }
  }
}

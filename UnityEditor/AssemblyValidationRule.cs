// Decompiled with JetBrains decompiler
// Type: AssemblyValidationRule
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class AssemblyValidationRule : Attribute
{
  public int Priority;
  private readonly RuntimePlatform _platform;

  public RuntimePlatform Platform
  {
    get
    {
      return this._platform;
    }
  }

  public AssemblyValidationRule(RuntimePlatform platform)
  {
    this._platform = platform;
    this.Priority = 0;
  }
}

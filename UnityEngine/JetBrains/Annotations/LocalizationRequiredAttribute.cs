// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.LocalizationRequiredAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
  public sealed class LocalizationRequiredAttribute : Attribute
  {
    public bool Required { get; private set; }

    public LocalizationRequiredAttribute()
      : this(true)
    {
    }

    public LocalizationRequiredAttribute(bool required)
    {
      this.Required = required;
    }
  }
}

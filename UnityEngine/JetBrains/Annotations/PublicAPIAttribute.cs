// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.PublicAPIAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace JetBrains.Annotations
{
  [MeansImplicitUse]
  public sealed class PublicAPIAttribute : Attribute
  {
    [NotNull]
    public string Comment { get; private set; }

    public PublicAPIAttribute()
    {
    }

    public PublicAPIAttribute([NotNull] string comment)
    {
      this.Comment = comment;
    }
  }
}

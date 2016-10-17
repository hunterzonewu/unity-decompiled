// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.PathReferenceAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.Parameter)]
  public class PathReferenceAttribute : Attribute
  {
    [NotNull]
    public string BasePath { get; private set; }

    public PathReferenceAttribute()
    {
    }

    public PathReferenceAttribute([PathReference] string basePath)
    {
      this.BasePath = basePath;
    }
  }
}

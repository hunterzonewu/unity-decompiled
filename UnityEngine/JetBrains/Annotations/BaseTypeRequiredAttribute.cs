// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.BaseTypeRequiredAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
  [BaseTypeRequired(typeof (Attribute))]
  public sealed class BaseTypeRequiredAttribute : Attribute
  {
    [NotNull]
    public Type BaseType { get; private set; }

    public BaseTypeRequiredAttribute([NotNull] Type baseType)
    {
      this.BaseType = baseType;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.ContractAnnotationAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  public sealed class ContractAnnotationAttribute : Attribute
  {
    public string Contract { get; private set; }

    public bool ForceFullStates { get; private set; }

    public ContractAnnotationAttribute([NotNull] string contract)
      : this(contract, false)
    {
    }

    public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
    {
      this.Contract = contract;
      this.ForceFullStates = forceFullStates;
    }
  }
}

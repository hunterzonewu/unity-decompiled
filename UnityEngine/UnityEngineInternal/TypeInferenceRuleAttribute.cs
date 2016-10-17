// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.TypeInferenceRuleAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngineInternal
{
  [AttributeUsage(AttributeTargets.Method)]
  [Serializable]
  public class TypeInferenceRuleAttribute : Attribute
  {
    private readonly string _rule;

    public TypeInferenceRuleAttribute(TypeInferenceRules rule)
      : this(rule.ToString())
    {
    }

    public TypeInferenceRuleAttribute(string rule)
    {
      this._rule = rule;
    }

    public override string ToString()
    {
      return this._rule;
    }
  }
}

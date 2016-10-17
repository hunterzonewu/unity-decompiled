// Decompiled with JetBrains decompiler
// Type: UnityEngine.Internal.DefaultValueAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.Internal
{
  [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter)]
  [Serializable]
  public class DefaultValueAttribute : Attribute
  {
    private object DefaultValue;

    public object Value
    {
      get
      {
        return this.DefaultValue;
      }
    }

    public DefaultValueAttribute(string value)
    {
      this.DefaultValue = (object) value;
    }

    public override bool Equals(object obj)
    {
      DefaultValueAttribute defaultValueAttribute = obj as DefaultValueAttribute;
      if (defaultValueAttribute == null)
        return false;
      if (this.DefaultValue == null)
        return defaultValueAttribute.Value == null;
      return this.DefaultValue.Equals(defaultValueAttribute.Value);
    }

    public override int GetHashCode()
    {
      if (this.DefaultValue == null)
        return base.GetHashCode();
      return this.DefaultValue.GetHashCode();
    }
  }
}

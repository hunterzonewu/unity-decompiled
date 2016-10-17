// Decompiled with JetBrains decompiler
// Type: AssemblyValidation
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

internal class AssemblyValidation
{
  private static Dictionary<RuntimePlatform, List<System.Type>> _rulesByPlatform;

  public static ValidationResult Validate(RuntimePlatform platform, IEnumerable<string> userAssemblies, params object[] options)
  {
    AssemblyValidation.WarmUpRulesCache();
    string[] strArray = userAssemblies as string[] ?? userAssemblies.ToArray<string>();
    if (strArray.Length != 0)
    {
      foreach (IValidationRule validationRule in AssemblyValidation.ValidationRulesFor(platform, options))
      {
        ValidationResult validationResult = validationRule.Validate((IEnumerable<string>) strArray, options);
        if (!validationResult.Success)
          return validationResult;
      }
    }
    return new ValidationResult()
    {
      Success = true
    };
  }

  private static void WarmUpRulesCache()
  {
    if (AssemblyValidation._rulesByPlatform != null)
      return;
    AssemblyValidation._rulesByPlatform = new Dictionary<RuntimePlatform, List<System.Type>>();
    foreach (System.Type type in ((IEnumerable<System.Type>) typeof (AssemblyValidation).Assembly.GetTypes()).Where<System.Type>(new Func<System.Type, bool>(AssemblyValidation.IsValidationRule)))
      AssemblyValidation.RegisterValidationRule(type);
  }

  private static bool IsValidationRule(System.Type type)
  {
    return AssemblyValidation.ValidationRuleAttributesFor(type).Any<AssemblyValidationRule>();
  }

  private static IEnumerable<IValidationRule> ValidationRulesFor(RuntimePlatform platform, params object[] options)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    return AssemblyValidation.ValidationRuleTypesFor(platform).Select<System.Type, IValidationRule>(new Func<System.Type, IValidationRule>(new AssemblyValidation.\u003CValidationRulesFor\u003Ec__AnonStorey69()
    {
      options = options
    }.\u003C\u003Em__E0)).Where<IValidationRule>((Func<IValidationRule, bool>) (v => v != null));
  }

  [DebuggerHidden]
  private static IEnumerable<System.Type> ValidationRuleTypesFor(RuntimePlatform platform)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AssemblyValidation.\u003CValidationRuleTypesFor\u003Ec__Iterator5 typesForCIterator5 = new AssemblyValidation.\u003CValidationRuleTypesFor\u003Ec__Iterator5()
    {
      platform = platform,
      \u003C\u0024\u003Eplatform = platform
    };
    // ISSUE: reference to a compiler-generated field
    typesForCIterator5.\u0024PC = -2;
    return (IEnumerable<System.Type>) typesForCIterator5;
  }

  private static IValidationRule CreateValidationRuleWithOptions(System.Type type, params object[] options)
  {
    List<object> objectList = new List<object>((IEnumerable<object>) options);
    object[] array;
    ConstructorInfo constructorInfo;
    while (true)
    {
      array = objectList.ToArray();
      constructorInfo = AssemblyValidation.ConstructorFor(type, (IEnumerable<object>) array);
      if (constructorInfo == null)
      {
        if (objectList.Count != 0)
          objectList.RemoveAt(objectList.Count - 1);
        else
          goto label_4;
      }
      else
        break;
    }
    return (IValidationRule) constructorInfo.Invoke(array);
label_4:
    return (IValidationRule) null;
  }

  private static ConstructorInfo ConstructorFor(System.Type type, IEnumerable<object> options)
  {
    System.Type[] array = options.Select<object, System.Type>((Func<object, System.Type>) (o => o.GetType())).ToArray<System.Type>();
    return type.GetConstructor(array);
  }

  internal static void RegisterValidationRule(System.Type type)
  {
    foreach (AssemblyValidationRule assemblyValidationRule in AssemblyValidation.ValidationRuleAttributesFor(type))
      AssemblyValidation.RegisterValidationRuleForPlatform(assemblyValidationRule.Platform, type);
  }

  internal static void RegisterValidationRuleForPlatform(RuntimePlatform platform, System.Type type)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AssemblyValidation.\u003CRegisterValidationRuleForPlatform\u003Ec__AnonStorey6A platformCAnonStorey6A = new AssemblyValidation.\u003CRegisterValidationRuleForPlatform\u003Ec__AnonStorey6A();
    // ISSUE: reference to a compiler-generated field
    platformCAnonStorey6A.platform = platform;
    // ISSUE: reference to a compiler-generated field
    if (!AssemblyValidation._rulesByPlatform.ContainsKey(platformCAnonStorey6A.platform))
    {
      // ISSUE: reference to a compiler-generated field
      AssemblyValidation._rulesByPlatform[platformCAnonStorey6A.platform] = new List<System.Type>();
    }
    // ISSUE: reference to a compiler-generated field
    if (AssemblyValidation._rulesByPlatform[platformCAnonStorey6A.platform].IndexOf(type) == -1)
    {
      // ISSUE: reference to a compiler-generated field
      AssemblyValidation._rulesByPlatform[platformCAnonStorey6A.platform].Add(type);
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    AssemblyValidation._rulesByPlatform[platformCAnonStorey6A.platform].Sort(new Comparison<System.Type>(platformCAnonStorey6A.\u003C\u003Em__E3));
  }

  internal static int CompareValidationRulesByPriority(System.Type a, System.Type b, RuntimePlatform platform)
  {
    int num1 = AssemblyValidation.PriorityFor(a, platform);
    int num2 = AssemblyValidation.PriorityFor(b, platform);
    if (num1 == num2)
      return 0;
    return num1 < num2 ? -1 : 1;
  }

  private static int PriorityFor(System.Type type, RuntimePlatform platform)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    return AssemblyValidation.ValidationRuleAttributesFor(type).Where<AssemblyValidationRule>(new Func<AssemblyValidationRule, bool>(new AssemblyValidation.\u003CPriorityFor\u003Ec__AnonStorey6B()
    {
      platform = platform
    }.\u003C\u003Em__E4)).Select<AssemblyValidationRule, int>((Func<AssemblyValidationRule, int>) (attr => attr.Priority)).FirstOrDefault<int>();
  }

  private static IEnumerable<AssemblyValidationRule> ValidationRuleAttributesFor(System.Type type)
  {
    return type.GetCustomAttributes(true).OfType<AssemblyValidationRule>();
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.Macros.MethodEvaluator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace UnityEditor.Macros
{
  public static class MethodEvaluator
  {
    public static object Eval(string assemblyFile, string typeName, string methodName, System.Type[] paramTypes, object[] args)
    {
      MethodEvaluator.AssemblyResolver assemblyResolver = new MethodEvaluator.AssemblyResolver(Path.GetDirectoryName(assemblyFile));
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(assemblyResolver.AssemblyResolve);
      try
      {
        Assembly assembly = Assembly.LoadFrom(assemblyFile);
        MethodInfo method = assembly.GetType(typeName, true).GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, paramTypes, (ParameterModifier[]) null);
        if (method == null)
          throw new ArgumentException(string.Format("Method {0}.{1}({2}) not found in assembly {3}!", (object) typeName, (object) methodName, (object) MethodEvaluator.ToCommaSeparatedString<System.Type>((IEnumerable<System.Type>) paramTypes), (object) assembly.FullName));
        return method.Invoke((object) null, args);
      }
      finally
      {
        AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(assemblyResolver.AssemblyResolve);
      }
    }

    private static string ToCommaSeparatedString<T>(IEnumerable<T> items)
    {
      return string.Join(", ", items.Select<T, string>((Func<T, string>) (o => o.ToString())).ToArray<string>());
    }

    public class AssemblyResolver
    {
      private readonly string _assemblyDirectory;

      public AssemblyResolver(string assemblyDirectory)
      {
        this._assemblyDirectory = assemblyDirectory;
      }

      public Assembly AssemblyResolve(object sender, ResolveEventArgs args)
      {
        string str = Path.Combine(this._assemblyDirectory, args.Name.Split(',')[0] + ".dll");
        if (File.Exists(str))
          return Assembly.LoadFrom(str);
        return (Assembly) null;
      }
    }
  }
}

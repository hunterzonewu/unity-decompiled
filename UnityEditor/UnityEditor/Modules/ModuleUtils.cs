// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.ModuleUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor.Modules
{
  internal static class ModuleUtils
  {
    internal static string[] GetAdditionalReferencesForUserScripts()
    {
      List<string> stringList = new List<string>();
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
        stringList.AddRange((IEnumerable<string>) platformSupportModule.AssemblyReferencesForUserScripts);
      return stringList.ToArray();
    }
  }
}

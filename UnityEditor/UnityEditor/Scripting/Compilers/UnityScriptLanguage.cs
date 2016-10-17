// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.UnityScriptLanguage
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Scripting.Compilers
{
  internal class UnityScriptLanguage : SupportedLanguage
  {
    public override string GetExtensionICanCompile()
    {
      return "js";
    }

    public override string GetLanguageName()
    {
      return "UnityScript";
    }

    public override ScriptCompilerBase CreateCompiler(MonoIsland island, bool buildingForEditor, BuildTarget targetPlatform, bool runUpdater)
    {
      return (ScriptCompilerBase) new UnityScriptCompiler(island, runUpdater);
    }
  }
}

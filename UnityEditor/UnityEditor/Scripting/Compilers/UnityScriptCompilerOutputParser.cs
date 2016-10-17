// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.UnityScriptCompilerOutputParser
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Text.RegularExpressions;

namespace UnityEditor.Scripting.Compilers
{
  internal class UnityScriptCompilerOutputParser : CompilerOutputParserBase
  {
    private static Regex sCompilerOutput = new Regex("\\s*(?<filename>.*)\\((?<line>\\d+),(?<column>\\d+)\\):\\s*[BU]C(?<type>W|E)(?<id>[^:]*):\\s*(?<message>.*)", RegexOptions.ExplicitCapture);

    protected override string GetErrorIdentifier()
    {
      return "E";
    }

    protected override Regex GetOutputRegex()
    {
      return UnityScriptCompilerOutputParser.sCompilerOutput;
    }
  }
}

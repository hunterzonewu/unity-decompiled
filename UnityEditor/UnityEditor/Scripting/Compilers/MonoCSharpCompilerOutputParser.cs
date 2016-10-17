// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.MonoCSharpCompilerOutputParser
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Text.RegularExpressions;

namespace UnityEditor.Scripting.Compilers
{
  internal class MonoCSharpCompilerOutputParser : CompilerOutputParserBase
  {
    private static Regex sCompilerOutput = new Regex("\\s*(?<filename>.*)\\((?<line>\\d+),(?<column>\\d+)\\):\\s*(?<type>warning|error)\\s*(?<id>[^:]*):\\s*(?<message>.*)", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    private static Regex sMissingMember = new Regex("[^`]*`(?<type_name>[^']+)'[^`]+`(?<member_name>[^']+)'", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    protected override Regex GetOutputRegex()
    {
      return MonoCSharpCompilerOutputParser.sCompilerOutput;
    }

    protected override string GetErrorIdentifier()
    {
      return "error";
    }

    protected override NormalizedCompilerStatus NormalizedStatusFor(Match match)
    {
      return CompilerOutputParserBase.TryNormalizeCompilerStatus(match, "CS0117", MonoCSharpCompilerOutputParser.sMissingMember);
    }
  }
}

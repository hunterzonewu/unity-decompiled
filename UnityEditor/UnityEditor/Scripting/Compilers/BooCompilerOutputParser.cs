// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.BooCompilerOutputParser
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Text.RegularExpressions;

namespace UnityEditor.Scripting.Compilers
{
  internal class BooCompilerOutputParser : CompilerOutputParserBase
  {
    private static Regex sCompilerOutput = new Regex("\\s*(?<filename>.*)\\((?<line>\\d+),(?<column>\\d+)\\):\\s*[BU]C(?<type>W|E)(?<id>[^:]*):\\s*(?<message>.*)", RegexOptions.ExplicitCapture);
    private static Regex sMissingMember = new Regex("[^']*'(?<member_name>[^']+)'[^']+'(?<type_name>[^']+)'", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    protected override string GetErrorIdentifier()
    {
      return "E";
    }

    protected override Regex GetOutputRegex()
    {
      return BooCompilerOutputParser.sCompilerOutput;
    }

    protected override NormalizedCompilerStatus NormalizedStatusFor(Match match)
    {
      return CompilerOutputParserBase.TryNormalizeCompilerStatus(match, "0019", BooCompilerOutputParser.sMissingMember);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.CompilerOutputParserBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnityEditor.Scripting.Compilers
{
  internal abstract class CompilerOutputParserBase
  {
    protected static CompilerMessage CreateInternalCompilerErrorMessage(string[] compileroutput)
    {
      CompilerMessage compilerMessage;
      compilerMessage.file = string.Empty;
      compilerMessage.message = string.Join("\n", compileroutput);
      compilerMessage.type = CompilerMessageType.Error;
      compilerMessage.line = 0;
      compilerMessage.column = 0;
      compilerMessage.normalizedStatus = new NormalizedCompilerStatus();
      return compilerMessage;
    }

    protected internal static CompilerMessage CreateCompilerMessageFromMatchedRegex(string line, Match m, string erroridentifier)
    {
      CompilerMessage compilerMessage;
      compilerMessage.file = m.Groups["filename"].Value;
      compilerMessage.message = line;
      compilerMessage.line = int.Parse(m.Groups["line"].Value);
      compilerMessage.column = int.Parse(m.Groups["column"].Value);
      compilerMessage.type = !(m.Groups["type"].Value == erroridentifier) ? CompilerMessageType.Warning : CompilerMessageType.Error;
      compilerMessage.normalizedStatus = new NormalizedCompilerStatus();
      return compilerMessage;
    }

    public virtual IEnumerable<CompilerMessage> Parse(string[] errorOutput, bool compilationHadFailure)
    {
      return this.Parse(errorOutput, new string[0], compilationHadFailure);
    }

    public virtual IEnumerable<CompilerMessage> Parse(string[] errorOutput, string[] standardOutput, bool compilationHadFailure)
    {
      bool flag = false;
      List<CompilerMessage> compilerMessageList = new List<CompilerMessage>();
      Regex outputRegex = this.GetOutputRegex();
      foreach (string line in errorOutput)
      {
        string input = line.Length <= 1000 ? line : line.Substring(0, 100);
        Match match = outputRegex.Match(input);
        if (match.Success)
        {
          CompilerMessage fromMatchedRegex = CompilerOutputParserBase.CreateCompilerMessageFromMatchedRegex(line, match, this.GetErrorIdentifier());
          fromMatchedRegex.normalizedStatus = this.NormalizedStatusFor(match);
          if (fromMatchedRegex.type == CompilerMessageType.Error)
            flag = true;
          compilerMessageList.Add(fromMatchedRegex);
        }
      }
      if (compilationHadFailure && !flag)
        compilerMessageList.Add(CompilerOutputParserBase.CreateInternalCompilerErrorMessage(errorOutput));
      return (IEnumerable<CompilerMessage>) compilerMessageList;
    }

    protected virtual NormalizedCompilerStatus NormalizedStatusFor(Match match)
    {
      return new NormalizedCompilerStatus();
    }

    protected abstract string GetErrorIdentifier();

    protected abstract Regex GetOutputRegex();

    protected static NormalizedCompilerStatus TryNormalizeCompilerStatus(Match match, string memberNotFoundError, Regex missingMemberRegex)
    {
      string str = match.Groups["id"].Value;
      NormalizedCompilerStatus normalizedCompilerStatus = new NormalizedCompilerStatus();
      if (str != memberNotFoundError)
        return normalizedCompilerStatus;
      normalizedCompilerStatus.code = NormalizedCompilerStatusCode.MemberNotFound;
      Match match1 = missingMemberRegex.Match(match.Groups["message"].Value);
      normalizedCompilerStatus.details = match1.Groups["type_name"].Value + "%" + match1.Groups["member_name"].Value;
      return normalizedCompilerStatus;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.Cil2AsOutputParser
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace UnityEditor.Scripting.Compilers
{
  internal class Cil2AsOutputParser : UnityScriptCompilerOutputParser
  {
    [DebuggerHidden]
    public override IEnumerable<CompilerMessage> Parse(string[] errorOutput, string[] standardOutput, bool compilationHadFailure)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Cil2AsOutputParser.\u003CParse\u003Ec__IteratorA parseCIteratorA = new Cil2AsOutputParser.\u003CParse\u003Ec__IteratorA() { errorOutput = errorOutput, \u003C\u0024\u003EerrorOutput = errorOutput };
      // ISSUE: reference to a compiler-generated field
      parseCIteratorA.\u0024PC = -2;
      return (IEnumerable<CompilerMessage>) parseCIteratorA;
    }

    private static CompilerMessage CompilerErrorFor(StringBuilder currentErrorBuffer)
    {
      return new CompilerMessage() { type = CompilerMessageType.Error, message = currentErrorBuffer.ToString() };
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEngine.Assertions.AssertionException
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.Assertions
{
  /// <summary>
  ///   <para>An exception that is thrown on a failure. Assertions.Assert._raiseExceptions needs to be set to true.</para>
  /// </summary>
  public class AssertionException : Exception
  {
    private string m_UserMessage;

    public override string Message
    {
      get
      {
        string str = base.Message;
        if (this.m_UserMessage != null)
          str = str + (object) '\n' + this.m_UserMessage;
        return str;
      }
    }

    public AssertionException(string message, string userMessage)
      : base(message)
    {
      this.m_UserMessage = userMessage;
    }
  }
}

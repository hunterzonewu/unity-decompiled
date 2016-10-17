// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.Message
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>Messages from the version control system.</para>
  /// </summary>
  public sealed class Message
  {
    private IntPtr m_thisDummy;

    /// <summary>
    ///   <para>The severity of the message.</para>
    /// </summary>
    public Message.Severity severity { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The message text.</para>
    /// </summary>
    public string message { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal Message()
    {
    }

    ~Message()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();

    /// <summary>
    ///   <para>Write the message to the console.</para>
    /// </summary>
    public void Show()
    {
      Message.Info(this.message);
    }

    private static void Info(string message)
    {
      Debug.Log((object) ("Version control:\n" + message));
    }

    /// <summary>
    ///   <para>Severity of a version control message.</para>
    /// </summary>
    [System.Flags]
    public enum Severity
    {
      Data = 0,
      Verbose = 1,
      Info = 2,
      Warning = Info | Verbose,
      Error = 4,
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEngine.Ping
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Ping any given IP address (given in dot notation).</para>
  /// </summary>
  public sealed class Ping
  {
    private IntPtr pingWrapper;

    /// <summary>
    ///   <para>Has the ping function completed?</para>
    /// </summary>
    public bool isDone { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>This property contains the ping time result after isDone returns true.</para>
    /// </summary>
    public int time { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The IP target of the ping.</para>
    /// </summary>
    public string ip { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Perform a ping to the supplied target IP address.</para>
    /// </summary>
    /// <param name="address"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Ping(string address);

    ~Ping()
    {
      this.DestroyPing();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void DestroyPing();
  }
}

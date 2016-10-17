// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.ChangeSet
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>Wrapper around a changeset description and ID.</para>
  /// </summary>
  public sealed class ChangeSet
  {
    /// <summary>
    ///   <para>The ID of  the default changeset.</para>
    /// </summary>
    public static string defaultID = "-1";
    private IntPtr m_thisDummy;

    /// <summary>
    ///   <para>Description of a changeset.</para>
    /// </summary>
    public string description { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Version control specific ID of a changeset.</para>
    /// </summary>
    public string id { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public ChangeSet()
    {
      this.InternalCreate();
    }

    public ChangeSet(string description)
    {
      this.InternalCreateFromString(description);
    }

    public ChangeSet(string description, string revision)
    {
      this.InternalCreateFromStringString(description, revision);
    }

    public ChangeSet(ChangeSet other)
    {
      this.InternalCopyConstruct(other);
    }

    ~ChangeSet()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InternalCreate();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InternalCopyConstruct(ChangeSet other);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InternalCreateFromString(string description);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InternalCreateFromStringString(string description, string changeSetID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();
  }
}

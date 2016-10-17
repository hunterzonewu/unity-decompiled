// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.Task
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>A UnityEditor.VersionControl.Task is created almost everytime UnityEditor.VersionControl.Provider is ask to perform an action.</para>
  /// </summary>
  public sealed class Task
  {
    private IntPtr m_thisDummy;

    public int userIdentifier { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Will contain the result of the Provider.ChangeSetDescription task.</para>
    /// </summary>
    public string text { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>A short description of the current task.</para>
    /// </summary>
    public string description { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get whether or not the task was completed succesfully.</para>
    /// </summary>
    public bool success { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Total time spent in task since the task was started.</para>
    /// </summary>
    public int secondsSpent { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Progress of current task in precent.</para>
    /// </summary>
    public int progressPct { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string progressMessage { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Some task return result codes, these are stored here.</para>
    /// </summary>
    public int resultCode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>May contain messages from the version control plugins.</para>
    /// </summary>
    public Message[] messages { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The result of some types of tasks.</para>
    /// </summary>
    public AssetList assetList
    {
      get
      {
        AssetList assetList = new AssetList();
        foreach (Asset asset in this.Internal_GetAssetList())
          assetList.Add(asset);
        return assetList;
      }
    }

    /// <summary>
    ///   <para>List of changesets returned by some tasks.</para>
    /// </summary>
    public ChangeSets changeSets
    {
      get
      {
        ChangeSets changeSets = new ChangeSets();
        foreach (ChangeSet changeSet in this.Internal_GetChangeSets())
          changeSets.Add(changeSet);
        return changeSets;
      }
    }

    internal Task()
    {
    }

    ~Task()
    {
      this.Dispose();
    }

    /// <summary>
    ///   <para>A blocking wait for the task to complete.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Wait();

    /// <summary>
    ///   <para>Upon completion of a task a completion task will be performed if it is set.</para>
    /// </summary>
    /// <param name="action">Which completion action to perform.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetCompletionAction(CompletionAction action);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Asset[] Internal_GetAssetList();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private ChangeSet[] Internal_GetChangeSets();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Message[] Internal_GetMessages();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();
  }
}

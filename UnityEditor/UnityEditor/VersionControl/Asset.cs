// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.Asset
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>This class containes information about the version control state of an asset.</para>
  /// </summary>
  public sealed class Asset
  {
    private GUID m_guid;

    /// <summary>
    ///   <para>Gets the version control state of the asset.</para>
    /// </summary>
    public Asset.States state { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Gets the path of the asset.</para>
    /// </summary>
    public string path { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the asset is a folder.</para>
    /// </summary>
    public bool isFolder { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true is the asset is read only.</para>
    /// </summary>
    public bool readOnly { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the instance of the Asset class actually refers to a .meta file.</para>
    /// </summary>
    public bool isMeta { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the asset is locked by the version control system.</para>
    /// </summary>
    public bool locked { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get the name of the asset.</para>
    /// </summary>
    public string name { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Gets the full name of the asset including extension.</para>
    /// </summary>
    public string fullName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the assets is in the current project.</para>
    /// </summary>
    public bool isInCurrentProject { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal bool IsUnderVersionControl
    {
      get
      {
        if (!this.IsState(Asset.States.Synced) && !this.IsState(Asset.States.OutOfSync))
          return this.IsState(Asset.States.AddedLocal);
        return true;
      }
    }

    public string prettyPath
    {
      get
      {
        return this.path;
      }
    }

    public Asset(string clientPath)
    {
      this.InternalCreateFromString(clientPath);
    }

    ~Asset()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InternalCreateFromString(string clientPath);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool IsChildOf(Asset other);

    internal static bool IsState(Asset.States isThisState, Asset.States partOfThisState)
    {
      return (isThisState & partOfThisState) != Asset.States.None;
    }

    public bool IsState(Asset.States state)
    {
      return Asset.IsState(this.state, state);
    }

    public bool IsOneOfStates(Asset.States[] states)
    {
      foreach (Asset.States state in states)
      {
        if ((this.state & state) != Asset.States.None)
          return true;
      }
      return false;
    }

    /// <summary>
    ///   <para>Opens the assets in an associated editor.</para>
    /// </summary>
    public void Edit()
    {
      UnityEngine.Object target = this.Load();
      if (!(target != (UnityEngine.Object) null))
        return;
      AssetDatabase.OpenAsset(target);
    }

    /// <summary>
    ///   <para>Loads the asset to memory.</para>
    /// </summary>
    public UnityEngine.Object Load()
    {
      if (this.state == Asset.States.DeletedLocal || this.isMeta)
        return (UnityEngine.Object) null;
      return AssetDatabase.LoadAssetAtPath(this.path, typeof (UnityEngine.Object));
    }

    internal static string StateToString(Asset.States state)
    {
      if (Asset.IsState(state, Asset.States.AddedLocal))
        return "Added Local";
      if (Asset.IsState(state, Asset.States.AddedRemote))
        return "Added Remote";
      if (Asset.IsState(state, Asset.States.CheckedOutLocal) && !Asset.IsState(state, Asset.States.LockedLocal))
        return "Checked Out Local";
      if (Asset.IsState(state, Asset.States.CheckedOutRemote) && !Asset.IsState(state, Asset.States.LockedRemote))
        return "Checked Out Remote";
      if (Asset.IsState(state, Asset.States.Conflicted))
        return "Conflicted";
      if (Asset.IsState(state, Asset.States.DeletedLocal))
        return "Deleted Local";
      if (Asset.IsState(state, Asset.States.DeletedRemote))
        return "Deleted Remote";
      if (Asset.IsState(state, Asset.States.Local))
        return "Local";
      if (Asset.IsState(state, Asset.States.LockedLocal))
        return "Locked Local";
      if (Asset.IsState(state, Asset.States.LockedRemote))
        return "Locked Remote";
      if (Asset.IsState(state, Asset.States.OutOfSync))
        return "Out Of Sync";
      return string.Empty;
    }

    internal static string AllStateToString(Asset.States state)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (Asset.IsState(state, Asset.States.AddedLocal))
        stringBuilder.AppendLine("Added Local");
      if (Asset.IsState(state, Asset.States.AddedRemote))
        stringBuilder.AppendLine("Added Remote");
      if (Asset.IsState(state, Asset.States.CheckedOutLocal))
        stringBuilder.AppendLine("Checked Out Local");
      if (Asset.IsState(state, Asset.States.CheckedOutRemote))
        stringBuilder.AppendLine("Checked Out Remote");
      if (Asset.IsState(state, Asset.States.Conflicted))
        stringBuilder.AppendLine("Conflicted");
      if (Asset.IsState(state, Asset.States.DeletedLocal))
        stringBuilder.AppendLine("Deleted Local");
      if (Asset.IsState(state, Asset.States.DeletedRemote))
        stringBuilder.AppendLine("Deleted Remote");
      if (Asset.IsState(state, Asset.States.Local))
        stringBuilder.AppendLine("Local");
      if (Asset.IsState(state, Asset.States.LockedLocal))
        stringBuilder.AppendLine("Locked Local");
      if (Asset.IsState(state, Asset.States.LockedRemote))
        stringBuilder.AppendLine("Locked Remote");
      if (Asset.IsState(state, Asset.States.OutOfSync))
        stringBuilder.AppendLine("Out Of Sync");
      if (Asset.IsState(state, Asset.States.Synced))
        stringBuilder.AppendLine("Synced");
      if (Asset.IsState(state, Asset.States.Missing))
        stringBuilder.AppendLine("Missing");
      if (Asset.IsState(state, Asset.States.ReadOnly))
        stringBuilder.AppendLine("ReadOnly");
      return stringBuilder.ToString();
    }

    internal string AllStateToString()
    {
      return Asset.AllStateToString(this.state);
    }

    internal string StateToString()
    {
      return Asset.StateToString(this.state);
    }

    /// <summary>
    ///   <para>Describes the various version control states an asset can have.</para>
    /// </summary>
    [System.Flags]
    public enum States
    {
      None = 0,
      Local = 1,
      Synced = 2,
      OutOfSync = 4,
      Missing = 8,
      CheckedOutLocal = 16,
      CheckedOutRemote = 32,
      DeletedLocal = 64,
      DeletedRemote = 128,
      AddedLocal = 256,
      AddedRemote = 512,
      Conflicted = 1024,
      LockedLocal = 2048,
      LockedRemote = 4096,
      Updating = 8192,
      ReadOnly = 16384,
      MetaFile = 32768,
    }
  }
}

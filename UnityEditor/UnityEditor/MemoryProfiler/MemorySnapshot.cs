// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.MemorySnapshot
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>MemorySnapshot is a profiling tool to help diagnose memory usage.</para>
  /// </summary>
  public static class MemorySnapshot
  {
    public static event System.Action<PackedMemorySnapshot> OnSnapshotReceived;

    /// <summary>
    ///   <para>Requests a new snapshot from the currently connected target of the profiler. Currently only il2cpp-based players are able to provide memory snapshots.</para>
    /// </summary>
    public static void RequestNewSnapshot()
    {
      ProfilerDriver.RequestMemorySnapshot();
    }

    private static void DispatchSnapshot(PackedMemorySnapshot snapshot)
    {
      System.Action<PackedMemorySnapshot> snapshotReceived = MemorySnapshot.OnSnapshotReceived;
      if (snapshotReceived == null)
        return;
      snapshotReceived(snapshot);
    }
  }
}

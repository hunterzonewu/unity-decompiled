// Decompiled with JetBrains decompiler
// Type: UnityEditor.MemoryProfiler.PackedMemorySnapshot
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.MemoryProfiler
{
  /// <summary>
  ///   <para>PackedMemorySnapshot is a compact representation of a memory snapshot that a player has sent through the profiler connection.</para>
  /// </summary>
  [Serializable]
  public class PackedMemorySnapshot
  {
    [SerializeField]
    internal VirtualMachineInformation m_VirtualMachineInformation = new VirtualMachineInformation();
    [SerializeField]
    internal PackedNativeType[] m_NativeTypes;
    [SerializeField]
    internal PackedNativeUnityEngineObject[] m_NativeObjects;
    [SerializeField]
    internal PackedGCHandle[] m_GcHandles;
    [SerializeField]
    internal Connection[] m_Connections;
    [SerializeField]
    internal MemorySection[] m_ManagedHeapSections;
    [SerializeField]
    internal MemorySection[] m_ManagedStacks;
    [SerializeField]
    internal TypeDescription[] m_TypeDescriptions;

    /// <summary>
    ///   <para>Descriptions of all the C++ unity types the profiled player knows about.</para>
    /// </summary>
    public PackedNativeType[] nativeTypes
    {
      get
      {
        return this.m_NativeTypes;
      }
    }

    /// <summary>
    ///   <para>All native C++ objects that were loaded at time of the snapshot.</para>
    /// </summary>
    public PackedNativeUnityEngineObject[] nativeObjects
    {
      get
      {
        return this.m_NativeObjects;
      }
    }

    /// <summary>
    ///   <para>All GC handles in use in the memorysnapshot.</para>
    /// </summary>
    public PackedGCHandle[] gcHandles
    {
      get
      {
        return this.m_GcHandles;
      }
    }

    /// <summary>
    ///   <para>Connections is an array of from,to pairs that describe which things are keeping which other things alive.</para>
    /// </summary>
    public Connection[] connections
    {
      get
      {
        return this.m_Connections;
      }
    }

    /// <summary>
    ///   <para>Array of actual managed heap memory sections.</para>
    /// </summary>
    public MemorySection[] managedHeapSections
    {
      get
      {
        return this.m_ManagedHeapSections;
      }
    }

    /// <summary>
    ///   <para>Descriptions of all the managed types that were known to the virtual machine when the snapshot was taken.</para>
    /// </summary>
    public TypeDescription[] typeDescriptions
    {
      get
      {
        return this.m_TypeDescriptions;
      }
    }

    /// <summary>
    ///   <para>Information about the virtual machine running executing the managade code inside the player.</para>
    /// </summary>
    public VirtualMachineInformation virtualMachineInformation
    {
      get
      {
        return this.m_VirtualMachineInformation;
      }
    }

    internal PackedMemorySnapshot()
    {
    }
  }
}

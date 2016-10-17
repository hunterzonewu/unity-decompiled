// Decompiled with JetBrains decompiler
// Type: UnityEditor.NavMeshBuilder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Navigation mesh builder interface.</para>
  /// </summary>
  public sealed class NavMeshBuilder
  {
    public static extern UnityEngine.Object navMeshSettingsObject { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if an asynchronous build is still running.</para>
    /// </summary>
    public static extern bool isRunning { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern UnityEngine.Object sceneNavMeshData { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Build the Navmesh.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void BuildNavMesh();

    /// <summary>
    ///   <para>Build the Navmesh Asyncronously.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void BuildNavMeshAsync();

    /// <summary>
    ///   <para>Clear all Navmeshes.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearAllNavMeshes();

    /// <summary>
    ///   <para>Cancel Navmesh construction.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Cancel();

    /// <summary>
    ///   <para>Builds the combined navmesh for the contents of multiple scenes.</para>
    /// </summary>
    /// <param name="paths">Array of paths to scenes that are used for building the navmesh.</param>
    public static void BuildNavMeshForMultipleScenes(string[] paths)
    {
      if (paths.Length == 0)
        return;
      for (int index1 = 0; index1 < paths.Length; ++index1)
      {
        for (int index2 = index1 + 1; index2 < paths.Length; ++index2)
        {
          if (paths[index1] == paths[index2])
            throw new Exception("No duplicate scene names are allowed");
        }
      }
      if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        return;
      if (!EditorSceneManager.OpenScene(paths[0]).IsValid())
        throw new Exception("Could not open scene: " + paths[0]);
      for (int index = 1; index < paths.Length; ++index)
        EditorSceneManager.OpenScene(paths[index], OpenSceneMode.Additive);
      NavMeshBuilder.BuildNavMesh();
      UnityEngine.Object sceneNavMeshData = NavMeshBuilder.sceneNavMeshData;
      for (int index = 0; index < paths.Length; ++index)
      {
        if (EditorSceneManager.OpenScene(paths[index]).IsValid())
        {
          NavMeshBuilder.sceneNavMeshData = sceneNavMeshData;
          EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }
      }
    }
  }
}

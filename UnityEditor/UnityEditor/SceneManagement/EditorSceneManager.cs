// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneManagement.EditorSceneManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;

namespace UnityEditor.SceneManagement
{
  /// <summary>
  ///   <para>Scene management in the editor.</para>
  /// </summary>
  public sealed class EditorSceneManager : SceneManager
  {
    /// <summary>
    ///   <para>The number of loaded scenes.</para>
    /// </summary>
    public static extern int loadedSceneCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Open a scene in the Editor.</para>
    /// </summary>
    /// <param name="scenePath">Path of the scene. Should be relative to the project folder. Like: "AssetsMyScenesMyScene.unity".</param>
    /// <param name="mode">Allows you to select how to open the specified scene, and whether to keep existing scenes in the Hierarchy. See SceneManagement.OpenSceneMode for more information about the options.</param>
    public static Scene OpenScene(string scenePath, [DefaultValue("OpenSceneMode.Single")] OpenSceneMode mode)
    {
      Scene scene;
      EditorSceneManager.INTERNAL_CALL_OpenScene(scenePath, mode, out scene);
      return scene;
    }

    [ExcludeFromDocs]
    public static Scene OpenScene(string scenePath)
    {
      OpenSceneMode mode = OpenSceneMode.Single;
      Scene scene;
      EditorSceneManager.INTERNAL_CALL_OpenScene(scenePath, mode, out scene);
      return scene;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_OpenScene(string scenePath, OpenSceneMode mode, out Scene value);

    /// <summary>
    ///   <para>Create a new scene.</para>
    /// </summary>
    /// <param name="setup">Allows you to select whether or not the default set of Game Objects should be added to the new scene. See SceneManagement.NewSceneSetup for more information about the options.</param>
    /// <param name="mode">Allows you to select how to open the new scene, and whether to keep existing scenes in the Hierarchy. See SceneManagement.NewSceneMode for more information about the options.</param>
    public static Scene NewScene(NewSceneSetup setup, [DefaultValue("NewSceneMode.Single")] NewSceneMode mode)
    {
      Scene scene;
      EditorSceneManager.INTERNAL_CALL_NewScene(setup, mode, out scene);
      return scene;
    }

    [ExcludeFromDocs]
    public static Scene NewScene(NewSceneSetup setup)
    {
      NewSceneMode mode = NewSceneMode.Single;
      Scene scene;
      EditorSceneManager.INTERNAL_CALL_NewScene(setup, mode, out scene);
      return scene;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_NewScene(NewSceneSetup setup, NewSceneMode mode, out Scene value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CreateSceneAsset(string scenePath, bool createDefaultGameObjects);

    /// <summary>
    ///   <para>Close the scene. If removeScene flag is true, the closed scene will also be removed from EditorSceneManager.</para>
    /// </summary>
    /// <param name="scene">The scene to be closed/removed.</param>
    /// <param name="removeScene">Bool flag to indicate if the scene should be removed after closing.</param>
    /// <returns>
    ///   <para>Returns true if the scene is closed/removed.</para>
    /// </returns>
    public static bool CloseScene(Scene scene, bool removeScene)
    {
      return EditorSceneManager.INTERNAL_CALL_CloseScene(ref scene, removeScene);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_CloseScene(ref Scene scene, bool removeScene);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetTargetSceneForNewGameObjects(int sceneHandle);

    internal static Scene GetSceneByHandle(int handle)
    {
      Scene scene;
      EditorSceneManager.INTERNAL_CALL_GetSceneByHandle(handle, out scene);
      return scene;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSceneByHandle(int handle, out Scene value);

    /// <summary>
    ///   <para>Allows you to reorder the scenes currently open in the Hierarchy window. Moves the source scene so it comes before the destination scene.</para>
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dst"></param>
    public static void MoveSceneBefore(Scene src, Scene dst)
    {
      EditorSceneManager.INTERNAL_CALL_MoveSceneBefore(ref src, ref dst);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_MoveSceneBefore(ref Scene src, ref Scene dst);

    /// <summary>
    ///   <para>Allows you to reorder the scenes currently open in the Hierarchy window. Moves the source scene so it comes after the destination scene.</para>
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dst"></param>
    public static void MoveSceneAfter(Scene src, Scene dst)
    {
      EditorSceneManager.INTERNAL_CALL_MoveSceneAfter(ref src, ref dst);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_MoveSceneAfter(ref Scene src, ref Scene dst);

    internal static bool SaveSceneAs(Scene scene)
    {
      return EditorSceneManager.INTERNAL_CALL_SaveSceneAs(ref scene);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_SaveSceneAs(ref Scene scene);

    /// <summary>
    ///   <para>Save a scene.</para>
    /// </summary>
    /// <param name="scene">The scene to be saved.</param>
    /// <param name="dstScenePath">The file path to save at. If not empty, the current open scene will be overwritten, or if never saved before, a save dialog is shown.</param>
    /// <param name="saveAsCopy">If set to true, the scene will be saved without changing the current scene and without clearing the unsaved changes marker.</param>
    /// <returns>
    ///   <para>True if the save succeeded, otherwise false.</para>
    /// </returns>
    public static bool SaveScene(Scene scene, [DefaultValue("\"\"")] string dstScenePath, [DefaultValue("false")] bool saveAsCopy)
    {
      return EditorSceneManager.INTERNAL_CALL_SaveScene(ref scene, dstScenePath, saveAsCopy);
    }

    [ExcludeFromDocs]
    public static bool SaveScene(Scene scene, string dstScenePath)
    {
      bool saveAsCopy = false;
      return EditorSceneManager.INTERNAL_CALL_SaveScene(ref scene, dstScenePath, saveAsCopy);
    }

    [ExcludeFromDocs]
    public static bool SaveScene(Scene scene)
    {
      bool saveAsCopy = false;
      string empty = string.Empty;
      return EditorSceneManager.INTERNAL_CALL_SaveScene(ref scene, empty, saveAsCopy);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_SaveScene(ref Scene scene, string dstScenePath, bool saveAsCopy);

    /// <summary>
    ///   <para>Save all open scenes.</para>
    /// </summary>
    /// <returns>
    ///   <para>Returns true if all open scenes are successfully saved.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SaveOpenScenes();

    /// <summary>
    ///   <para>Save a list of scenes.</para>
    /// </summary>
    /// <param name="scenes">List of scenes that should be saved.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SaveScenes(Scene[] scenes);

    /// <summary>
    ///   <para>Ask the user if they want to save the the modified scene(s).</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SaveCurrentModifiedScenesIfUserWantsTo();

    /// <summary>
    ///   <para>Ask the user if he wants to save any of the modfied input scenes.</para>
    /// </summary>
    /// <param name="scenes">Scenes that should be saved if they are modified.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SaveModifiedScenesIfUserWantsTo(Scene[] scenes);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool EnsureUntitledSceneHasBeenSaved(string operation);

    /// <summary>
    ///   <para>Mark the scene as modified.</para>
    /// </summary>
    /// <param name="scene">The scene to be marked as modified.</param>
    public static bool MarkSceneDirty(Scene scene)
    {
      return EditorSceneManager.INTERNAL_CALL_MarkSceneDirty(ref scene);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_MarkSceneDirty(ref Scene scene);

    /// <summary>
    ///   <para>Mark all the loaded scenes as modified.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void MarkAllScenesDirty();

    /// <summary>
    ///   <para>Returns the current setup of the SceneManager.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern SceneSetup[] GetSceneManagerSetup();

    /// <summary>
    ///   <para>Restore the setup of the SceneManager.</para>
    /// </summary>
    /// <param name="value">In this array, at least one scene should be loaded, and there must be one active scene.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RestoreSceneManagerSetup(SceneSetup[] value);
  }
}

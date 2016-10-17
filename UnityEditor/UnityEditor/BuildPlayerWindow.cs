// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildPlayerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor.Connect;
using UnityEditor.Modules;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  internal class BuildPlayerWindow : EditorWindow
  {
    private ListViewState lv = new ListViewState();
    private bool[] selectedLVItems = new bool[0];
    private int initialSelectedLVItem = -1;
    private Vector2 scrollPosition = new Vector2(0.0f, 0.0f);
    private const string kAssetsFolder = "Assets/";
    private const string kEditorBuildSettingsPath = "ProjectSettings/EditorBuildSettings.asset";
    private static BuildPlayerWindow.BuildPlatforms s_BuildPlatforms;
    private bool[] selectedBeforeDrag;
    private static BuildPlayerWindow.Styles styles;

    public BuildPlayerWindow()
    {
      this.position = new Rect(50f, 50f, 540f, 530f);
      this.minSize = new Vector2(630f, 580f);
      this.titleContent = new GUIContent("Build Settings");
    }

    private static void ShowBuildPlayerWindow()
    {
      EditorUserBuildSettings.selectedBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
      EditorWindow.GetWindow<BuildPlayerWindow>(true, "Build Settings");
    }

    private static void BuildPlayerAndRun()
    {
      if (BuildPlayerWindow.BuildPlayerWithDefaultSettings(false, BuildOptions.AutoRunPlayer))
        return;
      BuildPlayerWindow.ShowBuildPlayerWindow();
    }

    private static void BuildPlayerAndSelect()
    {
      if (BuildPlayerWindow.BuildPlayerWithDefaultSettings(false, BuildOptions.ShowBuiltPlayer))
        return;
      BuildPlayerWindow.ShowBuildPlayerWindow();
    }

    private static bool BuildPlayerWithDefaultSettings(bool askForBuildLocation, BuildOptions forceOptions)
    {
      return BuildPlayerWindow.BuildPlayerWithDefaultSettings(askForBuildLocation, forceOptions, true);
    }

    private static bool IsMetroPlayer(BuildTarget target)
    {
      return target == BuildTarget.WSAPlayer;
    }

    private static bool IsWP8Player(BuildTarget target)
    {
      return target == BuildTarget.WP8Player;
    }

    private static bool BuildPlayerWithDefaultSettings(bool askForBuildLocation, BuildOptions forceOptions, bool first)
    {
      bool updateExistingBuild = false;
      BuildPlayerWindow.InitBuildPlatforms();
      if (!UnityConnect.instance.canBuildWithUPID && !EditorUtility.DisplayDialog("Missing Project ID", "Because you are not a member of this project this build will not access Unity services.\nDo you want to continue?", "Yes", "No"))
        return false;
      BuildTarget selectedBuildTarget = BuildPlayerWindow.CalculateSelectedBuildTarget();
      if (!BuildPipeline.IsBuildTargetSupported(selectedBuildTarget))
        return false;
      IBuildWindowExtension buildWindowExtension = ModuleManager.GetBuildWindowExtension(ModuleManager.GetTargetStringFromBuildTargetGroup(BuildPlayerWindow.s_BuildPlatforms.BuildPlatformFromTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup).targetGroup));
      if (buildWindowExtension != null && (forceOptions & BuildOptions.AutoRunPlayer) != BuildOptions.None && !buildWindowExtension.EnabledBuildAndRunButton())
        return false;
      if (Unsupported.IsBleedingEdgeBuild())
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("This version of Unity is a BleedingEdge build that has not seen any manual testing.");
        stringBuilder.AppendLine("You should consider this build unstable.");
        stringBuilder.AppendLine("We strongly recommend that you use a normal version of Unity instead.");
        if (EditorUtility.DisplayDialog("BleedingEdge Build", stringBuilder.ToString(), "Cancel", "OK"))
          return false;
      }
      if (selectedBuildTarget == BuildTarget.BlackBerry && (forceOptions & BuildOptions.AutoRunPlayer) != BuildOptions.None && (string.IsNullOrEmpty(PlayerSettings.BlackBerry.deviceAddress) || string.IsNullOrEmpty(PlayerSettings.BlackBerry.devicePassword)))
      {
        Debug.LogError((object) EditorGUIUtility.TextContent("Author Id, Device Address and Device Password must all be set in order to use Build and Run").text);
        return false;
      }
      string str = string.Empty;
      bool flag = EditorUserBuildSettings.installInBuildFolder && PostprocessBuildPlayer.SupportsInstallInBuildFolder(selectedBuildTarget) && (Unsupported.IsDeveloperBuild() || BuildPlayerWindow.IsMetroPlayer(selectedBuildTarget) || BuildPlayerWindow.IsWP8Player(selectedBuildTarget));
      BuildOptions options = forceOptions;
      bool development = EditorUserBuildSettings.development;
      if (development)
        options |= BuildOptions.Development;
      if (EditorUserBuildSettings.allowDebugging && development)
        options |= BuildOptions.AllowDebugging;
      if (EditorUserBuildSettings.symlinkLibraries)
        options |= BuildOptions.SymlinkLibraries;
      if (EditorUserBuildSettings.exportAsGoogleAndroidProject)
        options |= BuildOptions.AcceptExternalModificationsToPlayer;
      if (EditorUserBuildSettings.webPlayerOfflineDeployment)
        options |= BuildOptions.WebPlayerOfflineDeployment;
      if (EditorUserBuildSettings.enableHeadlessMode)
        options |= BuildOptions.EnableHeadlessMode;
      if (EditorUserBuildSettings.connectProfiler && (development || selectedBuildTarget == BuildTarget.WSAPlayer || BuildPlayerWindow.IsWP8Player(selectedBuildTarget)))
        options |= BuildOptions.ConnectWithProfiler;
      if (EditorUserBuildSettings.buildScriptsOnly)
        options |= BuildOptions.BuildScriptsOnly;
      if (EditorUserBuildSettings.forceOptimizeScriptCompilation)
        options |= BuildOptions.ForceOptimizeScriptCompilation;
      if (flag)
        options |= BuildOptions.InstallInBuildFolder;
      if (!flag)
      {
        if (askForBuildLocation && !BuildPlayerWindow.PickBuildLocation(selectedBuildTarget, options, out updateExistingBuild))
          return false;
        str = EditorUserBuildSettings.GetBuildLocation(selectedBuildTarget);
        if (str.Length == 0)
          return false;
        if (!askForBuildLocation)
        {
          switch (InternalEditorUtility.BuildCanBeAppended(selectedBuildTarget, str))
          {
            case CanAppendBuild.Yes:
              updateExistingBuild = true;
              break;
            case CanAppendBuild.No:
              if (!BuildPlayerWindow.PickBuildLocation(selectedBuildTarget, options, out updateExistingBuild))
                return false;
              str = EditorUserBuildSettings.GetBuildLocation(selectedBuildTarget);
              if (str.Length == 0 || !Directory.Exists(FileUtil.DeleteLastPathNameComponent(str)))
                return false;
              break;
          }
        }
      }
      if (updateExistingBuild)
        options |= BuildOptions.AcceptExternalModificationsToPlayer;
      ArrayList arrayList = new ArrayList();
      foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
      {
        if (scene.enabled)
          arrayList.Add((object) scene.path);
      }
      string[] array = arrayList.ToArray(typeof (string)) as string[];
      bool delayToAfterScriptReload = false;
      if (EditorUserBuildSettings.activeBuildTarget != selectedBuildTarget)
      {
        if (!EditorUserBuildSettings.SwitchActiveBuildTarget(selectedBuildTarget))
        {
          Debug.LogErrorFormat("Could not switch to build target '{0}'.", (object) BuildPlayerWindow.s_BuildPlatforms.GetBuildTargetDisplayName(selectedBuildTarget));
          return false;
        }
        if (EditorApplication.isCompiling)
          delayToAfterScriptReload = true;
      }
      uint crc = 0;
      return BuildPipeline.BuildPlayerInternalNoCheck(array, str, selectedBuildTarget, options, delayToAfterScriptReload, out crc).Length == 0;
    }

    private void ActiveScenesGUI()
    {
      int num1 = 0;
      int row = this.lv.row;
      bool shift = Event.current.shift;
      bool actionKey = EditorGUI.actionKey;
      Event current = Event.current;
      Rect rect1 = GUILayoutUtility.GetRect(BuildPlayerWindow.styles.scenesInBuild, BuildPlayerWindow.styles.title);
      List<EditorBuildSettingsScene> source = new List<EditorBuildSettingsScene>((IEnumerable<EditorBuildSettingsScene>) EditorBuildSettings.scenes);
      this.lv.totalRows = source.Count;
      if (this.selectedLVItems.Length != source.Count)
        Array.Resize<bool>(ref this.selectedLVItems, source.Count);
      int[] numArray = new int[source.Count];
      for (int index = 0; index < numArray.Length; ++index)
      {
        EditorBuildSettingsScene buildSettingsScene = source[index];
        numArray[index] = num1;
        if (buildSettingsScene.enabled)
          ++num1;
      }
      foreach (ListViewElement listViewElement in ListViewGUILayout.ListView(this.lv, ListViewOptions.wantsReordering | ListViewOptions.wantsExternalFiles, BuildPlayerWindow.styles.box, new GUILayoutOption[0]))
      {
        EditorBuildSettingsScene buildSettingsScene = source[listViewElement.row];
        bool flag = File.Exists(buildSettingsScene.path);
        EditorGUI.BeginDisabledGroup(!flag);
        bool selectedLvItem = this.selectedLVItems[listViewElement.row];
        if (selectedLvItem && current.type == EventType.Repaint)
          BuildPlayerWindow.styles.selected.Draw(listViewElement.position, false, false, false, false);
        if (!flag)
          buildSettingsScene.enabled = false;
        Rect position = new Rect(listViewElement.position.x + 4f, listViewElement.position.y, BuildPlayerWindow.styles.toggleSize.x, BuildPlayerWindow.styles.toggleSize.y);
        EditorGUI.BeginChangeCheck();
        buildSettingsScene.enabled = GUI.Toggle(position, buildSettingsScene.enabled, string.Empty);
        if (EditorGUI.EndChangeCheck() && selectedLvItem)
        {
          for (int index = 0; index < source.Count; ++index)
          {
            if (this.selectedLVItems[index])
              source[index].enabled = buildSettingsScene.enabled;
          }
        }
        GUILayout.Space(BuildPlayerWindow.styles.toggleSize.x);
        string t = buildSettingsScene.path;
        if (t.StartsWith("Assets/"))
          t = t.Substring("Assets/".Length);
        if (t.EndsWith(".unity", StringComparison.InvariantCultureIgnoreCase))
          t = t.Substring(0, t.Length - ".unity".Length);
        Rect rect2 = GUILayoutUtility.GetRect(EditorGUIUtility.TempContent(t), BuildPlayerWindow.styles.levelString);
        if (Event.current.type == EventType.Repaint)
          BuildPlayerWindow.styles.levelString.Draw(rect2, EditorGUIUtility.TempContent(t), false, false, selectedLvItem, false);
        GUILayout.Label(!buildSettingsScene.enabled ? string.Empty : numArray[listViewElement.row].ToString(), BuildPlayerWindow.styles.levelStringCounter, new GUILayoutOption[1]
        {
          GUILayout.MaxWidth(36f)
        });
        EditorGUI.EndDisabledGroup();
        if (ListViewGUILayout.HasMouseUp(listViewElement.position) && !shift && !actionKey)
        {
          if (!shift && !actionKey)
            ListViewGUILayout.MultiSelection(row, listViewElement.row, ref this.initialSelectedLVItem, ref this.selectedLVItems);
        }
        else if (ListViewGUILayout.HasMouseDown(listViewElement.position))
        {
          if (!this.selectedLVItems[listViewElement.row] || shift || actionKey)
            ListViewGUILayout.MultiSelection(row, listViewElement.row, ref this.initialSelectedLVItem, ref this.selectedLVItems);
          this.lv.row = listViewElement.row;
          this.selectedBeforeDrag = new bool[this.selectedLVItems.Length];
          this.selectedLVItems.CopyTo((Array) this.selectedBeforeDrag, 0);
          this.selectedBeforeDrag[this.lv.row] = true;
        }
      }
      GUI.Label(rect1, BuildPlayerWindow.styles.scenesInBuild, BuildPlayerWindow.styles.title);
      if (GUIUtility.keyboardControl == this.lv.ID)
      {
        if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "SelectAll")
          Event.current.Use();
        else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "SelectAll")
        {
          for (int index = 0; index < this.selectedLVItems.Length; ++index)
            this.selectedLVItems[index] = true;
          this.lv.selectionChanged = true;
          Event.current.Use();
          GUIUtility.ExitGUI();
        }
      }
      if (this.lv.selectionChanged)
        ListViewGUILayout.MultiSelection(row, this.lv.row, ref this.initialSelectedLVItem, ref this.selectedLVItems);
      if (this.lv.fileNames != null)
      {
        Array.Sort<string>(this.lv.fileNames);
        int num2 = 0;
        for (int index = 0; index < this.lv.fileNames.Length; ++index)
        {
          if (this.lv.fileNames[index].EndsWith("unity", StringComparison.InvariantCultureIgnoreCase))
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            BuildPlayerWindow.\u003CActiveScenesGUI\u003Ec__AnonStorey32 guICAnonStorey32 = new BuildPlayerWindow.\u003CActiveScenesGUI\u003Ec__AnonStorey32();
            // ISSUE: reference to a compiler-generated field
            guICAnonStorey32.scenePath = FileUtil.GetProjectRelativePath(this.lv.fileNames[index]);
            // ISSUE: reference to a compiler-generated field
            if (guICAnonStorey32.scenePath == string.Empty)
            {
              // ISSUE: reference to a compiler-generated field
              guICAnonStorey32.scenePath = this.lv.fileNames[index];
            }
            // ISSUE: reference to a compiler-generated method
            if (!source.Any<EditorBuildSettingsScene>(new Func<EditorBuildSettingsScene, bool>(guICAnonStorey32.\u003C\u003Em__48)))
            {
              // ISSUE: reference to a compiler-generated field
              source.Insert(this.lv.draggedTo + num2++, new EditorBuildSettingsScene()
              {
                path = guICAnonStorey32.scenePath,
                enabled = true
              });
            }
          }
        }
        if (num2 != 0)
        {
          Array.Resize<bool>(ref this.selectedLVItems, source.Count);
          for (int index = 0; index < this.selectedLVItems.Length; ++index)
            this.selectedLVItems[index] = index >= this.lv.draggedTo && index < this.lv.draggedTo + num2;
        }
        this.lv.draggedTo = -1;
      }
      if (this.lv.draggedTo != -1)
      {
        List<EditorBuildSettingsScene> buildSettingsSceneList = new List<EditorBuildSettingsScene>();
        int index1 = 0;
        int index2 = 0;
        while (index2 < this.selectedLVItems.Length)
        {
          if (this.selectedBeforeDrag[index2])
          {
            buildSettingsSceneList.Add(source[index1]);
            source.RemoveAt(index1);
            --index1;
            if (this.lv.draggedTo >= index2)
              --this.lv.draggedTo;
          }
          ++index2;
          ++index1;
        }
        this.lv.draggedTo = this.lv.draggedTo > source.Count || this.lv.draggedTo < 0 ? source.Count : this.lv.draggedTo;
        source.InsertRange(this.lv.draggedTo, (IEnumerable<EditorBuildSettingsScene>) buildSettingsSceneList);
        for (int index3 = 0; index3 < this.selectedLVItems.Length; ++index3)
          this.selectedLVItems[index3] = index3 >= this.lv.draggedTo && index3 < this.lv.draggedTo + buildSettingsSceneList.Count;
      }
      if (current.type == EventType.KeyDown && (current.keyCode == KeyCode.Backspace || current.keyCode == KeyCode.Delete) && GUIUtility.keyboardControl == this.lv.ID)
      {
        int index1 = 0;
        int index2 = 0;
        while (index2 < this.selectedLVItems.Length)
        {
          if (this.selectedLVItems[index2])
          {
            source.RemoveAt(index1);
            --index1;
          }
          this.selectedLVItems[index2] = false;
          ++index2;
          ++index1;
        }
        this.lv.row = 0;
        current.Use();
      }
      EditorBuildSettings.scenes = source.ToArray();
    }

    private void AddOpenScenes()
    {
      List<EditorBuildSettingsScene> source = new List<EditorBuildSettingsScene>((IEnumerable<EditorBuildSettingsScene>) EditorBuildSettings.scenes);
      bool flag = false;
      for (int index = 0; index < SceneManager.sceneCount; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        BuildPlayerWindow.\u003CAddOpenScenes\u003Ec__AnonStorey33 scenesCAnonStorey33 = new BuildPlayerWindow.\u003CAddOpenScenes\u003Ec__AnonStorey33();
        // ISSUE: reference to a compiler-generated field
        scenesCAnonStorey33.scene = SceneManager.GetSceneAt(index);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if ((scenesCAnonStorey33.scene.path.Length != 0 || EditorSceneManager.SaveScene(scenesCAnonStorey33.scene, string.Empty, false)) && !source.Any<EditorBuildSettingsScene>(new Func<EditorBuildSettingsScene, bool>(scenesCAnonStorey33.\u003C\u003Em__49)))
        {
          // ISSUE: reference to a compiler-generated field
          source.Add(new EditorBuildSettingsScene()
          {
            path = scenesCAnonStorey33.scene.path,
            enabled = true
          });
          flag = true;
        }
      }
      if (!flag)
        return;
      EditorBuildSettings.scenes = source.ToArray();
      this.Repaint();
      GUIUtility.ExitGUI();
    }

    private static BuildTarget CalculateSelectedBuildTarget()
    {
      BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
      switch (buildTargetGroup)
      {
        case BuildTargetGroup.Standalone:
          return EditorUserBuildSettings.selectedStandaloneTarget;
        case BuildTargetGroup.WebPlayer:
          return EditorUserBuildSettings.webPlayerStreamed ? BuildTarget.WebPlayerStreamed : BuildTarget.WebPlayer;
        default:
          if (BuildPlayerWindow.s_BuildPlatforms == null)
            throw new Exception("Build platforms are not initialized.");
          BuildPlayerWindow.BuildPlatform buildPlatform = BuildPlayerWindow.s_BuildPlatforms.BuildPlatformFromTargetGroup(buildTargetGroup);
          if (buildPlatform == null)
            throw new Exception("Could not find build platform for target group " + (object) buildTargetGroup);
          return buildPlatform.DefaultTarget;
      }
    }

    private void ActiveBuildTargetsGUI()
    {
      GUILayout.BeginVertical();
      GUILayout.BeginVertical(GUILayout.Width((float) byte.MaxValue));
      GUILayout.Label(BuildPlayerWindow.styles.platformTitle, BuildPlayerWindow.styles.title, new GUILayoutOption[0]);
      this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, (GUIStyle) "OL Box");
      for (int index = 0; index < 2; ++index)
      {
        bool flag1 = index == 0;
        bool flag2 = false;
        foreach (BuildPlayerWindow.BuildPlatform buildPlatform in BuildPlayerWindow.s_BuildPlatforms.buildPlatforms)
        {
          if (BuildPlayerWindow.IsBuildTargetGroupSupported(buildPlatform.DefaultTarget) == flag1 && (BuildPlayerWindow.IsBuildTargetGroupSupported(buildPlatform.DefaultTarget) || buildPlatform.forceShowTarget))
          {
            this.ShowOption(buildPlatform, buildPlatform.title, !flag2 ? BuildPlayerWindow.styles.oddRow : BuildPlayerWindow.styles.evenRow);
            flag2 = !flag2;
          }
        }
        GUI.contentColor = Color.white;
      }
      GUILayout.EndScrollView();
      GUILayout.EndVertical();
      GUILayout.Space(10f);
      BuildTarget selectedBuildTarget = BuildPlayerWindow.CalculateSelectedBuildTarget();
      GUILayout.BeginHorizontal();
      GUI.enabled = BuildPipeline.IsBuildTargetSupported(selectedBuildTarget) && BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget) != BuildPipeline.GetBuildTargetGroup(selectedBuildTarget);
      if (GUILayout.Button(BuildPlayerWindow.styles.switchPlatform, new GUILayoutOption[1]
      {
        GUILayout.Width(110f)
      }))
      {
        EditorUserBuildSettings.SwitchActiveBuildTarget(selectedBuildTarget);
        GUIUtility.ExitGUI();
      }
      GUI.enabled = BuildPipeline.IsBuildTargetSupported(selectedBuildTarget);
      if (GUILayout.Button(new GUIContent("Player Settings..."), new GUILayoutOption[1]
      {
        GUILayout.Width(110f)
      }))
        Selection.activeObject = Unsupported.GetSerializedAssetInterfaceSingleton("PlayerSettings");
      GUILayout.EndHorizontal();
      GUI.enabled = true;
      GUILayout.EndVertical();
    }

    private void ShowAlert()
    {
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      GUILayout.BeginVertical();
      EditorGUILayout.HelpBox("Because you are not a member of this project this build will not access Unity services.", MessageType.Warning);
      GUILayout.EndVertical();
      GUILayout.Space(5f);
      GUILayout.EndHorizontal();
    }

    private void ShowOption(BuildPlayerWindow.BuildPlatform bp, GUIContent title, GUIStyle background)
    {
      Rect rect = GUILayoutUtility.GetRect(50f, 36f);
      ++rect.x;
      ++rect.y;
      GUI.contentColor = new Color(1f, 1f, 1f, !BuildPipeline.LicenseCheck(bp.DefaultTarget) ? 0.7f : 1f);
      bool on = EditorUserBuildSettings.selectedBuildTargetGroup == bp.targetGroup;
      if (Event.current.type == EventType.Repaint)
      {
        background.Draw(rect, GUIContent.none, false, false, on, false);
        GUI.Label(new Rect(rect.x + 3f, rect.y + 3f, 32f, 32f), title.image, GUIStyle.none);
        if (BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget) == bp.targetGroup)
          GUI.Label(new Rect((float) ((double) rect.xMax - (double) BuildPlayerWindow.styles.activePlatformIcon.width - 8.0), rect.y + 3f + (float) ((32 - BuildPlayerWindow.styles.activePlatformIcon.height) / 2), (float) BuildPlayerWindow.styles.activePlatformIcon.width, (float) BuildPlayerWindow.styles.activePlatformIcon.height), (Texture) BuildPlayerWindow.styles.activePlatformIcon, GUIStyle.none);
      }
      if (!GUI.Toggle(rect, on, title.text, BuildPlayerWindow.styles.platformSelector) || EditorUserBuildSettings.selectedBuildTargetGroup == bp.targetGroup)
        return;
      EditorUserBuildSettings.selectedBuildTargetGroup = bp.targetGroup;
      foreach (UnityEngine.Object @object in Resources.FindObjectsOfTypeAll(typeof (InspectorWindow)))
      {
        InspectorWindow inspectorWindow = @object as InspectorWindow;
        if ((UnityEngine.Object) inspectorWindow != (UnityEngine.Object) null)
          inspectorWindow.Repaint();
      }
    }

    private void OnGUI()
    {
      if (BuildPlayerWindow.styles == null)
      {
        BuildPlayerWindow.styles = new BuildPlayerWindow.Styles();
        BuildPlayerWindow.styles.toggleSize = BuildPlayerWindow.styles.toggle.CalcSize(new GUIContent("X"));
        this.lv.rowHeight = (int) BuildPlayerWindow.styles.levelString.CalcHeight(new GUIContent("X"), 100f);
      }
      BuildPlayerWindow.InitBuildPlatforms();
      if (!UnityConnect.instance.canBuildWithUPID)
        this.ShowAlert();
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      GUILayout.BeginVertical();
      string message = string.Empty;
      bool disabled = !AssetDatabase.IsOpenForEdit("ProjectSettings/EditorBuildSettings.asset", out message);
      EditorGUI.BeginDisabledGroup(disabled);
      this.ActiveScenesGUI();
      GUILayout.BeginHorizontal();
      if (disabled)
      {
        GUI.enabled = true;
        if (Provider.enabled && GUILayout.Button("Check out"))
        {
          Asset assetByPath = Provider.GetAssetByPath("ProjectSettings/EditorBuildSettings.asset");
          AssetList assets = new AssetList();
          assets.Add(assetByPath);
          Provider.Checkout(assets, CheckoutMode.Asset);
        }
        GUILayout.Label(message);
        GUI.enabled = false;
      }
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Add Open Scenes"))
        this.AddOpenScenes();
      GUILayout.EndHorizontal();
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal(GUILayout.Height(301f));
      this.ActiveBuildTargetsGUI();
      GUILayout.Space(10f);
      GUILayout.BeginVertical();
      this.ShowBuildTargetSettings();
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
      GUILayout.EndVertical();
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
    }

    private static BuildTarget RestoreLastKnownPlatformsBuildTarget(BuildPlayerWindow.BuildPlatform bp)
    {
      switch (bp.targetGroup)
      {
        case BuildTargetGroup.Standalone:
          return EditorUserBuildSettings.selectedStandaloneTarget;
        case BuildTargetGroup.WebPlayer:
          return EditorUserBuildSettings.webPlayerStreamed ? BuildTarget.WebPlayerStreamed : BuildTarget.WebPlayer;
        default:
          return bp.DefaultTarget;
      }
    }

    private static void InitBuildPlatforms()
    {
      if (BuildPlayerWindow.s_BuildPlatforms != null)
        return;
      BuildPlayerWindow.s_BuildPlatforms = new BuildPlayerWindow.BuildPlatforms();
      BuildPlayerWindow.RepairSelectedBuildTargetGroup();
    }

    internal static List<BuildPlayerWindow.BuildPlatform> GetValidPlatforms()
    {
      BuildPlayerWindow.InitBuildPlatforms();
      List<BuildPlayerWindow.BuildPlatform> buildPlatformList = new List<BuildPlayerWindow.BuildPlatform>();
      foreach (BuildPlayerWindow.BuildPlatform buildPlatform in BuildPlayerWindow.s_BuildPlatforms.buildPlatforms)
      {
        if (buildPlatform.targetGroup == BuildTargetGroup.Standalone || BuildPipeline.IsBuildTargetSupported(buildPlatform.DefaultTarget))
          buildPlatformList.Add(buildPlatform);
      }
      return buildPlatformList;
    }

    internal static bool IsBuildTargetGroupSupported(BuildTarget target)
    {
      if (target == BuildTarget.StandaloneWindows)
        return true;
      return BuildPipeline.IsBuildTargetSupported(target);
    }

    private static void RepairSelectedBuildTargetGroup()
    {
      BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
      if (buildTargetGroup != BuildTargetGroup.Unknown && BuildPlayerWindow.s_BuildPlatforms != null && BuildPlayerWindow.s_BuildPlatforms.BuildPlatformIndexFromTargetGroup(buildTargetGroup) >= 0)
        return;
      EditorUserBuildSettings.selectedBuildTargetGroup = BuildTargetGroup.WebPlayer;
    }

    private static bool IsAnyStandaloneModuleLoaded()
    {
      if (!ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneLinux)) && !ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneOSXIntel)))
        return ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneWindows));
      return true;
    }

    private static BuildTarget GetBestStandaloneTarget(BuildTarget selectedTarget)
    {
      if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(selectedTarget)))
        return selectedTarget;
      if (Application.platform == RuntimePlatform.WindowsEditor && ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneWindows)))
        return BuildTarget.StandaloneWindows;
      if (Application.platform == RuntimePlatform.OSXEditor && ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneOSXIntel)) || ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneOSXIntel)))
        return BuildTarget.StandaloneOSXIntel;
      return ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneLinux)) ? BuildTarget.StandaloneLinux : BuildTarget.StandaloneWindows;
    }

    private static string GetPlaybackEngineDownloadURL(string moduleName)
    {
      string unityVersionFull = InternalEditorUtility.GetUnityVersionFull();
      string str1 = string.Empty;
      string str2 = string.Empty;
      int length = unityVersionFull.LastIndexOf('_');
      if (length != -1)
      {
        str1 = unityVersionFull.Substring(length + 1);
        str2 = unityVersionFull.Substring(0, length);
      }
      Dictionary<string, string> dictionary = new Dictionary<string, string>()
      {
        {
          "SamsungTV",
          "Samsung-TV"
        },
        {
          "tvOS",
          "AppleTV"
        },
        {
          "OSXStandalone",
          "Mac"
        },
        {
          "WindowsStandalone",
          "Windows"
        },
        {
          "LinuxStandalone",
          "Linux"
        }
      };
      if (dictionary.ContainsKey(moduleName))
        moduleName = dictionary[moduleName];
      string str3 = "Unknown";
      string str4;
      string str5;
      if (str2.IndexOf('a') != -1 || str2.IndexOf('b') != -1)
      {
        str4 = "beta";
        str5 = "download";
      }
      else
      {
        str4 = "download";
        str5 = "download_unity";
      }
      if (Application.platform == RuntimePlatform.WindowsEditor)
        str3 = "TargetSupportInstaller";
      else if (Application.platform == RuntimePlatform.OSXEditor)
        str3 = "MacEditorTargetInstaller";
      string str6 = "http://" + str4 + ".unity3d.com/" + str5 + "/" + str1 + "/" + str3 + "/UnitySetup-" + moduleName + "-Support-for-Editor-" + str2;
      if (Application.platform == RuntimePlatform.WindowsEditor)
        str6 += ".exe";
      else if (Application.platform == RuntimePlatform.OSXEditor)
        str6 += ".pkg";
      return str6;
    }

    private void ShowBuildTargetSettings()
    {
      EditorGUIUtility.labelWidth = Mathf.Min(180f, (float) (((double) this.position.width - 265.0) * 0.469999998807907));
      BuildTarget selectedBuildTarget = BuildPlayerWindow.CalculateSelectedBuildTarget();
      BuildPlayerWindow.BuildPlatform platform = BuildPlayerWindow.s_BuildPlatforms.BuildPlatformFromTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
      bool flag1 = BuildPipeline.LicenseCheck(selectedBuildTarget);
      GUILayout.Space(18f);
      Rect rect = GUILayoutUtility.GetRect(50f, 36f);
      ++rect.x;
      GUI.Label(new Rect(rect.x + 3f, rect.y + 3f, 32f, 32f), platform.title.image, GUIStyle.none);
      GUI.Toggle(rect, false, platform.title.text, BuildPlayerWindow.styles.platformSelector);
      GUILayout.Space(10f);
      if (platform.targetGroup == BuildTargetGroup.WebGL && !BuildPipeline.IsBuildTargetSupported(selectedBuildTarget) && IntPtr.Size == 4)
      {
        GUILayout.Label("Building for WebGL requires a 64-bit Unity editor.");
        BuildPlayerWindow.GUIBuildButtons(false, false, false, platform);
      }
      else
      {
        string stringFromBuildTarget = ModuleManager.GetTargetStringFromBuildTarget(selectedBuildTarget);
        if (flag1 && !string.IsNullOrEmpty(stringFromBuildTarget) && ModuleManager.GetBuildPostProcessor(selectedBuildTarget) == null && (EditorUserBuildSettings.selectedBuildTargetGroup != BuildTargetGroup.Standalone || !BuildPlayerWindow.IsAnyStandaloneModuleLoaded()))
        {
          GUILayout.Label("No " + BuildPlayerWindow.s_BuildPlatforms.GetBuildTargetDisplayName(selectedBuildTarget) + " module loaded.");
          if (GUILayout.Button("Open Download Page", EditorStyles.miniButton, new GUILayoutOption[1]
          {
            GUILayout.ExpandWidth(false)
          }))
            Help.BrowseURL(BuildPlayerWindow.GetPlaybackEngineDownloadURL(stringFromBuildTarget));
          BuildPlayerWindow.GUIBuildButtons(false, false, false, platform);
        }
        else
        {
          if (Application.HasProLicense() && !InternalEditorUtility.HasAdvancedLicenseOnBuildTarget(selectedBuildTarget))
          {
            string text = string.Format("{0} is not included in your Unity Pro license. Your {0} build will include a Unity Personal Edition splash screen.\n\nYou must be eligible to use Unity Personal Edition to use this build option. Please refer to our EULA for further information.", (object) BuildPlayerWindow.s_BuildPlatforms.GetBuildTargetDisplayName(selectedBuildTarget));
            GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
            GUILayout.Label(text, EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("EULA", EditorStyles.miniButton, new GUILayoutOption[0]))
              Application.OpenURL("http://unity3d.com/legal/eula");
            if (GUILayout.Button(string.Format("Add {0} to your Unity Pro license", (object) BuildPlayerWindow.s_BuildPlatforms.GetBuildTargetDisplayName(selectedBuildTarget)), EditorStyles.miniButton, new GUILayoutOption[0]))
              Application.OpenURL("http://unity3d.com/get-unity");
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
          }
          GUIContent downloadErrorForTarget = BuildPlayerWindow.styles.GetDownloadErrorForTarget(selectedBuildTarget);
          if (downloadErrorForTarget != null)
          {
            GUILayout.Label(downloadErrorForTarget, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
            BuildPlayerWindow.GUIBuildButtons(false, false, false, platform);
          }
          else if (!flag1)
          {
            int index = BuildPlayerWindow.s_BuildPlatforms.BuildPlatformIndexFromTargetGroup(platform.targetGroup);
            GUILayout.Label(BuildPlayerWindow.styles.notLicensedMessages[index, 0], EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
            GUILayout.Space(5f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (BuildPlayerWindow.styles.notLicensedMessages[index, 1].text.Length != 0 && GUILayout.Button(BuildPlayerWindow.styles.notLicensedMessages[index, 1]))
              Application.OpenURL(BuildPlayerWindow.styles.notLicensedMessages[index, 2].text);
            GUILayout.EndHorizontal();
            BuildPlayerWindow.GUIBuildButtons(false, false, false, platform);
          }
          else
          {
            IBuildWindowExtension buildWindowExtension = ModuleManager.GetBuildWindowExtension(ModuleManager.GetTargetStringFromBuildTargetGroup(platform.targetGroup));
            if (buildWindowExtension != null)
              buildWindowExtension.ShowPlatformBuildOptions();
            GUI.changed = false;
            BuildTargetGroup targetGroup = platform.targetGroup;
            switch (targetGroup)
            {
              case BuildTargetGroup.Standalone:
                BuildTarget standaloneTarget = BuildPlayerWindow.GetBestStandaloneTarget(EditorUserBuildSettings.selectedStandaloneTarget);
                BuildTarget buildTarget = EditorUserBuildSettings.selectedStandaloneTarget;
                int selectedIndex1 = Math.Max(0, Array.IndexOf<BuildTarget>(BuildPlayerWindow.s_BuildPlatforms.standaloneSubtargets, BuildPlayerWindow.BuildPlatforms.DefaultTargetForPlatform(standaloneTarget)));
                int index1 = EditorGUILayout.Popup(BuildPlayerWindow.styles.standaloneTarget, selectedIndex1, BuildPlayerWindow.s_BuildPlatforms.standaloneSubtargetStrings, new GUILayoutOption[0]);
                if (index1 == selectedIndex1)
                {
                  Dictionary<GUIContent, BuildTarget> architecturesForPlatform = BuildPlayerWindow.BuildPlatforms.GetArchitecturesForPlatform(standaloneTarget);
                  if (architecturesForPlatform != null)
                  {
                    GUIContent[] array = new List<GUIContent>((IEnumerable<GUIContent>) architecturesForPlatform.Keys).ToArray();
                    int selectedIndex2 = 0;
                    if (index1 == selectedIndex1)
                    {
                      using (Dictionary<GUIContent, BuildTarget>.Enumerator enumerator = architecturesForPlatform.GetEnumerator())
                      {
                        while (enumerator.MoveNext())
                        {
                          KeyValuePair<GUIContent, BuildTarget> current = enumerator.Current;
                          if (current.Value == standaloneTarget)
                          {
                            selectedIndex2 = Math.Max(0, Array.IndexOf<GUIContent>(array, current.Key));
                            break;
                          }
                        }
                      }
                    }
                    int index2 = EditorGUILayout.Popup(BuildPlayerWindow.styles.architecture, selectedIndex2, array, new GUILayoutOption[0]);
                    buildTarget = architecturesForPlatform[array[index2]];
                  }
                }
                else
                  buildTarget = BuildPlayerWindow.s_BuildPlatforms.standaloneSubtargets[index1];
                if (buildTarget != EditorUserBuildSettings.selectedStandaloneTarget)
                {
                  EditorUserBuildSettings.selectedStandaloneTarget = buildTarget;
                  GUIUtility.ExitGUI();
                  break;
                }
                break;
              case BuildTargetGroup.WebPlayer:
                GUI.enabled = BuildPipeline.LicenseCheck(BuildTarget.WebPlayerStreamed);
                bool flag2 = EditorGUILayout.Toggle(BuildPlayerWindow.styles.webPlayerStreamed, EditorUserBuildSettings.webPlayerStreamed, new GUILayoutOption[0]);
                if (GUI.changed)
                  EditorUserBuildSettings.webPlayerStreamed = flag2;
                EditorUserBuildSettings.webPlayerOfflineDeployment = EditorGUILayout.Toggle(BuildPlayerWindow.styles.webPlayerOfflineDeployment, EditorUserBuildSettings.webPlayerOfflineDeployment, new GUILayoutOption[0]);
                break;
              case BuildTargetGroup.iPhone:
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                  EditorUserBuildSettings.symlinkLibraries = EditorGUILayout.Toggle(BuildPlayerWindow.styles.symlinkiOSLibraries, EditorUserBuildSettings.symlinkLibraries, new GUILayoutOption[0]);
                  break;
                }
                break;
              default:
                if (targetGroup == BuildTargetGroup.tvOS)
                  goto case BuildTargetGroup.iPhone;
                else
                  break;
            }
            GUI.enabled = true;
            bool enableBuildButton = buildWindowExtension == null || buildWindowExtension.EnabledBuildButton();
            bool enableBuildAndRunButton = false;
            bool flag3 = buildWindowExtension == null || buildWindowExtension.ShouldDrawScriptDebuggingCheckbox();
            bool flag4 = buildWindowExtension != null && buildWindowExtension.ShouldDrawExplicitNullCheckbox();
            bool flag5 = buildWindowExtension == null || buildWindowExtension.ShouldDrawDevelopmentPlayerCheckbox();
            bool flag6 = selectedBuildTarget == BuildTarget.StandaloneLinux || selectedBuildTarget == BuildTarget.StandaloneLinux64 || selectedBuildTarget == BuildTarget.StandaloneLinuxUniversal;
            IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(selectedBuildTarget);
            bool flag7 = buildPostProcessor != null && buildPostProcessor.SupportsScriptsOnlyBuild();
            bool canInstallInBuildFolder = false;
            if (BuildPipeline.IsBuildTargetSupported(selectedBuildTarget))
            {
              bool flag8 = buildWindowExtension == null || buildWindowExtension.ShouldDrawProfilerCheckbox();
              GUI.enabled = flag5;
              if (flag5)
                EditorUserBuildSettings.development = EditorGUILayout.Toggle(BuildPlayerWindow.styles.debugBuild, EditorUserBuildSettings.development, new GUILayoutOption[0]);
              bool development = EditorUserBuildSettings.development;
              GUI.enabled = development;
              GUI.enabled = GUI.enabled && platform.targetGroup != BuildTargetGroup.WebPlayer;
              if (flag8)
              {
                if (!GUI.enabled)
                {
                  if (!development)
                    BuildPlayerWindow.styles.profileBuild.tooltip = "Profiling only enabled in Development Player";
                  else if (platform.targetGroup == BuildTargetGroup.WebPlayer)
                    BuildPlayerWindow.styles.profileBuild.tooltip = "Autoconnect not available from webplayer. Manually connect in Profiler";
                }
                else
                  BuildPlayerWindow.styles.profileBuild.tooltip = string.Empty;
                EditorUserBuildSettings.connectProfiler = EditorGUILayout.Toggle(BuildPlayerWindow.styles.profileBuild, EditorUserBuildSettings.connectProfiler, new GUILayoutOption[0]);
              }
              GUI.enabled = development;
              if (flag3)
                EditorUserBuildSettings.allowDebugging = EditorGUILayout.Toggle(BuildPlayerWindow.styles.allowDebugging, EditorUserBuildSettings.allowDebugging, new GUILayoutOption[0]);
              bool flag9 = false;
              int num = 0;
              if (PlayerSettings.GetPropertyOptionalInt("ScriptingBackend", ref num, platform.targetGroup))
                flag9 = num == 1;
              if (buildWindowExtension != null && development && flag9 && buildWindowExtension.ShouldDrawForceOptimizeScriptsCheckbox())
                EditorUserBuildSettings.forceOptimizeScriptCompilation = EditorGUILayout.Toggle(BuildPlayerWindow.styles.forceOptimizeScriptCompilation, EditorUserBuildSettings.forceOptimizeScriptCompilation, new GUILayoutOption[0]);
              if (flag4)
              {
                GUI.enabled = !development;
                if (!GUI.enabled)
                  EditorUserBuildSettings.explicitNullChecks = true;
                EditorUserBuildSettings.explicitNullChecks = EditorGUILayout.Toggle(BuildPlayerWindow.styles.explicitNullChecks, EditorUserBuildSettings.explicitNullChecks, new GUILayoutOption[0]);
                GUI.enabled = development;
              }
              if (flag7)
                EditorUserBuildSettings.buildScriptsOnly = EditorGUILayout.Toggle(BuildPlayerWindow.styles.buildScriptsOnly, EditorUserBuildSettings.buildScriptsOnly, new GUILayoutOption[0]);
              GUI.enabled = !development;
              if (flag6)
                EditorUserBuildSettings.enableHeadlessMode = EditorGUILayout.Toggle(BuildPlayerWindow.styles.enableHeadlessMode, EditorUserBuildSettings.enableHeadlessMode && !development, new GUILayoutOption[0]);
              GUI.enabled = true;
              GUILayout.FlexibleSpace();
              canInstallInBuildFolder = Unsupported.IsDeveloperBuild() && PostprocessBuildPlayer.SupportsInstallInBuildFolder(selectedBuildTarget);
              if (enableBuildButton)
                enableBuildAndRunButton = buildWindowExtension == null ? !EditorUserBuildSettings.installInBuildFolder : buildWindowExtension.EnabledBuildAndRunButton() && !EditorUserBuildSettings.installInBuildFolder;
              if (platform.targetGroup == BuildTargetGroup.WebPlayer)
              {
                string message = string.Format("Please note that the Unity Web Player is deprecated. Building for Web Player will no longer be supported in future versions of Unity.");
                GUILayout.BeginVertical();
                EditorGUILayout.HelpBox(message, MessageType.Warning);
                GUILayout.EndVertical();
              }
            }
            else
            {
              GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
              GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
              int index2 = BuildPlayerWindow.s_BuildPlatforms.BuildPlatformIndexFromTargetGroup(platform.targetGroup);
              GUILayout.Label(BuildPlayerWindow.styles.GetTargetNotInstalled(index2, 0));
              if (BuildPlayerWindow.styles.GetTargetNotInstalled(index2, 1) != null && GUILayout.Button(BuildPlayerWindow.styles.GetTargetNotInstalled(index2, 1)))
                Application.OpenURL(BuildPlayerWindow.styles.GetTargetNotInstalled(index2, 2).text);
              GUILayout.EndVertical();
              GUILayout.FlexibleSpace();
              GUILayout.EndHorizontal();
            }
            BuildPlayerWindow.GUIBuildButtons(buildWindowExtension, enableBuildButton, enableBuildAndRunButton, canInstallInBuildFolder, platform);
          }
        }
      }
    }

    private static void GUIBuildButtons(bool enableBuildButton, bool enableBuildAndRunButton, bool canInstallInBuildFolder, BuildPlayerWindow.BuildPlatform platform)
    {
      BuildPlayerWindow.GUIBuildButtons((IBuildWindowExtension) null, enableBuildButton, enableBuildAndRunButton, canInstallInBuildFolder, platform);
    }

    private static void GUIBuildButtons(IBuildWindowExtension buildWindowExtension, bool enableBuildButton, bool enableBuildAndRunButton, bool canInstallInBuildFolder, BuildPlayerWindow.BuildPlatform platform)
    {
      GUILayout.FlexibleSpace();
      if (canInstallInBuildFolder)
        EditorUserBuildSettings.installInBuildFolder = GUILayout.Toggle((EditorUserBuildSettings.installInBuildFolder ? 1 : 0) != 0, "Install in Builds folder\n(for debugging with source code)", new GUILayoutOption[1]
        {
          GUILayout.ExpandWidth(false)
        });
      else
        EditorUserBuildSettings.installInBuildFolder = false;
      if (buildWindowExtension != null && Unsupported.IsDeveloperBuild())
        buildWindowExtension.ShowInternalPlatformBuildOptions();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (EditorGUILayout.LinkLabel(BuildPlayerWindow.styles.learnAboutUnityCloudBuild))
        Application.OpenURL(string.Format("{0}/from/editor/buildsettings?upid={1}&pid={2}&currentplatform={3}&selectedplatform={4}&unityversion={5}", (object) WebURLs.cloudBuildPage, (object) PlayerSettings.cloudProjectId, (object) PlayerSettings.productGUID, (object) EditorUserBuildSettings.activeBuildTarget, (object) BuildPlayerWindow.CalculateSelectedBuildTarget(), (object) Application.unityVersion));
      GUILayout.EndHorizontal();
      GUILayout.Space(6f);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUIContent content = BuildPlayerWindow.styles.build;
      if (platform.targetGroup == BuildTargetGroup.Android && EditorUserBuildSettings.exportAsGoogleAndroidProject)
        content = BuildPlayerWindow.styles.export;
      if (platform.targetGroup == BuildTargetGroup.iPhone && Application.platform != RuntimePlatform.OSXEditor)
        enableBuildAndRunButton = false;
      GUI.enabled = enableBuildButton;
      if (GUILayout.Button(content, new GUILayoutOption[1]
      {
        GUILayout.Width(110f)
      }))
      {
        BuildPlayerWindow.BuildPlayerWithDefaultSettings(true, BuildOptions.ShowBuiltPlayer);
        GUIUtility.ExitGUI();
      }
      GUI.enabled = enableBuildAndRunButton;
      if (GUILayout.Button(BuildPlayerWindow.styles.buildAndRun, new GUILayoutOption[1]
      {
        GUILayout.Width(110f)
      }))
      {
        BuildPlayerWindow.BuildPlayerWithDefaultSettings(true, BuildOptions.AutoRunPlayer);
        GUIUtility.ExitGUI();
      }
      GUILayout.EndHorizontal();
    }

    private static bool PickBuildLocation(BuildTarget target, BuildOptions options, out bool updateExistingBuild)
    {
      updateExistingBuild = false;
      string buildLocation = EditorUserBuildSettings.GetBuildLocation(target);
      if (target == BuildTarget.Android && EditorUserBuildSettings.exportAsGoogleAndroidProject)
      {
        string location = EditorUtility.SaveFolderPanel("Export Google Android Project", buildLocation, string.Empty);
        EditorUserBuildSettings.SetBuildLocation(target, location);
        return true;
      }
      string extensionForBuildTarget = PostprocessBuildPlayer.GetExtensionForBuildTarget(target, options);
      string directory = FileUtil.DeleteLastPathNameComponent(buildLocation);
      string pathNameComponent = FileUtil.GetLastPathNameComponent(buildLocation);
      string title = "Build " + BuildPlayerWindow.s_BuildPlatforms.GetBuildTargetDisplayName(target);
      string str = EditorUtility.SaveBuildPanel(target, title, directory, pathNameComponent, extensionForBuildTarget, out updateExistingBuild);
      if (str == string.Empty)
        return false;
      if (extensionForBuildTarget != string.Empty && FileUtil.GetPathExtension(str).ToLower() != extensionForBuildTarget)
        str = str + (object) '.' + extensionForBuildTarget;
      if (FileUtil.GetLastPathNameComponent(str) == string.Empty)
        return false;
      string path = !(extensionForBuildTarget != string.Empty) ? str : FileUtil.DeleteLastPathNameComponent(str);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      if (target == BuildTarget.iOS && Application.platform != RuntimePlatform.OSXEditor && (!BuildPlayerWindow.FolderIsEmpty(str) && !BuildPlayerWindow.UserWantsToDeleteFiles(str)))
        return false;
      EditorUserBuildSettings.SetBuildLocation(target, str);
      return true;
    }

    private static bool FolderIsEmpty(string path)
    {
      if (!Directory.Exists(path))
        return true;
      if (Directory.GetDirectories(path).Length == 0)
        return Directory.GetFiles(path).Length == 0;
      return false;
    }

    private static bool UserWantsToDeleteFiles(string path)
    {
      return EditorUtility.DisplayDialog("Deleting existing files", "WARNING: all files and folders located in target folder: '" + path + "' will be deleted by build process.", "OK", "Cancel");
    }

    public class SceneSorter : IComparer
    {
      int IComparer.Compare(object x, object y)
      {
        return new CaseInsensitiveComparer().Compare(y, x);
      }
    }

    public class BuildPlatform
    {
      public string name;
      public GUIContent title;
      public Texture2D smallIcon;
      public BuildTargetGroup targetGroup;
      public bool forceShowTarget;
      public string tooltip;

      public BuildTarget DefaultTarget
      {
        get
        {
          switch (this.targetGroup)
          {
            case BuildTargetGroup.Standalone:
              return BuildTarget.StandaloneWindows;
            case BuildTargetGroup.WebPlayer:
              return BuildTarget.WebPlayer;
            case BuildTargetGroup.iPhone:
              return BuildTarget.iOS;
            case BuildTargetGroup.PS3:
              return BuildTarget.PS3;
            case BuildTargetGroup.XBOX360:
              return BuildTarget.XBOX360;
            case BuildTargetGroup.Android:
              return BuildTarget.Android;
            case BuildTargetGroup.GLESEmu:
              return BuildTarget.StandaloneGLESEmu;
            case BuildTargetGroup.WebGL:
              return BuildTarget.WebGL;
            case BuildTargetGroup.Metro:
              return BuildTarget.WSAPlayer;
            case BuildTargetGroup.WP8:
              return BuildTarget.WP8Player;
            case BuildTargetGroup.BlackBerry:
              return BuildTarget.BlackBerry;
            case BuildTargetGroup.Tizen:
              return BuildTarget.Tizen;
            case BuildTargetGroup.PSP2:
              return BuildTarget.PSP2;
            case BuildTargetGroup.PS4:
              return BuildTarget.PS4;
            case BuildTargetGroup.XboxOne:
              return BuildTarget.XboxOne;
            case BuildTargetGroup.SamsungTV:
              return BuildTarget.SamsungTV;
            case BuildTargetGroup.Nintendo3DS:
              return BuildTarget.Nintendo3DS;
            case BuildTargetGroup.WiiU:
              return BuildTarget.WiiU;
            case BuildTargetGroup.tvOS:
              return BuildTarget.tvOS;
            default:
              return BuildTarget.iPhone;
          }
        }
      }

      public BuildPlatform(string locTitle, string iconId, BuildTargetGroup targetGroup, bool forceShowTarget)
        : this(locTitle, string.Empty, iconId, targetGroup, forceShowTarget)
      {
      }

      public BuildPlatform(string locTitle, string tooltip, string iconId, BuildTargetGroup targetGroup, bool forceShowTarget)
      {
        this.targetGroup = targetGroup;
        this.name = targetGroup == BuildTargetGroup.Unknown ? string.Empty : BuildPipeline.GetBuildTargetGroupName(this.DefaultTarget);
        this.title = EditorGUIUtility.TextContentWithIcon(locTitle, iconId);
        this.smallIcon = EditorGUIUtility.IconContent(iconId + ".Small").image as Texture2D;
        this.tooltip = tooltip;
        this.forceShowTarget = forceShowTarget;
      }
    }

    private class BuildPlatforms
    {
      public BuildPlayerWindow.BuildPlatform[] buildPlatforms;
      public BuildTarget[] standaloneSubtargets;
      public GUIContent[] standaloneSubtargetStrings;

      internal BuildPlatforms()
      {
        List<BuildPlayerWindow.BuildPlatform> buildPlatformList = new List<BuildPlayerWindow.BuildPlatform>();
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("Web Player", "BuildSettings.Web", BuildTargetGroup.WebPlayer, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("PC, Mac & Linux Standalone", "BuildSettings.Standalone", BuildTargetGroup.Standalone, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("iOS", "BuildSettings.iPhone", BuildTargetGroup.iPhone, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("tvOS", "BuildSettings.tvOS", BuildTargetGroup.tvOS, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("Android", "BuildSettings.Android", BuildTargetGroup.Android, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("Tizen", "BuildSettings.Tizen", BuildTargetGroup.Tizen, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("Xbox 360", "BuildSettings.XBox360", BuildTargetGroup.XBOX360, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("Xbox One", "BuildSettings.XboxOne", BuildTargetGroup.XboxOne, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("PS3", "BuildSettings.PS3", BuildTargetGroup.PS3, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("PS Vita", "BuildSettings.PSP2", BuildTargetGroup.PSP2, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("PS4", "BuildSettings.PS4", BuildTargetGroup.PS4, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("GLES Emulator", "BuildSettings.StandaloneGLESEmu", BuildTargetGroup.GLESEmu, false));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("Wii U", "BuildSettings.WiiU", BuildTargetGroup.WiiU, false));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("Windows Store", "BuildSettings.Metro", BuildTargetGroup.Metro, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("WebGL", "BuildSettings.WebGL", BuildTargetGroup.WebGL, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("Samsung TV", "BuildSettings.SamsungTV", BuildTargetGroup.SamsungTV, true));
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("Nintendo 3DS", "BuildSettings.N3DS", BuildTargetGroup.Nintendo3DS, false));
        using (List<BuildPlayerWindow.BuildPlatform>.Enumerator enumerator = buildPlatformList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            BuildPlayerWindow.BuildPlatform current = enumerator.Current;
            current.tooltip = BuildPipeline.GetBuildTargetGroupDisplayName(current.targetGroup) + " settings";
          }
        }
        this.buildPlatforms = buildPlatformList.ToArray();
        this.SetupStandaloneSubtargets();
      }

      private void SetupStandaloneSubtargets()
      {
        List<BuildTarget> buildTargetList = new List<BuildTarget>();
        List<GUIContent> guiContentList = new List<GUIContent>();
        if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneWindows)))
        {
          buildTargetList.Add(BuildTarget.StandaloneWindows);
          guiContentList.Add(EditorGUIUtility.TextContent("Windows"));
        }
        if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneOSXIntel)))
        {
          buildTargetList.Add(BuildTarget.StandaloneOSXIntel);
          guiContentList.Add(EditorGUIUtility.TextContent("Mac OS X"));
        }
        if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneLinux)))
        {
          buildTargetList.Add(BuildTarget.StandaloneLinux);
          guiContentList.Add(EditorGUIUtility.TextContent("Linux"));
        }
        this.standaloneSubtargets = buildTargetList.ToArray();
        this.standaloneSubtargetStrings = guiContentList.ToArray();
      }

      public string GetBuildTargetDisplayName(BuildTarget target)
      {
        foreach (BuildPlayerWindow.BuildPlatform buildPlatform in this.buildPlatforms)
        {
          if (buildPlatform.DefaultTarget == target)
            return buildPlatform.title.text;
        }
        if (target == BuildTarget.WebPlayerStreamed)
          return this.BuildPlatformFromTargetGroup(BuildTargetGroup.WebPlayer).title.text;
        for (int index = 0; index < this.standaloneSubtargets.Length; ++index)
        {
          if (this.standaloneSubtargets[index] == BuildPlayerWindow.BuildPlatforms.DefaultTargetForPlatform(target))
            return this.standaloneSubtargetStrings[index].text;
        }
        return "Unsupported Target";
      }

      public static Dictionary<GUIContent, BuildTarget> GetArchitecturesForPlatform(BuildTarget target)
      {
        BuildTarget buildTarget = target;
        switch (buildTarget)
        {
          case BuildTarget.StandaloneOSXUniversal:
          case BuildTarget.StandaloneOSXIntel:
label_5:
            return new Dictionary<GUIContent, BuildTarget>()
            {
              {
                EditorGUIUtility.TextContent("x86"),
                BuildTarget.StandaloneOSXIntel
              },
              {
                EditorGUIUtility.TextContent("x86_64"),
                BuildTarget.StandaloneOSXIntel64
              },
              {
                EditorGUIUtility.TextContent("Universal"),
                BuildTarget.StandaloneOSXUniversal
              }
            };
          case BuildTarget.StandaloneWindows:
label_3:
            return new Dictionary<GUIContent, BuildTarget>()
            {
              {
                EditorGUIUtility.TextContent("x86"),
                BuildTarget.StandaloneWindows
              },
              {
                EditorGUIUtility.TextContent("x86_64"),
                BuildTarget.StandaloneWindows64
              }
            };
          default:
            switch (buildTarget - 24)
            {
              case ~BuildTarget.iPhone:
              case (BuildTarget) 1:
label_4:
                return new Dictionary<GUIContent, BuildTarget>()
                {
                  {
                    EditorGUIUtility.TextContent("x86"),
                    BuildTarget.StandaloneLinux
                  },
                  {
                    EditorGUIUtility.TextContent("x86_64"),
                    BuildTarget.StandaloneLinux64
                  },
                  {
                    EditorGUIUtility.TextContent("x86 + x86_64 (Universal)"),
                    BuildTarget.StandaloneLinuxUniversal
                  }
                };
              case (BuildTarget) 3:
                goto label_5;
              default:
                switch (buildTarget - 17)
                {
                  case ~BuildTarget.iPhone:
                    goto label_4;
                  case BuildTarget.StandaloneOSXUniversal:
                    goto label_3;
                  default:
                    return (Dictionary<GUIContent, BuildTarget>) null;
                }
            }
        }
      }

      public static BuildTarget DefaultTargetForPlatform(BuildTarget target)
      {
        BuildTarget buildTarget = target;
        switch (buildTarget)
        {
          case BuildTarget.StandaloneLinux:
          case BuildTarget.StandaloneLinux64:
          case BuildTarget.StandaloneLinuxUniversal:
            return BuildTarget.StandaloneLinux;
          case BuildTarget.StandaloneWindows64:
label_2:
            return BuildTarget.StandaloneWindows;
          case BuildTarget.WSAPlayer:
            return BuildTarget.WSAPlayer;
          case BuildTarget.WP8Player:
            return BuildTarget.WP8Player;
          case BuildTarget.StandaloneOSXIntel64:
label_4:
            return BuildTarget.StandaloneOSXIntel;
          default:
            switch (buildTarget - 2)
            {
              case ~BuildTarget.iPhone:
              case BuildTarget.StandaloneOSXUniversal:
                goto label_4;
              case (BuildTarget) 3:
                goto label_2;
              default:
                return target;
            }
        }
      }

      public int BuildPlatformIndexFromTargetGroup(BuildTargetGroup group)
      {
        for (int index = 0; index < this.buildPlatforms.Length; ++index)
        {
          if (group == this.buildPlatforms[index].targetGroup)
            return index;
        }
        return -1;
      }

      public BuildPlayerWindow.BuildPlatform BuildPlatformFromTargetGroup(BuildTargetGroup group)
      {
        int index = this.BuildPlatformIndexFromTargetGroup(group);
        if (index != -1)
          return this.buildPlatforms[index];
        return (BuildPlayerWindow.BuildPlatform) null;
      }
    }

    private class Styles
    {
      public GUIStyle selected = (GUIStyle) "ServerUpdateChangesetOn";
      public GUIStyle box = (GUIStyle) "OL Box";
      public GUIStyle title = (GUIStyle) "OL title";
      public GUIStyle evenRow = (GUIStyle) "CN EntryBackEven";
      public GUIStyle oddRow = (GUIStyle) "CN EntryBackOdd";
      public GUIStyle platformSelector = (GUIStyle) "PlayerSettingsPlatform";
      public GUIStyle toggle = (GUIStyle) "Toggle";
      public GUIStyle levelString = (GUIStyle) "PlayerSettingsLevel";
      public GUIStyle levelStringCounter = new GUIStyle((GUIStyle) "Label");
      public GUIContent noSessionDialogText = EditorGUIUtility.TextContent("In order to publish your build to UDN, you need to sign in via the AssetStore and tick the 'Stay signed in' checkbox.");
      public GUIContent platformTitle = EditorGUIUtility.TextContent("Platform|Which platform to build for");
      public GUIContent switchPlatform = EditorGUIUtility.TextContent("Switch Platform");
      public GUIContent build = EditorGUIUtility.TextContent("Build");
      public GUIContent export = EditorGUIUtility.TextContent("Export");
      public GUIContent buildAndRun = EditorGUIUtility.TextContent("Build And Run");
      public GUIContent scenesInBuild = EditorGUIUtility.TextContent("Scenes In Build|Which scenes to include in the build");
      public Texture2D activePlatformIcon = EditorGUIUtility.IconContent("BuildSettings.SelectedIcon").image as Texture2D;
      public GUIContent[,] notLicensedMessages = new GUIContent[18, 3]
      {
        {
          EditorGUIUtility.TextContent("Unity Web Player building is disabled during the public preview beta. It will be enabled when Unity ships."),
          new GUIContent(string.Empty),
          new GUIContent("https://store.unity3d.com/shop/")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover Standalone Publishing."),
          new GUIContent(string.Empty),
          new GUIContent("https://store.unity3d.com/shop/")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover iOS Publishing."),
          EditorGUIUtility.TextContent("Go to Our Online Store"),
          new GUIContent("https://store.unity3d.com/shop/")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover Apple TV Publishing."),
          EditorGUIUtility.TextContent("Go to Our Online Store"),
          new GUIContent("https://store.unity3d.com/shop/")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover Android Publishing."),
          EditorGUIUtility.TextContent("Go to Our Online Store"),
          new GUIContent("https://store.unity3d.com/shop/")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover Tizen Publishing."),
          EditorGUIUtility.TextContent("Go to Our Online Store"),
          new GUIContent("https://store.unity3d.com/shop/")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover Xbox 360 Publishing."),
          EditorGUIUtility.TextContent("Contact sales"),
          new GUIContent("http://unity3d.com/company/sales?type=sales")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover Xbox One Publishing."),
          EditorGUIUtility.TextContent("Contact sales"),
          new GUIContent("http://unity3d.com/company/sales?type=sales")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover PS3 Publishing."),
          EditorGUIUtility.TextContent("Contact sales"),
          new GUIContent("http://unity3d.com/company/sales?type=sales")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover PS Vita Publishing."),
          EditorGUIUtility.TextContent("Contact sales"),
          new GUIContent("http://unity3d.com/company/sales?type=sales")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover PS4 Publishing."),
          EditorGUIUtility.TextContent("Contact sales"),
          new GUIContent("http://unity3d.com/company/sales?type=sales")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover GLESEmu Publishing"),
          EditorGUIUtility.TextContent("Contact sales"),
          new GUIContent("http://unity3d.com/company/sales?type=sales")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover Wii U Publishing."),
          EditorGUIUtility.TextContent("Contact sales"),
          new GUIContent("http://unity3d.com/company/sales?type=sales")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover Flash Publishing"),
          EditorGUIUtility.TextContent("Go to Our Online Store"),
          new GUIContent("https://store.unity3d.com/shop/")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover Windows Store Publishing."),
          EditorGUIUtility.TextContent("Go to Our Online Store"),
          new GUIContent("https://store.unity3d.com/shop/")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover Windows Phone 8 Publishing."),
          EditorGUIUtility.TextContent("Go to Our Online Store"),
          new GUIContent("https://store.unity3d.com/shop/")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover SamsungTV Publishing"),
          EditorGUIUtility.TextContent("Go to Our Online Store"),
          new GUIContent("https://store.unity3d.com/shop/")
        },
        {
          EditorGUIUtility.TextContent("Your license does not cover Nintendo 3DS Publishing"),
          EditorGUIUtility.TextContent("Contact sales"),
          new GUIContent("http://unity3d.com/company/sales?type=sales")
        }
      };
      private GUIContent[,] buildTargetNotInstalled = new GUIContent[18, 3]
      {
        {
          EditorGUIUtility.TextContent("Web Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("Standalone Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("iOS Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("Apple TV Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("Android Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("Tizen is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("Xbox 360 Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("Xbox One Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("PS3 Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("PS Vita Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("PS4 Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("GLESEmu Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("Wii U Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("Flash Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("Windows Store Player is not supported in\nthis build.\n\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("Windows Phone 8 Player is not supported\nin this build.\n\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("SamsungTV Player is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        },
        {
          EditorGUIUtility.TextContent("Ninteno 3DS is not supported in this build.\nDownload a build that supports it."),
          null,
          new GUIContent("http://unity3d.com/unity/download/")
        }
      };
      public GUIContent standaloneTarget = EditorGUIUtility.TextContent("Target Platform|Destination platform for standalone build");
      public GUIContent architecture = EditorGUIUtility.TextContent("Architecture|Build architecture for standalone");
      public GUIContent webPlayerStreamed = EditorGUIUtility.TextContent("Streamed|Is the web player streamed in?");
      public GUIContent webPlayerOfflineDeployment = EditorGUIUtility.TextContent("Offline Deployment|Web Player will not reference online resources");
      public GUIContent debugBuild = EditorGUIUtility.TextContent("Development Build");
      public GUIContent profileBuild = EditorGUIUtility.TextContent("Autoconnect Profiler");
      public GUIContent allowDebugging = EditorGUIUtility.TextContent("Script Debugging");
      public GUIContent symlinkiOSLibraries = EditorGUIUtility.TextContent("Symlink Unity libraries");
      public GUIContent explicitNullChecks = EditorGUIUtility.TextContent("Explicit Null Checks");
      public GUIContent enableHeadlessMode = EditorGUIUtility.TextContent("Headless Mode");
      public GUIContent buildScriptsOnly = EditorGUIUtility.TextContent("Scripts Only Build");
      public GUIContent forceOptimizeScriptCompilation = EditorGUIUtility.TextContent("Build Optimized Scripts|Compile IL2CPP using full compiler optimizations. Note this will obfuscate callstack output.");
      public GUIContent learnAboutUnityCloudBuild = EditorGUIUtility.TextContent("Learn about Unity Cloud Build");
      public const float kButtonWidth = 110f;
      private const string kShopURL = "https://store.unity3d.com/shop/";
      private const string kDownloadURL = "http://unity3d.com/unity/download/";
      private const string kMailURL = "http://unity3d.com/company/sales?type=sales";
      public Vector2 toggleSize;

      public Styles()
      {
        this.levelStringCounter.alignment = TextAnchor.MiddleRight;
      }

      public GUIContent GetTargetNotInstalled(int index, int item)
      {
        if (index >= this.buildTargetNotInstalled.GetLength(0))
          index = 0;
        return this.buildTargetNotInstalled[index, item];
      }

      public GUIContent GetDownloadErrorForTarget(BuildTarget target)
      {
        return (GUIContent) null;
      }
    }

    private class ScenePostprocessor : AssetPostprocessor
    {
      private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
      {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        for (int index = 0; index < movedAssets.Length; ++index)
        {
          string movedAsset = movedAssets[index];
          if (Path.GetExtension(movedAsset) == ".unity")
          {
            foreach (EditorBuildSettingsScene buildSettingsScene in scenes)
            {
              if (buildSettingsScene.path.ToLower() == movedFromPath[index].ToLower())
                buildSettingsScene.path = movedAsset;
            }
          }
        }
        EditorBuildSettings.scenes = scenes;
      }
    }
  }
}

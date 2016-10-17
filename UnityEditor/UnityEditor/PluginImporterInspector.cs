// Decompiled with JetBrains decompiler
// Type: UnityEditor.PluginImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Modules;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (PluginImporter))]
  internal class PluginImporterInspector : AssetImporterInspector
  {
    private static readonly BuildTarget[] m_StandaloneTargets = new BuildTarget[8]{ BuildTarget.StandaloneOSXIntel, BuildTarget.StandaloneOSXIntel64, BuildTarget.StandaloneOSXUniversal, BuildTarget.StandaloneWindows, BuildTarget.StandaloneWindows64, BuildTarget.StandaloneLinux, BuildTarget.StandaloneLinux64, BuildTarget.StandaloneLinuxUniversal };
    private int[] m_CompatibleWithPlatform = new int[PluginImporterInspector.GetPlatformGroupArraySize()];
    private Vector2 m_InformationScrollPosition = Vector2.zero;
    private EditorPluginImporterExtension m_EditorExtension = new EditorPluginImporterExtension();
    private DesktopPluginImporterExtension m_DesktopExtension = new DesktopPluginImporterExtension();
    private bool m_HasModified;
    private int m_CompatibleWithAnyPlatform;
    private int m_CompatibleWithEditor;
    private Dictionary<string, string> m_PluginInformation;
    private IPluginImporterExtension[] m_AdditionalExtensions;

    internal override bool showImportedObject
    {
      get
      {
        return false;
      }
    }

    internal IPluginImporterExtension[] additionalExtensions
    {
      get
      {
        if (this.m_AdditionalExtensions == null)
          this.m_AdditionalExtensions = new IPluginImporterExtension[2]
          {
            (IPluginImporterExtension) this.m_EditorExtension,
            (IPluginImporterExtension) this.m_DesktopExtension
          };
        return this.m_AdditionalExtensions;
      }
    }

    internal PluginImporter importer
    {
      get
      {
        return this.target as PluginImporter;
      }
    }

    internal PluginImporter[] importers
    {
      get
      {
        return this.targets.Cast<PluginImporter>().ToArray<PluginImporter>();
      }
    }

    private int compatibleWithStandalone
    {
      get
      {
        bool flag = false;
        foreach (BuildTarget standaloneTarget in PluginImporterInspector.m_StandaloneTargets)
        {
          if (this.m_CompatibleWithPlatform[(int) standaloneTarget] == -1)
            return -1;
          flag |= this.m_CompatibleWithPlatform[(int) standaloneTarget] > 0;
        }
        return flag ? 1 : 0;
      }
      set
      {
        foreach (int standaloneTarget in PluginImporterInspector.m_StandaloneTargets)
          this.m_CompatibleWithPlatform[standaloneTarget] = value;
      }
    }

    private static bool IgnorePlatform(BuildTarget platform)
    {
      return platform == BuildTarget.StandaloneGLESEmu;
    }

    private bool IsEditingPlatformSettingsSupported()
    {
      return this.targets.Length == 1;
    }

    private static int GetPlatformGroupArraySize()
    {
      int num = 0;
      using (List<Enum>.Enumerator enumerator = typeof (BuildTarget).EnumGetNonObsoleteValues().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          BuildTarget current = (BuildTarget) enumerator.Current;
          if ((BuildTarget) num < current + 1)
            num = (int) (current + 1);
        }
      }
      return num;
    }

    private static bool IsStandaloneTarget(BuildTarget buildTarget)
    {
      return ((IEnumerable<BuildTarget>) PluginImporterInspector.m_StandaloneTargets).Contains<BuildTarget>(buildTarget);
    }

    internal bool GetCompatibleWithPlatform(string platformName)
    {
      if (this.targets.Length > 1)
        throw new InvalidOperationException("Cannot be used while multiple plugins are selected");
      return this.m_CompatibleWithPlatform[(int) BuildPipeline.GetBuildTargetByName(platformName)] == 1;
    }

    internal void SetCompatibleWithPlatform(string platformName, bool enabled)
    {
      if (this.targets.Length > 1)
        throw new InvalidOperationException("Cannot be used while multiple plugins are selected");
      int num = !enabled ? 0 : 1;
      int buildTargetByName = (int) BuildPipeline.GetBuildTargetByName(platformName);
      if (this.m_CompatibleWithPlatform[buildTargetByName] == num)
        return;
      this.m_CompatibleWithPlatform[buildTargetByName] = num;
      this.m_HasModified = true;
    }

    private static List<BuildTarget> GetValidBuildTargets()
    {
      List<BuildTarget> buildTargetList = new List<BuildTarget>();
      using (List<Enum>.Enumerator enumerator = typeof (BuildTarget).EnumGetNonObsoleteValues().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          BuildTarget current = (BuildTarget) enumerator.Current;
          if (!PluginImporterInspector.IgnorePlatform(current) && (!ModuleManager.IsPlatformSupported(current) || ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(current)) || PluginImporterInspector.IsStandaloneTarget(current)))
            buildTargetList.Add(current);
        }
      }
      return buildTargetList;
    }

    private BuildPlayerWindow.BuildPlatform[] GetBuildPlayerValidPlatforms()
    {
      List<BuildPlayerWindow.BuildPlatform> validPlatforms = BuildPlayerWindow.GetValidPlatforms();
      List<BuildPlayerWindow.BuildPlatform> buildPlatformList = new List<BuildPlayerWindow.BuildPlatform>();
      if (this.m_CompatibleWithAnyPlatform > 0 || this.m_CompatibleWithEditor > 0)
        buildPlatformList.Add(new BuildPlayerWindow.BuildPlatform("Editor settings", "BuildSettings.Editor", BuildTargetGroup.Unknown, true)
        {
          name = BuildPipeline.GetEditorTargetName()
        });
      using (List<BuildPlayerWindow.BuildPlatform>.Enumerator enumerator = validPlatforms.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          BuildPlayerWindow.BuildPlatform current = enumerator.Current;
          if (!PluginImporterInspector.IgnorePlatform(current.DefaultTarget))
          {
            if (current.targetGroup == BuildTargetGroup.Standalone)
            {
              if (this.m_CompatibleWithAnyPlatform < 1 && this.compatibleWithStandalone < 1)
                continue;
            }
            else if (this.m_CompatibleWithAnyPlatform < 1 && this.m_CompatibleWithPlatform[(int) current.DefaultTarget] < 1 || ModuleManager.GetPluginImporterExtension(current.targetGroup) == null)
              continue;
            buildPlatformList.Add(current);
          }
        }
      }
      return buildPlatformList.ToArray();
    }

    private void ResetCompatability(ref int value, PluginImporterInspector.GetComptability getComptability)
    {
      value = !getComptability(this.importer) ? 0 : 1;
      foreach (PluginImporter importer in this.importers)
      {
        if (value != (!getComptability(importer) ? 0 : 1))
        {
          value = -1;
          break;
        }
      }
    }

    internal override void ResetValues()
    {
      base.ResetValues();
      this.m_HasModified = false;
      this.ResetCompatability(ref this.m_CompatibleWithAnyPlatform, (PluginImporterInspector.GetComptability) (imp => imp.GetCompatibleWithAnyPlatform()));
      this.ResetCompatability(ref this.m_CompatibleWithEditor, (PluginImporterInspector.GetComptability) (imp => imp.GetCompatibleWithEditor()));
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PluginImporterInspector.\u003CResetValues\u003Ec__AnonStorey7E valuesCAnonStorey7E = new PluginImporterInspector.\u003CResetValues\u003Ec__AnonStorey7E();
      using (List<BuildTarget>.Enumerator enumerator = PluginImporterInspector.GetValidBuildTargets().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          // ISSUE: reference to a compiler-generated field
          valuesCAnonStorey7E.platform = enumerator.Current;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.ResetCompatability(ref this.m_CompatibleWithPlatform[(int) valuesCAnonStorey7E.platform], new PluginImporterInspector.GetComptability(valuesCAnonStorey7E.\u003C\u003Em__12A));
        }
      }
      if (!this.IsEditingPlatformSettingsSupported())
        return;
      foreach (IPluginImporterExtension additionalExtension in this.additionalExtensions)
        additionalExtension.ResetValues(this);
      using (List<BuildTarget>.Enumerator enumerator = PluginImporterInspector.GetValidBuildTargets().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(enumerator.Current);
          if (importerExtension != null)
            importerExtension.ResetValues(this);
        }
      }
    }

    internal override bool HasModified()
    {
      bool flag = this.m_HasModified || base.HasModified();
      if (!this.IsEditingPlatformSettingsSupported())
        return flag;
      foreach (IPluginImporterExtension additionalExtension in this.additionalExtensions)
        flag |= additionalExtension.HasModified(this);
      using (List<BuildTarget>.Enumerator enumerator = PluginImporterInspector.GetValidBuildTargets().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(enumerator.Current);
          if (importerExtension != null)
            flag |= importerExtension.HasModified(this);
        }
      }
      return flag;
    }

    internal override void Apply()
    {
      base.Apply();
      foreach (PluginImporter importer in this.importers)
      {
        if (this.m_CompatibleWithAnyPlatform > -1)
          importer.SetCompatibleWithAnyPlatform(this.m_CompatibleWithAnyPlatform == 1);
        if (this.m_CompatibleWithEditor > -1)
          importer.SetCompatibleWithEditor(this.m_CompatibleWithEditor == 1);
        using (List<BuildTarget>.Enumerator enumerator = PluginImporterInspector.GetValidBuildTargets().GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            BuildTarget current = enumerator.Current;
            if (this.m_CompatibleWithPlatform[(int) current] > -1)
              importer.SetCompatibleWithPlatform(current, this.m_CompatibleWithPlatform[(int) current] == 1);
          }
        }
      }
      if (!this.IsEditingPlatformSettingsSupported())
        return;
      foreach (IPluginImporterExtension additionalExtension in this.additionalExtensions)
        additionalExtension.Apply(this);
      using (List<BuildTarget>.Enumerator enumerator = PluginImporterInspector.GetValidBuildTargets().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(enumerator.Current);
          if (importerExtension != null)
            importerExtension.Apply(this);
        }
      }
    }

    private void OnEnable()
    {
      if (!this.IsEditingPlatformSettingsSupported())
        return;
      foreach (IPluginImporterExtension additionalExtension in this.additionalExtensions)
        additionalExtension.OnEnable(this);
      using (List<BuildTarget>.Enumerator enumerator = PluginImporterInspector.GetValidBuildTargets().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(enumerator.Current);
          if (importerExtension != null)
          {
            importerExtension.OnEnable(this);
            importerExtension.ResetValues(this);
          }
        }
      }
      this.m_PluginInformation = new Dictionary<string, string>();
      this.m_PluginInformation["Path"] = this.importer.assetPath;
      this.m_PluginInformation["Type"] = !this.importer.isNativePlugin ? "Managed" : "Native";
    }

    private new void OnDisable()
    {
      base.OnDisable();
      if (!this.IsEditingPlatformSettingsSupported())
        return;
      foreach (IPluginImporterExtension additionalExtension in this.additionalExtensions)
        additionalExtension.OnDisable(this);
      using (List<BuildTarget>.Enumerator enumerator = PluginImporterInspector.GetValidBuildTargets().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(enumerator.Current);
          if (importerExtension != null)
            importerExtension.OnDisable(this);
        }
      }
    }

    private int ToggleWithMixedValue(int value, string title)
    {
      EditorGUI.showMixedValue = value == -1;
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUILayout.Toggle(title, value == 1, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        return flag ? 1 : 0;
      EditorGUI.showMixedValue = false;
      return value;
    }

    private void ShowGeneralOptions()
    {
      EditorGUI.BeginChangeCheck();
      this.m_CompatibleWithAnyPlatform = this.ToggleWithMixedValue(this.m_CompatibleWithAnyPlatform, "Any Platform");
      EditorGUI.BeginDisabledGroup(this.m_CompatibleWithAnyPlatform == 1);
      this.m_CompatibleWithEditor = this.ToggleWithMixedValue(this.m_CompatibleWithEditor, "Editor");
      EditorGUI.BeginChangeCheck();
      int num = this.ToggleWithMixedValue(this.compatibleWithStandalone, "Standalone");
      if (EditorGUI.EndChangeCheck())
      {
        this.compatibleWithStandalone = num;
        this.m_DesktopExtension.ValidateSingleCPUTargets(this);
      }
      using (List<BuildTarget>.Enumerator enumerator = PluginImporterInspector.GetValidBuildTargets().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          BuildTarget current = enumerator.Current;
          if (!PluginImporterInspector.IsStandaloneTarget(current))
            this.m_CompatibleWithPlatform[(int) current] = this.ToggleWithMixedValue(this.m_CompatibleWithPlatform[(int) current], current.ToString());
        }
      }
      EditorGUI.EndDisabledGroup();
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_HasModified = true;
    }

    private void ShowEditorSettings()
    {
      this.m_EditorExtension.OnPlatformSettingsGUI(this);
    }

    private void ShowPlatformSettings()
    {
      BuildPlayerWindow.BuildPlatform[] playerValidPlatforms = this.GetBuildPlayerValidPlatforms();
      if (playerValidPlatforms.Length <= 0)
        return;
      GUILayout.Label("Platform settings", EditorStyles.boldLabel, new GUILayoutOption[0]);
      int index = EditorGUILayout.BeginPlatformGrouping(playerValidPlatforms, (GUIContent) null);
      if (playerValidPlatforms[index].name == BuildPipeline.GetEditorTargetName())
      {
        this.ShowEditorSettings();
      }
      else
      {
        BuildTargetGroup targetGroup = playerValidPlatforms[index].targetGroup;
        if (targetGroup == BuildTargetGroup.Standalone)
        {
          this.m_DesktopExtension.OnPlatformSettingsGUI(this);
        }
        else
        {
          IPluginImporterExtension importerExtension = ModuleManager.GetPluginImporterExtension(targetGroup);
          if (importerExtension != null)
            importerExtension.OnPlatformSettingsGUI(this);
        }
      }
      EditorGUILayout.EndPlatformGrouping();
    }

    public override void OnInspectorGUI()
    {
      EditorGUI.BeginDisabledGroup(false);
      GUILayout.Label("Select platforms for plugin", EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
      this.ShowGeneralOptions();
      EditorGUILayout.EndVertical();
      GUILayout.Space(10f);
      if (this.IsEditingPlatformSettingsSupported())
        this.ShowPlatformSettings();
      EditorGUI.EndDisabledGroup();
      this.ApplyRevertGUI();
      if (this.targets.Length > 1)
        return;
      GUILayout.Label("Information", EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.m_InformationScrollPosition = EditorGUILayout.BeginVerticalScrollView(this.m_InformationScrollPosition);
      using (Dictionary<string, string>.Enumerator enumerator = this.m_PluginInformation.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, string> current = enumerator.Current;
          GUILayout.BeginHorizontal();
          GUILayout.Label(current.Key, new GUILayoutOption[1]
          {
            GUILayout.Width(50f)
          });
          GUILayout.TextField(current.Value);
          GUILayout.EndHorizontal();
        }
      }
      EditorGUILayout.EndScrollView();
      GUILayout.FlexibleSpace();
      if (!this.importer.isNativePlugin)
        return;
      EditorGUILayout.HelpBox("Once a native plugin is loaded from script, it's never unloaded. If you deselect a native plugin and it's already loaded, please restart Unity.", MessageType.Warning);
    }

    private delegate bool GetComptability(PluginImporter imp);
  }
}

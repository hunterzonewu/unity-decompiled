// Decompiled with JetBrains decompiler
// Type: UnityEditor.DesktopPluginImporterExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Modules;
using UnityEngine;

namespace UnityEditor
{
  internal class DesktopPluginImporterExtension : DefaultPluginImporterExtension
  {
    private DesktopPluginImporterExtension.DesktopSingleCPUProperty m_WindowsX86;
    private DesktopPluginImporterExtension.DesktopSingleCPUProperty m_WindowsX86_X64;
    private DesktopPluginImporterExtension.DesktopSingleCPUProperty m_LinuxX86;
    private DesktopPluginImporterExtension.DesktopSingleCPUProperty m_LinuxX86_X64;
    private DesktopPluginImporterExtension.DesktopSingleCPUProperty m_OSXX86;
    private DesktopPluginImporterExtension.DesktopSingleCPUProperty m_OSXX86_X64;

    public DesktopPluginImporterExtension()
      : base((DefaultPluginImporterExtension.Property[]) null)
    {
      this.properties = this.GetProperties();
    }

    private DefaultPluginImporterExtension.Property[] GetProperties()
    {
      List<DefaultPluginImporterExtension.Property> propertyList = new List<DefaultPluginImporterExtension.Property>();
      this.m_WindowsX86 = new DesktopPluginImporterExtension.DesktopSingleCPUProperty(EditorGUIUtility.TextContent("x86"), BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneWindows));
      this.m_WindowsX86_X64 = new DesktopPluginImporterExtension.DesktopSingleCPUProperty(EditorGUIUtility.TextContent("x86_x64"), BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneWindows64));
      this.m_LinuxX86 = new DesktopPluginImporterExtension.DesktopSingleCPUProperty(EditorGUIUtility.TextContent("x86"), BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneLinux), DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.x86);
      this.m_LinuxX86_X64 = new DesktopPluginImporterExtension.DesktopSingleCPUProperty(EditorGUIUtility.TextContent("x86_x64"), BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneLinux64), DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.x86_64);
      this.m_OSXX86 = new DesktopPluginImporterExtension.DesktopSingleCPUProperty(EditorGUIUtility.TextContent("x86"), BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneOSXIntel));
      this.m_OSXX86_X64 = new DesktopPluginImporterExtension.DesktopSingleCPUProperty(EditorGUIUtility.TextContent("x86_x64"), BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneOSXIntel64));
      propertyList.Add((DefaultPluginImporterExtension.Property) this.m_WindowsX86);
      propertyList.Add((DefaultPluginImporterExtension.Property) this.m_WindowsX86_X64);
      propertyList.Add((DefaultPluginImporterExtension.Property) this.m_LinuxX86);
      propertyList.Add((DefaultPluginImporterExtension.Property) this.m_LinuxX86_X64);
      propertyList.Add((DefaultPluginImporterExtension.Property) this.m_OSXX86);
      propertyList.Add((DefaultPluginImporterExtension.Property) this.m_OSXX86_X64);
      return propertyList.ToArray();
    }

    private DesktopPluginImporterExtension.DesktopPluginCPUArchitecture CalculateMultiCPUArchitecture(bool x86, bool x64)
    {
      if (x86 && x64)
        return DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.AnyCPU;
      if (x86 && !x64)
        return DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.x86;
      return !x86 && x64 ? DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.x86_64 : DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.None;
    }

    private bool IsUsableOnWindows(PluginImporter imp)
    {
      return !imp.isNativePlugin || Path.GetExtension(imp.assetPath) == ".dll";
    }

    private bool IsUsableOnOSX(PluginImporter imp)
    {
      if (!imp.isNativePlugin)
        return true;
      string extension = Path.GetExtension(imp.assetPath);
      return extension == ".so" || extension == ".bundle";
    }

    private bool IsUsableOnLinux(PluginImporter imp)
    {
      return !imp.isNativePlugin || Path.GetExtension(imp.assetPath) == ".so";
    }

    public override void OnPlatformSettingsGUI(PluginImporterInspector inspector)
    {
      PluginImporter importer = inspector.importer;
      EditorGUI.BeginChangeCheck();
      if (this.IsUsableOnWindows(importer))
      {
        EditorGUILayout.LabelField(EditorGUIUtility.TextContent("Windows"), EditorStyles.boldLabel, new GUILayoutOption[0]);
        this.m_WindowsX86.OnGUI(inspector);
        this.m_WindowsX86_X64.OnGUI(inspector);
        EditorGUILayout.Space();
      }
      if (this.IsUsableOnLinux(importer))
      {
        EditorGUILayout.LabelField(EditorGUIUtility.TextContent("Linux"), EditorStyles.boldLabel, new GUILayoutOption[0]);
        this.m_LinuxX86.OnGUI(inspector);
        this.m_LinuxX86_X64.OnGUI(inspector);
        EditorGUILayout.Space();
      }
      if (this.IsUsableOnOSX(importer))
      {
        EditorGUILayout.LabelField(EditorGUIUtility.TextContent("Mac OS X"), EditorStyles.boldLabel, new GUILayoutOption[0]);
        this.m_OSXX86.OnGUI(inspector);
        this.m_OSXX86_X64.OnGUI(inspector);
      }
      if (!EditorGUI.EndChangeCheck())
        return;
      this.ValidateUniversalTargets(inspector);
      this.hasModified = true;
    }

    public void ValidateSingleCPUTargets(PluginImporterInspector inspector)
    {
      DesktopPluginImporterExtension.DesktopSingleCPUProperty[] singleCpuPropertyArray = new DesktopPluginImporterExtension.DesktopSingleCPUProperty[6]{ this.m_WindowsX86, this.m_WindowsX86_X64, this.m_LinuxX86, this.m_LinuxX86_X64, this.m_OSXX86, this.m_OSXX86_X64 };
      foreach (DesktopPluginImporterExtension.DesktopSingleCPUProperty singleCpuProperty in singleCpuPropertyArray)
      {
        string str = !singleCpuProperty.IsTargetEnabled(inspector) ? DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.None.ToString() : singleCpuProperty.defaultValue.ToString();
        inspector.importer.SetPlatformData(singleCpuProperty.platformName, "CPU", str);
      }
      this.ValidateUniversalTargets(inspector);
    }

    private void ValidateUniversalTargets(PluginImporterInspector inspector)
    {
      bool x86_1 = this.m_LinuxX86.IsTargetEnabled(inspector);
      bool x64_1 = this.m_LinuxX86_X64.IsTargetEnabled(inspector);
      DesktopPluginImporterExtension.DesktopPluginCPUArchitecture multiCpuArchitecture1 = this.CalculateMultiCPUArchitecture(x86_1, x64_1);
      inspector.importer.SetPlatformData(BuildTarget.StandaloneLinuxUniversal, "CPU", multiCpuArchitecture1.ToString());
      inspector.SetCompatibleWithPlatform(BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneLinuxUniversal), x86_1 || x64_1);
      bool x86_2 = this.m_OSXX86.IsTargetEnabled(inspector);
      bool x64_2 = this.m_OSXX86_X64.IsTargetEnabled(inspector);
      DesktopPluginImporterExtension.DesktopPluginCPUArchitecture multiCpuArchitecture2 = this.CalculateMultiCPUArchitecture(x86_2, x64_2);
      inspector.importer.SetPlatformData(BuildTarget.StandaloneOSXUniversal, "CPU", multiCpuArchitecture2.ToString());
      inspector.SetCompatibleWithPlatform(BuildPipeline.GetBuildTargetName(BuildTarget.StandaloneOSXUniversal), x86_2 || x64_2);
    }

    public override string CalculateFinalPluginPath(string platformName, PluginImporter imp)
    {
      BuildTarget buildTargetByName = BuildPipeline.GetBuildTargetByName(platformName);
      bool flag1 = buildTargetByName == BuildTarget.StandaloneWindows || buildTargetByName == BuildTarget.StandaloneWindows64;
      bool flag2 = buildTargetByName == BuildTarget.StandaloneOSXIntel || buildTargetByName == BuildTarget.StandaloneOSXIntel64 || buildTargetByName == BuildTarget.StandaloneOSXUniversal;
      bool flag3 = buildTargetByName == BuildTarget.StandaloneLinux || buildTargetByName == BuildTarget.StandaloneLinux64 || buildTargetByName == BuildTarget.StandaloneLinuxUniversal;
      if (!flag3 && !flag2 && !flag1)
        throw new Exception(string.Format("Failed to resolve standalone platform, platform string '{0}', resolved target '{1}'", (object) platformName, (object) buildTargetByName.ToString()));
      if (flag1 && !this.IsUsableOnWindows(imp) || flag2 && !this.IsUsableOnOSX(imp) || flag3 && !this.IsUsableOnLinux(imp))
        return string.Empty;
      string platformData = imp.GetPlatformData(platformName, "CPU");
      if (string.Compare(platformData, "None", true) == 0)
        return string.Empty;
      if (!string.IsNullOrEmpty(platformData) && string.Compare(platformData, "AnyCPU", true) != 0)
        return Path.Combine(platformData, Path.GetFileName(imp.assetPath));
      return Path.GetFileName(imp.assetPath);
    }

    internal enum DesktopPluginCPUArchitecture
    {
      None,
      AnyCPU,
      x86,
      x86_64,
    }

    internal class DesktopSingleCPUProperty : DefaultPluginImporterExtension.Property
    {
      public DesktopSingleCPUProperty(GUIContent name, string platformName)
        : this(name, platformName, DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.AnyCPU)
      {
      }

      public DesktopSingleCPUProperty(GUIContent name, string platformName, DesktopPluginImporterExtension.DesktopPluginCPUArchitecture architecture)
        : base(name, "CPU", (object) architecture, platformName)
      {
      }

      internal bool IsTargetEnabled(PluginImporterInspector inspector)
      {
        return inspector.GetCompatibleWithPlatform(this.platformName);
      }

      internal override void OnGUI(PluginImporterInspector inspector)
      {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10f);
        EditorGUI.BeginChangeCheck();
        bool enabled = EditorGUILayout.Toggle(this.name, this.IsTargetEnabled(inspector), new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          this.value = !enabled ? (object) DesktopPluginImporterExtension.DesktopPluginCPUArchitecture.None : this.defaultValue;
          inspector.SetCompatibleWithPlatform(this.platformName, enabled);
        }
        EditorGUILayout.EndHorizontal();
      }
    }
  }
}

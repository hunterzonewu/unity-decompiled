// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.PluginsHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEditor.Modules;

namespace UnityEditorInternal
{
  internal class PluginsHelper
  {
    public static bool CheckFileCollisions(BuildTarget buildTarget)
    {
      IPluginImporterExtension importerExtension = (IPluginImporterExtension) null;
      if (ModuleManager.IsPlatformSupported(buildTarget))
        importerExtension = ModuleManager.GetPluginImporterExtension(buildTarget);
      if (importerExtension == null)
        importerExtension = BuildPipeline.GetBuildTargetGroup(buildTarget) != BuildTargetGroup.Standalone ? (IPluginImporterExtension) new DefaultPluginImporterExtension((DefaultPluginImporterExtension.Property[]) null) : (IPluginImporterExtension) new DesktopPluginImporterExtension();
      return importerExtension.CheckFileCollisions(BuildPipeline.GetBuildTargetName(buildTarget));
    }
  }
}

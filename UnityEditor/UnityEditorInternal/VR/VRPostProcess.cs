// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VR.VRPostProcess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

namespace UnityEditorInternal.VR
{
  internal static class VRPostProcess
  {
    [RegisterPlugins]
    private static IEnumerable<PluginDesc> RegisterPlugins(BuildTarget target)
    {
      if (target != BuildTarget.Android || !PlayerSettings.virtualRealitySupported)
        return (IEnumerable<PluginDesc>) new PluginDesc[0];
      PluginDesc pluginDesc = new PluginDesc();
      string path1 = EditorApplication.applicationContentsPath + "/VR/oculus/" + BuildPipeline.GetBuildTargetName(target);
      pluginDesc.pluginPath = Path.Combine(path1, "ovrplugin.aar");
      return (IEnumerable<PluginDesc>) new PluginDesc[1]{ pluginDesc };
    }
  }
}

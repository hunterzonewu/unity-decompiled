// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.IPluginImporterExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Modules
{
  internal interface IPluginImporterExtension
  {
    void ResetValues(PluginImporterInspector inspector);

    bool HasModified(PluginImporterInspector inspector);

    void Apply(PluginImporterInspector inspector);

    void OnEnable(PluginImporterInspector inspector);

    void OnDisable(PluginImporterInspector inspector);

    void OnPlatformSettingsGUI(PluginImporterInspector inspector);

    string CalculateFinalPluginPath(string buildTargetName, PluginImporter imp);

    bool CheckFileCollisions(string buildTargetName);
  }
}

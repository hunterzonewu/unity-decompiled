// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.IPlatformSupportModule
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Modules
{
  internal interface IPlatformSupportModule
  {
    string TargetName { get; }

    string JamTarget { get; }

    string[] NativeLibraries { get; }

    string[] AssemblyReferencesForUserScripts { get; }

    string ExtensionVersion { get; }

    GUIContent[] GetDisplayNames();

    IBuildPostprocessor CreateBuildPostprocessor();

    IScriptingImplementations CreateScriptingImplementations();

    ISettingEditorExtension CreateSettingsEditorExtension();

    IPreferenceWindowExtension CreatePreferenceWindowExtension();

    IBuildWindowExtension CreateBuildWindowExtension();

    ICompilationExtension CreateCompilationExtension();

    IPluginImporterExtension CreatePluginImporterExtension();

    IUserAssembliesValidator CreateUserAssembliesValidatorExtension();

    IDevice CreateDevice(string id);

    void OnActivate();

    void OnDeactivate();

    void OnLoad();

    void OnUnload();
  }
}

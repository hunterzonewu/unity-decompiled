// Decompiled with JetBrains decompiler
// Type: UnityEditor.VisualStudioIntegration.ISolutionSynchronizationSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.VisualStudioIntegration
{
  internal interface ISolutionSynchronizationSettings
  {
    int VisualStudioVersion { get; }

    string SolutionTemplate { get; }

    string SolutionProjectEntryTemplate { get; }

    string SolutionProjectConfigurationTemplate { get; }

    string EditorAssemblyPath { get; }

    string EngineAssemblyPath { get; }

    string MonoLibFolder { get; }

    string[] Defines { get; }

    string GetProjectHeaderTemplate(ScriptingLanguage language);

    string GetProjectFooterTemplate(ScriptingLanguage language);
  }
}

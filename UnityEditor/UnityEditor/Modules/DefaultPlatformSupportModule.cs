// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.DefaultPlatformSupportModule
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.Modules
{
  internal abstract class DefaultPlatformSupportModule : IPlatformSupportModule
  {
    protected ICompilationExtension compilationExtension;

    public abstract string TargetName { get; }

    public abstract string JamTarget { get; }

    public virtual string ExtensionVersion
    {
      get
      {
        return (string) null;
      }
    }

    public virtual string[] NativeLibraries
    {
      get
      {
        return new string[0];
      }
    }

    public virtual string[] AssemblyReferencesForUserScripts
    {
      get
      {
        return new string[0];
      }
    }

    public virtual GUIContent[] GetDisplayNames()
    {
      return (GUIContent[]) null;
    }

    public abstract IBuildPostprocessor CreateBuildPostprocessor();

    public virtual IScriptingImplementations CreateScriptingImplementations()
    {
      return (IScriptingImplementations) null;
    }

    public virtual ISettingEditorExtension CreateSettingsEditorExtension()
    {
      return (ISettingEditorExtension) null;
    }

    public virtual IPreferenceWindowExtension CreatePreferenceWindowExtension()
    {
      return (IPreferenceWindowExtension) null;
    }

    public virtual IBuildWindowExtension CreateBuildWindowExtension()
    {
      return (IBuildWindowExtension) null;
    }

    public virtual ICompilationExtension CreateCompilationExtension()
    {
      if (this.compilationExtension != null)
        return this.compilationExtension;
      return this.compilationExtension = (ICompilationExtension) new DefaultCompilationExtension();
    }

    public virtual IPluginImporterExtension CreatePluginImporterExtension()
    {
      return (IPluginImporterExtension) null;
    }

    public virtual IUserAssembliesValidator CreateUserAssembliesValidatorExtension()
    {
      return (IUserAssembliesValidator) null;
    }

    public virtual IDevice CreateDevice(string id)
    {
      throw new NotSupportedException();
    }

    public virtual void OnActivate()
    {
    }

    public virtual void OnDeactivate()
    {
    }

    public virtual void OnLoad()
    {
    }

    public virtual void OnUnload()
    {
    }
  }
}

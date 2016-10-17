// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorPluginImporterExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Modules;

namespace UnityEditor
{
  internal class EditorPluginImporterExtension : DefaultPluginImporterExtension
  {
    private EditorPluginImporterExtension.EditorPluginCPUArchitecture cpu;
    private EditorPluginImporterExtension.EditorPluginOSArchitecture os;

    public EditorPluginImporterExtension()
      : base(EditorPluginImporterExtension.GetProperties())
    {
    }

    private static DefaultPluginImporterExtension.Property[] GetProperties()
    {
      return new DefaultPluginImporterExtension.Property[2]{ new DefaultPluginImporterExtension.Property(EditorGUIUtility.TextContent("CPU|Is plugin compatible with 32bit or 64bit Editor?"), "CPU", (object) EditorPluginImporterExtension.EditorPluginCPUArchitecture.AnyCPU, BuildPipeline.GetEditorTargetName()), new DefaultPluginImporterExtension.Property(EditorGUIUtility.TextContent("OS|Is plugin compatible with Windows, OS X or Linux Editor?"), "OS", (object) EditorPluginImporterExtension.EditorPluginOSArchitecture.AnyOS, BuildPipeline.GetEditorTargetName()) };
    }

    internal enum EditorPluginCPUArchitecture
    {
      AnyCPU,
      x86,
      x86_64,
    }

    internal enum EditorPluginOSArchitecture
    {
      AnyOS,
      OSX,
      Windows,
      Linux,
    }
  }
}

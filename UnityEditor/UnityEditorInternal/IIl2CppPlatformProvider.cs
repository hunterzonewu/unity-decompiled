// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.IIl2CppPlatformProvider
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;

namespace UnityEditorInternal
{
  internal interface IIl2CppPlatformProvider
  {
    BuildTarget target { get; }

    bool emitNullChecks { get; }

    bool enableStackTraces { get; }

    bool enableArrayBoundsCheck { get; }

    bool loadSymbols { get; }

    string nativeLibraryFileName { get; }

    string il2CppFolder { get; }

    bool developmentMode { get; }

    string moduleStrippingInformationFolder { get; }

    bool supportsEngineStripping { get; }

    string[] includePaths { get; }

    string[] libraryPaths { get; }

    INativeCompiler CreateNativeCompiler();
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.VisualStudioIntegration;
using UnityEngine;

namespace UnityEditor
{
  public sealed class EditorSettings : UnityEngine.Object
  {
    public static extern string unityRemoteDevice { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern string unityRemoteCompression { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern string unityRemoteResolution { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern string externalVersionControl { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern SerializationMode serializationMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool webSecurityEmulationEnabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern string webSecurityEmulationHostUrl { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern EditorBehaviorMode defaultBehaviorMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern SpritePackerMode spritePackerMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern int spritePackerPaddingPower { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static string[] projectGenerationUserExtensions
    {
      get
      {
        return ((IEnumerable<string>) EditorSettings.Internal_ProjectGenerationUserExtensions.Split(new char[1]{ ';' }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((Func<string, string>) (s => s.TrimStart('.', '*'))).ToArray<string>();
      }
      set
      {
        EditorSettings.Internal_ProjectGenerationUserExtensions = string.Join(";", value);
      }
    }

    public static string[] projectGenerationBuiltinExtensions
    {
      get
      {
        return SolutionSynchronizer.BuiltinSupportedExtensions.Keys.ToArray<string>();
      }
    }

    internal static extern string Internal_ProjectGenerationUserExtensions { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern string projectGenerationRootNamespace { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}

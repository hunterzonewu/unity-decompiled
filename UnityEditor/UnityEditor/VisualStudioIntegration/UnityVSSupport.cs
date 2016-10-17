// Decompiled with JetBrains decompiler
// Type: UnityEditor.VisualStudioIntegration.UnityVSSupport
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.VisualStudioIntegration
{
  internal class UnityVSSupport
  {
    private static bool m_ShouldUnityVSBeActive;
    public static string s_LoadedUnityVS;
    private static string s_AboutLabel;

    public static void Initialize()
    {
      UnityVSSupport.Initialize((string) null);
    }

    public static void Initialize(string editorPath)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UnityVSSupport.\u003CInitialize\u003Ec__AnonStoreyC4 initializeCAnonStoreyC4 = new UnityVSSupport.\u003CInitialize\u003Ec__AnonStoreyC4();
      if (Application.platform != RuntimePlatform.WindowsEditor)
        return;
      // ISSUE: reference to a compiler-generated field
      initializeCAnonStoreyC4.externalEditor = editorPath ?? EditorPrefs.GetString("kScriptsDefaultApp");
      // ISSUE: reference to a compiler-generated field
      if (initializeCAnonStoreyC4.externalEditor.EndsWith("UnityVS.OpenFile.exe"))
      {
        // ISSUE: reference to a compiler-generated field
        initializeCAnonStoreyC4.externalEditor = SyncVS.FindBestVisualStudio();
        // ISSUE: reference to a compiler-generated field
        if (initializeCAnonStoreyC4.externalEditor != null)
        {
          // ISSUE: reference to a compiler-generated field
          EditorPrefs.SetString("kScriptsDefaultApp", initializeCAnonStoreyC4.externalEditor);
        }
      }
      // ISSUE: reference to a compiler-generated method
      KeyValuePair<VisualStudioVersion, string>[] array = SyncVS.InstalledVisualStudios.Where<KeyValuePair<VisualStudioVersion, string>>(new Func<KeyValuePair<VisualStudioVersion, string>, bool>(initializeCAnonStoreyC4.\u003C\u003Em__237)).ToArray<KeyValuePair<VisualStudioVersion, string>>();
      bool flag = array.Length > 0;
      UnityVSSupport.m_ShouldUnityVSBeActive = flag;
      if (!flag)
        return;
      string vstuBridgeAssembly = UnityVSSupport.GetVstuBridgeAssembly(array[0].Key);
      if (vstuBridgeAssembly == null)
      {
        // ISSUE: reference to a compiler-generated field
        Console.WriteLine("Unable to find bridge dll in registry for Microsoft Visual Studio Tools for Unity for " + initializeCAnonStoreyC4.externalEditor);
      }
      else if (!File.Exists(vstuBridgeAssembly))
      {
        Console.WriteLine("Unable to find bridge dll on disk for Microsoft Visual Studio Tools for Unity for " + vstuBridgeAssembly);
      }
      else
      {
        UnityVSSupport.s_LoadedUnityVS = vstuBridgeAssembly;
        InternalEditorUtility.SetupCustomDll(Path.GetFileNameWithoutExtension(vstuBridgeAssembly), vstuBridgeAssembly);
      }
    }

    public static bool ShouldUnityVSBeActive()
    {
      return UnityVSSupport.m_ShouldUnityVSBeActive;
    }

    private static string GetVstuBridgeAssembly(VisualStudioVersion version)
    {
      try
      {
        string vsTargetYear = string.Empty;
        switch (version)
        {
          case VisualStudioVersion.VisualStudio2010:
            vsTargetYear = "2010";
            break;
          case VisualStudioVersion.VisualStudio2012:
            vsTargetYear = "2012";
            break;
          case VisualStudioVersion.VisualStudio2013:
            vsTargetYear = "2013";
            break;
          case VisualStudioVersion.VisualStudio2015:
            vsTargetYear = "2015";
            break;
        }
        return UnityVSSupport.GetVstuBridgePathFromRegistry(vsTargetYear, true) ?? UnityVSSupport.GetVstuBridgePathFromRegistry(vsTargetYear, false);
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }

    private static string GetVstuBridgePathFromRegistry(string vsTargetYear, bool currentUser)
    {
      return (string) Registry.GetValue(string.Format("{0}\\Software\\Microsoft\\Microsoft Visual Studio {1} Tools for Unity", !currentUser ? (object) "HKEY_LOCAL_MACHINE" : (object) "HKEY_CURRENT_USER", (object) vsTargetYear), "UnityExtensionPath", (object) null);
    }

    public static void ScriptEditorChanged(string editorPath)
    {
      if (Application.platform != RuntimePlatform.WindowsEditor)
        return;
      UnityVSSupport.Initialize(editorPath);
      InternalEditorUtility.RequestScriptReload();
    }

    public static string GetAboutWindowLabel()
    {
      if (UnityVSSupport.s_AboutLabel != null)
        return UnityVSSupport.s_AboutLabel;
      UnityVSSupport.s_AboutLabel = UnityVSSupport.CalculateAboutWindowLabel();
      return UnityVSSupport.s_AboutLabel;
    }

    private static string CalculateAboutWindowLabel()
    {
      if (!UnityVSSupport.m_ShouldUnityVSBeActive)
        return string.Empty;
      Assembly assembly = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => a.Location == UnityVSSupport.s_LoadedUnityVS));
      if (assembly == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder("Microsoft Visual Studio Tools for Unity ");
      stringBuilder.Append((object) assembly.GetName().Version);
      stringBuilder.Append(" enabled");
      return stringBuilder.ToString();
    }
  }
}

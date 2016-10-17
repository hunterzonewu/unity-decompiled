// Decompiled with JetBrains decompiler
// Type: UnityEditor.PresetLibraryLocations
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal static class PresetLibraryLocations
  {
    public static string defaultLibraryLocation
    {
      get
      {
        return PresetLibraryLocations.GetDefaultFilePathForFileLocation(PresetFileLocation.PreferencesFolder);
      }
    }

    public static string defaultPresetLibraryPath
    {
      get
      {
        return Path.Combine(PresetLibraryLocations.defaultLibraryLocation, PresetLibraryLocations.defaultLibraryName);
      }
    }

    public static string defaultLibraryName
    {
      get
      {
        return "Default";
      }
    }

    public static List<string> GetAvailableFilesWithExtensionOnTheHDD(PresetFileLocation fileLocation, string fileExtensionWithoutDot)
    {
      List<string> exentionFromFolders = PresetLibraryLocations.GetFilesWithExentionFromFolders(PresetLibraryLocations.GetDirectoryPaths(fileLocation), fileExtensionWithoutDot);
      for (int index = 0; index < exentionFromFolders.Count; ++index)
        exentionFromFolders[index] = PresetLibraryLocations.ConvertToUnitySeperators(exentionFromFolders[index]);
      return exentionFromFolders;
    }

    public static string GetDefaultFilePathForFileLocation(PresetFileLocation fileLocation)
    {
      switch (fileLocation)
      {
        case PresetFileLocation.PreferencesFolder:
          return InternalEditorUtility.unityPreferencesFolder + "/Presets/";
        case PresetFileLocation.ProjectFolder:
          return "Assets/Editor/";
        default:
          Debug.LogError((object) "Enum not handled!");
          return string.Empty;
      }
    }

    private static List<string> GetDirectoryPaths(PresetFileLocation fileLocation)
    {
      List<string> stringList = new List<string>();
      switch (fileLocation)
      {
        case PresetFileLocation.PreferencesFolder:
          stringList.Add(PresetLibraryLocations.GetDefaultFilePathForFileLocation(PresetFileLocation.PreferencesFolder));
          break;
        case PresetFileLocation.ProjectFolder:
          string[] directories = Directory.GetDirectories("Assets/", "Editor", SearchOption.AllDirectories);
          stringList.AddRange((IEnumerable<string>) directories);
          break;
        default:
          Debug.LogError((object) "Enum not handled!");
          break;
      }
      return stringList;
    }

    private static List<string> GetFilesWithExentionFromFolders(List<string> folderPaths, string fileExtensionWithoutDot)
    {
      List<string> stringList = new List<string>();
      using (List<string>.Enumerator enumerator = folderPaths.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string[] files = Directory.GetFiles(enumerator.Current, "*." + fileExtensionWithoutDot);
          stringList.AddRange((IEnumerable<string>) files);
        }
      }
      return stringList;
    }

    public static PresetFileLocation GetFileLocationFromPath(string path)
    {
      if (path.Contains(InternalEditorUtility.unityPreferencesFolder))
        return PresetFileLocation.PreferencesFolder;
      if (path.Contains("Assets/"))
        return PresetFileLocation.ProjectFolder;
      Debug.LogError((object) ("Could not determine preset file location type " + path));
      return PresetFileLocation.ProjectFolder;
    }

    private static string ConvertToUnitySeperators(string path)
    {
      return path.Replace('\\', '/');
    }

    public static string GetParticleCurveLibraryExtension(bool singleCurve, bool signedRange)
    {
      string str1 = "particle";
      string str2 = !singleCurve ? str1 + "DoubleCurves" : str1 + "Curves";
      return !signedRange ? str2 + string.Empty : str2 + "Signed";
    }

    public static string GetCurveLibraryExtension(bool normalized)
    {
      return normalized ? "curvesNormalized" : "curves";
    }
  }
}

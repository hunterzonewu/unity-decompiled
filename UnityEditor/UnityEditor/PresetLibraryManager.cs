// Decompiled with JetBrains decompiler
// Type: UnityEditor.PresetLibraryManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class PresetLibraryManager : ScriptableSingleton<PresetLibraryManager>
  {
    private List<PresetLibraryManager.LibraryCache> m_LibraryCaches = new List<PresetLibraryManager.LibraryCache>();
    private static string s_LastError;

    private HideFlags libraryHideFlag
    {
      get
      {
        return HideFlags.DontSave;
      }
    }

    public void GetAvailableLibraries<T>(ScriptableObjectSaveLoadHelper<T> helper, out List<string> preferencesLibs, out List<string> projectLibs) where T : ScriptableObject
    {
      preferencesLibs = PresetLibraryLocations.GetAvailableFilesWithExtensionOnTheHDD(PresetFileLocation.PreferencesFolder, helper.fileExtensionWithoutDot);
      projectLibs = PresetLibraryLocations.GetAvailableFilesWithExtensionOnTheHDD(PresetFileLocation.ProjectFolder, helper.fileExtensionWithoutDot);
    }

    private string GetLibaryNameFromPath(string filePath)
    {
      return Path.GetFileNameWithoutExtension(filePath);
    }

    public T CreateLibrary<T>(ScriptableObjectSaveLoadHelper<T> helper, string presetLibraryPathWithoutExtension) where T : ScriptableObject
    {
      string libaryNameFromPath = this.GetLibaryNameFromPath(presetLibraryPathWithoutExtension);
      if (!InternalEditorUtility.IsValidFileName(libaryNameFromPath))
      {
        string invalidCharsOfFileName = InternalEditorUtility.GetDisplayStringOfInvalidCharsOfFileName(libaryNameFromPath);
        PresetLibraryManager.s_LastError = invalidCharsOfFileName.Length <= 0 ? "Invalid filename" : string.Format("A library filename cannot contain the following character{0}:  {1}", invalidCharsOfFileName.Length <= 1 ? (object) string.Empty : (object) "s", (object) invalidCharsOfFileName);
        return (T) null;
      }
      if ((Object) this.GetLibrary<T>(helper, presetLibraryPathWithoutExtension) != (Object) null)
      {
        PresetLibraryManager.s_LastError = "Library '" + libaryNameFromPath + "' already exists! Ensure a unique name.";
        return (T) null;
      }
      T obj = helper.Create();
      obj.hideFlags = this.libraryHideFlag;
      PresetLibraryManager.LibraryCache presetLibraryCache = this.GetPresetLibraryCache(helper.fileExtensionWithoutDot);
      presetLibraryCache.loadedLibraries.Add((ScriptableObject) obj);
      presetLibraryCache.loadedLibraryIDs.Add(presetLibraryPathWithoutExtension);
      PresetLibraryManager.s_LastError = (string) null;
      return obj;
    }

    public T GetLibrary<T>(ScriptableObjectSaveLoadHelper<T> helper, string presetLibraryPathWithoutExtension) where T : ScriptableObject
    {
      PresetLibraryManager.LibraryCache presetLibraryCache = this.GetPresetLibraryCache(helper.fileExtensionWithoutDot);
      for (int index = 0; index < presetLibraryCache.loadedLibraryIDs.Count; ++index)
      {
        if (presetLibraryCache.loadedLibraryIDs[index] == presetLibraryPathWithoutExtension)
        {
          if ((Object) presetLibraryCache.loadedLibraries[index] != (Object) null)
            return presetLibraryCache.loadedLibraries[index] as T;
          presetLibraryCache.loadedLibraries.RemoveAt(index);
          presetLibraryCache.loadedLibraryIDs.RemoveAt(index);
          Debug.LogError((object) ("Invalid library detected: Reload " + presetLibraryCache.loadedLibraryIDs[index] + " from HDD"));
          break;
        }
      }
      T obj = helper.Load(presetLibraryPathWithoutExtension);
      if (!((Object) obj != (Object) null))
        return (T) null;
      obj.hideFlags = this.libraryHideFlag;
      presetLibraryCache.loadedLibraries.Add((ScriptableObject) obj);
      presetLibraryCache.loadedLibraryIDs.Add(presetLibraryPathWithoutExtension);
      return obj;
    }

    public void UnloadAllLibrariesFor<T>(ScriptableObjectSaveLoadHelper<T> helper) where T : ScriptableObject
    {
      for (int index = 0; index < this.m_LibraryCaches.Count; ++index)
      {
        if (this.m_LibraryCaches[index].identifier == helper.fileExtensionWithoutDot)
        {
          this.m_LibraryCaches[index].UnloadScriptableObjects();
          this.m_LibraryCaches.RemoveAt(index);
          break;
        }
      }
    }

    public void SaveLibrary<T>(ScriptableObjectSaveLoadHelper<T> helper, T library, string presetLibraryPathWithoutExtension) where T : ScriptableObject
    {
      bool flag = File.Exists(presetLibraryPathWithoutExtension + "." + helper.fileExtensionWithoutDot);
      helper.Save(library, presetLibraryPathWithoutExtension);
      if (flag)
        return;
      AssetDatabase.Refresh();
    }

    public string GetLastError()
    {
      string lastError = PresetLibraryManager.s_LastError;
      PresetLibraryManager.s_LastError = (string) null;
      return lastError;
    }

    private PresetLibraryManager.LibraryCache GetPresetLibraryCache(string identifier)
    {
      using (List<PresetLibraryManager.LibraryCache>.Enumerator enumerator = this.m_LibraryCaches.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          PresetLibraryManager.LibraryCache current = enumerator.Current;
          if (current.identifier == identifier)
            return current;
        }
      }
      PresetLibraryManager.LibraryCache libraryCache = new PresetLibraryManager.LibraryCache(identifier);
      this.m_LibraryCaches.Add(libraryCache);
      return libraryCache;
    }

    private class LibraryCache
    {
      private List<ScriptableObject> m_LoadedLibraries = new List<ScriptableObject>();
      private List<string> m_LoadedLibraryIDs = new List<string>();
      private string m_Identifier;

      public string identifier
      {
        get
        {
          return this.m_Identifier;
        }
      }

      public List<ScriptableObject> loadedLibraries
      {
        get
        {
          return this.m_LoadedLibraries;
        }
      }

      public List<string> loadedLibraryIDs
      {
        get
        {
          return this.m_LoadedLibraryIDs;
        }
      }

      public LibraryCache(string identifier)
      {
        this.m_Identifier = identifier;
      }

      public void UnloadScriptableObjects()
      {
        using (List<ScriptableObject>.Enumerator enumerator = this.m_LoadedLibraries.GetEnumerator())
        {
          while (enumerator.MoveNext())
            Object.DestroyImmediate((Object) enumerator.Current);
        }
        this.m_LoadedLibraries.Clear();
        this.m_LoadedLibraryIDs.Clear();
      }
    }
  }
}

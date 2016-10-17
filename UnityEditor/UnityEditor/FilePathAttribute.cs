// Decompiled with JetBrains decompiler
// Type: UnityEditor.FilePathAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [AttributeUsage(AttributeTargets.Class)]
  internal class FilePathAttribute : Attribute
  {
    public string filepath { get; set; }

    public FilePathAttribute(string relativePath, FilePathAttribute.Location location)
    {
      if (string.IsNullOrEmpty(relativePath))
      {
        Debug.LogError((object) "Invalid relative path! (its null or empty)");
      }
      else
      {
        if ((int) relativePath[0] == 47)
          relativePath = relativePath.Substring(1);
        if (location == FilePathAttribute.Location.PreferencesFolder)
          this.filepath = InternalEditorUtility.unityPreferencesFolder + "/" + relativePath;
        else
          this.filepath = relativePath;
      }
    }

    public enum Location
    {
      PreferencesFolder,
      ProjectFolder,
    }
  }
}

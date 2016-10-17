// Decompiled with JetBrains decompiler
// Type: UnityEditor.Utils.Paths
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.Utils
{
  internal static class Paths
  {
    public static string Combine(params string[] components)
    {
      if (components.Length < 1)
        throw new ArgumentException("At least one component must be provided!");
      string path1 = components[0];
      for (int index = 1; index < components.Length; ++index)
        path1 = Path.Combine(path1, components[index]);
      return path1;
    }

    public static string[] Split(string path)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) path.Split(Path.DirectorySeparatorChar));
      int index = 0;
      while (index < stringList.Count)
      {
        stringList[index] = stringList[index].Trim();
        if (stringList[index].Equals(string.Empty))
          stringList.RemoveAt(index);
        else
          ++index;
      }
      return stringList.ToArray();
    }

    public static string GetFileOrFolderName(string path)
    {
      if (File.Exists(path))
        return Path.GetFileName(path);
      if (!Directory.Exists(path))
        throw new ArgumentException("Target '" + path + "' does not exist.");
      string[] strArray = Paths.Split(path);
      return strArray[strArray.Length - 1];
    }

    public static string CreateTempDirectory()
    {
      string tempFileName = Path.GetTempFileName();
      File.Delete(tempFileName);
      Directory.CreateDirectory(tempFileName);
      return tempFileName;
    }

    public static string NormalizePath(this string path)
    {
      if ((int) Path.DirectorySeparatorChar == 92)
        return path.Replace('/', Path.DirectorySeparatorChar);
      return path.Replace('\\', Path.DirectorySeparatorChar);
    }

    public static bool AreEqual(string pathA, string pathB, bool ignoreCase)
    {
      if (pathA == string.Empty && pathB == string.Empty)
        return true;
      if (string.IsNullOrEmpty(pathA) || string.IsNullOrEmpty(pathB))
        return false;
      return string.Compare(Path.GetFullPath(pathA), Path.GetFullPath(pathB), !ignoreCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) == 0;
    }
  }
}

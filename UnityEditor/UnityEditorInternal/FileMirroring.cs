// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.FileMirroring
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityEditorInternal
{
  internal static class FileMirroring
  {
    public static void MirrorFile(string from, string to)
    {
      FileMirroring.MirrorFile(from, to, new Func<string, string, bool>(FileMirroring.CanSkipCopy));
    }

    public static void MirrorFile(string from, string to, Func<string, string, bool> comparer)
    {
      if (comparer(from, to))
        return;
      if (!File.Exists(from))
      {
        FileMirroring.DeleteFileOrDirectory(to);
      }
      else
      {
        string directoryName = Path.GetDirectoryName(to);
        if (!Directory.Exists(directoryName))
          Directory.CreateDirectory(directoryName);
        File.Copy(from, to, true);
      }
    }

    public static void MirrorFolder(string from, string to)
    {
      FileMirroring.MirrorFolder(from, to, new Func<string, string, bool>(FileMirroring.CanSkipCopy));
    }

    public static void MirrorFolder(string from, string to, Func<string, string, bool> comparer)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      FileMirroring.\u003CMirrorFolder\u003Ec__AnonStorey72 folderCAnonStorey72 = new FileMirroring.\u003CMirrorFolder\u003Ec__AnonStorey72();
      // ISSUE: reference to a compiler-generated field
      folderCAnonStorey72.to = to;
      // ISSUE: reference to a compiler-generated field
      folderCAnonStorey72.from = from;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      folderCAnonStorey72.from = Path.GetFullPath(folderCAnonStorey72.from);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      folderCAnonStorey72.to = Path.GetFullPath(folderCAnonStorey72.to);
      // ISSUE: reference to a compiler-generated field
      if (!Directory.Exists(folderCAnonStorey72.from))
      {
        // ISSUE: reference to a compiler-generated field
        if (!Directory.Exists(folderCAnonStorey72.to))
          return;
        // ISSUE: reference to a compiler-generated field
        Directory.Delete(folderCAnonStorey72.to, true);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (!Directory.Exists(folderCAnonStorey72.to))
        {
          // ISSUE: reference to a compiler-generated field
          Directory.CreateDirectory(folderCAnonStorey72.to);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        IEnumerable<string> first = ((IEnumerable<string>) Directory.GetFileSystemEntries(folderCAnonStorey72.to)).Select<string, string>(new Func<string, string>(folderCAnonStorey72.\u003C\u003Em__FA));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        IEnumerable<string> second = ((IEnumerable<string>) Directory.GetFileSystemEntries(folderCAnonStorey72.from)).Select<string, string>(new Func<string, string>(folderCAnonStorey72.\u003C\u003Em__FB));
        foreach (string path2 in first.Except<string>(second))
        {
          // ISSUE: reference to a compiler-generated field
          FileMirroring.DeleteFileOrDirectory(Path.Combine(folderCAnonStorey72.to, path2));
        }
        foreach (string path2 in second)
        {
          // ISSUE: reference to a compiler-generated field
          string str1 = Path.Combine(folderCAnonStorey72.from, path2);
          // ISSUE: reference to a compiler-generated field
          string str2 = Path.Combine(folderCAnonStorey72.to, path2);
          FileMirroring.FileEntryType fileEntryType1 = FileMirroring.FileEntryTypeFor(str1);
          FileMirroring.FileEntryType fileEntryType2 = FileMirroring.FileEntryTypeFor(str2);
          if (fileEntryType1 == FileMirroring.FileEntryType.File && fileEntryType2 == FileMirroring.FileEntryType.Directory)
            FileMirroring.DeleteFileOrDirectory(str2);
          if (fileEntryType1 == FileMirroring.FileEntryType.Directory)
          {
            if (fileEntryType2 == FileMirroring.FileEntryType.File)
              FileMirroring.DeleteFileOrDirectory(str2);
            if (fileEntryType2 != FileMirroring.FileEntryType.Directory)
              Directory.CreateDirectory(str2);
            FileMirroring.MirrorFolder(str1, str2);
          }
          if (fileEntryType1 == FileMirroring.FileEntryType.File)
            FileMirroring.MirrorFile(str1, str2, comparer);
        }
      }
    }

    private static void DeleteFileOrDirectory(string path)
    {
      if (File.Exists(path))
      {
        File.Delete(path);
      }
      else
      {
        if (!Directory.Exists(path))
          return;
        Directory.Delete(path, true);
      }
    }

    private static string StripPrefix(string s, string prefix)
    {
      return s.Substring(prefix.Length + 1);
    }

    private static FileMirroring.FileEntryType FileEntryTypeFor(string fileEntry)
    {
      if (File.Exists(fileEntry))
        return FileMirroring.FileEntryType.File;
      return Directory.Exists(fileEntry) ? FileMirroring.FileEntryType.Directory : FileMirroring.FileEntryType.NotExisting;
    }

    public static bool CanSkipCopy(string from, string to)
    {
      if (!File.Exists(to))
        return false;
      return FileMirroring.AreFilesIdentical(from, to);
    }

    private static bool AreFilesIdentical(string filePath1, string filePath2)
    {
      using (FileStream fileStream1 = File.OpenRead(filePath1))
      {
        using (FileStream fileStream2 = File.OpenRead(filePath2))
        {
          if (fileStream1.Length != fileStream2.Length)
            return false;
          byte[] array1 = new byte[65536];
          byte[] array2 = new byte[65536];
          int num;
          while ((num = fileStream1.Read(array1, 0, array1.Length)) > 0)
          {
            fileStream2.Read(array2, 0, array2.Length);
            for (int index = 0; index < num; ++index)
            {
              if ((int) array1[index] != (int) array2[index])
                return false;
            }
          }
        }
      }
      return true;
    }

    private enum FileEntryType
    {
      File,
      Directory,
      NotExisting,
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.PragmaFixing30
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditorInternal;

namespace UnityEditor.Scripting
{
  internal class PragmaFixing30
  {
    private static void FixJavaScriptPragmas()
    {
      string[] strArray = PragmaFixing30.CollectBadFiles();
      if (strArray.Length == 0)
        return;
      if (!InternalEditorUtility.inBatchMode)
        PragmaFixingWindow.ShowWindow(strArray);
      else
        PragmaFixing30.FixFiles(strArray);
    }

    public static void FixFiles(string[] filesToFix)
    {
      foreach (string fileName in filesToFix)
      {
        try
        {
          PragmaFixing30.FixPragmasInFile(fileName);
        }
        catch (Exception ex)
        {
          UnityEngine.Debug.LogError((object) ("Failed to fix pragmas in file '" + fileName + "'.\n" + ex.Message));
        }
      }
    }

    private static bool FileNeedsPragmaFixing(string fileName)
    {
      return PragmaFixing30.CheckOrFixPragmas(fileName, true);
    }

    private static void FixPragmasInFile(string fileName)
    {
      PragmaFixing30.CheckOrFixPragmas(fileName, false);
    }

    private static bool CheckOrFixPragmas(string fileName, bool onlyCheck)
    {
      string oldText = File.ReadAllText(fileName);
      StringBuilder sb = new StringBuilder(oldText);
      PragmaFixing30.LooseComments(sb);
      Match match = PragmaFixing30.PragmaMatch(sb, "strict");
      if (!match.Success)
        return false;
      bool success1 = PragmaFixing30.PragmaMatch(sb, "downcast").Success;
      bool success2 = PragmaFixing30.PragmaMatch(sb, "implicit").Success;
      if (success1 && success2)
        return false;
      if (!onlyCheck)
        PragmaFixing30.DoFixPragmasInFile(fileName, oldText, match.Index + match.Length, success1, success2);
      return true;
    }

    private static void DoFixPragmasInFile(string fileName, string oldText, int fixPos, bool hasDowncast, bool hasImplicit)
    {
      string str1 = string.Empty;
      string str2 = !PragmaFixing30.HasWinLineEndings(oldText) ? "\n" : "\r\n";
      if (!hasImplicit)
        str1 = str1 + str2 + "#pragma implicit";
      if (!hasDowncast)
        str1 = str1 + str2 + "#pragma downcast";
      File.WriteAllText(fileName, oldText.Insert(fixPos, str1));
    }

    private static bool HasWinLineEndings(string text)
    {
      return text.IndexOf("\r\n") != -1;
    }

    [DebuggerHidden]
    private static IEnumerable<string> SearchRecursive(string dir, string mask)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PragmaFixing30.\u003CSearchRecursive\u003Ec__Iterator9 recursiveCIterator9 = new PragmaFixing30.\u003CSearchRecursive\u003Ec__Iterator9() { dir = dir, mask = mask, \u003C\u0024\u003Edir = dir, \u003C\u0024\u003Emask = mask };
      // ISSUE: reference to a compiler-generated field
      recursiveCIterator9.\u0024PC = -2;
      return (IEnumerable<string>) recursiveCIterator9;
    }

    private static void LooseComments(StringBuilder sb)
    {
      IEnumerator enumerator = new Regex("//").Matches(sb.ToString()).GetEnumerator();
      try
      {
label_5:
        while (enumerator.MoveNext())
        {
          int index = ((Capture) enumerator.Current).Index;
          while (true)
          {
            if (index < sb.Length && (int) sb[index] != 10 && (int) sb[index] != 13)
              sb[index++] = ' ';
            else
              goto label_5;
          }
        }
      }
      finally
      {
        IDisposable disposable = enumerator as IDisposable;
        if (disposable != null)
          disposable.Dispose();
      }
    }

    private static Match PragmaMatch(StringBuilder sb, string pragma)
    {
      return new Regex("#\\s*pragma\\s*" + pragma).Match(sb.ToString());
    }

    private static string[] CollectBadFiles()
    {
      List<string> stringList = new List<string>();
      foreach (string fileName in PragmaFixing30.SearchRecursive(Path.Combine(Directory.GetCurrentDirectory(), "Assets"), "*.js"))
      {
        try
        {
          if (PragmaFixing30.FileNeedsPragmaFixing(fileName))
            stringList.Add(fileName);
        }
        catch (Exception ex)
        {
          UnityEngine.Debug.LogError((object) ("Failed to fix pragmas in file '" + fileName + "'.\n" + ex.Message));
        }
      }
      return stringList.ToArray();
    }
  }
}

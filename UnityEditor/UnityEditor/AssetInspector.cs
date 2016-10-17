// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetInspector
  {
    private GUIContent[] m_Menu = new GUIContent[3]{ EditorGUIUtility.TextContent("Show Diff"), EditorGUIUtility.TextContent("Show History"), EditorGUIUtility.TextContent("Discard") };
    private GUIContent[] m_UnmodifiedMenu = new GUIContent[1]{ EditorGUIUtility.TextContent("Show History") };
    private static AssetInspector s_Instance;

    private AssetInspector()
    {
    }

    internal static bool IsAssetServerSetUp()
    {
      if (InternalEditorUtility.HasTeamLicense())
        return ASEditorBackend.SettingsAreValid();
      return false;
    }

    private bool HasFlag(ChangeFlags flags, ChangeFlags flagToCheck)
    {
      return (flagToCheck & flags) != ChangeFlags.None;
    }

    public static AssetInspector Get()
    {
      if (AssetInspector.s_Instance == null)
        AssetInspector.s_Instance = new AssetInspector();
      return AssetInspector.s_Instance;
    }

    private string AddChangesetFlag(string str, string strToAdd)
    {
      if (str != string.Empty)
      {
        str += ", ";
        str += strToAdd;
      }
      else
        str = strToAdd;
      return str;
    }

    private string GetGUID()
    {
      if (Selection.objects.Length == 0)
        return string.Empty;
      return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(Selection.objects[0]));
    }

    private void DoShowDiff(string guid)
    {
      List<string> stringList = new List<string>();
      List<CompareInfo> compareInfoList = new List<CompareInfo>();
      int workingItemChangeset = AssetServer.GetWorkingItemChangeset(guid);
      stringList.Add(guid);
      compareInfoList.Add(new CompareInfo(workingItemChangeset, -1, 0, 1));
      Debug.Log((object) ("Comparing asset revisions " + workingItemChangeset.ToString() + " and Local"));
      AssetServer.CompareFiles(stringList.ToArray(), compareInfoList.ToArray());
    }

    private void ContextMenuClick(object userData, string[] options, int selected)
    {
      if ((bool) userData && selected == 0)
        selected = 1;
      switch (selected)
      {
        case 0:
          this.DoShowDiff(this.GetGUID());
          break;
        case 1:
          ASEditorBackend.DoAS();
          ASEditorBackend.ASWin.ShowHistory();
          break;
        case 2:
          if (!ASEditorBackend.SettingsIfNeeded())
            Debug.Log((object) "Asset Server connection for current project is not set up");
          if (!EditorUtility.DisplayDialog("Discard changes", "Are you sure you want to discard local changes of selected asset?", "Discard", "Cancel"))
            break;
          AssetServer.DoUpdateWithoutConflictResolutionOnNextTick(new string[1]
          {
            this.GetGUID()
          });
          break;
      }
    }

    private ChangeFlags GetChangeFlags()
    {
      string guid = this.GetGUID();
      if (guid == string.Empty)
        return ChangeFlags.None;
      return AssetServer.GetChangeFlags(guid);
    }

    private string GetModificationString(ChangeFlags flags)
    {
      string str = string.Empty;
      if (this.HasFlag(flags, ChangeFlags.Undeleted))
        str = this.AddChangesetFlag(str, "undeleted");
      if (this.HasFlag(flags, ChangeFlags.Modified))
        str = this.AddChangesetFlag(str, "modified");
      if (this.HasFlag(flags, ChangeFlags.Renamed))
        str = this.AddChangesetFlag(str, "renamed");
      if (this.HasFlag(flags, ChangeFlags.Moved))
        str = this.AddChangesetFlag(str, "moved");
      if (this.HasFlag(flags, ChangeFlags.Created))
        str = this.AddChangesetFlag(str, "created");
      return str;
    }

    public void OnAssetStatusGUI(Rect r, int id, Object target, GUIStyle style)
    {
      if (target == (Object) null)
        return;
      string modificationString = this.GetModificationString(this.GetChangeFlags());
      GUIContent content = !(modificationString == string.Empty) ? new GUIContent("Locally " + modificationString) : EditorGUIUtility.TextContent("Asset is unchanged");
      if (!EditorGUI.DoToggle(r, id, false, content, style))
        return;
      GUIUtility.hotControl = 0;
      r = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f);
      EditorUtility.DisplayCustomMenu(r, !(modificationString == string.Empty) ? this.m_Menu : this.m_UnmodifiedMenu, -1, new EditorUtility.SelectMenuItemFunction(this.ContextMenuClick), (object) (modificationString == string.Empty));
    }
  }
}

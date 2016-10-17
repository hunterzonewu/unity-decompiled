// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.WindowResolve
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

namespace UnityEditor.VersionControl
{
  internal class WindowResolve : EditorWindow
  {
    private ListControl resolveList = new ListControl();
    private AssetList assetList = new AssetList();
    private bool cancelled;

    public void OnEnable()
    {
      this.position = new Rect(100f, 100f, 650f, 330f);
      this.minSize = new Vector2(650f, 330f);
    }

    public void OnDisable()
    {
      if (this.cancelled)
        return;
      WindowPending.UpdateAllWindows();
    }

    public static void Open(ChangeSet change)
    {
      Task task = Provider.ChangeSetStatus(change);
      task.Wait();
      WindowResolve.GetWindow().DoOpen(task.assetList);
    }

    public static void Open(AssetList assets)
    {
      Task task = Provider.Status(assets);
      task.Wait();
      WindowResolve.GetWindow().DoOpen(task.assetList);
    }

    private static WindowResolve GetWindow()
    {
      return EditorWindow.GetWindow<WindowResolve>(true, "Version Control Resolve");
    }

    private void DoOpen(AssetList resolve)
    {
      bool flag = true;
      this.assetList = resolve.Filter((flag ? 1 : 0) != 0, Asset.States.Conflicted);
      this.RefreshList();
    }

    private void RefreshList()
    {
      this.resolveList.Clear();
      bool flag = true;
      using (List<Asset>.Enumerator enumerator = this.assetList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Asset current = enumerator.Current;
          ListItem listItem = this.resolveList.Add((ListItem) null, current.prettyPath, current);
          if (flag)
          {
            this.resolveList.SelectedSet(listItem);
            flag = false;
          }
          else
            this.resolveList.SelectedAdd(listItem);
        }
      }
      if (this.assetList.Count == 0)
      {
        ChangeSet change = new ChangeSet("no files to resolve");
        this.resolveList.Add((ListItem) null, change.description, change).Dummy = true;
      }
      this.resolveList.Refresh();
      this.Repaint();
    }

    private void OnGUI()
    {
      this.cancelled = false;
      GUILayout.Label("Conflicting files to resolve", EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      Rect screenRect = new Rect(6f, 40f, this.position.width - 12f, this.position.height - 112f);
      GUILayout.BeginArea(screenRect);
      GUILayout.Box(string.Empty, new GUILayoutOption[2]
      {
        GUILayout.ExpandWidth(true),
        GUILayout.ExpandHeight(true)
      });
      GUILayout.EndArea();
      bool flag = this.resolveList.OnGUI(new Rect(screenRect.x + 2f, screenRect.y + 2f, screenRect.width - 4f, screenRect.height - 4f), true);
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      GUI.enabled = this.assetList.Count > 0;
      GUILayout.Label("Resolve selection by:");
      if (GUILayout.Button("using local version"))
      {
        Provider.Resolve(this.resolveList.SelectedAssets, ResolveMethod.UseMine).Wait();
        AssetDatabase.Refresh();
        this.Close();
      }
      if (GUILayout.Button("using incoming version"))
      {
        Provider.Resolve(this.resolveList.SelectedAssets, ResolveMethod.UseTheirs).Wait();
        AssetDatabase.Refresh();
        this.Close();
      }
      MergeMethod method = MergeMethod.MergeNone;
      if (GUILayout.Button("merging"))
        method = MergeMethod.MergeAll;
      if (method != MergeMethod.MergeNone)
      {
        Task task1 = Provider.Merge(this.resolveList.SelectedAssets, method);
        task1.Wait();
        if (task1.success)
        {
          Task task2 = Provider.Resolve(task1.assetList, ResolveMethod.UseMerged);
          task2.Wait();
          if (task2.success)
          {
            Task task3 = Provider.Status(this.assetList);
            task3.Wait();
            this.DoOpen(task3.assetList);
            if (task3.success && this.assetList.Count == 0)
              this.Close();
          }
          else
          {
            EditorUtility.DisplayDialog("Error resolving", "Error during resolve of files. Inspect log for details", "Close");
            AssetDatabase.Refresh();
          }
        }
        else
        {
          EditorUtility.DisplayDialog("Error merging", "Error during merge of files. Inspect log for details", "Close");
          AssetDatabase.Refresh();
        }
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.Space(12f);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUI.enabled = true;
      if (GUILayout.Button("Cancel"))
      {
        this.cancelled = true;
        this.Close();
      }
      GUILayout.EndHorizontal();
      GUILayout.Space(12f);
      if (!flag)
        return;
      this.Repaint();
    }
  }
}

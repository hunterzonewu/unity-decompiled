// Decompiled with JetBrains decompiler
// Type: UnityEditor.HeapshotWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class HeapshotWindow : EditorWindow
  {
    private List<string> heapshotFiles = new List<string>();
    private int itemIndex = -1;
    private int selectedItem = -1;
    private string lastOpenedHeapshotFile = string.Empty;
    private string lastOpenedProfiler = string.Empty;
    private List<HeapshotWindow.HeapshotUIObject> hsRoots = new List<HeapshotWindow.HeapshotUIObject>();
    private List<HeapshotWindow.HeapshotUIObject> hsAllObjects = new List<HeapshotWindow.HeapshotUIObject>();
    private List<HeapshotWindow.HeapshotUIObject> hsBackTraceObjects = new List<HeapshotWindow.HeapshotUIObject>();
    private Vector2 leftViewScrollPosition = Vector2.zero;
    private Vector2 rightViewScrollPosition = Vector2.zero;
    private SplitterState viewSplit = new SplitterState(new float[2]{ 50f, 50f }, (int[]) null, (int[]) null);
    private string[] titleNames = new string[5]{ "Field Name", "Type", "Pointer", "Size", "References/Referenced" };
    private SplitterState titleSplit1 = new SplitterState(new float[5]{ 30f, 25f, 15f, 15f, 15f }, new int[5]{ 200, 200, 50, 50, 50 }, (int[]) null);
    private SplitterState titleSplit2 = new SplitterState(new float[5]{ 30f, 25f, 15f, 15f, 15f }, new int[5]{ 200, 200, 50, 50, 50 }, (int[]) null);
    private int selectedHeapshot = -1;
    private const string heapshotExtension = ".heapshot";
    private HeapshotReader heapshotReader;
    private Rect guiRect;
    private int currentTab;
    private static HeapshotWindow.DelegateReceivedHeapshot onReceivedHeapshot;
    private static HeapshotWindow.UIStyles ms_Styles;
    private int[] connectionGuids;

    private string HeapshotPath
    {
      get
      {
        return Application.dataPath + "/../Heapshots";
      }
    }

    private static HeapshotWindow.UIStyles Styles
    {
      get
      {
        return HeapshotWindow.ms_Styles ?? (HeapshotWindow.ms_Styles = new HeapshotWindow.UIStyles());
      }
    }

    private static void Init()
    {
      EditorWindow.GetWindow(typeof (HeapshotWindow)).titleContent = EditorGUIUtility.TextContent("Mono heapshot");
    }

    private static void EventHeapShotReceived(string name)
    {
      Debug.Log((object) ("Received " + name));
      if (HeapshotWindow.onReceivedHeapshot == null)
        return;
      HeapshotWindow.onReceivedHeapshot(name);
    }

    private void OnReceivedHeapshot(string name)
    {
      this.SearchForHeapShots();
      this.OpenHeapshot(name);
    }

    private void SearchForHeapShots()
    {
      this.heapshotFiles.Clear();
      if (!Directory.Exists(this.HeapshotPath))
        return;
      string[] files = Directory.GetFiles(this.HeapshotPath, "*.heapshot");
      this.selectedHeapshot = -1;
      foreach (string str1 in files)
      {
        string str2 = str1.Substring(str1.LastIndexOf("\\") + 1);
        this.heapshotFiles.Add(str2.Substring(0, str2.IndexOf(".heapshot")));
      }
      if (this.heapshotFiles.Count <= 0)
        return;
      this.selectedHeapshot = this.heapshotFiles.Count - 1;
    }

    private void OnEnable()
    {
      HeapshotWindow.onReceivedHeapshot = new HeapshotWindow.DelegateReceivedHeapshot(this.OnReceivedHeapshot);
    }

    private void OnDisable()
    {
      HeapshotWindow.onReceivedHeapshot = (HeapshotWindow.DelegateReceivedHeapshot) null;
    }

    private void OnFocus()
    {
      this.SearchForHeapShots();
    }

    private void RefreshHeapshotUIObjects()
    {
      this.hsRoots.Clear();
      this.hsAllObjects.Clear();
      using (List<HeapshotReader.ReferenceInfo>.Enumerator enumerator = this.heapshotReader.Roots.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          HeapshotReader.ReferenceInfo current = enumerator.Current;
          this.hsRoots.Add(new HeapshotWindow.HeapshotUIObject(current.fieldInfo.name, current.referencedObject, false));
        }
      }
      SortedDictionary<string, List<HeapshotReader.ObjectInfo>> sortedDictionary = new SortedDictionary<string, List<HeapshotReader.ObjectInfo>>();
      using (List<HeapshotReader.ObjectInfo>.Enumerator enumerator = this.heapshotReader.Objects.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          HeapshotReader.ObjectInfo current = enumerator.Current;
          if (current.type == HeapshotReader.ObjectType.Managed)
          {
            string name = current.typeInfo.name;
            if (!sortedDictionary.ContainsKey(name))
              sortedDictionary.Add(name, new List<HeapshotReader.ObjectInfo>());
            sortedDictionary[name].Add(current);
          }
        }
      }
      using (SortedDictionary<string, List<HeapshotReader.ObjectInfo>>.Enumerator enumerator1 = sortedDictionary.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<string, List<HeapshotReader.ObjectInfo>> current1 = enumerator1.Current;
          HeapshotReader.ObjectInfo refObject = new HeapshotReader.ObjectInfo();
          HeapshotReader.FieldInfo field = new HeapshotReader.FieldInfo("(Unknown)");
          using (List<HeapshotReader.ObjectInfo>.Enumerator enumerator2 = current1.Value.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              HeapshotReader.ObjectInfo current2 = enumerator2.Current;
              refObject.references.Add(new HeapshotReader.ReferenceInfo(current2, field));
            }
          }
          this.hsAllObjects.Add(new HeapshotWindow.HeapshotUIObject(current1.Key + " x " + (object) current1.Value.Count, refObject, false)
          {
            IsDummyObject = true
          });
        }
      }
    }

    private int GetItemCount(List<HeapshotWindow.HeapshotUIObject> objects)
    {
      int num = 0;
      using (List<HeapshotWindow.HeapshotUIObject>.Enumerator enumerator = objects.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          HeapshotWindow.HeapshotUIObject current = enumerator.Current;
          ++num;
          if (current.IsExpanded)
            num += this.GetItemCount(current.Children);
        }
      }
      return num;
    }

    private void OpenHeapshot(string fileName)
    {
      this.heapshotReader = new HeapshotReader();
      string fileName1 = this.HeapshotPath + "/" + fileName;
      if (this.heapshotReader.Open(fileName1))
      {
        this.lastOpenedHeapshotFile = fileName;
        this.RefreshHeapshotUIObjects();
      }
      else
        Debug.LogError((object) ("Failed to read " + fileName1));
    }

    private void OnGUI()
    {
      GUI.Label(new Rect(0.0f, 0.0f, this.position.width, 20f), "Heapshots are located here: " + Path.Combine(Application.dataPath, "Heapshots"));
      GUI.Label(new Rect(0.0f, 20f, this.position.width, 20f), "Currently opened: " + this.lastOpenedHeapshotFile);
      GUI.Label(new Rect(100f, 40f, this.position.width, 20f), "Profiling: " + this.lastOpenedProfiler);
      this.DoActiveProfilerButton(new Rect(0.0f, 40f, 100f, 30f));
      if (GUI.Button(new Rect(0.0f, 70f, 200f, 20f), "CaptureHeapShot", EditorStyles.toolbarDropDown))
        ProfilerDriver.CaptureHeapshot();
      GUI.changed = false;
      this.selectedHeapshot = EditorGUI.Popup(new Rect(250f, 70f, 500f, 30f), "Click to open -->", this.selectedHeapshot, this.heapshotFiles.ToArray());
      if (GUI.changed && this.heapshotFiles[this.selectedHeapshot].Length > 0)
        this.OpenHeapshot(this.heapshotFiles[this.selectedHeapshot] + ".heapshot");
      GUILayout.BeginArea(new Rect(0.0f, 90f, this.position.width, 60f));
      SplitterGUILayout.BeginHorizontalSplit(this.viewSplit);
      GUILayout.BeginVertical();
      GUILayout.BeginHorizontal();
      string[] strArray = new string[2]{ "Roots", "All Objects" };
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (GUILayout.Toggle((this.currentTab == index ? 1 : 0) != 0, strArray[index], EditorStyles.toolbarButton, new GUILayoutOption[1]{ GUILayout.MaxHeight(16f) }))
          this.currentTab = index;
      }
      GUILayout.EndHorizontal();
      this.DoTitles(this.titleSplit1);
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      GUILayout.Label("Back trace references", EditorStyles.toolbarButton, new GUILayoutOption[1]
      {
        GUILayout.MaxHeight(16f)
      });
      this.DoTitles(this.titleSplit2);
      GUILayout.EndVertical();
      SplitterGUILayout.EndHorizontalSplit();
      GUILayout.EndArea();
      this.guiRect = new Rect(0.0f, 130f, (float) this.viewSplit.realSizes[0], 16f);
      float height1 = (float) this.GetItemCount(this.hsAllObjects) * 16f;
      Rect position = new Rect(this.guiRect.x, this.guiRect.y, this.guiRect.width, this.position.height - this.guiRect.y);
      this.leftViewScrollPosition = GUI.BeginScrollView(position, this.leftViewScrollPosition, new Rect(0.0f, 0.0f, position.width - 20f, height1));
      this.itemIndex = 0;
      this.guiRect.y = 0.0f;
      switch (this.currentTab)
      {
        case 0:
          this.DoHeapshotObjects(this.hsRoots, this.titleSplit1, 0, new HeapshotWindow.OnSelect(this.OnSelectObject));
          break;
        case 1:
          this.DoHeapshotObjects(this.hsAllObjects, this.titleSplit1, 0, new HeapshotWindow.OnSelect(this.OnSelectObject));
          break;
      }
      GUI.EndScrollView();
      this.guiRect = new Rect((float) this.viewSplit.realSizes[0], 130f, (float) this.viewSplit.realSizes[1], 16f);
      float height2 = (float) this.GetItemCount(this.hsBackTraceObjects) * 16f;
      position = new Rect(this.guiRect.x, this.guiRect.y, this.guiRect.width, this.position.height - this.guiRect.y);
      this.rightViewScrollPosition = GUI.BeginScrollView(position, this.rightViewScrollPosition, new Rect(0.0f, 0.0f, position.width - 20f, height2));
      if (this.hsBackTraceObjects.Count > 0)
      {
        this.guiRect.y = 0.0f;
        this.itemIndex = 0;
        this.DoHeapshotObjects(this.hsBackTraceObjects, this.titleSplit2, 0, (HeapshotWindow.OnSelect) null);
      }
      GUI.EndScrollView();
    }

    private void OnSelectObject(HeapshotWindow.HeapshotUIObject o)
    {
      this.hsBackTraceObjects.Clear();
      this.hsBackTraceObjects.Add(new HeapshotWindow.HeapshotUIObject(o.Name, o.ObjectInfo, true));
    }

    private void DoActiveProfilerButton(Rect position)
    {
      if (!EditorGUI.ButtonMouseDown(position, new GUIContent("Active Profler"), FocusType.Native, EditorStyles.toolbarDropDown))
        return;
      int connectedProfiler = ProfilerDriver.connectedProfiler;
      this.connectionGuids = ProfilerDriver.GetAvailableProfilers();
      int length = this.connectionGuids.Length;
      int[] selected = new int[1];
      bool[] enabled = new bool[length];
      string[] options = new string[length];
      for (int index = 0; index < length; ++index)
      {
        int connectionGuid = this.connectionGuids[index];
        bool flag = ProfilerDriver.IsIdentifierConnectable(connectionGuid);
        enabled[index] = flag;
        string connectionIdentifier = ProfilerDriver.GetConnectionIdentifier(connectionGuid);
        if (!flag)
          connectionIdentifier += " (Version mismatch)";
        options[index] = connectionIdentifier;
        if (connectionGuid == connectedProfiler)
          selected[0] = index;
      }
      EditorUtility.DisplayCustomMenu(position, options, enabled, selected, new EditorUtility.SelectMenuItemFunction(this.SelectProfilerClick), (object) null);
    }

    private void SelectProfilerClick(object userData, string[] options, int selected)
    {
      int connectionGuid = this.connectionGuids[selected];
      this.lastOpenedProfiler = ProfilerDriver.GetConnectionIdentifier(connectionGuid);
      ProfilerDriver.connectedProfiler = connectionGuid;
    }

    private void DoTitles(SplitterState splitter)
    {
      SplitterGUILayout.BeginHorizontalSplit(splitter);
      for (int index = 0; index < this.titleNames.Length; ++index)
        GUILayout.Toggle(0 != 0, this.titleNames[index], EditorStyles.toolbarButton, new GUILayoutOption[1]
        {
          GUILayout.MaxHeight(16f)
        });
      SplitterGUILayout.EndHorizontalSplit();
    }

    private void DoHeapshotObjects(List<HeapshotWindow.HeapshotUIObject> objects, SplitterState splitter, int indent, HeapshotWindow.OnSelect onSelect)
    {
      if (objects == null)
        return;
      Event current1 = Event.current;
      using (List<HeapshotWindow.HeapshotUIObject>.Enumerator enumerator = objects.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          HeapshotWindow.HeapshotUIObject current2 = enumerator.Current;
          Rect position = new Rect(14f * (float) indent, this.guiRect.y, 14f, this.guiRect.height);
          Rect[] rectArray = new Rect[this.titleNames.Length];
          float x = 14f * (float) (indent + 1);
          for (int index = 0; index < rectArray.Length; ++index)
          {
            float width = index != 0 ? (float) splitter.realSizes[index] : (float) splitter.realSizes[index] - x;
            rectArray[index] = new Rect(x, this.guiRect.y, width, this.guiRect.height);
            x += width;
          }
          if (current1.type == EventType.Repaint)
            ((this.itemIndex & 1) != 0 ? HeapshotWindow.Styles.entryOdd : HeapshotWindow.Styles.entryEven).Draw(new Rect(0.0f, 16f * (float) this.itemIndex, this.position.width, 16f), GUIContent.none, false, false, this.itemIndex == this.selectedItem, false);
          if (current2.HasChildren)
          {
            GUI.changed = false;
            bool flag = GUI.Toggle(position, current2.IsExpanded, GUIContent.none, HeapshotWindow.Styles.foldout);
            if (GUI.changed)
            {
              if (flag)
                current2.Expand();
              else
                current2.Collapse();
            }
          }
          GUI.changed = false;
          bool flag1 = GUI.Toggle(rectArray[0], this.itemIndex == this.selectedItem, current2.Name, HeapshotWindow.Styles.numberLabel);
          if (!current2.IsDummyObject)
          {
            GUI.Toggle(rectArray[1], this.itemIndex == this.selectedItem, current2.TypeName, HeapshotWindow.Styles.numberLabel);
            GUI.Toggle(rectArray[2], this.itemIndex == this.selectedItem, "0x" + current2.Code.ToString("X"), HeapshotWindow.Styles.numberLabel);
            GUI.Toggle(rectArray[3], this.itemIndex == this.selectedItem, current2.Size.ToString(), HeapshotWindow.Styles.numberLabel);
            GUI.Toggle(rectArray[4], this.itemIndex == this.selectedItem, string.Format("{0} / {1}", (object) current2.ReferenceCount, (object) current2.InverseReferenceCount), HeapshotWindow.Styles.numberLabel);
            if (GUI.changed && flag1 && onSelect != null)
            {
              this.selectedItem = this.itemIndex;
              onSelect(current2);
            }
          }
          ++this.itemIndex;
          this.guiRect.y += 16f;
          this.DoHeapshotObjects(current2.Children, splitter, indent + 1, onSelect);
        }
      }
    }

    public class HeapshotUIObject
    {
      private List<HeapshotWindow.HeapshotUIObject> children = new List<HeapshotWindow.HeapshotUIObject>();
      private string name;
      private HeapshotReader.ObjectInfo obj;
      private bool inverseReference;
      private bool isDummyObject;

      public bool HasChildren
      {
        get
        {
          if (this.inverseReference)
            return this.obj.inverseReferences.Count > 0;
          return this.obj.references.Count > 0;
        }
      }

      public bool IsExpanded
      {
        get
        {
          if (this.HasChildren)
            return this.children.Count > 0;
          return false;
        }
      }

      public string Name
      {
        get
        {
          return this.name;
        }
      }

      public uint Code
      {
        get
        {
          return this.obj.code;
        }
      }

      public uint Size
      {
        get
        {
          return this.obj.size;
        }
      }

      public int ReferenceCount
      {
        get
        {
          if (this.inverseReference)
            return this.obj.inverseReferences.Count;
          return this.obj.references.Count;
        }
      }

      public int InverseReferenceCount
      {
        get
        {
          if (this.inverseReference)
            return this.obj.references.Count;
          return this.obj.inverseReferences.Count;
        }
      }

      public bool IsDummyObject
      {
        get
        {
          return this.isDummyObject;
        }
        set
        {
          this.isDummyObject = value;
        }
      }

      public string TypeName
      {
        get
        {
          return this.obj.typeInfo.name;
        }
      }

      public HeapshotReader.ObjectInfo ObjectInfo
      {
        get
        {
          return this.obj;
        }
      }

      public List<HeapshotWindow.HeapshotUIObject> Children
      {
        get
        {
          if (this.HasChildren && this.IsExpanded)
            return this.children;
          return (List<HeapshotWindow.HeapshotUIObject>) null;
        }
      }

      public HeapshotUIObject(string name, HeapshotReader.ObjectInfo refObject, bool inverseReference)
      {
        this.name = name;
        this.obj = refObject;
        this.inverseReference = inverseReference;
      }

      public void Expand()
      {
        if (this.IsExpanded || !this.HasChildren)
          return;
        if (this.inverseReference)
        {
          using (List<HeapshotReader.BackReferenceInfo>.Enumerator enumerator = this.obj.inverseReferences.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              HeapshotReader.BackReferenceInfo current = enumerator.Current;
              this.children.Add(new HeapshotWindow.HeapshotUIObject(current.fieldInfo.name, current.parentObject, true));
            }
          }
        }
        else
        {
          using (List<HeapshotReader.ReferenceInfo>.Enumerator enumerator = this.obj.references.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              HeapshotReader.ReferenceInfo current = enumerator.Current;
              this.children.Add(new HeapshotWindow.HeapshotUIObject(current.fieldInfo.name, current.referencedObject, false));
            }
          }
        }
      }

      public void Collapse()
      {
        if (!this.IsExpanded)
          return;
        this.children.Clear();
      }
    }

    internal class UIStyles
    {
      public GUIStyle background = (GUIStyle) "OL Box";
      public GUIStyle header = (GUIStyle) "OL title";
      public GUIStyle rightHeader = (GUIStyle) "OL title TextRight";
      public GUIStyle entryEven = (GUIStyle) "OL EntryBackEven";
      public GUIStyle entryOdd = (GUIStyle) "OL EntryBackOdd";
      public GUIStyle numberLabel = (GUIStyle) "OL Label";
      public GUIStyle foldout = (GUIStyle) "IN foldout";
    }

    internal class UIOptions
    {
      public const float height = 16f;
      public const float foldoutWidth = 14f;
      public const float tabWidth = 50f;
    }

    private delegate void OnSelect(HeapshotWindow.HeapshotUIObject o);

    private delegate void DelegateReceivedHeapshot(string fileName);
  }
}

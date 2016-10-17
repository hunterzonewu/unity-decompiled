// Decompiled with JetBrains decompiler
// Type: UnityEditor.ConsoleWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Globalization;
using System.Text;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Console", useTypeNameAsIconName = true)]
  internal class ConsoleWindow : EditorWindow, IHasCustomMenu
  {
    private string m_ActiveText = string.Empty;
    private Vector2 m_TextScroll = Vector2.zero;
    private SplitterState spl = new SplitterState(new float[2]{ 70f, 30f }, new int[2]{ 32, 32 }, (int[]) null);
    private const int m_RowHeight = 32;
    private ListViewState m_ListView;
    private int m_ActiveInstanceID;
    private bool m_DevBuild;
    private static bool ms_LoadedIcons;
    internal static Texture2D iconInfo;
    internal static Texture2D iconWarn;
    internal static Texture2D iconError;
    internal static Texture2D iconInfoSmall;
    internal static Texture2D iconWarnSmall;
    internal static Texture2D iconErrorSmall;
    internal static Texture2D iconInfoMono;
    internal static Texture2D iconWarnMono;
    internal static Texture2D iconErrorMono;
    private int ms_LVHeight;
    private static ConsoleWindow ms_ConsoleWindow;

    public ConsoleWindow()
    {
      this.position = new Rect(200f, 200f, 800f, 400f);
      this.m_ListView = new ListViewState(0, 32);
    }

    private static void ShowConsoleWindowImmediate()
    {
      ConsoleWindow.ShowConsoleWindow(true);
    }

    public static void ShowConsoleWindow(bool immediate)
    {
      if ((UnityEngine.Object) ConsoleWindow.ms_ConsoleWindow == (UnityEngine.Object) null)
      {
        ConsoleWindow.ms_ConsoleWindow = ScriptableObject.CreateInstance<ConsoleWindow>();
        ConsoleWindow.ms_ConsoleWindow.Show(immediate);
        ConsoleWindow.ms_ConsoleWindow.Focus();
      }
      else
      {
        ConsoleWindow.ms_ConsoleWindow.Show(immediate);
        ConsoleWindow.ms_ConsoleWindow.Focus();
      }
    }

    internal static void LoadIcons()
    {
      if (ConsoleWindow.ms_LoadedIcons)
        return;
      ConsoleWindow.ms_LoadedIcons = true;
      ConsoleWindow.iconInfo = EditorGUIUtility.LoadIcon("console.infoicon");
      ConsoleWindow.iconWarn = EditorGUIUtility.LoadIcon("console.warnicon");
      ConsoleWindow.iconError = EditorGUIUtility.LoadIcon("console.erroricon");
      ConsoleWindow.iconInfoSmall = EditorGUIUtility.LoadIcon("console.infoicon.sml");
      ConsoleWindow.iconWarnSmall = EditorGUIUtility.LoadIcon("console.warnicon.sml");
      ConsoleWindow.iconErrorSmall = EditorGUIUtility.LoadIcon("console.erroricon.sml");
      ConsoleWindow.iconInfoMono = EditorGUIUtility.LoadIcon("console.infoicon.sml");
      ConsoleWindow.iconWarnMono = EditorGUIUtility.LoadIcon("console.warnicon.inactive.sml");
      ConsoleWindow.iconErrorMono = EditorGUIUtility.LoadIcon("console.erroricon.inactive.sml");
      ConsoleWindow.Constants.Init();
    }

    [RequiredByNativeCode]
    public static void LogChanged()
    {
      if ((UnityEngine.Object) ConsoleWindow.ms_ConsoleWindow == (UnityEngine.Object) null)
        return;
      ConsoleWindow.ms_ConsoleWindow.DoLogChanged();
    }

    public void DoLogChanged()
    {
      ConsoleWindow.ms_ConsoleWindow.Repaint();
    }

    private void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      ConsoleWindow.ms_ConsoleWindow = this;
      this.m_DevBuild = Unsupported.IsDeveloperBuild();
    }

    private void OnDisable()
    {
      if (!((UnityEngine.Object) ConsoleWindow.ms_ConsoleWindow == (UnityEngine.Object) this))
        return;
      ConsoleWindow.ms_ConsoleWindow = (ConsoleWindow) null;
    }

    private static bool HasMode(int mode, ConsoleWindow.Mode modeToCheck)
    {
      return ((ConsoleWindow.Mode) mode & modeToCheck) != (ConsoleWindow.Mode) 0;
    }

    private bool HasFlag(ConsoleWindow.ConsoleFlags flags)
    {
      return ((ConsoleWindow.ConsoleFlags) LogEntries.consoleFlags & flags) != (ConsoleWindow.ConsoleFlags) 0;
    }

    private void SetFlag(ConsoleWindow.ConsoleFlags flags, bool val)
    {
      LogEntries.SetConsoleFlag((int) flags, val);
    }

    internal static Texture2D GetIconForErrorMode(int mode, bool large)
    {
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.Error | ConsoleWindow.Mode.Assert | ConsoleWindow.Mode.Fatal | ConsoleWindow.Mode.AssetImportError | ConsoleWindow.Mode.ScriptingError | ConsoleWindow.Mode.ScriptCompileError | ConsoleWindow.Mode.GraphCompileError | ConsoleWindow.Mode.ScriptingAssertion))
      {
        if (large)
          return ConsoleWindow.iconError;
        return ConsoleWindow.iconErrorSmall;
      }
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.AssetImportWarning | ConsoleWindow.Mode.ScriptingWarning | ConsoleWindow.Mode.ScriptCompileWarning))
      {
        if (large)
          return ConsoleWindow.iconWarn;
        return ConsoleWindow.iconWarnSmall;
      }
      if (!ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.Log | ConsoleWindow.Mode.ScriptingLog))
        return (Texture2D) null;
      if (large)
        return ConsoleWindow.iconInfo;
      return ConsoleWindow.iconInfoSmall;
    }

    internal static GUIStyle GetStyleForErrorMode(int mode)
    {
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.Error | ConsoleWindow.Mode.Assert | ConsoleWindow.Mode.Fatal | ConsoleWindow.Mode.AssetImportError | ConsoleWindow.Mode.ScriptingError | ConsoleWindow.Mode.ScriptCompileError | ConsoleWindow.Mode.GraphCompileError | ConsoleWindow.Mode.ScriptingAssertion))
        return ConsoleWindow.Constants.ErrorStyle;
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.AssetImportWarning | ConsoleWindow.Mode.ScriptingWarning | ConsoleWindow.Mode.ScriptCompileWarning))
        return ConsoleWindow.Constants.WarningStyle;
      return ConsoleWindow.Constants.LogStyle;
    }

    internal static GUIStyle GetStatusStyleForErrorMode(int mode)
    {
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.Error | ConsoleWindow.Mode.Assert | ConsoleWindow.Mode.Fatal | ConsoleWindow.Mode.AssetImportError | ConsoleWindow.Mode.ScriptingError | ConsoleWindow.Mode.ScriptCompileError | ConsoleWindow.Mode.GraphCompileError | ConsoleWindow.Mode.ScriptingAssertion))
        return ConsoleWindow.Constants.StatusError;
      if (ConsoleWindow.HasMode(mode, ConsoleWindow.Mode.AssetImportWarning | ConsoleWindow.Mode.ScriptingWarning | ConsoleWindow.Mode.ScriptCompileWarning))
        return ConsoleWindow.Constants.StatusWarn;
      return ConsoleWindow.Constants.StatusLog;
    }

    private static string ContextString(LogEntry entry)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (ConsoleWindow.HasMode(entry.mode, ConsoleWindow.Mode.Error))
        stringBuilder.Append("Error ");
      else if (ConsoleWindow.HasMode(entry.mode, ConsoleWindow.Mode.Log))
        stringBuilder.Append("Log ");
      else
        stringBuilder.Append("Assert ");
      stringBuilder.Append("in file: ");
      stringBuilder.Append(entry.file);
      stringBuilder.Append(" at line: ");
      stringBuilder.Append(entry.line);
      if (entry.errorNum != 0)
      {
        stringBuilder.Append(" and errorNum: ");
        stringBuilder.Append(entry.errorNum);
      }
      return stringBuilder.ToString();
    }

    private static string GetFirstLine(string s)
    {
      int length = s.IndexOf("\n");
      if (length != -1)
        return s.Substring(0, length);
      return s;
    }

    private static string GetFirstTwoLines(string s)
    {
      int num = s.IndexOf("\n");
      if (num != -1)
      {
        int length = s.IndexOf("\n", num + 1);
        if (length != -1)
          return s.Substring(0, length);
      }
      return s;
    }

    private void SetActiveEntry(LogEntry entry)
    {
      if (entry != null)
      {
        this.m_ActiveText = entry.condition;
        if (this.m_ActiveInstanceID == entry.instanceID)
          return;
        this.m_ActiveInstanceID = entry.instanceID;
        if (entry.instanceID == 0)
          return;
        EditorGUIUtility.PingObject(entry.instanceID);
      }
      else
      {
        this.m_ActiveText = string.Empty;
        this.m_ActiveInstanceID = 0;
        this.m_ListView.row = -1;
      }
    }

    private static void ShowConsoleRow(int row)
    {
      ConsoleWindow.ShowConsoleWindow(false);
      if (!(bool) ((UnityEngine.Object) ConsoleWindow.ms_ConsoleWindow))
        return;
      ConsoleWindow.ms_ConsoleWindow.m_ListView.row = row;
      ConsoleWindow.ms_ConsoleWindow.m_ListView.selectionChanged = true;
      ConsoleWindow.ms_ConsoleWindow.Repaint();
    }

    private void OnGUI()
    {
      Event current = Event.current;
      ConsoleWindow.LoadIcons();
      GUILayout.BeginHorizontal(ConsoleWindow.Constants.Toolbar, new GUILayoutOption[0]);
      if (GUILayout.Button("Clear", ConsoleWindow.Constants.MiniButton, new GUILayoutOption[0]))
      {
        LogEntries.Clear();
        GUIUtility.keyboardControl = 0;
      }
      int count = LogEntries.GetCount();
      if (this.m_ListView.totalRows != count && (double) this.m_ListView.scrollPos.y >= (double) (this.m_ListView.rowHeight * this.m_ListView.totalRows - this.ms_LVHeight))
        this.m_ListView.scrollPos.y = (float) (count * 32 - this.ms_LVHeight);
      EditorGUILayout.Space();
      bool flag1 = this.HasFlag(ConsoleWindow.ConsoleFlags.Collapse);
      this.SetFlag(ConsoleWindow.ConsoleFlags.Collapse, GUILayout.Toggle(flag1, "Collapse", ConsoleWindow.Constants.MiniButtonLeft, new GUILayoutOption[0]));
      if (flag1 != this.HasFlag(ConsoleWindow.ConsoleFlags.Collapse))
      {
        this.m_ListView.row = -1;
        this.m_ListView.scrollPos.y = (float) (LogEntries.GetCount() * 32);
      }
      this.SetFlag(ConsoleWindow.ConsoleFlags.ClearOnPlay, GUILayout.Toggle(this.HasFlag(ConsoleWindow.ConsoleFlags.ClearOnPlay), "Clear on Play", ConsoleWindow.Constants.MiniButtonMiddle, new GUILayoutOption[0]));
      this.SetFlag(ConsoleWindow.ConsoleFlags.ErrorPause, GUILayout.Toggle(this.HasFlag(ConsoleWindow.ConsoleFlags.ErrorPause), "Error Pause", ConsoleWindow.Constants.MiniButtonRight, new GUILayoutOption[0]));
      EditorGUILayout.Space();
      if (this.m_DevBuild)
      {
        GUILayout.FlexibleSpace();
        this.SetFlag(ConsoleWindow.ConsoleFlags.StopForAssert, GUILayout.Toggle(this.HasFlag(ConsoleWindow.ConsoleFlags.StopForAssert), "Stop for Assert", ConsoleWindow.Constants.MiniButtonLeft, new GUILayoutOption[0]));
        this.SetFlag(ConsoleWindow.ConsoleFlags.StopForError, GUILayout.Toggle(this.HasFlag(ConsoleWindow.ConsoleFlags.StopForError), "Stop for Error", ConsoleWindow.Constants.MiniButtonRight, new GUILayoutOption[0]));
      }
      GUILayout.FlexibleSpace();
      int errorCount = 0;
      int warningCount = 0;
      int logCount = 0;
      LogEntries.GetCountsByType(ref errorCount, ref warningCount, ref logCount);
      bool val1 = GUILayout.Toggle(this.HasFlag(ConsoleWindow.ConsoleFlags.LogLevelLog), new GUIContent(logCount > 999 ? "999+" : logCount.ToString(), logCount <= 0 ? (Texture) ConsoleWindow.iconInfoMono : (Texture) ConsoleWindow.iconInfoSmall), ConsoleWindow.Constants.MiniButtonRight, new GUILayoutOption[0]);
      bool val2 = GUILayout.Toggle(this.HasFlag(ConsoleWindow.ConsoleFlags.LogLevelWarning), new GUIContent(warningCount > 999 ? "999+" : warningCount.ToString(), warningCount <= 0 ? (Texture) ConsoleWindow.iconWarnMono : (Texture) ConsoleWindow.iconWarnSmall), ConsoleWindow.Constants.MiniButtonMiddle, new GUILayoutOption[0]);
      bool val3 = GUILayout.Toggle(this.HasFlag(ConsoleWindow.ConsoleFlags.LogLevelError), new GUIContent(errorCount > 999 ? "999+" : errorCount.ToString(), errorCount <= 0 ? (Texture) ConsoleWindow.iconErrorMono : (Texture) ConsoleWindow.iconErrorSmall), ConsoleWindow.Constants.MiniButtonLeft, new GUILayoutOption[0]);
      this.SetFlag(ConsoleWindow.ConsoleFlags.LogLevelLog, val1);
      this.SetFlag(ConsoleWindow.ConsoleFlags.LogLevelWarning, val2);
      this.SetFlag(ConsoleWindow.ConsoleFlags.LogLevelError, val3);
      GUILayout.EndHorizontal();
      this.m_ListView.totalRows = LogEntries.StartGettingEntries();
      SplitterGUILayout.BeginVerticalSplit(this.spl);
      EditorGUIUtility.SetIconSize(new Vector2(32f, 32f));
      GUIContent content = new GUIContent();
      int controlId = GUIUtility.GetControlID(FocusType.Native);
      try
      {
        bool flag2 = false;
        bool flag3 = this.HasFlag(ConsoleWindow.ConsoleFlags.Collapse);
        foreach (ListViewElement listViewElement in ListViewGUI.ListView(this.m_ListView, ConsoleWindow.Constants.Box, new GUILayoutOption[0]))
        {
          if (current.type == EventType.MouseDown && current.button == 0 && listViewElement.position.Contains(current.mousePosition))
          {
            if (current.clickCount == 2)
              LogEntries.RowGotDoubleClicked(this.m_ListView.row);
            flag2 = true;
          }
          if (current.type == EventType.Repaint)
          {
            int mask = 0;
            string outString = (string) null;
            LogEntries.GetFirstTwoLinesEntryTextAndModeInternal(listViewElement.row, ref mask, ref outString);
            (listViewElement.row % 2 != 0 ? ConsoleWindow.Constants.EvenBackground : ConsoleWindow.Constants.OddBackground).Draw(listViewElement.position, false, false, this.m_ListView.row == listViewElement.row, false);
            content.text = outString;
            ConsoleWindow.GetStyleForErrorMode(mask).Draw(listViewElement.position, content, controlId, this.m_ListView.row == listViewElement.row);
            if (flag3)
            {
              Rect position = listViewElement.position;
              content.text = LogEntries.GetEntryCount(listViewElement.row).ToString((IFormatProvider) CultureInfo.InvariantCulture);
              Vector2 vector2 = ConsoleWindow.Constants.CountBadge.CalcSize(content);
              position.xMin = position.xMax - vector2.x;
              position.yMin += (float) (((double) position.yMax - (double) position.yMin - (double) vector2.y) * 0.5);
              position.x -= 5f;
              GUI.Label(position, content, ConsoleWindow.Constants.CountBadge);
            }
          }
        }
        if (flag2 && (double) this.m_ListView.scrollPos.y >= (double) (this.m_ListView.rowHeight * this.m_ListView.totalRows - this.ms_LVHeight))
          this.m_ListView.scrollPos.y = (float) (this.m_ListView.rowHeight * this.m_ListView.totalRows - this.ms_LVHeight - 1);
        if (this.m_ListView.totalRows == 0 || this.m_ListView.row >= this.m_ListView.totalRows || this.m_ListView.row < 0)
        {
          if (this.m_ActiveText.Length != 0)
            this.SetActiveEntry((LogEntry) null);
        }
        else
        {
          LogEntry logEntry = new LogEntry();
          LogEntries.GetEntryInternal(this.m_ListView.row, logEntry);
          this.SetActiveEntry(logEntry);
          LogEntries.GetEntryInternal(this.m_ListView.row, logEntry);
          if (this.m_ListView.selectionChanged || !this.m_ActiveText.Equals(logEntry.condition))
            this.SetActiveEntry(logEntry);
        }
        if (GUIUtility.keyboardControl == this.m_ListView.ID && current.type == EventType.KeyDown && (current.keyCode == KeyCode.Return && this.m_ListView.row != 0))
        {
          LogEntries.RowGotDoubleClicked(this.m_ListView.row);
          Event.current.Use();
        }
        if (current.type != EventType.Layout)
        {
          if (ListViewGUI.ilvState.rectHeight != 1)
            this.ms_LVHeight = ListViewGUI.ilvState.rectHeight;
        }
      }
      finally
      {
        LogEntries.EndGettingEntries();
        EditorGUIUtility.SetIconSize(Vector2.zero);
      }
      this.m_TextScroll = GUILayout.BeginScrollView(this.m_TextScroll, ConsoleWindow.Constants.Box);
      float minHeight = ConsoleWindow.Constants.MessageStyle.CalcHeight(GUIContent.Temp(this.m_ActiveText), this.position.width);
      EditorGUILayout.SelectableLabel(this.m_ActiveText, ConsoleWindow.Constants.MessageStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MinHeight(minHeight));
      GUILayout.EndScrollView();
      SplitterGUILayout.EndVerticalSplit();
      if (current.type != EventType.ValidateCommand && current.type != EventType.ExecuteCommand || (!(current.commandName == "Copy") || !(this.m_ActiveText != string.Empty)))
        return;
      if (current.type == EventType.ExecuteCommand)
        EditorGUIUtility.systemCopyBuffer = this.m_ActiveText;
      current.Use();
    }

    public void ToggleLogStackTraces(object userData)
    {
      Application.stackTraceLogType = (StackTraceLogType) userData;
    }

    public void AddItemsToMenu(GenericMenu menu)
    {
      if (Application.platform == RuntimePlatform.OSXEditor)
        menu.AddItem(new GUIContent("Open Player Log"), false, new GenericMenu.MenuFunction(InternalEditorUtility.OpenPlayerConsole));
      menu.AddItem(new GUIContent("Open Editor Log"), false, new GenericMenu.MenuFunction(InternalEditorUtility.OpenEditorConsole));
      foreach (int num in Enum.GetValues(typeof (StackTraceLogType)))
      {
        StackTraceLogType stackTraceLogType = (StackTraceLogType) num;
        menu.AddItem(new GUIContent("Stack Trace Logging/" + (object) stackTraceLogType), Application.stackTraceLogType == stackTraceLogType, new GenericMenu.MenuFunction2(this.ToggleLogStackTraces), (object) stackTraceLogType);
      }
    }

    internal class Constants
    {
      public static bool ms_Loaded;
      public static GUIStyle Box;
      public static GUIStyle Button;
      public static GUIStyle MiniButton;
      public static GUIStyle MiniButtonLeft;
      public static GUIStyle MiniButtonMiddle;
      public static GUIStyle MiniButtonRight;
      public static GUIStyle LogStyle;
      public static GUIStyle WarningStyle;
      public static GUIStyle ErrorStyle;
      public static GUIStyle EvenBackground;
      public static GUIStyle OddBackground;
      public static GUIStyle MessageStyle;
      public static GUIStyle StatusError;
      public static GUIStyle StatusWarn;
      public static GUIStyle StatusLog;
      public static GUIStyle Toolbar;
      public static GUIStyle CountBadge;

      public static void Init()
      {
        if (ConsoleWindow.Constants.ms_Loaded)
          return;
        ConsoleWindow.Constants.ms_Loaded = true;
        ConsoleWindow.Constants.Box = (GUIStyle) "CN Box";
        ConsoleWindow.Constants.Button = (GUIStyle) "Button";
        ConsoleWindow.Constants.MiniButton = (GUIStyle) "ToolbarButton";
        ConsoleWindow.Constants.MiniButtonLeft = (GUIStyle) "ToolbarButton";
        ConsoleWindow.Constants.MiniButtonMiddle = (GUIStyle) "ToolbarButton";
        ConsoleWindow.Constants.MiniButtonRight = (GUIStyle) "ToolbarButton";
        ConsoleWindow.Constants.Toolbar = (GUIStyle) "Toolbar";
        ConsoleWindow.Constants.LogStyle = (GUIStyle) "CN EntryInfo";
        ConsoleWindow.Constants.WarningStyle = (GUIStyle) "CN EntryWarn";
        ConsoleWindow.Constants.ErrorStyle = (GUIStyle) "CN EntryError";
        ConsoleWindow.Constants.EvenBackground = (GUIStyle) "CN EntryBackEven";
        ConsoleWindow.Constants.OddBackground = (GUIStyle) "CN EntryBackodd";
        ConsoleWindow.Constants.MessageStyle = (GUIStyle) "CN Message";
        ConsoleWindow.Constants.StatusError = (GUIStyle) "CN StatusError";
        ConsoleWindow.Constants.StatusWarn = (GUIStyle) "CN StatusWarn";
        ConsoleWindow.Constants.StatusLog = (GUIStyle) "CN StatusInfo";
        ConsoleWindow.Constants.CountBadge = (GUIStyle) "CN CountBadge";
      }
    }

    private enum Mode
    {
      Error = 1,
      Assert = 2,
      Log = 4,
      Fatal = 16,
      DontPreprocessCondition = 32,
      AssetImportError = 64,
      AssetImportWarning = 128,
      ScriptingError = 256,
      ScriptingWarning = 512,
      ScriptingLog = 1024,
      ScriptCompileError = 2048,
      ScriptCompileWarning = 4096,
      StickyError = 8192,
      MayIgnoreLineNumber = 16384,
      ReportBug = 32768,
      DisplayPreviousErrorInStatusBar = 65536,
      ScriptingException = 131072,
      DontExtractStacktrace = 262144,
      ShouldClearOnPlay = 524288,
      GraphCompileError = 1048576,
      ScriptingAssertion = 2097152,
    }

    private enum ConsoleFlags
    {
      Collapse = 1,
      ClearOnPlay = 2,
      ErrorPause = 4,
      Verbose = 8,
      StopForAssert = 16,
      StopForError = 32,
      Autoscroll = 64,
      LogLevelLog = 128,
      LogLevelWarning = 256,
      LogLevelError = 512,
    }
  }
}

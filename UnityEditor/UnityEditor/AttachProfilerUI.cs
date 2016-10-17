// Decompiled with JetBrains decompiler
// Type: UnityEditor.AttachProfilerUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Hardware;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AttachProfilerUI
  {
    private static string kEnterIPText = "<Enter IP>";
    private const int PLAYER_DIRECT_IP_CONNECT_GUID = 65261;
    private const int PLAYER_DIRECT_URL_CONNECT_GUID = 65262;
    private GUIContent m_CurrentProfiler;
    private static GUIContent ms_NotificationMessage;

    private void SelectProfilerClick(object userData, string[] options, int selected)
    {
      List<ProfilerChoise> source = (List<ProfilerChoise>) userData;
      if (selected >= source.Count<ProfilerChoise>())
        return;
      source[selected].ConnectTo();
    }

    public bool IsEditor()
    {
      return ProfilerDriver.IsConnectionEditor();
    }

    public string GetConnectedProfiler()
    {
      return ProfilerDriver.GetConnectionIdentifier(ProfilerDriver.connectedProfiler);
    }

    public static void DirectIPConnect(string ip)
    {
      ConsoleWindow.ShowConsoleWindow(true);
      AttachProfilerUI.ms_NotificationMessage = new GUIContent("Connecting to player...(this can take a while)");
      ProfilerDriver.DirectIPConnect(ip);
      AttachProfilerUI.ms_NotificationMessage = (GUIContent) null;
    }

    public static void DirectURLConnect(string url)
    {
      ConsoleWindow.ShowConsoleWindow(true);
      AttachProfilerUI.ms_NotificationMessage = new GUIContent("Connecting to player...(this can take a while)");
      ProfilerDriver.DirectURLConnect(url);
      AttachProfilerUI.ms_NotificationMessage = (GUIContent) null;
    }

    public void OnGUILayout(EditorWindow window)
    {
      if (this.m_CurrentProfiler == null)
        this.m_CurrentProfiler = EditorGUIUtility.TextContent("Active Profiler|Select connected player to profile");
      this.OnGUI(GUILayoutUtility.GetRect(this.m_CurrentProfiler, EditorStyles.toolbarDropDown, new GUILayoutOption[1]
      {
        GUILayout.Width(100f)
      }), this.m_CurrentProfiler);
      if (AttachProfilerUI.ms_NotificationMessage != null)
        window.ShowNotification(AttachProfilerUI.ms_NotificationMessage);
      else
        window.RemoveNotification();
    }

    private static void AddLastIPProfiler(List<ProfilerChoise> profilers)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AttachProfilerUI.\u003CAddLastIPProfiler\u003Ec__AnonStoreyA7 profilerCAnonStoreyA7 = new AttachProfilerUI.\u003CAddLastIPProfiler\u003Ec__AnonStoreyA7();
      // ISSUE: reference to a compiler-generated field
      profilerCAnonStoreyA7.lastIP = ProfilerIPWindow.GetLastIPString();
      // ISSUE: reference to a compiler-generated field
      if (string.IsNullOrEmpty(profilerCAnonStoreyA7.lastIP))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      profilers.Add(new ProfilerChoise()
      {
        Name = profilerCAnonStoreyA7.lastIP,
        Enabled = true,
        IsSelected = (Func<bool>) (() => ProfilerDriver.connectedProfiler == 65261),
        ConnectTo = new System.Action(profilerCAnonStoreyA7.\u003C\u003Em__1EA)
      });
    }

    private static void AddPlayerProfilers(List<ProfilerChoise> profilers)
    {
      foreach (int availableProfiler in ProfilerDriver.GetAvailableProfilers())
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AttachProfilerUI.\u003CAddPlayerProfilers\u003Ec__AnonStoreyA8 profilersCAnonStoreyA8 = new AttachProfilerUI.\u003CAddPlayerProfilers\u003Ec__AnonStoreyA8();
        // ISSUE: reference to a compiler-generated field
        profilersCAnonStoreyA8.guid = availableProfiler;
        // ISSUE: reference to a compiler-generated field
        string str = ProfilerDriver.GetConnectionIdentifier(profilersCAnonStoreyA8.guid);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = ProfilerDriver.IsIdentifierOnLocalhost(profilersCAnonStoreyA8.guid) && str.Contains("MetroPlayerX");
        // ISSUE: reference to a compiler-generated field
        bool flag2 = !flag1 && ProfilerDriver.IsIdentifierConnectable(profilersCAnonStoreyA8.guid);
        if (!flag2)
          str = !flag1 ? str + " (Version mismatch)" : str + " (Localhost prohibited)";
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        profilers.Add(new ProfilerChoise()
        {
          Name = str,
          Enabled = flag2,
          IsSelected = new Func<bool>(profilersCAnonStoreyA8.\u003C\u003Em__1EB),
          ConnectTo = new System.Action(profilersCAnonStoreyA8.\u003C\u003Em__1EC)
        });
      }
    }

    private static void AddDeviceProfilers(List<ProfilerChoise> profilers)
    {
      foreach (DevDevice device in DevDeviceList.GetDevices())
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AttachProfilerUI.\u003CAddDeviceProfilers\u003Ec__AnonStoreyA9 profilersCAnonStoreyA9 = new AttachProfilerUI.\u003CAddDeviceProfilers\u003Ec__AnonStoreyA9();
        bool flag = (device.features & DevDeviceFeatures.PlayerConnection) != DevDeviceFeatures.None;
        if (device.isConnected && flag)
        {
          // ISSUE: reference to a compiler-generated field
          profilersCAnonStoreyA9.url = "device://" + device.id;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          profilers.Add(new ProfilerChoise()
          {
            Name = device.name,
            Enabled = true,
            IsSelected = new Func<bool>(profilersCAnonStoreyA9.\u003C\u003Em__1ED),
            ConnectTo = new System.Action(profilersCAnonStoreyA9.\u003C\u003Em__1EE)
          });
        }
      }
    }

    private void AddEnterIPProfiler(List<ProfilerChoise> profilers, Rect buttonScreenRect)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      profilers.Add(new ProfilerChoise()
      {
        Name = AttachProfilerUI.kEnterIPText,
        Enabled = true,
        IsSelected = (Func<bool>) (() => false),
        ConnectTo = new System.Action(new AttachProfilerUI.\u003CAddEnterIPProfiler\u003Ec__AnonStoreyAA()
        {
          buttonScreenRect = buttonScreenRect
        }.\u003C\u003Em__1F0)
      });
    }

    public void OnGUI(Rect connectRect, GUIContent profilerLabel)
    {
      if (!EditorGUI.ButtonMouseDown(connectRect, profilerLabel, FocusType.Native, EditorStyles.toolbarDropDown))
        return;
      List<ProfilerChoise> profilerChoiseList = new List<ProfilerChoise>();
      profilerChoiseList.Clear();
      AttachProfilerUI.AddPlayerProfilers(profilerChoiseList);
      AttachProfilerUI.AddDeviceProfilers(profilerChoiseList);
      AttachProfilerUI.AddLastIPProfiler(profilerChoiseList);
      this.AddEnterIPProfiler(profilerChoiseList, GUIUtility.GUIToScreenRect(connectRect));
      string[] array1 = profilerChoiseList.Select<ProfilerChoise, string>((Func<ProfilerChoise, string>) (p => p.Name)).ToArray<string>();
      bool[] array2 = profilerChoiseList.Select<ProfilerChoise, bool>((Func<ProfilerChoise, bool>) (p => p.Enabled)).ToArray<bool>();
      int index = profilerChoiseList.FindIndex((Predicate<ProfilerChoise>) (p => p.IsSelected()));
      int[] selected;
      if (index == -1)
        selected = new int[0];
      else
        selected = new int[1]{ index };
      EditorUtility.DisplayCustomMenu(connectRect, array1, array2, selected, new EditorUtility.SelectMenuItemFunction(this.SelectProfilerClick), (object) profilerChoiseList);
    }
  }
}

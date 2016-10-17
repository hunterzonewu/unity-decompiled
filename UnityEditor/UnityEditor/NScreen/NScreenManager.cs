// Decompiled with JetBrains decompiler
// Type: UnityEditor.NScreen.NScreenManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace UnityEditor.NScreen
{
  internal class NScreenManager : ScriptableSingleton<NScreenManager>
  {
    [SerializeField]
    private bool m_BuildOnPlay = true;
    [SerializeField]
    private int m_LatestId;
    [SerializeField]
    private int m_SelectedSizeIndex;

    internal bool BuildOnPlay
    {
      get
      {
        if (!this.m_BuildOnPlay)
          return !this.HasBuild;
        return true;
      }
      set
      {
        this.m_BuildOnPlay = value;
      }
    }

    internal bool HasBuild
    {
      get
      {
        return Directory.Exists("Temp/NScreen/NScreen.app");
      }
    }

    internal int SelectedSizeIndex
    {
      get
      {
        return this.m_SelectedSizeIndex;
      }
      set
      {
        this.m_SelectedSizeIndex = value;
      }
    }

    static NScreenManager()
    {
      EditorApplication.playmodeStateChanged += new EditorApplication.CallbackFunction(NScreenManager.PlayModeStateChanged);
    }

    internal void ResetIds()
    {
      this.m_LatestId = 0;
    }

    internal int GetNewId()
    {
      return ++this.m_LatestId;
    }

    internal static void PlayModeStateChanged()
    {
      if (EditorApplication.isPaused)
        return;
      if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode && (Resources.FindObjectsOfTypeAll<RemoteGame>().Length > 0 && ScriptableSingleton<NScreenManager>.instance.BuildOnPlay))
        NScreenManager.Build();
      if (EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
        NScreenManager.StartAll();
      else if (EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
      {
        NScreenManager.StopAll();
      }
      else
      {
        if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
          return;
        NScreenManager.RepaintAllGameViews();
      }
    }

    internal static void Init()
    {
      RemoteGame window = (RemoteGame) EditorWindow.GetWindow(typeof (RemoteGame));
      if (!EditorApplication.isPlaying || window.IsRunning())
        return;
      window.id = ScriptableSingleton<NScreenManager>.instance.GetNewId();
      window.StartGame();
    }

    internal static void OpenAnotherWindow()
    {
      RemoteGame instance = ScriptableObject.CreateInstance<RemoteGame>();
      foreach (ContainerWindow window in ContainerWindow.windows)
      {
        foreach (View allChild in window.mainView.allChildren)
        {
          DockArea dockArea = allChild as DockArea;
          if (!((UnityEngine.Object) dockArea == (UnityEngine.Object) null) && dockArea.m_Panes.Any<EditorWindow>((Func<EditorWindow, bool>) (pane => pane.GetType() == typeof (RemoteGame))))
          {
            dockArea.AddTab((EditorWindow) instance);
            break;
          }
        }
      }
      instance.Show();
      if (!EditorApplication.isPlaying)
        return;
      instance.id = ScriptableSingleton<NScreenManager>.instance.GetNewId();
      instance.StartGame();
    }

    internal static void StartAll()
    {
      ScriptableSingleton<NScreenManager>.instance.ResetIds();
      foreach (RemoteGame remoteGame in Resources.FindObjectsOfTypeAll<RemoteGame>())
      {
        remoteGame.id = ScriptableSingleton<NScreenManager>.instance.GetNewId();
        remoteGame.StartGame();
      }
    }

    internal static void StopAll()
    {
      foreach (RemoteGame remoteGame in Resources.FindObjectsOfTypeAll<RemoteGame>())
        remoteGame.StopGame();
    }

    internal static void RepaintAllGameViews()
    {
      foreach (RemoteGame remoteGame in Resources.FindObjectsOfTypeAll<RemoteGame>())
      {
        remoteGame.Repaint();
        remoteGame.GameViewAspectWasChanged();
      }
    }

    internal static void Build()
    {
      string[] array = new string[EditorBuildSettings.scenes.Length];
      int newSize = 0;
      for (int index = 0; index < EditorBuildSettings.scenes.Length; ++index)
      {
        if (EditorBuildSettings.scenes[index].enabled)
        {
          array[newSize] = EditorBuildSettings.scenes[index].path;
          ++newSize;
        }
      }
      Array.Resize<string>(ref array, newSize);
      Directory.CreateDirectory("Temp/NScreen");
      ResolutionDialogSetting resolutionDialog = PlayerSettings.displayResolutionDialog;
      bool runInBackground = PlayerSettings.runInBackground;
      bool defaultIsFullScreen = PlayerSettings.defaultIsFullScreen;
      PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;
      PlayerSettings.runInBackground = true;
      PlayerSettings.defaultIsFullScreen = false;
      try
      {
        if (IntPtr.Size == 4)
          BuildPipeline.BuildPlayer(array, "Temp/NScreen/NScreen.app", BuildTarget.StandaloneOSXIntel, BuildOptions.None);
        else
          BuildPipeline.BuildPlayer(array, "Temp/NScreen/NScreen.app", BuildTarget.StandaloneOSXIntel64, BuildOptions.None);
      }
      finally
      {
        PlayerSettings.displayResolutionDialog = resolutionDialog;
        PlayerSettings.runInBackground = runInBackground;
        PlayerSettings.defaultIsFullScreen = defaultIsFullScreen;
      }
    }
  }
}

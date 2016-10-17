// Decompiled with JetBrains decompiler
// Type: UnityEditor.NScreen.RemoteGame
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;

namespace UnityEditor.NScreen
{
  internal class RemoteGame : EditorWindow, IHasCustomMenu
  {
    public bool shouldExit = true;
    private const float kMaxWidth = 1280f;
    private const float kMaxHeight = 720f;
    private Process remoteProcess;
    public NScreenBridge bridge;
    public bool shouldBuild;
    public int id;
    private int oldWidth;
    private int oldHeight;
    private Rect remoteViewRect;

    private int ToolBarHeight
    {
      get
      {
        return 17;
      }
    }

    internal void StartGame()
    {
      this.DoExitGame();
      this.wantsMouseMove = true;
      this.bridge = new NScreenBridge();
      this.bridge.InitServer(this.id);
      this.bridge.SetResolution((int) this.minSize.x, (int) this.minSize.y);
      this.remoteProcess = new Process();
      this.remoteProcess.EnableRaisingEvents = true;
      this.remoteProcess.Exited += new EventHandler(this.HandleExited);
      this.remoteProcess.StartInfo.FileName = "Temp/NScreen/NScreen.app/Contents/MacOS/NScreen";
      this.remoteProcess.StartInfo.Arguments = "-nscreenid " + (object) this.id;
      this.remoteProcess.StartInfo.UseShellExecute = false;
      try
      {
        this.remoteProcess.Start();
      }
      catch (Win32Exception ex)
      {
        this.remoteProcess = (Process) null;
        this.DoExitGame();
      }
      this.bridge.StartWatchdogForPid(this.remoteProcess.Id);
      this.shouldExit = false;
    }

    internal bool IsRunning()
    {
      return !this.shouldExit;
    }

    internal void StopGame()
    {
      this.shouldExit = true;
    }

    private void Update()
    {
      if (this.shouldExit)
        this.DoExitGame();
      else if ((UnityEngine.Object) this.bridge != (UnityEngine.Object) null)
      {
        if (this.oldWidth != (int) this.position.width || this.oldHeight != (int) this.position.height)
        {
          int num1 = (int) Mathf.Clamp(this.position.width, this.minSize.x, 1280f);
          int num2 = (int) Mathf.Clamp(this.position.height, this.minSize.y, 720f);
          bool fitsInsideRect = true;
          this.remoteViewRect = GameViewSizes.GetConstrainedRect(new Rect(0.0f, 0.0f, (float) num1, (float) num2), ScriptableSingleton<GameViewSizes>.instance.currentGroupType, ScriptableSingleton<NScreenManager>.instance.SelectedSizeIndex, out fitsInsideRect);
          this.remoteViewRect.y += (float) this.ToolBarHeight;
          this.remoteViewRect.height -= (float) this.ToolBarHeight;
          this.bridge.SetResolution((int) this.remoteViewRect.width, (int) this.remoteViewRect.height);
          this.oldWidth = (int) this.position.width;
          this.oldHeight = (int) this.position.height;
        }
        this.bridge.Update();
        this.Repaint();
      }
      if (!this.shouldBuild)
        return;
      this.shouldBuild = false;
      NScreenManager.Build();
    }

    private void SelectionCallback(int indexClicked, object objectSelected)
    {
      if (indexClicked == ScriptableSingleton<NScreenManager>.instance.SelectedSizeIndex)
        return;
      ScriptableSingleton<NScreenManager>.instance.SelectedSizeIndex = indexClicked;
      NScreenManager.RepaintAllGameViews();
    }

    internal void GameViewAspectWasChanged()
    {
      this.oldWidth = 0;
      this.oldHeight = 0;
    }

    private void OnGUI()
    {
      GUI.color = Color.white;
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      EditorGUILayout.GameViewSizePopup(ScriptableSingleton<GameViewSizes>.instance.currentGroupType, ScriptableSingleton<NScreenManager>.instance.SelectedSizeIndex, new System.Action<int, object>(this.SelectionCallback), EditorStyles.toolbarDropDown, GUILayout.Width(160f));
      GUILayout.FlexibleSpace();
      GUI.enabled = !Application.isPlaying;
      bool buildOnPlay = ScriptableSingleton<NScreenManager>.instance.BuildOnPlay;
      ScriptableSingleton<NScreenManager>.instance.BuildOnPlay = GUILayout.Toggle(ScriptableSingleton<NScreenManager>.instance.BuildOnPlay, "Build on Play", EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (buildOnPlay != ScriptableSingleton<NScreenManager>.instance.BuildOnPlay)
        NScreenManager.RepaintAllGameViews();
      if (GUILayout.Button("Build Now", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        this.shouldBuild = true;
      GUI.enabled = Application.isPlaying;
      GUILayout.EndHorizontal();
      if (!this.shouldExit && (UnityEngine.Object) this.bridge != (UnityEngine.Object) null)
      {
        Texture2D screenTexture = this.bridge.GetScreenTexture();
        if ((UnityEngine.Object) screenTexture != (UnityEngine.Object) null)
          GUI.DrawTexture(this.remoteViewRect, (Texture) screenTexture);
        if ((UnityEngine.Object) this == (UnityEngine.Object) EditorWindow.focusedWindow)
          this.bridge.SetInput((int) Event.current.mousePosition.x - (int) this.remoteViewRect.x, (int) this.position.height - (int) Event.current.mousePosition.y - (int) this.remoteViewRect.y + this.ToolBarHeight - (int) Mathf.Max(0.0f, this.position.height - 720f), Event.current.button, !Event.current.isKey ? -1 : (int) Event.current.keyCode, (int) Event.current.type);
        else
          this.bridge.ResetInput();
      }
      else
        GUILayout.Label("Game Stopped");
    }

    private void HandleExited(object sender, EventArgs e)
    {
      this.shouldExit = true;
    }

    internal void DoExitGame()
    {
      if (this.remoteProcess != null && !this.remoteProcess.HasExited)
      {
        this.remoteProcess.Kill();
        this.remoteProcess = (Process) null;
        this.Repaint();
      }
      if ((UnityEngine.Object) this.bridge != (UnityEngine.Object) null)
      {
        this.bridge.Shutdown();
        this.bridge = (NScreenBridge) null;
        this.oldWidth = 0;
        this.oldHeight = 0;
      }
      this.wantsMouseMove = false;
      this.shouldExit = true;
    }

    private void OnEnable()
    {
      this.titleContent = new GUIContent("Remote Game");
    }

    private void OnDestroy()
    {
      this.DoExitGame();
    }

    public void AddItemsToMenu(GenericMenu menu)
    {
      menu.AddItem(new GUIContent("Add Tab/Remote Game"), false, new GenericMenu.MenuFunction(NScreenManager.OpenAnotherWindow));
    }
  }
}

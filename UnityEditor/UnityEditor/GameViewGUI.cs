// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Text;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal class GameViewGUI
  {
    private static int m_FrameCounter;
    private static float m_ClientTimeAccumulator;
    private static float m_RenderTimeAccumulator;
    private static float m_MaxTimeAccumulator;
    private static float m_ClientFrameTime;
    private static float m_RenderFrameTime;
    private static float m_MaxFrameTime;
    private static GUIStyle s_SectionHeaderStyle;
    private static GUIStyle s_LabelStyle;

    private static GUIStyle sectionHeaderStyle
    {
      get
      {
        if (GameViewGUI.s_SectionHeaderStyle == null)
          GameViewGUI.s_SectionHeaderStyle = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle("BoldLabel");
        return GameViewGUI.s_SectionHeaderStyle;
      }
    }

    private static GUIStyle labelStyle
    {
      get
      {
        if (GameViewGUI.s_LabelStyle == null)
        {
          GameViewGUI.s_LabelStyle = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).label);
          GameViewGUI.s_LabelStyle.richText = true;
        }
        return GameViewGUI.s_LabelStyle;
      }
    }

    private static string FormatNumber(int num)
    {
      if (num < 1000)
        return num.ToString();
      if (num < 1000000)
        return ((double) num * 0.001).ToString("f1") + "k";
      return ((double) num * 1E-06).ToString("f1") + "M";
    }

    private static void UpdateFrameTime()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      float frameTime = UnityStats.frameTime;
      float renderTime = UnityStats.renderTime;
      GameViewGUI.m_ClientTimeAccumulator += frameTime;
      GameViewGUI.m_RenderTimeAccumulator += renderTime;
      GameViewGUI.m_MaxTimeAccumulator += Mathf.Max(frameTime, renderTime);
      ++GameViewGUI.m_FrameCounter;
      bool flag1 = (double) GameViewGUI.m_ClientFrameTime == 0.0 && (double) GameViewGUI.m_RenderFrameTime == 0.0;
      bool flag2 = GameViewGUI.m_FrameCounter > 30 || (double) GameViewGUI.m_ClientTimeAccumulator > 0.300000011920929 || (double) GameViewGUI.m_RenderTimeAccumulator > 0.300000011920929;
      if (flag1 || flag2)
      {
        GameViewGUI.m_ClientFrameTime = GameViewGUI.m_ClientTimeAccumulator / (float) GameViewGUI.m_FrameCounter;
        GameViewGUI.m_RenderFrameTime = GameViewGUI.m_RenderTimeAccumulator / (float) GameViewGUI.m_FrameCounter;
        GameViewGUI.m_MaxFrameTime = GameViewGUI.m_MaxTimeAccumulator / (float) GameViewGUI.m_FrameCounter;
      }
      if (!flag2)
        return;
      GameViewGUI.m_ClientTimeAccumulator = 0.0f;
      GameViewGUI.m_RenderTimeAccumulator = 0.0f;
      GameViewGUI.m_MaxTimeAccumulator = 0.0f;
      GameViewGUI.m_FrameCounter = 0;
    }

    private static string FormatDb(float vol)
    {
      if ((double) vol == 0.0)
        return "-∞ dB";
      return string.Format("{0:F1} dB", (object) (float) (20.0 * (double) Mathf.Log10(vol)));
    }

    [RequiredByNativeCode]
    public static void GameViewStatsGUI()
    {
      GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
      GUI.color = new Color(1f, 1f, 1f, 0.75f);
      float width = 300f;
      float height = 204f;
      int length = Network.connections.Length;
      if (length != 0)
        height += 220f;
      GUILayout.BeginArea(new Rect((float) ((double) GUIView.current.position.width - (double) width - 10.0), 27f, width, height), "Statistics", GUI.skin.window);
      GUILayout.Label("Audio:", GameViewGUI.sectionHeaderStyle, new GUILayoutOption[0]);
      StringBuilder stringBuilder1 = new StringBuilder(400);
      float audioLevel = UnityStats.audioLevel;
      stringBuilder1.Append("  Level: " + GameViewGUI.FormatDb(audioLevel) + (!EditorUtility.audioMasterMute ? "\n" : " (MUTED)\n"));
      stringBuilder1.Append(string.Format("  Clipping: {0:F1}%", (object) (float) (100.0 * (double) UnityStats.audioClippingAmount)));
      GUILayout.Label(stringBuilder1.ToString());
      GUI.Label(new Rect(170f, 40f, 120f, 20f), string.Format("DSP load: {0:F1}%", (object) (float) (100.0 * (double) UnityStats.audioDSPLoad)));
      GUI.Label(new Rect(170f, 53f, 120f, 20f), string.Format("Stream load: {0:F1}%", (object) (float) (100.0 * (double) UnityStats.audioStreamLoad)));
      GUILayout.Label("Graphics:", GameViewGUI.sectionHeaderStyle, new GUILayoutOption[0]);
      GameViewGUI.UpdateFrameTime();
      GUI.Label(new Rect(170f, 75f, 120f, 20f), string.Format("{0:F1} FPS ({1:F1}ms)", (object) (float) (1.0 / (double) Mathf.Max(GameViewGUI.m_MaxFrameTime, 1E-05f)), (object) (float) ((double) GameViewGUI.m_MaxFrameTime * 1000.0)));
      int screenBytes = UnityStats.screenBytes;
      int num1 = UnityStats.dynamicBatchedDrawCalls - UnityStats.dynamicBatches;
      int num2 = UnityStats.staticBatchedDrawCalls - UnityStats.staticBatches;
      StringBuilder stringBuilder2 = new StringBuilder(400);
      if ((double) GameViewGUI.m_ClientFrameTime > (double) GameViewGUI.m_RenderFrameTime)
        stringBuilder2.Append(string.Format("  CPU: main <b>{0:F1}</b>ms  render thread {1:F1}ms\n", (object) (float) ((double) GameViewGUI.m_ClientFrameTime * 1000.0), (object) (float) ((double) GameViewGUI.m_RenderFrameTime * 1000.0)));
      else
        stringBuilder2.Append(string.Format("  CPU: main {0:F1}ms  render thread <b>{1:F1}</b>ms\n", (object) (float) ((double) GameViewGUI.m_ClientFrameTime * 1000.0), (object) (float) ((double) GameViewGUI.m_RenderFrameTime * 1000.0)));
      stringBuilder2.Append(string.Format("  Batches: <b>{0}</b> \tSaved by batching: {1}\n", (object) UnityStats.batches, (object) (num1 + num2)));
      stringBuilder2.Append(string.Format("  Tris: {0} \tVerts: {1} \n", (object) GameViewGUI.FormatNumber(UnityStats.triangles), (object) GameViewGUI.FormatNumber(UnityStats.vertices)));
      stringBuilder2.Append(string.Format("  Screen: {0} - {1}\n", (object) UnityStats.screenRes, (object) EditorUtility.FormatBytes(screenBytes)));
      stringBuilder2.Append(string.Format("  SetPass calls: {0} \tShadow casters: {1} \n", (object) UnityStats.setPassCalls, (object) UnityStats.shadowCasters));
      stringBuilder2.Append(string.Format("  Visible skinned meshes: {0}  Animations: {1}", (object) UnityStats.visibleSkinnedMeshes, (object) UnityStats.visibleAnimations));
      GUILayout.Label(stringBuilder2.ToString(), GameViewGUI.labelStyle, new GUILayoutOption[0]);
      if (length != 0)
      {
        GUILayout.Label("Network:", GameViewGUI.sectionHeaderStyle, new GUILayoutOption[0]);
        GUILayout.BeginHorizontal();
        for (int i = 0; i < length; ++i)
          GUILayout.Label(UnityStats.GetNetworkStats(i));
        GUILayout.EndHorizontal();
      }
      else
        GUILayout.Label("Network: (no players connected)", GameViewGUI.sectionHeaderStyle, new GUILayoutOption[0]);
      GUILayout.EndArea();
    }
  }
}

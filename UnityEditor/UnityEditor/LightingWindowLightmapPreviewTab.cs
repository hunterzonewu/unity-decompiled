// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightingWindowLightmapPreviewTab
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using UnityEngine;

namespace UnityEditor
{
  internal class LightingWindowLightmapPreviewTab
  {
    private Vector2 m_ScrollPositionLightmaps = Vector2.zero;
    private Vector2 m_ScrollPositionMaps = Vector2.zero;
    private int m_SelectedLightmap = -1;
    private static LightingWindowLightmapPreviewTab.Styles s_Styles;

    private static void Header(ref Rect rect, float headerHeight, float headerLeftMargin, float maxLightmaps)
    {
      Rect rect1 = GUILayoutUtility.GetRect(rect.width, headerHeight);
      rect1.width = rect.width / maxLightmaps;
      rect1.y -= rect.height;
      rect.y += headerHeight;
      rect1.x += headerLeftMargin;
      EditorGUI.DropShadowLabel(rect1, "Intensity");
      rect1.x += rect1.width;
      EditorGUI.DropShadowLabel(rect1, "Directionality");
    }

    private void MenuSelectLightmapUsers(Rect rect, int lightmapIndex)
    {
      if (Event.current.type != EventType.ContextClick || !rect.Contains(Event.current.mousePosition))
        return;
      EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f), EditorGUIUtility.TempContent(new string[1]
      {
        "Select Lightmap Users"
      }), -1, new EditorUtility.SelectMenuItemFunction(this.SelectLightmapUsers), (object) lightmapIndex);
      Event.current.Use();
    }

    private void SelectLightmapUsers(object userData, string[] options, int selected)
    {
      int num = (int) userData;
      ArrayList arrayList = new ArrayList();
      foreach (MeshRenderer meshRenderer in UnityEngine.Object.FindObjectsOfType(typeof (MeshRenderer)) as MeshRenderer[])
      {
        if ((UnityEngine.Object) meshRenderer != (UnityEngine.Object) null && meshRenderer.lightmapIndex == num)
          arrayList.Add((object) meshRenderer.gameObject);
      }
      foreach (Terrain terrain in UnityEngine.Object.FindObjectsOfType(typeof (Terrain)) as Terrain[])
      {
        if ((UnityEngine.Object) terrain != (UnityEngine.Object) null && terrain.lightmapIndex == num)
          arrayList.Add((object) terrain.gameObject);
      }
      Selection.objects = arrayList.ToArray(typeof (UnityEngine.Object)) as UnityEngine.Object[];
    }

    public void LightmapPreview(Rect r)
    {
      if (LightingWindowLightmapPreviewTab.s_Styles == null)
        LightingWindowLightmapPreviewTab.s_Styles = new LightingWindowLightmapPreviewTab.Styles();
      bool flag = true;
      GUI.Box(r, string.Empty, (GUIStyle) "PreBackground");
      this.m_ScrollPositionLightmaps = EditorGUILayout.BeginScrollView(this.m_ScrollPositionLightmaps, GUILayout.Height(r.height));
      int lightmapIndex = 0;
      float maxLightmaps = 2f;
      foreach (LightmapData lightmap in LightmapSettings.lightmaps)
      {
        if ((UnityEngine.Object) lightmap.lightmapFar == (UnityEngine.Object) null && (UnityEngine.Object) lightmap.lightmapNear == (UnityEngine.Object) null)
        {
          ++lightmapIndex;
        }
        else
        {
          Texture2D texture2D = (!(bool) ((UnityEngine.Object) lightmap.lightmapFar) ? -1 : Math.Max(lightmap.lightmapFar.width, lightmap.lightmapFar.height)) <= (!(bool) ((UnityEngine.Object) lightmap.lightmapNear) ? -1 : Math.Max(lightmap.lightmapNear.width, lightmap.lightmapNear.height)) ? lightmap.lightmapNear : lightmap.lightmapFar;
          GUILayoutOption[] guiLayoutOptionArray = new GUILayoutOption[2]{ GUILayout.MaxWidth((float) texture2D.width * maxLightmaps), GUILayout.MaxHeight((float) texture2D.height) };
          Rect aspectRect = GUILayoutUtility.GetAspectRect((float) texture2D.width * maxLightmaps / (float) texture2D.height, guiLayoutOptionArray);
          if (flag)
          {
            LightingWindowLightmapPreviewTab.Header(ref aspectRect, 20f, 6f, maxLightmaps);
            flag = false;
          }
          aspectRect.width /= maxLightmaps;
          EditorGUI.DrawPreviewTexture(aspectRect, (Texture) lightmap.lightmapFar);
          this.MenuSelectLightmapUsers(aspectRect, lightmapIndex);
          if ((bool) ((UnityEngine.Object) lightmap.lightmapNear))
          {
            aspectRect.x += aspectRect.width;
            EditorGUI.DrawPreviewTexture(aspectRect, (Texture) lightmap.lightmapNear);
            this.MenuSelectLightmapUsers(aspectRect, lightmapIndex);
          }
          ++lightmapIndex;
        }
      }
      EditorGUILayout.EndScrollView();
    }

    public void UpdateLightmapSelection()
    {
      Terrain terrain = (Terrain) null;
      MeshRenderer component;
      if ((UnityEngine.Object) Selection.activeGameObject == (UnityEngine.Object) null || (UnityEngine.Object) (component = Selection.activeGameObject.GetComponent<MeshRenderer>()) == (UnityEngine.Object) null && (UnityEngine.Object) (terrain = Selection.activeGameObject.GetComponent<Terrain>()) == (UnityEngine.Object) null)
        this.m_SelectedLightmap = -1;
      else
        this.m_SelectedLightmap = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? terrain.lightmapIndex : component.lightmapIndex;
    }

    public void Maps()
    {
      if (LightingWindowLightmapPreviewTab.s_Styles == null)
        LightingWindowLightmapPreviewTab.s_Styles = new LightingWindowLightmapPreviewTab.Styles();
      GUI.changed = false;
      if (Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.OnDemand)
      {
        SerializedObject serializedObject = new SerializedObject(LightmapEditorSettings.GetLightmapSettings());
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LightingDataAsset"), LightingWindowLightmapPreviewTab.s_Styles.LightingDataAsset, new GUILayoutOption[0]);
        serializedObject.ApplyModifiedProperties();
      }
      GUILayout.Space(10f);
      LightmapData[] lightmaps = LightmapSettings.lightmaps;
      this.m_ScrollPositionMaps = GUILayout.BeginScrollView(this.m_ScrollPositionMaps);
      EditorGUI.BeginDisabledGroup(true);
      for (int index = 0; index < lightmaps.Length; ++index)
      {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(index.ToString());
        GUILayout.Space(5f);
        lightmaps[index].lightmapFar = this.LightmapField(lightmaps[index].lightmapFar, index);
        GUILayout.Space(10f);
        lightmaps[index].lightmapNear = this.LightmapField(lightmaps[index].lightmapNear, index);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
      }
      EditorGUI.EndDisabledGroup();
      GUILayout.EndScrollView();
    }

    private Texture2D LightmapField(Texture2D lightmap, int index)
    {
      Rect rect = GUILayoutUtility.GetRect(100f, 100f, EditorStyles.objectField);
      this.MenuSelectLightmapUsers(rect, index);
      Texture2D texture2D = EditorGUI.ObjectField(rect, (UnityEngine.Object) lightmap, typeof (Texture2D), false) as Texture2D;
      if (index == this.m_SelectedLightmap && Event.current.type == EventType.Repaint)
        LightingWindowLightmapPreviewTab.s_Styles.selectedLightmapHighlight.Draw(rect, false, false, false, false);
      return texture2D;
    }

    private class Styles
    {
      public GUIStyle selectedLightmapHighlight = (GUIStyle) "LightmapEditorSelectedHighlight";
      public GUIContent LightProbes = EditorGUIUtility.TextContent("Light Probes|A different LightProbes.asset can be assigned here. These assets are generated by baking a scene containing light probes.");
      public GUIContent LightingDataAsset = EditorGUIUtility.TextContent("Lighting Data Asset|A different LightingData.asset can be assigned here. These assets are generated by baking a scene in the OnDemand mode.");
      public GUIContent MapsArraySize = EditorGUIUtility.TextContent("Array Size|The length of the array of lightmaps.");
    }
  }
}

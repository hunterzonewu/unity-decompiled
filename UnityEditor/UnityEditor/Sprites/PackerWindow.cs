// Decompiled with JetBrains decompiler
// Type: UnityEditor.Sprites.PackerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Sprites
{
  internal class PackerWindow : SpriteUtilityWindow
  {
    private static string[] s_AtlasNamesEmpty = new string[1]{ "Sprite atlas cache is empty" };
    private static string[] s_PageNamesEmpty = new string[0];
    private string[] m_AtlasNames = PackerWindow.s_AtlasNamesEmpty;
    private string[] m_PageNames = PackerWindow.s_PageNamesEmpty;
    private int m_SelectedAtlas;
    private int m_SelectedPage;
    private Sprite m_SelectedSprite;

    private void OnEnable()
    {
      this.minSize = new Vector2(400f, 256f);
      this.titleContent = EditorGUIUtility.TextContent("Sprite Packer");
      this.Reset();
    }

    private void Reset()
    {
      this.RefreshAtlasNameList();
      this.RefreshAtlasPageList();
      this.m_SelectedAtlas = 0;
      this.m_SelectedPage = 0;
      this.m_SelectedSprite = (Sprite) null;
    }

    private void RefreshAtlasNameList()
    {
      this.m_AtlasNames = Packer.atlasNames;
      if (this.m_SelectedAtlas < this.m_AtlasNames.Length)
        return;
      this.m_SelectedAtlas = 0;
    }

    private void RefreshAtlasPageList()
    {
      if (this.m_AtlasNames.Length > 0)
      {
        Texture2D[] texturesForAtlas = Packer.GetTexturesForAtlas(this.m_AtlasNames[this.m_SelectedAtlas]);
        this.m_PageNames = new string[texturesForAtlas.Length];
        for (int index = 0; index < texturesForAtlas.Length; ++index)
          this.m_PageNames[index] = string.Format("Page {0}", (object) (index + 1));
      }
      else
        this.m_PageNames = PackerWindow.s_PageNamesEmpty;
      if (this.m_SelectedPage < this.m_PageNames.Length)
        return;
      this.m_SelectedPage = 0;
    }

    private void OnAtlasNameListChanged()
    {
      if (this.m_AtlasNames.Length > 0)
      {
        string[] atlasNames = Packer.atlasNames;
        if (this.m_AtlasNames[this.m_SelectedAtlas].Equals(atlasNames.Length > this.m_SelectedAtlas ? atlasNames[this.m_SelectedAtlas] : (string) null))
        {
          this.RefreshAtlasNameList();
          this.RefreshAtlasPageList();
          this.m_SelectedSprite = (Sprite) null;
          return;
        }
      }
      this.Reset();
    }

    private bool ValidateIsPackingEnabled()
    {
      if (EditorSettings.spritePackerMode != SpritePackerMode.Disabled)
        return true;
      EditorGUILayout.BeginVertical();
      GUILayout.Label("Sprite packing is disabled. Enable it in Edit > Project Settings > Editor.");
      if (GUILayout.Button("Open Project Editor Settings"))
        EditorApplication.ExecuteMenuItem("Edit/Project Settings/Editor");
      EditorGUILayout.EndVertical();
      return false;
    }

    private void DoToolbarGUI()
    {
      EditorGUILayout.BeginHorizontal();
      EditorGUI.BeginDisabledGroup(Application.isPlaying);
      if (GUILayout.Button("Pack", EditorStyles.toolbarButton, new GUILayoutOption[0]))
      {
        Packer.RebuildAtlasCacheIfNeeded(EditorUserBuildSettings.activeBuildTarget, true);
        this.m_SelectedSprite = (Sprite) null;
        this.RefreshAtlasPageList();
        this.RefreshState();
      }
      else
      {
        EditorGUI.BeginDisabledGroup(Packer.SelectedPolicy == Packer.kDefaultPolicy);
        if (GUILayout.Button("Repack", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        {
          Packer.RebuildAtlasCacheIfNeeded(EditorUserBuildSettings.activeBuildTarget, true, Packer.Execution.ForceRegroup);
          this.m_SelectedSprite = (Sprite) null;
          this.RefreshAtlasPageList();
          this.RefreshState();
        }
        EditorGUI.EndDisabledGroup();
      }
      EditorGUI.EndDisabledGroup();
      EditorGUI.BeginDisabledGroup(this.m_AtlasNames.Length == 0);
      GUILayout.Space(16f);
      GUILayout.Label("View atlas:");
      EditorGUI.BeginChangeCheck();
      this.m_SelectedAtlas = EditorGUILayout.Popup(this.m_SelectedAtlas, this.m_AtlasNames, EditorStyles.toolbarPopup, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        this.RefreshAtlasPageList();
        this.m_SelectedSprite = (Sprite) null;
      }
      EditorGUI.BeginChangeCheck();
      this.m_SelectedPage = EditorGUILayout.Popup(this.m_SelectedPage, this.m_PageNames, EditorStyles.toolbarPopup, new GUILayoutOption[1]
      {
        GUILayout.Width(70f)
      });
      if (EditorGUI.EndChangeCheck())
        this.m_SelectedSprite = (Sprite) null;
      EditorGUI.EndDisabledGroup();
      EditorGUI.BeginChangeCheck();
      string[] policies = Packer.Policies;
      int index = EditorGUILayout.Popup(Array.IndexOf<string>(policies, Packer.SelectedPolicy), policies, EditorStyles.toolbarPopup, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        Packer.SelectedPolicy = policies[index];
      EditorGUILayout.EndHorizontal();
    }

    private void OnSelectionChange()
    {
      if (Selection.activeObject == (UnityEngine.Object) null)
        return;
      Sprite activeObject = Selection.activeObject as Sprite;
      if (!((UnityEngine.Object) activeObject != (UnityEngine.Object) this.m_SelectedSprite))
        return;
      if ((UnityEngine.Object) activeObject != (UnityEngine.Object) null)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PackerWindow.\u003COnSelectionChange\u003Ec__AnonStoreyB5 changeCAnonStoreyB5 = new PackerWindow.\u003COnSelectionChange\u003Ec__AnonStoreyB5();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Packer.GetAtlasDataForSprite(activeObject, out changeCAnonStoreyB5.selAtlasName, out changeCAnonStoreyB5.selAtlasTexture);
        // ISSUE: reference to a compiler-generated method
        int index1 = ((IEnumerable<string>) this.m_AtlasNames).ToList<string>().FindIndex(new Predicate<string>(changeCAnonStoreyB5.\u003C\u003Em__219));
        if (index1 == -1)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        int index2 = ((IEnumerable<Texture2D>) Packer.GetTexturesForAtlas(changeCAnonStoreyB5.selAtlasName)).ToList<Texture2D>().FindIndex(new Predicate<Texture2D>(changeCAnonStoreyB5.\u003C\u003Em__21A));
        if (index2 == -1)
          return;
        this.m_SelectedAtlas = index1;
        this.m_SelectedPage = index2;
        this.RefreshAtlasPageList();
      }
      this.m_SelectedSprite = activeObject;
      this.Repaint();
    }

    private void RefreshState()
    {
      string[] atlasNames = Packer.atlasNames;
      if (!((IEnumerable<string>) atlasNames).SequenceEqual<string>((IEnumerable<string>) this.m_AtlasNames))
      {
        if (atlasNames.Length == 0)
        {
          this.Reset();
          return;
        }
        this.OnAtlasNameListChanged();
      }
      if (this.m_AtlasNames.Length == 0)
      {
        this.SetNewTexture((Texture2D) null);
      }
      else
      {
        if (this.m_SelectedAtlas >= this.m_AtlasNames.Length)
          this.m_SelectedAtlas = 0;
        string atlasName = this.m_AtlasNames[this.m_SelectedAtlas];
        Texture2D[] texturesForAtlas1 = Packer.GetTexturesForAtlas(atlasName);
        if (this.m_SelectedPage >= texturesForAtlas1.Length)
          this.m_SelectedPage = 0;
        this.SetNewTexture(texturesForAtlas1[this.m_SelectedPage]);
        Texture2D[] texturesForAtlas2 = Packer.GetAlphaTexturesForAtlas(atlasName);
        this.SetAlphaTextureOverride(this.m_SelectedPage >= texturesForAtlas2.Length ? (Texture2D) null : texturesForAtlas2[this.m_SelectedPage]);
      }
    }

    public void OnGUI()
    {
      if (!this.ValidateIsPackingEnabled())
        return;
      Matrix4x4 matrix = Handles.matrix;
      this.InitStyles();
      this.RefreshState();
      Rect rect = EditorGUILayout.BeginHorizontal(GUIContent.none, EditorStyles.toolbar, new GUILayoutOption[0]);
      this.DoToolbarGUI();
      GUILayout.FlexibleSpace();
      bool enabled = GUI.enabled;
      GUI.enabled = this.m_AtlasNames.Length > 0;
      this.DoAlphaZoomToolbarGUI();
      GUI.enabled = enabled;
      EditorGUILayout.EndHorizontal();
      if ((UnityEngine.Object) this.m_Texture == (UnityEngine.Object) null)
        return;
      EditorGUILayout.BeginHorizontal();
      this.m_TextureViewRect = new Rect(0.0f, rect.yMax, this.position.width - 16f, this.position.height - 16f - rect.height);
      GUILayout.FlexibleSpace();
      this.DoTextureGUI();
      EditorGUI.DropShadowLabel(new Rect(this.m_TextureViewRect.x, this.m_TextureViewRect.y + 10f, this.m_TextureViewRect.width, 20f), string.Format("{1}x{2}, {0}", (object) TextureUtil.GetTextureFormatString(this.m_Texture.format), (object) this.m_Texture.width, (object) this.m_Texture.height));
      EditorGUILayout.EndHorizontal();
      Handles.matrix = matrix;
    }

    private void DrawLineUtility(Vector2 from, Vector2 to)
    {
      SpriteEditorUtility.DrawLine(new Vector3((float) ((double) from.x * (double) this.m_Texture.width + 1.0 / (double) this.m_Zoom), (float) ((double) from.y * (double) this.m_Texture.height + 1.0 / (double) this.m_Zoom), 0.0f), new Vector3((float) ((double) to.x * (double) this.m_Texture.width + 1.0 / (double) this.m_Zoom), (float) ((double) to.y * (double) this.m_Texture.height + 1.0 / (double) this.m_Zoom), 0.0f));
    }

    private PackerWindow.Edge[] FindUniqueEdges(ushort[] indices)
    {
      PackerWindow.Edge[] edgeArray = new PackerWindow.Edge[indices.Length];
      int num = indices.Length / 3;
      for (int index = 0; index < num; ++index)
      {
        edgeArray[index * 3] = new PackerWindow.Edge(indices[index * 3], indices[index * 3 + 1]);
        edgeArray[index * 3 + 1] = new PackerWindow.Edge(indices[index * 3 + 1], indices[index * 3 + 2]);
        edgeArray[index * 3 + 2] = new PackerWindow.Edge(indices[index * 3 + 2], indices[index * 3]);
      }
      return ((IEnumerable<PackerWindow.Edge>) edgeArray).GroupBy<PackerWindow.Edge, PackerWindow.Edge>((Func<PackerWindow.Edge, PackerWindow.Edge>) (x => x)).Where<IGrouping<PackerWindow.Edge, PackerWindow.Edge>>((Func<IGrouping<PackerWindow.Edge, PackerWindow.Edge>, bool>) (x => x.Count<PackerWindow.Edge>() == 1)).Select<IGrouping<PackerWindow.Edge, PackerWindow.Edge>, PackerWindow.Edge>((Func<IGrouping<PackerWindow.Edge, PackerWindow.Edge>, PackerWindow.Edge>) (x => x.First<PackerWindow.Edge>())).ToArray<PackerWindow.Edge>();
    }

    protected override void DrawGizmos()
    {
      if (!((UnityEngine.Object) this.m_SelectedSprite != (UnityEngine.Object) null) || !((UnityEngine.Object) this.m_Texture != (UnityEngine.Object) null))
        return;
      Vector2[] spriteUvs = SpriteUtility.GetSpriteUVs(this.m_SelectedSprite, true);
      PackerWindow.Edge[] uniqueEdges = this.FindUniqueEdges(this.m_SelectedSprite.triangles);
      SpriteEditorUtility.BeginLines(new Color(0.3921f, 0.5843f, 0.9294f, 0.75f));
      foreach (PackerWindow.Edge edge in uniqueEdges)
        this.DrawLineUtility(spriteUvs[(int) edge.v0], spriteUvs[(int) edge.v1]);
      SpriteEditorUtility.EndLines();
    }

    private struct Edge
    {
      public ushort v0;
      public ushort v1;

      public Edge(ushort a, ushort b)
      {
        this.v0 = a;
        this.v1 = b;
      }

      public override bool Equals(object obj)
      {
        PackerWindow.Edge edge = (PackerWindow.Edge) obj;
        if ((int) this.v0 == (int) edge.v0 && (int) this.v1 == (int) edge.v1)
          return true;
        if ((int) this.v0 == (int) edge.v1)
          return (int) this.v1 == (int) edge.v0;
        return false;
      }

      public override int GetHashCode()
      {
        return ((int) this.v0 << 16 | (int) this.v1) ^ ((int) this.v1 << 16 | (int) this.v0).GetHashCode();
      }
    }
  }
}

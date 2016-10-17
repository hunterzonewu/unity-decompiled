// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProceduralTextureInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (ProceduralTexture))]
  [CanEditMultipleObjects]
  internal class ProceduralTextureInspector : TextureInspector
  {
    private bool m_MightHaveModified;

    protected override void OnDisable()
    {
      base.OnDisable();
      if (EditorApplication.isPlaying || InternalEditorUtility.ignoreInspectorChanges || !this.m_MightHaveModified)
        return;
      this.m_MightHaveModified = false;
      string[] strArray = new string[this.targets.GetLength(0)];
      int num = 0;
      foreach (ProceduralTexture target in this.targets)
      {
        SubstanceImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((Object) target)) as SubstanceImporter;
        if ((bool) ((Object) atPath))
          atPath.OnTextureInformationsChanged(target);
        string assetPath = AssetDatabase.GetAssetPath((Object) target.GetProceduralMaterial());
        bool flag = false;
        for (int index = 0; index < num; ++index)
        {
          if (strArray[index] == assetPath)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          strArray[num++] = assetPath;
      }
      for (int index = 0; index < num; ++index)
      {
        SubstanceImporter atPath = AssetImporter.GetAtPath(strArray[index]) as SubstanceImporter;
        if ((bool) ((Object) atPath) && EditorUtility.IsDirty(atPath.GetInstanceID()))
          AssetDatabase.ImportAsset(strArray[index], ImportAssetOptions.ForceUncompressedImport);
      }
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      if (GUI.changed)
        this.m_MightHaveModified = true;
      foreach (ProceduralTexture target in this.targets)
      {
        if ((bool) ((Object) target))
        {
          ProceduralMaterial proceduralMaterial = target.GetProceduralMaterial();
          if ((bool) ((Object) proceduralMaterial) && proceduralMaterial.isProcessing)
          {
            this.Repaint();
            SceneView.RepaintAll();
            GameView.RepaintAll();
            break;
          }
        }
      }
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      base.OnPreviewGUI(r, background);
      if (!(bool) this.target)
        return;
      ProceduralMaterial proceduralMaterial = (this.target as ProceduralTexture).GetProceduralMaterial();
      if (!(bool) ((Object) proceduralMaterial) || !ProceduralMaterialInspector.ShowIsGenerating(proceduralMaterial) || (double) r.width <= 50.0)
        return;
      EditorGUI.DropShadowLabel(new Rect(r.x, r.y, r.width, 20f), "Generating...");
    }
  }
}

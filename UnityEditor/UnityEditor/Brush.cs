// Decompiled with JetBrains decompiler
// Type: UnityEditor.Brush
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class Brush
  {
    internal const int kMinBrushSize = 3;
    private float[] m_Strength;
    private int m_Size;
    private Texture2D m_Brush;
    private Texture2D m_Preview;
    private Projector m_BrushProjector;

    public bool Load(Texture2D brushTex, int size)
    {
      if ((Object) this.m_Brush == (Object) brushTex && size == this.m_Size && this.m_Strength != null)
        return true;
      if ((Object) brushTex != (Object) null)
      {
        float num = (float) size;
        this.m_Size = size;
        this.m_Strength = new float[this.m_Size * this.m_Size];
        if (this.m_Size > 3)
        {
          for (int index1 = 0; index1 < this.m_Size; ++index1)
          {
            for (int index2 = 0; index2 < this.m_Size; ++index2)
              this.m_Strength[index1 * this.m_Size + index2] = brushTex.GetPixelBilinear(((float) index2 + 0.5f) / num, (float) index1 / num).a;
          }
        }
        else
        {
          for (int index = 0; index < this.m_Strength.Length; ++index)
            this.m_Strength[index] = 1f;
        }
        Object.DestroyImmediate((Object) this.m_Preview);
        this.m_Preview = new Texture2D(this.m_Size, this.m_Size, TextureFormat.ARGB32, false);
        this.m_Preview.hideFlags = HideFlags.HideAndDontSave;
        this.m_Preview.wrapMode = TextureWrapMode.Repeat;
        this.m_Preview.filterMode = UnityEngine.FilterMode.Point;
        Color[] colors = new Color[this.m_Size * this.m_Size];
        for (int index = 0; index < colors.Length; ++index)
          colors[index] = new Color(1f, 1f, 1f, this.m_Strength[index]);
        this.m_Preview.SetPixels(0, 0, this.m_Size, this.m_Size, colors, 0);
        this.m_Preview.Apply();
        if ((Object) this.m_BrushProjector == (Object) null)
          this.CreatePreviewBrush();
        this.m_BrushProjector.material.mainTexture = (Texture) this.m_Preview;
        this.m_Brush = brushTex;
        return true;
      }
      this.m_Strength = new float[1];
      this.m_Strength[0] = 1f;
      this.m_Size = 1;
      return false;
    }

    public float GetStrengthInt(int ix, int iy)
    {
      ix = Mathf.Clamp(ix, 0, this.m_Size - 1);
      iy = Mathf.Clamp(iy, 0, this.m_Size - 1);
      return this.m_Strength[iy * this.m_Size + ix];
    }

    public void Dispose()
    {
      if ((bool) ((Object) this.m_BrushProjector))
      {
        Object.DestroyImmediate((Object) this.m_BrushProjector.gameObject);
        this.m_BrushProjector = (Projector) null;
      }
      Object.DestroyImmediate((Object) this.m_Preview);
      this.m_Preview = (Texture2D) null;
    }

    public Projector GetPreviewProjector()
    {
      return this.m_BrushProjector;
    }

    private void CreatePreviewBrush()
    {
      this.m_BrushProjector = EditorUtility.CreateGameObjectWithHideFlags("TerrainInspectorBrushPreview", HideFlags.HideAndDontSave, typeof (Projector)).GetComponent(typeof (Projector)) as Projector;
      this.m_BrushProjector.enabled = false;
      this.m_BrushProjector.nearClipPlane = -1000f;
      this.m_BrushProjector.farClipPlane = 1000f;
      this.m_BrushProjector.orthographic = true;
      this.m_BrushProjector.orthographicSize = 10f;
      this.m_BrushProjector.transform.Rotate(90f, 0.0f, 0.0f);
      Material material = EditorGUIUtility.LoadRequired("SceneView/TerrainBrushMaterial.mat") as Material;
      material.SetTexture("_CutoutTex", (Texture) EditorGUIUtility.Load(EditorResourcesUtility.brushesPath + "brush_cutout.png"));
      this.m_BrushProjector.material = material;
      this.m_BrushProjector.enabled = false;
    }
  }
}

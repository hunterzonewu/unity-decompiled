// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Sprite))]
  internal class SpriteInspector : Editor
  {
    public readonly GUIContent[] spriteAlignmentOptions = new GUIContent[10]{ EditorGUIUtility.TextContent("Center"), EditorGUIUtility.TextContent("Top Left"), EditorGUIUtility.TextContent("Top"), EditorGUIUtility.TextContent("Top Right"), EditorGUIUtility.TextContent("Left"), EditorGUIUtility.TextContent("Right"), EditorGUIUtility.TextContent("Bottom Left"), EditorGUIUtility.TextContent("Bottom"), EditorGUIUtility.TextContent("Bottom Right"), EditorGUIUtility.TextContent("Custom") };
    public readonly GUIContent spriteAlignment = EditorGUIUtility.TextContent("Pivot|Sprite pivot point in its localspace. May be used for syncing animation frames of different sizes.");

    private Sprite sprite
    {
      get
      {
        return this.target as Sprite;
      }
    }

    private SpriteMetaData GetMetaData(string name)
    {
      TextureImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((Object) this.sprite)) as TextureImporter;
      if (!((Object) atPath != (Object) null))
        return new SpriteMetaData();
      if (atPath.spriteImportMode == SpriteImportMode.Single)
        return SpriteInspector.GetMetaDataInSingleMode(name, atPath);
      return SpriteInspector.GetMetaDataInMultipleMode(name, atPath);
    }

    private static SpriteMetaData GetMetaDataInMultipleMode(string name, TextureImporter textureImporter)
    {
      SpriteMetaData[] spritesheet = textureImporter.spritesheet;
      for (int index = 0; index < spritesheet.Length; ++index)
      {
        if (spritesheet[index].name.Equals(name))
          return spritesheet[index];
      }
      return new SpriteMetaData();
    }

    private static SpriteMetaData GetMetaDataInSingleMode(string name, TextureImporter textureImporter)
    {
      SpriteMetaData spriteMetaData = new SpriteMetaData();
      spriteMetaData.border = textureImporter.spriteBorder;
      spriteMetaData.name = name;
      spriteMetaData.pivot = textureImporter.spritePivot;
      spriteMetaData.rect = new Rect(0.0f, 0.0f, 1f, 1f);
      TextureImporterSettings dest = new TextureImporterSettings();
      textureImporter.ReadTextureSettings(dest);
      spriteMetaData.alignment = dest.spriteAlignment;
      return spriteMetaData;
    }

    public override void OnInspectorGUI()
    {
      bool name;
      bool alignment;
      bool border1;
      this.UnifiedValues(out name, out alignment, out border1);
      if (name)
        EditorGUILayout.LabelField("Name", this.sprite.name, new GUILayoutOption[0]);
      else
        EditorGUILayout.LabelField("Name", "-", new GUILayoutOption[0]);
      if (alignment)
        EditorGUILayout.LabelField(this.spriteAlignment, this.spriteAlignmentOptions[this.GetMetaData(this.sprite.name).alignment], new GUILayoutOption[0]);
      else
        EditorGUILayout.LabelField(this.spriteAlignment.text, "-", new GUILayoutOption[0]);
      if (border1)
      {
        Vector4 border2 = this.GetMetaData(this.sprite.name).border;
        EditorGUILayout.LabelField("Border", string.Format("L:{0} B:{1} R:{2} T:{3}", (object) border2.x, (object) border2.y, (object) border2.z, (object) border2.w), new GUILayoutOption[0]);
      }
      else
        EditorGUILayout.LabelField("Border", "-", new GUILayoutOption[0]);
    }

    private void UnifiedValues(out bool name, out bool alignment, out bool border)
    {
      name = true;
      alignment = true;
      border = true;
      if (this.targets.Length < 2)
        return;
      SpriteMetaData[] spritesheet = (AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((Object) this.sprite)) as TextureImporter).spritesheet;
      string str = (string) null;
      int num = -1;
      Vector4? nullable = new Vector4?();
      for (int index1 = 0; index1 < this.targets.Length; ++index1)
      {
        Sprite target = this.targets[index1] as Sprite;
        for (int index2 = 0; index2 < spritesheet.Length; ++index2)
        {
          if (spritesheet[index2].name.Equals(target.name))
          {
            if (spritesheet[index2].alignment != num && num > 0)
              alignment = false;
            else
              num = spritesheet[index2].alignment;
            if (spritesheet[index2].name != str && str != null)
              name = false;
            else
              str = spritesheet[index2].name;
            if ((spritesheet[index2].border != nullable.GetValueOrDefault() ? 1 : (!nullable.HasValue ? 1 : 0)) != 0 && nullable.HasValue)
              border = false;
            else
              nullable = new Vector4?(spritesheet[index2].border);
          }
        }
      }
    }

    public static Texture2D BuildPreviewTexture(int width, int height, Sprite sprite, Material spriteRendererMaterial, bool isPolygon)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return (Texture2D) null;
      float width1 = sprite.rect.width;
      float height1 = sprite.rect.height;
      Texture2D spriteTexture = UnityEditor.Sprites.SpriteUtility.GetSpriteTexture(sprite, false);
      if (!isPolygon)
        PreviewHelpers.AdjustWidthAndHeightForStaticPreview((int) width1, (int) height1, ref width, ref height);
      SavedRenderTargetState renderTargetState = new SavedRenderTargetState();
      RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
      RenderTexture.active = temporary;
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      GL.Clear(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
      Texture texture = (Texture) null;
      Vector4 vector = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
      bool flag1 = false;
      bool flag2 = false;
      if ((Object) spriteRendererMaterial != (Object) null)
      {
        flag1 = spriteRendererMaterial.HasProperty("_MainTex");
        flag2 = spriteRendererMaterial.HasProperty("_MainTex_TexelSize");
      }
      Material material = (Material) null;
      if ((Object) spriteRendererMaterial != (Object) null)
      {
        if (flag1)
        {
          texture = spriteRendererMaterial.GetTexture("_MainTex");
          spriteRendererMaterial.SetTexture("_MainTex", (Texture) spriteTexture);
        }
        if (flag2)
        {
          vector = spriteRendererMaterial.GetVector("_MainTex_TexelSize");
          spriteRendererMaterial.SetVector("_MainTex_TexelSize", TextureUtil.GetTexelSizeVector((Texture) spriteTexture));
        }
        spriteRendererMaterial.SetPass(0);
      }
      else
      {
        material = new Material(Shader.Find("Hidden/BlitCopy"));
        material.mainTexture = (Texture) spriteTexture;
        material.mainTextureScale = Vector2.one;
        material.mainTextureOffset = Vector2.zero;
        material.SetPass(0);
      }
      float num1 = sprite.rect.width / sprite.bounds.size.x;
      Vector2[] vertices = sprite.vertices;
      Vector2[] uv = sprite.uv;
      ushort[] triangles = sprite.triangles;
      Vector2 pivot = sprite.pivot;
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Color(new Color(1f, 1f, 1f, 1f));
      GL.Begin(4);
      for (int index = 0; index < triangles.Length; ++index)
      {
        ushort num2 = triangles[index];
        Vector2 vector2_1 = vertices[(int) num2];
        Vector2 vector2_2 = uv[(int) num2];
        GL.TexCoord(new Vector3(vector2_2.x, vector2_2.y, 0.0f));
        GL.Vertex3((vector2_1.x * num1 + pivot.x) / width1, (vector2_1.y * num1 + pivot.y) / height1, 0.0f);
      }
      GL.End();
      GL.PopMatrix();
      GL.sRGBWrite = false;
      if ((Object) spriteRendererMaterial != (Object) null)
      {
        if (flag1)
          spriteRendererMaterial.SetTexture("_MainTex", texture);
        if (flag2)
          spriteRendererMaterial.SetVector("_MainTex_TexelSize", vector);
      }
      Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) width, (float) height), 0, 0);
      texture2D.Apply();
      RenderTexture.ReleaseTemporary(temporary);
      renderTargetState.Restore();
      if ((Object) material != (Object) null)
        Object.DestroyImmediate((Object) material);
      return texture2D;
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
      bool isPolygon = false;
      TextureImporter atPath = AssetImporter.GetAtPath(assetPath) as TextureImporter;
      if ((Object) atPath != (Object) null)
        isPolygon = atPath.spriteImportMode == SpriteImportMode.Polygon;
      return SpriteInspector.BuildPreviewTexture(width, height, this.sprite, (Material) null, isPolygon);
    }

    public override bool HasPreviewGUI()
    {
      return this.target != (Object) null;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (this.target == (Object) null || Event.current.type != EventType.Repaint)
        return;
      bool isPolygon = false;
      TextureImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((Object) this.sprite)) as TextureImporter;
      if ((Object) atPath != (Object) null)
        isPolygon = atPath.spriteImportMode == SpriteImportMode.Polygon;
      SpriteInspector.DrawPreview(r, this.sprite, (Material) null, isPolygon);
    }

    public static void DrawPreview(Rect r, Sprite frame, Material spriteRendererMaterial, bool isPolygon)
    {
      if ((Object) frame == (Object) null)
        return;
      float num = Mathf.Min(r.width / frame.rect.width, r.height / frame.rect.height);
      Rect position = new Rect(r.x, r.y, frame.rect.width * num, frame.rect.height * num);
      position.center = r.center;
      Texture2D texture2D = SpriteInspector.BuildPreviewTexture((int) position.width, (int) position.height, frame, spriteRendererMaterial, isPolygon);
      EditorGUI.DrawTextureTransparent(position, (Texture) texture2D, ScaleMode.ScaleToFit);
      if (!Mathf.Approximately((frame.border * num).sqrMagnitude, 0.0f))
      {
        SpriteEditorUtility.BeginLines(new Color(0.0f, 1f, 0.0f, 0.7f));
        SpriteEditorUtility.EndLines();
      }
      Object.DestroyImmediate((Object) texture2D);
    }

    public override string GetInfoString()
    {
      if (this.target == (Object) null)
        return string.Empty;
      Sprite target = this.target as Sprite;
      return string.Format("({0}x{1})", (object) (int) target.rect.width, (object) (int) target.rect.height);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.CubemapInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (Cubemap))]
  internal class CubemapInspector : TextureInspector
  {
    private static readonly string[] kSizes = new string[8]{ "16", "32", "64", "128", "256", "512", "1024", "2048" };
    private static readonly int[] kSizesValues = new int[8]{ 16, 32, 64, 128, 256, 512, 1024, 2048 };
    private const int kTextureSize = 64;
    private Texture2D[] m_Images;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.InitTexturesFromCubemap();
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      if (this.m_Images != null)
      {
        for (int index = 0; index < this.m_Images.Length; ++index)
        {
          if ((bool) ((UnityEngine.Object) this.m_Images[index]) && !EditorUtility.IsPersistent((UnityEngine.Object) this.m_Images[index]))
            UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Images[index]);
        }
      }
      this.m_Images = (Texture2D[]) null;
    }

    private void InitTexturesFromCubemap()
    {
      Cubemap target = this.target as Cubemap;
      if (!((UnityEngine.Object) target != (UnityEngine.Object) null))
        return;
      if (this.m_Images == null)
        this.m_Images = new Texture2D[6];
      for (int index = 0; index < this.m_Images.Length; ++index)
      {
        if ((bool) ((UnityEngine.Object) this.m_Images[index]) && !EditorUtility.IsPersistent((UnityEngine.Object) this.m_Images[index]))
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Images[index]);
        if ((bool) ((UnityEngine.Object) TextureUtil.GetSourceTexture(target, (CubemapFace) index)))
        {
          this.m_Images[index] = TextureUtil.GetSourceTexture(target, (CubemapFace) index);
        }
        else
        {
          this.m_Images[index] = new Texture2D(64, 64, TextureFormat.ARGB32, false);
          this.m_Images[index].hideFlags = HideFlags.HideAndDontSave;
          TextureUtil.CopyCubemapFaceIntoTexture(target, (CubemapFace) index, this.m_Images[index]);
        }
      }
    }

    public override void OnInspectorGUI()
    {
      if (this.m_Images == null)
        this.InitTexturesFromCubemap();
      EditorGUIUtility.labelWidth = 50f;
      Cubemap target = this.target as Cubemap;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      GUILayout.BeginVertical();
      GUILayout.BeginHorizontal();
      this.ShowFace("Right\n(+X)", CubemapFace.PositiveX);
      this.ShowFace("Left\n(-X)", CubemapFace.NegativeX);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      this.ShowFace("Top\n(+Y)", CubemapFace.PositiveY);
      this.ShowFace("Bottom\n(-Y)", CubemapFace.NegativeY);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      this.ShowFace("Front\n(+Z)", CubemapFace.PositiveZ);
      this.ShowFace("Back\n(-Z)", CubemapFace.NegativeZ);
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      EditorGUIUtility.labelWidth = 0.0f;
      EditorGUILayout.Space();
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.HelpBox("Lowering face size is a destructive operation, you might need to re-assign the textures later to fix resolution issues. It's preferable to use Cubemap texture import type instead of Legacy Cubemap assets.", MessageType.Warning);
      int num = EditorGUILayout.IntPopup("Face size", TextureUtil.GetGLWidth((Texture) target), CubemapInspector.kSizes, CubemapInspector.kSizesValues, new GUILayoutOption[0]);
      bool useMipmap = EditorGUILayout.Toggle("MipMaps", TextureUtil.CountMipmaps((Texture) target) > 1, new GUILayoutOption[0]);
      bool linear = EditorGUILayout.Toggle("Linear", TextureUtil.GetLinearSampled((Texture) target), new GUILayoutOption[0]);
      bool readable = EditorGUILayout.Toggle("Readable", TextureUtil.IsCubemapReadable(target), new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      if (TextureUtil.ReformatCubemap(ref target, num, num, target.format, useMipmap, linear))
        this.InitTexturesFromCubemap();
      TextureUtil.MarkCubemapReadable(target, readable);
      target.Apply();
    }

    internal override void OnAssetStoreInspectorGUI()
    {
      this.OnInspectorGUI();
    }

    private void ShowFace(string label, CubemapFace face)
    {
      Cubemap target = this.target as Cubemap;
      int index = (int) face;
      GUI.changed = false;
      Texture2D textureRef = (Texture2D) CubemapInspector.ObjectField(label, (UnityEngine.Object) this.m_Images[index], typeof (Texture2D), false);
      if (!GUI.changed)
        return;
      TextureUtil.CopyTextureIntoCubemapFace(textureRef, target, face);
      this.m_Images[index] = textureRef;
    }

    public static UnityEngine.Object ObjectField(string label, UnityEngine.Object obj, System.Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
    {
      GUILayout.BeginHorizontal();
      GUI.Label(GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, 32f, EditorStyles.label, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      }), label, EditorStyles.label);
      UnityEngine.Object @object = EditorGUI.ObjectField(GUILayoutUtility.GetAspectRect(1f, EditorStyles.objectField, GUILayout.Width(64f)), obj, objType, allowSceneObjects);
      GUILayout.EndHorizontal();
      return @object;
    }
  }
}

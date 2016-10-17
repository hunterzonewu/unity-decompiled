// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShaderImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (ShaderImporter))]
  internal class ShaderImporterInspector : AssetImporterInspector
  {
    private List<string> propertyNames = new List<string>();
    private List<string> displayNames = new List<string>();
    private List<Texture> textures = new List<Texture>();
    private List<ShaderUtil.ShaderPropertyTexDim> dimensions = new List<ShaderUtil.ShaderPropertyTexDim>();

    internal override void OnHeaderControlsGUI()
    {
      Shader target = this.assetEditor.target as Shader;
      GUILayout.FlexibleSpace();
      if (!GUILayout.Button("Open...", EditorStyles.miniButton, new GUILayoutOption[0]))
        return;
      AssetDatabase.OpenAsset((UnityEngine.Object) target);
      GUIUtility.ExitGUI();
    }

    public void OnEnable()
    {
      this.ResetValues();
    }

    private void ShowDefaultTextures()
    {
      if (this.propertyNames.Count == 0)
        return;
      EditorGUILayout.LabelField("Default Maps", EditorStyles.boldLabel, new GUILayoutOption[0]);
      for (int index = 0; index < this.propertyNames.Count; ++index)
      {
        Texture texture1 = this.textures[index];
        Texture texture2 = (Texture) null;
        EditorGUI.BeginChangeCheck();
        System.Type objType;
        switch (this.dimensions[index])
        {
          case ShaderUtil.ShaderPropertyTexDim.TexDim2D:
            objType = typeof (Texture);
            break;
          case ShaderUtil.ShaderPropertyTexDim.TexDim3D:
            objType = typeof (Texture3D);
            break;
          case ShaderUtil.ShaderPropertyTexDim.TexDimCUBE:
            objType = typeof (Cubemap);
            break;
          case ShaderUtil.ShaderPropertyTexDim.TexDimAny:
            objType = typeof (Texture);
            break;
          default:
            objType = (System.Type) null;
            break;
        }
        if (objType != null)
          texture2 = EditorGUILayout.MiniThumbnailObjectField(GUIContent.Temp(!string.IsNullOrEmpty(this.displayNames[index]) ? this.displayNames[index] : ObjectNames.NicifyVariableName(this.propertyNames[index])), (UnityEngine.Object) texture1, objType, (EditorGUI.ObjectFieldValidator) null) as Texture;
        if (EditorGUI.EndChangeCheck())
          this.textures[index] = texture2;
      }
    }

    internal override bool HasModified()
    {
      if (base.HasModified())
        return true;
      ShaderImporter target = this.target as ShaderImporter;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return false;
      Shader shader = target.GetShader();
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null)
        return false;
      int propertyCount = ShaderUtil.GetPropertyCount(shader);
      for (int propertyIdx = 0; propertyIdx < propertyCount; ++propertyIdx)
      {
        string propertyName = ShaderUtil.GetPropertyName(shader, propertyIdx);
        for (int index = 0; index < this.propertyNames.Count; ++index)
        {
          if (this.propertyNames[index] == propertyName && (UnityEngine.Object) this.textures[index] != (UnityEngine.Object) target.GetDefaultTexture(propertyName))
            return true;
        }
      }
      return false;
    }

    internal override void ResetValues()
    {
      base.ResetValues();
      this.propertyNames = new List<string>();
      this.displayNames = new List<string>();
      this.textures = new List<Texture>();
      this.dimensions = new List<ShaderUtil.ShaderPropertyTexDim>();
      ShaderImporter target = this.target as ShaderImporter;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      Shader shader = target.GetShader();
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null)
        return;
      int propertyCount = ShaderUtil.GetPropertyCount(shader);
      for (int propertyIdx = 0; propertyIdx < propertyCount; ++propertyIdx)
      {
        if (ShaderUtil.GetPropertyType(shader, propertyIdx) == ShaderUtil.ShaderPropertyType.TexEnv)
        {
          string propertyName = ShaderUtil.GetPropertyName(shader, propertyIdx);
          string propertyDescription = ShaderUtil.GetPropertyDescription(shader, propertyIdx);
          Texture defaultTexture = target.GetDefaultTexture(propertyName);
          this.propertyNames.Add(propertyName);
          this.displayNames.Add(propertyDescription);
          this.textures.Add(defaultTexture);
          this.dimensions.Add(ShaderUtil.GetTexDim(shader, propertyIdx));
        }
      }
    }

    internal override void Apply()
    {
      base.Apply();
      ShaderImporter target = this.target as ShaderImporter;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      target.SetDefaultTextures(this.propertyNames.ToArray(), this.textures.ToArray());
      AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((UnityEngine.Object) target));
    }

    private static int GetNumberOfTextures(Shader shader)
    {
      int num = 0;
      int propertyCount = ShaderUtil.GetPropertyCount(shader);
      for (int propertyIdx = 0; propertyIdx < propertyCount; ++propertyIdx)
      {
        if (ShaderUtil.GetPropertyType(shader, propertyIdx) == ShaderUtil.ShaderPropertyType.TexEnv)
          ++num;
      }
      return num;
    }

    public override void OnInspectorGUI()
    {
      ShaderImporter target = this.target as ShaderImporter;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      Shader shader = target.GetShader();
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null)
        return;
      if (ShaderImporterInspector.GetNumberOfTextures(shader) != this.propertyNames.Count)
        this.ResetValues();
      this.ShowDefaultTextures();
      this.ApplyRevertGUI();
    }
  }
}

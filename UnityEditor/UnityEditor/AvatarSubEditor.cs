// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarSubEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class AvatarSubEditor : ScriptableObject
  {
    protected AvatarEditor m_Inspector;

    protected GameObject gameObject
    {
      get
      {
        return this.m_Inspector.m_GameObject;
      }
    }

    protected GameObject prefab
    {
      get
      {
        return this.m_Inspector.prefab;
      }
    }

    protected Dictionary<Transform, bool> modelBones
    {
      get
      {
        return this.m_Inspector.m_ModelBones;
      }
    }

    protected Transform root
    {
      get
      {
        if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
          return (Transform) null;
        return this.gameObject.transform;
      }
    }

    protected SerializedObject serializedObject
    {
      get
      {
        return this.m_Inspector.serializedObject;
      }
    }

    protected Avatar avatarAsset
    {
      get
      {
        return this.m_Inspector.avatar;
      }
    }

    private static void DoWriteAllAssets()
    {
      foreach (UnityEngine.Object target in Resources.FindObjectsOfTypeAll(typeof (UnityEngine.Object)))
      {
        if (AssetDatabase.Contains(target))
          EditorUtility.SetDirty(target);
      }
      EditorApplication.SaveAssets();
    }

    public virtual void Enable(AvatarEditor inspector)
    {
      this.m_Inspector = inspector;
    }

    public virtual void Disable()
    {
    }

    public virtual void OnDestroy()
    {
      if (!this.HasModified())
        return;
      AssetImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) this.avatarAsset));
      if (!(bool) ((UnityEngine.Object) atPath))
        return;
      if (EditorUtility.DisplayDialog("Unapplied import settings", "Unapplied import settings for '" + atPath.assetPath + "'", "Apply", "Revert"))
        this.ApplyAndImport();
      else
        this.ResetValues();
    }

    public virtual void OnInspectorGUI()
    {
    }

    public virtual void OnSceneGUI()
    {
    }

    protected bool HasModified()
    {
      return this.serializedObject.hasModifiedProperties;
    }

    protected virtual void ResetValues()
    {
      this.serializedObject.Update();
    }

    protected void Apply()
    {
      this.serializedObject.ApplyModifiedProperties();
    }

    public void ApplyAndImport()
    {
      this.Apply();
      AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((UnityEngine.Object) this.avatarAsset));
      this.ResetValues();
    }

    protected void ApplyRevertGUI()
    {
      EditorGUILayout.Space();
      GUILayout.BeginHorizontal();
      EditorGUI.BeginDisabledGroup(!this.HasModified());
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Revert"))
      {
        this.ResetValues();
        if (this.HasModified())
          Debug.LogError((object) "Avatar tool reports modified values after reset.");
      }
      if (GUILayout.Button("Apply"))
        this.ApplyAndImport();
      EditorGUI.EndDisabledGroup();
      if (GUILayout.Button("Done"))
      {
        this.m_Inspector.SwitchToAssetMode();
        GUIUtility.ExitGUI();
      }
      GUILayout.EndHorizontal();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class AssetImporterInspector : Editor
  {
    private ulong m_AssetTimeStamp;
    private bool m_MightHaveModified;
    private Editor m_AssetEditor;

    internal virtual Editor assetEditor
    {
      get
      {
        return this.m_AssetEditor;
      }
      set
      {
        this.m_AssetEditor = value;
      }
    }

    internal override string targetTitle
    {
      get
      {
        return this.assetEditor.targetTitle + " Import Settings";
      }
    }

    internal override int referenceTargetIndex
    {
      get
      {
        return base.referenceTargetIndex;
      }
      set
      {
        base.referenceTargetIndex = value;
        this.assetEditor.referenceTargetIndex = value;
      }
    }

    internal override IPreviewable preview
    {
      get
      {
        if (this.useAssetDrawPreview && (Object) this.assetEditor != (Object) null)
          return (IPreviewable) this.assetEditor;
        return base.preview;
      }
    }

    protected virtual bool useAssetDrawPreview
    {
      get
      {
        return true;
      }
    }

    internal virtual bool showImportedObject
    {
      get
      {
        return true;
      }
    }

    internal override void OnHeaderIconGUI(Rect iconRect)
    {
      this.assetEditor.OnHeaderIconGUI(iconRect);
    }

    internal override SerializedObject GetSerializedObjectInternal()
    {
      if (this.m_SerializedObject == null)
        this.m_SerializedObject = SerializedObject.LoadFromCache(this.GetInstanceID());
      if (this.m_SerializedObject == null)
        this.m_SerializedObject = new SerializedObject(this.targets);
      return this.m_SerializedObject;
    }

    public virtual void OnDisable()
    {
      AssetImporter target = this.target as AssetImporter;
      if (Unsupported.IsDestroyScriptableObject((ScriptableObject) this) && this.m_MightHaveModified && ((Object) target != (Object) null && !InternalEditorUtility.ignoreInspectorChanges) && (this.HasModified() && !this.AssetWasUpdated()))
      {
        string message = "Unapplied import settings for '" + target.assetPath + "'";
        if (this.targets.Length > 1)
          message = "Unapplied import settings for '" + (object) this.targets.Length + "' files";
        if (EditorUtility.DisplayDialog("Unapplied import settings", message, "Apply", "Revert"))
        {
          this.Apply();
          this.m_MightHaveModified = false;
          AssetImporterInspector.ImportAssets(this.GetAssetPaths());
        }
      }
      if (this.m_SerializedObject == null || !this.m_SerializedObject.hasModifiedProperties)
        return;
      this.m_SerializedObject.Cache(this.GetInstanceID());
      this.m_SerializedObject = (SerializedObject) null;
    }

    internal virtual void Awake()
    {
      this.ResetTimeStamp();
      this.ResetValues();
    }

    private string[] GetAssetPaths()
    {
      Object[] targets = this.targets;
      string[] strArray = new string[targets.Length];
      for (int index = 0; index < targets.Length; ++index)
      {
        AssetImporter assetImporter = targets[index] as AssetImporter;
        strArray[index] = assetImporter.assetPath;
      }
      return strArray;
    }

    internal virtual void ResetValues()
    {
      this.serializedObject.SetIsDifferentCacheDirty();
      this.serializedObject.Update();
    }

    internal virtual bool HasModified()
    {
      return this.serializedObject.hasModifiedProperties;
    }

    internal virtual void Apply()
    {
      this.serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    internal bool AssetWasUpdated()
    {
      AssetImporter target = this.target as AssetImporter;
      if ((long) this.m_AssetTimeStamp == 0L)
        this.ResetTimeStamp();
      if ((Object) target != (Object) null)
        return (long) this.m_AssetTimeStamp != (long) target.assetTimeStamp;
      return false;
    }

    internal void ResetTimeStamp()
    {
      AssetImporter target = this.target as AssetImporter;
      if (!((Object) target != (Object) null))
        return;
      this.m_AssetTimeStamp = target.assetTimeStamp;
    }

    internal void ApplyAndImport()
    {
      this.Apply();
      this.m_MightHaveModified = false;
      AssetImporterInspector.ImportAssets(this.GetAssetPaths());
      this.ResetValues();
    }

    private static void ImportAssets(string[] paths)
    {
      foreach (string path in paths)
        AssetDatabase.WriteImportSettingsIfDirty(path);
      try
      {
        AssetDatabase.StartAssetEditing();
        foreach (string path in paths)
          AssetDatabase.ImportAsset(path);
      }
      finally
      {
        AssetDatabase.StopAssetEditing();
      }
    }

    protected void RevertButton()
    {
      this.RevertButton("Revert");
    }

    protected void RevertButton(string buttonText)
    {
      if (!GUILayout.Button(buttonText))
        return;
      this.m_MightHaveModified = false;
      this.ResetTimeStamp();
      this.ResetValues();
      if (!this.HasModified())
        return;
      Debug.LogError((object) "Importer reports modified values after reset.");
    }

    protected bool ApplyButton()
    {
      return this.ApplyButton("Apply");
    }

    protected bool ApplyButton(string buttonText)
    {
      if (!GUILayout.Button(buttonText))
        return false;
      this.ApplyAndImport();
      return true;
    }

    protected virtual bool ApplyRevertGUIButtons()
    {
      EditorGUI.BeginDisabledGroup(!this.HasModified());
      this.RevertButton();
      bool flag = this.ApplyButton();
      EditorGUI.EndDisabledGroup();
      return flag;
    }

    protected void ApplyRevertGUI()
    {
      this.m_MightHaveModified = true;
      EditorGUILayout.Space();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      bool flag = this.ApplyRevertGUIButtons();
      if (this.AssetWasUpdated() && Event.current.type != EventType.Layout)
      {
        IPreviewable preview = this.preview;
        if (preview != null)
          preview.ReloadPreviewInstances();
        this.ResetTimeStamp();
        this.ResetValues();
        this.Repaint();
      }
      GUILayout.EndHorizontal();
      if (!flag)
        return;
      GUIUtility.ExitGUI();
    }
  }
}

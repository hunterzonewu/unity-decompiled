// Decompiled with JetBrains decompiler
// Type: UnityEditor.CreateAssetUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.IO;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class CreateAssetUtility
  {
    [SerializeField]
    private string m_Path = string.Empty;
    [SerializeField]
    private EndNameEditAction m_EndAction;
    [SerializeField]
    private int m_InstanceID;
    [SerializeField]
    private Texture2D m_Icon;
    [SerializeField]
    private string m_ResourceFile;

    public int instanceID
    {
      get
      {
        return this.m_InstanceID;
      }
    }

    public Texture2D icon
    {
      get
      {
        return this.m_Icon;
      }
    }

    public string folder
    {
      get
      {
        return Path.GetDirectoryName(this.m_Path);
      }
    }

    public string extension
    {
      get
      {
        return Path.GetExtension(this.m_Path);
      }
    }

    public string originalName
    {
      get
      {
        return Path.GetFileNameWithoutExtension(this.m_Path);
      }
    }

    public EndNameEditAction endAction
    {
      get
      {
        return this.m_EndAction;
      }
    }

    public void Clear()
    {
      this.m_EndAction = (EndNameEditAction) null;
      this.m_InstanceID = 0;
      this.m_Path = string.Empty;
      this.m_Icon = (Texture2D) null;
      this.m_ResourceFile = string.Empty;
    }

    private static bool IsPathDataValid(string filePath)
    {
      if (string.IsNullOrEmpty(filePath))
        return false;
      return AssetDatabase.GetMainAssetInstanceID(Path.GetDirectoryName(filePath)) != 0;
    }

    public bool BeginNewAssetCreation(int instanceID, EndNameEditAction newAssetEndAction, string filePath, Texture2D icon, string newAssetResourceFile)
    {
      string filePath1 = filePath.StartsWith("assets/", StringComparison.CurrentCultureIgnoreCase) ? AssetDatabase.GenerateUniqueAssetPath(filePath) : AssetDatabase.GetUniquePathNameAtSelectedPath(filePath);
      if (!CreateAssetUtility.IsPathDataValid(filePath1))
      {
        Debug.LogErrorFormat("Invalid generated unique path '{0}' (input path '{1}')", new object[2]
        {
          (object) filePath1,
          (object) filePath
        });
        this.Clear();
        return false;
      }
      this.m_InstanceID = instanceID;
      this.m_Path = filePath1;
      this.m_Icon = icon;
      this.m_EndAction = newAssetEndAction;
      this.m_ResourceFile = newAssetResourceFile;
      Selection.activeObject = EditorUtility.InstanceIDToObject(instanceID);
      return true;
    }

    public void EndNewAssetCreation(string name)
    {
      string pathName = this.folder + "/" + name + this.extension;
      EndNameEditAction endAction = this.m_EndAction;
      int instanceId = this.m_InstanceID;
      string resourceFile = this.m_ResourceFile;
      this.Clear();
      ProjectWindowUtil.EndNameEditAction(endAction, instanceId, pathName, resourceFile);
    }

    public bool IsCreatingNewAsset()
    {
      return !string.IsNullOrEmpty(this.m_Path);
    }
  }
}

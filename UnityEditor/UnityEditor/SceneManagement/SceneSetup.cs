// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneManagement.SceneSetup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEditor.SceneManagement
{
  /// <summary>
  ///   <para>The setup information for a scene in the SceneManager.</para>
  /// </summary>
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public class SceneSetup
  {
    [SerializeField]
    private string m_Path;
    [SerializeField]
    private bool m_IsLoaded;
    [SerializeField]
    private bool m_IsActive;

    /// <summary>
    ///   <para>Path of the scene. Should be relative to the project folder. Like: "AssetsMyScenesMyScene.unity".</para>
    /// </summary>
    public string path
    {
      get
      {
        return this.m_Path;
      }
      set
      {
        this.m_Path = value;
      }
    }

    /// <summary>
    ///   <para>If the scene is loaded.</para>
    /// </summary>
    public bool isLoaded
    {
      get
      {
        return this.m_IsLoaded;
      }
      set
      {
        this.m_IsLoaded = value;
      }
    }

    /// <summary>
    ///   <para>If the scene is active.</para>
    /// </summary>
    public bool isActive
    {
      get
      {
        return this.m_IsActive;
      }
      set
      {
        this.m_IsActive = value;
      }
    }
  }
}

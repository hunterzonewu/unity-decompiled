// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetPostprocessor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>AssetPostprocessor lets you hook into the import pipeline and run scripts prior or after importing assets.</para>
  /// </summary>
  public class AssetPostprocessor
  {
    private string m_PathName;

    /// <summary>
    ///   <para>The path name of the asset being imported.</para>
    /// </summary>
    public string assetPath
    {
      get
      {
        return this.m_PathName;
      }
      set
      {
        this.m_PathName = value;
      }
    }

    /// <summary>
    ///   <para>Reference to the asset importer.</para>
    /// </summary>
    public AssetImporter assetImporter
    {
      get
      {
        return AssetImporter.GetAtPath(this.assetPath);
      }
    }

    [Obsolete("To set or get the preview, call EditorUtility.SetAssetPreview or AssetPreview.GetAssetPreview instead", true)]
    public Texture2D preview
    {
      get
      {
        return (Texture2D) null;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Logs an import warning to the console.</para>
    /// </summary>
    /// <param name="warning"></param>
    /// <param name="context"></param>
    [ExcludeFromDocs]
    public void LogWarning(string warning)
    {
      UnityEngine.Object context = (UnityEngine.Object) null;
      this.LogWarning(warning, context);
    }

    /// <summary>
    ///   <para>Logs an import warning to the console.</para>
    /// </summary>
    /// <param name="warning"></param>
    /// <param name="context"></param>
    public void LogWarning(string warning, [DefaultValue("null")] UnityEngine.Object context)
    {
      Debug.LogWarning((object) warning, context);
    }

    /// <summary>
    ///   <para>Logs an import error message to the console.</para>
    /// </summary>
    /// <param name="warning"></param>
    /// <param name="context"></param>
    [ExcludeFromDocs]
    public void LogError(string warning)
    {
      UnityEngine.Object context = (UnityEngine.Object) null;
      this.LogError(warning, context);
    }

    /// <summary>
    ///   <para>Logs an import error message to the console.</para>
    /// </summary>
    /// <param name="warning"></param>
    /// <param name="context"></param>
    public void LogError(string warning, [DefaultValue("null")] UnityEngine.Object context)
    {
      Debug.LogError((object) warning, context);
    }

    /// <summary>
    ///   <para>Returns the version of the asset postprocessor.</para>
    /// </summary>
    public virtual uint GetVersion()
    {
      return 0;
    }

    /// <summary>
    ///   <para>Override the order in which importers are processed.</para>
    /// </summary>
    public virtual int GetPostprocessOrder()
    {
      return 0;
    }
  }
}

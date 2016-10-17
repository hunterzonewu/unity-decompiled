// Decompiled with JetBrains decompiler
// Type: UnityEditor.Sprites.Packer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor.Sprites
{
  /// <summary>
  ///   <para>Sprite Packer helpers.</para>
  /// </summary>
  public sealed class Packer
  {
    /// <summary>
    ///   <para>Name of the default Sprite Packer policy.</para>
    /// </summary>
    public static string kDefaultPolicy = typeof (DefaultPackerPolicy).Name;
    private static string[] m_policies = (string[]) null;
    private static string m_selectedPolicy = (string) null;
    private static Dictionary<string, System.Type> m_policyTypeCache = (Dictionary<string, System.Type>) null;

    /// <summary>
    ///   <para>Array of Sprite atlas names found in the current atlas cache.</para>
    /// </summary>
    public static extern string[] atlasNames { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Available Sprite Packer policies for this project.</para>
    /// </summary>
    public static string[] Policies
    {
      get
      {
        Packer.RegenerateList();
        return Packer.m_policies;
      }
    }

    /// <summary>
    ///   <para>The active Sprite Packer policy for this project.</para>
    /// </summary>
    public static string SelectedPolicy
    {
      get
      {
        Packer.RegenerateList();
        return Packer.m_selectedPolicy;
      }
      set
      {
        Packer.RegenerateList();
        if (value == null)
          throw new ArgumentNullException();
        if (!((IEnumerable<string>) Packer.m_policies).Contains<string>(value))
          throw new ArgumentException("Specified policy {0} is not in the policy list.", value);
        Packer.SetSelectedPolicy(value);
      }
    }

    /// <summary>
    ///   <para>Returns all atlas textures generated for the specified atlas.</para>
    /// </summary>
    /// <param name="atlasName">Atlas name.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture2D[] GetTexturesForAtlas(string atlasName);

    /// <summary>
    ///   <para>Returns all alpha atlas textures generated for the specified atlas.</para>
    /// </summary>
    /// <param name="atlasName">Name of the atlas.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture2D[] GetAlphaTexturesForAtlas(string atlasName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RebuildAtlasCacheIfNeeded(BuildTarget target, [DefaultValue("false")] bool displayProgressBar, [DefaultValue("Execution.Normal")] Packer.Execution execution);

    [ExcludeFromDocs]
    public static void RebuildAtlasCacheIfNeeded(BuildTarget target, bool displayProgressBar)
    {
      Packer.Execution execution = Packer.Execution.Normal;
      Packer.RebuildAtlasCacheIfNeeded(target, displayProgressBar, execution);
    }

    [ExcludeFromDocs]
    public static void RebuildAtlasCacheIfNeeded(BuildTarget target)
    {
      Packer.Execution execution = Packer.Execution.Normal;
      bool displayProgressBar = false;
      Packer.RebuildAtlasCacheIfNeeded(target, displayProgressBar, execution);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void GetAtlasDataForSprite(Sprite sprite, out string atlasName, [Writable] out Texture2D atlasTexture);

    private static void SetSelectedPolicy(string value)
    {
      Packer.m_selectedPolicy = value;
      PlayerSettings.spritePackerPolicy = Packer.m_selectedPolicy;
    }

    private static void RegenerateList()
    {
      if (Packer.m_policies != null)
        return;
      List<System.Type> source = new List<System.Type>();
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        try
        {
          foreach (System.Type type in assembly.GetTypes())
          {
            if (typeof (IPackerPolicy).IsAssignableFrom(type) && type != typeof (IPackerPolicy))
              source.Add(type);
          }
        }
        catch (Exception ex)
        {
          Debug.Log((object) string.Format("SpritePacker failed to get types from {0}. Error: {1}", (object) assembly.FullName, (object) ex.Message));
        }
      }
      Packer.m_policies = source.Select<System.Type, string>((Func<System.Type, string>) (t => t.Name)).ToArray<string>();
      Packer.m_policyTypeCache = new Dictionary<string, System.Type>();
      using (List<System.Type>.Enumerator enumerator = source.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          System.Type current = enumerator.Current;
          if (Packer.m_policyTypeCache.ContainsKey(current.Name))
          {
            System.Type type = Packer.m_policyTypeCache[current.Name];
            Debug.LogError((object) string.Format("Duplicate Sprite Packer policies found: {0} and {1}. Please rename one.", (object) current.FullName, (object) type.FullName));
          }
          else
            Packer.m_policyTypeCache[current.Name] = current;
        }
      }
      Packer.m_selectedPolicy = !string.IsNullOrEmpty(PlayerSettings.spritePackerPolicy) ? PlayerSettings.spritePackerPolicy : Packer.kDefaultPolicy;
      if (((IEnumerable<string>) Packer.m_policies).Contains<string>(Packer.m_selectedPolicy))
        return;
      Packer.SetSelectedPolicy(Packer.kDefaultPolicy);
    }

    internal static string GetSelectedPolicyId()
    {
      Packer.RegenerateList();
      System.Type type = Packer.m_policyTypeCache[Packer.m_selectedPolicy];
      IPackerPolicy instance = Activator.CreateInstance(type) as IPackerPolicy;
      return string.Format("{0}::{1}", (object) type.AssemblyQualifiedName, (object) instance.GetVersion());
    }

    internal static void ExecuteSelectedPolicy(BuildTarget target, int[] textureImporterInstanceIDs)
    {
      Packer.RegenerateList();
      (Activator.CreateInstance(Packer.m_policyTypeCache[Packer.m_selectedPolicy]) as IPackerPolicy).OnGroupAtlases(target, new PackerJob(), textureImporterInstanceIDs);
    }

    internal static void SaveUnappliedTextureImporterSettings()
    {
      foreach (InspectorWindow allInspectorWindow in InspectorWindow.GetAllInspectorWindows())
      {
        foreach (Editor activeEditor in allInspectorWindow.GetTracker().activeEditors)
        {
          TextureImporterInspector importerInspector = activeEditor as TextureImporterInspector;
          if (!((UnityEngine.Object) importerInspector == (UnityEngine.Object) null) && importerInspector.HasModified() && EditorUtility.DisplayDialog("Unapplied import settings", "Unapplied import settings for '" + (importerInspector.target as TextureImporter).assetPath + "'", "Apply", "Revert"))
            importerInspector.ApplyAndImport();
        }
      }
    }

    /// <summary>
    ///   <para>Sprite Packer execution mode.</para>
    /// </summary>
    public enum Execution
    {
      Normal,
      ForceRegroup,
    }
  }
}

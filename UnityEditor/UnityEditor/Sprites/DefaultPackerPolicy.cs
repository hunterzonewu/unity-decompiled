// Decompiled with JetBrains decompiler
// Type: UnityEditor.Sprites.DefaultPackerPolicy
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Sprites
{
  internal class DefaultPackerPolicy : IPackerPolicy
  {
    private const uint kDefaultPaddingPower = 3;

    protected virtual string TagPrefix
    {
      get
      {
        return "[TIGHT]";
      }
    }

    protected virtual bool AllowTightWhenTagged
    {
      get
      {
        return true;
      }
    }

    protected virtual bool AllowRotationFlipping
    {
      get
      {
        return false;
      }
    }

    public virtual int GetVersion()
    {
      return 1;
    }

    public void OnGroupAtlases(BuildTarget target, PackerJob job, int[] textureImporterInstanceIDs)
    {
      List<DefaultPackerPolicy.Entry> source1 = new List<DefaultPackerPolicy.Entry>();
      foreach (int importerInstanceId in textureImporterInstanceIDs)
      {
        TextureImporter textureImporter = EditorUtility.InstanceIDToObject(importerInstanceId) as TextureImporter;
        TextureFormat desiredFormat;
        ColorSpace colorSpace;
        int compressionQuality;
        textureImporter.ReadTextureImportInstructions(target, out desiredFormat, out colorSpace, out compressionQuality);
        TextureImporterSettings dest = new TextureImporterSettings();
        textureImporter.ReadTextureSettings(dest);
        foreach (Sprite sprite in ((IEnumerable<UnityEngine.Object>) AssetDatabase.LoadAllAssetRepresentationsAtPath(textureImporter.assetPath)).Select<UnityEngine.Object, Sprite>((Func<UnityEngine.Object, Sprite>) (x => x as Sprite)).Where<Sprite>((Func<Sprite, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).ToArray<Sprite>())
          source1.Add(new DefaultPackerPolicy.Entry()
          {
            sprite = sprite,
            settings = {
              format = desiredFormat,
              colorSpace = colorSpace,
              compressionQuality = !TextureUtil.IsCompressedTextureFormat(desiredFormat) ? 0 : compressionQuality,
              filterMode = !Enum.IsDefined(typeof (UnityEngine.FilterMode), (object) textureImporter.filterMode) ? UnityEngine.FilterMode.Bilinear : textureImporter.filterMode,
              maxWidth = 2048,
              maxHeight = 2048,
              generateMipMaps = textureImporter.mipmapEnabled,
              enableRotation = this.AllowRotationFlipping,
              allowsAlphaSplitting = textureImporter.GetAllowsAlphaSplitting(),
              paddingPower = !textureImporter.mipmapEnabled ? (uint) EditorSettings.spritePackerPaddingPower : 3U
            },
            atlasName = this.ParseAtlasName(textureImporter.spritePackingTag),
            packingMode = this.GetPackingMode(textureImporter.spritePackingTag, dest.spriteMeshType),
            anisoLevel = textureImporter.anisoLevel
          });
        Resources.UnloadAsset((UnityEngine.Object) textureImporter);
      }
      foreach (IGrouping<string, DefaultPackerPolicy.Entry> source2 in source1.GroupBy<DefaultPackerPolicy.Entry, string, DefaultPackerPolicy.Entry>((Func<DefaultPackerPolicy.Entry, string>) (e => e.atlasName), (Func<DefaultPackerPolicy.Entry, DefaultPackerPolicy.Entry>) (e => e)))
      {
        int num = 0;
        IEnumerable<IGrouping<AtlasSettings, DefaultPackerPolicy.Entry>> source3 = source2.GroupBy<DefaultPackerPolicy.Entry, AtlasSettings, DefaultPackerPolicy.Entry>((Func<DefaultPackerPolicy.Entry, AtlasSettings>) (t => t.settings), (Func<DefaultPackerPolicy.Entry, DefaultPackerPolicy.Entry>) (t => t));
        foreach (IGrouping<AtlasSettings, DefaultPackerPolicy.Entry> grouping in source3)
        {
          string key1 = source2.Key;
          if (source3.Count<IGrouping<AtlasSettings, DefaultPackerPolicy.Entry>>() > 1)
            key1 += string.Format(" (Group {0})", (object) num);
          AtlasSettings key2 = grouping.Key;
          key2.anisoLevel = 1;
          if (key2.generateMipMaps)
          {
            foreach (DefaultPackerPolicy.Entry entry in (IEnumerable<DefaultPackerPolicy.Entry>) grouping)
            {
              if (entry.anisoLevel > key2.anisoLevel)
                key2.anisoLevel = entry.anisoLevel;
            }
          }
          job.AddAtlas(key1, key2);
          foreach (DefaultPackerPolicy.Entry entry in (IEnumerable<DefaultPackerPolicy.Entry>) grouping)
            job.AssignToAtlas(key1, entry.sprite, entry.packingMode, SpritePackingRotation.None);
          ++num;
        }
      }
    }

    protected bool IsTagPrefixed(string packingTag)
    {
      packingTag = packingTag.Trim();
      if (packingTag.Length < this.TagPrefix.Length)
        return false;
      return packingTag.Substring(0, this.TagPrefix.Length) == this.TagPrefix;
    }

    private string ParseAtlasName(string packingTag)
    {
      string packingTag1 = packingTag.Trim();
      if (this.IsTagPrefixed(packingTag1))
        packingTag1 = packingTag1.Substring(this.TagPrefix.Length).Trim();
      if (packingTag1.Length == 0)
        return "(unnamed)";
      return packingTag1;
    }

    private SpritePackingMode GetPackingMode(string packingTag, SpriteMeshType meshType)
    {
      return meshType == SpriteMeshType.Tight && this.IsTagPrefixed(packingTag) == this.AllowTightWhenTagged ? SpritePackingMode.Tight : SpritePackingMode.Rectangle;
    }

    protected class Entry
    {
      public Sprite sprite;
      public AtlasSettings settings;
      public string atlasName;
      public SpritePackingMode packingMode;
      public int anisoLevel;
    }
  }
}

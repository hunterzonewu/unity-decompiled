// Decompiled with JetBrains decompiler
// Type: UnityEditor.SubstanceImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>The SubstanceImporter class lets you access the imported ProceduralMaterial instances.</para>
  /// </summary>
  public sealed class SubstanceImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Get a list of the names of the ProceduralMaterial prototypes in the package.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string[] GetPrototypeNames();

    /// <summary>
    ///   <para>Get the number of ProceduralMaterial instances.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int GetMaterialCount();

    /// <summary>
    ///   <para>Get an array with the ProceduralMaterial instances.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public ProceduralMaterial[] GetMaterials();

    /// <summary>
    ///   <para>Clone an existing ProceduralMaterial instance.</para>
    /// </summary>
    /// <param name="material"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string CloneMaterial(ProceduralMaterial material);

    /// <summary>
    ///   <para>Instantiate a new ProceduralMaterial instance from a prototype.</para>
    /// </summary>
    /// <param name="prototypeName"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string InstantiateMaterial(string prototypeName);

    /// <summary>
    ///   <para>Destroy an existing ProceduralMaterial instance.</para>
    /// </summary>
    /// <param name="material"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void DestroyMaterial(ProceduralMaterial material);

    /// <summary>
    ///   <para>Reset the ProceduralMaterial to its default values.</para>
    /// </summary>
    /// <param name="material"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ResetMaterial(ProceduralMaterial material);

    /// <summary>
    ///   <para>Rename an existing ProceduralMaterial instance.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="name"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool RenameMaterial(ProceduralMaterial material, string name);

    /// <summary>
    ///   <para>After modifying the shader of a ProceduralMaterial, call this function to apply the changes to the importer.</para>
    /// </summary>
    /// <param name="material"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void OnShaderModified(ProceduralMaterial material);

    /// <summary>
    ///   <para>Get the alpha source of the given texture in the ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="textureName"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public ProceduralOutputType GetTextureAlphaSource(ProceduralMaterial material, string textureName);

    /// <summary>
    ///   <para>Set the alpha source of the given texture in the ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="textureName"></param>
    /// <param name="alphaSource"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetTextureAlphaSource(ProceduralMaterial material, string textureName, ProceduralOutputType alphaSource);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool GetPlatformTextureSettings(string materialName, string platform, out int maxTextureWidth, out int maxTextureHeight, out int textureFormat, out int loadBehavior);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetPlatformTextureSettings(ProceduralMaterial material, string platform, int maxTextureWidth, int maxTextureHeight, int textureFormat, int loadBehavior);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private ScriptingProceduralMaterialInformation GetMaterialInformation(ProceduralMaterial material);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetMaterialInformation(ProceduralMaterial material, ScriptingProceduralMaterialInformation scriptingProcMatInfo);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CanShaderPropertyHostProceduralOutput(string name, ProceduralOutputType substanceType);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void ClearPlatformTextureSettings(string materialName, string platform);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void OnTextureInformationsChanged(ProceduralTexture texture);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void ExportBitmapsInternal(ProceduralMaterial material, string exportPath, bool alphaRemap);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsSubstanceParented(ProceduralTexture texture, ProceduralMaterial material);

    /// <summary>
    ///   <para>Get the material offset, which is used for all the textures that are part of this ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    public Vector2 GetMaterialOffset(ProceduralMaterial material)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      return this.GetMaterialInformation(material).offset;
    }

    /// <summary>
    ///   <para>Set the material offset, which is used for all the textures that are part of this ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="offset"></param>
    public void SetMaterialOffset(ProceduralMaterial material, Vector2 offset)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      ScriptingProceduralMaterialInformation materialInformation = this.GetMaterialInformation(material);
      materialInformation.offset = offset;
      this.SetMaterialInformation(material, materialInformation);
    }

    /// <summary>
    ///   <para>Get the material scale, which is used for all the textures that are part of this ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    public Vector2 GetMaterialScale(ProceduralMaterial material)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      return this.GetMaterialInformation(material).scale;
    }

    /// <summary>
    ///   <para>Set the material scale, which is used for all the textures that are part of this ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="scale"></param>
    public void SetMaterialScale(ProceduralMaterial material, Vector2 scale)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      ScriptingProceduralMaterialInformation materialInformation = this.GetMaterialInformation(material);
      materialInformation.scale = scale;
      this.SetMaterialInformation(material, materialInformation);
    }

    /// <summary>
    ///   <para>Check if the ProceduralMaterial needs to force generation of all its outputs.</para>
    /// </summary>
    /// <param name="material"></param>
    public bool GetGenerateAllOutputs(ProceduralMaterial material)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      return this.GetMaterialInformation(material).generateAllOutputs;
    }

    /// <summary>
    ///   <para>Specify if the ProceduralMaterial needs to force generation of all its outputs.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="generated"></param>
    public void SetGenerateAllOutputs(ProceduralMaterial material, bool generated)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      ScriptingProceduralMaterialInformation materialInformation = this.GetMaterialInformation(material);
      materialInformation.generateAllOutputs = generated;
      this.SetMaterialInformation(material, materialInformation);
    }

    /// <summary>
    ///   <para>Get the ProceduralMaterial animation update rate in millisecond.</para>
    /// </summary>
    /// <param name="material"></param>
    public int GetAnimationUpdateRate(ProceduralMaterial material)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      return this.GetMaterialInformation(material).animationUpdateRate;
    }

    /// <summary>
    ///   <para>Set the ProceduralMaterial animation update rate in millisecond.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="animation_update_rate"></param>
    public void SetAnimationUpdateRate(ProceduralMaterial material, int animation_update_rate)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      ScriptingProceduralMaterialInformation materialInformation = this.GetMaterialInformation(material);
      materialInformation.animationUpdateRate = animation_update_rate;
      this.SetMaterialInformation(material, materialInformation);
    }

    /// <summary>
    ///   <para>Return true if the mipmaps are generated for this ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    public bool GetGenerateMipMaps(ProceduralMaterial material)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      return this.GetMaterialInformation(material).generateMipMaps;
    }

    /// <summary>
    ///   <para>Force the generation of mipmaps for this ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="mode"></param>
    public void SetGenerateMipMaps(ProceduralMaterial material, bool mode)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      ScriptingProceduralMaterialInformation materialInformation = this.GetMaterialInformation(material);
      materialInformation.generateMipMaps = mode;
      this.SetMaterialInformation(material, materialInformation);
    }

    internal static bool IsProceduralTextureSlot(Material material, Texture tex, string name)
    {
      if (material is ProceduralMaterial && tex is ProceduralTexture && SubstanceImporter.CanShaderPropertyHostProceduralOutput(name, (tex as ProceduralTexture).GetProceduralOutputType()))
        return SubstanceImporter.IsSubstanceParented(tex as ProceduralTexture, material as ProceduralMaterial);
      return false;
    }

    /// <summary>
    ///   <para>Export the bitmaps generated by a ProceduralMaterial as TGA files.</para>
    /// </summary>
    /// <param name="material">The ProceduralMaterial whose output textures will be saved.</param>
    /// <param name="exportPath">Path to a folder where the output bitmaps will be saved. The folder will be created if it doesn't already exist.</param>
    /// <param name="alphaRemap">Indicates whether alpha channel remapping should be performed.</param>
    public void ExportBitmaps(ProceduralMaterial material, string exportPath, bool alphaRemap)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      if (exportPath == string.Empty)
        throw new ArgumentException("Invalid export path specified");
      if (!Directory.CreateDirectory(exportPath).Exists)
        throw new ArgumentException("Export folder " + exportPath + " doesn't exist and cannot be created.");
      this.ExportBitmapsInternal(material, exportPath, alphaRemap);
    }

    /// <summary>
    ///   <para>Export a XML preset string with the value of all parameters of a given ProceduralMaterial to the specified folder.</para>
    /// </summary>
    /// <param name="material">The ProceduralMaterial whose preset string will be saved.</param>
    /// <param name="exportPath">Path to a folder where the preset file will be saved. The folder will be created if it doesn't already exist.</param>
    public void ExportPreset(ProceduralMaterial material, string exportPath)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      if (exportPath == string.Empty)
        throw new ArgumentException("Invalid export path specified");
      if (!Directory.CreateDirectory(exportPath).Exists)
        throw new ArgumentException("Export folder " + exportPath + " doesn't exist and cannot be created.");
      File.WriteAllText(Path.Combine(exportPath, material.name + ".sbsprs"), material.preset);
    }
  }
}

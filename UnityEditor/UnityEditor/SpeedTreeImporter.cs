// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpeedTreeImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  /// <summary>
  ///   <para>AssetImportor for importing SpeedTree model assets.</para>
  /// </summary>
  public sealed class SpeedTreeImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Gets an array of name strings for wind quality value.</para>
    /// </summary>
    public static readonly string[] windQualityNames = new string[6]
    {
      "None",
      "Fastest",
      "Fast",
      "Better",
      "Best",
      "Palm"
    };

    /// <summary>
    ///   <para>Tells if the SPM file has been previously imported.</para>
    /// </summary>
    public bool hasImported { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the folder path where generated materials will be placed in.</para>
    /// </summary>
    public string materialFolderPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>How much to scale the tree model compared to what is in the .spm file.</para>
    /// </summary>
    public float scaleFactor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets a default main color.</para>
    /// </summary>
    public Color mainColor
    {
      get
      {
        Color color;
        this.INTERNAL_get_mainColor(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_mainColor(ref value);
      }
    }

    /// <summary>
    ///   <para>Gets and sets a default specular color.</para>
    /// </summary>
    public Color specColor
    {
      get
      {
        Color color;
        this.INTERNAL_get_specColor(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_specColor(ref value);
      }
    }

    /// <summary>
    ///   <para>Gets and sets a default Hue variation color and amount (in alpha).</para>
    /// </summary>
    public Color hueVariation
    {
      get
      {
        Color color;
        this.INTERNAL_get_hueVariation(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_hueVariation(ref value);
      }
    }

    /// <summary>
    ///   <para>Gets and sets a default Shininess value.</para>
    /// </summary>
    public float shininess { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets a default alpha test reference values.</para>
    /// </summary>
    public float alphaTestRef { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Tells if there is a billboard LOD.</para>
    /// </summary>
    public bool hasBillboard { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Enables smooth LOD transitions.</para>
    /// </summary>
    public bool enableSmoothLODTransition { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Indicates if the cross-fade LOD transition, applied to the last mesh LOD and the billboard, should be animated.</para>
    /// </summary>
    public bool animateCrossFading { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Proportion of the last 3D mesh LOD region width which is used for cross-fading to billboard tree.</para>
    /// </summary>
    public float billboardTransitionCrossFadeWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Proportion of the billboard LOD region width which is used for fading out the billboard.</para>
    /// </summary>
    public float fadeOutWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of floats of each LOD's screen height value.</para>
    /// </summary>
    public float[] LODHeights { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of booleans to enable shadow casting for each LOD.</para>
    /// </summary>
    public bool[] castShadows { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of booleans to enable shadow receiving for each LOD.</para>
    /// </summary>
    public bool[] receiveShadows { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of booleans to enable Light Probe lighting for each LOD.</para>
    /// </summary>
    public bool[] useLightProbes { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of Reflection Probe usages for each LOD.</para>
    /// </summary>
    public ReflectionProbeUsage[] reflectionProbeUsages { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of booleans to enable normal mapping for each LOD.</para>
    /// </summary>
    public bool[] enableBump { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets and sets an array of booleans to enable Hue variation effect for each LOD.</para>
    /// </summary>
    public bool[] enableHue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the best-possible wind quality on this asset (configured in SpeedTree modeler).</para>
    /// </summary>
    public int bestWindQuality { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Gets and sets an array of integers of the wind qualities on each LOD. Values will be clampped by BestWindQuality internally.</para>
    /// </summary>
    public int[] windQualities { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal bool materialsShouldBeRegenerated { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_mainColor(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_mainColor(ref Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_specColor(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_specColor(ref Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_hueVariation(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_hueVariation(ref Color value);

    /// <summary>
    ///   <para>Generates all necessary materials under materialFolderPath. If Version Control is enabled please first check out the folder.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void GenerateMaterials();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SetMaterialVersionToCurrent();
  }
}

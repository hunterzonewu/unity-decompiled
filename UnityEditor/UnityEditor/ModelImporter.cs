// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEditor.Animations;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Model importer lets you modify import settings from editor scripts.</para>
  /// </summary>
  public class ModelImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Material generation options.</para>
    /// </summary>
    [Obsolete("Use importMaterials, materialName and materialSearch instead")]
    public ModelImporterGenerateMaterials generateMaterials { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Import materials from file.</para>
    /// </summary>
    public bool importMaterials { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Material naming setting.</para>
    /// </summary>
    public ModelImporterMaterialName materialName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Existing material search setting.</para>
    /// </summary>
    public ModelImporterMaterialSearch materialSearch { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Global scale factor for importing.</para>
    /// </summary>
    public float globalScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is useFileUnits supported for this asset.</para>
    /// </summary>
    public bool isUseFileUnitsSupported { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Detect file units and import as 1FileUnit=1UnityUnit, otherwise it will import as 1cm=1UnityUnit.</para>
    /// </summary>
    public bool useFileUnits { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>File scale factor (if available) or default one. (Read-only).</para>
    /// </summary>
    public float fileScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is FileScale was used when importing.</para>
    /// </summary>
    public bool isFileScaleUsed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Controls import of BlendShapes.</para>
    /// </summary>
    public bool importBlendShapes { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Add to imported meshes.</para>
    /// </summary>
    public bool addCollider { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Smoothing angle (in degrees) for calculating normals.</para>
    /// </summary>
    public float normalSmoothingAngle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should tangents be split across UV seams.</para>
    /// </summary>
    [Obsolete("Please use tangentImportMode instead")]
    public bool splitTangentsAcrossSeams
    {
      get
      {
        return this.importTangents == ModelImporterTangents.CalculateLegacyWithSplitTangents;
      }
      set
      {
        if (this.importTangents == ModelImporterTangents.CalculateLegacyWithSplitTangents && !value)
        {
          this.importTangents = ModelImporterTangents.CalculateLegacy;
        }
        else
        {
          if (this.importTangents != ModelImporterTangents.CalculateLegacy || !value)
            return;
          this.importTangents = ModelImporterTangents.CalculateLegacyWithSplitTangents;
        }
      }
    }

    /// <summary>
    ///   <para>Swap primary and secondary UV channels when importing.</para>
    /// </summary>
    public bool swapUVChannels { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Generate secondary UV set for lightmapping.</para>
    /// </summary>
    public bool generateSecondaryUV { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Threshold for angle distortion (in degrees) when generating secondary UV.</para>
    /// </summary>
    public float secondaryUVAngleDistortion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Threshold for area distortion when generating secondary UV.</para>
    /// </summary>
    public float secondaryUVAreaDistortion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Hard angle (in degrees) for generating secondary UV.</para>
    /// </summary>
    public float secondaryUVHardAngle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Margin to be left between charts when packing secondary UV.</para>
    /// </summary>
    public float secondaryUVPackMargin { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Animation generation options.</para>
    /// </summary>
    public ModelImporterGenerateAnimations generateAnimations { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Generates the list of all imported take.</para>
    /// </summary>
    public TakeInfo[] importedTakeInfos { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Generates the list of all imported Transforms.</para>
    /// </summary>
    public string[] transformPaths { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Generates the list of all imported Animations.</para>
    /// </summary>
    public string[] referencedClips { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Are mesh vertices and indices accessible from script?</para>
    /// </summary>
    public bool isReadable { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Vertex optimization setting.</para>
    /// </summary>
    public bool optimizeMesh { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Normals import mode.</para>
    /// </summary>
    [Obsolete("normalImportMode is deprecated. Use importNormals instead")]
    public ModelImporterTangentSpaceMode normalImportMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Tangents import mode.</para>
    /// </summary>
    [Obsolete("tangentImportMode is deprecated. Use importTangents instead")]
    public ModelImporterTangentSpaceMode tangentImportMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Use normals vectors from file.</para>
    /// </summary>
    public ModelImporterNormals importNormals { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Use tangent vectors from file.</para>
    /// </summary>
    public ModelImporterTangents importTangents { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Bake Inverse Kinematics (IK) when importing.</para>
    /// </summary>
    public bool bakeIK { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is Bake Inverse Kinematics (IK) supported by this importer.</para>
    /// </summary>
    public bool isBakeIKSupported { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("use resampleCurves instead.")]
    public bool resampleRotations { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///         <para>If set to false, the importer will not resample animation curves when possible. Read more about.
    /// 
    /// Notes:
    /// 
    /// - Some unsupported FBX features (such as PreRotation or PostRotation on transforms) will override this setting. In these situations, animation curves will still be resampled even if the setting is disabled. For best results, avoid using PreRotation, PostRotation and GetRotationPivot.
    /// 
    /// - This option was introduced in Version 5.3. Prior to this version, Unity's import behaviour was as if this option was always enabled. Therefore enabling the option gives the same behaviour as pre-5.3 animation import.
    ///         </para>
    ///       </summary>
    public bool resampleCurves { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is import of tangents supported by this importer.</para>
    /// </summary>
    public bool isTangentImportSupported { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("Use animationCompression instead", true)]
    private bool reduceKeyframes
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Mesh compression setting.</para>
    /// </summary>
    public ModelImporterMeshCompression meshCompression { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Import animation from file.</para>
    /// </summary>
    public bool importAnimation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Animation optimization setting.</para>
    /// </summary>
    public bool optimizeGameObjects { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Animation optimization setting.</para>
    /// </summary>
    public string[] extraExposedTransformPaths { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Animation compression setting.</para>
    /// </summary>
    public ModelImporterAnimationCompression animationCompression { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Allowed error of animation rotation compression.</para>
    /// </summary>
    public float animationRotationError { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Allowed error of animation position compression.</para>
    /// </summary>
    public float animationPositionError { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Allowed error of animation scale compression.</para>
    /// </summary>
    public float animationScaleError { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The default wrap mode for the generated animation clips.</para>
    /// </summary>
    public WrapMode animationWrapMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Animator generation mode.</para>
    /// </summary>
    public ModelImporterAnimationType animationType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Controls how much oversampling is used when importing humanoid animations for retargeting.</para>
    /// </summary>
    public ModelImporterHumanoidOversampling humanoidOversampling { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The path of the transform used to generation the motion of the animation.</para>
    /// </summary>
    public string motionNodeName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Imports the HumanDescription from the given Avatar.</para>
    /// </summary>
    public Avatar sourceAvatar
    {
      get
      {
        return this.sourceAvatarInternal;
      }
      set
      {
        Avatar avatar = value;
        if ((UnityEngine.Object) value != (UnityEngine.Object) null)
        {
          ModelImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) value)) as ModelImporter;
          if ((UnityEngine.Object) atPath != (UnityEngine.Object) null)
          {
            this.humanDescription = atPath.humanDescription;
          }
          else
          {
            Debug.LogError((object) "Avatar must be from a ModelImporter, otherwise use ModelImporter.humanDescription");
            avatar = (Avatar) null;
          }
        }
        this.sourceAvatarInternal = avatar;
      }
    }

    internal Avatar sourceAvatarInternal { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The human description that is used to generate an Avatar during the import process.</para>
    /// </summary>
    public HumanDescription humanDescription
    {
      get
      {
        HumanDescription humanDescription;
        this.INTERNAL_get_humanDescription(out humanDescription);
        return humanDescription;
      }
      set
      {
        this.INTERNAL_set_humanDescription(ref value);
      }
    }

    [Obsolete("splitAnimations has been deprecated please use clipAnimations instead.", true)]
    public bool splitAnimations
    {
      get
      {
        return this.clipAnimations.Length != 0;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Animation clips to split animation into.</para>
    /// </summary>
    public ModelImporterClipAnimation[] clipAnimations { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Generate a list of all default animation clip based on TakeInfo.</para>
    /// </summary>
    public ModelImporterClipAnimation[] defaultClipAnimations { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal bool isAssetOlderOr42 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_humanDescription(out HumanDescription value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_humanDescription(ref HumanDescription value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateSkeletonPose(SkeletonBone[] skeletonBones, SerializedProperty serializedProperty);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateTransformMask(AvatarMask mask, SerializedProperty serializedProperty);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal AnimationClip GetPreviewAnimationClipForTake(string takeName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal string CalculateBestFittingPreviewGameObject();
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.BlendTree
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>Blend trees are used to blend continuously animation between their childs. They can either be 1D or 2D.</para>
  /// </summary>
  public sealed class BlendTree : Motion
  {
    /// <summary>
    ///   <para>Parameter that is used to compute the blending weight of the childs in 1D blend trees or on the X axis of a 2D blend tree.</para>
    /// </summary>
    public string blendParameter { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Parameter that is used to compute the blending weight of the childs on the Y axis of a 2D blend tree.</para>
    /// </summary>
    public string blendParameterY { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The Blending type can be either 1D or different types of 2D.</para>
    /// </summary>
    public BlendTreeType blendType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A copy of the list of the blend tree child motions.</para>
    /// </summary>
    public ChildMotion[] children { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>When active, the children's thresholds are automatically spread between 0 and 1.</para>
    /// </summary>
    public bool useAutomaticThresholds { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Sets the minimum threshold that will be used by the ChildMotion. Only used when useAutomaticThresholds is true.</para>
    /// </summary>
    public float minThreshold { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Sets the maximum threshold that will be used by the ChildMotion. Only used when useAutomaticThresholds is true.</para>
    /// </summary>
    public float maxThreshold { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal int recursiveBlendParameterCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public BlendTree()
    {
      BlendTree.Internal_Create(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create(BlendTree mono);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SetDirectBlendTreeParameter(int index, string parameter);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal string GetDirectBlendTreeParameter(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal int GetChildCount();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal Motion GetChildMotion(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SortChildren();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal string GetRecursiveBlendParameter(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal float GetRecursiveBlendParameterMin(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal float GetRecursiveBlendParameterMax(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SetInputBlendValue(string blendValueName, float value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal float GetInputBlendValue(string blendValueName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal AnimationClip[] GetAnimationClipsFlattened();

    /// <summary>
    ///   <para>Utility function to add a child motion to a blend trees.</para>
    /// </summary>
    /// <param name="motion">The motion to add as child.</param>
    /// <param name="position">The position of the child. When using 2D blend trees.</param>
    /// <param name="threshold">The threshold of the child. When using 1D blend trees.</param>
    public void AddChild(Motion motion)
    {
      this.AddChild(motion, Vector2.zero, 0.0f);
    }

    /// <summary>
    ///   <para>Utility function to add a child motion to a blend trees.</para>
    /// </summary>
    /// <param name="motion">The motion to add as child.</param>
    /// <param name="position">The position of the child. When using 2D blend trees.</param>
    /// <param name="threshold">The threshold of the child. When using 1D blend trees.</param>
    public void AddChild(Motion motion, Vector2 position)
    {
      this.AddChild(motion, position, 0.0f);
    }

    /// <summary>
    ///   <para>Utility function to add a child motion to a blend trees.</para>
    /// </summary>
    /// <param name="motion">The motion to add as child.</param>
    /// <param name="position">The position of the child. When using 2D blend trees.</param>
    /// <param name="threshold">The threshold of the child. When using 1D blend trees.</param>
    public void AddChild(Motion motion, float threshold)
    {
      this.AddChild(motion, Vector2.zero, threshold);
    }

    /// <summary>
    ///   <para>Utility function to remove the child of a blend tree.</para>
    /// </summary>
    /// <param name="index">The index of the blend tree to remove.</param>
    public void RemoveChild(int index)
    {
      Undo.RecordObject((Object) this, "Remove Child");
      ChildMotion[] children = this.children;
      ArrayUtility.RemoveAt<ChildMotion>(ref children, index);
      this.children = children;
    }

    internal void AddChild(Motion motion, Vector2 position, float threshold)
    {
      Undo.RecordObject((Object) this, "Added BlendTree Child");
      ChildMotion[] children = this.children;
      ArrayUtility.Add<ChildMotion>(ref children, new ChildMotion()
      {
        timeScale = 1f,
        motion = motion,
        position = position,
        threshold = threshold,
        directBlendParameter = "Blend"
      });
      this.children = children;
    }

    /// <summary>
    ///   <para>Utility function to add a child blend tree to a blend tree.</para>
    /// </summary>
    /// <param name="position">The position of the child. When using 2D blend trees.</param>
    /// <param name="threshold">The threshold of the child. When using 1D blend trees.</param>
    public BlendTree CreateBlendTreeChild(float threshold)
    {
      return this.CreateBlendTreeChild(Vector2.zero, threshold);
    }

    /// <summary>
    ///   <para>Utility function to add a child blend tree to a blend tree.</para>
    /// </summary>
    /// <param name="position">The position of the child. When using 2D blend trees.</param>
    /// <param name="threshold">The threshold of the child. When using 1D blend trees.</param>
    public BlendTree CreateBlendTreeChild(Vector2 position)
    {
      return this.CreateBlendTreeChild(position, 0.0f);
    }

    internal bool HasChild(BlendTree childTree, bool recursive)
    {
      foreach (ChildMotion child in this.children)
      {
        if ((Object) child.motion == (Object) childTree || recursive && child.motion is BlendTree && (child.motion as BlendTree).HasChild(childTree, true))
          return true;
      }
      return false;
    }

    internal BlendTree CreateBlendTreeChild(Vector2 position, float threshold)
    {
      Undo.RecordObject((Object) this, "Created BlendTree Child");
      BlendTree blendTree = new BlendTree();
      blendTree.name = "BlendTree";
      blendTree.hideFlags = HideFlags.HideInHierarchy;
      if (AssetDatabase.GetAssetPath((Object) this) != string.Empty)
        AssetDatabase.AddObjectToAsset((Object) blendTree, AssetDatabase.GetAssetPath((Object) this));
      this.AddChild((Motion) blendTree, position, threshold);
      return blendTree;
    }
  }
}

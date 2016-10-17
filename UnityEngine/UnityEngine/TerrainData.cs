// Decompiled with JetBrains decompiler
// Type: UnityEngine.TerrainData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The TerrainData class stores heightmaps, detail mesh positions, tree instances, and terrain texture alpha maps.</para>
  /// </summary>
  public sealed class TerrainData : Object
  {
    private static readonly int kMaximumResolution = TerrainData.Internal_GetMaximumResolution();
    private static readonly int kMinimumDetailResolutionPerPatch = TerrainData.Internal_GetMinimumDetailResolutionPerPatch();
    private static readonly int kMaximumDetailResolutionPerPatch = TerrainData.Internal_GetMaximumDetailResolutionPerPatch();
    private static readonly int kMaximumDetailPatchCount = TerrainData.Internal_GetMaximumDetailPatchCount();
    private static readonly int kMinimumAlphamapResolution = TerrainData.Internal_GetMinimumAlphamapResolution();
    private static readonly int kMaximumAlphamapResolution = TerrainData.Internal_GetMaximumAlphamapResolution();
    private static readonly int kMinimumBaseMapResolution = TerrainData.Internal_GetMinimumBaseMapResolution();
    private static readonly int kMaximumBaseMapResolution = TerrainData.Internal_GetMaximumBaseMapResolution();

    /// <summary>
    ///   <para>Width of the terrain in samples (Read Only).</para>
    /// </summary>
    public int heightmapWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Height of the terrain in samples (Read Only).</para>
    /// </summary>
    public int heightmapHeight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Resolution of the heightmap.</para>
    /// </summary>
    public int heightmapResolution
    {
      get
      {
        return this.Internal_heightmapResolution;
      }
      set
      {
        int num = value;
        if (value < 0 || value > TerrainData.kMaximumResolution)
        {
          Debug.LogWarning((object) ("heightmapResolution is clamped to the range of [0, " + (object) TerrainData.kMaximumResolution + "]."));
          num = Math.Min(TerrainData.kMaximumResolution, Math.Max(value, 0));
        }
        this.Internal_heightmapResolution = num;
      }
    }

    private int Internal_heightmapResolution { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The size of each heightmap sample.</para>
    /// </summary>
    public Vector3 heightmapScale
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_heightmapScale(out vector3);
        return vector3;
      }
    }

    /// <summary>
    ///   <para>The total size in world units of the terrain.</para>
    /// </summary>
    public Vector3 size
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_size(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_size(ref value);
      }
    }

    /// <summary>
    ///   <para>The thickness of the terrain used for collision detection.</para>
    /// </summary>
    public float thickness { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Strength of the waving grass in the terrain.</para>
    /// </summary>
    public float wavingGrassStrength { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Amount of waving grass in the terrain.</para>
    /// </summary>
    public float wavingGrassAmount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Speed of the waving grass.</para>
    /// </summary>
    public float wavingGrassSpeed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Color of the waving grass that the terrain has.</para>
    /// </summary>
    public Color wavingGrassTint
    {
      get
      {
        Color color;
        this.INTERNAL_get_wavingGrassTint(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_wavingGrassTint(ref value);
      }
    }

    /// <summary>
    ///   <para>Detail width of the TerrainData.</para>
    /// </summary>
    public int detailWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Detail height of the TerrainData.</para>
    /// </summary>
    public int detailHeight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Detail Resolution of the TerrainData.</para>
    /// </summary>
    public int detailResolution { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal int detailResolutionPerPatch { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Contains the detail texture/meshes that the terrain has.</para>
    /// </summary>
    public DetailPrototype[] detailPrototypes { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Contains the current trees placed in the terrain.</para>
    /// </summary>
    public TreeInstance[] treeInstances { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the number of tree instances.</para>
    /// </summary>
    public int treeInstanceCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The list of tree prototypes this are the ones available in the inspector.</para>
    /// </summary>
    public TreePrototype[] treePrototypes { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Number of alpha map layers.</para>
    /// </summary>
    public int alphamapLayers { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Resolution of the alpha map.</para>
    /// </summary>
    public int alphamapResolution
    {
      get
      {
        return this.Internal_alphamapResolution;
      }
      set
      {
        int num = value;
        if (value < TerrainData.kMinimumAlphamapResolution || value > TerrainData.kMaximumAlphamapResolution)
        {
          Debug.LogWarning((object) ("alphamapResolution is clamped to the range of [" + (object) TerrainData.kMinimumAlphamapResolution + ", " + (object) TerrainData.kMaximumAlphamapResolution + "]."));
          num = Math.Min(TerrainData.kMaximumAlphamapResolution, Math.Max(value, TerrainData.kMinimumAlphamapResolution));
        }
        this.Internal_alphamapResolution = num;
      }
    }

    private int Internal_alphamapResolution { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Width of the alpha map.</para>
    /// </summary>
    public int alphamapWidth
    {
      get
      {
        return this.alphamapResolution;
      }
    }

    /// <summary>
    ///   <para>Height of the alpha map.</para>
    /// </summary>
    public int alphamapHeight
    {
      get
      {
        return this.alphamapResolution;
      }
    }

    /// <summary>
    ///   <para>Resolution of the base map used for rendering far patches on the terrain.</para>
    /// </summary>
    public int baseMapResolution
    {
      get
      {
        return this.Internal_baseMapResolution;
      }
      set
      {
        int num = value;
        if (value < TerrainData.kMinimumBaseMapResolution || value > TerrainData.kMaximumBaseMapResolution)
        {
          Debug.LogWarning((object) ("baseMapResolution is clamped to the range of [" + (object) TerrainData.kMinimumBaseMapResolution + ", " + (object) TerrainData.kMaximumBaseMapResolution + "]."));
          num = Math.Min(TerrainData.kMaximumBaseMapResolution, Math.Max(value, TerrainData.kMinimumBaseMapResolution));
        }
        this.Internal_baseMapResolution = num;
      }
    }

    private int Internal_baseMapResolution { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    private int alphamapTextureCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Alpha map textures used by the Terrain. Used by Terrain Inspector for undo.</para>
    /// </summary>
    public Texture2D[] alphamapTextures
    {
      get
      {
        Texture2D[] texture2DArray = new Texture2D[this.alphamapTextureCount];
        for (int index = 0; index < texture2DArray.Length; ++index)
          texture2DArray[index] = this.GetAlphamapTexture(index);
        return texture2DArray;
      }
    }

    /// <summary>
    ///   <para>Splat texture used by the terrain.</para>
    /// </summary>
    public SplatPrototype[] splatPrototypes { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public TerrainData()
    {
      this.Internal_Create(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetMaximumResolution();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetMinimumDetailResolutionPerPatch();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetMaximumDetailResolutionPerPatch();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetMaximumDetailPatchCount();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetMinimumAlphamapResolution();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetMaximumAlphamapResolution();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetMinimumBaseMapResolution();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_GetMaximumBaseMapResolution();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void Internal_Create([Writable] TerrainData terrainData);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool HasUser(GameObject user);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void AddUser(GameObject user);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RemoveUser(GameObject user);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_heightmapScale(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_size(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_size(ref Vector3 value);

    /// <summary>
    ///   <para>Gets the height at a certain point x,y.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float GetHeight(int x, int y);

    /// <summary>
    ///   <para>Gets an interpolated height at a point x,y.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float GetInterpolatedHeight(float x, float y);

    /// <summary>
    ///   <para>Get an array of heightmap samples.</para>
    /// </summary>
    /// <param name="xBase">First x index of heightmap samples to retrieve.</param>
    /// <param name="yBase">First y index of heightmap samples to retrieve.</param>
    /// <param name="width">Number of samples to retrieve along the heightmap's x axis.</param>
    /// <param name="height">Number of samples to retrieve along the heightmap's y axis.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float[,] GetHeights(int xBase, int yBase, int width, int height);

    public void SetHeights(int xBase, int yBase, float[,] heights)
    {
      if (heights == null)
        throw new NullReferenceException();
      if (xBase + heights.GetLength(1) > this.heightmapWidth || xBase + heights.GetLength(1) < 0 || (yBase + heights.GetLength(0) < 0 || xBase < 0) || (yBase < 0 || yBase + heights.GetLength(0) > this.heightmapHeight))
        throw new ArgumentException(UnityString.Format("X or Y base out of bounds. Setting up to {0}x{1} while map size is {2}x{3}", (object) (xBase + heights.GetLength(1)), (object) (yBase + heights.GetLength(0)), (object) this.heightmapWidth, (object) this.heightmapHeight));
      this.Internal_SetHeights(xBase, yBase, heights.GetLength(1), heights.GetLength(0), heights);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetHeights(int xBase, int yBase, int width, int height, float[,] heights);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetHeightsDelayLOD(int xBase, int yBase, int width, int height, float[,] heights);

    public void SetHeightsDelayLOD(int xBase, int yBase, float[,] heights)
    {
      if (heights == null)
        throw new ArgumentNullException("heights");
      int length1 = heights.GetLength(0);
      int length2 = heights.GetLength(1);
      if (xBase < 0 || xBase + length2 < 0 || xBase + length2 > this.heightmapWidth)
        throw new ArgumentException(UnityString.Format("X out of bounds - trying to set {0}-{1} but the terrain ranges from 0-{2}", (object) xBase, (object) (xBase + length2), (object) this.heightmapWidth));
      if (yBase < 0 || yBase + length1 < 0 || yBase + length1 > this.heightmapHeight)
        throw new ArgumentException(UnityString.Format("Y out of bounds - trying to set {0}-{1} but the terrain ranges from 0-{2}", (object) yBase, (object) (yBase + length1), (object) this.heightmapHeight));
      this.Internal_SetHeightsDelayLOD(xBase, yBase, length2, length1, heights);
    }

    /// <summary>
    ///   <para>Gets the gradient of the terrain at point &amp;amp;amp;amp;lt;x,y&amp;amp;amp;amp;gt;.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float GetSteepness(float x, float y);

    /// <summary>
    ///   <para>Get an interpolated normal at a given location.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Vector3 GetInterpolatedNormal(float x, float y)
    {
      Vector3 vector3;
      TerrainData.INTERNAL_CALL_GetInterpolatedNormal(this, x, y, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetInterpolatedNormal(TerrainData self, float x, float y, out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal int GetAdjustedSize(int size);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_wavingGrassTint(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_wavingGrassTint(ref Color value);

    /// <summary>
    ///   <para>Set the resolution of the detail map.</para>
    /// </summary>
    /// <param name="detailResolution">Specifies the number of pixels in the detail resolution map. A larger detailResolution, leads to more accurate detail object painting.</param>
    /// <param name="resolutionPerPatch">Specifies the size in pixels of each individually rendered detail patch. A larger number reduces draw calls, but might increase triangle count since detail patches are culled on a per batch basis. A recommended value is 16. If you use a very large detail object distance and your grass is very sparse, it makes sense to increase the value.</param>
    public void SetDetailResolution(int detailResolution, int resolutionPerPatch)
    {
      if (detailResolution < 0)
      {
        Debug.LogWarning((object) "detailResolution must not be negative.");
        detailResolution = 0;
      }
      if (resolutionPerPatch < TerrainData.kMinimumDetailResolutionPerPatch || resolutionPerPatch > TerrainData.kMaximumDetailResolutionPerPatch)
      {
        Debug.LogWarning((object) ("resolutionPerPatch is clamped to the range of [" + (object) TerrainData.kMinimumDetailResolutionPerPatch + ", " + (object) TerrainData.kMaximumDetailResolutionPerPatch + "]."));
        resolutionPerPatch = Math.Min(TerrainData.kMaximumDetailResolutionPerPatch, Math.Max(resolutionPerPatch, TerrainData.kMinimumDetailResolutionPerPatch));
      }
      int num = detailResolution / resolutionPerPatch;
      if (num > TerrainData.kMaximumDetailPatchCount)
      {
        Debug.LogWarning((object) ("Patch count (detailResolution / resolutionPerPatch) is clamped to the range of [0, " + (object) TerrainData.kMaximumDetailPatchCount + "]."));
        num = Math.Min(TerrainData.kMaximumDetailPatchCount, Math.Max(num, 0));
      }
      this.Internal_SetDetailResolution(num, resolutionPerPatch);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetDetailResolution(int patchCount, int resolutionPerPatch);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void ResetDirtyDetails();

    /// <summary>
    ///   <para>Reloads all the values of the available prototypes (ie, detail mesh assets) in the TerrainData Object.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void RefreshPrototypes();

    /// <summary>
    ///   <para>Returns an array of all supported detail layer indices in the area.</para>
    /// </summary>
    /// <param name="xBase"></param>
    /// <param name="yBase"></param>
    /// <param name="totalWidth"></param>
    /// <param name="totalHeight"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int[] GetSupportedLayers(int xBase, int yBase, int totalWidth, int totalHeight);

    /// <summary>
    ///   <para>Returns a 2D array of the detail object density in the specific location.</para>
    /// </summary>
    /// <param name="xBase"></param>
    /// <param name="yBase"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="layer"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int[,] GetDetailLayer(int xBase, int yBase, int width, int height, int layer);

    public void SetDetailLayer(int xBase, int yBase, int layer, int[,] details)
    {
      this.Internal_SetDetailLayer(xBase, yBase, details.GetLength(1), details.GetLength(0), layer, details);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetDetailLayer(int xBase, int yBase, int totalWidth, int totalHeight, int detailIndex, int[,] data);

    /// <summary>
    ///   <para>Get the tree instance at the specified index. It is used as a faster version of treeInstances[index] as this function doesn't create the entire tree instances array.</para>
    /// </summary>
    /// <param name="index">The index of the tree instance.</param>
    public TreeInstance GetTreeInstance(int index)
    {
      TreeInstance treeInstance;
      TerrainData.INTERNAL_CALL_GetTreeInstance(this, index, out treeInstance);
      return treeInstance;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetTreeInstance(TerrainData self, int index, out TreeInstance value);

    /// <summary>
    ///   <para>Set the tree instance with new parameters at the specified index. However, TreeInstance.prototypeIndex and TreeInstance.position can not be changed otherwise an ArgumentException will be thrown.</para>
    /// </summary>
    /// <param name="index">The index of the tree instance.</param>
    /// <param name="instance">The new TreeInstance value.</param>
    public void SetTreeInstance(int index, TreeInstance instance)
    {
      TerrainData.INTERNAL_CALL_SetTreeInstance(this, index, ref instance);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetTreeInstance(TerrainData self, int index, ref TreeInstance instance);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RemoveTreePrototype(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RecalculateTreePositions();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RemoveDetailPrototype(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool NeedUpgradeScaledTreePrototypes();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void UpgradeScaledTreePrototype();

    /// <summary>
    ///   <para>Returns the alpha map at a position x, y given a width and height.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float[,,] GetAlphamaps(int x, int y, int width, int height);

    public void SetAlphamaps(int x, int y, float[,,] map)
    {
      if (map.GetLength(2) != this.alphamapLayers)
        throw new Exception(UnityString.Format("Float array size wrong (layers should be {0})", (object) this.alphamapLayers));
      this.Internal_SetAlphamaps(x, y, map.GetLength(1), map.GetLength(0), map);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetAlphamaps(int x, int y, int width, int height, float[,,] map);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RecalculateBasemapIfDirty();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SetBasemapDirty(bool dirty);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Texture2D GetAlphamapTexture(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void AddTree(out TreeInstance tree);

    internal int RemoveTrees(Vector2 position, float radius, int prototypeIndex)
    {
      return TerrainData.INTERNAL_CALL_RemoveTrees(this, ref position, radius, prototypeIndex);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int INTERNAL_CALL_RemoveTrees(TerrainData self, ref Vector2 position, float radius, int prototypeIndex);
  }
}

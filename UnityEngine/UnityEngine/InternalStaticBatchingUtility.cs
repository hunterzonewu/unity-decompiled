// Decompiled with JetBrains decompiler
// Type: UnityEngine.InternalStaticBatchingUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
  internal class InternalStaticBatchingUtility
  {
    private const int MaxVerticesInBatch = 64000;
    private const string CombinedMeshPrefix = "Combined Mesh";

    public static void CombineRoot(GameObject staticBatchRoot)
    {
      InternalStaticBatchingUtility.Combine(staticBatchRoot, false, false);
    }

    public static void Combine(GameObject staticBatchRoot, bool combineOnlyStatic, bool isEditorPostprocessScene)
    {
      GameObject[] objectsOfType = (GameObject[]) Object.FindObjectsOfType(typeof (GameObject));
      List<GameObject> gameObjectList = new List<GameObject>();
      foreach (GameObject gameObject in objectsOfType)
      {
        if ((!((Object) staticBatchRoot != (Object) null) || gameObject.transform.IsChildOf(staticBatchRoot.transform)) && (!combineOnlyStatic || gameObject.isStaticBatchable))
          gameObjectList.Add(gameObject);
      }
      InternalStaticBatchingUtility.CombineGameObjects(gameObjectList.ToArray(), staticBatchRoot, isEditorPostprocessScene);
    }

    public static void CombineGameObjects(GameObject[] gos, GameObject staticBatchRoot, bool isEditorPostprocessScene)
    {
      Matrix4x4 matrix4x4 = Matrix4x4.identity;
      Transform staticBatchRootTransform = (Transform) null;
      if ((bool) ((Object) staticBatchRoot))
      {
        matrix4x4 = staticBatchRoot.transform.worldToLocalMatrix;
        staticBatchRootTransform = staticBatchRoot.transform;
      }
      int batchIndex = 0;
      int num = 0;
      List<MeshSubsetCombineUtility.MeshInstance> meshes = new List<MeshSubsetCombineUtility.MeshInstance>();
      List<MeshSubsetCombineUtility.SubMeshInstance> subsets = new List<MeshSubsetCombineUtility.SubMeshInstance>();
      List<GameObject> subsetGOs = new List<GameObject>();
      Array.Sort((Array) gos, (IComparer) new InternalStaticBatchingUtility.SortGO());
      foreach (GameObject go in gos)
      {
        MeshFilter component1 = go.GetComponent(typeof (MeshFilter)) as MeshFilter;
        if (!((Object) component1 == (Object) null))
        {
          Mesh sharedMesh = component1.sharedMesh;
          if (!((Object) sharedMesh == (Object) null) && (isEditorPostprocessScene || sharedMesh.canAccess))
          {
            Renderer component2 = component1.GetComponent<Renderer>();
            if (!((Object) component2 == (Object) null) && component2.enabled && component2.staticBatchIndex == 0)
            {
              Material[] materialArray1 = component1.GetComponent<Renderer>().sharedMaterials;
              if (!((IEnumerable<Material>) materialArray1).Any<Material>((Func<Material, bool>) (m =>
              {
                if ((Object) m != (Object) null && (Object) m.shader != (Object) null)
                  return m.shader.disableBatching != DisableBatchingType.False;
                return false;
              })))
              {
                int vertexCount = sharedMesh.vertexCount;
                if (vertexCount != 0)
                {
                  MeshRenderer meshRenderer = component2 as MeshRenderer;
                  if (!((Object) meshRenderer != (Object) null) || !((Object) meshRenderer.additionalVertexStreams != (Object) null) || vertexCount == meshRenderer.additionalVertexStreams.vertexCount)
                  {
                    if (num + vertexCount > 64000)
                    {
                      InternalStaticBatchingUtility.MakeBatch(meshes, subsets, subsetGOs, staticBatchRootTransform, batchIndex++);
                      meshes.Clear();
                      subsets.Clear();
                      subsetGOs.Clear();
                      num = 0;
                    }
                    MeshSubsetCombineUtility.MeshInstance meshInstance = new MeshSubsetCombineUtility.MeshInstance();
                    meshInstance.meshInstanceID = sharedMesh.GetInstanceID();
                    meshInstance.rendererInstanceID = component2.GetInstanceID();
                    if ((Object) meshRenderer != (Object) null && (Object) meshRenderer.additionalVertexStreams != (Object) null)
                      meshInstance.additionalVertexStreamsMeshInstanceID = meshRenderer.additionalVertexStreams.GetInstanceID();
                    meshInstance.transform = matrix4x4 * component1.transform.localToWorldMatrix;
                    meshInstance.lightmapScaleOffset = component2.lightmapScaleOffset;
                    meshInstance.realtimeLightmapScaleOffset = component2.realtimeLightmapScaleOffset;
                    meshes.Add(meshInstance);
                    if (materialArray1.Length > sharedMesh.subMeshCount)
                    {
                      Debug.LogWarning((object) ("Mesh has more materials (" + (object) materialArray1.Length + ") than subsets (" + (object) sharedMesh.subMeshCount + ")"), (Object) component1.GetComponent<Renderer>());
                      Material[] materialArray2 = new Material[sharedMesh.subMeshCount];
                      for (int index = 0; index < sharedMesh.subMeshCount; ++index)
                        materialArray2[index] = component1.GetComponent<Renderer>().sharedMaterials[index];
                      component1.GetComponent<Renderer>().sharedMaterials = materialArray2;
                      materialArray1 = materialArray2;
                    }
                    for (int index = 0; index < Math.Min(materialArray1.Length, sharedMesh.subMeshCount); ++index)
                    {
                      subsets.Add(new MeshSubsetCombineUtility.SubMeshInstance()
                      {
                        meshInstanceID = component1.sharedMesh.GetInstanceID(),
                        vertexOffset = num,
                        subMeshIndex = index,
                        gameObjectInstanceID = go.GetInstanceID(),
                        transform = meshInstance.transform
                      });
                      subsetGOs.Add(go);
                    }
                    num += sharedMesh.vertexCount;
                  }
                }
              }
            }
          }
        }
      }
      InternalStaticBatchingUtility.MakeBatch(meshes, subsets, subsetGOs, staticBatchRootTransform, batchIndex);
    }

    private static void MakeBatch(List<MeshSubsetCombineUtility.MeshInstance> meshes, List<MeshSubsetCombineUtility.SubMeshInstance> subsets, List<GameObject> subsetGOs, Transform staticBatchRootTransform, int batchIndex)
    {
      if (meshes.Count < 2)
        return;
      MeshSubsetCombineUtility.MeshInstance[] array1 = meshes.ToArray();
      MeshSubsetCombineUtility.SubMeshInstance[] array2 = subsets.ToArray();
      string meshName = "Combined Mesh" + " (root: " + (!((Object) staticBatchRootTransform != (Object) null) ? "scene" : staticBatchRootTransform.name) + ")";
      if (batchIndex > 0)
        meshName = meshName + " " + (object) (batchIndex + 1);
      Mesh combinedMesh = StaticBatchingUtility.InternalCombineVertices(array1, meshName);
      StaticBatchingUtility.InternalCombineIndices(array2, combinedMesh);
      int subSetIndexForMaterial = 0;
      for (int index = 0; index < array2.Length; ++index)
      {
        MeshSubsetCombineUtility.SubMeshInstance subMeshInstance = array2[index];
        GameObject subsetGo = subsetGOs[index];
        Mesh mesh = combinedMesh;
        ((MeshFilter) subsetGo.GetComponent(typeof (MeshFilter))).sharedMesh = mesh;
        Renderer component = subsetGo.GetComponent<Renderer>();
        component.SetSubsetIndex(subMeshInstance.subMeshIndex, subSetIndexForMaterial);
        component.staticBatchRootTransform = staticBatchRootTransform;
        component.enabled = false;
        component.enabled = true;
        MeshRenderer meshRenderer = component as MeshRenderer;
        if ((Object) meshRenderer != (Object) null)
          meshRenderer.additionalVertexStreams = (Mesh) null;
        ++subSetIndexForMaterial;
      }
    }

    internal class SortGO : IComparer
    {
      int IComparer.Compare(object a, object b)
      {
        if (a == b)
          return 0;
        Renderer renderer1 = InternalStaticBatchingUtility.SortGO.GetRenderer(a as GameObject);
        Renderer renderer2 = InternalStaticBatchingUtility.SortGO.GetRenderer(b as GameObject);
        int num = InternalStaticBatchingUtility.SortGO.GetMaterialId(renderer1).CompareTo(InternalStaticBatchingUtility.SortGO.GetMaterialId(renderer2));
        if (num == 0)
          num = InternalStaticBatchingUtility.SortGO.GetLightmapIndex(renderer1).CompareTo(InternalStaticBatchingUtility.SortGO.GetLightmapIndex(renderer2));
        return num;
      }

      private static int GetMaterialId(Renderer renderer)
      {
        if ((Object) renderer == (Object) null || (Object) renderer.sharedMaterial == (Object) null)
          return 0;
        return renderer.sharedMaterial.GetInstanceID();
      }

      private static int GetLightmapIndex(Renderer renderer)
      {
        if ((Object) renderer == (Object) null)
          return -1;
        return renderer.lightmapIndex;
      }

      private static Renderer GetRenderer(GameObject go)
      {
        if ((Object) go == (Object) null)
          return (Renderer) null;
        MeshFilter component = go.GetComponent(typeof (MeshFilter)) as MeshFilter;
        if ((Object) component == (Object) null)
          return (Renderer) null;
        return component.GetComponent<Renderer>();
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.RenderThumbnailUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class RenderThumbnailUtility
  {
    public static Bounds CalculateVisibleBounds(GameObject prefab)
    {
      return prefab.GetComponent<Renderer>().bounds;
    }

    public static Texture2D Render(GameObject prefab)
    {
      if ((Object) prefab == (Object) null)
        return (Texture2D) null;
      if ((Object) prefab.GetComponent<Renderer>() == (Object) null)
        return (Texture2D) null;
      Texture2D texture2D = new Texture2D(64, 64);
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      texture2D.name = "Preview Texture";
      RenderTexture temporary = RenderTexture.GetTemporary(texture2D.width, texture2D.height);
      GameObject gameObject = new GameObject("Preview");
      gameObject.hideFlags = HideFlags.HideAndDontSave;
      Camera camera = gameObject.AddComponent(typeof (Camera)) as Camera;
      camera.cameraType = CameraType.Preview;
      camera.clearFlags = CameraClearFlags.Color;
      camera.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 0.0f);
      camera.cullingMask = 0;
      camera.enabled = false;
      camera.targetTexture = temporary;
      Light light = gameObject.AddComponent(typeof (Light)) as Light;
      light.type = LightType.Directional;
      Bounds visibleBounds = RenderThumbnailUtility.CalculateVisibleBounds(prefab);
      Vector3 vector3 = new Vector3(0.7f, 0.3f, 0.7f);
      float num = visibleBounds.extents.magnitude * 1.6f;
      gameObject.transform.position = visibleBounds.center + vector3.normalized * num;
      gameObject.transform.LookAt(visibleBounds.center);
      camera.nearClipPlane = num * 0.1f;
      camera.farClipPlane = num * 2.2f;
      Camera current = Camera.current;
      camera.RenderDontRestore();
      Graphics.SetupVertexLights(new Light[1]{ light });
      foreach (Renderer componentsInChild in prefab.GetComponentsInChildren(typeof (Renderer)))
      {
        if (componentsInChild.enabled)
        {
          Material[] sharedMaterials = componentsInChild.sharedMaterials;
          for (int material = 0; material < sharedMaterials.Length; ++material)
          {
            if (!((Object) sharedMaterials[material] == (Object) null))
            {
              Material original = sharedMaterials[material];
              string dependency = ShaderUtil.GetDependency(original.shader, "BillboardShader");
              if (dependency != null && dependency != string.Empty)
              {
                original = Object.Instantiate<Material>(original);
                original.shader = Shader.Find(dependency);
                original.hideFlags = HideFlags.HideAndDontSave;
              }
              for (int pass = 0; pass < original.passCount; ++pass)
              {
                if (original.SetPass(pass))
                  componentsInChild.RenderNow(material);
              }
              if ((Object) original != (Object) sharedMaterials[material])
                Object.DestroyImmediate((Object) original);
            }
          }
        }
      }
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) texture2D.width, (float) texture2D.height), 0, 0);
      RenderTexture.ReleaseTemporary(temporary);
      Object.DestroyImmediate((Object) gameObject);
      Camera.SetupCurrent(current);
      return texture2D;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarSkeletonDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class AvatarSkeletonDrawer
  {
    private static Color kSkeletonColor = new Color(0.4039216f, 0.4039216f, 0.4039216f, 0.25f);
    private static Color kDummyColor = new Color(0.2352941f, 0.2352941f, 0.2352941f, 0.25f);
    private static Color kHumanColor = new Color(0.0f, 0.8235294f, 0.2901961f, 0.25f);
    private static Color kErrorColor = new Color(1f, 0.0f, 0.0f, 0.25f);
    private static Color kErrorMessageColor = new Color(1f, 0.0f, 0.0f, 0.75f);
    private static Color kSelectedColor = new Color(0.5019608f, 0.7529412f, 1f, 0.15f);
    private static bool sPoseError;

    public static void DrawSkeleton(Transform reference, Dictionary<Transform, bool> actualBones)
    {
      AvatarSkeletonDrawer.DrawSkeleton(reference, actualBones, (AvatarSetupTool.BoneWrapper[]) null);
    }

    public static void DrawSkeleton(Transform reference, Dictionary<Transform, bool> actualBones, AvatarSetupTool.BoneWrapper[] bones)
    {
      if ((Object) reference == (Object) null || actualBones == null)
        return;
      AvatarSkeletonDrawer.sPoseError = false;
      Bounds bounds = new Bounds();
      Renderer[] componentsInChildren = reference.root.GetComponentsInChildren<Renderer>();
      if (componentsInChildren != null)
      {
        foreach (Renderer renderer in componentsInChildren)
        {
          bounds.Encapsulate(renderer.bounds.min);
          bounds.Encapsulate(renderer.bounds.max);
        }
      }
      Quaternion orientation = Quaternion.identity;
      if (bones != null)
        orientation = AvatarSetupTool.AvatarComputeOrientation(bones);
      AvatarSkeletonDrawer.DrawSkeletonSubTree(actualBones, bones, orientation, reference, bounds);
      Camera current = Camera.current;
      if (!AvatarSkeletonDrawer.sPoseError || !((Object) current != (Object) null))
        return;
      GUIStyle style = new GUIStyle(GUI.skin.label);
      style.normal.textColor = Color.red;
      style.wordWrap = false;
      style.alignment = TextAnchor.MiddleLeft;
      style.fontSize = 20;
      GUIContent content = new GUIContent("Character is not in T pose");
      Rect rect = GUILayoutUtility.GetRect(content, style);
      rect.x = 30f;
      rect.y = 30f;
      Handles.BeginGUI();
      GUI.Label(rect, content, style);
      Handles.EndGUI();
    }

    private static bool DrawSkeletonSubTree(Dictionary<Transform, bool> actualBones, AvatarSetupTool.BoneWrapper[] bones, Quaternion orientation, Transform tr, Bounds bounds)
    {
      if (!actualBones.ContainsKey(tr))
        return false;
      int num = 0;
      foreach (Transform tr1 in tr)
      {
        if (AvatarSkeletonDrawer.DrawSkeletonSubTree(actualBones, bones, orientation, tr1, bounds))
          ++num;
      }
      if (!actualBones[tr] && num <= 1)
        return false;
      int boneIndex = -1;
      if (bones != null)
      {
        for (int index = 0; index < bones.Length; ++index)
        {
          if ((Object) bones[index].bone == (Object) tr)
          {
            boneIndex = index;
            break;
          }
        }
      }
      bool flag = (double) AvatarSetupTool.GetBoneAlignmentError(bones, orientation, boneIndex) > 0.0;
      AvatarSkeletonDrawer.sPoseError |= flag;
      if (flag)
      {
        AvatarSkeletonDrawer.DrawPoseError(tr, bounds);
        Handles.color = AvatarSkeletonDrawer.kErrorColor;
      }
      else
        Handles.color = boneIndex == -1 ? (actualBones[tr] ? AvatarSkeletonDrawer.kSkeletonColor : AvatarSkeletonDrawer.kDummyColor) : AvatarSkeletonDrawer.kHumanColor;
      Handles.DoBoneHandle(tr, actualBones);
      if (Selection.activeObject == (Object) tr)
      {
        Handles.color = AvatarSkeletonDrawer.kSelectedColor;
        Handles.DoBoneHandle(tr, actualBones);
      }
      return true;
    }

    private static void DrawPoseError(Transform node, Bounds bounds)
    {
      if (!(bool) ((Object) Camera.current))
        return;
      GUIStyle style = new GUIStyle(GUI.skin.label);
      style.normal.textColor = Color.red;
      style.wordWrap = false;
      style.alignment = TextAnchor.MiddleLeft;
      Vector3 position = node.position;
      Vector3 vector3 = node.position + Vector3.up * 0.2f;
      vector3.x = (double) node.position.x > (double) node.root.position.x ? bounds.max.x : bounds.min.x;
      GUIContent content = new GUIContent(node.name);
      Rect sizedRect = HandleUtility.WorldPointToSizedRect(vector3, content, style);
      sizedRect.x += 2f;
      if ((double) node.position.x > (double) node.root.position.x)
        sizedRect.x -= sizedRect.width;
      Handles.BeginGUI();
      sizedRect.y -= style.CalcSize(content).y / 4f;
      GUI.Label(sizedRect, content, style);
      Handles.EndGUI();
      Handles.color = AvatarSkeletonDrawer.kErrorMessageColor;
      Handles.DrawLine(position, vector3);
    }
  }
}

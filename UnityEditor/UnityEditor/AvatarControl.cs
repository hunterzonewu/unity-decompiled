// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarControl
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class AvatarControl
  {
    private static Vector2[,] s_BonePositions = new Vector2[4, HumanTrait.BoneCount];
    private static AvatarControl.Styles s_Styles;

    private static AvatarControl.Styles styles
    {
      get
      {
        if (AvatarControl.s_Styles == null)
          AvatarControl.s_Styles = new AvatarControl.Styles();
        return AvatarControl.s_Styles;
      }
    }

    static AvatarControl()
    {
      int index1 = 0;
      AvatarControl.s_BonePositions[index1, 0] = new Vector2(0.0f, 0.08f);
      AvatarControl.s_BonePositions[index1, 1] = new Vector2(0.16f, 0.01f);
      AvatarControl.s_BonePositions[index1, 2] = new Vector2(-0.16f, 0.01f);
      AvatarControl.s_BonePositions[index1, 3] = new Vector2(0.21f, -0.4f);
      AvatarControl.s_BonePositions[index1, 4] = new Vector2(-0.21f, -0.4f);
      AvatarControl.s_BonePositions[index1, 5] = new Vector2(0.23f, -0.8f);
      AvatarControl.s_BonePositions[index1, 6] = new Vector2(-0.23f, -0.8f);
      AvatarControl.s_BonePositions[index1, 7] = new Vector2(0.0f, 0.25f);
      AvatarControl.s_BonePositions[index1, 8] = new Vector2(0.0f, 0.43f);
      AvatarControl.s_BonePositions[index1, 9] = new Vector2(0.0f, 0.66f);
      AvatarControl.s_BonePositions[index1, 10] = new Vector2(0.0f, 0.76f);
      AvatarControl.s_BonePositions[index1, 11] = new Vector2(0.14f, 0.6f);
      AvatarControl.s_BonePositions[index1, 12] = new Vector2(-0.14f, 0.6f);
      AvatarControl.s_BonePositions[index1, 13] = new Vector2(0.3f, 0.57f);
      AvatarControl.s_BonePositions[index1, 14] = new Vector2(-0.3f, 0.57f);
      AvatarControl.s_BonePositions[index1, 15] = new Vector2(0.48f, 0.3f);
      AvatarControl.s_BonePositions[index1, 16] = new Vector2(-0.48f, 0.3f);
      AvatarControl.s_BonePositions[index1, 17] = new Vector2(0.66f, 0.03f);
      AvatarControl.s_BonePositions[index1, 18] = new Vector2(-0.66f, 0.03f);
      AvatarControl.s_BonePositions[index1, 19] = new Vector2(0.25f, -0.89f);
      AvatarControl.s_BonePositions[index1, 20] = new Vector2(-0.25f, -0.89f);
      int index2 = 1;
      AvatarControl.s_BonePositions[index2, 9] = new Vector2(-0.2f, -0.62f);
      AvatarControl.s_BonePositions[index2, 10] = new Vector2(-0.15f, -0.3f);
      AvatarControl.s_BonePositions[index2, 21] = new Vector2(0.63f, 0.16f);
      AvatarControl.s_BonePositions[index2, 22] = new Vector2(0.15f, 0.16f);
      AvatarControl.s_BonePositions[index2, 23] = new Vector2(0.45f, -0.4f);
      int index3 = 2;
      AvatarControl.s_BonePositions[index3, 24] = new Vector2(-0.35f, 0.11f);
      AvatarControl.s_BonePositions[index3, 27] = new Vector2(0.19f, 0.11f);
      AvatarControl.s_BonePositions[index3, 30] = new Vector2(0.22f, 0.0f);
      AvatarControl.s_BonePositions[index3, 33] = new Vector2(0.16f, -0.12f);
      AvatarControl.s_BonePositions[index3, 36] = new Vector2(0.09f, -0.23f);
      AvatarControl.s_BonePositions[index3, 26] = new Vector2(-0.03f, 0.33f);
      AvatarControl.s_BonePositions[index3, 29] = new Vector2(0.65f, 0.16f);
      AvatarControl.s_BonePositions[index3, 32] = new Vector2(0.74f, 0.0f);
      AvatarControl.s_BonePositions[index3, 35] = new Vector2(0.66f, -0.14f);
      AvatarControl.s_BonePositions[index3, 38] = new Vector2(0.45f, -0.25f);
      for (int index4 = 0; index4 < 5; ++index4)
        AvatarControl.s_BonePositions[index3, 25 + index4 * 3] = Vector2.Lerp(AvatarControl.s_BonePositions[index3, 24 + index4 * 3], AvatarControl.s_BonePositions[index3, 26 + index4 * 3], 0.58f);
      int index5 = 3;
      for (int index4 = 0; index4 < 15; ++index4)
        AvatarControl.s_BonePositions[index5, 24 + index4 + 15] = Vector2.Scale(AvatarControl.s_BonePositions[index5 - 1, 24 + index4], new Vector2(-1f, 1f));
    }

    public static int ShowBoneMapping(int shownBodyView, AvatarControl.BodyPartFeedback bodyPartCallback, AvatarSetupTool.BoneWrapper[] bones, SerializedObject serializedObject, AvatarMappingEditor editor)
    {
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if ((bool) ((Object) AvatarControl.styles.Silhouettes[shownBodyView].image))
      {
        Rect rect = GUILayoutUtility.GetRect(AvatarControl.styles.Silhouettes[shownBodyView], GUIStyle.none, new GUILayoutOption[1]{ GUILayout.MaxWidth((float) AvatarControl.styles.Silhouettes[shownBodyView].image.width) });
        AvatarControl.DrawBodyParts(rect, shownBodyView, bodyPartCallback);
        for (int i = 0; i < bones.Length; ++i)
          AvatarControl.DrawBone(shownBodyView, i, rect, bones[i], serializedObject, editor);
      }
      else
        GUILayout.Label("texture missing,\nfix me!");
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      Rect lastRect = GUILayoutUtility.GetLastRect();
      string[] strArray = new string[4]{ "Body", "Head", "Left Hand", "Right Hand" };
      lastRect.x += 5f;
      lastRect.width = 70f;
      lastRect.yMin = lastRect.yMax - 69f;
      lastRect.height = 16f;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (GUI.Toggle(lastRect, shownBodyView == index, strArray[index], EditorStyles.miniButton))
          shownBodyView = index;
        lastRect.y += 16f;
      }
      return shownBodyView;
    }

    public static void DrawBodyParts(Rect rect, int shownBodyView, AvatarControl.BodyPartFeedback bodyPartCallback)
    {
      GUI.color = new Color(0.2f, 0.2f, 0.2f, 1f);
      if (AvatarControl.styles.Silhouettes[shownBodyView] != null)
        GUI.DrawTexture(rect, AvatarControl.styles.Silhouettes[shownBodyView].image);
      for (int i = 1; i < 9; ++i)
        AvatarControl.DrawBodyPart(shownBodyView, i, rect, bodyPartCallback((BodyPart) i));
    }

    protected static void DrawBodyPart(int shownBodyView, int i, Rect rect, AvatarControl.BodyPartColor bodyPartColor)
    {
      if (AvatarControl.styles.BodyPart[shownBodyView, i] == null || !((Object) AvatarControl.styles.BodyPart[shownBodyView, i].image != (Object) null))
        return;
      GUI.color = (bodyPartColor & AvatarControl.BodyPartColor.Green) != AvatarControl.BodyPartColor.Green ? ((bodyPartColor & AvatarControl.BodyPartColor.Red) != AvatarControl.BodyPartColor.Red ? Color.gray : Color.red) : Color.green;
      GUI.DrawTexture(rect, AvatarControl.styles.BodyPart[shownBodyView, i].image);
      GUI.color = Color.white;
    }

    public static List<int> GetViewsThatContainBone(int bone)
    {
      List<int> intList = new List<int>();
      if (bone < 0 || bone >= HumanTrait.BoneCount)
        return intList;
      for (int index = 0; index < 4; ++index)
      {
        if (AvatarControl.s_BonePositions[index, bone] != Vector2.zero)
          intList.Add(index);
      }
      return intList;
    }

    protected static void DrawBone(int shownBodyView, int i, Rect rect, AvatarSetupTool.BoneWrapper bone, SerializedObject serializedObject, AvatarMappingEditor editor)
    {
      if (AvatarControl.s_BonePositions[shownBodyView, i] == Vector2.zero)
        return;
      Vector2 bonePosition = AvatarControl.s_BonePositions[shownBodyView, i];
      bonePosition.y *= -1f;
      bonePosition.Scale(new Vector2(rect.width * 0.5f, rect.height * 0.5f));
      Vector2 vector2 = rect.center + bonePosition;
      int num = 19;
      Rect rect1 = new Rect(vector2.x - (float) num * 0.5f, vector2.y - (float) num * 0.5f, (float) num, (float) num);
      bone.BoneDotGUI(rect1, i, true, true, serializedObject, editor);
    }

    private class Styles
    {
      public GUIContent[] Silhouettes;
      public GUIContent[,] BodyPart;
      public GUILayoutOption ButtonSize;

      public Styles()
      {
        GUIContent[,] guiContentArray = new GUIContent[4, 9];
        guiContentArray[0, 1] = EditorGUIUtility.IconContent("AvatarInspector/Torso");
        guiContentArray[0, 2] = EditorGUIUtility.IconContent("AvatarInspector/Head");
        guiContentArray[0, 3] = EditorGUIUtility.IconContent("AvatarInspector/LeftArm");
        guiContentArray[0, 4] = EditorGUIUtility.IconContent("AvatarInspector/LeftFingers");
        guiContentArray[0, 5] = EditorGUIUtility.IconContent("AvatarInspector/RightArm");
        guiContentArray[0, 6] = EditorGUIUtility.IconContent("AvatarInspector/RightFingers");
        guiContentArray[0, 7] = EditorGUIUtility.IconContent("AvatarInspector/LeftLeg");
        guiContentArray[0, 8] = EditorGUIUtility.IconContent("AvatarInspector/RightLeg");
        guiContentArray[1, 2] = EditorGUIUtility.IconContent("AvatarInspector/HeadZoom");
        guiContentArray[2, 4] = EditorGUIUtility.IconContent("AvatarInspector/LeftHandZoom");
        guiContentArray[3, 6] = EditorGUIUtility.IconContent("AvatarInspector/RightHandZoom");
        this.BodyPart = guiContentArray;
        this.ButtonSize = GUILayout.MaxWidth(120f);
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    public enum BodyPartColor
    {
      Off = 0,
      Green = 1,
      Red = 2,
      IKGreen = 4,
      IKRed = 8,
    }

    public delegate AvatarControl.BodyPartColor BodyPartFeedback(BodyPart bodyPart);
  }
}

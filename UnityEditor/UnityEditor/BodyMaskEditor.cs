// Decompiled with JetBrains decompiler
// Type: UnityEditor.BodyMaskEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class BodyMaskEditor
  {
    private static BodyMaskEditor.Styles styles = new BodyMaskEditor.Styles();
    protected static Color[] m_MaskBodyPartPicker = new Color[14]{ new Color(1f, 0.5647059f, 0.0f), new Color(0.0f, 0.682353f, 0.9411765f), new Color(0.6705883f, 0.627451f, 0.0f), new Color(0.0f, 1f, 1f), new Color(0.9686275f, 0.5921569f, 0.4745098f), new Color(0.0f, 1f, 0.0f), new Color(0.3372549f, 0.454902f, 0.7254902f), new Color(1f, 1f, 0.0f), new Color(0.509804f, 0.7921569f, 0.6117647f), new Color(0.3215686f, 0.3215686f, 0.3215686f), new Color(1f, 0.4509804f, 0.4509804f), new Color(0.6235294f, 0.6235294f, 0.6235294f), new Color(0.7921569f, 0.7921569f, 0.7921569f), new Color(0.3960784f, 0.3960784f, 0.3960784f) };
    private static string sAvatarBodyMaskStr = "AvatarMask";
    private static int s_Hint = BodyMaskEditor.sAvatarBodyMaskStr.GetHashCode();

    public static void Show(SerializedProperty bodyMask, int count)
    {
      if (!(bool) ((Object) BodyMaskEditor.styles.UnityDude.image))
        return;
      Rect rect = GUILayoutUtility.GetRect(BodyMaskEditor.styles.UnityDude, GUIStyle.none, new GUILayoutOption[1]{ GUILayout.MaxWidth((float) BodyMaskEditor.styles.UnityDude.image.width) });
      rect.x += (float) (((double) GUIView.current.position.width - (double) rect.width) / 2.0);
      Color color = GUI.color;
      GUI.color = bodyMask.GetArrayElementAtIndex(0).intValue != 1 ? Color.red : Color.green;
      if ((bool) ((Object) BodyMaskEditor.styles.BodyPart[0].image))
        GUI.DrawTexture(rect, BodyMaskEditor.styles.BodyPart[0].image);
      GUI.color = new Color(0.2f, 0.2f, 0.2f, 1f);
      GUI.DrawTexture(rect, BodyMaskEditor.styles.UnityDude.image);
      for (int index = 1; index < count; ++index)
      {
        GUI.color = bodyMask.GetArrayElementAtIndex(index).intValue != 1 ? Color.red : Color.green;
        if ((bool) ((Object) BodyMaskEditor.styles.BodyPart[index].image))
          GUI.DrawTexture(rect, BodyMaskEditor.styles.BodyPart[index].image);
      }
      GUI.color = color;
      BodyMaskEditor.DoPicking(rect, bodyMask, count);
    }

    protected static void DoPicking(Rect rect, SerializedProperty bodyMask, int count)
    {
      if (!(bool) ((Object) BodyMaskEditor.styles.PickingTexture.image))
        return;
      int controlId = GUIUtility.GetControlID(BodyMaskEditor.s_Hint, FocusType.Native, rect);
      Event current = Event.current;
      if (current.GetTypeForControl(controlId) != EventType.MouseDown || !rect.Contains(current.mousePosition))
        return;
      current.Use();
      int x = (int) current.mousePosition.x - (int) rect.x;
      int y = BodyMaskEditor.styles.UnityDude.image.height - ((int) current.mousePosition.y - (int) rect.y);
      Color pixel = (BodyMaskEditor.styles.PickingTexture.image as Texture2D).GetPixel(x, y);
      bool flag1 = false;
      for (int index = 0; index < count; ++index)
      {
        if (BodyMaskEditor.m_MaskBodyPartPicker[index] == pixel)
        {
          GUI.changed = true;
          bodyMask.GetArrayElementAtIndex(index).intValue = bodyMask.GetArrayElementAtIndex(index).intValue != 1 ? 1 : 0;
          flag1 = true;
        }
      }
      if (flag1)
        return;
      bool flag2 = false;
      for (int index = 0; index < count && !flag2; ++index)
        flag2 = bodyMask.GetArrayElementAtIndex(index).intValue == 1;
      for (int index = 0; index < count; ++index)
        bodyMask.GetArrayElementAtIndex(index).intValue = flag2 ? 0 : 1;
      GUI.changed = true;
    }

    private class Styles
    {
      public GUIContent UnityDude = EditorGUIUtility.IconContent("AvatarInspector/BodySIlhouette");
      public GUIContent PickingTexture = EditorGUIUtility.IconContent("AvatarInspector/BodyPartPicker");
      public GUIContent[] BodyPart = new GUIContent[13]{ EditorGUIUtility.IconContent("AvatarInspector/MaskEditor_Root"), EditorGUIUtility.IconContent("AvatarInspector/Torso"), EditorGUIUtility.IconContent("AvatarInspector/Head"), EditorGUIUtility.IconContent("AvatarInspector/LeftLeg"), EditorGUIUtility.IconContent("AvatarInspector/RightLeg"), EditorGUIUtility.IconContent("AvatarInspector/LeftArm"), EditorGUIUtility.IconContent("AvatarInspector/RightArm"), EditorGUIUtility.IconContent("AvatarInspector/LeftFingers"), EditorGUIUtility.IconContent("AvatarInspector/RightFingers"), EditorGUIUtility.IconContent("AvatarInspector/LeftFeetIk"), EditorGUIUtility.IconContent("AvatarInspector/RightFeetIk"), EditorGUIUtility.IconContent("AvatarInspector/LeftFingersIk"), EditorGUIUtility.IconContent("AvatarInspector/RightFingersIk") };
    }
  }
}

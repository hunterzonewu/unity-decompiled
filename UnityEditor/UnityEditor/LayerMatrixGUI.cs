// Decompiled with JetBrains decompiler
// Type: UnityEditor.LayerMatrixGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class LayerMatrixGUI
  {
    public static void DoGUI(string title, ref bool show, ref Vector2 scrollPos, LayerMatrixGUI.GetValueFunc getValue, LayerMatrixGUI.SetValueFunc setValue)
    {
      int num1 = 0;
      for (int layer = 0; layer < 32; ++layer)
      {
        if (LayerMask.LayerToName(layer) != string.Empty)
          ++num1;
      }
      GUILayout.BeginHorizontal();
      GUILayout.Space(0.0f);
      show = EditorGUILayout.Foldout(show, title);
      GUILayout.EndHorizontal();
      if (!show)
        return;
      scrollPos = GUILayout.BeginScrollView(scrollPos, new GUILayoutOption[2]
      {
        GUILayout.MinHeight(120f),
        GUILayout.MaxHeight((float) (100 + (num1 + 1) * 16))
      });
      Rect rect1 = GUILayoutUtility.GetRect((float) (16 * num1 + 100), 100f);
      Rect topmostRect = GUIClip.topmostRect;
      Vector2 vector2 = GUIClip.Unclip(new Vector2(rect1.x, rect1.y));
      int num2 = 0;
      for (int layer = 0; layer < 32; ++layer)
      {
        if (LayerMask.LayerToName(layer) != string.Empty)
        {
          float num3 = (float) (130 + (num1 - num2) * 16) - (topmostRect.width + scrollPos.x);
          if ((double) num3 < 0.0)
            num3 = 0.0f;
          GUI.matrix = Matrix4x4.TRS(new Vector3((float) (130 + 16 * (num1 - num2)) + vector2.y + vector2.x + scrollPos.y - num3, vector2.y + scrollPos.y, 0.0f), Quaternion.identity, Vector3.one) * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 90f), Vector3.one);
          if (SystemInfo.graphicsDeviceVersion.StartsWith("Direct3D 9.0"))
            GUI.matrix *= Matrix4x4.TRS(new Vector3(-0.5f, -0.5f, 0.0f), Quaternion.identity, Vector3.one);
          GUI.Label(new Rect(2f - vector2.x - scrollPos.y, scrollPos.y - num3, 100f, 16f), LayerMask.LayerToName(layer), (GUIStyle) "RightLabel");
          ++num2;
        }
      }
      GUI.matrix = Matrix4x4.identity;
      int num4 = 0;
      for (int index1 = 0; index1 < 32; ++index1)
      {
        if (LayerMask.LayerToName(index1) != string.Empty)
        {
          int num3 = 0;
          Rect rect2 = GUILayoutUtility.GetRect((float) (30 + 16 * num1 + 100), 16f);
          GUI.Label(new Rect(rect2.x + 30f, rect2.y, 100f, 16f), LayerMask.LayerToName(index1), (GUIStyle) "RightLabel");
          for (int index2 = 31; index2 >= 0; --index2)
          {
            if (LayerMask.LayerToName(index2) != string.Empty)
            {
              if (num3 < num1 - num4)
              {
                GUIContent content = new GUIContent(string.Empty, LayerMask.LayerToName(index1) + "/" + LayerMask.LayerToName(index2));
                bool flag = getValue(index1, index2);
                bool val = GUI.Toggle(new Rect(130f + rect2.x + (float) (num3 * 16), rect2.y, 16f, 16f), flag, content);
                if (val != flag)
                  setValue(index1, index2, val);
              }
              ++num3;
            }
          }
          ++num4;
        }
      }
      GUILayout.EndScrollView();
    }

    public delegate bool GetValueFunc(int layerA, int layerB);

    public delegate void SetValueFunc(int layerA, int layerB, bool val);
  }
}

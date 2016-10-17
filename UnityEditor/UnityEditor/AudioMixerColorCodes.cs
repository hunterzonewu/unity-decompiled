// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerColorCodes
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Audio;
using UnityEngine;

namespace UnityEditor
{
  internal static class AudioMixerColorCodes
  {
    private static Color[] darkSkinColors = new Color[9]
    {
      new Color(0.5f, 0.5f, 0.5f, 0.2f),
      new Color(1f, 0.8156863f, 0.0f),
      new Color(0.9607843f, 0.6117647f, 0.01568628f),
      new Color(1f, 0.2941177f, 0.227451f),
      new Color(1f, 0.3803922f, 0.6117647f),
      new Color(0.6588235f, 0.4470588f, 0.7176471f),
      new Color(0.05098039f, 0.6117647f, 0.8235294f),
      new Color(0.0f, 0.7450981f, 0.7843137f),
      new Color(0.5411765f, 0.7529412f, 0.003921569f)
    };
    private static Color[] lightSkinColors = new Color[9]
    {
      new Color(0.5f, 0.5f, 0.5f, 0.2f),
      new Color(1f, 0.8392157f, 0.08627451f),
      new Color(0.9686275f, 0.5764706f, 0.0f),
      new Color(1f, 0.2941177f, 0.227451f),
      new Color(1f, 0.3803922f, 0.6117647f),
      new Color(0.6588235f, 0.4470588f, 0.7176471f),
      new Color(0.05098039f, 0.6117647f, 0.8235294f),
      new Color(0.0f, 0.7098039f, 0.7254902f),
      new Color(0.4470588f, 0.6627451f, 0.09411765f)
    };
    private static string[] colorNames = new string[9]
    {
      "No Color",
      "Yellow",
      "Orange",
      "Red",
      "Magenta",
      "Violet",
      "Blue",
      "Cyan",
      "Green"
    };

    private static string[] GetColorNames()
    {
      return AudioMixerColorCodes.colorNames;
    }

    private static Color[] GetColors()
    {
      if (EditorGUIUtility.isProSkin)
        return AudioMixerColorCodes.darkSkinColors;
      return AudioMixerColorCodes.lightSkinColors;
    }

    public static void AddColorItemsToGenericMenu(GenericMenu menu, AudioMixerGroupController[] groups)
    {
      Color[] colors = AudioMixerColorCodes.GetColors();
      string[] colorNames = AudioMixerColorCodes.GetColorNames();
      for (int index = 0; index < colors.Length; ++index)
      {
        bool flag = groups.Length == 1 && index == groups[0].userColorIndex;
        menu.AddItem(new GUIContent(colorNames[index]), (flag ? 1 : 0) != 0, new GenericMenu.MenuFunction2(AudioMixerColorCodes.ItemCallback), (object) new AudioMixerColorCodes.ItemData()
        {
          groups = groups,
          index = index
        });
      }
    }

    private static void ItemCallback(object data)
    {
      AudioMixerColorCodes.ItemData itemData = (AudioMixerColorCodes.ItemData) data;
      Undo.RecordObjects((Object[]) itemData.groups, "Change Group(s) Color");
      foreach (AudioMixerGroupController group in itemData.groups)
        group.userColorIndex = itemData.index;
    }

    public static Color GetColor(int index)
    {
      Color[] colors = AudioMixerColorCodes.GetColors();
      if (index >= 0 && index < colors.Length)
        return colors[index];
      Debug.LogError((object) ("Invalid color code index: " + (object) index));
      return Color.white;
    }

    private struct ItemData
    {
      public AudioMixerGroupController[] groups;
      public int index;
    }
  }
}

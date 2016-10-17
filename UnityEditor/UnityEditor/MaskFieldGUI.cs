// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaskFieldGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal static class MaskFieldGUI
  {
    internal static int DoMaskField(Rect position, int controlID, int mask, string[] flagNames, GUIStyle style)
    {
      int changedFlags;
      bool changedToValue;
      return MaskFieldGUI.DoMaskField(position, controlID, mask, flagNames, style, out changedFlags, out changedToValue);
    }

    internal static int DoMaskField(Rect position, int controlID, int mask, string[] flagNames, GUIStyle style, out int changedFlags, out bool changedToValue)
    {
      mask = MaskFieldGUI.MaskCallbackInfo.GetSelectedValueForControl(controlID, mask, out changedFlags, out changedToValue);
      List<int> intList = new List<int>();
      List<string> stringList = new List<string>() { "Nothing", "Everything" };
      for (int index = 0; index < flagNames.Length; ++index)
      {
        if ((mask & 1 << index) != 0)
          intList.Add(index + 2);
      }
      stringList.AddRange((IEnumerable<string>) flagNames);
      GUIContent content = EditorGUI.mixedValueContent;
      if (!EditorGUI.showMixedValue)
      {
        switch (intList.Count)
        {
          case 0:
            content = EditorGUIUtility.TempContent("Nothing");
            intList.Add(0);
            break;
          case 1:
            content = new GUIContent(stringList[intList[0]]);
            break;
          default:
            if (intList.Count >= flagNames.Length)
            {
              content = EditorGUIUtility.TempContent("Everything");
              intList.Add(1);
              mask = -1;
              break;
            }
            content = EditorGUIUtility.TempContent("Mixed ...");
            break;
        }
      }
      Event current = Event.current;
      if (current.type == EventType.Repaint)
        style.Draw(position, content, controlID, false);
      else if (current.type == EventType.MouseDown && position.Contains(current.mousePosition) || current.MainActionKeyForControl(controlID))
      {
        MaskFieldGUI.MaskCallbackInfo.m_Instance = new MaskFieldGUI.MaskCallbackInfo(controlID);
        current.Use();
        EditorUtility.DisplayCustomMenu(position, stringList.ToArray(), !EditorGUI.showMixedValue ? intList.ToArray() : new int[0], new EditorUtility.SelectMenuItemFunction(MaskFieldGUI.MaskCallbackInfo.m_Instance.SetMaskValueDelegate), (object) null);
      }
      return mask;
    }

    private class MaskCallbackInfo
    {
      private const string kMaskMenuChangedMessage = "MaskMenuChanged";
      public static MaskFieldGUI.MaskCallbackInfo m_Instance;
      private readonly int m_ControlID;
      private int m_Mask;
      private bool m_SetAll;
      private bool m_ClearAll;
      private bool m_DoNothing;
      private readonly GUIView m_SourceView;

      public MaskCallbackInfo(int controlID)
      {
        this.m_ControlID = controlID;
        this.m_SourceView = GUIView.current;
      }

      public static int GetSelectedValueForControl(int controlID, int mask, out int changedFlags, out bool changedToValue)
      {
        Event current = Event.current;
        changedFlags = 0;
        changedToValue = false;
        if (current.type == EventType.ExecuteCommand && current.commandName == "MaskMenuChanged")
        {
          if (MaskFieldGUI.MaskCallbackInfo.m_Instance == null)
          {
            Debug.LogError((object) "Mask menu has no instance");
            return mask;
          }
          if (MaskFieldGUI.MaskCallbackInfo.m_Instance.m_ControlID == controlID)
          {
            if (!MaskFieldGUI.MaskCallbackInfo.m_Instance.m_DoNothing)
            {
              if (MaskFieldGUI.MaskCallbackInfo.m_Instance.m_ClearAll)
              {
                mask = 0;
                changedFlags = -1;
                changedToValue = false;
              }
              else if (MaskFieldGUI.MaskCallbackInfo.m_Instance.m_SetAll)
              {
                mask = -1;
                changedFlags = -1;
                changedToValue = true;
              }
              else
              {
                mask ^= MaskFieldGUI.MaskCallbackInfo.m_Instance.m_Mask;
                changedFlags = MaskFieldGUI.MaskCallbackInfo.m_Instance.m_Mask;
                changedToValue = (mask & MaskFieldGUI.MaskCallbackInfo.m_Instance.m_Mask) != 0;
              }
              GUI.changed = true;
            }
            MaskFieldGUI.MaskCallbackInfo.m_Instance.m_DoNothing = false;
            MaskFieldGUI.MaskCallbackInfo.m_Instance.m_ClearAll = false;
            MaskFieldGUI.MaskCallbackInfo.m_Instance.m_SetAll = false;
            MaskFieldGUI.MaskCallbackInfo.m_Instance = (MaskFieldGUI.MaskCallbackInfo) null;
            current.Use();
          }
        }
        return mask;
      }

      internal void SetMaskValueDelegate(object userData, string[] options, int selected)
      {
        switch (selected)
        {
          case 0:
            this.m_ClearAll = true;
            break;
          case 1:
            this.m_SetAll = true;
            break;
          default:
            this.m_Mask = 1 << selected - 2;
            break;
        }
        if (!(bool) ((Object) this.m_SourceView))
          return;
        this.m_SourceView.SendEvent(EditorGUIUtility.CommandEvent("MaskMenuChanged"));
      }
    }
  }
}

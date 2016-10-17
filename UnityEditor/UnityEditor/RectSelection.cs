// Decompiled with JetBrains decompiler
// Type: UnityEditor.RectSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class RectSelection
  {
    private static int s_RectSelectionID = GUIUtility.GetPermanentControlID();
    private Vector2 m_SelectStartPoint;
    private Vector2 m_SelectMousePoint;
    private UnityEngine.Object[] m_SelectionStart;
    private bool m_RectSelecting;
    private Dictionary<GameObject, bool> m_LastSelection;
    private UnityEngine.Object[] m_CurrentSelection;
    private EditorWindow m_Window;

    public RectSelection(EditorWindow window)
    {
      this.m_Window = window;
    }

    public void OnGUI()
    {
      Event current1 = Event.current;
      Handles.BeginGUI();
      Vector2 mousePosition = current1.mousePosition;
      int rectSelectionId = RectSelection.s_RectSelectionID;
      EventType typeForControl = current1.GetTypeForControl(rectSelectionId);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == rectSelectionId && current1.button == 0)
          {
            GUIUtility.hotControl = rectSelectionId;
            this.m_SelectStartPoint = mousePosition;
            this.m_SelectionStart = Selection.objects;
            this.m_RectSelecting = false;
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == rectSelectionId && current1.button == 0)
          {
            GUIUtility.hotControl = 0;
            if (this.m_RectSelecting)
            {
              EditorApplication.modifierKeysChanged -= new EditorApplication.CallbackFunction(this.SendCommandsOnModifierKeys);
              this.m_RectSelecting = false;
              this.m_SelectionStart = new UnityEngine.Object[0];
              current1.Use();
              break;
            }
            if (current1.shift || EditorGUI.actionKey)
            {
              GameObject[] gameObjectArray;
              if (current1.shift)
                gameObjectArray = new GameObject[1]
                {
                  Selection.activeGameObject
                };
              else
                gameObjectArray = Selection.gameObjects;
              GameObject[] gameObjects = gameObjectArray;
              GameObject hovered = SceneViewPicking.GetHovered(current1.mousePosition, gameObjects);
              if ((UnityEngine.Object) hovered != (UnityEngine.Object) null)
                RectSelection.UpdateSelection(this.m_SelectionStart, (UnityEngine.Object) hovered, RectSelection.SelectionType.Subtractive, this.m_RectSelecting);
              else
                RectSelection.UpdateSelection(this.m_SelectionStart, (UnityEngine.Object) HandleUtility.PickGameObject(current1.mousePosition, true), RectSelection.SelectionType.Additive, this.m_RectSelecting);
            }
            else
              RectSelection.UpdateSelection(this.m_SelectionStart, (UnityEngine.Object) SceneViewPicking.PickGameObject(current1.mousePosition), RectSelection.SelectionType.Normal, this.m_RectSelecting);
            current1.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == rectSelectionId)
          {
            if (!this.m_RectSelecting && (double) (mousePosition - this.m_SelectStartPoint).magnitude > 6.0)
            {
              EditorApplication.modifierKeysChanged += new EditorApplication.CallbackFunction(this.SendCommandsOnModifierKeys);
              this.m_RectSelecting = true;
              this.m_LastSelection = (Dictionary<GameObject, bool>) null;
              this.m_CurrentSelection = (UnityEngine.Object[]) null;
            }
            if (this.m_RectSelecting)
            {
              this.m_SelectMousePoint = new Vector2(Mathf.Max(mousePosition.x, 0.0f), Mathf.Max(mousePosition.y, 0.0f));
              GameObject[] gameObjectArray = HandleUtility.PickRectObjects(EditorGUIExt.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint));
              this.m_CurrentSelection = (UnityEngine.Object[]) gameObjectArray;
              bool flag1 = false;
              if (this.m_LastSelection == null)
              {
                this.m_LastSelection = new Dictionary<GameObject, bool>();
                flag1 = true;
              }
              bool flag2 = flag1 | this.m_LastSelection.Count != gameObjectArray.Length;
              if (!flag2)
              {
                Dictionary<GameObject, bool> dictionary = new Dictionary<GameObject, bool>(gameObjectArray.Length);
                foreach (GameObject key in gameObjectArray)
                  dictionary.Add(key, false);
                using (Dictionary<GameObject, bool>.KeyCollection.Enumerator enumerator = this.m_LastSelection.Keys.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    GameObject current2 = enumerator.Current;
                    if (!dictionary.ContainsKey(current2))
                    {
                      flag2 = true;
                      break;
                    }
                  }
                }
              }
              if (flag2)
              {
                this.m_LastSelection = new Dictionary<GameObject, bool>(gameObjectArray.Length);
                foreach (GameObject key in gameObjectArray)
                  this.m_LastSelection.Add(key, false);
                if (gameObjectArray != null)
                {
                  if (current1.shift)
                    RectSelection.UpdateSelection(this.m_SelectionStart, (UnityEngine.Object[]) gameObjectArray, RectSelection.SelectionType.Additive, this.m_RectSelecting);
                  else if (EditorGUI.actionKey)
                    RectSelection.UpdateSelection(this.m_SelectionStart, (UnityEngine.Object[]) gameObjectArray, RectSelection.SelectionType.Subtractive, this.m_RectSelecting);
                  else
                    RectSelection.UpdateSelection(this.m_SelectionStart, (UnityEngine.Object[]) gameObjectArray, RectSelection.SelectionType.Normal, this.m_RectSelecting);
                }
              }
            }
            current1.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (GUIUtility.hotControl == rectSelectionId && this.m_RectSelecting)
          {
            EditorStyles.selectionRect.Draw(EditorGUIExt.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint), GUIContent.none, false, false, false, false);
            break;
          }
          break;
        case EventType.Layout:
          if (!Tools.viewToolActive)
          {
            HandleUtility.AddDefaultControl(rectSelectionId);
            break;
          }
          break;
        default:
          if (typeForControl == EventType.ExecuteCommand && rectSelectionId == GUIUtility.hotControl && current1.commandName == "ModifierKeysChanged")
          {
            if (current1.shift)
              RectSelection.UpdateSelection(this.m_SelectionStart, this.m_CurrentSelection, RectSelection.SelectionType.Additive, this.m_RectSelecting);
            else if (EditorGUI.actionKey)
              RectSelection.UpdateSelection(this.m_SelectionStart, this.m_CurrentSelection, RectSelection.SelectionType.Subtractive, this.m_RectSelecting);
            else
              RectSelection.UpdateSelection(this.m_SelectionStart, this.m_CurrentSelection, RectSelection.SelectionType.Normal, this.m_RectSelecting);
            current1.Use();
            break;
          }
          break;
      }
      Handles.EndGUI();
    }

    private static void UpdateSelection(UnityEngine.Object[] existingSelection, UnityEngine.Object newObject, RectSelection.SelectionType type, bool isRectSelection)
    {
      UnityEngine.Object[] newObjects;
      if (newObject == (UnityEngine.Object) null)
        newObjects = new UnityEngine.Object[0];
      else
        newObjects = new UnityEngine.Object[1]{ newObject };
      RectSelection.UpdateSelection(existingSelection, newObjects, type, isRectSelection);
    }

    private static void UpdateSelection(UnityEngine.Object[] existingSelection, UnityEngine.Object[] newObjects, RectSelection.SelectionType type, bool isRectSelection)
    {
      switch (type)
      {
        case RectSelection.SelectionType.Additive:
          if (newObjects.Length > 0)
          {
            UnityEngine.Object[] objectArray = new UnityEngine.Object[existingSelection.Length + newObjects.Length];
            Array.Copy((Array) existingSelection, (Array) objectArray, existingSelection.Length);
            for (int index = 0; index < newObjects.Length; ++index)
              objectArray[existingSelection.Length + index] = newObjects[index];
            Selection.activeObject = isRectSelection ? objectArray[0] : newObjects[0];
            Selection.objects = objectArray;
            break;
          }
          Selection.objects = existingSelection;
          break;
        case RectSelection.SelectionType.Subtractive:
          Dictionary<UnityEngine.Object, bool> dictionary = new Dictionary<UnityEngine.Object, bool>(existingSelection.Length);
          foreach (UnityEngine.Object key in existingSelection)
            dictionary.Add(key, false);
          foreach (UnityEngine.Object newObject in newObjects)
          {
            if (dictionary.ContainsKey(newObject))
              dictionary.Remove(newObject);
          }
          UnityEngine.Object[] array = new UnityEngine.Object[dictionary.Keys.Count];
          dictionary.Keys.CopyTo(array, 0);
          Selection.objects = array;
          break;
        default:
          Selection.objects = newObjects;
          break;
      }
    }

    internal void SendCommandsOnModifierKeys()
    {
      this.m_Window.SendEvent(EditorGUIUtility.CommandEvent("ModifierKeysChanged"));
    }

    private enum SelectionType
    {
      Normal,
      Additive,
      Subtractive,
    }
  }
}

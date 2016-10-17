// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AddCurvesPopup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AddCurvesPopup : EditorWindow
  {
    private static Vector2 windowSize = new Vector2(240f, 250f);
    private const float k_WindowPadding = 3f;
    internal static AnimationWindowState s_State;
    private static AddCurvesPopup s_AddCurvesPopup;
    private static long s_LastClosedTime;
    private static AddCurvesPopupHierarchy s_Hierarchy;

    internal static UnityEngine.Object animatableObject { get; set; }

    internal static GameObject gameObject { get; set; }

    internal static string path
    {
      get
      {
        return AnimationUtility.CalculateTransformPath(AddCurvesPopup.gameObject.transform, AddCurvesPopup.s_State.activeRootGameObject.transform);
      }
    }

    private void Init(Rect buttonRect)
    {
      buttonRect = GUIUtility.GUIToScreenRect(buttonRect);
      this.ShowAsDropDown(buttonRect, AddCurvesPopup.windowSize, new PopupLocationHelper.PopupLocation[1]
      {
        PopupLocationHelper.PopupLocation.Right
      });
    }

    private void OnDisable()
    {
      AddCurvesPopup.s_LastClosedTime = DateTime.Now.Ticks / 10000L;
      AddCurvesPopup.s_AddCurvesPopup = (AddCurvesPopup) null;
      AddCurvesPopup.s_Hierarchy = (AddCurvesPopupHierarchy) null;
    }

    internal static void AddNewCurve(AddCurvesPopupPropertyNode node)
    {
      AnimationWindowUtility.CreateDefaultCurves(AddCurvesPopup.s_State, node.curveBindings);
      TreeViewItem treeViewItem = !(node.parent.displayName == "GameObject") ? node.parent.parent : node.parent;
      AddCurvesPopup.s_State.hierarchyState.selectedIDs.Clear();
      AddCurvesPopup.s_State.hierarchyState.selectedIDs.Add(treeViewItem.id);
      AddCurvesPopup.s_State.hierarchyData.SetExpanded(treeViewItem, true);
      AddCurvesPopup.s_State.hierarchyData.SetExpanded(node.parent.id, true);
    }

    internal static bool ShowAtPosition(Rect buttonRect, AnimationWindowState state)
    {
      if (DateTime.Now.Ticks / 10000L < AddCurvesPopup.s_LastClosedTime + 50L)
        return false;
      Event.current.Use();
      if ((UnityEngine.Object) AddCurvesPopup.s_AddCurvesPopup == (UnityEngine.Object) null)
        AddCurvesPopup.s_AddCurvesPopup = ScriptableObject.CreateInstance<AddCurvesPopup>();
      AddCurvesPopup.s_State = state;
      AddCurvesPopup.s_AddCurvesPopup.Init(buttonRect);
      return true;
    }

    internal void OnGUI()
    {
      if (Event.current.type == EventType.Layout)
        return;
      if (AddCurvesPopup.s_Hierarchy == null)
        AddCurvesPopup.s_Hierarchy = new AddCurvesPopupHierarchy(AddCurvesPopup.s_State);
      Rect position = new Rect(1f, 1f, AddCurvesPopup.windowSize.x - 3f, AddCurvesPopup.windowSize.y - 3f);
      GUI.Box(new Rect(0.0f, 0.0f, AddCurvesPopup.windowSize.x, AddCurvesPopup.windowSize.y), GUIContent.none, new GUIStyle((GUIStyle) "grey_border"));
      AddCurvesPopup.s_Hierarchy.OnGUI(position, (EditorWindow) this);
    }
  }
}

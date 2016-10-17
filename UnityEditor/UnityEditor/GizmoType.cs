// Decompiled with JetBrains decompiler
// Type: UnityEditor.GizmoType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Determines how a gizmo is drawn or picked in the Unity editor.</para>
  /// </summary>
  public enum GizmoType
  {
    [Obsolete("Use NotInSelectionHierarchy instead (UnityUpgradable) -> NotInSelectionHierarchy")] NotSelected = -127,
    [Obsolete("Use InSelectionHierarchy instead (UnityUpgradable) -> InSelectionHierarchy")] SelectedOrChild = -127,
    Pickable = 1,
    NotInSelectionHierarchy = 2,
    Selected = 4,
    Active = 8,
    InSelectionHierarchy = 16,
    NonSelected = 32,
  }
}

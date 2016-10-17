// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchyGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationWindowHierarchyGUI : TreeViewGUI
  {
    private static readonly Color k_KeyColorInDopesheetMode = new Color(0.7f, 0.7f, 0.7f, 1f);
    private static readonly Color k_KeyColorForNonCurves = new Color(0.7f, 0.7f, 0.7f, 0.5f);
    private static readonly Color k_LeftoverCurveColor = Color.yellow;
    internal static int s_WasInsideValueRectFrame = -1;
    private readonly GUIContent k_AnimatePropertyLabel = new GUIContent("Add Property");
    private Color m_LightSkinPropertyTextColor = new Color(0.35f, 0.35f, 0.35f);
    private const float k_RowRightOffset = 10f;
    private const float k_ValueFieldWidth = 50f;
    private const float k_ValueFieldOffsetFromRightSide = 75f;
    private const float k_ColorIndicatorTopMargin = 3f;
    public const float k_DopeSheetRowHeight = 16f;
    public const float k_DopeSheetRowHeightTall = 32f;
    public const float k_AddCurveButtonNodeHeight = 40f;
    public const float k_RowBackgroundColorBrightness = 0.28f;
    private GUIStyle m_PlusButtonStyle;
    private GUIStyle m_AnimationRowEvenStyle;
    private GUIStyle m_AnimationRowOddStyle;
    private GUIStyle m_AnimationSelectionTextField;
    private GUIStyle m_AnimationLineStyle;
    private GUIStyle m_AnimationCurveDropdown;
    private AnimationWindowHierarchyNode m_RenamedNode;

    public AnimationWindowState state { get; set; }

    public AnimationWindowHierarchyGUI(TreeView treeView, AnimationWindowState state)
      : base(treeView)
    {
      this.state = state;
    }

    protected override void InitStyles()
    {
      base.InitStyles();
      if (this.m_PlusButtonStyle == null)
        this.m_PlusButtonStyle = new GUIStyle((GUIStyle) "OL Plus");
      if (this.m_AnimationRowEvenStyle == null)
        this.m_AnimationRowEvenStyle = new GUIStyle((GUIStyle) "AnimationRowEven");
      if (this.m_AnimationRowOddStyle == null)
        this.m_AnimationRowOddStyle = new GUIStyle((GUIStyle) "AnimationRowOdd");
      if (this.m_AnimationSelectionTextField == null)
        this.m_AnimationSelectionTextField = new GUIStyle((GUIStyle) "AnimationSelectionTextField");
      if (this.m_AnimationLineStyle == null)
      {
        this.m_AnimationLineStyle = new GUIStyle(TreeViewGUI.s_Styles.lineStyle);
        this.m_AnimationLineStyle.padding.left = 0;
      }
      if (this.m_AnimationCurveDropdown != null)
        return;
      this.m_AnimationCurveDropdown = new GUIStyle((GUIStyle) "AnimPropDropdown");
    }

    protected void DoNodeGUI(Rect rect, AnimationWindowHierarchyNode node, bool selected, bool focused, int row)
    {
      this.InitStyles();
      if (node is AnimationWindowHierarchyMasterNode)
        return;
      float indent = this.k_BaseIndent + (float) (node.depth + node.indent) * this.k_IndentWidth;
      if (node is AnimationWindowHierarchyAddButtonNode)
      {
        if (Event.current.type == EventType.MouseMove && AnimationWindowHierarchyGUI.s_WasInsideValueRectFrame >= 0)
        {
          if (AnimationWindowHierarchyGUI.s_WasInsideValueRectFrame >= Time.frameCount - 1)
            Event.current.Use();
          else
            AnimationWindowHierarchyGUI.s_WasInsideValueRectFrame = -1;
        }
        EditorGUI.BeginDisabledGroup(!(bool) ((UnityEngine.Object) this.state.activeGameObject) || !AnimationWindowUtility.GameObjectIsAnimatable(this.state.activeGameObject, this.state.activeAnimationClip));
        this.DoAddCurveButton(rect);
        EditorGUI.EndDisabledGroup();
      }
      else
      {
        this.DoRowBackground(rect, row);
        this.DoIconAndName(rect, node, selected, focused, indent);
        this.DoFoldout(node, rect, indent);
        EditorGUI.BeginDisabledGroup(this.state.animationIsReadOnly);
        this.DoValueField(rect, node, row);
        this.HandleContextMenu(rect, node);
        EditorGUI.EndDisabledGroup();
        this.DoCurveDropdown(rect, node);
        this.DoCurveColorIndicator(rect, node);
      }
      EditorGUIUtility.SetIconSize(Vector2.zero);
    }

    public override void BeginRowGUI()
    {
      base.BeginRowGUI();
      this.HandleDelete();
    }

    private void DoAddCurveButton(Rect rect)
    {
      float num1 = (float) (((double) rect.width - 230.0) / 2.0);
      float num2 = 10f;
      Rect rect1 = new Rect(rect.xMin + num1, rect.yMin + num2, rect.width - num1 * 2f, rect.height - num2 * 2f);
      if (!GUI.Button(rect1, this.k_AnimatePropertyLabel))
        return;
      AddCurvesPopup.gameObject = this.state.activeRootGameObject;
      AddCurvesPopupHierarchyDataSource.showEntireHierarchy = true;
      if (!AddCurvesPopup.ShowAtPosition(rect1, this.state))
        return;
      GUIUtility.ExitGUI();
    }

    private void DoRowBackground(Rect rect, int row)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if (row % 2 == 0)
        this.m_AnimationRowEvenStyle.Draw(rect, false, false, false, false);
      else
        this.m_AnimationRowOddStyle.Draw(rect, false, false, false, false);
    }

    private void DoFoldout(AnimationWindowHierarchyNode node, Rect rect, float indent)
    {
      if (this.m_TreeView.data.IsExpandable((TreeViewItem) node))
      {
        Rect position = rect;
        position.x = indent;
        position.width = this.k_FoldoutWidth;
        EditorGUI.BeginChangeCheck();
        bool expand = GUI.Toggle(position, this.m_TreeView.data.IsExpanded((TreeViewItem) node), GUIContent.none, TreeViewGUI.s_Styles.foldout);
        if (!EditorGUI.EndChangeCheck())
          return;
        if (Event.current.alt)
          this.m_TreeView.data.SetExpandedWithChildren((TreeViewItem) node, expand);
        else
          this.m_TreeView.data.SetExpanded((TreeViewItem) node, expand);
        if (!expand)
          return;
        this.m_TreeView.UserExpandedNode((TreeViewItem) node);
      }
      else
      {
        AnimationWindowHierarchyPropertyNode hierarchyPropertyNode = node as AnimationWindowHierarchyPropertyNode;
        AnimationWindowHierarchyState state = this.m_TreeView.state as AnimationWindowHierarchyState;
        if (hierarchyPropertyNode == null || !hierarchyPropertyNode.isPptrNode)
          return;
        Rect position = rect;
        position.x = indent;
        position.width = this.k_FoldoutWidth;
        EditorGUI.BeginChangeCheck();
        bool tallMode1 = state.GetTallMode((AnimationWindowHierarchyNode) hierarchyPropertyNode);
        bool tallMode2 = GUI.Toggle(position, tallMode1, GUIContent.none, TreeViewGUI.s_Styles.foldout);
        if (!EditorGUI.EndChangeCheck())
          return;
        state.SetTallMode((AnimationWindowHierarchyNode) hierarchyPropertyNode, tallMode2);
      }
    }

    private void DoIconAndName(Rect rect, AnimationWindowHierarchyNode node, bool selected, bool focused, float indent)
    {
      EditorGUIUtility.SetIconSize(new Vector2(13f, 13f));
      if (Event.current.type == EventType.Repaint)
      {
        if (selected)
          TreeViewGUI.s_Styles.selectionStyle.Draw(rect, false, false, true, focused);
        if (AnimationMode.InAnimationMode())
          rect.width -= 77f;
        bool flag1 = AnimationWindowUtility.IsNodeLeftOverCurve(node, this.state.activeRootGameObject);
        bool flag2 = AnimationWindowUtility.IsNodeAmbiguous(node, this.state.activeRootGameObject);
        string str = string.Empty;
        string tooltip = string.Empty;
        if (flag1)
        {
          str = " (Missing!)";
          tooltip = "The GameObject or Component is missing (" + node.path + ")";
        }
        if (flag2)
        {
          str = " (Duplicate GameObject name!)";
          tooltip = "Target for curve is ambiguous since there are multiple GameObjects with same name (" + node.path + ")";
        }
        if (node.depth == 0)
        {
          if ((UnityEngine.Object) this.state.activeRootGameObject != (UnityEngine.Object) null && (UnityEngine.Object) this.state.activeRootGameObject.transform.Find(node.path) == (UnityEngine.Object) null)
            flag1 = true;
          TreeViewGUI.s_Styles.content = new GUIContent(this.GetGameObjectName(node.path) + " : " + node.displayName + str, this.GetIconForNode((TreeViewItem) node), tooltip);
          Color textColor = this.m_AnimationLineStyle.normal.textColor;
          Color color = !EditorGUIUtility.isProSkin ? Color.black : Color.gray * 1.35f;
          this.SetStyleTextColor(this.m_AnimationLineStyle, flag1 || flag2 ? AnimationWindowHierarchyGUI.k_LeftoverCurveColor : color);
          rect.xMin += (float) (int) ((double) indent + (double) this.k_FoldoutWidth);
          GUI.Label(rect, TreeViewGUI.s_Styles.content, this.m_AnimationLineStyle);
          this.SetStyleTextColor(this.m_AnimationLineStyle, textColor);
        }
        else
        {
          TreeViewGUI.s_Styles.content = new GUIContent(node.displayName + str, this.GetIconForNode((TreeViewItem) node), tooltip);
          Color textColor = this.m_AnimationLineStyle.normal.textColor;
          Color color = !EditorGUIUtility.isProSkin ? this.m_LightSkinPropertyTextColor : Color.gray;
          this.SetStyleTextColor(this.m_AnimationLineStyle, flag1 || flag2 ? AnimationWindowHierarchyGUI.k_LeftoverCurveColor : color);
          rect.xMin += (float) (int) ((double) indent + (double) this.k_IndentWidth + (double) this.k_FoldoutWidth);
          GUI.Label(rect, TreeViewGUI.s_Styles.content, this.m_AnimationLineStyle);
          this.SetStyleTextColor(this.m_AnimationLineStyle, textColor);
        }
      }
      if (!this.IsRenaming(node.id) || Event.current.type == EventType.Layout)
        return;
      this.GetRenameOverlay().editFieldRect = new Rect(rect.x + this.k_IndentWidth, rect.y, (float) ((double) rect.width - (double) this.k_IndentWidth - 1.0), rect.height);
    }

    private string GetGameObjectName(string path)
    {
      if (string.IsNullOrEmpty(path) && (UnityEngine.Object) this.state.activeRootGameObject != (UnityEngine.Object) null)
        return this.state.activeRootGameObject.name;
      string[] strArray = path.Split('/');
      return strArray[strArray.Length - 1];
    }

    private string GetPathWithoutChildmostGameObject(string path)
    {
      if (string.IsNullOrEmpty(path))
        return string.Empty;
      int num = path.LastIndexOf('/');
      return path.Substring(0, num + 1);
    }

    private void DoValueField(Rect rect, AnimationWindowHierarchyNode node, int row)
    {
      bool flag1 = false;
      if (!AnimationMode.InAnimationMode())
        return;
      EditorGUI.BeginDisabledGroup(this.state.animationIsReadOnly);
      if (node is AnimationWindowHierarchyPropertyNode)
      {
        List<AnimationWindowCurve> curves = this.state.GetCurves(node, false);
        if (curves == null || curves.Count == 0)
          return;
        AnimationWindowCurve curve = curves[0];
        object currentValue = CurveBindingUtility.GetCurrentValue(this.state.activeRootGameObject, curve.binding);
        System.Type editorCurveValueType = CurveBindingUtility.GetEditorCurveValueType(this.state.activeRootGameObject, curve.binding);
        if (currentValue is float)
        {
          float num = (float) currentValue;
          Rect position = new Rect(rect.xMax - 75f, rect.y, 50f, rect.height);
          if (Event.current.type == EventType.MouseMove && position.Contains(Event.current.mousePosition))
            AnimationWindowHierarchyGUI.s_WasInsideValueRectFrame = Time.frameCount;
          EditorGUI.BeginChangeCheck();
          float f;
          if (editorCurveValueType == typeof (bool))
          {
            f = !EditorGUI.Toggle(position, (double) num != 0.0) ? 0.0f : 1f;
          }
          else
          {
            int controlId = GUIUtility.GetControlID(123456544, FocusType.Keyboard, position);
            bool flag2 = GUIUtility.keyboardControl == controlId && EditorGUIUtility.editingTextField && Event.current.type == EventType.KeyDown && ((int) Event.current.character == 10 || (int) Event.current.character == 3);
            f = EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, position, new Rect(0.0f, 0.0f, 0.0f, 0.0f), controlId, num, EditorGUI.kFloatFieldFormatString, this.m_AnimationSelectionTextField, false);
            if (flag2)
            {
              GUI.changed = true;
              Event.current.Use();
            }
          }
          if (float.IsInfinity(f) || float.IsNaN(f))
            f = 0.0f;
          if (EditorGUI.EndChangeCheck())
          {
            AnimationWindowKeyframe animationWindowKeyframe = (AnimationWindowKeyframe) null;
            using (List<AnimationWindowKeyframe>.Enumerator enumerator = curve.m_Keyframes.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                AnimationWindowKeyframe current = enumerator.Current;
                if (Mathf.Approximately(current.time, this.state.time.time))
                  animationWindowKeyframe = current;
              }
            }
            if (animationWindowKeyframe == null)
              AnimationWindowUtility.AddKeyframeToCurve(curve, (object) f, editorCurveValueType, this.state.time);
            else
              animationWindowKeyframe.value = (object) f;
            this.state.SaveCurve(curve);
            flag1 = true;
          }
        }
      }
      EditorGUI.EndDisabledGroup();
      if (!flag1)
        return;
      this.state.ResampleAnimation();
    }

    private void DoCurveDropdown(Rect rect, AnimationWindowHierarchyNode node)
    {
      rect = new Rect((float) ((double) rect.xMax - 10.0 - 12.0), rect.yMin + 2f, 22f, 12f);
      if (!GUI.Button(rect, GUIContent.none, this.m_AnimationCurveDropdown))
        return;
      this.state.SelectHierarchyItem(node.id, false, false);
      this.GenerateMenu(((IEnumerable<AnimationWindowHierarchyNode>) new AnimationWindowHierarchyNode[1]
      {
        node
      }).ToList<AnimationWindowHierarchyNode>()).DropDown(rect);
      Event.current.Use();
    }

    private void DoCurveColorIndicator(Rect rect, AnimationWindowHierarchyNode node)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color color = GUI.color;
      GUI.color = this.state.showCurveEditor ? (node.curves.Length != 1 || node.curves[0].isPPtrCurve ? AnimationWindowHierarchyGUI.k_KeyColorForNonCurves : CurveUtility.GetPropertyColor(node.curves[0].binding.propertyName)) : AnimationWindowHierarchyGUI.k_KeyColorInDopesheetMode;
      bool flag = false;
      if (AnimationMode.InAnimationMode())
      {
        foreach (AnimationWindowCurve curve in node.curves)
        {
          if (curve.m_Keyframes.Any<AnimationWindowKeyframe>((Func<AnimationWindowKeyframe, bool>) (key => this.state.time.ContainsTime(key.time))))
            flag = true;
        }
      }
      Texture image = !flag ? (Texture) CurveUtility.GetIconCurve() : (Texture) CurveUtility.GetIconKey();
      rect = new Rect((float) ((double) rect.xMax - 10.0 - (double) (image.width / 2) - 5.0), rect.yMin + 3f, (float) image.width, (float) image.height);
      GUI.DrawTexture(rect, image, ScaleMode.ScaleToFit, true, 1f);
      GUI.color = color;
    }

    private void HandleDelete()
    {
      if (!this.m_TreeView.HasFocus())
        return;
      switch (Event.current.type)
      {
        case EventType.KeyDown:
          if (Event.current.keyCode != KeyCode.Backspace && Event.current.keyCode != KeyCode.Delete)
            break;
          this.RemoveCurvesFromSelectedNodes();
          Event.current.Use();
          break;
        case EventType.ExecuteCommand:
          if (!(Event.current.commandName == "SoftDelete") && !(Event.current.commandName == "Delete"))
            break;
          if (Event.current.type == EventType.ExecuteCommand)
            this.RemoveCurvesFromSelectedNodes();
          Event.current.Use();
          break;
      }
    }

    private void HandleContextMenu(Rect rect, AnimationWindowHierarchyNode node)
    {
      if (Event.current.type != EventType.ContextClick || !rect.Contains(Event.current.mousePosition))
        return;
      this.state.SelectHierarchyItem(node.id, false, true);
      this.GenerateMenu(this.state.selectedHierarchyNodes).ShowAsContext();
      Event.current.Use();
    }

    private GenericMenu GenerateMenu(List<AnimationWindowHierarchyNode> interactedNodes)
    {
      List<AnimationWindowCurve> curvesAffectedByNodes1 = this.GetCurvesAffectedByNodes(interactedNodes, false);
      List<AnimationWindowCurve> curvesAffectedByNodes2 = this.GetCurvesAffectedByNodes(interactedNodes, true);
      bool flag1 = curvesAffectedByNodes1.Count == 1 && AnimationWindowUtility.ForceGrouping(curvesAffectedByNodes1[0].binding);
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(new GUIContent(curvesAffectedByNodes1.Count > 1 || flag1 ? "Remove Properties" : "Remove Property"), false, new GenericMenu.MenuFunction(this.RemoveCurvesFromSelectedNodes));
      bool flag2 = true;
      EditorCurveBinding[] curves = new EditorCurveBinding[curvesAffectedByNodes2.Count];
      for (int index = 0; index < curvesAffectedByNodes2.Count; ++index)
        curves[index] = curvesAffectedByNodes2[index].binding;
      RotationCurveInterpolation.Mode interpolationMode = this.GetRotationInterpolationMode(curves);
      if (interpolationMode == RotationCurveInterpolation.Mode.Undefined)
      {
        flag2 = false;
      }
      else
      {
        using (List<AnimationWindowHierarchyNode>.Enumerator enumerator = interactedNodes.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            if (!(enumerator.Current is AnimationWindowHierarchyPropertyGroupNode))
              flag2 = false;
          }
        }
      }
      if (flag2)
      {
        string str = !this.state.activeAnimationClip.legacy ? string.Empty : " (Not fully supported in Legacy)";
        genericMenu.AddItem(new GUIContent("Interpolation/Euler Angles" + str), interpolationMode == RotationCurveInterpolation.Mode.RawEuler, new GenericMenu.MenuFunction2(this.ChangeRotationInterpolation), (object) RotationCurveInterpolation.Mode.RawEuler);
        genericMenu.AddItem(new GUIContent("Interpolation/Euler Angles (Quaternion Approximation)"), interpolationMode == RotationCurveInterpolation.Mode.Baked, new GenericMenu.MenuFunction2(this.ChangeRotationInterpolation), (object) RotationCurveInterpolation.Mode.Baked);
        genericMenu.AddItem(new GUIContent("Interpolation/Quaternion"), interpolationMode == RotationCurveInterpolation.Mode.NonBaked, new GenericMenu.MenuFunction2(this.ChangeRotationInterpolation), (object) RotationCurveInterpolation.Mode.NonBaked);
      }
      if (AnimationMode.InAnimationMode())
      {
        genericMenu.AddSeparator(string.Empty);
        bool flag3 = true;
        bool flag4 = true;
        bool flag5 = true;
        using (List<AnimationWindowCurve>.Enumerator enumerator = curvesAffectedByNodes1.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnimationWindowCurve current = enumerator.Current;
            if (!current.HasKeyframe(this.state.time))
            {
              flag3 = false;
            }
            else
            {
              flag4 = false;
              if (!current.isPPtrCurve)
                flag5 = false;
            }
          }
        }
        string text1 = "Add Key";
        if (flag3)
          genericMenu.AddDisabledItem(new GUIContent(text1));
        else
          genericMenu.AddItem(new GUIContent(text1), false, new GenericMenu.MenuFunction2(this.AddKeysAtCurrentTime), (object) curvesAffectedByNodes1);
        string text2 = "Delete Key";
        if (flag4)
          genericMenu.AddDisabledItem(new GUIContent(text2));
        else
          genericMenu.AddItem(new GUIContent(text2), false, new GenericMenu.MenuFunction2(this.DeleteKeysAtCurrentTime), (object) curvesAffectedByNodes1);
        if (!flag5)
        {
          genericMenu.AddSeparator(string.Empty);
          List<KeyIdentifier> keyIdentifierList = new List<KeyIdentifier>();
          using (List<AnimationWindowCurve>.Enumerator enumerator = curvesAffectedByNodes1.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              AnimationWindowCurve current = enumerator.Current;
              if (!current.isPPtrCurve)
              {
                int keyframeIndex = current.GetKeyframeIndex(this.state.time);
                if (keyframeIndex != -1)
                {
                  CurveRenderer curveRenderer = CurveRendererCache.GetCurveRenderer(this.state.activeAnimationClip, current.binding);
                  int curveId = CurveUtility.GetCurveID(this.state.activeAnimationClip, current.binding);
                  keyIdentifierList.Add(new KeyIdentifier(curveRenderer, curveId, keyframeIndex));
                }
              }
            }
          }
        }
      }
      return genericMenu;
    }

    private void AddKeysAtCurrentTime(object obj)
    {
      this.AddKeysAtCurrentTime((List<AnimationWindowCurve>) obj);
    }

    private void AddKeysAtCurrentTime(List<AnimationWindowCurve> curves)
    {
      using (List<AnimationWindowCurve>.Enumerator enumerator = curves.GetEnumerator())
      {
        while (enumerator.MoveNext())
          AnimationWindowUtility.AddKeyframeToCurve(this.state, enumerator.Current, this.state.time);
      }
    }

    private void DeleteKeysAtCurrentTime(object obj)
    {
      this.DeleteKeysAtCurrentTime((List<AnimationWindowCurve>) obj);
    }

    private void DeleteKeysAtCurrentTime(List<AnimationWindowCurve> curves)
    {
      using (List<AnimationWindowCurve>.Enumerator enumerator = curves.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowCurve current = enumerator.Current;
          current.RemoveKeyframe(this.state.time);
          this.state.SaveCurve(current);
        }
      }
    }

    private void ChangeRotationInterpolation(object interpolationMode)
    {
      RotationCurveInterpolation.Mode mode = (RotationCurveInterpolation.Mode) interpolationMode;
      AnimationWindowCurve[] array = this.state.activeCurves.ToArray();
      EditorCurveBinding[] curveBindings = new EditorCurveBinding[array.Length];
      for (int index = 0; index < array.Length; ++index)
        curveBindings[index] = array[index].binding;
      RotationCurveInterpolation.SetInterpolation(this.state.activeAnimationClip, curveBindings, mode);
      this.MaintainTreeviewStateAfterRotationInterpolation(mode);
      this.state.hierarchyData.ReloadData();
    }

    private void RemoveCurvesFromSelectedNodes()
    {
      this.RemoveCurvesFromNodes(this.state.selectedHierarchyNodes);
    }

    private void RemoveCurvesFromNodes(List<AnimationWindowHierarchyNode> nodes)
    {
      using (List<AnimationWindowHierarchyNode>.Enumerator enumerator1 = nodes.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          AnimationWindowHierarchyNode windowHierarchyNode = enumerator1.Current;
          if (windowHierarchyNode.parent is AnimationWindowHierarchyPropertyGroupNode && windowHierarchyNode.binding.HasValue && AnimationWindowUtility.ForceGrouping(windowHierarchyNode.binding.Value))
            windowHierarchyNode = (AnimationWindowHierarchyNode) windowHierarchyNode.parent;
          using (List<AnimationWindowCurve>.Enumerator enumerator2 = (windowHierarchyNode is AnimationWindowHierarchyPropertyGroupNode || windowHierarchyNode is AnimationWindowHierarchyPropertyNode ? AnimationWindowUtility.FilterCurves(this.state.allCurves.ToArray(), windowHierarchyNode.path, windowHierarchyNode.animatableObjectType, windowHierarchyNode.propertyName) : AnimationWindowUtility.FilterCurves(this.state.allCurves.ToArray(), windowHierarchyNode.path, windowHierarchyNode.animatableObjectType)).GetEnumerator())
          {
            while (enumerator2.MoveNext())
              this.state.RemoveCurve(enumerator2.Current);
          }
        }
      }
      this.m_TreeView.ReloadData();
    }

    private List<AnimationWindowCurve> GetCurvesAffectedByNodes(List<AnimationWindowHierarchyNode> nodes, bool includeLinkedCurves)
    {
      List<AnimationWindowCurve> source = new List<AnimationWindowCurve>();
      using (List<AnimationWindowHierarchyNode>.Enumerator enumerator = nodes.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowHierarchyNode windowHierarchyNode = enumerator.Current;
          if (windowHierarchyNode.parent is AnimationWindowHierarchyPropertyGroupNode && includeLinkedCurves)
            windowHierarchyNode = (AnimationWindowHierarchyNode) windowHierarchyNode.parent;
          if (windowHierarchyNode is AnimationWindowHierarchyPropertyGroupNode || windowHierarchyNode is AnimationWindowHierarchyPropertyNode)
            source.AddRange((IEnumerable<AnimationWindowCurve>) AnimationWindowUtility.FilterCurves(this.state.allCurves.ToArray(), windowHierarchyNode.path, windowHierarchyNode.animatableObjectType, windowHierarchyNode.propertyName));
          else
            source.AddRange((IEnumerable<AnimationWindowCurve>) AnimationWindowUtility.FilterCurves(this.state.allCurves.ToArray(), windowHierarchyNode.path, windowHierarchyNode.animatableObjectType));
        }
      }
      return source.Distinct<AnimationWindowCurve>().ToList<AnimationWindowCurve>();
    }

    private void MaintainTreeviewStateAfterRotationInterpolation(RotationCurveInterpolation.Mode newMode)
    {
      List<int> selectedIds = this.state.hierarchyState.selectedIDs;
      List<int> expandedIds = this.state.hierarchyState.expandedIDs;
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      for (int index = 0; index < selectedIds.Count; ++index)
      {
        AnimationWindowHierarchyNode windowHierarchyNode = this.state.hierarchyData.FindItem(selectedIds[index]) as AnimationWindowHierarchyNode;
        if (windowHierarchyNode != null && !windowHierarchyNode.propertyName.Equals(RotationCurveInterpolation.GetPrefixForInterpolation(newMode)))
        {
          string oldValue = windowHierarchyNode.propertyName.Split('.')[0];
          string str = windowHierarchyNode.propertyName.Replace(oldValue, RotationCurveInterpolation.GetPrefixForInterpolation(newMode));
          intList1.Add(selectedIds[index]);
          intList2.Add((windowHierarchyNode.path + windowHierarchyNode.animatableObjectType.Name + str).GetHashCode());
        }
      }
      for (int index1 = 0; index1 < intList1.Count; ++index1)
      {
        if (selectedIds.Contains(intList1[index1]))
        {
          int index2 = selectedIds.IndexOf(intList1[index1]);
          selectedIds[index2] = intList2[index1];
        }
        if (expandedIds.Contains(intList1[index1]))
        {
          int index2 = expandedIds.IndexOf(intList1[index1]);
          expandedIds[index2] = intList2[index1];
        }
        if (this.state.hierarchyState.lastClickedID == intList1[index1])
          this.state.hierarchyState.lastClickedID = intList2[index1];
      }
      this.state.hierarchyState.selectedIDs = new List<int>((IEnumerable<int>) selectedIds);
      this.state.hierarchyState.expandedIDs = new List<int>((IEnumerable<int>) expandedIds);
    }

    private RotationCurveInterpolation.Mode GetRotationInterpolationMode(EditorCurveBinding[] curves)
    {
      if (curves == null || curves.Length == 0)
        return RotationCurveInterpolation.Mode.Undefined;
      RotationCurveInterpolation.Mode modeFromCurveData1 = RotationCurveInterpolation.GetModeFromCurveData(curves[0]);
      for (int index = 1; index < curves.Length; ++index)
      {
        RotationCurveInterpolation.Mode modeFromCurveData2 = RotationCurveInterpolation.GetModeFromCurveData(curves[index]);
        if (modeFromCurveData1 != modeFromCurveData2)
          return RotationCurveInterpolation.Mode.Undefined;
      }
      return modeFromCurveData1;
    }

    private void SetStyleTextColor(GUIStyle style, Color color)
    {
      style.normal.textColor = color;
      style.focused.textColor = color;
      style.active.textColor = color;
      style.hover.textColor = color;
    }

    public override void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible)
    {
      firstRowVisible = 0;
      lastRowVisible = this.m_TreeView.data.rowCount - 1;
    }

    public float GetNodeHeight(AnimationWindowHierarchyNode node)
    {
      if (node is AnimationWindowHierarchyAddButtonNode)
        return 40f;
      return (this.m_TreeView.state as AnimationWindowHierarchyState).GetTallMode(node) ? 32f : 16f;
    }

    public override Vector2 GetTotalSize()
    {
      List<TreeViewItem> rows = this.m_TreeView.data.GetRows();
      float y = 0.0f;
      for (int index = 0; index < rows.Count; ++index)
      {
        AnimationWindowHierarchyNode node = rows[index] as AnimationWindowHierarchyNode;
        y += this.GetNodeHeight(node);
      }
      return new Vector2(1f, y);
    }

    private float GetTopPixelOfRow(int row, List<TreeViewItem> rows)
    {
      float num = 0.0f;
      for (int index = 0; index < row && index < rows.Count; ++index)
      {
        AnimationWindowHierarchyNode row1 = rows[index] as AnimationWindowHierarchyNode;
        num += this.GetNodeHeight(row1);
      }
      return num;
    }

    public override Rect GetRowRect(int row, float rowWidth)
    {
      List<TreeViewItem> rows = this.m_TreeView.data.GetRows();
      AnimationWindowHierarchyNode node = rows[row] as AnimationWindowHierarchyNode;
      if (!node.topPixel.HasValue)
        node.topPixel = new float?(this.GetTopPixelOfRow(row, rows));
      float nodeHeight = this.GetNodeHeight(node);
      return new Rect(0.0f, node.topPixel.Value, rowWidth, nodeHeight);
    }

    public override void OnRowGUI(Rect rowRect, TreeViewItem node, int row, bool selected, bool focused)
    {
      AnimationWindowHierarchyNode node1 = node as AnimationWindowHierarchyNode;
      this.DoNodeGUI(rowRect, node1, selected, focused, row);
    }

    public override bool BeginRename(TreeViewItem item, float delay)
    {
      this.m_RenamedNode = item as AnimationWindowHierarchyNode;
      return this.GetRenameOverlay().BeginRename(this.GetGameObjectName(this.m_RenamedNode.path), item.id, delay);
    }

    protected override void SyncFakeItem()
    {
    }

    protected override void RenameEnded()
    {
      string name = this.GetRenameOverlay().name;
      string originalName = this.GetRenameOverlay().originalName;
      if (name != originalName)
      {
        foreach (AnimationWindowCurve curve in this.m_RenamedNode.curves)
        {
          string newPath = this.RenamePath(curve.path, name);
          EditorCurveBinding renamedBinding = AnimationWindowUtility.GetRenamedBinding(curve.binding, newPath);
          if (AnimationWindowUtility.CurveExists(renamedBinding, this.state.allCurves.ToArray()))
            Debug.LogWarning((object) "Curve already exists, renaming cancelled.");
          else
            AnimationWindowUtility.RenameCurvePath(curve, renamedBinding, this.state.activeAnimationClip);
        }
      }
      this.m_RenamedNode = (AnimationWindowHierarchyNode) null;
    }

    private string RenamePath(string oldPath, string newGameObjectName)
    {
      if (oldPath.Length <= 0)
        return newGameObjectName;
      string childmostGameObject = this.GetPathWithoutChildmostGameObject(oldPath);
      if (childmostGameObject.Length > 0)
        childmostGameObject += "/";
      return childmostGameObject + newGameObjectName;
    }

    protected override Texture GetIconForNode(TreeViewItem item)
    {
      if (item != null)
        return (Texture) item.icon;
      return (Texture) null;
    }
  }
}

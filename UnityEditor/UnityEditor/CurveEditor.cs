// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class CurveEditor : TimeArea, CurveUpdater
  {
    private static int s_SelectKeyHash = "SelectKeys".GetHashCode();
    private static int s_TangentControlIDHash = "s_TangentControlIDHash".GetHashCode();
    private List<int> m_DrawOrder = new List<int>();
    internal Bounds m_DefaultBounds = new Bounds(new Vector3(0.5f, 0.5f, 0.0f), new Vector3(1f, 1f, 0.0f));
    private Color m_TangentColor = new Color(1f, 1f, 1f, 0.5f);
    private List<CurveSelection> m_Selection = new List<CurveSelection>();
    private Bounds m_Bounds = new Bounds(Vector3.zero, Vector3.zero);
    private string m_AxisLabelFormat = "n1";
    private const float kMaxPickDistSqr = 100f;
    private const float kExactPickDistSqr = 16f;
    private const float kCurveTimeEpsilon = 1E-05f;
    private const string kPointValueFieldName = "pointValueField";
    private const string kPointTimeFieldName = "pointTimeField";
    private CurveWrapper[] m_AnimationCurves;
    public CurveEditor.CallbackFunction curvesUpdated;
    internal IPlayHead m_PlayHead;
    public float invSnap;
    private CurveMenuManager m_MenuManager;
    private CurveSelection lastSelected;
    private List<CurveSelection> preCurveDragSelection;
    private CurveSelection m_SelectedTangentPoint;
    private List<CurveSelection> s_SelectionBackup;
    private float s_TimeRangeSelectionStart;
    private float s_TimeRangeSelectionEnd;
    private bool s_TimeRangeSelectionActive;
    private List<CurveEditor.SavedCurve> m_CurveBackups;
    private CurveWrapper m_DraggingKey;
    private Vector2 m_DraggedCoord;
    private Vector2 m_MoveCoord;
    private Vector2 m_PreviousDrawPointCenter;
    private CurveEditor.AxisLock m_AxisLock;
    internal CurveEditor.Styles ms_Styles;
    private float s_StartClickedTime;
    private Vector2 s_StartMouseDragPosition;
    private Vector2 s_EndMouseDragPosition;
    private Vector2 s_StartKeyDragPosition;
    private CurveEditor.PickMode s_PickMode;
    private Vector2 pointEditingFieldPosition;
    private bool timeWasEdited;
    private bool valueWasEdited;
    private string focusedPointField;
    private CurveWrapper[] m_DraggingCurveOrRegion;

    public CurveWrapper[] animationCurves
    {
      get
      {
        if (this.m_AnimationCurves == null)
          this.m_AnimationCurves = new CurveWrapper[0];
        return this.m_AnimationCurves;
      }
      set
      {
        if (this.m_AnimationCurves == null)
          this.m_AnimationCurves = new CurveWrapper[0];
        this.m_AnimationCurves = value;
        for (int index = 0; index < this.m_AnimationCurves.Length; ++index)
          this.m_AnimationCurves[index].listIndex = index;
        this.SyncDrawOrder();
        this.SyncSelection();
        this.ValidateCurveList();
      }
    }

    public float activeTime
    {
      set
      {
        if (this.m_PlayHead == null || this.m_PlayHead.playing)
          return;
        this.m_PlayHead.currentTime = value;
      }
    }

    public Color tangentColor
    {
      get
      {
        return this.m_TangentColor;
      }
      set
      {
        this.m_TangentColor = value;
      }
    }

    internal List<CurveSelection> selectedCurves
    {
      get
      {
        return this.m_Selection;
      }
      set
      {
        this.m_Selection = value;
        this.lastSelected = (CurveSelection) null;
      }
    }

    public bool hasSelection
    {
      get
      {
        return this.m_Selection.Count != 0;
      }
    }

    public override Bounds drawingBounds
    {
      get
      {
        return this.m_Bounds;
      }
    }

    private bool editingPoints { get; set; }

    public CurveEditor(Rect rect, CurveWrapper[] curves, bool minimalGUI)
      : base(minimalGUI)
    {
      this.rect = rect;
      this.animationCurves = curves;
      float[] tickModulos = new float[29]
      {
        1E-07f,
        5E-07f,
        1E-06f,
        5E-06f,
        1E-05f,
        5E-05f,
        0.0001f,
        0.0005f,
        1f / 1000f,
        0.005f,
        0.01f,
        0.05f,
        0.1f,
        0.5f,
        1f,
        5f,
        10f,
        50f,
        100f,
        500f,
        1000f,
        5000f,
        10000f,
        50000f,
        100000f,
        500000f,
        1000000f,
        5000000f,
        1E+07f
      };
      this.hTicks = new TickHandler();
      this.hTicks.SetTickModulos(tickModulos);
      this.vTicks = new TickHandler();
      this.vTicks.SetTickModulos(tickModulos);
      this.margin = 40f;
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    public bool GetTopMostCurveID(out int curveID)
    {
      if (this.m_DrawOrder.Count > 0)
      {
        curveID = this.m_DrawOrder[this.m_DrawOrder.Count - 1];
        return true;
      }
      curveID = -1;
      return false;
    }

    private void SyncDrawOrder()
    {
      if (this.m_DrawOrder.Count == 0)
      {
        this.m_DrawOrder = ((IEnumerable<CurveWrapper>) this.m_AnimationCurves).Select<CurveWrapper, int>((Func<CurveWrapper, int>) (cw => cw.id)).ToList<int>();
      }
      else
      {
        List<int> intList = new List<int>(this.m_AnimationCurves.Length);
        for (int index1 = 0; index1 < this.m_DrawOrder.Count; ++index1)
        {
          int num = this.m_DrawOrder[index1];
          for (int index2 = 0; index2 < this.m_AnimationCurves.Length; ++index2)
          {
            if (this.m_AnimationCurves[index2].id == num)
            {
              intList.Add(num);
              break;
            }
          }
        }
        this.m_DrawOrder = intList;
        if (this.m_DrawOrder.Count == this.m_AnimationCurves.Length)
          return;
        for (int index1 = 0; index1 < this.m_AnimationCurves.Length; ++index1)
        {
          int id = this.m_AnimationCurves[index1].id;
          bool flag = false;
          for (int index2 = 0; index2 < this.m_DrawOrder.Count; ++index2)
          {
            if (this.m_DrawOrder[index2] == id)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            this.m_DrawOrder.Add(id);
        }
        if (this.m_DrawOrder.Count == this.m_AnimationCurves.Length)
          return;
        this.m_DrawOrder = ((IEnumerable<CurveWrapper>) this.m_AnimationCurves).Select<CurveWrapper, int>((Func<CurveWrapper, int>) (cw => cw.id)).ToList<int>();
      }
    }

    public CurveWrapper getCurveWrapperById(int id)
    {
      foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
      {
        if (animationCurve.id == id)
          return animationCurve;
      }
      return (CurveWrapper) null;
    }

    protected override void ApplySettings()
    {
      base.ApplySettings();
      this.RecalculateBounds();
    }

    internal void AddSelection(CurveSelection curveSelection)
    {
      this.m_Selection.Add(curveSelection);
      this.lastSelected = curveSelection;
    }

    internal void RemoveSelection(CurveSelection curveSelection)
    {
      this.m_Selection.Remove(curveSelection);
      this.lastSelected = (CurveSelection) null;
    }

    internal void ClearSelection()
    {
      this.m_Selection.Clear();
      this.lastSelected = (CurveSelection) null;
    }

    public void OnDisable()
    {
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    private void UndoRedoPerformed()
    {
      this.SelectNone();
    }

    private void ValidateCurveList()
    {
      for (int index = 0; index < this.m_AnimationCurves.Length; ++index)
      {
        int regionId1 = this.m_AnimationCurves[index].regionId;
        if (regionId1 >= 0)
        {
          if (index == this.m_AnimationCurves.Length - 1)
          {
            Debug.LogError((object) "Region has only one curve last! Regions should be added as two curves after each other with same regionId");
            return;
          }
          int regionId2 = this.m_AnimationCurves[++index].regionId;
          if (regionId1 != regionId2)
          {
            Debug.LogError((object) ("Regions should be added as two curves after each other with same regionId: " + (object) regionId1 + " != " + (object) regionId2));
            return;
          }
        }
      }
      if (this.m_DrawOrder.Count != this.m_AnimationCurves.Length)
      {
        Debug.LogError((object) ("DrawOrder and AnimationCurves mismatch: DrawOrder " + (object) this.m_DrawOrder.Count + ", AnimationCurves: " + (object) this.m_AnimationCurves.Length));
      }
      else
      {
        int count = this.m_DrawOrder.Count;
        for (int index = 0; index < count; ++index)
        {
          int regionId1 = this.getCurveWrapperById(this.m_DrawOrder[index]).regionId;
          if (regionId1 >= 0)
          {
            if (index == count - 1)
            {
              Debug.LogError((object) "Region has only one curve last! Regions should be added as two curves after each other with same regionId");
              break;
            }
            int regionId2 = this.getCurveWrapperById(this.m_DrawOrder[++index]).regionId;
            if (regionId1 != regionId2)
            {
              Debug.LogError((object) ("DrawOrder: Regions not added correctly after each other. RegionIds: " + (object) regionId1 + " , " + (object) regionId2));
              break;
            }
          }
        }
      }
    }

    private void UpdateTangentsFromSelection()
    {
      using (List<CurveSelection>.Enumerator enumerator = this.selectedCurves.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          CurveSelection current = enumerator.Current;
          CurveUtility.UpdateTangentsFromModeSurrounding(current.curveWrapper.curve, current.key);
        }
      }
    }

    private void SyncSelection()
    {
      this.Init();
      List<CurveSelection> curveSelectionList = new List<CurveSelection>(this.m_Selection.Count);
      using (List<CurveSelection>.Enumerator enumerator = this.m_Selection.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          CurveSelection current = enumerator.Current;
          CurveWrapper curveWrapper = current.curveWrapper;
          if (curveWrapper != null && (!curveWrapper.hidden || curveWrapper.groupId != -1))
          {
            curveWrapper.selected = CurveWrapper.SelectionMode.Selected;
            curveSelectionList.Add(current);
          }
        }
      }
      this.m_Selection = curveSelectionList;
      this.RecalculateBounds();
    }

    public void RecalculateBounds()
    {
      this.m_Bounds = this.m_DefaultBounds;
      if (this.animationCurves != null && ((double) this.hRangeMin == double.NegativeInfinity || (double) this.hRangeMax == double.PositiveInfinity || ((double) this.vRangeMin == double.NegativeInfinity || (double) this.vRangeMax == double.PositiveInfinity)))
      {
        bool flag = false;
        foreach (CurveWrapper animationCurve in this.animationCurves)
        {
          if (!animationCurve.hidden && animationCurve.curve.length != 0)
          {
            if (!flag)
            {
              this.m_Bounds = animationCurve.renderer.GetBounds();
              flag = true;
            }
            else
              this.m_Bounds.Encapsulate(animationCurve.renderer.GetBounds());
          }
        }
      }
      if ((double) this.hRangeMin != double.NegativeInfinity)
        this.m_Bounds.min = new Vector3(this.hRangeMin, this.m_Bounds.min.y, this.m_Bounds.min.z);
      if ((double) this.hRangeMax != double.PositiveInfinity)
        this.m_Bounds.max = new Vector3(this.hRangeMax, this.m_Bounds.max.y, this.m_Bounds.max.z);
      if ((double) this.vRangeMin != double.NegativeInfinity)
        this.m_Bounds.min = new Vector3(this.m_Bounds.min.x, this.vRangeMin, this.m_Bounds.min.z);
      if ((double) this.vRangeMax != double.PositiveInfinity)
        this.m_Bounds.max = new Vector3(this.m_Bounds.max.y, this.vRangeMax, this.m_Bounds.max.z);
      this.m_Bounds.size = new Vector3(Mathf.Max(this.m_Bounds.size.x, 0.1f), Mathf.Max(this.m_Bounds.size.y, 0.1f), 0.0f);
    }

    public void FrameSelected(bool horizontally, bool vertically)
    {
      Bounds bounds = new Bounds();
      if (!this.hasSelection)
      {
        bounds = this.drawingBounds;
        if (bounds.size == Vector3.zero)
          return;
      }
      else
      {
        bounds = new Bounds((Vector3) new Vector2(this.selectedCurves[0].keyframe.time, this.selectedCurves[0].keyframe.value), (Vector3) Vector2.zero);
        using (List<CurveSelection>.Enumerator enumerator = this.selectedCurves.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            CurveSelection current = enumerator.Current;
            bounds.Encapsulate((Vector3) new Vector2(current.curve[current.key].time, current.curve[current.key].value));
            if (current.key - 1 >= 0)
              bounds.Encapsulate((Vector3) new Vector2(current.curve[current.key - 1].time, current.curve[current.key - 1].value));
            if (current.key + 1 < current.curve.length)
              bounds.Encapsulate((Vector3) new Vector2(current.curve[current.key + 1].time, current.curve[current.key + 1].value));
          }
        }
        bounds.size = new Vector3(Mathf.Max(bounds.size.x, 0.1f), Mathf.Max(bounds.size.y, 0.1f), 0.0f);
      }
      if (horizontally)
        this.SetShownHRangeInsideMargins(bounds.min.x, bounds.max.x);
      if (!vertically)
        return;
      this.SetShownVRangeInsideMargins(bounds.min.y, bounds.max.y);
    }

    public void UpdateCurves(List<int> curveIds, string undoText)
    {
      using (List<int>.Enumerator enumerator = curveIds.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.GetCurveFromID(enumerator.Current).changed = true;
      }
      if (this.curvesUpdated == null)
        return;
      this.curvesUpdated();
    }

    public void UpdateCurves(List<ChangedCurve> curve, string undoText)
    {
    }

    internal CurveWrapper GetCurveFromID(int curveID)
    {
      if (this.m_AnimationCurves == null)
        return (CurveWrapper) null;
      foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
      {
        if (animationCurve.id == curveID)
          return animationCurve;
      }
      return (CurveWrapper) null;
    }

    private void Init()
    {
      if (this.selectedCurves == null || !this.hasSelection || this.selectedCurves[0].m_Host != null)
        return;
      using (List<CurveSelection>.Enumerator enumerator = this.selectedCurves.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.m_Host = this;
      }
    }

    internal void InitStyles()
    {
      if (this.ms_Styles != null)
        return;
      this.ms_Styles = new CurveEditor.Styles();
    }

    public void OnGUI()
    {
      this.BeginViewGUI();
      this.GridGUI();
      this.CurveGUI();
      this.EndViewGUI();
    }

    public void CurveGUI()
    {
      this.InitStyles();
      GUI.BeginGroup(this.drawRect);
      this.Init();
      GUIUtility.GetControlID(CurveEditor.s_SelectKeyHash, FocusType.Passive);
      Color white = Color.white;
      GUI.backgroundColor = white;
      GUI.contentColor = white;
      Color color = GUI.color;
      Event current1 = Event.current;
      if (current1.type != EventType.Repaint)
        this.EditSelectedPoints();
      EventType type = current1.type;
      switch (type)
      {
        case EventType.KeyDown:
          if ((current1.keyCode == KeyCode.Backspace || current1.keyCode == KeyCode.Delete) && this.hasSelection)
          {
            this.DeleteSelectedPoints();
            current1.Use();
            break;
          }
          break;
        case EventType.Repaint:
          this.DrawCurves(this.animationCurves);
          break;
        default:
          switch (type - 13)
          {
            case EventType.MouseDown:
            case EventType.MouseUp:
              bool flag1 = current1.type == EventType.ExecuteCommand;
              string commandName = current1.commandName;
              if (commandName != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (CurveEditor.\u003C\u003Ef__switch\u0024mapC == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  CurveEditor.\u003C\u003Ef__switch\u0024mapC = new Dictionary<string, int>(3)
                  {
                    {
                      "Delete",
                      0
                    },
                    {
                      "FrameSelected",
                      1
                    },
                    {
                      "SelectAll",
                      2
                    }
                  };
                }
                int num;
                // ISSUE: reference to a compiler-generated field
                if (CurveEditor.\u003C\u003Ef__switch\u0024mapC.TryGetValue(commandName, out num))
                {
                  switch (num)
                  {
                    case 0:
                      if (this.hasSelection)
                      {
                        if (flag1)
                          this.DeleteSelectedPoints();
                        current1.Use();
                        break;
                      }
                      break;
                    case 1:
                      if (flag1)
                        this.FrameSelected(true, true);
                      current1.Use();
                      break;
                    case 2:
                      if (flag1)
                        this.SelectAll();
                      current1.Use();
                      break;
                  }
                }
                else
                  break;
              }
              else
                break;
            case EventType.MouseDrag:
              CurveSelection nearest = this.FindNearest();
              if (nearest != null)
              {
                List<KeyIdentifier> keyList = new List<KeyIdentifier>();
                bool flag2 = false;
                using (List<CurveSelection>.Enumerator enumerator = this.selectedCurves.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    CurveSelection current2 = enumerator.Current;
                    keyList.Add(new KeyIdentifier(current2.curveWrapper.renderer, current2.curveID, current2.key));
                    if (current2.curveID == nearest.curveID && current2.key == nearest.key)
                      flag2 = true;
                  }
                }
                if (!flag2)
                {
                  keyList.Clear();
                  keyList.Add(new KeyIdentifier(nearest.curveWrapper.renderer, nearest.curveID, nearest.key));
                  this.ClearSelection();
                  this.AddSelection(nearest);
                }
                this.m_MenuManager = new CurveMenuManager((CurveUpdater) this);
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent(keyList.Count <= 1 ? "Delete Key" : "Delete Keys"), false, new GenericMenu.MenuFunction2(this.DeleteKeys), (object) keyList);
                menu.AddItem(new GUIContent(keyList.Count <= 1 ? "Edit Key..." : "Edit Keys..."), false, new GenericMenu.MenuFunction2(this.StartEditingSelectedPointsContext), (object) this.mousePositionInDrawing);
                menu.AddSeparator(string.Empty);
                this.m_MenuManager.AddTangentMenuItems(menu, keyList);
                menu.ShowAsContext();
                Event.current.Use();
                break;
              }
              break;
          }
      }
      if (current1.type == EventType.Repaint)
        this.EditSelectedPoints();
      EditorGUI.BeginChangeCheck();
      GUI.color = color;
      this.DragTangents();
      this.EditAxisLabels();
      this.SelectPoints();
      if (EditorGUI.EndChangeCheck())
      {
        this.RecalcSecondarySelection();
        this.RecalcCurveSelection();
      }
      EditorGUI.BeginChangeCheck();
      Vector2 vector2 = this.MovePoints();
      if (EditorGUI.EndChangeCheck() && this.m_DraggingKey != null)
      {
        this.activeTime = vector2.x + this.s_StartClickedTime;
        this.m_MoveCoord = vector2;
      }
      GUI.color = color;
      GUI.EndGroup();
    }

    private void RecalcCurveSelection()
    {
      foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
        animationCurve.selected = CurveWrapper.SelectionMode.None;
      using (List<CurveSelection>.Enumerator enumerator = this.selectedCurves.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          CurveSelection current = enumerator.Current;
          current.curveWrapper.selected = !current.semiSelected ? CurveWrapper.SelectionMode.Selected : CurveWrapper.SelectionMode.SemiSelected;
        }
      }
    }

    private void RecalcSecondarySelection()
    {
      List<CurveSelection> curveSelectionList = new List<CurveSelection>(this.m_Selection.Count);
      using (List<CurveSelection>.Enumerator enumerator = this.m_Selection.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          CurveSelection current = enumerator.Current;
          CurveWrapper curveWrapper = current.curveWrapper;
          int groupId = current.curveWrapper.groupId;
          if (groupId != -1 && !current.semiSelected)
          {
            curveSelectionList.Add(current);
            foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
            {
              if (animationCurve.groupId == groupId && animationCurve != curveWrapper)
                curveSelectionList.Add(new CurveSelection(animationCurve.id, this, current.key)
                {
                  semiSelected = true
                });
            }
          }
          else
            curveSelectionList.Add(current);
        }
      }
      curveSelectionList.Sort();
      int index = 0;
      while (index < curveSelectionList.Count - 1)
      {
        CurveSelection curveSelection1 = curveSelectionList[index];
        CurveSelection curveSelection2 = curveSelectionList[index + 1];
        if (curveSelection1.curveID == curveSelection2.curveID && curveSelection1.key == curveSelection2.key)
        {
          if (!curveSelection1.semiSelected || !curveSelection2.semiSelected)
            curveSelection1.semiSelected = false;
          curveSelectionList.RemoveAt(index + 1);
        }
        else
          ++index;
      }
      this.m_Selection = curveSelectionList;
    }

    private void DragTangents()
    {
      Event current1 = Event.current;
      int controlId = GUIUtility.GetControlID(CurveEditor.s_TangentControlIDHash, FocusType.Passive);
      switch (current1.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (current1.button != 0 || current1.alt)
            break;
          this.m_SelectedTangentPoint = (CurveSelection) null;
          float num = 100f;
          Vector2 mousePosition = Event.current.mousePosition;
          using (List<CurveSelection>.Enumerator enumerator = this.selectedCurves.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              CurveSelection current2 = enumerator.Current;
              Keyframe keyframe = current2.keyframe;
              if (CurveUtility.GetKeyTangentMode(keyframe, 0) == TangentMode.Editable)
              {
                CurveSelection selection = new CurveSelection(current2.curveID, this, current2.key, CurveSelection.SelectionType.InTangent);
                float sqrMagnitude = (this.DrawingToViewTransformPoint(this.GetPosition(selection)) - mousePosition).sqrMagnitude;
                if ((double) sqrMagnitude <= (double) num)
                {
                  this.m_SelectedTangentPoint = selection;
                  num = sqrMagnitude;
                }
              }
              if (CurveUtility.GetKeyTangentMode(keyframe, 1) == TangentMode.Editable)
              {
                CurveSelection selection = new CurveSelection(current2.curveID, this, current2.key, CurveSelection.SelectionType.OutTangent);
                float sqrMagnitude = (this.DrawingToViewTransformPoint(this.GetPosition(selection)) - mousePosition).sqrMagnitude;
                if ((double) sqrMagnitude <= (double) num)
                {
                  this.m_SelectedTangentPoint = selection;
                  num = sqrMagnitude;
                }
              }
            }
          }
          if (this.m_SelectedTangentPoint == null)
            break;
          GUIUtility.hotControl = controlId;
          current1.Use();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          GUIUtility.hotControl = 0;
          current1.Use();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != controlId)
            break;
          Vector2 positionInDrawing = this.mousePositionInDrawing;
          CurveSelection selectedTangentPoint = this.m_SelectedTangentPoint;
          Keyframe keyframe1 = selectedTangentPoint.keyframe;
          if (selectedTangentPoint.type == CurveSelection.SelectionType.InTangent)
          {
            Vector2 vector2 = positionInDrawing - new Vector2(keyframe1.time, keyframe1.value);
            keyframe1.inTangent = (double) vector2.x >= -9.99999974737875E-05 ? float.PositiveInfinity : vector2.y / vector2.x;
            CurveUtility.SetKeyTangentMode(ref keyframe1, 0, TangentMode.Editable);
            if (!CurveUtility.GetKeyBroken(keyframe1))
            {
              keyframe1.outTangent = keyframe1.inTangent;
              CurveUtility.SetKeyTangentMode(ref keyframe1, 1, TangentMode.Editable);
            }
          }
          else if (selectedTangentPoint.type == CurveSelection.SelectionType.OutTangent)
          {
            Vector2 vector2 = positionInDrawing - new Vector2(keyframe1.time, keyframe1.value);
            keyframe1.outTangent = (double) vector2.x <= 9.99999974737875E-05 ? float.PositiveInfinity : vector2.y / vector2.x;
            CurveUtility.SetKeyTangentMode(ref keyframe1, 1, TangentMode.Editable);
            if (!CurveUtility.GetKeyBroken(keyframe1))
            {
              keyframe1.inTangent = keyframe1.outTangent;
              CurveUtility.SetKeyTangentMode(ref keyframe1, 0, TangentMode.Editable);
            }
          }
          selectedTangentPoint.key = selectedTangentPoint.curve.MoveKey(selectedTangentPoint.key, keyframe1);
          CurveUtility.UpdateTangentsFromModeSurrounding(selectedTangentPoint.curveWrapper.curve, selectedTangentPoint.key);
          selectedTangentPoint.curveWrapper.changed = true;
          GUI.changed = true;
          Event.current.Use();
          break;
      }
    }

    private void DeleteSelectedPoints()
    {
      for (int index = this.selectedCurves.Count - 1; index >= 0; --index)
      {
        CurveSelection selectedCurve = this.selectedCurves[index];
        CurveWrapper curveWrapper = selectedCurve.curveWrapper;
        if (this.settings.allowDeleteLastKeyInCurve || curveWrapper.curve.keys.Length != 1)
        {
          curveWrapper.curve.RemoveKey(selectedCurve.key);
          CurveUtility.UpdateTangentsFromMode(curveWrapper.curve);
          curveWrapper.changed = true;
          GUI.changed = true;
        }
      }
      this.SelectNone();
    }

    private void DeleteKeys(object obj)
    {
      List<KeyIdentifier> keyIdentifierList = (List<KeyIdentifier>) obj;
      List<int> curveIds = new List<int>();
      for (int index = keyIdentifierList.Count - 1; index >= 0; --index)
      {
        if (this.settings.allowDeleteLastKeyInCurve || keyIdentifierList[index].curve.keys.Length != 1)
        {
          keyIdentifierList[index].curve.RemoveKey(keyIdentifierList[index].key);
          CurveUtility.UpdateTangentsFromMode(keyIdentifierList[index].curve);
          curveIds.Add(keyIdentifierList[index].curveId);
        }
      }
      string undoText = keyIdentifierList.Count <= 1 ? "Delete Key" : "Delete Keys";
      this.UpdateCurves(curveIds, undoText);
      this.SelectNone();
    }

    private float ClampVerticalValue(float value, int curveID)
    {
      value = Mathf.Clamp(value, this.vRangeMin, this.vRangeMax);
      CurveWrapper curveFromId = this.GetCurveFromID(curveID);
      if (curveFromId != null)
        value = Mathf.Clamp(value, curveFromId.vRangeMin, curveFromId.vRangeMax);
      return value;
    }

    private void TranslateSelectedKeys(Vector2 movement)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.UpdateCurvesFromPoints(new System.Action<CurveEditor.SavedCurve.SavedKeyFrame, CurveEditor.SavedCurve>(new CurveEditor.\u003CTranslateSelectedKeys\u003Ec__AnonStorey44()
      {
        movement = movement,
        \u003C\u003Ef__this = this
      }.\u003C\u003Em__75));
    }

    private void SetSelectedKeyPositions(Vector2 newPosition, bool updateTime, bool updateValue)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.UpdateCurvesFromPoints(new System.Action<CurveEditor.SavedCurve.SavedKeyFrame, CurveEditor.SavedCurve>(new CurveEditor.\u003CSetSelectedKeyPositions\u003Ec__AnonStorey45()
      {
        updateTime = updateTime,
        newPosition = newPosition,
        updateValue = updateValue,
        \u003C\u003Ef__this = this
      }.\u003C\u003Em__76));
    }

    private void UpdateCurvesFromPoints(System.Action<CurveEditor.SavedCurve.SavedKeyFrame, CurveEditor.SavedCurve> action)
    {
      List<CurveSelection> curveSelectionList = new List<CurveSelection>();
      using (List<CurveEditor.SavedCurve>.Enumerator enumerator = this.m_CurveBackups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          CurveEditor.SavedCurve current = enumerator.Current;
          List<CurveEditor.SavedCurve.SavedKeyFrame> savedKeyFrameList = new List<CurveEditor.SavedCurve.SavedKeyFrame>(current.keys.Count);
          for (int index1 = 0; index1 != current.keys.Count; ++index1)
          {
            CurveEditor.SavedCurve.SavedKeyFrame savedKeyFrame = current.keys[index1];
            if (savedKeyFrame.selected != CurveWrapper.SelectionMode.None)
            {
              savedKeyFrame = new CurveEditor.SavedCurve.SavedKeyFrame(savedKeyFrame.key, savedKeyFrame.selected);
              action(savedKeyFrame, current);
              for (int index2 = savedKeyFrameList.Count - 1; index2 >= 0; --index2)
              {
                if ((double) Mathf.Abs(savedKeyFrameList[index2].key.time - savedKeyFrame.key.time) < 9.99999974737875E-06)
                  savedKeyFrameList.RemoveAt(index2);
              }
            }
            savedKeyFrameList.Add(new CurveEditor.SavedCurve.SavedKeyFrame(savedKeyFrame.key, savedKeyFrame.selected));
          }
          savedKeyFrameList.Sort();
          Keyframe[] keyframeArray = new Keyframe[savedKeyFrameList.Count];
          for (int keyIndex = 0; keyIndex < savedKeyFrameList.Count; ++keyIndex)
          {
            CurveEditor.SavedCurve.SavedKeyFrame savedKeyFrame = savedKeyFrameList[keyIndex];
            keyframeArray[keyIndex] = savedKeyFrame.key;
            if (savedKeyFrame.selected != CurveWrapper.SelectionMode.None)
            {
              CurveSelection curveSelection = new CurveSelection(current.curveId, this, keyIndex);
              if (savedKeyFrame.selected == CurveWrapper.SelectionMode.SemiSelected)
                curveSelection.semiSelected = true;
              curveSelectionList.Add(curveSelection);
            }
          }
          this.selectedCurves = curveSelectionList;
          CurveWrapper curveFromId = this.GetCurveFromID(current.curveId);
          curveFromId.curve.keys = keyframeArray;
          curveFromId.changed = true;
        }
      }
      this.UpdateTangentsFromSelection();
    }

    private float SnapTime(float t)
    {
      if (EditorGUI.actionKey)
      {
        float periodOfLevel = this.hTicks.GetPeriodOfLevel(this.hTicks.GetLevelWithMinSeparation(5f));
        t = Mathf.Round(t / periodOfLevel) * periodOfLevel;
      }
      else if ((double) this.invSnap != 0.0)
        t = Mathf.Round(t * this.invSnap) / this.invSnap;
      return t;
    }

    private float SnapValue(float v)
    {
      if (EditorGUI.actionKey)
      {
        float periodOfLevel = this.vTicks.GetPeriodOfLevel(this.vTicks.GetLevelWithMinSeparation(5f));
        v = Mathf.Round(v / periodOfLevel) * periodOfLevel;
      }
      return v;
    }

    private Vector2 GetGUIPoint(Vector3 point)
    {
      return HandleUtility.WorldToGUIPoint(this.DrawingToViewTransformPoint(point));
    }

    private int OnlyOneEditableCurve()
    {
      int num1 = -1;
      int num2 = 0;
      for (int index = 0; index < this.m_AnimationCurves.Length; ++index)
      {
        CurveWrapper animationCurve = this.m_AnimationCurves[index];
        if (!animationCurve.hidden && !animationCurve.readOnly)
        {
          ++num2;
          num1 = index;
        }
      }
      if (num2 == 1)
        return num1;
      return -1;
    }

    private int GetCurveAtPosition(Vector2 position, out Vector2 closestPointOnCurve)
    {
      Vector2 viewTransformPoint1 = this.DrawingToViewTransformPoint(position);
      int num1 = (int) Mathf.Sqrt(100f);
      float num2 = 100f;
      int num3 = -1;
      closestPointOnCurve = (Vector2) Vector3.zero;
      for (int index1 = this.m_DrawOrder.Count - 1; index1 >= 0; --index1)
      {
        CurveWrapper curveWrapperById = this.getCurveWrapperById(this.m_DrawOrder[index1]);
        if (!curveWrapperById.hidden && !curveWrapperById.readOnly)
        {
          Vector2 lhs;
          lhs.x = position.x - (float) num1 / this.scale.x;
          lhs.y = curveWrapperById.renderer.EvaluateCurveSlow(lhs.x);
          lhs = this.DrawingToViewTransformPoint(lhs);
          for (int index2 = -num1; index2 < num1; ++index2)
          {
            Vector2 viewTransformPoint2;
            viewTransformPoint2.x = position.x + (float) (index2 + 1) / this.scale.x;
            viewTransformPoint2.y = curveWrapperById.renderer.EvaluateCurveSlow(viewTransformPoint2.x);
            viewTransformPoint2 = this.DrawingToViewTransformPoint(viewTransformPoint2);
            float num4 = HandleUtility.DistancePointLine((Vector3) viewTransformPoint1, (Vector3) lhs, (Vector3) viewTransformPoint2);
            float num5 = num4 * num4;
            if ((double) num5 < (double) num2)
            {
              num2 = num5;
              num3 = curveWrapperById.listIndex;
              closestPointOnCurve = (Vector2) HandleUtility.ProjectPointLine((Vector3) viewTransformPoint1, (Vector3) lhs, (Vector3) viewTransformPoint2);
            }
            lhs = viewTransformPoint2;
          }
        }
      }
      closestPointOnCurve = this.ViewToDrawingTransformPoint(closestPointOnCurve);
      return num3;
    }

    private void CreateKeyFromClick(object obj)
    {
      this.UpdateCurves(this.CreateKeyFromClick((Vector2) obj), "Add Key");
    }

    private List<int> CreateKeyFromClick(Vector2 position)
    {
      List<int> intList = new List<int>();
      int curveIndex = this.OnlyOneEditableCurve();
      if (curveIndex >= 0)
      {
        float x = position.x;
        CurveWrapper animationCurve = this.m_AnimationCurves[curveIndex];
        if (animationCurve.curve.keys.Length == 0 || (double) x < (double) animationCurve.curve.keys[0].time || (double) x > (double) animationCurve.curve.keys[animationCurve.curve.keys.Length - 1].time)
        {
          this.CreateKeyFromClick(curveIndex, position);
          intList.Add(animationCurve.id);
          return intList;
        }
      }
      Vector2 closestPointOnCurve;
      int curveAtPosition = this.GetCurveAtPosition(position, out closestPointOnCurve);
      this.CreateKeyFromClick(curveAtPosition, closestPointOnCurve.x);
      if (curveAtPosition >= 0)
        intList.Add(this.m_AnimationCurves[curveAtPosition].id);
      return intList;
    }

    private void CreateKeyFromClick(int curveIndex, float time)
    {
      time = Mathf.Clamp(time, this.settings.hRangeMin, this.settings.hRangeMax);
      if (curveIndex < 0)
        return;
      CurveSelection curveSelection1 = (CurveSelection) null;
      CurveWrapper animationCurve1 = this.m_AnimationCurves[curveIndex];
      if (animationCurve1.groupId == -1)
      {
        curveSelection1 = this.AddKeyAtTime(animationCurve1, time);
      }
      else
      {
        foreach (CurveWrapper animationCurve2 in this.m_AnimationCurves)
        {
          if (animationCurve2.groupId == animationCurve1.groupId)
          {
            CurveSelection curveSelection2 = this.AddKeyAtTime(animationCurve2, time);
            if (animationCurve2.id == animationCurve1.id)
              curveSelection1 = curveSelection2;
          }
        }
      }
      if (curveSelection1 != null)
      {
        this.ClearSelection();
        this.AddSelection(curveSelection1);
        this.RecalcSecondarySelection();
      }
      else
        this.SelectNone();
    }

    private void CreateKeyFromClick(int curveIndex, Vector2 position)
    {
      position.x = Mathf.Clamp(position.x, this.settings.hRangeMin, this.settings.hRangeMax);
      if (curveIndex < 0)
        return;
      CurveSelection curveSelection = (CurveSelection) null;
      CurveWrapper animationCurve1 = this.m_AnimationCurves[curveIndex];
      if (animationCurve1.groupId == -1)
      {
        curveSelection = this.AddKeyAtPosition(animationCurve1, position);
      }
      else
      {
        foreach (CurveWrapper animationCurve2 in this.m_AnimationCurves)
        {
          if (animationCurve2.groupId == animationCurve1.groupId)
          {
            if (animationCurve2.id == animationCurve1.id)
              curveSelection = this.AddKeyAtPosition(animationCurve2, position);
            else
              this.AddKeyAtTime(animationCurve2, position.x);
          }
        }
      }
      if (curveSelection != null)
      {
        this.ClearSelection();
        this.AddSelection(curveSelection);
        this.RecalcSecondarySelection();
      }
      else
        this.SelectNone();
    }

    private CurveSelection AddKeyAtTime(CurveWrapper cw, float time)
    {
      time = this.SnapTime(time);
      float num1 = (double) this.invSnap == 0.0 ? 0.0001f : 0.5f / this.invSnap;
      if (CurveUtility.HaveKeysInRange(cw.curve, time - num1, time + num1))
        return (CurveSelection) null;
      float curveDeltaSlow = cw.renderer.EvaluateCurveDeltaSlow(time);
      float num2 = this.ClampVerticalValue(this.SnapValue(cw.renderer.EvaluateCurveSlow(time)), cw.id);
      return this.AddKeyframeAndSelect(new Keyframe(time, num2, curveDeltaSlow, curveDeltaSlow), cw);
    }

    private CurveSelection AddKeyAtPosition(CurveWrapper cw, Vector2 position)
    {
      position.x = this.SnapTime(position.x);
      float num1 = (double) this.invSnap == 0.0 ? 0.0001f : 0.5f / this.invSnap;
      if (CurveUtility.HaveKeysInRange(cw.curve, position.x - num1, position.x + num1))
        return (CurveSelection) null;
      float num2 = 0.0f;
      return this.AddKeyframeAndSelect(new Keyframe(position.x, this.SnapValue(position.y), num2, num2), cw);
    }

    private CurveSelection AddKeyframeAndSelect(Keyframe key, CurveWrapper cw)
    {
      int num = cw.curve.AddKey(key);
      CurveUtility.SetKeyModeFromContext(cw.curve, num);
      CurveUtility.UpdateTangentsFromModeSurrounding(cw.curve, num);
      CurveSelection curveSelection = new CurveSelection(cw.id, this, num);
      cw.selected = CurveWrapper.SelectionMode.Selected;
      cw.changed = true;
      this.activeTime = key.time;
      return curveSelection;
    }

    private CurveSelection FindNearest()
    {
      Vector2 mousePosition = Event.current.mousePosition;
      bool flag = false;
      int curveID = -1;
      int keyIndex1 = -1;
      float num = 100f;
      for (int index = this.m_DrawOrder.Count - 1; index >= 0; --index)
      {
        CurveWrapper curveWrapperById = this.getCurveWrapperById(this.m_DrawOrder[index]);
        if (!curveWrapperById.readOnly && !curveWrapperById.hidden)
        {
          for (int keyIndex2 = 0; keyIndex2 < curveWrapperById.curve.keys.Length; ++keyIndex2)
          {
            Keyframe key = curveWrapperById.curve.keys[keyIndex2];
            float sqrMagnitude = (this.GetGUIPoint((Vector3) new Vector2(key.time, key.value)) - mousePosition).sqrMagnitude;
            if ((double) sqrMagnitude <= 16.0)
              return new CurveSelection(curveWrapperById.id, this, keyIndex2);
            if ((double) sqrMagnitude < (double) num)
            {
              flag = true;
              curveID = curveWrapperById.id;
              keyIndex1 = keyIndex2;
              num = sqrMagnitude;
            }
          }
          if (index == this.m_DrawOrder.Count - 1 && curveID >= 0)
            num = 16f;
        }
      }
      if (flag)
        return new CurveSelection(curveID, this, keyIndex1);
      return (CurveSelection) null;
    }

    public void SelectNone()
    {
      this.ClearSelection();
      foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
        animationCurve.selected = CurveWrapper.SelectionMode.None;
    }

    public void SelectAll()
    {
      int capacity = 0;
      foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
      {
        if (!animationCurve.hidden)
          capacity += animationCurve.curve.length;
      }
      List<CurveSelection> curveSelectionList = new List<CurveSelection>(capacity);
      foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
      {
        animationCurve.selected = CurveWrapper.SelectionMode.Selected;
        for (int keyIndex = 0; keyIndex < animationCurve.curve.length; ++keyIndex)
          curveSelectionList.Add(new CurveSelection(animationCurve.id, this, keyIndex));
      }
      this.selectedCurves = curveSelectionList;
    }

    public bool IsDraggingCurveOrRegion()
    {
      return this.m_DraggingCurveOrRegion != null;
    }

    public bool IsDraggingCurve(CurveWrapper cw)
    {
      if (this.m_DraggingCurveOrRegion != null && this.m_DraggingCurveOrRegion.Length == 1)
        return this.m_DraggingCurveOrRegion[0] == cw;
      return false;
    }

    public bool IsDraggingRegion(CurveWrapper cw1, CurveWrapper cw2)
    {
      if (this.m_DraggingCurveOrRegion == null || this.m_DraggingCurveOrRegion.Length != 2)
        return false;
      if (this.m_DraggingCurveOrRegion[0] != cw1)
        return this.m_DraggingCurveOrRegion[0] == cw2;
      return true;
    }

    private bool HandleCurveAndRegionMoveToFrontOnMouseDown(ref Vector2 timeValue, ref CurveWrapper[] curves)
    {
      Vector2 closestPointOnCurve;
      int curveAtPosition = this.GetCurveAtPosition(this.mousePositionInDrawing, out closestPointOnCurve);
      if (curveAtPosition >= 0)
      {
        this.MoveCurveToFront(this.m_AnimationCurves[curveAtPosition].id);
        timeValue = this.mousePositionInDrawing;
        curves = new CurveWrapper[1]
        {
          this.m_AnimationCurves[curveAtPosition]
        };
        return true;
      }
      for (int index = this.m_DrawOrder.Count - 1; index >= 0; --index)
      {
        CurveWrapper curveWrapperById = this.getCurveWrapperById(this.m_DrawOrder[index]);
        if (curveWrapperById != null && !curveWrapperById.hidden && curveWrapperById.curve.length != 0)
        {
          CurveWrapper cw2 = (CurveWrapper) null;
          if (index > 0)
            cw2 = this.getCurveWrapperById(this.m_DrawOrder[index - 1]);
          if (this.IsRegion(curveWrapperById, cw2))
          {
            float y = this.mousePositionInDrawing.y;
            float x = this.mousePositionInDrawing.x;
            float num1 = curveWrapperById.renderer.EvaluateCurveSlow(x);
            float num2 = cw2.renderer.EvaluateCurveSlow(x);
            if ((double) num1 > (double) num2)
            {
              float num3 = num1;
              num1 = num2;
              num2 = num3;
            }
            if ((double) y >= (double) num1 && (double) y <= (double) num2)
            {
              timeValue = this.mousePositionInDrawing;
              curves = new CurveWrapper[2]
              {
                curveWrapperById,
                cw2
              };
              this.MoveCurveToFront(curveWrapperById.id);
              return true;
            }
            --index;
          }
        }
      }
      return false;
    }

    private void SelectPoints()
    {
      int controlId = GUIUtility.GetControlID(897560, FocusType.Passive);
      Event current = Event.current;
      bool shift = current.shift;
      bool actionKey = EditorGUI.actionKey;
      EventType typeForControl = current.GetTypeForControl(controlId);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (current.clickCount == 2 && current.button == 0)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey46 pointsCAnonStorey46 = new CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey46();
            // ISSUE: reference to a compiler-generated field
            pointsCAnonStorey46.selectedPoint = this.FindNearest();
            // ISSUE: reference to a compiler-generated field
            if (pointsCAnonStorey46.selectedPoint != null)
            {
              if (!shift)
                this.ClearSelection();
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey47 pointsCAnonStorey47 = new CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey47();
              // ISSUE: reference to a compiler-generated field
              pointsCAnonStorey47.\u003C\u003Ef__ref\u002470 = pointsCAnonStorey46;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              for (pointsCAnonStorey47.keyIndex = 0; pointsCAnonStorey47.keyIndex < pointsCAnonStorey46.selectedPoint.curve.keys.Length; pointsCAnonStorey47.keyIndex = pointsCAnonStorey47.keyIndex + 1)
              {
                // ISSUE: reference to a compiler-generated method
                if (!this.selectedCurves.Any<CurveSelection>(new Func<CurveSelection, bool>(pointsCAnonStorey47.\u003C\u003Em__77)))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.AddSelection(new CurveSelection(pointsCAnonStorey46.selectedPoint.curveID, this, pointsCAnonStorey47.keyIndex));
                }
              }
            }
            else
              this.CreateKeyFromClick(this.mousePositionInDrawing);
            current.Use();
            break;
          }
          if (current.button == 0)
          {
            CurveSelection nearest = this.FindNearest();
            if (nearest == null || nearest.semiSelected)
            {
              Vector2 zero = Vector2.zero;
              CurveWrapper[] curves = (CurveWrapper[]) null;
              bool frontOnMouseDown = this.HandleCurveAndRegionMoveToFrontOnMouseDown(ref zero, ref curves);
              if (!shift && !actionKey && !frontOnMouseDown)
                this.SelectNone();
              GUIUtility.hotControl = controlId;
              this.s_EndMouseDragPosition = this.s_StartMouseDragPosition = current.mousePosition;
              this.s_PickMode = CurveEditor.PickMode.Click;
            }
            else
            {
              this.MoveCurveToFront(nearest.curveID);
              this.activeTime = nearest.keyframe.time;
              this.s_StartKeyDragPosition = new Vector2(nearest.keyframe.time, nearest.keyframe.value);
              if (shift)
              {
                if (this.lastSelected == null || nearest.curveID != this.lastSelected.curveID)
                {
                  if (!this.selectedCurves.Contains(nearest))
                    this.AddSelection(nearest);
                }
                else
                {
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey48 pointsCAnonStorey48 = new CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey48();
                  // ISSUE: reference to a compiler-generated field
                  pointsCAnonStorey48.rangeCurveID = nearest.curveID;
                  int num1 = Mathf.Min(this.lastSelected.key, nearest.key);
                  int num2 = Mathf.Max(this.lastSelected.key, nearest.key);
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey49 pointsCAnonStorey49 = new CurveEditor.\u003CSelectPoints\u003Ec__AnonStorey49();
                  // ISSUE: reference to a compiler-generated field
                  pointsCAnonStorey49.\u003C\u003Ef__ref\u002472 = pointsCAnonStorey48;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  for (pointsCAnonStorey49.keyIndex = num1; pointsCAnonStorey49.keyIndex <= num2; pointsCAnonStorey49.keyIndex = pointsCAnonStorey49.keyIndex + 1)
                  {
                    // ISSUE: reference to a compiler-generated method
                    if (!this.selectedCurves.Any<CurveSelection>(new Func<CurveSelection, bool>(pointsCAnonStorey49.\u003C\u003Em__78)))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.AddSelection(new CurveSelection(pointsCAnonStorey48.rangeCurveID, this, pointsCAnonStorey49.keyIndex));
                    }
                  }
                }
                Event.current.Use();
              }
              else if (actionKey)
              {
                if (!this.selectedCurves.Contains(nearest))
                  this.AddSelection(nearest);
                else
                  this.RemoveSelection(nearest);
                Event.current.Use();
              }
              else if (!this.selectedCurves.Contains(nearest))
              {
                this.ClearSelection();
                this.AddSelection(nearest);
              }
            }
            GUI.changed = true;
            HandleUtility.Repaint();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            GUIUtility.hotControl = 0;
            this.s_PickMode = CurveEditor.PickMode.None;
            Event.current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            this.s_EndMouseDragPosition = current.mousePosition;
            if (this.s_PickMode == CurveEditor.PickMode.Click)
            {
              this.s_PickMode = CurveEditor.PickMode.Marquee;
              this.s_SelectionBackup = !shift ? new List<CurveSelection>() : new List<CurveSelection>((IEnumerable<CurveSelection>) this.selectedCurves);
            }
            else
            {
              Rect rect = EditorGUIExt.FromToRect(this.s_StartMouseDragPosition, current.mousePosition);
              List<CurveSelection> curveSelectionList = new List<CurveSelection>((IEnumerable<CurveSelection>) this.s_SelectionBackup);
              foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
              {
                if (!animationCurve.readOnly && !animationCurve.hidden)
                {
                  int keyIndex = 0;
                  foreach (Keyframe key in animationCurve.curve.keys)
                  {
                    if (rect.Contains(this.GetGUIPoint((Vector3) new Vector2(key.time, key.value))))
                    {
                      curveSelectionList.Add(new CurveSelection(animationCurve.id, this, keyIndex));
                      this.MoveCurveToFront(animationCurve.id);
                    }
                    ++keyIndex;
                  }
                }
              }
              this.selectedCurves = curveSelectionList;
              GUI.changed = true;
            }
            current.Use();
            break;
          }
          break;
        default:
          if (typeForControl != EventType.Layout)
          {
            if (typeForControl == EventType.ContextClick)
            {
              Rect drawRect = this.drawRect;
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              Rect& local = @drawRect;
              float num1 = 0.0f;
              drawRect.y = num1;
              double num2 = (double) num1;
              // ISSUE: explicit reference operation
              (^local).x = (float) num2;
              Vector2 closestPointOnCurve;
              if (drawRect.Contains(Event.current.mousePosition) && this.GetCurveAtPosition(this.mousePositionInDrawing, out closestPointOnCurve) >= 0)
              {
                GenericMenu genericMenu = new GenericMenu();
                genericMenu.AddItem(new GUIContent("Add Key"), false, new GenericMenu.MenuFunction2(this.CreateKeyFromClick), (object) this.mousePositionInDrawing);
                genericMenu.ShowAsContext();
                Event.current.Use();
                break;
              }
              break;
            }
            break;
          }
          HandleUtility.AddDefaultControl(controlId);
          break;
      }
      if (this.s_PickMode != CurveEditor.PickMode.Marquee)
        return;
      GUI.Label(EditorGUIExt.FromToRect(this.s_StartMouseDragPosition, this.s_EndMouseDragPosition), GUIContent.none, this.ms_Styles.selectionRect);
    }

    private void EditAxisLabels()
    {
      int controlId = GUIUtility.GetControlID(18975602, FocusType.Keyboard);
      List<CurveWrapper> curveWrapperList = new List<CurveWrapper>();
      Vector2 axisUiScalars = this.GetAxisUiScalars(curveWrapperList);
      if ((double) axisUiScalars.y < 0.0 || curveWrapperList.Count <= 0 || curveWrapperList[0].setAxisUiScalarsCallback == null)
        return;
      Rect position1 = new Rect(0.0f, this.topmargin - 8f, this.leftmargin - 4f, 16f);
      Rect position2 = position1;
      position2.y -= position1.height;
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (current.button == 0)
          {
            if (position2.Contains(Event.current.mousePosition) && GUIUtility.hotControl == 0)
            {
              GUIUtility.keyboardControl = 0;
              GUIUtility.hotControl = controlId;
              GUI.changed = true;
              current.Use();
            }
            if (!position1.Contains(Event.current.mousePosition))
            {
              GUIUtility.keyboardControl = 0;
              break;
            }
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            GUIUtility.hotControl = 0;
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            float num = Mathf.Clamp01(Mathf.Max(axisUiScalars.y, Mathf.Pow(Mathf.Abs(axisUiScalars.y), 0.5f)) * 0.01f);
            axisUiScalars.y += HandleUtility.niceMouseDelta * num;
            if ((double) axisUiScalars.y < 1.0 / 1000.0)
              axisUiScalars.y = 1f / 1000f;
            this.SetAxisUiScalars(axisUiScalars, curveWrapperList);
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (GUIUtility.hotControl == 0)
          {
            EditorGUIUtility.AddCursorRect(position2, MouseCursor.SlideArrow);
            break;
          }
          break;
      }
      string fieldFormatString = EditorGUI.kFloatFieldFormatString;
      EditorGUI.kFloatFieldFormatString = this.m_AxisLabelFormat;
      float y = EditorGUI.FloatField(position1, axisUiScalars.y, this.ms_Styles.axisLabelNumberField);
      if ((double) axisUiScalars.y != (double) y)
        this.SetAxisUiScalars(new Vector2(axisUiScalars.x, y), curveWrapperList);
      EditorGUI.kFloatFieldFormatString = fieldFormatString;
    }

    public void BeginTimeRangeSelection(float time, bool addToSelection)
    {
      if (this.s_TimeRangeSelectionActive)
      {
        Debug.LogError((object) "BeginTimeRangeSelection can only be called once");
      }
      else
      {
        this.s_TimeRangeSelectionActive = true;
        this.s_TimeRangeSelectionStart = this.s_TimeRangeSelectionEnd = time;
        if (addToSelection)
          this.s_SelectionBackup = new List<CurveSelection>((IEnumerable<CurveSelection>) this.selectedCurves);
        else
          this.s_SelectionBackup = new List<CurveSelection>();
      }
    }

    public void TimeRangeSelectTo(float time)
    {
      if (!this.s_TimeRangeSelectionActive)
      {
        Debug.LogError((object) "TimeRangeSelectTo can only be called after BeginTimeRangeSelection");
      }
      else
      {
        this.s_TimeRangeSelectionEnd = time;
        List<CurveSelection> curveSelectionList = new List<CurveSelection>((IEnumerable<CurveSelection>) this.s_SelectionBackup);
        float num1 = Mathf.Min(this.s_TimeRangeSelectionStart, this.s_TimeRangeSelectionEnd);
        float num2 = Mathf.Max(this.s_TimeRangeSelectionStart, this.s_TimeRangeSelectionEnd);
        foreach (CurveWrapper animationCurve in this.m_AnimationCurves)
        {
          if (!animationCurve.readOnly && !animationCurve.hidden)
          {
            int keyIndex = 0;
            foreach (Keyframe key in animationCurve.curve.keys)
            {
              if ((double) key.time >= (double) num1 && (double) key.time < (double) num2)
                curveSelectionList.Add(new CurveSelection(animationCurve.id, this, keyIndex));
              ++keyIndex;
            }
          }
        }
        this.selectedCurves = curveSelectionList;
        this.RecalcSecondarySelection();
        this.RecalcCurveSelection();
      }
    }

    public void EndTimeRangeSelection()
    {
      if (!this.s_TimeRangeSelectionActive)
      {
        Debug.LogError((object) "EndTimeRangeSelection can only be called after BeginTimeRangeSelection");
      }
      else
      {
        this.s_TimeRangeSelectionStart = this.s_TimeRangeSelectionEnd = 0.0f;
        this.s_TimeRangeSelectionActive = false;
      }
    }

    public void CancelTimeRangeSelection()
    {
      if (!this.s_TimeRangeSelectionActive)
      {
        Debug.LogError((object) "CancelTimeRangeSelection can only be called after BeginTimeRangeSelection");
      }
      else
      {
        this.selectedCurves = this.s_SelectionBackup;
        this.s_TimeRangeSelectionActive = false;
      }
    }

    public void DrawTimeRange()
    {
      if (!this.s_TimeRangeSelectionActive || Event.current.type != EventType.Repaint)
        return;
      float x1 = Mathf.Min(this.s_TimeRangeSelectionStart, this.s_TimeRangeSelectionEnd);
      float x2 = Mathf.Max(this.s_TimeRangeSelectionStart, this.s_TimeRangeSelectionEnd);
      float x3 = this.GetGUIPoint(new Vector3(x1, 0.0f, 0.0f)).x;
      float x4 = this.GetGUIPoint(new Vector3(x2, 0.0f, 0.0f)).x;
      GUI.Label(new Rect(x3, -10000f, x4 - x3, 20000f), GUIContent.none, this.ms_Styles.selectionRect);
    }

    private void StartEditingSelectedPointsContext(object fieldPosition)
    {
      this.StartEditingSelectedPoints((Vector2) fieldPosition);
    }

    private void StartEditingSelectedPoints()
    {
      this.StartEditingSelectedPoints(new Vector2(this.selectedCurves.Min<CurveSelection>((Func<CurveSelection, float>) (x => x.keyframe.time)) + this.selectedCurves.Max<CurveSelection>((Func<CurveSelection, float>) (x => x.keyframe.time)), this.selectedCurves.Min<CurveSelection>((Func<CurveSelection, float>) (x => x.keyframe.value)) + this.selectedCurves.Max<CurveSelection>((Func<CurveSelection, float>) (x => x.keyframe.value))) * 0.5f);
    }

    private void StartEditingSelectedPoints(Vector2 fieldPosition)
    {
      this.pointEditingFieldPosition = fieldPosition;
      this.focusedPointField = "pointValueField";
      this.timeWasEdited = this.valueWasEdited = false;
      this.MakeCurveBackups();
      this.editingPoints = true;
    }

    private void FinishEditingPoints()
    {
      this.editingPoints = false;
    }

    private void EditSelectedPoints()
    {
      Event current = Event.current;
      if (this.editingPoints && !this.hasSelection)
        this.editingPoints = false;
      bool flag = false;
      if (current.type == EventType.KeyDown)
      {
        if (current.keyCode == KeyCode.KeypadEnter || current.keyCode == KeyCode.Return)
        {
          if (this.hasSelection && !this.editingPoints)
          {
            this.StartEditingSelectedPoints();
            current.Use();
          }
          else if (this.editingPoints)
          {
            this.FinishEditingPoints();
            current.Use();
          }
        }
        else if (current.keyCode == KeyCode.Escape)
          flag = true;
      }
      if (!this.editingPoints)
        return;
      Vector2 viewTransformPoint = this.DrawingToViewTransformPoint(this.pointEditingFieldPosition);
      Rect rect = Rect.MinMaxRect(this.leftmargin, this.topmargin, this.rect.width - this.rightmargin, this.rect.height - this.bottommargin);
      viewTransformPoint.x = Mathf.Clamp(viewTransformPoint.x, rect.xMin, rect.xMax - 80f);
      viewTransformPoint.y = Mathf.Clamp(viewTransformPoint.y, rect.yMin, rect.yMax - 36f);
      EditorGUI.BeginChangeCheck();
      GUI.SetNextControlName("pointTimeField");
      float x1 = this.PointFieldForSelection(new Rect(viewTransformPoint.x, viewTransformPoint.y, 80f, 18f), 1, (Func<CurveSelection, float>) (x => x.keyframe.time), "time");
      if (EditorGUI.EndChangeCheck())
        this.timeWasEdited = true;
      EditorGUI.BeginChangeCheck();
      GUI.SetNextControlName("pointValueField");
      float y = this.PointFieldForSelection(new Rect(viewTransformPoint.x, viewTransformPoint.y + 18f, 80f, 18f), 2, (Func<CurveSelection, float>) (x => x.keyframe.value), "value");
      if (EditorGUI.EndChangeCheck())
        this.valueWasEdited = true;
      if (this.timeWasEdited || this.valueWasEdited)
        this.SetSelectedKeyPositions(new Vector2(x1, y), this.timeWasEdited, this.valueWasEdited);
      if (flag)
        this.FinishEditingPoints();
      if (this.focusedPointField != null)
      {
        EditorGUI.FocusTextInControl(this.focusedPointField);
        if (current.type == EventType.Repaint)
          this.focusedPointField = (string) null;
      }
      if (current.type == EventType.KeyDown && (int) current.character == 9)
      {
        this.focusedPointField = !(GUI.GetNameOfFocusedControl() == "pointValueField") ? "pointValueField" : "pointTimeField";
        current.Use();
      }
      if (current.type != EventType.MouseDown)
        return;
      this.FinishEditingPoints();
    }

    private float PointFieldForSelection(Rect rect, int customID, Func<CurveSelection, float> memberGetter, string label)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CurveEditor.\u003CPointFieldForSelection\u003Ec__AnonStorey4A selectionCAnonStorey4A = new CurveEditor.\u003CPointFieldForSelection\u003Ec__AnonStorey4A();
      // ISSUE: reference to a compiler-generated field
      selectionCAnonStorey4A.memberGetter = memberGetter;
      // ISSUE: reference to a compiler-generated field
      selectionCAnonStorey4A.\u003C\u003Ef__this = this;
      float num1 = 0.0f;
      // ISSUE: reference to a compiler-generated method
      if (this.selectedCurves.All<CurveSelection>(new Func<CurveSelection, bool>(selectionCAnonStorey4A.\u003C\u003Em__7F)))
      {
        // ISSUE: reference to a compiler-generated field
        num1 = selectionCAnonStorey4A.memberGetter(this.selectedCurves[0]);
      }
      else
        EditorGUI.showMixedValue = true;
      Rect position = rect;
      position.x -= position.width;
      GUIStyle label1 = GUI.skin.label;
      label1.alignment = TextAnchor.UpperRight;
      int controlId = GUIUtility.GetControlID(customID, FocusType.Keyboard, rect);
      Color color = GUI.color;
      GUI.color = Color.white;
      GUI.Label(position, label, label1);
      float num2 = EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, rect, new Rect(0.0f, 0.0f, 0.0f, 0.0f), controlId, num1, EditorGUI.kFloatFieldFormatString, EditorStyles.numberField, false);
      GUI.color = color;
      EditorGUI.showMixedValue = false;
      return num2;
    }

    private void SetupKeyOrCurveDragging(Vector2 timeValue, CurveWrapper cw, int id, Vector2 mousePos)
    {
      this.m_DraggedCoord = timeValue;
      this.m_DraggingKey = cw;
      GUIUtility.hotControl = id;
      this.MakeCurveBackups();
      this.activeTime = timeValue.x;
      this.s_StartMouseDragPosition = mousePos;
      this.s_StartClickedTime = timeValue.x;
    }

    public Vector2 MovePoints()
    {
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      if (!this.hasSelection && !this.settings.allowDraggingCurvesAndRegions)
        return Vector2.zero;
      Event current1 = Event.current;
      switch (current1.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (current1.button == 0)
          {
            using (List<CurveSelection>.Enumerator enumerator = this.selectedCurves.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                CurveSelection current2 = enumerator.Current;
                if (!current2.curveWrapper.hidden && (double) (this.DrawingToViewTransformPoint(this.GetPosition(current2)) - current1.mousePosition).sqrMagnitude <= 100.0)
                {
                  this.SetupKeyOrCurveDragging(new Vector2(current2.keyframe.time, current2.keyframe.value), current2.curveWrapper, controlId, current1.mousePosition);
                  break;
                }
              }
            }
            if (this.settings.allowDraggingCurvesAndRegions && this.m_DraggingKey == null)
            {
              Vector2 zero = Vector2.zero;
              CurveWrapper[] curves = (CurveWrapper[]) null;
              if (this.HandleCurveAndRegionMoveToFrontOnMouseDown(ref zero, ref curves))
              {
                List<CurveSelection> curveSelectionList = new List<CurveSelection>();
                foreach (CurveWrapper curveWrapper in curves)
                {
                  for (int keyIndex = 0; keyIndex < curveWrapper.curve.keys.Length; ++keyIndex)
                    curveSelectionList.Add(new CurveSelection(curveWrapper.id, this, keyIndex));
                  this.MoveCurveToFront(curveWrapper.id);
                }
                this.preCurveDragSelection = this.selectedCurves;
                this.selectedCurves = curveSelectionList;
                this.SetupKeyOrCurveDragging(zero, curves[0], controlId, current1.mousePosition);
                this.m_DraggingCurveOrRegion = curves;
                break;
              }
              break;
            }
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            this.ResetDragging();
            GUI.changed = true;
            current1.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            Vector2 lhs = current1.mousePosition - this.s_StartMouseDragPosition;
            Vector3 vector3 = Vector3.zero;
            if (current1.shift && this.m_AxisLock == CurveEditor.AxisLock.None)
              this.m_AxisLock = (double) Mathf.Abs(lhs.x) <= (double) Mathf.Abs(lhs.y) ? CurveEditor.AxisLock.Y : CurveEditor.AxisLock.X;
            if (this.m_DraggingCurveOrRegion != null)
            {
              lhs.x = 0.0f;
              vector3 = (Vector3) this.ViewToDrawingTransformVector(lhs);
              vector3.y = this.SnapValue(vector3.y + this.s_StartKeyDragPosition.y) - this.s_StartKeyDragPosition.y;
            }
            else
            {
              switch (this.m_AxisLock)
              {
                case CurveEditor.AxisLock.None:
                  vector3 = (Vector3) this.ViewToDrawingTransformVector(lhs);
                  vector3.x = this.SnapTime(vector3.x + this.s_StartKeyDragPosition.x) - this.s_StartKeyDragPosition.x;
                  vector3.y = this.SnapValue(vector3.y + this.s_StartKeyDragPosition.y) - this.s_StartKeyDragPosition.y;
                  break;
                case CurveEditor.AxisLock.X:
                  lhs.y = 0.0f;
                  vector3 = (Vector3) this.ViewToDrawingTransformVector(lhs);
                  vector3.x = this.SnapTime(vector3.x + this.s_StartKeyDragPosition.x) - this.s_StartKeyDragPosition.x;
                  break;
                case CurveEditor.AxisLock.Y:
                  lhs.x = 0.0f;
                  vector3 = (Vector3) this.ViewToDrawingTransformVector(lhs);
                  vector3.y = this.SnapValue(vector3.y + this.s_StartKeyDragPosition.y) - this.s_StartKeyDragPosition.y;
                  break;
              }
            }
            this.TranslateSelectedKeys((Vector2) vector3);
            GUI.changed = true;
            current1.Use();
            return (Vector2) vector3;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl == controlId && current1.keyCode == KeyCode.Escape)
          {
            this.TranslateSelectedKeys(Vector2.zero);
            this.ResetDragging();
            GUI.changed = true;
            current1.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (this.m_DraggingCurveOrRegion != null)
          {
            EditorGUIUtility.AddCursorRect(new Rect(current1.mousePosition.x - 10f, current1.mousePosition.y - 10f, 20f, 20f), MouseCursor.ResizeVertical);
            break;
          }
          break;
      }
      return Vector2.zero;
    }

    private void ResetDragging()
    {
      if (this.m_DraggingCurveOrRegion != null)
      {
        this.selectedCurves = this.preCurveDragSelection;
        this.preCurveDragSelection = (List<CurveSelection>) null;
      }
      GUIUtility.hotControl = 0;
      this.m_DraggingKey = (CurveWrapper) null;
      this.m_DraggingCurveOrRegion = (CurveWrapper[]) null;
      this.m_MoveCoord = Vector2.zero;
      this.m_AxisLock = CurveEditor.AxisLock.None;
    }

    internal void MakeCurveBackups()
    {
      this.m_CurveBackups = new List<CurveEditor.SavedCurve>();
      int num = -1;
      CurveEditor.SavedCurve savedCurve = (CurveEditor.SavedCurve) null;
      for (int index = 0; index < this.selectedCurves.Count; ++index)
      {
        CurveSelection selectedCurve = this.selectedCurves[index];
        if (selectedCurve.curveID != num)
        {
          savedCurve = new CurveEditor.SavedCurve();
          num = savedCurve.curveId = selectedCurve.curveID;
          Keyframe[] keys = selectedCurve.curve.keys;
          savedCurve.keys = new List<CurveEditor.SavedCurve.SavedKeyFrame>(keys.Length);
          foreach (Keyframe key in keys)
            savedCurve.keys.Add(new CurveEditor.SavedCurve.SavedKeyFrame(key, CurveWrapper.SelectionMode.None));
          this.m_CurveBackups.Add(savedCurve);
        }
        savedCurve.keys[selectedCurve.key].selected = !selectedCurve.semiSelected ? CurveWrapper.SelectionMode.Selected : CurveWrapper.SelectionMode.SemiSelected;
      }
    }

    private Vector2 GetPosition(CurveSelection selection)
    {
      Keyframe keyframe = selection.keyframe;
      Vector2 vector2_1 = new Vector2(keyframe.time, keyframe.value);
      float num = 50f;
      if (selection.type == CurveSelection.SelectionType.InTangent)
      {
        Vector2 vec = new Vector2(1f, keyframe.inTangent);
        if ((double) keyframe.inTangent == double.PositiveInfinity)
          vec = new Vector2(0.0f, -1f);
        Vector2 vector2_2 = this.NormalizeInViewSpace(vec);
        return vector2_1 - vector2_2 * num;
      }
      if (selection.type != CurveSelection.SelectionType.OutTangent)
        return vector2_1;
      Vector2 vec1 = new Vector2(1f, keyframe.outTangent);
      if ((double) keyframe.outTangent == double.PositiveInfinity)
        vec1 = new Vector2(0.0f, -1f);
      Vector2 vector2_3 = this.NormalizeInViewSpace(vec1);
      return vector2_1 + vector2_3 * num;
    }

    private void MoveCurveToFront(int curveID)
    {
      int count = this.m_DrawOrder.Count;
      for (int index = 0; index < count; ++index)
      {
        if (this.m_DrawOrder[index] == curveID)
        {
          int regionId = this.getCurveWrapperById(curveID).regionId;
          if (regionId >= 0)
          {
            int num1 = 0;
            int num2 = -1;
            if (index - 1 >= 0)
            {
              int id = this.m_DrawOrder[index - 1];
              if (regionId == this.getCurveWrapperById(id).regionId)
              {
                num2 = id;
                num1 = -1;
              }
            }
            if (index + 1 < count && num2 < 0)
            {
              int id = this.m_DrawOrder[index + 1];
              if (regionId == this.getCurveWrapperById(id).regionId)
              {
                num2 = id;
                num1 = 0;
              }
            }
            if (num2 >= 0)
            {
              this.m_DrawOrder.RemoveRange(index + num1, 2);
              this.m_DrawOrder.Add(num2);
              this.m_DrawOrder.Add(curveID);
              this.ValidateCurveList();
              break;
            }
            Debug.LogError((object) "Unhandled region");
          }
          else
          {
            if (index == count - 1)
              break;
            this.m_DrawOrder.RemoveAt(index);
            this.m_DrawOrder.Add(curveID);
            this.ValidateCurveList();
            break;
          }
        }
      }
    }

    private bool IsCurveSelected(CurveWrapper cw)
    {
      if (cw != null)
        return cw.selected != CurveWrapper.SelectionMode.None;
      return false;
    }

    private bool IsRegionCurveSelected(CurveWrapper cw1, CurveWrapper cw2)
    {
      if (!this.IsCurveSelected(cw1))
        return this.IsCurveSelected(cw2);
      return true;
    }

    private bool IsRegion(CurveWrapper cw1, CurveWrapper cw2)
    {
      if (cw1 != null && cw2 != null && cw1.regionId >= 0)
        return cw1.regionId == cw2.regionId;
      return false;
    }

    private void DrawCurvesAndRegion(CurveWrapper cw1, CurveWrapper cw2, List<CurveSelection> selection, bool hasFocus)
    {
      this.DrawRegion(cw1, cw2, hasFocus);
      this.DrawCurveAndPoints(cw1, !this.IsCurveSelected(cw1) ? (List<CurveSelection>) null : selection, hasFocus);
      this.DrawCurveAndPoints(cw2, !this.IsCurveSelected(cw2) ? (List<CurveSelection>) null : selection, hasFocus);
    }

    private void DrawCurveAndPoints(CurveWrapper cw, List<CurveSelection> selection, bool hasFocus)
    {
      this.DrawCurve(cw, hasFocus);
      this.DrawPointsOnCurve(cw, selection, hasFocus);
    }

    private bool ShouldCurveHaveFocus(int indexIntoDrawOrder, CurveWrapper cw1, CurveWrapper cw2)
    {
      bool flag = false;
      if (indexIntoDrawOrder == this.m_DrawOrder.Count - 1)
        flag = true;
      else if (this.hasSelection)
        flag = this.IsCurveSelected(cw1) || this.IsCurveSelected(cw2);
      return flag;
    }

    private void DrawCurves(CurveWrapper[] curves)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      for (int indexIntoDrawOrder = 0; indexIntoDrawOrder < this.m_DrawOrder.Count; ++indexIntoDrawOrder)
      {
        CurveWrapper curveWrapperById = this.getCurveWrapperById(this.m_DrawOrder[indexIntoDrawOrder]);
        if (curveWrapperById != null && !curveWrapperById.hidden && curveWrapperById.curve.length != 0)
        {
          CurveWrapper cw2 = (CurveWrapper) null;
          if (indexIntoDrawOrder < this.m_DrawOrder.Count - 1)
            cw2 = this.getCurveWrapperById(this.m_DrawOrder[indexIntoDrawOrder + 1]);
          if (this.IsRegion(curveWrapperById, cw2))
          {
            ++indexIntoDrawOrder;
            bool hasFocus = this.ShouldCurveHaveFocus(indexIntoDrawOrder, curveWrapperById, cw2);
            this.DrawCurvesAndRegion(curveWrapperById, cw2, !this.IsRegionCurveSelected(curveWrapperById, cw2) ? (List<CurveSelection>) null : this.selectedCurves, hasFocus);
          }
          else
          {
            bool hasFocus = this.ShouldCurveHaveFocus(indexIntoDrawOrder, curveWrapperById, (CurveWrapper) null);
            this.DrawCurveAndPoints(curveWrapperById, !this.IsCurveSelected(curveWrapperById) ? (List<CurveSelection>) null : this.selectedCurves, hasFocus);
          }
        }
      }
      if (this.m_DraggingCurveOrRegion != null)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(1);
      GL.Color(this.m_TangentColor * new Color(1f, 1f, 1f, 0.75f));
      using (List<CurveSelection>.Enumerator enumerator = this.selectedCurves.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          CurveSelection current = enumerator.Current;
          if (!current.semiSelected)
          {
            Vector2 position1 = this.GetPosition(current);
            if (CurveUtility.GetKeyTangentMode(current.keyframe, 0) == TangentMode.Editable && (double) current.keyframe.time != (double) current.curve.keys[0].time)
              this.DrawLine(this.GetPosition(new CurveSelection(current.curveID, this, current.key, CurveSelection.SelectionType.InTangent)), position1);
            if (CurveUtility.GetKeyTangentMode(current.keyframe, 1) == TangentMode.Editable && (double) current.keyframe.time != (double) current.curve.keys[current.curve.keys.Length - 1].time)
            {
              Vector2 position2 = this.GetPosition(new CurveSelection(current.curveID, this, current.key, CurveSelection.SelectionType.OutTangent));
              this.DrawLine(position1, position2);
            }
          }
        }
      }
      GL.End();
      GUI.color = this.m_TangentColor;
      using (List<CurveSelection>.Enumerator enumerator = this.selectedCurves.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          CurveSelection current = enumerator.Current;
          if (!current.semiSelected)
          {
            if (CurveUtility.GetKeyTangentMode(current.keyframe, 0) == TangentMode.Editable && (double) current.keyframe.time != (double) current.curve.keys[0].time)
            {
              Vector2 position = this.GetPosition(new CurveSelection(current.curveID, this, current.key, CurveSelection.SelectionType.InTangent));
              this.DrawPoint(position.x, position.y, CurveWrapper.SelectionMode.None);
            }
            if (CurveUtility.GetKeyTangentMode(current.keyframe, 1) == TangentMode.Editable && (double) current.keyframe.time != (double) current.curve.keys[current.curve.keys.Length - 1].time)
            {
              Vector2 position = this.GetPosition(new CurveSelection(current.curveID, this, current.key, CurveSelection.SelectionType.OutTangent));
              this.DrawPoint(position.x, position.y, CurveWrapper.SelectionMode.None);
            }
          }
        }
      }
      if (this.m_DraggingKey == null)
        return;
      float vRangeMin = this.vRangeMin;
      float vRangeMax = this.vRangeMax;
      float min = Mathf.Max(vRangeMin, this.m_DraggingKey.vRangeMin);
      float max = Mathf.Min(vRangeMax, this.m_DraggingKey.vRangeMax);
      Vector2 lhs = this.m_DraggedCoord + this.m_MoveCoord;
      lhs.x = Mathf.Clamp(lhs.x, this.hRangeMin, this.hRangeMax);
      lhs.y = Mathf.Clamp(lhs.y, min, max);
      Vector2 viewTransformPoint = this.DrawingToViewTransformPoint(lhs);
      int num = (double) this.invSnap == 0.0 ? MathUtils.GetNumberOfDecimalsForMinimumDifference(this.shownArea.width / this.drawRect.width) : MathUtils.GetNumberOfDecimalsForMinimumDifference(1f / this.invSnap);
      int minimumDifference = MathUtils.GetNumberOfDecimalsForMinimumDifference(this.shownArea.height / this.drawRect.height);
      Vector2 vector2_1 = this.m_DraggingKey.getAxisUiScalarsCallback == null ? Vector2.one : this.m_DraggingKey.getAxisUiScalarsCallback();
      if ((double) vector2_1.x >= 0.0)
        lhs.x *= vector2_1.x;
      if ((double) vector2_1.y >= 0.0)
        lhs.y *= vector2_1.y;
      GUIContent content = new GUIContent(string.Format("{0}, {1}", (object) lhs.x.ToString("N" + (object) num), (object) lhs.y.ToString("N" + (object) minimumDifference)));
      Vector2 vector2_2 = this.ms_Styles.dragLabel.CalcSize(content);
      EditorGUI.DoDropShadowLabel(new Rect(viewTransformPoint.x, viewTransformPoint.y - vector2_2.y, vector2_2.x, vector2_2.y), content, this.ms_Styles.dragLabel, 0.3f);
    }

    private static List<Vector3> CreateRegion(CurveWrapper minCurve, CurveWrapper maxCurve, float deltaTime)
    {
      List<Vector3> vector3List = new List<Vector3>();
      List<float> floatList = new List<float>();
      float num1 = deltaTime;
      while ((double) num1 <= 1.0)
      {
        floatList.Add(num1);
        num1 += deltaTime;
      }
      if ((double) num1 != 1.0)
        floatList.Add(1f);
      foreach (Keyframe key in maxCurve.curve.keys)
      {
        if ((double) key.time > 0.0 && (double) key.time < 1.0)
        {
          floatList.Add(key.time - 0.0001f);
          floatList.Add(key.time);
          floatList.Add(key.time + 0.0001f);
        }
      }
      foreach (Keyframe key in minCurve.curve.keys)
      {
        if ((double) key.time > 0.0 && (double) key.time < 1.0)
        {
          floatList.Add(key.time - 0.0001f);
          floatList.Add(key.time);
          floatList.Add(key.time + 0.0001f);
        }
      }
      floatList.Sort();
      Vector3 vector3_1 = new Vector3(0.0f, maxCurve.renderer.EvaluateCurveSlow(0.0f), 0.0f);
      Vector3 vector3_2 = new Vector3(0.0f, minCurve.renderer.EvaluateCurveSlow(0.0f), 0.0f);
      for (int index = 0; index < floatList.Count; ++index)
      {
        float num2 = floatList[index];
        Vector3 vector3_3 = new Vector3(num2, maxCurve.renderer.EvaluateCurveSlow(num2), 0.0f);
        Vector3 vector3_4 = new Vector3(num2, minCurve.renderer.EvaluateCurveSlow(num2), 0.0f);
        if ((double) vector3_1.y >= (double) vector3_2.y && (double) vector3_3.y >= (double) vector3_4.y)
        {
          vector3List.Add(vector3_1);
          vector3List.Add(vector3_4);
          vector3List.Add(vector3_2);
          vector3List.Add(vector3_1);
          vector3List.Add(vector3_3);
          vector3List.Add(vector3_4);
        }
        else if ((double) vector3_1.y <= (double) vector3_2.y && (double) vector3_3.y <= (double) vector3_4.y)
        {
          vector3List.Add(vector3_2);
          vector3List.Add(vector3_3);
          vector3List.Add(vector3_1);
          vector3List.Add(vector3_2);
          vector3List.Add(vector3_4);
          vector3List.Add(vector3_3);
        }
        else
        {
          Vector2 zero = Vector2.zero;
          if (Mathf.LineIntersection((Vector2) vector3_1, (Vector2) vector3_3, (Vector2) vector3_2, (Vector2) vector3_4, ref zero))
          {
            vector3List.Add(vector3_1);
            vector3List.Add((Vector3) zero);
            vector3List.Add(vector3_2);
            vector3List.Add(vector3_3);
            vector3List.Add((Vector3) zero);
            vector3List.Add(vector3_4);
          }
          else
            Debug.Log((object) "Error: No intersection found! There should be one...");
        }
        vector3_1 = vector3_3;
        vector3_2 = vector3_4;
      }
      return vector3List;
    }

    public void DrawRegion(CurveWrapper curve1, CurveWrapper curve2, bool hasFocus)
    {
      float deltaTime = (float) (1.0 / ((double) this.rect.width / 10.0));
      List<Vector3> region = CurveEditor.CreateRegion(curve1, curve2, deltaTime);
      Color color1 = curve1.color;
      for (int index = 0; index < region.Count; ++index)
        region[index] = this.drawingToViewMatrix.MultiplyPoint(region[index]);
      Color color2;
      if (this.IsDraggingRegion(curve1, curve2))
      {
        color2 = Color.Lerp(color1, Color.black, 0.1f);
        color2.a = 0.4f;
      }
      else if (this.settings.useFocusColors && !hasFocus)
      {
        color2 = color1 * 0.4f;
        color2.a = 0.1f;
      }
      else
      {
        color2 = color1 * 1f;
        color2.a = 0.4f;
      }
      Shader.SetGlobalColor("_HandleColor", color2);
      HandleUtility.ApplyWireMaterial();
      GL.Begin(4);
      int num = region.Count / 3;
      for (int index = 0; index < num; ++index)
      {
        GL.Color(color2);
        GL.Vertex(region[index * 3]);
        GL.Vertex(region[index * 3 + 1]);
        GL.Vertex(region[index * 3 + 2]);
      }
      GL.End();
    }

    private void DrawCurve(CurveWrapper cw, bool hasFocus)
    {
      CurveRenderer renderer = cw.renderer;
      Color color = cw.color;
      if (this.IsDraggingCurve(cw) || cw.selected == CurveWrapper.SelectionMode.Selected)
        color = Color.Lerp(color, Color.white, 0.3f);
      else if (this.settings.useFocusColors && !hasFocus)
      {
        color *= 0.5f;
        color.a = 0.8f;
      }
      Rect shownArea = this.shownArea;
      renderer.DrawCurve(shownArea.xMin, shownArea.xMax, color, this.drawingToViewMatrix, this.settings.wrapColor);
    }

    private void DrawPointsOnCurve(CurveWrapper cw, List<CurveSelection> selected, bool hasFocus)
    {
      this.m_PreviousDrawPointCenter = new Vector2(float.MinValue, float.MinValue);
      if (selected == null)
      {
        Color color = cw.color;
        if (this.settings.useFocusColors && !hasFocus)
          color *= 0.5f;
        GUI.color = color;
        foreach (Keyframe key in cw.curve.keys)
          this.DrawPoint(key.time, key.value, CurveWrapper.SelectionMode.None);
      }
      else
      {
        GUI.color = Color.Lerp(cw.color, Color.white, 0.2f);
        int index = 0;
        while (index < selected.Count && selected[index].curveID != cw.id)
          ++index;
        int num = 0;
        foreach (Keyframe key in cw.curve.keys)
        {
          if (index < selected.Count && selected[index].key == num && selected[index].curveID == cw.id)
          {
            this.DrawPoint(key.time, key.value, !selected[index].semiSelected ? CurveWrapper.SelectionMode.Selected : CurveWrapper.SelectionMode.SemiSelected);
            ++index;
          }
          else
            this.DrawPoint(key.time, key.value, CurveWrapper.SelectionMode.None);
          ++num;
        }
        GUI.color = Color.white;
      }
    }

    private void DrawPoint(float x, float y, CurveWrapper.SelectionMode selected)
    {
      Vector3 vector3 = this.DrawingToViewTransformPoint(new Vector3(x, y, 0.0f));
      vector3 = new Vector3(Mathf.Floor(vector3.x), Mathf.Floor(vector3.y), 0.0f);
      Rect position = new Rect(vector3.x - 4f, vector3.y - 4f, (float) this.ms_Styles.pointIcon.width, (float) this.ms_Styles.pointIcon.height);
      Vector2 center = position.center;
      if ((double) (center - this.m_PreviousDrawPointCenter).magnitude <= 8.0)
        return;
      if (selected == CurveWrapper.SelectionMode.None)
      {
        GUI.Label(position, (Texture) this.ms_Styles.pointIcon, GUIStyle.none);
      }
      else
      {
        GUI.Label(position, (Texture) this.ms_Styles.pointIconSelected, GUIStyle.none);
        Color color = GUI.color;
        GUI.color = Color.white;
        if (selected == CurveWrapper.SelectionMode.Selected)
          GUI.Label(position, (Texture) this.ms_Styles.pointIconSelectedOverlay, GUIStyle.none);
        else
          GUI.Label(position, (Texture) this.ms_Styles.pointIconSemiSelectedOverlay, GUIStyle.none);
        GUI.color = color;
      }
      this.m_PreviousDrawPointCenter = center;
    }

    private void DrawLine(Vector2 lhs, Vector2 rhs)
    {
      GL.Vertex(this.DrawingToViewTransformPoint(new Vector3(lhs.x, lhs.y, 0.0f)));
      GL.Vertex(this.DrawingToViewTransformPoint(new Vector3(rhs.x, rhs.y, 0.0f)));
    }

    private Vector2 GetAxisUiScalars(List<CurveWrapper> curvesWithSameParameterSpace)
    {
      if (this.selectedCurves.Count <= 1)
      {
        if (this.m_DrawOrder.Count > 0)
        {
          CurveWrapper curveWrapperById = this.getCurveWrapperById(this.m_DrawOrder[this.m_DrawOrder.Count - 1]);
          if (curveWrapperById.getAxisUiScalarsCallback != null)
          {
            if (curvesWithSameParameterSpace != null)
              curvesWithSameParameterSpace.Add(curveWrapperById);
            return curveWrapperById.getAxisUiScalarsCallback();
          }
        }
        return Vector2.one;
      }
      Vector2 vector2_1 = new Vector2(-1f, -1f);
      if (this.selectedCurves.Count > 1)
      {
        bool flag1 = true;
        bool flag2 = true;
        Vector2 vector2_2 = Vector2.one;
        for (int index = 0; index < this.selectedCurves.Count; ++index)
        {
          CurveWrapper curveWrapper = this.selectedCurves[index].curveWrapper;
          if (curveWrapper.getAxisUiScalarsCallback != null)
          {
            Vector2 vector2_3 = curveWrapper.getAxisUiScalarsCallback();
            if (index == 0)
            {
              vector2_2 = vector2_3;
            }
            else
            {
              if ((double) Mathf.Abs(vector2_3.x - vector2_2.x) > 9.99999974737875E-06)
                flag1 = false;
              if ((double) Mathf.Abs(vector2_3.y - vector2_2.y) > 9.99999974737875E-06)
                flag2 = false;
              vector2_2 = vector2_3;
            }
            if (curvesWithSameParameterSpace != null)
              curvesWithSameParameterSpace.Add(curveWrapper);
          }
        }
        if (flag1)
          vector2_1.x = vector2_2.x;
        if (flag2)
          vector2_1.y = vector2_2.y;
      }
      return vector2_1;
    }

    private void SetAxisUiScalars(Vector2 newScalars, List<CurveWrapper> curvesInSameSpace)
    {
      using (List<CurveWrapper>.Enumerator enumerator = curvesInSameSpace.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          CurveWrapper current = enumerator.Current;
          Vector2 newAxisScalars = current.getAxisUiScalarsCallback();
          if ((double) newScalars.x >= 0.0)
            newAxisScalars.x = newScalars.x;
          if ((double) newScalars.y >= 0.0)
            newAxisScalars.y = newScalars.y;
          if (current.setAxisUiScalarsCallback != null)
            current.setAxisUiScalarsCallback(newAxisScalars);
        }
      }
    }

    public void GridGUI()
    {
      GUI.BeginGroup(this.drawRect);
      if (Event.current.type != EventType.Repaint)
      {
        GUI.EndGroup();
      }
      else
      {
        this.InitStyles();
        Color color = GUI.color;
        Vector2 axisUiScalars = this.GetAxisUiScalars((List<CurveWrapper>) null);
        Rect shownArea = this.shownArea;
        this.hTicks.SetRanges(shownArea.xMin * axisUiScalars.x, shownArea.xMax * axisUiScalars.x, this.drawRect.xMin, this.drawRect.xMax);
        this.vTicks.SetRanges(shownArea.yMin * axisUiScalars.y, shownArea.yMax * axisUiScalars.y, this.drawRect.yMin, this.drawRect.yMax);
        HandleUtility.ApplyWireMaterial();
        GL.Begin(1);
        this.hTicks.SetTickStrengths((float) this.settings.hTickStyle.distMin, (float) this.settings.hTickStyle.distFull, false);
        float y1;
        float y2;
        if (this.settings.hTickStyle.stubs)
        {
          y1 = shownArea.yMin;
          y2 = shownArea.yMin - 40f / this.scale.y;
        }
        else
        {
          y1 = Mathf.Max(shownArea.yMin, this.vRangeMin);
          y2 = Mathf.Min(shownArea.yMax, this.vRangeMax);
        }
        for (int level = 0; level < this.hTicks.tickLevels; ++level)
        {
          float strengthOfLevel = this.hTicks.GetStrengthOfLevel(level);
          if ((double) strengthOfLevel > 0.0)
          {
            GL.Color(this.settings.hTickStyle.color * new Color(1f, 1f, 1f, strengthOfLevel) * new Color(1f, 1f, 1f, 0.75f));
            float[] ticksAtLevel = this.hTicks.GetTicksAtLevel(level, true);
            for (int index = 0; index < ticksAtLevel.Length; ++index)
            {
              ticksAtLevel[index] /= axisUiScalars.x;
              if ((double) ticksAtLevel[index] > (double) this.hRangeMin && (double) ticksAtLevel[index] < (double) this.hRangeMax)
                this.DrawLine(new Vector2(ticksAtLevel[index], y1), new Vector2(ticksAtLevel[index], y2));
            }
          }
        }
        GL.Color(this.settings.hTickStyle.color * new Color(1f, 1f, 1f, 1f) * new Color(1f, 1f, 1f, 0.75f));
        if ((double) this.hRangeMin != double.NegativeInfinity)
          this.DrawLine(new Vector2(this.hRangeMin, y1), new Vector2(this.hRangeMin, y2));
        if ((double) this.hRangeMax != double.PositiveInfinity)
          this.DrawLine(new Vector2(this.hRangeMax, y1), new Vector2(this.hRangeMax, y2));
        this.vTicks.SetTickStrengths((float) this.settings.vTickStyle.distMin, (float) this.settings.vTickStyle.distFull, false);
        float x1;
        float x2;
        if (this.settings.vTickStyle.stubs)
        {
          x1 = shownArea.xMin;
          x2 = shownArea.xMin + 40f / this.scale.x;
        }
        else
        {
          x1 = Mathf.Max(shownArea.xMin, this.hRangeMin);
          x2 = Mathf.Min(shownArea.xMax, this.hRangeMax);
        }
        for (int level = 0; level < this.vTicks.tickLevels; ++level)
        {
          float strengthOfLevel = this.vTicks.GetStrengthOfLevel(level);
          if ((double) strengthOfLevel > 0.0)
          {
            GL.Color(this.settings.vTickStyle.color * new Color(1f, 1f, 1f, strengthOfLevel) * new Color(1f, 1f, 1f, 0.75f));
            float[] ticksAtLevel = this.vTicks.GetTicksAtLevel(level, true);
            for (int index = 0; index < ticksAtLevel.Length; ++index)
            {
              ticksAtLevel[index] /= axisUiScalars.y;
              if ((double) ticksAtLevel[index] > (double) this.vRangeMin && (double) ticksAtLevel[index] < (double) this.vRangeMax)
                this.DrawLine(new Vector2(x1, ticksAtLevel[index]), new Vector2(x2, ticksAtLevel[index]));
            }
          }
        }
        GL.Color(this.settings.vTickStyle.color * new Color(1f, 1f, 1f, 1f) * new Color(1f, 1f, 1f, 0.75f));
        if ((double) this.vRangeMin != double.NegativeInfinity)
          this.DrawLine(new Vector2(x1, this.vRangeMin), new Vector2(x2, this.vRangeMin));
        if ((double) this.vRangeMax != double.PositiveInfinity)
          this.DrawLine(new Vector2(x1, this.vRangeMax), new Vector2(x2, this.vRangeMax));
        GL.End();
        if (this.settings.showAxisLabels)
        {
          if (this.settings.hTickStyle.distLabel > 0 && (double) axisUiScalars.x > 0.0)
          {
            GUI.color = this.settings.hTickStyle.labelColor;
            int withMinSeparation = this.hTicks.GetLevelWithMinSeparation((float) this.settings.hTickStyle.distLabel);
            int minimumDifference = MathUtils.GetNumberOfDecimalsForMinimumDifference(this.hTicks.GetPeriodOfLevel(withMinSeparation));
            float[] ticksAtLevel = this.hTicks.GetTicksAtLevel(withMinSeparation, false);
            float[] numArray = (float[]) ticksAtLevel.Clone();
            float y3 = Mathf.Floor(this.drawRect.height);
            for (int index = 0; index < ticksAtLevel.Length; ++index)
            {
              numArray[index] /= axisUiScalars.x;
              if ((double) numArray[index] >= (double) this.hRangeMin && (double) numArray[index] <= (double) this.hRangeMax)
              {
                Vector2 vector2 = this.DrawingToViewTransformPoint(new Vector2(numArray[index], 0.0f));
                vector2 = new Vector2(Mathf.Floor(vector2.x), y3);
                float num = ticksAtLevel[index];
                TextAnchor textAnchor;
                Rect position;
                if (this.settings.hTickStyle.centerLabel)
                {
                  textAnchor = TextAnchor.UpperCenter;
                  position = new Rect(vector2.x, vector2.y - 16f - this.settings.hTickLabelOffset, 1f, 16f);
                }
                else
                {
                  textAnchor = TextAnchor.UpperLeft;
                  position = new Rect(vector2.x, vector2.y - 16f - this.settings.hTickLabelOffset, 50f, 16f);
                }
                if (this.ms_Styles.labelTickMarksX.alignment != textAnchor)
                  this.ms_Styles.labelTickMarksX.alignment = textAnchor;
                GUI.Label(position, num.ToString("n" + (object) minimumDifference) + this.settings.hTickStyle.unit, this.ms_Styles.labelTickMarksX);
              }
            }
          }
          if (this.settings.vTickStyle.distLabel > 0 && (double) axisUiScalars.y > 0.0)
          {
            GUI.color = this.settings.vTickStyle.labelColor;
            int withMinSeparation = this.vTicks.GetLevelWithMinSeparation((float) this.settings.vTickStyle.distLabel);
            float[] ticksAtLevel = this.vTicks.GetTicksAtLevel(withMinSeparation, false);
            float[] numArray = (float[]) ticksAtLevel.Clone();
            string format = "n" + (object) MathUtils.GetNumberOfDecimalsForMinimumDifference(this.vTicks.GetPeriodOfLevel(withMinSeparation));
            this.m_AxisLabelFormat = format;
            float width = 35f;
            if (!this.settings.vTickStyle.stubs && ticksAtLevel.Length > 1)
            {
              float num1 = ticksAtLevel[1];
              float num2 = ticksAtLevel[ticksAtLevel.Length - 1];
              width = Mathf.Max(this.ms_Styles.labelTickMarksY.CalcSize(new GUIContent(num1.ToString(format) + this.settings.vTickStyle.unit)).x, this.ms_Styles.labelTickMarksY.CalcSize(new GUIContent(num2.ToString(format) + this.settings.vTickStyle.unit)).x) + 6f;
            }
            for (int index = 0; index < ticksAtLevel.Length; ++index)
            {
              numArray[index] /= axisUiScalars.y;
              if ((double) numArray[index] >= (double) this.vRangeMin && (double) numArray[index] <= (double) this.vRangeMax)
              {
                Vector2 vector2 = this.DrawingToViewTransformPoint(new Vector2(0.0f, numArray[index]));
                vector2 = new Vector2(vector2.x, Mathf.Floor(vector2.y));
                float num = ticksAtLevel[index];
                GUI.Label(!this.settings.vTickStyle.centerLabel ? new Rect(0.0f, vector2.y - 13f, width, 16f) : new Rect(0.0f, vector2.y - 8f, this.leftmargin - 4f, 16f), num.ToString(format) + this.settings.vTickStyle.unit, this.ms_Styles.labelTickMarksY);
              }
            }
          }
        }
        GUI.color = color;
        GUI.EndGroup();
      }
    }

    private class SavedCurve
    {
      public int curveId;
      public List<CurveEditor.SavedCurve.SavedKeyFrame> keys;

      public class SavedKeyFrame : IComparable
      {
        public Keyframe key;
        public CurveWrapper.SelectionMode selected;

        public SavedKeyFrame(Keyframe key, CurveWrapper.SelectionMode selected)
        {
          this.key = key;
          this.selected = selected;
        }

        public int CompareTo(object _other)
        {
          float num = this.key.time - ((CurveEditor.SavedCurve.SavedKeyFrame) _other).key.time;
          if ((double) num < 0.0)
            return -1;
          return (double) num > 0.0 ? 1 : 0;
        }
      }
    }

    private enum AxisLock
    {
      None,
      X,
      Y,
    }

    private struct KeyFrameCopy
    {
      public float time;
      public float value;
      public float inTangent;
      public float outTangent;
      public int idx;
      public int selectionIdx;

      public KeyFrameCopy(int idx, int selectionIdx, Keyframe source)
      {
        this.idx = idx;
        this.selectionIdx = selectionIdx;
        this.time = source.time;
        this.value = source.value;
        this.inTangent = source.inTangent;
        this.outTangent = source.outTangent;
      }
    }

    internal class Styles
    {
      public Texture2D pointIcon = EditorGUIUtility.LoadIcon("curvekeyframe");
      public Texture2D pointIconSelected = EditorGUIUtility.LoadIcon("curvekeyframeselected");
      public Texture2D pointIconSelectedOverlay = EditorGUIUtility.LoadIcon("curvekeyframeselectedoverlay");
      public Texture2D pointIconSemiSelectedOverlay = EditorGUIUtility.LoadIcon("curvekeyframesemiselectedoverlay");
      public GUIStyle none = new GUIStyle();
      public GUIStyle labelTickMarksY = (GUIStyle) "CurveEditorLabelTickMarks";
      public GUIStyle selectionRect = (GUIStyle) "SelectionRect";
      public GUIStyle dragLabel = (GUIStyle) "ProfilerBadge";
      public GUIStyle axisLabelNumberField = new GUIStyle(EditorStyles.miniTextField);
      public GUIStyle labelTickMarksX;

      public Styles()
      {
        this.axisLabelNumberField.alignment = TextAnchor.UpperRight;
        this.labelTickMarksY.contentOffset = Vector2.zero;
        this.labelTickMarksX = new GUIStyle(this.labelTickMarksY);
        this.labelTickMarksX.clipping = TextClipping.Overflow;
      }
    }

    internal enum PickMode
    {
      None,
      Click,
      Marquee,
    }

    public delegate void CallbackFunction();
  }
}

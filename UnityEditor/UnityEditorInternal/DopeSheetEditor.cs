// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.DopeSheetEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  [Serializable]
  internal class DopeSheetEditor : TimeArea, CurveUpdater
  {
    public Bounds m_Bounds = new Bounds(Vector3.zero, Vector3.zero);
    private const float k_KeyframeOffset = -5.5f;
    private const float k_PptrKeyframeOffset = -1f;
    public AnimationWindowState state;
    [SerializeField]
    public EditorWindow m_Owner;
    private DopeSheetEditor.DopeSheetSelectionRect m_SelectionRect;
    private Texture m_DefaultDopeKeyIcon;
    private float m_DragStartTime;
    private bool m_MousedownOnKeyframe;
    private bool m_IsDragging;
    private bool m_IsDraggingPlayheadStarted;
    private bool m_IsDraggingPlayhead;
    private bool m_Initialized;
    private List<DopeSheetEditor.DrawElement> selectedKeysDrawBuffer;
    private List<DopeSheetEditor.DrawElement> unselectedKeysDrawBuffer;
    private List<DopeSheetEditor.DrawElement> dragdropKeysDrawBuffer;
    public bool m_SpritePreviewLoading;
    public int m_SpritePreviewCacheSize;

    public float contentHeight
    {
      get
      {
        float num = 0.0f;
        using (List<DopeLine>.Enumerator enumerator = this.state.dopelines.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            DopeLine current = enumerator.Current;
            num += !current.tallMode ? 16f : 32f;
          }
        }
        return num + 40f;
      }
    }

    public override Bounds drawingBounds
    {
      get
      {
        return this.m_Bounds;
      }
    }

    internal int assetPreviewManagerID
    {
      get
      {
        if ((UnityEngine.Object) this.m_Owner != (UnityEngine.Object) null)
          return this.m_Owner.GetInstanceID();
        return 0;
      }
    }

    public DopeSheetEditor(EditorWindow owner)
      : base(false)
    {
      this.m_Owner = owner;
    }

    internal void OnDestroy()
    {
      AssetPreview.DeletePreviewTextureManagerByID(this.assetPreviewManagerID);
    }

    public void OnGUI(Rect position, Vector2 scrollPosition)
    {
      this.Init();
      EditorGUI.BeginDisabledGroup(!this.state.clipIsEditable);
      this.HandleDragAndDropToEmptyArea();
      EditorGUI.EndDisabledGroup();
      EditorGUI.BeginDisabledGroup(this.state.animationIsReadOnly);
      GUIClip.Push(position, scrollPosition, Vector2.zero, false);
      Rect rect = this.DopelinesGUI(new Rect(0.0f, 0.0f, position.width, position.height), scrollPosition);
      if (GUI.enabled)
      {
        this.HandleKeyboard();
        this.HandleDragging();
        this.HandleSelectionRect(rect);
        this.HandleDelete();
      }
      GUIClip.Pop();
      EditorGUI.EndDisabledGroup();
    }

    public void Init()
    {
      if (!this.m_Initialized)
      {
        if ((UnityEngine.Object) this.m_DefaultDopeKeyIcon == (UnityEngine.Object) null)
          this.m_DefaultDopeKeyIcon = (Texture) EditorGUIUtility.LoadIcon("blendKey");
        this.hSlider = true;
        this.vSlider = false;
        this.hRangeLocked = false;
        this.vRangeLocked = true;
        this.hRangeMin = 0.0f;
        this.margin = 40f;
        this.scaleWithWindow = true;
        this.ignoreScrollWheelUntilClicked = false;
      }
      this.m_Initialized = true;
    }

    public void RecalculateBounds()
    {
      if (!(bool) ((UnityEngine.Object) this.state.activeAnimationClip))
        return;
      this.m_Bounds.SetMinMax(new Vector3(this.state.activeAnimationClip.startTime, 0.0f, 0.0f), new Vector3(this.state.activeAnimationClip.stopTime, 0.0f, 0.0f));
    }

    private Rect DopelinesGUI(Rect position, Vector2 scrollPosition)
    {
      Color color = GUI.color;
      Rect rect1 = position;
      this.selectedKeysDrawBuffer = new List<DopeSheetEditor.DrawElement>();
      this.unselectedKeysDrawBuffer = new List<DopeSheetEditor.DrawElement>();
      this.dragdropKeysDrawBuffer = new List<DopeSheetEditor.DrawElement>();
      if (Event.current.type == EventType.Repaint)
        this.m_SpritePreviewLoading = false;
      if (Event.current.type == EventType.MouseDown)
        this.m_IsDragging = false;
      this.UpdateSpritePreviewCacheSize();
      using (List<DopeLine>.Enumerator enumerator = this.state.dopelines.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          DopeLine current1 = enumerator.Current;
          current1.position = rect1;
          current1.position.height = !current1.tallMode ? 16f : 32f;
          if ((double) current1.position.yMin + (double) scrollPosition.y >= (double) position.yMin && (double) current1.position.yMin + (double) scrollPosition.y <= (double) position.yMax || (double) current1.position.yMax + (double) scrollPosition.y >= (double) position.yMin && (double) current1.position.yMax + (double) scrollPosition.y <= (double) position.yMax)
          {
            Event current2 = Event.current;
            EventType type = current2.type;
            switch (type)
            {
              case EventType.Repaint:
                this.DopeLineRepaint(current1);
                break;
              case EventType.DragUpdated:
              case EventType.DragPerform:
                if (this.state.clipIsEditable)
                {
                  this.HandleDragAndDrop(current1);
                  break;
                }
                break;
              default:
                if (type != EventType.MouseDown)
                {
                  if (type == EventType.ContextClick && !this.m_IsDraggingPlayhead)
                  {
                    this.HandleContextMenu(current1);
                    break;
                  }
                  break;
                }
                if (current2.button == 0)
                {
                  this.HandleMouseDown(current1);
                  break;
                }
                break;
            }
          }
          rect1.y += current1.position.height;
        }
      }
      if (Event.current.type == EventType.MouseUp)
      {
        this.m_IsDraggingPlayheadStarted = false;
        this.m_IsDraggingPlayhead = false;
      }
      Rect rect2 = new Rect(position.xMin, position.yMin, position.width, rect1.yMax - position.yMin);
      this.DrawElements(this.unselectedKeysDrawBuffer);
      this.DrawElements(this.selectedKeysDrawBuffer);
      this.DrawElements(this.dragdropKeysDrawBuffer);
      GUI.color = color;
      return rect2;
    }

    private void DrawGrid(Rect position)
    {
      this.TimeRuler(position, this.state.frameRate, false, true, 0.2f);
    }

    public void DrawMasterDopelineBackground(Rect position)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      AnimationWindowStyles.eventBackground.Draw(position, false, false, false, false);
    }

    private void UpdateSpritePreviewCacheSize()
    {
      int num = 1;
      using (List<DopeLine>.Enumerator enumerator = this.state.dopelines.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          DopeLine current = enumerator.Current;
          if (current.tallMode && current.isPptrDopeline)
            num += current.keys.Count;
        }
      }
      int size = num + DragAndDrop.objectReferences.Length;
      if (size <= this.m_SpritePreviewCacheSize)
        return;
      AssetPreview.SetPreviewTextureCacheSize(size, this.assetPreviewManagerID);
      this.m_SpritePreviewCacheSize = size;
    }

    private void DrawElements(List<DopeSheetEditor.DrawElement> elements)
    {
      Color color1 = GUI.color;
      Color color2 = Color.white;
      GUI.color = color2;
      Texture defaultDopeKeyIcon = this.m_DefaultDopeKeyIcon;
      using (List<DopeSheetEditor.DrawElement>.Enumerator enumerator = elements.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          DopeSheetEditor.DrawElement current = enumerator.Current;
          if (current.color != color2)
          {
            color2 = !GUI.enabled ? current.color * 0.8f : current.color;
            GUI.color = color2;
          }
          if ((UnityEngine.Object) current.texture != (UnityEngine.Object) null)
            GUI.DrawTexture(current.position, (Texture) current.texture);
          else
            GUI.DrawTexture(new Rect(current.position.center.x - (float) (defaultDopeKeyIcon.width / 2), current.position.center.y - (float) (defaultDopeKeyIcon.height / 2), (float) defaultDopeKeyIcon.width, (float) defaultDopeKeyIcon.height), defaultDopeKeyIcon, ScaleMode.ScaleToFit, true, 1f);
        }
      }
      GUI.color = color1;
    }

    private void DopeLineRepaint(DopeLine dopeline)
    {
      Color color1 = GUI.color;
      AnimationWindowHierarchyNode windowHierarchyNode = (AnimationWindowHierarchyNode) this.state.hierarchyData.FindItem(dopeline.m_HierarchyNodeID);
      Color color2 = windowHierarchyNode == null || windowHierarchyNode.depth <= 0 ? Color.gray.AlphaMultiplied(0.16f) : Color.gray.AlphaMultiplied(0.05f);
      if (!dopeline.isMasterDopeline)
        DopeSheetEditor.DrawBox(dopeline.position, color2);
      int? nullable = new int?();
      int count = dopeline.keys.Count;
      for (int keyIndex = 0; keyIndex < count; ++keyIndex)
      {
        AnimationWindowKeyframe key = dopeline.keys[keyIndex];
        if ((nullable.GetValueOrDefault() != key.m_TimeHash ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
        {
          nullable = new int?(key.m_TimeHash);
          Rect rect = this.GetKeyframeRect(dopeline, key);
          Color color3 = !dopeline.isMasterDopeline ? Color.gray.RGBMultiplied(1.2f) : Color.gray.RGBMultiplied(0.85f);
          Texture2D texture = (Texture2D) null;
          if (key.isPPtrCurve && dopeline.tallMode)
            texture = key.value != null ? AssetPreview.GetAssetPreview(((UnityEngine.Object) key.value).GetInstanceID(), this.assetPreviewManagerID) : (Texture2D) null;
          if ((UnityEngine.Object) texture != (UnityEngine.Object) null)
          {
            rect = this.GetPreviewRectFromKeyFrameRect(rect);
            color3 = Color.white.AlphaMultiplied(0.5f);
          }
          else if (key.value != null && key.isPPtrCurve && dopeline.tallMode)
            this.m_SpritePreviewLoading = true;
          if (Mathf.Approximately(key.time, 0.0f))
            rect.xMin -= 0.01f;
          if (this.AnyKeyIsSelectedAtTime(dopeline, keyIndex))
          {
            Color color4 = !dopeline.tallMode || !dopeline.isPptrDopeline ? new Color(0.34f, 0.52f, 0.85f, 1f) : Color.white;
            if (dopeline.isMasterDopeline)
              color4 = color4.RGBMultiplied(0.85f);
            this.selectedKeysDrawBuffer.Add(new DopeSheetEditor.DrawElement(rect, color4, texture));
          }
          else
            this.unselectedKeysDrawBuffer.Add(new DopeSheetEditor.DrawElement(rect, color3, texture));
        }
      }
      if (this.state.clipIsEditable && this.DoDragAndDrop(dopeline, dopeline.position, false))
      {
        float time = Mathf.Max(this.state.PixelToTime(Event.current.mousePosition.x), 0.0f);
        Color color3 = Color.gray.RGBMultiplied(1.2f);
        Texture2D texture = (Texture2D) null;
        foreach (UnityEngine.Object dropObjectReference in this.GetSortedDragAndDropObjectReferences())
        {
          Rect rect = this.GetDragAndDropRect(dopeline, this.state.TimeToPixel(time));
          if (dopeline.isPptrDopeline && dopeline.tallMode)
            texture = AssetPreview.GetAssetPreview(dropObjectReference.GetInstanceID(), this.assetPreviewManagerID);
          if ((UnityEngine.Object) texture != (UnityEngine.Object) null)
          {
            rect = this.GetPreviewRectFromKeyFrameRect(rect);
            color3 = Color.white.AlphaMultiplied(0.5f);
          }
          this.dragdropKeysDrawBuffer.Add(new DopeSheetEditor.DrawElement(rect, color3, texture));
          time += 1f / this.state.frameRate;
        }
      }
      GUI.color = color1;
    }

    private Rect GetPreviewRectFromKeyFrameRect(Rect keyframeRect)
    {
      keyframeRect.width -= 2f;
      keyframeRect.height -= 2f;
      keyframeRect.xMin += 2f;
      keyframeRect.yMin += 2f;
      return keyframeRect;
    }

    private Rect GetDragAndDropRect(DopeLine dopeline, float screenX)
    {
      Rect keyframeRect = this.GetKeyframeRect(dopeline, (AnimationWindowKeyframe) null);
      float keyframeOffset = this.GetKeyframeOffset(dopeline, (AnimationWindowKeyframe) null);
      float time = Mathf.Max(this.state.PixelToTime(screenX - keyframeRect.width * 0.5f, true), 0.0f);
      keyframeRect.center = new Vector2(this.state.TimeToPixel(time) + keyframeRect.width * 0.5f + keyframeOffset, keyframeRect.center.y);
      return keyframeRect;
    }

    private static void DrawBox(Rect position, Color color)
    {
      Color color1 = GUI.color;
      GUI.color = color;
      DopeLine.dopekeyStyle.Draw(position, GUIContent.none, 0, false);
      GUI.color = color1;
    }

    private GenericMenu GenerateMenu(DopeLine dopeline, bool clickedEmpty)
    {
      GenericMenu menu = new GenericMenu();
      this.state.recording = true;
      this.state.ResampleAnimation();
      string text1 = "Add Key";
      if (clickedEmpty)
        menu.AddItem(new GUIContent(text1), false, new GenericMenu.MenuFunction2(this.AddKeyToDopeline), (object) dopeline);
      else
        menu.AddDisabledItem(new GUIContent(text1));
      string text2 = this.state.selectedKeys.Count <= 1 ? "Delete Key" : "Delete Keys";
      if (this.state.selectedKeys.Count > 0)
        menu.AddItem(new GUIContent(text2), false, new GenericMenu.MenuFunction(this.DeleteSelectedKeys));
      else
        menu.AddDisabledItem(new GUIContent(text2));
      if (AnimationWindowUtility.ContainsFloatKeyframes(this.state.selectedKeys))
      {
        menu.AddSeparator(string.Empty);
        List<KeyIdentifier> keyList = new List<KeyIdentifier>();
        using (List<AnimationWindowKeyframe>.Enumerator enumerator = this.state.selectedKeys.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnimationWindowKeyframe current = enumerator.Current;
            if (!current.isPPtrCurve)
            {
              int keyframeIndex = current.curve.GetKeyframeIndex(AnimationKeyTime.Time(current.time, this.state.frameRate));
              if (keyframeIndex != -1)
              {
                CurveRenderer curveRenderer = CurveRendererCache.GetCurveRenderer(this.state.activeAnimationClip, current.curve.binding);
                int curveId = CurveUtility.GetCurveID(this.state.activeAnimationClip, current.curve.binding);
                keyList.Add(new KeyIdentifier(curveRenderer, curveId, keyframeIndex, current.curve.binding));
              }
            }
          }
        }
        new CurveMenuManager((CurveUpdater) this).AddTangentMenuItems(menu, keyList);
      }
      return menu;
    }

    private void HandleDragging()
    {
      int controlId = GUIUtility.GetControlID("dopesheetdrag".GetHashCode(), FocusType.Passive, new Rect());
      if ((Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseUp) && this.m_MousedownOnKeyframe)
      {
        if (Event.current.type == EventType.MouseDrag && !EditorGUI.actionKey && (!Event.current.shift && !this.m_IsDragging) && this.state.selectedKeys.Count > 0)
        {
          this.m_IsDragging = true;
          this.m_IsDraggingPlayheadStarted = true;
          GUIUtility.hotControl = controlId;
          this.m_DragStartTime = this.state.PixelToTime(Event.current.mousePosition.x);
          Event.current.Use();
        }
        float b = float.MaxValue;
        using (List<AnimationWindowKeyframe>.Enumerator enumerator = this.state.selectedKeys.GetEnumerator())
        {
          while (enumerator.MoveNext())
            b = Mathf.Min(enumerator.Current.time, b);
        }
        float frame = this.state.SnapToFrame(this.state.PixelToTime(Event.current.mousePosition.x));
        float deltaTime = Mathf.Max(frame - this.m_DragStartTime, b * -1f);
        if (this.m_IsDragging && !Mathf.Approximately(frame, this.m_DragStartTime))
        {
          this.m_DragStartTime = frame;
          this.state.MoveSelectedKeys(deltaTime, true, false);
          if (this.state.activeKeyframe != null && !this.state.playing)
            this.state.frame = this.state.TimeToFrameFloor(this.state.activeKeyframe.time);
          Event.current.Use();
        }
        if (Event.current.type == EventType.MouseUp)
        {
          if (this.m_IsDragging && GUIUtility.hotControl == controlId)
          {
            this.state.MoveSelectedKeys(deltaTime, true, true);
            Event.current.Use();
            this.m_IsDragging = false;
          }
          this.m_MousedownOnKeyframe = false;
          GUIUtility.hotControl = 0;
        }
      }
      if (!this.m_IsDraggingPlayheadStarted || Event.current.type != EventType.MouseDrag || Event.current.button != 1)
        return;
      this.m_IsDraggingPlayhead = true;
      Event.current.Use();
    }

    private void HandleKeyboard()
    {
      if (Event.current.type != EventType.ValidateCommand && Event.current.type != EventType.ExecuteCommand)
        return;
      string commandName = Event.current.commandName;
      if (commandName == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (DopeSheetEditor.\u003C\u003Ef__switch\u0024mapD == null)
      {
        // ISSUE: reference to a compiler-generated field
        DopeSheetEditor.\u003C\u003Ef__switch\u0024mapD = new Dictionary<string, int>(2)
        {
          {
            "SelectAll",
            0
          },
          {
            "FrameSelected",
            1
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!DopeSheetEditor.\u003C\u003Ef__switch\u0024mapD.TryGetValue(commandName, out num))
        return;
      if (num != 0)
      {
        if (num != 1)
          return;
        if (Event.current.type == EventType.ExecuteCommand)
          this.FrameSelected();
        Event.current.Use();
      }
      else
      {
        if (Event.current.type == EventType.ExecuteCommand)
          this.HandleSelectAll();
        Event.current.Use();
      }
    }

    private void HandleSelectAll()
    {
      using (List<DopeLine>.Enumerator enumerator1 = this.state.dopelines.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          DopeLine current = enumerator1.Current;
          using (List<AnimationWindowKeyframe>.Enumerator enumerator2 = current.keys.GetEnumerator())
          {
            while (enumerator2.MoveNext())
              this.state.SelectKey(enumerator2.Current);
          }
          this.state.SelectHierarchyItem(current, true, false);
        }
      }
    }

    private void HandleDelete()
    {
      switch (Event.current.type)
      {
        case EventType.ValidateCommand:
        case EventType.ExecuteCommand:
          if (!(Event.current.commandName == "SoftDelete") && !(Event.current.commandName == "Delete"))
            break;
          if (Event.current.type == EventType.ExecuteCommand)
            this.state.DeleteSelectedKeys();
          Event.current.Use();
          break;
        case EventType.KeyDown:
          if (Event.current.keyCode != KeyCode.Backspace && Event.current.keyCode != KeyCode.Delete)
            break;
          this.state.DeleteSelectedKeys();
          Event.current.Use();
          break;
      }
    }

    private void HandleSelectionRect(Rect rect)
    {
      if (this.m_SelectionRect == null)
        this.m_SelectionRect = new DopeSheetEditor.DopeSheetSelectionRect(this);
      if (this.m_MousedownOnKeyframe)
        return;
      this.m_SelectionRect.OnGUI(rect);
    }

    private void HandleDragAndDropToEmptyArea()
    {
      Event current = Event.current;
      if (current.type != EventType.DragPerform && current.type != EventType.DragUpdated || (!DopeSheetEditor.ValidateDragAndDropObjects() || (UnityEngine.Object) this.state.activeGameObject == (UnityEngine.Object) null))
        return;
      if (DragAndDrop.objectReferences[0].GetType() == typeof (Sprite) || DragAndDrop.objectReferences[0].GetType() == typeof (Texture2D))
      {
        if (this.DopelineForValueTypeExists(typeof (Sprite)))
          return;
        if (current.type == EventType.DragPerform)
        {
          EditorCurveBinding? newPptrDopeline = this.CreateNewPptrDopeline(typeof (Sprite));
          if (newPptrDopeline.HasValue)
            this.DoSpriteDropAfterGeneratingNewDopeline(newPptrDopeline);
        }
        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        current.Use();
      }
      else
        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
    }

    private void DoSpriteDropAfterGeneratingNewDopeline(EditorCurveBinding? spriteBinding)
    {
      if (DragAndDrop.objectReferences.Length == 1)
        Analytics.Event("Sprite Drag and Drop", "Drop single sprite into empty dopesheet", "null", 1);
      else
        Analytics.Event("Sprite Drag and Drop", "Drop multiple sprites into empty dopesheet", "null", 1);
      AnimationWindowCurve animationWindowCurve = new AnimationWindowCurve(this.state.activeAnimationClip, spriteBinding.Value, typeof (Sprite));
      this.state.SaveCurve(animationWindowCurve);
      this.PeformDragAndDrop(animationWindowCurve, 0.0f);
    }

    private bool DopelineForValueTypeExists(System.Type valueType)
    {
      string transformPath = AnimationUtility.CalculateTransformPath(this.state.activeGameObject.transform, this.state.activeRootGameObject.transform);
      using (List<DopeLine>.Enumerator enumerator = this.state.dopelines.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          DopeLine current = enumerator.Current;
          if (current.valueType == valueType)
          {
            AnimationWindowHierarchyNode windowHierarchyNode = (AnimationWindowHierarchyNode) this.state.hierarchyData.FindItem(current.m_HierarchyNodeID);
            if (windowHierarchyNode != null && windowHierarchyNode.path.Equals(transformPath))
            {
              DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
              return true;
            }
          }
        }
      }
      return false;
    }

    private EditorCurveBinding? CreateNewPptrDopeline(System.Type valueType)
    {
      List<EditorCurveBinding> animatableProperties = AnimationWindowUtility.GetAnimatableProperties(this.state.activeRootGameObject, this.state.activeRootGameObject, valueType);
      if (animatableProperties.Count == 0 && valueType == typeof (Sprite))
        return this.CreateNewSpriteRendererDopeline();
      if (animatableProperties.Count == 1)
        return new EditorCurveBinding?(animatableProperties[0]);
      List<string> stringList = new List<string>();
      using (List<EditorCurveBinding>.Enumerator enumerator = animatableProperties.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          EditorCurveBinding current = enumerator.Current;
          stringList.Add(current.type.Name);
        }
      }
      EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f), EditorGUIUtility.TempContent(stringList.ToArray()), -1, new EditorUtility.SelectMenuItemFunction(this.SelectTypeForCreatingNewPptrDopeline), (object) animatableProperties);
      return new EditorCurveBinding?();
    }

    private void SelectTypeForCreatingNewPptrDopeline(object userData, string[] options, int selected)
    {
      EditorCurveBinding[] editorCurveBindingArray = userData as EditorCurveBinding[];
      if (editorCurveBindingArray.Length <= selected)
        return;
      this.DoSpriteDropAfterGeneratingNewDopeline(new EditorCurveBinding?(editorCurveBindingArray[selected]));
    }

    private EditorCurveBinding? CreateNewSpriteRendererDopeline()
    {
      if (!(bool) ((UnityEngine.Object) this.state.activeGameObject.GetComponent<SpriteRenderer>()))
        this.state.activeGameObject.AddComponent<SpriteRenderer>();
      List<EditorCurveBinding> animatableProperties = AnimationWindowUtility.GetAnimatableProperties(this.state.activeRootGameObject, this.state.activeRootGameObject, typeof (SpriteRenderer), typeof (Sprite));
      if (animatableProperties.Count == 1)
        return new EditorCurveBinding?(animatableProperties[0]);
      Debug.LogError((object) "Unable to create animatable SpriteRenderer component");
      return new EditorCurveBinding?();
    }

    private void HandleDragAndDrop(DopeLine dopeline)
    {
      Event current = Event.current;
      if (current.type != EventType.DragPerform && current.type != EventType.DragUpdated)
        return;
      if (this.DoDragAndDrop(dopeline, dopeline.position, current.type == EventType.DragPerform))
      {
        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        current.Use();
      }
      else
        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
    }

    private void HandleMouseDown(DopeLine dopeline)
    {
      Event current1 = Event.current;
      if (!EditorGUI.actionKey && !current1.shift)
      {
        using (List<AnimationWindowKeyframe>.Enumerator enumerator = dopeline.keys.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnimationWindowKeyframe current2 = enumerator.Current;
            if (this.GetKeyframeRect(dopeline, current2).Contains(current1.mousePosition) && !this.state.KeyIsSelected(current2))
            {
              this.state.ClearSelections();
              break;
            }
          }
        }
      }
      float time = this.state.PixelToTime(Event.current.mousePosition.x);
      float num = time;
      if (Event.current.shift)
      {
        using (List<AnimationWindowKeyframe>.Enumerator enumerator = dopeline.keys.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnimationWindowKeyframe current2 = enumerator.Current;
            if (this.state.KeyIsSelected(current2))
            {
              if ((double) current2.time < (double) time)
                time = current2.time;
              if ((double) current2.time > (double) num)
                num = current2.time;
            }
          }
        }
      }
      bool flag = false;
      using (List<AnimationWindowKeyframe>.Enumerator enumerator1 = dopeline.keys.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          AnimationWindowKeyframe current2 = enumerator1.Current;
          if (this.GetKeyframeRect(dopeline, current2).Contains(current1.mousePosition))
          {
            flag = true;
            if (!this.state.KeyIsSelected(current2))
            {
              if (Event.current.shift)
              {
                using (List<AnimationWindowKeyframe>.Enumerator enumerator2 = dopeline.keys.GetEnumerator())
                {
                  while (enumerator2.MoveNext())
                  {
                    AnimationWindowKeyframe current3 = enumerator2.Current;
                    if (current3 == current2 || (double) current3.time > (double) time && (double) current3.time < (double) num)
                      this.state.SelectKey(current3);
                  }
                }
              }
              else
                this.state.SelectKey(current2);
              if (!dopeline.isMasterDopeline)
                this.state.SelectHierarchyItem(dopeline, EditorGUI.actionKey || current1.shift);
            }
            else if (EditorGUI.actionKey)
            {
              this.state.UnselectKey(current2);
              if (!this.state.AnyKeyIsSelected(dopeline))
                this.state.UnSelectHierarchyItem(dopeline);
            }
            this.state.activeKeyframe = current2;
            this.m_MousedownOnKeyframe = true;
            if (!this.state.playing)
              this.state.frame = this.state.TimeToFrameFloor(this.state.activeKeyframe.time);
            current1.Use();
          }
        }
      }
      if (dopeline.isMasterDopeline)
      {
        this.state.ClearHierarchySelection();
        using (List<int>.Enumerator enumerator = this.state.GetAffectedHierarchyIDs(this.state.selectedKeys).GetEnumerator())
        {
          while (enumerator.MoveNext())
            this.state.SelectHierarchyItem(enumerator.Current, true, true);
        }
      }
      if (!dopeline.position.Contains(Event.current.mousePosition))
        return;
      if (current1.clickCount == 2 && current1.button == 0 && (!Event.current.shift && !EditorGUI.actionKey))
        this.HandleDopelineDoubleclick(dopeline);
      if (current1.button != 1 || this.state.playing)
        return;
      this.state.frame = AnimationKeyTime.Time(this.state.PixelToTime(Event.current.mousePosition.x, true), this.state.frameRate).frame;
      if (flag)
        return;
      this.state.ClearSelections();
      this.m_IsDraggingPlayheadStarted = true;
      HandleUtility.Repaint();
      current1.Use();
    }

    private void HandleDopelineDoubleclick(DopeLine dopeline)
    {
      this.state.ClearSelections();
      AnimationKeyTime time = AnimationKeyTime.Time(this.state.PixelToTime(Event.current.mousePosition.x, true), this.state.frameRate);
      foreach (AnimationWindowCurve curve in dopeline.m_Curves)
        this.state.SelectKey(AnimationWindowUtility.AddKeyframeToCurve(this.state, curve, time));
      if (!this.state.playing)
        this.state.frame = time.frame;
      Event.current.Use();
    }

    private void HandleContextMenu(DopeLine dopeline)
    {
      if (!dopeline.position.Contains(Event.current.mousePosition))
        return;
      bool clickedEmpty = true;
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = dopeline.keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowKeyframe current = enumerator.Current;
          if (this.GetKeyframeRect(dopeline, current).Contains(Event.current.mousePosition))
          {
            clickedEmpty = false;
            break;
          }
        }
      }
      this.GenerateMenu(dopeline, clickedEmpty).ShowAsContext();
    }

    private Rect GetKeyframeRect(DopeLine dopeline, AnimationWindowKeyframe keyframe)
    {
      float time = keyframe == null ? 0.0f : keyframe.time;
      float width = 10f;
      if (dopeline.isPptrDopeline && dopeline.tallMode && (keyframe == null || keyframe.value != null))
        width = dopeline.position.height;
      if (dopeline.isPptrDopeline && dopeline.tallMode)
        return new Rect(this.state.TimeToPixel(this.state.SnapToFrame(time)) + this.GetKeyframeOffset(dopeline, keyframe), dopeline.position.yMin, width, dopeline.position.height);
      return new Rect(this.state.TimeToPixel(this.state.SnapToFrame(time)) + this.GetKeyframeOffset(dopeline, keyframe), dopeline.position.yMin, width, dopeline.position.height);
    }

    private float GetKeyframeOffset(DopeLine dopeline, AnimationWindowKeyframe keyframe)
    {
      return dopeline.isPptrDopeline && dopeline.tallMode && (keyframe == null || keyframe.value != null) ? -1f : -5.5f;
    }

    public void FrameClip()
    {
      if (!(bool) ((UnityEngine.Object) this.state.activeAnimationClip))
        return;
      this.SetShownHRangeInsideMargins(0.0f, Mathf.Max(this.state.activeAnimationClip.length, 1f));
    }

    public void FrameSelected()
    {
      float num1 = float.MaxValue;
      float num2 = float.MinValue;
      bool flag1 = this.state.selectedKeys.Count > 0;
      if (flag1)
      {
        using (List<AnimationWindowKeyframe>.Enumerator enumerator = this.state.selectedKeys.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnimationWindowKeyframe current = enumerator.Current;
            num1 = Mathf.Min(current.time, num1);
            num2 = Mathf.Max(current.time, num2);
          }
        }
      }
      bool flag2 = !flag1;
      if (!flag1 && this.state.hierarchyState.selectedIDs.Count > 0)
      {
        using (List<AnimationWindowCurve>.Enumerator enumerator = this.state.activeCurves.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnimationWindowCurve current = enumerator.Current;
            int count = current.m_Keyframes.Count;
            if (count > 1)
            {
              num1 = Mathf.Min(current.m_Keyframes[0].time, num1);
              num2 = Mathf.Max(current.m_Keyframes[count - 1].time, num2);
              flag2 = false;
            }
          }
        }
      }
      if (flag2)
      {
        this.FrameClip();
      }
      else
      {
        float num3 = Mathf.Min(this.state.FrameToTime(Mathf.Min(4f, this.state.frameRate)), Mathf.Max(this.state.activeAnimationClip.length, this.state.FrameToTime(4f)));
        float max = Mathf.Max(num2, num1 + num3);
        this.SetShownHRangeInsideMargins(num1, max);
      }
    }

    private bool DoDragAndDrop(DopeLine dopeLine, Rect position, bool perform)
    {
      return this.DoDragAndDrop(dopeLine, position, false, perform);
    }

    private bool DoDragAndDrop(DopeLine dopeLine, bool perform)
    {
      return this.DoDragAndDrop(dopeLine, new Rect(), true, perform);
    }

    private bool DoDragAndDrop(DopeLine dopeLine, Rect position, bool ignoreMousePosition, bool perform)
    {
      if (!ignoreMousePosition && !position.Contains(Event.current.mousePosition) || !DopeSheetEditor.ValidateDragAndDropObjects())
        return false;
      System.Type type = DragAndDrop.objectReferences[0].GetType();
      AnimationWindowCurve animationWindowCurve = (AnimationWindowCurve) null;
      if (dopeLine.valueType == type)
      {
        animationWindowCurve = dopeLine.m_Curves[0];
      }
      else
      {
        foreach (AnimationWindowCurve curve in dopeLine.m_Curves)
        {
          if (curve.isPPtrCurve)
          {
            if (curve.m_ValueType == type)
              animationWindowCurve = curve;
            Sprite[] draggedPathsOrObjects = SpriteUtility.GetSpriteFromDraggedPathsOrObjects();
            if (curve.m_ValueType == typeof (Sprite) && draggedPathsOrObjects != null && draggedPathsOrObjects.Length > 0)
            {
              animationWindowCurve = curve;
              type = typeof (Sprite);
            }
          }
        }
      }
      bool flag = true;
      if (animationWindowCurve != null)
      {
        if (perform)
        {
          if (DragAndDrop.objectReferences.Length == 1)
            Analytics.Event("Sprite Drag and Drop", "Drop single sprite into existing dopeline", "null", 1);
          else
            Analytics.Event("Sprite Drag and Drop", "Drop multiple sprites into existing dopeline", "null", 1);
          float time = Mathf.Max(this.state.PixelToTime(this.GetDragAndDropRect(dopeLine, Event.current.mousePosition.x).xMin, true), 0.0f);
          this.PeformDragAndDrop(this.GetCurveOfType(dopeLine, type), time);
        }
      }
      else
        flag = false;
      return flag;
    }

    private void PeformDragAndDrop(AnimationWindowCurve targetCurve, float time)
    {
      if (DragAndDrop.objectReferences.Length == 0 || targetCurve == null)
        return;
      this.state.ClearSelections();
      foreach (UnityEngine.Object dropObjectReference in this.GetSortedDragAndDropObjectReferences())
      {
        UnityEngine.Object @object = dropObjectReference;
        if (@object is Texture2D)
          @object = (UnityEngine.Object) SpriteUtility.TextureToSprite(dropObjectReference as Texture2D);
        this.CreateNewPPtrKeyframe(time, @object, targetCurve);
        time += 1f / this.state.activeAnimationClip.frameRate;
      }
      this.state.SaveCurve(targetCurve);
      DragAndDrop.AcceptDrag();
    }

    private UnityEngine.Object[] GetSortedDragAndDropObjectReferences()
    {
      UnityEngine.Object[] objectReferences = DragAndDrop.objectReferences;
      Array.Sort<UnityEngine.Object>(objectReferences, (Comparison<UnityEngine.Object>) ((a, b) => EditorUtility.NaturalCompare(a.name, b.name)));
      return objectReferences;
    }

    private void CreateNewPPtrKeyframe(float time, UnityEngine.Object value, AnimationWindowCurve targetCurve)
    {
      AnimationWindowKeyframe animationWindowKeyframe = new AnimationWindowKeyframe(targetCurve, new ObjectReferenceKeyframe() { time = time, value = value });
      AnimationKeyTime keyTime = AnimationKeyTime.Time(animationWindowKeyframe.time, this.state.frameRate);
      targetCurve.AddKeyframe(animationWindowKeyframe, keyTime);
      this.state.SelectKey(animationWindowKeyframe);
    }

    private static bool ValidateDragAndDropObjects()
    {
      if (DragAndDrop.objectReferences.Length == 0)
        return false;
      for (int index = 0; index < DragAndDrop.objectReferences.Length; ++index)
      {
        UnityEngine.Object objectReference1 = DragAndDrop.objectReferences[index];
        if (objectReference1 == (UnityEngine.Object) null)
          return false;
        if (index < DragAndDrop.objectReferences.Length - 1)
        {
          UnityEngine.Object objectReference2 = DragAndDrop.objectReferences[index + 1];
          bool flag = (objectReference1 is Texture2D || objectReference1 is Sprite) && (objectReference2 is Texture2D || objectReference2 is Sprite);
          if (objectReference1.GetType() != objectReference2.GetType() && !flag)
            return false;
        }
      }
      return true;
    }

    private AnimationWindowCurve GetCurveOfType(DopeLine dopeLine, System.Type type)
    {
      foreach (AnimationWindowCurve curve in dopeLine.m_Curves)
      {
        if (curve.m_ValueType == type)
          return curve;
      }
      return (AnimationWindowCurve) null;
    }

    private bool AnyKeyIsSelectedAtTime(DopeLine dopeLine, int keyIndex)
    {
      int timeHash = dopeLine.keys[keyIndex].m_TimeHash;
      int count = dopeLine.keys.Count;
      for (int index = keyIndex; index < count; ++index)
      {
        AnimationWindowKeyframe key = dopeLine.keys[index];
        if (key.m_TimeHash != timeHash)
          return false;
        if (this.state.KeyIsSelected(key))
          return true;
      }
      return false;
    }

    private void AddKeyToDopeline(object obj)
    {
      this.AddKeyToDopeline((DopeLine) obj);
    }

    private void AddKeyToDopeline(DopeLine dopeLine)
    {
      this.state.ClearSelections();
      foreach (AnimationWindowCurve curve in dopeLine.m_Curves)
        this.state.SelectKey(AnimationWindowUtility.AddKeyframeToCurve(this.state, curve, this.state.time));
    }

    private void DeleteSelectedKeys()
    {
      this.state.DeleteSelectedKeys();
    }

    public void UpdateCurves(List<int> curveIds, string undoText)
    {
    }

    public void UpdateCurves(List<ChangedCurve> changedCurves, string undoText)
    {
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.state.activeAnimationClip, undoText);
      using (List<ChangedCurve>.Enumerator enumerator = changedCurves.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ChangedCurve current = enumerator.Current;
          AnimationUtility.SetEditorCurve(this.state.activeAnimationClip, current.binding, current.curve);
        }
      }
    }

    private struct DrawElement
    {
      public Rect position;
      public Color color;
      public Texture2D texture;

      public DrawElement(Rect position, Color color, Texture2D texture)
      {
        this.position = position;
        this.color = color;
        this.texture = texture;
      }
    }

    internal class DopeSheetSelectionRect
    {
      private static int s_RectSelectionID = GUIUtility.GetPermanentControlID();
      public readonly GUIStyle createRect = (GUIStyle) "U2D.createRect";
      private Vector2 m_SelectStartPoint;
      private Vector2 m_SelectMousePoint;
      private bool m_ValidRect;
      private DopeSheetEditor owner;

      public DopeSheetSelectionRect(DopeSheetEditor owner)
      {
        this.owner = owner;
      }

      public void OnGUI(Rect position)
      {
        Event current1 = Event.current;
        Vector2 mousePosition = current1.mousePosition;
        int rectSelectionId = DopeSheetEditor.DopeSheetSelectionRect.s_RectSelectionID;
        switch (current1.GetTypeForControl(rectSelectionId))
        {
          case EventType.MouseDown:
            if (current1.button != 0 || !position.Contains(mousePosition))
              break;
            GUIUtility.hotControl = rectSelectionId;
            this.m_SelectStartPoint = mousePosition;
            this.m_ValidRect = false;
            current1.Use();
            break;
          case EventType.MouseUp:
            if (GUIUtility.hotControl != rectSelectionId || current1.button != 0)
              break;
            if (this.m_ValidRect)
            {
              if (!EditorGUI.actionKey)
                this.owner.state.ClearSelections();
              float frameRate = this.owner.state.frameRate;
              Rect currentTimeRect = this.GetCurrentTimeRect();
              AnimationKeyTime animationKeyTime1 = AnimationKeyTime.Time(currentTimeRect.xMin, frameRate);
              AnimationKeyTime animationKeyTime2 = AnimationKeyTime.Time(currentTimeRect.xMax, frameRate);
              GUI.changed = true;
              this.owner.state.ClearHierarchySelection();
              List<AnimationWindowKeyframe> animationWindowKeyframeList1 = new List<AnimationWindowKeyframe>();
              List<AnimationWindowKeyframe> animationWindowKeyframeList2 = new List<AnimationWindowKeyframe>();
              using (List<DopeLine>.Enumerator enumerator1 = this.owner.state.dopelines.GetEnumerator())
              {
                while (enumerator1.MoveNext())
                {
                  DopeLine current2 = enumerator1.Current;
                  if ((double) current2.position.yMin >= (double) currentTimeRect.yMin && (double) current2.position.yMax <= (double) currentTimeRect.yMax)
                  {
                    using (List<AnimationWindowKeyframe>.Enumerator enumerator2 = current2.keys.GetEnumerator())
                    {
                      while (enumerator2.MoveNext())
                      {
                        AnimationWindowKeyframe current3 = enumerator2.Current;
                        AnimationKeyTime animationKeyTime3 = AnimationKeyTime.Time(current3.time, frameRate);
                        if ((!current2.tallMode && animationKeyTime3.frame >= animationKeyTime1.frame && animationKeyTime3.frame <= animationKeyTime2.frame || current2.tallMode && animationKeyTime3.frame >= animationKeyTime1.frame && animationKeyTime3.frame < animationKeyTime2.frame) && (!animationWindowKeyframeList2.Contains(current3) && !animationWindowKeyframeList1.Contains(current3)))
                        {
                          if (!this.owner.state.KeyIsSelected(current3))
                            animationWindowKeyframeList2.Add(current3);
                          else if (this.owner.state.KeyIsSelected(current3))
                            animationWindowKeyframeList1.Add(current3);
                        }
                      }
                    }
                  }
                }
              }
              if (animationWindowKeyframeList2.Count == 0)
              {
                using (List<AnimationWindowKeyframe>.Enumerator enumerator = animationWindowKeyframeList1.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                    this.owner.state.UnselectKey(enumerator.Current);
                }
              }
              using (List<AnimationWindowKeyframe>.Enumerator enumerator = animationWindowKeyframeList2.GetEnumerator())
              {
                while (enumerator.MoveNext())
                  this.owner.state.SelectKey(enumerator.Current);
              }
              using (List<DopeLine>.Enumerator enumerator = this.owner.state.dopelines.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  DopeLine current2 = enumerator.Current;
                  if (this.owner.state.AnyKeyIsSelected(current2))
                    this.owner.state.SelectHierarchyItem(current2, true, false);
                }
              }
            }
            else
              this.owner.state.ClearSelections();
            current1.Use();
            GUIUtility.hotControl = 0;
            break;
          case EventType.MouseDrag:
            if (GUIUtility.hotControl != rectSelectionId)
              break;
            this.m_ValidRect = (double) Mathf.Abs((mousePosition - this.m_SelectStartPoint).x) > 1.0;
            if (this.m_ValidRect)
              this.m_SelectMousePoint = new Vector2(mousePosition.x, mousePosition.y);
            current1.Use();
            break;
          case EventType.Repaint:
            if (GUIUtility.hotControl != rectSelectionId || !this.m_ValidRect)
              break;
            EditorStyles.selectionRect.Draw(this.GetCurrentPixelRect(), GUIContent.none, false, false, false, false);
            break;
        }
      }

      public Rect GetCurrentPixelRect()
      {
        float num = 16f;
        Rect rect = AnimationWindowUtility.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint);
        rect.xMin = this.owner.state.TimeToPixel(this.owner.state.PixelToTime(rect.xMin, true), true);
        rect.xMax = this.owner.state.TimeToPixel(this.owner.state.PixelToTime(rect.xMax, true), true);
        rect.yMin = Mathf.Floor(rect.yMin / num) * num;
        rect.yMax = (Mathf.Floor(rect.yMax / num) + 1f) * num;
        return rect;
      }

      public Rect GetCurrentTimeRect()
      {
        float num = 16f;
        Rect rect = AnimationWindowUtility.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint);
        rect.xMin = this.owner.state.PixelToTime(rect.xMin, true);
        rect.xMax = this.owner.state.PixelToTime(rect.xMax, true);
        rect.yMin = Mathf.Floor(rect.yMin / num) * num;
        rect.yMax = (Mathf.Floor(rect.yMax / num) + 1f) * num;
        return rect;
      }

      private enum SelectionType
      {
        Normal,
        Additive,
        Subtractive,
      }
    }

    internal class DopeSheetPopup
    {
      private static int s_width = 96;
      private static int s_height = 112;
      private Rect position;
      private Rect backgroundRect;

      public DopeSheetPopup(Rect position)
      {
        this.position = position;
      }

      public void OnGUI(AnimationWindowState state, AnimationWindowKeyframe keyframe)
      {
        if (keyframe.isPPtrCurve)
          return;
        this.backgroundRect = this.position;
        this.backgroundRect.x = state.TimeToPixel(keyframe.time) + this.position.x - (float) (DopeSheetEditor.DopeSheetPopup.s_width / 2);
        this.backgroundRect.y += 16f;
        this.backgroundRect.width = (float) DopeSheetEditor.DopeSheetPopup.s_width;
        this.backgroundRect.height = (float) DopeSheetEditor.DopeSheetPopup.s_height;
        Rect backgroundRect1 = this.backgroundRect;
        backgroundRect1.height = 16f;
        Rect backgroundRect2 = this.backgroundRect;
        backgroundRect2.y += 16f;
        backgroundRect2.height = (float) DopeSheetEditor.DopeSheetPopup.s_width;
        GUI.Box(this.backgroundRect, string.Empty);
        GUI.Box(backgroundRect2, (Texture) AssetPreview.GetAssetPreview((UnityEngine.Object) keyframe.value));
        EditorGUI.BeginChangeCheck();
        UnityEngine.Object @object = EditorGUI.ObjectField(backgroundRect1, (UnityEngine.Object) keyframe.value, keyframe.curve.m_ValueType, false);
        if (!EditorGUI.EndChangeCheck())
          return;
        keyframe.value = (object) @object;
        state.SaveCurve(keyframe.curve);
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowState
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
  [Serializable]
  internal class AnimationWindowState : ScriptableObject, IPlayHead
  {
    private HashSet<int> m_ModifiedCurves = new HashSet<int>();
    [SerializeField]
    public AnimationWindowHierarchyState hierarchyState;
    [SerializeField]
    public AnimEditor animEditor;
    [SerializeField]
    public bool showCurveEditor;
    [SerializeField]
    private float m_CurrentTime;
    [SerializeField]
    private TimeArea m_timeArea;
    [SerializeField]
    private AnimationClip m_ActiveAnimationClip;
    [SerializeField]
    private GameObject m_ActiveGameObject;
    [SerializeField]
    private HashSet<int> m_SelectedKeyHashes;
    [SerializeField]
    private int m_ActiveKeyframeHash;
    [SerializeField]
    private bool m_Locked;
    private static List<AnimationWindowKeyframe> s_KeyframeClipboard;
    [NonSerialized]
    public System.Action onClipSelectionChanged;
    [NonSerialized]
    public AnimationWindowHierarchyDataSource hierarchyData;
    [NonSerialized]
    public bool m_FrameCurveEditor;
    private List<AnimationWindowCurve> m_AllCurvesCache;
    private List<AnimationWindowCurve> m_ActiveCurvesCache;
    private List<DopeLine> m_dopelinesCache;
    private List<AnimationWindowKeyframe> m_SelectedKeysCache;
    private List<CurveWrapper> m_ActiveCurveWrappersCache;
    private AnimationWindowKeyframe m_ActiveKeyframeCache;
    private EditorCurveBinding? m_lastAddedCurveBinding;
    private int m_PreviousRefreshHash;
    private GameObject m_PreviousActiveRootGameObject;
    private AnimationWindowState.RefreshType m_Refresh;
    public System.Action<float> onFrameRateChange;

    public AnimationWindowState.RefreshType refresh
    {
      get
      {
        return this.m_Refresh;
      }
      set
      {
        if (this.m_Refresh >= value)
          return;
        this.m_Refresh = value;
      }
    }

    public List<AnimationWindowCurve> allCurves
    {
      get
      {
        if (this.m_AllCurvesCache == null)
        {
          this.m_AllCurvesCache = new List<AnimationWindowCurve>();
          if ((UnityEngine.Object) this.activeAnimationClip != (UnityEngine.Object) null)
          {
            EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(this.activeAnimationClip);
            EditorCurveBinding[] referenceCurveBindings = AnimationUtility.GetObjectReferenceCurveBindings(this.activeAnimationClip);
            foreach (EditorCurveBinding editorCurveBinding in curveBindings)
            {
              if (AnimationWindowUtility.ShouldShowAnimationWindowCurve(editorCurveBinding))
                this.m_AllCurvesCache.Add(new AnimationWindowCurve(this.activeAnimationClip, editorCurveBinding, CurveBindingUtility.GetEditorCurveValueType(this.activeRootGameObject, editorCurveBinding)));
            }
            foreach (EditorCurveBinding editorCurveBinding in referenceCurveBindings)
              this.m_AllCurvesCache.Add(new AnimationWindowCurve(this.activeAnimationClip, editorCurveBinding, CurveBindingUtility.GetEditorCurveValueType(this.activeRootGameObject, editorCurveBinding)));
            this.m_AllCurvesCache.Sort();
          }
        }
        return this.m_AllCurvesCache;
      }
    }

    public List<AnimationWindowCurve> activeCurves
    {
      get
      {
        if (this.m_ActiveCurvesCache == null)
        {
          this.m_ActiveCurvesCache = new List<AnimationWindowCurve>();
          if (this.hierarchyState != null && this.hierarchyData != null)
          {
            using (List<int>.Enumerator enumerator1 = this.hierarchyState.selectedIDs.GetEnumerator())
            {
              while (enumerator1.MoveNext())
              {
                AnimationWindowHierarchyNode hierarchyNode = this.hierarchyData.FindItem(enumerator1.Current) as AnimationWindowHierarchyNode;
                if (hierarchyNode != null)
                {
                  using (List<AnimationWindowCurve>.Enumerator enumerator2 = this.GetCurves(hierarchyNode, true).GetEnumerator())
                  {
                    while (enumerator2.MoveNext())
                    {
                      AnimationWindowCurve current = enumerator2.Current;
                      if (!this.m_ActiveCurvesCache.Contains(current))
                        this.m_ActiveCurvesCache.Add(current);
                    }
                  }
                }
              }
            }
          }
        }
        return this.m_ActiveCurvesCache;
      }
    }

    public List<CurveWrapper> activeCurveWrappers
    {
      get
      {
        if (this.m_ActiveCurveWrappersCache == null || this.m_ActiveCurvesCache == null)
        {
          this.m_ActiveCurveWrappersCache = new List<CurveWrapper>();
          using (List<AnimationWindowCurve>.Enumerator enumerator = this.activeCurves.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              AnimationWindowCurve current = enumerator.Current;
              if (!current.isPPtrCurve)
                this.m_ActiveCurveWrappersCache.Add(AnimationWindowUtility.GetCurveWrapper(current, this.activeAnimationClip));
            }
          }
          if (!this.m_ActiveCurveWrappersCache.Any<CurveWrapper>())
          {
            using (List<AnimationWindowCurve>.Enumerator enumerator = this.allCurves.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                AnimationWindowCurve current = enumerator.Current;
                if (!current.isPPtrCurve)
                  this.m_ActiveCurveWrappersCache.Add(AnimationWindowUtility.GetCurveWrapper(current, this.activeAnimationClip));
              }
            }
          }
        }
        return this.m_ActiveCurveWrappersCache;
      }
    }

    public List<DopeLine> dopelines
    {
      get
      {
        if (this.m_dopelinesCache == null)
        {
          this.m_dopelinesCache = new List<DopeLine>();
          if (this.hierarchyData != null)
          {
            using (List<TreeViewItem>.Enumerator enumerator = this.hierarchyData.GetRows().GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                TreeViewItem current = enumerator.Current;
                AnimationWindowHierarchyNode windowHierarchyNode = current as AnimationWindowHierarchyNode;
                if (windowHierarchyNode != null && !(windowHierarchyNode is AnimationWindowHierarchyAddButtonNode))
                {
                  List<AnimationWindowCurve> animationWindowCurveList = !(current is AnimationWindowHierarchyMasterNode) ? this.GetCurves(windowHierarchyNode, true) : this.allCurves;
                  this.m_dopelinesCache.Add(new DopeLine(current.id, animationWindowCurveList.ToArray())
                  {
                    tallMode = this.hierarchyState.GetTallMode(windowHierarchyNode),
                    objectType = windowHierarchyNode.animatableObjectType,
                    hasChildren = !(windowHierarchyNode is AnimationWindowHierarchyPropertyNode),
                    isMasterDopeline = current is AnimationWindowHierarchyMasterNode
                  });
                }
              }
            }
          }
        }
        return this.m_dopelinesCache;
      }
    }

    public List<AnimationWindowHierarchyNode> selectedHierarchyNodes
    {
      get
      {
        List<AnimationWindowHierarchyNode> windowHierarchyNodeList = new List<AnimationWindowHierarchyNode>();
        if (this.hierarchyData != null)
        {
          using (List<int>.Enumerator enumerator = this.hierarchyState.selectedIDs.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              AnimationWindowHierarchyNode windowHierarchyNode = (AnimationWindowHierarchyNode) this.hierarchyData.FindItem(enumerator.Current);
              if (windowHierarchyNode != null && !(windowHierarchyNode is AnimationWindowHierarchyAddButtonNode))
                windowHierarchyNodeList.Add(windowHierarchyNode);
            }
          }
        }
        return windowHierarchyNodeList;
      }
    }

    public AnimationWindowKeyframe activeKeyframe
    {
      get
      {
        if (this.m_ActiveKeyframeCache == null)
        {
          using (List<AnimationWindowCurve>.Enumerator enumerator1 = this.allCurves.GetEnumerator())
          {
            while (enumerator1.MoveNext())
            {
              using (List<AnimationWindowKeyframe>.Enumerator enumerator2 = enumerator1.Current.m_Keyframes.GetEnumerator())
              {
                while (enumerator2.MoveNext())
                {
                  AnimationWindowKeyframe current = enumerator2.Current;
                  if (current.GetHash() == this.m_ActiveKeyframeHash)
                    this.m_ActiveKeyframeCache = current;
                }
              }
            }
          }
        }
        return this.m_ActiveKeyframeCache;
      }
      set
      {
        this.m_ActiveKeyframeCache = (AnimationWindowKeyframe) null;
        this.m_ActiveKeyframeHash = value == null ? 0 : value.GetHash();
      }
    }

    public List<AnimationWindowKeyframe> selectedKeys
    {
      get
      {
        if (this.m_SelectedKeysCache == null)
        {
          this.m_SelectedKeysCache = new List<AnimationWindowKeyframe>();
          using (List<AnimationWindowCurve>.Enumerator enumerator1 = this.allCurves.GetEnumerator())
          {
            while (enumerator1.MoveNext())
            {
              using (List<AnimationWindowKeyframe>.Enumerator enumerator2 = enumerator1.Current.m_Keyframes.GetEnumerator())
              {
                while (enumerator2.MoveNext())
                {
                  AnimationWindowKeyframe current = enumerator2.Current;
                  if (this.KeyIsSelected(current))
                    this.m_SelectedKeysCache.Add(current);
                }
              }
            }
          }
        }
        return this.m_SelectedKeysCache;
      }
    }

    private HashSet<int> selectedKeyHashes
    {
      get
      {
        return this.m_SelectedKeyHashes ?? (this.m_SelectedKeyHashes = new HashSet<int>());
      }
      set
      {
        this.m_SelectedKeyHashes = value;
      }
    }

    public AnimationClip activeAnimationClip
    {
      get
      {
        return this.m_ActiveAnimationClip;
      }
      set
      {
        if (!((UnityEngine.Object) this.m_ActiveAnimationClip != (UnityEngine.Object) value) || this.m_Locked)
          return;
        this.m_ActiveAnimationClip = value;
        if (this.onFrameRateChange != null)
          this.onFrameRateChange(this.frameRate);
        CurveBindingUtility.Cleanup();
        if (this.onClipSelectionChanged == null)
          return;
        this.onClipSelectionChanged();
      }
    }

    public GameObject activeGameObject
    {
      get
      {
        if (!this.m_Locked && (UnityEngine.Object) Selection.activeGameObject != (UnityEngine.Object) null && !EditorUtility.IsPersistent((UnityEngine.Object) Selection.activeGameObject))
          this.m_ActiveGameObject = Selection.activeGameObject;
        return this.m_ActiveGameObject;
      }
    }

    public GameObject activeRootGameObject
    {
      get
      {
        if ((UnityEngine.Object) this.activeGameObject != (UnityEngine.Object) null)
        {
          if (this.activeObjectIsPrefab)
            return (GameObject) null;
          Component componentInParents = AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(this.activeGameObject.transform);
          if ((UnityEngine.Object) componentInParents != (UnityEngine.Object) null)
            return componentInParents.gameObject;
        }
        return (GameObject) null;
      }
    }

    public Component activeAnimationPlayer
    {
      get
      {
        if ((UnityEngine.Object) this.activeGameObject != (UnityEngine.Object) null)
          return AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(this.activeGameObject.transform);
        return (Component) null;
      }
    }

    public bool disabled
    {
      get
      {
        return !(bool) ((UnityEngine.Object) this.activeRootGameObject) || !(bool) ((UnityEngine.Object) this.activeAnimationClip);
      }
    }

    public bool animationIsReadOnly
    {
      get
      {
        return !(bool) ((UnityEngine.Object) this.activeAnimationClip) || (this.m_ActiveAnimationClip.hideFlags & HideFlags.NotEditable) != HideFlags.None || !this.animationIsEditable;
      }
    }

    public bool animationIsEditable
    {
      get
      {
        return (bool) ((UnityEngine.Object) this.activeGameObject) && (!(bool) ((UnityEngine.Object) this.activeAnimationClip) || (this.activeAnimationClip.hideFlags & HideFlags.NotEditable) == HideFlags.None) && !this.activeObjectIsPrefab;
      }
    }

    public bool clipIsEditable
    {
      get
      {
        return (bool) ((UnityEngine.Object) this.activeAnimationClip) && (this.activeAnimationClip.hideFlags & HideFlags.NotEditable) == HideFlags.None && AssetDatabase.IsOpenForEdit((UnityEngine.Object) this.activeAnimationClip);
      }
    }

    public bool activeObjectIsPrefab
    {
      get
      {
        return (bool) ((UnityEngine.Object) this.activeGameObject) && (EditorUtility.IsPersistent((UnityEngine.Object) this.activeGameObject) || (this.activeGameObject.hideFlags & HideFlags.NotEditable) != HideFlags.None);
      }
    }

    public bool animatorIsOptimized
    {
      get
      {
        if (!(bool) ((UnityEngine.Object) this.activeRootGameObject))
          return false;
        Animator component = this.activeRootGameObject.GetComponent<Animator>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.isOptimizable)
          return !component.hasTransformHierarchy;
        return false;
      }
    }

    public bool clipOnlyMode
    {
      get
      {
        if ((UnityEngine.Object) this.activeRootGameObject == (UnityEngine.Object) null)
          return (UnityEngine.Object) this.activeAnimationClip != (UnityEngine.Object) null;
        return false;
      }
    }

    public bool locked
    {
      get
      {
        return this.m_Locked;
      }
      set
      {
        if (this.disabled)
          return;
        this.m_Locked = value;
        if (!((UnityEngine.Object) this.m_ActiveGameObject != (UnityEngine.Object) Selection.activeGameObject))
          return;
        this.OnSelectionChange();
      }
    }

    public float frameRate
    {
      get
      {
        if ((UnityEngine.Object) this.activeAnimationClip == (UnityEngine.Object) null)
          return 60f;
        return this.activeAnimationClip.frameRate;
      }
      set
      {
        if (!((UnityEngine.Object) this.activeAnimationClip != (UnityEngine.Object) null) || (double) value <= 0.0 || (double) value > 10000.0)
          return;
        using (List<AnimationWindowCurve>.Enumerator enumerator1 = this.allCurves.GetEnumerator())
        {
          while (enumerator1.MoveNext())
          {
            AnimationWindowCurve current1 = enumerator1.Current;
            using (List<AnimationWindowKeyframe>.Enumerator enumerator2 = current1.m_Keyframes.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                AnimationWindowKeyframe current2 = enumerator2.Current;
                int frame = AnimationKeyTime.Time(current2.time, this.frameRate).frame;
                current2.time = AnimationKeyTime.Frame(frame, value).time;
              }
            }
            this.SaveCurve(current1);
          }
        }
        AnimationEvent[] animationEvents = AnimationUtility.GetAnimationEvents(this.m_ActiveAnimationClip);
        foreach (AnimationEvent animationEvent in animationEvents)
        {
          int frame = AnimationKeyTime.Time(animationEvent.time, this.frameRate).frame;
          animationEvent.time = AnimationKeyTime.Frame(frame, value).time;
        }
        AnimationUtility.SetAnimationEvents(this.m_ActiveAnimationClip, animationEvents);
        this.m_ActiveAnimationClip.frameRate = value;
        if (this.onFrameRateChange == null)
          return;
        this.onFrameRateChange(this.frameRate);
      }
    }

    public int frame
    {
      get
      {
        return this.TimeToFrameFloor(this.currentTime);
      }
      set
      {
        this.currentTime = this.FrameToTime((float) value);
      }
    }

    public float currentTime
    {
      get
      {
        return this.m_CurrentTime;
      }
      set
      {
        if (Mathf.Approximately(this.m_CurrentTime, value))
          return;
        this.m_CurrentTime = value;
        this.ResampleAnimation();
      }
    }

    public AnimationKeyTime time
    {
      get
      {
        return AnimationKeyTime.Frame(this.frame, this.frameRate);
      }
    }

    public bool playing
    {
      get
      {
        return AnimationMode.InAnimationPlaybackMode();
      }
      set
      {
        if (value && !AnimationMode.InAnimationPlaybackMode())
        {
          AnimationMode.StartAnimationPlaybackMode();
          this.recording = true;
        }
        if (value || !AnimationMode.InAnimationPlaybackMode())
          return;
        AnimationMode.StopAnimationPlaybackMode();
        this.currentTime = this.FrameToTime((float) this.frame);
      }
    }

    public bool recording
    {
      get
      {
        return AnimationMode.InAnimationMode();
      }
      set
      {
        if (value && !AnimationMode.InAnimationMode())
        {
          AnimationMode.StartAnimationMode();
          Undo.postprocessModifications += new Undo.PostprocessModifications(this.PostprocessAnimationRecordingModifications);
        }
        else
        {
          if (value)
            return;
          AnimationMode.StopAnimationMode();
          Undo.postprocessModifications -= new Undo.PostprocessModifications(this.PostprocessAnimationRecordingModifications);
        }
      }
    }

    public TimeArea timeArea
    {
      get
      {
        return this.m_timeArea;
      }
      set
      {
        this.m_timeArea = value;
      }
    }

    public float pixelPerSecond
    {
      get
      {
        return this.timeArea.m_Scale.x;
      }
    }

    public float zeroTimePixel
    {
      get
      {
        return (float) ((double) this.timeArea.shownArea.xMin * (double) this.timeArea.m_Scale.x * -1.0);
      }
    }

    public float minVisibleTime
    {
      get
      {
        return this.m_timeArea.shownArea.xMin;
      }
    }

    public float maxVisibleTime
    {
      get
      {
        return this.m_timeArea.shownArea.xMax;
      }
    }

    public float visibleTimeSpan
    {
      get
      {
        return this.maxVisibleTime - this.minVisibleTime;
      }
    }

    public float minVisibleFrame
    {
      get
      {
        return this.minVisibleTime * this.frameRate;
      }
    }

    public float maxVisibleFrame
    {
      get
      {
        return this.maxVisibleTime * this.frameRate;
      }
    }

    public float visibleFrameSpan
    {
      get
      {
        return this.visibleTimeSpan * this.frameRate;
      }
    }

    public float minTime
    {
      get
      {
        if ((UnityEngine.Object) this.m_ActiveAnimationClip != (UnityEngine.Object) null)
          return this.m_ActiveAnimationClip.startTime;
        return 0.0f;
      }
    }

    public float maxTime
    {
      get
      {
        if ((UnityEngine.Object) this.m_ActiveAnimationClip != (UnityEngine.Object) null && (double) this.m_ActiveAnimationClip.stopTime > 0.0)
          return this.m_ActiveAnimationClip.stopTime;
        return 1f;
      }
    }

    public int minFrame
    {
      get
      {
        return this.TimeToFrameRound(this.minTime);
      }
    }

    public int maxFrame
    {
      get
      {
        return this.TimeToFrameRound(this.maxTime);
      }
    }

    public void OnGUI()
    {
      this.RefreshHashCheck();
      this.Refresh();
    }

    private void RefreshHashCheck()
    {
      int refreshHash = this.GetRefreshHash();
      if (this.m_PreviousRefreshHash == refreshHash)
        return;
      this.refresh = AnimationWindowState.RefreshType.Everything;
      this.m_PreviousRefreshHash = refreshHash;
    }

    private void Refresh()
    {
      if (this.refresh == AnimationWindowState.RefreshType.Everything)
      {
        CurveRendererCache.ClearCurveRendererCache();
        this.m_ActiveKeyframeCache = (AnimationWindowKeyframe) null;
        this.m_AllCurvesCache = (List<AnimationWindowCurve>) null;
        this.m_ActiveCurvesCache = (List<AnimationWindowCurve>) null;
        this.m_dopelinesCache = (List<DopeLine>) null;
        this.m_SelectedKeysCache = (List<AnimationWindowKeyframe>) null;
        this.m_ActiveCurveWrappersCache = (List<CurveWrapper>) null;
        if (this.hierarchyData != null)
          this.hierarchyData.UpdateData();
        if (this.m_lastAddedCurveBinding.HasValue)
          this.OnNewCurveAdded(this.m_lastAddedCurveBinding.Value);
        if (this.activeCurves.Count == 0 && this.dopelines.Count > 0)
          this.SelectHierarchyItem(this.dopelines[0], false, false);
        this.m_Refresh = AnimationWindowState.RefreshType.None;
      }
      else if (this.refresh == AnimationWindowState.RefreshType.CurvesOnly)
      {
        CurveRendererCache.ClearCurveRendererCache();
        this.m_ActiveKeyframeCache = (AnimationWindowKeyframe) null;
        this.m_ActiveCurvesCache = (List<AnimationWindowCurve>) null;
        this.m_ActiveCurveWrappersCache = (List<CurveWrapper>) null;
        this.m_SelectedKeysCache = (List<AnimationWindowKeyframe>) null;
        this.ReloadModifiedAnimationCurveCache();
        this.ReloadModifiedDopelineCache();
        this.m_Refresh = AnimationWindowState.RefreshType.None;
        this.m_ModifiedCurves.Clear();
      }
      if (!this.disabled || !this.recording)
        return;
      this.recording = false;
    }

    private int GetRefreshHash()
    {
      return (!((UnityEngine.Object) this.activeAnimationClip != (UnityEngine.Object) null) ? 0 : this.activeAnimationClip.GetHashCode()) ^ (!((UnityEngine.Object) this.activeRootGameObject != (UnityEngine.Object) null) ? 0 : this.activeRootGameObject.GetHashCode()) ^ (this.hierarchyState == null ? 0 : this.hierarchyState.expandedIDs.Count) ^ (this.hierarchyState == null ? 0 : this.hierarchyState.GetTallInstancesCount()) ^ (!this.showCurveEditor ? 0 : 1);
    }

    public void OnSelectionChange()
    {
      if (this.m_Locked)
        return;
      AnimationClip[] animationClipArray = new AnimationClip[0];
      if ((UnityEngine.Object) this.activeRootGameObject != (UnityEngine.Object) null && !(Selection.activeObject is AnimationClip))
      {
        AnimationClip[] animationClips = AnimationUtility.GetAnimationClips(this.activeRootGameObject);
        if ((UnityEngine.Object) this.activeAnimationClip == (UnityEngine.Object) null && (UnityEngine.Object) this.activeGameObject != (UnityEngine.Object) null)
          this.activeAnimationClip = animationClips.Length <= 0 ? (AnimationClip) null : animationClips[0];
        else if (!Array.Exists<AnimationClip>(animationClips, (Predicate<AnimationClip>) (x => (UnityEngine.Object) x == (UnityEngine.Object) this.activeAnimationClip)))
          this.activeAnimationClip = animationClips.Length <= 0 ? (AnimationClip) null : animationClips[0];
      }
      else if ((UnityEngine.Object) this.activeRootGameObject == (UnityEngine.Object) null)
      {
        this.m_ActiveAnimationClip = (AnimationClip) null;
        this.onFrameRateChange(this.frameRate);
      }
      if ((UnityEngine.Object) this.m_PreviousActiveRootGameObject != (UnityEngine.Object) this.activeRootGameObject)
        this.recording = false;
      this.m_PreviousActiveRootGameObject = this.activeRootGameObject;
    }

    public void OnControllerChange()
    {
      AnimationClip[] animationClips = AnimationUtility.GetAnimationClips(this.activeRootGameObject);
      this.activeAnimationClip = animationClips == null || animationClips.Length <= 0 ? (AnimationClip) null : animationClips[0];
      this.refresh = AnimationWindowState.RefreshType.Everything;
    }

    public void OnEnable()
    {
      this.hideFlags = HideFlags.HideAndDontSave;
      AnimationUtility.onCurveWasModified += new AnimationUtility.OnCurveWasModified(this.CurveWasModified);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    public void OnDisable()
    {
      CurveBindingUtility.Cleanup();
      this.recording = false;
      this.playing = false;
      AnimationUtility.onCurveWasModified -= new AnimationUtility.OnCurveWasModified(this.CurveWasModified);
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    public void UndoRedoPerformed()
    {
      this.refresh = AnimationWindowState.RefreshType.Everything;
    }

    private void CurveWasModified(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType type)
    {
      if ((UnityEngine.Object) clip != (UnityEngine.Object) this.activeAnimationClip)
        return;
      if (type == AnimationUtility.CurveModifiedType.CurveModified)
      {
        bool flag = false;
        int hashCode1 = binding.GetHashCode();
        using (List<AnimationWindowCurve>.Enumerator enumerator = this.allCurves.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            int hashCode2 = enumerator.Current.binding.GetHashCode();
            if (hashCode2 == hashCode1)
            {
              this.m_ModifiedCurves.Add(hashCode2);
              flag = true;
            }
          }
        }
        if (flag)
        {
          this.refresh = AnimationWindowState.RefreshType.CurvesOnly;
        }
        else
        {
          this.m_lastAddedCurveBinding = new EditorCurveBinding?(binding);
          this.refresh = AnimationWindowState.RefreshType.Everything;
        }
      }
      else
        this.refresh = AnimationWindowState.RefreshType.Everything;
    }

    public void SaveCurve(AnimationWindowCurve curve)
    {
      curve.m_Keyframes.Sort((Comparison<AnimationWindowKeyframe>) ((a, b) => a.time.CompareTo(b.time)));
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.activeAnimationClip, "Edit Curve");
      if (curve.isPPtrCurve)
      {
        ObjectReferenceKeyframe[] keyframes = curve.ToObjectCurve();
        if (keyframes.Length == 0)
          keyframes = (ObjectReferenceKeyframe[]) null;
        AnimationUtility.SetObjectReferenceCurve(this.activeAnimationClip, curve.binding, keyframes);
      }
      else
      {
        AnimationCurve curve1 = curve.ToAnimationCurve();
        if (curve1.keys.Length == 0)
          curve1 = (AnimationCurve) null;
        else
          QuaternionCurveTangentCalculation.UpdateTangentsFromMode(curve1, this.activeAnimationClip, curve.binding);
        AnimationUtility.SetEditorCurve(this.activeAnimationClip, curve.binding, curve1);
      }
      this.Repaint();
    }

    public void SaveSelectedKeys(List<AnimationWindowKeyframe> currentSelectedKeys)
    {
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      using (List<AnimationWindowKeyframe>.Enumerator enumerator1 = currentSelectedKeys.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          AnimationWindowKeyframe current1 = enumerator1.Current;
          if (!animationWindowCurveList.Contains(current1.curve))
            animationWindowCurveList.Add(current1.curve);
          List<AnimationWindowKeyframe> animationWindowKeyframeList = new List<AnimationWindowKeyframe>();
          using (List<AnimationWindowKeyframe>.Enumerator enumerator2 = current1.curve.m_Keyframes.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              AnimationWindowKeyframe current2 = enumerator2.Current;
              if (!currentSelectedKeys.Contains(current2) && AnimationKeyTime.Time(current1.time, this.frameRate).frame == AnimationKeyTime.Time(current2.time, this.frameRate).frame)
                animationWindowKeyframeList.Add(current2);
            }
          }
          using (List<AnimationWindowKeyframe>.Enumerator enumerator2 = animationWindowKeyframeList.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              AnimationWindowKeyframe current2 = enumerator2.Current;
              current1.curve.m_Keyframes.Remove(current2);
            }
          }
        }
      }
      using (List<AnimationWindowCurve>.Enumerator enumerator = animationWindowCurveList.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.SaveCurve(enumerator.Current);
      }
    }

    public void RemoveCurve(AnimationWindowCurve curve)
    {
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.activeAnimationClip, "Remove Curve");
      if (curve.isPPtrCurve)
        AnimationUtility.SetObjectReferenceCurve(this.activeAnimationClip, curve.binding, (ObjectReferenceKeyframe[]) null);
      else
        AnimationUtility.SetEditorCurve(this.activeAnimationClip, curve.binding, (AnimationCurve) null);
    }

    public bool AnyKeyIsSelected(DopeLine dopeline)
    {
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = dopeline.keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (this.KeyIsSelected(enumerator.Current))
            return true;
        }
      }
      return false;
    }

    public bool KeyIsSelected(AnimationWindowKeyframe keyframe)
    {
      return this.selectedKeyHashes.Contains(keyframe.GetHash());
    }

    public void SelectKey(AnimationWindowKeyframe keyframe)
    {
      int hash = keyframe.GetHash();
      if (!this.selectedKeyHashes.Contains(hash))
        this.selectedKeyHashes.Add(hash);
      this.m_SelectedKeysCache = (List<AnimationWindowKeyframe>) null;
    }

    public void SelectKeysFromDopeline(DopeLine dopeline)
    {
      if (dopeline == null)
        return;
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = dopeline.keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.SelectKey(enumerator.Current);
      }
    }

    public void UnselectKey(AnimationWindowKeyframe keyframe)
    {
      int hash = keyframe.GetHash();
      if (this.selectedKeyHashes.Contains(hash))
        this.selectedKeyHashes.Remove(hash);
      this.m_SelectedKeysCache = (List<AnimationWindowKeyframe>) null;
    }

    public void UnselectKeysFromDopeline(DopeLine dopeline)
    {
      if (dopeline == null)
        return;
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = dopeline.keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.UnselectKey(enumerator.Current);
      }
    }

    public void DeleteSelectedKeys()
    {
      if (this.selectedKeys.Count == 0)
        return;
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = this.selectedKeys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowKeyframe current = enumerator.Current;
          this.UnselectKey(current);
          current.curve.m_Keyframes.Remove(current);
          this.SaveCurve(current.curve);
        }
      }
    }

    public void MoveSelectedKeys(float deltaTime)
    {
      this.MoveSelectedKeys(deltaTime, false);
    }

    public void MoveSelectedKeys(float deltaTime, bool snapToFrame)
    {
      this.MoveSelectedKeys(deltaTime, snapToFrame, true);
    }

    public void MoveSelectedKeys(float deltaTime, bool snapToFrame, bool saveToClip)
    {
      List<AnimationWindowKeyframe> currentSelectedKeys = new List<AnimationWindowKeyframe>((IEnumerable<AnimationWindowKeyframe>) this.selectedKeys);
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = currentSelectedKeys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowKeyframe current = enumerator.Current;
          current.time += deltaTime;
          if (snapToFrame)
            current.time = this.SnapToFrame(current.time, !saveToClip);
        }
      }
      if (saveToClip)
        this.SaveSelectedKeys(currentSelectedKeys);
      this.ClearKeySelections();
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = currentSelectedKeys.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.SelectKey(enumerator.Current);
      }
    }

    public void CopyKeys()
    {
      if (AnimationWindowState.s_KeyframeClipboard == null)
        AnimationWindowState.s_KeyframeClipboard = new List<AnimationWindowKeyframe>();
      float num = float.MaxValue;
      AnimationWindowState.s_KeyframeClipboard.Clear();
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = this.selectedKeys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowKeyframe current = enumerator.Current;
          AnimationWindowState.s_KeyframeClipboard.Add(new AnimationWindowKeyframe(current));
          if ((double) current.time < (double) num)
            num = current.time;
        }
      }
      if (AnimationWindowState.s_KeyframeClipboard.Count > 0)
      {
        using (List<AnimationWindowKeyframe>.Enumerator enumerator = AnimationWindowState.s_KeyframeClipboard.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.time -= num;
        }
      }
      else
        this.CopyAllActiveCurves();
    }

    public void CopyAllActiveCurves()
    {
      using (List<AnimationWindowCurve>.Enumerator enumerator1 = this.activeCurves.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          using (List<AnimationWindowKeyframe>.Enumerator enumerator2 = enumerator1.Current.m_Keyframes.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              AnimationWindowKeyframe current = enumerator2.Current;
              AnimationWindowState.s_KeyframeClipboard.Add(new AnimationWindowKeyframe(current));
            }
          }
        }
      }
    }

    public void PasteKeys()
    {
      if (AnimationWindowState.s_KeyframeClipboard == null)
        AnimationWindowState.s_KeyframeClipboard = new List<AnimationWindowKeyframe>();
      HashSet<int> intSet = new HashSet<int>((IEnumerable<int>) this.m_SelectedKeyHashes);
      this.ClearKeySelections();
      AnimationWindowCurve animationWindowCurve1 = (AnimationWindowCurve) null;
      AnimationWindowCurve animationWindowCurve2 = (AnimationWindowCurve) null;
      float startTime = 0.0f;
      List<AnimationWindowCurve> source = new List<AnimationWindowCurve>();
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = AnimationWindowState.s_KeyframeClipboard.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowKeyframe current = enumerator.Current;
          if (!source.Any<AnimationWindowCurve>() || source.Last<AnimationWindowCurve>() != current.curve)
            source.Add(current.curve);
        }
      }
      bool flag = source.Count<AnimationWindowCurve>() == this.activeCurves.Count<AnimationWindowCurve>();
      int index = 0;
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = AnimationWindowState.s_KeyframeClipboard.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowKeyframe current = enumerator.Current;
          if (animationWindowCurve2 != null && current.curve != animationWindowCurve2)
            ++index;
          AnimationWindowKeyframe keyframe = new AnimationWindowKeyframe(current);
          keyframe.curve = !flag ? AnimationWindowUtility.BestMatchForPaste(keyframe.curve.binding, this.activeCurves) : this.activeCurves[index];
          if (keyframe.curve == null)
          {
            keyframe.curve = new AnimationWindowCurve(this.activeAnimationClip, current.curve.binding, current.curve.type);
            keyframe.time = current.time;
          }
          keyframe.time += this.time.time;
          if (keyframe.curve != null)
          {
            if (keyframe.curve.HasKeyframe(AnimationKeyTime.Time(keyframe.time, this.frameRate)))
              keyframe.curve.RemoveKeyframe(AnimationKeyTime.Time(keyframe.time, this.frameRate));
            if (animationWindowCurve1 == keyframe.curve)
              keyframe.curve.RemoveKeysAtRange(startTime, keyframe.time);
            keyframe.curve.m_Keyframes.Add(keyframe);
            this.SelectKey(keyframe);
            this.SaveCurve(keyframe.curve);
            animationWindowCurve1 = keyframe.curve;
            startTime = keyframe.time;
          }
          animationWindowCurve2 = current.curve;
        }
      }
      if (this.m_SelectedKeyHashes.Count == 0)
        this.m_SelectedKeyHashes = intSet;
      else
        this.ResampleAnimation();
    }

    public void ClearSelections()
    {
      this.ClearKeySelections();
      this.ClearHierarchySelection();
    }

    public void ClearKeySelections()
    {
      this.selectedKeyHashes.Clear();
      this.m_SelectedKeysCache = (List<AnimationWindowKeyframe>) null;
    }

    public void ClearHierarchySelection()
    {
      this.hierarchyState.selectedIDs.Clear();
    }

    private void ReloadModifiedDopelineCache()
    {
      if (this.m_dopelinesCache == null)
        return;
      using (List<DopeLine>.Enumerator enumerator = this.m_dopelinesCache.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          DopeLine current = enumerator.Current;
          foreach (AnimationWindowCurve curve in current.m_Curves)
          {
            if (this.m_ModifiedCurves.Contains(curve.binding.GetHashCode()))
              current.LoadKeyframes();
          }
        }
      }
    }

    private void ReloadModifiedAnimationCurveCache()
    {
      if (this.m_AllCurvesCache == null)
        return;
      using (List<AnimationWindowCurve>.Enumerator enumerator = this.m_AllCurvesCache.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowCurve current = enumerator.Current;
          if (this.m_ModifiedCurves.Contains(current.binding.GetHashCode()))
            current.LoadKeyframes(this.activeAnimationClip);
        }
      }
    }

    public void ResampleAnimation()
    {
      if ((UnityEngine.Object) this.activeAnimationClip == (UnityEngine.Object) null || this.animatorIsOptimized)
        return;
      if (!this.recording)
        this.recording = true;
      Undo.FlushUndoRecordObjects();
      AnimationMode.BeginSampling();
      CurveBindingUtility.SampleAnimationClip(this.activeRootGameObject, this.activeAnimationClip, this.currentTime);
      AnimationMode.EndSampling();
      SceneView.RepaintAll();
      ParticleSystemWindow instance = ParticleSystemWindow.GetInstance();
      if (!(bool) ((UnityEngine.Object) instance))
        return;
      instance.Repaint();
    }

    private void OnNewCurveAdded(EditorCurveBinding newCurve)
    {
      string propertyGroupName = AnimationWindowUtility.GetPropertyGroupName(newCurve.propertyName);
      int propertyNodeId = AnimationWindowUtility.GetPropertyNodeID(newCurve.path, newCurve.type, propertyGroupName);
      this.SelectHierarchyItem(propertyNodeId, false, false);
      if (newCurve.isPPtrCurve)
        this.hierarchyState.AddTallInstance(propertyNodeId);
      this.m_lastAddedCurveBinding = new EditorCurveBinding?();
    }

    public void Repaint()
    {
      if (!((UnityEngine.Object) this.animEditor != (UnityEngine.Object) null))
        return;
      this.animEditor.Repaint();
    }

    public List<AnimationWindowCurve> GetCurves(AnimationWindowHierarchyNode hierarchyNode, bool entireHierarchy)
    {
      return AnimationWindowUtility.FilterCurves(this.allCurves.ToArray(), hierarchyNode.path, hierarchyNode.animatableObjectType, hierarchyNode.propertyName);
    }

    public List<AnimationWindowKeyframe> GetAggregateKeys(AnimationWindowHierarchyNode hierarchyNode)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      DopeLine dopeLine = this.dopelines.FirstOrDefault<DopeLine>(new Func<DopeLine, bool>(new AnimationWindowState.\u003CGetAggregateKeys\u003Ec__AnonStorey43() { hierarchyNode = hierarchyNode }.\u003C\u003Em__71));
      if (dopeLine == null)
        return (List<AnimationWindowKeyframe>) null;
      return dopeLine.keys;
    }

    public void OnHierarchySelectionChanged(int[] selectedInstanceIDs)
    {
      this.HandleHierarchySelectionChanged(selectedInstanceIDs, true);
      using (List<DopeLine>.Enumerator enumerator = this.dopelines.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          DopeLine current = enumerator.Current;
          if (((IEnumerable<int>) selectedInstanceIDs).Contains<int>(current.m_HierarchyNodeID))
            this.SelectKeysFromDopeline(current);
          else
            this.UnselectKeysFromDopeline(current);
        }
      }
    }

    public void HandleHierarchySelectionChanged(int[] selectedInstanceIDs, bool triggerSceneSelectionSync)
    {
      this.m_ActiveCurvesCache = (List<AnimationWindowCurve>) null;
      this.m_FrameCurveEditor = true;
      if (!triggerSceneSelectionSync)
        return;
      this.SyncSceneSelection(selectedInstanceIDs);
    }

    public void SelectHierarchyItem(DopeLine dopeline, bool additive)
    {
      this.SelectHierarchyItem(dopeline.m_HierarchyNodeID, additive, true);
    }

    public void SelectHierarchyItem(DopeLine dopeline, bool additive, bool triggerSceneSelectionSync)
    {
      this.SelectHierarchyItem(dopeline.m_HierarchyNodeID, additive, triggerSceneSelectionSync);
    }

    public void SelectHierarchyItem(int hierarchyNodeID, bool additive, bool triggerSceneSelectionSync)
    {
      if (!additive)
        this.ClearHierarchySelection();
      this.hierarchyState.selectedIDs.Add(hierarchyNodeID);
      this.HandleHierarchySelectionChanged(this.hierarchyState.selectedIDs.ToArray(), triggerSceneSelectionSync);
    }

    public void UnSelectHierarchyItem(DopeLine dopeline)
    {
      this.UnSelectHierarchyItem(dopeline.m_HierarchyNodeID);
    }

    public void UnSelectHierarchyItem(int hierarchyNodeID)
    {
      this.hierarchyState.selectedIDs.Remove(hierarchyNodeID);
    }

    public List<int> GetAffectedHierarchyIDs(List<AnimationWindowKeyframe> keyframes)
    {
      List<int> intList = new List<int>();
      using (List<DopeLine>.Enumerator enumerator = this.GetAffectedDopelines(keyframes).GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          DopeLine current = enumerator.Current;
          if (!intList.Contains(current.m_HierarchyNodeID))
            intList.Add(current.m_HierarchyNodeID);
        }
      }
      return intList;
    }

    public List<DopeLine> GetAffectedDopelines(List<AnimationWindowKeyframe> keyframes)
    {
      List<DopeLine> dopeLineList = new List<DopeLine>();
      using (List<AnimationWindowCurve>.Enumerator enumerator1 = this.GetAffectedCurves(keyframes).GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          AnimationWindowCurve current1 = enumerator1.Current;
          using (List<DopeLine>.Enumerator enumerator2 = this.dopelines.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              DopeLine current2 = enumerator2.Current;
              if (!dopeLineList.Contains(current2) && ((IEnumerable<AnimationWindowCurve>) current2.m_Curves).Contains<AnimationWindowCurve>(current1))
                dopeLineList.Add(current2);
            }
          }
        }
      }
      return dopeLineList;
    }

    public List<AnimationWindowCurve> GetAffectedCurves(List<AnimationWindowKeyframe> keyframes)
    {
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = keyframes.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowKeyframe current = enumerator.Current;
          if (!animationWindowCurveList.Contains(current.curve))
            animationWindowCurveList.Add(current.curve);
        }
      }
      return animationWindowCurveList;
    }

    public DopeLine GetDopeline(int selectedInstanceID)
    {
      using (List<DopeLine>.Enumerator enumerator = this.dopelines.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          DopeLine current = enumerator.Current;
          if (current.m_HierarchyNodeID == selectedInstanceID)
            return current;
        }
      }
      return (DopeLine) null;
    }

    private void SyncSceneSelection(int[] selectedNodeIDs)
    {
      List<int> intList = new List<int>();
      foreach (int selectedNodeId in selectedNodeIDs)
      {
        AnimationWindowHierarchyNode windowHierarchyNode = this.hierarchyData.FindItem(selectedNodeId) as AnimationWindowHierarchyNode;
        if (!((UnityEngine.Object) this.activeRootGameObject == (UnityEngine.Object) null) && windowHierarchyNode != null && !(windowHierarchyNode is AnimationWindowHierarchyMasterNode))
        {
          Transform tr = this.activeRootGameObject.transform.Find(windowHierarchyNode.path);
          if ((UnityEngine.Object) tr != (UnityEngine.Object) null && (UnityEngine.Object) this.activeRootGameObject != (UnityEngine.Object) null && (UnityEngine.Object) this.activeAnimationPlayer == (UnityEngine.Object) AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(tr))
            intList.Add(tr.gameObject.GetInstanceID());
        }
      }
      Selection.instanceIDs = intList.ToArray();
    }

    private UndoPropertyModification[] PostprocessAnimationRecordingModifications(UndoPropertyModification[] modifications)
    {
      return AnimationRecording.Process(this, modifications);
    }

    public float PixelToTime(float pixel)
    {
      return this.PixelToTime(pixel, false);
    }

    public float PixelToTime(float pixel, bool snapToFrame)
    {
      float num = pixel - this.zeroTimePixel;
      if (snapToFrame)
        return this.SnapToFrame(num / this.pixelPerSecond);
      return num / this.pixelPerSecond;
    }

    public float TimeToPixel(float time)
    {
      return this.TimeToPixel(time, false);
    }

    public float TimeToPixel(float time, bool snapToFrame)
    {
      return (!snapToFrame ? time : this.SnapToFrame(time)) * this.pixelPerSecond + this.zeroTimePixel;
    }

    public float SnapToFrame(float time)
    {
      return Mathf.Round(time * this.frameRate) / this.frameRate;
    }

    public float SnapToFrame(float time, bool preventHashCollision)
    {
      if (preventHashCollision)
        return (float) ((double) Mathf.Round(time * this.frameRate) / (double) this.frameRate + 0.00999999977648258 / (double) this.frameRate);
      return this.SnapToFrame(time);
    }

    public string FormatFrame(int frame, int frameDigits)
    {
      return (frame / (int) this.frameRate).ToString() + ":" + ((float) frame % this.frameRate).ToString().PadLeft(frameDigits, '0');
    }

    public float TimeToFrame(float time)
    {
      return time * this.frameRate;
    }

    public float FrameToTime(float frame)
    {
      return frame / this.frameRate;
    }

    public float FrameToTimeFloor(float frame)
    {
      return (frame - 0.5f) / this.frameRate;
    }

    public float FrameToTimeCeiling(float frame)
    {
      return (frame + 0.5f) / this.frameRate;
    }

    public int TimeToFrameFloor(float time)
    {
      return Mathf.FloorToInt(this.TimeToFrame(time));
    }

    public int TimeToFrameRound(float time)
    {
      return Mathf.RoundToInt(this.TimeToFrame(time));
    }

    public float FrameToPixel(float i, Rect rect)
    {
      return (i - this.minVisibleFrame) * rect.width / this.visibleFrameSpan;
    }

    public float FrameDeltaToPixel(Rect rect)
    {
      return rect.width / this.visibleFrameSpan;
    }

    public float TimeToPixel(float time, Rect rect)
    {
      return this.FrameToPixel(time * this.frameRate, rect);
    }

    public float PixelToTime(float pixelX, Rect rect)
    {
      return pixelX * this.visibleTimeSpan / rect.width + this.minVisibleTime;
    }

    public float PixelDeltaToTime(Rect rect)
    {
      return this.visibleTimeSpan / rect.width;
    }

    public float SnapTimeToWholeFPS(float time)
    {
      return Mathf.Round(time * this.frameRate) / this.frameRate;
    }

    public enum RefreshType
    {
      None,
      CurvesOnly,
      Everything,
    }
  }
}

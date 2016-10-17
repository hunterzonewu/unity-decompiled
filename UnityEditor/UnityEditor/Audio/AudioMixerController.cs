// Decompiled with JetBrains decompiler
// Type: UnityEditor.Audio.AudioMixerController
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityEditor.Audio
{
  internal sealed class AudioMixerController : AudioMixer
  {
    public static float kMinVolume = -80f;
    public static float kVolumeWarp = 1.7f;
    public static string s_GroupEffectDisplaySeperator = "\\";
    [NonSerialized]
    public int m_HighlightEffectIndex = -1;
    [NonSerialized]
    private List<AudioMixerGroupController> m_CachedSelection;
    public static float kMaxEffect;
    [NonSerialized]
    private Dictionary<GUID, AudioParameterPath> m_ExposedParamPathCache;

    public AudioMixerGroupController[] allGroups
    {
      get
      {
        List<AudioMixerGroupController> groups = new List<AudioMixerGroupController>();
        AudioMixerController.GetGroupsRecurse(this.masterGroup, groups);
        return groups.ToArray();
      }
    }

    public int numExposedParameters { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public ExposedAudioParameter[] exposedParameters { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public AudioMixerGroupController masterGroup { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public AudioMixerSnapshot startSnapshot { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public AudioMixerSnapshotController TargetSnapshot { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public AudioMixerSnapshotController[] snapshots { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public List<AudioMixerGroupController> CachedSelection
    {
      get
      {
        if (this.m_CachedSelection == null)
          this.m_CachedSelection = new List<AudioMixerGroupController>();
        return this.m_CachedSelection;
      }
    }

    public int currentViewIndex { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public MixerGroupView[] views { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public bool isSuspended { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    private Dictionary<GUID, AudioParameterPath> exposedParamCache
    {
      get
      {
        if (this.m_ExposedParamPathCache == null)
          this.m_ExposedParamPathCache = new Dictionary<GUID, AudioParameterPath>();
        return this.m_ExposedParamPathCache;
      }
    }

    public event ChangedExposedParameterHandler ChangedExposedParameter;

    public AudioMixerController()
    {
      AudioMixerController.Internal_CreateAudioMixerController(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateAudioMixerController(AudioMixerController mono);

    private static void GetGroupsRecurse(AudioMixerGroupController group, List<AudioMixerGroupController> groups)
    {
      groups.Add(group);
      foreach (AudioMixerGroupController child in group.children)
        AudioMixerController.GetGroupsRecurse(child, groups);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int GetGroupVUInfo(GUID group, bool fader, ref float[] vuLevel, ref float[] vuPeak);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void UpdateMuteSolo();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void UpdateBypass();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool CurrentViewContainsGroup(GUID group);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CheckForCyclicReferences(AudioMixer mixer, AudioMixerGroup group);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern float GetMaxVolume();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern float GetVolumeSplitPoint();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool EditingTargetSnapshot();

    public void OnChangedExposedParameter()
    {
      if (this.ChangedExposedParameter == null)
        return;
      this.ChangedExposedParameter();
    }

    public void ClearEventHandlers()
    {
      if (this.ChangedExposedParameter == null)
        return;
      foreach (Delegate invocation in this.ChangedExposedParameter.GetInvocationList())
      {
        AudioMixerController audioMixerController = this;
        ChangedExposedParameterHandler parameterHandler = (ChangedExposedParameterHandler) Delegate.Remove((Delegate) audioMixerController.ChangedExposedParameter, invocation);
        audioMixerController.ChangedExposedParameter = parameterHandler;
      }
    }

    private string FindUniqueParameterName(string template, ExposedAudioParameter[] parameters)
    {
      string str = template;
      int num = 1;
      for (int index = 0; index < parameters.Length; ++index)
      {
        if (str == parameters[index].name)
        {
          str = template + " " + (object) num++;
          index = -1;
        }
      }
      return str;
    }

    private int SortFuncForExposedParameters(ExposedAudioParameter p1, ExposedAudioParameter p2)
    {
      return string.CompareOrdinal(this.ResolveExposedParameterPath(p1.guid, true), this.ResolveExposedParameterPath(p2.guid, true));
    }

    public void AddExposedParameter(AudioParameterPath path)
    {
      if (!this.ContainsExposedParameter(path.parameter))
      {
        List<ExposedAudioParameter> exposedAudioParameterList = new List<ExposedAudioParameter>((IEnumerable<ExposedAudioParameter>) this.exposedParameters);
        exposedAudioParameterList.Add(new ExposedAudioParameter()
        {
          name = this.FindUniqueParameterName("MyExposedParam", this.exposedParameters),
          guid = path.parameter
        });
        exposedAudioParameterList.Sort(new Comparison<ExposedAudioParameter>(this.SortFuncForExposedParameters));
        this.exposedParameters = exposedAudioParameterList.ToArray();
        this.OnChangedExposedParameter();
        this.exposedParamCache[path.parameter] = path;
        AudioMixerUtility.RepaintAudioMixerAndInspectors();
      }
      else
        Debug.LogError((object) "Cannot expose the same parameter more than once!");
    }

    public bool ContainsExposedParameter(GUID parameter)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<ExposedAudioParameter>) this.exposedParameters).Where<ExposedAudioParameter>(new Func<ExposedAudioParameter, bool>(new AudioMixerController.\u003CContainsExposedParameter\u003Ec__AnonStoreyC() { parameter = parameter }.\u003C\u003Em__2)).ToArray<ExposedAudioParameter>().Length > 0;
    }

    public void RemoveExposedParameter(GUID parameter)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioMixerController.\u003CRemoveExposedParameter\u003Ec__AnonStoreyD parameterCAnonStoreyD = new AudioMixerController.\u003CRemoveExposedParameter\u003Ec__AnonStoreyD();
      // ISSUE: reference to a compiler-generated field
      parameterCAnonStoreyD.parameter = parameter;
      // ISSUE: reference to a compiler-generated method
      this.exposedParameters = ((IEnumerable<ExposedAudioParameter>) this.exposedParameters).Where<ExposedAudioParameter>(new Func<ExposedAudioParameter, bool>(parameterCAnonStoreyD.\u003C\u003Em__3)).ToArray<ExposedAudioParameter>();
      this.OnChangedExposedParameter();
      // ISSUE: reference to a compiler-generated field
      if (this.exposedParamCache.ContainsKey(parameterCAnonStoreyD.parameter))
      {
        // ISSUE: reference to a compiler-generated field
        this.exposedParamCache.Remove(parameterCAnonStoreyD.parameter);
      }
      AudioMixerUtility.RepaintAudioMixerAndInspectors();
    }

    public string ResolveExposedParameterPath(GUID parameter, bool getOnlyBasePath)
    {
      if (this.exposedParamCache.ContainsKey(parameter))
        return this.exposedParamCache[parameter].ResolveStringPath(getOnlyBasePath);
      using (List<AudioMixerGroupController>.Enumerator enumerator = this.GetAllAudioGroupsSlow().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          if (current.GetGUIDForVolume() == parameter || current.GetGUIDForPitch() == parameter)
          {
            AudioGroupParameterPath groupParameterPath = new AudioGroupParameterPath(current, parameter);
            this.exposedParamCache[parameter] = (AudioParameterPath) groupParameterPath;
            return groupParameterPath.ResolveStringPath(getOnlyBasePath);
          }
          for (int index = 0; index < current.effects.Length; ++index)
          {
            AudioMixerEffectController effect = current.effects[index];
            foreach (MixerParameterDefinition effectParameter in MixerEffectDefinitions.GetEffectParameters(effect.effectName))
            {
              if (effect.GetGUIDForParameter(effectParameter.name) == parameter)
              {
                AudioEffectParameterPath effectParameterPath = new AudioEffectParameterPath(current, effect, parameter);
                this.exposedParamCache[parameter] = (AudioParameterPath) effectParameterPath;
                return effectParameterPath.ResolveStringPath(getOnlyBasePath);
              }
            }
          }
        }
      }
      return "Error finding Parameter path";
    }

    public static AudioMixerController CreateMixerControllerAtPath(string path)
    {
      AudioMixerController audioMixerController = new AudioMixerController();
      audioMixerController.CreateDefaultAsset(path);
      return audioMixerController;
    }

    public void CreateDefaultAsset(string path)
    {
      this.masterGroup = new AudioMixerGroupController((AudioMixer) this);
      this.masterGroup.name = "Master";
      this.masterGroup.PreallocateGUIDs();
      AudioMixerEffectController effect = new AudioMixerEffectController("Attenuation");
      effect.PreallocateGUIDs();
      this.masterGroup.InsertEffect(effect, 0);
      AudioMixerSnapshotController snapshotController = new AudioMixerSnapshotController((AudioMixer) this);
      snapshotController.name = "Snapshot";
      this.snapshots = new AudioMixerSnapshotController[1]
      {
        snapshotController
      };
      this.startSnapshot = (AudioMixerSnapshot) snapshotController;
      AssetDatabase.CreateAssetFromObjects(new UnityEngine.Object[4]
      {
        (UnityEngine.Object) this,
        (UnityEngine.Object) this.masterGroup,
        (UnityEngine.Object) effect,
        (UnityEngine.Object) snapshotController
      }, path);
    }

    private void BuildTestSetup(System.Random r, AudioMixerGroupController parent, int minSpan, int maxSpan, int maxGroups, string prefix, ref int numGroups)
    {
      int num = numGroups != 0 ? r.Next(minSpan, maxSpan + 1) : maxSpan;
      for (int index = 0; index < num; ++index)
      {
        string str = prefix + (object) index;
        AudioMixerGroupController newGroup = this.CreateNewGroup(str, false);
        this.AddChildToParent(newGroup, parent);
        if ((numGroups = numGroups + 1) >= maxGroups)
          break;
        this.BuildTestSetup(r, newGroup, minSpan, maxSpan <= minSpan ? minSpan : maxSpan - 1, maxGroups, str, ref numGroups);
      }
    }

    public void BuildTestSetup(int minSpan, int maxSpan, int maxGroups)
    {
      int numGroups = 0;
      this.DeleteGroups(this.masterGroup.children);
      this.BuildTestSetup(new System.Random(), this.masterGroup, minSpan, maxSpan, maxGroups, "G", ref numGroups);
    }

    public List<AudioMixerGroupController> GetAllAudioGroupsSlow()
    {
      List<AudioMixerGroupController> groups = new List<AudioMixerGroupController>();
      if ((UnityEngine.Object) this.masterGroup != (UnityEngine.Object) null)
        this.GetAllAudioGroupsSlowRecurse(this.masterGroup, ref groups);
      return groups;
    }

    private void GetAllAudioGroupsSlowRecurse(AudioMixerGroupController g, ref List<AudioMixerGroupController> groups)
    {
      groups.Add(g);
      foreach (AudioMixerGroupController child in g.children)
        this.GetAllAudioGroupsSlowRecurse(child, ref groups);
    }

    public bool HasMoreThanOneGroup()
    {
      return this.masterGroup.children.Length > 0;
    }

    private bool IsChildOf(AudioMixerGroupController child, List<AudioMixerGroupController> groups)
    {
      while ((UnityEngine.Object) child != (UnityEngine.Object) null)
      {
        child = this.FindParentGroup(this.masterGroup, child);
        if (groups.Contains(child))
          return true;
      }
      return false;
    }

    public bool AreAnyOfTheGroupsInTheListAncestors(List<AudioMixerGroupController> groups)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioMixerController.\u003CAreAnyOfTheGroupsInTheListAncestors\u003Ec__AnonStoreyE ancestorsCAnonStoreyE = new AudioMixerController.\u003CAreAnyOfTheGroupsInTheListAncestors\u003Ec__AnonStoreyE();
      // ISSUE: reference to a compiler-generated field
      ancestorsCAnonStoreyE.groups = groups;
      // ISSUE: reference to a compiler-generated field
      ancestorsCAnonStoreyE.\u003C\u003Ef__this = this;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return ancestorsCAnonStoreyE.groups.Any<AudioMixerGroupController>(new Func<AudioMixerGroupController, bool>(ancestorsCAnonStoreyE.\u003C\u003Em__4));
    }

    private void RemoveAncestorGroups(List<AudioMixerGroupController> groups)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioMixerController.\u003CRemoveAncestorGroups\u003Ec__AnonStoreyF groupsCAnonStoreyF = new AudioMixerController.\u003CRemoveAncestorGroups\u003Ec__AnonStoreyF();
      // ISSUE: reference to a compiler-generated field
      groupsCAnonStoreyF.groups = groups;
      // ISSUE: reference to a compiler-generated field
      groupsCAnonStoreyF.\u003C\u003Ef__this = this;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      groupsCAnonStoreyF.groups.RemoveAll(new Predicate<AudioMixerGroupController>(groupsCAnonStoreyF.\u003C\u003Em__5));
      // ISSUE: reference to a compiler-generated field
      object.Equals((object) this.AreAnyOfTheGroupsInTheListAncestors(groupsCAnonStoreyF.groups), (object) false);
    }

    private void DestroyExposedParametersContainedInEffect(AudioMixerEffectController effect)
    {
      Undo.RecordObject((UnityEngine.Object) this, "Changed Exposed Parameters");
      foreach (ExposedAudioParameter exposedParameter in this.exposedParameters)
      {
        if (effect.ContainsParameterGUID(exposedParameter.guid))
          this.RemoveExposedParameter(exposedParameter.guid);
      }
    }

    private void DestroyExposedParametersContainedInGroup(AudioMixerGroupController group)
    {
      Undo.RecordObject((UnityEngine.Object) this, "Remove Exposed Parameter");
      foreach (ExposedAudioParameter exposedParameter in this.exposedParameters)
      {
        if (group.GetGUIDForVolume() == exposedParameter.guid || group.GetGUIDForPitch() == exposedParameter.guid)
          this.RemoveExposedParameter(exposedParameter.guid);
      }
    }

    private void DeleteSubGroupRecursive(AudioMixerGroupController group)
    {
      foreach (AudioMixerGroupController child in group.children)
        this.DeleteSubGroupRecursive(child);
      foreach (AudioMixerEffectController effect in group.effects)
      {
        this.DestroyExposedParametersContainedInEffect(effect);
        Undo.DestroyObjectImmediate((UnityEngine.Object) effect);
      }
      this.DestroyExposedParametersContainedInGroup(group);
      Undo.DestroyObjectImmediate((UnityEngine.Object) group);
    }

    private void DeleteGroupsInternal(List<AudioMixerGroupController> groupsToDelete, List<AudioMixerGroupController> allGroups)
    {
      using (List<AudioMixerGroupController>.Enumerator enumerator = allGroups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          IEnumerable<AudioMixerGroupController> mixerGroupControllers = groupsToDelete.Intersect<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) current.children);
          if (mixerGroupControllers.Count<AudioMixerGroupController>() > 0)
          {
            Undo.RegisterCompleteObjectUndo((UnityEngine.Object) current, "Delete Group(s)");
            current.children = ((IEnumerable<AudioMixerGroupController>) current.children).Except<AudioMixerGroupController>(mixerGroupControllers).ToArray<AudioMixerGroupController>();
          }
        }
      }
      using (List<AudioMixerGroupController>.Enumerator enumerator = groupsToDelete.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.DeleteSubGroupRecursive(enumerator.Current);
      }
    }

    public void DeleteGroups(AudioMixerGroupController[] groups)
    {
      List<AudioMixerGroupController> list = ((IEnumerable<AudioMixerGroupController>) groups).ToList<AudioMixerGroupController>();
      this.RemoveAncestorGroups(list);
      this.DeleteGroupsInternal(list, this.GetAllAudioGroupsSlow());
      this.OnUnitySelectionChanged();
    }

    public void RemoveEffect(AudioMixerEffectController effect, AudioMixerGroupController group)
    {
      Undo.RecordObject((UnityEngine.Object) group, "Delete Effect");
      List<AudioMixerEffectController> effectControllerList = new List<AudioMixerEffectController>((IEnumerable<AudioMixerEffectController>) group.effects);
      effectControllerList.Remove(effect);
      group.effects = effectControllerList.ToArray();
      this.DestroyExposedParametersContainedInEffect(effect);
      Undo.DestroyObjectImmediate((UnityEngine.Object) effect);
    }

    public void OnSubAssetChanged()
    {
      AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((UnityEngine.Object) this));
    }

    public void CloneNewSnapshotFromTarget(bool storeUndoState)
    {
      List<AudioMixerSnapshotController> snapshotControllerList = new List<AudioMixerSnapshotController>((IEnumerable<AudioMixerSnapshotController>) this.snapshots);
      AudioMixerSnapshotController snapshotController = UnityEngine.Object.Instantiate<AudioMixerSnapshotController>(this.TargetSnapshot);
      snapshotController.name = this.TargetSnapshot.name + " - Copy";
      snapshotControllerList.Add(snapshotController);
      this.snapshots = snapshotControllerList.ToArray();
      this.TargetSnapshot = snapshotControllerList[snapshotControllerList.Count - 1];
      AssetDatabase.AddObjectToAsset((UnityEngine.Object) snapshotController, (UnityEngine.Object) this);
      if (storeUndoState)
        Undo.RegisterCreatedObjectUndo((UnityEngine.Object) snapshotController, string.Empty);
      this.OnSubAssetChanged();
    }

    public void RemoveTargetSnapshot()
    {
      if (this.snapshots.Length < 2)
        return;
      AudioMixerSnapshotController targetSnapshot = this.TargetSnapshot;
      Undo.RecordObject((UnityEngine.Object) this, "Remove Snapshot");
      List<AudioMixerSnapshotController> snapshotControllerList = new List<AudioMixerSnapshotController>((IEnumerable<AudioMixerSnapshotController>) this.snapshots);
      snapshotControllerList.Remove(targetSnapshot);
      this.snapshots = snapshotControllerList.ToArray();
      Undo.DestroyObjectImmediate((UnityEngine.Object) targetSnapshot);
      this.OnSubAssetChanged();
    }

    public void RemoveSnapshot(AudioMixerSnapshotController snapshot)
    {
      if (this.snapshots.Length < 2)
        return;
      AudioMixerSnapshotController snapshotController = snapshot;
      Undo.RecordObject((UnityEngine.Object) this, "Remove Snapshot");
      List<AudioMixerSnapshotController> snapshotControllerList = new List<AudioMixerSnapshotController>((IEnumerable<AudioMixerSnapshotController>) this.snapshots);
      snapshotControllerList.Remove(snapshotController);
      this.snapshots = snapshotControllerList.ToArray();
      Undo.DestroyObjectImmediate((UnityEngine.Object) snapshotController);
      this.OnSubAssetChanged();
    }

    public AudioMixerGroupController CreateNewGroup(string name, bool storeUndoState)
    {
      AudioMixerGroupController mixerGroupController = new AudioMixerGroupController((AudioMixer) this);
      mixerGroupController.name = name;
      mixerGroupController.PreallocateGUIDs();
      AudioMixerEffectController effect = new AudioMixerEffectController("Attenuation");
      this.AddNewSubAsset((UnityEngine.Object) effect, storeUndoState);
      effect.PreallocateGUIDs();
      mixerGroupController.InsertEffect(effect, 0);
      this.AddNewSubAsset((UnityEngine.Object) mixerGroupController, storeUndoState);
      return mixerGroupController;
    }

    public void AddChildToParent(AudioMixerGroupController child, AudioMixerGroupController parent)
    {
      this.RemoveGroupsFromParent(new AudioMixerGroupController[1]
      {
        child
      }, 0 != 0);
      parent.children = new List<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) parent.children)
      {
        child
      }.ToArray();
    }

    private void AddNewSubAsset(UnityEngine.Object obj, bool storeUndoState)
    {
      AssetDatabase.AddObjectToAsset(obj, (UnityEngine.Object) this);
      if (!storeUndoState)
        return;
      Undo.RegisterCreatedObjectUndo(obj, string.Empty);
    }

    public void RemoveGroupsFromParent(AudioMixerGroupController[] groups, bool storeUndoState)
    {
      List<AudioMixerGroupController> list = ((IEnumerable<AudioMixerGroupController>) groups).ToList<AudioMixerGroupController>();
      this.RemoveAncestorGroups(list);
      if (storeUndoState)
        Undo.RecordObject((UnityEngine.Object) this, "Remove group");
      using (List<AudioMixerGroupController>.Enumerator enumerator1 = list.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          AudioMixerGroupController current1 = enumerator1.Current;
          using (List<AudioMixerGroupController>.Enumerator enumerator2 = this.GetAllAudioGroupsSlow().GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              AudioMixerGroupController current2 = enumerator2.Current;
              List<AudioMixerGroupController> mixerGroupControllerList = new List<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) current2.children);
              if (mixerGroupControllerList.Contains(current1))
                mixerGroupControllerList.Remove(current1);
              if (current2.children.Length != mixerGroupControllerList.Count)
                current2.children = mixerGroupControllerList.ToArray();
            }
          }
        }
      }
    }

    public AudioMixerGroupController FindParentGroup(AudioMixerGroupController node, AudioMixerGroupController group)
    {
      for (int index = 0; index < node.children.Length; ++index)
      {
        if ((UnityEngine.Object) node.children[index] == (UnityEngine.Object) group)
          return node;
        AudioMixerGroupController parentGroup = this.FindParentGroup(node.children[index], group);
        if ((UnityEngine.Object) parentGroup != (UnityEngine.Object) null)
          return parentGroup;
      }
      return (AudioMixerGroupController) null;
    }

    public AudioMixerEffectController CopyEffect(AudioMixerEffectController sourceEffect)
    {
      AudioMixerEffectController effectController = new AudioMixerEffectController(sourceEffect.effectName);
      effectController.name = sourceEffect.name;
      effectController.PreallocateGUIDs();
      MixerParameterDefinition[] effectParameters = MixerEffectDefinitions.GetEffectParameters(sourceEffect.effectName);
      foreach (AudioMixerSnapshotController snapshot in this.snapshots)
      {
        float num;
        if (snapshot.GetValue(sourceEffect.GetGUIDForMixLevel(), out num))
          snapshot.SetValue(effectController.GetGUIDForMixLevel(), num);
        foreach (MixerParameterDefinition parameterDefinition in effectParameters)
        {
          if (snapshot.GetValue(sourceEffect.GetGUIDForParameter(parameterDefinition.name), out num))
            snapshot.SetValue(effectController.GetGUIDForParameter(parameterDefinition.name), num);
        }
      }
      AssetDatabase.AddObjectToAsset((UnityEngine.Object) effectController, (UnityEngine.Object) this);
      return effectController;
    }

    private AudioMixerGroupController DuplicateGroupRecurse(AudioMixerGroupController sourceGroup)
    {
      AudioMixerGroupController group = new AudioMixerGroupController((AudioMixer) this);
      List<AudioMixerEffectController> effectControllerList = new List<AudioMixerEffectController>();
      foreach (AudioMixerEffectController effect in sourceGroup.effects)
        effectControllerList.Add(this.CopyEffect(effect));
      List<AudioMixerGroupController> mixerGroupControllerList = new List<AudioMixerGroupController>();
      foreach (AudioMixerGroupController child in sourceGroup.children)
        mixerGroupControllerList.Add(this.DuplicateGroupRecurse(child));
      group.name = sourceGroup.name + " - Copy";
      group.PreallocateGUIDs();
      group.effects = effectControllerList.ToArray();
      group.children = mixerGroupControllerList.ToArray();
      group.solo = sourceGroup.solo;
      group.mute = sourceGroup.mute;
      group.bypassEffects = sourceGroup.bypassEffects;
      foreach (AudioMixerSnapshotController snapshot in this.snapshots)
      {
        float num;
        if (snapshot.GetValue(sourceGroup.GetGUIDForVolume(), out num))
          snapshot.SetValue(group.GetGUIDForVolume(), num);
        if (snapshot.GetValue(sourceGroup.GetGUIDForPitch(), out num))
          snapshot.SetValue(group.GetGUIDForPitch(), num);
      }
      AssetDatabase.AddObjectToAsset((UnityEngine.Object) group, (UnityEngine.Object) this);
      if (this.CurrentViewContainsGroup(sourceGroup.groupID))
        group.controller.AddGroupToCurrentView(group);
      return group;
    }

    public List<AudioMixerGroupController> DuplicateGroups(AudioMixerGroupController[] sourceGroups)
    {
      List<AudioMixerGroupController> list = ((IEnumerable<AudioMixerGroupController>) sourceGroups).ToList<AudioMixerGroupController>();
      this.RemoveAncestorGroups(list);
      List<AudioMixerGroupController> mixerGroupControllerList = new List<AudioMixerGroupController>();
      using (List<AudioMixerGroupController>.Enumerator enumerator = list.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          AudioMixerGroupController parentGroup = this.FindParentGroup(this.masterGroup, current);
          if ((UnityEngine.Object) parentGroup != (UnityEngine.Object) null && (UnityEngine.Object) current != (UnityEngine.Object) null)
          {
            AudioMixerGroupController mixerGroupController = this.DuplicateGroupRecurse(current);
            parentGroup.children = new List<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) parentGroup.children)
            {
              mixerGroupController
            }.ToArray();
            mixerGroupControllerList.Add(mixerGroupController);
          }
        }
      }
      return mixerGroupControllerList;
    }

    public void CopyEffectSettingsToAllSnapshots(AudioMixerGroupController group, int effectIndex, AudioMixerSnapshotController snapshot, bool includeWetParam)
    {
      AudioMixerSnapshotController[] snapshots = this.snapshots;
      for (int index = 0; index < snapshots.Length; ++index)
      {
        if (!((UnityEngine.Object) snapshots[index] == (UnityEngine.Object) snapshot))
        {
          AudioMixerEffectController effect = group.effects[effectIndex];
          MixerParameterDefinition[] effectParameters = MixerEffectDefinitions.GetEffectParameters(effect.effectName);
          float num;
          if (includeWetParam)
          {
            GUID guidForMixLevel = effect.GetGUIDForMixLevel();
            if (snapshot.GetValue(guidForMixLevel, out num))
              snapshots[index].SetValue(guidForMixLevel, num);
          }
          foreach (MixerParameterDefinition parameterDefinition in effectParameters)
          {
            GUID guidForParameter = effect.GetGUIDForParameter(parameterDefinition.name);
            if (snapshot.GetValue(guidForParameter, out num))
              snapshots[index].SetValue(guidForParameter, num);
          }
        }
      }
    }

    public void CopyAllSettingsToAllSnapshots(AudioMixerGroupController group, AudioMixerSnapshotController snapshot)
    {
      for (int effectIndex = 0; effectIndex < group.effects.Length; ++effectIndex)
        this.CopyEffectSettingsToAllSnapshots(group, effectIndex, snapshot, true);
      AudioMixerSnapshotController[] snapshots = this.snapshots;
      for (int index = 0; index < snapshots.Length; ++index)
      {
        if (!((UnityEngine.Object) snapshots[index] == (UnityEngine.Object) snapshot))
        {
          AudioMixerSnapshotController snapshot1 = snapshots[index];
          group.SetValueForVolume(this, snapshot1, group.GetValueForVolume(this, snapshot));
          group.SetValueForPitch(this, snapshot1, group.GetValueForPitch(this, snapshot));
        }
      }
    }

    public void CopyAttenuationToAllSnapshots(AudioMixerGroupController group, AudioMixerSnapshotController snapshot)
    {
      AudioMixerSnapshotController[] snapshots = this.snapshots;
      for (int index = 0; index < snapshots.Length; ++index)
      {
        if (!((UnityEngine.Object) snapshots[index] == (UnityEngine.Object) snapshot))
        {
          AudioMixerSnapshotController snapshot1 = snapshots[index];
          group.SetValueForVolume(this, snapshot1, group.GetValueForVolume(this, snapshot));
        }
      }
    }

    public void ReparentSelection(AudioMixerGroupController newParent, AudioMixerGroupController insertAfterThisNode, List<AudioMixerGroupController> selection)
    {
      Undo.RecordObject((UnityEngine.Object) newParent, "Change Audio Mixer Group Parent");
      using (List<AudioMixerGroupController>.Enumerator enumerator1 = this.GetAllAudioGroupsSlow().GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          AudioMixerGroupController current1 = enumerator1.Current;
          if (((IEnumerable<AudioMixerGroupController>) current1.children).Intersect<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) selection).Any<AudioMixerGroupController>())
          {
            Undo.RecordObject((UnityEngine.Object) current1, string.Empty);
            List<AudioMixerGroupController> mixerGroupControllerList = new List<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) current1.children);
            using (List<AudioMixerGroupController>.Enumerator enumerator2 = selection.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                AudioMixerGroupController current2 = enumerator2.Current;
                mixerGroupControllerList.Remove(current2);
              }
            }
            current1.children = mixerGroupControllerList.ToArray();
          }
        }
      }
      List<AudioMixerGroupController> mixerGroupControllerList1 = new List<AudioMixerGroupController>((IEnumerable<AudioMixerGroupController>) newParent.children);
      int index = mixerGroupControllerList1.IndexOf(insertAfterThisNode) + 1;
      mixerGroupControllerList1.InsertRange(index, (IEnumerable<AudioMixerGroupController>) selection);
      newParent.children = mixerGroupControllerList1.ToArray();
    }

    public static bool InsertEffect(AudioMixerEffectController effect, ref List<AudioMixerEffectController> targetEffects, int targetIndex)
    {
      if (targetIndex < 0 || targetIndex > targetEffects.Count)
      {
        Debug.LogError((object) ("Inserting effect failed! size: " + (object) targetEffects.Count + " at index: " + (object) targetIndex));
        return false;
      }
      targetEffects.Insert(targetIndex, effect);
      return true;
    }

    public static bool MoveEffect(ref List<AudioMixerEffectController> sourceEffects, int sourceIndex, ref List<AudioMixerEffectController> targetEffects, int targetIndex)
    {
      if (sourceEffects == targetEffects)
      {
        if (targetIndex > sourceIndex)
          --targetIndex;
        if (sourceIndex == targetIndex)
          return false;
      }
      if (sourceIndex < 0 || sourceIndex >= sourceEffects.Count || (targetIndex < 0 || targetIndex > targetEffects.Count))
        return false;
      AudioMixerEffectController effectController = sourceEffects[sourceIndex];
      sourceEffects.RemoveAt(sourceIndex);
      targetEffects.Insert(targetIndex, effectController);
      return true;
    }

    public static string FixNameForPopupMenu(string s)
    {
      return s;
    }

    public void ClearSendConnectionsTo(AudioMixerEffectController sendTarget)
    {
      using (List<AudioMixerGroupController>.Enumerator enumerator = this.GetAllAudioGroupsSlow().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          foreach (AudioMixerEffectController effect in enumerator.Current.effects)
          {
            if (effect.IsSend() && (UnityEngine.Object) effect.sendTarget == (UnityEngine.Object) sendTarget)
            {
              Undo.RecordObject((UnityEngine.Object) effect, "Clear Send target");
              effect.sendTarget = (AudioMixerEffectController) null;
            }
          }
        }
      }
    }

    private static Dictionary<object, AudioMixerController.ConnectionNode> BuildTemporaryGraph(List<AudioMixerGroupController> allGroups, AudioMixerGroupController groupWhoseEffectIsChanged, AudioMixerEffectController effectWhoseTargetIsChanged, AudioMixerEffectController targetToTest, AudioMixerGroupController modifiedGroup1, List<AudioMixerEffectController> modifiedGroupEffects1, AudioMixerGroupController modifiedGroup2, List<AudioMixerEffectController> modifiedGroupEffects2)
    {
      Dictionary<object, AudioMixerController.ConnectionNode> dictionary = new Dictionary<object, AudioMixerController.ConnectionNode>();
      using (List<AudioMixerGroupController>.Enumerator enumerator1 = allGroups.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          AudioMixerGroupController current1 = enumerator1.Current;
          dictionary[(object) current1] = new AudioMixerController.ConnectionNode()
          {
            group = current1,
            effect = (AudioMixerEffectController) null
          };
          object index = (object) current1;
          using (List<AudioMixerEffectController>.Enumerator enumerator2 = (!((UnityEngine.Object) current1 == (UnityEngine.Object) modifiedGroup1) ? (!((UnityEngine.Object) current1 == (UnityEngine.Object) modifiedGroup2) ? ((IEnumerable<AudioMixerEffectController>) current1.effects).ToList<AudioMixerEffectController>() : modifiedGroupEffects2) : modifiedGroupEffects1).GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              AudioMixerEffectController current2 = enumerator2.Current;
              if (!dictionary.ContainsKey((object) current2))
                dictionary[(object) current2] = new AudioMixerController.ConnectionNode();
              dictionary[(object) current2].group = current1;
              dictionary[(object) current2].effect = current2;
              if (!dictionary[index].targets.Contains((object) current2))
                dictionary[index].targets.Add((object) current2);
              AudioMixerEffectController effectController = !((UnityEngine.Object) current1 == (UnityEngine.Object) groupWhoseEffectIsChanged) || !((UnityEngine.Object) effectWhoseTargetIsChanged == (UnityEngine.Object) current2) ? current2.sendTarget : targetToTest;
              if ((UnityEngine.Object) effectController != (UnityEngine.Object) null)
              {
                if (!dictionary.ContainsKey((object) effectController))
                {
                  dictionary[(object) effectController] = new AudioMixerController.ConnectionNode();
                  dictionary[(object) effectController].group = current1;
                  dictionary[(object) effectController].effect = effectController;
                }
                if (!dictionary[(object) current2].targets.Contains((object) effectController))
                  dictionary[(object) current2].targets.Add((object) effectController);
              }
              index = (object) current2;
            }
          }
          dictionary[(object) current1].groupTail = index;
        }
      }
      return dictionary;
    }

    private static void ListTemporaryGraph(Dictionary<object, AudioMixerController.ConnectionNode> graph)
    {
      Debug.Log((object) "Listing temporary graph:");
      int num1 = 0;
      using (Dictionary<object, AudioMixerController.ConnectionNode>.Enumerator enumerator1 = graph.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<object, AudioMixerController.ConnectionNode> current1 = enumerator1.Current;
          Debug.Log((object) string.Format("Node {0}: {1}", (object) num1++, (object) current1.Value.GetDisplayString()));
          int num2 = 0;
          using (List<object>.Enumerator enumerator2 = current1.Value.targets.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              object current2 = enumerator2.Current;
              Debug.Log((object) string.Format("  Target {0}: {1}", (object) num2++, (object) graph[current2].GetDisplayString()));
            }
          }
        }
      }
    }

    private static bool CheckForCycle(object curr, Dictionary<object, AudioMixerController.ConnectionNode> graph, List<AudioMixerController.ConnectionNode> identifiedLoop)
    {
      AudioMixerController.ConnectionNode connectionNode = graph[curr];
      if (connectionNode.visited)
      {
        if (identifiedLoop != null)
        {
          identifiedLoop.Clear();
          identifiedLoop.Add(connectionNode);
        }
        return true;
      }
      connectionNode.visited = true;
      using (List<object>.Enumerator enumerator = connectionNode.targets.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (AudioMixerController.CheckForCycle(enumerator.Current, graph, identifiedLoop))
          {
            connectionNode.visited = false;
            if (identifiedLoop != null)
              identifiedLoop.Add(connectionNode);
            return true;
          }
        }
      }
      connectionNode.visited = false;
      return false;
    }

    public static bool DoesTheTemporaryGraphHaveAnyCycles(List<AudioMixerGroupController> allGroups, List<AudioMixerController.ConnectionNode> identifiedLoop, Dictionary<object, AudioMixerController.ConnectionNode> graph)
    {
      using (List<AudioMixerGroupController>.Enumerator enumerator = allGroups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (AudioMixerController.CheckForCycle((object) enumerator.Current, graph, identifiedLoop))
          {
            if (identifiedLoop != null)
            {
              AudioMixerController.ConnectionNode connectionNode = identifiedLoop[0];
              int index = 1;
              do
                ;
              while (index < identifiedLoop.Count && identifiedLoop[index++] != connectionNode);
              identifiedLoop.RemoveRange(index, identifiedLoop.Count - index);
              identifiedLoop.Reverse();
            }
            return true;
          }
        }
      }
      return false;
    }

    public static bool WillChangeOfEffectTargetCauseFeedback(List<AudioMixerGroupController> allGroups, AudioMixerGroupController groupWhoseEffectIsChanged, int effectWhoseTargetIsChanged, AudioMixerEffectController targetToTest, List<AudioMixerController.ConnectionNode> identifiedLoop)
    {
      Dictionary<object, AudioMixerController.ConnectionNode> graph = AudioMixerController.BuildTemporaryGraph(allGroups, groupWhoseEffectIsChanged, groupWhoseEffectIsChanged.effects[effectWhoseTargetIsChanged], targetToTest, (AudioMixerGroupController) null, (List<AudioMixerEffectController>) null, (AudioMixerGroupController) null, (List<AudioMixerEffectController>) null);
      using (List<AudioMixerGroupController>.Enumerator enumerator = allGroups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          foreach (AudioMixerGroupController child in current.children)
          {
            object groupTail = graph[(object) child].groupTail;
            if (!graph[groupTail].targets.Contains((object) current))
              graph[groupTail].targets.Add((object) current);
          }
        }
      }
      return AudioMixerController.DoesTheTemporaryGraphHaveAnyCycles(allGroups, identifiedLoop, graph);
    }

    public static bool WillModificationOfTopologyCauseFeedback(List<AudioMixerGroupController> allGroups, List<AudioMixerGroupController> groupsToBeMoved, AudioMixerGroupController newParentForMovedGroups, List<AudioMixerController.ConnectionNode> identifiedLoop)
    {
      Dictionary<object, AudioMixerController.ConnectionNode> graph = AudioMixerController.BuildTemporaryGraph(allGroups, (AudioMixerGroupController) null, (AudioMixerEffectController) null, (AudioMixerEffectController) null, (AudioMixerGroupController) null, (List<AudioMixerEffectController>) null, (AudioMixerGroupController) null, (List<AudioMixerEffectController>) null);
      using (List<AudioMixerGroupController>.Enumerator enumerator = allGroups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          foreach (AudioMixerGroupController child in current.children)
          {
            AudioMixerGroupController mixerGroupController = !groupsToBeMoved.Contains(child) ? current : newParentForMovedGroups;
            object groupTail = graph[(object) child].groupTail;
            if (!graph[groupTail].targets.Contains((object) mixerGroupController))
              graph[groupTail].targets.Add((object) mixerGroupController);
          }
        }
      }
      return AudioMixerController.DoesTheTemporaryGraphHaveAnyCycles(allGroups, identifiedLoop, graph);
    }

    public static bool WillMovingEffectCauseFeedback(List<AudioMixerGroupController> allGroups, AudioMixerGroupController sourceGroup, int sourceIndex, AudioMixerGroupController targetGroup, int targetIndex, List<AudioMixerController.ConnectionNode> identifiedLoop)
    {
      Dictionary<object, AudioMixerController.ConnectionNode> graph;
      if ((UnityEngine.Object) sourceGroup == (UnityEngine.Object) targetGroup)
      {
        List<AudioMixerEffectController> list = ((IEnumerable<AudioMixerEffectController>) sourceGroup.effects).ToList<AudioMixerEffectController>();
        if (!AudioMixerController.MoveEffect(ref list, sourceIndex, ref list, targetIndex))
          return false;
        graph = AudioMixerController.BuildTemporaryGraph(allGroups, (AudioMixerGroupController) null, (AudioMixerEffectController) null, (AudioMixerEffectController) null, sourceGroup, list, (AudioMixerGroupController) null, (List<AudioMixerEffectController>) null);
      }
      else
      {
        List<AudioMixerEffectController> list1 = ((IEnumerable<AudioMixerEffectController>) sourceGroup.effects).ToList<AudioMixerEffectController>();
        List<AudioMixerEffectController> list2 = ((IEnumerable<AudioMixerEffectController>) targetGroup.effects).ToList<AudioMixerEffectController>();
        if (!AudioMixerController.MoveEffect(ref list1, sourceIndex, ref list2, targetIndex))
          return false;
        graph = AudioMixerController.BuildTemporaryGraph(allGroups, (AudioMixerGroupController) null, (AudioMixerEffectController) null, (AudioMixerEffectController) null, sourceGroup, list1, targetGroup, list2);
      }
      using (List<AudioMixerGroupController>.Enumerator enumerator = allGroups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          foreach (AudioMixerGroupController child in current.children)
          {
            object groupTail = graph[(object) child].groupTail;
            if (!graph[groupTail].targets.Contains((object) current))
              graph[groupTail].targets.Add((object) current);
          }
        }
      }
      return AudioMixerController.DoesTheTemporaryGraphHaveAnyCycles(allGroups, identifiedLoop, graph);
    }

    public static float DbToLin(float x)
    {
      if ((double) x < (double) AudioMixerController.kMinVolume)
        return 0.0f;
      return Mathf.Pow(10f, x * 0.05f);
    }

    public void CloneViewFromCurrent()
    {
      Undo.RecordObject((UnityEngine.Object) this, "Create view");
      List<MixerGroupView> mixerGroupViewList = new List<MixerGroupView>((IEnumerable<MixerGroupView>) this.views);
      mixerGroupViewList.Add(new MixerGroupView()
      {
        name = this.views[this.currentViewIndex].name + " - Copy",
        guids = this.views[this.currentViewIndex].guids
      });
      this.views = mixerGroupViewList.ToArray();
      this.currentViewIndex = mixerGroupViewList.Count - 1;
    }

    public void DeleteView(int index)
    {
      Undo.RecordObject((UnityEngine.Object) this, "Delete view");
      List<MixerGroupView> mixerGroupViewList = new List<MixerGroupView>((IEnumerable<MixerGroupView>) this.views);
      mixerGroupViewList.RemoveAt(index);
      this.views = mixerGroupViewList.ToArray();
      this.ForceSetView(Mathf.Clamp(this.currentViewIndex, 0, mixerGroupViewList.Count - 1));
    }

    public void SetView(int index)
    {
      if (this.currentViewIndex == index)
        return;
      this.ForceSetView(index);
    }

    public void SanitizeGroupViews()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioMixerController.\u003CSanitizeGroupViews\u003Ec__AnonStorey10 viewsCAnonStorey10 = new AudioMixerController.\u003CSanitizeGroupViews\u003Ec__AnonStorey10();
      // ISSUE: reference to a compiler-generated field
      viewsCAnonStorey10.allGroups = this.GetAllAudioGroupsSlow();
      MixerGroupView[] views = this.views;
      for (int index = 0; index < views.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: object of a compiler-generated type is created
        views[index].guids = ((IEnumerable<GUID>) views[index].guids).SelectMany<GUID, AudioMixerGroupController, \u003C\u003E__AnonType0<GUID, AudioMixerGroupController>>(new Func<GUID, IEnumerable<AudioMixerGroupController>>(viewsCAnonStorey10.\u003C\u003Em__6), (Func<GUID, AudioMixerGroupController, \u003C\u003E__AnonType0<GUID, AudioMixerGroupController>>) ((x, y) => new \u003C\u003E__AnonType0<GUID, AudioMixerGroupController>(x, y))).Where<\u003C\u003E__AnonType0<GUID, AudioMixerGroupController>>((Func<\u003C\u003E__AnonType0<GUID, AudioMixerGroupController>, bool>) (param0 => param0.y.groupID == param0.x)).Select<\u003C\u003E__AnonType0<GUID, AudioMixerGroupController>, GUID>((Func<\u003C\u003E__AnonType0<GUID, AudioMixerGroupController>, GUID>) (param0 => param0.x)).ToArray<GUID>();
      }
      this.views = ((IEnumerable<MixerGroupView>) views).ToArray<MixerGroupView>();
    }

    public void ForceSetView(int index)
    {
      this.currentViewIndex = index;
      this.SanitizeGroupViews();
    }

    public void AddGroupToCurrentView(AudioMixerGroupController group)
    {
      MixerGroupView[] views = this.views;
      List<GUID> list = ((IEnumerable<GUID>) views[this.currentViewIndex].guids).ToList<GUID>();
      list.Add(group.groupID);
      views[this.currentViewIndex].guids = list.ToArray();
      this.views = ((IEnumerable<MixerGroupView>) views).ToArray<MixerGroupView>();
    }

    public void SetCurrentViewVisibility(GUID[] guids)
    {
      MixerGroupView[] views = this.views;
      views[this.currentViewIndex].guids = guids;
      this.views = ((IEnumerable<MixerGroupView>) views).ToArray<MixerGroupView>();
      this.SanitizeGroupViews();
    }

    public AudioMixerGroupController[] GetCurrentViewGroupList()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioMixerController.\u003CGetCurrentViewGroupList\u003Ec__AnonStorey11 listCAnonStorey11 = new AudioMixerController.\u003CGetCurrentViewGroupList\u003Ec__AnonStorey11();
      List<AudioMixerGroupController> allAudioGroupsSlow = this.GetAllAudioGroupsSlow();
      // ISSUE: reference to a compiler-generated field
      listCAnonStorey11.view = this.views[this.currentViewIndex];
      // ISSUE: reference to a compiler-generated method
      return allAudioGroupsSlow.Where<AudioMixerGroupController>(new Func<AudioMixerGroupController, bool>(listCAnonStorey11.\u003C\u003Em__A)).ToArray<AudioMixerGroupController>();
    }

    public static float VolumeToScreenMapping(float value, float screenRange, bool forward)
    {
      float num1 = AudioMixerController.GetVolumeSplitPoint() * screenRange;
      float num2 = screenRange - num1;
      if (forward)
      {
        if ((double) value > 0.0)
          return num1 - Mathf.Pow(value / AudioMixerController.GetMaxVolume(), 1f / AudioMixerController.kVolumeWarp) * num1;
        return Mathf.Pow(value / AudioMixerController.kMinVolume, 1f / AudioMixerController.kVolumeWarp) * num2 + num1;
      }
      if ((double) value < (double) num1)
        return Mathf.Pow((float) (1.0 - (double) value / (double) num1), AudioMixerController.kVolumeWarp) * AudioMixerController.GetMaxVolume();
      return Mathf.Pow((value - num1) / num2, AudioMixerController.kVolumeWarp) * AudioMixerController.kMinVolume;
    }

    public void OnUnitySelectionChanged()
    {
      this.m_CachedSelection = this.GetAllAudioGroupsSlow().Intersect<AudioMixerGroupController>(((IEnumerable<UnityEngine.Object>) Selection.GetFiltered(typeof (AudioMixerGroupController), SelectionMode.Deep)).Select<UnityEngine.Object, AudioMixerGroupController>((Func<UnityEngine.Object, AudioMixerGroupController>) (g => (AudioMixerGroupController) g))).ToList<AudioMixerGroupController>();
    }

    public class ConnectionNode
    {
      public List<object> targets = new List<object>();
      public bool visited;
      public object groupTail;
      public AudioMixerGroupController group;
      public AudioMixerEffectController effect;

      public string GetDisplayString()
      {
        string str = this.group.GetDisplayString();
        if ((UnityEngine.Object) this.effect != (UnityEngine.Object) null)
          str = str + AudioMixerController.s_GroupEffectDisplaySeperator + AudioMixerController.FixNameForPopupMenu(this.effect.effectName);
        return str;
      }
    }
  }
}

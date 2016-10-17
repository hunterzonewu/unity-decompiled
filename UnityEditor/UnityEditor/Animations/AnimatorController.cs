// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.AnimatorController
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.Director;
using UnityEngineInternal;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>The Animator Controller controls animation through layers with state machines, controlled by parameters.</para>
  /// </summary>
  public sealed class AnimatorController : RuntimeAnimatorController
  {
    internal PushUndoIfNeeded undoHandler = new PushUndoIfNeeded(true);
    private const string kControllerExtension = "controller";
    internal System.Action OnAnimatorControllerDirty;
    internal static AnimatorController lastActiveController;
    internal static int lastActiveLayerIndex;

    /// <summary>
    ///   <para>The layers in the controller.</para>
    /// </summary>
    public AnimatorControllerLayer[] layers { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Parameters are used to communicate between scripting and the controller. They are used to drive transitions and blendtrees for example.</para>
    /// </summary>
    public UnityEngine.AnimatorControllerParameter[] parameters { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal bool isAssetBundled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal bool pushUndo
    {
      set
      {
        this.undoHandler.pushUndo = value;
      }
    }

    [Obsolete("parameterCount is obsolete. Use parameters.Length instead.", true)]
    private int parameterCount
    {
      get
      {
        return 0;
      }
    }

    [Obsolete("layerCount is obsolete. Use layers.Length instead.", true)]
    private int layerCount
    {
      get
      {
        return 0;
      }
    }

    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    public AnimatorController()
    {
      AnimatorController.Internal_Create(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create(AnimatorController mono);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern AnimatorController GetEffectiveAnimatorController(Animator animator);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern AnimatorControllerPlayable FindAnimatorControllerPlayable(Animator animator, AnimatorController controller);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetAnimatorController(Animator behavior, AnimatorController controller);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal int IndexOfParameter(string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RenameParameter(string prevName, string newName);

    /// <summary>
    ///   <para>Creates a unique name for the parameter.</para>
    /// </summary>
    /// <param name="name">The desired name of the AnimatorParameter.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string MakeUniqueParameterName(string name);

    /// <summary>
    ///   <para>Creates a unique name for the layers.</para>
    /// </summary>
    /// <param name="name">The desired name of the AnimatorLayer.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string MakeUniqueLayerName(string name);

    /// <summary>
    ///   <para>Use this function to retrieve the owner of this behaviour.</para>
    /// </summary>
    /// <param name="behaviour">The State Machine Behaviour to get context for.</param>
    /// <returns>
    ///   <para>Returns the State Machine Behaviour edition context.</para>
    /// </returns>
    public static StateMachineBehaviourContext[] FindStateMachineBehaviourContext(StateMachineBehaviour behaviour)
    {
      return AnimatorController.Internal_FindStateMachineBehaviourContext((ScriptableObject) behaviour);
    }

    /// <summary>
    ///   <para>This function will create a StateMachineBehaviour instance based on the class define in this script.</para>
    /// </summary>
    /// <param name="script">MonoScript class to instantiate.</param>
    /// <returns>
    ///   <para>Returns instance id of created object, returns 0 if something is not valid.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int CreateStateMachineBehaviour(MonoScript script);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CanAddStateMachineBehaviours();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal MonoScript GetBehaviourMonoScript(AnimatorState state, int layerIndex, int behaviourIndex);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private ScriptableObject Internal_AddStateMachineBehaviourWithType(System.Type stateMachineBehaviourType, AnimatorState state, int layerIndex);

    /// <summary>
    ///   <para>Adds a state machine behaviour class of type stateMachineBehaviourType to the AnimatorState for layer layerIndex. This function should be used when you are dealing with synchronized layer and would like to add a state machine behaviour on a synchronized layer. C# Users can use a generic version.</para>
    /// </summary>
    /// <param name="stateMachineBehaviourType"></param>
    /// <param name="state"></param>
    /// <param name="layerIndex"></param>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    public StateMachineBehaviour AddEffectiveStateMachineBehaviour(System.Type stateMachineBehaviourType, AnimatorState state, int layerIndex)
    {
      return (StateMachineBehaviour) this.Internal_AddStateMachineBehaviourWithType(stateMachineBehaviourType, state, layerIndex);
    }

    public T AddEffectiveStateMachineBehaviour<T>(AnimatorState state, int layerIndex) where T : StateMachineBehaviour
    {
      return this.AddEffectiveStateMachineBehaviour(typeof (T), state, layerIndex) as T;
    }

    public T[] GetBehaviours<T>() where T : StateMachineBehaviour
    {
      return AnimatorController.ConvertStateMachineBehaviour<T>(this.GetBehaviours(typeof (T)));
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal ScriptableObject[] GetBehaviours(System.Type type);

    internal static T[] ConvertStateMachineBehaviour<T>(ScriptableObject[] rawObjects) where T : StateMachineBehaviour
    {
      if (rawObjects == null)
        return (T[]) null;
      T[] objArray = new T[rawObjects.Length];
      for (int index = 0; index < objArray.Length; ++index)
        objArray[index] = (T) rawObjects[index];
      return objArray;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal UnityEngine.Object[] CollectObjectsUsingParameter(string parameterName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void AddStateEffectiveBehaviour(AnimatorState state, int layerIndex, int instanceID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RemoveStateEffectiveBehaviour(AnimatorState state, int layerIndex, int behaviourIndex);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal StateMachineBehaviour[] Internal_GetEffectiveBehaviours(AnimatorState state, int layerIndex);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void Internal_SetEffectiveBehaviours(AnimatorState state, int layerIndex, StateMachineBehaviour[] behaviours);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern StateMachineBehaviourContext[] Internal_FindStateMachineBehaviourContext(ScriptableObject scriptableObject);

    internal string GetDefaultBlendTreeParameter()
    {
      for (int index = 0; index < this.parameters.Length; ++index)
      {
        if (this.parameters[index].type == UnityEngine.AnimatorControllerParameterType.Float)
          return this.parameters[index].name;
      }
      this.AddParameter("Blend", UnityEngine.AnimatorControllerParameterType.Float);
      return "Blend";
    }

    internal static void OnInvalidateAnimatorController(AnimatorController controller)
    {
      if (controller.OnAnimatorControllerDirty == null)
        return;
      controller.OnAnimatorControllerDirty();
    }

    internal AnimatorStateMachine FindEffectiveRootStateMachine(int layerIndex)
    {
      AnimatorControllerLayer layer = this.layers[layerIndex];
      while (layer.syncedLayerIndex != -1)
        layer = this.layers[layer.syncedLayerIndex];
      return layer.stateMachine;
    }

    /// <summary>
    ///   <para>Utility function to add a layer to the controller.</para>
    /// </summary>
    /// <param name="name">The name of the Layer.</param>
    /// <param name="layer">The layer to add.</param>
    public void AddLayer(string name)
    {
      AnimatorControllerLayer layer = new AnimatorControllerLayer();
      layer.name = this.MakeUniqueLayerName(name);
      layer.stateMachine = new AnimatorStateMachine();
      layer.stateMachine.name = layer.name;
      layer.stateMachine.hideFlags = HideFlags.HideInHierarchy;
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != string.Empty)
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) layer.stateMachine, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      this.AddLayer(layer);
    }

    /// <summary>
    ///   <para>Utility function to add a layer to the controller.</para>
    /// </summary>
    /// <param name="name">The name of the Layer.</param>
    /// <param name="layer">The layer to add.</param>
    public void AddLayer(AnimatorControllerLayer layer)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "Layer added");
      AnimatorControllerLayer[] layers = this.layers;
      ArrayUtility.Add<AnimatorControllerLayer>(ref layers, layer);
      this.layers = layers;
    }

    internal void RemoveLayers(List<int> layerIndexes)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "Layers removed");
      AnimatorControllerLayer[] layers = this.layers;
      using (List<int>.Enumerator enumerator = layerIndexes.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.RemoveLayerInternal(enumerator.Current, ref layers);
      }
      this.layers = layers;
    }

    private void RemoveLayerInternal(int index, ref AnimatorControllerLayer[] layerVector)
    {
      if (layerVector[index].syncedLayerIndex == -1 && (UnityEngine.Object) layerVector[index].stateMachine != (UnityEngine.Object) null)
      {
        this.undoHandler.DoUndo((UnityEngine.Object) layerVector[index].stateMachine, "Layer removed");
        layerVector[index].stateMachine.Clear();
        if (MecanimUtilities.AreSameAsset((UnityEngine.Object) this, (UnityEngine.Object) layerVector[index].stateMachine))
          Undo.DestroyObjectImmediate((UnityEngine.Object) layerVector[index].stateMachine);
      }
      ArrayUtility.Remove<AnimatorControllerLayer>(ref layerVector, layerVector[index]);
    }

    /// <summary>
    ///   <para>Utility function to remove a layer from the controller.</para>
    /// </summary>
    /// <param name="index">The index of the AnimatorLayer.</param>
    public void RemoveLayer(int index)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "Layer removed");
      AnimatorControllerLayer[] layers = this.layers;
      this.RemoveLayerInternal(index, ref layers);
      this.layers = layers;
    }

    /// <summary>
    ///   <para>Utility function to add a parameter to the controller.</para>
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="type">The type of the parameter.</param>
    /// <param name="paramater">The parameter to add.</param>
    public void AddParameter(string name, UnityEngine.AnimatorControllerParameterType type)
    {
      this.AddParameter(new UnityEngine.AnimatorControllerParameter()
      {
        name = this.MakeUniqueParameterName(name),
        type = type
      });
    }

    /// <summary>
    ///   <para>Utility function to add a parameter to the controller.</para>
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="type">The type of the parameter.</param>
    /// <param name="paramater">The parameter to add.</param>
    public void AddParameter(UnityEngine.AnimatorControllerParameter paramater)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "Parameter added");
      UnityEngine.AnimatorControllerParameter[] parameters = this.parameters;
      ArrayUtility.Add<UnityEngine.AnimatorControllerParameter>(ref parameters, paramater);
      this.parameters = parameters;
    }

    /// <summary>
    ///   <para>Utility function to remove a parameter from the controller.</para>
    /// </summary>
    /// <param name="index">The index of the AnimatorParameter.</param>
    public void RemoveParameter(int index)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "Parameter removed");
      UnityEngine.AnimatorControllerParameter[] parameters = this.parameters;
      ArrayUtility.Remove<UnityEngine.AnimatorControllerParameter>(ref parameters, parameters[index]);
      this.parameters = parameters;
    }

    public void RemoveParameter(UnityEngine.AnimatorControllerParameter parameter)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "Parameter removed");
      UnityEngine.AnimatorControllerParameter[] parameters = this.parameters;
      ArrayUtility.Remove<UnityEngine.AnimatorControllerParameter>(ref parameters, parameter);
      this.parameters = parameters;
    }

    /// <summary>
    ///   <para>Utility function that creates a new state  with the motion in it.</para>
    /// </summary>
    /// <param name="motion">The Motion that will be in the AnimatorState.</param>
    /// <param name="layerIndex">The layer where the Motion will be added.</param>
    public AnimatorState AddMotion(Motion motion)
    {
      return this.AddMotion(motion, 0);
    }

    /// <summary>
    ///   <para>Utility function that creates a new state  with the motion in it.</para>
    /// </summary>
    /// <param name="motion">The Motion that will be in the AnimatorState.</param>
    /// <param name="layerIndex">The layer where the Motion will be added.</param>
    public AnimatorState AddMotion(Motion motion, int layerIndex)
    {
      AnimatorState animatorState = this.layers[layerIndex].stateMachine.AddState(motion.name);
      animatorState.motion = motion;
      return animatorState;
    }

    public AnimatorState CreateBlendTreeInController(string name, out BlendTree tree)
    {
      return this.CreateBlendTreeInController(name, out tree, 0);
    }

    public AnimatorState CreateBlendTreeInController(string name, out BlendTree tree, int layerIndex)
    {
      tree = new BlendTree();
      tree.name = name;
      BlendTree blendTree = tree;
      string blendTreeParameter = this.GetDefaultBlendTreeParameter();
      tree.blendParameterY = blendTreeParameter;
      string str = blendTreeParameter;
      blendTree.blendParameter = str;
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != string.Empty)
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) tree, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      AnimatorState animatorState = this.layers[layerIndex].stateMachine.AddState(tree.name);
      animatorState.motion = (Motion) tree;
      return animatorState;
    }

    /// <summary>
    ///   <para>Creates an AnimatorController at the given path.</para>
    /// </summary>
    /// <param name="path">The path where the AnimatorController asset will be created.</param>
    /// <returns>
    ///   <para>The created AnimationController or null if an error occured.</para>
    /// </returns>
    public static AnimatorController CreateAnimatorControllerAtPath(string path)
    {
      AnimatorController animatorController = new AnimatorController();
      animatorController.name = Path.GetFileName(path);
      AssetDatabase.CreateAsset((UnityEngine.Object) animatorController, path);
      animatorController.pushUndo = false;
      animatorController.AddLayer("Base Layer");
      animatorController.pushUndo = true;
      return animatorController;
    }

    public static AnimationClip AllocateAnimatorClip(string name)
    {
      AnimationClip animationClip = AnimationWindowUtility.AllocateAndSetupClip(true);
      animationClip.name = name;
      return animationClip;
    }

    internal static AnimatorController CreateAnimatorControllerForClip(AnimationClip clip, GameObject animatedObject)
    {
      string assetPath = AssetDatabase.GetAssetPath((UnityEngine.Object) clip);
      if (string.IsNullOrEmpty(assetPath))
        return (AnimatorController) null;
      string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(FileUtil.DeleteLastPathNameComponent(assetPath), animatedObject.name + ".controller"));
      if (string.IsNullOrEmpty(uniqueAssetPath))
        return (AnimatorController) null;
      return AnimatorController.CreateAnimatorControllerAtPathWithClip(uniqueAssetPath, clip);
    }

    /// <summary>
    ///   <para>Creates an AnimatorController at the given path, and automatically create an AnimatorLayer  with an AnimatorStateMachine that will add a State with the AnimationClip in it.</para>
    /// </summary>
    /// <param name="path">The path where the AnimatorController will be created.</param>
    /// <param name="clip">The default clip that will be played by the AnimatorController.</param>
    public static AnimatorController CreateAnimatorControllerAtPathWithClip(string path, AnimationClip clip)
    {
      AnimatorController controllerAtPath = AnimatorController.CreateAnimatorControllerAtPath(path);
      controllerAtPath.AddMotion((Motion) clip);
      return controllerAtPath;
    }

    /// <summary>
    ///   <para>Sets the effective Motion for the AnimatorState. The Motion is either stored in the AnimatorStateMachine or in the AnimatorLayer's ovverrides. Use this function to set the Motion that is effectively used.</para>
    /// </summary>
    /// <param name="state">The AnimatorState which we want to set the Motion.</param>
    /// <param name="motion">The Motion that will be set.</param>
    /// <param name="layerIndex">The layer to set the Motion.</param>
    public void SetStateEffectiveMotion(AnimatorState state, Motion motion)
    {
      this.SetStateEffectiveMotion(state, motion, 0);
    }

    /// <summary>
    ///   <para>Sets the effective Motion for the AnimatorState. The Motion is either stored in the AnimatorStateMachine or in the AnimatorLayer's ovverrides. Use this function to set the Motion that is effectively used.</para>
    /// </summary>
    /// <param name="state">The AnimatorState which we want to set the Motion.</param>
    /// <param name="motion">The Motion that will be set.</param>
    /// <param name="layerIndex">The layer to set the Motion.</param>
    public void SetStateEffectiveMotion(AnimatorState state, Motion motion, int layerIndex)
    {
      if (this.layers[layerIndex].syncedLayerIndex == -1)
      {
        this.undoHandler.DoUndo((UnityEngine.Object) state, "Set Motion");
        state.motion = motion;
      }
      else
      {
        this.undoHandler.DoUndo((UnityEngine.Object) this, "Set Motion");
        AnimatorControllerLayer[] layers = this.layers;
        layers[layerIndex].SetOverrideMotion(state, motion);
        this.layers = layers;
      }
    }

    /// <summary>
    ///   <para>Gets the effective Motion for the AnimatorState. The Motion is either stored in the AnimatorStateMachine or in the AnimatorLayer's ovverrides. Use this function to get the Motion that is effectively used.</para>
    /// </summary>
    /// <param name="state">The AnimatorState which we want the Motion.</param>
    /// <param name="layerIndex">The layer that is queried.</param>
    public Motion GetStateEffectiveMotion(AnimatorState state)
    {
      return this.GetStateEffectiveMotion(state, 0);
    }

    /// <summary>
    ///   <para>Gets the effective Motion for the AnimatorState. The Motion is either stored in the AnimatorStateMachine or in the AnimatorLayer's ovverrides. Use this function to get the Motion that is effectively used.</para>
    /// </summary>
    /// <param name="state">The AnimatorState which we want the Motion.</param>
    /// <param name="layerIndex">The layer that is queried.</param>
    public Motion GetStateEffectiveMotion(AnimatorState state, int layerIndex)
    {
      if (this.layers[layerIndex].syncedLayerIndex == -1)
        return state.motion;
      return this.layers[layerIndex].GetOverrideMotion(state);
    }

    public void SetStateEffectiveBehaviours(AnimatorState state, int layerIndex, StateMachineBehaviour[] behaviours)
    {
      if (this.layers[layerIndex].syncedLayerIndex == -1)
      {
        this.undoHandler.DoUndo((UnityEngine.Object) state, "Set Behaviours");
        state.behaviours = behaviours;
      }
      else
      {
        this.undoHandler.DoUndo((UnityEngine.Object) this, "Set Behaviours");
        this.Internal_SetEffectiveBehaviours(state, layerIndex, behaviours);
      }
    }

    /// <summary>
    ///   <para>Gets the effective state machine behaviour list for the AnimatorState. Behaviours are either stored in the AnimatorStateMachine or in the AnimatorLayer's ovverrides. Use this function to get Behaviour list that is effectively used.</para>
    /// </summary>
    /// <param name="state">The AnimatorState which we want the Behaviour list.</param>
    /// <param name="layerIndex">The layer that is queried.</param>
    public StateMachineBehaviour[] GetStateEffectiveBehaviours(AnimatorState state, int layerIndex)
    {
      return this.Internal_GetEffectiveBehaviours(state, layerIndex);
    }
  }
}

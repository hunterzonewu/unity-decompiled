// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Director.Playable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Director
{
  /// <summary>
  ///   <para>Playables are customizable runtime objects that can be connected together in a tree to create complex behaviours.</para>
  /// </summary>
  [RequiredByNativeCode]
  public class Playable : IDisposable
  {
    internal IntPtr m_Ptr;
    internal int m_UniqueId;

    /// <summary>
    ///   <para>Current local time for this Playable.</para>
    /// </summary>
    public double time { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Current Experimental.Director.PlayState of this playable. This indicates whether the Playable is currently playing or paused.</para>
    /// </summary>
    public PlayState state { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The count of inputs on the Playable. This count includes slots that aren't connected to anything. This is equivalent to, but much faster than calling GetInputs().Length.</para>
    /// </summary>
    public int inputCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The count of inputs on the Playable. This count includes slots that aren't connected to anything. This is equivalent to, but much faster than calling GetOutputs().Length.</para>
    /// </summary>
    public int outputCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public Playable()
    {
      this.m_Ptr = IntPtr.Zero;
      this.m_UniqueId = this.GenerateUniqueId();
      this.InstantiateEnginePlayable();
    }

    internal Playable(bool callCPPConstructor)
    {
      this.m_Ptr = IntPtr.Zero;
      this.m_UniqueId = this.GenerateUniqueId();
      if (!callCPPConstructor)
        return;
      this.InstantiateEnginePlayable();
    }

    ~Playable()
    {
      this.Dispose(false);
    }

    public static implicit operator bool(Playable exists)
    {
      return !Playable.CompareIntPtr(exists, (Playable) null);
    }

    public static bool operator ==(Playable x, Playable y)
    {
      return Playable.CompareIntPtr(x, y);
    }

    public static bool operator !=(Playable x, Playable y)
    {
      return !Playable.CompareIntPtr(x, y);
    }

    private void Dispose(bool disposing)
    {
      this.ReleaseEnginePlayable();
      this.m_Ptr = IntPtr.Zero;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal int GetUniqueIDInternal();

    public static bool Connect(Playable source, Playable target)
    {
      return Playable.Connect(source, target, -1, -1);
    }

    /// <summary>
    ///   <para>Connects two Playables together.</para>
    /// </summary>
    /// <param name="source">Playable to be used as input.</param>
    /// <param name="target">Playable on which the input will be connected.</param>
    /// <param name="sourceOutputPort">Optional index of the output on the source Playable.</param>
    /// <param name="targetInputPort">Optional index of the input on the target Playable.</param>
    /// <returns>
    ///   <para>Returns false if the operation could not be completed.</para>
    /// </returns>
    public static bool Connect(Playable source, Playable target, int sourceOutputPort, int targetInputPort)
    {
      if (!Playable.CheckPlayableValidity(source, "source") && !Playable.CheckPlayableValidity(target, "target") || source != (Playable) null && !source.CheckInputBounds(sourceOutputPort, true) || !target.CheckInputBounds(targetInputPort, true))
        return false;
      return Playable.ConnectInternal(source, target, sourceOutputPort, targetInputPort);
    }

    /// <summary>
    ///   <para>Disconnects an input from a Playable.</para>
    /// </summary>
    /// <param name="right">Playable from which the input will be disconnected.</param>
    /// <param name="inputPort">Index of the input to disconnect.</param>
    /// <param name="target"></param>
    public static void Disconnect(Playable target, int inputPort)
    {
      if (!Playable.CheckPlayableValidity(target, "target") || !target.CheckInputBounds(inputPort))
        return;
      Playable.DisconnectInternal(target, inputPort);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void ReleaseEnginePlayable();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InstantiateEnginePlayable();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private int GenerateUniqueId();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private bool SetInputWeightInternal(int inputIndex, float weight);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private float GetInputWeightInternal(int inputIndex);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool ConnectInternal(Playable source, Playable target, int sourceOutputPort, int targetInputPort);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void DisconnectInternal(Playable target, int inputPort);

    /// <summary>
    ///   <para>Returns the Playable connected at the specified index.</para>
    /// </summary>
    /// <param name="inputPort">Index of the input.</param>
    /// <returns>
    ///   <para>Playable connected at the index specified, or null if the index is valid but is not connected to anything. This happens if there was once a Playable connected at the index, but was disconnected via Playable::Disconnect.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Playable GetInput(int inputPort);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Playable[] GetInputs();

    /// <summary>
    ///   <para>Safely disconnects all connected inputs and resizes the input array to 0.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ClearInputs();

    /// <summary>
    ///   <para>Returns the Playable connected at the specified output index.</para>
    /// </summary>
    /// <param name="outputPort">Index of the output.</param>
    /// <returns>
    ///   <para>Playable connected at the output index specified, or null if the index is valid but is not connected to anything. This happens if there was once a Playable connected at the index, but was disconnected via Playable::Disconnect.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Playable GetOutput(int outputPort);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Playable[] GetOutputs();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void GetInputsInternal(object list);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void GetOutputsInternal(object list);

    /// <summary>
    ///   <para>Implements IDisposable. Call this method to release the resources allocated by the Playable.</para>
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public override bool Equals(object p)
    {
      return Playable.CompareIntPtr(this, p as Playable);
    }

    public override int GetHashCode()
    {
      return this.m_UniqueId;
    }

    internal static bool CompareIntPtr(Playable lhs, Playable rhs)
    {
      bool flag1 = (object) lhs == null || !Playable.IsNativePlayableAlive(lhs);
      bool flag2 = (object) rhs == null || !Playable.IsNativePlayableAlive(rhs);
      if (flag2 && flag1)
        return true;
      if (flag2)
        return !Playable.IsNativePlayableAlive(lhs);
      if (flag1)
        return !Playable.IsNativePlayableAlive(rhs);
      return lhs.GetUniqueIDInternal() == rhs.GetUniqueIDInternal();
    }

    internal static bool IsNativePlayableAlive(Playable p)
    {
      return p.m_Ptr != IntPtr.Zero;
    }

    internal static bool CheckPlayableValidity(Playable playable, string name)
    {
      if (playable == (Playable) null)
        throw new NullReferenceException("Playable " + name + "is null");
      return true;
    }

    internal bool CheckInputBounds(int inputIndex)
    {
      return this.CheckInputBounds(inputIndex, false);
    }

    internal bool CheckInputBounds(int inputIndex, bool acceptAny)
    {
      if (inputIndex == -1 && acceptAny)
        return true;
      if (inputIndex < 0)
        throw new IndexOutOfRangeException("Index must be greater than 0");
      Playable[] inputs = this.GetInputs();
      if (inputs.Length <= inputIndex)
        throw new IndexOutOfRangeException("inputIndex " + (object) inputIndex + " is greater than the number of available inputs (" + (object) inputs.Length + ").");
      return true;
    }

    /// <summary>
    ///   <para>Get the weight of the Playable at a specified index.</para>
    /// </summary>
    /// <param name="inputIndex">Index of the Playable.</param>
    /// <returns>
    ///   <para>Weight of the input Playable. Returns -1 if there is no input connected at this input index.</para>
    /// </returns>
    public float GetInputWeight(int inputIndex)
    {
      if (this.CheckInputBounds(inputIndex))
        return this.GetInputWeightInternal(inputIndex);
      return -1f;
    }

    /// <summary>
    ///   <para>Set the weight of an input.</para>
    /// </summary>
    /// <param name="inputIndex">Index of the input.</param>
    /// <param name="weight">Weight of the input.</param>
    /// <returns>
    ///   <para>Returns false if there is no input Playable connected at that index.</para>
    /// </returns>
    public bool SetInputWeight(int inputIndex, float weight)
    {
      if (this.CheckInputBounds(inputIndex))
        return this.SetInputWeightInternal(inputIndex, weight);
      return false;
    }

    public void GetInputs(List<Playable> inputList)
    {
      inputList.Clear();
      this.GetInputsInternal((object) inputList);
    }

    public void GetOutputs(List<Playable> outputList)
    {
      outputList.Clear();
      this.GetOutputsInternal((object) outputList);
    }

    /// <summary>
    ///   <para>Prepares the Experimental.Director.Playable tree for the next frame. PrepareFrame is called before the tree is evaluated.</para>
    /// </summary>
    /// <param name="info">Data for the current frame.</param>
    public virtual void PrepareFrame(FrameData info)
    {
    }

    /// <summary>
    ///   <para>Evaluates the Playable with a delta time.</para>
    /// </summary>
    /// <param name="info">The Experimental.Director.FrameData for the current frame.</param>
    /// <param name="playerData">Custom data passed down the tree, specified in DirectorPlayer.Play.</param>
    public virtual void ProcessFrame(FrameData info, object playerData)
    {
    }

    /// <summary>
    ///   <para>Callback called when the current time has changed</para>
    /// </summary>
    /// <param name="localTime">New local time</param>
    public virtual void OnSetTime(float localTime)
    {
    }

    /// <summary>
    ///   <para>Callback called when the PlayState has changed</para>
    /// </summary>
    /// <param name="newState">New PlayState</param>
    public virtual void OnSetPlayState(PlayState newState)
    {
    }
  }
}

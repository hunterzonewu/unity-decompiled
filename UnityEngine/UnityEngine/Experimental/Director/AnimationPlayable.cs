// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Director.AnimationPlayable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.Director
{
  /// <summary>
  ///   <para>Base class for all animation related Playable classes.</para>
  /// </summary>
  public class AnimationPlayable : Playable
  {
    public AnimationPlayable()
      : base(false)
    {
      this.m_Ptr = IntPtr.Zero;
      this.InstantiateEnginePlayable();
    }

    public AnimationPlayable(bool final)
      : base(false)
    {
      this.m_Ptr = IntPtr.Zero;
      if (!final)
        return;
      this.InstantiateEnginePlayable();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InstantiateEnginePlayable();

    /// <summary>
    ///   <para>Adds an AnimationPlayable as an input.</para>
    /// </summary>
    /// <param name="source">A playable to connect.</param>
    /// <returns>
    ///   <para>Returns the index of the port the playable was connected to.</para>
    /// </returns>
    public virtual int AddInput(AnimationPlayable source)
    {
      Playable.Connect((Playable) source, (Playable) this, -1, -1);
      return this.GetInputs().Length - 1;
    }

    /// <summary>
    ///   <para>Sets an AnimationPlayable as an input.</para>
    /// </summary>
    /// <param name="source">AnimationPlayable to be used as input.</param>
    /// <param name="index">Index of the input.</param>
    /// <returns>
    ///   <para>Returns false if the operation could not be completed.</para>
    /// </returns>
    public virtual bool SetInput(AnimationPlayable source, int index)
    {
      if (!this.CheckInputBounds(index))
        return false;
      if (this.GetInputs()[index] != (Playable) null)
        Playable.Disconnect((Playable) this, index);
      return Playable.Connect((Playable) source, (Playable) this, -1, index);
    }

    public virtual bool SetInputs(IEnumerable<AnimationPlayable> sources)
    {
      int length = this.GetInputs().Length;
      for (int inputPort = 0; inputPort < length; ++inputPort)
        Playable.Disconnect((Playable) this, inputPort);
      bool flag = false;
      int num = 0;
      foreach (AnimationPlayable source in sources)
      {
        if (num < length)
          flag |= Playable.Connect((Playable) source, (Playable) this, -1, num);
        else
          flag |= Playable.Connect((Playable) source, (Playable) this, -1, -1);
        this.SetInputWeight(num, 1f);
        ++num;
      }
      for (int inputIndex = num; inputIndex < length; ++inputIndex)
        this.SetInputWeight(inputIndex, 0.0f);
      return flag;
    }

    /// <summary>
    ///   <para>Removes a playable from the list of inputs.</para>
    /// </summary>
    /// <param name="index">Index of the playable to remove.</param>
    /// <param name="playable">AnimationPlayable to remove.</param>
    /// <returns>
    ///   <para>Returns false if the removal could not be removed because it wasn't found.</para>
    /// </returns>
    public virtual bool RemoveInput(int index)
    {
      if (!this.CheckInputBounds(index))
        return false;
      Playable.Disconnect((Playable) this, index);
      return true;
    }

    /// <summary>
    ///   <para>Removes a playable from the list of inputs.</para>
    /// </summary>
    /// <param name="index">Index of the playable to remove.</param>
    /// <param name="playable">AnimationPlayable to remove.</param>
    /// <returns>
    ///   <para>Returns false if the removal could not be removed because it wasn't found.</para>
    /// </returns>
    public virtual bool RemoveInput(AnimationPlayable playable)
    {
      if (!Playable.CheckPlayableValidity((Playable) playable, "playable"))
        return false;
      Playable[] inputs = this.GetInputs();
      for (int inputPort = 0; inputPort < inputs.Length; ++inputPort)
      {
        if (inputs[inputPort] == (Playable) playable)
        {
          Playable.Disconnect((Playable) this, inputPort);
          return true;
        }
      }
      return false;
    }

    /// <summary>
    ///   <para>Disconnects all input playables.</para>
    /// </summary>
    /// <returns>
    ///   <para>Returns false if the removal fails.</para>
    /// </returns>
    public virtual bool RemoveAllInputs()
    {
      foreach (Playable input in this.GetInputs())
        this.RemoveInput(input as AnimationPlayable);
      return true;
    }
  }
}

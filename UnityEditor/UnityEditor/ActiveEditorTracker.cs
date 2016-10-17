// Decompiled with JetBrains decompiler
// Type: UnityEditor.ActiveEditorTracker
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  public sealed class ActiveEditorTracker
  {
    private MonoReloadableIntPtrClear m_Property;

    public Editor[] activeEditors { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool isDirty { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool isLocked { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public InspectorMode inspectorMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public bool hasComponentsWhichCannotBeMultiEdited { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static ActiveEditorTracker sharedTracker
    {
      get
      {
        ActiveEditorTracker sharedTracker = new ActiveEditorTracker();
        ActiveEditorTracker.SetupSharedTracker(sharedTracker);
        return sharedTracker;
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public ActiveEditorTracker();

    ~ActiveEditorTracker()
    {
      this.Dispose();
    }

    public override bool Equals(object o)
    {
      return this.m_Property.m_IntPtr == (o as ActiveEditorTracker).m_Property.m_IntPtr;
    }

    public override int GetHashCode()
    {
      return this.m_Property.m_IntPtr.GetHashCode();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Dispose();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Destroy();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int GetVisible(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetVisible(int index, int visible);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ClearDirty();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void RebuildIfNecessary();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ForceRebuild();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void VerifyModifiedMonoBehaviours();

    [Obsolete("Use Editor.CreateEditor instead")]
    public static Editor MakeCustomEditor(UnityEngine.Object obj)
    {
      return Editor.CreateEditor(obj);
    }

    public static bool HasCustomEditor(UnityEngine.Object obj)
    {
      return CustomEditorAttributes.FindCustomEditorType(obj, false) != null;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetupSharedTracker(ActiveEditorTracker sharedTracker);
  }
}

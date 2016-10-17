// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializedObject
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>SerializedObject and SerializedProperty are classes for editing properties on objects in a completely generic way that automatically handles undo and styling UI for prefabs.</para>
  /// </summary>
  public sealed class SerializedObject
  {
    private IntPtr m_Property;

    /// <summary>
    ///   <para>The inspected object (Read Only).</para>
    /// </summary>
    public UnityEngine.Object targetObject { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The inspected objects (Read Only).</para>
    /// </summary>
    public UnityEngine.Object[] targetObjects { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal bool hasModifiedProperties { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal InspectorMode inspectorMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Does the serialized object represents multiple objects due to multi-object editing? (Read Only)</para>
    /// </summary>
    public bool isEditingMultipleObjects { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Create SerializedObject for inspected object.</para>
    /// </summary>
    /// <param name="obj"></param>
    public SerializedObject(UnityEngine.Object obj)
    {
      this.InternalCreate(new UnityEngine.Object[1]{ obj });
    }

    /// <summary>
    ///   <para>Create SerializedObject for inspected object.</para>
    /// </summary>
    /// <param name="objs"></param>
    public SerializedObject(UnityEngine.Object[] objs)
    {
      this.InternalCreate(objs);
    }

    ~SerializedObject()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InternalCreate(UnityEngine.Object[] monoObjs);

    /// <summary>
    ///   <para>Update serialized object's representation.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Update();

    /// <summary>
    ///   <para>Update hasMultipleDifferentValues cache on the next Update() call.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetIsDifferentCacheDirty();

    /// <summary>
    ///   <para>Update serialized object's representation, only if the object has been modified since the last call to Update or if it is a script.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void UpdateIfDirtyOrScript();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();

    /// <summary>
    ///   <para>Get the first serialized property.</para>
    /// </summary>
    public SerializedProperty GetIterator()
    {
      SerializedProperty iteratorInternal = this.GetIterator_Internal();
      iteratorInternal.m_SerializedObject = this;
      return iteratorInternal;
    }

    /// <summary>
    ///   <para>Find serialized property by name.</para>
    /// </summary>
    /// <param name="propertyPath"></param>
    public SerializedProperty FindProperty(string propertyPath)
    {
      SerializedProperty iteratorInternal = this.GetIterator_Internal();
      iteratorInternal.m_SerializedObject = this;
      if (iteratorInternal.FindPropertyInternal(propertyPath))
        return iteratorInternal;
      return (SerializedProperty) null;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private SerializedProperty GetIterator_Internal();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void Cache(int instanceID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern SerializedObject LoadFromCache(int instanceID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private PropertyModification ExtractPropertyModification(string propertyPath);

    /// <summary>
    ///   <para>Apply property modifications.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool ApplyModifiedProperties();

    /// <summary>
    ///   <para>Applies property modifications without registering an undo operation.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool ApplyModifiedPropertiesWithoutUndo();

    /// <summary>
    ///   <para>Copies a value from a SerializedProperty to the same serialized property on this serialized object.</para>
    /// </summary>
    /// <param name="prop"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void CopyFromSerializedProperty(SerializedProperty prop);
  }
}

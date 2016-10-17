// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializedProperty
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>SerializedProperty and SerializedObject are classes for editing properties on objects in a completely generic way that automatically handles undo and styling UI for prefabs.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public sealed class SerializedProperty
  {
    private IntPtr m_Property;
    internal SerializedObject m_SerializedObject;

    /// <summary>
    ///   <para>SerializedObject this property belongs to (Read Only).</para>
    /// </summary>
    public SerializedObject serializedObject
    {
      get
      {
        return this.m_SerializedObject;
      }
    }

    /// <summary>
    ///   <para>Does this property represent multiple different values due to multi-object editing? (Read Only)</para>
    /// </summary>
    public bool hasMultipleDifferentValues { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal int hasMultipleDifferentValuesBitwise { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Nice display name of the property. (Read Only)</para>
    /// </summary>
    public string displayName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Name of the property. (Read Only)</para>
    /// </summary>
    public string name { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Type name of the property. (Read Only)</para>
    /// </summary>
    public string type { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Tooltip of the property. (Read Only)</para>
    /// </summary>
    public string tooltip { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Nesting depth of the property. (Read Only)</para>
    /// </summary>
    public int depth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Full path of the property. (Read Only)</para>
    /// </summary>
    public string propertyPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal int hashCodeForPropertyPathWithoutArrayIndex { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is this property editable? (Read Only)</para>
    /// </summary>
    public bool editable { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool isAnimated { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is this property expanded in the inspector?</para>
    /// </summary>
    public bool isExpanded { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Does it have child properties? (Read Only)</para>
    /// </summary>
    public bool hasChildren { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Does it have visible child properties? (Read Only)</para>
    /// </summary>
    public bool hasVisibleChildren { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is property part of a prefab instance? (Read Only)</para>
    /// </summary>
    public bool isInstantiatedPrefab { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is property's value different from the prefab it belongs to?</para>
    /// </summary>
    public bool prefabOverride { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Type of this property (Read Only).</para>
    /// </summary>
    public SerializedPropertyType propertyType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Value of an integer property.</para>
    /// </summary>
    public int intValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Value of a integer property as a long.</para>
    /// </summary>
    public long longValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Value of a boolean property.</para>
    /// </summary>
    public bool boolValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Value of a float property.</para>
    /// </summary>
    public float floatValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Value of a float property as a double.</para>
    /// </summary>
    public double doubleValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Value of a string property.</para>
    /// </summary>
    public string stringValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Value of a color property.</para>
    /// </summary>
    public Color colorValue
    {
      get
      {
        Color color;
        this.INTERNAL_get_colorValue(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_colorValue(ref value);
      }
    }

    /// <summary>
    ///   <para>Value of a animation curve property.</para>
    /// </summary>
    public AnimationCurve animationCurveValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal Gradient gradientValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Value of an object reference property.</para>
    /// </summary>
    public UnityEngine.Object objectReferenceValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public int objectReferenceInstanceIDValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal string objectReferenceStringValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal string objectReferenceTypeString { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal string layerMaskStringValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Enum index of an enum property.</para>
    /// </summary>
    public int enumValueIndex { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Names of enumeration of an enum property.</para>
    /// </summary>
    public string[] enumNames { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Display-friendly names of enumeration of an enum property.</para>
    /// </summary>
    public string[] enumDisplayNames { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Value of a 2D vector property.</para>
    /// </summary>
    public Vector2 vector2Value
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_vector2Value(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_vector2Value(ref value);
      }
    }

    /// <summary>
    ///   <para>Value of a 3D vector property.</para>
    /// </summary>
    public Vector3 vector3Value
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_vector3Value(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_vector3Value(ref value);
      }
    }

    /// <summary>
    ///   <para>Value of a 4D vector property.</para>
    /// </summary>
    public Vector4 vector4Value
    {
      get
      {
        Vector4 vector4;
        this.INTERNAL_get_vector4Value(out vector4);
        return vector4;
      }
      set
      {
        this.INTERNAL_set_vector4Value(ref value);
      }
    }

    /// <summary>
    ///   <para>Value of a quaternion property.</para>
    /// </summary>
    public Quaternion quaternionValue
    {
      get
      {
        Quaternion quaternion;
        this.INTERNAL_get_quaternionValue(out quaternion);
        return quaternion;
      }
      set
      {
        this.INTERNAL_set_quaternionValue(ref value);
      }
    }

    /// <summary>
    ///   <para>Value of a rectangle property.</para>
    /// </summary>
    public Rect rectValue
    {
      get
      {
        Rect rect;
        this.INTERNAL_get_rectValue(out rect);
        return rect;
      }
      set
      {
        this.INTERNAL_set_rectValue(ref value);
      }
    }

    /// <summary>
    ///   <para>Value of bounds property.</para>
    /// </summary>
    public Bounds boundsValue
    {
      get
      {
        Bounds bounds;
        this.INTERNAL_get_boundsValue(out bounds);
        return bounds;
      }
      set
      {
        this.INTERNAL_set_boundsValue(ref value);
      }
    }

    /// <summary>
    ///   <para>Is this property an array? (Read Only)</para>
    /// </summary>
    public bool isArray { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The number of elements in the array. If the SerializedObject contains multiple objects it will return the smallest number of elements. So it is always possible to iterate through the SerializedObject and only get properties found in all objects.</para>
    /// </summary>
    public int arraySize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal SerializedProperty()
    {
    }

    ~SerializedProperty()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();

    /// <summary>
    ///   <para>See if contained serialized properties are equal.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool EqualContents(SerializedProperty x, SerializedProperty y);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SetBitAtIndexForAllTargetsImmediate(int index, bool value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_colorValue(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_colorValue(ref Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool ValidateObjectReferenceValue(UnityEngine.Object obj);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void AppendFoldoutPPtrValue(UnityEngine.Object obj);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_vector2Value(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_vector2Value(ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_vector3Value(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_vector3Value(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_vector4Value(out Vector4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_vector4Value(ref Vector4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_quaternionValue(out Quaternion value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_quaternionValue(ref Quaternion value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_rectValue(out Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_rectValue(ref Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_boundsValue(out Bounds value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_boundsValue(ref Bounds value);

    /// <summary>
    ///   <para>Move to next property.</para>
    /// </summary>
    /// <param name="enterChildren"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool Next(bool enterChildren);

    /// <summary>
    ///   <para>Move to next visible property.</para>
    /// </summary>
    /// <param name="enterChildren"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool NextVisible(bool enterChildren);

    /// <summary>
    ///   <para>Move to first property of the object.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Reset();

    /// <summary>
    ///   <para>Count remaining visible properties.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int CountRemaining();

    /// <summary>
    ///   <para>Count visible children of this property, including this property itself.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int CountInProperty();

    /// <summary>
    ///   <para>Returns a copy of the SerializedProperty iterator in its current state. This is useful if you want to keep a reference to the current property but continue with the iteration.</para>
    /// </summary>
    public SerializedProperty Copy()
    {
      SerializedProperty serializedProperty = this.CopyInternal();
      serializedProperty.m_SerializedObject = this.m_SerializedObject;
      return serializedProperty;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private SerializedProperty CopyInternal();

    /// <summary>
    ///   <para>Duplicates the serialized property.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool DuplicateCommand();

    /// <summary>
    ///   <para>Deletes the serialized property.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool DeleteCommand();

    /// <summary>
    ///   <para>Retrieves the SerializedProperty at a relative path to the current property.</para>
    /// </summary>
    /// <param name="relativePropertyPath"></param>
    public SerializedProperty FindPropertyRelative(string relativePropertyPath)
    {
      SerializedProperty serializedProperty = this.Copy();
      if (serializedProperty.FindPropertyRelativeInternal(relativePropertyPath))
        return serializedProperty;
      return (SerializedProperty) null;
    }

    [ExcludeFromDocs]
    public SerializedProperty GetEndProperty()
    {
      return this.GetEndProperty(false);
    }

    /// <summary>
    ///   <para>Retrieves the SerializedProperty that defines the end range of this property.</para>
    /// </summary>
    /// <param name="includeInvisible"></param>
    public SerializedProperty GetEndProperty([DefaultValue("false")] bool includeInvisible)
    {
      SerializedProperty serializedProperty = this.Copy();
      if (includeInvisible)
        serializedProperty.Next(false);
      else
        serializedProperty.NextVisible(false);
      return serializedProperty;
    }

    /// <summary>
    ///   <para>Retrieves an iterator that allows you to iterator over the current nexting of a serialized property.</para>
    /// </summary>
    [DebuggerHidden]
    public IEnumerator GetEnumerator()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SerializedProperty.\u003CGetEnumerator\u003Ec__Iterator1()
      {
        \u003C\u003Ef__this = this
      };
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool FindPropertyInternal(string propertyPath);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool FindPropertyRelativeInternal(string propertyPath);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal int[] GetLayerMaskSelectedIndex();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal string[] GetLayerMaskNames();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void ToggleLayerMaskAtIndex(int index);

    /// <summary>
    ///   <para>Returns the element at the specified index in the array.</para>
    /// </summary>
    /// <param name="index"></param>
    public SerializedProperty GetArrayElementAtIndex(int index)
    {
      SerializedProperty serializedProperty = this.Copy();
      if (serializedProperty.GetArrayElementAtIndexInternal(index))
        return serializedProperty;
      return (SerializedProperty) null;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private bool GetArrayElementAtIndexInternal(int index);

    /// <summary>
    ///   <para>Insert an empty element at the specified index in the array.</para>
    /// </summary>
    /// <param name="index"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InsertArrayElementAtIndex(int index);

    /// <summary>
    ///   <para>Delete the element at the specified index in the array.</para>
    /// </summary>
    /// <param name="index"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void DeleteArrayElementAtIndex(int index);

    /// <summary>
    ///   <para>Remove all elements from the array.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ClearArray();

    /// <summary>
    ///   <para>Move an array element from srcIndex to dstIndex.</para>
    /// </summary>
    /// <param name="srcIndex"></param>
    /// <param name="dstIndex"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool MoveArrayElement(int srcIndex, int dstIndex);

    internal void SetToValueOfTarget(UnityEngine.Object target)
    {
      SerializedProperty property = new SerializedObject(target).FindProperty(this.propertyPath);
      if (property == null)
      {
        UnityEngine.Debug.LogError((object) (target.name + " does not have the property " + this.propertyPath));
      }
      else
      {
        switch (this.propertyType)
        {
          case SerializedPropertyType.Integer:
            this.intValue = property.intValue;
            break;
          case SerializedPropertyType.Boolean:
            this.boolValue = property.boolValue;
            break;
          case SerializedPropertyType.Float:
            this.floatValue = property.floatValue;
            break;
          case SerializedPropertyType.String:
            this.stringValue = property.stringValue;
            break;
          case SerializedPropertyType.Color:
            this.colorValue = property.colorValue;
            break;
          case SerializedPropertyType.ObjectReference:
            this.objectReferenceValue = property.objectReferenceValue;
            break;
          case SerializedPropertyType.LayerMask:
            this.intValue = property.intValue;
            break;
          case SerializedPropertyType.Enum:
            this.enumValueIndex = property.enumValueIndex;
            break;
          case SerializedPropertyType.Vector2:
            this.vector2Value = property.vector2Value;
            break;
          case SerializedPropertyType.Vector3:
            this.vector3Value = property.vector3Value;
            break;
          case SerializedPropertyType.Vector4:
            this.vector4Value = property.vector4Value;
            break;
          case SerializedPropertyType.Rect:
            this.rectValue = property.rectValue;
            break;
          case SerializedPropertyType.ArraySize:
            this.intValue = property.intValue;
            break;
          case SerializedPropertyType.Character:
            this.intValue = property.intValue;
            break;
          case SerializedPropertyType.AnimationCurve:
            this.animationCurveValue = property.animationCurveValue;
            break;
          case SerializedPropertyType.Bounds:
            this.boundsValue = property.boundsValue;
            break;
          case SerializedPropertyType.Gradient:
            this.gradientValue = property.gradientValue;
            break;
        }
      }
    }
  }
}

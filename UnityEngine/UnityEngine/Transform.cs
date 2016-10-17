// Decompiled with JetBrains decompiler
// Type: UnityEngine.Transform
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Position, rotation and scale of an object.</para>
  /// </summary>
  public class Transform : Component, IEnumerable
  {
    /// <summary>
    ///   <para>The position of the transform in world space.</para>
    /// </summary>
    public Vector3 position
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_position(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_position(ref value);
      }
    }

    /// <summary>
    ///   <para>Position of the transform relative to the parent transform.</para>
    /// </summary>
    public Vector3 localPosition
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_localPosition(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_localPosition(ref value);
      }
    }

    /// <summary>
    ///   <para>The rotation as Euler angles in degrees.</para>
    /// </summary>
    public Vector3 eulerAngles
    {
      get
      {
        return this.rotation.eulerAngles;
      }
      set
      {
        this.rotation = Quaternion.Euler(value);
      }
    }

    /// <summary>
    ///   <para>The rotation as Euler angles in degrees relative to the parent transform's rotation.</para>
    /// </summary>
    public Vector3 localEulerAngles
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_localEulerAngles(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_localEulerAngles(ref value);
      }
    }

    /// <summary>
    ///   <para>The red axis of the transform in world space.</para>
    /// </summary>
    public Vector3 right
    {
      get
      {
        return this.rotation * Vector3.right;
      }
      set
      {
        this.rotation = Quaternion.FromToRotation(Vector3.right, value);
      }
    }

    /// <summary>
    ///   <para>The green axis of the transform in world space.</para>
    /// </summary>
    public Vector3 up
    {
      get
      {
        return this.rotation * Vector3.up;
      }
      set
      {
        this.rotation = Quaternion.FromToRotation(Vector3.up, value);
      }
    }

    /// <summary>
    ///   <para>The blue axis of the transform in world space.</para>
    /// </summary>
    public Vector3 forward
    {
      get
      {
        return this.rotation * Vector3.forward;
      }
      set
      {
        this.rotation = Quaternion.LookRotation(value);
      }
    }

    /// <summary>
    ///   <para>The rotation of the transform in world space stored as a Quaternion.</para>
    /// </summary>
    public Quaternion rotation
    {
      get
      {
        Quaternion quaternion;
        this.INTERNAL_get_rotation(out quaternion);
        return quaternion;
      }
      set
      {
        this.INTERNAL_set_rotation(ref value);
      }
    }

    /// <summary>
    ///   <para>The rotation of the transform relative to the parent transform's rotation.</para>
    /// </summary>
    public Quaternion localRotation
    {
      get
      {
        Quaternion quaternion;
        this.INTERNAL_get_localRotation(out quaternion);
        return quaternion;
      }
      set
      {
        this.INTERNAL_set_localRotation(ref value);
      }
    }

    internal RotationOrder rotationOrder { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The scale of the transform relative to the parent.</para>
    /// </summary>
    public Vector3 localScale
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_localScale(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_localScale(ref value);
      }
    }

    /// <summary>
    ///   <para>The parent of the transform.</para>
    /// </summary>
    public Transform parent
    {
      get
      {
        return this.parentInternal;
      }
      set
      {
        if (this is RectTransform)
          Debug.LogWarning((object) "Parent of RectTransform is being set with parent property. Consider using the SetParent method instead, with the worldPositionStays argument set to false. This will retain local orientation and scale rather than world orientation and scale, which can prevent common UI scaling issues.", (Object) this);
        this.parentInternal = value;
      }
    }

    internal Transform parentInternal { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Matrix that transforms a point from world space into local space (Read Only).</para>
    /// </summary>
    public Matrix4x4 worldToLocalMatrix
    {
      get
      {
        Matrix4x4 matrix4x4;
        this.INTERNAL_get_worldToLocalMatrix(out matrix4x4);
        return matrix4x4;
      }
    }

    /// <summary>
    ///   <para>Matrix that transforms a point from local space into world space (Read Only).</para>
    /// </summary>
    public Matrix4x4 localToWorldMatrix
    {
      get
      {
        Matrix4x4 matrix4x4;
        this.INTERNAL_get_localToWorldMatrix(out matrix4x4);
        return matrix4x4;
      }
    }

    /// <summary>
    ///   <para>Returns the topmost transform in the hierarchy.</para>
    /// </summary>
    public Transform root { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The number of children the Transform has.</para>
    /// </summary>
    public int childCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The global scale of the object (Read Only).</para>
    /// </summary>
    public Vector3 lossyScale
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_lossyScale(out vector3);
        return vector3;
      }
    }

    /// <summary>
    ///   <para>Has the transform changed since the last time the flag was set to 'false'?</para>
    /// </summary>
    public bool hasChanged { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    protected Transform()
    {
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_position(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_position(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_localPosition(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_localPosition(ref Vector3 value);

    internal Vector3 GetLocalEulerAngles(RotationOrder order)
    {
      Vector3 vector3;
      Transform.INTERNAL_CALL_GetLocalEulerAngles(this, order, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetLocalEulerAngles(Transform self, RotationOrder order, out Vector3 value);

    internal void SetLocalEulerAngles(Vector3 euler, RotationOrder order)
    {
      Transform.INTERNAL_CALL_SetLocalEulerAngles(this, ref euler, order);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetLocalEulerAngles(Transform self, ref Vector3 euler, RotationOrder order);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_localEulerAngles(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_localEulerAngles(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_rotation(out Quaternion value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_rotation(ref Quaternion value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_localRotation(out Quaternion value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_localRotation(ref Quaternion value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_localScale(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_localScale(ref Vector3 value);

    public void SetParent(Transform parent)
    {
      this.SetParent(parent, true);
    }

    /// <summary>
    ///   <para>Set the parent of the transform.</para>
    /// </summary>
    /// <param name="parent">The parent Transform to use.</param>
    /// <param name="worldPositionStays">If true, the parent-relative position, scale and rotation is modified such that the object keeps the same world space position, rotation and scale as before.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetParent(Transform parent, bool worldPositionStays);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_worldToLocalMatrix(out Matrix4x4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_localToWorldMatrix(out Matrix4x4 value);

    /// <summary>
    ///   <para>Moves the transform in the direction and distance of translation.</para>
    /// </summary>
    /// <param name="translation"></param>
    /// <param name="relativeTo"></param>
    [ExcludeFromDocs]
    public void Translate(Vector3 translation)
    {
      Space relativeTo = Space.Self;
      this.Translate(translation, relativeTo);
    }

    /// <summary>
    ///   <para>Moves the transform in the direction and distance of translation.</para>
    /// </summary>
    /// <param name="translation"></param>
    /// <param name="relativeTo"></param>
    public void Translate(Vector3 translation, [DefaultValue("Space.Self")] Space relativeTo)
    {
      if (relativeTo == Space.World)
        this.position += translation;
      else
        this.position += this.TransformDirection(translation);
    }

    /// <summary>
    ///   <para>Moves the transform by x along the x axis, y along the y axis, and z along the z axis.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="relativeTo"></param>
    [ExcludeFromDocs]
    public void Translate(float x, float y, float z)
    {
      Space relativeTo = Space.Self;
      this.Translate(x, y, z, relativeTo);
    }

    /// <summary>
    ///   <para>Moves the transform by x along the x axis, y along the y axis, and z along the z axis.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="relativeTo"></param>
    public void Translate(float x, float y, float z, [DefaultValue("Space.Self")] Space relativeTo)
    {
      this.Translate(new Vector3(x, y, z), relativeTo);
    }

    /// <summary>
    ///   <para>Moves the transform in the direction and distance of translation.</para>
    /// </summary>
    /// <param name="translation"></param>
    /// <param name="relativeTo"></param>
    public void Translate(Vector3 translation, Transform relativeTo)
    {
      if ((bool) ((Object) relativeTo))
        this.position += relativeTo.TransformDirection(translation);
      else
        this.position += translation;
    }

    /// <summary>
    ///   <para>Moves the transform by x along the x axis, y along the y axis, and z along the z axis.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="relativeTo"></param>
    public void Translate(float x, float y, float z, Transform relativeTo)
    {
      this.Translate(new Vector3(x, y, z), relativeTo);
    }

    /// <summary>
    ///   <para>Applies a rotation of eulerAngles.z degrees around the z axis, eulerAngles.x degrees around the x axis, and eulerAngles.y degrees around the y axis (in that order).</para>
    /// </summary>
    /// <param name="eulerAngles"></param>
    /// <param name="relativeTo"></param>
    [ExcludeFromDocs]
    public void Rotate(Vector3 eulerAngles)
    {
      Space relativeTo = Space.Self;
      this.Rotate(eulerAngles, relativeTo);
    }

    /// <summary>
    ///   <para>Applies a rotation of eulerAngles.z degrees around the z axis, eulerAngles.x degrees around the x axis, and eulerAngles.y degrees around the y axis (in that order).</para>
    /// </summary>
    /// <param name="eulerAngles"></param>
    /// <param name="relativeTo"></param>
    public void Rotate(Vector3 eulerAngles, [DefaultValue("Space.Self")] Space relativeTo)
    {
      Quaternion quaternion = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
      if (relativeTo == Space.Self)
        this.localRotation = this.localRotation * quaternion;
      else
        this.rotation = this.rotation * Quaternion.Inverse(this.rotation) * quaternion * this.rotation;
    }

    /// <summary>
    ///   <para>Applies a rotation of zAngle degrees around the z axis, xAngle degrees around the x axis, and yAngle degrees around the y axis (in that order).</para>
    /// </summary>
    /// <param name="xAngle"></param>
    /// <param name="yAngle"></param>
    /// <param name="zAngle"></param>
    /// <param name="relativeTo"></param>
    [ExcludeFromDocs]
    public void Rotate(float xAngle, float yAngle, float zAngle)
    {
      Space relativeTo = Space.Self;
      this.Rotate(xAngle, yAngle, zAngle, relativeTo);
    }

    /// <summary>
    ///   <para>Applies a rotation of zAngle degrees around the z axis, xAngle degrees around the x axis, and yAngle degrees around the y axis (in that order).</para>
    /// </summary>
    /// <param name="xAngle"></param>
    /// <param name="yAngle"></param>
    /// <param name="zAngle"></param>
    /// <param name="relativeTo"></param>
    public void Rotate(float xAngle, float yAngle, float zAngle, [DefaultValue("Space.Self")] Space relativeTo)
    {
      this.Rotate(new Vector3(xAngle, yAngle, zAngle), relativeTo);
    }

    internal void RotateAroundInternal(Vector3 axis, float angle)
    {
      Transform.INTERNAL_CALL_RotateAroundInternal(this, ref axis, angle);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_RotateAroundInternal(Transform self, ref Vector3 axis, float angle);

    /// <summary>
    ///   <para>Rotates the transform around axis by angle degrees.</para>
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="angle"></param>
    /// <param name="relativeTo"></param>
    [ExcludeFromDocs]
    public void Rotate(Vector3 axis, float angle)
    {
      Space relativeTo = Space.Self;
      this.Rotate(axis, angle, relativeTo);
    }

    /// <summary>
    ///   <para>Rotates the transform around axis by angle degrees.</para>
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="angle"></param>
    /// <param name="relativeTo"></param>
    public void Rotate(Vector3 axis, float angle, [DefaultValue("Space.Self")] Space relativeTo)
    {
      if (relativeTo == Space.Self)
        this.RotateAroundInternal(this.transform.TransformDirection(axis), angle * ((float) Math.PI / 180f));
      else
        this.RotateAroundInternal(axis, angle * ((float) Math.PI / 180f));
    }

    /// <summary>
    ///   <para>Rotates the transform about axis passing through point in world coordinates by angle degrees.</para>
    /// </summary>
    /// <param name="point"></param>
    /// <param name="axis"></param>
    /// <param name="angle"></param>
    public void RotateAround(Vector3 point, Vector3 axis, float angle)
    {
      Vector3 position = this.position;
      Vector3 vector3 = Quaternion.AngleAxis(angle, axis) * (position - point);
      this.position = point + vector3;
      this.RotateAroundInternal(axis, angle * ((float) Math.PI / 180f));
    }

    /// <summary>
    ///   <para>Rotates the transform so the forward vector points at target's current position.</para>
    /// </summary>
    /// <param name="target">Object to point towards.</param>
    /// <param name="worldUp">Vector specifying the upward direction.</param>
    [ExcludeFromDocs]
    public void LookAt(Transform target)
    {
      Vector3 up = Vector3.up;
      this.LookAt(target, up);
    }

    /// <summary>
    ///   <para>Rotates the transform so the forward vector points at target's current position.</para>
    /// </summary>
    /// <param name="target">Object to point towards.</param>
    /// <param name="worldUp">Vector specifying the upward direction.</param>
    public void LookAt(Transform target, [DefaultValue("Vector3.up")] Vector3 worldUp)
    {
      if (!(bool) ((Object) target))
        return;
      this.LookAt(target.position, worldUp);
    }

    /// <summary>
    ///   <para>Rotates the transform so the forward vector points at worldPosition.</para>
    /// </summary>
    /// <param name="worldPosition">Point to look at.</param>
    /// <param name="worldUp">Vector specifying the upward direction.</param>
    public void LookAt(Vector3 worldPosition, [DefaultValue("Vector3.up")] Vector3 worldUp)
    {
      Transform.INTERNAL_CALL_LookAt(this, ref worldPosition, ref worldUp);
    }

    /// <summary>
    ///   <para>Rotates the transform so the forward vector points at worldPosition.</para>
    /// </summary>
    /// <param name="worldPosition">Point to look at.</param>
    /// <param name="worldUp">Vector specifying the upward direction.</param>
    [ExcludeFromDocs]
    public void LookAt(Vector3 worldPosition)
    {
      Vector3 up = Vector3.up;
      Transform.INTERNAL_CALL_LookAt(this, ref worldPosition, ref up);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_LookAt(Transform self, ref Vector3 worldPosition, ref Vector3 worldUp);

    /// <summary>
    ///   <para>Transforms direction from local space to world space.</para>
    /// </summary>
    /// <param name="direction"></param>
    public Vector3 TransformDirection(Vector3 direction)
    {
      Vector3 vector3;
      Transform.INTERNAL_CALL_TransformDirection(this, ref direction, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_TransformDirection(Transform self, ref Vector3 direction, out Vector3 value);

    /// <summary>
    ///   <para>Transforms direction x, y, z from local space to world space.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Vector3 TransformDirection(float x, float y, float z)
    {
      return this.TransformDirection(new Vector3(x, y, z));
    }

    /// <summary>
    ///   <para>Transforms a direction from world space to local space. The opposite of Transform.TransformDirection.</para>
    /// </summary>
    /// <param name="direction"></param>
    public Vector3 InverseTransformDirection(Vector3 direction)
    {
      Vector3 vector3;
      Transform.INTERNAL_CALL_InverseTransformDirection(this, ref direction, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_InverseTransformDirection(Transform self, ref Vector3 direction, out Vector3 value);

    /// <summary>
    ///   <para>Transforms the direction x, y, z from world space to local space. The opposite of Transform.TransformDirection.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Vector3 InverseTransformDirection(float x, float y, float z)
    {
      return this.InverseTransformDirection(new Vector3(x, y, z));
    }

    /// <summary>
    ///   <para>Transforms vector from local space to world space.</para>
    /// </summary>
    /// <param name="vector"></param>
    public Vector3 TransformVector(Vector3 vector)
    {
      Vector3 vector3;
      Transform.INTERNAL_CALL_TransformVector(this, ref vector, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_TransformVector(Transform self, ref Vector3 vector, out Vector3 value);

    /// <summary>
    ///   <para>Transforms vector x, y, z from local space to world space.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Vector3 TransformVector(float x, float y, float z)
    {
      return this.TransformVector(new Vector3(x, y, z));
    }

    /// <summary>
    ///   <para>Transforms a vector from world space to local space. The opposite of Transform.TransformVector.</para>
    /// </summary>
    /// <param name="vector"></param>
    public Vector3 InverseTransformVector(Vector3 vector)
    {
      Vector3 vector3;
      Transform.INTERNAL_CALL_InverseTransformVector(this, ref vector, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_InverseTransformVector(Transform self, ref Vector3 vector, out Vector3 value);

    /// <summary>
    ///   <para>Transforms the vector x, y, z from world space to local space. The opposite of Transform.TransformVector.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Vector3 InverseTransformVector(float x, float y, float z)
    {
      return this.InverseTransformVector(new Vector3(x, y, z));
    }

    /// <summary>
    ///   <para>Transforms position from local space to world space.</para>
    /// </summary>
    /// <param name="position"></param>
    public Vector3 TransformPoint(Vector3 position)
    {
      Vector3 vector3;
      Transform.INTERNAL_CALL_TransformPoint(this, ref position, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_TransformPoint(Transform self, ref Vector3 position, out Vector3 value);

    /// <summary>
    ///   <para>Transforms the position x, y, z from local space to world space.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Vector3 TransformPoint(float x, float y, float z)
    {
      return this.TransformPoint(new Vector3(x, y, z));
    }

    /// <summary>
    ///   <para>Transforms position from world space to local space.</para>
    /// </summary>
    /// <param name="position"></param>
    public Vector3 InverseTransformPoint(Vector3 position)
    {
      Vector3 vector3;
      Transform.INTERNAL_CALL_InverseTransformPoint(this, ref position, out vector3);
      return vector3;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_InverseTransformPoint(Transform self, ref Vector3 position, out Vector3 value);

    /// <summary>
    ///   <para>Transforms the position x, y, z from world space to local space. The opposite of Transform.TransformPoint.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Vector3 InverseTransformPoint(float x, float y, float z)
    {
      return this.InverseTransformPoint(new Vector3(x, y, z));
    }

    /// <summary>
    ///   <para>Unparents all children.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void DetachChildren();

    /// <summary>
    ///   <para>Move the transform to the start of the local transform list.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetAsFirstSibling();

    /// <summary>
    ///   <para>Move the transform to the end of the local transform list.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetAsLastSibling();

    /// <summary>
    ///   <para>Sets the sibling index.</para>
    /// </summary>
    /// <param name="index">Index to set.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetSiblingIndex(int index);

    /// <summary>
    ///   <para>Gets the sibling index.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int GetSiblingIndex();

    /// <summary>
    ///   <para>Finds a child by name and returns it.</para>
    /// </summary>
    /// <param name="name">Name of child to be found.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Transform Find(string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SendTransformChangedScale();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_lossyScale(out Vector3 value);

    /// <summary>
    ///   <para>Is this transform a child of parent?</para>
    /// </summary>
    /// <param name="parent"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool IsChildOf(Transform parent);

    public Transform FindChild(string name)
    {
      return this.Find(name);
    }

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) new Transform.Enumerator(this);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="angle"></param>
    [Obsolete("use Transform.Rotate instead.")]
    public void RotateAround(Vector3 axis, float angle)
    {
      Transform.INTERNAL_CALL_RotateAround(this, ref axis, angle);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_RotateAround(Transform self, ref Vector3 axis, float angle);

    [Obsolete("use Transform.Rotate instead.")]
    public void RotateAroundLocal(Vector3 axis, float angle)
    {
      Transform.INTERNAL_CALL_RotateAroundLocal(this, ref axis, angle);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_RotateAroundLocal(Transform self, ref Vector3 axis, float angle);

    /// <summary>
    ///   <para>Returns a transform child by index.</para>
    /// </summary>
    /// <param name="index">Index of the child transform to return. Must be smaller than Transform.childCount.</param>
    /// <returns>
    ///   <para>Transform child by index.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Transform GetChild(int index);

    [WrapperlessIcall]
    [Obsolete("use Transform.childCount instead.")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int GetChildCount();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool IsNonUniformScaleTransform();

    private sealed class Enumerator : IEnumerator
    {
      private int currentIndex = -1;
      private Transform outer;

      public object Current
      {
        get
        {
          return (object) this.outer.GetChild(this.currentIndex);
        }
      }

      internal Enumerator(Transform outer)
      {
        this.outer = outer;
      }

      public bool MoveNext()
      {
        return ++this.currentIndex < this.outer.childCount;
      }

      public void Reset()
      {
        this.currentIndex = -1;
      }
    }
  }
}

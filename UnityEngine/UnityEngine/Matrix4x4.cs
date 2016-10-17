// Decompiled with JetBrains decompiler
// Type: UnityEngine.Matrix4x4
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A standard 4x4 transformation matrix.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct Matrix4x4
  {
    public float m00;
    public float m10;
    public float m20;
    public float m30;
    public float m01;
    public float m11;
    public float m21;
    public float m31;
    public float m02;
    public float m12;
    public float m22;
    public float m32;
    public float m03;
    public float m13;
    public float m23;
    public float m33;

    public float this[int row, int column]
    {
      get
      {
        return this[row + column * 4];
      }
      set
      {
        this[row + column * 4] = value;
      }
    }

    public float this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.m00;
          case 1:
            return this.m10;
          case 2:
            return this.m20;
          case 3:
            return this.m30;
          case 4:
            return this.m01;
          case 5:
            return this.m11;
          case 6:
            return this.m21;
          case 7:
            return this.m31;
          case 8:
            return this.m02;
          case 9:
            return this.m12;
          case 10:
            return this.m22;
          case 11:
            return this.m32;
          case 12:
            return this.m03;
          case 13:
            return this.m13;
          case 14:
            return this.m23;
          case 15:
            return this.m33;
          default:
            throw new IndexOutOfRangeException("Invalid matrix index!");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.m00 = value;
            break;
          case 1:
            this.m10 = value;
            break;
          case 2:
            this.m20 = value;
            break;
          case 3:
            this.m30 = value;
            break;
          case 4:
            this.m01 = value;
            break;
          case 5:
            this.m11 = value;
            break;
          case 6:
            this.m21 = value;
            break;
          case 7:
            this.m31 = value;
            break;
          case 8:
            this.m02 = value;
            break;
          case 9:
            this.m12 = value;
            break;
          case 10:
            this.m22 = value;
            break;
          case 11:
            this.m32 = value;
            break;
          case 12:
            this.m03 = value;
            break;
          case 13:
            this.m13 = value;
            break;
          case 14:
            this.m23 = value;
            break;
          case 15:
            this.m33 = value;
            break;
          default:
            throw new IndexOutOfRangeException("Invalid matrix index!");
        }
      }
    }

    /// <summary>
    ///   <para>The inverse of this matrix (Read Only).</para>
    /// </summary>
    public Matrix4x4 inverse
    {
      get
      {
        return Matrix4x4.Inverse(this);
      }
    }

    /// <summary>
    ///   <para>Returns the transpose of this matrix (Read Only).</para>
    /// </summary>
    public Matrix4x4 transpose
    {
      get
      {
        return Matrix4x4.Transpose(this);
      }
    }

    /// <summary>
    ///   <para>Is this the identity matrix?</para>
    /// </summary>
    public bool isIdentity { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The determinant of the matrix.</para>
    /// </summary>
    public float determinant
    {
      get
      {
        return Matrix4x4.Determinant(this);
      }
    }

    /// <summary>
    ///   <para>Returns a matrix with all elements set to zero (Read Only).</para>
    /// </summary>
    public static Matrix4x4 zero
    {
      get
      {
        return new Matrix4x4() { m00 = 0.0f, m01 = 0.0f, m02 = 0.0f, m03 = 0.0f, m10 = 0.0f, m11 = 0.0f, m12 = 0.0f, m13 = 0.0f, m20 = 0.0f, m21 = 0.0f, m22 = 0.0f, m23 = 0.0f, m30 = 0.0f, m31 = 0.0f, m32 = 0.0f, m33 = 0.0f };
      }
    }

    /// <summary>
    ///   <para>Returns the identity matrix (Read Only).</para>
    /// </summary>
    public static Matrix4x4 identity
    {
      get
      {
        return new Matrix4x4() { m00 = 1f, m01 = 0.0f, m02 = 0.0f, m03 = 0.0f, m10 = 0.0f, m11 = 1f, m12 = 0.0f, m13 = 0.0f, m20 = 0.0f, m21 = 0.0f, m22 = 1f, m23 = 0.0f, m30 = 0.0f, m31 = 0.0f, m32 = 0.0f, m33 = 1f };
      }
    }

    public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
    {
      return new Matrix4x4() { m00 = (float) ((double) lhs.m00 * (double) rhs.m00 + (double) lhs.m01 * (double) rhs.m10 + (double) lhs.m02 * (double) rhs.m20 + (double) lhs.m03 * (double) rhs.m30), m01 = (float) ((double) lhs.m00 * (double) rhs.m01 + (double) lhs.m01 * (double) rhs.m11 + (double) lhs.m02 * (double) rhs.m21 + (double) lhs.m03 * (double) rhs.m31), m02 = (float) ((double) lhs.m00 * (double) rhs.m02 + (double) lhs.m01 * (double) rhs.m12 + (double) lhs.m02 * (double) rhs.m22 + (double) lhs.m03 * (double) rhs.m32), m03 = (float) ((double) lhs.m00 * (double) rhs.m03 + (double) lhs.m01 * (double) rhs.m13 + (double) lhs.m02 * (double) rhs.m23 + (double) lhs.m03 * (double) rhs.m33), m10 = (float) ((double) lhs.m10 * (double) rhs.m00 + (double) lhs.m11 * (double) rhs.m10 + (double) lhs.m12 * (double) rhs.m20 + (double) lhs.m13 * (double) rhs.m30), m11 = (float) ((double) lhs.m10 * (double) rhs.m01 + (double) lhs.m11 * (double) rhs.m11 + (double) lhs.m12 * (double) rhs.m21 + (double) lhs.m13 * (double) rhs.m31), m12 = (float) ((double) lhs.m10 * (double) rhs.m02 + (double) lhs.m11 * (double) rhs.m12 + (double) lhs.m12 * (double) rhs.m22 + (double) lhs.m13 * (double) rhs.m32), m13 = (float) ((double) lhs.m10 * (double) rhs.m03 + (double) lhs.m11 * (double) rhs.m13 + (double) lhs.m12 * (double) rhs.m23 + (double) lhs.m13 * (double) rhs.m33), m20 = (float) ((double) lhs.m20 * (double) rhs.m00 + (double) lhs.m21 * (double) rhs.m10 + (double) lhs.m22 * (double) rhs.m20 + (double) lhs.m23 * (double) rhs.m30), m21 = (float) ((double) lhs.m20 * (double) rhs.m01 + (double) lhs.m21 * (double) rhs.m11 + (double) lhs.m22 * (double) rhs.m21 + (double) lhs.m23 * (double) rhs.m31), m22 = (float) ((double) lhs.m20 * (double) rhs.m02 + (double) lhs.m21 * (double) rhs.m12 + (double) lhs.m22 * (double) rhs.m22 + (double) lhs.m23 * (double) rhs.m32), m23 = (float) ((double) lhs.m20 * (double) rhs.m03 + (double) lhs.m21 * (double) rhs.m13 + (double) lhs.m22 * (double) rhs.m23 + (double) lhs.m23 * (double) rhs.m33), m30 = (float) ((double) lhs.m30 * (double) rhs.m00 + (double) lhs.m31 * (double) rhs.m10 + (double) lhs.m32 * (double) rhs.m20 + (double) lhs.m33 * (double) rhs.m30), m31 = (float) ((double) lhs.m30 * (double) rhs.m01 + (double) lhs.m31 * (double) rhs.m11 + (double) lhs.m32 * (double) rhs.m21 + (double) lhs.m33 * (double) rhs.m31), m32 = (float) ((double) lhs.m30 * (double) rhs.m02 + (double) lhs.m31 * (double) rhs.m12 + (double) lhs.m32 * (double) rhs.m22 + (double) lhs.m33 * (double) rhs.m32), m33 = (float) ((double) lhs.m30 * (double) rhs.m03 + (double) lhs.m31 * (double) rhs.m13 + (double) lhs.m32 * (double) rhs.m23 + (double) lhs.m33 * (double) rhs.m33) };
    }

    public static Vector4 operator *(Matrix4x4 lhs, Vector4 v)
    {
      Vector4 vector4;
      vector4.x = (float) ((double) lhs.m00 * (double) v.x + (double) lhs.m01 * (double) v.y + (double) lhs.m02 * (double) v.z + (double) lhs.m03 * (double) v.w);
      vector4.y = (float) ((double) lhs.m10 * (double) v.x + (double) lhs.m11 * (double) v.y + (double) lhs.m12 * (double) v.z + (double) lhs.m13 * (double) v.w);
      vector4.z = (float) ((double) lhs.m20 * (double) v.x + (double) lhs.m21 * (double) v.y + (double) lhs.m22 * (double) v.z + (double) lhs.m23 * (double) v.w);
      vector4.w = (float) ((double) lhs.m30 * (double) v.x + (double) lhs.m31 * (double) v.y + (double) lhs.m32 * (double) v.z + (double) lhs.m33 * (double) v.w);
      return vector4;
    }

    public static bool operator ==(Matrix4x4 lhs, Matrix4x4 rhs)
    {
      if (lhs.GetColumn(0) == rhs.GetColumn(0) && lhs.GetColumn(1) == rhs.GetColumn(1) && lhs.GetColumn(2) == rhs.GetColumn(2))
        return lhs.GetColumn(3) == rhs.GetColumn(3);
      return false;
    }

    public static bool operator !=(Matrix4x4 lhs, Matrix4x4 rhs)
    {
      return !(lhs == rhs);
    }

    public override int GetHashCode()
    {
      return this.GetColumn(0).GetHashCode() ^ this.GetColumn(1).GetHashCode() << 2 ^ this.GetColumn(2).GetHashCode() >> 2 ^ this.GetColumn(3).GetHashCode() >> 1;
    }

    public override bool Equals(object other)
    {
      if (!(other is Matrix4x4))
        return false;
      Matrix4x4 matrix4x4 = (Matrix4x4) other;
      if (this.GetColumn(0).Equals((object) matrix4x4.GetColumn(0)) && this.GetColumn(1).Equals((object) matrix4x4.GetColumn(1)) && this.GetColumn(2).Equals((object) matrix4x4.GetColumn(2)))
        return this.GetColumn(3).Equals((object) matrix4x4.GetColumn(3));
      return false;
    }

    public static Matrix4x4 Inverse(Matrix4x4 m)
    {
      Matrix4x4 matrix4x4;
      Matrix4x4.INTERNAL_CALL_Inverse(ref m, out matrix4x4);
      return matrix4x4;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Inverse(ref Matrix4x4 m, out Matrix4x4 value);

    public static Matrix4x4 Transpose(Matrix4x4 m)
    {
      Matrix4x4 matrix4x4;
      Matrix4x4.INTERNAL_CALL_Transpose(ref m, out matrix4x4);
      return matrix4x4;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Transpose(ref Matrix4x4 m, out Matrix4x4 value);

    internal static bool Invert(Matrix4x4 inMatrix, out Matrix4x4 dest)
    {
      return Matrix4x4.INTERNAL_CALL_Invert(ref inMatrix, out dest);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_Invert(ref Matrix4x4 inMatrix, out Matrix4x4 dest);

    public static float Determinant(Matrix4x4 m)
    {
      return Matrix4x4.INTERNAL_CALL_Determinant(ref m);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float INTERNAL_CALL_Determinant(ref Matrix4x4 m);

    /// <summary>
    ///   <para>Get a column of the matrix.</para>
    /// </summary>
    /// <param name="i"></param>
    public Vector4 GetColumn(int i)
    {
      return new Vector4(this[0, i], this[1, i], this[2, i], this[3, i]);
    }

    /// <summary>
    ///   <para>Returns a row of the matrix.</para>
    /// </summary>
    /// <param name="i"></param>
    public Vector4 GetRow(int i)
    {
      return new Vector4(this[i, 0], this[i, 1], this[i, 2], this[i, 3]);
    }

    /// <summary>
    ///   <para>Sets a column of the matrix.</para>
    /// </summary>
    /// <param name="i"></param>
    /// <param name="v"></param>
    public void SetColumn(int i, Vector4 v)
    {
      this[0, i] = v.x;
      this[1, i] = v.y;
      this[2, i] = v.z;
      this[3, i] = v.w;
    }

    /// <summary>
    ///   <para>Sets a row of the matrix.</para>
    /// </summary>
    /// <param name="i"></param>
    /// <param name="v"></param>
    public void SetRow(int i, Vector4 v)
    {
      this[i, 0] = v.x;
      this[i, 1] = v.y;
      this[i, 2] = v.z;
      this[i, 3] = v.w;
    }

    /// <summary>
    ///   <para>Transforms a position by this matrix (generic).</para>
    /// </summary>
    /// <param name="v"></param>
    public Vector3 MultiplyPoint(Vector3 v)
    {
      Vector3 vector3;
      vector3.x = (float) ((double) this.m00 * (double) v.x + (double) this.m01 * (double) v.y + (double) this.m02 * (double) v.z) + this.m03;
      vector3.y = (float) ((double) this.m10 * (double) v.x + (double) this.m11 * (double) v.y + (double) this.m12 * (double) v.z) + this.m13;
      vector3.z = (float) ((double) this.m20 * (double) v.x + (double) this.m21 * (double) v.y + (double) this.m22 * (double) v.z) + this.m23;
      float num = 1f / ((float) ((double) this.m30 * (double) v.x + (double) this.m31 * (double) v.y + (double) this.m32 * (double) v.z) + this.m33);
      vector3.x *= num;
      vector3.y *= num;
      vector3.z *= num;
      return vector3;
    }

    /// <summary>
    ///   <para>Transforms a position by this matrix (fast).</para>
    /// </summary>
    /// <param name="v"></param>
    public Vector3 MultiplyPoint3x4(Vector3 v)
    {
      Vector3 vector3;
      vector3.x = (float) ((double) this.m00 * (double) v.x + (double) this.m01 * (double) v.y + (double) this.m02 * (double) v.z) + this.m03;
      vector3.y = (float) ((double) this.m10 * (double) v.x + (double) this.m11 * (double) v.y + (double) this.m12 * (double) v.z) + this.m13;
      vector3.z = (float) ((double) this.m20 * (double) v.x + (double) this.m21 * (double) v.y + (double) this.m22 * (double) v.z) + this.m23;
      return vector3;
    }

    /// <summary>
    ///   <para>Transforms a direction by this matrix.</para>
    /// </summary>
    /// <param name="v"></param>
    public Vector3 MultiplyVector(Vector3 v)
    {
      Vector3 vector3;
      vector3.x = (float) ((double) this.m00 * (double) v.x + (double) this.m01 * (double) v.y + (double) this.m02 * (double) v.z);
      vector3.y = (float) ((double) this.m10 * (double) v.x + (double) this.m11 * (double) v.y + (double) this.m12 * (double) v.z);
      vector3.z = (float) ((double) this.m20 * (double) v.x + (double) this.m21 * (double) v.y + (double) this.m22 * (double) v.z);
      return vector3;
    }

    /// <summary>
    ///   <para>Creates a scaling matrix.</para>
    /// </summary>
    /// <param name="v"></param>
    public static Matrix4x4 Scale(Vector3 v)
    {
      return new Matrix4x4() { m00 = v.x, m01 = 0.0f, m02 = 0.0f, m03 = 0.0f, m10 = 0.0f, m11 = v.y, m12 = 0.0f, m13 = 0.0f, m20 = 0.0f, m21 = 0.0f, m22 = v.z, m23 = 0.0f, m30 = 0.0f, m31 = 0.0f, m32 = 0.0f, m33 = 1f };
    }

    /// <summary>
    ///   <para>Sets this matrix to a translation, rotation and scaling matrix.</para>
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="q"></param>
    /// <param name="s"></param>
    public void SetTRS(Vector3 pos, Quaternion q, Vector3 s)
    {
      this = Matrix4x4.TRS(pos, q, s);
    }

    /// <summary>
    ///   <para>Creates a translation, rotation and scaling matrix.</para>
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="q"></param>
    /// <param name="s"></param>
    public static Matrix4x4 TRS(Vector3 pos, Quaternion q, Vector3 s)
    {
      Matrix4x4 matrix4x4;
      Matrix4x4.INTERNAL_CALL_TRS(ref pos, ref q, ref s, out matrix4x4);
      return matrix4x4;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_TRS(ref Vector3 pos, ref Quaternion q, ref Vector3 s, out Matrix4x4 value);

    /// <summary>
    ///   <para>Returns a nicely formatted string for this matrix.</para>
    /// </summary>
    /// <param name="format"></param>
    public override string ToString()
    {
      return UnityString.Format("{0:F5}\t{1:F5}\t{2:F5}\t{3:F5}\n{4:F5}\t{5:F5}\t{6:F5}\t{7:F5}\n{8:F5}\t{9:F5}\t{10:F5}\t{11:F5}\n{12:F5}\t{13:F5}\t{14:F5}\t{15:F5}\n", (object) this.m00, (object) this.m01, (object) this.m02, (object) this.m03, (object) this.m10, (object) this.m11, (object) this.m12, (object) this.m13, (object) this.m20, (object) this.m21, (object) this.m22, (object) this.m23, (object) this.m30, (object) this.m31, (object) this.m32, (object) this.m33);
    }

    /// <summary>
    ///   <para>Returns a nicely formatted string for this matrix.</para>
    /// </summary>
    /// <param name="format"></param>
    public string ToString(string format)
    {
      return UnityString.Format("{0}\t{1}\t{2}\t{3}\n{4}\t{5}\t{6}\t{7}\n{8}\t{9}\t{10}\t{11}\n{12}\t{13}\t{14}\t{15}\n", (object) this.m00.ToString(format), (object) this.m01.ToString(format), (object) this.m02.ToString(format), (object) this.m03.ToString(format), (object) this.m10.ToString(format), (object) this.m11.ToString(format), (object) this.m12.ToString(format), (object) this.m13.ToString(format), (object) this.m20.ToString(format), (object) this.m21.ToString(format), (object) this.m22.ToString(format), (object) this.m23.ToString(format), (object) this.m30.ToString(format), (object) this.m31.ToString(format), (object) this.m32.ToString(format), (object) this.m33.ToString(format));
    }

    /// <summary>
    ///   <para>Creates an orthogonal projection matrix.</para>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="bottom"></param>
    /// <param name="top"></param>
    /// <param name="zNear"></param>
    /// <param name="zFar"></param>
    public static Matrix4x4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
    {
      Matrix4x4 matrix4x4;
      Matrix4x4.INTERNAL_CALL_Ortho(left, right, bottom, top, zNear, zFar, out matrix4x4);
      return matrix4x4;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Ortho(float left, float right, float bottom, float top, float zNear, float zFar, out Matrix4x4 value);

    /// <summary>
    ///   <para>Creates a perspective projection matrix.</para>
    /// </summary>
    /// <param name="fov"></param>
    /// <param name="aspect"></param>
    /// <param name="zNear"></param>
    /// <param name="zFar"></param>
    public static Matrix4x4 Perspective(float fov, float aspect, float zNear, float zFar)
    {
      Matrix4x4 matrix4x4;
      Matrix4x4.INTERNAL_CALL_Perspective(fov, aspect, zNear, zFar, out matrix4x4);
      return matrix4x4;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Perspective(float fov, float aspect, float zNear, float zFar, out Matrix4x4 value);
  }
}

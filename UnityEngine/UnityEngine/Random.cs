// Decompiled with JetBrains decompiler
// Type: UnityEngine.Random
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class for generating random data.</para>
  /// </summary>
  public sealed class Random
  {
    /// <summary>
    ///   <para>Sets the seed for the random number generator.</para>
    /// </summary>
    public static extern int seed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns a random number between 0.0 [inclusive] and 1.0 [inclusive] (Read Only).</para>
    /// </summary>
    public static extern float value { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns a random point inside a sphere with radius 1 (Read Only).</para>
    /// </summary>
    public static Vector3 insideUnitSphere
    {
      get
      {
        Vector3 vector3;
        Random.INTERNAL_get_insideUnitSphere(out vector3);
        return vector3;
      }
    }

    /// <summary>
    ///   <para>Returns a random point inside a circle with radius 1 (Read Only).</para>
    /// </summary>
    public static Vector2 insideUnitCircle
    {
      get
      {
        Vector2 output;
        Random.GetRandomUnitCircle(out output);
        return output;
      }
    }

    /// <summary>
    ///   <para>Returns a random point on the surface of a sphere with radius 1 (Read Only).</para>
    /// </summary>
    public static Vector3 onUnitSphere
    {
      get
      {
        Vector3 vector3;
        Random.INTERNAL_get_onUnitSphere(out vector3);
        return vector3;
      }
    }

    /// <summary>
    ///   <para>Returns a random rotation (Read Only).</para>
    /// </summary>
    public static Quaternion rotation
    {
      get
      {
        Quaternion quaternion;
        Random.INTERNAL_get_rotation(out quaternion);
        return quaternion;
      }
    }

    /// <summary>
    ///   <para>Returns a random rotation with uniform distribution (Read Only).</para>
    /// </summary>
    public static Quaternion rotationUniform
    {
      get
      {
        Quaternion quaternion;
        Random.INTERNAL_get_rotationUniform(out quaternion);
        return quaternion;
      }
    }

    /// <summary>
    ///   <para>Returns a random float number between and min [inclusive] and max [inclusive] (Read Only).</para>
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Range(float min, float max);

    /// <summary>
    ///   <para>Returns a random integer number between min [inclusive] and max [exclusive] (Read Only).</para>
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public static int Range(int min, int max)
    {
      return Random.RandomRangeInt(min, max);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int RandomRangeInt(int min, int max);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_insideUnitSphere(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetRandomUnitCircle(out Vector2 output);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_onUnitSphere(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_rotation(out Quaternion value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_rotationUniform(out Quaternion value);

    [Obsolete("Use Random.Range instead")]
    public static float RandomRange(float min, float max)
    {
      return Random.Range(min, max);
    }

    [Obsolete("Use Random.Range instead")]
    public static int RandomRange(int min, int max)
    {
      return Random.Range(min, max);
    }

    /// <summary>
    ///   <para>Generates a random color from HSV and alpha ranges.</para>
    /// </summary>
    /// <param name="hueMin">Minimum hue [0..1].</param>
    /// <param name="hueMax">Maximum hue [0..1].</param>
    /// <param name="saturationMin">Minimum saturation [0..1].</param>
    /// <param name="saturationMax">Maximum saturation[0..1].</param>
    /// <param name="valueMin">Minimum value [0..1].</param>
    /// <param name="valueMax">Maximum value [0..1].</param>
    /// <param name="alphaMin">Minimum alpha [0..1].</param>
    /// <param name="alphaMax">Maximum alpha [0..1].</param>
    /// <returns>
    ///   <para>A random color with HSV and alpha values in the input ranges.</para>
    /// </returns>
    public static Color ColorHSV()
    {
      return Random.ColorHSV(0.0f, 1f, 0.0f, 1f, 0.0f, 1f, 1f, 1f);
    }

    /// <summary>
    ///   <para>Generates a random color from HSV and alpha ranges.</para>
    /// </summary>
    /// <param name="hueMin">Minimum hue [0..1].</param>
    /// <param name="hueMax">Maximum hue [0..1].</param>
    /// <param name="saturationMin">Minimum saturation [0..1].</param>
    /// <param name="saturationMax">Maximum saturation[0..1].</param>
    /// <param name="valueMin">Minimum value [0..1].</param>
    /// <param name="valueMax">Maximum value [0..1].</param>
    /// <param name="alphaMin">Minimum alpha [0..1].</param>
    /// <param name="alphaMax">Maximum alpha [0..1].</param>
    /// <returns>
    ///   <para>A random color with HSV and alpha values in the input ranges.</para>
    /// </returns>
    public static Color ColorHSV(float hueMin, float hueMax)
    {
      return Random.ColorHSV(hueMin, hueMax, 0.0f, 1f, 0.0f, 1f, 1f, 1f);
    }

    /// <summary>
    ///   <para>Generates a random color from HSV and alpha ranges.</para>
    /// </summary>
    /// <param name="hueMin">Minimum hue [0..1].</param>
    /// <param name="hueMax">Maximum hue [0..1].</param>
    /// <param name="saturationMin">Minimum saturation [0..1].</param>
    /// <param name="saturationMax">Maximum saturation[0..1].</param>
    /// <param name="valueMin">Minimum value [0..1].</param>
    /// <param name="valueMax">Maximum value [0..1].</param>
    /// <param name="alphaMin">Minimum alpha [0..1].</param>
    /// <param name="alphaMax">Maximum alpha [0..1].</param>
    /// <returns>
    ///   <para>A random color with HSV and alpha values in the input ranges.</para>
    /// </returns>
    public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax)
    {
      return Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, 0.0f, 1f, 1f, 1f);
    }

    /// <summary>
    ///   <para>Generates a random color from HSV and alpha ranges.</para>
    /// </summary>
    /// <param name="hueMin">Minimum hue [0..1].</param>
    /// <param name="hueMax">Maximum hue [0..1].</param>
    /// <param name="saturationMin">Minimum saturation [0..1].</param>
    /// <param name="saturationMax">Maximum saturation[0..1].</param>
    /// <param name="valueMin">Minimum value [0..1].</param>
    /// <param name="valueMax">Maximum value [0..1].</param>
    /// <param name="alphaMin">Minimum alpha [0..1].</param>
    /// <param name="alphaMax">Maximum alpha [0..1].</param>
    /// <returns>
    ///   <para>A random color with HSV and alpha values in the input ranges.</para>
    /// </returns>
    public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax)
    {
      return Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, 1f, 1f);
    }

    /// <summary>
    ///   <para>Generates a random color from HSV and alpha ranges.</para>
    /// </summary>
    /// <param name="hueMin">Minimum hue [0..1].</param>
    /// <param name="hueMax">Maximum hue [0..1].</param>
    /// <param name="saturationMin">Minimum saturation [0..1].</param>
    /// <param name="saturationMax">Maximum saturation[0..1].</param>
    /// <param name="valueMin">Minimum value [0..1].</param>
    /// <param name="valueMax">Maximum value [0..1].</param>
    /// <param name="alphaMin">Minimum alpha [0..1].</param>
    /// <param name="alphaMax">Maximum alpha [0..1].</param>
    /// <returns>
    ///   <para>A random color with HSV and alpha values in the input ranges.</para>
    /// </returns>
    public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax)
    {
      Color rgb = Color.HSVToRGB(Mathf.Lerp(hueMin, hueMax, Random.value), Mathf.Lerp(saturationMin, saturationMax, Random.value), Mathf.Lerp(valueMin, valueMax, Random.value), true);
      rgb.a = Mathf.Lerp(alphaMin, alphaMax, Random.value);
      return rgb;
    }
  }
}

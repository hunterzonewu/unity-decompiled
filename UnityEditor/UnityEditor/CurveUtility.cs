// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal static class CurveUtility
  {
    private const int kBrokenMask = 1;
    private const int kLeftTangentMask = 6;
    private const int kRightTangentMask = 24;
    private static Texture2D iconKey;
    private static Texture2D iconCurve;
    private static Texture2D iconNone;

    public static int GetPathAndTypeID(string path, System.Type type)
    {
      return path.GetHashCode() * 27 ^ type.GetHashCode();
    }

    public static int GetCurveID(AnimationClip clip, EditorCurveBinding curveData)
    {
      return (!((UnityEngine.Object) clip == (UnityEngine.Object) null) ? clip.GetInstanceID() : 0) * 19603 ^ curveData.path.GetHashCode() * 729 ^ curveData.type.GetHashCode() * 27 ^ curveData.propertyName.GetHashCode();
    }

    public static int GetCurveGroupID(AnimationClip clip, EditorCurveBinding curveData)
    {
      if (curveData.type != typeof (Transform))
        return -1;
      int num = !((UnityEngine.Object) clip == (UnityEngine.Object) null) ? clip.GetInstanceID() : 0;
      string str = curveData.propertyName.Substring(0, curveData.propertyName.Length - 1);
      return num * 19603 ^ curveData.path.GetHashCode() * 729 ^ curveData.type.GetHashCode() * 27 ^ str.GetHashCode();
    }

    public static Texture2D GetIconCurve()
    {
      if ((UnityEngine.Object) CurveUtility.iconCurve == (UnityEngine.Object) null)
        CurveUtility.iconCurve = EditorGUIUtility.LoadIcon("animationanimated");
      return CurveUtility.iconCurve;
    }

    public static Texture2D GetIconKey()
    {
      if ((UnityEngine.Object) CurveUtility.iconKey == (UnityEngine.Object) null)
        CurveUtility.iconKey = EditorGUIUtility.LoadIcon("animationkeyframe");
      return CurveUtility.iconKey;
    }

    public static bool HaveKeysInRange(AnimationCurve curve, float beginTime, float endTime)
    {
      for (int index = curve.length - 1; index >= 0; --index)
      {
        if ((double) curve[index].time >= (double) beginTime && (double) curve[index].time < (double) endTime)
          return true;
      }
      return false;
    }

    public static void RemoveKeysInRange(AnimationCurve curve, float beginTime, float endTime)
    {
      for (int index = curve.length - 1; index >= 0; --index)
      {
        if ((double) curve[index].time >= (double) beginTime && (double) curve[index].time < (double) endTime)
          curve.RemoveKey(index);
      }
    }

    public static void UpdateTangentsFromMode(AnimationCurve curve)
    {
      for (int index = 0; index < curve.length; ++index)
        CurveUtility.UpdateTangentsFromMode(curve, index);
    }

    private static float CalculateLinearTangent(AnimationCurve curve, int index, int toIndex)
    {
      return (float) (((double) curve[index].value - (double) curve[toIndex].value) / ((double) curve[index].time - (double) curve[toIndex].time));
    }

    private static void UpdateTangentsFromMode(AnimationCurve curve, int index)
    {
      if (index < 0 || index >= curve.length)
        return;
      Keyframe key = curve[index];
      if (CurveUtility.GetKeyTangentMode(key, 0) == TangentMode.Linear && index >= 1)
      {
        key.inTangent = CurveUtility.CalculateLinearTangent(curve, index, index - 1);
        curve.MoveKey(index, key);
      }
      if (CurveUtility.GetKeyTangentMode(key, 1) == TangentMode.Linear && index + 1 < curve.length)
      {
        key.outTangent = CurveUtility.CalculateLinearTangent(curve, index, index + 1);
        curve.MoveKey(index, key);
      }
      if (CurveUtility.GetKeyTangentMode(key, 0) != TangentMode.Smooth && CurveUtility.GetKeyTangentMode(key, 1) != TangentMode.Smooth)
        return;
      curve.SmoothTangents(index, 0.0f);
    }

    public static void UpdateTangentsFromModeSurrounding(AnimationCurve curve, int index)
    {
      CurveUtility.UpdateTangentsFromMode(curve, index - 2);
      CurveUtility.UpdateTangentsFromMode(curve, index - 1);
      CurveUtility.UpdateTangentsFromMode(curve, index);
      CurveUtility.UpdateTangentsFromMode(curve, index + 1);
      CurveUtility.UpdateTangentsFromMode(curve, index + 2);
    }

    public static float CalculateSmoothTangent(Keyframe key)
    {
      if ((double) key.inTangent == double.PositiveInfinity)
        key.inTangent = 0.0f;
      if ((double) key.outTangent == double.PositiveInfinity)
        key.outTangent = 0.0f;
      return (float) (((double) key.outTangent + (double) key.inTangent) * 0.5);
    }

    public static void SetKeyBroken(ref Keyframe key, bool broken)
    {
      if (broken)
        key.tangentMode |= 1;
      else
        key.tangentMode &= -2;
    }

    public static bool GetKeyBroken(Keyframe key)
    {
      return (key.tangentMode & 1) != 0;
    }

    public static void SetKeyTangentMode(ref Keyframe key, int leftRight, TangentMode mode)
    {
      if (leftRight == 0)
      {
        key.tangentMode &= -7;
        key.tangentMode |= (int) mode << 1;
      }
      else
      {
        key.tangentMode &= -25;
        key.tangentMode |= (int) mode << 3;
      }
      if (CurveUtility.GetKeyTangentMode(key, leftRight) == mode)
        return;
      Debug.Log((object) "bug");
    }

    public static TangentMode GetKeyTangentMode(Keyframe key, int leftRight)
    {
      if (leftRight == 0)
        return (TangentMode) ((key.tangentMode & 6) >> 1);
      return (TangentMode) ((key.tangentMode & 24) >> 3);
    }

    public static void SetKeyModeFromContext(AnimationCurve curve, int keyIndex)
    {
      Keyframe key = curve[keyIndex];
      bool broken = false;
      if (keyIndex > 0 && CurveUtility.GetKeyBroken(curve[keyIndex - 1]))
        broken = true;
      if (keyIndex < curve.length - 1 && CurveUtility.GetKeyBroken(curve[keyIndex + 1]))
        broken = true;
      CurveUtility.SetKeyBroken(ref key, broken);
      if (broken)
      {
        if (keyIndex > 0)
          CurveUtility.SetKeyTangentMode(ref key, 0, CurveUtility.GetKeyTangentMode(curve[keyIndex - 1], 1));
        if (keyIndex < curve.length - 1)
          CurveUtility.SetKeyTangentMode(ref key, 1, CurveUtility.GetKeyTangentMode(curve[keyIndex + 1], 0));
      }
      else
      {
        TangentMode mode = TangentMode.Smooth;
        if (keyIndex > 0 && CurveUtility.GetKeyTangentMode(curve[keyIndex - 1], 1) != TangentMode.Smooth)
          mode = TangentMode.Editable;
        if (keyIndex < curve.length - 1 && CurveUtility.GetKeyTangentMode(curve[keyIndex + 1], 0) != TangentMode.Smooth)
          mode = TangentMode.Editable;
        CurveUtility.SetKeyTangentMode(ref key, 0, mode);
        CurveUtility.SetKeyTangentMode(ref key, 1, mode);
      }
      curve.MoveKey(keyIndex, key);
    }

    public static string GetClipName(AnimationClip clip)
    {
      if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
        return "[No Clip]";
      string name = clip.name;
      if ((clip.hideFlags & HideFlags.NotEditable) != HideFlags.None)
        name += " (Read-Only)";
      return name;
    }

    public static Color GetBalancedColor(Color c)
    {
      return new Color((float) (0.150000005960464 + 0.75 * (double) c.r), (float) (0.200000002980232 + 0.600000023841858 * (double) c.g), (float) (0.100000001490116 + 0.899999976158142 * (double) c.b));
    }

    public static Color GetPropertyColor(string name)
    {
      Color color = Color.white;
      int num = 0;
      if (name.StartsWith("m_LocalPosition"))
        num = 1;
      if (name.StartsWith("localEulerAngles"))
        num = 2;
      if (name.StartsWith("m_LocalScale"))
        num = 3;
      if (num == 1)
      {
        if (name.EndsWith(".x"))
          color = Handles.xAxisColor;
        else if (name.EndsWith(".y"))
          color = Handles.yAxisColor;
        else if (name.EndsWith(".z"))
          color = Handles.zAxisColor;
      }
      else if (num == 2)
      {
        if (name.EndsWith(".x"))
          color = (Color) AnimEditor.kEulerXColor;
        else if (name.EndsWith(".y"))
          color = (Color) AnimEditor.kEulerYColor;
        else if (name.EndsWith(".z"))
          color = (Color) AnimEditor.kEulerZColor;
      }
      else if (num == 3)
      {
        if (name.EndsWith(".x"))
          color = CurveUtility.GetBalancedColor(new Color(0.7f, 0.4f, 0.4f));
        else if (name.EndsWith(".y"))
          color = CurveUtility.GetBalancedColor(new Color(0.4f, 0.7f, 0.4f));
        else if (name.EndsWith(".z"))
          color = CurveUtility.GetBalancedColor(new Color(0.4f, 0.4f, 0.7f));
      }
      else if (name.EndsWith(".x"))
        color = Handles.xAxisColor;
      else if (name.EndsWith(".y"))
        color = Handles.yAxisColor;
      else if (name.EndsWith(".z"))
        color = Handles.zAxisColor;
      else if (name.EndsWith(".w"))
        color = new Color(1f, 0.5f, 0.0f);
      else if (name.EndsWith(".r"))
        color = CurveUtility.GetBalancedColor(Color.red);
      else if (name.EndsWith(".g"))
        color = CurveUtility.GetBalancedColor(Color.green);
      else if (name.EndsWith(".b"))
        color = CurveUtility.GetBalancedColor(Color.blue);
      else if (name.EndsWith(".a"))
        color = CurveUtility.GetBalancedColor(Color.yellow);
      else if (name.EndsWith(".width"))
        color = CurveUtility.GetBalancedColor(Color.blue);
      else if (name.EndsWith(".height"))
      {
        color = CurveUtility.GetBalancedColor(Color.yellow);
      }
      else
      {
        float f = 6.283185f * (float) (name.GetHashCode() % 1000);
        color = CurveUtility.GetBalancedColor(Color.HSVToRGB(f - Mathf.Floor(f), 1f, 1f));
      }
      color.a = 1f;
      return color;
    }
  }
}

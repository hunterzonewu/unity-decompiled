// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveMenuManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class CurveMenuManager
  {
    private CurveUpdater updater;

    public CurveMenuManager(CurveUpdater updater)
    {
      this.updater = updater;
    }

    public void AddTangentMenuItems(GenericMenu menu, List<KeyIdentifier> keyList)
    {
      bool flag = keyList.Count > 0;
      bool on1 = flag;
      bool on2 = flag;
      bool on3 = flag;
      bool on4 = flag;
      bool on5 = flag;
      bool on6 = flag;
      bool on7 = flag;
      bool on8 = flag;
      bool on9 = flag;
      bool on10 = flag;
      using (List<KeyIdentifier>.Enumerator enumerator = keyList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Keyframe keyframe = enumerator.Current.keyframe;
          TangentMode keyTangentMode1 = CurveUtility.GetKeyTangentMode(keyframe, 0);
          TangentMode keyTangentMode2 = CurveUtility.GetKeyTangentMode(keyframe, 1);
          bool keyBroken = CurveUtility.GetKeyBroken(keyframe);
          if (keyTangentMode1 != TangentMode.Smooth || keyTangentMode2 != TangentMode.Smooth)
            on1 = false;
          if (keyBroken || keyTangentMode1 != TangentMode.Editable || keyTangentMode2 != TangentMode.Editable)
            on2 = false;
          if (keyBroken || keyTangentMode1 != TangentMode.Editable || ((double) keyframe.inTangent != 0.0 || keyTangentMode2 != TangentMode.Editable) || (double) keyframe.outTangent != 0.0)
            on3 = false;
          if (!keyBroken)
            on4 = false;
          if (!keyBroken || keyTangentMode1 != TangentMode.Editable)
            on5 = false;
          if (!keyBroken || keyTangentMode1 != TangentMode.Linear)
            on6 = false;
          if (!keyBroken || keyTangentMode1 != TangentMode.Stepped)
            on7 = false;
          if (!keyBroken || keyTangentMode2 != TangentMode.Editable)
            on8 = false;
          if (!keyBroken || keyTangentMode2 != TangentMode.Linear)
            on9 = false;
          if (!keyBroken || keyTangentMode2 != TangentMode.Stepped)
            on10 = false;
        }
      }
      if (flag)
      {
        menu.AddItem(EditorGUIUtility.TextContent("Auto"), on1, new GenericMenu.MenuFunction2(this.SetSmooth), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Free Smooth"), on2, new GenericMenu.MenuFunction2(this.SetEditable), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Flat"), on3, new GenericMenu.MenuFunction2(this.SetFlat), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Broken"), on4, new GenericMenu.MenuFunction2(this.SetBroken), (object) keyList);
        menu.AddSeparator(string.Empty);
        menu.AddItem(EditorGUIUtility.TextContent("Left Tangent/Free"), on5, new GenericMenu.MenuFunction2(this.SetLeftEditable), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Left Tangent/Linear"), on6, new GenericMenu.MenuFunction2(this.SetLeftLinear), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Left Tangent/Constant"), on7, new GenericMenu.MenuFunction2(this.SetLeftConstant), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Right Tangent/Free"), on8, new GenericMenu.MenuFunction2(this.SetRightEditable), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Right Tangent/Linear"), on9, new GenericMenu.MenuFunction2(this.SetRightLinear), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Right Tangent/Constant"), on10, new GenericMenu.MenuFunction2(this.SetRightConstant), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Both Tangents/Free"), on8 && on5, new GenericMenu.MenuFunction2(this.SetBothEditable), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Both Tangents/Linear"), on9 && on6, new GenericMenu.MenuFunction2(this.SetBothLinear), (object) keyList);
        menu.AddItem(EditorGUIUtility.TextContent("Both Tangents/Constant"), on10 && on7, new GenericMenu.MenuFunction2(this.SetBothConstant), (object) keyList);
      }
      else
      {
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Auto"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Free Smooth"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Flat"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Broken"));
        menu.AddSeparator(string.Empty);
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Left Tangent/Free"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Left Tangent/Linear"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Left Tangent/Constant"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Right Tangent/Free"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Right Tangent/Linear"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Right Tangent/Constant"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Both Tangents/Free"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Both Tangents/Linear"));
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Both Tangents/Constant"));
      }
    }

    public void SetSmooth(object keysToSet)
    {
      this.SetBoth(TangentMode.Smooth, (List<KeyIdentifier>) keysToSet);
    }

    public void SetEditable(object keysToSet)
    {
      this.SetBoth(TangentMode.Editable, (List<KeyIdentifier>) keysToSet);
    }

    public void SetFlat(object keysToSet)
    {
      this.SetBoth(TangentMode.Editable, (List<KeyIdentifier>) keysToSet);
      this.Flatten((List<KeyIdentifier>) keysToSet);
    }

    public void SetBoth(TangentMode mode, List<KeyIdentifier> keysToSet)
    {
      List<ChangedCurve> curve1 = new List<ChangedCurve>();
      List<int> curveIds = new List<int>();
      using (List<KeyIdentifier>.Enumerator enumerator = keysToSet.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyIdentifier current = enumerator.Current;
          AnimationCurve curve2 = current.curve;
          Keyframe keyframe = current.keyframe;
          CurveUtility.SetKeyBroken(ref keyframe, false);
          CurveUtility.SetKeyTangentMode(ref keyframe, 1, mode);
          CurveUtility.SetKeyTangentMode(ref keyframe, 0, mode);
          if (mode == TangentMode.Editable)
          {
            float smoothTangent = CurveUtility.CalculateSmoothTangent(keyframe);
            keyframe.inTangent = smoothTangent;
            keyframe.outTangent = smoothTangent;
          }
          curve2.MoveKey(current.key, keyframe);
          CurveUtility.UpdateTangentsFromModeSurrounding(curve2, current.key);
          ChangedCurve changedCurve = new ChangedCurve(curve2, current.binding);
          if (!curve1.Contains(changedCurve))
            curve1.Add(changedCurve);
          curveIds.Add(current.curveId);
        }
      }
      if (this.updater is DopeSheetEditor)
        this.updater.UpdateCurves(curve1, "Set Tangents");
      else
        this.updater.UpdateCurves(curveIds, "Set Tangents");
    }

    public void Flatten(List<KeyIdentifier> keysToSet)
    {
      List<ChangedCurve> curve1 = new List<ChangedCurve>();
      List<int> curveIds = new List<int>();
      using (List<KeyIdentifier>.Enumerator enumerator = keysToSet.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyIdentifier current = enumerator.Current;
          AnimationCurve curve2 = current.curve;
          Keyframe keyframe = current.keyframe;
          keyframe.inTangent = 0.0f;
          keyframe.outTangent = 0.0f;
          curve2.MoveKey(current.key, keyframe);
          CurveUtility.UpdateTangentsFromModeSurrounding(curve2, current.key);
          ChangedCurve changedCurve = new ChangedCurve(curve2, current.binding);
          if (!curve1.Contains(changedCurve))
            curve1.Add(changedCurve);
          curveIds.Add(current.curveId);
        }
      }
      if (this.updater is DopeSheetEditor)
        this.updater.UpdateCurves(curve1, "Set Tangents");
      else
        this.updater.UpdateCurves(curveIds, "Set Tangents");
    }

    public void SetBroken(object _keysToSet)
    {
      List<ChangedCurve> curve1 = new List<ChangedCurve>();
      List<KeyIdentifier> keyIdentifierList = (List<KeyIdentifier>) _keysToSet;
      List<int> curveIds = new List<int>();
      using (List<KeyIdentifier>.Enumerator enumerator = keyIdentifierList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyIdentifier current = enumerator.Current;
          AnimationCurve curve2 = current.curve;
          Keyframe keyframe = current.keyframe;
          CurveUtility.SetKeyBroken(ref keyframe, true);
          if (CurveUtility.GetKeyTangentMode(keyframe, 1) == TangentMode.Smooth)
            CurveUtility.SetKeyTangentMode(ref keyframe, 1, TangentMode.Editable);
          if (CurveUtility.GetKeyTangentMode(keyframe, 0) == TangentMode.Smooth)
            CurveUtility.SetKeyTangentMode(ref keyframe, 0, TangentMode.Editable);
          curve2.MoveKey(current.key, keyframe);
          CurveUtility.UpdateTangentsFromModeSurrounding(curve2, current.key);
          ChangedCurve changedCurve = new ChangedCurve(curve2, current.binding);
          if (!curve1.Contains(changedCurve))
            curve1.Add(changedCurve);
          curveIds.Add(current.curveId);
        }
      }
      if (this.updater is DopeSheetEditor)
        this.updater.UpdateCurves(curve1, "Set Tangents");
      else
        this.updater.UpdateCurves(curveIds, "Set Tangents");
    }

    public void SetLeftEditable(object keysToSet)
    {
      this.SetTangent(0, TangentMode.Editable, (List<KeyIdentifier>) keysToSet);
    }

    public void SetLeftLinear(object keysToSet)
    {
      this.SetTangent(0, TangentMode.Linear, (List<KeyIdentifier>) keysToSet);
    }

    public void SetLeftConstant(object keysToSet)
    {
      this.SetTangent(0, TangentMode.Stepped, (List<KeyIdentifier>) keysToSet);
    }

    public void SetRightEditable(object keysToSet)
    {
      this.SetTangent(1, TangentMode.Editable, (List<KeyIdentifier>) keysToSet);
    }

    public void SetRightLinear(object keysToSet)
    {
      this.SetTangent(1, TangentMode.Linear, (List<KeyIdentifier>) keysToSet);
    }

    public void SetRightConstant(object keysToSet)
    {
      this.SetTangent(1, TangentMode.Stepped, (List<KeyIdentifier>) keysToSet);
    }

    public void SetBothEditable(object keysToSet)
    {
      this.SetTangent(2, TangentMode.Editable, (List<KeyIdentifier>) keysToSet);
    }

    public void SetBothLinear(object keysToSet)
    {
      this.SetTangent(2, TangentMode.Linear, (List<KeyIdentifier>) keysToSet);
    }

    public void SetBothConstant(object keysToSet)
    {
      this.SetTangent(2, TangentMode.Stepped, (List<KeyIdentifier>) keysToSet);
    }

    public void SetTangent(int leftRight, TangentMode mode, List<KeyIdentifier> keysToSet)
    {
      List<int> curveIds = new List<int>();
      List<ChangedCurve> curve1 = new List<ChangedCurve>();
      using (List<KeyIdentifier>.Enumerator enumerator = keysToSet.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyIdentifier current = enumerator.Current;
          AnimationCurve curve2 = current.curve;
          Keyframe keyframe = current.keyframe;
          CurveUtility.SetKeyBroken(ref keyframe, true);
          if (leftRight == 2)
          {
            CurveUtility.SetKeyTangentMode(ref keyframe, 0, mode);
            CurveUtility.SetKeyTangentMode(ref keyframe, 1, mode);
          }
          else
          {
            CurveUtility.SetKeyTangentMode(ref keyframe, leftRight, mode);
            if (CurveUtility.GetKeyTangentMode(keyframe, 1 - leftRight) == TangentMode.Smooth)
              CurveUtility.SetKeyTangentMode(ref keyframe, 1 - leftRight, TangentMode.Editable);
          }
          if (mode == TangentMode.Stepped && (leftRight == 0 || leftRight == 2))
            keyframe.inTangent = float.PositiveInfinity;
          if (mode == TangentMode.Stepped && (leftRight == 1 || leftRight == 2))
            keyframe.outTangent = float.PositiveInfinity;
          curve2.MoveKey(current.key, keyframe);
          CurveUtility.UpdateTangentsFromModeSurrounding(curve2, current.key);
          ChangedCurve changedCurve = new ChangedCurve(curve2, current.binding);
          if (!curve1.Contains(changedCurve))
            curve1.Add(changedCurve);
          curveIds.Add(current.curveId);
        }
      }
      if (this.updater is DopeSheetEditor)
        this.updater.UpdateCurves(curve1, "Set Tangents");
      else
        this.updater.UpdateCurves(curveIds, "Set Tangents");
    }
  }
}

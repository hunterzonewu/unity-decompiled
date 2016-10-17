// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.MaterialAnimationUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal static class MaterialAnimationUtility
  {
    private const string kMaterialPrefix = "material.";

    private static UndoPropertyModification[] CreateUndoPropertyModifications(int count, Object target)
    {
      UndoPropertyModification[] propertyModificationArray = new UndoPropertyModification[count];
      for (int index = 0; index < propertyModificationArray.Length; ++index)
      {
        propertyModificationArray[index].previousValue = new PropertyModification();
        propertyModificationArray[index].previousValue.target = target;
      }
      return propertyModificationArray;
    }

    private static void SetupPropertyModification(string name, float value, UndoPropertyModification prop)
    {
      prop.previousValue.propertyPath = "material." + name;
      prop.previousValue.value = value.ToString();
    }

    private static void ApplyMaterialModificationToAnimationRecording(MaterialProperty materialProp, Object target, float value)
    {
      UndoPropertyModification[] propertyModifications = MaterialAnimationUtility.CreateUndoPropertyModifications(1, target);
      MaterialAnimationUtility.SetupPropertyModification(materialProp.name, value, propertyModifications[0]);
      UndoPropertyModification[] propertyModificationArray = Undo.postprocessModifications(propertyModifications);
    }

    private static void ApplyMaterialModificationToAnimationRecording(MaterialProperty materialProp, Object target, Color color)
    {
      UndoPropertyModification[] propertyModifications = MaterialAnimationUtility.CreateUndoPropertyModifications(4, target);
      MaterialAnimationUtility.SetupPropertyModification(materialProp.name + ".r", color.r, propertyModifications[0]);
      MaterialAnimationUtility.SetupPropertyModification(materialProp.name + ".g", color.g, propertyModifications[1]);
      MaterialAnimationUtility.SetupPropertyModification(materialProp.name + ".b", color.b, propertyModifications[2]);
      MaterialAnimationUtility.SetupPropertyModification(materialProp.name + ".a", color.a, propertyModifications[3]);
      UndoPropertyModification[] propertyModificationArray = Undo.postprocessModifications(propertyModifications);
    }

    private static void ApplyMaterialModificationToAnimationRecording(string name, Object target, Vector4 vec)
    {
      UndoPropertyModification[] propertyModifications = MaterialAnimationUtility.CreateUndoPropertyModifications(4, target);
      MaterialAnimationUtility.SetupPropertyModification(name + ".x", vec.x, propertyModifications[0]);
      MaterialAnimationUtility.SetupPropertyModification(name + ".y", vec.y, propertyModifications[1]);
      MaterialAnimationUtility.SetupPropertyModification(name + ".z", vec.z, propertyModifications[2]);
      MaterialAnimationUtility.SetupPropertyModification(name + ".w", vec.w, propertyModifications[3]);
      UndoPropertyModification[] propertyModificationArray = Undo.postprocessModifications(propertyModifications);
    }

    public static bool IsAnimated(MaterialProperty materialProp, Renderer target)
    {
      if (materialProp.type == MaterialProperty.PropType.Texture)
        return AnimationMode.IsPropertyAnimated((Object) target, "material." + materialProp.name + "_ST");
      return AnimationMode.IsPropertyAnimated((Object) target, "material." + materialProp.name);
    }

    public static void SetupMaterialPropertyBlock(MaterialProperty materialProp, int changedMask, Renderer target)
    {
      MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
      target.GetPropertyBlock(materialPropertyBlock);
      materialProp.WriteToMaterialPropertyBlock(materialPropertyBlock, changedMask);
      target.SetPropertyBlock(materialPropertyBlock);
    }

    public static bool ApplyMaterialModificationToAnimationRecording(MaterialProperty materialProp, int changedMask, Renderer target, object oldValue)
    {
      switch (materialProp.type)
      {
        case MaterialProperty.PropType.Color:
          MaterialAnimationUtility.SetupMaterialPropertyBlock(materialProp, changedMask, target);
          MaterialAnimationUtility.ApplyMaterialModificationToAnimationRecording(materialProp, (Object) target, (Color) oldValue);
          return true;
        case MaterialProperty.PropType.Vector:
          MaterialAnimationUtility.SetupMaterialPropertyBlock(materialProp, changedMask, target);
          MaterialAnimationUtility.ApplyMaterialModificationToAnimationRecording(materialProp, (Object) target, (Color) ((Vector4) oldValue));
          return true;
        case MaterialProperty.PropType.Float:
        case MaterialProperty.PropType.Range:
          MaterialAnimationUtility.SetupMaterialPropertyBlock(materialProp, changedMask, target);
          MaterialAnimationUtility.ApplyMaterialModificationToAnimationRecording(materialProp, (Object) target, (float) oldValue);
          return true;
        case MaterialProperty.PropType.Texture:
          if (!MaterialProperty.IsTextureOffsetAndScaleChangedMask(changedMask))
            return false;
          string name = materialProp.name + "_ST";
          MaterialAnimationUtility.SetupMaterialPropertyBlock(materialProp, changedMask, target);
          MaterialAnimationUtility.ApplyMaterialModificationToAnimationRecording(name, (Object) target, (Vector4) oldValue);
          return true;
        default:
          return false;
      }
    }
  }
}

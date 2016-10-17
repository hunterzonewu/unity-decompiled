// Decompiled with JetBrains decompiler
// Type: UnityEditor.RagdollBuilder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections;
using UnityEngine;

namespace UnityEditor
{
  internal class RagdollBuilder : ScriptableWizard
  {
    public float totalMass = 20f;
    private Vector3 right = Vector3.right;
    private Vector3 up = Vector3.up;
    private Vector3 forward = Vector3.forward;
    private Vector3 worldRight = Vector3.right;
    private Vector3 worldUp = Vector3.up;
    private Vector3 worldForward = Vector3.forward;
    public Transform pelvis;
    public Transform leftHips;
    public Transform leftKnee;
    public Transform leftFoot;
    public Transform rightHips;
    public Transform rightKnee;
    public Transform rightFoot;
    public Transform leftArm;
    public Transform leftElbow;
    public Transform rightArm;
    public Transform rightElbow;
    public Transform middleSpine;
    public Transform head;
    public float strength;
    public bool flipForward;
    private ArrayList bones;
    private RagdollBuilder.BoneInfo rootBone;

    private string CheckConsistency()
    {
      this.PrepareBones();
      Hashtable hashtable = new Hashtable();
      foreach (RagdollBuilder.BoneInfo bone in this.bones)
      {
        if ((bool) ((UnityEngine.Object) bone.anchor))
        {
          if (hashtable[(object) bone.anchor] != null)
          {
            RagdollBuilder.BoneInfo boneInfo = (RagdollBuilder.BoneInfo) hashtable[(object) bone.anchor];
            return string.Format("{0} and {1} may not be assigned to the same bone.", (object) bone.name, (object) boneInfo.name);
          }
          hashtable[(object) bone.anchor] = (object) bone;
        }
      }
      foreach (RagdollBuilder.BoneInfo bone in this.bones)
      {
        if ((UnityEngine.Object) bone.anchor == (UnityEngine.Object) null)
          return string.Format("{0} has not been assigned yet.\n", (object) bone.name);
      }
      return string.Empty;
    }

    private void OnDrawGizmos()
    {
      if (!(bool) ((UnityEngine.Object) this.pelvis))
        return;
      Gizmos.color = Color.red;
      Gizmos.DrawRay(this.pelvis.position, this.pelvis.TransformDirection(this.right));
      Gizmos.color = Color.green;
      Gizmos.DrawRay(this.pelvis.position, this.pelvis.TransformDirection(this.up));
      Gizmos.color = Color.blue;
      Gizmos.DrawRay(this.pelvis.position, this.pelvis.TransformDirection(this.forward));
    }

    [MenuItem("GameObject/3D Object/Ragdoll...", false, 2000)]
    private static void CreateWizard()
    {
      ScriptableWizard.DisplayWizard<RagdollBuilder>("Create Ragdoll");
    }

    private void DecomposeVector(out Vector3 normalCompo, out Vector3 tangentCompo, Vector3 outwardDir, Vector3 outwardNormal)
    {
      outwardNormal = outwardNormal.normalized;
      normalCompo = outwardNormal * Vector3.Dot(outwardDir, outwardNormal);
      tangentCompo = outwardDir - normalCompo;
    }

    private void CalculateAxes()
    {
      if ((UnityEngine.Object) this.head != (UnityEngine.Object) null && (UnityEngine.Object) this.pelvis != (UnityEngine.Object) null)
        this.up = RagdollBuilder.CalculateDirectionAxis(this.pelvis.InverseTransformPoint(this.head.position));
      if ((UnityEngine.Object) this.rightElbow != (UnityEngine.Object) null && (UnityEngine.Object) this.pelvis != (UnityEngine.Object) null)
      {
        Vector3 normalCompo;
        Vector3 tangentCompo;
        this.DecomposeVector(out normalCompo, out tangentCompo, this.pelvis.InverseTransformPoint(this.rightElbow.position), this.up);
        this.right = RagdollBuilder.CalculateDirectionAxis(tangentCompo);
      }
      this.forward = Vector3.Cross(this.right, this.up);
      if (!this.flipForward)
        return;
      this.forward = -this.forward;
    }

    private void OnWizardUpdate()
    {
      this.errorString = this.CheckConsistency();
      this.CalculateAxes();
      if (this.errorString.Length != 0)
        this.helpString = "Drag all bones from the hierarchy into their slots.\nMake sure your character is in T-Stand.\n";
      else
        this.helpString = "Make sure your character is in T-Stand.\nMake sure the blue axis faces in the same direction the chracter is looking.\nUse flipForward to flip the direction";
      this.isValid = this.errorString.Length == 0;
    }

    private void PrepareBones()
    {
      if ((bool) ((UnityEngine.Object) this.pelvis))
      {
        this.worldRight = this.pelvis.TransformDirection(this.right);
        this.worldUp = this.pelvis.TransformDirection(this.up);
        this.worldForward = this.pelvis.TransformDirection(this.forward);
      }
      this.bones = new ArrayList();
      this.rootBone = new RagdollBuilder.BoneInfo();
      this.rootBone.name = "Pelvis";
      this.rootBone.anchor = this.pelvis;
      this.rootBone.parent = (RagdollBuilder.BoneInfo) null;
      this.rootBone.density = 2.5f;
      this.bones.Add((object) this.rootBone);
      this.AddMirroredJoint("Hips", this.leftHips, this.rightHips, "Pelvis", this.worldRight, this.worldForward, -20f, 70f, 30f, typeof (CapsuleCollider), 0.3f, 1.5f);
      this.AddMirroredJoint("Knee", this.leftKnee, this.rightKnee, "Hips", this.worldRight, this.worldForward, -80f, 0.0f, 0.0f, typeof (CapsuleCollider), 0.25f, 1.5f);
      this.AddJoint("Middle Spine", this.middleSpine, "Pelvis", this.worldRight, this.worldForward, -20f, 20f, 10f, (System.Type) null, 1f, 2.5f);
      this.AddMirroredJoint("Arm", this.leftArm, this.rightArm, "Middle Spine", this.worldUp, this.worldForward, -70f, 10f, 50f, typeof (CapsuleCollider), 0.25f, 1f);
      this.AddMirroredJoint("Elbow", this.leftElbow, this.rightElbow, "Arm", this.worldForward, this.worldUp, -90f, 0.0f, 0.0f, typeof (CapsuleCollider), 0.2f, 1f);
      this.AddJoint("Head", this.head, "Middle Spine", this.worldRight, this.worldForward, -40f, 25f, 25f, (System.Type) null, 1f, 1f);
    }

    private void OnWizardCreate()
    {
      this.Cleanup();
      this.BuildCapsules();
      this.AddBreastColliders();
      this.AddHeadCollider();
      this.BuildBodies();
      this.BuildJoints();
      this.CalculateMass();
    }

    private RagdollBuilder.BoneInfo FindBone(string name)
    {
      foreach (RagdollBuilder.BoneInfo bone in this.bones)
      {
        if (bone.name == name)
          return bone;
      }
      return (RagdollBuilder.BoneInfo) null;
    }

    private void AddMirroredJoint(string name, Transform leftAnchor, Transform rightAnchor, string parent, Vector3 worldTwistAxis, Vector3 worldSwingAxis, float minLimit, float maxLimit, float swingLimit, System.Type colliderType, float radiusScale, float density)
    {
      this.AddJoint("Left " + name, leftAnchor, parent, worldTwistAxis, worldSwingAxis, minLimit, maxLimit, swingLimit, colliderType, radiusScale, density);
      this.AddJoint("Right " + name, rightAnchor, parent, worldTwistAxis, worldSwingAxis, minLimit, maxLimit, swingLimit, colliderType, radiusScale, density);
    }

    private void AddJoint(string name, Transform anchor, string parent, Vector3 worldTwistAxis, Vector3 worldSwingAxis, float minLimit, float maxLimit, float swingLimit, System.Type colliderType, float radiusScale, float density)
    {
      RagdollBuilder.BoneInfo boneInfo = new RagdollBuilder.BoneInfo();
      boneInfo.name = name;
      boneInfo.anchor = anchor;
      boneInfo.axis = worldTwistAxis;
      boneInfo.normalAxis = worldSwingAxis;
      boneInfo.minLimit = minLimit;
      boneInfo.maxLimit = maxLimit;
      boneInfo.swingLimit = swingLimit;
      boneInfo.density = density;
      boneInfo.colliderType = colliderType;
      boneInfo.radiusScale = radiusScale;
      if (this.FindBone(parent) != null)
        boneInfo.parent = this.FindBone(parent);
      else if (name.StartsWith("Left"))
        boneInfo.parent = this.FindBone("Left " + parent);
      else if (name.StartsWith("Right"))
        boneInfo.parent = this.FindBone("Right " + parent);
      boneInfo.parent.children.Add((object) boneInfo);
      this.bones.Add((object) boneInfo);
    }

    private void BuildCapsules()
    {
      foreach (RagdollBuilder.BoneInfo bone in this.bones)
      {
        if (bone.colliderType == typeof (CapsuleCollider))
        {
          int direction;
          float distance;
          if (bone.children.Count == 1)
          {
            Vector3 position = ((RagdollBuilder.BoneInfo) bone.children[0]).anchor.position;
            RagdollBuilder.CalculateDirection(bone.anchor.InverseTransformPoint(position), out direction, out distance);
          }
          else
          {
            Vector3 position = bone.anchor.position - bone.parent.anchor.position + bone.anchor.position;
            RagdollBuilder.CalculateDirection(bone.anchor.InverseTransformPoint(position), out direction, out distance);
            if (bone.anchor.GetComponentsInChildren(typeof (Transform)).Length > 1)
            {
              Bounds bounds = new Bounds();
              foreach (Transform componentsInChild in bone.anchor.GetComponentsInChildren(typeof (Transform)))
                bounds.Encapsulate(bone.anchor.InverseTransformPoint(componentsInChild.position));
              distance = (double) distance <= 0.0 ? bounds.min[direction] : bounds.max[direction];
            }
          }
          CapsuleCollider capsuleCollider = bone.anchor.gameObject.AddComponent<CapsuleCollider>();
          capsuleCollider.direction = direction;
          Vector3 zero = Vector3.zero;
          zero[direction] = distance * 0.5f;
          capsuleCollider.center = zero;
          capsuleCollider.height = Mathf.Abs(distance);
          capsuleCollider.radius = Mathf.Abs(distance * bone.radiusScale);
        }
      }
    }

    private void Cleanup()
    {
      foreach (RagdollBuilder.BoneInfo bone in this.bones)
      {
        if ((bool) ((UnityEngine.Object) bone.anchor))
        {
          foreach (UnityEngine.Object componentsInChild in bone.anchor.GetComponentsInChildren(typeof (Joint)))
            UnityEngine.Object.DestroyImmediate(componentsInChild);
          foreach (UnityEngine.Object componentsInChild in bone.anchor.GetComponentsInChildren(typeof (Rigidbody)))
            UnityEngine.Object.DestroyImmediate(componentsInChild);
          foreach (UnityEngine.Object componentsInChild in bone.anchor.GetComponentsInChildren(typeof (Collider)))
            UnityEngine.Object.DestroyImmediate(componentsInChild);
        }
      }
    }

    private void BuildBodies()
    {
      foreach (RagdollBuilder.BoneInfo bone in this.bones)
      {
        bone.anchor.gameObject.AddComponent<Rigidbody>();
        bone.anchor.GetComponent<Rigidbody>().mass = bone.density;
      }
    }

    private void BuildJoints()
    {
      foreach (RagdollBuilder.BoneInfo bone in this.bones)
      {
        if (bone.parent != null)
        {
          CharacterJoint characterJoint = bone.anchor.gameObject.AddComponent<CharacterJoint>();
          bone.joint = characterJoint;
          characterJoint.axis = RagdollBuilder.CalculateDirectionAxis(bone.anchor.InverseTransformDirection(bone.axis));
          characterJoint.swingAxis = RagdollBuilder.CalculateDirectionAxis(bone.anchor.InverseTransformDirection(bone.normalAxis));
          characterJoint.anchor = Vector3.zero;
          characterJoint.connectedBody = bone.parent.anchor.GetComponent<Rigidbody>();
          characterJoint.enablePreprocessing = false;
          SoftJointLimit softJointLimit = new SoftJointLimit();
          softJointLimit.contactDistance = 0.0f;
          softJointLimit.limit = bone.minLimit;
          characterJoint.lowTwistLimit = softJointLimit;
          softJointLimit.limit = bone.maxLimit;
          characterJoint.highTwistLimit = softJointLimit;
          softJointLimit.limit = bone.swingLimit;
          characterJoint.swing1Limit = softJointLimit;
          softJointLimit.limit = 0.0f;
          characterJoint.swing2Limit = softJointLimit;
        }
      }
    }

    private void CalculateMassRecurse(RagdollBuilder.BoneInfo bone)
    {
      float mass = bone.anchor.GetComponent<Rigidbody>().mass;
      foreach (RagdollBuilder.BoneInfo child in bone.children)
      {
        this.CalculateMassRecurse(child);
        mass += child.summedMass;
      }
      bone.summedMass = mass;
    }

    private void CalculateMass()
    {
      this.CalculateMassRecurse(this.rootBone);
      float num = this.totalMass / this.rootBone.summedMass;
      foreach (RagdollBuilder.BoneInfo bone in this.bones)
        bone.anchor.GetComponent<Rigidbody>().mass *= num;
      this.CalculateMassRecurse(this.rootBone);
    }

    private static void CalculateDirection(Vector3 point, out int direction, out float distance)
    {
      direction = 0;
      if ((double) Mathf.Abs(point[1]) > (double) Mathf.Abs(point[0]))
        direction = 1;
      if ((double) Mathf.Abs(point[2]) > (double) Mathf.Abs(point[direction]))
        direction = 2;
      distance = point[direction];
    }

    private static Vector3 CalculateDirectionAxis(Vector3 point)
    {
      int direction = 0;
      float distance;
      RagdollBuilder.CalculateDirection(point, out direction, out distance);
      Vector3 zero = Vector3.zero;
      zero[direction] = (double) distance <= 0.0 ? -1f : 1f;
      return zero;
    }

    private static int SmallestComponent(Vector3 point)
    {
      int index = 0;
      if ((double) Mathf.Abs(point[1]) < (double) Mathf.Abs(point[0]))
        index = 1;
      if ((double) Mathf.Abs(point[2]) < (double) Mathf.Abs(point[index]))
        index = 2;
      return index;
    }

    private static int LargestComponent(Vector3 point)
    {
      int index = 0;
      if ((double) Mathf.Abs(point[1]) > (double) Mathf.Abs(point[0]))
        index = 1;
      if ((double) Mathf.Abs(point[2]) > (double) Mathf.Abs(point[index]))
        index = 2;
      return index;
    }

    private static int SecondLargestComponent(Vector3 point)
    {
      int num1 = RagdollBuilder.SmallestComponent(point);
      int num2 = RagdollBuilder.LargestComponent(point);
      if (num1 < num2)
      {
        int num3 = num2;
        num2 = num1;
        num1 = num3;
      }
      if (num1 == 0 && num2 == 1)
        return 2;
      return num1 == 0 && num2 == 2 ? 1 : 0;
    }

    private Bounds Clip(Bounds bounds, Transform relativeTo, Transform clipTransform, bool below)
    {
      int index = RagdollBuilder.LargestComponent(bounds.size);
      if ((double) Vector3.Dot(this.worldUp, relativeTo.TransformPoint(bounds.max)) > (double) Vector3.Dot(this.worldUp, relativeTo.TransformPoint(bounds.min)) == below)
      {
        Vector3 min = bounds.min;
        min[index] = relativeTo.InverseTransformPoint(clipTransform.position)[index];
        bounds.min = min;
      }
      else
      {
        Vector3 max = bounds.max;
        max[index] = relativeTo.InverseTransformPoint(clipTransform.position)[index];
        bounds.max = max;
      }
      return bounds;
    }

    private Bounds GetBreastBounds(Transform relativeTo)
    {
      Bounds bounds = new Bounds();
      bounds.Encapsulate(relativeTo.InverseTransformPoint(this.leftHips.position));
      bounds.Encapsulate(relativeTo.InverseTransformPoint(this.rightHips.position));
      bounds.Encapsulate(relativeTo.InverseTransformPoint(this.leftArm.position));
      bounds.Encapsulate(relativeTo.InverseTransformPoint(this.rightArm.position));
      Vector3 size = bounds.size;
      size[RagdollBuilder.SmallestComponent(bounds.size)] = size[RagdollBuilder.LargestComponent(bounds.size)] / 2f;
      bounds.size = size;
      return bounds;
    }

    private void AddBreastColliders()
    {
      if ((UnityEngine.Object) this.middleSpine != (UnityEngine.Object) null && (UnityEngine.Object) this.pelvis != (UnityEngine.Object) null)
      {
        Bounds bounds1 = this.Clip(this.GetBreastBounds(this.pelvis), this.pelvis, this.middleSpine, false);
        BoxCollider boxCollider1 = this.pelvis.gameObject.AddComponent<BoxCollider>();
        boxCollider1.center = bounds1.center;
        boxCollider1.size = bounds1.size;
        Bounds bounds2 = this.Clip(this.GetBreastBounds(this.middleSpine), this.middleSpine, this.middleSpine, true);
        BoxCollider boxCollider2 = this.middleSpine.gameObject.AddComponent<BoxCollider>();
        boxCollider2.center = bounds2.center;
        boxCollider2.size = bounds2.size;
      }
      else
      {
        Bounds bounds = new Bounds();
        bounds.Encapsulate(this.pelvis.InverseTransformPoint(this.leftHips.position));
        bounds.Encapsulate(this.pelvis.InverseTransformPoint(this.rightHips.position));
        bounds.Encapsulate(this.pelvis.InverseTransformPoint(this.leftArm.position));
        bounds.Encapsulate(this.pelvis.InverseTransformPoint(this.rightArm.position));
        Vector3 size = bounds.size;
        size[RagdollBuilder.SmallestComponent(bounds.size)] = size[RagdollBuilder.LargestComponent(bounds.size)] / 2f;
        BoxCollider boxCollider = this.pelvis.gameObject.AddComponent<BoxCollider>();
        boxCollider.center = bounds.center;
        boxCollider.size = size;
      }
    }

    private void AddHeadCollider()
    {
      if ((bool) ((UnityEngine.Object) this.head.GetComponent<Collider>()))
        UnityEngine.Object.Destroy((UnityEngine.Object) this.head.GetComponent<Collider>());
      float num = Vector3.Distance(this.leftArm.transform.position, this.rightArm.transform.position) / 4f;
      SphereCollider sphereCollider = this.head.gameObject.AddComponent<SphereCollider>();
      sphereCollider.radius = num;
      Vector3 zero = Vector3.zero;
      int direction;
      float distance;
      RagdollBuilder.CalculateDirection(this.head.InverseTransformPoint(this.pelvis.position), out direction, out distance);
      zero[direction] = (double) distance <= 0.0 ? num : -num;
      sphereCollider.center = zero;
    }

    private class BoneInfo
    {
      public ArrayList children = new ArrayList();
      public string name;
      public Transform anchor;
      public CharacterJoint joint;
      public RagdollBuilder.BoneInfo parent;
      public float minLimit;
      public float maxLimit;
      public float swingLimit;
      public Vector3 axis;
      public Vector3 normalAxis;
      public float radiusScale;
      public System.Type colliderType;
      public float density;
      public float summedMass;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightProbeGroupEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace UnityEditor
{
  internal class LightProbeGroupEditor : IEditablePoint
  {
    private static readonly Color kCloudColor = new Color(0.7843137f, 0.7843137f, 0.07843138f, 0.85f);
    private static readonly Color kSelectedCloudColor = new Color(0.3f, 0.6f, 1f, 1f);
    private List<int> m_Selection = new List<int>();
    private Vector3 m_LastPosition = Vector3.zero;
    private Quaternion m_LastRotation = Quaternion.identity;
    private Vector3 m_LastScale = Vector3.one;
    private bool m_Editing;
    private List<Vector3> m_SourcePositions;
    private readonly LightProbeGroupSelection m_SerializedSelectedProbes;
    private readonly LightProbeGroup m_Group;
    private bool m_ShouldRecalculateTetrahedra;

    public Bounds selectedProbeBounds
    {
      get
      {
        if (this.m_Selection.Count == 0)
          return new Bounds();
        if (this.m_Selection.Count == 1)
          return new Bounds(this.GetWorldPosition(this.m_Selection[0]), new Vector3(1f, 1f, 1f));
        Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        using (List<int>.Enumerator enumerator = this.m_Selection.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Vector3 worldPosition = this.GetWorldPosition(enumerator.Current);
            if ((double) worldPosition.x < (double) min.x)
              min.x = worldPosition.x;
            if ((double) worldPosition.y < (double) min.y)
              min.y = worldPosition.y;
            if ((double) worldPosition.z < (double) min.z)
              min.z = worldPosition.z;
            if ((double) worldPosition.x > (double) max.x)
              max.x = worldPosition.x;
            if ((double) worldPosition.y > (double) max.y)
              max.y = worldPosition.y;
            if ((double) worldPosition.z > (double) max.z)
              max.z = worldPosition.z;
          }
        }
        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
      }
    }

    public int Count
    {
      get
      {
        return this.m_SourcePositions.Count;
      }
    }

    public int SelectedCount
    {
      get
      {
        return this.m_Selection.Count;
      }
    }

    public LightProbeGroupEditor(LightProbeGroup group)
    {
      this.m_Group = group;
      this.MarkTetrahedraDirty();
      this.m_SerializedSelectedProbes = ScriptableObject.CreateInstance<LightProbeGroupSelection>();
      this.m_SerializedSelectedProbes.hideFlags = HideFlags.HideAndDontSave;
    }

    public void SetEditing(bool editing)
    {
      this.m_Editing = editing;
    }

    public void AddProbe(Vector3 position)
    {
      Undo.RegisterCompleteObjectUndo(new UnityEngine.Object[2]
      {
        (UnityEngine.Object) this.m_Group,
        (UnityEngine.Object) this.m_SerializedSelectedProbes
      }, "Add Probe");
      this.m_SourcePositions.Add(position);
      this.SelectProbe(this.m_SourcePositions.Count - 1);
      this.MarkTetrahedraDirty();
    }

    private void SelectProbe(int i)
    {
      if (this.m_Selection.Contains(i))
        return;
      this.m_Selection.Add(i);
    }

    public void SelectAllProbes()
    {
      this.DeselectProbes();
      for (int i = 0; i < this.m_SourcePositions.Count; ++i)
        this.SelectProbe(i);
    }

    public void DeselectProbes()
    {
      this.m_Selection.Clear();
    }

    private IEnumerable<Vector3> SelectedProbePositions()
    {
      return (IEnumerable<Vector3>) this.m_Selection.Select<int, Vector3>((Func<int, Vector3>) (t => this.m_SourcePositions[t])).ToList<Vector3>();
    }

    public void DuplicateSelectedProbes()
    {
      if (this.m_Selection.Count == 0)
        return;
      Undo.RegisterCompleteObjectUndo(new UnityEngine.Object[2]
      {
        (UnityEngine.Object) this.m_Group,
        (UnityEngine.Object) this.m_SerializedSelectedProbes
      }, "Duplicate Probes");
      foreach (Vector3 selectedProbePosition in this.SelectedProbePositions())
        this.m_SourcePositions.Add(selectedProbePosition);
      this.MarkTetrahedraDirty();
    }

    private void CopySelectedProbes()
    {
      IEnumerable<Vector3> source = this.SelectedProbePositions();
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (Vector3[]));
      StringWriter stringWriter = new StringWriter();
      xmlSerializer.Serialize((TextWriter) stringWriter, (object) source.Select<Vector3, Vector3>((Func<Vector3, Vector3>) (pos => this.m_Group.transform.TransformPoint(pos))).ToArray<Vector3>());
      stringWriter.Close();
      GUIUtility.systemCopyBuffer = stringWriter.ToString();
    }

    private static bool CanPasteProbes()
    {
      try
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (Vector3[]));
        StringReader stringReader = new StringReader(GUIUtility.systemCopyBuffer);
        xmlSerializer.Deserialize((TextReader) stringReader);
        stringReader.Close();
        return true;
      }
      catch
      {
        return false;
      }
    }

    private bool PasteProbes()
    {
      try
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (Vector3[]));
        StringReader stringReader = new StringReader(GUIUtility.systemCopyBuffer);
        Vector3[] vector3Array = (Vector3[]) xmlSerializer.Deserialize((TextReader) stringReader);
        stringReader.Close();
        if (vector3Array.Length == 0)
          return false;
        Undo.RegisterCompleteObjectUndo(new UnityEngine.Object[2]
        {
          (UnityEngine.Object) this.m_Group,
          (UnityEngine.Object) this.m_SerializedSelectedProbes
        }, "Paste Probes");
        int count = this.m_SourcePositions.Count;
        foreach (Vector3 position in vector3Array)
          this.m_SourcePositions.Add(this.m_Group.transform.InverseTransformPoint(position));
        this.DeselectProbes();
        for (int i = count; i < count + vector3Array.Length; ++i)
          this.SelectProbe(i);
        this.MarkTetrahedraDirty();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public void RemoveSelectedProbes()
    {
      if (this.m_Selection.Count == 0)
        return;
      Undo.RegisterCompleteObjectUndo(new UnityEngine.Object[2]
      {
        (UnityEngine.Object) this.m_Group,
        (UnityEngine.Object) this.m_SerializedSelectedProbes
      }, "Delete Probes");
      foreach (int index in (IEnumerable<int>) this.m_Selection.OrderByDescending<int, int>((Func<int, int>) (x => x)))
        this.m_SourcePositions.RemoveAt(index);
      this.DeselectProbes();
      this.MarkTetrahedraDirty();
    }

    public void PullProbePositions()
    {
      this.m_SourcePositions = new List<Vector3>((IEnumerable<Vector3>) this.m_Group.probePositions);
      this.m_Selection = new List<int>((IEnumerable<int>) this.m_SerializedSelectedProbes.m_Selection);
    }

    public void PushProbePositions()
    {
      bool flag = false;
      if (this.m_Group.probePositions.Length != this.m_SourcePositions.Count || this.m_SerializedSelectedProbes.m_Selection.Count != this.m_Selection.Count)
        flag = true;
      if (!flag)
      {
        if (((IEnumerable<Vector3>) this.m_Group.probePositions).Where<Vector3>((Func<Vector3, int, bool>) ((t, i) => t != this.m_SourcePositions[i])).Any<Vector3>())
          flag = true;
        for (int index = 0; index < this.m_SerializedSelectedProbes.m_Selection.Count; ++index)
        {
          if (this.m_SerializedSelectedProbes.m_Selection[index] != this.m_Selection[index])
            flag = true;
        }
      }
      if (!flag)
        return;
      this.m_Group.probePositions = this.m_SourcePositions.ToArray();
      this.m_SerializedSelectedProbes.m_Selection = this.m_Selection;
    }

    private void DrawTetrahedra()
    {
      if (Event.current.type != EventType.Repaint || !(bool) ((UnityEngine.Object) SceneView.lastActiveSceneView))
        return;
      LightmapVisualization.DrawTetrahedra(this.m_ShouldRecalculateTetrahedra, SceneView.lastActiveSceneView.camera.transform.position);
      this.m_ShouldRecalculateTetrahedra = false;
    }

    public void HandleEditMenuHotKeyCommands()
    {
      if (Event.current.type != EventType.ValidateCommand && Event.current.type != EventType.ExecuteCommand)
        return;
      bool flag = Event.current.type == EventType.ExecuteCommand;
      string commandName = Event.current.commandName;
      if (commandName == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (LightProbeGroupEditor.\u003C\u003Ef__switch\u0024map1B == null)
      {
        // ISSUE: reference to a compiler-generated field
        LightProbeGroupEditor.\u003C\u003Ef__switch\u0024map1B = new Dictionary<string, int>(6)
        {
          {
            "SoftDelete",
            0
          },
          {
            "Delete",
            0
          },
          {
            "Duplicate",
            1
          },
          {
            "SelectAll",
            2
          },
          {
            "Cut",
            3
          },
          {
            "Copy",
            4
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!LightProbeGroupEditor.\u003C\u003Ef__switch\u0024map1B.TryGetValue(commandName, out num))
        return;
      switch (num)
      {
        case 0:
          if (flag)
            this.RemoveSelectedProbes();
          Event.current.Use();
          break;
        case 1:
          if (flag)
            this.DuplicateSelectedProbes();
          Event.current.Use();
          break;
        case 2:
          if (flag)
            this.SelectAllProbes();
          Event.current.Use();
          break;
        case 3:
          if (flag)
          {
            this.CopySelectedProbes();
            this.RemoveSelectedProbes();
          }
          Event.current.Use();
          break;
        case 4:
          if (flag)
            this.CopySelectedProbes();
          Event.current.Use();
          break;
      }
    }

    public static void TetrahedralizeSceneProbes(out Vector3[] positions, out int[] indices)
    {
      LightProbeGroup[] objectsOfType = UnityEngine.Object.FindObjectsOfType(typeof (LightProbeGroup)) as LightProbeGroup[];
      if (objectsOfType == null)
      {
        positions = new Vector3[0];
        indices = new int[0];
      }
      else
      {
        List<Vector3> vector3List = new List<Vector3>();
        foreach (LightProbeGroup lightProbeGroup in objectsOfType)
        {
          foreach (Vector3 probePosition in lightProbeGroup.probePositions)
          {
            Vector3 vector3 = lightProbeGroup.transform.TransformPoint(probePosition);
            vector3List.Add(vector3);
          }
        }
        if (vector3List.Count == 0)
        {
          positions = new Vector3[0];
          indices = new int[0];
        }
        else
          Lightmapping.Tetrahedralize(vector3List.ToArray(), out indices, out positions);
      }
    }

    public bool OnSceneGUI(Transform transform)
    {
      if (!this.m_Group.enabled)
        return this.m_Editing;
      if (Event.current.type == EventType.Layout)
      {
        if (this.m_LastPosition != this.m_Group.transform.position || this.m_LastRotation != this.m_Group.transform.rotation || this.m_LastScale != this.m_Group.transform.localScale)
          this.MarkTetrahedraDirty();
        this.m_LastPosition = this.m_Group.transform.position;
        this.m_LastRotation = this.m_Group.transform.rotation;
        this.m_LastScale = this.m_Group.transform.localScale;
      }
      bool firstSelect = false;
      if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && (this.SelectedCount == 0 && PointEditor.FindNearest(Event.current.mousePosition, transform, (IEditablePoint) this) != -1) && !this.m_Editing)
      {
        this.m_Editing = true;
        firstSelect = true;
      }
      bool flag = Event.current.type == EventType.MouseUp;
      if (this.m_Editing && PointEditor.SelectPoints((IEditablePoint) this, transform, ref this.m_Selection, firstSelect))
        Undo.RegisterCompleteObjectUndo(new UnityEngine.Object[2]
        {
          (UnityEngine.Object) this.m_Group,
          (UnityEngine.Object) this.m_SerializedSelectedProbes
        }, "Select Probes");
      if (this.m_Editing && flag && this.SelectedCount == 0)
      {
        this.m_Editing = false;
        this.MarkTetrahedraDirty();
      }
      if ((Event.current.type == EventType.ValidateCommand || Event.current.type == EventType.ExecuteCommand) && Event.current.commandName == "Paste")
      {
        if (Event.current.type == EventType.ValidateCommand && LightProbeGroupEditor.CanPasteProbes())
          Event.current.Use();
        if (Event.current.type == EventType.ExecuteCommand && this.PasteProbes())
        {
          Event.current.Use();
          this.m_Editing = true;
        }
      }
      this.DrawTetrahedra();
      PointEditor.Draw((IEditablePoint) this, transform, this.m_Selection, true);
      if (!this.m_Editing)
        return this.m_Editing;
      this.HandleEditMenuHotKeyCommands();
      if (this.m_Editing && PointEditor.MovePoints((IEditablePoint) this, transform, this.m_Selection))
      {
        Undo.RegisterCompleteObjectUndo(new UnityEngine.Object[2]
        {
          (UnityEngine.Object) this.m_Group,
          (UnityEngine.Object) this.m_SerializedSelectedProbes
        }, "Move Probes");
        if (LightmapVisualization.dynamicUpdateLightProbes)
          this.MarkTetrahedraDirty();
      }
      if (this.m_Editing && flag && !LightmapVisualization.dynamicUpdateLightProbes)
        this.MarkTetrahedraDirty();
      return this.m_Editing;
    }

    public void MarkTetrahedraDirty()
    {
      this.m_ShouldRecalculateTetrahedra = true;
    }

    public Vector3 GetPosition(int idx)
    {
      return this.m_SourcePositions[idx];
    }

    public Vector3 GetWorldPosition(int idx)
    {
      return this.m_Group.transform.TransformPoint(this.m_SourcePositions[idx]);
    }

    public void SetPosition(int idx, Vector3 position)
    {
      if (this.m_SourcePositions[idx] == position)
        return;
      this.m_SourcePositions[idx] = position;
    }

    public Color GetDefaultColor()
    {
      return LightProbeGroupEditor.kCloudColor;
    }

    public Color GetSelectedColor()
    {
      return LightProbeGroupEditor.kSelectedCloudColor;
    }

    public float GetPointScale()
    {
      return 10f * AnnotationUtility.iconSize;
    }

    public Vector3[] GetSelectedPositions()
    {
      Vector3[] vector3Array = new Vector3[this.SelectedCount];
      for (int index = 0; index < this.SelectedCount; ++index)
        vector3Array[index] = this.m_SourcePositions[this.m_Selection[index]];
      return vector3Array;
    }

    public void UpdateSelectedPosition(int idx, Vector3 position)
    {
      if (idx > this.SelectedCount - 1)
        return;
      this.m_SourcePositions[this.m_Selection[idx]] = position;
    }

    public IEnumerable<Vector3> GetPositions()
    {
      return (IEnumerable<Vector3>) this.m_SourcePositions;
    }

    public Vector3[] GetUnselectedPositions()
    {
      return this.m_SourcePositions.Where<Vector3>((Func<Vector3, int, bool>) ((t, i) => !this.m_Selection.Contains(i))).ToArray<Vector3>();
    }
  }
}

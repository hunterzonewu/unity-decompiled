// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssemblyReferenceChecker
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityEditor
{
  internal class AssemblyReferenceChecker
  {
    private HashSet<string> referencedMethods = new HashSet<string>();
    private HashSet<string> referencedTypes = new HashSet<string>();
    private HashSet<string> definedMethods = new HashSet<string>();
    private HashSet<AssemblyDefinition> assemblyDefinitions = new HashSet<AssemblyDefinition>();
    private HashSet<string> assemblyFileNames = new HashSet<string>();
    private DateTime startTime = DateTime.MinValue;

    private void CollectReferencesFromRootsRecursive(string dir, IEnumerable<string> roots, bool ignoreSystemDlls)
    {
      foreach (string root in roots)
      {
        string str1 = Path.Combine(dir, root);
        if (!this.assemblyFileNames.Contains(root))
        {
          AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(str1);
          if (!ignoreSystemDlls || !this.IsiPhoneIgnoredSystemDll(((AssemblyNameReference) assemblyDefinition.get_Name()).get_Name()))
          {
            this.assemblyFileNames.Add(root);
            this.assemblyDefinitions.Add(assemblyDefinition);
            using (Collection<AssemblyNameReference>.Enumerator enumerator = assemblyDefinition.get_MainModule().get_AssemblyReferences().GetEnumerator())
            {
              // ISSUE: explicit reference operation
              while (((Collection<AssemblyNameReference>.Enumerator) @enumerator).MoveNext())
              {
                // ISSUE: explicit reference operation
                string str2 = ((Collection<AssemblyNameReference>.Enumerator) @enumerator).get_Current().get_Name() + ".dll";
                if (!this.assemblyFileNames.Contains(str2))
                  this.CollectReferencesFromRootsRecursive(dir, (IEnumerable<string>) new string[1]{ str2 }, (ignoreSystemDlls ? 1 : 0) != 0);
              }
            }
          }
        }
      }
    }

    public void CollectReferencesFromRoots(string dir, IEnumerable<string> roots, bool withMethods, float progressValue, bool ignoreSystemDlls)
    {
      this.CollectReferencesFromRootsRecursive(dir, roots, ignoreSystemDlls);
      AssemblyDefinition[] array = ((IEnumerable<AssemblyDefinition>) this.assemblyDefinitions).ToArray<AssemblyDefinition>();
      this.referencedTypes = MonoAOTRegistration.BuildReferencedTypeList(array);
      if (!withMethods)
        return;
      this.CollectReferencedMethods(array, this.referencedMethods, this.definedMethods, progressValue);
    }

    public void CollectReferences(string path, bool withMethods, float progressValue, bool ignoreSystemDlls)
    {
      this.assemblyDefinitions = new HashSet<AssemblyDefinition>();
      foreach (string path1 in !Directory.Exists(path) ? new string[0] : Directory.GetFiles(path))
      {
        if (!(Path.GetExtension(path1) != ".dll"))
        {
          AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(path1);
          if (!ignoreSystemDlls || !this.IsiPhoneIgnoredSystemDll(((AssemblyNameReference) assemblyDefinition.get_Name()).get_Name()))
          {
            this.assemblyFileNames.Add(Path.GetFileName(path1));
            this.assemblyDefinitions.Add(assemblyDefinition);
          }
        }
      }
      AssemblyDefinition[] array = ((IEnumerable<AssemblyDefinition>) this.assemblyDefinitions).ToArray<AssemblyDefinition>();
      this.referencedTypes = MonoAOTRegistration.BuildReferencedTypeList(array);
      if (!withMethods)
        return;
      this.CollectReferencedMethods(array, this.referencedMethods, this.definedMethods, progressValue);
    }

    private void CollectReferencedMethods(AssemblyDefinition[] definitions, HashSet<string> referencedMethods, HashSet<string> definedMethods, float progressValue)
    {
      foreach (AssemblyDefinition definition in definitions)
      {
        using (Collection<TypeDefinition>.Enumerator enumerator = definition.get_MainModule().get_Types().GetEnumerator())
        {
          // ISSUE: explicit reference operation
          while (((Collection<TypeDefinition>.Enumerator) @enumerator).MoveNext())
          {
            // ISSUE: explicit reference operation
            this.CollectReferencedMethods(((Collection<TypeDefinition>.Enumerator) @enumerator).get_Current(), referencedMethods, definedMethods, progressValue);
          }
        }
      }
    }

    private void CollectReferencedMethods(TypeDefinition typ, HashSet<string> referencedMethods, HashSet<string> definedMethods, float progressValue)
    {
      this.DisplayProgress(progressValue);
      using (Collection<TypeDefinition>.Enumerator enumerator = typ.get_NestedTypes().GetEnumerator())
      {
        // ISSUE: explicit reference operation
        while (((Collection<TypeDefinition>.Enumerator) @enumerator).MoveNext())
        {
          // ISSUE: explicit reference operation
          this.CollectReferencedMethods(((Collection<TypeDefinition>.Enumerator) @enumerator).get_Current(), referencedMethods, definedMethods, progressValue);
        }
      }
      using (Collection<MethodDefinition>.Enumerator enumerator1 = typ.get_Methods().GetEnumerator())
      {
        // ISSUE: explicit reference operation
        while (((Collection<MethodDefinition>.Enumerator) @enumerator1).MoveNext())
        {
          // ISSUE: explicit reference operation
          MethodDefinition current1 = ((Collection<MethodDefinition>.Enumerator) @enumerator1).get_Current();
          if (current1.get_HasBody())
          {
            using (Collection<Instruction>.Enumerator enumerator2 = current1.get_Body().get_Instructions().GetEnumerator())
            {
              // ISSUE: explicit reference operation
              while (((Collection<Instruction>.Enumerator) @enumerator2).MoveNext())
              {
                // ISSUE: explicit reference operation
                Instruction current2 = ((Collection<Instruction>.Enumerator) @enumerator2).get_Current();
                if (OpCode.op_Equality((OpCode) OpCodes.Call, current2.get_OpCode()))
                  referencedMethods.Add(current2.get_Operand().ToString());
              }
            }
            definedMethods.Add(((MemberReference) current1).ToString());
          }
        }
      }
    }

    private void DisplayProgress(float progressValue)
    {
      TimeSpan timeSpan = DateTime.Now - this.startTime;
      string[] strArray = new string[2]{ "Fetching assembly references", "Building list of referenced assemblies..." };
      if (timeSpan.TotalMilliseconds < 100.0)
        return;
      if (EditorUtility.DisplayCancelableProgressBar(strArray[0], strArray[1], progressValue))
        throw new OperationCanceledException();
      this.startTime = DateTime.Now;
    }

    public bool HasReferenceToMethod(string methodName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return this.referencedMethods.Any<string>(new Func<string, bool>(new AssemblyReferenceChecker.\u003CHasReferenceToMethod\u003Ec__AnonStoreyBA() { methodName = methodName }.\u003C\u003Em__229));
    }

    public bool HasDefinedMethod(string methodName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return this.definedMethods.Any<string>(new Func<string, bool>(new AssemblyReferenceChecker.\u003CHasDefinedMethod\u003Ec__AnonStoreyBB() { methodName = methodName }.\u003C\u003Em__22A));
    }

    public bool HasReferenceToType(string typeName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return this.referencedTypes.Any<string>(new Func<string, bool>(new AssemblyReferenceChecker.\u003CHasReferenceToType\u003Ec__AnonStoreyBC() { typeName = typeName }.\u003C\u003Em__22B));
    }

    public AssemblyDefinition[] GetAssemblyDefinitions()
    {
      return ((IEnumerable<AssemblyDefinition>) this.assemblyDefinitions).ToArray<AssemblyDefinition>();
    }

    public string[] GetAssemblyFileNames()
    {
      return this.assemblyFileNames.ToArray<string>();
    }

    public string WhoReferencesClass(string klass, bool ignoreSystemDlls)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssemblyReferenceChecker.\u003CWhoReferencesClass\u003Ec__AnonStoreyBD classCAnonStoreyBd = new AssemblyReferenceChecker.\u003CWhoReferencesClass\u003Ec__AnonStoreyBD();
      // ISSUE: reference to a compiler-generated field
      classCAnonStoreyBd.klass = klass;
      using (HashSet<AssemblyDefinition>.Enumerator enumerator = this.assemblyDefinitions.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AssemblyDefinition current = enumerator.Current;
          if (!ignoreSystemDlls || !this.IsiPhoneIgnoredSystemDll(((AssemblyNameReference) current.get_Name()).get_Name()))
          {
            // ISSUE: reference to a compiler-generated method
            if (MonoAOTRegistration.BuildReferencedTypeList(new AssemblyDefinition[1]{ current }).Any<string>(new Func<string, bool>(classCAnonStoreyBd.\u003C\u003Em__22C)))
              return ((AssemblyNameReference) current.get_Name()).get_Name();
          }
        }
      }
      return (string) null;
    }

    private bool IsiPhoneIgnoredSystemDll(string name)
    {
      if (!name.StartsWith("System") && !name.Equals("UnityEngine") && !name.Equals("UnityEngine.Networking"))
        return name.Equals("Mono.Posix");
      return true;
    }

    public static bool GetScriptsHaveMouseEvents(string path)
    {
      AssemblyReferenceChecker referenceChecker = new AssemblyReferenceChecker();
      referenceChecker.CollectReferences(path, true, 0.0f, true);
      return referenceChecker.HasDefinedMethod("OnMouse");
    }
  }
}

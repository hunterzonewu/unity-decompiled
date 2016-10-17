// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssemblyTypeInfoGenerator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using Mono.Cecil;
using Mono.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.SerializationLogic;
using UnityEngine;

namespace UnityEditor
{
  internal class AssemblyTypeInfoGenerator
  {
    private List<AssemblyTypeInfoGenerator.ClassInfo> classes_ = new List<AssemblyTypeInfoGenerator.ClassInfo>();
    private TypeResolver typeResolver = new TypeResolver((GenericInstanceMethod) null);
    private AssemblyDefinition assembly_;

    public AssemblyTypeInfoGenerator.ClassInfo[] ClassInfoArray
    {
      get
      {
        return this.classes_.ToArray();
      }
    }

    public AssemblyTypeInfoGenerator(string assembly, string[] searchDirs)
    {
      string str = assembly;
      ReaderParameters readerParameters1 = new ReaderParameters();
      readerParameters1.set_AssemblyResolver(AssemblyTypeInfoGenerator.AssemblyResolver.WithSearchDirs(searchDirs));
      ReaderParameters readerParameters2 = readerParameters1;
      this.assembly_ = AssemblyDefinition.ReadAssembly(str, readerParameters2);
    }

    public AssemblyTypeInfoGenerator(string assembly, IAssemblyResolver resolver)
    {
      string str = assembly;
      ReaderParameters readerParameters1 = new ReaderParameters();
      readerParameters1.set_AssemblyResolver(resolver);
      ReaderParameters readerParameters2 = readerParameters1;
      this.assembly_ = AssemblyDefinition.ReadAssembly(str, readerParameters2);
    }

    private string GetMonoEmbeddedFullTypeNameFor(TypeReference type)
    {
      TypeSpecification typeSpecification = type as TypeSpecification;
      return (typeSpecification == null || !((TypeReference) typeSpecification).get_IsRequiredModifier() ? (!type.get_IsRequiredModifier() ? type.get_FullName() : type.GetElementType().get_FullName()) : typeSpecification.get_ElementType().get_FullName()).Replace('/', '+').Replace('<', '[').Replace('>', ']');
    }

    private TypeReference ResolveGenericInstanceType(TypeReference typeToResolve, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      ArrayType arrayType = typeToResolve as ArrayType;
      if (arrayType != null)
        typeToResolve = (TypeReference) new ArrayType(this.ResolveGenericInstanceType(((TypeSpecification) arrayType).get_ElementType(), genericInstanceTypeMap), arrayType.get_Rank());
      while (genericInstanceTypeMap.ContainsKey(typeToResolve))
        typeToResolve = genericInstanceTypeMap[typeToResolve];
      if (typeToResolve.get_IsGenericInstance())
      {
        GenericInstanceType genericInstanceType = (GenericInstanceType) typeToResolve;
        typeToResolve = this.MakeGenericInstance(((TypeSpecification) genericInstanceType).get_ElementType(), (IEnumerable<TypeReference>) genericInstanceType.get_GenericArguments(), genericInstanceTypeMap);
      }
      return typeToResolve;
    }

    private void AddType(TypeReference typeRef, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssemblyTypeInfoGenerator.\u003CAddType\u003Ec__AnonStorey67 typeCAnonStorey67 = new AssemblyTypeInfoGenerator.\u003CAddType\u003Ec__AnonStorey67();
      // ISSUE: reference to a compiler-generated field
      typeCAnonStorey67.typeRef = typeRef;
      // ISSUE: reference to a compiler-generated field
      typeCAnonStorey67.\u003C\u003Ef__this = this;
      // ISSUE: reference to a compiler-generated method
      if (this.classes_.Any<AssemblyTypeInfoGenerator.ClassInfo>(new Func<AssemblyTypeInfoGenerator.ClassInfo, bool>(typeCAnonStorey67.\u003C\u003Em__DE)))
        return;
      TypeDefinition type;
      try
      {
        // ISSUE: reference to a compiler-generated field
        type = typeCAnonStorey67.typeRef.Resolve();
      }
      catch (AssemblyResolutionException ex)
      {
        return;
      }
      catch (NotSupportedException ex)
      {
        return;
      }
      if (type == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (typeCAnonStorey67.typeRef.get_IsGenericInstance())
      {
        // ISSUE: reference to a compiler-generated field
        Collection<TypeReference> genericArguments = ((GenericInstanceType) typeCAnonStorey67.typeRef).get_GenericArguments();
        Collection<GenericParameter> genericParameters = type.get_GenericParameters();
        for (int index = 0; index < genericArguments.get_Count(); ++index)
        {
          if (genericParameters.get_Item(index) != genericArguments.get_Item(index))
            genericInstanceTypeMap[(TypeReference) genericParameters.get_Item(index)] = genericArguments.get_Item(index);
        }
        // ISSUE: reference to a compiler-generated field
        this.typeResolver.Add((GenericInstanceType) typeCAnonStorey67.typeRef);
      }
      bool flag = false;
      try
      {
        flag = UnitySerializationLogic.ShouldImplementIDeserializable((TypeReference) type);
      }
      catch
      {
      }
      if (!flag)
      {
        this.AddNestedTypes(type, genericInstanceTypeMap);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.classes_.Add(new AssemblyTypeInfoGenerator.ClassInfo()
        {
          name = this.GetMonoEmbeddedFullTypeNameFor(typeCAnonStorey67.typeRef),
          fields = this.GetFields(type, typeCAnonStorey67.typeRef.get_IsGenericInstance(), genericInstanceTypeMap)
        });
        this.AddNestedTypes(type, genericInstanceTypeMap);
        // ISSUE: reference to a compiler-generated field
        this.AddBaseType(typeCAnonStorey67.typeRef, genericInstanceTypeMap);
      }
      // ISSUE: reference to a compiler-generated field
      if (!typeCAnonStorey67.typeRef.get_IsGenericInstance())
        return;
      // ISSUE: reference to a compiler-generated field
      this.typeResolver.Remove((GenericInstanceType) typeCAnonStorey67.typeRef);
    }

    private void AddNestedTypes(TypeDefinition type, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      using (Collection<TypeDefinition>.Enumerator enumerator = type.get_NestedTypes().GetEnumerator())
      {
        // ISSUE: explicit reference operation
        while (((Collection<TypeDefinition>.Enumerator) @enumerator).MoveNext())
        {
          // ISSUE: explicit reference operation
          this.AddType((TypeReference) ((Collection<TypeDefinition>.Enumerator) @enumerator).get_Current(), genericInstanceTypeMap);
        }
      }
    }

    private void AddBaseType(TypeReference typeRef, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      TypeReference typeRef1 = typeRef.Resolve().get_BaseType();
      if (typeRef1 == null)
        return;
      if (typeRef.get_IsGenericInstance() && typeRef1.get_IsGenericInstance())
      {
        GenericInstanceType genericInstanceType = (GenericInstanceType) typeRef1;
        typeRef1 = this.MakeGenericInstance(((TypeSpecification) genericInstanceType).get_ElementType(), (IEnumerable<TypeReference>) genericInstanceType.get_GenericArguments(), genericInstanceTypeMap);
      }
      this.AddType(typeRef1, genericInstanceTypeMap);
    }

    private TypeReference MakeGenericInstance(TypeReference genericClass, IEnumerable<TypeReference> arguments, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssemblyTypeInfoGenerator.\u003CMakeGenericInstance\u003Ec__AnonStorey68 instanceCAnonStorey68 = new AssemblyTypeInfoGenerator.\u003CMakeGenericInstance\u003Ec__AnonStorey68();
      // ISSUE: reference to a compiler-generated field
      instanceCAnonStorey68.genericInstanceTypeMap = genericInstanceTypeMap;
      // ISSUE: reference to a compiler-generated field
      instanceCAnonStorey68.\u003C\u003Ef__this = this;
      GenericInstanceType genericInstanceType = new GenericInstanceType(genericClass);
      // ISSUE: reference to a compiler-generated method
      using (IEnumerator<TypeReference> enumerator = arguments.Select<TypeReference, TypeReference>(new Func<TypeReference, TypeReference>(instanceCAnonStorey68.\u003C\u003Em__DF)).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          TypeReference current = enumerator.Current;
          genericInstanceType.get_GenericArguments().Add(current);
        }
      }
      return (TypeReference) genericInstanceType;
    }

    private AssemblyTypeInfoGenerator.FieldInfo[] GetFields(TypeDefinition type, bool isGenericInstance, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      List<AssemblyTypeInfoGenerator.FieldInfo> fieldInfoList = new List<AssemblyTypeInfoGenerator.FieldInfo>();
      using (Collection<FieldDefinition>.Enumerator enumerator = type.get_Fields().GetEnumerator())
      {
        // ISSUE: explicit reference operation
        while (((Collection<FieldDefinition>.Enumerator) @enumerator).MoveNext())
        {
          // ISSUE: explicit reference operation
          FieldDefinition current = ((Collection<FieldDefinition>.Enumerator) @enumerator).get_Current();
          AssemblyTypeInfoGenerator.FieldInfo? fieldInfo = this.GetFieldInfo(type, current, isGenericInstance, genericInstanceTypeMap);
          if (fieldInfo.HasValue)
            fieldInfoList.Add(fieldInfo.Value);
        }
      }
      return fieldInfoList.ToArray();
    }

    private AssemblyTypeInfoGenerator.FieldInfo? GetFieldInfo(TypeDefinition type, FieldDefinition field, bool isDeclaringTypeGenericInstance, Dictionary<TypeReference, TypeReference> genericInstanceTypeMap)
    {
      if (!this.WillSerialize(field))
        return new AssemblyTypeInfoGenerator.FieldInfo?();
      AssemblyTypeInfoGenerator.FieldInfo fieldInfo = new AssemblyTypeInfoGenerator.FieldInfo();
      fieldInfo.name = ((MemberReference) field).get_Name();
      TypeReference type1 = !isDeclaringTypeGenericInstance ? ((FieldReference) field).get_FieldType() : this.ResolveGenericInstanceType(((FieldReference) field).get_FieldType(), genericInstanceTypeMap);
      fieldInfo.type = this.GetMonoEmbeddedFullTypeNameFor(type1);
      return new AssemblyTypeInfoGenerator.FieldInfo?(fieldInfo);
    }

    private bool WillSerialize(FieldDefinition field)
    {
      try
      {
        return UnitySerializationLogic.WillUnitySerialize(field, this.typeResolver);
      }
      catch (Exception ex)
      {
        Debug.LogFormat("Field '{0}' from '{1}', exception {2}", (object) ((FieldReference) field).get_FullName(), (object) ((MemberReference) field).get_Module().get_FullyQualifiedName(), (object) ex.Message);
        return false;
      }
    }

    public AssemblyTypeInfoGenerator.ClassInfo[] GatherClassInfo()
    {
      using (Collection<ModuleDefinition>.Enumerator enumerator1 = this.assembly_.get_Modules().GetEnumerator())
      {
        // ISSUE: explicit reference operation
        while (((Collection<ModuleDefinition>.Enumerator) @enumerator1).MoveNext())
        {
          // ISSUE: explicit reference operation
          using (Collection<TypeDefinition>.Enumerator enumerator2 = ((Collection<ModuleDefinition>.Enumerator) @enumerator1).get_Current().get_Types().GetEnumerator())
          {
            // ISSUE: explicit reference operation
            while (((Collection<TypeDefinition>.Enumerator) @enumerator2).MoveNext())
            {
              // ISSUE: explicit reference operation
              TypeDefinition current = ((Collection<TypeDefinition>.Enumerator) @enumerator2).get_Current();
              if (!(((TypeReference) current).get_Name() == "<Module>"))
                this.AddType((TypeReference) current, new Dictionary<TypeReference, TypeReference>());
            }
          }
        }
      }
      return this.classes_.ToArray();
    }

    public struct FieldInfo
    {
      public string name;
      public string type;
    }

    public struct ClassInfo
    {
      public string name;
      public AssemblyTypeInfoGenerator.FieldInfo[] fields;
    }

    private class AssemblyResolver : BaseAssemblyResolver
    {
      private readonly IDictionary m_Assemblies;

      private AssemblyResolver()
        : this((IDictionary) new Hashtable())
      {
      }

      private AssemblyResolver(IDictionary assemblyCache)
      {
        base.\u002Ector();
        this.m_Assemblies = assemblyCache;
      }

      public static IAssemblyResolver WithSearchDirs(params string[] searchDirs)
      {
        AssemblyTypeInfoGenerator.AssemblyResolver assemblyResolver = new AssemblyTypeInfoGenerator.AssemblyResolver();
        foreach (string searchDir in searchDirs)
          assemblyResolver.AddSearchDirectory(searchDir);
        return (IAssemblyResolver) assemblyResolver;
      }

      public virtual AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
      {
        AssemblyDefinition assembly = (AssemblyDefinition) this.m_Assemblies[(object) name.get_Name()];
        if (assembly != null)
          return assembly;
        AssemblyDefinition assemblyDefinition = base.Resolve(name, parameters);
        this.m_Assemblies[(object) name.get_Name()] = (object) assemblyDefinition;
        return assemblyDefinition;
      }
    }
  }
}

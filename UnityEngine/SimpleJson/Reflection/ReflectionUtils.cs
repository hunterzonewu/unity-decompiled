// Decompiled with JetBrains decompiler
// Type: SimpleJson.Reflection.ReflectionUtils
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace SimpleJson.Reflection
{
  [GeneratedCode("reflection-utils", "1.0.0")]
  internal class ReflectionUtils
  {
    private static readonly object[] EmptyObjects = new object[0];

    public static Attribute GetAttribute(MemberInfo info, Type type)
    {
      if (info == null || type == null || !Attribute.IsDefined(info, type))
        return (Attribute) null;
      return Attribute.GetCustomAttribute(info, type);
    }

    public static Attribute GetAttribute(Type objectType, Type attributeType)
    {
      if (objectType == null || attributeType == null || !Attribute.IsDefined((MemberInfo) objectType, attributeType))
        return (Attribute) null;
      return Attribute.GetCustomAttribute((MemberInfo) objectType, attributeType);
    }

    public static Type[] GetGenericTypeArguments(Type type)
    {
      return type.GetGenericArguments();
    }

    public static bool IsTypeGenericeCollectionInterface(Type type)
    {
      if (!type.IsGenericType)
        return false;
      Type genericTypeDefinition = type.GetGenericTypeDefinition();
      if (genericTypeDefinition != typeof (IList<>) && genericTypeDefinition != typeof (ICollection<>))
        return genericTypeDefinition == typeof (IEnumerable<>);
      return true;
    }

    public static bool IsAssignableFrom(Type type1, Type type2)
    {
      return type1.IsAssignableFrom(type2);
    }

    public static bool IsTypeDictionary(Type type)
    {
      if (typeof (IDictionary).IsAssignableFrom(type))
        return true;
      if (!type.IsGenericType)
        return false;
      return type.GetGenericTypeDefinition() == typeof (IDictionary<,>);
    }

    public static bool IsNullableType(Type type)
    {
      if (type.IsGenericType)
        return type.GetGenericTypeDefinition() == typeof (Nullable<>);
      return false;
    }

    public static object ToNullableType(object obj, Type nullableType)
    {
      if (obj == null)
        return (object) null;
      return Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static bool IsValueType(Type type)
    {
      return type.IsValueType;
    }

    public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
    {
      return (IEnumerable<ConstructorInfo>) type.GetConstructors();
    }

    public static ConstructorInfo GetConstructorInfo(Type type, params Type[] argsType)
    {
      foreach (ConstructorInfo constructor in ReflectionUtils.GetConstructors(type))
      {
        ParameterInfo[] parameters = constructor.GetParameters();
        if (argsType.Length == parameters.Length)
        {
          int index = 0;
          bool flag = true;
          foreach (ParameterInfo parameter in constructor.GetParameters())
          {
            if (parameter.ParameterType != argsType[index])
            {
              flag = false;
              break;
            }
          }
          if (flag)
            return constructor;
        }
      }
      return (ConstructorInfo) null;
    }

    public static IEnumerable<PropertyInfo> GetProperties(Type type)
    {
      return (IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    }

    public static IEnumerable<FieldInfo> GetFields(Type type)
    {
      return (IEnumerable<FieldInfo>) type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    }

    public static MethodInfo GetGetterMethodInfo(PropertyInfo propertyInfo)
    {
      return propertyInfo.GetGetMethod(true);
    }

    public static MethodInfo GetSetterMethodInfo(PropertyInfo propertyInfo)
    {
      return propertyInfo.GetSetMethod(true);
    }

    public static ReflectionUtils.ConstructorDelegate GetContructor(ConstructorInfo constructorInfo)
    {
      return ReflectionUtils.GetConstructorByReflection(constructorInfo);
    }

    public static ReflectionUtils.ConstructorDelegate GetContructor(Type type, params Type[] argsType)
    {
      return ReflectionUtils.GetConstructorByReflection(type, argsType);
    }

    public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(ConstructorInfo constructorInfo)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new ReflectionUtils.ConstructorDelegate(new ReflectionUtils.\u003CGetConstructorByReflection\u003Ec__AnonStorey2() { constructorInfo = constructorInfo }.\u003C\u003Em__6);
    }

    public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(Type type, params Type[] argsType)
    {
      ConstructorInfo constructorInfo = ReflectionUtils.GetConstructorInfo(type, argsType);
      if (constructorInfo == null)
        return (ReflectionUtils.ConstructorDelegate) null;
      return ReflectionUtils.GetConstructorByReflection(constructorInfo);
    }

    public static ReflectionUtils.GetDelegate GetGetMethod(PropertyInfo propertyInfo)
    {
      return ReflectionUtils.GetGetMethodByReflection(propertyInfo);
    }

    public static ReflectionUtils.GetDelegate GetGetMethod(FieldInfo fieldInfo)
    {
      return ReflectionUtils.GetGetMethodByReflection(fieldInfo);
    }

    public static ReflectionUtils.GetDelegate GetGetMethodByReflection(PropertyInfo propertyInfo)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new ReflectionUtils.GetDelegate(new ReflectionUtils.\u003CGetGetMethodByReflection\u003Ec__AnonStorey3() { methodInfo = ReflectionUtils.GetGetterMethodInfo(propertyInfo) }.\u003C\u003Em__7);
    }

    public static ReflectionUtils.GetDelegate GetGetMethodByReflection(FieldInfo fieldInfo)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new ReflectionUtils.GetDelegate(new ReflectionUtils.\u003CGetGetMethodByReflection\u003Ec__AnonStorey4() { fieldInfo = fieldInfo }.\u003C\u003Em__8);
    }

    public static ReflectionUtils.SetDelegate GetSetMethod(PropertyInfo propertyInfo)
    {
      return ReflectionUtils.GetSetMethodByReflection(propertyInfo);
    }

    public static ReflectionUtils.SetDelegate GetSetMethod(FieldInfo fieldInfo)
    {
      return ReflectionUtils.GetSetMethodByReflection(fieldInfo);
    }

    public static ReflectionUtils.SetDelegate GetSetMethodByReflection(PropertyInfo propertyInfo)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new ReflectionUtils.SetDelegate(new ReflectionUtils.\u003CGetSetMethodByReflection\u003Ec__AnonStorey5() { methodInfo = ReflectionUtils.GetSetterMethodInfo(propertyInfo) }.\u003C\u003Em__9);
    }

    public static ReflectionUtils.SetDelegate GetSetMethodByReflection(FieldInfo fieldInfo)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new ReflectionUtils.SetDelegate(new ReflectionUtils.\u003CGetSetMethodByReflection\u003Ec__AnonStorey6() { fieldInfo = fieldInfo }.\u003C\u003Em__A);
    }

    public sealed class ThreadSafeDictionary<TKey, TValue> : IEnumerable, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
      private readonly object _lock = new object();
      private readonly ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> _valueFactory;
      private Dictionary<TKey, TValue> _dictionary;

      public ICollection<TKey> Keys
      {
        get
        {
          return (ICollection<TKey>) this._dictionary.Keys;
        }
      }

      public ICollection<TValue> Values
      {
        get
        {
          return (ICollection<TValue>) this._dictionary.Values;
        }
      }

      public TValue this[TKey key]
      {
        get
        {
          return this.Get(key);
        }
        set
        {
          throw new NotImplementedException();
        }
      }

      public int Count
      {
        get
        {
          return this._dictionary.Count;
        }
      }

      public bool IsReadOnly
      {
        get
        {
          throw new NotImplementedException();
        }
      }

      public ThreadSafeDictionary(ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> valueFactory)
      {
        this._valueFactory = valueFactory;
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return (IEnumerator) this._dictionary.GetEnumerator();
      }

      private TValue Get(TKey key)
      {
        TValue obj;
        if (this._dictionary == null || !this._dictionary.TryGetValue(key, out obj))
          return this.AddValue(key);
        return obj;
      }

      private TValue AddValue(TKey key)
      {
        TValue obj = this._valueFactory(key);
        lock (this._lock)
        {
          if (this._dictionary == null)
          {
            this._dictionary = new Dictionary<TKey, TValue>();
            this._dictionary[key] = obj;
          }
          else
          {
            TValue local_2;
            if (this._dictionary.TryGetValue(key, out local_2))
              return local_2;
            Dictionary<TKey, TValue> local_3 = new Dictionary<TKey, TValue>((IDictionary<TKey, TValue>) this._dictionary);
            local_3[key] = obj;
            this._dictionary = local_3;
          }
        }
        return obj;
      }

      public void Add(TKey key, TValue value)
      {
        throw new NotImplementedException();
      }

      public bool ContainsKey(TKey key)
      {
        return this._dictionary.ContainsKey(key);
      }

      public bool Remove(TKey key)
      {
        throw new NotImplementedException();
      }

      public bool TryGetValue(TKey key, out TValue value)
      {
        value = this[key];
        return true;
      }

      public void Add(KeyValuePair<TKey, TValue> item)
      {
        throw new NotImplementedException();
      }

      public void Clear()
      {
        throw new NotImplementedException();
      }

      public bool Contains(KeyValuePair<TKey, TValue> item)
      {
        throw new NotImplementedException();
      }

      public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
      {
        throw new NotImplementedException();
      }

      public bool Remove(KeyValuePair<TKey, TValue> item)
      {
        throw new NotImplementedException();
      }

      public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
      {
        return (IEnumerator<KeyValuePair<TKey, TValue>>) this._dictionary.GetEnumerator();
      }
    }

    public delegate object GetDelegate(object source);

    public delegate void SetDelegate(object source, object value);

    public delegate object ConstructorDelegate(params object[] args);

    public delegate TValue ThreadSafeDictionaryValueFactory<TKey, TValue>(TKey key);
  }
}

// Decompiled with JetBrains decompiler
// Type: SimpleJson.PocoJsonSerializerStrategy
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using SimpleJson.Reflection;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace SimpleJson
{
  [GeneratedCode("simple-json", "1.0.0")]
  internal class PocoJsonSerializerStrategy : IJsonSerializerStrategy
  {
    internal static readonly Type[] EmptyTypes = new Type[0];
    internal static readonly Type[] ArrayConstructorParameterTypes = new Type[1]{ typeof (int) };
    private static readonly string[] Iso8601Format = new string[3]{ "yyyy-MM-dd\\THH:mm:ss.FFFFFFF\\Z", "yyyy-MM-dd\\THH:mm:ss\\Z", "yyyy-MM-dd\\THH:mm:ssK" };
    internal IDictionary<Type, ReflectionUtils.ConstructorDelegate> ConstructorCache;
    internal IDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>> GetCache;
    internal IDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>> SetCache;

    public PocoJsonSerializerStrategy()
    {
      this.ConstructorCache = (IDictionary<Type, ReflectionUtils.ConstructorDelegate>) new ReflectionUtils.ThreadSafeDictionary<Type, ReflectionUtils.ConstructorDelegate>(new ReflectionUtils.ThreadSafeDictionaryValueFactory<Type, ReflectionUtils.ConstructorDelegate>(this.ContructorDelegateFactory));
      this.GetCache = (IDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>>) new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>>(new ReflectionUtils.ThreadSafeDictionaryValueFactory<Type, IDictionary<string, ReflectionUtils.GetDelegate>>(this.GetterValueFactory));
      this.SetCache = (IDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>) new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>(new ReflectionUtils.ThreadSafeDictionaryValueFactory<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>(this.SetterValueFactory));
    }

    protected virtual string MapClrMemberNameToJsonFieldName(string clrPropertyName)
    {
      return clrPropertyName;
    }

    internal virtual ReflectionUtils.ConstructorDelegate ContructorDelegateFactory(Type key)
    {
      return ReflectionUtils.GetContructor(key, !key.IsArray ? PocoJsonSerializerStrategy.EmptyTypes : PocoJsonSerializerStrategy.ArrayConstructorParameterTypes);
    }

    internal virtual IDictionary<string, ReflectionUtils.GetDelegate> GetterValueFactory(Type type)
    {
      IDictionary<string, ReflectionUtils.GetDelegate> dictionary = (IDictionary<string, ReflectionUtils.GetDelegate>) new Dictionary<string, ReflectionUtils.GetDelegate>();
      foreach (PropertyInfo property in ReflectionUtils.GetProperties(type))
      {
        if (property.CanRead)
        {
          MethodInfo getterMethodInfo = ReflectionUtils.GetGetterMethodInfo(property);
          if (!getterMethodInfo.IsStatic && getterMethodInfo.IsPublic)
            dictionary[this.MapClrMemberNameToJsonFieldName(property.Name)] = ReflectionUtils.GetGetMethod(property);
        }
      }
      foreach (FieldInfo field in ReflectionUtils.GetFields(type))
      {
        if (!field.IsStatic && field.IsPublic)
          dictionary[this.MapClrMemberNameToJsonFieldName(field.Name)] = ReflectionUtils.GetGetMethod(field);
      }
      return dictionary;
    }

    internal virtual IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> SetterValueFactory(Type type)
    {
      IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> dictionary = (IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>) new Dictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>();
      foreach (PropertyInfo property in ReflectionUtils.GetProperties(type))
      {
        if (property.CanWrite)
        {
          MethodInfo setterMethodInfo = ReflectionUtils.GetSetterMethodInfo(property);
          if (!setterMethodInfo.IsStatic && setterMethodInfo.IsPublic)
            dictionary[this.MapClrMemberNameToJsonFieldName(property.Name)] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(property.PropertyType, ReflectionUtils.GetSetMethod(property));
        }
      }
      foreach (FieldInfo field in ReflectionUtils.GetFields(type))
      {
        if (!field.IsInitOnly && !field.IsStatic && field.IsPublic)
          dictionary[this.MapClrMemberNameToJsonFieldName(field.Name)] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(field.FieldType, ReflectionUtils.GetSetMethod(field));
      }
      return dictionary;
    }

    public virtual bool TrySerializeNonPrimitiveObject(object input, out object output)
    {
      if (!this.TrySerializeKnownTypes(input, out output))
        return this.TrySerializeUnknownTypes(input, out output);
      return true;
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    public virtual object DeserializeObject(object value, Type type)
    {
      if (type == null)
        throw new ArgumentNullException("type");
      string str = value as string;
      if (type == typeof (Guid) && string.IsNullOrEmpty(str))
        return (object) new Guid();
      if (value == null)
        return (object) null;
      object source = (object) null;
      if (str != null)
      {
        if (str.Length != 0)
        {
          if (type == typeof (DateTime) || ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof (DateTime))
            return (object) DateTime.ParseExact(str, PocoJsonSerializerStrategy.Iso8601Format, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
          if (type == typeof (DateTimeOffset) || ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof (DateTimeOffset))
            return (object) DateTimeOffset.ParseExact(str, PocoJsonSerializerStrategy.Iso8601Format, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
          if (type == typeof (Guid) || ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof (Guid))
            return (object) new Guid(str);
          return (object) str;
        }
        source = type != typeof (Guid) ? (!ReflectionUtils.IsNullableType(type) || Nullable.GetUnderlyingType(type) != typeof (Guid) ? (object) str : (object) null) : (object) new Guid();
        if (!ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof (Guid))
          return (object) str;
      }
      else if (value is bool)
        return value;
      bool flag1 = value is long;
      bool flag2 = value is double;
      if (flag1 && type == typeof (long) || flag2 && type == typeof (double))
        return value;
      if (flag2 && type != typeof (double) || flag1 && type != typeof (long))
      {
        object obj = !typeof (IConvertible).IsAssignableFrom(type) ? value : Convert.ChangeType(value, type, (IFormatProvider) CultureInfo.InvariantCulture);
        if (ReflectionUtils.IsNullableType(type))
          return ReflectionUtils.ToNullableType(obj, type);
        return obj;
      }
      IDictionary<string, object> dictionary1 = value as IDictionary<string, object>;
      if (dictionary1 != null)
      {
        IDictionary<string, object> dictionary2 = dictionary1;
        if (ReflectionUtils.IsTypeDictionary(type))
        {
          Type[] genericTypeArguments = ReflectionUtils.GetGenericTypeArguments(type);
          Type type1 = genericTypeArguments[0];
          Type type2 = genericTypeArguments[1];
          IDictionary dictionary3 = (IDictionary) this.ConstructorCache[typeof (Dictionary<,>).MakeGenericType(type1, type2)]((object[]) null);
          foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) dictionary2)
            dictionary3.Add((object) keyValuePair.Key, this.DeserializeObject(keyValuePair.Value, type2));
          source = (object) dictionary3;
        }
        else if (type == typeof (object))
        {
          source = value;
        }
        else
        {
          source = this.ConstructorCache[type]((object[]) null);
          foreach (KeyValuePair<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> keyValuePair in (IEnumerable<KeyValuePair<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>) this.SetCache[type])
          {
            object obj;
            if (dictionary2.TryGetValue(keyValuePair.Key, out obj))
            {
              obj = this.DeserializeObject(obj, keyValuePair.Value.Key);
              keyValuePair.Value.Value(source, obj);
            }
          }
        }
      }
      else
      {
        IList<object> objectList1 = value as IList<object>;
        if (objectList1 != null)
        {
          IList<object> objectList2 = objectList1;
          IList list = (IList) null;
          if (type.IsArray)
          {
            list = (IList) this.ConstructorCache[type](new object[1]
            {
              (object) objectList2.Count
            });
            int num = 0;
            foreach (object obj in (IEnumerable<object>) objectList2)
              list[num++] = this.DeserializeObject(obj, type.GetElementType());
          }
          else if (ReflectionUtils.IsTypeGenericeCollectionInterface(type) || ReflectionUtils.IsAssignableFrom(typeof (IList), type))
          {
            Type genericTypeArgument = ReflectionUtils.GetGenericTypeArguments(type)[0];
            list = (IList) this.ConstructorCache[typeof (List<>).MakeGenericType(genericTypeArgument)](new object[1]
            {
              (object) objectList2.Count
            });
            foreach (object obj in (IEnumerable<object>) objectList2)
              list.Add(this.DeserializeObject(obj, genericTypeArgument));
          }
          source = (object) list;
        }
      }
      return source;
    }

    protected virtual object SerializeEnum(Enum p)
    {
      return (object) Convert.ToDouble((object) p, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
    protected virtual bool TrySerializeKnownTypes(object input, out object output)
    {
      bool flag = true;
      if (input is DateTime)
        output = (object) ((DateTime) input).ToUniversalTime().ToString(PocoJsonSerializerStrategy.Iso8601Format[0], (IFormatProvider) CultureInfo.InvariantCulture);
      else if (input is DateTimeOffset)
        output = (object) ((DateTimeOffset) input).ToUniversalTime().ToString(PocoJsonSerializerStrategy.Iso8601Format[0], (IFormatProvider) CultureInfo.InvariantCulture);
      else if (input is Guid)
        output = (object) ((Guid) input).ToString("D");
      else if ((object) (input as Uri) != null)
      {
        output = (object) input.ToString();
      }
      else
      {
        Enum p = input as Enum;
        if (p != null)
        {
          output = this.SerializeEnum(p);
        }
        else
        {
          flag = false;
          output = (object) null;
        }
      }
      return flag;
    }

    [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
    protected virtual bool TrySerializeUnknownTypes(object input, out object output)
    {
      if (input == null)
        throw new ArgumentNullException("input");
      output = (object) null;
      Type type = input.GetType();
      if (type.FullName == null)
        return false;
      IDictionary<string, object> dictionary = (IDictionary<string, object>) new JsonObject();
      foreach (KeyValuePair<string, ReflectionUtils.GetDelegate> keyValuePair in (IEnumerable<KeyValuePair<string, ReflectionUtils.GetDelegate>>) this.GetCache[type])
      {
        if (keyValuePair.Value != null)
          dictionary.Add(this.MapClrMemberNameToJsonFieldName(keyValuePair.Key), keyValuePair.Value(input));
      }
      output = (object) dictionary;
      return true;
    }
  }
}

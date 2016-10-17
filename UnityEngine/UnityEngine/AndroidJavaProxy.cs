// Decompiled with JetBrains decompiler
// Type: UnityEngine.AndroidJavaProxy
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Reflection;

namespace UnityEngine
{
  /// <summary>
  ///   <para>This class can be used to implement any java interface. Any java vm method invocation matching the interface on the proxy object will automatically be passed to the c# implementation.</para>
  /// </summary>
  public class AndroidJavaProxy
  {
    /// <summary>
    ///   <para>Java interface implemented by the proxy.</para>
    /// </summary>
    public readonly AndroidJavaClass javaInterface;

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="javaInterface">Java interface to be implemented by the proxy.</param>
    public AndroidJavaProxy(string javaInterface)
      : this(new AndroidJavaClass(javaInterface))
    {
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="javaInterface">Java interface to be implemented by the proxy.</param>
    public AndroidJavaProxy(AndroidJavaClass javaInterface)
    {
      this.javaInterface = javaInterface;
    }

    /// <summary>
    ///   <para>Called by the java vm whenever a method is invoked on the java proxy interface. You can override this to run special code on method invokation, or you can leave the implementation as is, and leave the default behavior which is to look for c# methods matching the signature of the java method.</para>
    /// </summary>
    /// <param name="methodName">Name of the invoked java method.</param>
    /// <param name="args">Arguments passed from the java vm - converted into AndroidJavaObject, AndroidJavaClass or a primitive.</param>
    /// <param name="javaArgs">Arguments passed from the java vm - all objects are represented by AndroidJavaObject, int for instance is represented by a java.lang.Integer object.</param>
    public virtual AndroidJavaObject Invoke(string methodName, object[] args)
    {
      Exception inner = (Exception) null;
      BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
      System.Type[] types = new System.Type[args.Length];
      for (int index = 0; index < args.Length; ++index)
        types[index] = args[index] != null ? args[index].GetType() : typeof (AndroidJavaObject);
      try
      {
        MethodInfo method = this.GetType().GetMethod(methodName, bindingAttr, (Binder) null, types, (ParameterModifier[]) null);
        if (method != null)
          return _AndroidJNIHelper.Box(method.Invoke((object) this, args));
      }
      catch (TargetInvocationException ex)
      {
        inner = ex.InnerException;
      }
      catch (Exception ex)
      {
        inner = ex;
      }
      string[] strArray = new string[args.Length];
      for (int index = 0; index < types.Length; ++index)
        strArray[index] = types[index].ToString();
      if (inner != null)
        throw new TargetInvocationException(this.GetType().ToString() + "." + methodName + "(" + string.Join(",", strArray) + ")", inner);
      throw new Exception("No such proxy method: " + (object) this.GetType() + "." + methodName + "(" + string.Join(",", strArray) + ")");
    }

    /// <summary>
    ///   <para>Called by the java vm whenever a method is invoked on the java proxy interface. You can override this to run special code on method invokation, or you can leave the implementation as is, and leave the default behavior which is to look for c# methods matching the signature of the java method.</para>
    /// </summary>
    /// <param name="methodName">Name of the invoked java method.</param>
    /// <param name="args">Arguments passed from the java vm - converted into AndroidJavaObject, AndroidJavaClass or a primitive.</param>
    /// <param name="javaArgs">Arguments passed from the java vm - all objects are represented by AndroidJavaObject, int for instance is represented by a java.lang.Integer object.</param>
    public virtual AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
    {
      object[] args = new object[javaArgs.Length];
      for (int index = 0; index < javaArgs.Length; ++index)
        args[index] = _AndroidJNIHelper.Unbox(javaArgs[index]);
      return this.Invoke(methodName, args);
    }
  }
}

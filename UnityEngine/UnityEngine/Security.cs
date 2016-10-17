// Decompiled with JetBrains decompiler
// Type: UnityEngine.Security
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using Mono.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using UnityEngine.Internal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Webplayer security related class.</para>
  /// </summary>
  public sealed class Security
  {
    private static List<Assembly> _verifiedAssemblies = new List<Assembly>();
    private static readonly string kSignatureExtension = ".signature";
    private const string publicVerificationKey = "<RSAKeyValue><Modulus>uP7lsvrE6fNoQWhUIdJnQrgKoGXBkgWgs5l1xmS9gfyNkFSXgugIpfmN/0YrtL57PezYFXN0CogAnOpOtcUmpcIrh524VL/7bIh+jDUaOCG292PIx92dtzqCTvbUdCYUmaag9VlrdAw05FxYQJi2iZ/X6EiuO1TnqpVNFCDb6pXPAssoO4Uxn9JXBzL0muNRdcmFGRiLp7JQOL7a2aeU9mF9qjMprnww0k8COa6tHdnNWJqaxdFO+Etk3os0ns/gQ2FWrztKemM1Wfu7lk/B1F+V2g0adwlTiuyNHw6to+5VQXWK775RXB9wAGr8KhsVD5IJvmxrdBT8KVEWve+OXQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

    /// <summary>
    ///   <para>Prefetch the webplayer socket security policy from a non-default port number.</para>
    /// </summary>
    /// <param name="ip">IP address of server.</param>
    /// <param name="atPort">Port from where socket policy is read.</param>
    /// <param name="timeout">Time to wait for response.</param>
    [ExcludeFromDocs]
    public static bool PrefetchSocketPolicy(string ip, int atPort)
    {
      int timeout = 3000;
      return UnityEngine.Security.PrefetchSocketPolicy(ip, atPort, timeout);
    }

    /// <summary>
    ///   <para>Prefetch the webplayer socket security policy from a non-default port number.</para>
    /// </summary>
    /// <param name="ip">IP address of server.</param>
    /// <param name="atPort">Port from where socket policy is read.</param>
    /// <param name="timeout">Time to wait for response.</param>
    public static bool PrefetchSocketPolicy(string ip, int atPort, [DefaultValue("3000")] int timeout)
    {
      return (bool) UnityEngine.Security.GetUnityCrossDomainHelperMethod("PrefetchSocketPolicy").Invoke((object) null, new object[3]{ (object) ip, (object) atPort, (object) timeout });
    }

    /// <summary>
    ///   <para>Get secret from Chain of Trust system.</para>
    /// </summary>
    /// <param name="name">The name of the secret.</param>
    /// <returns>
    ///   <para>The secret.</para>
    /// </returns>
    [SecuritySafeCritical]
    public static string GetChainOfTrustValue(string name)
    {
      Assembly callingAssembly = Assembly.GetCallingAssembly();
      if (!UnityEngine.Security._verifiedAssemblies.Contains(callingAssembly))
        throw new ArgumentException("Calling assembly needs to be verified by Security.LoadAndVerifyAssembly");
      byte[] publicKeyToken = callingAssembly.GetName().GetPublicKeyToken();
      return UnityEngine.Security.GetChainOfTrustValueInternal(name, UnityEngine.Security.TokenToHex(publicKeyToken));
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetChainOfTrustValueInternal(string name, string publicKeyToken);

    private static MethodInfo GetUnityCrossDomainHelperMethod(string methodname)
    {
      System.Type type = Types.GetType("UnityEngine.UnityCrossDomainHelper", "CrossDomainPolicyParser, Version=1.0.0.0, Culture=neutral");
      if (type == null)
        throw new SecurityException("Cant find type UnityCrossDomainHelper");
      MethodInfo method = type.GetMethod(methodname);
      if (method == null)
        throw new SecurityException("Cant find " + methodname);
      return method;
    }

    internal static string TokenToHex(byte[] token)
    {
      if (token == null || 8 > token.Length)
        return string.Empty;
      return string.Format("{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}{6:x2}{7:x2}", (object) token[0], (object) token[1], (object) token[2], (object) token[3], (object) token[4], (object) token[5], (object) token[6], (object) token[7]);
    }

    internal static void ClearVerifiedAssemblies()
    {
      UnityEngine.Security._verifiedAssemblies.Clear();
    }

    /// <summary>
    ///         <para>Loads an assembly and checks that it is allowed to be used in the webplayer.
    /// Note: The single argument version of this API will always issue an error message.  An authorisation key is always needed.</para>
    ///       </summary>
    /// <param name="assemblyData">Assembly to verify.</param>
    /// <param name="authorizationKey">Public key used to verify assembly.</param>
    /// <returns>
    ///   <para>Loaded, verified, assembly, or null if the assembly cannot be verfied.</para>
    /// </returns>
    [SecuritySafeCritical]
    public static Assembly LoadAndVerifyAssembly(byte[] assemblyData, string authorizationKey)
    {
      RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
      cryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>uP7lsvrE6fNoQWhUIdJnQrgKoGXBkgWgs5l1xmS9gfyNkFSXgugIpfmN/0YrtL57PezYFXN0CogAnOpOtcUmpcIrh524VL/7bIh+jDUaOCG292PIx92dtzqCTvbUdCYUmaag9VlrdAw05FxYQJi2iZ/X6EiuO1TnqpVNFCDb6pXPAssoO4Uxn9JXBzL0muNRdcmFGRiLp7JQOL7a2aeU9mF9qjMprnww0k8COa6tHdnNWJqaxdFO+Etk3os0ns/gQ2FWrztKemM1Wfu7lk/B1F+V2g0adwlTiuyNHw6to+5VQXWK775RXB9wAGr8KhsVD5IJvmxrdBT8KVEWve+OXQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
      byte[] hash = SHA1.Create().ComputeHash(assemblyData);
      byte[] rgbSignature = Convert.FromBase64String(authorizationKey);
      bool flag;
      try
      {
        flag = cryptoServiceProvider.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), rgbSignature);
      }
      catch (CryptographicException ex)
      {
        Debug.LogError((object) "Unable to verify that this assembly has been authorized by Unity.  The assembly will not be loaded.");
        flag = false;
      }
      if (!flag)
        return (Assembly) null;
      return UnityEngine.Security.LoadAndVerifyAssemblyInternal(assemblyData);
    }

    /// <summary>
    ///         <para>Loads an assembly and checks that it is allowed to be used in the webplayer.
    /// Note: The single argument version of this API will always issue an error message.  An authorisation key is always needed.</para>
    ///       </summary>
    /// <param name="assemblyData">Assembly to verify.</param>
    /// <param name="authorizationKey">Public key used to verify assembly.</param>
    /// <returns>
    ///   <para>Loaded, verified, assembly, or null if the assembly cannot be verfied.</para>
    /// </returns>
    [SecuritySafeCritical]
    public static Assembly LoadAndVerifyAssembly(byte[] assemblyData)
    {
      if (Application.GetBuildUnityVersion() < Application.GetNumericUnityVersion("4.5.0a4"))
        return UnityEngine.Security.LoadAndVerifyAssemblyInternal(assemblyData);
      Debug.LogError((object) "Unable to verify assembly data; you must provide an authorization key when loading this assembly.");
      return (Assembly) null;
    }

    [SecuritySafeCritical]
    private static Assembly LoadAndVerifyAssemblyInternal(byte[] assemblyData)
    {
      Assembly assembly = Assembly.Load(assemblyData);
      byte[] publicKey = assembly.GetName().GetPublicKey();
      if (publicKey == null || publicKey.Length == 0)
        return (Assembly) null;
      RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
      cryptoServiceProvider.ImportCspBlob(publicKey);
      using (MemoryStream memoryStream = new MemoryStream(assemblyData))
      {
        if (!new StrongName((RSA) cryptoServiceProvider).Verify((Stream) memoryStream))
          return (Assembly) null;
        UnityEngine.Security._verifiedAssemblies.Add(assembly);
        return assembly;
      }
    }

    internal static bool VerifySignature(string file, byte[] publicKey)
    {
      try
      {
        string path = file + UnityEngine.Security.kSignatureExtension;
        if (!File.Exists(path))
          return false;
        using (RSACryptoServiceProvider cryptoServiceProvider1 = new RSACryptoServiceProvider())
        {
          cryptoServiceProvider1.ImportCspBlob(publicKey);
          using (SHA1CryptoServiceProvider cryptoServiceProvider2 = new SHA1CryptoServiceProvider())
            return cryptoServiceProvider1.VerifyData(File.ReadAllBytes(file), (object) cryptoServiceProvider2, File.ReadAllBytes(path));
        }
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
      return false;
    }
  }
}

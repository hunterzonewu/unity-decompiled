// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkCRC
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This class holds information about which networked scripts use which QoS channels for updates.</para>
  /// </summary>
  public class NetworkCRC
  {
    private Dictionary<string, int> m_Scripts = new Dictionary<string, int>();
    internal static NetworkCRC s_Singleton;
    private bool m_ScriptCRCCheck;

    internal static NetworkCRC singleton
    {
      get
      {
        if (NetworkCRC.s_Singleton == null)
          NetworkCRC.s_Singleton = new NetworkCRC();
        return NetworkCRC.s_Singleton;
      }
    }

    /// <summary>
    ///   <para>A dictionary of script QoS channels.</para>
    /// </summary>
    public Dictionary<string, int> scripts
    {
      get
      {
        return this.m_Scripts;
      }
    }

    /// <summary>
    ///   <para>Enables a CRC check between server and client that ensures the NetworkBehaviour scripts match.</para>
    /// </summary>
    public static bool scriptCRCCheck
    {
      get
      {
        return NetworkCRC.singleton.m_ScriptCRCCheck;
      }
      set
      {
        NetworkCRC.singleton.m_ScriptCRCCheck = value;
      }
    }

    public static void ReinitializeScriptCRCs(Assembly callingAssembly)
    {
      NetworkCRC.singleton.m_Scripts.Clear();
      foreach (System.Type type in callingAssembly.GetTypes())
      {
        if (type.BaseType == typeof (NetworkBehaviour))
        {
          MethodInfo method = type.GetMethod(".cctor", BindingFlags.Static);
          if (method != null)
            method.Invoke((object) null, new object[0]);
        }
      }
    }

    /// <summary>
    ///   <para>This is used to setup script network settings CRC data.</para>
    /// </summary>
    /// <param name="name">Script name.</param>
    /// <param name="channel">QoS Channel.</param>
    public static void RegisterBehaviour(string name, int channel)
    {
      NetworkCRC.singleton.m_Scripts[name] = channel;
    }

    internal static bool Validate(CRCMessageEntry[] scripts, int numChannels)
    {
      return NetworkCRC.singleton.ValidateInternal(scripts, numChannels);
    }

    private bool ValidateInternal(CRCMessageEntry[] remoteScripts, int numChannels)
    {
      if (this.m_Scripts.Count != remoteScripts.Length)
      {
        if (LogFilter.logWarn)
          Debug.LogWarning((object) "Network configuration mismatch detected. The number of networked scripts on the client does not match the number of networked scripts on the server. This could be caused by lazy loading of scripts on the client. This warning can be disabled by the checkbox in NetworkManager Script CRC Check.");
        this.Dump(remoteScripts);
        return false;
      }
      foreach (CRCMessageEntry remoteScript in remoteScripts)
      {
        if (LogFilter.logDebug)
          Debug.Log((object) ("Script: " + remoteScript.name + " Channel: " + (object) remoteScript.channel));
        if (this.m_Scripts.ContainsKey(remoteScript.name))
        {
          int script = this.m_Scripts[remoteScript.name];
          if (script != (int) remoteScript.channel)
          {
            if (LogFilter.logError)
              Debug.LogError((object) ("HLAPI CRC Channel Mismatch. Script: " + remoteScript.name + " LocalChannel: " + (object) script + " RemoteChannel: " + (object) remoteScript.channel));
            this.Dump(remoteScripts);
            return false;
          }
        }
        if ((int) remoteScript.channel >= numChannels)
        {
          if (LogFilter.logError)
            Debug.LogError((object) ("HLAPI CRC channel out of range! Script: " + remoteScript.name + " Channel: " + (object) remoteScript.channel));
          this.Dump(remoteScripts);
          return false;
        }
      }
      return true;
    }

    private void Dump(CRCMessageEntry[] remoteScripts)
    {
      using (Dictionary<string, int>.KeyCollection.Enumerator enumerator = this.m_Scripts.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          Debug.Log((object) ("CRC Local Dump " + current + " : " + (object) this.m_Scripts[current]));
        }
      }
      foreach (CRCMessageEntry remoteScript in remoteScripts)
        Debug.Log((object) ("CRC Remote Dump " + remoteScript.name + " : " + (object) remoteScript.channel));
    }
  }
}

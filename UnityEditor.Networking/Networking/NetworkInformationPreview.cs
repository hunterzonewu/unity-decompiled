// Decompiled with JetBrains decompiler
// Type: UnityEditor.Networking.NetworkInformationPreview
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor.Networking
{
  [CustomPreview(typeof (GameObject))]
  internal class NetworkInformationPreview : ObjectPreview
  {
    private NetworkInformationPreview.Styles m_Styles = new NetworkInformationPreview.Styles();
    private List<NetworkInformationPreview.NetworkIdentityInfo> m_Info;
    private List<NetworkInformationPreview.NetworkBehaviourInfo> m_Behaviours;
    private NetworkIdentity m_Identity;
    private GUIContent m_Title;

    public override void Initialize(Object[] targets)
    {
      base.Initialize(targets);
      this.GetNetworkInformation(this.target as GameObject);
    }

    public override GUIContent GetPreviewTitle()
    {
      if (this.m_Title == null)
        this.m_Title = new GUIContent("Network Information");
      return this.m_Title;
    }

    public override bool HasPreviewGUI()
    {
      if (this.m_Info != null)
        return this.m_Info.Count > 0;
      return false;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (Event.current.type != EventType.Repaint || this.m_Info == null || this.m_Info.Count == 0)
        return;
      if (this.m_Styles == null)
        this.m_Styles = new NetworkInformationPreview.Styles();
      Vector2 vector2 = new Vector2(140f, 16f);
      Vector2 maxNameLabelSize = this.GetMaxNameLabelSize();
      r = new RectOffset(-5, -5, -5, -5).Add(r);
      float x = r.x + 10f;
      float y1 = r.y + 10f;
      Rect position1 = new Rect(x, y1, vector2.x, vector2.y);
      Rect position2 = new Rect(vector2.x, y1, maxNameLabelSize.x, maxNameLabelSize.y);
      using (List<NetworkInformationPreview.NetworkIdentityInfo>.Enumerator enumerator = this.m_Info.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetworkInformationPreview.NetworkIdentityInfo current = enumerator.Current;
          GUI.Label(position1, current.name, this.m_Styles.labelStyle);
          GUI.Label(position2, current.value, this.m_Styles.componentName);
          position1.y += position1.height;
          position1.x = x;
          position2.y += position2.height;
        }
      }
      float y2 = position1.y;
      if (this.m_Behaviours == null || this.m_Behaviours.Count <= 0)
        return;
      Vector2 behaviourLabelSize = this.GetMaxBehaviourLabelSize();
      Rect position3 = new Rect(x, position1.y + 10f, behaviourLabelSize.x, behaviourLabelSize.y);
      GUI.Label(position3, new GUIContent("Network Behaviours"), this.m_Styles.labelStyle);
      position3.x += 20f;
      position3.y += position3.height;
      using (List<NetworkInformationPreview.NetworkBehaviourInfo>.Enumerator enumerator = this.m_Behaviours.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetworkInformationPreview.NetworkBehaviourInfo current = enumerator.Current;
          if (!((Object) current.behaviour == (Object) null))
          {
            if (current.behaviour.enabled)
              GUI.Label(position3, current.name, this.m_Styles.componentName);
            else
              GUI.Label(position3, current.name, this.m_Styles.disabledName);
            position3.y += position3.height;
            y2 = position3.y;
          }
        }
      }
      if (this.m_Identity.observers != null && this.m_Identity.observers.Count > 0)
      {
        Rect position4 = new Rect(x, y2 + 10f, 200f, 20f);
        GUI.Label(position4, new GUIContent("Network observers"), this.m_Styles.labelStyle);
        position4.x += 20f;
        position4.y += position4.height;
        foreach (NetworkConnection observer in this.m_Identity.observers)
        {
          GUI.Label(position4, observer.address + ":" + (object) observer.connectionId, this.m_Styles.componentName);
          position4.y += position4.height;
          y2 = position4.y;
        }
      }
      if (this.m_Identity.clientAuthorityOwner == null)
        return;
      GUI.Label(new Rect(x, y2 + 10f, 400f, 20f), new GUIContent("Client Authority: " + (object) this.m_Identity.clientAuthorityOwner), this.m_Styles.labelStyle);
    }

    private Vector2 GetMaxNameLabelSize()
    {
      Vector2 zero = Vector2.zero;
      using (List<NetworkInformationPreview.NetworkIdentityInfo>.Enumerator enumerator = this.m_Info.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Vector2 vector2 = this.m_Styles.labelStyle.CalcSize(enumerator.Current.value);
          if ((double) zero.x < (double) vector2.x)
            zero.x = vector2.x;
          if ((double) zero.y < (double) vector2.y)
            zero.y = vector2.y;
        }
      }
      return zero;
    }

    private Vector2 GetMaxBehaviourLabelSize()
    {
      Vector2 zero = Vector2.zero;
      using (List<NetworkInformationPreview.NetworkBehaviourInfo>.Enumerator enumerator = this.m_Behaviours.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Vector2 vector2 = this.m_Styles.labelStyle.CalcSize(enumerator.Current.name);
          if ((double) zero.x < (double) vector2.x)
            zero.x = vector2.x;
          if ((double) zero.y < (double) vector2.y)
            zero.y = vector2.y;
        }
      }
      return zero;
    }

    private void GetNetworkInformation(GameObject gameObject)
    {
      this.m_Identity = gameObject.GetComponent<NetworkIdentity>();
      if (!((Object) this.m_Identity != (Object) null))
        return;
      this.m_Info = new List<NetworkInformationPreview.NetworkIdentityInfo>();
      this.m_Info.Add(this.GetAssetId());
      this.m_Info.Add(NetworkInformationPreview.GetString("Scene ID", this.m_Identity.sceneId.ToString()));
      if (!Application.isPlaying)
        return;
      this.m_Info.Add(NetworkInformationPreview.GetString("Network ID", this.m_Identity.netId.ToString()));
      this.m_Info.Add(NetworkInformationPreview.GetString("Player Controller ID", this.m_Identity.playerControllerId.ToString()));
      this.m_Info.Add(NetworkInformationPreview.GetBoolean("Is Client", this.m_Identity.isClient));
      this.m_Info.Add(NetworkInformationPreview.GetBoolean("Is Server", this.m_Identity.isServer));
      this.m_Info.Add(NetworkInformationPreview.GetBoolean("Has Authority", this.m_Identity.hasAuthority));
      this.m_Info.Add(NetworkInformationPreview.GetBoolean("Is Local Player", this.m_Identity.isLocalPlayer));
      NetworkBehaviour[] components = gameObject.GetComponents<NetworkBehaviour>();
      if (components.Length <= 0)
        return;
      this.m_Behaviours = new List<NetworkInformationPreview.NetworkBehaviourInfo>();
      foreach (NetworkBehaviour networkBehaviour in components)
        this.m_Behaviours.Add(new NetworkInformationPreview.NetworkBehaviourInfo()
        {
          name = new GUIContent(networkBehaviour.GetType().FullName),
          behaviour = networkBehaviour
        });
    }

    private NetworkInformationPreview.NetworkIdentityInfo GetAssetId()
    {
      string str = this.m_Identity.assetId.ToString();
      if (string.IsNullOrEmpty(str))
        str = "<object has no prefab>";
      return NetworkInformationPreview.GetString("Asset ID", str);
    }

    private static NetworkInformationPreview.NetworkIdentityInfo GetString(string name, string value)
    {
      return new NetworkInformationPreview.NetworkIdentityInfo()
      {
        name = new GUIContent(name),
        value = new GUIContent(value)
      };
    }

    private static NetworkInformationPreview.NetworkIdentityInfo GetBoolean(string name, bool value)
    {
      return new NetworkInformationPreview.NetworkIdentityInfo()
      {
        name = new GUIContent(name),
        value = new GUIContent(!value ? "No" : "Yes")
      };
    }

    private class NetworkIdentityInfo
    {
      public GUIContent name;
      public GUIContent value;
    }

    private class NetworkBehaviourInfo
    {
      public NetworkBehaviour behaviour;
      public GUIContent name;
    }

    private class Styles
    {
      public GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
      public GUIStyle componentName = new GUIStyle(EditorStyles.boldLabel);
      public GUIStyle disabledName = new GUIStyle(EditorStyles.miniLabel);

      public Styles()
      {
        Color color = new Color(0.7f, 0.7f, 0.7f);
        this.labelStyle.padding.right += 20;
        this.labelStyle.normal.textColor = color;
        this.labelStyle.active.textColor = color;
        this.labelStyle.focused.textColor = color;
        this.labelStyle.hover.textColor = color;
        this.labelStyle.onNormal.textColor = color;
        this.labelStyle.onActive.textColor = color;
        this.labelStyle.onFocused.textColor = color;
        this.labelStyle.onHover.textColor = color;
        this.componentName.normal.textColor = color;
        this.componentName.active.textColor = color;
        this.componentName.focused.textColor = color;
        this.componentName.hover.textColor = color;
        this.componentName.onNormal.textColor = color;
        this.componentName.onActive.textColor = color;
        this.componentName.onFocused.textColor = color;
        this.componentName.onHover.textColor = color;
        this.disabledName.normal.textColor = color;
        this.disabledName.active.textColor = color;
        this.disabledName.focused.textColor = color;
        this.disabledName.hover.textColor = color;
        this.disabledName.onNormal.textColor = color;
        this.disabledName.onActive.textColor = color;
        this.disabledName.onFocused.textColor = color;
        this.disabledName.onHover.textColor = color;
      }
    }
  }
}

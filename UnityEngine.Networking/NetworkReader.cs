// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkReader
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;
using System.Text;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>General purpose serializer for UNET (for reading byte arrays).</para>
  /// </summary>
  public class NetworkReader
  {
    private const int k_MaxStringLength = 32768;
    private const int k_InitialStringBufferSize = 1024;
    private NetBuffer m_buf;
    private static byte[] s_StringReaderBuffer;
    private static Encoding s_Encoding;

    /// <summary>
    ///   <para>The current position within the buffer.</para>
    /// </summary>
    public uint Position
    {
      get
      {
        return this.m_buf.Position;
      }
    }

    /// <summary>
    ///   <para>Creates a new NetworkReader object.</para>
    /// </summary>
    /// <param name="buffer">A buffer to construct the reader with, this buffer is NOT copied.</param>
    public NetworkReader()
    {
      this.m_buf = new NetBuffer();
      NetworkReader.Initialize();
    }

    public NetworkReader(NetworkWriter writer)
    {
      this.m_buf = new NetBuffer(writer.AsArray());
      NetworkReader.Initialize();
    }

    /// <summary>
    ///   <para>Creates a new NetworkReader object.</para>
    /// </summary>
    /// <param name="buffer">A buffer to construct the reader with, this buffer is NOT copied.</param>
    public NetworkReader(byte[] buffer)
    {
      this.m_buf = new NetBuffer(buffer);
      NetworkReader.Initialize();
    }

    private static void Initialize()
    {
      if (NetworkReader.s_Encoding != null)
        return;
      NetworkReader.s_StringReaderBuffer = new byte[1024];
      NetworkReader.s_Encoding = (Encoding) new UTF8Encoding();
    }

    /// <summary>
    ///   <para>Sets the current position of the reader's stream to the start of the stream.</para>
    /// </summary>
    public void SeekZero()
    {
      this.m_buf.SeekZero();
    }

    internal void Replace(byte[] buffer)
    {
      this.m_buf.Replace(buffer);
    }

    /// <summary>
    ///   <para>Reads a 32-bit variable-length-encoded value.</para>
    /// </summary>
    /// <returns>
    ///   <para>The 32 bit value read.</para>
    /// </returns>
    public uint ReadPackedUInt32()
    {
      byte num1 = this.ReadByte();
      if ((int) num1 < 241)
        return (uint) num1;
      byte num2 = this.ReadByte();
      if ((int) num1 >= 241 && (int) num1 <= 248)
        return (uint) (240 + 256 * ((int) num1 - 241)) + (uint) num2;
      byte num3 = this.ReadByte();
      if ((int) num1 == 249)
        return (uint) (2288 + 256 * (int) num2) + (uint) num3;
      byte num4 = this.ReadByte();
      if ((int) num1 == 250)
        return (uint) ((int) num2 + ((int) num3 << 8) + ((int) num4 << 16));
      byte num5 = this.ReadByte();
      if ((int) num1 >= 251)
        return (uint) ((int) num2 + ((int) num3 << 8) + ((int) num4 << 16) + ((int) num5 << 24));
      throw new IndexOutOfRangeException("ReadPackedUInt32() failure: " + (object) num1);
    }

    /// <summary>
    ///   <para>Reads a 64-bit variable-length-encoded value.</para>
    /// </summary>
    /// <returns>
    ///   <para>The 64 bit value read.</para>
    /// </returns>
    public ulong ReadPackedUInt64()
    {
      byte num1 = this.ReadByte();
      if ((int) num1 < 241)
        return (ulong) num1;
      byte num2 = this.ReadByte();
      if ((int) num1 >= 241 && (int) num1 <= 248)
        return (ulong) (240L + 256L * ((long) num1 - 241L)) + (ulong) num2;
      byte num3 = this.ReadByte();
      if ((int) num1 == 249)
        return (ulong) (2288L + 256L * (long) num2) + (ulong) num3;
      byte num4 = this.ReadByte();
      if ((int) num1 == 250)
        return (ulong) ((long) num2 + ((long) num3 << 8) + ((long) num4 << 16));
      byte num5 = this.ReadByte();
      if ((int) num1 == 251)
        return (ulong) ((long) num2 + ((long) num3 << 8) + ((long) num4 << 16) + ((long) num5 << 24));
      byte num6 = this.ReadByte();
      if ((int) num1 == 252)
        return (ulong) ((long) num2 + ((long) num3 << 8) + ((long) num4 << 16) + ((long) num5 << 24) + ((long) num6 << 32));
      byte num7 = this.ReadByte();
      if ((int) num1 == 253)
        return (ulong) ((long) num2 + ((long) num3 << 8) + ((long) num4 << 16) + ((long) num5 << 24) + ((long) num6 << 32) + ((long) num7 << 40));
      byte num8 = this.ReadByte();
      if ((int) num1 == 254)
        return (ulong) ((long) num2 + ((long) num3 << 8) + ((long) num4 << 16) + ((long) num5 << 24) + ((long) num6 << 32) + ((long) num7 << 40) + ((long) num8 << 48));
      byte num9 = this.ReadByte();
      if ((int) num1 == (int) byte.MaxValue)
        return (ulong) ((long) num2 + ((long) num3 << 8) + ((long) num4 << 16) + ((long) num5 << 24) + ((long) num6 << 32) + ((long) num7 << 40) + ((long) num8 << 48) + ((long) num9 << 56));
      throw new IndexOutOfRangeException("ReadPackedUInt64() failure: " + (object) num1);
    }

    /// <summary>
    ///   <para>Reads a NetworkInstanceId from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>The NetworkInstanceId read.</para>
    /// </returns>
    public NetworkInstanceId ReadNetworkId()
    {
      return new NetworkInstanceId(this.ReadPackedUInt32());
    }

    /// <summary>
    ///   <para>Reads a NetworkSceneId from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>The NetworkSceneId read.</para>
    /// </returns>
    public NetworkSceneId ReadSceneId()
    {
      return new NetworkSceneId(this.ReadPackedUInt32());
    }

    /// <summary>
    ///   <para>Reads a byte from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>The value read.</para>
    /// </returns>
    public byte ReadByte()
    {
      return this.m_buf.ReadByte();
    }

    /// <summary>
    ///   <para>Reads a signed byte from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>Value read.</para>
    /// </returns>
    public sbyte ReadSByte()
    {
      return (sbyte) this.m_buf.ReadByte();
    }

    /// <summary>
    ///   <para>Reads a signed 16 bit integer from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>Value read.</para>
    /// </returns>
    public short ReadInt16()
    {
      return (short) (ushort) ((uint) (ushort) (0U | (uint) this.m_buf.ReadByte()) | (uint) (ushort) ((uint) this.m_buf.ReadByte() << 8));
    }

    /// <summary>
    ///   <para>Reads an unsigned 16 bit integer from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>Value read.</para>
    /// </returns>
    public ushort ReadUInt16()
    {
      return (ushort) ((uint) (ushort) (0U | (uint) this.m_buf.ReadByte()) | (uint) (ushort) ((uint) this.m_buf.ReadByte() << 8));
    }

    /// <summary>
    ///   <para>Reads a signed 32bit integer from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>Value read.</para>
    /// </returns>
    public int ReadInt32()
    {
      return (int) (0U | (uint) this.m_buf.ReadByte() | (uint) this.m_buf.ReadByte() << 8 | (uint) this.m_buf.ReadByte() << 16 | (uint) this.m_buf.ReadByte() << 24);
    }

    /// <summary>
    ///   <para>Reads an unsigned 32 bit integer from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>Value read.</para>
    /// </returns>
    public uint ReadUInt32()
    {
      return 0U | (uint) this.m_buf.ReadByte() | (uint) this.m_buf.ReadByte() << 8 | (uint) this.m_buf.ReadByte() << 16 | (uint) this.m_buf.ReadByte() << 24;
    }

    /// <summary>
    ///   <para>Reads a signed 64 bit integer from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>Value read.</para>
    /// </returns>
    public long ReadInt64()
    {
      return (long) (0UL | (ulong) this.m_buf.ReadByte() | (ulong) this.m_buf.ReadByte() << 8 | (ulong) this.m_buf.ReadByte() << 16 | (ulong) this.m_buf.ReadByte() << 24 | (ulong) this.m_buf.ReadByte() << 32 | (ulong) this.m_buf.ReadByte() << 40 | (ulong) this.m_buf.ReadByte() << 48 | (ulong) this.m_buf.ReadByte() << 56);
    }

    /// <summary>
    ///   <para>Reads an unsigned 64 bit integer from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>Value read.</para>
    /// </returns>
    public ulong ReadUInt64()
    {
      return 0UL | (ulong) this.m_buf.ReadByte() | (ulong) this.m_buf.ReadByte() << 8 | (ulong) this.m_buf.ReadByte() << 16 | (ulong) this.m_buf.ReadByte() << 24 | (ulong) this.m_buf.ReadByte() << 32 | (ulong) this.m_buf.ReadByte() << 40 | (ulong) this.m_buf.ReadByte() << 48 | (ulong) this.m_buf.ReadByte() << 56;
    }

    /// <summary>
    ///   <para>Reads a float from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>Value read.</para>
    /// </returns>
    public float ReadSingle()
    {
      return FloatConversion.ToSingle(this.ReadUInt32());
    }

    /// <summary>
    ///   <para>Reads a double from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>Value read.</para>
    /// </returns>
    public double ReadDouble()
    {
      return FloatConversion.ToDouble(this.ReadUInt64());
    }

    /// <summary>
    ///   <para>Reads a string from the stream. (max of 32k bytes).</para>
    /// </summary>
    /// <returns>
    ///   <para>Value read.</para>
    /// </returns>
    public string ReadString()
    {
      ushort num = this.ReadUInt16();
      if ((int) num == 0)
        return string.Empty;
      if ((int) num >= 32768)
        throw new IndexOutOfRangeException("ReadString() too long: " + (object) num);
      while ((int) num > NetworkReader.s_StringReaderBuffer.Length)
        NetworkReader.s_StringReaderBuffer = new byte[NetworkReader.s_StringReaderBuffer.Length * 2];
      this.m_buf.ReadBytes(NetworkReader.s_StringReaderBuffer, (uint) num);
      return new string(NetworkReader.s_Encoding.GetChars(NetworkReader.s_StringReaderBuffer, 0, (int) num));
    }

    /// <summary>
    ///   <para>Reads a char from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>Value read.</para>
    /// </returns>
    public char ReadChar()
    {
      return (char) this.m_buf.ReadByte();
    }

    /// <summary>
    ///   <para>Reads a boolean from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>The value read.</para>
    /// </returns>
    public bool ReadBoolean()
    {
      return (int) this.m_buf.ReadByte() == 1;
    }

    /// <summary>
    ///   <para>Reads a number of bytes from the stream.</para>
    /// </summary>
    /// <param name="count">Number of bytes to read.</param>
    /// <returns>
    ///   <para>Bytes read. (this is a copy).</para>
    /// </returns>
    public byte[] ReadBytes(int count)
    {
      if (count < 0)
        throw new IndexOutOfRangeException("NetworkReader ReadBytes " + (object) count);
      byte[] buffer = new byte[count];
      this.m_buf.ReadBytes(buffer, (uint) count);
      return buffer;
    }

    /// <summary>
    ///   <para>This read a 16-bit byte count and a array of bytes of that size from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>The bytes read from the stream.</para>
    /// </returns>
    public byte[] ReadBytesAndSize()
    {
      ushort num = this.ReadUInt16();
      if ((int) num == 0)
        return (byte[]) null;
      return this.ReadBytes((int) num);
    }

    /// <summary>
    ///   <para>Reads a Unity Vector2 object.</para>
    /// </summary>
    /// <returns>
    ///   <para>The vector read from the stream.</para>
    /// </returns>
    public Vector2 ReadVector2()
    {
      return new Vector2(this.ReadSingle(), this.ReadSingle());
    }

    /// <summary>
    ///   <para>Reads a Unity Vector3 objects.</para>
    /// </summary>
    /// <returns>
    ///   <para>The vector read from the stream.</para>
    /// </returns>
    public Vector3 ReadVector3()
    {
      return new Vector3(this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
    }

    /// <summary>
    ///   <para>Reads a Unity Vector4 object.</para>
    /// </summary>
    /// <returns>
    ///   <para>The vector read from the stream.</para>
    /// </returns>
    public Vector4 ReadVector4()
    {
      return new Vector4(this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
    }

    /// <summary>
    ///   <para>Reads a unity Color objects.</para>
    /// </summary>
    /// <returns>
    ///   <para>The color read from the stream.</para>
    /// </returns>
    public Color ReadColor()
    {
      return new Color(this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
    }

    /// <summary>
    ///   <para>Reads a unity color32 objects.</para>
    /// </summary>
    /// <returns>
    ///   <para>The colo read from the stream.</para>
    /// </returns>
    public Color32 ReadColor32()
    {
      return new Color32(this.ReadByte(), this.ReadByte(), this.ReadByte(), this.ReadByte());
    }

    /// <summary>
    ///   <para>Reads a Unity Quaternion object.</para>
    /// </summary>
    /// <returns>
    ///   <para>The quaternion read from the stream.</para>
    /// </returns>
    public Quaternion ReadQuaternion()
    {
      return new Quaternion(this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
    }

    /// <summary>
    ///   <para>Reads a Unity Rect object.</para>
    /// </summary>
    /// <returns>
    ///   <para>The rect read from the stream.</para>
    /// </returns>
    public Rect ReadRect()
    {
      return new Rect(this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
    }

    /// <summary>
    ///   <para>Reads a unity Plane object.</para>
    /// </summary>
    /// <returns>
    ///   <para>The plane read from the stream.</para>
    /// </returns>
    public Plane ReadPlane()
    {
      return new Plane(this.ReadVector3(), this.ReadSingle());
    }

    /// <summary>
    ///   <para>Reads a Unity Ray object.</para>
    /// </summary>
    /// <returns>
    ///   <para>The ray read from the stream.</para>
    /// </returns>
    public Ray ReadRay()
    {
      return new Ray(this.ReadVector3(), this.ReadVector3());
    }

    /// <summary>
    ///   <para>Reads a unity Matrix4x4 object.</para>
    /// </summary>
    /// <returns>
    ///   <para>The matrix read from the stream.</para>
    /// </returns>
    public Matrix4x4 ReadMatrix4x4()
    {
      return new Matrix4x4() { m00 = this.ReadSingle(), m01 = this.ReadSingle(), m02 = this.ReadSingle(), m03 = this.ReadSingle(), m10 = this.ReadSingle(), m11 = this.ReadSingle(), m12 = this.ReadSingle(), m13 = this.ReadSingle(), m20 = this.ReadSingle(), m21 = this.ReadSingle(), m22 = this.ReadSingle(), m23 = this.ReadSingle(), m30 = this.ReadSingle(), m31 = this.ReadSingle(), m32 = this.ReadSingle(), m33 = this.ReadSingle() };
    }

    /// <summary>
    ///   <para>Reads a NetworkHash128 assetId.</para>
    /// </summary>
    /// <returns>
    ///   <para>The assetId object read from the stream.</para>
    /// </returns>
    public NetworkHash128 ReadNetworkHash128()
    {
      NetworkHash128 networkHash128;
      networkHash128.i0 = this.ReadByte();
      networkHash128.i1 = this.ReadByte();
      networkHash128.i2 = this.ReadByte();
      networkHash128.i3 = this.ReadByte();
      networkHash128.i4 = this.ReadByte();
      networkHash128.i5 = this.ReadByte();
      networkHash128.i6 = this.ReadByte();
      networkHash128.i7 = this.ReadByte();
      networkHash128.i8 = this.ReadByte();
      networkHash128.i9 = this.ReadByte();
      networkHash128.i10 = this.ReadByte();
      networkHash128.i11 = this.ReadByte();
      networkHash128.i12 = this.ReadByte();
      networkHash128.i13 = this.ReadByte();
      networkHash128.i14 = this.ReadByte();
      networkHash128.i15 = this.ReadByte();
      return networkHash128;
    }

    /// <summary>
    ///   <para>Reads a reference to a Transform from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>The transform object read.</para>
    /// </returns>
    public Transform ReadTransform()
    {
      NetworkInstanceId netId = this.ReadNetworkId();
      if (netId.IsEmpty())
        return (Transform) null;
      GameObject localObject = ClientScene.FindLocalObject(netId);
      if (!((UnityEngine.Object) localObject == (UnityEngine.Object) null))
        return localObject.transform;
      if (LogFilter.logDebug)
        Debug.Log((object) ("ReadTransform netId:" + (object) netId));
      return (Transform) null;
    }

    /// <summary>
    ///   <para>Reads a reference to a GameObject from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>The GameObject referenced.</para>
    /// </returns>
    public GameObject ReadGameObject()
    {
      NetworkInstanceId netId = this.ReadNetworkId();
      if (netId.IsEmpty())
        return (GameObject) null;
      GameObject gameObject = !NetworkServer.active ? ClientScene.FindLocalObject(netId) : NetworkServer.FindLocalObject(netId);
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null && LogFilter.logDebug)
        Debug.Log((object) ("ReadGameObject netId:" + (object) netId + "go: null"));
      return gameObject;
    }

    /// <summary>
    ///   <para>Reads a reference to a NetworkIdentity from the stream.</para>
    /// </summary>
    /// <returns>
    ///   <para>The NetworkIdentity object read.</para>
    /// </returns>
    public NetworkIdentity ReadNetworkIdentity()
    {
      NetworkInstanceId netId = this.ReadNetworkId();
      if (netId.IsEmpty())
        return (NetworkIdentity) null;
      GameObject gameObject = !NetworkServer.active ? ClientScene.FindLocalObject(netId) : NetworkServer.FindLocalObject(netId);
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
        return gameObject.GetComponent<NetworkIdentity>();
      if (LogFilter.logDebug)
        Debug.Log((object) ("ReadNetworkIdentity netId:" + (object) netId + "go: null"));
      return (NetworkIdentity) null;
    }

    /// <summary>
    ///   <para>Returns a string representation of the reader's buffer.</para>
    /// </summary>
    /// <returns>
    ///   <para>Buffer contents.</para>
    /// </returns>
    public override string ToString()
    {
      return this.m_buf.ToString();
    }

    public TMsg ReadMessage<TMsg>() where TMsg : MessageBase, new()
    {
      TMsg instance = Activator.CreateInstance<TMsg>();
      instance.Deserialize(this);
      return instance;
    }
  }
}

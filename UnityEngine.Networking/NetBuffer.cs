// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetBuffer
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  internal class NetBuffer
  {
    private const int k_InitialSize = 64;
    private const float k_GrowthFactor = 1.5f;
    private const int k_BufferSizeWarning = 134217728;
    private byte[] m_Buffer;
    private uint m_Pos;

    public uint Position
    {
      get
      {
        return this.m_Pos;
      }
    }

    public NetBuffer()
    {
      this.m_Buffer = new byte[64];
    }

    public NetBuffer(byte[] buffer)
    {
      this.m_Buffer = buffer;
    }

    public byte ReadByte()
    {
      if ((long) this.m_Pos >= (long) this.m_Buffer.Length)
        throw new IndexOutOfRangeException("NetworkReader:ReadByte out of range:" + this.ToString());
      return this.m_Buffer[(IntPtr) this.m_Pos++];
    }

    public void ReadBytes(byte[] buffer, uint count)
    {
      if ((long) (this.m_Pos + count) > (long) this.m_Buffer.Length)
        throw new IndexOutOfRangeException("NetworkReader:ReadBytes out of range: (" + (object) count + ") " + this.ToString());
      for (ushort index = 0; (uint) index < count; ++index)
        buffer[(int) index] = this.m_Buffer[(IntPtr) (this.m_Pos + (uint) index)];
      this.m_Pos += count;
    }

    public void ReadChars(char[] buffer, uint count)
    {
      if ((long) (this.m_Pos + count) > (long) this.m_Buffer.Length)
        throw new IndexOutOfRangeException("NetworkReader:ReadChars out of range: (" + (object) count + ") " + this.ToString());
      for (ushort index = 0; (uint) index < count; ++index)
        buffer[(int) index] = (char) this.m_Buffer[(IntPtr) (this.m_Pos + (uint) index)];
      this.m_Pos += count;
    }

    internal ArraySegment<byte> AsArraySegment()
    {
      return new ArraySegment<byte>(this.m_Buffer, 0, (int) this.m_Pos);
    }

    public void WriteByte(byte value)
    {
      this.WriteCheckForSpace((ushort) 1);
      this.m_Buffer[(IntPtr) this.m_Pos] = value;
      ++this.m_Pos;
    }

    public void WriteByte2(byte value0, byte value1)
    {
      this.WriteCheckForSpace((ushort) 2);
      this.m_Buffer[(IntPtr) this.m_Pos] = value0;
      this.m_Buffer[(IntPtr) (this.m_Pos + 1U)] = value1;
      this.m_Pos += 2U;
    }

    public void WriteByte4(byte value0, byte value1, byte value2, byte value3)
    {
      this.WriteCheckForSpace((ushort) 4);
      this.m_Buffer[(IntPtr) this.m_Pos] = value0;
      this.m_Buffer[(IntPtr) (this.m_Pos + 1U)] = value1;
      this.m_Buffer[(IntPtr) (this.m_Pos + 2U)] = value2;
      this.m_Buffer[(IntPtr) (this.m_Pos + 3U)] = value3;
      this.m_Pos += 4U;
    }

    public void WriteByte8(byte value0, byte value1, byte value2, byte value3, byte value4, byte value5, byte value6, byte value7)
    {
      this.WriteCheckForSpace((ushort) 8);
      this.m_Buffer[(IntPtr) this.m_Pos] = value0;
      this.m_Buffer[(IntPtr) (this.m_Pos + 1U)] = value1;
      this.m_Buffer[(IntPtr) (this.m_Pos + 2U)] = value2;
      this.m_Buffer[(IntPtr) (this.m_Pos + 3U)] = value3;
      this.m_Buffer[(IntPtr) (this.m_Pos + 4U)] = value4;
      this.m_Buffer[(IntPtr) (this.m_Pos + 5U)] = value5;
      this.m_Buffer[(IntPtr) (this.m_Pos + 6U)] = value6;
      this.m_Buffer[(IntPtr) (this.m_Pos + 7U)] = value7;
      this.m_Pos += 8U;
    }

    public void WriteBytesAtOffset(byte[] buffer, ushort targetOffset, ushort count)
    {
      uint num = (uint) count + (uint) targetOffset;
      this.WriteCheckForSpace((ushort) num);
      if ((int) targetOffset == 0 && (int) count == buffer.Length)
      {
        buffer.CopyTo((Array) this.m_Buffer, (long) this.m_Pos);
      }
      else
      {
        for (int index = 0; index < (int) count; ++index)
          this.m_Buffer[(int) targetOffset + index] = buffer[index];
      }
      if (num <= this.m_Pos)
        return;
      this.m_Pos = num;
    }

    public void WriteBytes(byte[] buffer, ushort count)
    {
      this.WriteCheckForSpace(count);
      if ((int) count == buffer.Length)
      {
        buffer.CopyTo((Array) this.m_Buffer, (long) this.m_Pos);
      }
      else
      {
        for (int index = 0; index < (int) count; ++index)
          this.m_Buffer[(long) this.m_Pos + (long) index] = buffer[index];
      }
      this.m_Pos += (uint) count;
    }

    private void WriteCheckForSpace(ushort count)
    {
      if ((long) (this.m_Pos + (uint) count) < (long) this.m_Buffer.Length)
        return;
      int length = (int) ((double) this.m_Buffer.Length * 1.5);
      while ((long) (this.m_Pos + (uint) count) >= (long) length)
      {
        length = (int) ((double) length * 1.5);
        if (length > 134217728)
          Debug.LogWarning((object) ("NetworkBuffer size is " + (object) length + " bytes!"));
      }
      byte[] numArray = new byte[length];
      this.m_Buffer.CopyTo((Array) numArray, 0);
      this.m_Buffer = numArray;
    }

    public void FinishMessage()
    {
      ushort num = (ushort) (this.m_Pos - 4U);
      this.m_Buffer[0] = (byte) ((uint) num & (uint) byte.MaxValue);
      this.m_Buffer[1] = (byte) ((int) num >> 8 & (int) byte.MaxValue);
    }

    public void SeekZero()
    {
      this.m_Pos = 0U;
    }

    public void Replace(byte[] buffer)
    {
      this.m_Buffer = buffer;
      this.m_Pos = 0U;
    }

    public override string ToString()
    {
      return string.Format("NetBuf sz:{0} pos:{1}", (object) this.m_Buffer.Length, (object) this.m_Pos);
    }
  }
}

using MineX.Common.Structs;
using System;
using System.IO;
using System.Text;

namespace MineX.Common.Utils
{
    public class BinaryStreamWriter
    {
        private Stream Stream { get; set; }

        public BinaryStreamWriter()
        {

        }

        public BinaryStreamWriter(Stream stream)
        {
            Stream = stream;
        }

        [Obsolete]
        public void Write(byte value)
        {
            Stream.WriteByte(value);
        }

        [Obsolete]
        public void Write(ushort value, ByteOrder order)
        {
            byte[] bytes;

            if (order == ByteOrder.LittleEndian)
            {
                bytes = new byte[] {
                    (byte)((value >> 8) & 0xFF),
                    (byte)(value & 0xFF)
                };
            }
            else
            {
                bytes = new byte[] {
                    (byte)(value & 0xFF),
                    (byte)((value >> 8) & 0xFF)
                };
            }

            Stream.Write(bytes, 0, bytes.Length);
        }

        [Obsolete]
        public void Write(int value, ByteOrder order)
        {
            byte[] bytes;

            if (order == ByteOrder.LittleEndian)
            {
                bytes = new byte[] {
                    (byte)((value >> 24) & 0xFF),
                    (byte)((value >> 16) & 0xFF),
                    (byte)((value >> 8) & 0xFF),
                    (byte)(value & 0xFF)
                };
            }
            else
            {
                bytes = new byte[] {
                    (byte)(value & 0xFF),
                    (byte)((value >> 8) & 0xFF),
                    (byte)((value >> 16) & 0xFF),
                    (byte)((value >> 24) & 0xFF)
                };
            }

            Stream.Write(bytes, 0, bytes.Length);
        }

        [Obsolete]
        public void Write(string value, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(value);
            Stream.Write(bytes, 0, bytes.Length);
            Stream.WriteByte((byte)'\n');
        }

        [Obsolete]
        /// Source: https://wiki.vg/Data_types#VarInt_and_VarLong
        public void Write(VarInt value)
        {
            do
            {
                byte temp = (byte)(value.Value & 0b01111111);
                value.Value >>= 7;
                if (value.Value != 0)
                {
                    temp |= 0b10000000;
                }
                Stream.WriteByte(temp);
            } while (value.Value != 0);
        }

        public void WriteByte(byte value)
        {
            Stream.WriteByte(value);
        }

        public void WriteByteArray(byte[] array)
        {
            Stream.Write(array);
        }

        public void WriteByteArray(byte[] array, int offset, int length)
        {
            Stream.Write(array, offset, length);
        }

        public void WriteShort(short value, ByteOrder order)
        {
            byte[] bytes;

            if (order == ByteOrder.LittleEndian)
            {
                bytes = new byte[] {
                    (byte)((value >> 8) & 0xFF),
                    (byte)(value & 0xFF)
                };
            }
            else
            {
                bytes = new byte[] {
                    (byte)(value & 0xFF),
                    (byte)((value >> 8) & 0xFF)
                };
            }

            Stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteInt(int value, ByteOrder order)
        {
            byte[] bytes;

            if (order == ByteOrder.LittleEndian)
            {
                bytes = new byte[] {
                    (byte)((value >> 24) & 0xFF),
                    (byte)((value >> 16) & 0xFF),
                    (byte)((value >> 8) & 0xFF),
                    (byte)(value & 0xFF)
                };
            }
            else
            {
                bytes = new byte[] {
                    (byte)(value & 0xFF),
                    (byte)((value >> 8) & 0xFF),
                    (byte)((value >> 16) & 0xFF),
                    (byte)((value >> 24) & 0xFF)
                };
            }

            Stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteString(string value, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(value);
            Stream.Write(bytes, 0, bytes.Length);
            Stream.WriteByte((byte)'\n');
        }

        public void WriteStringRaw(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            WriteVarInt(bytes.Length);
            WriteByteArray(bytes);
        }

        /// Source: https://wiki.vg/Data_types#VarInt_and_VarLong
        public void WriteVarInt(VarInt value)
        {
            do
            {
                byte temp = (byte)(value.Value & 0b01111111);
                value.Value = (int)((uint)value.Value >> 7);
                if (value.Value != 0)
                {
                    temp |= 0b10000000;
                }
                Stream.WriteByte(temp);
            } while (value.Value != 0);
        }
    }
}

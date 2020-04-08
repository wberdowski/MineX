using MineX.Common.Structs;
using System;
using System.IO;
using System.Text;

namespace MineX.Common.Utils
{
    public class BinaryStreamReader
    {
        private Stream Stream { get; set; }

        public BinaryStreamReader()
        {

        }

        public BinaryStreamReader(Stream stream)
        {
            Stream = stream;
        }

        public byte ReadByte()
        {
            return (byte)Stream.ReadByte();
        }

        public byte[] ReadByteArray(int offset, int length)
        {
            byte[] bytes = new byte[length];
            Stream.Read(bytes, offset, length);
            return bytes;
        }

        public ushort ReadUShort(ByteOrder order)
        {
            var bytes = new byte[sizeof(ushort)];
            Stream.Read(bytes, 0, bytes.Length);

            if (order == ByteOrder.LittleEndian)
            {
                return (ushort)(bytes[1] << 8 | bytes[0]);
            }
            else
            {
                return (ushort)(bytes[0] << 8 | bytes[1]);
            }
        }

        public int ReadInt32(ByteOrder order)
        {
            var bytes = new byte[sizeof(int)];
            Stream.Read(bytes, 0, bytes.Length);

            if (order == ByteOrder.LittleEndian)
            {
                return bytes[3] << 24 | bytes[2] << 16 | bytes[1] << 8 | bytes[0];
            }
            else
            {
                return bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3];
            }
        }

        public string ReadString()
        {
            using (var buffer = new MemoryStream())
            {
                byte value;
                while (true)
                {
                    value = (byte)Stream.ReadByte();
                    if (value == '\0') break;
                    buffer.WriteByte(value);
                }

                return Encoding.UTF8.GetString(buffer.ToArray());
            }
        }

        /// Source: https://wiki.vg/Data_types#VarInt_and_VarLong
        public VarInt ReadVarInt()
        {
            int numRead = 0;
            VarInt result = new VarInt(0);
            byte read;
            do
            {
                read = (byte)Stream.ReadByte();
                int value = (read & 0b01111111);
                result.Value |= (value << (7 * numRead));
                numRead++;
                if (numRead > 5)
                {
                    throw new Exception("VarInt is too big");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }
    }
}

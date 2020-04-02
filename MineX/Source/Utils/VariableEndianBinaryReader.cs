using System.IO;
using System.Text;

namespace MineX.Utils
{
    public class VariableEndianBinaryReader
    {
        private Stream Stream { get; set; }

        public VariableEndianBinaryReader()
        {

        }

        public VariableEndianBinaryReader(Stream stream)
        {
            Stream = stream;
        }

        public byte ReadByte()
        {
            return (byte)Stream.ReadByte();
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
    }
}

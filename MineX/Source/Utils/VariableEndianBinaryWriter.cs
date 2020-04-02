using System.IO;
using System.Text;

namespace MineX.Utils
{
    public class VariableEndianBinaryWriter
    {
        private Stream Stream { get; set; }

        public VariableEndianBinaryWriter()
        {

        }

        public VariableEndianBinaryWriter(Stream stream)
        {
            Stream = stream;
        }

        public void Write(byte value)
        {
            Stream.WriteByte(value);
        }

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

        public void Write(string value, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(value);
            Stream.Write(bytes, 0, bytes.Length);
            Stream.WriteByte((byte)'\n');
        }
    }
}

using System;

namespace MineX.Query
{
    public abstract class QueryPacket
    {
        public const ushort Magic = 0xFEFD;
        public QueryPacketType Type { get; set; }
        public Session Session { get; set; }

        public virtual byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}

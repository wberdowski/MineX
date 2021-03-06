﻿using MineX.Common.Structs;
using MineX.Common.Utils;
using System.IO;

namespace MineX.Query
{
    public class HandshakeRequest : QueryPacket
    {
        public HandshakeRequest(Session session)
        {
            Type = QueryPacketType.Handshake;
            Session = session;
        }

        public override byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryStreamWriter(stream);

                writer.Write(Magic, ByteOrder.LittleEndian);
                writer.Write((byte)Type);
                writer.Write(Session.Id, ByteOrder.LittleEndian);

                return stream.ToArray();
            }
        }
    }
}


using System.Buffers.Binary;
using System.Text;

namespace SquadNET.Core.Squad.Entities
{
    public readonly struct PacketInfo
    {
        public static readonly PacketInfo Empty = new(0, 0, []);

        public const int SizeFieldLength = 4;

        public const int IdFieldLength = 4;

        public const int TypeFieldLength = 4;

        public const int EmptyStringLength = 1;

        public const byte EmptyStringTerminator = 0;

        public bool IsBroken { get; }

        public int Size { get; }

        public int Id { get; }

        public int Type { get; }

        public byte[] Body { get; }

        public PacketInfo(int id, int type, string body, bool isBroken = false, Encoding encoding = null)
            : this(id, type, (encoding ?? Encoding.UTF8).GetBytes(body), isBroken)
        {
        }

        public PacketInfo(int id, int type, byte[] body, bool isBroken = false)
        {
            Size = 8 + body.Length + 1 + 1;
            Type = type;
            Id = id;
            Body = body;
            IsBroken = isBroken;
        }

        public static PacketInfo Read(Stream stream)
        {
            byte[] array = new byte[4];
            if (stream.Read(array, 0, 4) != 4)
            {
                throw new Exception("invalid amount of bytes for size received");
            }

            int num = BinaryPrimitives.ReadInt32LittleEndian(array);
            array = new byte[num];
            if (stream.Read(array, 0, num) != num)
            {
                throw new Exception("invalid amount of bytes for rest of packet received");
            }

            int id = BinaryPrimitives.ReadInt32LittleEndian(array[0..4]);
            int type = BinaryPrimitives.ReadInt32LittleEndian(array[4..8]);
            byte[] subArray = array[8..(array.Length - 2)];
            return new PacketInfo(id, type, subArray);
        }

        public static int ParseSize(byte[] bytes)
        {
            if (bytes.Length != 4)
            {
                throw new Exception("invalid packet size bytes received");
            }

            return BinaryPrimitives.ReadInt32LittleEndian(bytes);
        }

        public static PacketInfo Parse(byte[] bytes)
        {
            int id = BinaryPrimitives.ReadInt32LittleEndian(bytes[0..4]);
            int type = BinaryPrimitives.ReadInt32LittleEndian(bytes[4..8]);
            byte[] subArray = bytes[8..(bytes.Length - 2)];
            return new PacketInfo(id, type, subArray, subArray.SequenceEqual(new byte[7] { 0, 0, 0, 1, 0, 0, 0 }));
        }

        public byte[] ToArray()
        {
            using MemoryStream memoryStream = new MemoryStream();
            byte[] array = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(array, Size);
            memoryStream.Write(array);
            BinaryPrimitives.WriteInt32LittleEndian(array, Id);
            memoryStream.Write(array);
            BinaryPrimitives.WriteInt32LittleEndian(array, Type);
            memoryStream.Write(array);
            memoryStream.Write(Body);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(0);
            return memoryStream.ToArray();
        }

        public override string ToString()
        {
            return string.Format("Size: {0}, Id: {1}, Type: {2}, Body Size: {3}; Hex: {4}", Size, Id, Type, Body.Length, string.Join("", from x in ToArray()
                                                                                                                                         select x.ToString("X2")));
        }
    }
}

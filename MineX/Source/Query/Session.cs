namespace MineX.Query
{
    public struct Session
    {
        public int Id { get; }

        public Session(int id)
        {
            Id = id & 0x0F0F0F0F;
        }
    }
}

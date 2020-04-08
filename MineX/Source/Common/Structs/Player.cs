using System;

namespace MineX.Common.Structs
{
    public class Player
    {
        public string Name { get; private set; }
        public Guid Id { get; private set; }

        public Player()
        {

        }

        public Player(string name, Guid id)
        {
            Name = name;
            Id = id;
        }
    }
}

using System;
using Roton.Core;

namespace Roton.Emulation.Models
{
    internal class Actor : IActor
    {
        public Actor()
        {
            if (Location == null)
                Location = new Location();
            if (Vector == null)
                Vector = new Vector();
            if (UnderTile == null)
                UnderTile = new Tile();
        }

        public virtual int Cycle { get; set; }
        public virtual int Follower { get; set; }
        public virtual int Instruction { get; set; }
        public virtual int Leader { get; set; }
        public virtual int Length { get; set; }
        public virtual IXyPair Location { get; }
        public virtual int P1 { get; set; }
        public virtual int P2 { get; set; }
        public virtual int P3 { get; set; }
        public virtual int Pointer { get; set; }
        public virtual ITile UnderTile { get; }
        public virtual IXyPair Vector { get; }
        public virtual byte[] Code { get; set; }

        public virtual bool IsAttached => false;
        public override string ToString()
        {
            var name = "";
            if (Code != null)
            {
                // walk the code to get the name
                var data = Code;
                if (data[0] == 0x40)
                {
                    var length = data.Length;
                    for (var i = 1; i < length; i++)
                    {
                        if (data[i] == 0x0D)
                        {
                            var nameData = new byte[i - 1];
                            Array.Copy(data, 1, nameData, 0, nameData.Length);
                            name = nameData.ToStringValue();
                            break;
                        }
                    }
                }
                name = string.IsNullOrWhiteSpace(name) ? string.Empty : $" {name}";
            }
            name = Location + name;
            return name;
        }
    }
}
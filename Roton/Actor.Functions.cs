using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public partial class Actor
    {
        private string _code;

        public Actor()
        {
            if (Location == null)
            {
                Location = new Location();
            }
            if (Vector == null)
            {
                Vector = new Vector();
            }
            if (UnderTile == null)
            {
                UnderTile = new Tile();
            }
        }

        public string Code
        {
            get
            {
                if (Heap != null)
                {
                    if (Heap.Contains(Pointer))
                    {
                        return Heap[Pointer].ToStringValue();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return _code;
                }
            }
            set
            {
                if (Heap != null)
                {
                    if (Pointer < 0 || !Heap.Contains(Pointer))
                    {
                        Pointer = Heap.Allocate(value.ToBytes());
                    }
                    else
                    {
                        Heap[Pointer] = value.ToBytes();
                    }
                }
                else
                {
                    _code = value;
                }
            }
        }

        public void CopyFrom(Actor actor)
        {
            this.Cycle = actor.Cycle;
            this.Follower = actor.Follower;
            this.Instruction = actor.Instruction;
            this.Leader = actor.Leader;
            this.Length = actor.Length;
            this.Location.CopyFrom(actor.Location);
            this.P1 = actor.P1;
            this.P2 = actor.P2;
            this.P3 = actor.P3;
            this.Pointer = actor.Pointer;
            this.UnderTile.CopyFrom(actor.UnderTile);
            this.Vector.CopyFrom(actor.Vector);

            if (this.Heap != actor.Heap)
            {
                this.Code = actor.Code;
            }
        }

        public void DuplicateFrom(Actor actor)
        {
            CopyFrom(actor);
            var code = this.Code;
            this.Pointer = -1;
            this.Code = code;
        }

        virtual public bool IsAttached {
            get 
            { 
                return false;
            } 
        }

        public override string ToString()
        {
            string name = "";
            if (Heap != null && Pointer >= 0)
            {
                // walk the code to get the name
                byte[] data = Heap[Pointer];
                if (data[0] == 0x40)
                {
                    int length = data.Length;
                    for (int i = 1; i < length; i++)
                    {
                        if (data[i] == 0x0D)
                        {
                            byte[] nameData = new byte[i - 1];
                            Array.Copy(data, 1, nameData, 0, nameData.Length);
                            name = nameData.ToStringValue();
                            break;
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = "";
                }
                else
                {
                    name = " " + name;
                }
            }
            name = Location.ToString() + name;
            return name;
        }

        public int X
        {
            get { return Location.X; }
            set { Location.X = value; }
        }

        public int Y
        {
            get { return Location.Y; }
            set { Location.Y = value; }
        }
    }
}

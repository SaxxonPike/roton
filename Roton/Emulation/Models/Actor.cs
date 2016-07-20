using System;
using Roton.Core;

namespace Roton.Emulation.Models
{
    internal class Actor : IActor
    {
        /// <summary>
        /// Create an actor.
        /// </summary>
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

        /// <summary>
        /// The frequency at which the actor will run its action code.
        /// </summary>
        public virtual int Cycle { get; set; }

        /// <summary>
        /// The index of the actor this actor has following it, when applicable (ex. centipede segments and heads)
        /// </summary>
        public virtual int Follower { get; set; }

        /// <summary>
        /// The offset where this actor's code will be executed from next.
        /// </summary>
        public virtual int Instruction { get; set; }

        /// <summary>
        /// The index of the actor this actor is following, when applicable (ex. centipede segments)
        /// </summary>
        public virtual int Leader { get; set; }

        /// <summary>
        /// Length of the code this actor references.
        /// </summary>
        public virtual int Length { get; set; }

        /// <summary>
        /// Location in board coordinates of this actor. The upper left visible corner is (1, 1).
        /// </summary>
        public virtual IXyPair Location { get; }

        /// <summary>
        /// Parameter data. The use of this field will vary depending on the element.
        /// </summary>
        public virtual int P1 { get; set; }

        /// <summary>
        /// Parameter data. The use of this field will vary depending on the element.
        /// </summary>
        public virtual int P2 { get; set; }

        /// <summary>
        /// Parameter data. The use of this field will vary depending on the element.
        /// </summary>
        public virtual int P3 { get; set; }

        /// <summary>
        /// Pointer index within the Heap the code references.
        /// </summary>
        public virtual int Pointer { get; set; }

        /// <summary>
        /// Tile underneath the actor.
        /// </summary>
        public virtual ITile UnderTile { get; }

        /// <summary>
        /// Actor's vector, when applicable.
        /// </summary>
        public virtual IXyPair Vector { get; }

        /// <summary>
        /// Get or set this actor's code.
        /// </summary>
        public virtual byte[] Code { get; set; }

        /// <summary>
        /// If true, the actor is attached to a Context.
        /// </summary>
        public virtual bool IsAttached => false;

        /// <summary>
        /// Retrieve the name of the actor from the code.
        /// </summary>
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
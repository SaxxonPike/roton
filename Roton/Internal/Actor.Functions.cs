using System;
using Roton.Core;

namespace Roton.Internal
{
    internal partial class Actor
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
        /// Get or set this actor's code.
        /// </summary>
        public virtual char[] Code { get; set; }

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
using Roton.Emulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Roton
{
    public partial class Actor : ICode, ICodeSeekable
    {
        /// <summary>
        /// The frequency at which the actor will run its action code.
        /// </summary>
        virtual public int Cycle { get; set; }

        /// <summary>
        /// The index of the actor this actor has following it, when applicable (ex. centipede segments and heads)
        /// </summary>
        virtual public int Follower { get; set; }

        /// <summary>
        /// The Heap in which the code is contained.
        /// </summary>
        virtual internal Heap Heap { get; set; }

        /// <summary>
        /// The offset where this actor's code will be executed from next.
        /// </summary>
        virtual public int Instruction { get; set; }

        /// <summary>
        /// The index of the actor this actor is following, when applicable (ex. centipede segments)
        /// </summary>
        virtual public int Leader { get; set; }

        /// <summary>
        /// Length of the code this actor references.
        /// </summary>
        virtual public int Length { get; set; }

        /// <summary>
        /// Location in board coordinates of this actor. The upper left visible corner is (1, 1).
        /// </summary>
        virtual public Location Location { get; set; }

        /// <summary>
        /// Parameter data. The use of this field will vary depending on the element.
        /// </summary>
        virtual public int P1 { get; set; }

        /// <summary>
        /// Parameter data. The use of this field will vary depending on the element.
        /// </summary>
        virtual public int P2 { get; set; }

        /// <summary>
        /// Parameter data. The use of this field will vary depending on the element.
        /// </summary>
        virtual public int P3 { get; set; }

        /// <summary>
        /// Pointer index within the Heap the code references.
        /// </summary>
        virtual public int Pointer { get; set; }

        /// <summary>
        /// Tile underneath the actor.
        /// </summary>
        virtual public Tile UnderTile { get; set; }

        /// <summary>
        /// Actor's vector, when applicable.
        /// </summary>
        virtual public Vector Vector { get; set; }
    }
}

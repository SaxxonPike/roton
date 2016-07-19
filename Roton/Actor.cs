using Roton.Emulation;

namespace Roton
{
    public partial class Actor : ICode, ICodeSeekable
    {
        /// <summary>
        /// The frequency at which the actor will run its action code.
        /// </summary>
        public virtual int Cycle { get; set; }

        /// <summary>
        /// The index of the actor this actor has following it, when applicable (ex. centipede segments and heads)
        /// </summary>
        public virtual int Follower { get; set; }

        /// <summary>
        /// The Heap in which the code is contained.
        /// </summary>
        internal virtual Heap Heap { get; set; }

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
        public virtual Location Location { get; set; }

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
        public virtual Tile UnderTile { get; protected set; }

        /// <summary>
        /// Actor's vector, when applicable.
        /// </summary>
        public virtual Vector Vector { get; protected set; }
    }
}
using System.Collections.Generic;

namespace Roton.Core
{
    public interface IContext
    {
        int ActorCapacity { get; }
        IActorList Actors { get; }
        IBoard BoardData { get; }
        IList<IPackedBoard> Boards { get; }
        IElementList Elements { get; }
        byte[] Memory { get; }
        IWorld WorldData { get; }
        int WorldSize { get; }
        int BoardIndex { get; set; }

        void ExecuteOnce();
        void PackBoard();
        void Refresh();
        byte[] Save();
        void Save(string filename);
        void SetBoard(int boardIndex);
        void Start();
        void Stop();
        ITile TileAt(int x, int y);
        void UnpackBoard();
    }
}
using System.Collections.Generic;

namespace Roton.Core
{
    public interface IContext
    {
        IActorList Actors { get; }
        IBoard Board { get; }
        IList<IPackedBoard> Boards { get; }
        IDrumBank Drums { get; }
        IElementList Elements { get; }
        ITileGrid Tiles { get; }
        IWorld WorldData { get; }
        int WorldSize { get; }

        byte[] DumpMemory();
        void ExecuteOnce();
        void PackBoard();
        void Refresh();
        byte[] Save();
        void Save(string filename);
        void SetBoard(int boardIndex);
        void Start();
        void Stop();
        void UnpackBoard();
    }
}
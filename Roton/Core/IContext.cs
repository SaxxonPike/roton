using System.Collections.Generic;

namespace Roton.Core
{
    public interface IContext
    {
        int WorldSize { get; }

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
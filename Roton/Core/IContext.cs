﻿using System.Collections.Generic;

namespace Roton.Core
{
    public interface IContext
    {
        int ActorCapacity { get; }
        IActorList Actors { get; }
        int Board { get; set; }
        IBoard BoardData { get; }
        IList<IPackedBoard> Boards { get; }
        ContextEngine ContextEngine { get; }
        IElementList Elements { get; }
        int Height { get; }
        IKeyboard Keyboard { get; set; }
        byte[] Memory { get; }
        int ScreenHeight { get; }
        bool ScreenWide { get; }
        int ScreenWidth { get; }
        ISerializer Serializer { get; }
        ISpeaker Speaker { get; set; }
        ITerminal Terminal { get; set; }
        int Width { get; }
        IWorld WorldData { get; }
        int WorldSize { get; }

        IActor CreateActor();
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
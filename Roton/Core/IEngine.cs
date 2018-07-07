﻿using System;
using System.Collections.Generic;
using Roton.Emulation.Execution;
using Roton.Emulation.Mapping;
using Roton.Events;
using Roton.FileIo;

namespace Roton.Core
{
    public interface IEngine
    {
        event DataEventHandler RequestReplaceContext;
        event EventHandler Terminated;

        bool TitleScreen { get; }
        
        void ClearWorld();
        void ExecuteCode(int index, IExecutable instructionSource, string name);
        void FadePurple();
        bool GetPlayerTimeElapsed(int interval);
        void PackBoard();
        int ReadKey();
        void SetBoard(int boardIndex);
        void SetEditorMode();
        void SetGameMode();
        void ShowInGameHelp();
        void Start();
        void Stop();
        void UnpackBoard(int boardIndex);
        void WaitForTick();
    }
}
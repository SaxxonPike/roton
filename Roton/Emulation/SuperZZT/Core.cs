﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal partial class Core
    {
        internal void MoveActorOnRiver(int index)
        {
            var actor = Actors[index];
            var vector = new Vector();
            int underId = actor.UnderTile.Id;

            if (underId == Elements.RiverNId)
            {
                vector.SetTo(0, -1);
            }
            else if (underId == Elements.RiverSId)
            {
                vector.SetTo(0, 1);
            }
            else if (underId == Elements.RiverWId)
            {
                vector.SetTo(-1, 0);
            }
            else if (underId == Elements.RiverEId)
            {
                vector.SetTo(1, 0);
            }

            if (ElementAt(actor.Location).Index == Elements.PlayerId)
            {
                ElementAt(actor.Location.Sum(vector)).Interact(actor.Location.Sum(vector), 0, vector);
            }

            if (vector.IsNonZero)
            {
                var target = actor.Location.Sum(vector);
                if (ElementAt(target).Floor)
                {
                    MoveActor(index, target);
                }
            }
        }

        internal override void StartMain()
        {
            GameSpeed = 4;
            DefaultSaveName = "SAVED";
            DefaultBoardName = "TEMP";
            DefaultWorldName = "MONSTER";
            Display.GenerateFadeMatrix();
            if (!WorldLoaded)
            {
                ClearWorld();
            }
            SetGameMode();
            TitleScreenLoop();
        }
    }
}

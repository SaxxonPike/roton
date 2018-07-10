using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Interactions;

namespace Roton.Emulation.Zzt
{
    public class ZztInteractionList : InteractionList
    {
        public ZztInteractionList(ICollection<IInteraction> actions) : base(new Dictionary<int, IInteraction>
            {
                {0x01, actions.OfType<BoardEdgeInteraction>().Single()},
                {0x05, actions.OfType<AmmoInteraction>().Single()},
                {0x06, actions.OfType<TorchInteraction>().Single()},
                {0x07, actions.OfType<GemInteraction>().Single()},
                {0x08, actions.OfType<KeyInteraction>().Single()},
                {0x09, actions.OfType<DoorInteraction>().Single()},
                {0x0A, actions.OfType<ScrollInteraction>().Single()},
                {0x0B, actions.OfType<PassageInteraction>().Single()},
                {0x0D, actions.OfType<BombInteraction>().Single()},
                {0x0E, actions.OfType<EnergizerInteraction>().Single()},
                {0x0F, actions.OfType<EnemyInteraction>().Single()},
                {0x12, actions.OfType<EnemyInteraction>().Single()},
                {0x13, actions.OfType<WaterInteraction>().Single()},
                {0x14, actions.OfType<ForestInteraction>().Single()},
                {0x18, actions.OfType<PuzzleInteraction>().Single()},
                {0x19, actions.OfType<PuzzleInteraction>().Single()},
                {0x1A, actions.OfType<PuzzleInteraction>().Single()},
                {0x1B, actions.OfType<FakeWallInteraction>().Single()},
                {0x1C, actions.OfType<InvisibleWallInteraction>().Single()},
                {0x1E, actions.OfType<TransporterInteraction>().Single()},
                {0x22, actions.OfType<EnemyInteraction>().Single()},
                {0x23, actions.OfType<EnemyInteraction>().Single()},
                {0x24, actions.OfType<ObjectInteraction>().Single()},
                {0x25, actions.OfType<SlimeInteraction>().Single()},
                {0x29, actions.OfType<EnemyInteraction>().Single()},
                {0x2A, actions.OfType<EnemyInteraction>().Single()},
                {0x2C, actions.OfType<EnemyInteraction>().Single()},
                {0x2D, actions.OfType<EnemyInteraction>().Single()},
            },
            actions.OfType<DefaultInteraction>().Single())
        {
        }
    }
}
using FluentAssertions;
using NUnit.Framework;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration
{
    public class PassageTestFixture : AllContextIntegrationTestFixture
    {
        public PassageTestFixture(Context context) : base(context)
        {
        }
        
        [Test]
        public void Passage_ShouldSendPlayerToCorrectBoard()
        {
            // Set up board 1.
            World.BoardIndex = 1;
            Engine.ClearBoard();
            var passage1 = Actors[SpawnTo(1, 1, ElementList.PassageId, 1)];
            passage1.P3 = 0;
            MovePlayerTo(2, 1);
            Engine.PackBoard();
            
            // Set up board 0.
            World.BoardIndex = 0;
            Engine.ClearBoard();
            var passage0 = Actors[SpawnTo(1, 1, ElementList.PassageId, 1)];
            passage0.P3 = 1;
            MovePlayerTo(2, 1);
            
            // Walk the player into the passage and out of it on the destination board.
            Type(AnsiKey.Left);
            Type(AnsiKey.Right);
            
            // Play out steps.
            StepAllKeys();
            
            // Assert.
            World.BoardIndex.Should().Be(1);
        }
    }
}
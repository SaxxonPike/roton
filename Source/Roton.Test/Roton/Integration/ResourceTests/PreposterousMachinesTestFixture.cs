using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.ResourceTests
{
    public class PreposterousMachinesTestFixture : OriginalContextTestFixture
    {
        [Test]
        [Explicit]
        public void Test_MandelbrotRenderer()
        {
            // Since this will run a long time, we want to disable OOP tracing
            // otherwise we'll just have gigs of logs.
            DisableTracer();

            // Load in the board file.
            UnpackBoardResource("PreposterousMachines.MandelbrotRenderer.brd");
            //DumpActorCode();
            Step(4);
            TouchActor(80);
            Step(50000);

            for (var y = 1; y <= 25; y++)
                for (var x = 1; x <= 60; x++)
                    TestContext.Out.WriteLine($"[{x:D2},{y:D2}] {TileAt(x, y)}");
        }
    }
}
using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.ResourceTests
{
    public class PreposterousMachinesTestFixture : OriginalContextBaseIntegrationTestFixture
    {
        [Test]
        [Explicit]
        public void Test_SineRenderer()
        {
            DisableTracer();
            UnpackBoardResource("PreposterousMachines.SineRenderer.brd");
            DumpActorCode();
            Step(4);
            TouchActor(18);
            Step(135);
            DumpActorCode();
            Step(1);
        }
    }
}
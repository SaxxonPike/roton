using NUnit.Framework;
using Roton.Emulation.Data.Impl;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Emulation
{
    public class GenericTestFixture : AllContextBaseTestFixture
    {
        public GenericTestFixture(ContextEngine contextEngine) : base(contextEngine)
        {
        }

        [Test]
        public void Test1()
        {
            Step();
        }
    }
}
using NUnit.Framework;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Infrastructure
{
    [TestFixture(ContextEngine.Original)]
    [TestFixture(ContextEngine.Super)]
    public abstract class AllContextBaseTestFixture : ContextBaseTestFixture
    {
        protected AllContextBaseTestFixture(ContextEngine contextEngine) : base(contextEngine)
        {
        }
    }
}
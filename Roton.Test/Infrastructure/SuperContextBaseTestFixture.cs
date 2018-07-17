using NUnit.Framework;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Infrastructure
{
    [TestFixture]
    public abstract class SuperContextBaseTestFixture : ContextBaseTestFixture
    {
        protected SuperContextBaseTestFixture(ContextEngine contextEngine) : base(ContextEngine.Super)
        {
        }
    }
}
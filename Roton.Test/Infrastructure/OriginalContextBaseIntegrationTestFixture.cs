using NUnit.Framework;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Infrastructure
{
    [TestFixture]
    public abstract class OriginalContextBaseIntegrationTestFixture : ContextBaseIntegrationTestFixture
    {
        protected OriginalContextBaseIntegrationTestFixture() : base(Context.Original)
        {
        }
    }
}
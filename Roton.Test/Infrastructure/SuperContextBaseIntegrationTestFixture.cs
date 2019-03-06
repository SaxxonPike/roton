using NUnit.Framework;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Infrastructure
{
    [TestFixture]
    public abstract class SuperContextBaseIntegrationTestFixture : ContextBaseIntegrationTestFixture
    {
        protected SuperContextBaseIntegrationTestFixture(Context context) : base(Context.Super)
        {
        }
    }
}
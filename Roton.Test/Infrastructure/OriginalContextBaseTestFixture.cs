using NUnit.Framework;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Infrastructure
{
    [TestFixture]
    public abstract class OriginalContextBaseTestFixture : ContextBaseTestFixture
    {
        protected OriginalContextBaseTestFixture() : base(Context.Original)
        {
        }
    }
}
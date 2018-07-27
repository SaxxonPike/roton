using NUnit.Framework;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Infrastructure
{
    [TestFixture]
    public abstract class SuperContextBaseTestFixture : ContextBaseTestFixture
    {
        protected SuperContextBaseTestFixture(Context context) : base(Context.Super)
        {
        }
    }
}
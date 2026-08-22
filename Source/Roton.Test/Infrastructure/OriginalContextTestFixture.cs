using NUnit.Framework;

namespace Roton.Test.Infrastructure;

[TestFixture]
public abstract class OriginalContextTestFixture() : ContextTestFixture(Context.Original);
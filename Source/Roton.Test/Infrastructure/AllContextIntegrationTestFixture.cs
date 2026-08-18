using NUnit.Framework;

namespace Roton.Test.Infrastructure;

[TestFixture(Context.Original)]
[TestFixture(Context.Super)]
public abstract class AllContextIntegrationTestFixture(Context context) : ContextBaseIntegrationTestFixture(context);
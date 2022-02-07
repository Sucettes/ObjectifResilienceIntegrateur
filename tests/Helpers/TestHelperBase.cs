using Bogus;
using Spk.Common.Helpers.Guard;
using Xunit.Abstractions;

namespace Gwenael.Tests.Helpers
{
    public abstract class TestHelperBase
    {
        protected readonly ITestOutputHelper TestOutputHelper;
        protected static readonly Faker Faker = new();

        protected TestHelperBase(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper.GuardIsNotNull(nameof(testOutputHelper));
        }
    }
}
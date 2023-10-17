using AutoFixture.Xunit2;

namespace LongRunningJobImitator.Services.Tests.AutoFixtureConfiguration
{
    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] objects) : base(new AutoMoqDataAttribute(), objects)
        {
        }
    }
}

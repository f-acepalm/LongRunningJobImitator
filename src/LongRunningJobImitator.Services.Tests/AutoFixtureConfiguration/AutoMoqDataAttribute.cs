using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace LongRunningJobImitator.Services.Tests.AutoFixtureConfiguration
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(CreateFixture)
        {
        }

        private static IFixture CreateFixture()
        {
            Fixture fixture = new();
            fixture.Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true,
            });

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));

            fixture.Behaviors.Add(new OmitOnRecursionBehavior(2));

            return fixture;
        }
    }

}

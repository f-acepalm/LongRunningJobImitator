using LongRunningJobImitator.Services.Interfaces;

namespace LongRunningJobImitator.Services
{
    public class LongProcessImitator : ILongProcessImitator
    {
        private Random _random = new Random();

        public async Task DoSomething()
        {
            var delay = _random.Next(1000, 1500);
            await Task.Delay(delay);
        }
    }
}

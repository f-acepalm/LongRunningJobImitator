using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongRunningJobImitator.Services.Interfaces
{
    public interface ILongProcessImitator
    {
        Task DoSomething();
    }
}

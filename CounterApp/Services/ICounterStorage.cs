using CounterApp.Models;

namespace CounterApp.Services;

public interface ICounterStorage
{
    Task<List<Counter>> LoadAsync();
    Task SaveAsync(IEnumerable<Counter> counters);
}
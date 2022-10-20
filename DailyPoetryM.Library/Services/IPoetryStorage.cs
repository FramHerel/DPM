using System.Linq.Expressions;
using DailyPoetryM.Models;

namespace DailyPoetryM.Services;

public interface IPoetryStorage
{
    bool IsInitialized { get; }

    Task InitializedAsync();
    Task<Poetry> GetPoetryAsync(int id);
    Task<IEnumerable<Poetry>> GetPoetriesAsync
        (
        Expression<Func<Poetry, bool>> where,
        int skip,
        int take
        );
}
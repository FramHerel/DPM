using System.Linq.Expressions;
using DailyPoetryM.Models;

namespace DailyPoetryM.Services;

public class PoetryStorage : IPoetryStorage
{
    public bool IsInitialized => Preferences.Get(PoetryStorageConstant.VersionKey, 0) == PoetryStorageConstant.Version;

    public async Task InitializedAsync()
    {
        throw new NotImplementedException();
    }
    public async Task<Poetry> GetPoetryAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Poetry>> GetPoetriesAsync(Expression<Func<Poetry, bool>> where, int skip, int take)
    {
        throw new NotImplementedException();
    }
}

public static class PoetryStorageConstant
{
    public const int Version = 1;
    public const string VersionKey = nameof(PoetryStorageConstant) + "." + nameof(Version);
}

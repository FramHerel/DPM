using System.Linq.Expressions;
using DailyPoetryM.Models;
using SQLite;

namespace DailyPoetryM.Services;

public class PoetryStorage : IPoetryStorage
{
    public const string DbName = "poetrydb.sqlite3";
    public static readonly string PoetryDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);

    private SQLiteAsyncConnection? _connection;
    private SQLiteAsyncConnection Connection => _connection ??= new SQLiteAsyncConnection(PoetryDbPath);

    private readonly IPreferenceStorage _preferenceStorage;

    public PoetryStorage(IPreferenceStorage preferenceStorage)
    {
        _preferenceStorage = preferenceStorage;
    }

    public bool IsInitialized => _preferenceStorage.Get(PoetryStorageConstant.VersionKey, 0) == PoetryStorageConstant.Version;

    public async Task InitializedAsync()
    {
        await using var dbFileStream = new FileStream(PoetryDbPath, FileMode.OpenOrCreate);
        await using var dbAssetStream = typeof(PoetryStorage).Assembly.GetManifestResourceStream(DbName) ??
            throw new Exception($"Manifest not found: {DbName}");
        await dbAssetStream.CopyToAsync(dbFileStream);

        _preferenceStorage.Set(PoetryStorageConstant.VersionKey, PoetryStorageConstant.Version);
    }
    public async Task<Poetry> GetPoetryAsync(int id) => await Connection.Table<Poetry>().FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Poetry>> GetPoetriesAsync(Expression<Func<Poetry, bool>> where, int skip, int take) => 
        await Connection.Table<Poetry>().Where(where).Skip(skip).Take(take).ToListAsync();

    public async Task CloesAsync() => await Connection.CloseAsync();
}

public static class PoetryStorageConstant
{
    public const int Version = 1;
    public const string VersionKey = nameof(PoetryStorageConstant) + "." + nameof(Version);
}

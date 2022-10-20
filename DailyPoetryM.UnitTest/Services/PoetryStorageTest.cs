using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DailyPoetryM.Models;
using DailyPoetryM.Services;
using Moq;

namespace DailyPoetryM.UnitTest.Services;

public class PoetryStorageTest : IDisposable
{
    public PoetryStorageTest()
    {
        File.Delete(PoetryStorage.PoetryDbPath);
    }

    public void Dispose()
    {
        File.Delete(PoetryStorage.PoetryDbPath);
    }

    [Fact]
    public void IsInitialized_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        preferenceStorageMock
            .Setup(p => p.Get(PoetryStorageConstant.VersionKey, 0))
            .Returns(PoetryStorageConstant.Version);
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        Assert.True(poetryStorage.IsInitialized);
    }

    [Fact]
    public async Task TestInitializeAsync_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        Assert.False(File.Exists(PoetryStorage.PoetryDbPath));
        await poetryStorage.InitializedAsync();
        Assert.True(File.Exists(PoetryStorage.PoetryDbPath));

        preferenceStorageMock.Verify(p => p.Set(PoetryStorageConstant.VersionKey, PoetryStorageConstant.Version), Times.Once);
    }

    [Fact]
    public async Task GetPoetryAsync_Default()
    {
        var poetryStorage = await GetInitializedPoetryStorage();

        var poetry = await poetryStorage.GetPoetryAsync(10001);
        Assert.Equal("临江仙 · 夜归临皋", poetry.Name);

        await poetryStorage.CloesAsync();
    }

    [Fact]
    public async Task GetPoetriesAsync_Default()
    {
        var poetryStorage = await GetInitializedPoetryStorage();

        var poetries = await poetryStorage.GetPoetriesAsync(Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true), Expression.Parameter(typeof(Poetry), "p")), 0, int.MaxValue);
        Assert.Equal(30,poetries.Count());
        await poetryStorage.CloesAsync();
    }

    public async Task<PoetryStorage> GetInitializedPoetryStorage()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        await poetryStorage.InitializedAsync();
        return poetryStorage;
    }
}

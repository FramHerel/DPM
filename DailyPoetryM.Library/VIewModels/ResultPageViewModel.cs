using System.Linq.Expressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryM.Models;
using DailyPoetryM.Services;
using TheSalLab.MauiInfiniteScrolling;

namespace DailyPoetryM.ViewModels;

public class ResultPageViewModel : ObservableObject
{
    public Expression<Func<Poetry, bool>> Where
    {
        get => _where;
        set => _canLoadMore = SetProperty(ref _where, value);

    }
    private Expression<Func<Poetry, bool>> _where;

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    private string _status;

    public MauiInfiniteScrollCollection<Poetry> Poetries { get; }

    //TODO 测试用
    private readonly IPoetryStorage _poetryStorage;

    public ResultPageViewModel(IPoetryStorage poetryStorage)
    {
        //TODO 测试用
        Where = Expression.Lambda<Func<Poetry, bool>>(
                Expression.Constant(true),
                Expression.Parameter(typeof(Poetry), "p"));
        _poetryStorage = poetryStorage;

        Poetries = new MauiInfiniteScrollCollection<Poetry>
        {
            OnCanLoadMore = () => _canLoadMore,
            OnLoadMore = async () =>
            {
                Status = Loading;
                var poetries = (await poetryStorage.GetPoetriesAsync(Where, Poetries.Count, PageSize)).ToList();
                Status = string.Empty;

                if (poetries.Count < PageSize)  //NoMoreResult
                {
                    _canLoadMore = false;
                    Status = NoMoreResult;
                }

                if (Poetries.Count == 0 && poetries.Count == 0) //NoResult
                {
                    _canLoadMore = false;
                    Status = NoResult;
                }

                return poetries;
            }
        };

        _lazyNavigatedToCommand = new Lazy<AsyncRelayCommand>(() =>
            new AsyncRelayCommand(NavigatedToCommandFunction));
    }

    private Lazy<AsyncRelayCommand> _lazyNavigatedToCommand;
    public AsyncRelayCommand NavigatedToCommand => _lazyNavigatedToCommand.Value;

    //private AsyncRelayCommand _navigatedToCommand;

    //public AsyncRelayCommand NavigatedToCommand
    //{
    //    set => _navigatedToCommand ??= new AsyncRelayCommand(NavigatedToCommandFunction);
    //}


    //private RelayCommand _navigatedToCommand;
    //public RelayCommand NavigatedToCommand =>
    //    _navigatedToCommand ??= new RelayCommand(async () =>
    //    {
    //        await NavigatedToCommandFunction();
    //    });

    public async Task NavigatedToCommandFunction()
    {
        await _poetryStorage.InitializedAsync();
        Poetries.Clear();
        await Poetries.LoadMoreAsync();
    }

    private bool _canLoadMore;
    public const int PageSize = 20;
    public const string Loading = "正在载入";
    public const string NoResult = "没有满足条件的结果";
    public const string NoMoreResult = "没有更多结果";
}

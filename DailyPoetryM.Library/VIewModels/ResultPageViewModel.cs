using System.Linq.Expressions;
using CommunityToolkit.Mvvm.ComponentModel;
using DailyPoetryM.Models;
using DailyPoetryM.Services;
using TheSalLab.MauiInfiniteScrolling;

namespace DailyPoetryM.VIewModels;

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

    public ResultPageViewModel(IPoetryStorage poetryStorage)
    {
        Poetries = new MauiInfiniteScrollCollection<Poetry>
        {
            OnCanLoadMore = () => _canLoadMore,
            OnLoadMore = async () =>
            {
                Status = Loading;
                var poetries = (await poetryStorage.GetPoetriesAsync(Where, Poetries.Count, PageSize)).ToList();
                return poetries;

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
            }
        };
    }

    private bool _canLoadMore;

    public const int PageSize = 20;

    public const string Loading = "正在载入";
    public const string NoResult = "没有满足条件的结果";
    public const string NoMoreResult = "没有更多结果";
}

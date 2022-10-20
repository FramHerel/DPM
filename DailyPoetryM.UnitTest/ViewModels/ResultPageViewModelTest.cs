using System.Linq.Expressions;
using DailyPoetryM.Models;

namespace DailyPoetryM.UnitTest.ViewModels;

public class ResultPageViewModelTest
{
    [Fact]
    public async Task Poetries_Default()
    {
        var where = Expression.Lambda<Func<Poetry, bool>>(
                Expression.Constant(true),
                Expression.Parameter(typeof(Poetry), "p"));
    }
}

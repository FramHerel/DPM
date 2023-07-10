using DailyPoetryM.Services;
using DailyPoetryM.ViewModels;

namespace DailyPoetryM;

public class ServiceLocator
{
    private IServiceProvider _serviceProvider;

    public ResultPageViewModel ResultPageViewModel =>
        _serviceProvider.GetService<ResultPageViewModel>();

    public ServiceLocator()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
        serviceCollection.AddSingleton<IPreferenceStorage, PreferenceStorage>();
        serviceCollection.AddSingleton<ResultPageViewModel>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}

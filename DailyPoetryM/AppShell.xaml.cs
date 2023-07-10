using DailyPoetryM.Pages;
namespace DailyPoetryM;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Items.Add(new FlyoutItem
        {
            Title = nameof(ResultPage),
            Route = nameof(ResultPage),
            Items = { new ShellContent {ContentTemplate = new DataTemplate(typeof(ResultPage)) } }
        });
    }
}

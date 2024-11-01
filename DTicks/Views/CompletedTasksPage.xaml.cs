using DTicks.ViewModels;

namespace DTicks.Views;

public partial class CompletedTasksPage : ContentPage
{
	public CompletedTasksPage(CompletedTasksViewModel completedTasksViewModel)
	{
		InitializeComponent();

		BindingContext = completedTasksViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as CompletedTasksViewModel)?.OnAppearing();
    }
}
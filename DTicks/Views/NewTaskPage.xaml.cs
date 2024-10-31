using mvvmdemo.ViewModels;

namespace DTicks.Views;

public partial class NewTaskPage : ContentPage
{
	public NewTaskPage(NewTaskViewModel newTaskViewModel)
	{
		InitializeComponent();

		BindingContext = newTaskViewModel;
	}
}
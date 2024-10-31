using DTicks.ViewModels;

namespace DTicks.Views;

public partial class TaskDetailPage : ContentPage
{
	public TaskDetailPage(TaskDetailViewModel taskDetailViewModel)
	{
		InitializeComponent();

		BindingContext = taskDetailViewModel;
	}
}
using DTicks.ViewModels;
using System.Formats.Tar;

namespace DTicks.Views;

public partial class TaskModifyPage : ContentPage
{
	public TaskModifyPage(TaskModifyViewModel taskModifyViewModel)
	{
		InitializeComponent();
		BindingContext = taskModifyViewModel;
	}
}
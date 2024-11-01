using DTicks.Models;
using DTicks.ViewModels;

namespace DTicks.Views;

public partial class TasksPage : ContentPage
{
	public TasksPage(TasksViewModel tasksViewModel)
	{
		InitializeComponent();

		BindingContext = tasksViewModel;
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as TasksViewModel)?.OnAppearing();
	}

	//  private void taskCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
	//  {
	//      var send = (TaskItem)((CheckBox)sender).BindingContext;
	//      ((TasksViewModel)this.TaskListView.BindingContext).ItemChecked.Execute(send);
	//  }
}
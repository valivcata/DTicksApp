using DTicks.Models;
using DTicks.Services;
using DTicks.ViewModels;

namespace mvvmdemo.ViewModels;

public class NewTaskViewModel : BaseViewModel
{
    private readonly TasksViewModel ItemsViewModel;
    private string title = string.Empty;
    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }

    private string description = string.Empty;
    public string Description
    {
        get => description;
        set => SetProperty(ref description, value);
    }

    private DateOnly dueDate = new DateOnly();

    public DateOnly DueDate
    {
        get => dueDate;
        set => SetProperty(ref dueDate, value);
    }

    public Command SaveCommand { get; }
    public Command CancelCommand { get; }

    public NewTaskViewModel(TasksViewModel ItemsViewModel)
    {
        this.ItemsViewModel = ItemsViewModel;
        SaveCommand = new Command(OnSave, ValidateSave);
        CancelCommand = new Command(OnCancel);

        this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
    }

    private bool ValidateSave()
    {
        return !String.IsNullOrWhiteSpace(title)
            && !String.IsNullOrWhiteSpace(description);
    }

    private async void OnSave()
    {
        TaskItem newItem = new()
        {
            Id = 0,
            Title = Title,
            Description = Description,
            //DueDate = DueDate,
            IsDone = false
        };

        _ = await DbService.Instance;

        //await DataStore.AddItemAsync(newItem);
        await DbService.UpsertAsync(newItem);

        ItemsViewModel.ReloadItemsCommand.Execute(null);

        // This will pop the current page off the navigation stack
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancel()
    {
        // This will pop the current page off the navigation stack
        await Shell.Current.GoToAsync("..");
    }
}

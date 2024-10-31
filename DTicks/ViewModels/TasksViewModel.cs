using DTicks.Models;
using DTicks.Services;
using DTicks.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DTicks.ViewModels;

public class TasksViewModel : BaseViewModel
{
    private TaskItem? _selectedItem;

    private bool isChecked;

    public bool IsChecked
    {
        get => isChecked;
        set => SetProperty(ref isChecked, value);
    }

    public ObservableCollection<TaskItem> Items { get; private set; } = [];
    public Command LoadItemsCommand { get; }
    public Command ReloadItemsCommand { get; }
    public Command AddItemCommand { get; }
    public Command ClearCommand { get; }
    public Command<TaskItem> ItemSwiped { get; }
    public Command<TaskItem> ItemTapped { get; }
    public Command<TaskItem> DeleteCommand { get; }
    public Command<TaskItem> ItemChecked { get; }

    public TasksViewModel()
    {
        PageTitle = "Tasks";

        ClearCommand = new Command(async () => await ClearTasks());
        LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        ReloadItemsCommand = new Command(async () => await LoadItemsAsync());
        AddItemCommand = new Command(async () => await OnAddItem());
        DeleteCommand = new Command<TaskItem>(OnDeleteItem);
        ItemSwiped = new Command<TaskItem>(OnItemSwiped);
        ItemTapped = new Command<TaskItem>(OnItemTaped);
        ItemChecked = new Command<TaskItem>(OnItemChecked);

        Task.Run(LoadItemsAsync);
    }

    public async void OnItemChecked(TaskItem item)
    {
        _ = await DbService.Instance;

        item.IsDone = !item.IsDone;
        IsChecked = item.IsDone;

        await DbService.UpsertAsync(item);

        //_ = await DbService.UpdateCheck(item);
        ReloadItemsCommand.Execute(null);

        //MoveTaskToEnd(item);
    }

    private void MoveTaskToEnd(TaskItem task)
    {
        // Remove the task from its current position
        Items.Remove(task);

        // Add the task to the end of the list
        Items.Add(task);
    }

    public async void OnDeleteItem(TaskItem? item)
    {
        _ = await DbService.Instance;

        Items.Remove(item);

        await DbService.DeleteItemAsync(item);

    }


    public async Task ClearTasks()
    {
        _ = await DbService.Instance;

        foreach (var item in Items)
        {
            await DbService.DeleteItemAsync(item);
        } 
        Items.Clear();
    }

    public async Task LoadItemsAsync()
    {
        _ = await DbService.Instance;

        var list = await DbService.GetItemsAsync();

        Items.Clear();

        if (list != null)
        {
            foreach (var item in list)
            {
                if (item != null)
                    Items.Add(item);
            }
        }
    }

    async Task ExecuteLoadItemsCommand()
    {
        IsBusy = true;

        await LoadItemsAsync();

        IsBusy = false;
    }

    private async Task OnAddItem()
    {
        await Shell.Current.GoToAsync(nameof(NewTaskPage));
    }

    public TaskItem? SelectedItem
    {
        get => _selectedItem;
        set
        {
            SetProperty(ref _selectedItem, value);
            OnItemTaped(value);
        }
    }

    async void OnItemSwiped(TaskItem? item)
    {
        if (item == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(TaskModifyPage)}?{nameof(TaskModifyViewModel.ItemId)}={item.Id}");
    }

    async void OnItemTaped(TaskItem? item)
    {
        if (item == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(TaskDetailPage)}?{nameof(TaskDetailViewModel.ItemId)}={item.Id}");
    }
    public void OnAppearing()
    {
        IsBusy = true;
        SelectedItem = null;
    }
}

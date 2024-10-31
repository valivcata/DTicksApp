using DTicks.Models;
using DTicks.Services;
using System.Diagnostics;

namespace DTicks.ViewModels;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public class TaskDetailViewModel : BaseViewModel
{
    private TaskItem item = new();
    private readonly TasksViewModel ItemsViewModel;
    public Command GoBackCommand { get; private set; }

    private string title = string.Empty;
    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }

    private string? description;
    public string? Description
    {
        get => description;
        set => SetProperty(ref description, value);
    }

    private int itemId;
    public int ItemId
    {
        get => itemId;
        set
        {
            itemId = value;
            LoadItemId(value);
        }
    }

    public async void LoadItemId(int itemId)
    {
        _ = await DbService.Instance;
        var item = await DbService.GetItemAsync(itemId);
        Title = item.Title;
        Description = item.Description;
    }

    public TaskDetailViewModel(TasksViewModel ItemsViewModel)
    {
        this.ItemsViewModel = ItemsViewModel;
        GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }
}

using DTicks.Models;
using DTicks.Services;
using System.Diagnostics;

namespace DTicks.ViewModels;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public class TaskModifyViewModel : BaseViewModel
{
    private TaskItem item = new();
    private readonly TasksViewModel ItemsViewModel;
    public Command SaveCommand { get; private set; }
    public Command CancelCommand { get; }

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

    private DateOnly dueDate = new DateOnly();

    public DateOnly DueDate
    {
        get => dueDate;
        set => SetProperty(ref dueDate, value);
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
        //DueDate = item.DueDate;
    }

    public TaskModifyViewModel(TasksViewModel ItemsViewModel)
    {
        this.ItemsViewModel = ItemsViewModel;
        SaveCommand = new Command(async () => await ExecuteSave(), CanExecuteSave);
        CancelCommand = new Command(OnCancel);
        this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
    }

    private bool CanExecuteSave()
    {
        if (item != null)
            return item.Description != Description || item.Title != Title;
        return false;
    }

    private async Task ExecuteSave()
    {
        item.Id = ItemId;
        item.Description = Description ?? string.Empty;
        item.Title = Title;
        //item.DueDate = DueDate;

        _ = await DbService.UpsertAsync(item);
        ItemsViewModel.ReloadItemsCommand.Execute(null);

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}

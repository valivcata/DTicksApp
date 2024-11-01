using DTicks.Models;
using DTicks.Services;
using DTicks.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTicks.ViewModels
{
    public class CompletedTasksViewModel : BaseViewModel
    {
        private TaskItem? _selectedItem;

        public TaskItem? SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemTaped(value);
            }
        }

        public ObservableCollection<TaskItem> CompletedItems { get; private set; } = [];
        public Command LoadCompletedItemsCommand { get; }
        public Command<TaskItem> ItemTapped { get; }
        public Command ClearCommand { get; }

        public CompletedTasksViewModel()
        {
            PageTitle = "Completed Tasks";

            LoadCompletedItemsCommand = new Command(async () => await ExecuteLoadCompletedItemsCommand());
            ItemTapped = new Command<TaskItem>(OnItemTaped);
            ClearCommand = new Command(async () => await ClearTasks());
        }

        public async Task ClearTasks()
        {
            _ = await DbService.Instance;

            foreach (var item in CompletedItems)
            {
                await DbService.DeleteItemAsync(item);
            }
            CompletedItems.Clear();
        }

        public async Task LoadCompletedItemsAsync()
        {
            _ = await DbService.Instance;

            var list = await DbService.GetItemsDoneAsync();

            CompletedItems.Clear();

            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item != null)
                    {
                        CompletedItems.Add(item);
                    }
                }
            }
        }

        async Task ExecuteLoadCompletedItemsCommand()
        {
            IsBusy = true;

            await LoadCompletedItemsAsync();

            IsBusy = false;
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
}

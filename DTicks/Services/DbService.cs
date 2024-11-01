using DTicks.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTicks.Services
{
    public class DbService
    {
        private static SQLiteAsyncConnection Database;
        public const string DatabaseFilename = "dailyticks.db3";

        public const SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        public static readonly AsyncLazy<DbService> Instance = new(async () =>
        {
            var instance = new DbService();

            _ = await Database.CreateTableAsync<TaskItem>();

            return instance;
        });

        public DbService()
        {
            Database = new SQLiteAsyncConnection(DatabasePath, Flags);
        }

        public static Task<List<TaskItem>> GetItemsAsync()
        {
            return Database.Table<TaskItem>().ToListAsync();
        }

        public static Task<TaskItem> GetItemAsync(int id)
        {
            return Database.Table<TaskItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public static Task<int> UpdateCheck(TaskItem item)
        {
            item.IsDone = !item.IsDone;
            return Database.UpdateAsync(item);
            
        }

        public static Task<List<TaskItem>> GetItemsNotDoneAsync()
        {
            return Database.Table<TaskItem>().Where(i => !i.IsDone).ToListAsync();
        }

        public static Task<List<TaskItem>> GetItemsDoneAsync()
        {
            return Database.Table<TaskItem>().Where(i => i.IsDone).ToListAsync();
        }

        public static Task<List<TaskItem>> GetItemsForSearch(string newValue)
        {
            return Database.Table<TaskItem>().Where(i => i.Title.Contains(newValue.ToLower(), StringComparison.CurrentCultureIgnoreCase)).ToListAsync();
        }

        public static Task<int> UpsertAsync(TaskItem item)
        {
            if (item.Id > 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public static Task<int> DeleteItemAsync(TaskItem item)
        {
            return Database.DeleteAsync(item);
        }
    }
}

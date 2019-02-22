using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BikeSharing.Models;

namespace BikeSharing.MobileApp.Helpers
{
    public class TodoItemManager
    {
        IRestService restService;

        public TodoItemManager(IRestService service)
        {
            restService = service;
        }

        public Task<List<UserProfile>> GetTasksAsync()
        {
            return restService.RefreshDataAsync();
        }

        public Task SaveTaskAsync(UserProfile item, bool isNewItem = false)
        {
            return restService.SaveTodoItemAsync(item, isNewItem);
        }

        public Task DeleteTaskAsync(UserProfile item)
        {
            return restService.DeleteTodoItemAsync(item.Id.ToString());
        }
    }
}

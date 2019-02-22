using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BikeSharing.Models;

namespace BikeSharing.MobileApp.Helpers
{
    public interface IRestService
    {
        Task<List<UserProfile>> RefreshDataAsync();

        Task SaveTodoItemAsync(UserProfile item, bool isNewItem);

        Task DeleteTodoItemAsync(string id);
    }
}

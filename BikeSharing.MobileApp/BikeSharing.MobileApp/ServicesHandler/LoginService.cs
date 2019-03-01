using BikeSharing.MobileApp.RestAPIClient;
using BikeSharing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikeSharing.MobileApp.ServicesHandler
{
    public class LoginService
    {
        // fetch the RestClient<T>
        RestClient<UserProfile> _restClient = new RestClient<UserProfile>();

        // Boolean function with the following parameters of username & password.
        public async Task<bool> CheckLoginIfExists(string Username, string Password)
        {
            var check = await _restClient.checkLogin(Username, Password);

            return check;
        }
    }
}

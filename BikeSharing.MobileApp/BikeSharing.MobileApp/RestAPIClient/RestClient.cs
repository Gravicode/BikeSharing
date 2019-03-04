using BikeSharing.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BikeSharing.MobileApp.RestAPIClient
{
    public class RestClient<T>
    {
        private const string MainWebServiceUrl = "http://bikesharingservices.azurewebsites.net/"; // Put your main host url here
        private const string LoginWebServiceUrl = MainWebServiceUrl + "api/UserProfile/"; // put your api extension url/uri here

        // This code matches the HTTP Request that we included in our api controller
        public async Task<bool> checkLogin(string Username, string Password)
        {
            var httpClient = new HttpClient();
            // http://MainHost/api/UserCredentials/username=foo/password=foo. The api value and response value should match in order to return a true status code. 
            
            var response = await httpClient.GetAsync(LoginWebServiceUrl + "username=" + Username + "/" + "password=" + Password);
           
            return response.IsSuccessStatusCode; // return either true or false
        }

        public async Task<bool> InsertData(UserProfile item)
        {
            var httpClient = new HttpClient();
            var uri = new Uri("https://bikesharingservices.azurewebsites.net/api/UserProfile/PostUserProfile");
            var Json = JsonConvert.SerializeObject(item);
            var ItemContent = new StringContent(Json, Encoding.UTF8, "application/json");
            var res = await httpClient.PostAsync(uri, ItemContent);
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}

using BikeSharing.MobileApp.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BikeSharing.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace BikeSharing.MobileApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage
	{
        //bool isNewItem;
        String uname, pass,email,phonenumber;
        public UserProfile item { get; set; } 
		public RegisterPage (bool isNew = false)
		{
            item = new UserProfile();
			InitializeComponent ();
            //isNewItem = isNew;
            BindingContext = item;
        }

        private MediaFile _mediaFile;
        private string URL { get; set; }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            uname = usernameEntry.Text;
            pass = passwordEntry.Text;
            email = emailEntry.Text;
            phonenumber = phonenumberEntry.Text;

            if (uname == null)
            {
                DisplayAlert("Alert", "Please Enter Username", "Ok");
            }
            else if (pass == null)
            {
                DisplayAlert("Alert", "Please Enter Password", "Ok");
            }
            else
            {
                LoginAsync();
            }
            //var todoItem = (UserProfile)BindingContext;
            //await App.TodoManager.SaveTaskAsync(todoItem, isNewItem);
            //MessagingCenter.Send<UserProfile>(this.item, "newitem");
            //await DisplayAlert("Saved", item + "Successfully!", "OK");
            //await Navigation.PushModalAsync(new TabbedMenu());
            //await Navigation.PushModalAsync(new TabbedMenu());
        }

        private async void btnSelectPic_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "This is not support on your device.", "OK");
                return;
            }
            else
            {
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                _mediaFile = await CrossMedia.Current.PickPhotoAsync();
                if (_mediaFile == null) return;
                imageView.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
                UploadedUrl.Text = "Image URL:";
            }
        }

        //Upload picture button    
        private async void btnUpload_Clicked(object sender, EventArgs e)
        {
            if (_mediaFile == null)
            {
                await DisplayAlert("Error", "There was an error when trying to get your image.", "OK");
                return;
            }
            else
            {
                UploadImage(_mediaFile.GetStream());
            }
        }

        //Upload to blob function    
        private async void UploadImage(Stream stream)
        {
            try
            {
                Busy();
                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=padidata;AccountKey=G0CSG/ndz66adthIyIicdqh6lTv0oHc7D5HrgiGMjoBZhYNGsknrz9bMNkT3w2cSssh2BFoPF8s7P3FZNDOZJQ==;EndpointSuffix=core.windows.net");
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference("bikesharingregistrasi");
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                var name = Guid.NewGuid().ToString();
                var blockBlob = container.GetBlockBlobReference($"{name}.jpg");
                blockBlob.Properties.ContentType = "image/jpg";
                //blockBlob.UploadFromByteArrayAsync(byteArrayThumbnail, 0, byteArrayThumbnail.Length);
                await blockBlob.UploadFromStreamAsync(stream);
                URL = blockBlob.Uri.OriginalString;
                UploadedUrl.Text = URL;
                NotBusy();
                await DisplayAlert("Uploaded", "Image uploaded to Blob Storage Successfully!", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("error", null, ex.ToString(), "ok");
            }
        }

        public void Busy()
        {
            uploadIndicator.IsVisible = true;
            uploadIndicator.IsRunning = true;
            btnSelectPic.IsEnabled = false;
            //btnTakePic.IsEnabled = false;
            btnUpload.IsEnabled = false;
        }

        public void NotBusy()
        {
            uploadIndicator.IsVisible = false;
            uploadIndicator.IsRunning = false;
            btnSelectPic.IsEnabled = true;
            //btnTakePic.IsEnabled = true;
            btnUpload.IsEnabled = true;
        }

        public async void LoginAsync()
        {
            try
            {
                var fromContent = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string,string>("username", uname),
                        new KeyValuePair<string,string>("password", pass),
                        new KeyValuePair<string,string>("email", email),
                        new KeyValuePair<string,string>("phonenumber", phonenumber)
                    });

                var myHttpClient = new HttpClient();
                var response = await myHttpClient.PostAsync
                    ("https://bikesharingservices.azurewebsites.net/api/userprofile/PostUserProfile", fromContent);

                var json = await response.Content.ReadAsStringAsync();
                try
                {
                    var data = JsonConvert.DeserializeObject<Data>(json);
                    if (data.ResponseCode.Equals("1"))
                    {
                        await Navigation.PushModalAsync(new UserProfilePage());
                    }
                    else
                    {
                        await DisplayAlert("Alert", data.Message, "Ok");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", ex.Message, "Ok");
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Alert", e.Message, "Ok");
            }
        }

        public class Data
        {
            public string ResponseCode { get; set; }
            public string Message { get; set; }
            public string UserId { get; set; }
        }

    }
}
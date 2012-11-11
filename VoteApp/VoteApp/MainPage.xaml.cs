﻿using System;                               
using System.Collections.Generic;           
using System.IO;                            
using System.Linq;                          
using System.Runtime.Serialization;         
using Microsoft.WindowsAzure.MobileServices;
using Windows.Foundation;                   
using Windows.Foundation.Collections;       
using Windows.UI.Xaml;                      
using Windows.UI.Xaml.Controls;             
using Windows.UI.Xaml.Controls.Primitives;  
using Windows.UI.Xaml.Data;                 
using Windows.UI.Xaml.Input;                
using Windows.UI.Xaml.Media;                
using Windows.UI.Xaml.Navigation;           
using Windows.UI.Popups;                    
//using Microsoft.Live;                             We should need this?
using Windows.UI.Popups;                    


namespace VoteApp
{    
    public class TodoItem
    {
        public int Id { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "complete")]
        public bool Complete { get; set; }
    }


/*
 *  Ras, you can get live data like username and email contacts throught this.
 *  I got the info from a Microsoft guy when you were sleeping.
 *  He pointed me to this tutorial.
 *  https://www.windowsazure.com/en-us/develop/mobile/tutorials/single-sign-on-windows-8-dotnet/
 *  You need the live skd.
 *  http://msdn.microsoft.com/en-US/live/ff621310
 */

/*
    private LiveConnectSession session;

    private async System.Threading.Tasks.Task Authenticate()
    {
        LiveAuthClient liveIdClient = new LiveAuthClient("<< INSERT REDIRECT DOMAIN HERE >>");

        while (session == null)
        {
            // Force a logout to make it easier to test with multiple Microsoft Accounts
            if (liveIdClient.CanLogout)
                liveIdClient.Logout();

            LiveLoginResult result = await liveIdClient.LoginAsync(new[] { "wl.basic" });
            if (result.Status == LiveConnectSessionStatus.Connected)
            {
                session = result.Session;
                LiveConnectClient client = new LiveConnectClient(result.Session);
                LiveOperationResult meResult = await client.GetAsync("me");
                MobileServiceUser loginResult = await App.MobileService.LoginAsync(result.Session.AuthenticationToken);


                string title = string.Format("Welcome {0}!", meResult.Result["first_name"]);
                var message = string.Format("You are now logged in - {0}", loginResult.UserId);
                var dialog = new MessageDialog(message, title);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
            else
            {
                session = null;
                var dialog = new MessageDialog("You must log in.", "Login Required");
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
        }
    }
 */

    public sealed partial class MainPage : Page
    {
        // MobileServiceCollectionView implements ICollectionView (useful for databinding to lists) and 
        // is integrated with your Mobile Service to make it easy to bind your data to the ListView
        private MobileServiceCollectionView<TodoItem> items;

        private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();

        public string userInputText = "";

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void InsertTodoItem(TodoItem todoItem)
        {
            // This code inserts a new TodoItem into the database. When the operation completes
            // and Mobile Services has assigned an Id, the item is added to the CollectionView
            await todoTable.InsertAsync(todoItem);
            items.Add(todoItem); 
        }

        private void RefreshTodoItems()
        {
            // This code refreshes the entries in the list view be querying the TodoItems table.
            // The query excludes completed TodoItems
            items = todoTable
                .Where(todoItem => todoItem.Complete == false)
                .ToCollectionView();
            
            // Commented - MZ
            //ListItems.ItemsSource = items;
        }

        private async void UpdateCheckedTodoItem(TodoItem item)
        {
            // This code takes a freshly completed TodoItem and updates the database. When the MobileService 
            // responds, the item is removed from the list 
            await todoTable.UpdateAsync(item);
            items.Remove(item);
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTodoItems();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            // Commented - MZ
            // var todoItem = new TodoItem { Text = TextInput.Text };
            // InsertTodoItem(todoItem);
        }

        private void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            TodoItem item = cb.DataContext as TodoItem;
            UpdateCheckedTodoItem(item);
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await Authenticate();
            RefreshTodoItems();
        }

        private MobileServiceUser user;
        private async System.Threading.Tasks.Task Authenticate()
        {
            while (user == null)
            {
                string message;
                try
                {
                    user = await App.MobileService
                        .LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
                    message =
                        string.Format("You are now logged in - {0}", user.UserId);
                }
                catch (InvalidOperationException)
                {
                    message = "You must log in. Login Required";
                }

                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
        }

        private async void NoRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Need to get the user name here..
            //questionTextField.Text = Windows.System.UserProfile.UserInformation.GetDisplayNameAsync().ToString();//s +" clicked \"Yes\"";
        }

        private async void YesRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Need to get the user name here..
            //questionTextField.Text = Windows.System.UserProfile.UserInformation.GetDisplayNameAsync().ToString();//s +" clicked \"Yes\"";
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            this.userInputText = questionTextField.Text;

        }
    }
}

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
using Microsoft.Live;
using Windows.UI.Popups;                    
             
namespace VoteApp
{    
    public class TodoItem
    {
        public int Id { get; set; }

        //[DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "complete")]
        public bool Complete { get; set; }

        [DataMember(Name = "Question")]
        public string Question { get; set; }

        [DataMember(Name = "Answer")]
        public string Answer { get; set; }
    }

    public sealed partial class MainPage : Page
    {
        // MobileServiceCollectionView implements ICollectionView (useful for databinding to lists) and 
        // is integrated with your Mobile Service to make it easy to bind your data to the ListView
        private MobileServiceCollectionView<TodoItem> items;

        private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();

        public string userInputText = "";
        private string answer = "";

        public MainPage()
        {
            this.InitializeComponent();
        }

/*
 *  Get live data like username and email contacts throught this.
 *  Also gets rid of the 
 *  He pointed me to this tutorial.
 *  https://www.windowsazure.com/en-us/develop/mobile/tutorials/single-sign-on-windows-8-dotnet/
 *  You need the live skd.
 *  http://msdn.microsoft.com/en-US/live/ff621310
 */

    private MobileServiceUser user;
    private LiveConnectSession session;
    private string liveUsername;

    private async System.Threading.Tasks.Task Authenticate()
    {
        LiveAuthClient liveIdClient = new LiveAuthClient("https://voteapp.azure-mobile.net");

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


                // save live user name
                liveUsername = string.Format("" + meResult.Result["first_name"]);
            }
            else
            {
                questionTextField.Text = "Not connected";

                session = null;
                var dialog = new MessageDialog("You must log in.", "Login Required");
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
        }
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
            //var todoItem = new TodoItem { Text = TextInput.Text };
            //InsertTodoItem(todoItem);
        }

        private void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            TodoItem item = cb.DataContext as TodoItem;
            UpdateCheckedTodoItem(item);
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Disabling authentication for now.
            await Authenticate();
            RefreshTodoItems();
        }

        private void YesRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Need to get the user name here..
            // TODO: The radio button needs a string label (eg. 'Tron' or 'Looper') that can be sent to the server.
            answer = "Yes";
            //questionTextField.Text = Windows.System.UserProfile.UserInformation.GetDisplayNameAsync().ToString();//s +" clicked \"Yes\"";
        }

        private void NoRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Need to get the user name here..
            answer = "No";
            //questionTextField.Text = Windows.System.UserProfile.UserInformation.GetDisplayNameAsync().ToString();//s +" clicked \"Yes\"";
        }
        
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //this.userInputText = questionTextField.Text;
            var todoItem = new TodoItem { Question = questionTextField.Text, Answer = answer };
            InsertTodoItem(todoItem);
        }
    }
}

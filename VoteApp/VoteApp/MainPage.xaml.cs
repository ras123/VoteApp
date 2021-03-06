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
using Microsoft.Live;             

namespace VoteApp
{    
    public class QuestionItem
    {
        public int Id { get; set; }

        [DataMember(Name = "UserId")]
        public string UserId { get; set; }

        [DataMember(Name = "Question")]
        public string Question { get; set; }

        [DataMember(Name = "Answer")]
        public string Answer { get; set; }

        [DataMember(Name = "Friend")]
        public string Friend { get; set; }
    }

    public sealed partial class MainPage : Page
    {
        // MobileServiceCollectionView implements ICollectionView (useful for databinding to lists) and 
        // is integrated with your Mobile Service to make it easy to bind your data to the ListView
        private MobileServiceCollectionView<QuestionItem> items;

        private IMobileServiceTable<QuestionItem> questionTable = App.MobileService.GetTable<QuestionItem>();

        //private string Mike = "MicrosoftAccount:02ae8c9c32a856d3b1b9ed42e021f330";
        public string userInputText = "";
        private string answer = "";
        private string userId = "";

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void InsertTodoItem(QuestionItem questionItem)
        {
            // This code inserts a new TodoItem into the database. When the operation completes
            // and Mobile Services has assigned an Id, the item is added to the CollectionView
            await questionTable.InsertAsync(questionItem);
            items.Add(questionItem); 
        }

        private void RefreshTodoItems()
        {
            // Unanswered questions.
            items = questionTable
                .Where(todoItem => todoItem.UserId == userId && todoItem.Answer.Length == 0)
                .ToCollectionView();

            PendingQuestions.ItemsSource = items;

            // Answered questions.
            items = questionTable
                .Where(todoItem => todoItem.UserId == userId && todoItem.Answer.Length != 0)
                .ToCollectionView();

            AnsweredQuestions.ItemsSource = items;

            // Questions we've been asked.
            items = questionTable
                .Where(todoItem => todoItem.Friend == userId)
                .ToCollectionView();

            FriendsQuestions.ItemsSource = items;
        }

        private async void UpdateCheckedTodoItem(QuestionItem item)
        {
            // This code takes a freshly completed TodoItem and updates the database. When the MobileService 
            // responds, the item is removed from the list 
            await questionTable.UpdateAsync(item);
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
            QuestionItem item = cb.DataContext as QuestionItem;
            UpdateCheckedTodoItem(item);
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Disabling authentication for now.
            await Authenticate();
            RefreshTodoItems();
        }

/*
 *  Get live data like username and email contacts throught this.
 *  Also gets rid of the 
 *  He pointed me to this tutorial.
 *  https://www.windowsazure.com/en-us/develop/mobile/tutorials/single-sign-on-windows-8-dotnet/
 *  You need the live skd.
 *  http://msdn.microsoft.com/en-US/live/ff621310
 */

        private LiveConnectSession session;
        private async System.Threading.Tasks.Task Authenticate()
        {
            LiveAuthClient liveIdClient = new LiveAuthClient("https://voteapp.azure-mobile.net/");

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
                    var message = string.Format("You are now logged in.");// - {0}", loginResult.UserId);
                    userId = loginResult.UserId;
                    //userIDTextString.Text = userId;
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


        private void YesRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Need to get the user name here..
            // TODO: The radio button needs a string label (eg. 'Tron' or 'Looper') that can be sent to the server.
            answer = "Yes";
            QuestionAnswered();
            //questionTextField.Text = Windows.System.UserProfile.UserInformation.GetDisplayNameAsync().ToString();//s +" clicked \"Yes\"";
        }

        private void NoRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Need to get the user name here..
            answer = "No";
            QuestionAnswered();
            //questionTextField.Text = Windows.System.UserProfile.UserInformation.GetDisplayNameAsync().ToString();//s +" clicked \"Yes\"";
        }

        private void QuestionAnswered()
        {
            // Need to get the user name here..
            var todoItem = new QuestionItem { UserId = userId, Question = questionTextField.Text, Answer = answer, Friend = FriendBox.Text };
            InsertTodoItem(todoItem);
            RefreshTodoItems();
            //questionTextField.Text = Windows.System.UserProfile.UserInformation.GetDisplayNameAsync().ToString();//s +" clicked \"Yes\"";
        }
        
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //this.userInputText = questionTextField.Text;
            var todoItem = new QuestionItem { UserId = userId, Question = questionTextField.Text, Answer = "", Friend = FriendBox.Text };
            InsertTodoItem(todoItem);
        }
    }
}


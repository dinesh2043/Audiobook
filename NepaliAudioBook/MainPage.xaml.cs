using System;
using System.Net;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using Microsoft.Phone.Net.NetworkInformation;
using System.Threading;
using System.Windows.Navigation;
using Microsoft.Phone.BackgroundAudio;
using System.Windows.Media;

namespace NepaliAudioBook
{
    public partial class MainPage : PhoneApplicationPage
    {
        Color currentAccentColorHex = (Color)Application.Current.Resources["PhoneAccentColor"];
        private Popup popup;
        private BackgroundWorker backroungWorker;
        bool IgnoreMenuSelectionChanged = false;
        //ApplicationBarMenuItem menuItem;

        //initilizing the global variable
        App app = Application.Current as App;
        
        //ParserCat parser = new ParserCat();
        //String url = "http://www.nepaliaudiobook.com/json/main_json.php";
        //ApplicationBarMenuItem menuItem = App.Current.Resources["GlobalAppMenu"] as ApplicationBarMenuItem;
        
        // Constructor
        public MainPage()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                
                InitializeComponent();
                
                //ApplicationBar = App.Current.Resources["GlobalAppBar"] as ApplicationBar;
                
                //ShowSplash();
                //parser.parseJson(url);
                //adControl1.ErrorOccurred += new EventHandler<Microsoft.Advertising.AdErrorEventArgs>(adcontrol_ErrorOccurred);
                parseJson();
                ShowSplash();
                
                ApplicationBar = new ApplicationBar();
                ApplicationBar.Mode = ApplicationBarMode.Default;
                ApplicationBar.Opacity = 1.0;
                ApplicationBar.IsVisible = true;
                ApplicationBar.IsMenuEnabled = true;
            }
            else {
                MessageBox.Show("This Application Needs Connect Internet");
            }
            
            //PageTitle.Text = menuItem.Text;

        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // to remove the selection on tasklist
            //and generate a flag to by pass the selection chenged event
            if (menuList.SelectedIndex > 0)
            {
                IgnoreMenuSelectionChanged = true;
                menuList.SelectedIndex = -1;
            }
            button1.Visibility = Visibility.Collapsed;
            switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.Playing:
                    button1.Visibility = Visibility.Visible;
                    app.backAudioPlayer = true;
                    break;
            }
            //ApplicationBarMenuItem menuItem = new ApplicationBarMenuItem();

            //taskList.Items.Clear();
            //taskList.SelectedIndex = -1;
            if (app.fromPlayer == true) // to check if the playerindex is changed
            {
                
                if (app.prePlayerIndex > 0)
                {
                    ListBoxItem lbi3 = (ListBoxItem)(menuList.ItemContainerGenerator.ContainerFromItem(menuList.Items[app.prePlayerIndex]));

                    lbi3.Foreground = new SolidColorBrush(Colors.White);
                }
                SolidColorBrush thameColor = new SolidColorBrush(currentAccentColorHex);
                ListBoxItem lbi2 = (ListBoxItem)(menuList.ItemContainerGenerator.ContainerFromItem(menuList.Items[app.playerIndex]));

                lbi2.Foreground = thameColor;
                app.prePlayerIndex = app.playerIndex;
                app.fromPlayer = false;
                // to change the flag back to normal after bypassing the selection changed event
                // for the normal procedure of application
                IgnoreMenuSelectionChanged = false;
            }
            if (app.fromTracks == true) // to check if the playerindex is changed
            {
                if (app.preTrackIndex > 0)
                {
                    ListBoxItem lbi4 = (ListBoxItem)(menuList.ItemContainerGenerator.ContainerFromItem(menuList.Items[app.preTrackIndex]));

                    lbi4.Foreground = new SolidColorBrush(Colors.White);
                }

                ListBoxItem lbi5 = (ListBoxItem)(menuList.ItemContainerGenerator.ContainerFromItem(menuList.Items[app.indexSelected]));

                lbi5.Foreground = new SolidColorBrush(Colors.Red);
                app.preTrackIndex = app.indexSelected;
                app.fromTracks = false;
                // to change the flag back to normal after bypassing the selection changed event
                // for the normal procedure of application
                IgnoreMenuSelectionChanged = false;
            }
            }
        //void adcontrol_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        //{
        //}
        private void generateRecent()
        {
            menuList.Items.Clear();
            PageTitle.Text ="Recent";
            app.albumId = new List<string>();
            //var rootObject = JsonConvert.DeserializeObject<Main.RootObject>(e.Result);
            app.songUrl = new List<string>();
            app.albumName = new List<string>();
            app.ImageUrlList = new List<string>();
            app.subNoteList = new List<string>();
            app.trackNameList = new List<string>();
            foreach (var track in app.Recents["Recent"])
            {
                this.menuList.Items.Add(new MyData() { album = track.album_name, author = track.track_name, imageUrl = track.image_icon });
                app.songUrl.Add(track.media_file);
                app.albumName.Add(track.album_name);
                app.ImageUrlList.Add(track.image_icon);
                app.subNoteList.Add(track.sub_note);
                app.trackNameList.Add(track.album_name);
            }
        }
        
        
        public void ShowSplash()
        {
            this.popup = new Popup();
            this.popup.Child = new CustomSplashScreen();
            this.popup.IsOpen = true;
            StartLoadingData();
        }

        private void StartLoadingData()
        {
            backroungWorker = new BackgroundWorker();
            backroungWorker.DoWork += new DoWorkEventHandler(backroungWorker_DoWork);
            backroungWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backroungWorker_RunWorkerCompleted);
            backroungWorker.RunWorkerAsync();
        }
        void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                this.popup.IsOpen = false;
            }
            );
        }
        void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //here we can load data
            Thread.Sleep(7000);
        }
        
        private void exitBtn_Click(object sender, EventArgs e)
        {
            //Do work for your application here.
            MessageBox.Show("Button works");
            
        }
        public void parseJson()
        {
            String url = "http://www.nepaliaudiobook.com/json/main_json.php";
            // for connecting to the jsonfile 
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri(url));
        }
        public void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            
            var rootObject = JsonConvert.DeserializeObject< Main.RootObject>(e.Result);
            app.Categories = new Dictionary<string, List<Main.Album>>();
            app.Recents = new Dictionary<string, List<Main.Track>>();
            app.MenuList = new List<string>();
            //initilizing the dictonary
            
            foreach (var item in rootObject.categories)
            {
                //menuList.Items.Add(item.cat_name);
                app.MenuList.Add(item.cat_name);
                if (item.cat_name.Equals("Recent"))
                {
                    app.Recents.Add(item.cat_name, item.tracks);
                }
                else
                {
                    app.Categories.Add(item.cat_name, item.albums);
                }
            }
           
            foreach (var item in app.MenuList)
            {
                ApplicationBarMenuItem menuItem = new ApplicationBarMenuItem();
                menuItem.Text = item;
                ApplicationBar.MenuItems.Add(menuItem);
                menuItem.Click += new EventHandler(menuItem_Click);
            }
            if (app.menuSelectedValue != null)
            {
                loadSelecteditem(app.menuSelectedValue);
            }
            else
            {
                generateRecent();
            }
        }
        
            
        
        private void menuItem_Click(object sender, EventArgs e)
        {
            ApplicationBarMenuItem mnu = (ApplicationBarMenuItem)sender;
            //MessageBox.Show("Menu item 1 works!" + mnu.Text);
            PageTitle.Text = mnu.Text;
            
            if (app.Categories.ContainsKey(mnu.Text))
            {
                /**
                ShowSplash();
                menuList.Items.Clear();
                
                app.albumId.Clear();
                app.songUrl.Clear();
                app.albumName.Clear();
                app.ImageUrlList.Clear();
                app.subNoteList.Clear();
                app.trackNameList.Clear();
                app.trackImage = new List<string>();
                //object Category = app.Categories[mnu.Text];
                foreach (var cat in app.Categories[mnu.Text])
                {
                    this.menuList.Items.Add(new MyData() { album = cat.album_name, author = cat.author_name, imageUrl = cat.image_icon });
                    //albumList.Items.Add(album.album_name);
                    app.albumName.Add(cat.album_name);
                    app.albumId.Add(cat.album_id);
                    app.trackImage.Add(cat.image_icon);
                    app.subNoteList.Add(cat.sub_note);
                }
                 * **/
                CreateListItems(mnu.Text);
            }
            else
            {
                generateRecent();
            }
            
            
        }
        
        public class MyData
        {
            public string album { get; set; }
            public string author { get; set; }
            public string imageUrl { get; set; }

        }

        private void menuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (PageTitle.Text.Equals("Recent"))
            {
                if (!IgnoreMenuSelectionChanged)
                {
                    app.songUrlSelectedIndex = menuList.SelectedIndex;
                    if (app.playerIndex == menuList.SelectedIndex && app.backAudioPlayer == true)
                    {
                        NavigationService.Navigate(new Uri("/PlayerPage.xaml?msg=" + "NowPlaying", UriKind.Relative));
                    }
                    else
                    {

                        //if (menuList.SelectedIndex != -1)
                        //{
                        //app.songUrlSelectedIndex = menuList.SelectedIndex;
                        //app.trackImage = app.ImageUrlList[menuList.SelectedIndex];
                        NavigationService.Navigate(new Uri("/PlayerPage.xaml?msg=" + app.albumName[menuList.SelectedIndex], UriKind.Relative));
                        //menuList.SelectedIndex = -1;

                        //}
                    }
                }
            }
            else
            {
                //if (menuList.SelectedIndex != -1)
                //{
                if (!IgnoreMenuSelectionChanged)
                {
                    app.indexSelected = menuList.SelectedIndex;
                    NavigationService.Navigate(new Uri("/TracksPage.xaml?msg=" + app.albumName[menuList.SelectedIndex], UriKind.Relative));
                }
                //MessageBox.Show(""+albumList.SelectedValue);
                //menuList.SelectedIndex = -1;
                //}

            }
        }

        public void loadSelecteditem(string p)
        {
            
            if (app.Categories.ContainsKey(p))
            {
                CreateListItems(p); 
            }
            else
            {
                generateRecent();
            }
        }
        public void CreateListItems(string itemClicked)
        {
            //ShowSplash();
            menuList.Items.Clear();
            PageTitle.Text = itemClicked;
            /**
            app.albumId = new List<string>();
            //var rootObject = JsonConvert.DeserializeObject<Main.RootObject>(e.Result);
            app.songUrl = new List<string>();
            app.albumName = new List<string>();
            app.ImageUrlList = new List<string>();
            app.subNoteList = new List<string>();
            app.trackNameList = new List<string>();
            **/
            app.albumId.Clear();
            app.songUrl.Clear();
            app.albumName.Clear();
            app.ImageUrlList.Clear();
            app.subNoteList.Clear();
            app.trackNameList.Clear();
            app.trackImage = new List<string>();
            //object Category = app.Categories[mnu.Text];
            foreach (var cat in app.Categories[itemClicked])
            {
                this.menuList.Items.Add(new MyData() { album = cat.album_name, author = cat.author_name, imageUrl = cat.image_icon });
                //albumList.Items.Add(album.album_name);
                app.albumName.Add(cat.album_name);
                app.albumId.Add(cat.album_id);
                app.trackImage.Add(cat.image_icon);
                app.subNoteList.Add(cat.sub_note);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PlayerPage.xaml?msg=" + "NowPlaying", UriKind.Relative));
        }
    }
}

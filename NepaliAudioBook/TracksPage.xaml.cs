using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Threading;
using Microsoft.Phone.Shell;
using Microsoft.Phone.BackgroundAudio;

namespace NepaliAudioBook
{
    public partial class TracksPage : PhoneApplicationPage
    {
        Color currentAccentColorHex = (Color)Application.Current.Resources["PhoneAccentColor"];
        App app = Application.Current as App;
        //ApplicationBarMenuItem menuItem = App.Current.Resources["GlobalAppMenu"] as ApplicationBarMenuItem;
        bool IgnoreSelectionChanged = false;
        
        public TracksPage()
        {
            InitializeComponent();
            //ApplicationBar = App.Current.Resources["GlobalAppBar"] as ApplicationBar;
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            
            parseJson();
            //ShowSplash();
            
        }
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // to remove the selection on tasklist
            //and generate a flag to by pass the selection chenged event
            if (taskList.SelectedIndex>0){
                IgnoreSelectionChanged = true;
                taskList.SelectedIndex = -1;
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
            
            PageTitle.Text = NavigationContext.QueryString["msg"];
            if (app.fromPlayer == true){
                
            //taskList.SelectedIndex = -1;
            //taskList.SelectedIndex = app.playerIndex;
            //TextBlock trackName = taskList.Items[app.playerIndex] as TextBlock;
            //trackName.Foreground = new SolidColorBrush(Color.FromArgb(0,250, 15, 15));
                if (app.prePlayerIndex > 0)
                {
                    ListBoxItem lbi3 = (ListBoxItem)(taskList.ItemContainerGenerator.ContainerFromItem(taskList.Items[app.prePlayerIndex]));

                    lbi3.Foreground = new SolidColorBrush(Colors.White);
                }
                SolidColorBrush thameColor = new SolidColorBrush(currentAccentColorHex);
                ListBoxItem lbi2 = (ListBoxItem)(taskList.ItemContainerGenerator.ContainerFromItem(taskList.Items[app.playerIndex]));

                lbi2.Foreground = thameColor;
                app.prePlayerIndex = app.playerIndex;
                app.fromPlayer = false;
                // to change the flag back to normal after bypassing the selection changed event
                // for the normal procedure of application
                IgnoreSelectionChanged = false;
            }
        }
        

        private void parseJson()
        {
            String url = "http://www.nepaliaudiobook.com/json/album_json.php?album_id="+app.albumId[app.indexSelected];
            // for connecting to the jsonfile 
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri(url));
        }
        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            app.songUrl = new List<string>();
            app.trackNameList = new List<string>();
            //app.songUrl.Clear();
            //app.trackNameList.Clear();
            app.ImageUrlList = new List<string>();
            //app.ImageUrlList.Clear();
            
            var rootObject = JsonConvert.DeserializeObject<ITrack.RootObject>(e.Result);
            foreach (var track in rootObject.tracks)
            {
                taskList.Items.Add(new MyData() { trackName = track.track_name, subNote = app.subNoteList[app.indexSelected], imageUrl = app.trackImage[app.indexSelected] });
                //taskList.Items.Add(track.track_name);
                app.songUrl.Add(track.media_file);
                app.trackNameList.Add(track.track_name);
                app.ImageUrlList.Add(app.trackImage[app.indexSelected]);
                //app.trackImage = app.ImageUrlList[app.indexSelected];
            }
            foreach (var item in app.MenuList)
            {
                ApplicationBarMenuItem menuItem = new ApplicationBarMenuItem();
                menuItem.Text = item;
                ApplicationBar.MenuItems.Add(menuItem);
                menuItem.Click += new EventHandler(menuItem_Click);
            }
        }
        private void menuItem_Click(object sender, EventArgs e)
        {
            //ShowSplash();
            //menuList.Items.Clear();
            ApplicationBarMenuItem mnu = (ApplicationBarMenuItem)sender;
            //MessageBox.Show("Menu item 1 works!" + mnu.Text);
            //NavigationService.Navigate(new Uri("/MainPage.xaml?msg=" + mnu.Text, UriKind.Relative));
            app.menuSelectedValue = mnu.Text;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        public class MyData
        {
            public string trackName { get; set; }
            public string subNote { get; set; }
            public string imageUrl { get; set; }

        }
        /**
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
        **/
        private void taskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!IgnoreSelectionChanged){
                    app.songUrlSelectedIndex = taskList.SelectedIndex;
                    if (app.playerIndex == taskList.SelectedIndex && app.backAudioPlayer == true)
                    {
                        NavigationService.Navigate(new Uri("/PlayerPage.xaml?msg=" + "NowPlaying", UriKind.Relative));
                    }
                    else
                    {
                        NavigationService.Navigate(new Uri("/PlayerPage.xaml?msg=" + app.trackNameList[app.songUrlSelectedIndex], UriKind.Relative));
                    }
                }
        
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PlayerPage.xaml?msg=" + "NowPlaying", UriKind.Relative));
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                app.fromTracks = true;
            this.NavigationService.GoBack();
            //NavigationService.Navigate(new Uri("/TracksPage.xaml?msg=" + app.albumName[index], UriKind.Relative));
            // do some stuff ...

            // cancel the navigation
            //e.Cancel = true;
        }
 
    }
}
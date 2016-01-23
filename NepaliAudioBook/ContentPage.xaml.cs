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
using System.Collections;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Threading;



namespace NepaliAudioBook
{
    public partial class ContentPage 
    {
        App app = Application.Current as App;
        public ContentPage()
        {
            InitializeComponent();
            //ShowSplash();
            parseJson();
        }
        /**
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
            Thread.Sleep(10000);
        }

        **/

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            PageTitle.Text = NavigationContext.QueryString["msg"];
        }
        
        private void parseJson()
        {
            String url = "http://www.nepaliaudiobook.com/json/main_json.php";
            // for connecting to the jsonfile 
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri(url));
        }
        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            app.albumId = new List<string>();
            var rootObject = JsonConvert.DeserializeObject< Main.RootObject>(e.Result);
            app.songUrl = new List<string>();
            app.albumName = new List<string>();
            app.ImageUrlList = new List<string>();
            app.subNoteList = new List<string>();
            app.trackNameList = new List<string>();
            foreach (var item in rootObject.categories)
            {
                if (item.cat_name == PageTitle.Text) 
                {
                    if (item.cat_name == "Recent")
                    {
                        foreach (var track in item.tracks)
                        {
                            albumList.Items.Add(new MyData() { album = track.album_name, author = track.track_name, imageUrl = track.image_icon });
                            app.songUrl.Add(track.media_file);
                            app.albumName.Add(track.album_name);
                            app.ImageUrlList.Add(track.image_icon);
                            app.subNoteList.Add(track.sub_note);
                            app.trackNameList.Add(track.album_name);
                        }
                    }
                    else foreach (var album in item.albums)
                    {
                        
                        this.albumList.Items.Add(new MyData() { album = album.album_name, author = album.author_name , imageUrl= album.image_icon });
                        //albumList.Items.Add(album.album_name);
                        app.albumName.Add(album.album_name);
                        app.albumId.Add(album.album_id);
                        app.ImageUrlList.Add(album.image_icon);
                        app.subNoteList.Add(album.sub_note);
                    }
                    //MessageBox.Show("here !!!!");
                }
            }
        }
        public class MyData
        {
            public string album { get; set; }
            public string author { get; set; }
            public string imageUrl { get; set; }
            
        }
       
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void albumList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PageTitle.Text.Equals("Recent"))
            {
                app.songUrlSelectedIndex = albumList.SelectedIndex;
                //app.trackImage = app.ImageUrlList[albumList.SelectedIndex];
                NavigationService.Navigate(new Uri("/PlayerPage.xaml?msg=" + app.albumName[albumList.SelectedIndex], UriKind.Relative));
            }else{
                app.indexSelected = albumList.SelectedIndex;
                NavigationService.Navigate(new Uri("/TracksPage.xaml?msg=" + app.albumName[albumList.SelectedIndex], UriKind.Relative));
                //MessageBox.Show(""+albumList.SelectedValue);
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
            
    }
}
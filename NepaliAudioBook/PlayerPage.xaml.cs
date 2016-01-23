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
using Microsoft.Phone.BackgroundAudio;
using System.IO.IsolatedStorage;
using System.Windows.Resources;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace NepaliAudioBook
{
    public partial class PlayerPage : PhoneApplicationPage 
    {
        App app = Application.Current as App;
        int index = 0;
        ImageSourceConverter s = new ImageSourceConverter();
        

        // check Songs playing time -> update TextBlocks and Slider
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        
        public PlayerPage()
        {
            InitializeComponent();
            index = app.songUrlSelectedIndex;
            // handle PlayState from AudioPlayback Agent
            BackgroundAudioPlayer.Instance.PlayStateChanged += new EventHandler(Instance_PlayStateChanged);
            
        }
        void Instance_PlayStateChanged(object sender, EventArgs e)
        {
            // if something is playing (a new song)
            if (BackgroundAudioPlayer.Instance.Track != null)
            {
                // show soung info
                //TitleTextBlock.Text = BackgroundAudioPlayer.Instance.Track.Title;
                //ArtistTextBlock.Text = BackgroundAudioPlayer.Instance.Track.Artist;
                //PageTitle.Text = BackgroundAudioPlayer.Instance.Track.Album.ToString();
                // handle slider and texts
                slider.Minimum = 0;
                slider.Maximum = BackgroundAudioPlayer.Instance.Track.Duration.TotalMilliseconds;
                slider.Value = BackgroundAudioPlayer.Instance.Position.TotalMilliseconds;
                string text = BackgroundAudioPlayer.Instance.Track.Duration.ToString();
            }
        }
        
        private void startTimer()
        {
            // start timer to check position of the song
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // song is playing
            if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState)
            {
                // handle slider
                slider.Minimum = 0;
                slider.Value = BackgroundAudioPlayer.Instance.Position.TotalMilliseconds;
                slider.Maximum = BackgroundAudioPlayer.Instance.Track.Duration.TotalMilliseconds;
                // display text
                string text = BackgroundAudioPlayer.Instance.Position.ToString();
                StartTime.Text = text.Substring(0, 8);
                text = BackgroundAudioPlayer.Instance.Track.Duration.ToString();
                EndTime.Text = text.Substring(0, 8);
                if (slider.Value== slider.Maximum){
                    ForwardMethod();
                }
            }
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            /**
            switch(BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.Unknown:
                    startTimer();
                    base.OnNavigatedTo(e);
                    PageTitle.Text = NavigationContext.QueryString["msg"];
                    //image1.Source = app.songUrl[index];
                    image1.Source = (ImageSource)s.ConvertFromString(app.ImageUrlList[index]);
                    //new AudioTrack(new Uri("http://relay.radioreference.com:80/346246215", UriKind.RelativeOrAbsolute),
                    //"Title","Artist","Album",null),
                    AudioTrack audioTrack = new AudioTrack(new Uri("" + app.songUrl[app.songUrlSelectedIndex]),
                           NavigationContext.QueryString["msg"],
                           app.ImageUrlList[index],
                           app.trackNameList[index],
                           null,
                           null,
                           EnabledPlayerControls.Pause);
                    BackgroundAudioPlayer.Instance.Track = audioTrack;
                    break;
                case PlayState.Playing:
                    PageTitle.Text = BackgroundAudioPlayer.Instance.Track.Album.ToString();
                    image1.Source = (ImageSource)s.ConvertFromString(BackgroundAudioPlayer.Instance.Track.Artist.ToString());
                    startTimer();
                    break;
            }
             * **/

            if (NavigationContext.QueryString["msg"] == "NowPlaying")
            {
                PageTitle.Text = BackgroundAudioPlayer.Instance.Track.Album.ToString();
                image1.Source = (ImageSource)s.ConvertFromString(BackgroundAudioPlayer.Instance.Track.Artist.ToString());
                startTimer();
            }
            else
            {
                startTimer();
                base.OnNavigatedTo(e);
                PageTitle.Text = NavigationContext.QueryString["msg"];
                // to track the index when a song is selected
                app.playerIndex = index;
                //image1.Source = app.songUrl[index];
                image1.Source = (ImageSource)s.ConvertFromString(app.ImageUrlList[index]);
                //new AudioTrack(new Uri("http://relay.radioreference.com:80/346246215", UriKind.RelativeOrAbsolute),
                //"Title","Artist","Album",null),
                AudioTrack audioTrack = new AudioTrack(new Uri("" + app.songUrl[app.songUrlSelectedIndex]),
                       NavigationContext.QueryString["msg"],
                       app.ImageUrlList[index],
                       app.trackNameList[index],
                       null,
                       null,
                       EnabledPlayerControls.Pause);
                BackgroundAudioPlayer.Instance.Track = audioTrack;
            }
            
            ApplicationBarIconButton b = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            b.IconUri = new Uri("/image/Pause.png", UriKind.Relative);
            
        }
        private void ForwardMethod()
        {
            
            index ++;
            
            if (index < app.songUrl.Count())
            {
                
                PageTitle.Text = app.trackNameList[index];
                image1.Source = (ImageSource)s.ConvertFromString(app.ImageUrlList[index]);
                AudioTrack audioTrack = new AudioTrack(new Uri("" + app.songUrl[index]),
                   app.trackNameList[index],
                   app.ImageUrlList[index],
                   app.trackNameList[index],
                   null,
                   null,
                   EnabledPlayerControls.Pause);
                BackgroundAudioPlayer.Instance.Track = audioTrack;
                startTimer();
                app.playerIndex = index;
                
            }
            else
            {
                index--;
            }
        }
        

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            ForwardMethod();
            /*
            index ++;
            
            if (index < app.songUrl.Count())
            {
                
                PageTitle.Text = app.trackNameList[index];
                image1.Source = (ImageSource)s.ConvertFromString(app.ImageUrlList[index]);
                AudioTrack audioTrack = new AudioTrack(new Uri("" + app.songUrl[index]),
                   app.trackNameList[index],
                   app.ImageUrlList[index],
                   app.trackNameList[index],
                   null,
                   null,
                   EnabledPlayerControls.Pause);
                BackgroundAudioPlayer.Instance.Track = audioTrack;
                startTimer();
                app.playerIndex = index;
                
            }
            else
            {
                index--;
            }
             */
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            ApplicationBarIconButton b = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            //trackList.taskList.SelectedIndex = index;
            switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.Playing:
                    BackgroundAudioPlayer.Instance.Pause();
                    b.IconUri = new Uri("/image/Play.png", UriKind.Relative);
                    break;
                case PlayState.Paused:
                    BackgroundAudioPlayer.Instance.Play();
                    b.IconUri = new Uri("/image/Pause.png", UriKind.Relative);
                    break;
                case PlayState.Unknown:
                    
                    break;

            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {

            if (BackgroundAudioPlayer.Instance.PlayerState != PlayState.Stopped)
            {
                /**
                if(BackgroundAudioPlayer.Instance.BufferingProgress == 0)
                {
                    NavigationService.GoBack();
                }
                else
                {
                 * **/
                    BackgroundAudioPlayer.Instance.Close();
                    //BackgroundAudioPlayer.Instance.Stop();
                    //b.IconUri = new Uri("/image/Back.png", UriKind.Relative);
                    dispatcherTimer.Stop();
                    dispatcherTimer.Tick -= new EventHandler(dispatcherTimer_Tick);
                    BackgroundAudioPlayer.Instance.PlayStateChanged -= new EventHandler(Instance_PlayStateChanged);
                    app.fromPlayer = true;
                    app.backAudioPlayer = false;
                    NavigationService.GoBack();
                    //NavigationService.Navigate(new Uri("/TracksPage.xaml?msg=" + app.albumName[index], UriKind.Relative));
            }   
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            index --;
            
            if (index > -1)
            {
                
                PageTitle.Text = app.trackNameList[index];
                image1.Source = (ImageSource)s.ConvertFromString(app.ImageUrlList[index]);
                AudioTrack audioTrack = new AudioTrack(new Uri("" + app.songUrl[index]),
                   app.trackNameList[index],
                   app.ImageUrlList[index],
                   app.trackNameList[index],
                   null,
                   null,
                   EnabledPlayerControls.Pause);
                BackgroundAudioPlayer.Instance.Track = audioTrack;
                startTimer();
                app.playerIndex = index;
            }
            else
            {
                index++;
            }
        }

        private void slider_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            // get slider value
            int sliderValue = (int)slider.Value;
            // create timespan object with milliseconds (from slider value)
            TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, sliderValue);
            // set a new position of the song
            BackgroundAudioPlayer.Instance.Position = timeSpan;
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                app.fromPlayer = true;
                this.NavigationService.GoBack();
            //NavigationService.Navigate(new Uri("/TracksPage.xaml?msg=" + app.albumName[index], UriKind.Relative));
            // do some stuff ...

            // cancel the navigation
            //e.Cancel = true;
        }
    
    }

}
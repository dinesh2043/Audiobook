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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NepaliAudioBook
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }
        //defining the global variable
        public Dictionary<string, List<Main.Album>> Categories {get; set;}
        public Dictionary<string, List<Main.Track>> Recents { get; set; }
        public Dictionary<string, List<ITrack>> Tracks { get; set; }
        public List<string> MenuList { get; set;}
        public string menuSelectedValue {get; set; }

        //public Dictionary<string, string> albums { get; set; }
        //public Dictionary<string, string> tracks { get; set; }
        public List<string> albumId { get; set; }
        public List<string> songUrl { get; set; }
        public List<string> recientList { get; set; }
        public List<string> albumName { get; set; }
        public List<string> ImageUrlList { get; set; }
        public List<string> subNoteList { get; set; }
        public List<string> trackNameList { get; set; }
        public int songUrlSelectedIndex { get; set; }
        public List<string> trackImage { get; set; }
        public int indexSelected { get; set; }
        public string selectedValue { get; set; }

        public int playerIndex { get; set; }
        public int prePlayerIndex { get; set; }
        public int preTrackIndex { get; set; }
        public bool fromPlayer { get; set; }
        public bool fromTracks { get; set; }
        public bool backAudioPlayer { get; set; }
        //public List<string> trackUrl { get; set; }
        //public int trackSelectedIndex { get; set; }

        ParserCat parser = new ParserCat();
        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            //RootFrame.Background = App.Current.Resources["MainBackground"] as ImageBrush;
            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            Util.Trace("***** Application Launching\t\t ( " + DateTime.Now.Ticks + " *****)");
            //parser.parseJson();
            //ApplicationBar appBar = ((ApplicationBar)Application.Current.Resources["GlobalAppBar"]);
            //ApplicationBarMenuItem menuItem = ((ApplicationBarMenuItem)Application.Current.Resources["GlobalAppMenu"]);
            //menuItem.Click + = new EventHandler(menuItem_Click);
            /**
            List<string> menuItems = new List<string>(new string[] { "Recent", "Novels", "Others", "Bubul", "Stories", "Gazal", "Yo Sanjh Yo Subash", "Popular" });
            
            for (int i = 0; i < menuItems.Count(); i++)
            {
                ApplicationBarMenuItem menuItem = new ApplicationBarMenuItem();
                menuItem.Text = menuItems[i];
                appBar.MenuItems.Add(menuItem);
                menuItem.Click += new EventHandler(menuItem_Click);
            }
             * **/
        }
        /**
        private void menuItem_Click(object sender, EventArgs e)
        {
            ApplicationBarMenuItem mnu = (ApplicationBarMenuItem)sender;
            //MessageBox.Show("here !!!!"+mnu.Text);
            MainPage mp = new MainPage();
            mp.loadSelecteditem(mnu.Text);
        }
         ***/
        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            Util.Trace("***** Application Activated\t\t ( " + DateTime.Now.Ticks + " *****)");
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            Util.Trace("***** Application Deactivated\t\t ( " + DateTime.Now.Ticks + " *****)");
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            Util.Trace("***** Application Closing\t\t ( " + DateTime.Now.Ticks + " *****)");
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }


        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}
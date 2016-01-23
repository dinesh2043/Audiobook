using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Phone.Shell;

namespace NepaliAudioBook
{
    public class ParserCat
    {
        //initilizing the global variable

        App app = Application.Current as App;

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
            //mPage.createAppBar(app.MenuList);
             
        }
    }

}

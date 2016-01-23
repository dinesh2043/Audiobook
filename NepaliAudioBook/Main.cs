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
using System.Collections.Generic;


namespace NepaliAudioBook
{
    public class Main
    {
        public class Info
        {
            public string onOff { get; set; }
            public string msg { get; set; }
            public string version { get; set; }
            public string i_onOff { get; set; }
            public string i_msg { get; set; }
            public string i_version { get; set; }
        }

        public class Track
        {
            public string album_name { get; set; }
            public string album_id { get; set; }
            public string track_name { get; set; }
            public string sub_note { get; set; }
            public string image_icon { get; set; }
            public string media_file { get; set; }
            public string track_id { get; set; }
        }

        public class Album
        {
            public string album_name { get; set; }
            public string album_id { get; set; }
            public string author_name { get; set; }
            public string sub_note { get; set; }
            public string image_icon { get; set; }
        }

        public class Category
        {
            public string cat_name { get; set; }
            public List<Track> tracks { get; set; }
            public List<Album> albums { get; set; }
        }

        public class RootObject
        {
            public List<Info> info { get; set; }
            public List<Category> categories { get; set; }
        }


    }
}

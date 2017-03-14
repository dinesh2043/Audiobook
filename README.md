# Nepali Audiobook application for windows phone
### It is a Windows phone application developed for nepali Nepali audience and previously it was published in the windows phone app store but it is currently suspended in app store. Due to the busy schedule I am unable to do those necessary upgrades but, I am plannaing to do those in recent future.

This is a windows client application which uses the web service provided in json file to perform its operation. This application uses dynamic user interface creation according to the contents of json file available in web. Which has made this application robust and it works normally even if there is some changes in the web server contents and application update itself according to the changes. The complete user interface including menu is created dynamically. The application consists of front-end and back end classes as follows; MainPage, ContentPage, TracksPage and PlayerPage. Microsoft phone BackgroundAudio -> AudioPlayerAgent is used for creating the audio player for this application. Other important class for this applications are ParserCat.cs json parser, SplashScreen.cs, Main.cs object class and AudioPlayer.cs class. Since, it was published as a free app in app stored I have also used Microsoft Advertising SDK to create some ravinue through this app but now they have Ad meditation SDK which is flexible. Since, I have allocated some space below title bar for advertisment and in emulator it doesnot work due to that reasion there is a empty space in the user interface.    

### In the following image we can see the splash screen when the application is started.
![img](https://github.com/dinesh2043/Audiobook/blob/master/img7.jpg)

### When the application first loads it displays the contents of the recient updates in the web server which can be seen in the following image.

![img](https://github.com/dinesh2043/Audiobook/blob/master/img1.jpg)

### The following picture shows the menu items and all of the user interface contents and menu items are dynamically created according to the data gathered by ParcerCat.cs class. 

![img](https://github.com/dinesh2043/Audiobook/blob/master/img2.jpg)

### Following image shows the contents of others menu item and all of this items have sub-items.

![img](https://github.com/dinesh2043/Audiobook/blob/master/img3.jpg)

### This image consists the dynamic user interface contents generated when one of the items of others is selected. This list contains the audio track list.

![img](https://github.com/dinesh2043/Audiobook/blob/master/img4.jpg)

### In the following picture you can see the audio player created with the implementations of PlayerPage classes implementation. It has implemented slide option to forward the track playing process. It automitically goes to the next track when one track is completed. Pause, Stop, Next and Previous features are also implemented.

![img](https://github.com/dinesh2043/Audiobook/blob/master/img5.jpg)

### When the application is playing the tracks in the background now playing button is displayed in all the pages to navigate back to the player page and if other tracks are selected then the audio playing in the background will be stoped and new audio starts to play.

![img](https://github.com/dinesh2043/Audiobook/blob/master/img6.jpg)


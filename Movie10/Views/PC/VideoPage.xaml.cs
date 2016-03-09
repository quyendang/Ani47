using Microsoft.Advertising.WinRT.UI;
using Movie10.Utils;
using Movie10.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using VungleSDK;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Movie10.Views.PC
{
    public sealed partial class VideoPage : UserControl
    {
        private bool haveUser = false;
        public EventHandler BackClick;
        InterstitialAd MyVideoAd = new InterstitialAd();
        public MoviesItems currentMovie;
        public bool readyPlay = false;
        public VideoPage()
        {
            this.InitializeComponent();
           
            // sdkInstance.
            var applicationSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (applicationSettings.Values.ContainsKey("user"))
            {
                userTbx.Visibility = Visibility.Collapsed;
                emailTbx.Visibility = Visibility.Collapsed;
                this.haveUser = true;
            }
            try
            {
                // wire up all 4 events, see below for function templates
                MyVideoAd.AdReady += MyVideoAd_AdReady;
                MyVideoAd.ErrorOccurred += MyVideoAd_ErrorOccurred;
                MyVideoAd.Completed += MyVideoAd_Completed;
                MyVideoAd.Cancelled += MyVideoAd_Cancelled;

                // pre-fetch an ad 30-60 seconds before you need it
                MyVideoAd.RequestAd(AdType.Video, "11562921", "0d9b9468-423c-4f5b-8697-43d2f83f766a");
               
                //sdkInstance = AdFactory.GetInstance("56547b0ebc3d01137b00000f");
               // sdkInstance.OnAdPlayableChanged += SdkInstance_OnAdPlayableChanged;
            }
            catch { }
        }

      void MyVideoAd_AdReady(object sender, object e)
        {
            Debug.WriteLine(e.ToString());           
        }

        void MyVideoAd_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            Debug.WriteLine(e.ErrorMessage.ToString());
        }

       void MyVideoAd_Completed(object sender, object e)
        {
            Debug.WriteLine(e.ToString());
        }

        void MyVideoAd_Cancelled(object sender, object e)
        {
            Debug.WriteLine(e.ToString());
        }
        
        public async void Init(MoviesItems m)
        {
            try
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += VideoPage_BackRequested;
                listcommentList.Items.Clear();
                currentMovie = m;
                if (App.ViewModel.movieDetails != null)
                    App.ViewModel.movieDetails = (MovieDetail)null;

                if (m == null)
                    return;
                App.ViewModel.IsLoading = true;
                var client = new HttpClient();
                string a = await client.GetStringAsync("http://quyen23.cloudapp.net/quyenpro/getmoviepart.php?film_id=" + m.id);
                List<Episode> listEpisode = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Episode>>(a);
                string b = await client.GetStringAsync("http://quyen23.cloudapp.net/quyenpro/getcommentbyid.php?film_id=" + m.id);
                List<Comment> cmtTemp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Comment>>(b);
                if (cmtTemp != null)
                {
                    foreach (var item in cmtTemp)
                    {
                        Comment usercmt = new Comment();
                        usercmt.id = item.id;
                        usercmt.user = item.user;
                        usercmt.comment = item.comment;
                        usercmt.film_id = item.film_id;
                        listcommentList.Items.Add(usercmt);
                    }
                }

                string c = await client.GetStringAsync("http://quyen23.cloudapp.net/quyenpro/getdownloadpart.php?part_id=" + listEpisode[0].id);
                List<DownloadPart> listDl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DownloadPart>>(c);
                // string b = await client.GetStringAsync("http://quyen23.cloudapp.net/quyenpro/getmoviebyid.php?film_id=" + m.id);
                listEspidode.ItemsSource = listEpisode;
                infoText.Text = m.storyline;
                nameFilm.Text = m.title.ToUpper();
                //mediaElement.Source = new Uri(listDl[0].url, UriKind.RelativeOrAbsolute);
                //mediaElement.Play();
                MediaElement media = new MediaElement();
                media.AutoPlay = false;
                media.AreTransportControlsEnabled = true;
                media.HorizontalAlignment = HorizontalAlignment.Stretch;
                media.TransportControls = new CustomMediaTransportControls();
                media.TransportControls.IsCompact = false;
                media.Source = new Uri(listDl[0].url, UriKind.RelativeOrAbsolute);
                this.PlayGrid.Children.Clear();
                this.PlayGrid.Children.Add(media);
                this.AddAdControls();
                App.ViewModel.IsLoading = false;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                App.ViewModel.IsLoading = false;
                var dialog = new Windows.UI.Popups.MessageDialog("please select another episode!");
                await dialog.ShowAsync();
                return;
            }
            //
        }
        public void AddAdControls()
        {
            try
            {
                AdControl ads = new AdControl();
                ads.ApplicationId = "0d9b9468-423c-4f5b-8697-43d2f83f766a";
                ads.AdUnitId = "11561408";
                ads.Height = 80;
                ads.Width = 480;
                adGtrid.Children.Clear();
                adGtrid.Children.Add(ads);
            }
            catch
            {

            }
        }
        private void VideoPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if(BackClick!=null)
            {
                BackClick(this, null);
            }
        }

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void qualityButton_Click(object sender, RoutedEventArgs e)
        {
            //var popupMenu = new PopupMenu();
            //foreach(var item in App.ViewModel.movieDetails.DownloadList)
            //{
            //    popupMenu.Commands.Add(new UICommand(item.FormatReslution, command => mediaElement.Source = new Uri(item.VideoUrl, UriKind.RelativeOrAbsolute)));
            //}
            //var button = (Button)sender;
            //var transform = button.TransformToVisual(null);
            //var point = transform.TransformPoint(new Point(button.Width, 0));
            //var ignoreAsyncResult = popupMenu.ShowAsync(point);
        }

        private async void listEspidode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           try
            {
                Episode itemSl = listEspidode.SelectedItem as Episode;
                var client = new HttpClient();
                string a = await client.GetStringAsync("http://quyen23.cloudapp.net/quyenpro/getdownloadpart.php?part_id="+itemSl.id);
                List<DownloadPart> listDl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DownloadPart>>(a);
                MediaElement media = new MediaElement();
                media.AutoPlay = false;
                media.AreTransportControlsEnabled = true;
                media.HorizontalAlignment = HorizontalAlignment.Stretch;
                media.TransportControls = new CustomMediaTransportControls();
                media.TransportControls.IsCompact = false;
                media.Source = new Uri(listDl[0].url, UriKind.RelativeOrAbsolute);
                this.PlayGrid.Children.Clear();
                this.PlayGrid.Children.Add(media);
                string b = await client.GetStringAsync("http://api.adf.ly/api.php?key=eabe4bbeb0cd5b538a2c6dd4e45827f8&uid=7463384&advert_type=int&domain=adf.ly&url=" + listDl[0].url);
                Debug.WriteLine(b);
                if ((InterstitialAdState.Ready) == (MyVideoAd.State))
                {
                    MyVideoAd.Show();
                }
                else
                {
                    if (App.ViewModel.readyPlay)
                    {
                        await App.ViewModel.sdkInstance.PlayAdAsync(new AdConfig { Incentivized = true, SoundEnabled = false });
                    }
                }
            }
            catch { }
        }

        private void RelatedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MoviesItems itemSl = RelatedList.SelectedItem as MoviesItems;
            //this.Init(itemSl);
            //RelatedList.SelectedItem = null;
        }

        private async void commentTbx_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            try
            {
                if (!haveUser && (userTbx.Text == "" || emailTbx.Text == ""))
                {
                    var dialog = new Windows.UI.Popups.MessageDialog("please enter yourname & email!");
                    await dialog.ShowAsync();
                    return;
                }
                else
                {
                    if (!haveUser && userTbx.Text != "" && emailTbx.Text != "")
                    {
                        var applicationSettings1 = Windows.Storage.ApplicationData.Current.RoamingSettings;
                        if (applicationSettings1.Values.ContainsKey("user"))
                            applicationSettings1.Values["user"] = (string)userTbx.Text;
                        else
                            applicationSettings1.Values.Add("user", (string)userTbx.Text);
                        if (applicationSettings1.Values.ContainsKey("email"))
                            applicationSettings1.Values["email"] = (string)emailTbx.Text;
                        else
                            applicationSettings1.Values.Add("email", (string)emailTbx.Text);
                        haveUser = true;
                        userTbx.Visibility = Visibility.Collapsed;
                        emailTbx.Visibility = Visibility.Collapsed;
                    }
                    else { }
                }
                if (e.Key != Windows.System.VirtualKey.Enter)
                    return;
                if (commentTbx.Text == null)
                    return;
                var client = new HttpClient();
                App.ViewModel.IsLoading = true;
                var applicationSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                string user = "";
                string email = "";
                if (applicationSettings.Values.ContainsKey("user"))
                    user = (string)applicationSettings.Values["user"];

                if (applicationSettings.Values.ContainsKey("email"))
                    email = (string)applicationSettings.Values["email"];

                string a = await client.GetStringAsync("http://quyen23.cloudapp.net/quyenpro/postcomment.php?email=" + email + "&user=" + user + "&film_id=" + currentMovie.id + "&comment=" + commentTbx.Text);
                if (a.Contains("1"))
                {
                    Comment usercmt = new Comment();
                    usercmt.id = DateTime.Now.ToString();
                    usercmt.user = user;
                    usercmt.email = email;
                    usercmt.comment = commentTbx.Text;
                    usercmt.film_id = currentMovie.id;
                    listcommentList.Items.Add(usercmt);
                    var selectedIndex = listcommentList.Items.Count - 1;
                    if (selectedIndex < 0)
                        return;

                    listcommentList.SelectedIndex = selectedIndex;
                    listcommentList.UpdateLayout();

                    listcommentList.ScrollIntoView(listcommentList.SelectedItem);
                    commentTbx.Text = "";
                    App.ViewModel.IsLoading = false;
                }
            }
            catch { }
        }

        private async void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!haveUser && (userTbx.Text == "" || emailTbx.Text == ""))
                {
                    var dialog = new Windows.UI.Popups.MessageDialog("please enter yourname & email!");
                    await dialog.ShowAsync();
                    return;
                }
                else
                {
                    if (!haveUser && userTbx.Text != "" && emailTbx.Text != "")
                    {
                        var applicationSettings1 = Windows.Storage.ApplicationData.Current.RoamingSettings;
                        if (applicationSettings1.Values.ContainsKey("user"))
                            applicationSettings1.Values["user"] = (string)userTbx.Text;
                        else
                            applicationSettings1.Values.Add("user", (string)userTbx.Text);
                        if (applicationSettings1.Values.ContainsKey("email"))
                            applicationSettings1.Values["email"] = (string)emailTbx.Text;
                        else
                            applicationSettings1.Values.Add("email", (string)emailTbx.Text);
                        haveUser = true;
                        userTbx.Visibility = Visibility.Collapsed;
                        emailTbx.Visibility = Visibility.Collapsed;
                    }
                    else { }
                }
                if (commentTbx.Text == null)
                    return;
                var client = new HttpClient();
                App.ViewModel.IsLoading = true;
                var applicationSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                string user = "";
                string email = "";
                if (applicationSettings.Values.ContainsKey("user"))
                    user = (string)applicationSettings.Values["user"];

                if (applicationSettings.Values.ContainsKey("email"))
                    email = (string)applicationSettings.Values["email"];

                string a = await client.GetStringAsync("http://quyen23.cloudapp.net/quyenpro/postcomment.php?email=" + email + "&user=" + user + "&film_id=" + currentMovie.id + "&comment=" + commentTbx.Text);
                if (a.Contains("1"))
                {
                    Comment usercmt = new Comment();
                    usercmt.id = DateTime.Now.ToString();
                    usercmt.user = user;
                    usercmt.email = email;
                    usercmt.comment = commentTbx.Text;
                    usercmt.film_id = currentMovie.id;
                    listcommentList.Items.Add(usercmt);
                    listcommentList.Items.Add(usercmt);
                    var selectedIndex = listcommentList.Items.Count - 1;
                    if (selectedIndex < 0)
                        return;

                    listcommentList.SelectedIndex = selectedIndex;
                    listcommentList.UpdateLayout();

                    listcommentList.ScrollIntoView(listcommentList.SelectedItem);
                    commentTbx.Text = "";
                    App.ViewModel.IsLoading = false;
                }
            }
            catch { }
        }
    }
}

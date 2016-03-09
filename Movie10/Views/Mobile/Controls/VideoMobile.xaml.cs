using Movie10.Utils;
using Movie10.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Movie10.Views.Mobile.Controls
{
    public sealed partial class VideoMobile : UserControl
    {
        private bool haveUser = false;
        public EventHandler GenneralPage;
        public MoviesItems currentMovie;
        public VideoMobile()
        {
            this.InitializeComponent();
            var applicationSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (applicationSettings.Values.ContainsKey("user"))
            {
                userTbx.Visibility = Visibility.Collapsed;
                emailTbx.Visibility = Visibility.Collapsed;
                this.haveUser = true;
            }
        }
        public async void Init(MoviesItems m)
        {
            try
            {
                
                if (GenneralPage != null)
                    GenneralPage(this, null);
                listcommentList.Items.Clear();
                currentMovie = m;
                if (App.ViewModel.movieDetails != null)
                    App.ViewModel.movieDetails = (MovieDetail)null;

                if (m == null)
                    return;
                App.ViewModel.IsLoading = true;
                infoText.Text = m.storyline;
                nameFilm.Text = m.title.ToUpper();
                mainPivot.SelectedIndex = 0;
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
                //mediaElement.Source = new Uri(listDl[0].url, UriKind.RelativeOrAbsolute);
                //mediaElement.Play();
                MediaElement media = new MediaElement();
                media.AutoPlay = false;
                media.AreTransportControlsEnabled = true;
                //media.HorizontalAlignment = HorizontalAlignment.Stretch;
                media.TransportControls = new MediaTransportControls();
                media.TransportControls.IsCompact = true;
                media.TransportControls.IsZoomButtonVisible = false;
                media.TransportControls.IsZoomEnabled = false;
                media.TransportControls.IsVolumeButtonVisible = false;
                media.TransportControls.IsVolumeEnabled = false;
                media.TransportControls.BorderBrush = new SolidColorBrush(Colors.Green);
                //media.TransportControls.IsCompact = false;
                media.Source = new Uri(listDl[0].url, UriKind.RelativeOrAbsolute);
                this.PlayGrid.Children.Clear();
                this.PlayGrid.Children.Add(media);
                //this.AddAdControls();
                App.ViewModel.IsLoading = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                App.ViewModel.IsLoading = false;
                var dialog = new Windows.UI.Popups.MessageDialog("please select another episode!");
                await dialog.ShowAsync();
                return;
            }
            //
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
                    listcommentList.Focus(Windows.UI.Xaml.FocusState.Pointer);
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
                    listcommentList.Focus(Windows.UI.Xaml.FocusState.Pointer);
                    App.ViewModel.IsLoading = false;
                }
            }
            catch { }
        }

        private void userTbx_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Enter)
                return;
            emailTbx.Focus(Windows.UI.Xaml.FocusState.Keyboard);
        }

        private void emailTbx_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Enter)
                return;
            commentTbx.Focus(Windows.UI.Xaml.FocusState.Keyboard);
        }

        private async void listEspidode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Episode itemSl = listEspidode.SelectedItem as Episode;
                var client = new HttpClient();
                string a = await client.GetStringAsync("http://quyen23.cloudapp.net/quyenpro/getdownloadpart.php?part_id=" + itemSl.id);
                List<DownloadPart> listDl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DownloadPart>>(a);
                MediaElement media = new MediaElement();
                media.AutoPlay = false;
                media.AreTransportControlsEnabled = true;
                //media.HorizontalAlignment = HorizontalAlignment.Stretch;
                media.TransportControls = new MediaTransportControls();
                media.TransportControls.IsCompact = true;
                media.TransportControls.IsZoomButtonVisible = false;
                media.TransportControls.IsZoomEnabled = false;
                media.TransportControls.IsVolumeButtonVisible = false;
                media.TransportControls.IsVolumeEnabled = false;
                media.TransportControls.BorderBrush = new SolidColorBrush(Colors.Green);
                //media.TransportControls.IsCompact = false;
                media.Source = new Uri(listDl[0].url, UriKind.RelativeOrAbsolute);
                this.PlayGrid.Children.Clear();
                this.PlayGrid.Children.Add(media);
                mainPivot.SelectedIndex = 0;
            }
            catch { }
        }
    }
}

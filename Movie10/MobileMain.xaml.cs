using Movie10.Views.Mobile.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Movie10.ViewModels;
using Windows.UI.Core;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Movie10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MobileMain : Page
    {
        public bool cangoBack = false;
        public CategoryControl category;
        public MovieMobile moviemobile;
        public VideoMobile videoMobile;
        public SearchControl searchControl;
        public YearControl yearControl;
        bool ablePlay = false;
        public int PageNum = 0;
        public string PageUrl = "http://quyen23.cloudapp.net/quyenpro/getmovie.php";
        public MobileMain()
        {
            this.InitializeComponent();
            DataContext = App.ViewModel;
            SystemNavigationManager.GetForCurrentView().BackRequested += VideoPage_BackRequested;
            category = new CategoryControl();
            category.CategoryTap = new EventHandler<ViewModels.MovieCategory>(Genres_Select);
            yearControl = new YearControl();
            yearControl.CategoryTap = new EventHandler<MovieCategory>(Genres_Select);
            moviemobile = new MovieMobile();
            moviemobile.MovieClick = new EventHandler<MoviesItems>(Movie_Click);
            videoMobile = new VideoMobile();
            videoMobile.GenneralPage = new EventHandler(Check_Back);
            searchControl = new SearchControl();
            searchControl.SearchEnter = new EventHandler<string>(Key_Enter);
            searchControl.GenneralPage = new EventHandler(Check_Back);
            //sdkInstance = AdFactory.GetInstance("56547b0ebc3d01137b00000f");
            //sdkInstance.OnAdPlayableChanged += SdkInstance_OnAdPlayableChanged;
        }
        
        private void Check_Back(object sender, EventArgs e)
        {
            try
            {
                this.cangoBack = true;
            }
            catch { }
        }
        private void VideoPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (cangoBack)
            {
                MovieGrid.Children.Clear();
                MovieGrid.Children.Add(moviemobile);
                this.cangoBack = false;
                e.Handled = true;
            }
            else
            {
                //if (this.ablePlay)
                //{
                //    await sdkInstance.PlayAdAsync(new AdConfig { Incentivized = true });
                //    this.ablePlay = false;
                //    e.Handled = true;
                //}
            }
                
        }
        private void Key_Enter(object sender, string e)
        {
            if (e == null)
                return;
            MovieGrid.Children.Clear();
            MovieGrid.Children.Add(moviemobile);
            App.ViewModel.Search(e);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.UI.ViewManagement.StatusBar typebar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await typebar.HideAsync();
            }
            if (!App.ViewModel.IsDataLoaded)
            {
                if (e.NavigationMode == NavigationMode.New)
                {
                    moviemobile.Init(this.PageUrl);
                    MovieGrid.Children.Clear();
                    MovieGrid.Children.Add(moviemobile);
                }
            }
        }
        private void Movie_Click(object sender, MoviesItems e)
        {
            if(e!=null)
            {
                videoMobile.Init(e);
                MovieGrid.Children.Clear();
                MovieGrid.Children.Add(videoMobile);
            }
        }

        private void Genres_Select(object sender, MovieCategory e)
        {
            if(e!=null)
            {
                this.PageNum = 0;
                this.PageUrl = e.Url;
                moviemobile.Init(e.Title.ToUpper());
                this.MovieGrid.Children.Clear();
                this.MovieGrid.Children.Add(moviemobile);
                ScenarioControl.SelectedItem = null;
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

        private void SearchBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

        private void ListBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.MovieGrid.Children.Clear();
            moviemobile.Init("MOVIES");
            this.MovieGrid.Children.Add(moviemobile);
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
            // SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void ListBoxItem_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            this.MovieGrid.Children.Clear();
            moviemobile.Init("SERIES");
            this.MovieGrid.Children.Add(moviemobile);
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

        private void ListBoxItem_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            this.MovieGrid.Children.Clear();
            moviemobile.Init("NEW ADDED");
            this.MovieGrid.Children.Add(moviemobile);
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

        private void ListBoxItem_Tapped_3(object sender, TappedRoutedEventArgs e)
        {
            this.cangoBack = true;
            MovieGrid.Children.Clear();
            MovieGrid.Children.Add(yearControl);
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

        private void ListBoxItem_Tapped_4(object sender, TappedRoutedEventArgs e)
        {
            this.cangoBack = true;
            MovieGrid.Children.Clear();
            MovieGrid.Children.Add(category);
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

        private void ScenarioControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void searchBtx_Click(object sender, RoutedEventArgs e)
        {
            MovieGrid.Children.Clear();
            MovieGrid.Children.Add(searchControl);
            searchControl.Init();
        }

        private async void sharebtn_Click(object sender, RoutedEventArgs e)
        {
                String pfn = Package.Current.Id.FamilyName;
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:SHARE?PFN=" + pfn + ""));
        }

        private async void ratebtn_Click(object sender, RoutedEventArgs e)
        {
            String pfn = Package.Current.Id.FamilyName;
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:REVIEW?PFN=" + pfn + ""));
        }

        private void AdControls_ErrorOccurred(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        {
            Debug.WriteLine(e.ErrorMessage);
            AdControls.Visibility = Visibility.Collapsed;
        }
    }
}

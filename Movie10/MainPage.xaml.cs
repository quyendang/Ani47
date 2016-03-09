using Movie10.ViewModels;
using Movie10.Views.PC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Movie10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public int PageNum = 0;
        public string PageUrl = "http://quyen23.cloudapp.net/quyenpro/getmovie.php";
        public MovieControl movieView;
        public VideoPage videoPage;
        public bool cangoBack = false;
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = App.ViewModel;
            movieView = new MovieControl();
            videoPage = new VideoPage();
            videoPage.BackClick = new EventHandler(Back_Click);
            movieView.MovieClick = new EventHandler<MoviesItems>(MovieClick_Select);
        }

        private void Back_Click(object sender, EventArgs e)
        {
            this.MovieGrid.Children.Clear();
            this.MovieGrid.Children.Add(movieView);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void MovieClick_Select(object sender, MoviesItems e)
        {
            if(e !=null)
            {
                this.MovieGrid.Children.Clear();
                this.videoPage.Init(e);
                this.MovieGrid.Children.Add(videoPage);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(!App.ViewModel.IsDataLoaded)
            {
                if(e.NavigationMode == NavigationMode.New)
                {
                    movieView.Init(this.PageUrl);
                    MovieGrid.Children.Clear();
                    MovieGrid.Children.Add(movieView);
                    App.ViewModel.LoadData();
                }
            }
        }
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MovieCategory itemSl = categoryListView.SelectedItem as MovieCategory;
            this.PageNum = 0;
            this.PageUrl = itemSl.Url;
            movieView.Init(itemSl.Title.ToUpper());
            this.MovieGrid.Children.Clear();
            this.MovieGrid.Children.Add(movieView);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void GridView_Loaded(object sender, RoutedEventArgs e)
        {
            GridView ListBox = sender as GridView;
            if (ListBox == null)
                return;
            ListBox.SelectedItem = null;
            if (ListBox.Tag != null || ListBox.Visibility != Visibility.Visible)
                return;
            ScrollViewer SimpleVisualChild = App.FindSimpleVisualChild<ScrollViewer>(ListBox);
            if (SimpleVisualChild == null)
                return;
            ListBox.Tag = SimpleVisualChild;

            SimpleVisualChild.RegisterPropertyChangedCallback(ScrollViewer.VerticalOffsetProperty, new DependencyPropertyChangedCallback(VerticalOffsetChanged));
        }

        private void VerticalOffsetChanged(DependencyObject sender, DependencyProperty dp)
        {
            ScrollViewer SimpleVisualChild = sender as ScrollViewer;
            if (SimpleVisualChild == null)
                return;
            if (SimpleVisualChild.VerticalOffset < SimpleVisualChild.ScrollableHeight)
                return;
            this.PageNum++;
            
            App.ViewModel.getPhim(this.PageNum, this.PageUrl,"");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void SearchBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key!= Windows.System.VirtualKey.Enter)
                return;
            MovieGrid.Children.Clear();
            MovieGrid.Children.Add(movieView);
            App.ViewModel.Search(SearchBox.Text);
        }

        private void ScenarioControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.MovieGrid.Children.Clear();
            movieView.Init("MOVIES");
            this.MovieGrid.Children.Add(movieView);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void ListBoxItem_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            this.MovieGrid.Children.Clear();
            movieView.Init("SERIES");
            this.MovieGrid.Children.Add(movieView);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void ListBoxItem_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            this.MovieGrid.Children.Clear();
            movieView.Init("NEW ADDED");
            this.MovieGrid.Children.Add(movieView);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void ListBoxItem_Tapped_3(object sender, TappedRoutedEventArgs e)
        {
            //movieView.Init("http://pubfilm.com/tag/new-added");
            //this.MovieGrid.Children.Clear();
            //this.MovieGrid.Children.Add(movieView);
        }

        private void ListBoxItem_Tapped_4(object sender, TappedRoutedEventArgs e)
        {
            this.MovieGrid.Children.Clear();
            movieView.Init("2015");
            this.MovieGrid.Children.Add(movieView);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void ListBoxItem_Tapped_5(object sender, TappedRoutedEventArgs e)
        {
            this.MovieGrid.Children.Clear();
            movieView.Init("2014");
            this.MovieGrid.Children.Add(movieView);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void ListBoxItem_Tapped_6(object sender, TappedRoutedEventArgs e)
        {
            this.MovieGrid.Children.Clear();
            movieView.Init("2013");
            this.MovieGrid.Children.Add(movieView);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void ListBoxItem_Tapped_7(object sender, TappedRoutedEventArgs e)
        {
            this.MovieGrid.Children.Clear();
            movieView.Init("2012");
            this.MovieGrid.Children.Add(movieView);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void ListBoxItem_Tapped_8(object sender, TappedRoutedEventArgs e)
        {
            this.MovieGrid.Children.Clear();
            movieView.Init("2011");
            this.MovieGrid.Children.Add(movieView);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void ListBoxItem_Tapped_9(object sender, TappedRoutedEventArgs e)
        {
            this.MovieGrid.Children.Clear();
            movieView.Init("2010");
            this.MovieGrid.Children.Add(movieView);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void ScenarioControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }
    }
}

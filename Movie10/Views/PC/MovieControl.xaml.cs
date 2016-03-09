using Microsoft.Advertising.WinRT.UI;
using Movie10.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class MovieControl : UserControl
    {
        public MovieControl()
        {
            this.InitializeComponent();
            DataContext = App.ViewModel;
        }
        public string PageUrl;
        public int PageNum = 0;
        public string type = "";
        public EventHandler<MoviesItems> MovieClick;
        public void Init(string url)
        {
            try
            {
                if (url != null)
                {
                    if (url.Contains("http"))
                        this.PageUrl = url;
                    else
                    {
                        this.PageUrl = "http://quyen23.cloudapp.net/quyenpro/getbytype.php";
                        this.type = url;
                    }
                    this.PageNum = 0;
                    App.ViewModel.getPhim(this.PageNum, this.PageUrl, this.type);
                    AddAdControls();
                }
            }
            catch { }
        }
        

        private void VerticalOffsetChanged(DependencyObject sender, DependencyProperty dp)
        {
            ScrollViewer SimpleVisualChild = sender as ScrollViewer;
            if (SimpleVisualChild == null)
                return;
            if (SimpleVisualChild.VerticalOffset < SimpleVisualChild.ScrollableHeight)
                return;
            this.PageNum = this.PageNum + 1;
            //if (PageNum == 5)
            //{
            //    String pfn = Package.Current.Id.FamilyName;
            //    await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:REVIEW?PFN=" + pfn + ""));
            //}
            //else
            //{ }
            App.ViewModel.getPhim(this.PageNum, this.PageUrl, this.type);
        }
        
        private void MovieList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MovieList == null)
                return;
            MoviesItems itemSl = MovieList.SelectedItem as MoviesItems;
            if (MovieClick != null)
                MovieClick(this, itemSl);
        }
        public void AddAdControls()
        {
            try
            {
                AdControl ads = new AdControl();
                ads.ApplicationId = "0d9b9468-423c-4f5b-8697-43d2f83f766a";
                ads.AdUnitId = "11561407";
                ads.ErrorOccurred += Ads_ErrorOccurred;
                ads.VerticalAlignment = VerticalAlignment.Bottom;
                ads.Height = 100;
                ads.Width = 640;
                AdsGrid.Children.Clear();
                AdsGrid.Children.Add(ads);
            }
            catch
            {

            }
        }

        private void Ads_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            Debug.WriteLine(e.ErrorMessage);
        }

        private void MovieList_Loaded(object sender, RoutedEventArgs e)
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
    }
}

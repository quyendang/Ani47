using Movie10.ViewModels;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Movie10.Views.Mobile.Controls
{
    public sealed partial class YearControl : UserControl
    {
        public EventHandler<MovieCategory> CategoryTap;
        public EventHandler GenneralPage;
        public YearControl()
        {
            this.InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void categoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itemSelect = categoryList.SelectedItem as MovieCategory;
            if (CategoryTap != null)
                CategoryTap(this, itemSelect);
        }
    }
}

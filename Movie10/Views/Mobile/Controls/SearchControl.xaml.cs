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
    public sealed partial class SearchControl : UserControl
    {
        public EventHandler<string> SearchEnter;
        public EventHandler GenneralPage;
        public SearchControl()
        {
            this.InitializeComponent();
        }
        public void Init()
        {
            try
            {
                searchBox.Text = "";
                searchBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
                if (GenneralPage != null)
                    GenneralPage(this, null);
            }
            catch { }
        }
        private void searchBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Enter)
                return; 
            if (SearchEnter != null)
                SearchEnter(this, searchBox.Text);
        }
    }
}

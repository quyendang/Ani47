﻿#pragma checksum "D:\Project\Movie10\Movie10\Views\Mobile\Controls\MovieMobile.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "85EC76BB533F959B66890B9C850E5863"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Movie10.Views.Mobile.Controls
{
    partial class MovieMobile : 
        global::Windows.UI.Xaml.Controls.UserControl, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.MovieList = (global::Windows.UI.Xaml.Controls.GridView)(target);
                    #line 531 "..\..\..\..\..\Views\Mobile\Controls\MovieMobile.xaml"
                    ((global::Windows.UI.Xaml.Controls.GridView)this.MovieList).SelectionChanged += this.MovieList_SelectionChanged;
                    #line 531 "..\..\..\..\..\Views\Mobile\Controls\MovieMobile.xaml"
                    ((global::Windows.UI.Xaml.Controls.GridView)this.MovieList).Loaded += this.MovieList_Loaded;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}


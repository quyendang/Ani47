﻿#pragma checksum "D:\Project\Movie10\Movie10\Views\Mobile\Controls\VideoMobile.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "017617ADF8F8A453779F20E1C08797AC"
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
    partial class VideoMobile : 
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
                    this.mainPivot = (global::Windows.UI.Xaml.Controls.Pivot)(target);
                }
                break;
            case 2:
                {
                    this.cmtGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3:
                {
                    this.listcommentList = (global::Windows.UI.Xaml.Controls.ListView)(target);
                }
                break;
            case 4:
                {
                    this.userTbx = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 2136 "..\..\..\..\..\Views\Mobile\Controls\VideoMobile.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.userTbx).KeyUp += this.userTbx_KeyUp;
                    #line default
                }
                break;
            case 5:
                {
                    this.emailTbx = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 2137 "..\..\..\..\..\Views\Mobile\Controls\VideoMobile.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.emailTbx).KeyUp += this.emailTbx_KeyUp;
                    #line default
                }
                break;
            case 6:
                {
                    this.cmtsGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 7:
                {
                    this.commentTbx = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 2143 "..\..\..\..\..\Views\Mobile\Controls\VideoMobile.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.commentTbx).KeyUp += this.commentTbx_KeyUp;
                    #line default
                }
                break;
            case 8:
                {
                    this.sendBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 2144 "..\..\..\..\..\Views\Mobile\Controls\VideoMobile.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.sendBtn).Click += this.sendBtn_Click;
                    #line default
                }
                break;
            case 9:
                {
                    this.listEspidode = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    #line 2088 "..\..\..\..\..\Views\Mobile\Controls\VideoMobile.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListView)this.listEspidode).SelectionChanged += this.listEspidode_SelectionChanged;
                    #line default
                }
                break;
            case 10:
                {
                    this.mainGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 11:
                {
                    this.PlayGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 12:
                {
                    this.infoGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 13:
                {
                    this.nameFilm = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 14:
                {
                    this.infoText = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
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


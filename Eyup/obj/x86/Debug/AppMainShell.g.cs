﻿#pragma checksum "C:\Projects\Eyup\Eyup\AppMainShell.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6BC6547045622B31D6C00C8D7B1FA845"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Eyup
{
    partial class AppMainShell : 
        global::Windows.UI.Xaml.Controls.Page, 
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
                    this.ContactsPage = (global::Eyup.Views.ContactsPage)(target);
                }
                break;
            case 2:
                {
                    this.AppMainShellFrame = (global::Windows.UI.Xaml.Controls.Frame)(target);
                }
                break;
            case 3:
                {
                    this.ContactPickerPopup = (global::Windows.UI.Xaml.Controls.Primitives.Popup)(target);
                    #line 26 "..\..\..\AppMainShell.xaml"
                    ((global::Windows.UI.Xaml.Controls.Primitives.Popup)this.ContactPickerPopup).Loaded += this.ContactPickerPopup_Loaded;
                    #line default
                }
                break;
            case 4:
                {
                    this.ContactPicker = (global::Eyup.Views.ContactPicker)(target);
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

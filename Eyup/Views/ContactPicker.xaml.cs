using Eyup.Helpers;
using Eyup.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Eyup.Views
{

    public sealed partial class ContactPicker : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<AppContactEventArgs> AppContactPicked;

        public ContactPicker()
        {
            this.InitializeComponent();
            Loaded += ContactPicker_Loaded;
        }

        private void ContactPicker_Loaded(object sender, RoutedEventArgs e)
        {
            AppContacts = App.AppContacts;
        }

        private ObservableCollection<AppContact> appContacts;

        public ObservableCollection<AppContact> AppContacts
        {
            get { return appContacts; }
            set
            {
                appContacts = value;
                OnPropertyChanged("AppContacts");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AppContactsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppContactPicked?.Invoke(this, new AppContactEventArgs(AppContactsGridView.SelectedItem as AppContact));
        }
    }
}

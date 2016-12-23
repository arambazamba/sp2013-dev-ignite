﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.SharePoint.Client;
using System.Device.Location;
using Microsoft.Phone.Tasks;
using Microsoft.SharePoint.Phone.Application;

namespace GeoApp
{
    /// <summary>
    /// ListItem Display Form
    /// </summary>
    public partial class DisplayForm : PhoneApplicationPage
    {
        DisplayItemViewModel viewModel;

        BingMapsTask map = new BingMapsTask();

        /// <summary>
        /// Constructor for Display Form
        /// </summary>
        public DisplayForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Code to execute when app navigates to Display Form
        /// </summary>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            viewModel = App.MainViewModel.SelectedItemDisplayViewModelInstance;
            viewModel.InitializationCompleted += new EventHandler<InitializationCompletedEventArgs>(OnViewModelInitialization);
            if (!viewModel.IsInitialized)
                viewModel.Initialize();

            this.DataContext = viewModel;
        }

        /// <summary>
        /// Code to execute when app navigates away from Display Form
        /// </summary>
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            viewModel.InitializationCompleted -= new EventHandler<InitializationCompletedEventArgs>(OnViewModelInitialization);
        }

        /// <summary>
        /// Code to execute when ViewModel initialization completes
        /// </summary>
        private void OnViewModelInitialization(object sender, InitializationCompletedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                //If initialization has failed show error message and return
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message, e.Error.GetType().Name, MessageBoxButton.OK);
                }
            });
        }

        /// <summary>
        /// Code to execute when user click on cancel button on the Page
        /// </summary>
        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            //Navigate to the ListView Page
            NavigationService.Navigate(new Uri("/Views/List.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Code to execute when user click on location hyperlink to see location on Bing Map
        /// </summary>
        private void OnLocationClick(object sender, RoutedEventArgs e)
        {
            GeoCoordinate coordinate = (sender as HyperlinkButton).Tag as GeoCoordinate;
            if (coordinate == null)
                return;

            map.Center = coordinate;
            map.SearchTerm = string.Format("maps: {0},{1}", coordinate.Latitude, coordinate.Longitude);
            map.Show();
        }


    }
}


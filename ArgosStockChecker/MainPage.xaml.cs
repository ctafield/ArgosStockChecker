using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Windows.Devices.Geolocation;
using Windows.System;
using ArgosApi;
using ArgosApi.Types;
using ArgosStockChecker.Classes;
using ArgosStockChecker.Interfaces;
using ArgosStockChecker.UserControls;
using ArgosStockChecker.ViewModels;
using Clarity.Phone.Extensions;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Telerik.Windows.Controls;

namespace ArgosStockChecker
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            InteractionEffectManager.AllowedTypes.Add(typeof(RadDataBoundListBoxItem));

            //Shows the rate reminder message, according to the settings of the RateReminder.
            (App.Current as App).rateReminder.Notify();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();

                RefreshLocation();

            }        
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            RefreshRecentlyViewed();

        }

        private void RefreshRecentlyViewed()
        {
            var rh = new RecentHelper();
            var items = rh.GetRecentItems();
            lstRecentItems.DataContext = items;

        }

        private void panoramaChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void RefreshLocation()
        {
           
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 100;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromSeconds(60),
                    timeout: TimeSpan.FromSeconds(10)
                    );

                var latitude = geoposition.Coordinate.Latitude;
                var longitude = geoposition.Coordinate.Longitude;

                var api = new ArgosAPI();
                var result = await api.GetStores(longitude, latitude);

                radListStores.DataContext = result;

                var settingsHelper = new SettingsHelper();
                settingsHelper.SaveStores(result);

                var currentApp = ((IArgosApp) (App.Current));

                currentApp.StoresChanged();

            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // todo: say blocked
                    stackNoLocation.Visibility = Visibility.Visible;
                    radListStores.Visibility = Visibility.Collapsed;
                }
                else
                {
                    // something else happened acquring the location
                }
            }

        }

        private void txtSearch_IconTapped(object sender, EventArgs e)
        {
            DoSearch();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoSearch();
            }
        }

        private void DoSearch()
        {
            var searchQuery = txtSearchText.Text.Trim();

            if (Regex.Match(searchQuery, @"^\d{3}/?\d{4}$").Success)
            {
                NavigationService.Navigate(new Uri("/ProductDetails.xaml?id=" + searchQuery, UriKind.Relative));    
            }
            else
            {
                NavigationService.Navigate(new Uri("/SearchResults.xaml?query=" + searchQuery, UriKind.Relative));    
            }                       
        }
       
        private void lstRecent_Tap(object sender, ListBoxItemTapEventArgs e)
        {
            var item = lstRecentItems.SelectedItem as ProductViewModel;
            if (item == null)
                return;

            NavigationService.Navigate(new Uri("/ProductDetails.xaml?id=" + item.Id, UriKind.Relative));    

        }

        private void radListStores_Tap(object sender, ListBoxItemTapEventArgs e)
        {
            var storeInfo = radListStores.SelectedItem as StoreInfo;
            if (storeInfo == null)
                return;

            var dh = new DialogService();

            var storeDetails = new StoreDetails();
            storeDetails.SetInfo( storeInfo);
            dh.Child = storeDetails;

            dh.AnimationType = DialogService.AnimationTypes.Fade;

            dh.Show();

        }

        private void btnClearRecent_Click(object sender, RoutedEventArgs e)
        {
            var rh = new RecentHelper();
            rh.ClearRecentItems();

            RefreshRecentlyViewed();
        }

        private async void btnAppChallenge_Click(object sender, RoutedEventArgs e)
        {
            // aa8bb968-4381-40d3-a215-ca447d172f6e
            await Launcher.LaunchUriAsync(new Uri("appchallengeuk:appGuid=aa8bb968-4381-40d3-a215-ca447d172f6e", UriKind.Absolute)); //(without the brackets)
        }

        private void btnPrivacy_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Privacy.xaml", UriKind.Relative));

        }

        private void btnRate_Click(object sender, RoutedEventArgs e)
        {
            var task = new MarketplaceReviewTask();
            task.Show();
        }
    }

}

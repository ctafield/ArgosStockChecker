using System.Windows;
using System.Windows.Controls;
using ArgosApi.Types;
using Microsoft.Phone.Tasks;

namespace ArgosStockChecker.UserControls
{
    public partial class StoreDetails : UserControl
    {

        public StoreInfo ViewModel { get; set; }

        public StoreDetails()
        {
            InitializeComponent();
            Loaded += StoreDetails_Loaded;
        }

        void StoreDetails_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "40c0a99f-bc8e-43a0-9190-c37f80941c20";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "Q1jX6aRJzmFUT7hRHyKwBw";

        }

        public void SetInfo(StoreInfo value)
        {
            ViewModel = value.Clone();
            this.DataContext = ViewModel;
        }
        
        private void btnNavigate_Click(object sender, RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(
                new System.Uri(
                    string.Format(
                        "ms-drive-to:?destination.latitude={0}&destination.longitude={1}&destination.name={2}",
                        ViewModel.Latitude, ViewModel.Longitude, "Argos")));
        }

        private void btnNavigateWalk_Click(object sender, RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(
                new System.Uri(
                    string.Format(
                        "ms-walk-to:?destination.latitude={0}&destination.longitude={1}&destination.name={2}",
                        ViewModel.Latitude, ViewModel.Longitude, "Argos")));
        }

        private void btnPhone_Click(object sender, RoutedEventArgs e)
        {
            var phoneCallTask = new PhoneCallTask
            {
                PhoneNumber = ViewModel.Telephone,
                DisplayName = "Argos " + ViewModel.Name
            };

            phoneCallTask.Show();
        }
    }
}

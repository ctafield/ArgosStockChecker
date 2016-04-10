using System;
using System.Linq;
using System.ServiceModel.Channels;
using System.Windows;
using ArgosApi;
using ArgosStockChecker.Classes;
using ArgosStockChecker.Interfaces;
using ArgosStockChecker.UserControls;
using ArgosStockChecker.ViewModels;
using Clarity.Phone.Extensions;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Telerik.Windows.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace ArgosStockChecker
{
    public partial class ProductDetails : PhoneApplicationPage
    {

        private string ProductId { get; set; }
        public ProductViewModel ViewModel { get; set; }

        public ProductDetails()
        {
            InitializeComponent();
            ViewModel = new ProductViewModel();

            Loaded += ProductDetails_Loaded;
        }

        void ProductDetails_Loaded(object sender, RoutedEventArgs e)
        {
            ProductId = NavigationContext.QueryString["id"];

            DataContext = ViewModel;
            RefreshStoreData();
            RefreshProductData();
            RefreshReviews();
        }

        private async void RefreshReviews()
        {
            var api = new ArgosAPI();
            var result = await api.GetReviews(ProductId);


            if (result == null || result.Results == null || result.Results.Count == 0)
            {
                lstReviews.Visibility = Visibility.Collapsed;
                return;
            }

            var index = 0;

            foreach (var x in result.Results)
            {
                var model = new ReviewViewModel()
                {
                    Text = x.ReviewText,
                    RatingNumeric = x.Rating,
                    Title = x.Title,
                    UserName = x.UserNickname,
                    UserLocation = x.UserLocation,
                    ReviewDate = x.SubmissionTime,
                    IsRecommended = x.IsRecommended,
                    Index = index++
                };

                ViewModel.Reviews.Add(model);
            }

        }

        private async void RefreshProductData()
        {

            try
            {
                var api = new ArgosAPI();
                var result = await api.GetProductDetails(ProductId);

                if (result == null)
                {
                    MessageBox.Show("Unable to find product with the id " + ProductId, "error finding product", MessageBoxButton.OK);
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();
                    return;
                }

                ViewModel.Description = result.Description;
                ViewModel.Title = result.Title;
                ViewModel.PreviewImageUrl = result.PreviewImageUrl;
                ViewModel.Id = result.Id;
                ViewModel.PriceFormatted = result.PriceFormatted;
                ViewModel.WasPriceFormatted = result.WasPriceFormatted;
                ViewModel.Promotions = result.Promotions;
                
                var image = result.Images.FirstOrDefault(x => x.Type == "largeImage");
                if (image != null)
                    ViewModel.LargeImageUrl = image.Url;

                if (result.Images.Any(x => x.Type == "video"))
                    ViewModel.Videos = result.Images.Where(x => x.Type == "video" && x.Application=="video/mp4").Select(x =>
                    new VideoThumbnail()
                    {
                        Thumbnail = image.Url,
                        Url = x.Url
                    }).ToList();

                if (result.Images.Any(x => x.Type == "image"))
                    ViewModel.ProductImages = result.Images.Where(x => x.Type == "image").Select(x => x.Url).ToList();

                var rh = new RecentHelper();
                rh.AddRecentItem(ViewModel);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }

        private async void RefreshStoreData()
        {
            var settingsHelper = new SettingsHelper();
            var stores = settingsHelper.GetStores();

            if (stores == null)
            {
                ((IArgosApp)(App.Current)).StoresChangedEvent += (sender, args) => RefreshStoreData();
                return;
            }

            var api = new ArgosAPI();

            var index = 0;

            foreach (var store in stores)
            {
                var result = await api.CheckProductStock(store.Id, ProductId);
                var stockInfo = new StockInfo(store, result);
                stockInfo.Index = index++;
                ViewModel.StockInfo.Add(stockInfo);
            }
        }

        private void radListStores_Tap(object sender, ListBoxItemTapEventArgs e)
        {
            var storeInfo = listStockInfo.SelectedItem as StockInfo;
            if (storeInfo == null)
                return;

            var dh = new DialogService();

            var storeDetails = new StoreDetails();
            storeDetails.SetInfo(storeInfo);
            dh.Child = storeDetails;

            dh.AnimationType = DialogService.AnimationTypes.Fade;

            dh.Show();
        }

        private void btnPromotions_Click(object sender, RoutedEventArgs e)
        {
            var dh = new DialogService();

            var promotions = new Promotions();
            promotions.SetInfo(ViewModel.Promotions);
            dh.Child = promotions;

            dh.AnimationType = DialogService.AnimationTypes.Fade;

            dh.Show();
        }

        private void btnVideo_Tap(object sender, GestureEventArgs e)
        {
            var item = radVideos.SelectedItem as VideoThumbnail;
            if (item == null)
                return;

            var task = new MediaPlayerLauncher
            {
                Controls = MediaPlaybackControls.All,
                Location = MediaLocationType.Data,
                Media = new Uri(item.Url, UriKind.RelativeOrAbsolute)
            };

            task.Show();

        }
    }

}
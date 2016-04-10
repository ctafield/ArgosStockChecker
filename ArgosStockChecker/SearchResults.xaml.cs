using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ArgosApi;
using ArgosStockChecker.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Wallet;
using Telerik.Windows.Controls;

namespace ArgosStockChecker
{
    public partial class SearchResults : PhoneApplicationPage
    {

        public SearchResultsViewModel ViewModel { get; private set;  }

        public SearchResults()
        {
            InitializeComponent();
            ViewModel = new SearchResultsViewModel();
            Loaded += SearchResults_Loaded;
        }

        void SearchResults_Loaded(object sender, RoutedEventArgs e)
        {

            if (ViewModel.Results != null && ViewModel.Results.Count > 0)
                return;

            DataContext = ViewModel;

            ViewModel.Query = NavigationContext.QueryString["query"];

            RefreshData();
        }

        private async void RefreshData()
        {
            var api = new ArgosAPI();
            var results = await api.Search(ViewModel.Query);

            if (results == null || results.Count == 0)
            {
                stackNoResults.Visibility = Visibility.Visible;
                lstResults.Visibility = Visibility.Collapsed;
                return;
            }

            stackNoResults.Visibility = Visibility.Collapsed;
            lstResults.Visibility = Visibility.Visible;

            var index = 0;

            foreach (var result in results)
            {
                var model = new SearchResultViewModel(result)
                {
                    Index = index ++
                };

                ViewModel.Results.Add(model);
            }
        }

        private void searchItem_Tap(object sender, ListBoxItemTapEventArgs e)
        {
            var list = sender as RadDataBoundListBox;
            if (list == null)
                return;

            var item = list.SelectedItem as SearchResultViewModel;
            if (item == null)
                return;

            NavigationService.Navigate(new Uri("/ProductDetails.xaml?id=" + item.Id, UriKind.Relative));
           
        }
    }
}
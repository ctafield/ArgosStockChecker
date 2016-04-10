using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace ArgosStockChecker
{
    public partial class Privacy : PhoneApplicationPage
    {
        public Privacy()
        {
            InitializeComponent();
        }

        private void btnPrivacyPolicy_Click(object sender, RoutedEventArgs e)
        {
            const string argosUrl = "http://www.argos.co.uk/static/StaticDisplay/includeName/privacyPolicy.htm";
            var browserTask = new WebBrowserTask();
            browserTask.Uri = new Uri(argosUrl, UriKind.Absolute);
            browserTask.Show();
        }

    }
}
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using ArgosApi.Types;

namespace ArgosStockChecker.ViewModels
{
    public class SearchResultViewModel : SearchResult
    {
        public int Index
        {
            get;
            set;
        }

        public SolidColorBrush BackgroundColour
        {
            get
            {
                if (Index % 2 == 0)
                    return new SolidColorBrush(XnaToColour(Microsoft.Xna.Framework.Color.PowderBlue));
                else
                    return new SolidColorBrush(Colors.Transparent);
            }
        }

        private Color XnaToColour(Microsoft.Xna.Framework.Color xnaColour)
        {
            return new Color()
            {
                A = 70,
                R = xnaColour.R,
                B = xnaColour.B,
                G = xnaColour.G
            };
        }

        public string ThumbnailUrl
        {
            get { return string.Format("http://argos.scene7.com/is/image/Argos/{0}_R_SET?wid=280&hei=280 ", Id); }
        }

        public string FormattedPrice
        {
            get { return CurrenySymbol  + Price; }
        }

        public string CurrenySymbol
        {
            get { return "£"; }
             
        }

        public Visibility RatingVisibility
        {
            get { return string.IsNullOrEmpty(AverageRating) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public double RatingNumeric
        {
            get
            {
                double rating;
                if (double.TryParse(AverageRating, out rating))
                    return rating;
                return 0;               
            }
        }

        public Visibility SpecialOffersVisibility
        {
            get { return string.IsNullOrEmpty(SpecialOffers) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public SearchResultViewModel(SearchResult result)
        {
            this.AverageRating = result.AverageRating;
            this.Id = result.Id;
            this.Image1 = result.Image1;
            this.Image2 = result.Image2;
            this.Price = result.Price;
            this.PricePreviously = result.PricePreviously;
            this.Reviews = result.Reviews;
            this.Title = result.Title;
            this.SpecialOffers = result.SpecialOffers;
        }

    }

}

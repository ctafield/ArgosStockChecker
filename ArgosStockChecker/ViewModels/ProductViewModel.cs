using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows;
using ArgosStockChecker.Annotations;
using ArgosStockChecker.Classes;
using Microsoft.Phone.Controls;

namespace ArgosStockChecker.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private string _title;
        private string _description;
        private string _previewImageUrl;
        private string _id;
        private string _price;
        private string _largeImageUrl;
        private string _wasPriceFormatted;
        private string _priceFormatted;
        private List<string> _promotions;
        private List<string> _productImages;
        private List<VideoThumbnail> _videos;

        private ObservableCollection<ReviewViewModel> _reviews;

        [IgnoreDataMember]
        public ObservableCollection<StockInfo> StockInfo { get; set; }

        public string Title
        {
            get
            {
                return RemoveTags(_title);
            }
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        private string RemoveTags(string title)
        {
            if (string.IsNullOrEmpty(title))
                return title;

            return title.Replace("&reg;", "®");
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                OnPropertyChanged();
            }
        }


        public string PreviewImageUrl
        {
            get { return _previewImageUrl; }
            set
            {
                if (value == _previewImageUrl) return;
                _previewImageUrl = value;
                OnPropertyChanged();
            }
        }

        public string Id
        {
            get
            {
                if (!string.IsNullOrEmpty(_id) && !_id.Contains(@"/"))
                    return _id.Substring(0, 3) + "/" + _id.Substring(3);
                else if (!string.IsNullOrEmpty(_id))
                    return _id;
                return null;
            }
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Price
        {
            get { return _price; }
            set
            {
                if (value == _price) return;
                _price = value;
                OnPropertyChanged();
            }
        }

        public string LargeImageUrl
        {
            get { return _largeImageUrl; }
            set
            {
                if (value == _largeImageUrl) return;
                _largeImageUrl = value;
                OnPropertyChanged();
            }
        }

        public List<string> ProductImages
        {
            get { return _productImages; }
            set
            {
                if (Equals(value, _productImages)) return;
                _productImages = value;
                OnPropertyChanged();
            }
        }

        public string WasPriceFormatted
        {
            get { return _wasPriceFormatted; }
            set
            {
                if (value == _wasPriceFormatted) return;
                _wasPriceFormatted = value;
                OnPropertyChanged();
            }
        }

        public string PriceFormatted
        {
            get { return _priceFormatted; }
            set
            {
                if (value == _priceFormatted) return;
                _priceFormatted = value;
                OnPropertyChanged();
            }
        }

        public Visibility PromotionsVisibility
        {
            get { return (Promotions == null || Promotions.Count == 0) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public List<string> Promotions
        {
            get { return _promotions; }
            set
            {
                if (Equals(value, _promotions)) return;
                _promotions = value;
                OnPropertyChanged();
                OnPropertyChanged("PromotionsVisibility");
            }
        }

        public List<VideoThumbnail> Videos
        {
            get { return _videos; }
            set
            {
                if (Equals(value, _videos)) return;
                _videos = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ReviewViewModel> Reviews
        {
            get { return _reviews; }
            set
            {
                if (Equals(value, _reviews)) return;
                _reviews = value;
                OnPropertyChanged();
            }
        }

        public ProductViewModel()
        {
            StockInfo = new ObservableCollection<StockInfo>();
            Reviews = new ObservableCollection<ReviewViewModel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

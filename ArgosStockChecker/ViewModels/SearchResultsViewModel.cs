using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ArgosStockChecker.Annotations;

namespace ArgosStockChecker.ViewModels
{
    public class SearchResultsViewModel : INotifyPropertyChanged
    {

        public SearchResultsViewModel()
        {
            Results = new ObservableCollection<SearchResultViewModel>();
        }

        private string _query;

        public ObservableCollection<SearchResultViewModel> Results { get; set; } 

        public string Query
        {
            get { return _query; }
            set
            {
                if (value == _query) return;
                _query = value;
                OnPropertyChanged();
            }
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

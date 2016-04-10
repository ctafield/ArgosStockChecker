using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgosApi.Types
{
    public class SearchResult
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string AverageRating { get; set; }
        public string Reviews { get; set; }
        public string Price { get; set; }
        public string PricePreviously { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string SpecialOffers { get; set; }
    }
}

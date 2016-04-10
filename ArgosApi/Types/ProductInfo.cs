using System;
using System.Collections.Generic;
using System.Linq;

namespace ArgosApi.Types
{
    public class ProductInfo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ProductImage> Images { get; set; }

        public string PreviewImageUrl
        {
            get
            {
                var image = Images.FirstOrDefault(x => x.Type == "thumbnail");
                if (image != null)
                    return image.Url;
                return null;
            }
        }

        public string Price { get; set; }

        public string PriceFormatted
        {
            get
            {
                if (String.IsNullOrEmpty(Price))
                    return string.Empty;

                return "£" + Price;
            }
        }

        public string WasPrice { get; set; }

        public string WasPriceFormatted
        {
            get
            {
                if (String.IsNullOrEmpty(WasPrice))
                    return string.Empty;

                return "Was " + "£" + WasPrice;
            }
        }

        public List<string> Promotions { get; set; }
    }
}

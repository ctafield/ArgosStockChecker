using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using ArgosApi.Types;
using ArgosApi.Types.Reviews;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ArgosApi
{
    public class ArgosAPI
    {

        private HttpClient Client { get; set; }

        private const string ApiArgosRoot = "http://api.argos.co.uk";
        private const string ApiHomeRetailGroupRoot = "http://api.homeretailgroup.com";

        private const string iPadApiKey = "uk4tbngzceyxpwwvfcbtkvkj";
        private const string ApiKey = "apiKey=" + iPadApiKey;


        // todo: move into settings
        public string Scale
        {
            get { return "miles"; }
        }

        // todo: move into settings
        public string MaxStoreResults
        {
            get { return "5"; }
        }


        public ArgosAPI()
        {
            Client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip });
            Client.DefaultRequestHeaders.Add("User-Agent", "ArgosiPadApp/169 CFNetwork/672.1.13 Darwin/14.0.0");
        }

        private string FormatProductCode(string productId)
        {
            if (productId.Contains(@"/"))
                return productId.Replace(@"/", "");
            return productId;
        }

        /// <summary>
        /// Retrieves the closest stores to the given location.
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public async Task<List<StoreInfo>> GetStores(double latitude, double longitude)
        {
            var path = string.Format("/location/argos/store?origin={0},{1}&maxResults={2}&scale={3}&{4}", longitude.ToString(CultureInfo.InvariantCulture), 
                                                                                                          latitude.ToString(CultureInfo.InvariantCulture), 
                                                                                                          MaxStoreResults,
                                                                                                          Scale,
                                                                                                          ApiKey);

            var response = await Client.GetAsync(ApiHomeRetailGroupRoot + path);

            var content = await response.Content.ReadAsStringAsync();

            XDocument document = XDocument.Parse(content);

            XNamespace loc = "http://schemas.homeretailgroup.com/location";
            XNamespace cmn = "http://schemas.homeretailgroup.com/common";

            try
            {
                var stores = document.Element(loc + "Location").Elements(loc + "Store")
                                            .Select(x => new StoreInfo()
                                            {
                                                Name = GetElementValue(x.Element(loc + "Name")),
                                                Id = GetAttributeValue(x.Attribute("id")),
                                                AddressLine1 = x.Element(loc + "Address").Element(cmn + "Line1").Value,
                                                AddressLine2 = x.Element(loc + "Address").Element(cmn + "Line2").Value,
                                                AddressLine3 = x.Element(loc + "Address").Element(cmn + "Line3").Value,
                                                PostCode = x.Element(loc + "Address").Element(cmn + "PostCode").Value,
                                                Telephone = GetTelephone(x.Element(loc + "ContactDetails")),
                                                Town = x.Element(loc + "Address").Element(cmn + "Town").Value,
                                                County = x.Element(loc + "Address").Element(cmn + "County").Value,
                                                Latitude = x.Element(loc + "Position").Element(loc + "GeoLocation").Attribute("latitude").Value,
                                                Longitude = x.Element(loc + "Position").Element(loc + "GeoLocation").Attribute("longitude").Value,
                                                Distance = (FormatDistance(GetElementValue(x.Element(loc + "DistanceFromOrigin"))) + " " + x.Element(loc + "DistanceFromOrigin").Attribute("units").Value).Trim()
                                            })
                                            .ToList();


                return stores;

            }
            catch (Exception)
            {

                
            }

            return null;

        }


        private string FormatDistance(string s)
        {
            if (string.IsNullOrEmpty(s) || !s.Contains("."))
                return s;

            return s.Substring(0, s.IndexOf('.') + 2);

        }

        private string GetTelephone(XElement xName)
        {
            if (xName == null)
                return string.Empty;

            XNamespace cmn = "http://schemas.homeretailgroup.com/common";

            var teleNode = xName.Element(cmn + "Telephone");
            if (teleNode == null)
                return string.Empty;

            return teleNode.Value;

        }

        private string GetAttributeValue(XAttribute element, string defaultValue = null)
        {
            if (element != null)
                return element.Value;
            return defaultValue;
        }

        private string GetElementValue(XElement element, string defaultValue = null)
        {
            if (element != null)
                return element.Value;
            return defaultValue;
        }

        /// <summary>
        /// Checks the stock of a product at a given store
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductStock> CheckProductStock(string storeId, string productId)
        {
            var path = string.Format("/stock/{0}/{1}?{2}", storeId, FormatProductCode(productId), ApiKey);

            var response = await Client.GetAsync(ApiArgosRoot + path);

            var content = await response.Content.ReadAsStringAsync();

            XDocument document = XDocument.Parse(content);

            var productInfo = document.Elements("product")
                                        .Select(x => new ProductStock()
                                        {
                                            Available = ResolveAvailabilityBoolean(GetElementValue(x.Element("availabilityStatus"))),
                                            Balance = GetElementValue(x.Element("balance")),
                                            StoreNumber = GetElementValue(x.Element("storeNumber")),
                                            CollectUntil = GetElementValue(x.Element("latestInStoreCollectionDate"))
                                        })
                                        .ToList().FirstOrDefault();

            return productInfo;
        }

        private bool ResolveAvailabilityBoolean(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (string.Compare(value, "available", StringComparison.InvariantCultureIgnoreCase) == 0)
                return true;

            return false;
        }


        /// <summary>
        /// Searches for a product.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<SearchResult>> Search(string query)
        {
            var path = string.Format("/webex/search?searchText" +
                                     "={0}&itemsPerPage=100&{1}", query, ApiKey);

            var response = await Client.GetAsync(ApiArgosRoot + path);

            var content = await response.Content.ReadAsStringAsync();

            try
            {
                XDocument document = XDocument.Parse(content);

                var searchResults = document.Element("productbrowses").Elements("productbrowse")
                                .Select(x => new SearchResult()
                                {
                                    Id = GetElementValue(x.Element("productReference")),
                                    Price = GetElementValue(x.Element("sellingPrice")),
                                    PricePreviously = GetElementValue(x.Element("wasPrice")),
                                    Title = GetElementValue(x.Element("productName")),
                                    AverageRating = GetRating(x.Element("productRating")),
                                    Reviews = GetReviews(x.Element("productRating")),
                                    Image1 = GetElementValue(x.Element("imageName1")),
                                    Image2 = GetElementValue(x.Element("imageName2")),
                                    SpecialOffers = GetSpecialOffers(x)
                                })
                                .ToList();

                return searchResults;
            }
            catch (Exception)
            {

                return null;
            }


        }

        private string GetSpecialOffers(XElement element)
        {

            if (element == null)
                return string.Empty;

            var offerElement = element.Element("specialOfferCount");
            if (offerElement == null)
                return string.Empty;

            var count = offerElement.Value;

            if (count == "0")
                return string.Empty;

            var offer = element.Element("specialOfferDescription");
            if (offer == null)
                return string.Empty;

            return offer.Value;
        }

        private string GetRating(XElement element)
        {
            if (element == null)
                return string.Empty;

            return GetElementValue(element.Element("averageRating"));
        }

        private string GetReviews(XElement element)
        {
            if (element == null)
                return string.Empty;

            return GetElementValue(element.Element("numberReviews"));
        }

        /// <summary>
        /// Gets the details for a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductInfo> GetProductDetails(string productId)
        {
            try
            {
                var path = string.Format("/product/argos/{0}?{1}", FormatProductCode(productId), ApiKey);

                var response = await Client.GetAsync(ApiHomeRetailGroupRoot + path);

                var content = await response.Content.ReadAsStringAsync();

                XDocument document = XDocument.Parse(content);

                XNamespace prd = "http://schemas.homeretailgroup.com/product";

                var productInfo = document.Elements(prd + "Product")
                    .Select(x => new ProductInfo()
                    {
                        Id = GetAttributeValue(x.Attribute("id")),
                        Title = GetElementValue(x.Element(prd + "ShortDescription")),
                        Description = GetElementValue(x.Element(prd + "LongDescription")),
                        Images = ParseImages(x.Element(prd + "AssociatedMedia")),
                        Price = x.Element(prd + "PricingInformation").Element(prd + "CurrentPrice").Element(prd + "Price").Value,
                        WasPrice = GetWasPrice(x.Element(prd + "PricingInformation").Element(prd + "WasPrice")),
                        Promotions = CheckPromotions(x)
                    })
                    .ToList().FirstOrDefault();

                return productInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;

        }

        private List<string> CheckPromotions(XElement xElement)
        {
            if (xElement == null)
                return null;

            XNamespace prm = "http://schemas.homeretailgroup.com/promotion";

            var promoElement = xElement.Element(prm + "RelatedPromotions");
            if (promoElement == null)
                return null;

            return promoElement.Elements(prm + "Promotion").Select(x => x.Value).ToList();
        }

        private string GetWasPrice(XElement element)
        {
            if (element == null)
                return string.Empty;

            XNamespace prd = "http://schemas.homeretailgroup.com/product";

            var val = element.Element(prd + "Price");

            return (val == null) ? string.Empty : val.Value;
        }

        private List<ProductImage> ParseImages(XElement element)
        {
            XNamespace prd = "http://schemas.homeretailgroup.com/product";

            var nodes = element.Elements(prd + "Content").Select(x => new ProductImage()
            {
                Index = GetAttributeValue(x.Attribute("index")),
                Url = GetAttributeValue(x.Attribute("href")),
                Type = GetAttributeValue(x.Attribute("usage")),
                Application = GetAttributeValue(x.Attribute("type"))
            }).ToList();

            return nodes;
        }

        public async Task<ArgosReviews> GetReviews(string productId)
        {
            
            // GET http://argos.ugc.bazaarvoice.com/data/reviews.json?apiversion=5.1&passkey=dnm7lafdy1y0xd3kp0dka6eln&filter=ProductId:1984802&locale=en_GB&Limit=25&Offset=0&Sort=SubmissionTime:desc

            try
            {
                var path =
                    string.Format(
                        "http://argos.ugc.bazaarvoice.com/data/reviews.json?apiversion=5.1&passkey=dnm7lafdy1y0xd3kp0dka6eln&filter=ProductId:{0}&locale=en_GB&Limit=50&Offset=0&Sort=SubmissionTime:desc",
                        FormatProductCode(productId));

                var response = await Client.GetAsync(path);

                var content = await response.Content.ReadAsStringAsync();

                var t = DeserialiseResponse<ArgosReviews>(content);

                return t;

            }
            catch (Exception)
            {

                return null;
            }

        }


        protected T DeserialiseResponse<T>(string responseContent)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(responseContent))
                return default(T);

            var result = JsonConvert.DeserializeObject<T>(responseContent, new JsonSerializerSettings
            {
                Error = delegate(object sender, ErrorEventArgs args)
                {
                    if (Debugger.IsAttached)
                        Debugger.Break();

                    errors.Add(args.ErrorContext.Path + " : " + args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                }
            });

            return result;
        }

    }

}

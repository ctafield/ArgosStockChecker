using System.Device.Location;

namespace ArgosApi.Types
{
    public class StoreInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string PostCode { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Distance { get; set; }
        public string Telephone { get; set; }

        public GeoCoordinate Position
        {
            get
            {
                double lat;
                double lon;

                if (!double.TryParse(Latitude, out lat))
                    return null;

                if (!double.TryParse(Longitude, out lon))
                    return null;

                return new GeoCoordinate(lat, lon);
            }
        }

        public StoreInfo Clone()
        {
            return (StoreInfo)this.MemberwiseClone();
        }
    }


}

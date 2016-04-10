using System.Windows.Media;
using ArgosApi.Types;

namespace ArgosStockChecker.Classes
{

    public class StockInfo : StoreInfo
    {

        public bool HasStock { get; set; }
        public string Balance { get; set; }

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

        public StockInfo(StoreInfo store, ProductStock stock)
        {
            this.Id = store.Id;
            this.Name = store.Name;
            this.County = store.County;
            this.Distance = store.Distance;
            this.Latitude = store.Latitude;
            this.Longitude = store.Longitude;
            this.PostCode = store.PostCode;
            this.Town = store.Town;
            this.Telephone = store.Telephone;
            this.AddressLine1 = store.AddressLine1;
            this.AddressLine2 = store.AddressLine2;
            this.AddressLine3 = store.AddressLine3;

            if (stock != null)
            {
                this.HasStock = stock.Available;
                this.Balance = stock.Balance;
            }
        }

    }

}

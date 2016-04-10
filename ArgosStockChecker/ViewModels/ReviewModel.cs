using System;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace ArgosStockChecker.ViewModels
{
    public class ReviewViewModel
    {
        public string Text { get; set; }
        public double? RatingNumeric { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public string UserLocation { get; set; }
        public string ReviewDate { get; set; }
        public bool IsRecommended { get; set; }


        public string ReviewDateFormatted
        {
            get
            {
                try
                {
                    DateTime dt = XmlConvert.ToDateTime(ReviewDate, XmlDateTimeSerializationMode.Utc);
                    return dt.ToString("dd MMMM yy");
                }
                catch (Exception)
                {
                    return string.Empty;
                }
                
            }
        }

        public Visibility RatingVisibility
        {
            get { return RatingNumeric.HasValue ? Visibility.Visible : Visibility.Collapsed; }
        }

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

    }
}

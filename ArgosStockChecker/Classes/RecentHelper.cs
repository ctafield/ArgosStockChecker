using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using ArgosStockChecker.ViewModels;

namespace ArgosStockChecker.Classes
{

    internal class RecentHelper
    {

        private const int RecentItemsCount = 5;
        private const string SettingsKey = "RecentItems";

        public List<ProductViewModel> GetRecentItems()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(SettingsKey))
                return null;

            return IsolatedStorageSettings.ApplicationSettings[SettingsKey] as List<ProductViewModel>;
        }

        public void ClearRecentItems()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingsKey))
                IsolatedStorageSettings.ApplicationSettings.Remove(SettingsKey);

            IsolatedStorageSettings.ApplicationSettings.Save();
        }


        private object addLock = new object();

        public void AddRecentItem(ProductViewModel item)
        {

            lock (addLock)
            {

                var recentItems = GetRecentItems();
                if (recentItems == null)
                    recentItems = new List<ProductViewModel>();

                var existing =
                    recentItems.SingleOrDefault(
                        x => string.Compare(x.Id, item.Id, StringComparison.InvariantCultureIgnoreCase) == 0);
                if (existing != null)
                {
                    recentItems.Remove(existing);
                }

                recentItems.Insert(0, item);

                var firstFew = recentItems.Take(RecentItemsCount).ToList();

                if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingsKey))
                    IsolatedStorageSettings.ApplicationSettings[SettingsKey] = firstFew;
                else
                    IsolatedStorageSettings.ApplicationSettings.Add(SettingsKey, firstFew);

                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

    }

}

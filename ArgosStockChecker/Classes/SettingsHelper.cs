using System.Collections.Generic;
using System.IO.IsolatedStorage;
using ArgosApi.Types;

namespace ArgosStockChecker.Classes
{
    
    internal class SettingsHelper
    {

        public void SaveStores(List<StoreInfo> stores)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("Stores"))
                IsolatedStorageSettings.ApplicationSettings["Stores"] = stores;
            else
                IsolatedStorageSettings.ApplicationSettings.Add("Stores", stores);

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public List<StoreInfo> GetStores()
        {

            if (!IsolatedStorageSettings.ApplicationSettings.Contains("Stores"))
                return null;

            var stores = IsolatedStorageSettings.ApplicationSettings["Stores"] as List<StoreInfo>;
            return stores;
        }

    }

}

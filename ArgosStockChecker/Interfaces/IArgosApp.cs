using System;
using System.Windows;

namespace ArgosStockChecker.Interfaces
{
    public interface IArgosApp
    {

        event EventHandler StoresChangedEvent;
        void StoresChanged();

        string SessionId { get; set; }

    }
}

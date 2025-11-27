using ContourSearcher.UI.Constant;
using ContourSearcher.UI.DataExchange;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourSearcher.UI.Helpers
{
    internal class UIHelper
    {
        public static void RefreshObservableCollection<TItem>(ObservableCollection<TItem> observable, List<TItem> input)
        {
            observable.Clear();
            foreach (var img in input)
            {
                observable.Add(img);
            }
        }
    }
}

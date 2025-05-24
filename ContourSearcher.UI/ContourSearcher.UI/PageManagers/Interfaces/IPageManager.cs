using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ContourSearcher.UI.PageManagers.Interfaces
{
    internal interface IPageManager
    {
        public EventHandler<PageManagerEventArgs> OnPageChanged { get; set; }

        void RegisterPage(string name, Page page);

        void UnregisterPage(string name);

        void Switch(string name, Frames frame = Frames.Main);
    }
}

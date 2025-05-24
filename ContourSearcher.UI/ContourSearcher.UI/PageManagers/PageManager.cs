using ContourSearcher.UI.PageManagers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ContourSearcher.UI.PageManagers
{
    internal enum Frames
    { 
        Main, Right
    }

    internal class PageManagerEventArgs : EventArgs
    {
        public Page Page { get; }

        public Frames Frame { get; }

        public PageManagerEventArgs(Page page, Frames frame)
        {
            Page = page;
            Frame = frame;
        }
    }

    internal class PageManager : IPageManager
    {
        #region Events
        private EventHandler<PageManagerEventArgs> m_OnPageChanged;
        #endregion

        #region Fields
        private Dictionary<string, Page> m_pages;

        public EventHandler<PageManagerEventArgs> OnPageChanged 
        {
            get => m_OnPageChanged;
            set => m_OnPageChanged = value;
        }
        #endregion

        #region Ctor
        public PageManager()
        {
            m_pages = new Dictionary<string, Page>();
        }
        #endregion

        #region Methods

        public void RegisterPage(string name, Page page)
        { 
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if(page == null)
                throw new ArgumentNullException(nameof(page));

            if(m_pages.ContainsKey(name))
                return;

            m_pages.Add(name, page);
        }

        public void UnregisterPage(string name)
        { 
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if(!m_pages.ContainsKey(name))
                return;

            m_pages.Remove(name);
        }

        public void Switch(string name, Frames frame)
        {
            Page page = null;

            if (m_pages.TryGetValue(name, out page))
            {
                m_OnPageChanged?.Invoke(this, new PageManagerEventArgs(page, frame));
            }            
        }

        #endregion
    }
}

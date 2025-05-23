using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourSearcher.UI.DataExchange
{
    internal static class ShareData
    {
        private static Dictionary<string, dynamic> m_Storage { get; set; }

        public static void InsertItem<T>(string key, T obj)
        {
            if (m_Storage.ContainsKey(key))
                return;

            m_Storage.Add(key, obj);
        }

        public static T GetItem<T>(string key)
        { 
            return m_Storage[key];
        }

        public static void RemoveItem(string key)
        { 
            if(!m_Storage.ContainsKey(key))
                return;

            m_Storage.Remove(key);
        }

        public static void Clear()
        { 
            if(m_Storage.Count == 0)
                return;

            m_Storage.Clear();
        }

        static ShareData()
        {
            m_Storage = new Dictionary<string, dynamic>();
        }
    }
}

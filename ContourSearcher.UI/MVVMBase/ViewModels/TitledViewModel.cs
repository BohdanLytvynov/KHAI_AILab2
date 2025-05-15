using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMBase.ViewModels
{
    public class TitledViewModel : ViewModelBase
    {
        #region Fields
        private string m_title;
        #endregion

        #region Properties
        public string Title 
        {
            get=>m_title;
            set=>Set(ref m_title, value);
        }
        #endregion

        #region Ctor
        public TitledViewModel()
        {
            m_title = string.Empty;
        }
        #endregion
    }
}

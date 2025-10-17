using MVVMBase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Mat = UserControls.Math.Matrix<double>;

namespace ContourSearcher.UI.ViewModels.Models
{
    internal class FilterViewModel : ViewModelBase
    {
        #region Fields
        private int m_showNumber;
        private Mat m_kernel;
        private bool m_callGetMatrix;
        private Action<Mat> m_getMatrix;
        private bool m_isValid;
        #endregion

        #region Properties
        public bool CallGetMatrix 
        { 
            get=> m_callGetMatrix;
            set=> Set(ref m_callGetMatrix, value);
        }

        public Action<Mat> GetMatrixFunction 
        { 
            get=> m_getMatrix;
            set=> Set(ref m_getMatrix, value);
        }

        public Mat Kernel 
        {
            get=>m_kernel;
            set=> Set(ref m_kernel, value);
        }

        public int ShowNumber
        { 
            get=> m_showNumber;
            set=> Set(ref m_showNumber, value);
        }

        public bool IsValid 
        {
            get=> m_isValid;
            set=> Set(ref m_isValid, value);
        }
        #endregion

        #region Ctor
        public FilterViewModel(int showNumber)
        {
            #region Init Fields
            m_showNumber = showNumber;
            m_kernel = new Mat(3,3,0);
            m_getMatrix = new Action<Mat>(GetMatrix);
            #endregion
        }
        #endregion

        #region Methods
        private void GetMatrix(Mat matrix)
        {   
            this.Kernel = matrix;
        }

        public Mat GetMatrix()
        { 
            CallGetMatrix = !CallGetMatrix;
            return Kernel;
        }
        #endregion
    }
}

using ContourSearcher.UI.Enums;
using MVVMBase.Commands;
using MVVMBase.ViewModels;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Models.Filters
{
    internal abstract class FilterViewModel : ViewModelBase
    {
        #region Events
        public event Action<FilterViewModel> OnApplyButtonPressedEvent;
        public event Action<FilterViewModel> OnDeleteButtonPressedEvent;
        #endregion

        #region Fields
        private int m_showNumber;
        private bool m_isValid;

        private string m_filterName;
        private NumberDataType m_depth;
        private int m_kernelRows;
        private int m_kernelColumns;
        private int m_anchorX;
        private int m_anchorY;
        private bool m_normalize;
        private FilterBorderType m_borderType;
        #endregion

        #region Properties
        public bool ExternalCheck { get; set; }
        public string FilterName { get => m_filterName; set => Set(ref m_filterName, value); }

        public NumberDataType Depth { get => m_depth; set => Set(ref m_depth, value); }
        public int KernelRows { get => m_kernelRows; set => Set(ref m_kernelRows, value); }
        public int KernelColumns { get => m_kernelColumns; set => Set(ref m_kernelColumns, value); }
        public int AnchorX { get => m_anchorX; set => Set(ref m_anchorX, value); }
        public int AnchorY { get => m_anchorY; set => Set(ref m_anchorY, value); }
        public bool Normalize { get => m_normalize; set => Set(ref m_normalize, value); }
        public FilterBorderType BorderType { get => m_borderType; set => Set(ref m_borderType, value); }

        public FilterTypes FilterType { get; protected set; }

        public int ShowNumber
        {
            get => m_showNumber;
            set => Set(ref m_showNumber, value);
        }

        public bool IsValid
        {
            get => m_isValid;
            set => Set(ref m_isValid, value);
        }
        #endregion

        #region Commands
        public ICommand OnApplyButtonPressed { get; }

        public ICommand OnDeleteButtonPressed { get; }
        #endregion

        #region Ctor
        protected FilterViewModel(int showNumber)
        {
            m_filterName = string.Empty;
            m_showNumber = showNumber;
            m_isValid = true;
            m_kernelColumns = 3;
            m_kernelRows = 3;
            m_anchorX = -1;
            m_anchorY = -1;

            OnApplyButtonPressed = new Command
                (
                 OnApplyButtonPressedExecute,
                 CanOnApplyButtonPressedExecute
                );

            OnDeleteButtonPressed = new Command
                (
                 OnDeleteButtonPressedExecute,
                 CanOnDeleteButtonPressedExecute
                );
        }

        protected FilterViewModel() : this(-1)
        {
            
        }
        #endregion

        #region Methods
        protected virtual bool CanOnApplyButtonPressedExecute(object p) => IsValid;

        protected virtual void OnApplyButtonPressedExecute(object p)
        {
            Check();

            OnApplyButtonPressedEvent?.Invoke(this);
        }

        protected virtual bool CanOnDeleteButtonPressedExecute(object p) => true;

        protected virtual void OnDeleteButtonPressedExecute(object p)
        {
            OnDeleteButtonPressedEvent?.Invoke(this);
        }

        public virtual bool Check()
        { 
            IsValid = KernelRows > 0 && KernelColumns > 0 && ExternalCheck;
            return IsValid;
        }

        #endregion
    }
}

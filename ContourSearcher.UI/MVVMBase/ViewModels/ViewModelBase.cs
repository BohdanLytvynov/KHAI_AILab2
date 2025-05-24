using MVVMBase.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MVVMBase.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        private Dispatcher? m_Dispatcher;

        public Dispatcher? Dispatcher
        {
            get => m_Dispatcher;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                m_Dispatcher = value;
            }
        }

        #region INotify Property Changed Implementation

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            var temp = Volatile.Read(ref this.PropertyChanged);
            temp?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        #endregion

        public virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            if (value.Equals(field))
            {
                return false;
            }
            else
            {
                field = value;
                OnPropertyChanged(propName);
                return true;
            }
        }

        public virtual bool SetToDefaultIfNull<T>(ref T field, T value, T defValue, [CallerMemberName] string propName = "")
        {
            if (field == null)
            {
                field = defValue;
            }

            if (value == null)
            {
                value = defValue;
            }

            if (value.Equals(field))
            {
                return false;
            }
            else
            {
                field = value;
                OnPropertyChanged(propName);
                return true;
            }
        }

        #region IDataErrorInfo

        public virtual string Error => throw new NotImplementedException();

        public virtual string this[string columnName] => throw new NotImplementedException();

        private bool[] m_ValidArray = null;

        #endregion

        protected void InitValidArray(int count)
        {
            m_ValidArray = new bool[count];
        }

        protected bool ValidateFields(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                if (!m_ValidArray[i])
                    return false;
            }

            return true;
        }

        protected bool ValidateField(int index)
        { 
            return m_ValidArray[index];
        }

        protected void ResetValidArray()
        {
            int len = m_ValidArray.Length;

            for (int i = 0; i < len; i++)
            {
                m_ValidArray[i] = false;
            }
        }

        protected void SetValidArrayValue(int pos, bool value)
        {
            m_ValidArray[pos] = value;
        }

        protected int GetValidArrayLastIndex()
        {
            return m_ValidArray.Length - 1;
        }

        protected int GetValidArrayCount()
        {
            return m_ValidArray.Length;
        }

        protected void QueueJobToDispatcher(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            m_Dispatcher?.Invoke(action);
        }

        protected void InitValidArray(ViewModelBase viewModelBase)
        {
            int count = 0;

            var t = viewModelBase.GetType();

            var prop = t.GetProperties();

            foreach (var p in prop)
            {
                if (p.GetCustomAttribute<ValidateProperty>() != null)
                { 
                    count++;
                }
            }

            m_ValidArray = new bool[count];
        }
    }
}

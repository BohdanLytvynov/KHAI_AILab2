using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserControls
{
    /// <summary>
    /// Interaction logic for MatrixRow.xaml
    /// </summary>
    public partial class MatrixRow : UserControl
    {
        #region Fields
        private double m_cellWidth;
        #endregion

        #region Ctor

        public MatrixRow(double width, double height, double cellWidth)
        {
            InitializeComponent();
            this.Width = width;
            this.Height = height;
            m_cellWidth = cellWidth;
        }

        #endregion

        #region Methods
        public void AddCell(string value)
        { 
            Cell cell = new Cell(value) 
            { Height = this.Height };
            cell.Width = m_cellWidth;
            cell.Height = Height;
            this.Row.Children.Add(cell);
        }

        public void Clear()
        { 
            this.Row.Children.Clear();
        }

        public List<double> GetRow()
        { 
            List<double> row = new List<double>();

            foreach (var item in this.Row.Children)
            {
                var c = item as Cell;
                row.Add(c.Value);
            }

            return row;
        }
        #endregion
    }
}

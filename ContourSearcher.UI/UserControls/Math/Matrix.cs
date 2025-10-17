using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserControls.Math
{
    public class Matrix<T> : IEquatable<Matrix<T>>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        #region Fields
        #endregion

        #region Properties
        public uint Rows { get; private set; }
        public uint Cols { get; private set; }
        public List<List<T>> matrix { get; set; }
        #endregion

        #region Ctor
        public Matrix()
        {
            Rows = 0; 
            Cols = 0; 
            matrix = new List<List<T>>();
        }

        public Matrix(uint rows, uint cols)
        {
            Rows = rows; 
            Cols = cols;
            matrix = new List<List<T>>();
        }

        public Matrix(uint rows, uint cols, T initValue)
        {
            Rows = rows;
            Cols = cols;
            matrix = new List<List<T>>();
            Create(rows, cols, initValue);
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (List<T> list in matrix)
            {
                foreach (T item in list)
                {
                    stringBuilder.Append($" {item} ");
                }
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }

        public bool Equals(Matrix<T>? other)
        {
            if(other == null) return false;

            if(object.ReferenceEquals(this, other)) return true;

            return matrix.SequenceEqual(other.matrix);
        }

        public void Clear()
        {
            foreach (var item in matrix)
            {
                item.Clear();
            }

            matrix.Clear();

            Rows = 0;
            Cols = 0;
        }

        public void Create(uint r, uint c, T value)
        {
            Clear();

            Rows = r;
            Cols = c;

            for (uint i = 0; i < r; i++)
            {
                List<T> list = new List<T>();
                for (uint j = 0; j < c; j++)
                {
                    list.Add(value);
                }
                matrix.Add(list);
            }
        }

        public void Populate(List<List<T>> src)
        {
            Clear();

            Rows = (uint)src.Count;
            if (src[0] == null)
            {
                Cols = 0;
            }
            else
            {
                Cols = (uint)src[0].Count;
            }

            foreach (var r in src)
            {
                List<T> row = new List<T>();
                foreach (var c in r)
                {
                    row.Add(c);
                }
                matrix.Add(row);
            }
        }

        public void FillCurrent(Matrix<T> src)
        { 
            if(src == null || object.ReferenceEquals(this, src))
                return;
            uint srcR = src.Rows;
            uint srcC = src.Cols;
            for (int i = 0; i < Rows; i++)
            { 
                for (int j = 0;j < Cols; j++)
                {
                    if (i < srcR && j < srcC)
                    {
                        matrix[i][j] = src[(uint)i, (uint)j];
                    }
                }
            }
        }

        public List<List<T>> ToJaggedArray()
        { 
            List<List<T>> res = new List<List<T>>();

            foreach (var r in matrix)
            {
                List<T> row = new List<T>();
                foreach (var c in r)
                {
                    row.Add(c);
                }
                res.Add(row);
            }

            return res;
        }

        public T this[uint r, uint c]
        {
            get => At(r, c);
            set => Set(r, c, value);
        }

        public T At(uint r, uint c)
        {
            if (r < Rows && c < Cols)
                return matrix[(int)r][(int)c];
            return default;
        }

        public void Set(uint r, uint c, T value)
        {
            if (r < Rows && c < Cols)
            {
                matrix[(int)r][(int)c] = value;
            }
        }
        #endregion
    }
}

using System;

namespace MatrixLibrary
{
    /// <summary>
    /// Matrix class to provide properties and functionality important to applications of linear algebra
    /// </summary>
    public class Matrix
    {
        // properties
        public decimal[,] grid { get; set; }
        public int rows => grid?.GetLength(0) ?? 0;
        public int columns => grid?.GetLength(1) ?? 0;

        // constructors
        public Matrix(int rows, int columns)
        { grid = new decimal[rows, columns]; }

        public Matrix(decimal[,] matrix)
        { grid = _Deepcopy(matrix); }

        public Matrix(Matrix a)
        { grid = _Deepcopy(a.grid); }

        // matrix grid deep copy function
        private static decimal[,] _Deepcopy(decimal[,] matrix)
        {
            if (matrix == null) throw new ArgumentException("cannot copy null matrix");

            int n = matrix.GetLength(0), m = matrix.GetLength(1);

            decimal[,] copy = new decimal[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    copy[i, j] = matrix[i, j];
            return copy;
        }

        // indexer for ease of access
        public decimal this[int i, int j]
        {
            get => grid[i, j];
            set => grid[i, j] = value;
        }

        // operators
        public static Matrix operator +(Matrix a) => a;
        public static Matrix operator -(Matrix a)
        {
            // todo: is a new matrix necessary?
            Matrix b = new Matrix(a);
            for (int i = 0; i < b.rows; i++)
                for (int j = 0; j < b.columns; j++)
                    b[i, j] = -a[i, j];
            return b;
        }

        /// <summary>
        /// returns a new matrix for which each entry at indices i,j 
        /// is the sum of the values of a and b at the same indices
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.rows != b.rows || a.columns != b.columns)
                throw new ArgumentException("matrices must be the same size in rows and columns to add");

            // todo: is a new matrix necessary?
            Matrix result = new Matrix(a.rows, a.columns);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.columns; j++)
                    result[i, j] = a[i, j] + b[i, j];
            return result;
        }


        /// <summary>
        /// returns a new matrix for which each entry at indices i,j 
        /// is the difference of the values of a and b at the same indices
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.rows != b.rows || a.columns != b.columns)
                throw new ArgumentException("matrices must be the same size in rows and columns to add");

            // todo: is a new matrix necessary?
            Matrix result = new Matrix(a.rows, a.columns);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.columns; j++)
                    result[i, j] = a[i,j] - b[i, j];
            return result;
        }

        /// <summary>
        /// returns the matrix product of a and b as a new matrix
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.columns != b.rows)
                throw new ArgumentException("cannot multiply matrices with different sized columns/rows");

            int m = a.rows, n = b.columns;
            Matrix result = new Matrix(m, n);

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    decimal sum = 0;
                    for (int k = 0; k < m; k++)
                        sum += a[i, k] * b[k, j];
                    result[i, j] = sum;
                }
            return result;
        }

        /// <summary>
        /// returns true if a and b are the same size 
        /// and every value in a for any index i,j equals the value at the corresponding index i,j of b,
        /// else false
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Matrix a, Matrix b)
        {
            if (a.rows != b.rows || a.columns != b.columns)
                return false;
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.columns; j++)
                    if (a[i, j] != b[i, j])
                        return false;
            return true;
        }

        /// <summary>
        /// returns true if a and b are not the same size 
        /// or any value in a for any index i,j does not equal the value at the corresponding index i,j of b,
        /// else false
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Matrix a, Matrix b) => !(a == b);

        // overrides
        public override bool Equals(object obj)
        {
            if (obj == null || !(this.GetType().Equals(obj.GetType())))
                return false;

            return this == (Matrix)obj;
        }
    }
}

using System;

namespace Matrix
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

        public Matrix(decimal[,] grid)
        { grid = _Deepcopy(grid); }

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
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.rows != b.rows || a.columns != b.columns)
                throw new ArgumentException("matrices must be the same size in rows and columns to add");

            // todo: is a new matrix necessary?
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.columns; i++)
                    a[i, j] = a[i, j] + b[i, j];
            return a;
        }
        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.rows != b.rows || a.columns != b.columns)
                throw new ArgumentException("matrices must be the same size in rows and columns to add");
            
            // todo: is a new matrix necessary?
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.columns; j++)
                    a[i, j] -= b[i, j];
            return a;
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.columns != b.rows)
                throw new ArgumentException("cannot multiply matrices with different sized columns/rows");

            int m = a.rows, n = b.columns;
            decimal[,] result = new decimal[m, n];

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    decimal sum = 0;
                    for (int k = 0; k < m; k++)
                        sum += a[i, k] * b[k, j];
                    result[i, j] = sum;
                }
            return new Matrix(result);
        }
        public override string ToString() => "Matrix";
    }

    /// <summary>
    /// Extension class to provide additional functionality to the Matrix class
    /// 
    /// These could be implemented in the main matrix class as additional properties, 
    /// but I think that properties are set upon object creation.
    /// These properties aren't necessary upon object creation, so for now they're here.
    /// </summary>
    public static class MatrixExtensions
    {
        /// <summary>
        /// determines if the column vectors of a given matrix are linearly independent
        /// by computing the determinant of the matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>true if the column vectors of the matrix are linearly independent, else false</returns>
        public static bool IsLinearlyIndependent(this decimal[,] matrix)
        {
            int n = matrix.GetLength(0), m = matrix.GetLength(1);

            // more column vectors than equations means linearly dependent
            if (m > n) return false;

            // compare rref to identity matrix
            var rref = RREF(matrix);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (i == j) continue;
                    if (rref[i, j] != 0)
                        return false;
                }

            return true;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MatrixLibrary
{
    //todo: refactor class to use Vector class
    /// <summary>
    /// Matrix class to provide properties and functionality important to applications of linear algebra
    /// </summary>
    public class Matrix : IEnumerable<Vector>
    {
        // properties
        public List<Vector> grid { get; set; }
        public int rows => grid?.First()?.height ?? 0;
        public int columns => grid?.Count ?? 0;

        // constructors
        public Matrix()
        { grid = new List<Vector>(); }

        public Matrix(int rowCount, int colCount)
        {
            grid = new List<Vector>(colCount);
            for(int j = 0; j < colCount; j++)
                grid.Add(new Vector(rowCount));
        }

        public Matrix(List<Vector> matrix)
        { grid = _Deepcopy(matrix); }

        public Matrix(Matrix a)
        { grid = _Deepcopy(a.grid); }

        public Matrix(decimal[,] matrix)
        {
            if(matrix == null || matrix.GetLength(0) == 0 || matrix.GetLength(1) == 0)
                throw new ArgumentNullException();

            int height = matrix.GetLength(0), width = matrix.GetLength(1);

            // iterate over columns
            grid = new List<Vector>(width);
            for (int j = 0; j < width; j++)
            {
                decimal[] v = new decimal[height];
                for (int i = 0; i < height; i++)
                    v[i] = matrix[i, j];
                grid.Add(new Vector(v));
            }
        }

        // matrix grid deep copy function
        private static List<Vector> _Deepcopy(List<Vector> matrix)
        {
            if (matrix == null || matrix.Count == 0) throw new ArgumentException("cannot copy null/empty matrix");
            int vHeight = matrix[0].height;
            if(matrix.Any(v => v.height != vHeight)) throw new ArgumentException("All vectors must be the same height");

            List<Vector> copy = new List<Vector>(matrix.Count);
            foreach (Vector v in matrix)
                copy.Add(new Vector(v));

            return copy;
        }

        // indexer for ease of access
        public decimal this[int i, int j]
        {
            // get ith row and jth column instead of ith column vector and jth row
            get => grid[j][i];
            set => grid[j][i] = value;
        }

        // operators
        public static Matrix operator +(Matrix a)
        {
            if (a == null) throw new ArgumentNullException();
            return a;
        }
        public static Matrix operator -(Matrix a)
        {
            if (a == null) throw new ArgumentNullException();

            Matrix b = new Matrix();
            foreach (Vector v in a)
                b.grid.Add(new Vector(-v));
            
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
            if (a == null || b == null)
                throw new ArgumentNullException();
            if (a.rows != b.rows || a.columns != b.columns)
                throw new ArgumentException("matrices must have the same number of rows and columns respectively");

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
            if (a == null || b == null)
                throw new ArgumentNullException();
            if (a.rows != b.rows || a.columns != b.columns)
                throw new ArgumentException("matrices must have the same number of rows and columns respectively");

            Matrix result = new Matrix(a.rows, a.columns);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.columns; j++)
                    result[i, j] = a[i, j] - b[i, j];
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
            if (a == null || b == null)
                throw new ArgumentNullException();
            if (a.columns != b.rows)
                throw new ArgumentException("a must have the same number of columns as b does rows for multiplication");

            Matrix result = new Matrix(a.rows, b.columns);

            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < b.columns; j++)
                {
                    decimal sum = 0;
                    for (int k = 0; k < a.rows; k++)
                        sum += a[i, k] * b[k, j];
                    result[i, j] = sum;
                }
            return result;
        }

        /// <summary>
        /// returns a new matrix with for which every entry in every row
        /// is equivalent to every entry of every row of a scaled by scalar
        /// </summary>
        /// <param name="scalar"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Matrix operator *(decimal scalar, Matrix a)
        {
            if (a == null) throw new ArgumentNullException();

            // if all entries will be 0 why waste time actually multiplying?
            if (scalar == 0) return new Matrix(a.rows, a.columns);

            Matrix result = new Matrix(a.rows, a.columns);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.columns; j++)
                    result[i, j] = scalar * a[i, j];
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
            if (ReferenceEquals(a, b)) return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

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

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<Vector> GetEnumerator() => grid.GetEnumerator();
    }
}

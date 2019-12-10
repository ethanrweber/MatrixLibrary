using System;

namespace MatrixLibrary
{
    /// <summary>
    /// Library of matrix functions
    /// implementations are non-generic and specify type decimal to ensure reliability and accuracy
    /// </summary>
    public static class MatrixFunctions
    {
        /// <summary>
        /// returns the given matrix in reduced row echelon form using Gaussian Elimination
        /// </summary>
        /// <param name="matrix">m x n matrix</param>
        /// <returns>n x m matrix in reduced row echelon form</returns>
        public static Matrix RREF(Matrix matrix)
        {
            Matrix rref = new Matrix(matrix);

            int lead = 0, rowCount = rref.rows, columnCount = rref.columns;

            for (int r = 0; r < rowCount; r++)
            {
                if (columnCount <= lead)
                    break;
                int i = r;
                while (rref[i, lead] == 0)
                {
                    i++;
                    if (rowCount == i)
                    {
                        i = r;
                        lead++;
                        if (columnCount == lead)
                        {
                            lead--;
                            break;
                        }
                    }
                }

                for (int k = 0; k < columnCount; k++)
                {
                    decimal temp = rref[i, k];
                    rref[i, k] = rref[r, k];
                    rref[r, k] = temp;
                }

                var div = rref[r, lead];
                if (div != 0)
                    for (int k = 0; k < columnCount; k++)
                        rref[r, k] /= div;

                for (int j = 0; j < rowCount; j++)
                    if (j != r)
                    {
                        var mult = rref[j, lead];
                        for (int k = 0; k < columnCount; k++)
                        {
                            rref[j, k] = rref[j, k] - mult * rref[r, k];
                            if (rref[j, k] % 1 == 0)
                                rref[j, k] = (int)rref[j, k]; // remove rounding issue: 1.000000 -> 1
                        }
                    }

                lead++;
            }

            return rref;
        }

        /// <summary>
        /// prints a matrix to console output
        /// </summary>
        /// <param name="matrix">matrix to be printed</param>
        /// <param name="round">number of decimal points to round answers, default 2</param>
        public static void PrintMatrix(Matrix matrix, int round = 2)
        {
            for (int i = 0; i < matrix.rows; i++)
            {
                for (int j = 0; j < matrix.columns; j++)
                    Console.Write(decimal.Round(matrix[i, j], round) + "\t");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// computes the determinant of a matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static decimal GetDeterminant(Matrix matrix)
        {
            int n = matrix.rows;

            if (n == 0) throw new Exception("matrix cannot be empty");
            if (n == 1) return matrix[0, 0];

            decimal det = 0;
            for (int j = 0; j < n; j++)
                det += (decimal)Math.Pow(-1, 1 + j) * matrix[1, j] * GetDeterminant(GetSubmatrix(matrix, 1, j));

            return det;
        }

        /// <summary>
        /// retrieves a submatrix of the parent matrix, useful for determinant calculations
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>returns an (m-1) x (n-1) sub-matrix of the m x n parent matrix
        /// excluding row 'row' and column 'col'</returns>
        public static Matrix GetSubmatrix(Matrix parent, int row, int col)
        {
            int pn = parent.rows;
            if (pn == 0) throw new ArgumentException("matrix must have values");
            if (pn == 1) return new Matrix(new decimal[0, 0]);

            int n = pn - 1, rOffset = 0, cOffset = 0;

            Matrix matrix = new Matrix(n, n);
            for (int i = 0; i < n; i++)
            {
                if (row == i)
                    rOffset = 1;
                for (int j = 0; j < n; j++)
                {
                    if (col == j)
                        cOffset = 1;
                    matrix[i, j] = parent[i + rOffset, j + cOffset];
                }

                cOffset = 0;
            }

            return matrix;
        }

        /// <summary>
        /// generates an identity matrix of size n x n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Matrix Identity(int n)
        {
            Matrix identity = new Matrix(n, n);
            for (int i = 0; i < n; i++)
                identity[i, i] = 1;
            return identity;
        }

        /// <summary>
        /// generates the transpose of a matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix GetTranspose(Matrix matrix)
        {
            int n = matrix.rows, m = matrix.columns;

            Matrix transpose = new Matrix(m, n);
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    transpose[i, j] = matrix[j, i];
            return transpose;
        }

        /// <summary>
        /// generates the inverse of a matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix GetInverse(Matrix matrix)
        {
            int n = matrix.rows;
            if (n != matrix.columns)
                throw new ArgumentException("Cannot inverse a non-square matrix");

            Matrix toRref = new Matrix(n, 2 * n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    toRref[i, j] = matrix[i, j];

                for (int j = n; j < 2 * n; j++)
                    toRref[i, j] = 0;
                toRref[i, i + n] = 1;
            }

            Matrix rref = RREF(toRref);

            Matrix result = new Matrix(n, n);
            for (int i = 0; i < n; i++)
                for (int j = n; j < 2 * n; j++)
                    result[i, j - n] = rref[i, j];

            return result;
        }
    }

    /// <summary>
    /// Extension class to provide additional functionality to the Matrix class
    /// </summary>
    public static class MatrixExtensions
    {
        /// <summary>
        /// determines if the column vectors of a given matrix are linearly independent
        /// by computing the determinant of the matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>true if the column vectors of the matrix are linearly independent, else false</returns>
        public static bool IsLinearlyIndependent(this Matrix matrix)
        {
            int n = matrix.rows, m = matrix.columns;

            // more column vectors than equations means linearly dependent
            if (m > n) return false;

            // compare rref to identity matrix
            var rref = MatrixFunctions.RREF(matrix);
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

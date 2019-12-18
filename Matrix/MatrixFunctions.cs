using System;
using System.Collections.Generic;
using System.Linq;

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
        public static Matrix RREF(Matrix rref)
        {
            if(rref == null) throw new ArgumentNullException();

            Matrix matrix = new Matrix(rref);
            int lead = 0, rows = matrix.rows, cols = matrix.columns;

            for (int r = 0; r < rows; r++)
            {
                if (cols <= lead)
                    break;
                int i = r;
                while (matrix[i, lead] == 0)
                {
                    i++;
                    if (rows == i)
                    {
                        i = r;
                        lead++;
                        if (cols == lead)
                        {
                            lead--;
                            break;
                        }
                    }
                }

                for (int k = 0; k < cols; k++)
                {
                    decimal temp = matrix[i, k];
                    matrix[i, k] = matrix[r, k];
                    matrix[r, k] = temp;
                }

                var div = matrix[r, lead];
                if (div != 0)
                    for (int k = 0; k < cols; k++)
                        matrix[r, k] /= div;

                for (int j = 0; j < rows; j++)
                    if (j != r)
                    {
                        var mult = matrix[j, lead];
                        for (int k = 0; k < cols; k++)
                        {
                            matrix[j, k] = matrix[j, k] - mult * matrix[r, k];
                            if (matrix[j, k] % 1 == 0)
                                matrix[j, k] = (int)matrix[j, k]; // remove rounding issue: 1.000000 -> 1
                        }
                    }

                lead++;
            }

            return matrix;
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
            if (matrix == null) throw new ArgumentNullException();
            if (matrix.rows == 0) throw new ArgumentException("matrix cannot be empty");
            if (matrix.rows == 1) return matrix[0, 0];

            decimal det = 0;
            for (int j = 0; j < matrix.rows; j++)
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
        private static Matrix GetSubmatrix(Matrix parent, int row, int col)
        {
            if (row < 0 || row >= parent.rows || col < 0 || col >= parent.columns)
                throw new ArgumentException("row and column must be valid rows/columns of the parent matrix");

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
            if (n < 1) throw new ArgumentException("invalid n: n must be >= 1");

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
            if (matrix == null) throw new ArgumentNullException();
            if (matrix.rows < 1 || matrix.columns < 1) throw new ArgumentException("invalid size rows/columns, must be >= 1");

            Matrix transpose = new Matrix(matrix.columns, matrix.rows);
            for (int i = 0; i < matrix.columns; i++)
                for (int j = 0; j < matrix.rows; j++)
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
            if (matrix == null) throw new ArgumentNullException();
            // todo: is returning null the best option?
            if (!matrix.isInvertible()) return null;

            int n = matrix.rows;

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

        /// <summary>
        /// calculate the rank of a matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static int Rank(Matrix matrix)
        {
            if (matrix == null) throw new ArgumentNullException();
            Matrix rref = RREF(matrix);

            int rank = 0;
            for(int i = 0; i < rref.rows; i++)
                for(int j = 0; j < rref.columns; j++)
                    if (rref[i, j] != 0)
                    { rank++; break; }

            return rank;
        }

        /// <summary>
        /// produces an output matrix representing an orthogonal basis for an input matrix by the Gram-Schmidt Process
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix GramSchmidt(Matrix matrix)
        {
            if (matrix == null) throw new ArgumentNullException();
            Matrix result = new Matrix(matrix);

            for (int i = 1; i < matrix.columns; i++)
                for (int j = i-1; j >= 0; j--)
                    result.grid[i] -= (matrix.grid[i] * result.grid[j] / result.grid[j].magnitudeSquared) * result.grid[j];
            return result;
        }

        /// <summary>
        /// Decomposes a matrix A into 2 matrices, Q and R, such that Q transpose * Q = I
        /// and R is an upper triangular matrix such that A * Q inverse = A * Q transpose = R.
        /// </summary>
        /// <param name="A"></param>
        /// <returns>Matrix array of length 2: M[0] = Q, M[1] = R</returns>
        public static Matrix[] QRDecomposition(Matrix A)
        {
            if(A == null) throw new ArgumentNullException();
            Matrix Q = Normalize(GramSchmidt(A));
            Matrix R = A * GetTranspose(Q);
            return new Matrix[] {Q, R};
        }

        /// <summary>
        /// given a matrix A and a vector b, this will approximate Ax = b with AtAx = Atb
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Vector LeastSquares(Matrix A, Vector b)
        {
            if(A == null || b == null) throw new ArgumentNullException();
            if(A.rows != b.height) throw new ArgumentException("cannot solve for vector with a different height than the matrix");

            Matrix At = GetTranspose(A);
            Matrix AtA = At * A;
            Vector AtB = At * b;
            Matrix AtAxAtB = new Matrix(AtA);
            AtAxAtB.grid.Add(AtB);

            Matrix rref = RREF(AtAxAtB);
            Vector result = new Vector(rref.grid.Last());
            return result;
        }

        /// <summary>
        /// returns a new matrix for which every vector is
        /// the normalized version of its corresponding vector in the input matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Matrix Normalize(Matrix matrix)
        {
            if(matrix == null) throw new ArgumentNullException();
            Matrix result = new Matrix(matrix);
            for (int i = 0; i < result.columns; i++)
                result[i] /= result[i].magnitude;
            return result;
        }

        /// <summary>
        /// determines if a matrix is invertible
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static bool isInvertible(this Matrix matrix) => GetDeterminant(matrix) != 0;
    }
}

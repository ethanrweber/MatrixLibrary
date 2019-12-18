using MatrixLibrary;
using static MatrixLibrary.MatrixFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MatrixTester
{
    [TestClass]
    public class MatrixFunctionsTest
    {
        // normal functions
        [TestMethod]
        public void TestRREF()
        {
            Matrix a = new Matrix(new decimal[,] { { 2, 1 }, { 1, 2 } });
            Matrix b = new Matrix(new decimal[,] { { 1, 0 }, { 0, 1 } });
            Assert.AreEqual(RREF(a), b);
        }

        [TestMethod]
        public void TestRREF_ThrowsArgumentException()
        {
            Matrix a = null;
            Assert.ThrowsException<ArgumentNullException>(() => RREF(a));
        }

        [TestMethod]
        public void TestDeterminant_2x2()
        {
            Matrix a = new Matrix(new decimal[,] { { 2, 4 }, { 6, 8 } });
            Assert.AreEqual(GetDeterminant(a), -8);
        }

        [TestMethod]
        public void TestDeterminant_4x4()
        {
            Matrix a = new Matrix(new decimal[,] { { 1, 3, 5, 9 }, { 1, 3, 1, 7 }, { 4, 3, 9, 7 }, { 5, 2, 0, 9 } });
            Assert.AreEqual(GetDeterminant(a), -376);
        }

        [TestMethod]
        public void TestDeterminant_ThrowsArgumentNullException()
        {
            Matrix a = null;
            Assert.ThrowsException<ArgumentNullException>(() => GetDeterminant(a));
        }

        [TestMethod]
        public void TestIdentity()
        {
            Matrix a = new Matrix(new decimal[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } });
            Assert.AreEqual(a, Identity(3));
        }

        [TestMethod]
        public void TestIdentity_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => Identity(0));
        }

        [TestMethod]
        public void TestTranspose()
        {
            Matrix a = new Matrix(new decimal[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            Matrix aTranspose = new Matrix(new decimal[,] { { 1, 4, 7 }, { 2, 5, 8 }, { 3, 6, 9 } });
            Assert.AreEqual(GetTranspose(a), aTranspose);
        }

        [TestMethod]
        public void TestTranspose_ThrowsArgumentNullException()
        {
            Matrix a = null;
            Assert.ThrowsException<ArgumentNullException>(() => GetTranspose(a));
        }

        [TestMethod]
        public void TestInverse()
        {
            Matrix a = new Matrix(new decimal[,] { {4, 7}, {2, 6} });
            Matrix aInverse = new Matrix(new decimal[,] {{0.6m, -0.7m}, {-0.2m, 0.4m}});
            Assert.AreEqual(aInverse, GetInverse(a));
        }

        [TestMethod]
        public void TestInverse_ReturnsNull()
        {
            Matrix a = new Matrix(new decimal[,] { { 1, 4, 7 }, { 2, 5, 8 }, { 3, 6, 9 } });
            Matrix inverse = GetInverse(a);
            // a has no inverse
            Assert.AreEqual(null, inverse);
        }

        [TestMethod]
        public void TestInverse_ThrowsArgumentNullException()
        {
            Matrix a = null;
            Assert.ThrowsException<ArgumentNullException>(() => GetInverse(a));
        }

        [TestMethod]
        public void TestIsInvertible()
        {
            Matrix a = new Matrix(new decimal[,] { { 1, 3, 5, 9 }, { 1, 3, 1, 7 }, { 4, 3, 9, 7 }, { 5, 2, 0, 9 } });
            Assert.IsTrue(a.isInvertible());
        }

        [TestMethod]
        public void TestRank()
        {
            Matrix a = new Matrix(new decimal[,] {{1, 2, 4, 4}, {3, 4, 8, 0}});
            Assert.AreEqual(2, Rank(a));
        }

        [TestMethod]
        public void TestRankTranspose()
        {
            Matrix a = new Matrix(new decimal[,] {{1, 1, 0, 2},{-1, -1, 0, -2}});
            Matrix at = GetTranspose(a);
            Assert.AreEqual(Rank(a), Rank(at));
        }

        [TestMethod]
        public void TestRank_ThrowsArgumentNullException()
        {
            Matrix a = null;
            Assert.ThrowsException<ArgumentNullException>(() => Rank(a));
        }

        [TestMethod]
        public void TestGramSchmidt()
        {
            Matrix a = new Matrix(new decimal[,] {{1, 2}, {1, 2}, {0, 3}});
            Matrix result = new Matrix(new decimal[,] {{1, 0}, {1, 0}, {0, 3}});
            Assert.AreEqual(result, GramSchmidt(a));
        }

        [TestMethod]
        public void TestGramSchmidt_ThrowsArgumentNullException()
        {
            Matrix a = null;
            Assert.ThrowsException<ArgumentNullException>(() => GramSchmidt(a));
        }

        [TestMethod]
        public void TestQRDecomposition()
        {
            Matrix a = new Matrix(new decimal[,] {{2, -2, 18}, {2, 1, 0}, {1, 2, 0}});
            Matrix[] QR = QRDecomposition(a);
            Matrix Q = new Matrix(new decimal[,] {{2m/3m, -2m/3m, 1m/3m}, {2m/3m, 1m/3m, -2m/3m}, {1m/3m, 2m/3m, 2m/3m}});
            Matrix R = new Matrix(new decimal[,] {{3, 0, 12}, {0, 3, -12}, {0, 0, 6}});

            // cannot directly compare QR and Q and R with == because of decimal precision issues from multiplication
            // ex: 2/3 + 1/3 = 0.99999999999999999999
            // todo: maybe fix if possible?
            bool areVeryClose = true;
            for(int i = 0; i < Q.rows; i++)
                for (int j = 0; j < Q.columns; j++)
                    if (Math.Abs(Q[i, j] - QR[0][i, j]) > 0.000000001m)
                    {
                        areVeryClose = false;
                        break;
                    }
            if(areVeryClose)
                for(int i = 0; i < R.rows; i++)
                    for(int j = 0; j < R.columns; j++)
                        if (Math.Abs(R[i, j] - QR[1][i, j]) > 0.000000001m)
                        {
                            areVeryClose = false;
                            break;
                        }
            Assert.IsTrue(areVeryClose);
        }

        public void TestQRDecomposition_ThrowsArgumentNullException()
        {
            Matrix a = null;
            Assert.ThrowsException<ArgumentNullException>(() => QRDecomposition(a));
        }

        [TestMethod]
        public void TestLeastSquares()
        {
            Matrix A = new Matrix(new decimal[,] {{1, 0}, {1, 1}, {1, 2}});
            Vector b = new Vector(new decimal[] {6, 0, 0});
            Vector result = new Vector(new decimal[] {5, -3});
            Assert.AreEqual(result, LeastSquares(A, b));
        }

        [TestMethod]
        public void TestLeastSquares_ThrowsArgumentNullException()
        {
            Matrix a = null;
            Vector b = new Vector(new decimal[] { 6, 0, 0 });
            Assert.ThrowsException<ArgumentNullException>(() => LeastSquares(a, b));
        }
    }
}

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
            Assert.AreEqual(inverse, null);
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
    }
}

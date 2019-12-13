using System;
using MatrixLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MatrixTester
{
    [TestClass]
    public class MatrixTest
    {
        [TestMethod]
        public void TestEqualityByReference()
        {
            Matrix a = new Matrix(2, 2);
            Matrix b = a;
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void TestEqualityByValue()
        {
            Matrix a = new Matrix(new decimal[,] { { 1, 2, 3, 4 } });
            Matrix b = new Matrix(new decimal[,] { { 1, 2, 3, 4 } });
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void TestInequalityWithNull()
        {
            Matrix a = new Matrix(2, 2);
            Matrix b = null;
            Assert.AreNotEqual(a, b);
        }

        [TestMethod]
        public void TestIndexerGet()
        {
            Matrix a = new Matrix(new decimal[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            Assert.AreEqual(a[1, 1], a[1, 1]);
        }

        [TestMethod]
        public void TestIndexerSet()
        {
            Matrix a = new Matrix(new decimal[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            a[1, 1] = 25;
            Assert.AreEqual(a[1, 1], 25);
        }

        [TestMethod]
        public void TestPositiveUnaryOperator() // +
        {
            Matrix a = new Matrix(new decimal[,] { { 2, 4 }, { 6, 8 } });
            Matrix b = new Matrix(+a);
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void TestNegativeUnaryOperator() // -
        {
            Matrix a = new Matrix(new decimal[,] { { 2, 4 }, { 6, 8 } });
            Matrix b = new Matrix(-a);
            Matrix c = new Matrix(new decimal[,] { { -2, -4 }, { -6, -8 } });
            Assert.AreEqual(b, c);
        }

        [TestMethod]
        public void TestPositiveBinaryOperator() // a + b = c
        {
            Matrix a = new Matrix(new decimal[,] { { 2, 4 }, { 6, 8 } });
            Matrix b = new Matrix(new decimal[,] { { 4, 8 }, { 12, 16 } });
            Assert.AreEqual(a + a, b);
        }

        [TestMethod]
        public void TestNegativeBinaryOperator() // a - b = c
        {
            Matrix a = new Matrix(new decimal[,] { { 4, 8 }, { 12, 16 }});
            Matrix b = new Matrix(new decimal[,] { { 1, 2 }, { 3, 4 } });
            Matrix c = new Matrix(new decimal[,] { { 3, 6 }, { 9, 12 } });
            Assert.AreEqual(a - b, c);
        }

        [TestMethod]
        public void TestPositiveBinaryOperator_ThrowsArgumentException()
        {
            Matrix a = new Matrix(new decimal[,] { { 1, 1 } });
            Matrix b = new Matrix(new decimal[,] { { 1, 1, 1 } });
            Assert.ThrowsException<ArgumentException>(() => a + b);
        }


        [TestMethod]
        public void TestNegativeBinaryOperator_ThrowsArgumentException()
        {
            Matrix a = new Matrix(new decimal[,] { { 1, 1 } });
            Matrix b = new Matrix(new decimal[,] { { 1, 1, 1 } });
            Assert.ThrowsException<ArgumentException>(() => a - b);
        }

        [TestMethod]
        public void TestMultiplication_MatrixByMatrix() // a * b = c
        {
            Matrix a = new Matrix(new decimal[,] { { 2, 4 }, { 6, 8 } });
            Matrix aSquared = new Matrix(new decimal[,] { { 28, 40 }, { 60, 88 } });
            Assert.AreEqual(a * a, aSquared);
        }

        [TestMethod]
        public void TestMultiplication_MatrixByScalar()
        {
            Matrix a = new Matrix(new decimal[,] {{1, 2, 3}, {4, 5, 6}});
            int scalar = 2;
            Matrix result = new Matrix(new decimal[,] {{2, 4, 6}, {8, 10, 12}});
            Assert.AreEqual(result, scalar * a);
        }

        [TestMethod]
        public void TestMultiplication_MatrixByVector()
        {
            Matrix a = new Matrix(new decimal[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            Vector b = new Vector(new decimal[] { 2, 1, 3 });
            Vector result = new Vector(new decimal[] { 13, 31, 49 });
            Assert.AreEqual(result, a * b);
        }

        [TestMethod]
        public void TestMultiplication_ThrowsArgumentException()
        {
            Matrix a = new Matrix(new decimal[,] { { 1, 1, 1 }, { 1, 1, 1 } });
            Matrix b = new Matrix(new decimal[,] { { 1, 1 } });
            Assert.ThrowsException<ArgumentException>(() => a * b);
        }
    }
}

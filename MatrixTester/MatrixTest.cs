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
        public void TestMultiplication() // a * b = c
        {
            Matrix a = new Matrix(new decimal[,] { { 2, 4 }, { 6, 8 } });
            Matrix aSquared = new Matrix(new decimal[,] { { 28, 40 }, { 60, 88 } });

            Assert.AreEqual(a * a, aSquared);
        }

        [TestMethod]
        public void TestMultiplication_ThrowsArgumentException()
        {
            decimal[,] twoByThree = new decimal[,] { { 1, 1, 1 }, { 1, 1, 1 } };
            decimal[,] oneByTwo = new decimal[,] { { 1, 1 } };
            Matrix a = new Matrix(twoByThree);
            Matrix b = new Matrix(oneByTwo);

            Assert.ThrowsException<ArgumentException>(() => a * b);
        }
    }
}

using System;
using MatrixLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MatrixTester
{
    [TestClass]
    public class VectorTest
    {
        [TestMethod]
        public void TestEqualityByReference()
        {
            Vector a = new Vector(2);
            Vector b = a;
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void TestEqualityByValue()
        {
            Vector a = new Vector(new decimal[] { 1, 2, 3, 4 });
            Vector b = new Vector(new decimal[] { 1, 2, 3, 4 });
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void TestInequalityWithNull()
        {
            Vector a = new Vector(new decimal[] { 1, 2, 3, 4 });
            Vector b = null;
            Assert.AreNotEqual(a, b);
        }

        [TestMethod]
        public void TestIndexerGet()
        {
            Vector a = new Vector(new decimal[] {1, 2, 3, 4});

            Assert.AreEqual(2, a[1]);
        }

        [TestMethod]
        public void TestIndexerSet()
        {
            Vector a = new Vector(new decimal[] { 1, 2, 3, 4 });
            a[1] = 25;

            Assert.AreEqual(25, a[1]);
        }

        [TestMethod]
        public void TestPositiveUnaryOperator() // +
        {
            Vector a = new Vector(new decimal[] { 1, 2, 3, 4 });
            Vector b = new Vector(+a);
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void TestNegativeUnaryOperator() // -
        {
            Vector a = new Vector(new decimal[] { 1, 2, 3, 4 });
            Vector b = new Vector(-a);
            Vector c = new Vector(new decimal[] {-1, -2, -3, -4});
            Assert.AreEqual(b, c);
        }

        [TestMethod]
        public void TestPositiveBinaryOperator() // a + b = c
        {
            Vector a = new Vector(new decimal[] { 1, 2, 3, 4 });
            Vector b = new Vector(new decimal[] { 2, 4, 6, 8 });
            Assert.AreEqual(a + a, b);
        }

        [TestMethod]
        public void TestNegativeBinaryOperator() // a - b = c
        {
            Vector a = new Vector(new decimal[] { 1, 2, 3, 4 });
            Vector b = new Vector(new decimal[] { 2, 4, 6, 8 });
            Assert.AreEqual(b - a, a);
        }

        [TestMethod]
        public void TestPositiveBinaryOperator_ThrowsArgumentException()
        {
            Vector a = new Vector(new decimal[] { 1, 1 });
            Vector b = new Vector(new decimal[] { 1, 1, 1 });
            Assert.ThrowsException<ArgumentException>(() => a + b);
        }

        [TestMethod]
        public void TestNegativeBinaryOperator_ThrowsArgumentException()
        {
            Vector a = new Vector(new decimal[] { 1, 1 });
            Vector b = new Vector(new decimal[] { 1, 1, 1 });
            Assert.ThrowsException<ArgumentException>(() => a - b);
        }

        [TestMethod]
        public void TestMultiplication_ScalarByVector() // a * b = c
        {
            decimal scalar = 2;
            Vector a = new Vector(new decimal[] { 1, 2, 3, 4 });
            Vector twoA = new Vector(new decimal[] {2, 4, 6, 8});
            Assert.AreEqual(scalar * a, twoA);
        }

        [TestMethod]
        public void TestMultiplication_VectorByVector()
        {
            Vector a = new Vector(new decimal[] { 1, 2 });
            decimal aDota = 1 * 1 + 2 * 2;
            Assert.AreEqual(a * a, aDota);
        }

        [TestMethod]
        public void TestMultiplication_ThrowsArgumentException()
        {
            Vector a = new Vector(new decimal[] { 1, 1 });
            Vector b = new Vector(new decimal[] { 1, 1, 1 });
            Assert.ThrowsException<ArgumentException>(() => a * b);
        }

        [TestMethod]
        public void TestIsOrthogonalTo()
        {
            Vector a = new Vector(new decimal[] {1, 0});
            Vector b = new Vector(new decimal[] { 0, 1 });
            Assert.IsTrue(a.isOrthogonalTo(b));
        }
    }
}

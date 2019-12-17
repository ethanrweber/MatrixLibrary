using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLibrary
{
    public class VectorFunctions
    {
        /// <summary>
        /// returns a new unit vector of the input vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector Normalize(Vector v) => new Vector(v / v.magnitude);
    }

    public static class VectorExtensions
    {
        public static bool isOrthogonalTo(this Vector a, Vector b) => a * b == 0;
    }
}

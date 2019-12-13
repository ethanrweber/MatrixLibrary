using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLibrary
{
    public class VectorFunctions
    {
        
    }

    public static class VectorExtensions
    {
        public static bool isOrthogonalTo(this Vector a, Vector b) => a * b == 0;
    }
}

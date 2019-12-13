using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixLibrary
{
    public class Vector : IEnumerable<decimal>
    {
        private decimal[] column { get; set; }
        public int height => column.Length;
        public decimal magnitude => (decimal)Math.Sqrt((double)column.Sum(x => x * x));
        
        // constructors
        public Vector(int height)
        { column = new decimal[height]; }

        public Vector(decimal[] arr)
        { column = _Deepcopy(arr); }

        public Vector(Vector vector)
        { column = _Deepcopy(vector.column); }

        private static decimal[] _Deepcopy(decimal[] arr)
        {
            decimal[] copy = new decimal[arr.Length];
            for (int i = 0; i < copy.Length; i++)
                copy[i] = arr[i];
            return copy;
        }

        // indexer
        public decimal this[int i]
        {
            get => column[i];
            set => column[i] = value;
        }

        // operator overloading
        /// <summary>
        /// returns v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector operator +(Vector v)
        {
            if (v == null) throw new ArgumentNullException();
            return v;
        }

        /// <summary>
        /// negates every value in a vector and returns the result as a new vector
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Vector operator -(Vector a)
        {
            if (a == null) throw new ArgumentNullException();

            Vector b = new Vector(a.height);
            for (int i = 0; i < b.height; i++)
                b[i] = -a[i];
            return b;
        }

        /// <summary>
        /// computes the sum of each entry between a and b and returns the results as a new vector
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector operator +(Vector a, Vector b)
        {
            if(a == null || b == null) throw new ArgumentNullException();
            if(a.height != b.height) throw new ArgumentException("cannot add vectors of different heights");

            Vector result = new Vector(a.height);
            for (int i = 0; i < result.height; i++)
                result[i] = a[i] + b[i];
            return result;
        }

        /// <summary>
        /// computes the difference of each entry between a and b and returns a new vector
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector operator -(Vector a, Vector b)
        {
            if (a == null || b == null) throw new ArgumentNullException();
            if (a.height != b.height) throw new ArgumentException("cannot add vectors of different heights");

            Vector result = new Vector(a.height);
            for (int i = 0; i < result.height; i++)
                result[i] = a[i] - b[i];
            return result;
        }

        /// <summary>
        /// scales a vector a by a scalar scalar
        /// </summary>
        /// <param name="scalar"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Vector operator *(decimal scalar, Vector a)
        {
            if (a == null) throw new ArgumentNullException();

            // if all entries will be 0 why waste time actually multiplying?
            if (scalar == 0) return new Vector(a.height);

            Vector result = new Vector(a.height);
            for (int i = 0; i < a.height; i++)
                result[i] = scalar * a[i];
            return result;
        }

        /// <summary>
        /// computes the dot/inner product between vectors a and b 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static decimal operator *(Vector a, Vector b)
        {
            if (a == null || b == null) throw new ArgumentNullException();
            if (a.height != b.height) throw new ArgumentException("cannot multiply vectors of different heights");

            decimal result = 0;
            for (int i = 0; i < a.height; i++)
                result += a[i] * b[i];
            return result;
        }

        /// <summary>
        /// if a and b are equal by reference, return true
        /// if a and null or b and null are equal by ref, return false
        /// if a and b have a different height, return false
        /// if there are any values that don't match between a and b, return false
        /// else return true, a and b are equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Vector a, Vector b)
        {
            if (ReferenceEquals(a, b)) return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            if (a.height != b.height)
                return false;

            for (int i = 0; i < a.height; i++)
                if (a[i] != b[i])
                    return false;
            return true;
        }

        /// <summary>
        /// inverse of == operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Vector a, Vector b) => !(a == b);

        // override of class object
        public override bool Equals(object obj)
        {
            if (obj == null || !(this.GetType().Equals(obj.GetType())))
                return false;

            return this == (Vector)obj;
        }

        // implements IEnumerable
        public IEnumerator<decimal> GetEnumerator() => column.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// converts a vector into a unit vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector Normalize(Vector v)
        {
            decimal sum = v.column.Sum();

            Vector result = new Vector(v);
            for (int i = 0; i < result.height; i++)
                result[i] /= sum;
            return result;
        }
    }
}

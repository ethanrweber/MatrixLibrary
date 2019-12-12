using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLibrary
{
    public class Vector
    {
        public decimal[] column { get; set; }
        public int height => column.Length;
        
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
        public static Vector operator +(Vector v)
        {
            if (v == null) throw new ArgumentNullException();
            return v;
        }

        public static Vector operator -(Vector a)
        {
            if (a == null) throw new ArgumentNullException();

            Vector b = new Vector(a.height);
            for (int i = 0; i < b.height; i++)
                b[i] = -a[i];
            return b;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            if(a == null || b == null) throw new ArgumentNullException();
            if(a.height != b.height) throw new ArgumentException("cannot add vectors of different heights");

            Vector result = new Vector(a.height);
            for (int i = 0; i < result.height; i++)
                result[i] = a[i] + b[i];
            return result;
        }

        public static Vector operator -(Vector a, Vector b)
        {
            if (a == null || b == null) throw new ArgumentNullException();
            if (a.height != b.height) throw new ArgumentException("cannot add vectors of different heights");

            Vector result = new Vector(a.height);
            for (int i = 0; i < result.height; i++)
                result[i] = a[i] - b[i];
            return result;
        }

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
        /// computes the dot product between vectors a and b 
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

        public static bool operator !=(Vector a, Vector b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (obj == null || !(this.GetType().Equals(obj.GetType())))
                return false;

            return this == (Vector)obj;
        }
    }
}

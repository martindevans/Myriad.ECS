using System.Numerics;
using System.Runtime.CompilerServices;

#pragma warning disable IDE1006 // Naming Styles

namespace NBodyIntegrator.Mathematics
{
    /// <summary>A 3 component vector of doubles.</summary>
    public readonly struct double3
        : IEquatable<double3>, IFormattable
    {
        /// <summary>x component of the vector.</summary>
        public readonly double X;
        /// <summary>y component of the vector.</summary>
        public readonly double Y;
        /// <summary>z component of the vector.</summary>
        public readonly double Z;

        /// <summary>double3 zero value.</summary>
        public static readonly double3 Zero = default;

        /// <summary>Constructs a double3 vector from three double values.</summary>
        /// <param name="x">The constructed vector's x component will be set to this value.</param>
        /// <param name="y">The constructed vector's y component will be set to this value.</param>
        /// <param name="z">The constructed vector's z component will be set to this value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>Constructs a double3 vector from a float vector.</summary>
        /// <param name="xyz">The constructed vector's xyz components will be set to this value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double3(Vector3 xyz)
        {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
        }

        /// <summary>Constructs a double3 vector from a single double value by assigning it to every component.</summary>
        /// <param name="v">double to convert to double3</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double3(double v)
        {
            X = v;
            Y = v;
            Z = v;
        }

        /// <summary>Constructs a double3 vector from a single float value by converting it to double and assigning it to every component.</summary>
        /// <param name="v">float to convert to double3</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double3(float v)
        {
            X = v;
            Y = v;
            Z = v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3(double3 v) { return new Vector3((float)v.X, (float)v.Y, (float)v.Z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double3(Vector3 v) { return new double3(v.X, v.Y, v.Z); }

        /// <summary>Implicitly converts a single double value to a double3 vector by assigning it to every component.</summary>
        /// <param name="v">double to convert to double3</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double3(double v) { return new double3(v); }

        /// <summary>Implicitly converts a single float value to a double3 vector by converting it to double and assigning it to every component.</summary>
        /// <param name="v">float to convert to double3</param>
        /// <returns>Converted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double3(float v) { return new double3(v); }

        /// <summary>Returns the result of a componentwise multiplication operation on two double3 vectors.</summary>
        /// <param name="lhs">Left hand side double3 to use to compute componentwise multiplication.</param>
        /// <param name="rhs">Right hand side double3 to use to compute componentwise multiplication.</param>
        /// <returns>double3 result of the componentwise multiplication.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator *(double3 lhs, double3 rhs) { return new double3(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z); }

        /// <summary>Returns the result of a componentwise multiplication operation on a double3 vector and a double value.</summary>
        /// <param name="lhs">Left hand side double3 to use to compute componentwise multiplication.</param>
        /// <param name="rhs">Right hand side double to use to compute componentwise multiplication.</param>
        /// <returns>double3 result of the componentwise multiplication.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator *(double3 lhs, double rhs) { return new double3(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs); }

        /// <summary>Returns the result of a componentwise multiplication operation on a double value and a double3 vector.</summary>
        /// <param name="lhs">Left hand side double to use to compute componentwise multiplication.</param>
        /// <param name="rhs">Right hand side double3 to use to compute componentwise multiplication.</param>
        /// <returns>double3 result of the componentwise multiplication.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator *(double lhs, double3 rhs) { return new double3(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z); }


        /// <summary>Returns the result of a componentwise addition operation on two double3 vectors.</summary>
        /// <param name="lhs">Left hand side double3 to use to compute componentwise addition.</param>
        /// <param name="rhs">Right hand side double3 to use to compute componentwise addition.</param>
        /// <returns>double3 result of the componentwise addition.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator +(double3 lhs, double3 rhs) { return new double3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z); }

        /// <summary>Returns the result of a componentwise addition operation on a double3 vector and a double value.</summary>
        /// <param name="lhs">Left hand side double3 to use to compute componentwise addition.</param>
        /// <param name="rhs">Right hand side double to use to compute componentwise addition.</param>
        /// <returns>double3 result of the componentwise addition.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator +(double3 lhs, double rhs) { return new double3(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs); }

        /// <summary>Returns the result of a componentwise addition operation on a double value and a double3 vector.</summary>
        /// <param name="lhs">Left hand side double to use to compute componentwise addition.</param>
        /// <param name="rhs">Right hand side double3 to use to compute componentwise addition.</param>
        /// <returns>double3 result of the componentwise addition.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator +(double lhs, double3 rhs) { return new double3(lhs + rhs.X, lhs + rhs.Y, lhs + rhs.Z); }


        /// <summary>Returns the result of a componentwise subtraction operation on two double3 vectors.</summary>
        /// <param name="lhs">Left hand side double3 to use to compute componentwise subtraction.</param>
        /// <param name="rhs">Right hand side double3 to use to compute componentwise subtraction.</param>
        /// <returns>double3 result of the componentwise subtraction.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator -(double3 lhs, double3 rhs) { return new double3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z); }

        /// <summary>Returns the result of a componentwise subtraction operation on a double3 vector and a double value.</summary>
        /// <param name="lhs">Left hand side double3 to use to compute componentwise subtraction.</param>
        /// <param name="rhs">Right hand side double to use to compute componentwise subtraction.</param>
        /// <returns>double3 result of the componentwise subtraction.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator -(double3 lhs, double rhs) { return new double3(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs); }

        /// <summary>Returns the result of a componentwise subtraction operation on a double value and a double3 vector.</summary>
        /// <param name="lhs">Left hand side double to use to compute componentwise subtraction.</param>
        /// <param name="rhs">Right hand side double3 to use to compute componentwise subtraction.</param>
        /// <returns>double3 result of the componentwise subtraction.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator -(double lhs, double3 rhs) { return new double3(lhs - rhs.X, lhs - rhs.Y, lhs - rhs.Z); }


        /// <summary>Returns the result of a componentwise division operation on two double3 vectors.</summary>
        /// <param name="lhs">Left hand side double3 to use to compute componentwise division.</param>
        /// <param name="rhs">Right hand side double3 to use to compute componentwise division.</param>
        /// <returns>double3 result of the componentwise division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator /(double3 lhs, double3 rhs) { return new double3(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z); }

        /// <summary>Returns the result of a componentwise division operation on a double3 vector and a double value.</summary>
        /// <param name="lhs">Left hand side double3 to use to compute componentwise division.</param>
        /// <param name="rhs">Right hand side double to use to compute componentwise division.</param>
        /// <returns>double3 result of the componentwise division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator /(double3 lhs, double rhs) { return new double3(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs); }

        /// <summary>Returns the result of a componentwise division operation on a double value and a double3 vector.</summary>
        /// <param name="lhs">Left hand side double to use to compute componentwise division.</param>
        /// <param name="rhs">Right hand side double3 to use to compute componentwise division.</param>
        /// <returns>double3 result of the componentwise division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator /(double lhs, double3 rhs) { return new double3(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z); }


        /// <summary>Returns the result of a componentwise modulus operation on two double3 vectors.</summary>
        /// <param name="lhs">Left hand side double3 to use to compute componentwise modulus.</param>
        /// <param name="rhs">Right hand side double3 to use to compute componentwise modulus.</param>
        /// <returns>double3 result of the componentwise modulus.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator %(double3 lhs, double3 rhs) { return new double3(lhs.X % rhs.X, lhs.Y % rhs.Y, lhs.Z % rhs.Z); }

        /// <summary>Returns the result of a componentwise modulus operation on a double3 vector and a double value.</summary>
        /// <param name="lhs">Left hand side double3 to use to compute componentwise modulus.</param>
        /// <param name="rhs">Right hand side double to use to compute componentwise modulus.</param>
        /// <returns>double3 result of the componentwise modulus.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator %(double3 lhs, double rhs) { return new double3(lhs.X % rhs, lhs.Y % rhs, lhs.Z % rhs); }

        /// <summary>Returns the result of a componentwise modulus operation on a double value and a double3 vector.</summary>
        /// <param name="lhs">Left hand side double to use to compute componentwise modulus.</param>
        /// <param name="rhs">Right hand side double3 to use to compute componentwise modulus.</param>
        /// <returns>double3 result of the componentwise modulus.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator %(double lhs, double3 rhs) { return new double3(lhs % rhs.X, lhs % rhs.Y, lhs % rhs.Z); }

        /// <summary>Returns the result of a componentwise unary minus operation on a double3 vector.</summary>
        /// <param name="val">Value to use when computing the componentwise unary minus.</param>
        /// <returns>double3 result of the componentwise unary minus.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator -(double3 val) { return new double3(-val.X, -val.Y, -val.Z); }


        /// <summary>Returns the result of a componentwise unary plus operation on a double3 vector.</summary>
        /// <param name="val">Value to use when computing the componentwise unary plus.</param>
        /// <returns>double3 result of the componentwise unary plus.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double3 operator +(double3 val) { return new double3(+val.X, +val.Y, +val.Z); }

        /// <summary>Swizzles the vector.</summary>
        public double3 xxx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(X, X, X);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 xxy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(X, X, Y);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 xxz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(X, X, Z);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 xyx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(X, Y, X);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 xyy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(X, Y, Y);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 xyz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(X, Y, Z);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 xzx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(X, Z, X);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 xzy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(X, Z, Y);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 xzz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(X, Z, Z);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 yxx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Y, X, X);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 yxy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Y, X, Y);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 yxz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Y, X, Z);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 yyx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Y, Y, X);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 yyy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Y, Y, Y);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 yyz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Y, Y, Z);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 yzx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Y, Z, X);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 yzy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Y, Z, Y);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 yzz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Y, Z, Z);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 zxx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Z, X, X);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 zxy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Z, X, Y);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 zxz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Z, X, Z);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 zyx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Z, Y, X);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 zyy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Z, Y, Y);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 zyz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Z, Y, Z);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 zzx
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Z, Z, X);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 zzy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Z, Z, Y);
        }


        /// <summary>Swizzles the vector.</summary>
        public double3 zzz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Z, Z, Z);
        }

        /// <summary>Returns the double element at a specified index.</summary>
        public double this[int index]
        {
            get
            {

                if ((uint)index >= 3)
                    throw new ArgumentException("index must be between[0...2]");

                unsafe
                {
                    fixed (double3* array = &this)
                    {
                        return ((double*)array)[index];
                    }
                }
            }
            set
            {

                if ((uint)index >= 3)
                    throw new ArgumentException("index must be between[0...2]");

                unsafe
                {
                    fixed (double* array = &X)
                    {
                        array[index] = value;
                    }
                }
            }
        }

        /// <summary>Returns true if the double3 is equal to a given double3, false otherwise.</summary>
        /// <param name="rhs">Right hand side argument to compare equality with.</param>
        /// <returns>The result of the equality comparison.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(double3 rhs)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return X == rhs.X && Y == rhs.Y && Z == rhs.Z;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(double3 lhs, double3 rhs)
        {
            return lhs.Equals(rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(double3 lhs, double3 rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>Returns true if the double3 is equal to a given double3, false otherwise.</summary>
        /// <param name="o">Right hand side argument to compare equality with.</param>
        /// <returns>The result of the equality comparison.</returns>
        public override bool Equals(object? o) { return o is double3 converted && Equals(converted); }


        /// <summary>Returns a hash code for the double3.</summary>
        /// <returns>The computed hash code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return unchecked(X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode());

        }


        /// <summary>Returns a string representation of the double3.</summary>
        /// <returns>String representation of the value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"double3({X}, {Y}, {Z})";
        }

        /// <summary>Returns a string representation of the double3 using a specified format and culture-specific format information.</summary>
        /// <param name="format">Format string to use during string formatting.</param>
        /// <param name="formatProvider">Format provider to use during string formatting.</param>
        /// <returns>String representation of the value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return $"double3({X.ToString(format, formatProvider)}, {Y.ToString(format, formatProvider)}, {Z.ToString(format, formatProvider)})";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double LengthSquared()
        {
            return X * X
                 + Y * Y
                 + Z * Z;
        }

        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        public double3 Abs()
        {
            return new double3(
                Math.Abs(X),
                Math.Abs(Y),
                Math.Abs(Z)
            );
        }

        public static double3 Lerp(double3 start, double3 end, double t)
        {
            return new double3(
                double.Lerp(start.X, end.X, t),
                double.Lerp(start.Y, end.Y, t),
                double.Lerp(start.Z, end.Z, t)
            );
        }
    }
}

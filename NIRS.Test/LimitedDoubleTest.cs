using MyDouble;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NIRS.Test
{
    public class LimitedDoubleTest
    {
        private const double eps = 1e-9;

        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_plus_half()
        {
            LimitedDouble a = new LimitedDouble(1.5);
            LimitedDouble b = new LimitedDouble(2.5);

            var c = a + b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - 4) && c.IsInt());
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_plus_Int()
        {
            LimitedDouble a = new LimitedDouble(1.5);
            LimitedDouble b = new LimitedDouble(2.0);

            var c = a + b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - 3.5) && c.IsHalfInt());
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_plus_half()
        {
            LimitedDouble a = new LimitedDouble(1.0);
            LimitedDouble b = new LimitedDouble(2.5);

            var c = a + b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - 3.5) && c.IsHalfInt());
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_plus_Int()
        {
            LimitedDouble a = new LimitedDouble(1.0);
            LimitedDouble b = new LimitedDouble(2.0);

            var c = a + b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - 3) && c.IsInt());
        }



        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_minus_half()
        {
            LimitedDouble a = new LimitedDouble(1.5);
            LimitedDouble b = new LimitedDouble(2.5);

            var c = a - b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - -1) && c.IsInt());
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_minus_Int()
        {
            LimitedDouble a = new LimitedDouble(1.5);
            LimitedDouble b = new LimitedDouble(2.0);

            var c = a - b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - -0.5) && c.IsHalfInt());
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_minus_half()
        {
            LimitedDouble a = new LimitedDouble(1.0);
            LimitedDouble b = new LimitedDouble(2.5);

            var c = a - b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - -1.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_minus_Int()
        {
            LimitedDouble a = new LimitedDouble(1.0);
            LimitedDouble b = new LimitedDouble(2.0);

            var c = a - b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - -1));
        }


        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_plus_Doublehalf()
        {
            LimitedDouble a = new LimitedDouble(1.5);
            double b = 1.5;

            var c = a + b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - 3));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_plus_DoubleInt()
        {
            LimitedDouble a = new LimitedDouble(1.5);
            double b = 1;

            var c = a + b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - 2.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_plus_Doublehalf()
        {
            LimitedDouble a = new LimitedDouble(1.0);
            double b = 1.5;

            var c = a + b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - 2.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_plus_DoubleInt()
        {
            LimitedDouble a = new LimitedDouble(1.0);
            double b = 1;

            var c = a + b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - 2));
        }



        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_minus_Doublehalf()
        {
            LimitedDouble a = new LimitedDouble(1.5);
            double b = 1.5;

            var c = a - b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - 0));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_minus_DoubleInt()
        {
            LimitedDouble a = new LimitedDouble(1.5);
            double b = 1;

            var c = a - b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - 0.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_minus_Doublehalf()
        {
            LimitedDouble a = new LimitedDouble(1.0);
            double b = 1.5;

            var c = a - b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - -0.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_minus_DoubleInt()
        {
            LimitedDouble a = new LimitedDouble(1.0);
            double b = 1;

            var c = a - b;

            Assert.True(eps >= Math.Abs(c.GetDouble() - 0));
        }



        [Fact]
        public void checking_the_limitedDouble_initializing_and_return_of_double_a_negative_DoubleInt()
        {
            LimitedDouble a = new LimitedDouble(-1.0);

            var c = a.GetDouble();

            Assert.True(eps >= Math.Abs(c - -1));
        }
        [Fact]
        public void checking_the_limitedDouble_initializing_and_return_of_int_a_negative_DoubleInt()
        {
            LimitedDouble a = new LimitedDouble(-1.0);

            var c = a.GetInt();

            Assert.True(c == -1);
        }
    }
}

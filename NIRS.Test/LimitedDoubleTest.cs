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
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 5);
            LimitedDouble b = new LimitedDouble(2, 5);

            // Act
            var c = a + b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - 4));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_plus_Int()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 5);
            LimitedDouble b = new LimitedDouble(2, 0);

            // Act
            var c = a + b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - 3.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_plus_half()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 0);
            LimitedDouble b = new LimitedDouble(2, 5);

            // Act
            var c = a + b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - 3.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_plus_Int()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 0);
            LimitedDouble b = new LimitedDouble(2, 0);

            // Act
            var c = a + b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - 3));
        }



        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_minus_half()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 5);
            LimitedDouble b = new LimitedDouble(2, 5);

            // Act
            var c = a - b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - -1));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_minus_Int()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 5);
            LimitedDouble b = new LimitedDouble(2, 0);

            // Act
            var c = a - b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - -0.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_minus_half()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 0);
            LimitedDouble b = new LimitedDouble(2, 5);

            // Act
            var c = a - b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - -1.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_minus_Int()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 0);
            LimitedDouble b = new LimitedDouble(2, 0);

            // Act
            var c = a - b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - -1));
        }


        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_plus_Doublehalf()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 5);
            double b = 1.5;

            // Act
            var c = a + b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - 3));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_plus_DoubleInt()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 5);
            double b = 1;

            // Act
            var c = a + b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - 2.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_plus_Doublehalf()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 0);
            double b = 1.5;

            // Act
            var c = a + b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - 2.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_plus_DoubleInt()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 0);
            double b = 1;

            // Act
            var c = a + b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - 2));
        }



        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_minus_Doublehalf()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 5);
            double b = 1.5;

            // Act
            var c = a - b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - 0));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_half_minus_DoubleInt()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 5);
            double b = 1;

            // Act
            var c = a - b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - 0.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_minus_Doublehalf()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 0);
            double b = 1.5;

            // Act
            var c = a - b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - -0.5));
        }
        [Fact]
        public void checking_the_limitedDouble_arithmetic_Int_minus_DoubleInt()
        {
            // Arrange
            LimitedDouble a = new LimitedDouble(1, 0);
            double b = 1;

            // Act
            var c = a - b;

            // Assert
            Assert.True(eps >= Math.Abs(c.Double() - 0));
        }
    }
}

using NIRS.Interfaces;
using NIRS.Barrel_Folder;
using NIRS.Helpers;
using System.Collections.Generic;
using Xunit;
using System.Linq;


namespace NIRS.Test
{
    public class BarrelSizeTest
    {
        private IBarrel GetBarrel()
        {
            List<Point2D> bendingPoints = new List<Point2D>
            {
                new Point2D(0,2),
                new Point2D(1,2),
                new Point2D(3,1),
                new Point2D(5,1)
            };
            Point2D endChamber = new Point2D(3, 1);
            IBarrel barrel = new Barrel(bendingPoints,endChamber,Dimension.R);
            return barrel;
        }
        [Fact]
        public void checking_the_radius_value()
        {
            // Arrange
            var barrel = GetBarrel();
            var bs = barrel.BarrelSize;
            List<double> Rs = new List<double>();
            var lastCoordinate = 5;

            // Act
            for(int i = 0; i <= lastCoordinate; i++)
            {
                Rs.Add(bs.R(i));
            }

            // Assert
            Assert.Equal(Rs,new List<double> {2,2,1.5,1,1,1});
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Compatibility;

namespace TagsCloudVisualization
{
	[TestFixture]
	class CircularCloudLayouterTests : CircularCloudLayouterTestsBase
	{

		[Test]
		public void WithValidArguments_CircularCloudLayouterConstructor_NotThrowingException()
		{
			var center = new Point(1, 1);
			var act = new Action(() => new CircularCloudLayouter(center));
			act.ShouldNotThrow();
		}

		[TestCase(20, 10)]
		[TestCase(30, 90)]
		public void ForValidSizeOfFirsRectangle_GetNewCoordinates_Returns_ValidPosition(int width, int height)
		{
			var rectangleSize = new Size(width, height);
			var rectangleLocationShouldBe = new Point(ccl.Center.X - (int)Math.Round(width / 2.0), ccl.Center.Y - (int)Math.Round(height / 2.0));
			var rectangle = ccl.PutNextRectangle(rectangleSize);
			rectangle.Location.ShouldBeEquivalentTo(rectangleLocationShouldBe);
		}

		[TestCase(20, 10)]
		public void ForValidSizeOfRectangle_GetCoordinatesForRectangleInTheCenter_ReturnsValidCoordinates(int width, int height)
		{
			var rectangleLocationShouldBe = new Point(ccl.Center.X - (int)Math.Round(width / 2.0), ccl.Center.Y - (int)Math.Round(height / 2.0));
			var coordinates = ccl.GetCoordinatesForRectangleInTheCenter(new Size(width, height));
			coordinates.ShouldBeEquivalentTo(rectangleLocationShouldBe);
		}

		[Test]
		public void AfterFirstRectanglePositioning_CurrentCoordinates_ShouldBeNotZeroAndNotNull()
		{
			var zeroCoordinates = new RadiusAndAngleData(0, 0);
			ccl.PutNextRectangle(new Size(120, 30));
			var currentCoordinates = ccl.CurrentRadiusAndAngles;

			currentCoordinates.Should().NotBeNull();
			currentCoordinates.Should().NotBe(zeroCoordinates);
		}

		[Test]
		public void AfterSecondRectanglePositioning_CurrentCoordinates_ShouldBeNotZero()
		{
			ccl.PutNextRectangle(new Size(10, 40));
			var pastCoordinates = (RadiusAndAngleData)ccl.CurrentRadiusAndAngles.Clone();
			ccl.PutNextRectangle(new Size(120, 30));
			var currentCoordinates = ccl.CurrentRadiusAndAngles;

			currentCoordinates.Should().NotBe(pastCoordinates);
		}

		[TestCase(20, 10)]
		[TestCase(300, 400)]
		public void DuringFirstRectangleAddition_PutNextRectangle_RegistersRectangle(int width, int height)
		{
			var rectangleLocation = ccl.PutNextRectangle(new Size(width, height)).Location;
			for (var x = rectangleLocation.X; x < rectangleLocation.X + width; x++)
				for (var y = rectangleLocation.Y; y < rectangleLocation.Y - width; y--)
					ccl.MatrixOfUsedPixels[x, y].Should().BeTrue();
		}


		[TestCase(600, 740, 280, 470)]
		[TestCase(260, 310, 320, 420)]
		[TestCase(35, 55, 135, 155)]
		public void ForSameRectangles_CheckPixelsAreFree_ReturnsFalse(int minX, int maxX, int minY, int maxY)
		{
			PrepairForTestingBoundaryCheckingMethod(minX, maxX, minY, maxY);

			var rectangleSize = new Size(maxX - minX, maxY - minY);
			var rectanglePosition = new Point(minX, minY);
			ccl.CheckPixelsAreFree(rectanglePosition, rectangleSize).Should().BeFalse();
		}

		[TestCase(600, 740, 280, 470)]
		[TestCase(260, 310, 320, 420)]
		[TestCase(35, 55, 135, 155)]
		public void ForBiggerRectangles_CheckPixelsAreFree_ReturnsFalse(int minX, int maxX, int minY, int maxY)
		{
			PrepairForTestingBoundaryCheckingMethod(minX, maxX, minY, maxY);

			var rectangleSize = new Size(maxX - minX + 2, maxY - minY + 2);
			var rectanglePosition = new Point(minX - 1, maxY - 1);
			ccl.CheckPixelsAreFree(rectanglePosition, rectangleSize).Should().BeFalse();
		}

		[TestCase(600, 740, 280, 470)]
		[TestCase(260, 310, 320, 420)]
		[TestCase(35, 55, 135, 155)]
		public void ForSmallerRectangles_CheckPixelsAreFree_ReturnsFalse(int minX, int maxX, int minY, int maxY)
		{
			PrepairForTestingBoundaryCheckingMethod(minX, maxX, minY, maxY);

			var rectangleSize = new Size(maxX - minX - 2, maxY - minY - 2);
			var rectanglePosition = new Point(minX + 1, minY + 1);
			ccl.CheckPixelsAreFree(rectanglePosition, rectangleSize).Should().BeFalse();
		}

		[TestCase(400, 440, 300, 320)]
		[TestCase(250, 300, 300, 400)]
		[TestCase(10, 20, 10, 20)]
		public void ForBothBoundariesOfSameRectangle_CheckRectangleBoundariesAreFree_ReturnsFalse(int minX, int maxX, int minY, int maxY)
		{
			PrepairForTestingBoundaryCheckingMethod(minX, maxX, minY, maxY);

			var rectangleSize = new Size(maxX - minX, maxY - minY);
			ccl.CheckRectangleBoundariesAreFree(minX, maxX, rectangleSize, minY, maxY, true).Should().BeFalse();
			ccl.CheckRectangleBoundariesAreFree(minY, maxY, rectangleSize, minX, maxX, false).Should().BeFalse();
		}

		[TestCase(400, 440, 300, 320)]
		[TestCase(250, 300, 300, 400)]
		[TestCase(10, 20, 10, 20)]
		public void ForBothBoundariesOfBiggerRectangle_CheckRectangleBoundariesAreFree_ReturnsTrue(int minX, int maxX, int minY, int maxY)
		{
			PrepairForTestingBoundaryCheckingMethod(minX, maxX, minY, maxY);

			var BiggerRectangleSize = new Size(maxX - minX + 2, maxY - minY + 2);
			ccl.CheckRectangleBoundariesAreFree(minX - 1, maxX, BiggerRectangleSize, minY-1, maxY, true).Should().BeTrue();
			ccl.CheckRectangleBoundariesAreFree(minY - 1, maxY, BiggerRectangleSize, minX - 1, maxX, false).Should().BeTrue();
		}


		[TestCase(400, 440, 300, 320)]
		[TestCase(250, 300, 300, 400)]
		[TestCase(10, 20, 10, 20)]
		public void ForRectangleOverPrevious_CheckRectangleInsideIsFree_ReturnsFalse(int minX, int maxX, int minY, int maxY)
		{
			PrepairForTestingBoundaryCheckingMethod(minX, maxX, minY, maxY);

			ccl.CheckRectangleInsideIsFree(minX - 1, maxX, minY, maxY + 1, 5).Should().BeFalse();
		}

		[TestCase(400, 440, 300, 320)]
		[TestCase(250, 300, 300, 400)]
		[TestCase(10, 20, 10, 20)]
		public void ForRectangleInsidePrevious_CheckRectangleInsideIsFree_ReturnsFalse(int minX, int maxX, int minY, int maxY)
		{
			PrepairForTestingBoundaryCheckingMethod(minX, maxX, minY, maxY);

			ccl.CheckRectangleInsideIsFree(minX + 3, maxX - 4, minY + 3, maxY - 4, 5).Should().BeFalse();
		}

		[TestCase(70)]
		public void DegreeToCortesianCoordinates_ReturnValidRadians(int degrees)
		{
			ccl.DegreeesToRadians(degrees).ShouldBeEquivalentTo(Math.PI * degrees / 180);
		}

		[Test]
		public void PolarToCortesianCoordinates_ReturnValidCoordinates()
		{
			ccl.PutNextRectangle(new Size(1, 1));
			var x = ccl.Center.X + Math.Round(ccl.CurrentRadiusAndAngles.Radius * Math.Cos(ccl.CurrentRadiusAndAngles.Angle));
			var y = ccl.Center.Y + Math.Round(ccl.CurrentRadiusAndAngles.Radius * Math.Sin(ccl.CurrentRadiusAndAngles.Angle));
			var resultShouldBe = new Point((int)x, (int)y);
			ccl.PolarToCortesianCoordinates().ShouldBeEquivalentTo(resultShouldBe);
		}
	}
}

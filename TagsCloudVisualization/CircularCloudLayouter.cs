using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
	class CircularCloudLayouter
	{
		public Size BmpResolution { get; set; }
		public RadiusAndAngleData RadiusAndAngleDeltas { get; private set; }
		public RadiusAndAngleData CurrentRadiusAndAngles { get; private set; }
		public bool[,] MatrixOfUsedPixels;
		public Point Center { get; private set; }

		public CircularCloudLayouter(Point center)
		{
			Center = center;
			RadiusAndAngleDeltas = new RadiusAndAngleData(1, DegreeesToRadians(7));
		}

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			if (MatrixOfUsedPixels == null)
				MatrixOfUsedPixels = new bool[BmpResolution.Width, BmpResolution.Height];
			var position = GetNewCoordinates(rectangleSize);
			var result = new Rectangle(position, rectangleSize);

			RegisterRectangle(result);
			return result;
		}

		public Point GetNewCoordinates(Size rectangleSize)
		{
			Point? result = null;
			while (result == null)
			{
				if (CurrentRadiusAndAngles == null)
					result = GetCoordinatesForRectangleInTheCenter(rectangleSize);
				
				else
				{
					result = GetCoordinatesForRectangle(rectangleSize);
					ChangeRadiusAndAngle();
				}
			}
			return (Point)result;
		}

		public Point GetCoordinatesForRectangleInTheCenter(Size rectangleSize)
		{
			CurrentRadiusAndAngles = new RadiusAndAngleData((int)Math.Round(rectangleSize.Height / 2.0), DegreeesToRadians(90));
			return new Point((int)Math.Round(Center.X - rectangleSize.Width / 2.0), (int)Math.Round(Center.Y - rectangleSize.Height / 2.0));
		}

		public Point? GetCoordinatesForRectangle(Size rectangleSize)
		{
			var cortesianCoordinates = PolarToCortesianCoordinates();
			var pixelsAreFree = CheckPixelsAreFree(cortesianCoordinates, rectangleSize);
			if (pixelsAreFree)
				return cortesianCoordinates;
			return null;
		}

		public bool CheckPixelsAreFree(Point rectanglePosition, Size rectangleSize)
		{
			var minX = rectanglePosition.X-1;
			var maxX = rectanglePosition.X + rectangleSize.Width +1;
			var minY = rectanglePosition.Y-1;
			var maxY = rectanglePosition.Y + rectangleSize.Height + 1;

			if (!CheckRectangleBoundariesAreFree(minX, maxX, rectangleSize, minY, maxY, true))
				return false;

			if (!CheckRectangleBoundariesAreFree(minY, maxY, rectangleSize, minX, maxX, false))
				return false;

			var checkIterationSize = Math.Min( (int)Math.Round(rectangleSize.Width/10f), (int)Math.Round(rectangleSize.Height/10f));
			if (!CheckRectangleInsideIsFree(minX, maxX, minY, maxY, checkIterationSize))
				return false;

			return true;
		}

		public bool CheckRectangleBoundariesAreFree(int minI, int maxI, Size rectangleSize, int minJ, int maxJ, bool iIsX)
		{
			var deltaI = iIsX ? rectangleSize.Width : rectangleSize.Height;
			for (int i = Math.Max(0, minI); i <= maxI; i += deltaI)
			{
				if (iIsX && i >= BmpResolution.Width || !iIsX && i >= BmpResolution.Height)
					continue;
				for (int j = Math.Max(0, minJ); j <= maxJ; j++)
				{
					if (!iIsX && j >= BmpResolution.Width || iIsX && j >= BmpResolution.Height)
						continue;
					if (iIsX && MatrixOfUsedPixels[i, j] || !iIsX && MatrixOfUsedPixels[j, i])
						return false;
				}
			}
			return true;
		}

		public bool CheckRectangleInsideIsFree(int minX, int maxX, int minY, int maxY, int stepSize)
		{
			for (int x = Math.Max(0, minX); x < Math.Min(BmpResolution.Width, maxX + 1); x += stepSize)
			{
				for (int y = Math.Max(0, minY); y < Math.Min(BmpResolution.Height, maxY + 1); y += stepSize)
				{
					if (MatrixOfUsedPixels[x, y])
						return false;
				}
			}
			return true;
		}

		public void ChangeRadiusAndAngle()
		{
			CurrentRadiusAndAngles.Angle += RadiusAndAngleDeltas.Angle;
			if (CurrentRadiusAndAngles.Angle >= 2 * Math.PI || CurrentRadiusAndAngles.Radius == 0)
			{
				CurrentRadiusAndAngles.Angle %= 2 * Math.PI;
				CurrentRadiusAndAngles.Radius += RadiusAndAngleDeltas.Radius;
			}
		}

		public void RegisterRectangle(Rectangle rectangle)
		{
			for (int i = Math.Max(0, rectangle.X); i < Math.Min(BmpResolution.Width, rectangle.X + rectangle.Width); i++)
			{
				for (int j = Math.Max(0, rectangle.Y); j < Math.Min(BmpResolution.Height, rectangle.Y + rectangle.Height); j++)
				{
					MatrixOfUsedPixels[i, j] = true;
				}
			}
		}

		public double DegreeesToRadians(double degrees)
		{
			return Math.PI * degrees / 180;
		}

		public Point PolarToCortesianCoordinates()
		{
			var x = Center.X + Math.Round(CurrentRadiusAndAngles.Radius * Math.Cos(CurrentRadiusAndAngles.Angle));
			var y = Center.Y - Math.Round(CurrentRadiusAndAngles.Radius * Math.Sin(CurrentRadiusAndAngles.Angle));
			return new Point((int)x, (int)y);
		}

	}


}

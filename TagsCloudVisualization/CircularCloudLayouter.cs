using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
	class CircularCloudLayouter
	{
		private List<Rectangle> InsertedRectangles;
		private Spiral spiral;

		public Point Center { get; }

		public CircularCloudLayouter(Point center)
		{
			Center = center;
			spiral = new Spiral();
			InsertedRectangles = new List<Rectangle>();
		}

		public Rectangle PutNextRectangle(Size rectangleSize)
		{
			while (true)
			{
				var position = GetRectangleCoordinates(rectangleSize);
				var result = new Rectangle(position, rectangleSize);
				if (CheckForIntersectionWithPreviousRectangles(result))
					continue;

				RegisterRectangle(result);
				return result;
			}
		}

		
		private Point GetRectangleCoordinates(Size rectangleSize)
		{
			if (InsertedRectangles.Count == 0)
				return GetCoordinatesForRectangleInTheCenter(rectangleSize);

			return GetCoordinatesForRectangle(rectangleSize);
		}

		public Point GetCoordinatesForRectangleInTheCenter(Size rectangleSize)
		{
			return new Point((int)Math.Round(Center.X - rectangleSize.Width / 2.0), (int)Math.Round(Center.Y - rectangleSize.Height / 2.0));
		}

		public Point GetCoordinatesForRectangle(Size rectangleSize)
		{
			spiral.UpdateCoordinates();
			return PolarToCortesianCoordinates(spiral.Radius, spiral.Angle);
			
		}

		public bool CheckForIntersectionWithPreviousRectangles(Rectangle rectangle)
		{
			foreach (var previousRectangle in InsertedRectangles)
			{
				if (rectangle.IntersectsWith(previousRectangle))
					return true;
			}
			return false;
		}

		public void RegisterRectangle(Rectangle rectangle)
		{
			InsertedRectangles.Add(rectangle);
		}

		public Point PolarToCortesianCoordinates(int radius, int angle)
		{
			var x = Center.X + Math.Round(radius * Math.Cos(CommonUsefulMethods.DegreeesToRadians(angle)));
			var y = Center.Y - Math.Round(radius * Math.Sin(CommonUsefulMethods.DegreeesToRadians(angle)));
			return new Point((int)x, (int)y);
		}

	}


}

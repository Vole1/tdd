using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
	public class Spiral
	{
		public int Radius { get; private set; }
		private const int deltaRadius = 3;
		public int Angle { get; private set; }
		private const int deltaAngle = 7;
		public Spiral()
		{
			Radius = 0;
			Angle = 0;
		}

		public void UpdateCoordinates()
		{
			Angle += deltaAngle;
			if (Angle >= 360)
			{
				Angle %= 360;
				Radius += deltaRadius;
			}
		}
	}
}

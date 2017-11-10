using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
	public class RadiusAndAngleData : ICloneable
	{
		public double Radius { get; set; }
		public double Angle { get; set; }

		public RadiusAndAngleData(double radiusIn, double angleIn)
		{
			Radius = radiusIn;
			Angle = angleIn;
		}


		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}

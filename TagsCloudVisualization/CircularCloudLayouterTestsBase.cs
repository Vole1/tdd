using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TagsCloudVisualization
{
	class CircularCloudLayouterTestsBase
	{
		protected CircularCloudLayouter ccl;
		[SetUp]
		public void SetUp()
		{
			ccl = new CircularCloudLayouter(new Point(300, 300));
			ccl.BmpResolution = new Size(1200, 1000);
			ccl.MatrixOfUsedPixels = new bool[ccl.BmpResolution.Width, ccl.BmpResolution.Height];
		}

		protected void PrepairForTestingBoundaryCheckingMethod(int minX, int maxX, int minY, int maxY)
		{
			var rectangleToRegister = new Rectangle(minX, minY, maxX - minX, maxY - minY);
			ccl.RegisterRectangle(rectangleToRegister);
		}
	}
}

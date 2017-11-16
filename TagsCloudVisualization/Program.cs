using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
	class Program
	{
		public static void Main()
		{
			var rectangles = GetSizes();
			Visualizer.Visualize(rectangles);
		}

		private static IEnumerable<Size> GetSizes()
		{
			var rnd = new Random();

			//var coefficient = 0.991;
			var coefficient = 1;

			var result = new Size[100];
			for (var i = 0; i < 100; i++)
			{
				//var width = rnd.Next(80, 120);
				//var height = rnd.Next(40, 60);

				var width = rnd.Next(30, 100);
				var height = rnd.Next(30, 100);

				var currentWidth = (int)Math.Round(width * Math.Pow(coefficient, i));
				var currentHeight = (int)Math.Round(height * Math.Pow(coefficient, i));
				result[i] = new Size(currentWidth, currentHeight);
			}
			return result;
		}
	}
}

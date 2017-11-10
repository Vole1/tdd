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
			//var width = rnd.Next(150, 190);
			//var height = rnd.Next(60, 70);
			var height = rnd.Next(100, 150);
			var width = rnd.Next(60, 70);

			var coefficient = 0.995;

			var result = new Size[100];
			for (var i = 0; i < 100; i++)
			{
				//var width = rnd.Next(30, 100);
				//var height = rnd.Next(30, 100);
				var currentWidth = (int)Math.Round(width * Math.Pow(coefficient, i));
				var currentHeight = (int)Math.Round(height * Math.Pow(coefficient, i));
				result[i] = new Size(currentWidth, currentHeight);
			}
			return result;
		}
	}
}

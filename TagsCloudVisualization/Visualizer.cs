using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
	static class Visualizer
	{
		private static Size resolution { get; set; }
		private static Bitmap bmp { get; set; }

		public static Graphics DrawingGraphics { get; set; }

		public static CircularCloudLayouter Ccl { get; set; }
		private static Point center { get; set; }

		static Visualizer()
		{
			resolution = new Size(1200, 1000);
			center = new Point(resolution.Width / 2, resolution.Height / 2);
			bmp = new Bitmap(resolution.Width, resolution.Height);
			DrawingGraphics = Graphics.FromImage(bmp);
			DrawingGraphics.Clear(Color.Black);
		}

		public static void Visualize(IEnumerable<Size> sizes)
		{
			var ccl = new CircularCloudLayouter(center);
			ccl.BmpResolution = resolution;
			foreach (var size in sizes)
			{
				var rect = ccl.PutNextRectangle(size);
				DrawingGraphics.DrawRectangle(new Pen(Color.Aqua), rect);
			}
			bmp.Save("picture3.bmp");
		}
	}
}

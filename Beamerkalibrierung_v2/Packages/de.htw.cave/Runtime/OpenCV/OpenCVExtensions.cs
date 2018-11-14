using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenCvSharp;

namespace Htw.Cave.OpenCV
{
    public static class OpenCVExtensions
    {
		public static Vector2 ToVector2(this Point2d source)
		{
			return new Vector2((float)source.X, (float)source.Y);
		}

        public static IEnumerable<Vector2> ToVector2(this IEnumerable<Point2d> source)
        {
            return source.Select(point => point.ToVector2());
        }

		public static float[,] ToFloats(this Mat mat)
		{
			double[,] data = new double[mat.Rows, mat.Cols];
			mat.GetArray(0, 0, data);

			return new float[,] {
				{(float)data[0, 0], (float)data[0, 1], (float)data[0, 2]},
				{(float)data[1, 0], (float)data[1, 1], (float)data[1, 2]},
				{(float)data[2, 0], (float)data[2, 1], (float)data[2, 2]}
			};
		}
	}
}

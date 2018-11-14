using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenCvSharp;

namespace Htw.Cave.OpenCV
{
    public static class UnityExtensions
    {
        public static Point2d ToPoint2d(this Vector2 source)
        {
            return new Point2d((double)source.x, (double)source.y);
        }

        public static IEnumerable<Point2d> ToPoint2d(this IEnumerable<Vector2> source)
        {
            return source.Select(vector => vector.ToPoint2d());
        }
	}
}

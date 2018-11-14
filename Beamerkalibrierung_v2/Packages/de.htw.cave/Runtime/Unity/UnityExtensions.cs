using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenCvSharp;

namespace Htw.Cave.Unity
{
    public static class UnityExtensions
    {
		public static float[,] ToFloats(this Matrix4x4 mat)
		{
			// OPTIMIZE
			return new float[,] {
				{mat[0, 0], mat[0, 1], mat[0, 2]},
				{mat[1, 0], mat[1, 1], mat[1, 2]},
				{mat[2, 0], mat[2, 1], mat[2, 2]}
			};
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenCvSharp;
using Htw.Cave.Math;
using Htw.Cave.OpenCV;

namespace Htw.Cave
{
    public class Testing : MonoBehaviour
    {
		public void Awake()
		{
			Vector2[] source = {
				new Vector2(-5f, -5f),
				new Vector2(5f, -5f),
				new Vector2(-5f, 5f),
				new Vector2(5f, 5f)
			};

			Vector2[] destination = {
				new Vector2(-4f, -6f),
				new Vector2(5f, -6f),
				new Vector2(-4f, 5f),
				new Vector2(5f, 5f)
			};

			float[,] mult = Cv2.FindHomography(source.ToPoint2d(), destination.ToPoint2d()).ToFloats();

			for (int i = 0; i < mult.GetLength(0); i++)
    			for (int j = 0; j < mult.GetLength(1); j++)
        			Debug.Log(mult[i, j]);

			Matrix4x4 mat = Projection.Bimber(Homography.Find(source, destination));
			Debug.Log(mat);

            Projection.Screen screen = new Projection.Screen();
            //Projection.Homography(screen.NearPlane(new Transform(), 0f, 0f));
		}

        public void Update()
		{

		}
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect;

namespace Htw.Cave.Kinect
{
    public static class KinectExtensions
    {
		public static UnityEngine.Vector4 ToUnityVector4(this Windows.Kinect.Vector4 vector)
		{
			return new UnityEngine.Vector4(vector.X, vector.Y, vector.Z, vector.W);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenCvSharp;
using Htw.Cave.OpenCV;

namespace Htw.Cave.Math
{
    public static class Homography
    {
        public static float[,] Find(IEnumerable<Vector2> source, IEnumerable<Vector2> destination)
        {
            // https://www.learnopencv.com/image-alignment-feature-based-using-opencv-c-python/
            // https://github.com/opencv/opencv/blob/master/modules/calib3d/src/fundam.cpp
            // https://github.com/shimat/opencvsharp
            // https://github.com/shimat/opencvsharp/blob/a0d61065240eada9706fb2df2bf7ea5ce8f599d7/src/OpenCvSharp/Cv2/Cv2_calib3d.cs

            return Cv2.FindHomography(source.ToPoint2d(), destination.ToPoint2d()).ToFloats();
        }
    }
}

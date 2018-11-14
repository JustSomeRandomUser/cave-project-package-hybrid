using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Htw.Cave.Math
{
    public static partial class Projection
    {
        public static Matrix4x4 Holographic(float[,] nearPlane)
        {
            //       plane matrix P 3x2      holographic matrix H 4x4
            //       column = vertical            row = vertical
            //            |  0   1              |  0   1   2   3
            //          --+--------           --+----------------
            //   row    0 | -x   x    column  0 | H00 H10  0  H20
            //    =     1 | -y   y      =     1 | H01 H11  0  H21
            // horizont 2 |  n   f   horizont 2 |  0   0   d   0
            //                                3 | H02 H12  0   1
            //
            // https://en.wikibooks.org/wiki/Cg_Programming/Unity/Projection_for_Virtual_Reality
            // https://en.wikibooks.org/wiki/Cg_Programming/Vertex_Transformations
            // https://csc.lsu.edu/~kooima/pdfs/gen-perspective.pdf
            // https://github.com/joshferns/KinectHolographic

            float left = nearPlane[0, 0];
            float right = nearPlane[0, 1];
            float bottom = nearPlane[1, 0];
            float top = nearPlane[1, 1];
            float near = nearPlane[2, 0];
            float far = nearPlane[2, 1];

            Matrix4x4 holographic = new Matrix4x4();

            holographic[0] = (2f * near) / (right - left);
            holographic[1] = 0f;
            holographic[2] = 0f;
            holographic[3] = 0f;
            holographic[4] = 0f;
            holographic[5] = (2f * near) / (top - bottom);
            holographic[6] = 0f;
            holographic[7] = 0f;
            holographic[8] = (right + left) / (right - left);
            holographic[9] = (top + bottom) / (top - bottom);
            holographic[10] = -(far + near) / (far - near);
            holographic[11] = -1f;
            holographic[12] = 0f;
            holographic[13] = 0f;
            holographic[14] = -(2f * far * near) / (far - near);
            holographic[15] = 0f;

            return holographic;
        }

        public static Matrix4x4 Bimber(float[,] homography)
        {
            //          bimber matrix B 4x4
            //             row = vertical
            //            |  0   1   2   3
            //          --+----------------
            //  column  0 | H00 H10  0  H20
            //    =     1 | H01 H11  0  H21
            // horizont 2 |  0   0   d   0
            //          3 | H02 H12  0   1
            //
            // with H = homography matrix
            // approximation of depth buffer with d = 1-|H20|-|H21|
            // https://www.youtube.com/watch?v=fVJeJMWZcq8

            Matrix4x4 bimber = new Matrix4x4();

            bimber[0] = homography[0, 0];
            bimber[1] = homography[1, 0];
            bimber[2] = 0f;
            bimber[3] = homography[2, 0];
            bimber[4] = homography[0, 1];
            bimber[5] = homography[1, 1];
            bimber[6] = 0f;
            bimber[7] = homography[2, 1];
            bimber[8] = 0f;
            bimber[9] = 0f;
            bimber[10] = 1f - (bimber[3] < 0f ? -bimber[3] : bimber[3]) - (bimber[7] < 0f ? -bimber[7] : bimber[7]);
            bimber[11] = 0f;
            bimber[12] = homography[0, 2];
            bimber[13] = homography[1, 2];
            bimber[14] = 0f;
            bimber[15] = 1f;

            return bimber;
        }
    }
}

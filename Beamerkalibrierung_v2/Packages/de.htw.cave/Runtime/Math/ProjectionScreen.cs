using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Htw.Cave.Math
{
    public static partial class Projection
    {
        public class Screen
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
    		public float Width { get; set; }
    		public float Height { get; set; }
    		public Vector2 Size
    		{
    			get
                {
                    return new Vector2(this.Width, this.Height);
                }
    			set
                {
                    this.Width = value.x;
                    this.Height = value.y;
                }
    		}
    		public Vector3 Center
    		{
    			get
    			{
    				return new Vector3(this.X + this.Width / 2f, this.Y + this.Height / 2f, this.Z);
    			}
    			set
    			{
    				this.X = value.x - this.Width / 2f;
                    this.Y = value.y - this.Height / 2f;
    				this.Z = value.z;
    			}
    		}
            public Rect Surface => new Rect(this.X, this.Y, this.Width, this.Height);
            public Vector3 Position => new Vector3(this.X, this.Y, this.Z);

            public Screen()
            {
                this.X = 0f;
                this.Y = 0f;
                this.Z = 0f;
                this.Width = 0f;
                this.Height = 0f;
            }

            public Screen(float x, float y, float z, float width, float height)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
                this.Width = width;
                this.Height = height;
            }

            public Screen(Vector3 position, float width, float height)
            {
                this.X = position.x;
                this.Y = position.y;
                this.Z = position.z;
                this.Width = width;
                this.Height = height;
            }

    		public float[,] NearPlane(Transform target, float near, float far)
    		{
    			Vector3 nearCenter = target.position + target.forward * near;
                Plane plane = new Plane(-target.forward, nearCenter);

                //  nearOrigin
                //       x-------------+
                //       |             |
                //       |      x      |
                //       |  nearCenter |
                //       +------------ x
                //                   nearEnd
                float distance = 0f;
                Vector3 direction = (this.Position - target.position).normalized;
                Ray ray = new Ray(target.position, direction);
                plane.Raycast(ray, out distance);

                Vector3 nearOrigin = -(target.InverseTransformPoint(nearCenter) - target.InverseTransformPoint(target.position + direction * distance));
                float left = nearOrigin.x;
                float top = nearOrigin.y;

                direction = (this.Position + new Vector3(this.Width, this.Height, 0f) - target.position).normalized;
                ray = new Ray(target.position, direction);
                plane.Raycast(ray, out distance);

    			Vector3 nearEnd = -(target.InverseTransformPoint(nearCenter) - target.InverseTransformPoint((target.position + direction * distance)));
    			float right = nearEnd.x;
                float bottom = nearEnd.y;

                return new float[3,2]{
                    {left, right},
                    {bottom, top},
                    {near, far}
                };
    		}

    		public Vector3[] ToVector3()
    		{
                //       | xMin     xMax
                //     --+--------------
                //  yMin |  2        3
                //       |
                //  yMax |  0        1
    			return new Vector3[]{
    				new Vector3(this.Surface.xMin, this.Surface.yMax, this.Z),
    				new Vector3(this.Surface.xMax, this.Surface.yMax, this.Z),
    				new Vector3(this.Surface.xMin, this.Surface.yMin, this.Z),
    				new Vector3(this.Surface.xMax, this.Surface.yMin, this.Z)
    			};
    		}

    		public float[,] ToFloats()
    		{
    			return new float[,]{
    				{this.Surface.xMin, this.Surface.yMax, this.Z},
    				{this.Surface.xMax, this.Surface.yMax, this.Z},
    				{this.Surface.xMin, this.Surface.yMin, this.Z},
    				{this.Surface.xMax, this.Surface.yMin, this.Z}
    			};
    		}
        }
    }
}

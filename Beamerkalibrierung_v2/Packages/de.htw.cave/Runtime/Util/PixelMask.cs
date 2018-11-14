using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Htw.Cave.Util
{
    public class PixelMask
    {
		public Vector2[] points { get; set; }

		public PixelMask()
		{
			this.points = null;
		}

		public PixelMask(Vector2[] points)
		{
			this.points = points;
		}

		public bool Contains(Vector2 point)
		{
			for(int i = 1; i <= points.Length; ++i)
			{
				Vector2[] line = {
					points[i - 1],
					points[i % points.Length]
				};

				if(!IsOnRightSide(line, point))
					return false;
			}

			return true;
		}

		public void Apply(Texture2D texture, Color color)
		{
			Color[] pixels = texture.GetPixels();

			for(int x = 0; x < texture.width; ++x)
			{
				for(int y = 0; y < texture.height; ++y)
				{
					int offset = y * texture.width + x;
					pixels[offset] = color;
					Vector2 point = new Vector2((float)x / (float)texture.width, (float)y / (float)texture.height);

					if(Contains(point))
						pixels[offset].a = 0f;
				}
			}

			texture.SetPixels(pixels);
			texture.Apply();
		}

		private bool IsOnRightSide(Vector2[] line, Vector2 point)
		{
			return (point.y - line[0].y) * (line[1].x - line[0].x) - (point.x - line[0].x) * (line[1].y - line[0].y) <= 0;
		}
    }
}

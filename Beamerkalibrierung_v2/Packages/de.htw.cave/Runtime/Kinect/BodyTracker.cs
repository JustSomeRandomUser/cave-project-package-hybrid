using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect;
using Microsoft.Kinect.Face;
using Htw.Cave.Tracking;

namespace Htw.Cave.Kinect
{
    // https://docs.microsoft.com/en-us/previous-versions/windows/kinect/dn758675(v%3dieb.10)
	// https://github.com/Kinect/samples/tree/master/JointOrientationBasics-Unity/Assets/JointOrientationBasics/Scripts
    // https://medium.com/@lisajamhoury/understanding-kinect-v2-joints-and-coordinate-system-4f4b90b9df16
    public sealed class BodyTracker : ITracker
    {
        public UnityEngine.Vector4 FloorClipPlane { get; private set; }

        private KinectSensor KinectSensor { get; set; }
        private BodyFrameReader BodyFrameReader { get; set; }
        private Body[] Bodies { get; set; }
        private int BodyCount { get; set; }

        public BodyTracker(KinectSensor kinectSensor)
        {
            this.KinectSensor = kinectSensor;
            this.BodyFrameReader = this.KinectSensor.BodyFrameSource.OpenReader();

			if(!this.KinectSensor.IsOpen)
				this.KinectSensor.Open();

            this.BodyFrameReader.FrameArrived += this.BodyFrameArrived;
            this.BodyCount = this.KinectSensor.BodyFrameSource.BodyCount;
            this.Bodies = new Body[this.BodyCount];
			this.FloorClipPlane = UnityEngine.Vector4.zero;
        }

        public void StopFrameReaders()
        {
            if(this.BodyFrameReader != null)
            {
                this.BodyFrameReader.Dispose();
                this.BodyFrameReader = null;
            }
        }

		public Body ClosestBody(out float distance)
		{
			Body body = null;
			float closestDistance = distance = float.MaxValue;

			if(this.Bodies == null)
				return null;

			for(int i = 0; i < this.BodyCount; ++i)
			{
				if(!this.Bodies[i].IsTracked)
					continue;

				CameraSpacePoint bodyPosition = this.Bodies[i].Joints[JointType.SpineBase].Position;
				float bodyDistance = (new UnityEngine.Vector3(bodyPosition.X, bodyPosition.Y, bodyPosition.Z)).sqrMagnitude;

				if(body == null || bodyDistance < closestDistance)
				{
					body = this.Bodies[i];
					closestDistance = bodyDistance;
				}
			}

			distance = closestDistance;
			return body;
		}

		public Body ClosestBody()
		{
			return ClosestBody(out _);
		}

		public Body MosterCentralBody()
		{
			return null;
		}

        private void BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using(BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if(bodyFrame == null)
                    return;

                bodyFrame.GetAndRefreshBodyData(this.Bodies);
				this.FloorClipPlane = FloorCorrection(bodyFrame.FloorClipPlane);
            }
        }

		private UnityEngine.Vector4 FloorCorrection(Windows.Kinect.Vector4 floorClipPlane)
		{
			return floorClipPlane.ToUnityVector4();
		}
    }
}

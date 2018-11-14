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
    public sealed class FaceTracker : ITracker
    {
        public static FaceFrameFeatures AllFaceFrameFeatures =>
            FaceFrameFeatures.BoundingBoxInColorSpace
    		| FaceFrameFeatures.PointsInColorSpace
    	    | FaceFrameFeatures.BoundingBoxInInfraredSpace
    		| FaceFrameFeatures.PointsInInfraredSpace
    		| FaceFrameFeatures.RotationOrientation
    		| FaceFrameFeatures.FaceEngagement
    		| FaceFrameFeatures.Glasses
    		| FaceFrameFeatures.Happy
    		| FaceFrameFeatures.LeftEyeClosed
    		| FaceFrameFeatures.RightEyeClosed
    		| FaceFrameFeatures.LookingAway
    		| FaceFrameFeatures.MouthMoved
    		| FaceFrameFeatures.MouthOpen;

        private KinectSensor KinectSensor { get; set; }
        private BodyFrameReader BodyFrameReader { get; set; }
        private Body[] Bodies { get; set; }
        private int BodyCount { get; set; }
        private FaceFrameSource[] FaceFrameSources { get; set; }
        private FaceFrameReader[] FaceFrameReaders { get; set; }
        private FaceFrameResult[] FaceFrameResults { get; set; }

        public FaceTracker(KinectSensor kinectSensor, FaceFrameFeatures faceFeatures)
        {
            this.KinectSensor = kinectSensor;
            this.BodyFrameReader = this.KinectSensor.BodyFrameSource.OpenReader();

            if(!this.KinectSensor.IsOpen)
                this.KinectSensor.Open();

            this.BodyFrameReader.FrameArrived += this.BodyFrameArrived;
            this.BodyCount = this.KinectSensor.BodyFrameSource.BodyCount;
            this.Bodies = new Body[this.BodyCount];
            this.FaceFrameSources = new FaceFrameSource[this.BodyCount];
            this.FaceFrameReaders = new FaceFrameReader[this.BodyCount];
            this.FaceFrameResults = new FaceFrameResult[this.BodyCount];

            for(int i = 0; i < this.BodyCount; ++i)
            {
                this.FaceFrameSources[i] = FaceFrameSource.Create(this.KinectSensor, 0, faceFeatures);
                this.FaceFrameReaders[i] = this.FaceFrameSources[i].OpenReader();
            }
        }

        public void StopFrameReaders()
        {
            for(int i = 0; i < this.BodyCount; ++i)
            {
                if(this.FaceFrameReaders[i] != null)
                {
                    this.FaceFrameReaders[i].Dispose();
                    this.FaceFrameReaders[i] = null;
                }

                if(this.FaceFrameSources[i] != null)
                    this.FaceFrameSources[i] = null;
            }

            if(this.BodyFrameReader != null)
            {
                this.BodyFrameReader.Dispose();
                this.BodyFrameReader = null;
            }
        }

        public FaceFrameResult GetResults(int i)
        {
            return this.FaceFrameResults[i];
        }

        private void BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using(BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if(bodyFrame == null)
                    return;

                bodyFrame.GetAndRefreshBodyData(this.Bodies);

                for(int i = 0; i < this.BodyCount; ++i)
                {
                    if(!this.FaceFrameSources[i].IsTrackingIdValid && this.Bodies[i].IsTracked)
                        this.FaceFrameSources[i].TrackingId = this.Bodies[i].TrackingId;

                    if(this.FaceFrameReaders[i] == null)
                        continue;

                    FaceFrame frame = this.FaceFrameReaders[i].AcquireLatestFrame();

                    if(frame != null && frame.FaceFrameResult != null)
                        FaceFrameResults[i] = frame.FaceFrameResult;
                }
            }
        }
    }
}

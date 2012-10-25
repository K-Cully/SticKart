
using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Kinect.Toolbox;

namespace SticKart.Gestures
{
    public abstract class GestureDetector
    {
        public event Action<string> OnGestureDetected;

        private List<GestureEntry> gestureEntries;

        private DateTime lastGestureDate = DateTime.Now;

        
        public GestureDetector(int maxRecordedPositions = 20, int millisecondsBetweenGestures = 0)
        {
            this.gestureEntries = new List<GestureEntry>();
            this.MaxRecordedPositions = maxRecordedPositions;
            this.MillisecondsBetweenGestures = millisecondsBetweenGestures;
        }


        public int MillisecondsBetweenGestures { get; set; }

        public int MaxRecordedPositions { get; private set; }

        public List<GestureEntry> GestureEntries
        {
            get
            {
                return this.gestureEntries;
            }
        }


        public virtual void Add(SkeletonPoint position, KinectSensor sensor, CoordinateMapper coordinateMapper)
        {
            GestureEntry newEntry = new GestureEntry(position.ToVector3(), DateTime.Now);
            this.gestureEntries.Add(newEntry);

            Vector2 unscaledPosition = Tools.Convert(sensor, position, coordinateMapper);
            
            // Remove an entry if the number of recorded positions has been exceeded.
            if (this.gestureEntries.Count > this.MaxRecordedPositions)
            {
                this.gestureEntries.RemoveAt(0);
            }

            // Look for gestures
            this.LookForGesture();
        }

        protected abstract void LookForGesture()
        { }

        protected void RaiseGestureDetected(string gesture)
        {
            // Check if the time is too close to the last recorded time
            if (DateTime.Now.Subtract(this.lastGestureDate).TotalMilliseconds > this.MillisecondsBetweenGestures)
            {
                if (OnGestureDetected != null)
                    this.OnGestureDetected(gesture);

                lastGestureDate = DateTime.Now;
            }

            this.gestureEntries.Clear();
        }

    }
}

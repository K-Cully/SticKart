using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Kinect.Toolbox;

namespace SticKart.Gestures
{
    /// <summary>
    /// An abstract base class to derive all gesture detectors from.
    /// </summary>
    public abstract class GestureDetector
    {        
        /// <summary>
        /// The last time a gesture was detected.
        /// </summary>
        private DateTime lastGestureDate = DateTime.Now;

        /// <summary>
        /// The list of gesture entries to check for a gesture.
        /// </summary>
        protected List<GestureEntry> GestureEntries;

        /// <summary>
        /// The number of milliseconds delay between gestures.
        /// </summary>
        protected int MillisecondsBetweenGestures;

        /// <summary>
        /// The maximum number of positions to track for this gesture detector.
        /// </summary>
        protected int MaxRecordedPositions;
        
        /// <summary>
        /// Initalizes an instance of the <see cref="GestureDetector"/> base class.
        /// </summary>
        /// <param name="jointToTrack">The joint to track with this gesture detector.</param>
        /// <param name="maxRecordedPositions">THe maximum number of positions to check for a gesture against.</param>
        /// <param name="millisecondsBetweenGestures">The delay to apply between gestures, in milliseconds.</param>
        public GestureDetector(JointType jointToTrack = JointType.HandRight, int maxRecordedPositions = 20, int millisecondsBetweenGestures = 0)
        {
            this.GestureDetected = GestureType.None;
            this.GestureEntries = new List<GestureEntry>();
            this.JointToTrack = jointToTrack;
            this.MaxRecordedPositions = maxRecordedPositions;
            this.MillisecondsBetweenGestures = millisecondsBetweenGestures;
        }

        /// <summary>
        /// Gets the type of joint which the gesture detector is tracking.
        /// </summary>
        public JointType JointToTrack { get; private set; }

        /// <summary>
        /// Gets the the type of gesture detected. Will be none if no gesture has been detected.
        /// </summary>
        public GestureType GestureDetected { get; private set; }
        
        /// <summary>
        /// Adds the skeleton point to the positions being tracked by the gesture detector.
        /// Checks if the gesture has been detected.
        /// </summary>
        /// <param name="position">The position of the joint being tracked.</param>
        public virtual void Add(SkeletonPoint position)
        {
            if (this.GestureDetected == GestureType.None)
            {
                return;
            }
            else
            {
                GestureEntry newEntry = new GestureEntry(position.ToVector3(), DateTime.Now);
                this.GestureEntries.Add(newEntry);
                // Remove an entry if the number of recorded positions has been exceeded.
                if (this.GestureEntries.Count > this.MaxRecordedPositions)
                {
                    this.GestureEntries.RemoveAt(0);
                }

                // Look for gestures
                this.LookForGesture();
            }
        }

        /// <summary>
        /// Abstract method to check for a gesture.
        /// </summary>
        protected abstract void LookForGesture();

        /// <summary>
        /// Checks that the correct amount of time has passed since the last gesture and sets the gesture input as found.
        /// </summary>
        /// <param name="gestureType">The </param>
        protected virtual void GestureFound(GestureType gestureType)
        {
            // Check if the time is too close to the last recorded time.
            if (DateTime.Now.Subtract(this.lastGestureDate).TotalMilliseconds > this.MillisecondsBetweenGestures)
            {
                this.GestureDetected = gestureType;
                lastGestureDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Resets the gesture detector.
        /// This must be called after a detected gesture is read to reactivate the detector.
        /// </summary>
        public virtual void Reset()
        {
            this.GestureDetected = GestureType.None;
            this.GestureEntries.Clear();
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="GestureDetector.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Input.Gestures
{
    using System;
    using System.Collections.Generic;
    using Kinect.Toolbox;
    using Microsoft.Kinect;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// An abstract base class to derive all gesture detectors from.
    /// </summary>
    public abstract class GestureDetector
    {        
        /// <summary>
        /// The list of gesture entries to check for a gesture.
        /// </summary>
        protected List<GestureEntry> gestureEntries;

        /// <summary>
        /// The number of milliseconds delay between gestures.
        /// </summary>
        protected int millisecondsBetweenGestures;

        /// <summary>
        /// The maximum number of positions to track for this gesture detector.
        /// </summary>
        protected int maxRecordedPositions;

        /// <summary>
        /// The last time a gesture was detected.
        /// </summary>
        private DateTime lastGestureDate = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureDetector"/> class.
        /// </summary>
        /// <param name="jointToTrack">The joint to track with this gesture detector.</param>
        /// <param name="maxRecordedPositions">THe maximum number of positions to check for a gesture against.</param>
        /// <param name="millisecondsBetweenGestures">The delay to apply between gestures, in milliseconds.</param>
        public GestureDetector(JointType jointToTrack = JointType.HandRight, int maxRecordedPositions = 20, int millisecondsBetweenGestures = 0)
        {
            this.GestureDetected = GestureType.None;
            this.gestureEntries = new List<GestureEntry>();
            this.JointToTrack = jointToTrack;
            this.maxRecordedPositions = maxRecordedPositions;
            this.millisecondsBetweenGestures = millisecondsBetweenGestures;
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
                GestureEntry newEntry = new GestureEntry(position.ToVector3(), DateTime.UtcNow);
                this.gestureEntries.Add(newEntry);
                if (this.gestureEntries.Count > this.maxRecordedPositions)
                {
                    this.gestureEntries.RemoveAt(0);
                }

                this.LookForGesture();
            }
        }

        /// <summary>
        /// Resets the gesture detector.
        /// This must be called after a detected gesture is read to reactivate the detector.
        /// </summary>
        public virtual void Reset()
        {
            this.GestureDetected = GestureType.None;
            this.gestureEntries.Clear();
        }

        /// <summary>
        /// Abstract method to check for a gesture.
        /// </summary>
        protected abstract void LookForGesture();

        /// <summary>
        /// Checks that the correct amount of time has passed since the last gesture and sets the gesture input as found.
        /// </summary>
        /// <param name="gestureType">The type of gesture detected.</param>
        protected virtual void GestureFound(GestureType gestureType)
        {
            // Check if the time is too close to the last recorded time.
            if (DateTime.UtcNow.Subtract(this.lastGestureDate).TotalMilliseconds > this.millisecondsBetweenGestures)
            {
                this.GestureDetected = gestureType;
                this.lastGestureDate = DateTime.UtcNow;
            }
        }
    }
}

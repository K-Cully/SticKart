// -----------------------------------------------------------------------
// <copyright file="HorizontalSwipeGestureDetector.cs" company="Microsoft">
// http://kinecttoolbox.codeplex.com/license
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
    /// A horizontal swipe gesture detector class.
    /// The code here is mostly taken from the Kinect toolbox (http://kinecttoolbox.codeplex.com/).
    /// I had to make some modifications as I had to port the entire base class to work with XNA.
    /// </summary>
    public class HorizontalSwipeGestureDetector : GestureDetector
    {
        /// <summary>
        /// The minimum swipe length.
        /// </summary>
        private float swipeMinimumLength;
        
        /// <summary>
        /// The maximum swipe height.
        /// </summary>
        private float swipeMaximumHeight;

        /// <summary>
        /// The minimum swipe duration.
        /// </summary>
        private int swipeMinimumDuration;

        /// <summary>
        /// The maximum swipe duration.
        /// </summary>
        private int swipeMaximumDuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalSwipeGestureDetector"/> class.
        /// </summary>
        /// <param name="jointToTrack">The joint to track with this gesture detector.</param>
        /// <param name="maxRecordedPositions">THe maximum number of positions to check for a gesture against.</param>
        /// <param name="millisecondsBetweenGestures">The delay to apply between gestures, in milliseconds.</param>
        public HorizontalSwipeGestureDetector(JointType jointToTrack = JointType.HandRight, int maxRecordedPositions = 20, int millisecondsBetweenGestures = 1200)
            : base(jointToTrack, maxRecordedPositions, millisecondsBetweenGestures)
        {
            this.swipeMinimumLength = 0.65f;
            this.swipeMaximumHeight = 0.1f;
            this.swipeMinimumDuration = 600;
            this.swipeMaximumDuration = 1800;
        }

        /// <summary>
        /// Checks for a swipe gesture which fits the criteria defined by the functions passed in.
        /// </summary>
        /// <param name="heightFunction">The function defining the height deviation.</param>
        /// <param name="directionFunction">The function defining the direction.</param>
        /// <param name="lengthFunction">The function defining the length deviation.</param>
        /// <param name="minTime">The minimum time to complete the swipe.</param>
        /// <param name="maxTime">The maximum time to complete the swipe.</param>
        /// <returns>Whether the swipe criteria are met or not.</returns>
        protected bool ScanPositions(Func<Vector3, Vector3, bool> heightFunction, Func<Vector3, Vector3, bool> directionFunction, Func<Vector3, Vector3, bool> lengthFunction, int minTime, int maxTime)
        {
            int start = 0;
            for (int index = 1; index < this.gestureEntries.Count - 1; index++)
            {
                if (!heightFunction(this.gestureEntries[0].Position, this.gestureEntries[index].Position) || 
                    !directionFunction(this.gestureEntries[index].Position, this.gestureEntries[index + 1].Position))
                {
                    start = index;
                }

                if (lengthFunction(this.gestureEntries[index].Position, this.gestureEntries[start].Position))
                {
                    double totalMilliseconds = (this.gestureEntries[index].Time - this.gestureEntries[start].Time).TotalMilliseconds;
                    if (totalMilliseconds >= minTime && totalMilliseconds <= maxTime)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks for a swipe gesture and raises the appropriate event if detected.
        /// </summary>
        protected override void LookForGesture()
        {
            // Swipe to right
            if (this.ScanPositions(
                (p1, p2) => Math.Abs(p2.Y - p1.Y) < this.swipeMaximumHeight,
                (p1, p2) => p2.X - p1.X > -0.01f,
                (p1, p2) => Math.Abs(p2.X - p1.X) > this.swipeMinimumLength,
                this.swipeMinimumDuration, 
                this.swipeMaximumDuration))
            {
                this.GestureFound(GestureType.SwipeRight);
                return;
            }

            // Swipe to left
            if (this.ScanPositions(
                (p1, p2) => Math.Abs(p2.Y - p1.Y) < this.swipeMaximumHeight,
                (p1, p2) => p2.X - p1.X < 0.01f,
                (p1, p2) => Math.Abs(p2.X - p1.X) > this.swipeMinimumLength,
                this.swipeMinimumDuration, 
                this.swipeMaximumDuration))
            {
                this.GestureFound(GestureType.SwipeLeft);
                return;
            }
        }
    }
}

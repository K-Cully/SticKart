using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Kinect.Toolbox;

namespace SticKart.Gestures
{
    /// <summary>
    /// A swipe gesture detector class.
    /// The code here is mostly taken from the Kinect toolbox (http://kinecttoolbox.codeplex.com/).
    /// I had to make some modifications as I had to port the entire base class to work with XNA.
    /// </summary>
    public class SwipeGestureDetector : GestureDetector
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
        /// Initalizes a new instance of the <see cref="SwipeGestureDetector"/> class.
        /// </summary>
        /// <param name="jointToTrack">The joint to track with this gesture detector.</param>
        /// <param name="maxRecordedPositions">THe maximum number of positions to check for a gesture against.</param>
        /// <param name="millisecondsBetweenGestures">The delay to apply between gestures, in milliseconds.</param>
        public SwipeGestureDetector(JointType jointToTrack = JointType.HandRight, int maxRecordedPositions = 20, int millisecondsBetweenGestures = 0)
            : base(jointToTrack, maxRecordedPositions, millisecondsBetweenGestures)
        {
            this.swipeMinimumLength = 0.4f;
            this.swipeMaximumHeight = 0.2f;
            this.swipeMinimumDuration = 250;
            this.swipeMaximumDuration = 1500;
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
        protected bool ScanPositions(Func<Vector3, Vector3, bool> heightFunction, Func<Vector3, Vector3, bool> directionFunction,
            Func<Vector3, Vector3, bool> lengthFunction, int minTime, int maxTime)
        {
            int start = 0;
            for (int index = 1; index < this.GestureEntries.Count - 1; index++)
            {
                if (!heightFunction(this.GestureEntries[0].Position, this.GestureEntries[index].Position) || 
                    !directionFunction(this.GestureEntries[index].Position, this.GestureEntries[index + 1].Position))
                {
                    start = index;
                }

                if (lengthFunction(this.GestureEntries[index].Position, this.GestureEntries[start].Position))
                {
                    double totalMilliseconds = (this.GestureEntries[index].Time - this.GestureEntries[start].Time).TotalMilliseconds;
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
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) < this.swipeMaximumHeight, // Height
                (p1, p2) => p2.X - p1.X > -0.01f, // Progression to right
                (p1, p2) => Math.Abs(p2.X - p1.X) > this.swipeMinimumLength, // Length
                this.swipeMinimumDuration, this.swipeMaximumDuration)) // Duration
            {
                this.GestureFound(GestureType.SwipeToRight);
                return;
            }

            // Swipe to left
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) < this.swipeMaximumHeight,  // Height
                (p1, p2) => p2.X - p1.X < 0.01f, // Progression to left
                (p1, p2) => Math.Abs(p2.X - p1.X) > this.swipeMinimumLength, // Length
                this.swipeMinimumDuration, this.swipeMaximumDuration)) // Duration
            {
                this.GestureFound(GestureType.SwipeToLeft);
                return;
            }
        }
    }
}

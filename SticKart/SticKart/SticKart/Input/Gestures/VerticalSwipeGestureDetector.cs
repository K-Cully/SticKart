﻿// -----------------------------------------------------------------------
// <copyright file="VerticalSwipeGestureDetector.cs" company="None">
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
    /// A vertical swipe gesture detector class.
    /// </summary>
    public class VerticalSwipeGestureDetector : GestureDetector
    {
        /// <summary>
        /// The minimum swipe height.
        /// </summary>
        private float swipeMinimumHeight;

        /// <summary>
        /// The maximum swipe width.
        /// </summary>
        private float swipeMaximumWidth;
        
        /// <summary>
        /// The minimum swipe duration.
        /// </summary>
        private int swipeMinimumDuration;

        /// <summary>
        /// The maximum swipe duration.
        /// </summary>
        private int swipeMaximumDuration;

        /// <summary>
        /// Whether swipe down detection is enabled or not. 
        /// </summary>
        private bool swipeDownEnabled;

        /// <summary>
        /// Whether swipe up detection is enabled or not. 
        /// </summary>
        private bool swipeUpEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalSwipeGestureDetector"/> class.
        /// </summary>
        /// <param name="jointToTrack">The joint to track with this gesture detector.</param>
        /// <param name="maxRecordedPositions">The maximum number of positions to check for a gesture against.</param>
        /// <param name="millisecondsBetweenGestures">The delay to apply between gestures, in milliseconds.</param>
        /// <param name="minimumHeight">The minimum height offset to detect as a gesture.</param>
        /// <param name="maximumWidth">The maximum width deviation to allow.</param>
        /// <param name="minimumDuration">The minimum duration of a movement to interpret as a gesture, in milliseconds.</param>
        /// <param name="maximumDuration">The maximum duration of a movement to interpret as a gesture, in milliseconds.</param>
        /// <param name="swipeUpEnabled">Whether swipe up gestures should be detected or not.</param>
        /// <param name="swipeDownEnabled">Whether swipe down gestures should be detected or not.</param>
        public VerticalSwipeGestureDetector(JointType jointToTrack = JointType.HandRight, int maxRecordedPositions = 20, int millisecondsBetweenGestures = 1200, float minimumHeight = 0.5f, float maximumWidth = 0.175f, int minimumDuration = 400, int maximumDuration = 1800, bool swipeUpEnabled = true, bool swipeDownEnabled = true)
            : base(jointToTrack, maxRecordedPositions, millisecondsBetweenGestures)
        {
            this.swipeMinimumHeight = minimumHeight;
            this.swipeMaximumWidth = maximumWidth;
            this.swipeMinimumDuration = minimumDuration;
            this.swipeMaximumDuration = maximumDuration;
            this.swipeUpEnabled = swipeUpEnabled;
            this.swipeDownEnabled = swipeDownEnabled;
        }
        
        /// <summary>
        /// Checks for a vertical swipe gesture which fits the criteria defined by the functions passed in.
        /// </summary>
        /// <param name="widthFunction">The function defining the width deviation.</param>
        /// <param name="directionFunction">The function defining the direction.</param>
        /// <param name="heightFunction">The function defining the height deviation.</param>
        /// <param name="minTime">The minimum time to complete the swipe.</param>
        /// <param name="maxTime">The maximum time to complete the swipe.</param>
        /// <returns>Whether the swipe criteria are met or not.</returns>
        protected bool ScanPositions(Func<Vector3, Vector3, bool> widthFunction, Func<Vector3, Vector3, bool> directionFunction, Func<Vector3, Vector3, bool> heightFunction, int minTime, int maxTime)
        {
            int start = 0;
            for (int index = 1; index < this.GestureEntries.Count - 1; index++)
            {
                if (!widthFunction(this.GestureEntries[0].Position, this.GestureEntries[index].Position) ||
                    !directionFunction(this.GestureEntries[index].Position, this.GestureEntries[index + 1].Position))
                {
                    start = index;
                }

                if (heightFunction(this.GestureEntries[index].Position, this.GestureEntries[start].Position))
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
        /// Checks for a vertical swipe gesture and raises the appropriate event if detected.
        /// </summary>
        protected override void LookForGesture()
        {
            // Swipe up
            if (this.swipeUpEnabled && this.ScanPositions(
                (p1, p2) => Math.Abs(p2.X - p1.X) < this.swipeMaximumWidth,
                (p1, p2) => p2.Y - p1.Y > -0.01f,
                (p1, p2) => Math.Abs(p2.Y - p1.Y) > this.swipeMinimumHeight,
                this.swipeMinimumDuration,
                this.swipeMaximumDuration))
            {
                this.GestureFound(GestureType.SwipeUp);
                return;
            }

            // Swipe down
            if (this.swipeDownEnabled && this.ScanPositions(
                (p1, p2) => Math.Abs(p2.X - p1.X) < this.swipeMaximumWidth,
                (p1, p2) => p2.Y - p1.Y < 0.01f,
                (p1, p2) => Math.Abs(p2.Y - p1.Y) > this.swipeMinimumHeight,
                this.swipeMinimumDuration,
                this.swipeMaximumDuration))
            {
                this.GestureFound(GestureType.SwipeDown);
                return;
            }
        }
    }
}

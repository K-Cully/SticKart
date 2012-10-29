using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;

namespace SticKart.Gestures
{
    public class PushGestureDetector : GestureDetector
    {
        /// <summary>
        /// The minimum push length.
        /// </summary>
        private float pushMinimumLength;
        
        /// <summary>
        /// The maximum push height offset.
        /// </summary>
        private float pushMaximumHeight;
        
        /// <summary>
        /// The maximum push width offset.
        /// </summary>
        private float pushMaximumWidth;

        /// <summary>
        /// The minimum push duration in milliseconds.
        /// </summary>
        private int pushMinimumDuration;

        /// <summary>
        /// The maximum push duration in milliseconds.
        /// </summary>
        private int pushMaximumDuration;

        /// <summary>
        /// Initalizes a new instance of the <see cref="PushGestureDetector"/> class.
        /// </summary>
        /// <param name="jointToTrack">The joint to track with this gesture detector.</param>
        /// <param name="maxRecordedPositions">THe maximum number of positions to check for a gesture against.</param>
        /// <param name="millisecondsBetweenGestures">The delay to apply between gestures, in milliseconds.</param>
        public PushGestureDetector(JointType jointToTrack = JointType.HandRight, int maxRecordedPositions = 20, int millisecondsBetweenGestures = 500)
            : base(jointToTrack, maxRecordedPositions, millisecondsBetweenGestures)
        {
            this.pushMinimumLength = 0.4f;
            this.pushMaximumHeight = 0.3f;
            this.pushMaximumWidth = 0.3f;
            this.pushMinimumDuration = 250;
            this.pushMaximumDuration = 1800;
        }

        /// <summary>
        /// Checks for a push gesture which fits the criteria defined by the functions passed in.
        /// </summary>
        /// <param name="heightFunction">The function limiting the height deviation.</param>
        /// <param name="widthFunction">The function limiting width deviation.</param>
        /// <param name="directionFunction">The function to check the direction.</param>
        /// <param name="lengthFunction">The function limiting the length deviation.</param>
        /// <param name="minTime">The minimum time to complete the push.</param>
        /// <param name="maxTime">The maximum time to complete the push.</param>
        /// <returns>Whether the push criteria are met or not.</returns>
        protected bool ScanPositions(Func<Vector3, Vector3, bool> heightFunction, Func<Vector3, Vector3, bool> widthFunction,
            Func<Vector3, Vector3, bool> directionFunction, Func<Vector3, Vector3, bool> lengthFunction, int minTime, int maxTime)
        {
            int start = 0;
            for (int index = 1; index < this.GestureEntries.Count - 1; index++)
            {
                if (!heightFunction(this.GestureEntries[0].Position, this.GestureEntries[index].Position) || 
                    !widthFunction(this.GestureEntries[0].Position, this.GestureEntries[index].Position) ||
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
            if (ScanPositions((p1, p2) => Math.Abs(p2.Y - p1.Y) < this.pushMaximumHeight, // Height
                (p1, p2) => Math.Abs(p2.X - p1.X) < this.pushMaximumWidth,
                (p1, p2) => p2.Z - p1.Z < 0.01f, // Progression towards the sensor
                (p1, p2) => Math.Abs(p2.Z - p1.Z) > this.pushMinimumLength, // Length
                this.pushMinimumDuration, this.pushMaximumDuration)) // Duration
            {
                this.GestureFound(GestureType.Push);
                return;
            }
        }
    }
}

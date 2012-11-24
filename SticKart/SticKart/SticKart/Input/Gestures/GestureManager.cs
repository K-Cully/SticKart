// -----------------------------------------------------------------------
// <copyright file="GestureManager.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Input.Gestures
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Microsoft.Kinect;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Manages the initialization and updating of all gesture detectors.
    /// </summary>
    public class GestureManager
    {
        /// <summary>
        /// Stores the gesture detectors in use.
        /// </summary>
        private Collection<GestureDetector> gestureDetectors;

        /// <summary>
        /// Stores detected gestures in order of arrival.
        /// </summary>
        private Queue<GestureType> detectedGestures;

        /// <summary>
        /// Storage space for a skeleton's joints.
        /// </summary>
        private JointCollection skeletonJoints;

        /// <summary>
        /// The primary hand to track.
        /// </summary>
        private JointType activeHand;

        /// <summary>
        /// The shoulder connected to the primary hand.
        /// </summary>
        private JointType activeShoulder;

        /// <summary>
        /// The time limit between leg lifts to count as running in milliseconds.
        /// </summary>
        private int runTimeLimit;

        /// <summary>
        /// The time limit between leg lifts to count as jumping in milliseconds.
        /// </summary>
        private int jumpTimeLimit;

        /// <summary>
        /// The last time the player lifted their leg.
        /// </summary>
        private DateTime lastLegLiftTime;

        /// <summary>
        /// The last leg lifted.
        /// </summary>
        private JointType lastLegLifted;

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureManager"/> class.
        /// </summary>
        /// <param name="primaryHand">The hand to primarily track.</param>
        public GestureManager(JointType primaryHand = JointType.HandRight)
        {
            this.runTimeLimit = 800;
            this.jumpTimeLimit = 90;
            this.lastLegLiftTime = DateTime.UtcNow;
            this.lastLegLifted = JointType.Spine;
            this.activeHand = primaryHand;
            if (this.activeHand == JointType.HandRight)
            {
                this.activeShoulder = JointType.ShoulderRight;
            }
            else
            {
                this.activeHand = JointType.ShoulderLeft;
            }

            this.detectedGestures = new Queue<GestureType>();
            this.gestureDetectors = new Collection<GestureDetector>();
            this.skeletonJoints = null;

            HorizontalSwipeGestureDetector swipeGestureDetector = new HorizontalSwipeGestureDetector(this.activeHand, 50, 1200);
            this.gestureDetectors.Add(swipeGestureDetector);
            PushGestureDetector pushGestureDetector = new PushGestureDetector(this.activeHand, 30);
            this.gestureDetectors.Add(pushGestureDetector);
            VerticalSwipeGestureDetector rightLegSwipeGestureDetector = new VerticalSwipeGestureDetector(JointType.AnkleRight, 60, 100, 0.035f, 0.3f, 100, 2000, true, false);
            this.gestureDetectors.Add(rightLegSwipeGestureDetector);
            VerticalSwipeGestureDetector leftLegSwipeGestureDetector = new VerticalSwipeGestureDetector(JointType.AnkleLeft, 60, 100, 0.035f, 0.3f, 100, 2000, true, false);
            this.gestureDetectors.Add(leftLegSwipeGestureDetector);
            VerticalSwipeGestureDetector headSwipeGestureDetector = new VerticalSwipeGestureDetector(JointType.Head, 45, 1200, 0.35f, 0.2f, 400, 1800);
            this.gestureDetectors.Add(headSwipeGestureDetector);
        }
        
        /// <summary>
        /// Gets position of the active hand.
        /// </summary>
        public SkeletonPoint HandPosition
        {
            get
            {
                if (this.skeletonJoints == null)
                {
                    SkeletonPoint point = new SkeletonPoint();
                    point.X = 0;
                    point.Y = 0;
                    point.Z = 0;
                    return point;
                }
                else
                {
                    return this.skeletonJoints[this.activeHand].Position;
                }
            }
        }

        /// <summary>
        /// gets the position of the active shoulder.
        /// </summary>
        public SkeletonPoint ShoulderPosition
        {
            get
            {
                if (this.skeletonJoints == null)
                {
                    SkeletonPoint point = new SkeletonPoint();
                    point.X = 0;
                    point.Y = 0;
                    point.Z = 0;
                    return point;
                }
                else
                {
                    return this.skeletonJoints[this.activeShoulder].Position;
                }
            }
        }

        /// <summary>
        /// Gets the next detected gesture in order of detection.
        /// </summary>
        /// <returns>The gesture type or None if no more gestures are available.</returns>
        public GestureType GetNextDetectedGesture()
        {
            if (this.detectedGestures.Count > 0)
            {
                return this.detectedGestures.Dequeue();
            }
            else
            {
                return GestureType.None;
            }
        }

        /// <summary>
        /// Updates all the gesture detectors based on the skeleton passed in.
        /// </summary>
        /// <param name="skeleton">The skeleton being tracked.</param>
        public void Update(Skeleton skeleton)
        {
            this.skeletonJoints = skeleton.Joints;
            foreach (GestureDetector gestureDetector in this.gestureDetectors)
            {
                if (this.skeletonJoints[gestureDetector.JointToTrack].TrackingState != JointTrackingState.NotTracked)
                {
                    gestureDetector.Add(this.skeletonJoints[gestureDetector.JointToTrack].Position);
                    if (gestureDetector.GestureDetected != GestureType.None)
                    {
                        if (gestureDetector.JointToTrack == JointType.AnkleLeft || gestureDetector.JointToTrack == JointType.AnkleRight)
                        {
                            this.ProcessLegGesture(gestureDetector.JointToTrack);
                        }
                        else if (gestureDetector.JointToTrack == JointType.Head)
                        {
                            if (gestureDetector.GestureDetected == GestureType.SwipeDown)
                            {
                                this.detectedGestures.Enqueue(GestureType.Crouch);
                            }
                            else if (gestureDetector.GestureDetected == GestureType.SwipeUp)
                            {
                                this.detectedGestures.Enqueue(GestureType.Stand);
                            }
                        }
                        else
                        {
                            this.detectedGestures.Enqueue(gestureDetector.GestureDetected);
                        }

                        gestureDetector.Reset();
                    }
                }
            }
        }

        /// <summary>
        /// Processes basic leg gestures into jump and run gestures.
        /// </summary>
        /// <param name="jointTracked">The joint used for the gesture.</param>
        private void ProcessLegGesture(JointType jointTracked)
        {
            double timeSinceLastGesture = (int)DateTime.UtcNow.Subtract(this.lastLegLiftTime).TotalMilliseconds;
            if ((this.lastLegLifted == JointType.AnkleLeft && jointTracked == JointType.AnkleRight) ||
                (this.lastLegLifted == JointType.AnkleRight && jointTracked == JointType.AnkleLeft))
            {
                if (timeSinceLastGesture < this.jumpTimeLimit)
                {
                    this.detectedGestures.Enqueue(GestureType.Jump);
                }
                else if (timeSinceLastGesture < this.runTimeLimit)
                {
                    this.detectedGestures.Enqueue(GestureType.Run);
                }
            }

            this.lastLegLifted = jointTracked;
            this.lastLegLiftTime = DateTime.UtcNow;
        }
    }
}

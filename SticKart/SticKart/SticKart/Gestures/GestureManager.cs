using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;

namespace SticKart.Gestures
{
    /// <summary>
    /// An enumeration of the different gesture types available.
    /// </summary>
    public enum GestureType 
    { 
        None, 
        SwipeLeft, 
        SwipeRight, 
        SwipeUp, 
        SwipeDown, 
        Push, 
        Jump, 
        Run,
        Crouch,
        Stand
    }

    /// <summary>
    /// Manages the initalization and updating of all gesture detectors.
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
        /// The time limit between leg lifts to count as running in millisceonds.
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
        /// <param name="primaryHand">The hand to primarly track.</param>
        public GestureManager(JointType primaryHand = JointType.HandRight)
        {
            this.runTimeLimit = 800;
            this.jumpTimeLimit = 120;
            this.lastLegLiftTime = DateTime.Now;
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

            HorizontalSwipeGestureDetector swipeGestureDetector = new HorizontalSwipeGestureDetector(this.activeHand);
            this.gestureDetectors.Add(swipeGestureDetector);
            PushGestureDetector pushGesturedetector = new PushGestureDetector(this.activeHand, 30);
            this.gestureDetectors.Add(pushGesturedetector);
            VerticalSwipeGestureDetector rightLegSwipeGestureDetector = new VerticalSwipeGestureDetector(JointType.AnkleRight, 25, 300, 0.15f, 0.175f, 150, 2000, true, false);
            this.gestureDetectors.Add(rightLegSwipeGestureDetector);
            VerticalSwipeGestureDetector leftLegSwipeGestureDetector = new VerticalSwipeGestureDetector(JointType.AnkleLeft, 25, 300, 0.15f, 0.175f, 150, 2000, true, false);
            this.gestureDetectors.Add(leftLegSwipeGestureDetector);
            VerticalSwipeGestureDetector headSwipeGestureDetector = new VerticalSwipeGestureDetector(JointType.Head, 45, 1200, 0.35f, 0.2f, 400, 1800);
            this.gestureDetectors.Add(headSwipeGestureDetector);
        }
        
        /// <summary>
        /// Gets position of the active hand relative to the shoulder.
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
                    SkeletonPoint handPosition = this.skeletonJoints[this.activeHand].Position;
                    SkeletonPoint shoulderPosition = this.skeletonJoints[this.activeShoulder].Position;
                    if (this.activeHand == JointType.HandRight)
                    {
                        handPosition.X -= shoulderPosition.X;
                    }
                    else
                    {
                        handPosition.X += shoulderPosition.X;
                    }
                    return handPosition;
                }
            }
        }

        /// <summary>
        /// Processes basic leg gestures into jump and run gestures.
        /// </summary>
        /// <param name="jointTracked">The joint used for the gesture.</param>
        private void ProcessLegGesture(JointType jointTracked)
        {
            double timeSinceLastGesture = (int)(DateTime.Now.Subtract(this.lastLegLiftTime).TotalMilliseconds);
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
            this.lastLegLiftTime = DateTime.Now;
        }

        /// <summary>
        /// Gets the next detected gesture in order of detection.
        /// </summary>
        /// <returns>The gesture type or None if nop more gestures are available.</returns>
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
    }
}

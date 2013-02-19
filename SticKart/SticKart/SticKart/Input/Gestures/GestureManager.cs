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
    using Kinect.Toolbox;
    using Microsoft.Kinect;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Manages the initialization and updating of all gesture detectors.
    /// </summary>
    public class GestureManager
    {
        /// <summary>
        /// The maximum change in body size before the system resets itself.
        /// </summary>
        private const float BodySizeThreshold = 0.15f;

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
        
        #region arms

        /// <summary>
        /// The primary hand to track.
        /// </summary>
        private JointType activeHand;

        /// <summary>
        /// The shoulder connected to the primary hand.
        /// </summary>
        private JointType activeShoulder;

        #endregion

        #region legs

        /// <summary>
        /// The time limit between leg lifts to count as running in seconds.
        /// </summary>
        private double runTimeLimit;

        /// <summary>
        /// The time, in seconds, since the player last lifted a leg.
        /// </summary>
        private double lastLegLiftCounter;

        /// <summary>
        /// The last leg lifted.
        /// </summary>
        private JointType lastLegLifted;

        /// <summary>
        /// A value indicating whether to accept a left leg lift gesture or not.
        /// </summary>
        private bool acceptLeftLegLift;

        /// <summary>
        /// A value indicating whether to accept a right leg lift gesture or not.
        /// </summary>
        private bool acceptRightLegLift;

        /// <summary>
        /// The standard y position of the player's spine joint.
        /// </summary>
        private float standardSpineY;

        /// <summary>
        /// The player body deviation threshold before detecting a jump.
        /// </summary>
        private float jumpThreshold;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureManager"/> class.
        /// </summary>
        /// <param name="primaryHand">The hand to primarily track.</param>
        public GestureManager(JointType primaryHand = JointType.HandRight)
        {
            this.PlayerBodySize = 0.0f;
            this.jumpThreshold = 0.175f;
            this.standardSpineY = 0.0f;
            this.runTimeLimit = 1.5;
            this.lastLegLiftCounter = 0.0f;
            this.lastLegLifted = JointType.FootLeft;
            this.acceptLeftLegLift = true;
            this.acceptRightLegLift = true;
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
            VerticalSwipeGestureDetector rightLegSwipeGestureDetector = new VerticalSwipeGestureDetector(JointType.FootRight, 60, 10, 0.035f, 0.3f, 200, 2000, true, true);
            this.gestureDetectors.Add(rightLegSwipeGestureDetector);
            VerticalSwipeGestureDetector leftLegSwipeGestureDetector = new VerticalSwipeGestureDetector(JointType.FootLeft, 60, 10, 0.035f, 0.3f, 200, 2000, true, true);
            this.gestureDetectors.Add(leftLegSwipeGestureDetector);
            VerticalSwipeGestureDetector headSwipeGestureDetector = new VerticalSwipeGestureDetector(JointType.Head, 45, 400, 0.45f, 0.4f, 400, 1800);
            this.gestureDetectors.Add(headSwipeGestureDetector);
        }

        /// <summary>
        /// Gets the player's body size.
        /// </summary>
        public float PlayerBodySize { get; private set; }
        
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
        /// Gets the position of the active shoulder.
        /// </summary>
        public SkeletonPoint ShoulderPosition
        {
            get
            {
                SkeletonPoint point = new SkeletonPoint();
                if (this.skeletonJoints == null)
                {
                    point.X = 0;
                    point.Y = 0;
                    point.Z = 0;
                }
                else
                {
                    point = this.skeletonJoints[JointType.Spine].Position;
                    point.Y += this.PlayerBodySize * 0.375f;
                    if (this.activeShoulder == JointType.ShoulderRight)
                    {
                        point.X += this.PlayerBodySize * 0.31f;
                    }
                    else
                    {
                        point.X -= this.PlayerBodySize * 0.31f;
                    }
                }

                return point;
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
        /// Resets all gesture detectors.
        /// </summary>
        public void ResetGestures()
        {
            this.lastLegLiftCounter = 0.0f;
            this.lastLegLifted = JointType.FootLeft;
            this.acceptLeftLegLift = true;
            this.acceptRightLegLift = true;
            foreach (GestureDetector gestureDetector in this.gestureDetectors)
            {
                gestureDetector.Reset();
            }
        }

        /// <summary>
        /// Resets the player specific settings of the gesture manager.
        /// </summary>
        /// <param name="skeleton">The player's skeletal representation.</param>
        public void ResetPlayerSettings(Skeleton skeleton)
        {
            this.skeletonJoints = skeleton.Joints;
            this.standardSpineY = this.skeletonJoints[JointType.Spine].Position.Y;
            this.PlayerBodySize = this.CalculateBodySize();
            this.SetGesturesToPlayer();
        }

        /// <summary>
        /// Updates all the gesture detectors based on the skeleton passed in.
        /// </summary>
        /// <param name="skeleton">The skeleton being tracked.</param>
        /// <param name="gameTime">The game time.</param>
        /// <returns>A value indicating whether the player's body size has changed or not.</returns>
        public bool Update(Skeleton skeleton, GameTime gameTime)
        {
            if (this.lastLegLiftCounter < this.runTimeLimit)
            {
                this.lastLegLiftCounter += gameTime.ElapsedGameTime.TotalSeconds;
            }

            this.skeletonJoints = skeleton.Joints;
            foreach (GestureDetector gestureDetector in this.gestureDetectors)
            {
                if (this.skeletonJoints[gestureDetector.JointToTrack].TrackingState != JointTrackingState.NotTracked)
                {
                    gestureDetector.Add(this.skeletonJoints[gestureDetector.JointToTrack].Position);
                    if (gestureDetector.GestureDetected != GestureType.None)
                    {
                        if (gestureDetector.JointToTrack == JointType.FootLeft || gestureDetector.JointToTrack == JointType.FootRight)
                        {
                            this.ProcessLegGesture(gestureDetector.JointToTrack, gestureDetector.GestureDetected);
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

            return MathHelper.Distance(this.PlayerBodySize, this.CalculateBodySize()) > GestureManager.BodySizeThreshold;
        }

        /// <summary>
        /// Processes basic leg gestures into jump and run gestures.
        /// </summary>
        /// <param name="jointTracked">The joint used for the gesture.</param>
        /// <param name="gesture">The gesture detected.</param>
        private void ProcessLegGesture(JointType jointTracked, GestureType gesture)
        {
            if (gesture == GestureType.SwipeUp)
            {
                if ((this.acceptRightLegLift && jointTracked == JointType.FootRight) || (this.acceptLeftLegLift && jointTracked == JointType.FootLeft))
                {                    
                    if (this.lastLegLiftCounter < this.runTimeLimit)
                    {
                        float distance = MathHelper.Distance(this.skeletonJoints[JointType.Spine].Position.Y, this.standardSpineY);
                        if (distance > this.jumpThreshold)
                        {
                            this.detectedGestures.Enqueue(GestureType.Jump);
                        }
                        else
                        {
                            this.detectedGestures.Enqueue(GestureType.Run);
                        }

                        this.acceptLeftLegLift = true;
                        this.acceptRightLegLift = true;
                        this.lastLegLiftCounter = this.runTimeLimit;
                    }
                    else
                    {
                        this.lastLegLiftCounter = 0.0;
                        if (jointTracked == JointType.FootLeft)
                        {
                            this.acceptLeftLegLift = false;
                        }
                        else
                        {
                            this.acceptRightLegLift = false;
                        }
                    }
                }
            }
            else
            {
                if (jointTracked == JointType.FootLeft)
                {
                    this.acceptLeftLegLift = true;
                }
                else
                {
                    this.acceptRightLegLift = true;
                }
            }
        }

        /// <summary>
        /// Calculates the currently tracked skeleton's body size.
        /// </summary>
        /// <returns>The body size.</returns>
        private float CalculateBodySize()
        {
            Vector3 neckPos = Tools.ToVector3(this.skeletonJoints[JointType.ShoulderCenter].Position);
            Vector3 hipToNeck = neckPos - Tools.ToVector3(this.skeletonJoints[JointType.HipRight].Position);
            Vector3 neckToHead = Tools.ToVector3(this.skeletonJoints[JointType.Head].Position) - neckPos;
            return neckToHead.Length() + hipToNeck.Length();
        }

        /// <summary>
        /// Resets all gesture detectors based on the size of the player.
        /// </summary>
        private void SetGesturesToPlayer()
        {
            this.gestureDetectors = new Collection<GestureDetector>();
            HorizontalSwipeGestureDetector swipeGestureDetector = new HorizontalSwipeGestureDetector(this.activeHand, 50, 1200, this.PlayerBodySize * 0.95f);
            this.gestureDetectors.Add(swipeGestureDetector);
            PushGestureDetector pushGestureDetector = new PushGestureDetector(this.activeHand, 30, 1000, this.PlayerBodySize * 0.45f);
            this.gestureDetectors.Add(pushGestureDetector);
            VerticalSwipeGestureDetector rightLegSwipeGestureDetector = new VerticalSwipeGestureDetector(JointType.FootRight, 60, 10, 0.053f * this.PlayerBodySize, 0.3f, 200, 2000, true, true);
            this.gestureDetectors.Add(rightLegSwipeGestureDetector);
            VerticalSwipeGestureDetector leftLegSwipeGestureDetector = new VerticalSwipeGestureDetector(JointType.FootLeft, 60, 10, 0.053f * this.PlayerBodySize, 0.3f, 200, 2000, true, true);
            this.gestureDetectors.Add(leftLegSwipeGestureDetector);
            VerticalSwipeGestureDetector headSwipeGestureDetector = new VerticalSwipeGestureDetector(JointType.Head, 45, 400, 0.67f * this.PlayerBodySize, 0.4f, 400, 1800);
            this.gestureDetectors.Add(headSwipeGestureDetector);
        }
    }
}

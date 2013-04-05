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
        /// The time the inactive hand must be ahead of the active hand for to trigger an active hand switch.
        /// </summary>
        private const float HandSwitchDelay = 0.6f;

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

        /// <summary>
        /// A timer to trigger an active hand switch.
        /// </summary>
        private float handSwitchTimer;

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
            this.handSwitchTimer = 0.0f;
            this.PlayerBodySize = 0.0f;
            this.jumpThreshold = 0.1f;
            this.standardSpineY = 0.0f;
            this.runTimeLimit = 1.5;
            this.lastLegLiftCounter = 0.0f;
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
        /// Gets a value indicating whether the player is using their right hand or not.
        /// </summary>
        public bool IsActiveHandRight
        {
            get
            {
                return this.activeHand == JointType.HandRight;
            }
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
            this.detectedGestures.Clear();
            this.lastLegLiftCounter = 0.0f;
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
            if (skeleton != null)
            {
                this.skeletonJoints = skeleton.Joints;
                this.standardSpineY = this.skeletonJoints[JointType.Spine].Position.Y;
                this.PlayerBodySize = this.CalculateBodySize();
                this.SetGesturesToPlayer();
            }
        }

        /// <summary>
        /// Updates all the gesture detectors based on the skeleton passed in.
        /// </summary>
        /// <param name="skeleton">The skeleton being tracked.</param>
        /// <param name="gameTime">The game time.</param>
        /// <param name="lookForEditorPoses">A value indicating whether to check for editor poses or not.</param>
        /// <returns>A value indicating whether the player's body size has changed or not.</returns>
        public bool Update(Skeleton skeleton, GameTime gameTime, bool lookForEditorPoses)
        {
            if (this.lastLegLiftCounter < this.runTimeLimit)
            {
                this.lastLegLiftCounter += gameTime.ElapsedGameTime.TotalSeconds;
            }

            this.skeletonJoints = skeleton.Joints;

            if (lookForEditorPoses)
            {
                this.UpdateHandTracking(gameTime);
                this.CheckForPoses();
            }

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
        /// Checks for any poses which are not handled by the gesture detectors.
        /// </summary>
        private void CheckForPoses()
        {
            if (this.skeletonJoints[JointType.HandLeft].TrackingState == JointTrackingState.NotTracked || this.skeletonJoints[JointType.HandRight].TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            if (this.activeHand == JointType.HandRight)
            {
                if (this.skeletonJoints[JointType.HandLeft].Position.Y > this.skeletonJoints[JointType.ElbowLeft].Position.Y)
                {
                    this.detectedGestures.Enqueue(GestureType.Place);
                }

                if (this.skeletonJoints[JointType.HandLeft].Position.X > this.skeletonJoints[JointType.Spine].Position.X)
                {
                    this.detectedGestures.Enqueue(GestureType.Swap);
                }
            }
            else if (this.activeHand == JointType.HandLeft)
            {
                if (this.skeletonJoints[JointType.HandRight].Position.Y > this.skeletonJoints[JointType.ElbowRight].Position.Y)
                {
                    this.detectedGestures.Enqueue(GestureType.Place);
                }

                if (this.skeletonJoints[JointType.HandRight].Position.X < this.skeletonJoints[JointType.Spine].Position.X)
                {
                    this.detectedGestures.Enqueue(GestureType.Swap);
                }
            }
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
                        if (distance > this.jumpThreshold && this.skeletonJoints[JointType.Spine].Position.Y > this.standardSpineY)
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
        /// Updates the hand tracking system and checks if the active hand should be swapped.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        private void UpdateHandTracking(GameTime gameTime)
        {
            JointType inactiveHand = this.activeHand == JointType.HandLeft ? JointType.HandRight : JointType.HandLeft;
            Vector3 activeHandPosition = Tools.ToVector3(this.skeletonJoints[this.activeHand].Position);
            Vector3 inactiveHandPosition = Tools.ToVector3(this.skeletonJoints[inactiveHand].Position);
            Vector3 hipPosition = Tools.ToVector3(this.skeletonJoints[JointType.HipCenter].Position);
            if (activeHandPosition.Y < hipPosition.Y && inactiveHandPosition.Y > hipPosition.Y && activeHandPosition.Z > inactiveHandPosition.Z)
            {
                this.SwapActiveHand();
                this.handSwitchTimer = 0.0f;
            }
            else if (activeHandPosition.Z > inactiveHandPosition.Z)
            {
                this.handSwitchTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.handSwitchTimer >= GestureManager.HandSwitchDelay)
                {
                    this.SwapActiveHand();
                    this.handSwitchTimer = 0.0f;
                }
            }
            else
            {
                this.handSwitchTimer = 0.0f;
            }
        }

        /// <summary>
        /// Swaps which hand is being actively tracked and resets the hand based gesture detectors.
        /// </summary>
        private void SwapActiveHand()
        {
            this.activeHand = this.activeHand == JointType.HandLeft ? JointType.HandRight : JointType.HandLeft;
            this.activeShoulder = this.activeHand == JointType.HandLeft ? JointType.ShoulderLeft : JointType.ShoulderRight;
            for (int count = 0; count < this.gestureDetectors.Count; count++)
            {
                if (this.gestureDetectors[count].JointToTrack == JointType.HandLeft || this.gestureDetectors[count].JointToTrack == JointType.HandRight)
                {
                    this.gestureDetectors[count].JointToTrack = this.activeHand;
                    this.gestureDetectors[count].Reset();
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

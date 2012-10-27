using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Kinect;

namespace SticKart.Gestures
{
    /// <summary>
    /// An enumeration of the different gesture types available.
    /// </summary>
    public enum GestureType 
    { 
        None, 
        SwipeToLeft, 
        SwipeToRight, 
        SwipeUp, 
        SwipeDown, 
        Push, 
        Jump, 
        Crouch 
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
        /// Initializes a new instance of the <see cref="GestureManager"/> class.
        /// </summary>
        public GestureManager()
        {
            this.detectedGestures = new Queue<GestureType>();
            this.gestureDetectors = new Collection<GestureDetector>();
            this.skeletonJoints = null;

            SwipeGestureDetector swipeGestureDetector = new SwipeGestureDetector();
            this.gestureDetectors.Add(swipeGestureDetector);
            // TODO: add gesture detectors.
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
                // TODO: check joint.Trackingstate
                gestureDetector.Add(this.skeletonJoints[gestureDetector.JointToTrack].Position);
                if (gestureDetector.GestureDetected != GestureType.None)
                {
                    this.detectedGestures.Enqueue(gestureDetector.GestureDetected);
                    gestureDetector.Reset();
                }
            }
        }
    }
}

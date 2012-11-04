namespace SticKart.Input.Gestures
{
    /// <summary>
    /// An enumeration of the different gesture types available.
    /// </summary>
    public enum GestureType
    {
        /// <summary>
        /// No gesture. To be used in place of null.
        /// </summary>
        None,

        /// <summary>
        /// A horizontal swiping gesture to the left.
        /// </summary>
        SwipeLeft,

        /// <summary>
        /// A horizontal swiping gesture to the right.
        /// </summary>
        SwipeRight,

        /// <summary>
        /// A vertical swiping gesture upwards.
        /// </summary>
        SwipeUp,

        /// <summary>
        /// A vertical swiping gesture downwards.
        /// </summary>
        SwipeDown,

        /// <summary>
        /// A horizontal push gesture towards the sensor.
        /// </summary>
        Push,

        /// <summary>
        /// A jump gesture composed of both feet leaving the ground at the same or almost he same time.
        /// </summary>
        Jump,

        /// <summary>
        /// A running gesture composed of each foot leaving the ground one after the other in quick succession.
        /// </summary>
        Run,

        /// <summary>
        /// A quick fall in the head's position.
        /// </summary>
        Crouch,
        
        /// <summary>
        /// A quick rise in the head's position.
        /// </summary>
        Stand
    }
}

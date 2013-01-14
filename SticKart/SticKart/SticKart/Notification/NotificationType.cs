// -----------------------------------------------------------------------
// <copyright file="NotificationType.cs" company="None">
// Copyright Keith Cully 2013.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Notification
{
    /// <summary>
    /// An enumeration of notification types.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// No notification.
        /// </summary>
        None,

        /// <summary>
        /// A notification to tell the player to step back.
        /// </summary>
        StepBack,

        /// <summary>
        /// A notification on how to perform a push gesture.
        /// </summary>
        PushGesture,

        /// <summary>
        /// A notification on how to perform a swipe gesture.
        /// </summary>
        SwipeGesture,

        /// <summary>
        /// A notification on how to perform a voice command.
        /// </summary>
        VoiceCommand,

        /// <summary>
        /// A notification on how to control the stick man's run action.
        /// </summary>
        Run,

        /// <summary>
        /// A notification on how to control the stick man's jump action.
        /// </summary>
        JumpUp,

        /// <summary>
        /// A notification on how to control the stick man's jump down action.
        /// </summary>
        JumpDown,

        /// <summary>
        /// A notification on how to control the stick man's crouch action.
        /// </summary>
        Crouch,
        
        /// <summary>
        /// A notification about the scrolling death that awaits.
        /// </summary>
        ScrollingDeath,

        /// <summary>
        /// A notification about how to exit a level.
        /// </summary>
        Exit,

        /// <summary>
        /// A notification about how to use the cart.
        /// </summary>
        Cart,

        /// <summary>
        /// A notification about bonuses.
        /// </summary>
        Bonus,

        /// <summary>
        /// A notification about obstacles.
        /// </summary>
        Obstacle,

        /// <summary>
        /// A notification about power ups.
        /// </summary>
        PowerUp
    }
}

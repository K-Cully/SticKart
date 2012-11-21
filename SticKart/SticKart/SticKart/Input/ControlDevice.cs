// -----------------------------------------------------------------------
// <copyright file="ControlDevice.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Input
{
    /// <summary>
    /// An enumeration of control devices which can be used with the input manager.
    /// </summary>
    public enum ControlDevice
    {
        /// <summary>
        /// A kinect sensor.
        /// </summary>
        Kinect,

        /// <summary>
        /// A keyboard.
        /// </summary>
        Keyboard,

        /// <summary>
        /// An Xbox360 controller.
        /// </summary>
        GamePad,

        /// <summary>
        /// A touch screen.
        /// </summary>
        Touch
    }
}

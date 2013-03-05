// -----------------------------------------------------------------------
// <copyright file="InputCommand.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Input
{
    /// <summary>
    /// An enumeration of commands which can be received from the input manager.
    /// </summary>
    public enum InputCommand
    {
        /// <summary>
        /// No input command. To be used in place of null.
        /// </summary>
        None,

        /// <summary>
        /// Select up command for menu navigation.
        /// </summary>
        Up,

        /// <summary>
        /// Select down command for menu navigation.
        /// </summary>
        Down,

        /// <summary>
        /// Select left command for menu navigation.
        /// </summary>
        Left,

        /// <summary>
        /// Select right command for menu navigation.
        /// </summary>
        Right,

        /// <summary>
        /// Player jump command.
        /// </summary>
        Jump,

        /// <summary>
        /// Player stand command.
        /// </summary>
        Stand,

        /// <summary>
        /// Player crouch command.
        /// </summary>
        Crouch,

        /// <summary>
        /// Player run command.
        /// </summary>
        Run,

        /// <summary>
        /// Command to select the currently highlighted menu item.
        /// </summary>
        Select,

        /// <summary>
        /// Command to select the menu item at the cursor position.
        /// </summary>
        SelectAt,

        /// <summary>
        /// Command to pause the game.
        /// </summary>
        Pause,

        /// <summary>
        /// Command to exit the game.
        /// </summary>
        Exit,

        /// <summary>
        /// Command to go to the next page.
        /// </summary>
        NextPage,

        /// <summary>
        /// Command to go to the last page.
        /// </summary>
        PreviousPage,

        /// <summary>
        /// Command to go to place an object.
        /// </summary>
        Place
    }
}

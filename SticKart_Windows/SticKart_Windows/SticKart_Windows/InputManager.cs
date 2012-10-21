using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SticKart_Windows
{
    /// <summary>
    /// A wrapper to manage input commands for the game SticKart.
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// An enumeration of commands which can be received from this input manager.
        /// </summary>
        public enum Command { Up, Down, Left, Right, Jump, Crouch, Run, Select, Pause, Exit }

        /// <summary>
        /// An enumeration of control devices which can be used with this input manager.
        /// </summary>
        public enum ControlDevice { Kinect, Keyboard, GamePad, Touch }

        #region privateVariables

        /// <summary>
        /// A structure to hold the current keyboard state.
        /// </summary>
        private KeyboardState keyboardState;

        /// <summary>
        /// A structure to hold the current game pad state.
        /// </summary>
        private GamePadState gamePadstate;

        /// <summary>
        /// The dimensions of the graphics viewport.
        /// </summary>
        private Vector2 screenDimensions;

        /// <summary>
        /// A list of the commands received in any given frame.
        /// </summary>
        private List<Command> commands;

        /// <summary>
        /// The active control device.
        /// </summary>
        private ControlDevice controlDevice;

        #endregion

        // TODO: Put Kinect interface here

        #region accessors

        /// <summary>
        /// Gets a list of commands received in any frame. 
        /// </summary>
        public List<Command> Commands
        {
            get
            {
                return this.commands;
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Initalises a new instance of the <see cref="InputManager"/> class.
        /// </summary>
        /// <param name="screenDimensions">The dimensions of the graphics viewport.</param>
        /// <param name="controlDevice">The type of device to take input from.</param>
        public InputManager(Vector2 screenDimensions, ControlDevice controlDevice)
        {
            this.screenDimensions = screenDimensions;
            this.controlDevice = controlDevice;
            this.commands = new List<Command>();
        }

        #endregion

        #region privateMethods

        /// <summary>
        /// Adds commands to the command list based on game pad input. 
        /// </summary>
        private void GetGamePadInput()
        {
            this.gamePadstate = GamePad.GetState(PlayerIndex.One);
            // TODO
        }

        /// <summary>
        /// Adds commands to the command list based on Kinect input. 
        /// </summary>
        private void GetKinectInput()
        {
            // TODO
        }

        /// <summary>
        /// Adds commands to the command list based on keyboard input. 
        /// </summary>
        private void GetKeyboardInput()
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                commands.Add(Command.Exit);
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.P))
                {
                    commands.Add(Command.Pause);
                }
                else
                {
                    if (keyboardState.IsKeyDown(Keys.Enter))
                    {
                        commands.Add(Command.Select);
                    }
                    if (keyboardState.IsKeyDown(Keys.W))
                    {
                        commands.Add(Command.Up);
                    }
                    if (keyboardState.IsKeyDown(Keys.S))
                    {
                        commands.Add(Command.Down);
                    }
                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        commands.Add(Command.Left);
                    }
                    if (keyboardState.IsKeyDown(Keys.D))
                    {
                        commands.Add(Command.Right);
                    }
                }
            }
        }

        /// <summary>
        /// Adds commands to the command list based on touch input. 
        /// </summary>
        private void GetTouchInput()
        {
            // TODO
        }

        #endregion

        #region publicMethods

        /// <summary>
        /// Checks for user input this frame.
        /// </summary>
        /// <returns>Whether any new commands have been picked up or not.</returns>
        public bool Update()
        {
            this.commands.Clear();
            switch (controlDevice)
            {
                case ControlDevice.Kinect:
                    this.GetKinectInput();
                    break;
                case ControlDevice.Keyboard:
                    this.GetKeyboardInput();
                    break;
                case ControlDevice.GamePad:
                    this.GetGamePadInput();
                    break;
                case ControlDevice.Touch:
                    this.GetTouchInput();
                    break;
                default:
                    break;
            }

            return this.commands.Count > 0;
        }

        #endregion
    }
}

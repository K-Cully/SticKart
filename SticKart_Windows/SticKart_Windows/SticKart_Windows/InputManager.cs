using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Kinect;

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
        public enum Command { Up, Down, Left, Right, Jump, Crouch, Run, Select, SelectAt, Pause, Exit }

        /// <summary>
        /// An enumeration of control devices which can be used with this input manager.
        /// </summary>
        public enum ControlDevice { Kinect, Keyboard, GamePad, Touch }

        #region privateVariables

        /// <summary>
        /// The Kinect sensor, if any, used by the input manager.
        /// </summary>
        KinectSensor kinectSensor;

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

        /// <summary>
        /// The position coordinate which the user selected.
        /// </summary>
        private Vector2 selectionPosition;

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

        /// <summary>
        /// Gets the screen position selected by the user.
        /// If this is equal to Vector2.Zero then no position was selected.
        /// </summary>
        public Vector2 SelectionPosition
        {
            get
            {
                // TODO: Will possibly have to convert from input space to screen space.
                return this.selectionPosition;
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
            this.selectionPosition = Vector2.Zero;
            this.screenDimensions = screenDimensions;
            this.controlDevice = controlDevice;
            this.commands = new List<Command>();
            this.kinectSensor = null;

            if (this.controlDevice == ControlDevice.Kinect)
            {
                if (!this.TryStartKinect())
                {
                    this.kinectSensor = null;
                    this.controlDevice = ControlDevice.Keyboard;
                }
            }
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

        /// <summary>
        /// Tries to start the Kinect sensor.
        /// </summary>
        /// <returns>Whether the Kinect was successfully started or not.</returns>
        private bool TryStartKinect()
        {
            bool successful = true;
            if (KinectSensor.KinectSensors.Count > 0) // At least one sensor is connected.
            {
                this.kinectSensor = KinectSensor.KinectSensors[0];
                if (this.kinectSensor.Status == KinectStatus.Connected)
                {
                    this.kinectSensor.DepthStream.Enable();
                    this.kinectSensor.SkeletonStream.Enable();
                    // TODO: Replace events with polling.
                    this.kinectSensor.AllFramesReady += new System.EventHandler<AllFramesReadyEventArgs>(kinectSensor_AllFramesReady);
                    try
                    {
                        this.kinectSensor.Start();
                    }
                    catch (IOException) // Kinect sensor is in use by another process.
                    {
                        successful = false;
                    }
                }
            }
            else
            {
                successful = false;
            }

            return successful;
        }

        /// <summary>
        /// Stops the Kinect sensor if it is active.
        /// </summary>
        private void StopKinect()
        {
            if (this.kinectSensor != null)
            {
                this.kinectSensor.Stop();
                this.kinectSensor.AudioSource.Stop();
            }
        }
        
        /// <summary>
        /// An event handler which is activated once all active data streams on the Kinect sensor are ready for querying.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void kinectSensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            // TODO: Implement this or replace with polling.
            // throw new System.NotImplementedException();
        }

        #endregion

        #region publicMethods

        /// <summary>
        /// Disposes of any resources used by the input manager.
        /// </summary>
        public void Dispose()
        {
            this.StopKinect();
        }

        /// <summary>
        /// Checks for user input this frame.
        /// </summary>
        /// <returns>Whether any new commands have been picked up or not.</returns>
        public bool Update()
        {
            this.selectionPosition = Vector2.Zero;
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

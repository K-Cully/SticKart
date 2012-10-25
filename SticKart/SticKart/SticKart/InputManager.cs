using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Kinect;

namespace SticKart
{
    /// <summary>
    /// A wrapper to manage input commands for the game SticKart.
    /// </summary>
    public class InputManager
    {
        #region enums

        /// <summary>
        /// An enumeration of commands which can be received from this input manager.
        /// </summary>
        public enum Command { Up, Down, Left, Right, Jump, Crouch, Run, Select, SelectAt, Pause, Exit }

        /// <summary>
        /// An enumeration of control devices which can be used with this input manager.
        /// </summary>
        public enum ControlDevice { Kinect, Keyboard, GamePad, Touch }

        #endregion

        #region kinect

        /// <summary>
        /// The Kinect sensor, if any, used by the input manager.
        /// </summary>
        private KinectSensor kinectSensor;

        /// <summary>
        /// The coordinate mapper for the Kinect sensor.
        /// </summary>
        private CoordinateMapper coordinateMapper;

        /// <summary>
        /// The last frame's skeleton data.
        /// </summary>
        private Skeleton[] skeletonData;

        /// <summary>
        /// The current skeleton frame coming from the Kinect.
        /// </summary>
        private SkeletonFrame skeletonFrame;

        #endregion

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

        /// <summary>
        /// The position coordinate which the user selected.
        /// </summary>
        private Vector2 selectionPosition;

        #endregion
        
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
            this.coordinateMapper = null;

            if (this.controlDevice == ControlDevice.Kinect)
            {
                if (!this.TryStartKinect())
                {
                    this.kinectSensor = null;
                    this.controlDevice = ControlDevice.Keyboard;
                }
                else
                {
                    this.coordinateMapper = new CoordinateMapper(this.kinectSensor);
                }
            }
        }
                
        #endregion

        #region inputMethods

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
            // TODO: wrap and add gesture tracking to skeleton data
            if (this.ReadSkelletonFrame())
            {
                // TODO: check if primary skelleton is always [0]
                foreach (Skeleton skeleton in skeletonData)
                {
                    switch (skeleton.TrackingState)
                    {
                        case SkeletonTrackingState.NotTracked:
                            break;
                        case SkeletonTrackingState.PositionOnly:
                            break;
                        case SkeletonTrackingState.Tracked:
                            // TODO: log position of crucial joints and monitor over time. 
                            // TODO: check joint.Trackingstate
                            SkeletonPoint leftHandPos = skeleton.Joints[JointType.HandLeft].Position;
                            SkeletonPoint rightHandPos = skeleton.Joints[JointType.HandRight].Position;
                            SkeletonPoint headPos = skeleton.Joints[JointType.Head].Position;
                            SkeletonPoint leftFootPos = skeleton.Joints[JointType.FootLeft].Position;
                            SkeletonPoint rightFootPos = skeleton.Joints[JointType.FootRight].Position;
                            break;
                        default:
                            break;
                    }
                }
            }            
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
            else if (keyboardState.IsKeyDown(Keys.P))
            {
                commands.Add(Command.Pause);
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    commands.Add(Command.Select);
                }
                if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                {
                    commands.Add(Command.Up);
                    commands.Add(Command.Jump);
                }
                else if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                {
                    commands.Add(Command.Down);
                    commands.Add(Command.Crouch);
                }
                else if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                {
                    commands.Add(Command.Left);
                }
                else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                {
                    commands.Add(Command.Right);
                    commands.Add(Command.Run);
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

        #region kinectMethods

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
                    this.kinectSensor.DepthStream.Enable(); // Need this enabled for coordinate mapping.
                    this.kinectSensor.SkeletonStream.Enable();
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
        /// Reads the current skelleton frame from the skelleton stream of the kinect sensor.
        /// </summary>
        /// <returns>Whether a frame was read or not.</returns>
        private bool ReadSkelletonFrame()
        {
            using (this.skeletonFrame = this.kinectSensor.SkeletonStream.OpenNextFrame(0))
            {
                // Sometimes we get a null frame back if no data is ready
                if (null == skeletonFrame)
                {
                    return false;
                }

                // Reallocate if necessary
                if (null == skeletonData || skeletonData.Length != skeletonFrame.SkeletonArrayLength)
                {
                    skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                }

                skeletonFrame.CopySkeletonDataTo(skeletonData);
                return true;
            }
        }

        #endregion

        #region interface

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

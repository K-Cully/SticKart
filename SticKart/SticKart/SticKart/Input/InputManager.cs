﻿// -----------------------------------------------------------------------
// <copyright file="InputManager.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Input
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Display.Notification;
    using Gestures;
    using Kinect.Toolbox;
    using Menu;
    using Microsoft.Kinect;
    using Microsoft.Speech.AudioFormat;
    using Microsoft.Speech.Recognition;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Input.Touch;
    
    /// <summary>
    /// A wrapper to manage input commands for the game.
    /// </summary>
    public class InputManager
    {
        #region constants

        /// <summary>
        /// Speech utterance confidence threshold, below which speech is treated as if it hadn't been heard.
        /// </summary> 
        private const double SpeechConfidenceThreshold = 0.6;

        /// <summary>
        /// The delay to apply between key presses.
        /// </summary>
        private const int KeyDelayMillisceonds = 150;

        /// <summary>
        /// The amount of time which should pass before logging a foot tracking state.
        /// </summary>
        private const float FootTrackingTime = 0.25f;

        /// <summary>
        /// The maximum number of foot tracking states to log.
        /// </summary>
        private const int MaxLoggedFootStates = 16;

        /// <summary>
        /// The delay, in seconds, to apply between angle resets.
        /// </summary>
        private const float AngleResetTime = 10.0f;

        #endregion

        #region kinect_motion_variables

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
        /// A list of values indicating whether the player's feet were tracked at specific points in time.
        /// </summary>
        private List<bool> footTrackingLog;

        /// <summary>
        /// A timer for logging foot tracking states.
        /// </summary>
        private float footTrackingTimer;

        /// <summary>
        /// The current skeleton frame coming from the Kinect.
        /// </summary>
        private SkeletonFrame skeletonFrame;

        /// <summary>
        /// A value indicating whether the colour stream should be read or not.
        /// </summary>
        private bool colourStreamEnabled;

        /// <summary>
        /// The current colour frame coming from the Kinect.
        /// </summary>
        private ColorImageFrame colourFrame;

        /// <summary>
        /// The last frame's colour data.
        /// </summary>
        private byte[] colourData;

        /// <summary>
        /// A value indicating whether the angle of the Kinect sensor has been adjusted to the player or not.
        /// </summary>
        private bool kinectAngleSet;

        /// <summary>
        /// The maximum angle allowed between the Kinect sensor and the main tracking point on the player.
        /// </summary>
        private float thresholdAngleToTrackingPoint;

        /// <summary>
        /// The gesture manager to use for monitoring gestures.
        /// </summary>
        private GestureManager gestureManager;

        /// <summary>
        /// The minimum distance the player must be at to play.
        /// </summary>
        private float minimumPlayerDistance;

        /// <summary>
        /// The maximum distance the player can be at to play.
        /// </summary>
        private float maximumPlayerDistance;

        /// <summary>
        /// The optimal position for the player to be standing, in sensor view space.
        /// </summary>
        private Vector3 optimalPosition;

        /// <summary>
        /// A timer for the angle reset delay.
        /// </summary>
        private float angleResetTimer;

        #endregion

        #region kinect_speech_variables
        
        /// <summary>
        /// Speech recognition engine using audio data from Kinect.
        /// </summary>
        private SpeechRecognitionEngine speechEngine;
        
        /// <summary>
        /// The selected word.
        /// </summary>
        private string selectedWord;
        
        #endregion

        #region private_variables

        /// <summary>
        /// A structure to hold the current keyboard state.
        /// </summary>
        private KeyboardState keyboardState;

        /// <summary>
        /// A structure to hold the current game pad state.
        /// </summary>
        private GamePadState gamePadstate;
        
        /// <summary>
        /// A structure to hold the currently available touch gesture.
        /// </summary>
        private GestureSample touchGesture;        

        /// <summary>
        /// The dimensions of the graphics viewport.
        /// </summary>
        private Vector2 screenDimensions;

        /// <summary>
        /// A list of the commands received in any given frame.
        /// </summary>
        private List<InputCommand> commands;

        /// <summary>
        /// The active control device.
        /// </summary>
        private ControlDevice controlDevice;

        /// <summary>
        /// The position coordinate which the user selected.
        /// </summary>
        private Vector2 selectionPosition;

        /// <summary>
        /// The time of the last key press.
        /// </summary>
        private DateTime lastKeyPressTime;

        #endregion
        
        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InputManager"/> class.
        /// </summary>
        /// <param name="screenDimensions">The dimensions of the graphics viewport.</param>
        /// <param name="controlDevice">The type of device to take input from.</param>
        /// <param name="enableColourStream">A value indicating whether to enable the colour stream or not.</param>
        public InputManager(Vector2 screenDimensions, ControlDevice controlDevice, bool enableColourStream = false)
        {
            this.footTrackingLog = new List<bool>();
            for (int count = 0; count < InputManager.MaxLoggedFootStates; count++)
            {
                this.footTrackingLog.Add(true);
            }

            this.footTrackingTimer = 0.0f;
            this.ColourFrameSize = Vector2.Zero;
            this.selectionPosition = Vector2.Zero;
            this.screenDimensions = screenDimensions;
            this.controlDevice = controlDevice;
            this.commands = new List<InputCommand>();
            this.kinectSensor = null;
            this.coordinateMapper = null;
            this.gestureManager = null;
            this.minimumPlayerDistance = 2.30f;
            this.maximumPlayerDistance = 3.15f;
            this.optimalPosition = new Vector3(0.0f, 0.0f, this.minimumPlayerDistance + 0.2f);
            this.PlayerFloorPosition = Vector2.Zero;
            this.selectedWord = null;
            this.touchGesture = new GestureSample();
            this.keyboardState = new KeyboardState();
            this.gamePadstate = new GamePadState();
            this.lastKeyPressTime = DateTime.UtcNow;
            this.kinectAngleSet = false;
            this.thresholdAngleToTrackingPoint = 0.1f;
            this.colourStreamEnabled = false;
            this.angleResetTimer = 5.0f;

            if (this.controlDevice == ControlDevice.Kinect)
            {
                this.colourStreamEnabled = enableColourStream;
                this.InitializeKinect();
            }
        }        
                
        #endregion

        #region accessors

        /// <summary>
        /// Gets a list of commands received in any frame. 
        /// </summary>
        public List<InputCommand> Commands
        {
            get
            {
                return this.commands;
            }
        }

        /// <summary>
        /// Gets the data from the last colour frame read from the Kinect sensor.
        /// </summary>
        public byte[] ColourData
        {
            get
            {
                if (this.colourStreamEnabled)
                {
                    return this.colourData;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the size of the colour frame read in from the Kinect sensor.
        /// </summary>
        public Vector2 ColourFrameSize { get; private set; }

        /// <summary>
        /// Gets a value indicating whether there is a word available or not.
        /// </summary>
        public bool VoiceCommandAvailable
        {
            get
            {
                if (this.selectedWord == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets the available voice command if any.
        /// </summary>
        public string LastVoiceCommand
        {
            get
            {
                string word = this.selectedWord;
                this.selectedWord = null;
                return word;
            }
        }

        /// <summary>
        /// Gets the screen position selected by the user.
        /// If this is equal to Vector2.Zero then no position was selected.
        /// </summary>
        public Vector2 SelectedPosition
        {
            get
            {
                switch (this.controlDevice)
                {
                    case ControlDevice.Kinect:
                        this.selectionPosition = this.HandPosition;
                        break;
                    case ControlDevice.Touch:
                        break;
                    default:
                        this.selectionPosition = Vector2.Zero;
                        break;
                }

                return this.selectionPosition;
            }
        }

        /// <summary>
        /// Gets the screen coordinates of the active hand.
        /// </summary>
        public Vector2 HandPosition
        {
            get
            {
                if (this.kinectSensor == null)
                {
                    return Vector2.Zero;
                }
                else
                {
                    float bodySize = this.gestureManager.PlayerBodySize != 0.0f ? this.gestureManager.PlayerBodySize : 0.65f;
                    Vector2 scaling = this.screenDimensions * 1.0f / bodySize;
                    Vector3 originalHandPosition = Tools.ToVector3(this.gestureManager.HandPosition);
                    Vector3 handUnit = originalHandPosition;
                    handUnit.Normalize();
                    float depthToShoulder = this.gestureManager.ShoulderPosition.Z - originalHandPosition.Z;
                    originalHandPosition += handUnit * depthToShoulder;

                    Vector2 handPosition = new Vector2(originalHandPosition.X, originalHandPosition.Y);
                    Vector2 shoulderPosition = new Vector2(this.gestureManager.ShoulderPosition.X, this.gestureManager.ShoulderPosition.Y);                    
                    Vector2 relativeHandPosition = handPosition - shoulderPosition;
                    relativeHandPosition.X *= scaling.X;
                    relativeHandPosition.Y *= -scaling.Y;
                    return (this.screenDimensions / 2.0f) + relativeHandPosition;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the player is using their right hand or not.
        /// </summary>
        public bool IsActiveHandRight
        {
            get
            {
                if (this.kinectSensor == null || this.controlDevice != ControlDevice.Kinect)
                {
                    return true;
                }
                else
                {
                    return this.gestureManager.IsActiveHandRight;
                }
            }
        }

        /// <summary>
        /// Gets the position of the player on a plane horizontal to the Kinect sensor.
        /// </summary>
        public Vector2 PlayerFloorPosition { get; private set; }

        #endregion

        #region public_methods

        /// <summary>
        /// Disposes of any resources used by the input manager.
        /// </summary>
        public void Dispose()
        {
            this.StopKinect();
        }

        /// <summary>
        /// Initializes the speech engine and adds the menu commands passed in into it as recognizable grammar.
        /// </summary>
        /// <param name="menuCommandNames">The list of menu item names to add to the speech engine.</param>
        public void InitializeSpeechEngine(List<string> menuCommandNames)
        {
            if (this.controlDevice == ControlDevice.Kinect && this.kinectSensor != null)
            {
                if (!this.TryStartSpeechEngine(menuCommandNames))
                {
                    this.speechEngine = null;
                }
            }
        }

        /// <summary>
        /// Checks for user input this frame.
        /// </summary>
        /// <returns>Whether any new commands have been picked up or not.</returns>
        /// <param name="gameTime">The game time.</param>
        /// <param name="allowReset">A value indicating whether the input system may be reset or not.</param>
        /// <param name="allowEdit">A value indicating whether to accept edit commands.</param>
        public bool Update(GameTime gameTime, bool allowReset, bool allowEdit)
        {
            this.selectionPosition = Vector2.Zero;
            this.commands.Clear();
            switch (this.controlDevice)
            {
                case ControlDevice.Kinect:
                    this.GetKinectInput(gameTime, allowReset, allowEdit);
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

        /// <summary>
        /// Flushes the input manager.
        /// Should be called at the start of each level.
        /// </summary>
        public void Reset()
        {
            this.Commands.Clear();
            if (this.gestureManager != null)
            {
                this.gestureManager.ResetGestures();
                this.gestureManager.ResetPlayerSettings(this.GetClosestSkeleton());
            }
        }

        #endregion

        #region input_methods

        /// <summary>
        /// Adds commands to the command list based on game pad input. 
        /// </summary>
        private void GetGamePadInput()
        {
            if (this.gamePadstate != null && DateTime.UtcNow.Subtract(this.lastKeyPressTime).TotalMilliseconds > InputManager.KeyDelayMillisceonds)
            {
                this.gamePadstate = GamePad.GetState(PlayerIndex.One);
                this.lastKeyPressTime = DateTime.UtcNow;
                if (this.gamePadstate.Buttons.Back == ButtonState.Pressed)
                {
                    this.commands.Add(InputCommand.Exit);
                }
                else if (this.gamePadstate.Buttons.Start == ButtonState.Pressed)
                {
                    this.commands.Add(InputCommand.Pause);
                }
                else
                {
                    if (this.gamePadstate.Buttons.A == ButtonState.Pressed)
                    {
                        this.commands.Add(InputCommand.Select);
                    }

                    if (this.gamePadstate.ThumbSticks.Left.Y > 0.1f || this.gamePadstate.DPad.Up == ButtonState.Pressed)
                    {
                        this.commands.Add(InputCommand.Up);
                        this.commands.Add(InputCommand.Jump);
                    }
                    else if (this.gamePadstate.ThumbSticks.Left.Y < -0.1f || this.gamePadstate.DPad.Down == ButtonState.Pressed)
                    {
                        this.commands.Add(InputCommand.Down);
                        this.commands.Add(InputCommand.Crouch);
                    }
                    else if (this.gamePadstate.IsButtonDown(Buttons.RightTrigger))
                    {
                        this.commands.Add(InputCommand.Run);
                    }

                    if (this.gamePadstate.ThumbSticks.Left.X > 0.1f || this.gamePadstate.DPad.Right == ButtonState.Pressed)
                    {
                        this.commands.Add(InputCommand.Right);
                    }
                    else if (this.gamePadstate.ThumbSticks.Left.X < -0.1f || this.gamePadstate.DPad.Left == ButtonState.Pressed)
                    {
                        this.commands.Add(InputCommand.Left);
                    }
                }
            }
        }

        /// <summary>
        /// Adds commands to the command list based on Kinect input. 
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="allowReset">A value indicating Whether the gesture system is allowed be reset or not.</param>
        /// <param name="allowEdit">A value indicating whether to accept edit commands.</param>
        private void GetKinectInput(GameTime gameTime, bool allowReset, bool allowEdit)
        {
            if (this.angleResetTimer < InputManager.AngleResetTime)
            {
                allowReset = false;
                this.angleResetTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (this.footTrackingTimer < InputManager.FootTrackingTime)
            {
                this.footTrackingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (this.colourStreamEnabled)
            {
                this.ReadColourFrame();
            }

            if (this.ReadSkeletonFrame())
            {
                this.ProcessSkeleton(gameTime, allowReset, allowEdit, this.GetClosestSkeleton());
            }         
        }

        /// <summary>
        /// Adds commands to the command list based on keyboard input. 
        /// </summary>
        private void GetKeyboardInput()
        {
            if (this.keyboardState != null && DateTime.UtcNow.Subtract(this.lastKeyPressTime).TotalMilliseconds > InputManager.KeyDelayMillisceonds)
            {
                this.keyboardState = Keyboard.GetState();
                this.lastKeyPressTime = DateTime.UtcNow;
                if (this.keyboardState.IsKeyDown(Keys.Escape))
                {
                    this.commands.Add(InputCommand.Exit);
                }
                else if (this.keyboardState.IsKeyDown(Keys.P))
                {
                    this.commands.Add(InputCommand.Pause);
                }
                else
                {
                    if (this.keyboardState.IsKeyDown(Keys.Enter))
                    {
                        this.commands.Add(InputCommand.Select);
                    }

                    if (this.keyboardState.IsKeyDown(Keys.W) || this.keyboardState.IsKeyDown(Keys.Up))
                    {
                        this.commands.Add(InputCommand.Up);
                        this.commands.Add(InputCommand.Jump);
                    }
                    else if (this.keyboardState.IsKeyDown(Keys.S) || this.keyboardState.IsKeyDown(Keys.Down))
                    {
                        this.commands.Add(InputCommand.Down);
                        this.commands.Add(InputCommand.Crouch);
                    }
                    else if (this.keyboardState.IsKeyDown(Keys.A) || this.keyboardState.IsKeyDown(Keys.Left))
                    {
                        this.commands.Add(InputCommand.Left);
                    }
                    else if (this.keyboardState.IsKeyDown(Keys.D) || this.keyboardState.IsKeyDown(Keys.Right))
                    {
                        this.commands.Add(InputCommand.Right);
                        this.commands.Add(InputCommand.Run);
                    }
                }
            }
        }

        /// <summary>
        /// Adds commands to the command list based on touch input. 
        /// </summary>
        private void GetTouchInput()
        {
            if (TouchPanel.IsGestureAvailable)
            {
                this.touchGesture = TouchPanel.ReadGesture();
                switch (this.touchGesture.GestureType)
                {
                    case Microsoft.Xna.Framework.Input.Touch.GestureType.DoubleTap:
                        this.commands.Add(InputCommand.Run);
                        break;
                    case Microsoft.Xna.Framework.Input.Touch.GestureType.Flick:
                        Vector2 velocity = this.touchGesture.Delta;
                        if (velocity.X * velocity.X > velocity.Y * velocity.Y)
                        {
                            if (velocity.X > 0.0f)
                            {
                                this.commands.Add(InputCommand.Right);
                            }
                            else
                            {
                                this.commands.Add(InputCommand.Left);
                            }
                        }
                        else
                        {
                            if (velocity.Y > 0.0f)
                            {
                                this.commands.Add(InputCommand.Down);
                                this.commands.Add(InputCommand.Crouch);
                            }
                            else
                            {
                                this.commands.Add(InputCommand.Up);
                                this.commands.Add(InputCommand.Jump);
                            }
                        }

                        break;
                    case Microsoft.Xna.Framework.Input.Touch.GestureType.Tap:
                        this.selectionPosition = this.touchGesture.Position; // TODO: scale to viewport resolution.
                        this.commands.Add(InputCommand.SelectAt);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region kinect_methods

        /// <summary>
        /// Updates the gesture manager and Kinect tracking states based on the player's skeleton.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="allowReset">A value indicating Whether the gesture system is allowed be reset or not.</param>
        /// <param name="allowEdit">A value indicating whether to accept edit commands.</param>
        /// <param name="skeleton">The skeleton to process.</param>
        private void ProcessSkeleton(GameTime gameTime, bool allowReset, bool allowEdit, Skeleton skeleton)
        {
            if (skeleton != null)
            {
                if (skeleton.Position.Z > this.minimumPlayerDistance && skeleton.Position.Z < this.maximumPlayerDistance)
                {
                    if (this.kinectAngleSet)
                    {
                        if (allowReset)
                        {
                            this.UpdateKinectTrackingState(skeleton);
                        }

                        if (this.kinectAngleSet)
                        {
                            if (allowReset)
                            {
                                this.kinectAngleSet = !this.gestureManager.Update(skeleton, gameTime, allowEdit);
                            }
                            else
                            {
                                this.gestureManager.Update(skeleton, gameTime, allowEdit);
                            }

                            this.PlayerFloorPosition = Vector2.Zero;
                            this.ApplyKinectGestures();
                        }
                    }
                    else if (this.AdjustSensorAngle(skeleton))
                    {
                        this.gestureManager.ResetPlayerSettings(skeleton);
                        this.angleResetTimer = 0.0f;
                    }
                }
                else if (skeleton.Position.Z < this.maximumPlayerDistance)
                {
                    NotificationManager.AddNotification(NotificationType.StepBack);
                    this.PlayerFloorPosition = new Vector2(skeleton.Position.Z, skeleton.Position.X);
                }
                else
                {
                    NotificationManager.AddNotification(NotificationType.StepForward);
                    this.PlayerFloorPosition = new Vector2(skeleton.Position.Z, skeleton.Position.X);
                }
            }
        }

        /// <summary>
        /// Retrieves the closest skeleton to the optimal position.
        /// </summary>
        /// <returns>The closest skeleton to the optimal position, if any.</returns>
        private Skeleton GetClosestSkeleton()
        {
            Skeleton closestSkeleton = null;
            float storedDistanceFromOptimal = float.MaxValue;
            float currentDistanceFromOptimal = 0.0f;
            float optimalRadius = 0.6f;
            foreach (Skeleton skeleton in this.skeletonData)
            {
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    if (closestSkeleton == null)
                    {
                        closestSkeleton = skeleton;
                        storedDistanceFromOptimal = (this.optimalPosition - Tools.ToVector3(skeleton.Position)).Length();
                    }
                    else
                    {
                        currentDistanceFromOptimal = (this.optimalPosition - Tools.ToVector3(skeleton.Position)).Length();
                        if (currentDistanceFromOptimal < optimalRadius || storedDistanceFromOptimal < optimalRadius)
                        {
                            if (currentDistanceFromOptimal < storedDistanceFromOptimal)
                            {
                                closestSkeleton = skeleton;
                                storedDistanceFromOptimal = currentDistanceFromOptimal;
                            }
                        }
                        else if (skeleton.Position.Z < closestSkeleton.Position.Z)
                        {
                            closestSkeleton = skeleton;
                            storedDistanceFromOptimal = (this.optimalPosition - Tools.ToVector3(skeleton.Position)).Length();
                        }
                    }
                }
            }

            return closestSkeleton;
        }

        /// <summary>
        /// Updates the current tracking state of the player by the Kinect ensor.
        /// </summary>
        /// <param name="skeleton">The current skeleton being tracked.</param>
        private void UpdateKinectTrackingState(Skeleton skeleton)
        {
            Vector3 skeletonPositionLeveled = Tools.ToVector3(skeleton.Position);
            skeletonPositionLeveled.Y = 0.0f;
            if ((this.optimalPosition - skeletonPositionLeveled).Length() < 0.3f)
            {
                this.kinectAngleSet = skeleton.Joints[JointType.Head].TrackingState != JointTrackingState.NotTracked;
                if (this.kinectAngleSet)
                {
                    if (this.footTrackingTimer > InputManager.FootTrackingTime)
                    {
                        this.footTrackingTimer = 0.0f;
                        this.footTrackingLog.RemoveAt(0);
                        this.footTrackingLog.Add(skeleton.Joints[JointType.FootRight].TrackingState == JointTrackingState.Tracked || skeleton.Joints[JointType.FootLeft].TrackingState == JointTrackingState.Tracked);
                    }

                    this.kinectAngleSet = false;
                    foreach (bool trackingLog in this.footTrackingLog)
                    {
                        if (trackingLog == true)
                        {
                            this.kinectAngleSet = trackingLog;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initializes all Kinect based control systems.
        /// Defaults to keyboard input if the Kinect is not available.
        /// </summary>
        private void InitializeKinect()
        {
            if (!this.TryStartKinect())
            {
                this.colourStreamEnabled = false;
                this.kinectSensor = null;
                this.controlDevice = ControlDevice.Keyboard;
            }
            else
            {
                this.coordinateMapper = new CoordinateMapper(this.kinectSensor);
                this.gestureManager = new GestureManager();
            }
        }

        /// <summary>
        /// Tries to start the speech engine.
        /// </summary>
        /// <param name="selectableNames">The list of menu item names to add to the speech engine.</param>
        /// <returns>Whether the speech engine was successfully started or not.</returns>
        private bool TryStartSpeechEngine(List<string> selectableNames)
        {
            RecognizerInfo recognizerInfo = this.GetKinectRecognizer();

            if (recognizerInfo == null)
            {
                return false;
            }
            else
            {
                this.speechEngine = new SpeechRecognitionEngine(recognizerInfo.Id);
                Choices grammarChoices = new Choices();
                foreach (string name in selectableNames)
                {
                    grammarChoices.Add(new SemanticResultValue(name.ToLower(), name));
                }

                grammarChoices.Add(new SemanticResultValue(MenuConstants.PauseCommandName.ToLower(), MenuConstants.PauseCommandName));
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Culture = recognizerInfo.Culture;
                grammarBuilder.Append(grammarChoices);
                Grammar grammar = new Grammar(grammarBuilder);
                this.speechEngine.LoadGrammar(grammar);
                this.speechEngine.SpeechRecognized += this.SpeechRecognized;
                this.speechEngine.SetInputToAudioStream(this.kinectSensor.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                return true;
            }
        }

        /// <summary>
        /// Tries to start the Kinect sensor.
        /// </summary>
        /// <returns>Whether the Kinect was successfully started or not.</returns>
        private bool TryStartKinect()
        {
            // Check that at least one Kinect for Windows sensor is connected and is not in use by another process.
            bool successful = true;
            if (KinectSensor.KinectSensors.Count > 0)
            {
                this.kinectSensor = KinectSensor.KinectSensors[0];
                if (this.kinectSensor.Status == KinectStatus.Connected)
                {
                    if (this.colourStreamEnabled)
                    {
                        this.kinectSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                    }

                    this.kinectSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    TransformSmoothParameters smoothing = new TransformSmoothParameters();
                    smoothing.Smoothing = 0.6f;
                    smoothing.Correction = 0.2f;
                    smoothing.JitterRadius = 0.125f;
                    smoothing.Prediction = 0.5f;
                    smoothing.MaxDeviationRadius = 0.04f;
                    this.kinectSensor.SkeletonStream.Enable(smoothing);
                    try
                    {
                        this.kinectSensor.Start();
                    }
                    catch (IOException)
                    {
                        successful = false;
                    }
                }
                else
                {
                    successful = false;
                }
            }
            else
            {
                successful = false;
            }

            return successful;
        }

        /// <summary>
        /// Stops the Kinect sensor and the speech engine if they are active.
        /// </summary>
        private void StopKinect()
        {
            if (this.kinectSensor != null)
            {
                this.kinectSensor.AudioSource.Stop();
                this.kinectSensor.Stop();
            }

            if (null != this.speechEngine)
            {
                this.speechEngine.SpeechRecognized -= this.SpeechRecognized;
                this.speechEngine.RecognizeAsyncStop();
            }
        }

        /// <summary>
        /// Adjusts the angle of the Kinect sensor so that the player is in the sensor's field of view. 
        /// </summary>
        /// <param name="skeleton">The player's skeleton.</param>
        /// <returns>A value indicating if the the angle was set to its final position.</returns>
        private bool AdjustSensorAngle(Skeleton skeleton)
        {
            bool set = false;
            if (skeleton.Joints[JointType.HipCenter].TrackingState == JointTrackingState.Tracked && skeleton.Joints[JointType.Spine].TrackingState == JointTrackingState.Tracked)
            {
                Vector2 spinePoint = Vector2.Normalize(new Vector2(skeleton.Joints[JointType.Spine].Position.Z, skeleton.Joints[JointType.Spine].Position.Y));
                Vector2 hipPoint = Vector2.Normalize(new Vector2(skeleton.Joints[JointType.HipCenter].Position.Z, skeleton.Joints[JointType.HipCenter].Position.Y));
                Vector2 trackingPoint = hipPoint - (spinePoint - hipPoint);
                float angle = MathHelper.ToDegrees((float)Math.Asin(trackingPoint.Y));
                if (angle > this.thresholdAngleToTrackingPoint || -angle > this.thresholdAngleToTrackingPoint)
                {
                    this.kinectSensor.TrySetElevationAngle(this.kinectSensor.ElevationAngle + (int)Math.Round(angle));
                }

                set = true;
                this.kinectAngleSet = true;
            }
            else if (skeleton.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked)
            {
                this.kinectSensor.TrySetElevationAngle(this.kinectSensor.ElevationAngle - 4);
            }
            else if (skeleton.Joints[JointType.FootLeft].TrackingState == JointTrackingState.Tracked)
            {
                this.kinectSensor.TrySetElevationAngle(this.kinectSensor.ElevationAngle + 4);
            }

            return set;
        }

        /// <summary>
        /// Reads the current skeleton frame from the skeleton stream of the kinect sensor.
        /// </summary>
        /// <returns>Whether a frame was read or not.</returns>
        private bool ReadSkeletonFrame()
        {
            using (this.skeletonFrame = this.kinectSensor.SkeletonStream.OpenNextFrame(0))
            {
                // Sometimes we get a null frame back if no data is ready
                if (null == this.skeletonFrame)
                {
                    return false;
                }

                // Reallocate if necessary
                if (null == this.skeletonData || this.skeletonData.Length != this.skeletonFrame.SkeletonArrayLength)
                {
                    this.skeletonData = new Skeleton[this.skeletonFrame.SkeletonArrayLength];
                }

                this.skeletonFrame.CopySkeletonDataTo(this.skeletonData);
                return true;
            }
        }

        /// <summary>
        /// Reads the current colour frame from the colour stream of the kinect sensor.
        /// </summary>
        /// <returns>Whether a frame was read or not.</returns>
        private bool ReadColourFrame()
        {
            using (this.colourFrame = this.kinectSensor.ColorStream.OpenNextFrame(0))
            {
                if (null == this.colourFrame)
                {
                    return false;
                }

                if (null == this.colourData || this.colourData.Length != this.colourFrame.PixelDataLength)
                {
                    this.colourData = new byte[this.colourFrame.PixelDataLength];
                }

                this.ColourFrameSize = new Vector2(this.colourFrame.Width, this.colourFrame.Height);
                this.colourFrame.CopyPixelDataTo(this.colourData);
                return true;
            }
        }

        /// <summary>
        /// Turns detected gestures into standard input commands.
        /// </summary>
        private void ApplyKinectGestures()
        {
            Gestures.GestureType gestureToApply;
            do
            {
                gestureToApply = this.gestureManager.GetNextDetectedGesture();
                switch (gestureToApply)
                {
                    case Gestures.GestureType.SwipeLeft:
                        this.commands.Add(InputCommand.NextPage);
                        break;
                    case Gestures.GestureType.SwipeRight:
                        this.commands.Add(InputCommand.PreviousPage);
                        break;
                    case Gestures.GestureType.Crouch:
                        this.commands.Add(InputCommand.Crouch);
                        break;
                    case Gestures.GestureType.Run:
                        this.commands.Add(InputCommand.Run);
                        break;
                    case Gestures.GestureType.Jump:
                        this.commands.Add(InputCommand.Jump);
                        break;
                    case Gestures.GestureType.Stand:
                        this.commands.Add(InputCommand.Stand);
                        break;
                    case Gestures.GestureType.Push:
                        this.commands.Add(InputCommand.SelectAt);
                        break;
                    case Gestures.GestureType.Place:
                        this.commands.Add(InputCommand.Place);
                        break;
                    case Gestures.GestureType.Swap:
                        this.commands.Add(InputCommand.Swap);
                        break;
                    default:
                        break;
                }
            }
            while (gestureToApply != Gestures.GestureType.None);
        }

        /// <summary>
        /// Gets the metadata for the speech recognizer (acoustic model) most suitable to process audio from Kinect device.
        /// </summary>
        /// <returns>RecognizerInfo if found, null otherwise.</returns>
        private RecognizerInfo GetKinectRecognizer()
        {
            RecognizerInfo kinectRecognizer = null;
            foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    kinectRecognizer = recognizer;
                }

                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && CultureInfo.CurrentCulture.Name.Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    kinectRecognizer = recognizer;
                    break;
                }
            }

            return kinectRecognizer;
        }

        /// <summary>
        /// Event handler for recognized speech events.
        /// </summary>
        /// <param name="sender">Object sending the event.</param>
        /// <param name="e">Event arguments.</param>
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= InputManager.SpeechConfidenceThreshold)
            {
                this.selectedWord = e.Result.Semantics.Value.ToString();
            }
        }

        #endregion
    }
}

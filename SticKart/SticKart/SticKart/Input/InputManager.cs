namespace SticKart.Input
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
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
        private const double SpeechConfidenceThreshold = 0.3;

        /// <summary>
        /// The delay to apply between key presses.
        /// </summary>
        private const int KeyDelayMillisceonds = 150;

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
        /// The current skeleton frame coming from the Kinect.
        /// </summary>
        private SkeletonFrame skeletonFrame;

        /// <summary>
        /// The gesture manager to use for monitoring gestures.
        /// </summary>
        private GestureManager gestureManager;

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
        public InputManager(Vector2 screenDimensions, ControlDevice controlDevice)
        {
            this.selectionPosition = Vector2.Zero;
            this.screenDimensions = screenDimensions;
            this.controlDevice = controlDevice;
            this.commands = new List<InputCommand>();
            this.kinectSensor = null;
            this.coordinateMapper = null;
            this.gestureManager = null;
            this.selectedWord = null;
            this.touchGesture = new GestureSample();
            this.keyboardState = new KeyboardState();
            this.gamePadstate = new GamePadState();
            this.lastKeyPressTime = DateTime.UtcNow;

            if (this.controlDevice == ControlDevice.Kinect)
            {
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
                    Vector2 position = Tools.Convert(this.kinectSensor, this.gestureManager.HandPosition, this.coordinateMapper);
                    position.X *= this.screenDimensions.X;
                    float centerOffsetX = position.X - (this.screenDimensions.X / 2.0f);
                    float scaleX = Math.Min(Math.Max(1.0f, (centerOffsetX * 0.00390625f) * (centerOffsetX * 0.00390625f)), 1.6f);
                    position.X = (this.screenDimensions.X / 2.0f) + (centerOffsetX * scaleX);
                    position.Y *= this.screenDimensions.Y * 2.0f;
                    return position;
                }
            }
        }

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
        /// Checks for user input this frame.
        /// </summary>
        /// <returns>Whether any new commands have been picked up or not.</returns>
        public bool Update()
        {
            this.selectionPosition = Vector2.Zero;
            this.commands.Clear();
            switch (this.controlDevice)
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

        #region input_methods

        /// <summary>
        /// Adds commands to the command list based on game pad input. 
        /// </summary>
        private void GetGamePadInput()
        {
            this.gamePadstate = GamePad.GetState(PlayerIndex.One);
            if (this.gamePadstate != null && DateTime.UtcNow.Subtract(this.lastKeyPressTime).TotalMilliseconds > InputManager.KeyDelayMillisceonds)
            {
                this.lastKeyPressTime = DateTime.UtcNow;
                if (this.gamePadstate.Buttons.Back == ButtonState.Released)
                {
                    this.commands.Add(InputCommand.Exit);
                }
                else if (this.gamePadstate.Buttons.Start == ButtonState.Released)
                {
                    this.commands.Add(InputCommand.Pause);
                }
                else
                {
                    if (this.gamePadstate.Buttons.A == ButtonState.Released)
                    {
                        this.commands.Add(InputCommand.Select);
                    }

                    if (this.gamePadstate.ThumbSticks.Left.Y > 0.1f || this.gamePadstate.DPad.Up == ButtonState.Released)
                    {
                        this.commands.Add(InputCommand.Up);
                        this.commands.Add(InputCommand.Jump);
                    }
                    else if (this.gamePadstate.ThumbSticks.Left.Y < -0.1f || this.gamePadstate.DPad.Down == ButtonState.Released)
                    {
                        this.commands.Add(InputCommand.Down);
                        this.commands.Add(InputCommand.Crouch);
                    }
                    else if (this.gamePadstate.IsButtonDown(Buttons.RightTrigger))
                    {
                        this.commands.Add(InputCommand.Run);
                    }

                    if (this.gamePadstate.ThumbSticks.Left.X > 0.1f || this.gamePadstate.DPad.Right == ButtonState.Released)
                    {
                        this.commands.Add(InputCommand.Right);
                    }
                    else if (this.gamePadstate.ThumbSticks.Left.X < -0.1f || this.gamePadstate.DPad.Left == ButtonState.Released)
                    {
                        this.commands.Add(InputCommand.Left);
                    }
                }
            }
        }

        /// <summary>
        /// Adds commands to the command list based on Kinect input. 
        /// </summary>
        private void GetKinectInput()
        {
            if (this.ReadSkeletonFrame())
            {
                bool skeletonLogged = false;
                foreach (Skeleton skeleton in this.skeletonData)
                {
                    switch (skeleton.TrackingState)
                    {
                        case SkeletonTrackingState.NotTracked:
                            break;
                        case SkeletonTrackingState.PositionOnly:
                            break;
                        case SkeletonTrackingState.Tracked:
                            this.gestureManager.Update(skeleton);
                            skeletonLogged = true;
                            break;
                        default:
                            break;
                    }

                    if (skeletonLogged)
                    {
                        break;
                    }
                }
            }

            Gestures.GestureType gestureToApply;
            do
            {
                gestureToApply = this.gestureManager.GetNextDetectedGesture();
                switch (gestureToApply)
                {
                    case Gestures.GestureType.SwipeLeft:
                        this.commands.Add(InputCommand.Left);
                        break;
                    case Gestures.GestureType.SwipeRight:
                        this.commands.Add(InputCommand.Run);
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
                    case Gestures.GestureType.Push:
                        this.commands.Add(InputCommand.SelectAt);
                        break;
                    default:
                        break;
                }
            }
            while (gestureToApply != Gestures.GestureType.None);
        }

        /// <summary>
        /// Adds commands to the command list based on keyboard input. 
        /// </summary>
        private void GetKeyboardInput()
        {
            this.keyboardState = Keyboard.GetState();
            if (this.keyboardState != null && DateTime.UtcNow.Subtract(this.lastKeyPressTime).TotalMilliseconds > InputManager.KeyDelayMillisceonds)
            {
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
        /// Initializes all Kinect based control systems.
        /// Defaults to keyboard input if the Kinect is not available.
        /// </summary>
        private void InitializeKinect()
        {
            if (!this.TryStartKinect())
            {
                this.kinectSensor = null;
                this.controlDevice = ControlDevice.Keyboard;
            }
            else
            {
                this.coordinateMapper = new CoordinateMapper(this.kinectSensor);
                this.gestureManager = new GestureManager();
                if (!this.TryStartSpeechEngine())
                {
                    this.speechEngine = null;
                }
            }
        }

        /// <summary>
        /// Tries to start the speech engine.
        /// </summary>
        /// <returns>Whether the speech engine was successfully started or not.</returns>
        private bool TryStartSpeechEngine()
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
                grammarChoices.Add(new SemanticResultValue(SelectableNames.PlayButtonName.ToLower(), SelectableNames.PlayButtonName));
                grammarChoices.Add(new SemanticResultValue(SelectableNames.BackButtonName.ToLower(), SelectableNames.BackButtonName));
                grammarChoices.Add(new SemanticResultValue(SelectableNames.LeaderboardButtonName.ToLower(), SelectableNames.LeaderboardButtonName));
                grammarChoices.Add(new SemanticResultValue(SelectableNames.OptionsButtonName.ToLower(), SelectableNames.OptionsButtonName));
                grammarChoices.Add(new SemanticResultValue(SelectableNames.ExitButtonName.ToLower(), SelectableNames.ExitButtonName));
                grammarChoices.Add(new SemanticResultValue(SelectableNames.PauseCommandName.ToLower(), SelectableNames.PauseCommandName));
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Culture = recognizerInfo.Culture;
                grammarBuilder.Append(grammarChoices);
                Grammar grammar = new Grammar(grammarBuilder);

                //// TODO: Possibly create a grammar from grammar definition XML file.
                // using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.SpeechGrammar)))
                // {
                //     var g = new Grammar(memoryStream);
                //     speechEngine.LoadGrammar(g);
                // }
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
                    this.kinectSensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                    this.kinectSensor.SkeletonStream.Enable();
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

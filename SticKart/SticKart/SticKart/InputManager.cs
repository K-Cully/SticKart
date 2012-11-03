using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Kinect.Toolbox;
using SticKart.Gestures;
using SticKart.Menu;

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
        public enum Command { None, Up, Down, Left, Right, Jump, Crouch, Run, Select, SelectAt, Pause, Exit }

        /// <summary>
        /// An enumeration of control devices which can be used with this input manager.
        /// </summary>
        public enum ControlDevice { Kinect, Keyboard, GamePad, Touch }

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
        /// Speech utterance confidence threshold, below which speech is treated as if it hadn't been heard.
        /// </summary> 
        private const double SpeechConfidenceThreshold = 0.3;

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
        /// Gets whether there is a word available or not.
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
                    float centerOffsetX = position.X - (screenDimensions.X / 2.0f);
                    float xScaling = Math.Min(Math.Max(1.0f, (centerOffsetX * 0.00390625f) * (centerOffsetX * 0.00390625f)), 1.6f);
                    position.X = (screenDimensions.X / 2.0f) + centerOffsetX * xScaling;
                    position.Y *= this.screenDimensions.Y * 2.0f;
                    return position;
                }
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
            this.gestureManager = null;
            this.selectedWord = null;
            this.touchGesture = new GestureSample();
            this.keyboardState = new KeyboardState();
            this.gamePadstate = new GamePadState();

            if (this.controlDevice == ControlDevice.Kinect)
            {
                this.InitalizeKinect();
            }
        }        
                
        #endregion

        #region input_methods

        /// <summary>
        /// Adds commands to the command list based on game pad input. 
        /// </summary>
        private void GetGamePadInput()
        {
            this.gamePadstate = GamePad.GetState(PlayerIndex.One);
            if (this.gamePadstate != null)
            {
                if (this.gamePadstate.Buttons.Back == ButtonState.Released)
                {
                    commands.Add(Command.Exit);
                }
                else if (this.gamePadstate.Buttons.Start == ButtonState.Released)
                {
                    commands.Add(Command.Pause);
                }
                else
                {
                    if (this.gamePadstate.Buttons.A == ButtonState.Released)
                    {
                        commands.Add(Command.Select);
                    }

                    if (this.gamePadstate.ThumbSticks.Left.Y > 0.1f || this.gamePadstate.DPad.Up == ButtonState.Released)
                    {
                        commands.Add(Command.Up);
                        commands.Add(Command.Jump);
                    }
                    else if (this.gamePadstate.ThumbSticks.Left.Y < -0.1f || this.gamePadstate.DPad.Down == ButtonState.Released)
                    {
                        commands.Add(Command.Down);
                        commands.Add(Command.Crouch);
                    }
                    else if (this.gamePadstate.IsButtonDown(Buttons.RightTrigger))
                    {
                        commands.Add(Command.Run);
                    }

                    if (this.gamePadstate.ThumbSticks.Left.X > 0.1f || this.gamePadstate.DPad.Right == ButtonState.Released)
                    {
                        commands.Add(Command.Right);
                    }
                    else if (this.gamePadstate.ThumbSticks.Left.X < -0.1f || this.gamePadstate.DPad.Left == ButtonState.Released)
                    {
                        commands.Add(Command.Left);
                    }
                }
            }
        }

        /// <summary>
        /// Adds commands to the command list based on Kinect input. 
        /// </summary>
        private void GetKinectInput()
        {
            if (this.ReadSkelletonFrame())
            {
                bool skeletonLogged = false;
                foreach (Skeleton skeleton in skeletonData)
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
                switch (gestureToApply) // TODO: Change this logic.
                {
                    case Gestures.GestureType.SwipeLeft:
                        this.commands.Add(Command.Left);
                        break;
                    case Gestures.GestureType.SwipeRight:
                        this.commands.Add(Command.Run);
                        break;
                    case Gestures.GestureType.Crouch:
                        this.commands.Add(Command.Crouch);
                        break;
                    case Gestures.GestureType.Run:
                        this.commands.Add(Command.Run);
                        break;
                    case Gestures.GestureType.Jump:
                        this.commands.Add(Command.Jump);
                        break;
                    case Gestures.GestureType.Push:
                        this.commands.Add(Command.SelectAt);
                        break;
                    default:
                        break;
                }

            } while (gestureToApply != Gestures.GestureType.None);
        }
        
        /// <summary>
        /// Adds commands to the command list based on keyboard input. 
        /// </summary>
        private void GetKeyboardInput()
        {
            keyboardState = Keyboard.GetState();
            if (keyboardState != null)
            {
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
                        this.commands.Add(Command.Run);
                        break;
                    case Microsoft.Xna.Framework.Input.Touch.GestureType.Flick:
                        Vector2 velocity = this.touchGesture.Delta;
                        if (velocity.X * velocity.X > velocity.Y * velocity.Y) // Movement greater along x than y.
                        {
                            if (velocity.X > 0.0f)
                            {
                                this.commands.Add(Command.Right);
                            }
                            else
                            {
                                this.commands.Add(Command.Left);
                            }
                        }
                        else
                        {
                            if (velocity.Y > 0.0f)
                            {
                                this.commands.Add(Command.Down);
                                this.commands.Add(Command.Crouch);
                            }
                            else
                            {
                                this.commands.Add(Command.Up);
                                this.commands.Add(Command.Jump);
                            }
                        }
                        break;
                    case Microsoft.Xna.Framework.Input.Touch.GestureType.Tap:
                        this.selectionPosition = this.touchGesture.Position; // TODO: scale to viewport resolution.
                        this.commands.Add(Command.SelectAt);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region kinect_methods

        /// <summary>
        /// Initalizes all Kinect based control systems.
        /// Defaults to keyboard input if the Kinect is not available.
        /// </summary>
        private void InitalizeKinect()
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
            RecognizerInfo recognizerInfo = GetKinectRecognizer();

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
                //using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.SpeechGrammar)))
                //{
                //    var g = new Grammar(memoryStream);
                //    speechEngine.LoadGrammar(g);
                //}
                this.speechEngine.LoadGrammar(grammar);
                this.speechEngine.SpeechRecognized += SpeechRecognized;
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
            bool successful = true;
            if (KinectSensor.KinectSensors.Count > 0) // At least one sensor is connected.
            {                
                this.kinectSensor = KinectSensor.KinectSensors[0];
                if (this.kinectSensor.Status == KinectStatus.Connected)
                {
                    this.kinectSensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30); // Need this enabled for coordinate mapping.
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
                else // Xbox Kinect not supported
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
                this.speechEngine.SpeechRecognized -= SpeechRecognized;
                this.speechEngine.RecognizeAsyncStop();
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
        
        /// <summary>
        /// Gets the metadata for the speech recognizer (acoustic model) most suitable to process audio from Kinect device.
        /// </summary>
        /// <returns>RecognizerInfo if found, null otherwise.</returns>
        private static RecognizerInfo GetKinectRecognizer()
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

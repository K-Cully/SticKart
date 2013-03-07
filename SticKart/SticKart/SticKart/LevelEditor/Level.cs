// -----------------------------------------------------------------------
// <copyright file="Level.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.LevelEditor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Display;
    using Game.Entities;
    using Game.Level;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a level which the level editor can modify.
    /// </summary>
    public class Level
    {
        #region graphics

        /// <summary>
        /// The sprite batch to render with.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// A cart sprite.
        /// </summary>
        private Sprite cartSprite;

        /// <summary>
        /// A switch sprite.
        /// </summary>
        private Sprite switchSprite;

        /// <summary>
        /// A platform sprite.
        /// </summary>
        private Sprite platformSprite;

        /// <summary>
        /// An edge sprite.
        /// </summary>
        private Sprite edgeSprite;

        /// <summary>
        /// The start position sprite.
        /// </summary>
        private Sprite startSprite;

        /// <summary>
        /// The exit sprite.
        /// </summary>
        private Sprite exitSprite;

        /// <summary>
        /// The jump power up sprite.
        /// </summary>
        private Sprite jumpSprite;

        /// <summary>
        /// The speed power up sprite.
        /// </summary>
        private Sprite speedSprite;

        /// <summary>
        /// The invincible power up sprite.
        /// </summary>
        private Sprite invincibleSprite;

        /// <summary>
        /// The health power up sprite.
        /// </summary>
        private Sprite healthSprite;

        /// <summary>
        /// The rock sprite.
        /// </summary>
        private Sprite rockSprite;

        /// <summary>
        /// The fire sprite.
        /// </summary>
        private Sprite fireSprite;

        /// <summary>
        /// The spike sprite.
        /// </summary>
        private Sprite spikeSprite;

        /// <summary>
        /// The coin sprite.
        /// </summary>
        private Sprite coinSprite;

        /// <summary>
        /// The ruby sprite.
        /// </summary>
        private Sprite rubySprite;

        /// <summary>
        /// The diamond sprite.
        /// </summary>
        private Sprite diamondSprite;

        /// <summary>
        /// A texture to apply to rocky terrain.
        /// </summary>
        private Texture2D rockyTerrain;

        /// <summary>
        /// The background to display in a level.
        /// </summary>
        private Background background;

        #endregion

        #region private_entities

        /// <summary>
        /// The size of the game display area.
        /// </summary>
        private Vector2 screenDimensions;

        /// <summary>
        /// A list of platform descriptions.
        /// </summary>
        private List<PlatformDescription> platformDescriptions;

        /// <summary>
        /// A list of interactive entity descriptions.
        /// </summary>
        private List<InteractiveEntityDescription> interactiveEntityDescriptions;

        /// <summary>
        /// A list of points which define the ends of the floor edges.
        /// </summary>
        private List<Vector2> floorEdgePoints;

        /// <summary>
        /// The index of the cart entity.
        /// </summary>
        private int cartIndex;

        /// <summary>
        /// The index of the switch entity.
        /// </summary>
        private int switchIndex;

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/> class.
        /// </summary>
        /// <param name="screenDimensions">The dimensions of the game area.</param>
        public Level(Vector2 screenDimensions)
        {
            this.StartPosition = Vector2.Zero;
            this.ExitPosition = Vector2.Zero;
            this.platformDescriptions = new List<PlatformDescription>();
            this.interactiveEntityDescriptions = new List<InteractiveEntityDescription>();
            this.floorEdgePoints = new List<Vector2>();
            this.platformSprite = new Sprite();
            this.switchSprite = new Sprite();
            this.cartSprite = new Sprite();
            this.edgeSprite = new Sprite();
            this.startSprite = new Sprite();
            this.exitSprite = new Sprite();
            this.invincibleSprite = new Sprite();
            this.jumpSprite = new Sprite();
            this.speedSprite = new Sprite();
            this.healthSprite = new Sprite();
            this.fireSprite = new Sprite();
            this.spikeSprite = new Sprite();
            this.rockSprite = new Sprite();
            this.coinSprite = new Sprite();
            this.diamondSprite = new Sprite();
            this.rubySprite = new Sprite();
            this.background = new Background(screenDimensions, 0.8f);
            this.screenDimensions = screenDimensions;
            this.cartIndex = -1;
            this.switchIndex = -1;
        }

        #endregion

        #region public_accessors

        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        public Vector2 StartPosition { get; set; }

        /// <summary>
        /// Gets or sets the exit position.
        /// </summary>
        public Vector2 ExitPosition { get; set; }

        /// <summary>
        /// Gets the last floor edge point.
        /// </summary>
        public Vector2 LastFloorPoint
        {
            get
            {
                if (this.floorEdgePoints.Count == 0)
                {
                    return Vector2.Zero;
                }
                else
                {
                    return this.floorEdgePoints[this.floorEdgePoints.Count - 1];
                }
            }
        }

        #endregion

        #region content_loading

        /// <summary>
        /// Loads any resources used by a level.
        /// </summary>
        /// <param name="spriteBatch">the game's sprite batch.</param>
        /// <param name="contentManager">The game's content manager.</param>
        public void LoadContent(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.spriteBatch = spriteBatch;
            this.cartSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.CartFull);
            this.switchSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.SwitchStatic);
            this.platformSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Platform);
            this.edgeSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Floor);
            this.startSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.StickManSubPath + EntityConstants.StickManStanding);
            this.exitSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.Exit);
            this.invincibleSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.InvincibleName);
            this.jumpSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.JumpName);
            this.speedSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.SpeedName);
            this.healthSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.PowerUpFolderSubPath + EntityConstants.HealthName);
            this.fireSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.FireName);            
            this.spikeSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.SpikeName);
            this.rockSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.ObstacleFolderSubPath + EntityConstants.RockName);
            this.coinSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.CoinName);
            this.diamondSprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.DiamondName);
            this.rubySprite.InitializeAndLoad(spriteBatch, contentManager, EntityConstants.SpritesFolderPath + EntityConstants.BonusFolderSubPath + EntityConstants.RubyName);
            this.rockyTerrain = contentManager.Load<Texture2D>(ContentLocations.Scenery + ContentLocations.RockyTerrain);
            this.background.InitializeAndLoad(spriteBatch, contentManager, ContentLocations.RockyBackGround);
        }

        #endregion

        #region insertion

        /// <summary>
        /// Adds a platform to the level.
        /// </summary>
        /// <param name="length">The length of the platform.</param>
        /// <param name="position">The position of the platform.</param>
        public void AddPlatform(float length, Vector2 position)
        {
            PlatformDescription platformDescription = new PlatformDescription();
            platformDescription.Length = length;
            platformDescription.Position = position;
            this.platformDescriptions.Add(platformDescription);
        }
        
        /// <summary>
        /// Checks if a position is free.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>A value indacating whether the space is occupied or not.</returns>
        public bool IsPositionFree(Vector2 position)
        {
            float radiusSquared = 1024.0f;
            foreach (InteractiveEntityDescription entity in this.interactiveEntityDescriptions)
            {
                if ((entity.Position - position).LengthSquared() < radiusSquared)
                {
                    return false;
                }
            }

            foreach (PlatformDescription platform in this.platformDescriptions)
            {
                if ((platform.Position - position).LengthSquared() < radiusSquared)
                {
                    return false;
                }
            }

            foreach (Vector2 point in this.floorEdgePoints)
            {
                if (position.X > point.X)
                {
                    // TODO: finished here
                    if (position.X - point.X < 33.0f)
                    {
                        if (position.X - point.Y < radiusSquared)
                        {
                            return false;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Adds an interactive entity to the level.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <param name="position">The position of the entity.</param>
        /// <param name="size">The size of the entity in display coordinates.</param>
        public void AddInteractiveEntity(string name, Vector2 position, Vector2 size)
        {
            InteractiveEntityDescription entityDescription = new InteractiveEntityDescription();
            entityDescription.Position = position;
            entityDescription.Name = name;
            entityDescription.Dimensions = size;
            this.interactiveEntityDescriptions.Add(entityDescription);
        }

        /// <summary>
        /// Adds a cart to the level.
        /// </summary>
        /// <param name="position">The position of the entity.</param>
        /// <param name="size">The size of the entity in display coordinates.</param>
        public void AddCart(Vector2 position, Vector2 size)
        {
            if (this.cartIndex == -1)
            {
                this.cartIndex = this.interactiveEntityDescriptions.Count;
                InteractiveEntityDescription entityDescription = new InteractiveEntityDescription();
                entityDescription.Position = position;
                entityDescription.Name = EntityConstants.CartBody;
                entityDescription.Dimensions = size;
                this.interactiveEntityDescriptions.Add(entityDescription);
            }
            else
            {
                this.interactiveEntityDescriptions[this.cartIndex].Position = position;
                this.interactiveEntityDescriptions[this.cartIndex].Dimensions = size;
            }
        }

        /// <summary>
        /// Adds a switch to the level.
        /// </summary>
        /// <param name="position">The position of the entity.</param>
        /// <param name="size">The size of the entity in display coordinates.</param>
        public void AddSwitch(Vector2 position, Vector2 size)
        {
            if (this.switchIndex == -1)
            {
                this.switchIndex = this.interactiveEntityDescriptions.Count;
                InteractiveEntityDescription entityDescription = new InteractiveEntityDescription();
                entityDescription.Position = position;
                entityDescription.Name = EntityConstants.Switch;
                entityDescription.Dimensions = size;
                this.interactiveEntityDescriptions.Add(entityDescription);
            }
            else
            {
                this.interactiveEntityDescriptions[this.switchIndex].Position = position;
                this.interactiveEntityDescriptions[this.switchIndex].Dimensions = size;
            }
        }

        /// <summary>
        /// Adds a point to the floor edges.
        /// </summary>
        /// <param name="point">The point to add.</param>
        public void AddFloorPoint(Vector2 point)
        {
            this.floorEdgePoints.Add(point);
        }

        #endregion

        #region removal

        /// <summary>
        /// Clears the level.
        /// </summary>
        public void Clear()
        {
            this.StartPosition = Vector2.Zero;
            this.ExitPosition = Vector2.Zero;
            this.platformDescriptions.Clear();
            this.interactiveEntityDescriptions.Clear();
            this.floorEdgePoints.Clear();
            this.background.Reset();
            this.cartIndex = -1;
            this.switchIndex = -1;
        }

        /// <summary>
        /// Removes the last platform added.
        /// </summary>
        public void RemoveLastPlatform()
        {
            if (this.platformDescriptions.Count > 0)
            {
                this.platformDescriptions.RemoveAt(this.platformDescriptions.Count - 1);
            }
        }
        
        /// <summary>
        /// Removes the last interactive entity added.
        /// </summary>
        public void RemoveLastInteractiveEntity()
        {
            if (this.interactiveEntityDescriptions.Count > 0)
            {
                this.interactiveEntityDescriptions.RemoveAt(this.interactiveEntityDescriptions.Count - 1);
            }
        }

        /// <summary>
        /// Removes the cart and switch from the level.
        /// </summary>
        public void RemoveCartAndSwitch()
        {
            if (this.cartIndex > this.switchIndex)
            {
                this.RemoveCart();
                this.RemoveSwitch();
            }
            else
            {
                this.RemoveSwitch();
                this.RemoveCart();
            }
        }

        /// <summary>
        /// Removes the last floor point added.
        /// </summary>
        /// <returns>The last floor point.</returns>
        public Vector2 RemoveLastFloorPoint()
        {
            if (this.floorEdgePoints.Count > 0)
            {
                this.floorEdgePoints.RemoveAt(this.floorEdgePoints.Count - 1);
            }

            if (this.floorEdgePoints.Count > 0)
            {
                return this.floorEdgePoints[this.floorEdgePoints.Count - 1];
            }
            else
            {
                return Vector2.Zero;
            }
        }

        /// <summary>
        /// Retrieves the last floor edge's angle.
        /// </summary>
        /// <returns>The angle in radians.</returns>
        public float GetLastFloorEdgeAngle()
        {
            if (this.floorEdgePoints.Count > 2)
            {
                Vector2 lastEdgeDirection = this.floorEdgePoints[this.floorEdgePoints.Count - 1] - this.floorEdgePoints[this.floorEdgePoints.Count - 2];
                lastEdgeDirection.Normalize();
                return (float)Math.Asin(lastEdgeDirection.Y);
            }
            else
            {
                return 0.0f;
            }
        }

        #endregion

        #region save_and_load

        /// <summary>
        /// Saves the level to isolated storage.
        /// </summary>
        /// <param name="levelNumber">The level number.</param>
        /// <param name="contentManagerFormat">Whether to format the data for the content manager or the xml serializer.</param>
        public void Save(int levelNumber, bool contentManagerFormat)
        {
            if (levelNumber > 0)
            {
#if WINDOWS_PHONE
                using (IsolatedStorageFile levelFile = IsolatedStorageFile.GetUserStoreForApplication())
#else
                using (IsolatedStorageFile levelFile = IsolatedStorageFile.GetUserStoreForDomain())
#endif
                {
                    string directoryName = contentManagerFormat ? "ContentManager_" + levelNumber.ToString() : levelNumber.ToString();
                    if (levelFile.DirectoryExists(directoryName))
                    {
                        foreach (string name in levelFile.GetFileNames(directoryName + "/*"))
                        {
                            levelFile.DeleteFile(directoryName + "/" + name);
                        }

                        levelFile.DeleteDirectory(directoryName);
                    }

                    levelFile.CreateDirectory(directoryName);
                    this.SerializeLevelPoints(directoryName, levelFile, contentManagerFormat);
                    this.SerializePlatforms(directoryName, levelFile, contentManagerFormat);
                    this.SerializeInteractiveEntities(directoryName, levelFile, contentManagerFormat);
                }
            }
            else
            {
                throw new Exception("The level value must be greater than 0.");
            }
        }

        /// <summary>
        /// Loads the level from isolated storage.
        /// </summary>
        /// <param name="levelNumber">The level number.</param>
        public void Load(int levelNumber)
        {
            if (levelNumber > 0)
            {
#if WINDOWS_PHONE
                using (IsolatedStorageFile levelFile = IsolatedStorageFile.GetUserStoreForApplication())
#else
                using (IsolatedStorageFile levelFile = IsolatedStorageFile.GetUserStoreForDomain())
#endif
                {
                    if (levelFile.DirectoryExists(levelNumber.ToString()))
                    {
                        this.Clear();
                        this.DeserializeLevelPoints(levelNumber.ToString(), levelFile);
                        this.DeserializePlatforms(levelNumber.ToString(), levelFile);
                        this.DeserializeInteractiveEntities(levelNumber.ToString(), levelFile);
                    }
                    else
                    {
                        throw new Exception("Level " + levelNumber.ToString() + " does not exist.");
                    }
                }
            }
            else
            {
                throw new Exception("The level value must be greater than 0.");
            }
        }

        #endregion

        #region drawing

        /// <summary>
        /// Draws the level.
        /// </summary>
        public void Draw()
        {
            this.background.Update();
            this.background.Draw();
            if (this.StartPosition != Vector2.Zero)
            {
                Camera2D.Draw(this.startSprite, this.StartPosition, 0.0f);
            }

            if (this.ExitPosition != Vector2.Zero)
            {
                Camera2D.Draw(this.exitSprite, this.ExitPosition, 0.0f);
            }

            SceneryRenderer.DrawTerrain(this.spriteBatch, this.rockyTerrain, this.floorEdgePoints, this.screenDimensions.Y * 1.5f);
            foreach (PlatformDescription platformDescription in this.platformDescriptions)
            {
                this.DrawPlatform(platformDescription);
            }

            foreach (InteractiveEntityDescription entityDescription in this.interactiveEntityDescriptions)
            {
                this.DrawInteractiveEntity(entityDescription);
            }
        }

        /// <summary>
        /// Draws the floor to the screen.
        /// </summary>
        private void DrawFloor()
        {
            Vector2 startPoint = Vector2.Zero;
            foreach (Vector2 point in this.floorEdgePoints)
            {
                if (startPoint != Vector2.Zero)
                {
                    Vector2 direction = point - startPoint;
                    direction.Normalize();
                    Camera2D.Draw(this.edgeSprite, (startPoint + point) / 2.0f, (float)Math.Asin(direction.Y));
                }

                startPoint = point;
            }                
        }

        /// <summary>
        /// Draws an interactive entity.
        /// </summary>
        /// <param name="entityDescription">The entity's description.</param>
        private void DrawInteractiveEntity(InteractiveEntityDescription entityDescription)
        {
            switch (entityDescription.Name)
            {
                case EntityConstants.FireName:
                    Camera2D.Draw(this.fireSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.SpikeName:
                    Camera2D.Draw(this.spikeSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.RockName:
                    Camera2D.Draw(this.rockSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.InvincibleName:
                    Camera2D.Draw(this.invincibleSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.HealthName:
                    Camera2D.Draw(this.healthSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.SpeedName:
                    Camera2D.Draw(this.speedSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.JumpName:
                    Camera2D.Draw(this.jumpSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.CoinName:
                    Camera2D.Draw(this.coinSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.DiamondName:
                    Camera2D.Draw(this.diamondSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.RubyName:
                    Camera2D.Draw(this.rubySprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.Switch:
                    Camera2D.Draw(this.switchSprite, entityDescription.Position, 0.0f);
                    break;
                case EntityConstants.CartBody:
                    Camera2D.Draw(this.cartSprite, entityDescription.Position, 0.0f);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Draws a platform to the screen.
        /// </summary>
        /// <param name="platformDescription">The platform description to use draw.</param>
        private void DrawPlatform(PlatformDescription platformDescription)
        {
            Camera2D.Draw(this.platformSprite, platformDescription.Position, 0.0f);
            if (platformDescription.Length > this.platformSprite.Width)
            {
                int count = 0;

                // Useable area on middle sprites (removing rounded ends)
                float offset = this.platformSprite.Width - this.platformSprite.Height;
                float halfLeftOver = (platformDescription.Length - offset) * 0.5f;

                // Leftover greater than useable area on end sprites
                while (halfLeftOver > this.platformSprite.Width - (this.platformSprite.Height / 2.0f))
                {
                    count++;
                    Camera2D.Draw(this.platformSprite, new Vector2(platformDescription.Position.X + (offset * count), platformDescription.Position.Y), 0.0f);
                    Camera2D.Draw(this.platformSprite, new Vector2(platformDescription.Position.X - (offset * count), platformDescription.Position.Y), 0.0f);
                    halfLeftOver -= offset;
                }

                // Fill in ends
                if (halfLeftOver > 0.0f)
                {
                    Camera2D.Draw(this.platformSprite, platformDescription.Position + new Vector2((platformDescription.Length / 2.0f) - (this.platformSprite.Width / 2.0f), 0.0f), 0.0f);
                    Camera2D.Draw(this.platformSprite, platformDescription.Position + new Vector2(-(platformDescription.Length / 2.0f) + (this.platformSprite.Width / 2.0f), 0.0f), 0.0f);
                }
            }
        }

        #endregion

        #region serialization

        /// <summary>
        /// Serializes the start position, exit position and floor points to a file.
        /// </summary>
        /// <param name="directory">The level directory name.</param>
        /// <param name="levelFile">The isolated storage file.</param>
        /// <param name="contentManagerFormat">Whether to format the data for the content manager or the xml serializer.</param>
        private void SerializeLevelPoints(string directory, IsolatedStorageFile levelFile, bool contentManagerFormat)
        {
            using (IsolatedStorageFileStream levelStream = new IsolatedStorageFileStream(directory + "/" + LevelConstants.PointSubPath + ".xml", FileMode.OpenOrCreate, levelFile))
            {
                List<Vector2> levelPoints = new List<Vector2>();
                levelPoints.Add(this.StartPosition);
                levelPoints.Add(this.ExitPosition);
                levelPoints.AddRange(this.floorEdgePoints);
                if (contentManagerFormat)
                {
                    string vectorArrayAsString = "\n";
                    foreach (Vector2 vector in levelPoints)
                    {
                        vectorArrayAsString += vector.X + " " + vector.Y + "\n";
                    }

                    XAttribute typeAttribute = new XAttribute(LevelSerializationConstants.TypeAttributeName, LevelSerializationConstants.VectorArrayTypeField);
                    XElement assetElement = new XElement(LevelSerializationConstants.AssetTag, typeAttribute);
                    assetElement.SetValue(vectorArrayAsString);
                    XDocument xmlSerializer = new XDocument(new XElement(LevelSerializationConstants.XnaContentTag, assetElement));
                    xmlSerializer.Save(levelStream);
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Vector2>));
                    serializer.Serialize(levelStream, levelPoints);
                }
            }
        }

        /// <summary>
        /// Serializes the platforms to a file.
        /// </summary>
        /// <param name="directory">The level directory name.</param>
        /// <param name="levelFile">The isolated storage file.</param>
        /// <param name="contentManagerFormat">Whether to format the data for the content manager or the xml serializer.</param>
        private void SerializePlatforms(string directory, IsolatedStorageFile levelFile, bool contentManagerFormat)
        {
            using (IsolatedStorageFileStream levelStream = new IsolatedStorageFileStream(directory + "/" + LevelConstants.PlatformSubPath + ".xml", FileMode.OpenOrCreate, levelFile))
            {
                if (contentManagerFormat)
                {
                    XAttribute typeAttribute = new XAttribute(LevelSerializationConstants.TypeAttributeName, LevelSerializationConstants.PlatformArrayTypeField);
                    XElement assetElement = new XElement(LevelSerializationConstants.AssetTag, typeAttribute);
                    XElement itemElement;
                    foreach (PlatformDescription platform in this.platformDescriptions)
                    {
                        itemElement = new XElement(LevelSerializationConstants.ItemTag);
                        string positionAsString = platform.Position.X + " " + platform.Position.Y;
                        itemElement.Add(new XElement(LevelSerializationConstants.PositionTag, positionAsString));
                        itemElement.Add(new XElement(LevelSerializationConstants.LengthTag, platform.Length.ToString()));
                        assetElement.Add(itemElement);
                    }

                    XDocument xmlSerializer = new XDocument(new XElement(LevelSerializationConstants.XnaContentTag, assetElement));
                    xmlSerializer.Save(levelStream);
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<PlatformDescription>));
                    serializer.Serialize(levelStream, this.platformDescriptions);
                }
            }
        }

        /// <summary>
        /// Serializes the interactive entities to a file.
        /// </summary>
        /// <param name="directory">The level directory name.</param>
        /// <param name="levelFile">The isolated storage file.</param>
        /// <param name="contentManagerFormat">Whether to format the data for the content manager or the xml serializer.</param>
        private void SerializeInteractiveEntities(string directory, IsolatedStorageFile levelFile, bool contentManagerFormat)
        {
            using (IsolatedStorageFileStream levelStream = new IsolatedStorageFileStream(directory + "/" + LevelConstants.InteractiveSubPath + ".xml", FileMode.OpenOrCreate, levelFile))
            {
                if (contentManagerFormat)
                {
                    XAttribute typeAttribute = new XAttribute(LevelSerializationConstants.TypeAttributeName, LevelSerializationConstants.EntityArrayTypeField);
                    XElement assetElement = new XElement(LevelSerializationConstants.AssetTag, typeAttribute);
                    XElement itemElement;
                    string vectorAsString = string.Empty;
                    foreach (InteractiveEntityDescription entity in this.interactiveEntityDescriptions)
                    {
                        itemElement = new XElement(LevelSerializationConstants.ItemTag);
                        itemElement.Add(new XElement(LevelSerializationConstants.NameTag, entity.Name));
                        vectorAsString = entity.Position.X + " " + entity.Position.Y;
                        itemElement.Add(new XElement(LevelSerializationConstants.PositionTag, vectorAsString));
                        vectorAsString = entity.Dimensions.X + " " + entity.Dimensions.Y;
                        itemElement.Add(new XElement(LevelSerializationConstants.DimensionsTag, vectorAsString));
                        assetElement.Add(itemElement);
                    }

                    XDocument xmlSerializer = new XDocument(new XElement(LevelSerializationConstants.XnaContentTag, assetElement));
                    xmlSerializer.Save(levelStream);
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<InteractiveEntityDescription>));
                    serializer.Serialize(levelStream, this.interactiveEntityDescriptions);
                }
            }
        }

        /// <summary>
        /// Deserializes the start position, exit position and floor points from a file.
        /// </summary>
        /// <param name="directory">The level directory name.</param>
        /// <param name="levelFile">The isolated storage file.</param>
        private void DeserializeLevelPoints(string directory, IsolatedStorageFile levelFile)
        {
            using (IsolatedStorageFileStream levelStream = new IsolatedStorageFileStream(directory + "/" + LevelConstants.PointSubPath + ".xml", FileMode.OpenOrCreate, levelFile))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<Vector2>));
                List<Vector2> levelPoints = new List<Vector2>();
                levelPoints = (List<Vector2>)deserializer.Deserialize(levelStream);
                this.StartPosition = levelPoints[0];
                levelPoints.RemoveAt(0);
                this.ExitPosition = levelPoints[0];
                levelPoints.RemoveAt(0);
                this.floorEdgePoints = new List<Vector2>(levelPoints);
            }
        }

        /// <summary>
        /// Deserializes the platforms from a file.
        /// </summary>
        /// <param name="directory">The level directory name.</param>
        /// <param name="levelFile">The isolated storage file.</param>
        private void DeserializePlatforms(string directory, IsolatedStorageFile levelFile)
        {
            using (IsolatedStorageFileStream levelStream = new IsolatedStorageFileStream(directory + "/" + LevelConstants.PlatformSubPath + ".xml", FileMode.OpenOrCreate, levelFile))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<PlatformDescription>));
                this.platformDescriptions = (List<PlatformDescription>)deserializer.Deserialize(levelStream);
            }
        }

        /// <summary>
        /// Deserializes the interactive entities from a file.
        /// </summary>
        /// <param name="directory">The level directory name.</param>
        /// <param name="levelFile">The isolated storage file.</param>
        private void DeserializeInteractiveEntities(string directory, IsolatedStorageFile levelFile)
        {
            using (IsolatedStorageFileStream levelStream = new IsolatedStorageFileStream(directory + "/" + LevelConstants.InteractiveSubPath + ".xml", FileMode.OpenOrCreate, levelFile))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<InteractiveEntityDescription>));
                this.interactiveEntityDescriptions = (List<InteractiveEntityDescription>)deserializer.Deserialize(levelStream);
            }

            for (int count = 0; count < this.interactiveEntityDescriptions.Count; count++)
            {
                if (this.interactiveEntityDescriptions[count].Name == EntityConstants.CartBody)
                {
                    this.cartIndex = count;
                }
                else if (this.interactiveEntityDescriptions[count].Name == EntityConstants.Switch)
                {
                    this.switchIndex = count;
                }
            }
        }

        #endregion
        
        /// <summary>
        /// Removes the cart from the level.
        /// </summary>
        private void RemoveCart()
        {
            if (this.cartIndex != -1)
            {
                this.interactiveEntityDescriptions.RemoveAt(this.cartIndex);
                this.cartIndex = -1;
            }
        }

        /// <summary>
        /// Removes the switch from the level.
        /// </summary>
        private void RemoveSwitch()
        {
            if (this.switchIndex != -1)
            {
                this.interactiveEntityDescriptions.RemoveAt(this.switchIndex);
                this.switchIndex = -1;
            }
        }
    }
}

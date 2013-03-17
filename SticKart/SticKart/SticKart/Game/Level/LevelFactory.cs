// -----------------------------------------------------------------------
// <copyright file="LevelFactory.cs" company="None">
// Copyright Keith Cully 2012.
// </copyright>
// -----------------------------------------------------------------------

namespace SticKart.Game.Level
{
    using System.Collections.Generic;
    using Entities;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using FarseerPhysics.SamplesFramework;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A factory class for creating levels.
    /// </summary>
    public static class LevelFactory
    {
        /// <summary>
        /// Creates the bonuses, obstacles and power ups contained in a level.
        /// </summary>
        /// <param name="interactiveEntityDescriptions">A list of interactive entity descriptions.</param>
        /// <param name="physicsWorld">The physics world to create the entities in.</param>
        /// <param name="interactiveEntities">An empty list to store the interactive entities in.</param>
        /// <param name="mineCart">The mine cart entity.</param>
        /// <param name="cartSwitch">The switch entity.</param>
        /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
        /// <param name="contentManager">The game's content manager.</param>
        public static void CreateInteractiveEntities(List<InteractiveEntityDescription> interactiveEntityDescriptions, ref World physicsWorld, ref List<InteractiveEntity> interactiveEntities, ref MineCart mineCart, ref Switch cartSwitch, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            if (interactiveEntities.Count == 0)
            {
                foreach (InteractiveEntityDescription description in interactiveEntityDescriptions)
                {
                    if (EntityConstants.PowerUpNames.Contains(description.Name))
                    {                       
                        interactiveEntities.Add(new PowerUp(ref physicsWorld, spriteBatch, contentManager, description, EntitySettingsLoader.GetPowerUpSettings(description.Name)));
                    }
                    else if (EntityConstants.BonusNames.Contains(description.Name) || EntityConstants.ObstacleNames.Contains(description.Name))
                    {
                        interactiveEntities.Add(new BonusOrObstacle(ref physicsWorld, spriteBatch, contentManager, description, EntitySettingsLoader.GetObstacleOrBonusSetting(description.Name)));  
                    }
                    else if (description.Name == EntityConstants.CartBody)
                    {
                        mineCart = new MineCart(spriteBatch, contentManager, ref physicsWorld, description.Position, 100.0f, 240.0f, 350.0f, 80.0f, -80.0f);
                    }
                    else if (description.Name == EntityConstants.Switch)
                    {
                        cartSwitch = new Switch(spriteBatch, contentManager, ref physicsWorld, description.Position, mineCart);
                    }
                }
            }
        }

        /// <summary>
        /// Creates the platforms contained in a level.
        /// </summary>
        /// <param name="platformDescriptions">A list of platform descriptions.</param>
        /// <param name="physicsWorld">The physics world to create the platforms in.</param>
        /// <param name="platforms">An empty list of to store the platforms in.</param>
        /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
        /// <param name="contentManager">The game's content manager.</param>
        public static void CreatePlatforms(List<PlatformDescription> platformDescriptions, ref World physicsWorld, ref List<Platform> platforms, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            if (platforms.Count == 0)
            {
                foreach (PlatformDescription description in platformDescriptions)
                {
                    platforms.Add(new Platform(description, ref physicsWorld, spriteBatch, contentManager));
                }
            }
        }
        
        /// <summary>
        /// Creates the floor of a level.
        /// </summary>
        /// <param name="points">The points which define the floor.</param>
        /// <param name="physicsWorld">The physics world to create the floor in.</param>
        /// <param name="floorEdges">An empty list to store the floor edges in.</param>
        /// <param name="visualFloorEdges">An empty list to store the displayed floor edges in.</param>
        /// <param name="levelHeight">The height of the level.</param>
        public static void CreateFloor(List<Vector2> points, ref World physicsWorld, ref List<Body> floorEdges, ref List<VisualEdge> visualFloorEdges, float levelHeight)
        {
            if (floorEdges.Count == 0 && visualFloorEdges.Count == 0 && points.Count > 0)
            {
                Vector2 startPoint = points[0];
                foreach (Vector2 point in points)
                {
                    if (point != startPoint)
                    {
                        Body body = BodyFactory.CreateEdge(physicsWorld, ConvertUnits.ToSimUnits(startPoint), ConvertUnits.ToSimUnits(point));
                        body.CollisionCategories = EntityConstants.FloorCategory;
                        floorEdges.Add(body);
                        visualFloorEdges.Add(new VisualEdge(startPoint, point));
                        startPoint = point;
                    }
                }

                // Add a wall at the end of the level.
                Body lastBody = BodyFactory.CreateEdge(physicsWorld, ConvertUnits.ToSimUnits(startPoint), ConvertUnits.ToSimUnits(new Vector2(startPoint.X, -levelHeight)));
                lastBody.CollisionCategories = EntityConstants.FloorCategory;
                lastBody.UserData = new object();
                floorEdges.Add(lastBody);
            }
        }

        /// <summary>
        /// Removes the floor bodies from the world and clears the list.
        /// </summary>
        /// <param name="physicsWorld">The physics world containing the floor.</param>
        /// <param name="floorEdges">The list of floor edge bodies.</param>
        /// <param name="visualFloorEdges">The list of visual floor edges.</param>
        public static void DisposeOfFloor(ref World physicsWorld, ref List<Body> floorEdges, ref List<VisualEdge> visualFloorEdges)
        {
            visualFloorEdges.Clear();
            foreach (Body body in floorEdges)
            {
                physicsWorld.RemoveBody(body);
            }

            floorEdges.Clear();
        }

        /// <summary>
        /// Disposes of the interactive entities and clears the list.
        /// </summary>
        /// <param name="physicsWorld">The physics world containing the entities' bodies.</param>
        /// <param name="interactiveEntities">the list of interactive entities.</param>
        /// <param name="mineCart">The mine cart.</param>
        /// <param name="cartSwitch">The switch entity.</param>
        public static void DisposeOfInteractiveEntities(ref World physicsWorld, ref List<InteractiveEntity> interactiveEntities, ref MineCart mineCart, ref Switch cartSwitch)
        {
            if (mineCart != null)
            {
                mineCart.Dispose(ref physicsWorld);
                mineCart = null;
                cartSwitch.Dispose(ref physicsWorld);
                cartSwitch = null;
            }

            foreach (InteractiveEntity entity in interactiveEntities)
            {
                entity.Dispose(ref physicsWorld);
            }

            interactiveEntities.Clear();
        }

        /// <summary>
        /// Disposes of the platforms and clears the list.
        /// </summary>
        /// <param name="physicsWorld">The physics world containing the platforms' bodies.</param>
        /// <param name="platforms">The list of platforms.</param>
        public static void DisposeOfPlatforms(ref World physicsWorld, ref List<Platform> platforms)
        {
            foreach (Platform platform in platforms)
            {
                platform.Dispose(ref physicsWorld);
            }

            platforms.Clear();
        }
    }
}

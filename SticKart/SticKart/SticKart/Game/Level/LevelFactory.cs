namespace SticKart.Game.Level
{
    using System.Collections.Generic;
    using Entities;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using FarseerPhysics.SamplesFramework;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Content;

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
        /// <param name="spriteBatch">The sprite batch to use for rendering.</param>
        /// <param name="contentManager">The game's content manager.</param>
        public static void CreateInteractiveEntities(List<InteractiveEntityDescription> interactiveEntityDescriptions, ref World physicsWorld, ref List<InteractiveEntity> interactiveEntities, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            if (interactiveEntities.Count == 0)
            {
                // TODO: Implement
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
        /// <param name="levelHeight">The height of the level.</param>
        public static void CreateFloor(List<Vector2> points, ref World physicsWorld, ref List<Body> floorEdges, float levelHeight)
        {
            if (floorEdges.Count == 0 && points.Count > 0)
            {
                Vector2 startPoint = points[0];
                foreach (Vector2 point in points)
                {
                    if (point != startPoint)
                    {
                        Body body = BodyFactory.CreateEdge(physicsWorld, ConvertUnits.ToSimUnits(startPoint), ConvertUnits.ToSimUnits(point));
                        body.CollisionCategories = EntityConstants.FloorCategory;
                        floorEdges.Add(body);
                        startPoint = point;
                    }
                }

                // Add a wall at the end of the level.
                floorEdges.Add(BodyFactory.CreateEdge(physicsWorld, ConvertUnits.ToSimUnits(startPoint), ConvertUnits.ToSimUnits(new Vector2(startPoint.X, -levelHeight))));
            }
        }

        /// <summary>
        /// Removes the floor bodies from the world and clears the list.
        /// </summary>
        /// <param name="physicsWorld">The physics world containing the floor.</param>
        /// <param name="floorEdges">The list of floor edge bodies.</param>
        public static void DisposeOfFloor(ref World physicsWorld, ref List<Body> floorEdges)
        {
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
        public static void DisposeOfInteractiveEntities(ref World physicsWorld, ref List<InteractiveEntity> interactiveEntities)
        {
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

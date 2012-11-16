namespace SticKart.Game.Entities
{
    using FarseerPhysics.Dynamics;

    /// <summary>
    /// Defines the abstract base class for an interactive level entity.
    /// </summary>
    public abstract class InteractiveEntity
    {
        // TODO: Implement.

        /// <summary>
        /// Destroys the body associated with the interactive entity.
        /// </summary>
        /// <param name="physicsWorld">The physics world containing the body.</param>
        public virtual void Dispose(ref World physicsWorld)
        {
            // TODO: Implement
        }
    }
}

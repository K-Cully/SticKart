using System;

namespace SticKart
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SticKart game = new SticKart())
            {
                game.Run();
            }
        }
    }
#endif
}

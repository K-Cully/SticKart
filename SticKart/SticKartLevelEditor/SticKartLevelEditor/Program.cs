using System;

namespace SticKartLevelEditor
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LevelEditorGame game = new LevelEditorGame())
            {
                game.Run();
            }
        }
    }
#endif
}


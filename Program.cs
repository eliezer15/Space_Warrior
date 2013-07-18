using System;

namespace SpaceAdventure
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SpaceAdventure game = new SpaceAdventure())
            {
                game.Run();
            }
        }
    }
#endif
}

